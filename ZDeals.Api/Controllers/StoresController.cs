using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(ApiRoutes.Stores.Base)]
    public class StoresController : ControllerBase
    {

        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<Result>> Search()
        {
            return await _storeService.SearchDealsAsync();
        }


        [HttpGet("{storeId}")]
        public async Task<ActionResult<Result>> GetById(int storeId)
        {
            return await _storeService.GetStoreByIdAsync(storeId);
        }


        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateStoreRequest request)
        {
            var result = await _storeService.CreateStoreAsync(request);
            return Created($"api/stores/{result.Data.Id}", result);
        }

        [HttpPut("{storeId}")]
        public async Task<ActionResult<Result>> Update(int storeId, [FromBody] UpdateStoreRequest request)
        {
            return await _storeService.UpdateStoreAsync(storeId, request);
        }
    }
}