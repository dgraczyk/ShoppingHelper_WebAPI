using Application.Contracts.Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class ShopProductRepository : AsyncRepository<ShopProduct>, IShopProductRepository
    {
        public ShopProductRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }
    }
}
