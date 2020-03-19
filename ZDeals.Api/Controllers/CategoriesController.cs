using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;

namespace ZDeals.Api.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet(ApiRoutes.Categories.SearchCategories)]
        public async Task<ActionResult<Result>> Search()
        {
            return await _categoryService.SearchCategories();
        }

        [HttpPost(ApiRoutes.Categories.CreateCategory)]
        public async Task<ActionResult<Category>> Create(CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategory(request);
            return Created($"api/categories/{result.Data.Id}", result);
        }

        [HttpGet(ApiRoutes.Categories.GetCategoryById)]
        public async Task<ActionResult<Result>> GetById(int categoryId)
        {
            return await _categoryService.GetCategoryById(categoryId);
        }
    }
}