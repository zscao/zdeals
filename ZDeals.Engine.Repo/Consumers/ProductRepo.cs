
using MassTransit;

using Microsoft.Extensions.Logging;

using System;
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
                    var product = new ProductEntity
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
                        ParsedTime = message.ParsedTime,
                        Store = message.Uri.Authority,
                        Url = message.Uri.ToString()
                    };

                    var entry = _dbContext.Products.Add(product);

                    var saved = await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Product saved. Result: {saved}. Id: {entry.Entity.Id}, Title: {p.Title}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to save proudct.", ex);
                }

            }
        }
    }
}
