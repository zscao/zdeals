using System;

namespace ZDeals.Web.Service.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Highlight { get; set; }
        public string Description { get; set; }

        public string FullPrice { get; set; }
        public string DealPrice { get; set; }
        public string Discount { get; set; }

        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public Store Store { get; set; }

        public string Picture { get; set; }

        public string Source { get; set; }

    }
}
