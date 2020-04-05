
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Api.Options;

namespace ZDeals.Api.ServiceConfigure
{
    public static class CorsConfigure
    {
        public static void AddCors(this IServiceCollection services, IConfiguration configuration, string policy)
        {
            var corsOptions = new CorsOptions();
            configuration.GetSection("CorsOptions").Bind(corsOptions);
            var origins = corsOptions.AllowedOrigins?.Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries) ?? new[] { "*" };

            services.AddCors(options =>
            {
                options.AddPolicy(policy, builder =>
                {
                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("token-expired");
                });
            });
        }
    }
}
