using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

using ZDeals.Web.Api.Constants;

namespace ZDeals.Web.Api.Middlewares
{
    /// <summary>
    /// used to set session cookies
    /// </summary>
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {            
            if (context.Request.Cookies.Keys.Contains(CookieKeys.SessionTokenKey) == false)
            {
                context.Response.OnStarting(() =>
                {
                    AddSessionToken(context.Response.Cookies);
                    AddSessionId(context.Request.Cookies, context.Response.Cookies);
                    UpdateCookieConsent(context.Request.Cookies, context.Response.Cookies);

                    return Task.CompletedTask;
                });
            }
            await _next.Invoke(context);
        }


        // issue an session token
        private void AddSessionToken(IResponseCookies responseCookies)
        {
            var token = GenerateRandomId();
            var options = new CookieOptions
            {
                Path = "/",
                IsEssential = true,
            };

            responseCookies.Append(CookieKeys.SessionTokenKey, token, options);
        }

        // issue an track id
        private void AddSessionId(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        {
            if (requestCookies.TryGetValue(CookieKeys.SessionIdKey, out string token) == false) 
                token = GenerateRandomId();

            var options = new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(180),
                IsEssential = true,
            };

            responseCookies.Append(CookieKeys.SessionIdKey, token, options);
        }

        // update the cookie consent
        private void UpdateCookieConsent(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        {
            if (requestCookies.TryGetValue(CookieKeys.CookieConsent, out string token) == false) return;

            var options = new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(180),
                IsEssential = true,
            };

            responseCookies.Append(CookieKeys.CookieConsent, token, options);
        }

        private string GenerateRandomId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
