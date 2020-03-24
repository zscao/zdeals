using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;

namespace ZDeals.Api.Controllers
{
    [ApiController]
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
        [HttpGet(ApiRoutes.Deals.SearchDeals)]
        public async Task<ActionResult<Result>> Search(int? pageSize, int? pageNumber)
        {
            return await _dealService.SearchDeals(pageSize, pageNumber);
        }

        [HttpPost(ApiRoutes.Deals.CreateDeal)]
        public async Task<ActionResult<Result>> Create(CreateDealRequest request)
        {
            var result = await _dealService.CreateDeal(request);

            return Created($"/api/deals/{result.Data.Id}", result);
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
        [HttpGet(ApiRoutes.Deals.GetDealById)]
        public async Task<ActionResult<Result>> GetById(int dealId)
        {
            return await _dealService.GetDealById(dealId);
        }

        [HttpGet(ApiRoutes.Deals.GetDealStore)]
        public async Task<ActionResult<Result>> GetStore(int dealId)
        {
            return await _dealService.GetDealStore(dealId);
        }
    }
}