using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealService
    {
        Task<Result<DealsSearchResult>> SearchDeals(DealsSearchRequest request);

        Task<Result<Deal>> GetDealById(int dealId);

        Task<Result<Deal>> MarkDealExpired(int dealId);
    }

    public class DealsSearchRequest
    {
        public string Category { get; set; }
        public string Keywords { get; set; }

        public int? PageNumber { get; set; }

        public string Store { get; set; }
    }
}
