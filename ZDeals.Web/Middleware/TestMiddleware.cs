using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;
using ZDeals.Web.Helpers;
using ZDeals.Web.Models;
using ZDeals.Web.Service;

namespace ZDeals.Web.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var url = request.Path.Value.ToLower();
            var queryString = request.QueryString.ToString();
            if (!string.IsNullOrEmpty(queryString)) url += queryString;

            Console.WriteLine($"URL: {url}");

            await _next(httpContext);
        }
    }
}
