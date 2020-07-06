using System;
using System.Threading;
using System.Threading.Tasks;

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
    }
}
