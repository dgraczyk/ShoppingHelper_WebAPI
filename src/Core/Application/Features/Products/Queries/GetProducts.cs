using Application.Contracts.Persistence;
using Application.Features.Products.Queries.DTO;
using Application.Models.Requests;
using Application.Models.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class GetProducts
    {
        public class Query : PagingQuery, IRequest<PagedResponse<ProductDto>>
        {
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedResponse<ProductDto>>
        {
            private readonly IProductRepository productRepository;
            private readonly IMapper mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                this.productRepository = productRepository;
                this.mapper = mapper;
            }

            public async Task<PagedResponse<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Product> products;

                if(string.IsNullOrEmpty(request.Name))
                {
                    products = await this.productRepository.GetAllAsync();
                }
                else
                {
                    products = await this.productRepository.GetProductsByName(request.Name);
                }

                return new PagedResponse<ProductDto>(
                    this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products),
                    request.PageNumber, request.PageSize);
            }
        }
    }
}
