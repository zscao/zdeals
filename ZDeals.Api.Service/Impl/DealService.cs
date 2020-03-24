﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public DealService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedDealList>> SearchDeals(int? pageSize, int? pageNumber)
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

            var paged = new PagedDealList
            {
                Data = deals.Select(x => x.ToDealListModel()),
                PageSize = size,
                PageNumber = number,
                TotalCount = total
            };

            return new Result<PagedDealList>(paged);
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
                Store = null,
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
            deal.HighLight = request.HighLight;
            deal.Discount = request.Discount;
            deal.FullPrice = request.FullPrice;
            deal.DealPrice = request.DealPrice;

            deal.PublishedDate = request.PublishedDate;
            deal.ExpiryDate = request.ExpiryDate;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<Deal>(deal.ToDealModel());
        }
    }
}
