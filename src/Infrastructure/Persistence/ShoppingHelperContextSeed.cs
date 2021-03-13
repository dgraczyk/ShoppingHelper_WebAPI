using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Persistence
{
    public class ShoppingHelperContextSeed
    {
        public static async Task SeedAsync(ShoppingHelperDbContext dbContext, ILogger<ShoppingHelperContextSeed> logger)
        {
            logger.LogInformation("Inserting sample data into the database.");

            await CreateCategories(dbContext, logger);
            await CreateShops(dbContext, logger);
            await CreateProducts(dbContext, logger);

            logger.LogInformation("Database filled.");
        }

        private static async Task CreateCategories(ShoppingHelperDbContext dbContext, ILogger<ShoppingHelperContextSeed> logger)
        {
            if(await dbContext.Categories.AnyAsync())
            {
                logger.LogInformation("Categories are already inserted.");
                return;
            }

            dbContext.Categories.Add(new Category { Name = "Hardware" });
            dbContext.Categories.Add(new Category { Name = "Juices " });

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Categories created.");
        }

        private static async Task CreateShops(ShoppingHelperDbContext dbContext, ILogger<ShoppingHelperContextSeed> logger)
        {
            if (await dbContext.Shops.AnyAsync())
            {
                logger.LogInformation("Shops are already inserted.");
                return;
            }

            dbContext.Shops.Add(new Shop { Name = "Amazon" });
            dbContext.Shops.Add(new Shop { Name = "SuperShop" });

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Shops created.");
        }

        private static async Task CreateProducts(ShoppingHelperDbContext dbContext, ILogger<ShoppingHelperContextSeed> logger)
        {
            if (await dbContext.Products.AnyAsync())
            {
                logger.LogInformation("Products are already inserted.");
                return;
            }

            var laptop = new Product("Laptop", "Asus", null, null, 1);
            var appleJuice = new Product("Apple juice", "AppleVendor", 2, Domain.Enums.SizeUnits.l, 2);

            dbContext.Products.Add(laptop);
            dbContext.Products.Add(appleJuice);

            await dbContext.SaveChangesAsync();

            var laptopInAmazon = ProductInShop.CreateProductInShop(laptop, 1);
            laptopInAmazon.AddBasePrice(new Price { PriceValue = 3333 });

            var laptopInSuperShop = ProductInShop.CreateProductInShop(laptop, 2);
            laptopInSuperShop.AddBasePrice(new Price { PriceValue = 3000 });

            var appleJuiceInSuperShop = ProductInShop.CreateProductInShop(appleJuice, 2);
            appleJuiceInSuperShop.AddBasePrice(new Price { PriceValue = 4, PricePerSizeUnit = 2, SizeUnit = Domain.Enums.SizeUnits.l});
            appleJuiceInSuperShop.AddPromotionPrice(new Price { PriceValue = 2.50m, PricePerSizeUnit = 1.25m, SizeUnit = Domain.Enums.SizeUnits.l, PromotionConstraints = "If you use special coupon." });

            dbContext.ProductsInShops.Add(laptopInAmazon);
            dbContext.ProductsInShops.Add(laptopInSuperShop);
            dbContext.ProductsInShops.Add(appleJuiceInSuperShop);

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Products created.");
        }
    }
}
