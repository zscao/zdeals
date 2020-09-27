using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Identity.Contract.Models;

namespace ZDeals.Identity
{
    public interface IAccountService
    {
        Task<Result<User>> AuthenticateAsync(string username, string password);

        Task<Result> ChangePassword(string username, string currentPassword, string newPassword);

    }
}
