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

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var home = "https://www.google.com";

            if(int.TryParse(id, out int dealId) == false) return new RedirectResult(home);

            var result = await _dealService.Visit(dealId);
            if (result.HasError() || string.IsNullOrEmpty(result.Data?.Source)) return new RedirectResult(home);

            return new RedirectResult(result.Data.Source);            
        }
    }
}
