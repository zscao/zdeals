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
        public async Task<ActionResult<Result>> Search(int? pageSize, int? pageNumber)
        {
            return await _dealService.SearchDeals(pageSize, pageNumber);
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
            return await _dealService.GetDealById(dealId);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create([FromBody] CreateDealRequest request)
        {
            var result = await _dealService.CreateDeal(request);

            return Created($"{ApiRoutes.Deals.Base}/{result.Data?.Id}", result);
        }

        [HttpPut("{dealId}")]
        public async Task<ActionResult<Result>> Update(int dealId, [FromBody] UpdateDealRequest request)
        {
            return await _dealService.UpdateDeal(dealId, request);
        }

        [HttpGet("{dealId}/store")]
        public async Task<ActionResult<Result>> GetStore(int dealId)
        {
            return await _dealService.GetDealStore(dealId);
        }

        [HttpGet("{dealId}/pictures")]
        public async Task<ActionResult<Result>> GetPictures(int dealId)
        {
            return await _dealService.GetPictures(dealId);
        }

        [HttpPut("{dealId}/pictures")]
        public async Task<ActionResult<Result>> SavePicture(int dealId, SaveDealPictureRequest request)
        {
            var result = await _dealService.SavePicture(dealId, request);
            return Created($"{ApiRoutes.Deals.Base}/{dealId}/pictures/{result.Data.FileName}", result);
        }
    }
}