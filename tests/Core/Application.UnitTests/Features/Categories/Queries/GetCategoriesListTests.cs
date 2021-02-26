using Application.Contracts.Persistence;
using Application.Features.Categories;
using Application.Features.Categories.Queries;
using Application.UnitTests.Mocks;
using AutoMapper;
using AutoMapper.Internal;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Features.Categories.Queries
{
    public class GetCategoriesListTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICategoryRepository> mockRepository;

        public GetCategoriesListTests()
        {
            var configurationProvider = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingProfile>();
            });

            mapper = configurationProvider.CreateMapper();

            mockRepository = RepositoryMocks.GetCategoryRepository();
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnList()
        {
            var listCount = (await mockRepository.Object.GetAllAsync()).Count;

            var handler = new GetCategoriesList.Handler(mapper, mockRepository.Object);

            var result = await handler.Handle(new GetCategoriesList.Query(), CancellationToken.None);

            result.Count().Should().Be(listCount);
            result.ForAll(x => x.Name.Should().NotBeNullOrEmpty());

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Exactly(2)); // first in this test
        }
    }
}
