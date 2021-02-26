using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Price : BaseEntity
    {
        public decimal PriceValue { get; set; }
        public decimal? PricePerSizeUnit { get; set; }
        public SizeUnits? SizeUnit { get; set; }
        public DateTime Created { get; set; }
        public bool IsPromotionPrice { get; set; }
        public string PromotionConstraints { get; set; }

        public int ProductInShopId { get; set; }
        public ProductInShop ProductInShop { get; set; }
    }
}
