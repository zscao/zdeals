using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ZDeals.Identity.Contract.Models;
using ZDeals.Common.Constants;
using ZDeals.Common.Options;
using ZDeals.Identity.Contract.Responses;
using ZDeals.Common;
using System.Threading.Tasks;
using ZDeals.Identity.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZDeals.Identity.Service.Helpers;
using ZDeals.Identity.Data.Entities;

namespace ZDeals.Identity.Service.Impl
{
    public class JwtService: IJwtService
    {
        private readonly ZIdentityDbContext _dbContext;
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUserService _userService;

        public JwtService(ZIdentityDbContext dbContext, JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters, IUserService userService)
        {
            _dbContext = dbContext;
            _jwtOptions = jwtOptions;
            _tokenValidationParameters = tokenValidationParameters;
            _userService = userService;
        }

        public async Task<Result<JwtResponse>> GenerateJwtTokenAsync(User user)
        {
            var helper = new PasswordGenerator();
            var refreshToken = helper.GenerateRandomPassword(32);

            var jwtId = Guid.NewGuid().ToString();
            var token = GenerateJWT(user, jwtId);

            var entity = new RefreshTokenEntity
            {
                Token = refreshToken,
                JwtId = jwtId,
                UserId = user.UserId,
                CreatedTime = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenLifetime),
                Used = false,
            };

            var entry = _dbContext.RefreshTokens.Add(entity);
            var saved = await _dbContext.SaveChangesAsync();

            var response = new JwtResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                User = user
            };

            return new Result<JwtResponse>(response);
        }


        public async Task<Result<JwtResponse>> RefreshTokenAsync(string token, string refreshToken)
        {
            var error = new Error(ErrorType.Authentication) { Code = Common.ErrorCodes.Identity.InvalidToken, Message = "Invalid Token." };
            var result = new Result<JwtResponse>(error);

            var principal = GetPrincipalFromToken(token);
            if (principal == null)
                return result;

            var expiryTick = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryTick);

            // token has expired
            if (expiryTimeUtc > DateTime.UtcNow)
            {
                var e = new Error(ErrorType.Validation) { Code = Common.ErrorCodes.Common.Invalid, Message = "This token hasnt' expired yet." };
                return new Result<JwtResponse>(e);
            }

            var tokenEntity = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if (tokenEntity == null)
                return result;

            // refresh token is expired or used
            if (DateTime.UtcNow > tokenEntity.ExpiryTime || tokenEntity.Used)
                return result;

            tokenEntity.Used = true;
            await _dbContext.SaveChangesAsync();

            // jwt id does not match
            var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (tokenEntity.JwtId != jti)
                return result;

            var username = principal.FindFirst(ClaimTypes.Name);
            if (username == null || string.IsNullOrEmpty(username.Value))
                return result;

            // couldn't find the user
            var userResult = await _userService.GetUserByName(username.Value);
            if (userResult.HasError())
                return result;

            return await GenerateJwtTokenAsync(userResult.Data);
        }


        public string GenerateJWT(User user, string jwtId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? ApiRoles.Anonymous),
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
