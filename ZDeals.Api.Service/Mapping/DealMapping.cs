using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities.Sales;

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
                Descrition = entity.Description,
                DealPrice = entity.DealPrice,
                FullPrice = entity.FullPrice,
                Discount = entity.Discount,
                PublishedDate = entity.PublishedDate,
                ExpiryDate = entity.ExpiryDate,
                Store = entity.Store?.ToStoreModel(),
                Source = entity.Source,
            };
        }

        public static DealList ToDealListModel(this DealEntity entity)
        {
            if (entity == null) return null;

            return new DealList
            {
                Id = entity.Id,
                Title = entity.Title,
                PublishedDate = entity.PublishedDate,
                ExpiryDate = entity.ExpiryDate,
                Store = entity.Store?.ToStoreModel()
            };
        }
    }
}
