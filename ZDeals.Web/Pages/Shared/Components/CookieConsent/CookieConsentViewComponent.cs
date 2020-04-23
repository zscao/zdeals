using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ZDeals.Web.Pages.Shared.Components.CookieConsent
{
    public class CookieConsentViewComponent: ViewComponent
    {
        private readonly IAntiforgery _antiforgery;


        public CookieConsentViewComponent(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await Task.FromResult(new CookieConsentData());

            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            data.ShowBanner = !consentFeature?.CanTrack ?? false;

            data.RequestValidationToken = _antiforgery.GetTokens(HttpContext).RequestToken;

            return View(data);
        }
    }

    public class CookieConsentData
    {
        public bool ShowBanner { get; set; }
        public string RequestValidationToken { get; set; }
    }
}
