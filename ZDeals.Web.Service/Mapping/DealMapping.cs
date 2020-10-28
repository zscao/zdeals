using System;

using ZDeals.Data.Entities;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Mapping
{
    public static class DealMapping
    {
        public static Deal? ToDealModel(this DealEntity entity, string? pictureStorageUrl)
        {
            if (entity == null) return null;

            var pictureWithStorage = string.IsNullOrEmpty(pictureStorageUrl) ? entity.DefaultPicture : $"{pictureStorageUrl}/deals/{entity.DefaultPicture}";

            return new Deal
            {
                Id = entity.Id,
                Title = entity.Title,
                Highlight = entity.Highlight,
                Description = entity.Description,
                DealPriceString = entity.DealPrice.ToPriceWithCurrency(),
                UsedPriceString = entity.DealPrice < entity.UsedPrice ? entity.UsedPrice.ToPriceWithCurrency() : string.Empty,
                Discount = entity.Discount,
                FreeShipping = entity.FreeShipping,
                CreatedTimeString = entity.CreatedTime.ToCreatedTimeString(),
                Store = entity.Store?.ToStoreModel(),
                Picture = pictureWithStorage,
                Source = entity.Source,
                Brand = entity.Brand?.Name
            };
        }

        public static string ToCreatedTimeString(this DateTime createdTime)
        {
            var span = DateTime.UtcNow - createdTime;

            if (span.TotalMinutes < 60)
                return span.TotalMinutes.ToString("0 minutes ago");
            else if (span.TotalHours < 24)
                return span.TotalHours.ToString("0 hours ago");
            else
                return span.TotalDays.ToString("0 days ago");
        }
    }
}
