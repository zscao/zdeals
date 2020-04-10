using System.Collections.Generic;

namespace ZDeals.Web.Service.Models
{
    public class DealsSearchResult
    {
        public IEnumerable<Deal> Deals { get; set; }

        public bool More { get; set; }
    }
}
