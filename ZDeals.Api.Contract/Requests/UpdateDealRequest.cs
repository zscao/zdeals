using System;

namespace ZDeals.Api.Contract.Requests
{
    public class UpdateDealRequest
    {
        public string Title { get; set; }
        public string HighLight { get; set; }
        public string Description { get; set; }

        public decimal FullPrice { get; set; }
        public decimal DealPrice { get; set; }
        public string Discount { get; set; }

        /// <summary>
        /// brand code
        /// </summary>
        public string Brand { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
