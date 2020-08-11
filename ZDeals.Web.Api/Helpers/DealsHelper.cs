using System.Collections.Generic;
using System.Linq;

using ZDeals.Web.Api.Options;
using ZDeals.Web.Service;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Api.Helpers
{
    static class DealsHelper
    {
        public static IEnumerable<Deal> SetPictureAbsoluteUrl(IEnumerable<Deal> deals, PictureStorageOptions options)
        {
            if (string.IsNullOrEmpty(options.GetPictureUrl)) return deals;

            var result = new List<Deal>(deals);
            foreach (var deal in result)
            {
                deal.Picture = $"{options.GetPictureUrl}/deals/{deal.Picture}";
            }

            return result;
        }

        public static IEnumerable<Deal> SetSourceToLocal(IEnumerable<Deal> deals, string baseUrl)
        {
            var result = new List<Deal>(deals);
            foreach(var deal in result)
            {
                deal.Source = $"{baseUrl}/deal/{deal.Id}";
            }
            return result;
        }

        public static string? GetMoreLink(DealsSearchResult result, DealsSearchRequest request)
        {
            if (!result.More) return null;

            var link = "/api/deals?";

            return link
                .AddQueryString("category", request.Category)
                .AddQueryString("keywords", request.Keywords)
                .AddQueryString("store", request.Store)
                .AddQueryString("sort", request.Sort)
                .AddQueryString("page", $"{result.Page + 1}");
        }

        static string AddQueryString(this string url, string key, string? value)
        {
            if (string.IsNullOrEmpty(value)) return url;

            if (url == null) url = "";

            var prefix = url.IndexOf('?') >=0 ? "&" : "?";
            return $"{url}{prefix}{key}={value}";
        }
    }
}
