﻿using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Web.Service.Models;

namespace ZDeals.Web.Service
{
    public interface IDealSearchService
    {
        Task<Result<DealSearchResult?>> SearchDeals(DealSearchRequest request);
    }

    public class DealSearchRequest
    {
        public string? Category { get; set; }
        public string? Keywords { get; set; }

        public int? Page { get; set; }

        public string? Store { get; set; }

        /// <summary>
        /// contains brand code
        /// </summary>
        public string? Brand { get; set; }

        public string? Sort { get; set; }

        /// <summary>
        /// Delivery
        /// </summary>
        public string? Del { get; set; }
    }
}
