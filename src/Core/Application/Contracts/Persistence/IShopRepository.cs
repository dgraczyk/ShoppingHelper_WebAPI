using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IShopRepository : IAsyncRepository<Shop>
    {
    }
}
