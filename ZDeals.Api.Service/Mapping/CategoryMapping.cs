using System.Collections.Generic;
using System.Linq;
using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Api.Service.Mapping
{
    public static class CategoryMapping
    {
        public static Category ToCategoryMode(this CategoryEntity entity)
        {
            if (entity == null) return null;

            return new Category
            {
                Id = entity.Id,
                Code = entity.Code,
                Title = entity.Title
            };
        }

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

        public static IEnumerable<CategoryListView> ToCategoryList(this CategoryTreeView category)
        {
            return BuildChildren(category);
        }


        private static IEnumerable<CategoryListView> BuildChildren(CategoryTreeView category, IEnumerable<Category> path = null)
        {
            if (category == null) return null;

            var result = new List<CategoryListView>();

            var p = path?.ToList() ?? new List<Category>();
            p.Add(new Category
            {
                Id = category.Id,
                Code = category.Code,
                Title = category.Title
            });
            
            result.Add(new CategoryListView
            {
                Id = category.Id,
                Code = category.Code,
                Title = category.Title,
                Path = p
            });

            if (category.Children != null)
                foreach (var cate in category.Children)
                    result.AddRange(BuildChildren(cate, p));

            return result;
        }
    }
}
