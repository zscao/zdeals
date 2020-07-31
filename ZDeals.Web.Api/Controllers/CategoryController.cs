using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<Result>> GetCategoryList()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return result;
        }

        [HttpGet("tree")]
        public async Task<ActionResult<Result>> GetCategoryTree()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            return result;
        }
    }
}
