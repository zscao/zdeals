
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

using ZDeals.Engine.Bot.Settings;
using ZDeals.Engine.Core;

namespace ZDeals.Engine.Bot.Startup
{
    static class CrawlersConfigure
    {

        internal static IServiceCollection AddCrawlers(this IServiceCollection services, IConfiguration configuration)
        {
            var setting = new CrawlersSetting();
            configuration.GetSection("Crawlers").Bind(setting);

            foreach (var crawler in setting.Items)
            {
                var crawlerType = Type.GetType(crawler.Type);
                if (crawlerType == null || crawlerType.IsClass == false)
                {
                    Console.WriteLine($"Can't resolve crawler {crawler.Type}");
                    continue;
                }
                else if(crawlerType.IsSubclassOf(typeof(ICrawler)) == false)
                {
                    Console.WriteLine($"{crawlerType.FullName} is not a crawler.");
                    continue;
                }
                services.AddGenericTransient(crawlerType);

                var genericOptionType = typeof(CrawlerOption<>);
                var optionType = genericOptionType.MakeGenericType(crawlerType);
                var option = Activator.CreateInstance(optionType, new object[] { crawler.StartUrl, TimeSpan.Parse(crawler.Timeout) });
                services.AddGenericSingleton(option);

                var genericServiceType = typeof(BotService<>);
                var serviceType = genericServiceType.MakeGenericType(crawlerType);
                services.AddGenericHostedService(serviceType);
            }

            return services;
        }

        static void AddGenericTransient(this IServiceCollection services, Type t)
        {
            if (t.IsClass == false) return;

            var addTransientMethod = typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "AddTransient" &&
                    m.IsGenericMethod == true &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1);

            if (addTransientMethod == null) return;

            var method = addTransientMethod.MakeGenericMethod(new[] { t });
            method.Invoke(services, new[] { services });
        }

        static void AddGenericSingleton(this IServiceCollection services, object instance)
        {
            var addSingletonMethod = typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "AddSingleton" &&
                    m.IsGenericMethod == true &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 2 &&
                    m.GetParameters().Last().ParameterType.Name == "TService");

            if (addSingletonMethod == null) return;

            var method = addSingletonMethod.MakeGenericMethod(new[] { instance.GetType() });
            method.Invoke(services, new[] { services, instance });
        }

        static void AddGenericHostedService(this IServiceCollection services, Type t)
        {
            var addHostedServiceMethod = typeof(ServiceCollectionHostedServiceExtensions)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "AddHostedService" &&
                    m.IsGenericMethod == true &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1
                );

            if (addHostedServiceMethod == null) return;

            var method = addHostedServiceMethod.MakeGenericMethod(new[] { t });
            method.Invoke(services, new[] { services });
        }
    }
}
