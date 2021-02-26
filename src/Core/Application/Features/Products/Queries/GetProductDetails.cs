using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.Products.Queries.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class GetProductDetails
    {
        public class Query : IRequest<ProductDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductDto>
        {
            private readonly IMapper mapper;
            private readonly IProductRepository productRepository;

            public Handler(IMapper mapper, IProductRepository productRepository)
            {
                this.mapper = mapper;
                this.productRepository = productRepository;
            }

            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await this.productRepository.GetProductWithPrices(request.Id);

                if (product == null)
                    throw new NotFoundException(nameof(Product));

                return this.mapper.Map<ProductDto>(product);
            }
        }
    }
}
