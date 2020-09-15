using System;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IDashboardService
    {
        Task<Result<DealVisitStatis>> GetDailyVisitStatis(DateTime start, DateTime end);
    }
}
