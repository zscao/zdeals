using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Message.Models;

namespace ZDeals.Engine.Core
{
    public interface ICrawler
    {
        Task StartCrawling(string url, CancellationTokenSource cts);

        event EventHandler<PageParsedEventArgs> PageParsed;
    }


    public class PageParsedEventArgs
    {
        public Uri PageUri { get; set; }

        public Product Product { get; set; }
    }
}
