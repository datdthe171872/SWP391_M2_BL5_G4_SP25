using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Dashboard.Account
{
	public class CreateModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public CreateModel(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[BindProperty]
		public CreateAccountDto Input { get; set; }

		public List<string> Roles { get; set; }

		public async Task OnGetAsync()
		{
			Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				return Page();
			}
			var existingUser = await _userManager.FindByEmailAsync(Input.Email);
			if (existingUser != null)
			{
				ModelState.AddModelError("Input.Email", "Email already exists. Please use a different email address.");
				Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				return Page();
			}
			var user = new User
			{
				UserName = Input.Email,
				Email = Input.Email,
				FullName = Input.FullName,
				Address = Input.Address,
				PhoneNumber = Input.PhoneNumber,
				CreatedAt = DateTime.Now,
				isDelete = true
			};

			var defaultPassword = "123@qwE";
			var result = await _userManager.CreateAsync(user, defaultPassword);

			if (result.Succeeded)
			{
				if (!string.IsNullOrEmpty(Input.Role))
				{
					await _userManager.AddToRoleAsync(user, Input.Role);
				}

				return RedirectToPage("/Dashboard/Account/Index");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}

				Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				return Page();
			}
		}

	}
}
