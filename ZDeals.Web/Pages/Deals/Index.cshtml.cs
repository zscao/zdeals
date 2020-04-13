using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Web.Models;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Pages.Deals
{
    public class IndexModel : PageModel
    {
        const int DefaultPageSize = 10;
        const int DefaultPageNumber = 1;

        private readonly IDealService _dealService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IDealService dealService, ICategoryService categoryService)
        {
            _dealService = dealService;
            _categoryService = categoryService;
        }

        public DealsSearchResult DealResult { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">category code</param>
        /// <param name="w">keywords</param>
        /// <param name="p">page number</param>
        /// <param name="ps">page size</param>
        /// <returns></returns>
        public async Task OnGet(string c, string w)
        {
            var query = new DealQuery
            {
                CategoryCode = c,
                Keywords = w,
            };

            ViewData["DealQuery"] = query;

            var result = await _dealService.SearchDeals(query.CategoryCode, query.Keywords, DefaultPageSize, DefaultPageNumber);
            if (result.HasError())
            {
                DealResult = new DealsSearchResult()
                {
                    Deals = new List<Deal>()
                };
            }

            HttpContext.Response.Headers.Add("More-Deals", result.Data.More.ToString().ToLower());

            DealResult = result.Data;
        }


        public async Task<PartialViewResult> OnGetMore(string c, string w, int? p)
        {
            int pageNumber = p ?? DefaultPageNumber;

            var result = await _dealService.SearchDeals(c, w, DefaultPageSize, pageNumber);
            if (result.HasError())
            {
                DealResult = new DealsSearchResult()
                {
                    Deals = new List<Deal>()
                };
            }

            HttpContext.Response.Headers.Add("More-Deals", result.Data.More.ToString().ToLower());

            return Partial("_DealListPartial", result.Data);
        }

    }
}