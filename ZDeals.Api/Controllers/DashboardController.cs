using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Service;
using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles = ApiRoles.Admin )]
    [ApiController]
    [Route(ApiRoutes.Dashboard.Base)]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("dailyVisit")]
        [ProducesDefaultResponseType(typeof(DealVisitStatis))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> GetDailyVisitStatis()
        {
            var start = DateTime.Today.AddDays(-30);
            var end = DateTime.Today;

            var result = await _dashboardService.GetDailyVisitStatis(start, end);
            return result;
        }
    }
}