using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Api.Helpers;
using ZDeals.Web.Api.Models;
using ZDeals.Web.Api.Options;
using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly IDealService _dealService;
        private readonly PictureStorageOptions _pictureStorageOptions;

        public DealsController(IDealService dealService, PictureStorageOptions pictureStorageOptions)
        {
            _dealService = dealService;
            _pictureStorageOptions = pictureStorageOptions;
        }

        /// <summary>
        /// returns all deals in the system
        /// </summary>
        /// <remarks>
        ///     Sample request: /api/deals
        /// </remarks>
        /// <response code="200">all deals in the system</response>
        [HttpGet]
        public async Task<ActionResult<Result>> Search([FromQuery] DealsSearchRequest request)
        {
            var result = await _dealService.SearchDeals(request);

            if (result.HasError() == false && string.IsNullOrEmpty(_pictureStorageOptions.GetPictureUrl) == false)
                result.Data.Deals.SetPictureAbsoluteUrl(_pictureStorageOptions);

            return result;
        }
    }
}
