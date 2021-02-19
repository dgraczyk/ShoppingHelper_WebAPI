using Application.Features.Shops.Commands;
using Application.Features.Shops.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ShopsController : BaseController
    {        
        [HttpGet]
        public async Task<ActionResult<GetShopsList.ShopDto>> GetAllShops()
        {
            var dtos = await Mediator.Send(new GetShopsList.Query());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateShop.CreateShopCommand createCommand)
        {
            var response = await Mediator.Send(createCommand);
            return Ok(response);
        }
    }
}
