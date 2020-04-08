using ZDeals.Data.Entities.Sales;
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
    }
}
