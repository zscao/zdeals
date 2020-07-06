
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Startup;

namespace ZDeals.Engine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Engine ...");

            await CreateHostBuilder(args).Build().RunAsync();

            Console.WriteLine("Engine has stopped.");
        }
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<TaskMan>();

                    services.SetupMassTransit();
                });
        }
    }

}
