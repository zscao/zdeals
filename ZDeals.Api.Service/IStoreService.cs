using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IStoreService
    {
        Task<Result<PagedStores>> SearchStoresAsync();

        Task<Result<Store>> GetStoreByIdAsync(int storeId);

        Task<Result<Store>> CreateStoreAsync(CreateStoreRequest request);

        Task<Result<Store>> UpdateStoreAsync(int storeId, UpdateStoreRequest request);

    }
}
