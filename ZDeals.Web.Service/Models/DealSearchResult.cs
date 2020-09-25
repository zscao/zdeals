using System.Collections.Generic;

namespace ZDeals.Web.Service.Models
{
    public class DealSearchResult
    {
        public IEnumerable<Deal>? Deals { get; set; }

        public string? Category { get; set; }
        public string? Keywords { get; set; }

        public int Page { get; set; }

        public string? Sort { get; set; }

        public bool More { get; set; }

        public IEnumerable<DealFilter>? Filters { get; set; }

    }
}
