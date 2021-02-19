using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries
{
    public class GetCategoriesList
    {
        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Query : IRequest<IEnumerable<CategoryDto>>
        { 
        }

        public class Handler : IRequestHandler<Query, IEnumerable<CategoryDto>>
        {
            private readonly IMapper mapper;
            private readonly ICategoryRepository categoryRepository;

            public Handler(IMapper mapper, ICategoryRepository categoryRepository)
            {
                this.mapper = mapper;
                this.categoryRepository = categoryRepository;
            }

            public async Task<IEnumerable<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = (await categoryRepository.GetAllAsync()).OrderBy(x => x.Name);
                return mapper.Map<IEnumerable<CategoryDto>>(result);
            }
        }

    }
}
