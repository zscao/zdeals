using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Mapping
{
    public static class DealMapping
    {
        public static  Deal ToDealModel(this DealEntity entity)
        {
            if (entity == null) return null;

            return new Deal
            {
                Id = entity.Id,
                Title = entity.Title,
                Highlight = entity.Highlight,
                Description = entity.Description,
                DealPrice = entity.DealPrice,
                FullPrice = entity.FullPrice,
                Discount = entity.Discount,
                ExpiryDate = entity.ExpiryDate,
                Store = entity.Store?.ToStoreModel(),
                Source = entity.Source,
                Brand = entity.Brand?.ToBrandModel(),
                FreeShipping = entity.FreeShipping,
                Status = entity.Status.ToString(),
            };
        }
    }
}
