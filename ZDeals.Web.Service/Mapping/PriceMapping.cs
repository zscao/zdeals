using System.Globalization;

namespace ZDeals.Web.Service.Mapping
{
    public static class PriceMapping
    {
        public static string ToPriceWithCurrency(this decimal price)
        {
            var format = price.Equals(decimal.Truncate(price)) ? "C0" : "C";
            return price.ToString(format, CultureInfo.CurrentCulture);
        }
    }
}
