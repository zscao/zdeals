using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ZDeals.Engine.Schedulers.Impl
{
    public class InMemeoryScheduler : ISiteScheduler
    {
        private readonly ILogger<MySqlScheduler> _logger;

        private ConcurrentQueue<PageToCrawl> _pages;
        private ConcurrentBag<string> _crawledPages;

        public string SiteCode { get; set; } = "default";

        public InMemeoryScheduler(ILogger<MySqlScheduler> logger)
        {
            _logger = logger;

            _pages = new ConcurrentQueue<PageToCrawl>();
            _crawledPages = new ConcurrentBag<string>();

            _logger.LogDebug($"SiteScheduler constructed");
        }

        public int Count => _pages.Count;

        public void Add(PageToCrawl page)
        {
            if (_crawledPages.Contains(page.Uri.AbsolutePath) == false)
            {
                _crawledPages.Add(page.Uri.AbsolutePath);
                _pages.Enqueue(page);
            }
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            foreach (var page in pages) Add(page);
        }


        public PageToCrawl GetNext()
        {
            var success = _pages.TryDequeue(out PageToCrawl page);
            if (success == false) return null;

            _logger.LogDebug($"Got next page {page.Uri} for {this.SiteCode}");

            return page;
        }

        public bool IsUriKnown(Uri uri)
        {
            return _crawledPages.Contains(uri.AbsolutePath);
        }
        public void AddKnownUri(Uri uri)
        {
            if (_crawledPages.Contains(uri.AbsolutePath) == false) _crawledPages.Add(uri.AbsolutePath);
        }

        public void Clear()
        {
            _pages.Clear();
        }

        public void Dispose()
        {
            _pages = null;
            _crawledPages = null;
        }
    }
}
