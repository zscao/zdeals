using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Linq;

using ZDeals.Common.AspNetCore.Filters;
using ZDeals.Common.AspNetCore.Responses;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class WebConfigure
    {
        public static void AddWebAndValidations(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ResponseFilter));
            })
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = env.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.HttpOnly = HttpOnlyPolicy.None;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(x => x.Errors
                            .Select(p => new ErrorDetail
                            {
                                Code = Common.ErrorCodes.Common.Invalid,
                                Message = p.ErrorMessage
                            }))
                        .ToList();

                    var result = new ErrorResponse
                    {
                        Status = 400,
                        Message = "Validation errors.",
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
        }
    }
}
