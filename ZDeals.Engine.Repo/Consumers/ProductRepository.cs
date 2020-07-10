
using MassTransit;

using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using ZDeals.Engine.Data;
using ZDeals.Engine.Data.Entities;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Repo.Consumers
{
    class ProductRepository : IConsumer<PageParsed>
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(EngineDbContext dbContext, ILogger<ProductRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<PageParsed> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Receive PageParsed: {message.Uri}");

            if(message.Product != null)
            {
                var p = message.Product;                

                var entity = new ProductEntity
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

                try
                {
                    var entry = _dbContext.Products.Add(entity);
                    var saved = await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Product saved. Result: {saved}. Id: {entry.Entity.Id}, Title: {p.Title}");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Failed to save proudct.", ex);
                }
            }
        }
    }
}
