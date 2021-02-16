using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Price : BaseEntity
    {
        public decimal PriceValue { get; set; }
        public decimal PricePerSizeUnit { get; set; }
        public SizeUnits SizeUnit { get; set; }
        public DateTime Created { get; set; }
        public bool IsPromotionPrice { get; set; }
        public string PromotionConstraints { get; set; }

        public int ShopProductId { get; set; }
        public ShopProduct ShopProduct { get; set; }
    }
}
