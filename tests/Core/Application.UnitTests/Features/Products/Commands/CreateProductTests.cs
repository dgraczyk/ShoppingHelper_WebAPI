using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.Products.Commands;
using Application.Features.Products.Commands.DTO;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Features.Products.Commands
{
    public class CreateProductTests
    {
        private readonly Mock<IProductInShopRepository> productInShopRepository;
        private readonly Mock<IProductRepository> productRepository;
        private readonly Mock<ICategoryRepository> categoryRepository;
        private readonly Mock<IShopRepository> shopRepository;

        public CreateProductTests()
        {
            this.productInShopRepository = new Mock<IProductInShopRepository>();
            this.productRepository = new Mock<IProductRepository>();
            this.categoryRepository = new Mock<ICategoryRepository>();
            this.shopRepository = new Mock<IShopRepository>();
        }


        [Fact]
        public async void Handle_ShouldCreateProductInShop()
        {
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category());
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.DoesProductExist(It.IsAny<Product>())).ReturnsAsync(false);
            productInShopRepository.Setup(x => x.DoesProductExistInShop(It.IsAny<Product>(), It.IsAny<int>())).ReturnsAsync(false);
            productInShopRepository.Setup(x => x.AddAsync(It.IsAny<ProductInShop>())).ReturnsAsync(new ProductInShop());

            var handler = this.CreateHandler();

            await handler.Handle(this.CreateValidProductCommand(), CancellationToken.None);

            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Once());
        }

        [Fact]
        public void Handle_ShouldThrowNotFoundExceptionIfCategoryIdNotExist()
        {
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Category));

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidProductCommand(), CancellationToken.None);

            func.Should().Throw<NotFoundException>();

            categoryRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);
            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        [Fact]
        public void Handle_ShouldThrowNotFoundExceptionIfShopIdNotExist()
        {
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category());
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Shop));

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidProductCommand(), CancellationToken.None);

            func.Should().Throw<NotFoundException>();

            shopRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);
            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        [Fact]
        public void Handle_ShouldThrowBadRequestExceptionIfProductAlreadyExist()
        {
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category());
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.DoesProductExist(It.IsAny<Product>())).ReturnsAsync(true);

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidProductCommand(), CancellationToken.None);

            func.Should().Throw<BadRequestException>();

            productRepository.Verify(repo => repo.DoesProductExist(It.IsAny<Product>()), Times.Once);
            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        [Fact]
        public void Handle_ShouldThrowBadRequestExceptionIfProductAlreadyExistInShop()
        {
            categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category());
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.DoesProductExist(It.IsAny<Product>())).ReturnsAsync(false);
            productInShopRepository.Setup(x => x.DoesProductExistInShop(It.IsAny<Product>(), It.IsAny<int>())).ReturnsAsync(true);

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidProductCommand(), CancellationToken.None);

            func.Should().Throw<BadRequestException>();

            productInShopRepository.Verify(repo => repo.DoesProductExistInShop(It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        private CreateProduct.Handler CreateHandler()
        {
            return new CreateProduct.Handler(
                this.productInShopRepository.Object,
                this.productRepository.Object,
                this.shopRepository.Object,
                this.categoryRepository.Object,
                new Mock<IMapper>().Object);
        }

        private CreateProduct.CreateProductCommand CreateValidProductCommand()
        {
            return new CreateProduct.CreateProductCommand
            {
                Name = "TestName",
                Vendor = "TestVendor",
                CategoryId = 1,
                ShopId = 1,
                BasePrice = new BasePriceDto
                {
                    Price = 2
                }
            };
        }
    }
}
