using Abot2.Core;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ZDeals.Engine.Crawlers.Scorptec
{
    class ProductFeedHyperLinkParser : IHtmlParser
    {
        public IEnumerable<HyperLink> GetLinks(CrawledPage crawledPage)
        {
            ConcurrentBag<HyperLink> result = new ConcurrentBag<HyperLink>();
            if (crawledPage.Uri.LocalPath.EndsWith("product_feed.xml") == false) return result;

            var xml = crawledPage.Content.Text;
            var document = new XmlDocument();
            document.LoadXml(xml.Trim());

            var regex = new Regex(@"/product/Monitors/[\w\d-]+");

            var nodes = document.DocumentElement.ChildNodes;
            Parallel.For(0, nodes.Count, i =>
            {
                var node = nodes[i];
                if (node.ChildNodes.Count == 0) return;

                var url = node.ChildNodes[0].InnerText;
                if (regex.IsMatch(url) == false) return;

                result.Add(new HyperLink { HrefValue = new Uri(url) });
            });

            return result;

        }
    }
}
