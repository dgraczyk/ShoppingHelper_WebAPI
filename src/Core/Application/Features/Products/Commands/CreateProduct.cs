using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Extensions;
using Application.Features.Products.Commands.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class CreateProduct
    {
        public class CreateProductCommand : IRequest<int>
        {
            public int CategoryId { get; set; }
            public int ShopId { get; set; }

            public string Name { get; set; }            
            public string Vendor { get; set; }
            
            public SizeDto Size { get; set; }

            public BasePriceDto BasePrice { get; set; }
            public PromotionPriceDto PromotionPrice { get; set; }
        }

        public class Validator : AbstractValidator<CreateProductCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);

                RuleFor(x => x.Vendor)
                    .Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);

                When(x => x.Size != null, () =>
                {
                    RuleFor(x => x.Size)
                        .SetValidator(new SizeDto.Validator());
                });

                RuleFor(x => x.BasePrice)
                    .NotNull()
                    .SetValidator(new BasePriceDto.Validator());

                When(x => x.PromotionPrice != null, () =>
                {
                    RuleFor(x => x.PromotionPrice)
                        .SetValidator(new PromotionPriceDto.Validator());
                });

                RuleFor(x => x.CategoryId)
                    .GreaterThan(0);

                RuleFor(x => x.ShopId)
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<CreateProductCommand, int>
        {
            private readonly IProductInShopRepository productInShopRepository;
            private readonly IProductRepository productRepository;
            private readonly IShopRepository shopRepository;
            private readonly ICategoryRepository categoryRepository;
            private readonly IMapper mapper;
            private readonly Validator validator;

            public Handler(
                IProductInShopRepository shopProductRepository, 
                IProductRepository productRepository,
                IShopRepository shopRepository, 
                ICategoryRepository categoryRepository,
                IMapper mapper)
            {   
                this.productInShopRepository = shopProductRepository;
                this.productRepository = productRepository;
                this.shopRepository = shopRepository;
                this.categoryRepository = categoryRepository;
                this.mapper = mapper;
                this.validator = new Validator();
            }

            public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                await this.validator.ValidateAndThrowAsync(request, cancellationToken);

                if(await this.categoryRepository.GetByIdAsync(request.CategoryId) == null)
                {
                    throw new NotFoundException(nameof(Category));
                }

                if (await this.shopRepository.GetByIdAsync(request.ShopId) == null)
                {
                    throw new NotFoundException(nameof(Shop));
                }

                var product = new Product(request.Name, request.Vendor, request.Size?.Value, request.Size?.Unit.ToEnum<SizeUnits>(), request.CategoryId);

                if(await this.productRepository.DoesProductExist(product))
                {
                    throw new BadRequestException($"Product '{product.Name}({product.Vendor})' already exist.");
                }

                if (await this.productInShopRepository.DoesProductExistInShop(product, request.ShopId))
                {
                    throw new BadRequestException($"Product '{product.Name}({product.Vendor})' already exist in shop.");
                }

                var productInShop = ProductInShop.CreateProductInShop(product, request.ShopId);
                
                productInShop.AddBasePrice(mapper.Map<Price>(request.BasePrice));

                if(request.PromotionPrice != null)
                {
                    productInShop.AddPromotionPrice(mapper.Map<Price>(request.PromotionPrice));
                }

                await this.productInShopRepository.AddAsync(productInShop);

                return product.Id;
            }
        }
    }
}
