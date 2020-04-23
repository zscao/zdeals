using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ZDeals.Web.Pages
{
    public class CookieModel : PageModel
    {
        public async Task<IActionResult> OnPostConsentAsync()
        {
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            var consentNeeded = !consentFeature?.CanTrack ?? false;

            if (!consentNeeded) return new NoContentResult();

            var identity = HttpContext.User.Identity;
            if (identity != null)
            {
                var authProperties = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
            }

            var result = new { Cookie = consentFeature.CreateConsentCookie() };
            return new OkObjectResult(result);
        }
    }
}