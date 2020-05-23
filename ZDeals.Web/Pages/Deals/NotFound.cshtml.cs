using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDeals.Web.Models;

namespace ZDeals.Web.Pages.Deals
{
    public class NotFoundModel : PageModel
    {
        public void OnGet(string c, string w)
        {
            var query = new DealQuery
            {
                CategoryCode = c,
                Keywords = w,
            };

            ViewData["DealQuery"] = query;

        }
    }
}