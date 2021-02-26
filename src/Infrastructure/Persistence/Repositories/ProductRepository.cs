using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ProductRepository : AsyncRepository<Product>, IProductRepository
    {
        public ProductRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product> GetProductWithPrices(int id)
        {
            var product = await this.dbContext.Products
                .Include(x => x.Category)              
                .Include(x => x.ProductInShops)
                    .ThenInclude(x => x.Prices)                    
                .Include(x => x.ProductInShops)
                    .ThenInclude(x => x.Shop)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }
    }
}
