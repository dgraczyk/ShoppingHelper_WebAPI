using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Vendor { get; set; }
        public decimal? Size { get; set; }
        public SizeUnits? SizeUnit { get; set; }
        public int Hash { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductInShop> ProductInShops { get; set; }

        private Product()
        {
            this.ProductInShops = new List<ProductInShop>();
        }

        public Product(string name, string vendor, decimal? size, SizeUnits? sizeUnit, int categoryId)
        {
            Name = name;
            Vendor = vendor;
            Size = size;
            SizeUnit = sizeUnit;
            CategoryId = categoryId;
            ProductInShops = new List<ProductInShop>();
            Hash = HashCode.Combine(Name.ToLower(), Vendor.ToLower(), Size, SizeUnit);
        }
    }    
}
