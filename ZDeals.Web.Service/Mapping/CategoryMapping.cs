using System.Collections.Generic;
using System.Linq;
using ZDeals.Data.Entities;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Mapping
{
    public static class CategoryMapping
    {
        public static CategoryTreeView ToCategoryTreeView(this CategoryEntity entity)
        {
            if (entity == null) return null;

            return new CategoryTreeView
            {
                Id = entity.Id,
                Code = entity.Code,
                Title = entity.Title,
                DisplayOrder = entity.DisplayOrder
            };
        }

        public static IEnumerable<CategoryListView> ToCategoryList(this CategoryTreeView category, bool includeRootNode)
        {
            if (category == null) return null;

            if (!includeRootNode) return BuildChildren(category.Children);

            var result = new List<CategoryListView>();
            var path = new List<Category> { category };
            result.Add(new CategoryListView
            {
                Id = category.Id,
                Code = category.Code,
                Title = category.Title,
                Path = path
            });

            if(category.Children?.Count() > 0) result.AddRange(BuildChildren(category.Children, path));

            return result;
        }

        private static IEnumerable<CategoryListView> BuildChildren(IEnumerable<CategoryTreeView> categories, IEnumerable<Category> path = null)
        {
            if (categories == null || categories.Count() == 0) return null;

            var result = new List<CategoryListView>();

            foreach (var category in categories)
            {
                var p = path?.ToList() ?? new List<Category>();
                p.Add(category);

                result.Add(new CategoryListView
                {
                    Id = category.Id,
                    Code = category.Code,
                    Title = category.Title,
                    Path = p
                });

                if (category.Children?.Count() > 0)
                    result.AddRange(BuildChildren(category.Children, p));
            }

            return result;
        }
    }
}
