using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZDeals.Web.Api.Options;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Impl;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<ICategoryService, CategoryService>();


            var pictureStorageOptions = new PictureStorageOptions();
            configuration.GetSection("PictureStorageOptions").Bind(pictureStorageOptions);
            services.AddSingleton(pictureStorageOptions);
        }
    }
}
