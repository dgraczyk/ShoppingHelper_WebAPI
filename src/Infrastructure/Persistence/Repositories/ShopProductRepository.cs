using Application.Contracts.Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class ShopProductRepository : AsyncRepository<ProductInShop>, IProductInShopRepository
    {
        public ShopProductRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }
    }
}
