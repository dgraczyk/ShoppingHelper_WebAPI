using Application.Contracts.Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class ProductRepository : AsyncRepository<Product>, IProductRepository
    {
        public ProductRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }
    }
}
