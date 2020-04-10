using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;

using ZDeals.Web.Service;

namespace ZDeals.Web.Pages.Deals
{
    public class BuyNowModel : PageModel
    {
        private IDealService _dealService;

        public BuyNowModel(IDealService dealService)
        {
            _dealService = dealService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var result = await _dealService.GetDealById(id);
            if (result.HasError())
                return Redirect("deals");

            return Redirect(result.Data.Source);
        }
    }
}