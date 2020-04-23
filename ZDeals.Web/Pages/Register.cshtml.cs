using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using ZDeals.Identity;
using ZDeals.Identity.Contract.Requests;

namespace ZDeals.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var request = new CreateUserRequest
            {
                Username = Username,
                Password = Password,
                Nickname = Username,
                Role = "Member"
            };
            var result = await _userService.CreateUserAsync(request);
            
            if (result.HasError()) return Page();
            
            return RedirectToPage("/Login");
        }
    }
}