using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ZDeals.Web.Pages.Shared.Components.NewsLetter
{
    public class NewsLetterViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await Task.FromResult(true);
            return View();
        }
    }
}
