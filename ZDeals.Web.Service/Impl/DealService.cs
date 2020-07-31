using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class DealService : IDealService
    {
        const int PageSize = 20;

        private readonly ZDealsDbContext _dbContext;
        private readonly ICategoryService _categoryService;

        public DealService(ZDealsDbContext dbContext, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
        }

        public async Task<Result<Deal>> MarkDealExpired(int dealId)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal does not exist." };
                return new Result<Deal>(error);
            }

            deal.ExpiryDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return new Result<Deal>(deal.ToDealModel());
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

        public async Task<Result<DealsSearchResult>> SearchDeals(DealsSearchRequest request)
        {
            var categoryIds = new List<int>();

            if (!string.IsNullOrEmpty(request?.Category) && request.Category != Common.Constants.DefaultValues.DealsCategoryRoot)
            {
                var cateResult = await _categoryService.GetCategoryTreeAsync(request.Category);
                if (cateResult.HasError())
                    return new Result<DealsSearchResult>(cateResult.Errors);

                categoryIds = cateResult.Data.ToCategoryList(includeRootNode: true).Select(x => x.Id).ToList();
            }

            IQueryable<DealEntity> query = 
                _dbContext.Deals
                    .Include(x => x.Store)
                    .Where(x => !x.Deleted && x.VerifiedTime < System.DateTime.UtcNow && (x.ExpiryDate == null || x.ExpiryDate > System.DateTime.UtcNow));

            if (categoryIds?.Count > 0) query = query.Where(x => x.DealCategory.Any(c => categoryIds.Contains(c.CategoryId)));
            if (!string.IsNullOrWhiteSpace(request?.Keywords)) query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Keywords}%"));

            // get filters 
            var storeFilter = GetStoreFilter(query.Select(x => x.Store.Name).Distinct().ToList());


            // apply filters
            if (!string.IsNullOrEmpty(request?.Store))
            {
                var stores = request.Store.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if(stores.Length > 0)
                {
                    query = query.Where(x => stores.Contains(x.Store.Name));
                }
            }

            var pageNumber = request?.PageNumber ?? 1;
            var skipped = PageSize * (pageNumber - 1);
            var deals = await query.OrderByDescending(x => x.Id).Skip(skipped).Take(PageSize).ToListAsync();            

            var result = new DealsSearchResult
            {
                Deals = deals.Select(x => x.ToDealModel()).ToList(),
                Category = request?.Category,
                Keywords = request.Keywords,
                More = deals.Count >= PageSize,
                Filters = new List<DealFilter>() { storeFilter }
            };

            return new Result<DealsSearchResult>(result);
        }

        private DealFilter GetStoreFilter(IEnumerable<string> stores )
        {
            return new DealFilter
            {
                Code = "store",
                Title = "Store",
                FilterType = FilterType.MultipleSelection,
                Items = stores.Select(x => new FilterItem { Name = x, Value = x })
            };
        }
    }
}
