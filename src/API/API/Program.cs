using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Serilog;
using System;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting application");
                
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var dbContext = services.GetRequiredService<ShoppingHelperDbContext>();
                    var logger = services.GetRequiredService<ILogger<ShoppingHelperContextSeed>>();

                    dbContext.Database.EnsureCreated();

                    await ShoppingHelperContextSeed.SeedAsync(dbContext, logger);
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.Information("Closing application.");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
