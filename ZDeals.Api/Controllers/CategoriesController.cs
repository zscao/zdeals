using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;
using ZDeals.Api.Service;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Category>>> Search()
        {
            var result = await _categoryService.SearchCategories();
            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }

            return result.Data.ToArray();
        }

        [HttpPost(ApiRoutes.Categories.CreateCategory)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> Create(CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategory(request);

            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }

            return Created($"api/categories/{result.Data.Id}", result.Data);
        }

        [HttpGet(ApiRoutes.Categories.GetCategoryById)]
        public async Task<ActionResult<Category>> GetById(int categoryId)
        {
            var result = await _categoryService.GetCategoryById(categoryId);
            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }
            return result.Data;
        }
    }
}