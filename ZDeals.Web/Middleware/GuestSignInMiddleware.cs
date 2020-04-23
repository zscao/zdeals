using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZDeals.Common.Constants;

namespace ZDeals.Web.Middleware
{
    /// <summary>
    /// create a cookie for anonymous users
    /// </summary>
    public class GuestSignInMiddleware
    {
        private readonly RequestDelegate _next;

        public GuestSignInMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var user = httpContext.User;
            if(user == null || user.Identity == null || user.Identity.IsAuthenticated == false)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                    new Claim("NickName", "Anonymous"),
                    new Claim(ClaimTypes.Role, ApiRoles.Anonymous)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var consentFeature = httpContext.Features.Get<ITrackingConsentFeature>();
                var canTrack = consentFeature?.CanTrack ?? false;

                var authProperties = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    IsPersistent = canTrack
                };

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            await _next(httpContext);
        }
    }
}
