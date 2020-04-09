using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Pages.Shared.Components.SearchBar
{
    public class SearchBarViewComponent: ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public SearchBarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _categoryService.GetCategoryTreeAsync();
            var model = new SearchBarModel
            {
                Categories = result.Data.ToCategoryList(true)
            };
            return View(model);
        }
    }


    public class SearchBarModel
    {
        public IEnumerable<CategoryListView> Categories { get; set; }
    }
}
