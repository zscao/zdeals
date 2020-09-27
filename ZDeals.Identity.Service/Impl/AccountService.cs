using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Data;
using ZDeals.Identity.Service.Helpers;
using ZDeals.Identity.Service.Mapping;

namespace ZDeals.Identity.Service.Impl
{
    public class AccountService : IAccountService
    {
        private readonly ZIdentityDbContext _dbContext;

        public AccountService(ZIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<Result> ChangePassword(string username, string currentPassword, string newPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Common.ErrorCodes.Identity.UserNotFound, Message = "User does not exist." };
                return new Result(error);
            }

            var hasher = new PasswordHasher();
            var verifyResult = hasher.VerifyHashedPassword(user.Password, currentPassword);
            if(verifyResult == PasswordVerificationResult.Failed)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Common.ErrorCodes.Identity.InvalidPassword, Message = "Invalid password." };
                return new Result(error);
            }

            var hashedPassword = hasher.HashPassword(newPassword);

            user.Password = hashedPassword;
            var saved = await _dbContext.SaveChangesAsync();

            return new Result();
        }

    }
}
