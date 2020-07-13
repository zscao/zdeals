using System;
using System.Globalization;

namespace ZDeals.Engine.Core.Helpers
{
    public static class PriceHelper
    {
        public static (bool Success, decimal Value) ParsePrice(string priceString, decimal defaultValue = 0)
        {
            if (string.IsNullOrEmpty(priceString)) return (false, defaultValue);

            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-AU");
            if (decimal.TryParse(priceString, style, provider, out decimal price) == false) return (false, defaultValue);

            return (true, price);
        }

        public static bool IsNumeric(this object x) 
        { 
            return x == null ? false : IsNumeric(x.GetType()); 
        }

        // Method where you know the type of the object
        public static bool IsNumeric(Type type) 
        { 
            return IsNumeric(type, Type.GetTypeCode(type)); 
        }

        // Method where you know the type and the type code of the object
        public static bool IsNumeric(Type type, TypeCode typeCode) 
        { 
            return (typeCode == TypeCode.Decimal || (type.IsPrimitive && typeCode != TypeCode.Object && typeCode != TypeCode.Boolean && typeCode != TypeCode.Char)); 
        }
    }
}
