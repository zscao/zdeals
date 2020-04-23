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
        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService; ;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Result>> Login(LoginRequest request)
        {
            var authResult = await _userService.AuthenticateAsync(request.Username, request.Password);
            if (authResult.HasError())
                return authResult;

            return await _jwtService.GenerateJwtTokenAsync(authResult.Data);
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<Result>> Refresh(RefreshTokenRequest request)
        {
            return await _jwtService.RefreshTokenAsync(request.Token, request.RefreshToken);
        }

        [Authorize(Roles = ApiRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
        }
    }
}