using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZDeals.Api.Constants;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Options;

namespace ZDeals.Api.Helpers
{
    public static class AuthenticationHelper
    {
        public static  string GenerateJWT(User user, JwtOptions options)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? AppRoles.Guest)
                }),
                Issuer = options.Issuer,
                Audience = options.Audience,
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
