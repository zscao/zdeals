using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Api.Service.Mapping
{
    public static class CategoryMapping
    {
        public static Category ToCategoryModel(this CategoryEntity entity)
        {
            if (entity == null) return null;

            return new Category
            {
                Id = entity.Id,
                Code = entity.Code,
                Title = entity.Title,
                DisplayOrder = entity.DisplayOrder
            };
        }
    }
}
