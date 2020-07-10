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
                x.AddConsumer<ProductRepository>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    cfg.Host("ubuntuvm");

                    cfg.ReceiveEndpoint("product_repository_queue", e =>
                    {
                        e.ConfigureConsumer<ProductRepository>(context);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}
