using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles = ApiRoles.Admin)]
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
        [ProducesDefaultResponseType(typeof(IEnumerable<CategoryDetail>))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Search()
        {
            return await _categoryService.SearchCategoriesAsync();
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(CategoryDetail))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Create(CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategoryAsync(request);
            if (result.HasError()) return result;

            return Created($"api/categories/{result.Data.Id}", result);
        }

        [HttpPut("{categoryId}")]
        [ProducesDefaultResponseType(typeof(CategoryDetail))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Update(int categoryId, UpdateCategoryRequest request)
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryId, request);
            return result;
        }

        [HttpGet("{categoryId}")]
        [ProducesDefaultResponseType(typeof(CategoryDetail))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetById(int categoryId)
        {
            return await _categoryService.GetCategoryByIdAsync(categoryId);
        }

        [HttpGet("list")]
        [ProducesDefaultResponseType(typeof(CategoryList))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetCategoryList()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            if (result.HasError()) return result;

            var list = new CategoryList { Data = result.Data.ToCategoryList() };

            return new Result<CategoryList>(list);
        }

        [HttpGet("tree")]
        [ProducesDefaultResponseType(typeof(CategoryTreeView))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetCategoryTree()
        {
            return await _categoryService.GetCategoryTreeAsync();
        }
    }
}