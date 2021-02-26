using Application.Contracts.Persistence;
using Domain.Entities;
using Moq;
using System.Collections.Generic;

namespace Application.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<ICategoryRepository> GetCategoryRepository()
        {
            var categories = new List<Category>
            {
                new Category
                {   
                    Id = 1,
                    Name = "Hardware"
                },
                new Category
                {   
                    Name = "Shoes"
                }                
            };

            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(
                (Category category) =>
                {
                    categories.Add(category);
                    return category;
                });

            return mockRepository;
        }
       
        public static Mock<IShopRepository> GetShopRepository()
        {
            var shops = new List<Shop>
            {
                new Shop
                {
                    Name = "Amazon"
                },
                new Shop
                {
                    Name = "SecondShop"
                }
            };

            var mockRepository = new Mock<IShopRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(shops);

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Shop>())).ReturnsAsync(
                (Shop shop) =>
                {
                    shops.Add(shop);
                    return shop;
                });

            return mockRepository;
        }

        public static Mock<IProductRepository> GetProductRepository()
        {
            var shops = new List<Product>
            {
                new Product
                {
                    Name = "TestProduct",
                    Category = new Category { Name = "Test" },
                    CategoryId = 1,
                    Size = 2,
                    SizeUnit = Domain.Enums.SizeUnits.kg,
                    Vendor = "TestCompany",
                    ProductInShops = new List<ProductInShop>
                    {
                        new ProductInShop
                        {
                            Shop = new Shop { Name = "TestShop"},
                            ShopId = 1,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceValue = 1.5m,
                                    PricePerSizeUnit = 1,
                                    SizeUnit = Domain.Enums.SizeUnits.g                                    
                                },
                                new Price
                                {
                                    PriceValue = 0.5m,
                                    IsPromotionPrice = true,
                                    PromotionConstraints = "with coupon",
                                    PricePerSizeUnit = 0.1m,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                }
                            }                            
                        },
                        new ProductInShop
                        {
                            Shop = new Shop { Name = "SecondTestShop"},
                            ShopId = 1,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceValue = 1.5m,
                                    PricePerSizeUnit = 1,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                },
                                new Price
                                {
                                    PriceValue = 0.5m,
                                    IsPromotionPrice = true,
                                    PromotionConstraints = "with coupon",
                                    PricePerSizeUnit = 0.1m,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                }
                            }
                        }
                    }
                },
                new Product
                {
                    Name = "SecondProduct",
                    Category = new Category { Name = "Second" },
                    CategoryId = 1,
                    Size = 2,
                    SizeUnit = Domain.Enums.SizeUnits.kg,
                    Vendor = "TestCompany",
                    ProductInShops = new List<ProductInShop>
                    {
                        new ProductInShop
                        {
                            Shop = new Shop { Name = "TestShop"},
                            ShopId = 1,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceValue = 1.5m,
                                    PricePerSizeUnit = 1,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                },
                                new Price
                                {
                                    PriceValue = 0.5m,
                                    IsPromotionPrice = true,
                                    PromotionConstraints = "with coupon",
                                    PricePerSizeUnit = 0.1m,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                }
                            }
                        },
                        new ProductInShop
                        {
                            Shop = new Shop { Name = "SecondTestShop"},
                            ShopId = 1,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceValue = 1.5m,
                                    PricePerSizeUnit = 1,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                },
                                new Price
                                {
                                    PriceValue = 0.5m,
                                    IsPromotionPrice = true,
                                    PromotionConstraints = "with coupon",
                                    PricePerSizeUnit = 0.1m,
                                    SizeUnit = Domain.Enums.SizeUnits.g
                                }
                            }
                        }
                    }
                },
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(shops);
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(shops[0]);
            mockRepository.Setup(repo => repo.GetProductWithPrices(It.IsAny<int>())).ReturnsAsync(shops[0]);
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).ReturnsAsync(
                (Product product) =>
                {
                    shops.Add(product);
                    return product;
                });

            return mockRepository;
        }
    }
}
