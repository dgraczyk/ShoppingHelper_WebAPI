using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }
                
        [HttpGet]
        public async Task<ActionResult<GetCategoriesList.CategoryDto>> GetAllCategories()
        {
            var dtos = await mediator.Send(new GetCategoriesList.Query());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody]Create.Command createCommand)
        {
            var response = await mediator.Send(createCommand);
            return Ok(response);
        }
    }
}
