using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWP391_M2_BL5_G4_SP25.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public RegisterInput Input { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var userExist = await _userManager.FindByEmailAsync(Input.Email);
            if(userExist != null)
            {
                ModelState.AddModelError(string.Empty,"Email existed!");
                return Page();
            }
            var user = new User {Email = Input.Email, FullName = Input.Fullname,UserName = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);


            if (result.Succeeded)
            {
                if (Input.RoleType)
                {
                    await _userManager.AddToRoleAsync(user, "JobSeeker");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["StatusMessage"] = "Register successfully!";
                return RedirectToPage("/Account/Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
