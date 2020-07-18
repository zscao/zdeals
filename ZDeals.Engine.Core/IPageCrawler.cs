using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Core
{
    public interface IPageCrawler
    {
        Task StartCrawling(string url, string store, CancellationTokenSource cts);

        event EventHandler<PageCrawledEventArgs> OnPageCrawled;
    }


    public class PageCrawledEventArgs
    {
        public PageCrawled Page { get; set; }
    }
}
