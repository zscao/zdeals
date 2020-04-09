using Microsoft.AspNetCore.Builder;

using ZDeals.Web.Middleware;

namespace ZDeals.Web.ServiceConfigure
{
    public static class MiddlewareConfigure
    {
        public static IApplicationBuilder UseTest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
