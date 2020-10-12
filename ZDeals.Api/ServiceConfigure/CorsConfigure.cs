
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ZDeals.Common.AspNetCore.Options;

namespace ZDeals.Api.ServiceConfigure
{
    public static class CorsConfigure
    {
        public static bool AddCors(this IServiceCollection services, IConfiguration configuration, string policy)
        {
            var corsOptions = new CorsOptions();
            configuration.GetSection("CorsOptions").Bind(corsOptions);

            var allowed = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? corsOptions.AllowedOrigins;
            if (string.IsNullOrEmpty(allowed)) return false;

            var origins = allowed.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (origins.Length == 0) return false;

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

            return true;
        }
    }
}
