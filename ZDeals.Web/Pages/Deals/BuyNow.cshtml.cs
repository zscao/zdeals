using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;
using ZDeals.Net;
using ZDeals.Web.Service;

namespace ZDeals.Web.Pages.Deals
{
    public class BuyNowModel : PageModel
    {
        private IDealService _dealService;
        private IPageService _pageService;

        public BuyNowModel(IDealService dealService, IPageService pageService)
        {
            _dealService = dealService;
            _pageService = pageService;
        }

        public async Task<IActionResult> OnGet(int id, string c)
        {
            var result = await _dealService.GetDealById(id);
            if (result.HasError())
                return RedirectToPage("NotFound", new { c });

            var source = result.Data.Source;
            var status = await _pageService.CheckPageStatus(source);

            if(status == PageStatus.NotFound)
            {
                await _dealService.MarkDealExpired(id);
                return RedirectToPage("NotFound", new { c });
            }

            return Redirect(result.Data.Source);
        }
    }
}