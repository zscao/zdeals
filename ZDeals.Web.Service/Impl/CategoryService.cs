using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities.Sales;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ZDealsDbContext _dbContext;

        public CategoryService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CategoryTreeView>> GetCategoryTreeAsync(string rootCode = null)
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
                root = categories.OrderBy(x => x.Id).FirstOrDefault(x => x.ParentId == null);
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
