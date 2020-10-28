namespace ZDeals.Web.Service.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Highlight { get; set; }
        public string? Description { get; set; }

        public string? UsedPriceString { get; set; }
        public string? DealPriceString { get; set; }
        public string? Discount { get; set; }

        public bool FreeShipping { get; set; }

        public string? CreatedTimeString { get; set; }

        public Store? Store { get; set; }

        public string? Picture { get; set; }

        public string? Source { get; set; }

        public string? Brand { get; set; }

    }
}
