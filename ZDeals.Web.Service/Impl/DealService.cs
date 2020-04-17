using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        private readonly ICategoryService _categoryService;

        public DealService(ZDealsDbContext dbContext, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
        }

        public async Task<Result<Deal>> GetDealById(int dealId)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId && x.Deleted == false);
            if(deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal not found." };
                return new Result<Deal>(error);
            }

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<DealsSearchResult>> SearchDeals(string categoryCode = null, string keywords = null, int pageSize = 20, int pageNumber = 1)
        {
            var categoryIds = new List<int>();

            if (!string.IsNullOrEmpty(categoryCode) || categoryCode != Common.Constants.DefaultValues.DealsCategoryRoot)
            {
                var cateResult = await _categoryService.GetCategoryTreeAsync(categoryCode);
                if (cateResult.HasError())
                    return new Result<DealsSearchResult>(cateResult.Errors);

                categoryIds = cateResult.Data.ToCategoryList(includeRootNode: true).Select(x => x.Id).ToList();
            }

            IQueryable<DealEntity> query = _dbContext.Deals.Include(x => x.Store).Where(x => !x.Deleted);
            if (categoryIds?.Count > 0) query = query.Where(x => x.DealCategory.Any(c => categoryIds.Contains(c.CategoryId)));
            if (!string.IsNullOrWhiteSpace(keywords)) query = query.Where(x => EF.Functions.Like(x.Title, $"%{keywords}%"));

            var skipped = pageSize * (pageNumber - 1);
            var deals = await query.OrderByDescending(x => x.Id).Skip(skipped).Take(pageSize).ToListAsync();

            var result = new DealsSearchResult
            {
                Deals = deals.Select(x => x.ToDealModel()).ToList(),
                More = deals.Count >= pageSize
            };

            return new Result<DealsSearchResult>(result);
        }
    }
}
