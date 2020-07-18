using System;

namespace ZDeals.Engine.Message.Events
{
    public class PageVisited
    {
        public Uri Uri { get; set; }
        public Uri ParentUri { get; set; }
        public string ContentType { get; set; }
        public DateTime VisitedTime { get; set; }
    }

    public static class VisitedPageContentType {
        public const string Product = "product";
        public const string Index = "index";
        public const string Unknown = "unknown";
    }
}
