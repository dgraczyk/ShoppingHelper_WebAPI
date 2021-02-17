using Application.Contracts.Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class ShopRepository : AsyncRepository<Shop>, IShopRepository
    {
        public ShopRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }
    }
}
