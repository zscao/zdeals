using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDetail>>> SearchCategoriesAsync();

        Task<Result<CategoryDetail>> GetCategoryByIdAsync(int categoryId);

        Task<Result<CategoryDetail>> GetCategoryByCodeAsync(string categoryCode);

        Task<Result<CategoryDetail>> CreateCategoryAsync(CreateCategoryRequest request);

        Task<Result<CategoryDetail>> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request);

        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(int? rootId = null);

        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(string rootCode);
    }
}
