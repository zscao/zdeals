using System.Collections.Generic;

using ZDeals.Web.Models;
using ZDeals.Web.Service.Models;
using ZDeals.Web.Service.Mapping;
using System.Linq;

namespace ZDeals.Web.Helpers
{
    public static class BreadCrumbHelper
    {
        public static BreadCrumb GenerateCategoryBreadCrumb(string categoryCode, CategoryTreeView category = null, string baseUrl = "/")
        {
            var result = new BreadCrumb
            {
                Items = new List<BreadCrumbItem>()
            };

            if (category == null) return result;

            var list = category.ToCategoryList(true);
            var cate = list.SingleOrDefault(x => x.Code == categoryCode);

            if(cate != null)
            {
                foreach(var path in cate.Path)
                {
                    result.Items.Add(new BreadCrumbItem
                    {
                        Title = path.Title,
                        Link = baseUrl + path.Code
                    });
                }
            }

            return result;
        }
    }
}
