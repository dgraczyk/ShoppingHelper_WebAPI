using API.IntegrationTests.Helpers;
using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace API.IntegrationTests.Controllers
{
    public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public CategoryControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnsSuccessResult()
        {
            var client = factory.GetHttpClient();

            var response = await client.GetAsync("/api/category/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<GetCategoriesList.CategoryDto>>(responseString);

            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);            
        }

        [Fact]
        public async Task Create_ShouldReturnsSuccessResult()
        {
            var client = factory.GetHttpClient();

            var request = new CreateCategory.CreateCategoryCommand()
            {
                Name = "TestCategory"
            };
            
            var response = await client.PostAsJsonAsync("/api/category/", request);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<int>(responseString);

            result.Should().BeGreaterThan(0);            
        }
    }
}
