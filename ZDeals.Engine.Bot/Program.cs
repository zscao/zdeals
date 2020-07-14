using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Bot.Startup;

namespace ZDeals.Engine.Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting App ...");

            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("App is cancelled by the user.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in Main method. {0}", ex);
            }

            Console.WriteLine("App stopped.");
        }


        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddEngineDbContext(hostContext.Configuration);

                services.AddCrawlers(hostContext.Configuration);

                services.SetupMassTransit();
            });
    }
}
