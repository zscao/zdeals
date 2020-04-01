using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ZDeals.Api.Contract;
using ZDeals.Api.Contract.Requests;
using ZDeals.Api.Contract.Responses;

namespace ZDeals.Api.Controllers
{
    [Route(ApiRoutes.Users.Base)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var response = new AuthenticationResponse
            {
                Username = request.Username,
                Token = GenerateJWT(request)
            };
            return Ok(response);
        }


        private string GenerateJWT(LoginRequest request)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    //new Claim(ClaimTypes.Role, "guest")
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}