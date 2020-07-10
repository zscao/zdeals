namespace ZDeals.Engine.Message.Models
{
    public class Product
    {
        public string Title { get; set; }
        public string[] HighLight { get; set; }
        public string[] Description { get; set; }

        public decimal FullPrice { get; set; }
        public decimal SalePrice { get; set; }

        public string PriceCurrency { get; set; }
        /// <summary>
        /// the URL of images
        /// </summary>
        public string[] images { get; set; }


        public string Manufacturer { get; set; }
        public string Brand { get; set; }

        public string Category { get; set; }

        public string Sku { get; set; }
        public string Mpn { get; set; }
    }
}
