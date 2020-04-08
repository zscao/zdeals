using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities.Sales;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class DealService : IDealService
    {
        private readonly ZDealsDbContext _dbContext;

        public DealService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<DealsSearchResult>> SearchDeals(string categoryCode = null)
        {
            int? categoryId = null;
            if(!string.IsNullOrEmpty(categoryCode))
            {
                var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.Code == categoryCode);
                categoryId = category?.Id;
            }

            IQueryable<DealEntity> query = _dbContext.Deals.Include(x => x.Store);
            if (categoryId.HasValue) query = query.Where(x => x.DealCategory.Any(c => c.CategoryId == categoryId.Value));

            var deals = await query.ToListAsync();

            var result = new DealsSearchResult
            {
                Deals = deals.Select(x => x.ToDealModel()),
                More = deals.Count > 0
            };

            return new Result<DealsSearchResult>(result);
        }
    }
}
