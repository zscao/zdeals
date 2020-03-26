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
    public class StoreService : IStoreService
    {
        private readonly ZDealsDbContext _dbContext; 

        public StoreService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedStoreList>> SearchDeals()
        {
            var stores = await _dbContext.Stores.Select(x => x.ToStoreModel()).ToListAsync();

            return new Result<PagedStoreList> { Data = new PagedStoreList { Data = stores } };
        }
        public async Task<Result<Store>> CreateStore(CreateStoreRequest request)
        {
            var store = new StoreEntity
            {
                Name = request.Name,
                Website = request.Website
            };

            var entry = _dbContext.Stores.Add(store);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Store>(entry.Entity.ToStoreModel());
        }

        public async Task<Result<Store>> GetStoreById(int storeId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
            if(store == null)
            {
                return new Result<Store>(new Error(ErrorType.NotFound) { Code = Sales.StoreNotFound, Message = "Store does not exist" });
            }

            return new Result<Store>(store.ToStoreModel());
        }


    }
}
