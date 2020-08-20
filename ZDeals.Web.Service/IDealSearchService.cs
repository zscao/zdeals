using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealSearchService
    {
        Task<Result<DealsSearchResult?>> SearchDeals(DealsSearchRequest request);
    }

    public class DealsSearchRequest
    {
        public string? Category { get; set; }
        public string? Keywords { get; set; }

        public int? Page { get; set; }

        public string? Store { get; set; }

        public string? Brand { get; set; }

        public string? Sort { get; set; }
    }
}
