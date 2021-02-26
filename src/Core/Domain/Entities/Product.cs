using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Vendor { get; set; }
        public decimal? Size { get; set; }
        public SizeUnits? SizeUnit { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductInShop> ProductInShops { get; set; }

        public Product()
        {
            this.ProductInShops = new List<ProductInShop>();
        }      
    }    
}
