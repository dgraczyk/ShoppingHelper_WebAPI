using Application.Contracts.Persistence;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class CategoryRepository : AsyncRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShoppingHelperDbContext dbContext) : base(dbContext)
        {
        }
    }
}
