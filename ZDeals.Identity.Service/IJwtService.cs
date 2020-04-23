using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Responses;

namespace ZDeals.Identity
{
    public interface IJwtService
    {
        Task<Result<JwtResponse>> GenerateJwtTokenAsync(User user);
        Task<Result<JwtResponse>> RefreshTokenAsync(string token, string refreshToken);
    }
}
