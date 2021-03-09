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
    }
}
