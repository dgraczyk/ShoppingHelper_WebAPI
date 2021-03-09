using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<Product> GetProductWithPrices(int id);
        Task<bool> DoesProductExist(Product product);
    }
}
