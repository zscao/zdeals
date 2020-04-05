using System.Security.Claims;

using ZDeals.Api.Contract.Models;

namespace ZDeals.Identity
{
    public interface IAuthenticationService
    {
        string GenerateJWT(User user, string jwtId);

        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
