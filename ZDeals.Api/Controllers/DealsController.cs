using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;
using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;
using ZDeals.Api.Service;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<PagedDealList> Search(int? pageSize, int? pageNumber)
        {
            var response = new PagedDealList
            {
                Data = new List<DealList>()
                {
                    new DealList
                    {
                        Id = 1,
                        Title = "20% off on all Dell monitors"
                    }
                },
                TotalCount = 1,
                PageSize = pageSize ?? 10,
                PageNumber = pageNumber ?? 1,
                PrevPage = null,
                NextPage = "api/deals?pageSize=10&pageNumber=2"
            };
            return response;
        }

        [HttpPost(ApiRoutes.Deals.CreateDeal)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Deal>> Create(CreateDealRequest request)
        {
            var result = await _dealService.CreateDeal(request);
            if (result.HasError())
                return BadRequest(result.Errors);

            return Created($"/api/deals/{result.Data.Id}", result.Data);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Deal>> GetById(int dealId)
        {
            var result = await _dealService.GetDealById(dealId);
            if (result.HasError())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet(ApiRoutes.Deals.GetDealStore)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Store>> GetStore(int dealId)
        {
            var result = await _dealService.GetDealStore(dealId);
            if (result.HasError())
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPut(ApiRoutes.Deals.UpdateDealStore)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Deal>> UpdateDealStore(int dealId, int storeId)
        {
            return await Task.FromResult(new Deal());
        }
    }
}