using Abot2.Core;

namespace ZDeals.Engine.Core
{
    public interface IStoreScheduler: IScheduler
    {
        void AddTrackedPages(string store);
    }
}
