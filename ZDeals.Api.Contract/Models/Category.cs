using System.Collections.Generic;

namespace ZDeals.Api.Contract.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }

    public class CategoryDetail: Category
    {
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
    }


    public class CategoryTreeView: Category
    {
        public IEnumerable<CategoryTreeView> Children { get; set; }
    }

    public class CategoryListView: Category
    {
        public IEnumerable<Category> Path { get; set; }
    }

    public class CategoryList : DataList<CategoryListView>
    {
    }
}
