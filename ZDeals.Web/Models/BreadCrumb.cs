using System.Collections.Generic;

namespace ZDeals.Web.Models
{
    public class BreadCrumb
    {
        public List<BreadCrumbItem> Items { get; set; }
    }


    public class BreadCrumbItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
