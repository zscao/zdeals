using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ZDeals.Api.Contract.Models;
using ZDeals.Common.Constants;
using ZDeals.Identity.Options;

namespace ZDeals.Identity.Impl
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters)
        {
            _jwtOptions = jwtOptions;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public string GenerateJWT(User user, string jwtId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? ApiRoles.Guest),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtId)
                }),
                Issuer = _jwtOptions.Issuer,
                Expires = DateTime.UtcNow.Add(_jwtOptions.TokenLifetime),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var parameter = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = _tokenValidationParameters.ValidateIssuerSigningKey,
                    IssuerSigningKey = _tokenValidationParameters.IssuerSigningKey,
                    ValidateIssuer = _tokenValidationParameters.ValidateIssuer,
                    ValidIssuer = _tokenValidationParameters.ValidIssuer,
                    ValidateAudience = _tokenValidationParameters.ValidateAudience,
                    RequireExpirationTime = _tokenValidationParameters.RequireExpirationTime,
                    ValidateLifetime = false // here we don't validate lifetime
                };

                var principal = handler.ValidateToken(token, parameter, out var validatedToken);
                if (!IsValidJwtToken(validatedToken)) return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsValidJwtToken(SecurityToken validatedToken)
        {
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                return jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
