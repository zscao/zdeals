using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Common.Constants;
using ZDeals.Identity;
using ZDeals.Identity.Contract;
using ZDeals.Identity.Contract.Models;
using ZDeals.Identity.Contract.Requests;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [Route(ApiRoutes.Users.Base)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = ApiRoles.Admin)]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(User))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            return Created($"/users/{result.Data.UserId}", result);
        }

    }
}