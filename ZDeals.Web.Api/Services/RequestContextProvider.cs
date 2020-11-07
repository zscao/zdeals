using Microsoft.AspNetCore.Http;

using ZDeals.Web.Service;

namespace ZDeals.Web.Api.Services
{
    public class RequestContextProvider : IRequestContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public RequestContext Context 
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;

                httpContext.Request.Cookies.TryGetValue(Constants.CookieKeys.SessionTokenKey, out string sessionToken);
                httpContext.Request.Cookies.TryGetValue(Constants.CookieKeys.SessionIdKey, out string sessionId);

                var clientIp = httpContext.Connection.RemoteIpAddress;

                return new RequestContext 
                { 
                    SessionToken = sessionToken, 
                    SessionId = sessionId ,
                    ClientIP = clientIp.ToString()
                };
            }
        }
    }
}
