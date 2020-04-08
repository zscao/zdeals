using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

using ZDeals.Api.Setup;

namespace ZDeals.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host = await host.SeedDataAsync();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
