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
                x.AddConsumer<CrawledPageProductParser>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntuvm");                    

                    cfg.ReceiveEndpoint("parse_page_queue", e =>
                    {
                        e.ConfigureConsumer<CrawledPageProductParser>(context);
                    });
                }));

            });

            services.AddMassTransitHostedService();

            EndpointConvention.Map<CrawlPage>(new System.Uri("queue:crawl_page_queue"));

            return services;
        }
    }
}
