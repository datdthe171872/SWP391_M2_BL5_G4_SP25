using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Dashboard.Account
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
	}
}
