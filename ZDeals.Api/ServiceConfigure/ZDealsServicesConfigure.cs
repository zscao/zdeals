using Microsoft.Extensions.DependencyInjection;

using ZDeals.Api.Service;
using ZDeals.Api.Service.Impl;

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
        }
    }
}
