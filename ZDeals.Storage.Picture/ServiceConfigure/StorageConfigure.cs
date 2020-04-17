using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZDeals.Storage.FileSystem;

namespace ZDeals.Storage.Picture.ServiceConfigure
{
    public static class StorageConfigure
    {
        public static void AddStorageServices(this IServiceCollection services, IConfiguration configuration)
        {

            var storageOptions = new FileSystemStorageOptions();
            configuration.GetSection("FileSystemStorageOptions").Bind(storageOptions);

            services.AddScoped<IBlobService>(options =>
            {
                return new FileSystemBlobService(storageOptions);
            });
        }
    }
}
