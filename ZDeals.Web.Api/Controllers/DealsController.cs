using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Api.Helpers;
using ZDeals.Web.Api.Options;
using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly IDealSearchService _dealSearchService;
        private readonly PictureStorageOptions _pictureStorageOptions;

        public DealsController(IDealSearchService dealSearchService, PictureStorageOptions pictureStorageOptions)
        {
            _dealSearchService = dealSearchService;
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
            var result = await _dealSearchService.SearchDeals(request);
            if (result.HasError()) return result;

            var data = result.Data;

            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            data.Deals.SetSourceToLocal(baseUrl);

            if (string.IsNullOrEmpty(_pictureStorageOptions.GetPictureUrl) == false)
                data.Deals.SetPictureAbsoluteUrl(_pictureStorageOptions);
           
            return result;
        }
    }
}
