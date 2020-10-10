
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System.Globalization;

using ZDeals.Web.Api.ServiceConfigure;

namespace ZDeals.Web.Api
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowedOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(HostEnvironment.IsDevelopment())
                services.AddCors(Configuration, CorsPolicyName);
            
            services.AddWebAndValidations(HostEnvironment);
            services.AddJwtAuthentication(Configuration);

            services.AddZDealsDbContext(Configuration, HostEnvironment);
            services.AddZDealsServices(Configuration);

            services.AddMemoryCache();

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var culture = new CultureInfo("en-AU");
            CultureInfo.DefaultThreadCurrentCulture = culture;

            string pathBase = Configuration.GetValue<string>("PathBase") ?? "/";
            app.UsePathBase(pathBase);

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{pathBase}swagger/v1/swagger.json", "ZDeals Web Api v1");
                });
            
                app.UseCors(CorsPolicyName);
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy();
            app.UseMiddleware<Middlewares.CookieMiddleware>();

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    httpContext.Request.Cookies.TryGetValue(Constants.CookieKeys.SessionTokenKey, out string sessionToken);
                    httpContext.Request.Cookies.TryGetValue(Constants.CookieKeys.SessionIdKey, out string sessionId);

                    diagnosticContext.Set("SessionToken", sessionToken);
                    diagnosticContext.Set("SessionId", sessionId);
                };

                options.MessageTemplate = "Request {RequestMethod} {RequestPath} responsed {StatusCode} in {Elapsed} ms. SessionToken:{SessionToken} SessionId:{SessionId}";
            });

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
