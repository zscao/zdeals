using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities;
using ZDeals.Net;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class DealService : IDealService
    {

        private readonly ZDealsDbContext _dbContext;
        private readonly IPageService _pageService;

        public DealService(ZDealsDbContext dbContext, IPageService pageService)
        {
            _dbContext = dbContext;
            _pageService = pageService;
        }

        public async Task<Result<Deal?>> Visit(int dealId, string clientIp)
        {
            var result = new Result<Deal?>();

            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId);
            if(deal == null)
            {
                result.Errors.Add(new Error(ErrorType.NotFound) 
                { 
                    Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal not found." 
                });
                return result;
            }

            var pageStatus = await _pageService.CheckPageStatus(deal.Source);
            if(pageStatus == PageStatus.NotFound)
            {
                deal.ExpiryDate = DateTime.UtcNow;
                result.Errors.Add(new Error(ErrorType.BadRequest) 
                {
                    Code = Common.ErrorCodes.Sales.DealExpired, Message = "Deal has expired." 
                });
            }

            var visit = new VisitHistoryEntity
            {
                DealId = dealId,
                ClientIp = clientIp,
                VisitedTime = DateTime.UtcNow
            };
            var entry = _dbContext.DealVisitHistory.Add(visit);

            var saved = await _dbContext.SaveChangesAsync();

            //result.Data = deal.ToDealModel();
            return result;
        }

        public async Task<Result<Deal?>> MarkDealExpired(int dealId)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId);
            if (deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal does not exist." };
                return new Result<Deal?>(error);
            }

            deal.ExpiryDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return new Result<Deal?>(deal.ToDealModel());
        }

        public async Task<Result<Deal?>> GetDealById(int dealId)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId && x.Deleted == false);
            if(deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal not found." };
                return new Result<Deal?>(error);
            }

            return new Result<Deal?>(deal.ToDealModel());
        }
    }
}
