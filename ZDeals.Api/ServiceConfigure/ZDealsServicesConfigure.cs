using Microsoft.Extensions.DependencyInjection;

using ZDeals.Api.Service;
using ZDeals.Api.Service.Impl;
using ZDeals.Api.Services;

namespace ZDeals.Api.ServiceConfigure
{
    public static class ZDealsServicesConfigure
    {
        public static void AddZDealsServices(this IServiceCollection services)
        {
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IRequestContextProvider, RequestContextProvider>();
        }
    }
}
