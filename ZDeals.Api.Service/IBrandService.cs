using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;

namespace ZDeals.Api.Service
{
    public interface IBrandService
    {
        Task<Result<PagedBrands>> SearchBrandsAsync(string name);

        Task<Result<Brand>> GetBrandByIdAsync(int brandId);

        Task<Result<Brand>> CreateBrandAsync(CreateBrandRequest request);

        Task<Result<Brand>> UpdateBrandAsync(int brandId, UpdateBrandRequest request);

    }
}
