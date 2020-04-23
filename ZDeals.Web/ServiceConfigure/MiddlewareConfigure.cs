using Microsoft.AspNetCore.Builder;

using ZDeals.Web.Middleware;

namespace ZDeals.Web.ServiceConfigure
{
    public static class MiddlewareConfigure
    {
        public static IApplicationBuilder UseGuestSignIn(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GuestSignInMiddleware>();
        }
    }
}
