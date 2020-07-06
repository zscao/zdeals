using System;

namespace ZDeals.Engine.Message.Events
{
    public class PageParsed
    {
        public string Url { get; set; }
        public DateTimeOffset ParseTime { get; set; }
    }
}
