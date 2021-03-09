using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppingHelperDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ShoppingHelperConnectionString")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductInShopRepository, ProductInShopRepository>();

            return services;
        }
    }
}
