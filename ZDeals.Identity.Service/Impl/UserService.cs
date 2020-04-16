using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Requests;
using ZDeals.Identity.Service.Helpers;
using ZDeals.Identity.Service.Mapping;
using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities.Identity;
using ZDeals.Identity.Contract.Responses;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System;
using ZDeals.Common.Options;

namespace ZDeals.Identity.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly ZDealsDbContext _dbContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly JwtOptions _jwtOptions;

        public UserService(ZDealsDbContext dbContext, IAuthenticationService authenticationService, JwtOptions jwtOptions)
        {
            _dbContext = dbContext;
            _authenticationService = authenticationService;
            _jwtOptions = jwtOptions;
        }

        public async Task<Result<User>> GetUserByName(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);
            if(user == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Identity.UserNotFound, Message = "User not found." };
                return new Result<User>(error);
            }

            return new Result<User>(user.ToUserModel());
        }

        public async Task<Result<User>> CreateUserAsync(CreateUserRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user != null)
            {
                var error = new Error(ErrorType.Validation) { Code = Common.ErrorCodes.Identity.UserExists, Message = "Username already used." };
                return new Result<User>(error);
            }

            var hasher = new PasswordHasher();
            var hashedPassword = hasher.HashPassword(request.Password);

            user = new UserEntity
            {
                Username = request.Username,
                Password = hashedPassword,
                Nickname = request.Nickname,
                Role = request.Role ?? "User"
            };

            var entry = _dbContext.Users.Add(user);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<User>(entry.Entity.ToUserModel());
        }

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            var hasher = new PasswordHasher();
            var generator = new PasswordGenerator();

            var hashedPassword = user?.Password ?? hasher.HashPassword(generator.GenerateRandomPassword(8));
            var result = hasher.VerifyHashedPassword(hashedPassword, password);

            if(result == PasswordVerificationResult.Failed)
            {
                var error = new Error(ErrorType.Authentication) { Code = Common.ErrorCodes.Identity.UserNotFound, Message = "Invalid username and password combination." };
                return new Result<AuthenticationResponse>(error);
            }

            var response = await GenerateAuthenticaionResponse(user.ToUserModel());
            return new Result<AuthenticationResponse>(response);           
        }



        public async Task<Result<AuthenticationResponse>> RefreshTokenAsync(string token, string refreshToken)
        {
            var error = new Error(ErrorType.Authentication) { Code = Common.ErrorCodes.Identity.InvalidToken, Message = "Invalid Token." };
            var result = new Result<AuthenticationResponse>(error);

            var principal = _authenticationService.GetPrincipalFromToken(token);
            if (principal == null)
                return result;

            var expiryTick = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryTick);

            // token has expired
            if(expiryTimeUtc > DateTime.UtcNow)
            {
                var e = new Error(ErrorType.Validation) { Code = Common.ErrorCodes.Common.Invalid, Message = "This token hasnt' expired yet." };
                return new Result<AuthenticationResponse>(e); 
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
            var userResult = await GetUserByName(username.Value);
            if (userResult.HasError())
                return result;

            var response = await GenerateAuthenticaionResponse(userResult.Data);
            return new Result<AuthenticationResponse>(response);
        }


        private async Task<AuthenticationResponse> GenerateAuthenticaionResponse(User user)
        {
            var helper = new PasswordGenerator();
            var refreshToken = helper.GenerateRandomPassword(32);

            var jwtId = Guid.NewGuid().ToString();
            var token = _authenticationService.GenerateJWT(user, jwtId);

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

            return new AuthenticationResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                User = user
            };
        }
    }
}
