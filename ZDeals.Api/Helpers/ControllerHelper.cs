using Microsoft.AspNetCore.Mvc;

using ZDeals.Api.Service;

namespace ZDeals.Api.Helpers
{
    public static class ControllerHelper
    {
        public static RequestContext GetRequestContext(this ControllerBase controller)
        {
            var user = controller.HttpContext.User?.Identity;
            if (user == null) return null;

            return new RequestContext
            {
                Username = user.Name
            };
        }
    }
}
