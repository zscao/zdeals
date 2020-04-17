using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Mapping
{
    public static class DealPictureMapping
    {
        public static DealPicture ToDealPictureModel(this DealPictureEntity entity, bool isDefault = false)
        {
            if (entity == null) return null;

            return new DealPicture
            {
                Title = entity.Title,
                FileName = entity.FileName,
                Alt = entity.Alt,
                IsDefaultPicture = isDefault
            };
        }
    }
}
