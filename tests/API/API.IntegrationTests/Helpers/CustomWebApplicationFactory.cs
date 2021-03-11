using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System.Linq;
using System.Net.Http;

namespace API.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // remove the existing DbContextOptions configuration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ShoppingHelperDbContext>));
                
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ShoppingHelperDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ShoppingHelperDbContextInMemoryTest");                   
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ShoppingHelperDbContext>();                    

                    if(context.Database.EnsureCreated())
                    {
                        DatabaseHelper.InitializeDatabaseForTests(context);
                    }
                };
            });
        }

        public HttpClient GetHttpClient()
        {
            return CreateClient();
        }
    }
}
