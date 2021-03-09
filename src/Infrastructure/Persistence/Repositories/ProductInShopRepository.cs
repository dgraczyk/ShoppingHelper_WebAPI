using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ProductInShopRepository : AsyncRepository<ProductInShop>, IProductInShopRepository
    {
        public ProductInShopRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DoesProductExistInShop(Product product, int shopId)
        {
            return await this.dbContext.ProductsInShops.AnyAsync(x => x.Product.Hash == product.Hash && x.ShopId == shopId);
        }

        public async Task<ProductInShop> GetProductInShop(int productId, int shopId)
        {
            return await this.dbContext.ProductsInShops.FirstOrDefaultAsync(x => x.ProductId == productId && x.ShopId == shopId);
        }
    }
}
