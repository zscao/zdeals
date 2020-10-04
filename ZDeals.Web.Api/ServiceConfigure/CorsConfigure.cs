
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ZDeals.Common.AspNetCore.Options;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class CorsConfigure
    {
        public static void AddCors(this IServiceCollection services, IConfiguration configuration, string policy)
        {
            var corsOptions = new CorsOptions();
            configuration.GetSection("CorsOptions").Bind(corsOptions);

            var allowed = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? corsOptions.AllowedOrigins;
            var origins = allowed?.Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries) ?? new[] { "*" };

            services.AddCors(options =>
            {
                options.AddPolicy(policy, builder =>
                {
                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("token-expired");
                });
            });
        }
    }
}
