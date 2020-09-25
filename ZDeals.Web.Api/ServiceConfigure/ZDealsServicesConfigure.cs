using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Net;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Impl;
using ZDeals.Web.Service.Options;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IDealSearchService, DealSearchService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IPageService, Net.Impl.PageService>();

            // cache service            
            services.AddSingleton<IDealSearchCache, DealSearchCache>();

            var pictureStorageOptions = new PictureStorageOptions();
            configuration.GetSection("PictureStorageOptions").Bind(pictureStorageOptions);
            services.AddSingleton(pictureStorageOptions);
        }
    }
}
