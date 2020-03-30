using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Api.Service.Impl
{
    public class DealService : IDealService
    {
        private readonly ZDealsDbContext _dbContext;
        private readonly IStoreService _storeService;

        public DealService(ZDealsDbContext dbContext, IStoreService storeService)
        {
            _dbContext = dbContext;
            _storeService = storeService;
        }

        public async Task<Result<PagedDeals>> SearchDeals(int? pageSize, int? pageNumber)
        {
            int size = pageSize ?? 20;
            int number = pageNumber ?? 1;

            var total = await _dbContext.Deals.CountAsync();

            var deals = await _dbContext.Deals.AsNoTracking()
                .Include(x => x.Store)
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


        public async Task<Result<Deal>> GetDealById(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).FirstOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "The deal does not exist." });
            }

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<Deal>> CreateDeal(CreateDealRequest request)
        {
            var store = await FindStoreForUrl(request.Source);

            var deal = new DealEntity
            {
                Title = request.Title,
                Highlight = request.HighLight,
                Description = request.Description,
                FullPrice = request.FullPrice,
                DealPrice = request.DealPrice,
                Discount = request.Discount,
                PublishedDate = request.PublishedDate,
                ExpiryDate = request.ExpiryDate,
                Source = request.Source,
                Store = store,
            };
            
            var entry =_dbContext.Deals.Add(deal);            
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Deal>(entry.Entity.ToDealModel());
        }

        public async Task<Result<Store>> GetDealStore(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).FirstOrDefaultAsync(x => x.Id == dealId);
            if(deal?.Store == null)
            {
                return new Result<Store>(new Error(ErrorType.NotFound) { Code = Sales.StoreNotFound, Message = "Store not found" });
            }

            return new Result<Store>(deal.Store.ToStoreModel());
        }

        public async Task<Result<Deal>> UpdateDeal(int dealId, UpdateDealRequest request)
        {
            var deal = await _dbContext.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new Error(ErrorType.Validation) { Code = Sales.DealNotFound, Message = "Deal does not exist" });
            }

            deal.Title = request.Title;
            deal.Highlight = request.HighLight;
            deal.Description = request.Description;
            deal.Discount = request.Discount;
            deal.FullPrice = request.FullPrice;
            deal.DealPrice = request.DealPrice;

            deal.PublishedDate = request.PublishedDate;
            deal.ExpiryDate = request.ExpiryDate;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<Deal>(deal.ToDealModel());
        }


        public async Task<Result<DealPictureList>> GetPictures(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == dealId);
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

        public async Task<Result<DealPicture>> SavePicture(int dealId, SaveDealPictureRequest request)
        {
            var deal = await _dbContext.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
                return new Result<DealPicture>(new Error(ErrorType.NotFound) { Code = Sales.DealNotFound, Message = "Could not find the deal" });

            var picture = await _dbContext.DealPictures.FirstOrDefaultAsync(x => x.DealId == dealId && x.FileName == request.FileName);
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

    }
}
