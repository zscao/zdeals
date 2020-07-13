using AngleSharp.Dom;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

using ZDeals.Engine.Message.Models;

namespace ZDeals.Engine.Crawlers.Scorptec
{
    class ProductParser
    {
        public Product Parse(IDocument document, Uri pageUri)
        {
            if (document == null) return null;

            var nodes = document.QuerySelectorAll("script[type='application/ld+json']");
            if (nodes.Count() == 0) return null;

            Product product = new Product();

            foreach(var node in nodes)
            {
                var json = node.Text();
                dynamic token = JToken.Parse(json);

                var type = token["@type"];
                if(type == "BreadcrumbList")
                {
                    // get the category from bread crumb
                    var items = token.itemListElement;
                    if(items != null)
                    {
                        var cates = new List<string>();
                        foreach (dynamic item in items)
                        {
                            if(!string.IsNullOrEmpty(item.name.Value)) 
                                cates.Add(item.name.Value);
                        }

                        product.Category = string.Join(" > ", cates);
                    }
                }
                else if (type == "Product")
                {
                    var offers = token.offers;
                    if (offers?.price == null) break;
                    if (decimal.TryParse(offers.price.Value, out decimal price) == false) break;

                    product.SalePrice = price;
                    product.PriceCurrency = offers.priceCurrency ?? "AUD";

                    product.Title = token.name;
                    product.Description = new string[] { token.description };
                    product.Mpn = token.mpn;
                    product.Sku = token.sku;

                    if (token.brand != null)
                    {
                        product.Brand = token.brand.name;
                        product.Manufacturer = token.brand.name;
                    }
                    product.Images = token.images as string[];

                    product.HighLight = new string[0];
                }
            }

            if (string.IsNullOrEmpty(product.Title)) return null;

            var titleNode = document.QuerySelector("h1#product_name");
            if (titleNode != null) product.Title = titleNode.Text();

            return product;
        }
    }
}
