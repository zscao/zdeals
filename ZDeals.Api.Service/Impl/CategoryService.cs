using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ZDealsDbContext _dbContext; 

        public CategoryService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<CategoryDetail>>> SearchCategoriesAsync()
        {
            var categories = await _dbContext.Categories.Select(x => x.ToCategoryDetail()).ToListAsync();

            return new Result<IEnumerable<CategoryDetail>> { Data = categories };
        }
        public async Task<Result<CategoryDetail>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            request.Code = request.Code?.ToLower().Trim();
            if(string.IsNullOrEmpty(request.Code))
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.CategoryNotFound, Message = "Code can't be empty." };
                return new Result<CategoryDetail>(error);
            }

            var existing = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Code == request.Code);
            if(existing != null)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.CategoryCodeDuplicate, Message = $"Code {request.Code} already exists" };
                return new Result<CategoryDetail>(error);
            }

            var parent = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Id == (request.ParentId ?? 0));
            if(parent == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Parent category does not exist." };
                return new Result<CategoryDetail>(error);
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

            return new Result<CategoryDetail>(entry.Entity.ToCategoryDetail());
        }

        public async Task<Result<CategoryDetail>> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Id == categoryId);
            if (category == null)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.CategoryNotFound, Message = $"Category does not exist." };
                return new Result<CategoryDetail>(error);
            }

            if(category.ParentId == null)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.CategoryChangeNotAllowed, Message = "Cannot change root categories." };
                return new Result<CategoryDetail>(error);
            }

            var parent = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Id == request.ParentId);
            if (parent == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Parent category does not exist." };
                return new Result<CategoryDetail>(error);
            }

            category.Title = request.Title;
            category.ParentId = parent.Id;
            category.DisplayOrder = request.DisplayOrder;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<CategoryDetail>(category.ToCategoryDetail());
        }

        public async Task<Result<CategoryDetail>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Id == categoryId);
            if(category == null)
            {
                return new Result<CategoryDetail>(new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Category does not exist" });
            }

            return new Result<CategoryDetail>(category.ToCategoryDetail());
        }

        public async Task<Result<CategoryDetail>> GetCategoryByCodeAsync(string categoryCode)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Code == categoryCode);
            if (category == null)
            {
                return new Result<CategoryDetail>(new Error(ErrorType.NotFound) { Code = Sales.CategoryNotFound, Message = "Category does not exist" });
            }

            return new Result<CategoryDetail>(category.ToCategoryDetail());
        }

        public async Task<Result<CategoryTreeView>> GetCategoryTreeAsync(int? rootId = null)
        {
            var categories = await _dbContext.Categories.ToListAsync();

            CategoryEntity root = null;
            if (rootId.HasValue)
            {
                root = categories.SingleOrDefault(x => x.Id == rootId.Value);
            }
            else
            {
                // find the minimal id as the default root id
                root = categories.Where(x => x.ParentId == null).OrderBy(x => x.Id).FirstOrDefault(); 
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

        public async Task<Result<CategoryTreeView>> GetCategoryTreeAsync(string rootCode)
        {
            var categories = await _dbContext.Categories.ToListAsync();

            CategoryEntity root = null;
            if (!string.IsNullOrEmpty(rootCode))
            {
                root = categories.SingleOrDefault(x => x.Code == rootCode);
            }
            else
            {
                // find the minimal id as the default root id
                root = categories.Where(x => x.ParentId == null).OrderBy(x => x.Id).FirstOrDefault();
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
