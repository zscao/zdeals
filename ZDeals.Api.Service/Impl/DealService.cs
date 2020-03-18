using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Api.Service.Impl
{
    public class DealService : IDealService
    {
        private readonly ZDealsDbContext _dbContext;

        public DealService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Deal>> GetDealById(int dealId)
        {
            var deal = await _dbContext.Deals.Include(x => x.Store).FirstOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new NotFoundError { Code = 1, Message = "The deal does not exist." });
            }

            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<Deal>> CreateDeal(CreateDealRequest request)
        {
            var store = request.storeId > 0
                ? await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == request.storeId)
                : null;

            var deal = new DealEntity
            {
                Title = request.Title,
                HighLight = request.HighLight,
                Descrition = request.Descrition,
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
                return new Result<Store>(new NotFoundError() { Code = 2, Message = "Store not found" });
            }

            return new Result<Store>(deal.Store.ToStoreModel());
        }

        public async Task<Result<Deal>> UpdateDeal(int dealId, UpdateDealRequest request)
        {
            var deal = await _dbContext.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                return new Result<Deal>(new NotFoundError { Code = 4, Message = "Deal does not exist" });
            }

            deal.Title = request.Title;
            deal.HighLight = request.HighLight;
            deal.Discount = request.Discount;
            deal.FullPrice = request.FullPrice;
            deal.DealPrice = request.DealPrice;

            deal.PublishedDate = request.PublishedDate;
            deal.ExpiryDate = request.ExpiryDate;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<Deal>(deal.ToDealModel());
        }

        public async Task<Result<Deal>> UpdateStore(int dealId, int storeId)
        {
            var deal = await _dbContext.Deals.FirstOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
            {
                return new Result<Deal>(new NotFoundError { Code = 4, Message = "Deal does not exist" });
            }

            var store = await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
            if(store == null)
            {
                return new Result<Deal>(new NotFoundError { Code = 5, Message = "Store does not exist" });
            }

            deal.Store = store;
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Deal>(deal.ToDealModel());
        }
    }
}
