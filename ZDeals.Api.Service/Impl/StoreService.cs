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
            var total = await _dbContext.Stores.CountAsync();
            var stores = await _dbContext.Stores.AsNoTracking().ToListAsync();

            var paged = new PagedStoreList
            {
                Data = stores.Select(x => x.ToStoreModel()),
                TotalCount = total,
                PageSize = total,
                PageNumber = 1
            };

            return new Result<PagedStoreList> { Data = paged };
        }

        public async Task<Result<Store>> GetStoreById(int storeId)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
            if (store == null)
            {
                return new Result<Store>(new Error(ErrorType.NotFound) { Code = Sales.StoreNotFound, Message = "Store does not exist" });
            }

            return new Result<Store>(store.ToStoreModel());
        }

        public async Task<Result<Store>> CreateStore(CreateStoreRequest request)
        {
            var store = new StoreEntity
            {
                Name = request.Name,
                Website = request.Website,
                Domain = request.Domain
            };

            var entry = _dbContext.Stores.Add(store);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Store>(entry.Entity.ToStoreModel());
        }



    }
}
