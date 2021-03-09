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
    public class AddPriceToProductTests
    {
        private readonly Mock<IProductInShopRepository> productInShopRepository;
        private readonly Mock<IProductRepository> productRepository;
        private readonly Mock<IShopRepository> shopRepository;

        public AddPriceToProductTests()
        {
            this.productInShopRepository = new Mock<IProductInShopRepository>();
            this.productRepository = new Mock<IProductRepository>();
            this.shopRepository = new Mock<IShopRepository>();
        }

        [Fact]
        public void Handle_ShouldThrowNotFoundExceptionIfShopIdNotExist()
        {   
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Shop));

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidCommand(), CancellationToken.None);

            func.Should().Throw<NotFoundException>();

            shopRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);

            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
            productInShopRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        [Fact]
        public void Handle_ShouldThrowNotFoundExceptionIfProductIdNotExist()
        {
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Product));

            var handler = this.CreateHandler();

            Func<Task> func = async () => await handler.Handle(this.CreateValidCommand(), CancellationToken.None);

            func.Should().Throw<NotFoundException>();

            productRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);

            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
            productInShopRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProductInShop>()), Times.Never());
        }

        [Fact]
        public async void Handle_ShouldAddProductToShopIfNotExistAndNextAddPriceToProduct()
        {
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.CreateProduct());
            productInShopRepository.Setup(x => x.GetProductInShop(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(default(ProductInShop));

            var handler = this.CreateHandler();

            await handler.Handle(this.CreateValidCommand(), CancellationToken.None);
                        
            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Once());
            productInShopRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProductInShop>()), Times.Once());
        }

        [Fact]
        public async void Handle_ShouldNotAddProductToShopIfExistAndNextAddPriceToProduct()
        {
            shopRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Shop());
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(this.CreateProduct());
            productInShopRepository.Setup(x => x.GetProductInShop(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new ProductInShop());

            var handler = this.CreateHandler();

            await handler.Handle(this.CreateValidCommand(), CancellationToken.None);

            productInShopRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductInShop>()), Times.Never());
            productInShopRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProductInShop>()), Times.Once());
        }

        private AddPriceToProduct.Handler CreateHandler()
        {
            return new AddPriceToProduct.Handler(                
                this.productRepository.Object,
                this.shopRepository.Object,
                this.productInShopRepository.Object,
                new Mock<IMapper>().Object);
        }

        private AddPriceToProduct.AddPriceToProductCommand CreateValidCommand()
        {
            return new AddPriceToProduct.AddPriceToProductCommand
            {
                ProductId = 1,
                ShopId = 1,
                BasePrice = new BasePriceDto
                {
                    Price = 2
                }
            };
        }

        private Product CreateProduct()
        {
            return new Product("TestName", "TestVendor", null, null, 1);           
        }
    }
}
