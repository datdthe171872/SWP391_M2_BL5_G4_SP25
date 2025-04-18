using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public LoginModel(SignInManager<User> signInManager, EmailSender emailSender, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public bool Confirm { get; set; }

        [BindProperty]
        public LoginInput Input { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(Confirm)
            {
                //if (!ModelState.IsValid) return Page();
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    TempData["StatusMessage"] = "Cann't find user";
                    return RedirectToPage("/Account/Login");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Page("/Account/ForgetPassword", null, new
                {
                    email = user.Email,
                    token = token
                }, Request.Scheme);

                await _emailSender.SendEmailAsync(Email, "Đặt lại mật khẩu", $"Bấm vào link sau để đặt lại mật khẩu: <a href='{resetLink}'>Reset Password</a>");
                return RedirectToPage("/ConfirmEmail");
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);

                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = "Login successfully!";
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            
        }

    }
}
