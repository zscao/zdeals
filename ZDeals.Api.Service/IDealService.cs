﻿using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IDealService
    {
        Task<Result<PagedDeals>> SearchDealsAsync(SearchDealRequest request);

        Task<Result<Deal>> CreateDealAsync(CreateDealRequest request);

        Task<Result<Deal>> GetDealByIdAsync(int dealId);

        Task<Result<DealExistence>> CheckExistenceBySourceAsync(string source);

        Task<Result<Store>> GetDealStoreAsync(int dealId);

        Task<Result<Deal>> UpdateDealAsync(int dealId, UpdateDealRequest request);

        Task<Result<DealPictureList>> GetPicturesAsync(int dealId);

        Task<Result<DealPicture>> SavePictureAsync(int dealId, SaveDealPictureRequest request);

        Task<Result<DealCategoryList>> GetCategoriesAsync(int dealId);

        Task<Result<DealCategoryList>> SaveCategoriesAsync(int dealId, SaveDealCategoriesRequest request);

        Task<Result<DealPriceList>> GetPricesAsync(int dealId);

        Task<Result<DealPriceList>> AddPricesAsync(AddPricesRequest request);

        Task<Result<Deal>> DeleteDealAsync(int dealId);

        Task<Result<Deal>> VerifyDealAsync(int dealId);

        Task<Result<Deal>> RecycleDealAsync(int dealId);
    }
}
