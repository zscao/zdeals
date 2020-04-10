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
            var deal = _dbContext.Deals.SingleOrDefault(x => x.Id == dealId);
            if(deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal not found." };
                return new Result<Deal>(error);
            }

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<DealsSearchResult>> SearchDeals(string categoryCode = null, string keywords = null)
        {
            var categoryIds = new List<int>();

            if (!string.IsNullOrEmpty(categoryCode))
            {
                var cateResult = await _categoryService.GetCategoryTreeAsync(categoryCode);
                if (cateResult.HasError())
                    return new Result<DealsSearchResult>(cateResult.Errors);

                categoryIds = cateResult.Data.ToCategoryList(includeRootNode: true).Select(x => x.Id).ToList();
            }

            IQueryable<DealEntity> query = _dbContext.Deals.Include(x => x.Store);
            if (categoryIds?.Count > 0) query = query.Where(x => x.DealCategory.Any(c => categoryIds.Contains(c.CategoryId)));

            if (!string.IsNullOrWhiteSpace(keywords)) query = query.Where(x => EF.Functions.Like(x.Title, $"%{keywords}%"));

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
