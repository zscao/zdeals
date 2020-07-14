using Abot2.Core;

namespace ZDeals.Engine.Schedulers
{
    /// <summary>
    /// a Abot schedular for site
    /// </summary>
    public interface ISiteScheduler: IScheduler
    {
        public string SiteCode { get; set; }
    }
}
