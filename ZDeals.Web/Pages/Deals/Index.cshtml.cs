using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Collections.Generic;
using System.Threading.Tasks;
using ZDeals.Common.Constants;
using ZDeals.Web.Models;
using ZDeals.Web.Options;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Pages.Deals
{
    public class IndexModel : PageModel
    {
        const int DefaultPageSize = 10;
        const int DefaultPageNumber = 1;

        private readonly IDealSearchService _dealService;
        private readonly PictureStorageOptions _pictureStorageOptions;

        public IndexModel(IDealSearchService dealService, PictureStorageOptions pictureStorageOptions)
        {
            _dealService = dealService;
            _pictureStorageOptions = pictureStorageOptions;
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

            var data = await SearchDeals(query.CategoryCode, query.Keywords, DefaultPageNumber);

            HttpContext.Response.Headers.Add("More-Deals", data.More.ToString().ToLower());

            DealResult = data;
        }


        public async Task<PartialViewResult> OnGetMore(string c, string w, int? p)
        {
            int pageNumber = p ?? DefaultPageNumber;

            var data = await SearchDeals(c, w, pageNumber);

            HttpContext.Response.Headers.Add("More-Deals", data.More.ToString().ToLower());

            return Partial("_DealListPartial", data);
        }


        private async Task<DealsSearchResult> SearchDeals(string categoryCode, string keywords, int pageNumber)
        {
            var result = await _dealService.SearchDeals(new DealsSearchRequest
            {
                Category = categoryCode,
                Keywords = keywords,
                Store = null,
                Page = pageNumber
            });
            if (result.HasError())
            {
                DealResult = new DealsSearchResult()
                {
                    Deals = new List<Deal>(),
                    Category = categoryCode,
                    Keywords = keywords
                };
            }
            else
            {
                foreach (var deal in result.Data.Deals)
                {
                    if (!string.IsNullOrEmpty(deal.Picture))
                        deal.Picture = $"{_pictureStorageOptions.GetPictureUrl}/{DefaultValues.DealPicturesContainer}/{deal.Picture}";
                }
            }

            return result.Data;
        }

    }
}