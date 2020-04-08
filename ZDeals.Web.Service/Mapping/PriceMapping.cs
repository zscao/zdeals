namespace ZDeals.Web.Service.Mapping
{
    public static class PriceMapping
    {
        public static string ToPriceWithCurrency(this decimal price)
        {
            return price.ToString("$0.00");
        }
    }
}
