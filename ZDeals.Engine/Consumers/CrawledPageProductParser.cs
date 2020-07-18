using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

using MassTransit;

using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Message.Events;
using ZDeals.Engine.PageParsers;

namespace ZDeals.Engine.Consumers
{
    class CrawledPageProductParser : IConsumer<PageCrawled>
    {
        private readonly ILogger<CrawledPageProductParser> _logger;

        public CrawledPageProductParser(ILogger<CrawledPageProductParser> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PageCrawled> context)
        {
            var page = context.Message;

            _logger.LogInformation($"Parsing crawled page {page.Uri}");


            IProductParser parser = null;
            if (page?.Uri.Authority == "www.centrecom.com.au")
            {
                parser = new CentreComProductParser();
            }
            else if(page?.Uri.Authority == "www.scorptec.com.au")
            {
                parser = new ScorptecProductParser();
            }

            if (parser != null)
            {
                var visited = new PageVisited
                {
                    Uri = page.Uri,
                    ParentUri = page.ParentUri,
                    ContentType = page.IsRoot ? VisitedPageContentType.Index : VisitedPageContentType.Unknown,
                    VisitedTime = page.CrawledTime
                };

                var document = ParseToAngleSharpHtml(page.Content?.Text);

                var product = parser.Parse(document);
                if (product != null)
                {
                    visited.ContentType = VisitedPageContentType.Product;

                    await context.Publish(new ProductParsed
                    {
                        Uri = page.Uri,
                        ParsedTime = page.CrawledTime,
                        Product = product
                    });
                }

                await context.Publish(visited);
            }
        }

        private IHtmlDocument ParseToAngleSharpHtml(string text)
        {
            var parser = new HtmlParser();

            IHtmlDocument document;
            try
            {
                document = parser.ParseDocument(text);
            }
            catch (Exception e)
            {
                document = parser.ParseDocument("");
            }

            return document;
        }
    }
}
