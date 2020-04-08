using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Common;
using ZDeals.Common.Constants;
using ZDeals.Identity;
using ZDeals.Identity.Options;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [Route(ApiRoutes.Users.Base)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;
        public UserController(IUserService userService, JwtOptions jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions;
        }

        [Authorize(Roles = ApiRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
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
    }
}