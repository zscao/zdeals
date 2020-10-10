using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.Constants;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Impl
{
    public class DealService : IDealService
    {
        private readonly ZDealsDbContext _dbContext;
        private readonly ICategoryService _categoryService;
        private readonly RequestContext _requestContext;

        public DealService(ZDealsDbContext dbContext, ICategoryService categoryService, IRequestContextProvider requestContextProvider)
        {
            _dbContext = dbContext;
            _categoryService = categoryService;
            
            _requestContext = requestContextProvider.Context;
        }

        public async Task<Result<PagedDeals>> SearchDealsAsync(SearchDealRequest request)
        {
            int size = request.PageSize ?? 10;
            int number = request.PageNumber ?? 1;

            var query = _dbContext.Deals.AsQueryable();

            // set deal status
            if (request.Status == DealStatusValue.Deleted)
                query = query.Where(x => x.Status == DealStatus.Deleted);
            else if (request.Status == DealStatusValue.Verified)
                query = query.Where(x => x.Status == DealStatus.Verified);
            else if (request.Status == DealStatusValue.Created)
                query = query.Where(x => x.Status == DealStatus.Created);
            else
                query = query.Where(x => x.Status != DealStatus.Deleted);

            if (!string.IsNullOrEmpty(request.Category) && request.Category != DefaultValues.DealsCategoryRoot)
            {
                var children = await FindCategoryChildIds(request.Category);
                if (children.Count() > 0) query = query.Where(x => x.DealCategory.Any(c => children.Contains(c.CategoryId)));
            }

            if (!string.IsNullOrEmpty(request.Keywords))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Keywords.Trim()}%"));

            if (!string.IsNullOrEmpty(request.Store))
            {
                if (int.TryParse(request.Store, out int storeId))
                    query = query.Where(x => x.StoreId == storeId);
            }

            var total = await query.CountAsync();

            var deals = await query.AsNoTracking()
                .Include(x => x.Store)
                .Include(x => x.Brand)
                .OrderByDescending(x => x.Id)
                .Skip((number - 1) * size)
                .Take(size)
                .ToListAsync();

            var paged = new PagedDeals
            {
                Data = deals.Select(x => x.ToDealModel()),
                PageSize = size,
                PageNumber = number,
                TotalCount = total
            };

            return new Result<PagedDeals>(paged);
        }


        public async Task<Result<Deal>> GetDealByIdAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "The deal does not exist." });
            }

            return new Result<Deal>(deal.ToDealModel());
        }
        public async Task<Result<DealExistence>> CheckExistenceBySourceAsync(string source)
        {
            var result = new Result<DealExistence>(new DealExistence { Existing = false });

            var deal = await _dbContext.Deals.Include(x => x.Store).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Source == source);
            if (deal != null)
            {
                result.Data.Existing = true;
                result.Data.Deal = deal.ToDealModel();
            }

            return result;
        }


        public async Task<Result<Deal>> CreateDealAsync(CreateDealRequest request)
        {
            var exist = await _dbContext.Deals.AnyAsync(x => x.Source == request.Source);
            if (exist)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.DealSourceDuplicate, Message = "Deal source duplicate." };
                return new Result<Deal>(error);
            }

            var store = await FindStoreForUrl(request.Source);

            if(store == null)
            {
                var error = new Error(ErrorType.BadRequest) { Code = Sales.StoreNotFound, Message = "Couldn't find store." };
                return new Result<Deal>(error);
            }

            var deal = new DealEntity
            {
                Title = request.Title,
                Source = request.Source,
                Store = store,
                CreatedTime = DateTime.UtcNow,
            };

            if (!string.IsNullOrEmpty(request.Category))
            {
                var category = _dbContext.Categories.SingleOrDefault(x => x.Code == request.Category);
                if (category != null)
                {
                    deal.DealCategory = new List<DealCategoryJoin>
                    {
                        new DealCategoryJoin { Deal = deal, Category = category }
                    };
                }
            }

            var entry =_dbContext.Deals.Add(deal);

            _dbContext.DealActionHistory.Add(new ActionHistoryEntity
            {
                Deal = deal,
                Action = DealActionValues.Create,
                ActedBy = _requestContext.Username,
                ActedOn = DateTime.UtcNow,
            });

            var saved = await _dbContext.SaveChangesAsync();

            //if (!string.IsNullOrEmpty(request.Category))
            //{
            //    var category = _dbContext.Categories.SingleOrDefault(x => x.Code == request.Category);
            //    if (category != null)
            //    {
            //        deal = entry.Entity;
            //        deal.DealCategory = new List<DealCategoryJoin>();
            //        deal.DealCategory.Add(new DealCategoryJoin { Deal = deal, Category = category });

            //        saved = await _dbContext.SaveChangesAsync();
            //    }
            //}

            return new Result<Deal>(entry.Entity.ToDealModel());
        }

        public async Task<Result<Store>> GetDealStoreAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal?.Store == null)
            {
                return new Result<Store>(new Error(ErrorType.NotFound) { Code = Sales.StoreNotFound, Message = "Store not found" });
            }

            return new Result<Store>(deal.Store.ToStoreModel());
        }

        public async Task<Result<Deal>> UpdateDealAsync(int dealId, UpdateDealRequest request)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new Error(ErrorType.Validation) { Code = Sales.DealNotFound, Message = "Deal does not exist" });
            }

            var brand = await _dbContext.Brands.FirstOrDefaultAsync(x => x.Code == request.Brand);

            deal.Title = request.Title;
            deal.Highlight = request.HighLight;
            deal.Description = request.Description;
            deal.Discount = request.Discount;
            deal.FullPrice = request.FullPrice;
            deal.DealPrice = request.DealPrice;
            deal.ExpiryDate = request.ExpiryDate;
            deal.Brand = brand;
            deal.FreeShipping = request.FreeShipping;

            // if the deal is change, then needs to be verify again
            deal.Status = DealStatus.Created;

            _dbContext.DealActionHistory.Add(new ActionHistoryEntity
            {
                DealId = dealId,
                Action = DealActionValues.Update,
                ActedBy = _requestContext.Username,
                ActedOn = DateTime.UtcNow,
            });

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<Deal>(deal.ToDealModel());
        }


        public async Task<Result<Deal>> DeleteDealAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
                return new Result<Deal>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "The deal does not exist." });

            deal.Status = DealStatus.Deleted;
            _dbContext.DealActionHistory.Add(new ActionHistoryEntity
            {
                DealId = dealId,
                Action = DealActionValues.Delete,
                ActedBy = _requestContext.Username,
                ActedOn = DateTime.UtcNow,
            });

            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<Deal>> VerifyDealAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
                return new Result<Deal>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "The deal does not exist." });

            deal.Status = DealStatus.Verified;

            _dbContext.DealActionHistory.Add(new ActionHistoryEntity
            {
                DealId = dealId,
                Action = DealActionValues.Verify,
                ActedBy = _requestContext.Username,
                ActedOn = DateTime.UtcNow,
            });

            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<Deal>> RecycleDealAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).Include(x => x.Store).SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
                return new Result<Deal>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "The deal does not exist." });

            deal.Status = DealStatus.Created;

            _dbContext.DealActionHistory.Add(new ActionHistoryEntity
            {
                DealId = dealId,
                Action = DealActionValues.Recycle,
                ActedBy = _requestContext.Username,
                ActedOn = DateTime.UtcNow,
            });

            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<DealPictureList>> GetPicturesAsync(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Pictures).SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
                return new Result<DealPictureList>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "Could not find the deal" });

            var list = new DealPictureList
            {
                Data = deal.Pictures.Select(x => x.ToDealPictureModel(isDefault: x.FileName == deal.DefaultPicture))
            };

            if(list.Data?.Count() > 1)
            {
                list.Data = list.Data.OrderBy(x => x.IsDefaultPicture ? 0 : 1);
            }

            return new Result<DealPictureList>(list);
        }

        public async Task<Result<DealPicture>> SavePictureAsync(int dealId, SaveDealPictureRequest request)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
                return new Result<DealPicture>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "Could not find the deal" });

            var picture = await _dbContext.DealPictures.SingleOrDefaultAsync(x => x.DealId == dealId && x.FileName == request.FileName);
            if(picture != null)
            {
                picture.Title = request.Title;
                picture.Alt = request.Alt;
            }
            else
            {
                var entry = _dbContext.DealPictures.Add(new DealPictureEntity
                {
                    FileName = request.FileName,
                    Title = request.Title,
                    Alt = request.Alt,
                    DealId = deal.Id
                });
                picture = entry.Entity;
            }

            if(string.IsNullOrEmpty(deal.DefaultPicture) || request.IsDefaultPicture) deal.DefaultPicture = request.FileName;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<DealPicture>(picture.ToDealPictureModel(request.IsDefaultPicture));
        }

        public async Task<Result<DealCategoryList>> GetCategoriesAsync(int dealId)
        {
            var categories = await _dbContext.DealCategories.Include(x => x.Category).Where(x => x.DealId == dealId).ToListAsync();

            var list = new DealCategoryList
            {
                Data = categories.Select(x => x.Category.ToCategoryModel())
            };

            return new Result<DealCategoryList>(list);
        }

        public async Task<Result<DealCategoryList>> SaveCategoriesAsync(int dealId, SaveDealCategoriesRequest request)
        {
            var deal = await _dbContext.Deals.Include(x => x.DealCategory).SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
                return new Result<DealCategoryList>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "Could not find the deal" });

            var toRemove = deal.DealCategory.Where(x => request.Categories.Contains(x.CategoryId) == false).ToList();
            foreach (var c in toRemove)
                deal.DealCategory.Remove(c);

            foreach(var c in request.Categories)
            {
                if (deal.DealCategory.Any(x => x.CategoryId == c) == false) 
                    deal.DealCategory.Add(new DealCategoryJoin { DealId = dealId, CategoryId = c });
            }

            var saved = await _dbContext.SaveChangesAsync();

            return await GetCategoriesAsync(dealId);
        }


        #region private methods
        private async Task<StoreEntity> FindStoreForUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch
            {
                return null;
            }

            var host = uri.Host.ToLower();
            var store = await _dbContext.Stores.FirstOrDefaultAsync(x => host.EndsWith(x.Domain));

            return store;
        }


        private async Task<IEnumerable<int>> FindCategoryChildIds(string categoryCode)
        {
            var cateResult = await _categoryService.GetCategoryTreeAsync(categoryCode);
            if (cateResult.HasError()) return new int[0];

            return cateResult.Data.ToCategoryList().Select(x => x.Id);
        }

        #endregion
    }
}
