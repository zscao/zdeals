using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IStoreService
    {
        Task<Result<PagedStores>> SearchDeals();

        Task<Result<Store>> GetStoreById(int storeId);

        Task<Result<Store>> CreateStore(CreateStoreRequest request);

        Task<Result<Store>> UpdateStore(int storeId, UpdateStoreRequest request);

    }
}
