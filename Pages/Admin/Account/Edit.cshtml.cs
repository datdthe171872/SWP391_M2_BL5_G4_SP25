using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Account
{
	public class EditModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public EditModel(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[BindProperty]
		public EditAccountDto Account { get; set; } = new EditAccountDto();

		public List<string> Roles { get; set; }

		public async Task OnGetAsync(int id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return;
			}
			Account = new EditAccountDto
			{
				Id = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				Address = user.Address,
				PhoneNumber = user.PhoneNumber,
				Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
			};
			Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				return Page();
			}

			var user = await _userManager.FindByIdAsync(Account.Id.ToString());
			if (user == null)
			{
				return NotFound();
			}

			user.Email = Account.Email;
			user.FullName = Account.FullName;
			user.Address = Account.Address;
			user.PhoneNumber = Account.PhoneNumber;
			user.isDelete = Account.IsDelete;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
				return Page();
			}

			var currentRoles = await _userManager.GetRolesAsync(user);
			var currentRole = currentRoles.FirstOrDefault();

			if (currentRole != Account.Role)
			{
				if (!string.IsNullOrEmpty(currentRole))
				{
					await _userManager.RemoveFromRoleAsync(user, currentRole);
				}

				if (!string.IsNullOrEmpty(Account.Role))
				{
					await _userManager.AddToRoleAsync(user, Account.Role);
				}
			}

			return RedirectToPage("/Admin/Account/Index");
		}
	}
}
