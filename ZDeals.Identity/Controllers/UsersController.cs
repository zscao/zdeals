using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Common.Constants;
using ZDeals.Identity;
using ZDeals.Identity.Contract;
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
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
        }

    }
}