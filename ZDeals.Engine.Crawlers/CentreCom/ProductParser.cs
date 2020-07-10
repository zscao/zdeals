using AngleSharp.Dom;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using ZDeals.Engine.Core.Helpers;
using ZDeals.Engine.Message.Models;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    class ProductParser
    {
        public Product Parse(IDocument document, Uri pageUri)
        {
            if (document == null) return null;

            var metaData = document.QuerySelectorAll("div[itemscope] > meta");
            if (!metaData.Any()) return null;

            // get title
            var title = metaData.GetContentByItemProp("name");
            if (string.IsNullOrWhiteSpace(title)) return null;

            // get price
            var offers = document.QuerySelectorAll("div[itemscope] > span[itemprop='offers'] > meta");
            var priceCurrency = offers.GetContentByItemProp("priceCurrency");

            var priceString = offers.GetContentByItemProp("price");
            if (string.IsNullOrWhiteSpace(priceString)) return null;

            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-AU");
            if (decimal.TryParse(priceString, style, provider, out decimal price) == false) return null;

            var fullPriceString = document.QuerySelector(".prod_price_rrp > span")?.InnerHtml;

            decimal fullPrice = 0;
            if (!string.IsNullOrWhiteSpace(fullPriceString))
                decimal.TryParse(fullPriceString, style, provider, out fullPrice);


            // get description and highlight
            var desc = metaData.GetContentByItemProp("description") ?? "";
            var regex_newline = new Regex("\r\n|\r|\n|\\?");
            var regex_space = new Regex("[ ]{2,}");
            var description = desc.Split("||", StringSplitOptions.RemoveEmptyEntries).Select(x => regex_space.Replace(regex_newline.Replace(x.Trim(), " "), "")).ToArray();
            
            var highlights = document.QuerySelectorAll(".prod_sales_points > ul > li").Select(el => el.InnerHtml).ToArray();


            // get images
            var images = new List<string>();
            var image = metaData.GetContentByItemProp("image");
            if (!string.IsNullOrEmpty(image))
            {
                images.Add(image);
            }
            else
            {
                var slideImageNodes = document.QuerySelectorAll("#gallery-1 > a.rsImg[href]");
                foreach (var node in slideImageNodes)
                {
                    var src = node.GetAttribute("href");
                    if (!string.IsNullOrEmpty(src))
                        images.Add(UriHelper.MakeUri(pageUri, src).ToString());
                }
            }

            // get category
            var category = metaData.GetContentByItemProp("category");

            return new Product
            {
                Title = title,
                PriceCurrency = priceCurrency,
                SalePrice = price,
                FullPrice = fullPrice,
                Description = description.ToArray(),
                HighLight = highlights,
                images = images.ToArray(),
                Category = category,
                Manufacturer = metaData.GetContentByItemProp("manufacturer"),
                Brand = metaData.GetContentByItemProp("brand"),
                Sku = metaData.GetContentByItemProp("sku"),
                Mpn = metaData.GetContentByItemProp("mpn")
            };
        }
    }

    public static class HtmlCollectionExtensions
    {
        //public static string GetContentByKey(this IHtmlCollection<IElement> elements, string keyAttributeName, string keyValue, string contentAttributeName)
        //{
        //    return elements?.FirstOrDefault(x => x.GetAttribute(keyAttributeName) == keyValue)?.GetAttribute(contentAttributeName);
        //}

        public static string GetContentByItemProp(this IHtmlCollection<IElement> elements, string keyValue)
        {
            return elements?.FirstOrDefault(x => x.GetAttribute("itemprop") == keyValue)?.GetAttribute("content");
        }
    }
}
