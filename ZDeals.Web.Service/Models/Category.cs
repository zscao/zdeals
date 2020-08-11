using System.Collections.Generic;

namespace ZDeals.Web.Service.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string? Code { get; set; }
        public string? Title { get; set; }
    }

    public class CategoryTreeView : Category
    {
        public int DisplayOrder { get; set; }

        public IEnumerable<CategoryTreeView>? Children { get; set; }
    }

    public class CategoryListView : Category
    {
        public IEnumerable<Category>? Path { get; set; }
    }
}
