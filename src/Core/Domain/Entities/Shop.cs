using System.Collections.Generic;

namespace Domain.Entities
{
    public class Shop : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; }

        public Shop()
        {
            this.ShopProducts = new List<ShopProduct>();
        }
    }
}
