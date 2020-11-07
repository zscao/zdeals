using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly IDealSearchService _dealSearchService;
        private readonly IDealService _dealService;

        public DealsController(IDealSearchService dealSearchService, IDealService dealService)
        {
            _dealSearchService = dealSearchService;
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
        public async Task<ActionResult<Result>> Search([FromQuery] DealSearchRequest request)
        {
            var result = await _dealSearchService.SearchDeals(request);
            return result;
        }

        [HttpPost("visit/{id}")]
        public async Task<ActionResult<Result>> Visit(int id)
        {
            var result = await _dealService.Visit(id);
            return result;
        }
        
        [HttpGet("price/{id}")]
        public async Task<ActionResult<Result>> PriceHistory(int id)
        {
            var result = await _dealService.GetDealPriceHistory(id);
            return result;
        }
    }
}
