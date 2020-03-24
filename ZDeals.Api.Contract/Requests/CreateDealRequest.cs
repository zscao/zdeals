using System;

namespace ZDeals.Api.Contract.Requests
{
    public class CreateDealRequest
    {
        public string Title { get; set; }
        public string HighLight { get; set; }
        public string Description { get; set; }

        public decimal FullPrice { get; set; }
        public decimal DealPrice { get; set; }
        public string Discount { get; set; }

        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string Source { get; set; }
    }
}
