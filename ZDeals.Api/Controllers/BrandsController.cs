﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Models;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Service;
using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles = ApiRoles.Admin )]
    [ApiController]
    [Route(ApiRoutes.Brands.Base)]
    public class BrandsController : ControllerBase
    {

        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(PagedBrands))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [SwaggerOperation(OperationId = "SearchBrands")]
        public async Task<ActionResult<Result>> Search([FromQuery] string name)
        {
            return await _brandService.SearchBrandsAsync(name);
        }


        [HttpGet("{brandId}")]
        [ProducesDefaultResponseType(typeof(Brand))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [SwaggerOperation(OperationId = "GetBrand")]
        public async Task<ActionResult<Result>> GetById(int brandId)
        {
            return await _brandService.GetBrandByIdAsync(brandId);
        }


        [HttpPost]
        [ProducesDefaultResponseType(typeof(Brand))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [SwaggerOperation(OperationId = "CreateBrand")]
        public async Task<ActionResult<Result>> Create(CreateBrandRequest request)
        {
            var result = await _brandService.CreateBrandAsync(request);
            return Created($"api/brands/{result.Data.Id}", result);
        }

        [HttpPut("{brandId}")]
        [ProducesDefaultResponseType(typeof(Brand))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [SwaggerOperation(OperationId = "UpdateBrand")]
        public async Task<ActionResult<Result>> Update(int brandId, [FromBody] UpdateBrandRequest request)
        {
            return await _brandService.UpdateBrandAsync(brandId, request);
        }
    }
}