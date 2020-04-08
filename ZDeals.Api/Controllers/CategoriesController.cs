using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(ApiRoutes.Categories.Base)]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<Result>> Search()
        {
            return await _categoryService.SearchCategoriesAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryTreeView>> Create(CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategoryAsync(request);
            return Created($"api/categories/{result.Data.Id}", result);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Result>> GetById(int categoryId)
        {
            return await _categoryService.GetCategoryByIdAsync(categoryId);
        }

        [HttpGet("list")]
        public async Task<ActionResult<Result>> GetCategoryList()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            if (result.HasError()) return result;

            var list = new CategoryList { Data = result.Data.ToCategoryListView() };

            return new Result<CategoryList>(list);
        }

        [HttpGet("tree")]
        public async Task<ActionResult<Result>> GetCategoryTree()
        {
            return await _categoryService.GetCategoryTreeAsync();
        }
    }
}