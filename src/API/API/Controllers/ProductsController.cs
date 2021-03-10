using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Products.Queries.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductDetails(int id)
        {
            var dto = await Mediator.Send(new GetProductDetails.Query { Id = id });
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetProductsByCategoryName([FromQuery] GetProductsByCategoryName.Query query)
        {
            var items = await Mediator.Send(query);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateProduct.CreateProductCommand createCommand)
        {
            var response = await Mediator.Send(createCommand);
            return Ok(response);
        }

        [HttpPost("prices")]
        public async Task<ActionResult> CreatePrice([FromBody] AddPriceToProduct.AddPriceToProductCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }       
    }
}
