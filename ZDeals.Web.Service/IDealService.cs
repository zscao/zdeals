using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealService
    {
        Task<Result<DealsSearchResult>> SearchDeals(string categoryCode = null, string keywords = null);
    }
}
