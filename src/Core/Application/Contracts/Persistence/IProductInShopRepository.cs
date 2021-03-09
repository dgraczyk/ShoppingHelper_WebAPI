using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
    public interface IProductInShopRepository : IAsyncRepository<ProductInShop>
    {
        Task<bool> DoesProductExistInShop(Product product, int shopId);
        Task<ProductInShop> GetProductInShop(int productId, int shopId);
    }
}
