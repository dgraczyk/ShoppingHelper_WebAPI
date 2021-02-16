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
    }
}
