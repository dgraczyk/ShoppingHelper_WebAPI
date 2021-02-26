using System.Collections.Generic;

namespace Domain.Entities
{
    public class ShopProduct : BaseEntity
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<Price> Prices { get; set; }

        public ShopProduct()
        {
            this.Prices = new List<Price>();
        }

        public static ShopProduct CreateProductInShop(Product product, int shopId)
        {
            var shopProduct = new ShopProduct
            {
                Product = product,
                ShopId = shopId
            };
            
            product.ShopProducts.Add(shopProduct);

            return shopProduct;
        }

        public void AddBasePrice(Price price)
        {
            this.Prices.Add(price);
        }

        public void AddPromotionPrice(Price price)
        {
            price.IsPromotionPrice = true;
            this.Prices.Add(price);
        }
    }
}
