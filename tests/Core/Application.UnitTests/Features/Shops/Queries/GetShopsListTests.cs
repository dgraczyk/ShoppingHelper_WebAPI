using Application.Contracts.Persistence;
using Application.Features.Shops;
using Application.Features.Shops.Queries;
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


namespace Application.UnitTests.Features.Shops.Queries
{
    public class GetShopsListTests
    {
        private readonly IMapper mapper;
        private readonly Mock<IShopRepository> mockRepository;

        public GetShopsListTests()
        {
            var configurationProvider = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingProfile>();
            });

            mapper = configurationProvider.CreateMapper();

            mockRepository = RepositoryMocks.GetShopRepository();
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnList()
        {
            var listCount = (await mockRepository.Object.GetAllAsync()).Count;

            var handler = new GetShopsList.Handler(mapper, mockRepository.Object);

            var result = await handler.Handle(new GetShopsList.Query(), CancellationToken.None);

            result.Count().Should().Be(listCount);
            result.ForAll(x => x.Name.Should().NotBeNullOrEmpty());

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Exactly(2)); // first in this test
        }
    }
}
