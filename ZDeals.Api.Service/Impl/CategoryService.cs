using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Result<IEnumerable<CategoryTreeView>>> SearchCategoriesAsync()
        {
            var categories = await _dbContext.Categories.Select(x => x.ToCategoryTreeView()).ToListAsync();

            return new Result<IEnumerable<CategoryTreeView>> { Data = categories };
        }
        public async Task<Result<CategoryTreeView>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var parent = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.ParentId);
            if(parent == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Parent category does not exist." };
                return new Result<CategoryTreeView>(error);
            }

            var catetory = new CategoryEntity
            {
                Code = request.Code,
                Title = request.Title,
                ParentId = parent.Id,
                DisplayOrder = request.DisplayOrder
            };

            var entry = _dbContext.Categories.Add(catetory);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<CategoryTreeView>(entry.Entity.ToCategoryTreeView());
        }

        public async Task<Result<CategoryTreeView>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            if(category == null)
            {
                return new Result<CategoryTreeView>(new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Category does not exist" });
            }

            return new Result<CategoryTreeView>(category.ToCategoryTreeView());
        }

        public async Task<Result<CategoryTreeView>> GetCategoryTreeAsync(int? rootId = null)
        {
            var categories = await _dbContext.Categories.ToListAsync();

            CategoryEntity root = null;
            if (rootId.HasValue)
            {
                root = categories.FirstOrDefault(x => x.Id == rootId.Value);
            }
            else
            {
                // find the minimal id as the default root id
                root = categories.OrderBy(x => x.Id).FirstOrDefault(); 
            }

            if (root == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Category does not exist" };
                return new Result<CategoryTreeView>(error);
            }

            var result = root.ToCategoryTreeView();
            result.Children = BuildCategoryTree(categories, result);

            return new Result<CategoryTreeView>(result);

        }

        private IEnumerable<CategoryTreeView> BuildCategoryTree(IEnumerable<CategoryEntity> categories, CategoryTreeView parent = null)
        {
            if (categories == null) return null;

            var result = new List<CategoryTreeView>();
            var children = categories.Where(x => x.ParentId == parent?.Id).OrderBy(x => x.DisplayOrder);

            foreach (var child in children)
            {
                var cate = child.ToCategoryTreeView();
                result.Add(cate);

                cate.Children = BuildCategoryTree(categories, cate);
            }

            return result;
        }
    }
}
