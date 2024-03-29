using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZDeals.Identity.ServiceConfigure;

namespace ZDeals.Identity
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowedOrigins";
        private bool _corsEnabled = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _corsEnabled = services.AddCors(Configuration, CorsPolicyName);

            services.AddWebAndValidations();

            services.AddJwtAuthentication(Configuration);

            services.AddZDealsDbContext(Configuration);
            services.AddZDealsServices(Configuration);

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string pathBase = Configuration.GetValue<string>("PathBase") ?? "/";
            app.UsePathBase(pathBase);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{pathBase}swagger/v1/swagger.json", "ZDeals API V1");
                });
            }

            if(_corsEnabled) app.UseCors(CorsPolicyName);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
