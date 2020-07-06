using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using ZDeals.Engine.Parser.Consumers;

namespace ZDeals.Engine.Parser.Startup
{
    static class MassTransitConfigure
    {
        public static IServiceCollection SetupMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TestPageParser>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntuvm");

                    cfg.ReceiveEndpoint("page_parse_queue", e =>
                    {
                        e.ConfigureConsumer<TestPageParser>(context);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
