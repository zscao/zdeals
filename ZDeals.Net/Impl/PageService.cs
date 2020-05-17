using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZDeals.Net.Impl
{
    public class PageService : IPageService
    {
        public async Task<PageStatus> CheckPageStatus(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
                return PageStatus.Normal;
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return PageStatus.NotFound;
            else
                return PageStatus.UnableToCheck;

        }
    }
}
