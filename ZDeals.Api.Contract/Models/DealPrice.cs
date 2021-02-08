using System;

namespace ZDeals.Api.Contract.Models
{
    public class DealPrice
    {
        public decimal Price { get; set; }
        public DateTime UpdatedTime { get; set; }
        /// <summary>
        /// indicate who collected the price
        /// </summary>
        public string PriceSource { get; set; }
    }

    public class DealPriceList : DataList<DealPrice>
    {
    }
}
