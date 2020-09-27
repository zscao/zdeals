using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Requests;

namespace ZDeals.Identity
{
    public interface IUserService
    {
        Task<Result<User>> CreateUserAsync(CreateUserRequest request);

        Task<Result<User>> GetUserByName(string username);

    }
}
