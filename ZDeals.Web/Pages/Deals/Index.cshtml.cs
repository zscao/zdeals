using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDeals.Web.Helpers;
using ZDeals.Web.Models;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Pages.Deals
{
    public class IndexModel : PageModel
    {
        private readonly IDealService _dealService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IDealService dealService, ICategoryService categoryService)
        {
            _dealService = dealService;
            _categoryService = categoryService;
        }

        public DealsSearchResult DealResult { get; private set; }

        public async Task OnGet(string c, string w)
        {
            var query = new DealQuery
            {
                CategoryCode = c,
                Keywords = w
            };

            ViewData["DealQuery"] = query;

            var result = await _dealService.SearchDeals(query.CategoryCode, query.Keywords);
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