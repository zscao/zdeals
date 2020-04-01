using ZDeals.Api.Contract.Models;
using ZDeals.Data.Entities.Accounts;

namespace ZDeals.Api.Service.Mapping
{
    public static class UserMapping
    {
        public static User ToUserModel(this UserEntity entity)
        {
            if (entity == null) return null;

            return new User
            {
                Username = entity.Username,
                Nickname = entity.Nickname,
                Role = entity.Role
            };
        }
    }
}
