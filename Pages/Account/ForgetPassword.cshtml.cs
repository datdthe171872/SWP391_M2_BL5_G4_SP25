using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailSender _emailSender;
        public ForgetPasswordModel(UserManager<User> userManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnGetAsync(string Email,string Token)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData["StatusMessage"] = "Can not find user";
                return Page();
            }
            var result = await _userManager.ResetPasswordAsync(user, Token, "Abc123@");
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(Email, "Mật khẩu mới", $"mật khẩu mới :<br> <div>Abc123@</div>");
                TempData["StatusMessage"] = "Password has been restored !";
                return RedirectToPage("/Account/Login");
            }
            TempData["StatusMessage"] = "Có lỗi xảy ra khi đặt lại mật khẩu.";
            return RedirectToPage("/Error");
        }

    }
}
