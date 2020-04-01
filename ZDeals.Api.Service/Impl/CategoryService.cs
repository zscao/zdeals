using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Api.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ZDealsDbContext _dbContext; 

        public CategoryService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<Category>>> SearchCategoriesAsync()
        {
            var categories = await _dbContext.Categories.Select(x => x.ToCategoryModel()).ToListAsync();

            return new Result<IEnumerable<Category>> { Data = categories };
        }
        public async Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var catetory = new CategoryEntity
            {
                Code = request.Code,
                Title = request.Title,
                DisplayOrder = request.DisplayOrder
            };

            var entry = _dbContext.Categories.Add(catetory);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Category>(entry.Entity.ToCategoryModel());
        }

        public async Task<Result<Category>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if(category == null)
            {
                return new Result<Category>(new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Category does not exist" });
            }

            return new Result<Category>(category.ToCategoryModel());
        }


    }
}
