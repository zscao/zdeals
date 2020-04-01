using Microsoft.Extensions.DependencyInjection;

using ZDeals.Api.Service;
using ZDeals.Api.Service.Impl;
using ZDeals.Storage;
using ZDeals.Storage.FileSystem;

namespace ZDeals.Api.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services)
        {
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IBlobService>(options =>
            {
                return new FileSystemBlobService(new FileSystemStorageConfig
                {
                    Directory = "D:\\Temp\\zdeals\\images"
                });
            });
        }
    }
}
