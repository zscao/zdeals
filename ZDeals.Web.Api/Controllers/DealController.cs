using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class DealController : Controller
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService)
        {
            _dealService = dealService;
        }

        /// <summary>
        /// deprecated, as server side redirection is not in use 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            return BadRequest();

            //var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            //if (int.TryParse(id, out int dealId) == false) return new RedirectResult(baseUrl);


            //var clientIp = HttpContext.Connection.RemoteIpAddress;
            //var result = await _dealService.Visit(dealId, clientIp.ToString());
            //if (result.HasError() || string.IsNullOrEmpty(result.Data?.Source)) return new RedirectResult(baseUrl);

            //return new RedirectResult(result.Data.Source);


        }
    }
}