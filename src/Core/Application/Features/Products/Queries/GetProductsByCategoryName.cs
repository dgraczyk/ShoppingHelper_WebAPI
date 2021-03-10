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
    public class GetProductsByCategoryName
    {
        public class Query : PagingQuery, IRequest<PagedResponse<ProductDto>>
        {
            public string Category { get; set; }
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
                var items = await this.productRepository.GetProductsByCategoryName(request.Category);

                return new PagedResponse<ProductDto>(
                    this.mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(items),
                    request.PageNumber, request.PageSize);
            }
        }
    }
}
