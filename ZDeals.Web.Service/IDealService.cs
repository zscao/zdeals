using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealService
    {
        Task<Result<Deal>> GetDealById(int dealId);

        Task<Result<Deal>> MarkDealExpired(int dealId);

        /// <summary>
        /// the deal is explictly visited by a user
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        Task<Result<Deal>> Visit(int dealId, string clientIp);
    }
}
