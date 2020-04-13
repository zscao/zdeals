using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<Category>>> SearchCategoriesAsync();

        Task<Result<Category>> GetCategoryByIdAsync(int categoryId);

        Task<Result<Category>> GetCategoryByCodeAsync(string categoryCode);

        Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request);

        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(int? rootId = null);

        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(string rootCode);
    }
}
