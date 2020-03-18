using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<Category>>> SearchCategories();

        Task<Result<Category>> GetCategoryById(int categoryId);

        Task<Result<Category>> CreateCategory(CreateCategoryRequest request);
    }
}
