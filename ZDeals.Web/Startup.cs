using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ZDeals.Storage;
using ZDeals.Storage.FileSystem;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Impl;
using ZDeals.Web.ServiceConfigure;

namespace ZDeals.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.ZDealsDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ZDealsLocal"));
            });

            services.AddScoped<IDealService, DealService>();
            services.AddScoped<ICategoryService, CategoryService>();

            var storageOptions = new FileSystemStorageOptions();
            Configuration.GetSection("FileSystemStorageOptions").Bind(storageOptions);

            services.AddScoped<IBlobService>(options =>
            {
                return new FileSystemBlobService(storageOptions);
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseTest();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
