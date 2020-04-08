using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using ZDeals.Web.Service;

namespace ZDeals.Web.Pages.Shared.Components.MainCategory
{
    public class MainCategoryViewComponent: ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public MainCategoryViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            return View(result.Data);
        }
    }
}
