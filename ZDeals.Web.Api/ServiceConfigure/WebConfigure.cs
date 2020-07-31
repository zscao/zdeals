using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.AspNetCore.Filters;

namespace ZDeals.Web.Api.ServiceConfigure
{
    public static class WebConfigure
    {
        public static void AddWebAndValidations(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ResponseFilter));
            })
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

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
