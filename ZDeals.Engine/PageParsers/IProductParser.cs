using AngleSharp.Dom;

using ZDeals.Engine.Message.Models;

namespace ZDeals.Engine.PageParsers
{
    interface IProductParser
    {
        Product Parse(IDocument document);
    }
}
