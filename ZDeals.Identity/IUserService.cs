using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;
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
