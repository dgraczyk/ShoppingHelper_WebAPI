using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.Products.Commands.DTO;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class AddPriceToProduct 
    {
        public class AddPriceToProductCommand : IRequest
        {
            public int ProductId { get; set; }
            public int ShopId { get; set; }

            public BasePriceDto BasePrice { get; set; }
            public PromotionPriceDto PromotionPrice { get; set; }
        }

        public class Validator : AbstractValidator<AddPriceToProductCommand>
        {
            public Validator()
            {
                RuleFor(x => x.BasePrice)
                    .NotNull()
                    .SetValidator(new BasePriceDto.Validator());

                When(x => x.PromotionPrice != null, () =>
                {
                    RuleFor(x => x.PromotionPrice)
                        .SetValidator(new PromotionPriceDto.Validator());
                });

                RuleFor(x => x.ProductId)
                    .GreaterThan(0);

                RuleFor(x => x.ShopId)
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<AddPriceToProductCommand>
        {
            private readonly Validator validator;
            private readonly IProductRepository productRepository;
            private readonly IShopRepository shopRepository;
            private readonly IProductInShopRepository productInShopRepository;
            private readonly IMapper mapper;

            public Handler(
                IProductRepository productRepository, 
                IShopRepository shopRepository, 
                IProductInShopRepository productInShopRepository,
                IMapper mapper)
            {
                this.validator = new Validator();

                this.productRepository = productRepository;
                this.shopRepository = shopRepository;
                this.productInShopRepository = productInShopRepository;
                this.mapper = mapper;
            }

            public async Task<Unit> Handle(AddPriceToProductCommand request, CancellationToken cancellationToken)
            {
                await this.validator.ValidateAndThrowAsync(request, cancellationToken);

                if (await this.shopRepository.GetByIdAsync(request.ShopId) == null)
                {
                    throw new NotFoundException(nameof(Shop));
                }

                if (await this.productRepository.GetByIdAsync(request.ProductId) == null)
                {
                    throw new NotFoundException(nameof(Product));
                }

                var productInShop = await this.productInShopRepository.GetProductInShop(request.ProductId, request.ShopId);

                if(productInShop == null)
                {
                    productInShop = new ProductInShop()
                    {
                        ShopId = request.ShopId,
                        ProductId = request.ProductId
                    };

                    await this.productInShopRepository.AddAsync(productInShop);
                }

                productInShop.AddBasePrice(mapper.Map<Price>(request.BasePrice));

                if (request.PromotionPrice != null)
                {
                    productInShop.AddPromotionPrice(mapper.Map<Price>(request.PromotionPrice));
                }

                await this.productInShopRepository.UpdateAsync(productInShop);

                return Unit.Value;
            }
        }
    }
}
