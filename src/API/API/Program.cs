using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<ShoppingHelperDbContext>();
                
                dbContext.Database.EnsureCreated();
                
                if(!dbContext.Categories.Any())
                {
                    dbContext.Categories.AddRange(new Category { Name = "Makarony" }, new Category { Name = "Sosy" });
                }

                if (!dbContext.Shops.Any())
                {
                    dbContext.Shops.AddRange(new Shop { Name = "Lidl" }, new Shop { Name = "Biedronka" });
                }

                await dbContext.SaveChangesAsync();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
