using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Identity.Data;
using ZDeals.Identity;

namespace ZDeals.Api.Setup
{
    public static class DataSeeding
    {
        public static async Task<IHost> SeedDataAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<ZIdentityDbContext>())
            {
                try
                {
                    if(dbContext.Users.Any() == false)
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        await userService.CreateUserAsync(new Identity.Contract.Requests.CreateUserRequest
                        {
                            Username = "test",
                            Nickname = "Admin TEST",
                            Password = "Test123!",
                            Role = "Admin"
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
            return host;
        }
    }
}
