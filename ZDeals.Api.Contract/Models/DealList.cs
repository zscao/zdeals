using System;

namespace ZDeals.Api.Contract.Models
{
    public class DealList
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public Store Store { get; set; }
    }
}
