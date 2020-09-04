using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class ZDealsDbConextConfigure
    {
        public static void AddZDealsDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<Data.ZDealsDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("ZDealsLocal"));
                if(env.IsDevelopment()) options.EnableSensitiveDataLogging();
            });
        }
    }
}
