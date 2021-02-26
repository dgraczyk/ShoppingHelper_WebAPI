using Application.Contracts.Persistence;
using Application.Features.Shops.Commands;
using Application.UnitTests.Mocks;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace Application.UnitTests.Features.Shops.Commands
{
    public class CreateShopTests
    {
        private readonly Mock<IShopRepository> mockRepository;

        public CreateShopTests()
        {
            mockRepository = RepositoryMocks.GetShopRepository  ();
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldAddToRepository()
        {
            var initialListCount = (await mockRepository.Object.GetAllAsync()).Count;

            var handler = new CreateShop.Handler(mockRepository.Object);

            await handler.Handle(new CreateShop.CreateShopCommand { Name = "TestShop" }, CancellationToken.None);

            var categories = await mockRepository.Object.GetAllAsync();

            categories.Count.Should().Be(initialListCount + 1);

            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Shop>()), Times.Once());
        }

        [Fact]
        public void Handle_InvalidCommand_EmptyName_ShouldThrowValidationException()
        {
            var handler = new CreateShop.Handler(mockRepository.Object);

            Func<Task> func = async () => await handler.Handle(new CreateShop.CreateShopCommand { Name = "" }, CancellationToken.None);

            func.Should().Throw<ValidationException>();

            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Shop>()), Times.Never());
        }
    }
}
