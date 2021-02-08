using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Common;
using ZDeals.Data;

namespace ZDeals.Api.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly ZDealsDbContext _dbContext;
        private readonly RequestContext _requestContext;

        public ProductService(ZDealsDbContext dbContext, IRequestContextProvider requestContextProvider)
        {
            _dbContext = dbContext;
            
            _requestContext = requestContextProvider.Context;
        }

        public async Task<Result<PagedTrackedProducts>> GetTrackedProduct(DateTime date, int page)
        {
            int pageSize = 50;
            if (page < 1) page = 1;
            
            var query = _dbContext.Deals.AsNoTracking().Where(x => x.CreatedTime >= date);
            int total = await query.CountAsync();

            var products = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TrackedProduct { Url = x.Source, AddedDate = x.CreatedTime })
                .ToListAsync();

            var paged = new PagedTrackedProducts
            {
                Data = products,
                PageSize = pageSize,
                PageNumber = page,
                TotalCount = total
            };

            return new Result<PagedTrackedProducts>(paged);
        }
    }
}
