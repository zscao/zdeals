using System;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IProductService
    {
        Task<Result<PagedTrackedProducts>> GetTrackedProduct(DateTime date, int page);
    }
}
