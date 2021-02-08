using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Service;
using ZDeals.Common;
using ZDeals.Common.Constants;

namespace ZDeals.Api.Controllers
{
    [Authorize(Roles =ApiRoles.Api)]
    [ApiController]
    [Route(ApiRoutes.Products.Base)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<ActionResult<Result>> GetTrackedProducts([FromQuery] DateTime date, int? page)
        {
            int pageNumber = page ?? 1;
            return await _productService.GetTrackedProduct(date, pageNumber);
        }
    }
}
