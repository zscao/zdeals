using System;

namespace ZDeals.Engine.Schedulers.Repo
{
    public interface IScheduledPageRepo
    {
        bool IsVisitedUri(Uri uri);
    }
}
