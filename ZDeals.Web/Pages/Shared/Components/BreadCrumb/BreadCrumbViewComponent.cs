using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System.Collections.Generic;
using System.Threading.Tasks;

using ZDeals.Web.Helpers;
using ZDeals.Web.Service;

namespace ZDeals.Web.Pages.Shared.Components.BreadCrumb
{
    public class BreadCrumbViewComponent: ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public BreadCrumbViewComponent(ICategoryService categorySerive)
        {
            _categoryService = categorySerive;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Models.BreadCrumb breadCrumb = null;

            var request = HttpContext.Request;
            var url = request.Path.Value.ToLower();
            if (url.Equals("/deals"))
            {
                string code = null;
                if (request.Query.TryGetValue("c", out StringValues values))
                {
                    code = values.ToString();
                }

                breadCrumb = await GetBreadCrumb(code);
            }

            if (breadCrumb == null) breadCrumb = new Models.BreadCrumb() { Items = new List<Models.BreadCrumbItem>() };

            return View(breadCrumb);
        }

        private async Task<Models.BreadCrumb> GetBreadCrumb(string categoryCode)
        {
            var code = Common.Constants.DefaultValues.DealsCategoryRoot;
            var cateResult = await _categoryService.GetCategoryTreeAsync(code);
            var baseUrl = "/deals?c=";

            return BreadCrumbHelper.GenerateCategoryBreadCrumb(categoryCode ?? code, cateResult.Data, baseUrl);
        }
    }
}
