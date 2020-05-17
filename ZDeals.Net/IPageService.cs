using System;
using System.Threading.Tasks;

namespace ZDeals.Net
{
    public interface IPageService
    {
        Task<PageStatus> CheckPageStatus(string url);
    }
}
