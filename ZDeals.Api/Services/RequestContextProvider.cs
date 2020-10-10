using Microsoft.AspNetCore.Http;

using ZDeals.Api.Service;

namespace ZDeals.Api.Services
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
                var user = httpContext.User?.Identity;

                return new RequestContext(username: user?.Name);
            }
        }
    }
}
