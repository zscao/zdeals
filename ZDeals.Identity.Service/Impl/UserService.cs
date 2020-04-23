using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Requests;
using ZDeals.Identity.Data;
using ZDeals.Identity.Data.Entities;
using ZDeals.Identity.Service.Helpers;
using ZDeals.Identity.Service.Mapping;

namespace ZDeals.Identity.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly ZIdentityDbContext _dbContext;

        public UserService(ZIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<Result<User>> AuthenticateAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            var hasher = new PasswordHasher();
            var generator = new PasswordGenerator();

            var hashedPassword = user?.Password ?? hasher.HashPassword(generator.GenerateRandomPassword(8));
            var result = hasher.VerifyHashedPassword(hashedPassword, password);

            if(result == PasswordVerificationResult.Failed)
            {
                var error = new Error(ErrorType.Authentication) { Code = Common.ErrorCodes.Identity.UserNotFound, Message = "Invalid username and password combination." };
                return new Result<User>(error);
            }

            return new Result<User>(user.ToUserModel());           
        }



    }
}
