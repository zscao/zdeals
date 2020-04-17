using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Data.Entities;

namespace ZDeals.Identity.Service.Mapping
{
    public static class UserMapping
    {
        public static User ToUserModel(this UserEntity entity)
        {
            if (entity == null) return null;

            return new User
            {
                UserId = entity.Id,
                Username = entity.Username,
                Nickname = entity.Nickname,
                Role = entity.Role
            };
        }
    }
}
