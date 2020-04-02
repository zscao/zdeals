using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Api.Service;
using ZDeals.Api.Service.Impl;
using ZDeals.Storage;
using ZDeals.Storage.FileSystem;

namespace ZDeals.Api.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();

            var storageOptions = new FileSystemStorageOptions();
            configuration.GetSection("FileSystemStorageOptions").Bind(storageOptions);

            services.AddScoped<IBlobService>(options =>
            {
                return new FileSystemBlobService(storageOptions);
            });
        }
    }
}
