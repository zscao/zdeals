using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface ICategoryService
    {
        Task<Result<CategoryTreeView>> GetCategoryTreeAsync(int? rootId = null);
    }
}
