using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Engine.Data;

namespace ZDeals.Engine.Bot.Startup
{
    public static class EngineDbConextConfigure
    {
        public static void AddEngineDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EngineDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("ZDealsEngineLocal"));
            }, ServiceLifetime.Transient);
        }
    }
}
