using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using ZDeals.Identity;
using ZDeals.Identity.Data;
using ZDeals.Identity.Service.Impl;
using ZDeals.Net;
using ZDeals.Net.Impl;
using ZDeals.Web.Options;
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
            services.AddDbContext<ZIdentityDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ZDealsIdentity"));
            });

            services.AddScoped<IUserService, UserService>();


            services.AddDbContext<Data.ZDealsDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("ZDealsLocal"));
            });

            services.AddScoped<IDealService, DealService>();
            services.AddScoped<IDealSearchService, DealSearchService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPageService, PageService>();

            var pictureStorageOptions = new PictureStorageOptions();
            var url = Environment.GetEnvironmentVariable("PICTURE_STORAGE_URL");
            if (!string.IsNullOrEmpty(url))
            {
                pictureStorageOptions.GetPictureUrl = url;
            }
            else
            {
                Configuration.GetSection("PictureStorageOptions").Bind(pictureStorageOptions);
            }
            services.AddSingleton(pictureStorageOptions);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddAntiforgery(options => options.HeaderName = "CSRF-TOKEN");

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var culture = new CultureInfo("en-AU");
            CultureInfo.DefaultThreadCurrentCulture = culture;

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

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGuestSignIn();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
