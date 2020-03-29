using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;

namespace ZDeals.Api.Controllers
{
    [ApiController]
    public class StoresController : ControllerBase
    {

        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet(ApiRoutes.Stores.SearchStores)]
        public async Task<ActionResult<Result>> Search()
        {
            return await _storeService.SearchDeals();
        }


        [HttpGet(ApiRoutes.Stores.GetStoreById)]
        public async Task<ActionResult<Result>> GetById(int storeId)
        {
            return await _storeService.GetStoreById(storeId);
        }


        [HttpPost(ApiRoutes.Stores.CreateStore)]
        public async Task<ActionResult<Result>> Create(CreateStoreRequest request)
        {
            var result = await _storeService.CreateStore(request);
            return Created($"api/stores/{result.Data.Id}", result);
        }

        [HttpPut(ApiRoutes.Stores.UpdateStore)]
        public async Task<ActionResult<Result>> Update(int storeId, [FromBody] UpdateStoreRequest request)
        {
            return await _storeService.UpdateStore(storeId, request);
        }
    }
}