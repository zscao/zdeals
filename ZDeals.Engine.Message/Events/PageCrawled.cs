
using System;
using System.Text;

namespace ZDeals.Engine.Message.Events
{
    public class PageCrawled
    {
        public Uri Uri { get; set; }
        public Uri ParentUri { get; set; }

        public DateTime CrawledTime { get; set; }

        public PageContent Content { get; set; }

        public bool IsRoot { get; set; }
    }

    public class PageContent
    {
        public string Charset { get; set; }
        public string Text { get; set; }
    }
}
