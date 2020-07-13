using System.Collections.Generic;

namespace ZDeals.Engine.Bot.Settings
{
    class CrawlersSetting
    {
        public string Message { get; set; }
        public List<CrawlerOptions> Items { get; set; }
    }

    class CrawlerOptions
    {
        public string Type { get; set; }
        public string StartUrl { get; set; }
        public string Timeout { get; set; }
    }
}
