using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using ZDeals.Engine.Consumers;
using ZDeals.Engine.Message.Commands;

namespace ZDeals.Engine.Startup
{
    static class MassTransitConfigure
    {
        public static IServiceCollection SetupMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PagePasedMonitor>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntuvm");                    

                    cfg.ReceiveEndpoint("monitor_queue", e =>
                    {
                        e.ConfigureConsumer<PagePasedMonitor>(context);
                    });
                }));

            });

            services.AddMassTransitHostedService();

            EndpointConvention.Map<ParsePage>(new System.Uri("queue:page_parse_queue"));

            return services;
        }
    }
}
