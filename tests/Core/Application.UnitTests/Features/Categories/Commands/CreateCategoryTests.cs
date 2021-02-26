using Application.Contracts.Persistence;
using Application.Features.Categories.Commands;
using Application.UnitTests.Mocks;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Features.Categories.Commands
{
    public class CreateCategoryTests
    {   
        private readonly Mock<ICategoryRepository> mockRepository;

        public CreateCategoryTests()
        {
            mockRepository = RepositoryMocks.GetCategoryRepository();
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldAddToRepository()
        {
            var initialListCount = (await mockRepository.Object.GetAllAsync()).Count;

            var handler = new CreateCategory.Handler(mockRepository.Object);

            await handler.Handle(new CreateCategory.CreateCategoryCommand { Name = "TestCategory" }, CancellationToken.None);

            var categories = await mockRepository.Object.GetAllAsync();

            categories.Count.Should().Be(initialListCount + 1);

            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public void Handle_InvalidCommand_EmptyName_ShouldThrowValidationException()
        {
            var handler = new CreateCategory.Handler(mockRepository.Object);

            Func<Task> func = async () => await handler.Handle(new CreateCategory.CreateCategoryCommand { Name = "" }, CancellationToken.None);

            func.Should().Throw<ValidationException>();

            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never());
        }
    }
}
