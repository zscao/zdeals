using System.Collections.Generic;

using ZDeals.Web.Api.Options;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Api.Helpers
{
    static class DealPictureHelper
    {
        public static IEnumerable<Deal> SetPictureAbsoluteUrl(this IEnumerable<Deal> deals, PictureStorageOptions options)
        {
            if (deals == null || string.IsNullOrEmpty(options.GetPictureUrl)) return deals;

            foreach (var deal in deals)
            {
                deal.Picture = $"{options.GetPictureUrl}/deals/{deal.Picture}";
            }

            return deals;
        }
    }
}
