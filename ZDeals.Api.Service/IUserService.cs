using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IUserService
    {
        Task<Result<User>> CreateUserAsync(CreateUserRequest request);

        Task<Result<User>> AuthenticateAsync(LoginRequest request);
    }
}
