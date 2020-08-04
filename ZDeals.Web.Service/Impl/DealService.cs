using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class DealService : IDealService
    {

        private readonly ZDealsDbContext _dbContext;

        public DealService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Result<Deal>> Visit(int dealId)
        {
            return GetDealById(dealId);
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
    }
}
