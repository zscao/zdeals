using System;
using System.Collections.Generic;

namespace ZDeals.Api.Contract.Models
{
    public class DealVisitStatis
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String StatisMode { get; set; }


        public IEnumerable<VisitDetail> Details { get; set; }
    }

    public class VisitDetail
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public static class StatisMode
    {
        public const string Hourly = "Hourly";
        public const string Daily = "Daily";
        public const string Monthly = "Monthly";
    }
}
