using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Parser.Startup;

namespace ZDeals.Engine.Parser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Parser ...");

            await CreateHostBuilder(args).Build().RunAsync();

            Console.WriteLine("Parser is stopped.");
        }


        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.SetupMassTransit();
                });
        }
    }
}
