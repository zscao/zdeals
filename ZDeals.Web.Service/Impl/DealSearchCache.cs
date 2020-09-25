
using Microsoft.Extensions.Caching.Memory;

using System;

using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service.Impl
{
    public class DealSearchCache : IDealSearchCache
    {
        private readonly IMemoryCache _memoryCache;

        public DealSearchCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DealSearchResult? GetSearchResult(DealSearchRequest request)
        {
            string key = GetCacheKey(request);
            if (string.IsNullOrEmpty(key)) return null;

            var cached = _memoryCache.TryGetValue(key, out DealSearchResult result);
            return cached ? result : null;
        }

        public void SetSearchResult(DealSearchRequest request, DealSearchResult result)
        {
            string key = GetCacheKey(request);
            if (string.IsNullOrEmpty(key)) return;

            _memoryCache.Set(key, result, TimeSpan.FromMinutes(5));
        }


        private string GetCacheKey(DealSearchRequest request)
        {
            // now we only cache the default search
            if (string.IsNullOrEmpty(request.Category)
                && string.IsNullOrEmpty(request.Keywords)
                && string.IsNullOrEmpty(request.Sort)
                && string.IsNullOrEmpty(request.Store)
                && string.IsNullOrEmpty(request.Del)
                && string.IsNullOrEmpty(request.Brand)
                && (request.Page.HasValue == false || request.Page.Value == 1)) return "_Default";

            return string.Empty;
        }
    }
}
