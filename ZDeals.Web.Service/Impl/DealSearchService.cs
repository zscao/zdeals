using Microsoft.EntityFrameworkCore;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities;
using ZDeals.Web.Service.Helpers;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;
using ZDeals.Web.Service.Options;

namespace ZDeals.Web.Service.Impl
{
    public class DealSearchService : IDealSearchService
    {
        const int PageSize = 20;
        const string FreeShipping = "free";

        private readonly ZDealsDbContext _dbContext;
        private readonly PictureStorageOptions _pictureStorageOptions;
        private readonly ICategoryService _categoryService;
        private readonly IDealSearchCache _dealSearchCache;

        public DealSearchService(ZDealsDbContext dbContext, PictureStorageOptions pictureStorageOptions, ICategoryService categoryService, IDealSearchCache dealSearchCache)
        {
            _dbContext = dbContext;
            _pictureStorageOptions = pictureStorageOptions;
            _categoryService = categoryService;
            _dealSearchCache = dealSearchCache;
        }

        public async Task<Result<DealSearchResult?>> SearchDeals(DealSearchRequest request)
        {
            var result = _dealSearchCache.GetSearchResult(request);
            if(result != null) return new Result<DealSearchResult?>(result);

            var categoryIds = await GetCategoryIds(request.Category);

            var query = GetQueryableSql(request, categoryIds);
            var storeFilter = await GetStoreFilter(query);
            var brandFilter = await GetBrandFilter(query);
            var deliveryFilter = GetDeliveryFilter();

            // apply stores filter
            // get store filters 
            string[] selectedStores = request.Store?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            if (selectedStores.Count() > 0)
            {
                storeFilter.ApplySelected(selectedStores);

                IEnumerable<int> storeIds = selectedStores.Select(x => int.TryParse(x, out int s) ? s : 0);
                query = query.Where(x => x.StoreId.HasValue && storeIds.Contains(x.StoreId.Value));
            }

            // apply brands filter
            // get brand filter
            var brandCodes = request.Brand?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (brandCodes?.Length > 0)
            {
                brandFilter.ApplySelected(brandCodes);

                var brandIds = GetBrandIds(brandCodes);
                query = query.Where(x => x.BrandId != null && brandIds.Contains(x.BrandId.Value));
            }
            // apply delivery filter
            if(request.Del == FreeShipping)
            {
                deliveryFilter.ApplySelected(new string[] { FreeShipping });
                query = query.Where(x => x.FreeShipping);
            }

            // apply sort
            if (request.Sort == "price_desc")
                query = query.OrderByDescending(x => x.DealPrice);
            else if (request.Sort == "price_asc")
                query = query.OrderBy(x => x.DealPrice);
            else if (string.IsNullOrEmpty(request.Keywords))
            {
                query = query.OrderByDescending(x => x.Id);
            }

            var page = request.Page ?? 1;
            var skipped = PageSize * (page - 1);
            var deals = await query.Skip(skipped).Take(PageSize).Include(x => x.Store).ToListAsync();
            var dealsModel = deals.Select(x => x.ToDealModel(_pictureStorageOptions?.GetPictureUrl)!).ToList();

            result = new DealSearchResult
            {
                Deals = dealsModel,
                Category = request.Category,
                Keywords = request.Keywords,
                Page = page,
                Sort = request.Sort ?? "default",
                More = deals.Count >= PageSize,
                Filters = new List<DealFilter> {
                    brandFilter,
                    deliveryFilter,
                    storeFilter,
                }
            };

            _dealSearchCache.SetSearchResult(request, result);

            return new Result<DealSearchResult?>(result);
        }

        private IQueryable<DealEntity> GetQueryableSql(DealSearchRequest request, IEnumerable<int> categoryIds)
        {
            var parameters = new List<object>();
            string sql = $" SELECT * FROM Deals d " +
                         $" WHERE (d.Status = 1) " +
                         $" AND (d.ExpiryDate IS NULL OR d.ExpiryDate > UTC_TIMESTAMP()) ";

            if (categoryIds.Count() > 0)
            {
                var categories = string.Join(",", categoryIds);
                sql += $" AND (EXISTS (SELECT 1 FROM DealCategory dc WHERE d.Id = dc.DealId AND dc.CategoryId IN ({categories}) )) ";
            }

            if (!string.IsNullOrEmpty(request.Keywords))
            {
                var keywordList = request.Keywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if(keywordList.Length > 0)
                {
                    sql += $" AND MATCH(d.Title) AGAINST (@keywords IN BOOLEAN MODE) ";

                    var keywords = string.Join(" ", keywordList.Select(x => $"{x}*"));
                    parameters.Add(new MySqlParameter("keywords", keywords));
                }
            }

            IQueryable<DealEntity> query = _dbContext.Deals.FromSqlRaw(sql, parameters.ToArray());
            return query;
        }

