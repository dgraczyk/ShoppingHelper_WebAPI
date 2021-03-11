using Domain.Entities;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.IntegrationTests.Helpers
{
    public static class DatabaseHelper
    {
        public static void InitializeDatabaseForTests(ShoppingHelperDbContext dbContext)
        {
            CreateCategories(dbContext);
            CreateShops(dbContext);
            CreateProducts(dbContext);
        }

        private static void CreateCategories(ShoppingHelperDbContext dbContext)
        {
            dbContext.Categories.Add(new Category { Name = "Hardware" });
            dbContext.Categories.Add(new Category { Name = "Toys" });

            dbContext.SaveChanges();
        }

        private static void CreateShops(ShoppingHelperDbContext dbContext)
        {
            dbContext.Shops.Add(new Shop { Name = "Amazon" });
            dbContext.Shops.Add(new Shop { Name = "SuperShop" });

            dbContext.SaveChanges();
        }

        private static void CreateProducts(ShoppingHelperDbContext dbContext)
        {
            var laptop = new Product("Laptop", "Asus", null, null, 1);
            var car = new Product("Car", "Lego", null, null, 2);

            dbContext.Products.Add(laptop);
            dbContext.Products.Add(car);

            dbContext.SaveChanges();

            var laptopInAmazon = ProductInShop.CreateProductInShop(laptop, 1);            
            laptopInAmazon.AddBasePrice(new Price { PriceValue = 3333 });

            var laptopInSuperShop = ProductInShop.CreateProductInShop(laptop, 2);
            laptopInSuperShop.AddBasePrice(new Price { PriceValue = 3000 });

            var carInAmazon = ProductInShop.CreateProductInShop(car, 1);
            carInAmazon.AddBasePrice(new Price { PriceValue = 100 });
            carInAmazon.AddPromotionPrice(new Price { PriceValue = 50 });

            dbContext.ProductsInShops.Add(laptopInAmazon);
            dbContext.ProductsInShops.Add(laptopInSuperShop);
            dbContext.ProductsInShops.Add(carInAmazon);

            dbContext.SaveChanges();
        }
    }
}
