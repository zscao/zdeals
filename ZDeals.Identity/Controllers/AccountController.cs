using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ZDeals.Common;
using ZDeals.Common.AspNetCore.Responses;
using ZDeals.Identity;
using ZDeals.Identity.Contract;
using ZDeals.Identity.Contract.Requests;
using ZDeals.Identity.Contract.Responses;

namespace ZDeals.Api.Controllers
{
    [Authorize]
    [Route(ApiRoutes.Account.Base)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService; ;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesDefaultResponseType(typeof(JwtResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Login(LoginRequest request)
        {
            var authResult = await _accountService.AuthenticateAsync(request.Username, request.Password);
            if (authResult.HasError())
                return authResult;

            return await _jwtService.GenerateJwtTokenAsync(authResult.Data);
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesDefaultResponseType(typeof(JwtResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> Refresh(RefreshTokenRequest request)
        {
            return await _jwtService.RefreshTokenAsync(request.Token, request.RefreshToken);
        }

        [HttpPut("password")]
        [ProducesResponseType(204)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Result>> ChangePassword(ChangePasswordRequest request)
        {
            var user = HttpContext.User?.Identity;
            return await _accountService.ChangePassword(user?.Name, request.CurrentPassword, request.NewPassword);
        }

    }
}