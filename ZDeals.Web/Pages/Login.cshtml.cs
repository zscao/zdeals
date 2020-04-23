using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

using ZDeals.Identity;
using ZDeals.Identity.Contract.Models;

namespace ZDeals.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        public bool Remember { get; set; }

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _userService.AuthenticateAsync(this.Username, this.Password);
            
            if (result.HasError()) return Page();
            
            await SignInUserAsync(result.Data);
            return RedirectToPage("/Index");
        }

        private async Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("NickName", user.Nickname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
                IsPersistent = this.Remember
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}