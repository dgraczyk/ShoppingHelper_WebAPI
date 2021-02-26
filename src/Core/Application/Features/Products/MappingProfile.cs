using Application.Extensions;
using Application.Features.Products.Commands.DTO;
using Application.Features.Products.Queries.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Products
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasePriceDto, Price>()
                .ForMember(x => x.PriceValue, y => y.MapFrom(z => z.Price))
                .ForMember(x => x.SizeUnit, y => y.MapFrom(z => z.PriceSizeUnitType.ToEnum<SizeUnits>()));
            CreateMap<PromotionPriceDto, Price>()
                .ForMember(x => x.PriceValue, y => y.MapFrom(z => z.Price))
                .ForMember(x => x.SizeUnit, y => y.MapFrom(z => z.PriceSizeUnitType.ToEnum<SizeUnits>()));

            CreateMap<Price, PriceDto>()
                .ForMember(x => x.Created, y => y.MapFrom(z => z.Created.ToShortDateString()))
                .ForMember(x => x.PricePerSizeUnit, y => y.MapFrom(z => string.Join(' ', z.PricePerSizeUnit, z.SizeUnit)));

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Category, y => y.MapFrom(z => z.Category.Name))
                .ForMember(x => x.Size, y => y.MapFrom(z => string.Join(' ', z.Size, z.SizeUnit)))
                .ForMember(x => x.Offerts, y => y.MapFrom(z => z.ProductInShops));

            CreateMap<ProductInShop, ProductOffertDto>()
                .ForMember(x => x.Shop, y => y.MapFrom(z => z.Shop.Name));
        }
    }
}
