using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ZDeals.Web.Pages.Shared.Components.SearchBar
{
    public class SearchBarViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await Task.FromResult(true);
            return View();
        }
    }
}
