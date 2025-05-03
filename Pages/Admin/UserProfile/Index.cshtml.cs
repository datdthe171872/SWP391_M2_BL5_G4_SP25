using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.UserProfile
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[BindProperty]
		public UserProfileDto UserProfile { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id = null)
		{
			User user;

			if (id.HasValue)
			{
				user = await _userManager.FindByIdAsync(id.Value.ToString());
			}
			else
			{
				user = await _userManager.GetUserAsync(User);
			}

			if (user == null)
			{
				return NotFound();
			}

			var roles = await _userManager.GetRolesAsync(user);
			UserProfile = new UserProfileDto
			{
				Id = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				Address = user.Address,
				PhoneNumber = user.PhoneNumber,
			};

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
				return NotFound();
			}

			user.FullName = UserProfile.FullName;
			user.Address = UserProfile.Address;
			user.PhoneNumber = UserProfile.PhoneNumber;

			if (user.Email != UserProfile.Email)
			{
				var setEmailResult = await _userManager.SetEmailAsync(user, UserProfile.Email);
				if (!setEmailResult.Succeeded)
				{
					foreach (var error in setEmailResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					return Page();
				}
			}

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return Page();
			}

			await _signInManager.RefreshSignInAsync(user);
			TempData["StatusMessage"] = "Profile update successfull!";
			return RedirectToPage();
		}
	}
}
