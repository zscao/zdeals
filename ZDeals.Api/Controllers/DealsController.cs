using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Helpers;
using ZDeals.Api.Service;
using ZDeals.Common;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles = ApiRoles.Admin)]
    [ApiController]
    [Route(ApiRoutes.Deals.Base)]
    public class DealsController : ControllerBase
    {
        private readonly IDealService _dealService;

        public DealsController(IDealService dealService)
        {            
            _dealService = dealService;
        }

        /// <summary>
        /// returns all deals in the system
        /// </summary>
        /// <remarks>
        ///     Sample request: /api/deals
        /// </remarks>
        /// <response code="200">all deals in the system</response>
        [HttpGet]
        public async Task<ActionResult<Result>> Search([FromQuery] SearchDealRequest request)
        {
            return await _dealService.SearchDealsAsync(request);
        }

        /// <summary>
        /// returns the details of a deal specified by id
        /// </summary>
        /// <remarks>
        ///     Sample request: /api/deals/1
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">details of the deal</response>
        /// <response code="400">the deal is not found</response>
        [HttpGet("{dealId}")]
        public async Task<ActionResult<Result>> GetById(int dealId)
        {
            return await _dealService.GetDealByIdAsync(dealId);
        }

        [HttpGet("exist")]
        public async Task<ActionResult<Result>> CheckExistenceBySource([FromQuery] string source)
        {
            return await _dealService.CheckExistenceBySourceAsync(source);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create([FromBody] CreateDealRequest request)
        {
            var result = await _dealService.CreateDealAsync(request);

            return Created($"{ApiRoutes.Deals.Base}/{result.Data?.Id}", result);
        }

        [HttpPut("{dealId}")]
        public async Task<ActionResult<Result>> Update(int dealId, [FromBody] UpdateDealRequest request)
        {
            return await _dealService.UpdateDealAsync(dealId, request);
        }

        [HttpDelete("{dealId}")]
        public async Task<ActionResult<Result>> Delete(int dealId)
        {
            return await _dealService.DeleteDealAsync(dealId);
        }

        [HttpPost("{dealId}/verify")]
        public async Task<ActionResult<Result>> Verify(int dealId)
        {
            _dealService.RequestContext = this.GetRequestContext();
            return await _dealService.VerifyDealAsync(dealId);
        }

        [HttpPost("{dealId}/recycle")]
        public async Task<ActionResult<Result>> Recycle(int dealId)
        {
            _dealService.RequestContext = this.GetRequestContext();
            return await _dealService.RecycleDealAsync(dealId);
        }

        [HttpGet("{dealId}/store")]
        public async Task<ActionResult<Result>> GetStore(int dealId)
        {
            return await _dealService.GetDealStoreAsync(dealId);
        }

        [HttpGet("{dealId}/pictures")]
        public async Task<ActionResult<Result>> GetPictures(int dealId)
        {
            return await _dealService.GetPicturesAsync(dealId);
        }

        [HttpPut("{dealId}/pictures")]
        public async Task<ActionResult<Result>> SavePicture(int dealId, SaveDealPictureRequest request)
        {
            var result = await _dealService.SavePictureAsync(dealId, request);
            return Created($"{ApiRoutes.Deals.Base}/{dealId}/pictures/{result.Data.FileName}", result);
        }

        [HttpGet("{dealId}/categories")]
        public async Task<ActionResult<Result>> GetCategories(int dealId)
        {
            return await _dealService.GetCategoriesAsync(dealId);
        }

        [HttpPut("{dealId}/categories")]
        public async Task<ActionResult<Result>> SaveCategories(int dealId, SaveDealCategoriesRequest request)
        {
            return await _dealService.SaveCategoriesAsync(dealId, request);
        }
    }
}