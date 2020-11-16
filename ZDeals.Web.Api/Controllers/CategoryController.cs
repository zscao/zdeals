using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("list")]
        [ProducesDefaultResponseType(typeof(IEnumerable<CategoryListView>))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetCategoryList()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return result;
        }

        [HttpGet("tree")]
        [ProducesDefaultResponseType(typeof(CategoryTreeView))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetCategoryTree()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            return result;
        }
    }
}
