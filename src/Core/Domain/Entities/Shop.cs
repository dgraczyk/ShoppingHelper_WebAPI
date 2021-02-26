using System.Collections.Generic;

namespace Domain.Entities
{
    public class Shop : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductInShop> ProductsInShop { get; set; }

        public Shop()
        {
            this.ProductsInShop = new List<ProductInShop>();
        }
    }
}
