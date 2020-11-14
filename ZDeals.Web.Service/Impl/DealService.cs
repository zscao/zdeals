using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Data;
using ZDeals.Data.Entities;
using ZDeals.Net;
using ZDeals.Web.Service.Mapping;
using ZDeals.Web.Service.Models;
using ZDeals.Web.Service.Options;

namespace ZDeals.Web.Service.Impl
{
    public class DealService : IDealService
    {

        private readonly ZDealsDbContext _dbContext;
        private readonly PictureStorageOptions _pictureStorageOptions;
        private readonly IPageService _pageService;
        private readonly IRequestContextProvider _requestContextProvider;

        public DealService(ZDealsDbContext dbContext, PictureStorageOptions pictureStorageOptions, IPageService pageService,IRequestContextProvider requestContextProvider)
        {
            _dbContext = dbContext;
            _pictureStorageOptions = pictureStorageOptions;
            _pageService = pageService;
            _requestContextProvider = requestContextProvider;
        }

        public async Task<Result<Deal?>> Visit(int dealId)
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
                    Code = Common.ErrorCodes.Sales.DealExpired, Message = "This deal has expired." 
                });
            }

            var context = _requestContextProvider.Context;

            var visit = new VisitHistoryEntity
            {
                DealId = dealId,
                ClientIp = context.ClientIP,
                VisitedTime = DateTime.UtcNow,
                SessionToken = context.SessionToken,
                SessionId = context.SessionId
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

            return new Result<Deal?>(deal.ToDealModel(_pictureStorageOptions?.GetPictureUrl));
        }

        public async Task<Result<Deal?>> GetDealById(int dealId)
        {
            var deal = await _dbContext.Deals.SingleOrDefaultAsync(x => x.Id == dealId && x.Status == DealStatus.Verified);
            if(deal == null)
            {
                var error = new Error(ErrorType.NotFound) { Code = Common.ErrorCodes.Sales.DealNotFound, Message = "Deal not found." };
                return new Result<Deal?>(error);
            }

            return new Result<Deal?>(deal.ToDealModel(_pictureStorageOptions?.GetPictureUrl));
        }

        public async Task<Result<IEnumerable<DealPriceHistory>>> GetDealPriceHistory(int dealId)
        {
            var data = await _dbContext.DealPrices.Where(x => x.DealId == dealId).OrderBy(x => x.UpdatedTime).ToListAsync();

            var list = new List<DealPriceHistory>();
            foreach (var d in data)
            {
                list.Add(new DealPriceHistory
                {
                    Price = d.Price,
                    Date = d.UpdatedTime
                });
            }
            // add a record to present the price for now
            if(list.Count > 0)
            {
                list.Add(new DealPriceHistory
                {
                    Price = list.Last().Price,
                    Date = DateTime.UtcNow
                });
            }

            return new Result<IEnumerable<DealPriceHistory>>(list);
        }

    }
}
