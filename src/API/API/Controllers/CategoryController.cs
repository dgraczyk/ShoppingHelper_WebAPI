using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CategoryController : BaseController
    {                   
        [HttpGet]
        public async Task<ActionResult<GetCategoriesList.CategoryDto>> GetAllCategories()
        {
            var dtos = await Mediator.Send(new GetCategoriesList.Query());
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody]CreateCategory.CreateCategoryCommand createCommand)
        {
            var response = await Mediator.Send(createCommand);
            return Ok(response);
        }
    }
}
