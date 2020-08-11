using System;
using ZDeals.Web.Service.Mapping;

namespace ZDeals.Web.Service.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Highlight { get; set; }
        public string? Description { get; set; }

        public decimal FullPrice { get; set; }
        public decimal DealPrice { get; set; }
        public string? Discount { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? VerifiedTime { get; set; }

        public string? VerifiedBy { get; set; }

        public Store? Store { get; set; }

        public string? Picture { get; set; }

        public string? Source { get; set; }

        public string FullPriceString
        {
            get
            {
                return DealPrice < FullPrice ? FullPrice.ToPriceWithCurrency() : string.Empty;
            }
        }

        public string DealPriceString
        {
            get
            {
                return DealPrice.ToPriceWithCurrency();
            }
        }

        public string CreatedTimeString
        {
            get
            {
                var span = DateTime.UtcNow - CreatedTime;
                if (span.TotalHours < 24)
                    return span.TotalHours.ToString("0 hours ago");
                else
                    return span.TotalDays.ToString("0 days ago");
            }
        }

    }
}
