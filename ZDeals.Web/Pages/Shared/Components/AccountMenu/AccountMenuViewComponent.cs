using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZDeals.Common.Constants;

namespace ZDeals.Web.Pages.Shared.Components.AccountMenu
{
    public class AccountMenuViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.User ;

            string role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            string userName = user.Identity.Name;
            string nickName = user.Claims.FirstOrDefault(x => x.Type == "NickName")?.Value;

            bool isAuthenticate = !string.IsNullOrEmpty(role) && role != ApiRoles.Anonymous && user.Identity.IsAuthenticated;

            var data = new AccountMenuData
            {
                IsAuthenticate = isAuthenticate,
                UserName = userName,
                NickName = nickName,
            };

            var result = await Task.FromResult(data);
            return View(result);
        }
    }


    public class AccountMenuData
    {
        public bool IsAuthenticate { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
    }

}
