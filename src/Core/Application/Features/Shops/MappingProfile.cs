using Application.Features.Shops.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Shops
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Shop, GetShopsList.ShopDto>();
        }
    }
}
