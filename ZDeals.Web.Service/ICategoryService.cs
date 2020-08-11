using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface ICategoryService
    {
        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(string? rootCode = null);

        Task<Result<IEnumerable<CategoryListView>>> GetCategoryListAsync(string? rootCode = null);
    }
}
