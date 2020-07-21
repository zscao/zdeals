
using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Engine.Data;
using ZDeals.Engine.Data.Entities;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Repo.Consumers
{
    class ProductRepo : IConsumer<ProductParsed>
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<ProductRepo> _logger;

        public ProductRepo(EngineDbContext dbContext, ILogger<ProductRepo> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<ProductParsed> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Received PageParsed: {message.Uri}");

            if(message.Product != null)
            {
                var p = message.Product;

                try
                {
                    var product = _dbContext.Products.Include(p => p.PriceHistory).FirstOrDefault(x => x.Url == message.Uri.AbsoluteUri);
                    if (product == null)
                    {
                        product = new ProductEntity
                        {
                            Title = p.Title,
                            HighLight = string.Join('\n', p.HighLight),
                            Description = string.Join('\n', p.Description),
                            PriceCurrency = p.PriceCurrency,
                            FullPrice = p.FullPrice,
                            SalePrice = p.SalePrice,
                            Manufacturer = p.Manufacturer,
                            Brand = p.Brand,
                            Sku = p.Sku,
                            Mpn = p.Mpn,

                            // meta data
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = message.ParsedTime,
                            Store = message.Uri.Authority,
                            Url = message.Uri.ToString()
                        };

                        var entry = _dbContext.Products.Add(product);
                    }
                    else
                    {

                        var history = product.PriceHistory.OrderBy(x => x.Sequence).LastOrDefault();
                        if (history == null || Math.Abs(product.SalePrice - history.Price) > 0.01m)
                        {
                            product.PriceHistory.Add(new PriceHistoryEntity
                            {
                                ProductId = product.Id,
                                Price = product.SalePrice,
                                Sequence = (history?.Sequence ?? 0) + 1,
                                UpdatedDate = product.UpdatedTime
                            });
                        }

                        product.Title = p.Title;
                        product.HighLight = string.Join('\n', p.HighLight);
                        product.Description = string.Join('\n', p.Description);
                        product.PriceCurrency = p.PriceCurrency;
                        product.FullPrice = p.FullPrice;
                        product.SalePrice = p.SalePrice;
                        product.Manufacturer = p.Manufacturer;
                        product.Brand = p.Brand;
                        product.Sku = p.Sku;
                        product.Mpn = p.Mpn;

                        product.UpdatedTime = message.ParsedTime;
                    }
                    var saved = await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Product saved. Result: {saved}. Id: {product.Id}, Title: {p.Title}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to save proudct.", ex);
                }

            }
        }
    }
}
