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
    public class DealSearchService : IDealSearchService
    {
        const int PageSize = 20;

        private readonly ZDealsDbContext _dbContext;
        private readonly ICategoryService _categoryService;

        public DealSearchService(ZDealsDbContext dbContext, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
        }

        public async Task<Result<DealsSearchResult?>> SearchDeals(DealsSearchRequest request)
        {
            var categoryIds = new List<int>();

            if (!string.IsNullOrEmpty(request.Category) && request.Category != Common.Constants.DefaultValues.DealsCategoryRoot)
            {
                var cateResult = await _categoryService.GetCategoryTreeAsync(request.Category);
                if (cateResult.HasError())
                    return new Result<DealsSearchResult?>(cateResult.Errors);

                categoryIds = cateResult.Data.ToCategoryList(includeRootNode: true).Select(x => x.Id).ToList();
            }

            IQueryable<DealEntity> query = 
                _dbContext.Deals
                    .Include(x => x.Store)
                    .Where(x => !x.Deleted && x.VerifiedTime <DateTime.UtcNow && (x.ExpiryDate == null || x.ExpiryDate > System.DateTime.UtcNow));

            if (categoryIds?.Count > 0) query = query.Where(x => x.DealCategory.Any(c => categoryIds.Contains(c.CategoryId)));
            if (!string.IsNullOrWhiteSpace(request.Keywords)) query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Keywords}%"));

            // get store filters 
            var stores = request.Store?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var storeFilter = GetStoreFilter(query.Select(x => x.Store.Name).Distinct().ToList(), stores);

            // get brand filter
            var brands = request.Brand?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var brandFilter = GetBrandFilter(query.Select(x => x.Brand).Distinct().ToList(), brands);

            // apply filters
            if (stores?.Length > 0)
            {                    
                query = query.Where(x => stores.Contains(x.Store.Name));
            }

            if(brands?.Length > 0)
            {
                query = query.Where(x => brands.Contains(x.Brand));
            }

            // apply sort
            if (request?.Sort == "price_desc")
                query = query.OrderByDescending(x => x.DealPrice);
            else if (request?.Sort == "price_asc")
                query = query.OrderBy(x => x.DealPrice);
            else
                query = query.OrderByDescending(x => x.Id);

            var page = request?.Page ?? 1;
            var skipped = PageSize * (page - 1);
            var deals = await query.Skip(skipped).Take(PageSize).ToListAsync();

            var result = new DealsSearchResult
            {
                Deals = deals.Select(x => x.ToDealModel()!).ToList(),
                Category = request?.Category,
                Keywords = request?.Keywords,
                Page = page,
                Sort = request?.Sort ?? "default",
                More = deals.Count >= PageSize,
                Filters = new List<DealFilter>() { storeFilter, brandFilter }
            };

            return new Result<DealsSearchResult?>(result);
        }

        private DealFilter GetStoreFilter(IEnumerable<string> stores, string[]? selected)
        {
            return new DealFilter
            {
                Code = "store",
                Title = "Store",
                FilterType = FilterType.MultipleSelection,
                Items = stores.Select(x => new FilterItem 
                { 
                    Name = x, 
                    Value = x,
                    Selected = selected?.Contains(x) ?? false
                })
            };
        }

        private DealFilter GetBrandFilter(IEnumerable<string> brands, string[]? selected)
        {
            return new DealFilter
            {
                Code = "brand",
                Title = "Brand",
                FilterType = FilterType.MultipleSelection,
                Items = brands.Where(x => !string.IsNullOrEmpty(x)).Select(x => new FilterItem
                {
                    Name = x,
                    Value = x,
                    Selected = selected?.Contains(x) ?? false
                })
            };
        }
    }
}
