using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Identity.Contract;
using ZDeals.Identity.Contract.Requests;
using ZDeals.Common;
using ZDeals.Common.Constants;
using ZDeals.Common.Options;
using ZDeals.Identity;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [Route(ApiRoutes.Users.Base)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;
        public UsersController(IUserService userService, JwtOptions jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Result>> Login(LoginRequest request)
        {
            return await _userService.AuthenticateAsync(request.Username, request.Password);            
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<Result>> Refresh(RefreshTokenRequest request)
        {
            return await _userService.RefreshTokenAsync(request.Token, request.RefreshToken);
        }

        [Authorize(Roles = ApiRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
        }
    }
}