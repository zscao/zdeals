using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Helpers;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities.Accounts;

namespace ZDeals.Api.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly ZDealsDbContext _dbContext;

        public UserService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<User>> AuthenticateAsync(LoginRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            var hasher = new PasswordHasher();
            var generator = new PasswordGenerator();

            var hashedPassword = user?.Password ?? hasher.HashPassword(generator.GenerateRandomPassword(8));
            var verifiedResult = hasher.VerifyHashedPassword(hashedPassword, request.Password);

            if(verifiedResult == PasswordVerificationResult.Failed)
            {
                var error = new Error(ErrorType.Authentication) { Code = Common.ErrorCodes.Accounts.UserNotFound, Message = "Invalid username and password combination." };
                return new Result<User>(error);
            }

            return new Result<User>(user.ToUserModel());
        }

        public async Task<Result<User>> CreateUserAsync(CreateUserRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user != null)
            {
                var error = new Error(ErrorType.Validation) { Code = Common.ErrorCodes.Accounts.UserExists, Message = "Username already used." };
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

    }
}
