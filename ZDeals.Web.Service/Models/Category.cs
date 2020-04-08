using System.Collections.Generic;

namespace ZDeals.Web.Service.Models
{
    public class CategoryTreeView
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public IEnumerable<CategoryTreeView> Children { get; set; }
    }
}
