using System;

namespace ZDeals.Api.Contract.Models
{
    public class TrackedProduct
    {
        public string Url { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class PagedTrackedProducts: PagedData<TrackedProduct>
    {
    }
}
