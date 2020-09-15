using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Common;
using ZDeals.Data;

namespace ZDeals.Api.Service.Impl
{
    public class DashboardService: IDashboardService
    {
        private readonly ZDealsDbContext _dbContext;

        public DashboardService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<DealVisitStatis>> GetDailyVisitStatis(DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;

            string sql = @"SELECT DATE(VisitedTime) As VisitDate, COUNT(*) as VisitCount FROM VisitHistory GROUP BY DATE(VisitedTime)";

            var data = await _dbContext.DealVisitDeatail.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var details = new List<VisitDetail>();
            for(var date = start; date <= end; date = date.AddDays(1))
            {
                var d = data.FirstOrDefault(x => x.VisitDate == date);
                details.Add(new VisitDetail { Date = date, Count = d?.VisitCount ?? 0 });
            }

            var result = new DealVisitStatis()
            {
                StartDate = start,
                EndDate = end,
                StatisMode = StatisMode.Daily,
                Details = details
            };

            return new Result<DealVisitStatis>(result);
        }
    }
}
