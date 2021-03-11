using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class ProductInShopTests
    {
        [Fact]
        public void CreateProductInShop_ShouldCreateProductInShop()
        {
            var product = new Product("Test", "TestVendor", 1, Enums.SizeUnits.l, 1);
            var shopId = 1;

            var productInShop = ProductInShop.CreateProductInShop(product, shopId);

            productInShop.Should().NotBeNull();
            productInShop.Product.Should().NotBeNull();
            productInShop.Product.Should().BeSameAs(product);
            productInShop.ShopId.Should().Be(shopId);

            product.ProductInShops.Should().NotBeEmpty();
            product.ProductInShops.Should().Contain(productInShop);
        }

        [Fact]
        public void AddBasePrice_ShouldAddPriceToProductInShop()
        {
            var product = new Product("Test", "TestVendor", 1, Enums.SizeUnits.l, 1);
            var productInShop = ProductInShop.CreateProductInShop(product, 1);

            var price = new Price();

            productInShop.AddBasePrice(price);

            productInShop.Prices.Should().NotBeEmpty();
            productInShop.Prices.Should().Contain(price);
            price.IsPromotionPrice.Should().BeFalse();
        }

        [Fact]
        public void AddPromotionPrice_ShouldAddPriceToProductInShopAndSetPropertyIsPromotionOnTrue()
        {
            var product = new Product("Test", "TestVendor", 1, Enums.SizeUnits.l, 1);
            var productInShop = ProductInShop.CreateProductInShop(product, 1);

            var price = new Price();

            productInShop.AddPromotionPrice(price);

            productInShop.Prices.Should().NotBeEmpty();
            productInShop.Prices.Should().Contain(price);
            price.IsPromotionPrice.Should().BeTrue();
        }
    }
}
