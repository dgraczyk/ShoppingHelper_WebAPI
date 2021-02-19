using Application.Features.Categories.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Categories
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, GetCategoriesList.CategoryDto>();
        }
    }
}
