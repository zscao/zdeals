using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using ZDeals.Api.Constants;
using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;
using ZDeals.Api.Helpers;
using ZDeals.Api.Options;
using ZDeals.Api.Service;
using ZDeals.Common;

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

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Result>> Login(LoginRequest request)
        {
            var result = await _userService.AuthenticateAsync(request);
            if (result.HasError())
            {
                return result;
            }

            var response = new AuthenticationResponse
            {
                Token = AuthenticationHelper.GenerateJWT(result.Data, _jwtOptions),
                User = result.Data
            };

            return new Result<AuthenticationResponse>(response);
        }



    }
}