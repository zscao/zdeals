using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZDeals.Identity.ServiceConfigure
{
    public static class ZDealsDbConextConfigure
    {
        public static void AddZDealsDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.ZIdentityDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("ZDealsLocal"));
            });
        }
    }
}
