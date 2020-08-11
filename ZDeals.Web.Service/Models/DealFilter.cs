using System.Collections.Generic;

namespace ZDeals.Web.Service.Models
{
    public class DealFilter
    {
        public string? Code { get; set; }
        public string? Title { get; set; }
        public FilterType FilterType { get; set; }

        public IEnumerable<FilterItem>? Items { get; set; }
        
    }

    public class FilterItem
    {
        public string? Value { get; set; }
        public string? Name { get; set; }

        public bool Selected { get; set; }
    }

    public enum FilterType
    {
        MultipleSelection = 0,
        SingleSelect = 1, 
    }
}
