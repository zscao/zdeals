using ZDeals.Data.Entities.Sales;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Mapping
{
    public static class DealMapping
    {
        public static Deal ToDealModel(this DealEntity entity)
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
                PublishedDate = entity.PublishedDate,
                ExpiryDate = entity.ExpiryDate,
                Store = entity.Store?.ToStoreModel(),
                Picture = entity.DefaultPicture,
                Source = entity.Source,
            };
        }
    }
}
