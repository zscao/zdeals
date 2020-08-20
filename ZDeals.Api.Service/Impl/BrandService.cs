using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service.Mapping;
using ZDeals.Common;
using ZDeals.Common.ErrorCodes;
using ZDeals.Data;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Service.Impl
{
    public class BrandService : IBrandService
    {
        private readonly ZDealsDbContext _dbContext; 

        public BrandService(ZDealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagedBrands>> SearchBrandsAsync(string name)
        {
            var query = _dbContext.Brands.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{name}%"));

            var brands = await query
                .OrderBy(x => x.Name)
                .Select(x => x.ToBrandModel())
                .ToListAsync();

            var paged = new PagedBrands
            {
                Data = brands,
                TotalCount = brands.Count,
                PageSize = brands.Count,
                PageNumber = 1
            };

            paged.Data = paged.Data.OrderBy(x => x.Name);

            return new Result<PagedBrands> { Data = paged };
        }

        public async Task<Result<Brand>> GetBrandByIdAsync(int brandId)
        {
            var brand = await _dbContext.Brands.SingleOrDefaultAsync(x => x.Id == brandId);
            if (brand == null)
            {
                return new Result<Brand>(new Error(ErrorType.NotFound) { Code = Sales.BrandNotFound, Message = "Brand does not exist" });
            }

            return new Result<Brand>(brand.ToBrandModel());
        }

        public async Task<Result<Brand>> CreateBrandAsync(CreateBrandRequest request)
        {
            var brand = new BrandEntity
            {
                Code = request.Code,
                Name = request.Name,
                DisplayOrder = request.DisplayOrder
            };

            var entry = _dbContext.Brands.Add(brand);
            var saved = await _dbContext.SaveChangesAsync();

            return new Result<Brand>(entry.Entity.ToBrandModel());
        }

        public async Task<Result<Brand>> UpdateBrandAsync(int brandId, UpdateBrandRequest request)
        {
            var brand = await _dbContext.Brands.SingleOrDefaultAsync(x => x.Id == brandId);
            if(brand == null)
            {
                return new Result<Brand>(new Error(ErrorType.NotFound) { Code = Sales.BrandNotFound, Message = "Brand does not exist" });
            }

            brand.Name = request.Name;
            brand.DisplayOrder = request.DisplayOrder;

            var saved = await _dbContext.SaveChangesAsync();
            return new Result<Brand>(brand.ToBrandModel());
        }

    }
}
