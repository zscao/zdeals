using ZDeals.Data.Entities;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Mapping
{
    public static class StoreMapping
    {
        public static Store ToStoreModel(this StoreEntity entity)
        {
            if (entity == null) return null;

            return new Store
            {
                Id = entity.Id,
                Name = entity.Name,
                Website = entity.Website,
                Domain = entity.Domain,
            };
        }
    }
}
