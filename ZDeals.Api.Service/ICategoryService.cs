using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryTreeView>>> SearchCategoriesAsync();

        Task<Result<CategoryTreeView>> GetCategoryByIdAsync(int categoryId);

        Task<Result<CategoryTreeView>> CreateCategoryAsync(CreateCategoryRequest request);

        Task<Result<CategoryTreeView>> GetCategoryTree(int? rootId = null); 
    }
}
