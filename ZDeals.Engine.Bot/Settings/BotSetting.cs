using System.Collections.Generic;

namespace ZDeals.Engine.Bot.Settings
{
    class BotSetting
    {
        public string Message { get; set; }
        public List<CrawlerSetting> Crawlers { get; set; }
    }

    class CrawlerSetting
    {
        public string Type { get; set; }
        public string Store { get; set; }
        public string StartUrl { get; set; }
        public string Timeout { get; set; }

        public int MaxPagesToCrawl { get; set; }

        public int MinCrawlDelaySeconds { get; set; }
    }
}
