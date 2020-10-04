using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

namespace ZDeals.Web.Api.Middlewares
{
    /// <summary>
    /// used to set session cookies
    /// </summary>
    public class CookieMiddleware
    {
        const string SessionTokenKey = "session-token";
        const string SessionIdKey = "ft-session-id";
        const string CookieConsent = "cookie-consent";

        private readonly RequestDelegate _next;

        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {            
            if (context.Request.Cookies.Keys.Contains(SessionTokenKey) == false)
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

            responseCookies.Append(SessionTokenKey, token, options);
        }

        // issue an track id
        private void AddSessionId(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        {
            if (requestCookies.TryGetValue(SessionIdKey, out string token) == false) 
                token = GenerateRandomId();

            var options = new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(180),
                IsEssential = true,
            };

            responseCookies.Append(SessionIdKey, token, options);
        }

        // update the cookie consent
        private void UpdateCookieConsent(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        {
            if (requestCookies.TryGetValue(CookieConsent, out string token) == false) return;

            var options = new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(180),
                IsEssential = true,
            };

            responseCookies.Append(CookieConsent, token, options);
        }

        private string GenerateRandomId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
