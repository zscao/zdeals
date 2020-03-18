using System;

namespace ZDeals.Api.Contract.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HighLight { get; set; }
        public string Descrition { get; set; }

        public decimal FullPrice { get; set; }
        public decimal DealPrice { get; set; }
        public string Discount { get; set; }

        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public Store Store { get; set; }

        public string Source { get; set; }

    }
}
