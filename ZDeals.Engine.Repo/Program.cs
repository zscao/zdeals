using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Repo.Startup;

namespace ZDeals.Engine.Repo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Engine.Repo ...");

            await CreateHostBuilder(args).RunConsoleAsync();

            Console.WriteLine("Engine.Repo has stopped.");
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddEngineDbContext(hostContext.Configuration);
                    services.SetupMassTransit();
                });
        }
    }
}
