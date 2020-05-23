using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealService
    {
        Task<Result<DealsSearchResult>> SearchDeals(string categoryCode, string keywords, int pageSize, int pageNumber);

        Task<Result<Deal>> GetDealById(int dealId);

        Task<Result<Deal>> MarkDealExpired(int dealId);
    }
}
