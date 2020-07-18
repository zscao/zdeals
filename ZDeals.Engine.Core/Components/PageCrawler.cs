using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZDeals.Engine.Core.Components
{
    public class PageCrawler : IPageCrawler
    {
        public event EventHandler<PageCrawledEventArgs> OnPageCrawled;

        public Task StartCrawling(string url, string store, CancellationTokenSource cts)
        {
            throw new NotImplementedException();
        }
    }
}
