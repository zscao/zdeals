using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Identity.Service.Impl;

namespace ZDeals.Identity.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
