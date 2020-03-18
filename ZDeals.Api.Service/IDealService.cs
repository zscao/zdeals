using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IDealService
    {
        Task<Result<Deal>> CreateDeal(CreateDealRequest request);

        Task<Result<Deal>> GetDealById(int dealId);

        Task<Result<Store>> GetDealStore(int dealId);

        Task<Result<Deal>> UpdateDeal(int dealId, UpdateDealRequest request);

        Task<Result<Deal>> UpdateStore(int dealId, int storeId);
    }
}
