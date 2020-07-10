
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Bot.Startup;
using ZDeals.Engine.Core;
using ZDeals.Engine.Crawlers.CentreCom;

namespace ZDeals.Engine.Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting App ...");

            try
            {
                await CreateHostBuilder(args).RunConsoleAsync();
            }
            catch (OperationCanceledException ex)
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
                services.AddTransient<ICrawler, ClearanceCrawler>();
                services.AddSingleton<CrawlerOption<ICrawler>>(service => 
                    new CrawlerOption<ICrawler> 
                    { 
                        StartUrl = "https://www.centrecom.com.au/clearance", 
                        //StartUrl = "https://www.centrecom.com.au/thermaltake-tt-premium-pure-20-argb-edition-led-fan",
                        Timeout = new TimeSpan(1, 0, 0) 
                    });
                //services.AddTransient<ICrawler, TestCrawler>();
                services.AddHostedService<BotService<ICrawler>>();

                services.SetupMassTransit();
            });
    }
}
