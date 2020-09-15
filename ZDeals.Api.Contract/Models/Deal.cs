using System;

namespace ZDeals.Api.Contract.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Highlight { get; set; }
        public string Description { get; set; }

        public decimal FullPrice { get; set; }
        public decimal DealPrice { get; set; }
        public string Discount { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public Store Store { get; set; }

        public string Source { get; set; }

        public Brand Brand { get; set; }

        public bool FreeShipping { get; set; }

        public string Status { get; set; }

    }
}
