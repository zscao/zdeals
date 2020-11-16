using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles = ApiRoles.Admin )]
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
        [ProducesDefaultResponseType(typeof(PagedStores))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Search()
        {
            return await _storeService.SearchStoresAsync();
        }


        [HttpGet("{storeId}")]
        [ProducesDefaultResponseType(typeof(Store))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetById(int storeId)
        {
            return await _storeService.GetStoreByIdAsync(storeId);
        }


        [HttpPost]
        [ProducesDefaultResponseType(typeof(Store))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Create(CreateStoreRequest request)
        {
            var result = await _storeService.CreateStoreAsync(request);
            return Created($"api/stores/{result.Data.Id}", result);
        }

        [HttpPut("{storeId}")]
        [ProducesDefaultResponseType(typeof(Store))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Update(int storeId, [FromBody] UpdateStoreRequest request)
        {
            return await _storeService.UpdateStoreAsync(storeId, request);
        }
    }
}