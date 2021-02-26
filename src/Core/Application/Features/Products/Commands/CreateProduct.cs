using Application.Contracts.Persistence;
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
            private readonly IShopRepository shopRepository;
            private readonly ICategoryRepository categoryRepository;

            public Validator(IShopRepository shopRepository, ICategoryRepository categoryRepository)
            {
                this.shopRepository = shopRepository;
                this.categoryRepository = categoryRepository;

                RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(100);

                RuleFor(x => x.Vendor)
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
                    .MustAsync(this.CategoryExist)
                    .WithMessage("Category doesn't exist");

                RuleFor(x => x.ShopId)
                    .MustAsync(this.ShopExist)
                    .WithMessage("Shop doesn't exist");
            }

            private async Task<bool> CategoryExist(int categoryId, CancellationToken token)
            {
                var category = await this.categoryRepository.GetByIdAsync(categoryId);
                return category != null;
            }

            private async Task<bool> ShopExist(int shopId, CancellationToken token)
            {
                var shop = await this.shopRepository.GetByIdAsync(shopId);
                return shop != null;
            }
        }

        public class Handler : IRequestHandler<CreateProductCommand, int>
        {
            private readonly IShopProductRepository shopProductRepository;
            private readonly IMapper mapper;
            private readonly Validator validator;

            public Handler(
                IShopProductRepository shopProductRepository, 
                IShopRepository shopRepository, 
                ICategoryRepository categoryRepository,
                IMapper mapper)
            {   
                this.shopProductRepository = shopProductRepository;
                this.mapper = mapper;
                this.validator = new Validator(shopRepository, categoryRepository);
            }

            public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                await this.validator.ValidateAndThrowAsync(request, cancellationToken);

                var product = new Product
                {
                    Name = request.Name,
                    Vendor = request.Vendor,
                    Size = request.Size.Value,
                    SizeUnit = request.Size.Unit.ToEnum<SizeUnits>(),
                    CategoryId = request.CategoryId                    
                };

                var productInShop = ShopProduct.CreateProductInShop(product, request.ShopId);
                
                productInShop.AddBasePrice(mapper.Map<Price>(request.BasePrice));

                if(request.PromotionPrice != null)
                {
                    productInShop.AddPromotionPrice(mapper.Map<Price>(request.PromotionPrice));
                }

                await this.shopProductRepository.AddAsync(productInShop);

                return product.Id;
            }
        }
    }
}
