using System.Threading.Tasks;

using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Requests;
using ZDeals.Identity.Contract.Responses;
using ZDeals.Common;

namespace ZDeals.Identity
{
    public interface IUserService
    {
        Task<Result<User>> CreateUserAsync(CreateUserRequest request);

        Task<Result<User>> GetUserByName(string username);

        Task<Result<AuthenticationResponse>> AuthenticateAsync(string username, string password);

        Task<Result<AuthenticationResponse>> RefreshTokenAsync(string token, string refreshToken);
    }
}
