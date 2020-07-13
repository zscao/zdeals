using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace ZDeals.Engine.Bot.Startup
{
    static class MassTransitConfigure
    {
        internal static IServiceCollection SetupMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntuvm");
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
