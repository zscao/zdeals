using System.Security.Claims;

using ZDeals.Identity.Contract.Models;

namespace ZDeals.Identity
{
    public interface IAuthenticationService
    {
        string GenerateJWT(User user, string jwtId);

        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
