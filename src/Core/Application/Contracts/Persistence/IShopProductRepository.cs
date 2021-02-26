using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IShopProductRepository : IAsyncRepository<ShopProduct>
    {
    }
}
