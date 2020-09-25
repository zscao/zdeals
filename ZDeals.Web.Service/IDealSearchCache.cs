
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealSearchCache
    {
        void SetSearchResult(DealSearchRequest request, DealSearchResult result);

        DealSearchResult? GetSearchResult(DealSearchRequest request);
    }
}
