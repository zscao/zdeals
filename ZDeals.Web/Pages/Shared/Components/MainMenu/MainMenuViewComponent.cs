using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ZDeals.Web.Pages.Shared.Components.MainMenu
{
    public class MainMenuViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await Task.FromResult(true);
            return View();
        }
    }
}
