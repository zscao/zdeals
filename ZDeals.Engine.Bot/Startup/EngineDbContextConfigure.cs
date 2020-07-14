using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZDeals.Engine.Bot.Startup
{
    public static class EngineDbConextConfigure
    {
        public static void AddEngineDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.EngineDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("ZDealsEngineLocal"));
            }, ServiceLifetime.Transient);
        }
    }
}
