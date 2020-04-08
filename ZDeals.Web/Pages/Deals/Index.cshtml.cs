using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Pages.Deals
{
    public class IndexModel : PageModel
    {
        private readonly IDealService _dealService;

        public IndexModel(IDealService dealService)
        {
            _dealService = dealService;
        }

        public DealsSearchResult DealResult { get; private set; }

        public async Task OnGet(string c)
        {
            var result = await _dealService.SearchDeals(c);
            if (result.HasError())
            {
                DealResult = new DealsSearchResult()
                {
                    Deals = new List<Deal>()
                };
            }

            DealResult = result.Data;
        }
    }
}