        /// <summary>
        /// Original method, deprecated
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /*
        private async Task<Result<DealsSearchResult?>> SearchDealsEFCore(DealsSearchRequest request)
        {
            var categoryIds = await GetCategoryIds(request.Category);

            IQueryable<DealEntity> query = 
                _dbContext.Deals
                    .Include(x => x.Store)
                    .Where(x => !x.Deleted && x.VerifiedTime <DateTime.UtcNow && (x.ExpiryDate == null || x.ExpiryDate > System.DateTime.UtcNow));

            if (categoryIds?.Count > 0) query = query.Where(x => x.DealCategory.Any(c => categoryIds.Contains(c.CategoryId)));
            if (!string.IsNullOrWhiteSpace(request.Keywords)) query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Keywords}%"));

            // get store filters 
            var stores = request.Store?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var storeFilter = GetStoreFilter(query.Select(x => x.Store).Distinct().ToList(), stores);

            // get brand filter
            var brandCodes = request.Brand?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var brandIds = GetBrandIds(brandCodes);
            var brandFilter = GetBrandFilter(query.Select(x => x.BrandId).Distinct().ToList(), brandIds);

            // apply filters
            if (stores?.Length > 0)
            {                    
                query = query.Where(x => stores.Contains(x.Store.Name));
            }

            if(brandIds.Length > 0)
            {
                query = query.Where(x => x.BrandId != null && brandIds.Contains(x.BrandId.Value));
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
        */

        private async Task<List<int>> GetCategoryIds(string? category)
        {
            var result = new List<int>();

            if (!string.IsNullOrEmpty(category) && category != Common.Constants.DefaultValues.DealsCategoryRoot)
            {
                var cateResult = await _categoryService.GetCategoryTreeAsync(category);
                if (cateResult.HasError())
                    return result;

                result = cateResult.Data.ToCategoryList(includeRootNode: true).Select(x => x.Id).ToList();
            }

            return result;
        }

        private async Task<DealFilter> GetStoreFilter(IQueryable<DealEntity> query)
        {
            var stores = await query.Select(x => x.Store).Distinct().ToListAsync();

            var items = stores
                .Where(x => x != null)
                .OrderBy(x => x.Name)
                .Select(x => new FilterItem
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                    Selected = false
                })
                .ToList();

            return new DealFilter
            {
                Code = "store",
                Title = "Store",
                FilterType = FilterType.MultipleSelection,
                Items = items
            };
        }

        private async Task<DealFilter> GetBrandFilter(IQueryable<DealEntity> query)
        {
            var brands = await query.Select(x => x.Brand).Distinct().ToListAsync();

            var items = brands
                .Where(x => x != null)
                .OrderByDescending(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new FilterItem
                {
                    Name = x.Name,
                    Value = x.Code,
                    Selected = false
                })
                .ToList();

            return new DealFilter
            {
                Code = "brand",
                Title = "Brand",
                FilterType = FilterType.MultipleSelection,
                Items = items
            };
        }

        private DealFilter GetDeliveryFilter()
        {
            var freeShipping = new FilterItem
            {
                Name = "Free Shipping",
                Value = FreeShipping,
                Selected = false
            };

            return new DealFilter
            {
                Code = "del",
                Title = "Delivery",
                FilterType = FilterType.MultipleSelection,
                Items = new FilterItem[] { freeShipping }
            };
        }

        private int[] GetBrandIds(IEnumerable<string>? brandCodes)
        {
            if (brandCodes == null || brandCodes.Count() == 0) return new int[0];

            var codes = brandCodes.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return _dbContext.Brands.Where(x => codes.Contains(x.Code)).Select(x => x.Id).ToArray();
        }
    }
}
