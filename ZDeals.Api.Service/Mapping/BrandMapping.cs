using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Mapping
{
    public static class BrandMapping
    {
        public static Brand ToBrandModel(this BrandEntity entity)
        {
            if (entity == null) return null;

            return new Brand
            {
                Code = entity.Code,
                Name = entity.Name,
                DisplayOrder = entity.DisplayOrder
            };
        }
    }
}
