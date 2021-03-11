using API.IntegrationTests.Helpers;
using Application.Features.Products.Commands;
using Application.Features.Products.Commands.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace API.IntegrationTests.Controllers
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ProductsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Create_ShouldReturnsSuccessResult()
        {
            var client = factory.GetHttpClient();

            var request = new CreateProduct.CreateProductCommand()
            {
                CategoryId = 1,
                ShopId = 1,
                Name = "TestProduct",
                Vendor = "Factory",
                BasePrice = new BasePriceDto { Price  = 100}
            };

            var response = await client.PostAsJsonAsync("/api/products/", request);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<int>(responseString);

            result.Should().BeGreaterThan(0);
            
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.LocalPath.Should().Be($"/api/Products/{result}");
        }

        [Fact]
        public async Task Create_ShouldFailIfRequestIsNotValid()
        {
            var client = factory.GetHttpClient();

            var request = new CreateProduct.CreateProductCommand()
            {   
                Name = "TestProduct"             
            };

            var response = await client.PostAsJsonAsync("/api/products/", request);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<string>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNullOrEmpty();
        }
    }
}
