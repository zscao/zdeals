using System;

using ZDeals.Engine.Message.Models;

namespace ZDeals.Engine.Message.Events
{
    public class ProductParsed
    {
        public Uri Uri { get; set; }
        public DateTime ParsedTime { get; set; }

        public Product Product { get; set; }
    }
}
