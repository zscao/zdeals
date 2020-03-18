using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;
using ZDeals.Api.Service;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedStoreList>> Search()
        {
            var result = await _storeService.SearchDeals();
            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }

            return result.Data;
        }

        [HttpPost(ApiRoutes.Stores.CreateStore)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Store>> Create(CreateStoreRequest request)
        {
            var result = await _storeService.CreateStore(request);

            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }

            return Created($"api/stores/{result.Data.Id}", result.Data);
        }

        [HttpGet(ApiRoutes.Stores.GetStoreById)]
        public async Task<ActionResult<Store>> GetById(int storeId)
        {
            var result = await _storeService.GetStoreById(storeId);
            if (result.HasError())
            {
                return BadRequest(result.Errors);
            }
            return result.Data;
        }
    }
}