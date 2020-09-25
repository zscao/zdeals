using System.Collections.Generic;
using System.Linq;

using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Helpers
{
    public static class DealFilterHelper
    {
        public static DealFilter ApplySelected(this DealFilter filter, IEnumerable<string>? selected)
        {
            if (filter.Items == null) return filter;
            if (selected == null || selected.Count() == 0) return filter;

            foreach(var item in filter.Items)
            {
                item.Selected = selected.Contains(item.Value);
            }
            return filter;
        }
    }
}
