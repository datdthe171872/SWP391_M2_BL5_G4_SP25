using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyDBContext _dbContext;
        public ChangePasswordModel(UserManager<User> userManager, SignInManager<User> signInManager,MyDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = context;
        }

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; }

        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            Header.JobCategories = _dbContext.JobCategories.Where(x=>x.isDelete ==false).ToList();
            var user = await _userManager.GetUserAsync(User);
            Header.User = user;
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user); // Đăng nhập lại sau khi đổi mật khẩu
            TempData["StatusMessage"] = "Your password has been changed.";

            return RedirectToPage("/Account/Logout");
        }
    }
}
