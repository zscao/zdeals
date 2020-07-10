using Abot2.Core;
using Abot2.Poco;

using System.Collections.Generic;

using ZDeals.Engine.Core.Helpers;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    class ClearanceHyperLinkParser : IHtmlParser
    {
        public IEnumerable<HyperLink> GetLinks(CrawledPage crawledPage)
        {
            var result = new List<HyperLink>();

            var document = crawledPage.AngleSharpHtmlDocument;
            var products = document.QuerySelectorAll("a.prbox_link[href]");

            foreach (var p in products)
            {
                var url = p.GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    result.Add(new HyperLink { HrefValue = UriHelper.MakeUri(crawledPage.ParentUri, url) });
                }
            }

            return result;
        }
    }
}
