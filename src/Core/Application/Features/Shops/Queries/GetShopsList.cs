using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Queries
{
    public class GetShopsList
    {
        public class ShopDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Query : IRequest<IEnumerable<ShopDto>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<ShopDto>>
        {
            private readonly IMapper mapper;
            private readonly IShopRepository shopRepository;

            public Handler(IMapper mapper, IShopRepository shopRepository)
            {
                this.mapper = mapper;
                this.shopRepository = shopRepository;
            }

            public async Task<IEnumerable<ShopDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = (await shopRepository.GetAllAsync()).OrderBy(x => x.Name);
                return mapper.Map<IEnumerable<ShopDto>>(result);
            }
        }
    }
}
