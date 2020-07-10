using System;

namespace ZDeals.Engine.Core.Helpers
{
    public static class UriHelper
    {
        public static Uri MakeUri(Uri parentUri, string href)
        {
            if (href.StartsWith("http://") || href.StartsWith("https://"))
                return new Uri(href);
            else if (href.StartsWith("//"))
                return new Uri($"{parentUri.Scheme}:{href}");
            else if (href.StartsWith("/"))
                return new Uri(parentUri.GetLeftPart(UriPartial.Authority) + href);
            else
                return new Uri(parentUri, href);
        }
    }
}
