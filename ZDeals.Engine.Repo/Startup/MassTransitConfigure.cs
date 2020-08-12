using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using ZDeals.Engine.Repo.Consumers;

namespace ZDeals.Engine.Repo.Startup
{
    static class MassTransitConfigure
    {
        public static IServiceCollection SetupMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductRepo>(c => c.UseConcurrentMessageLimit(1));
                x.AddConsumer<VisitedPageRepo>(c => c.UseConcurrentMessageLimit(1));

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntu2004");

                    cfg.ReceiveEndpoint("product_repository_queue", e =>
                    {
                        e.ConfigureConsumer<ProductRepo>(context);
                    });

                    cfg.ReceiveEndpoint("visited_page_repository_queue", e =>
                    {
                        e.ConfigureConsumer<VisitedPageRepo>(context);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
