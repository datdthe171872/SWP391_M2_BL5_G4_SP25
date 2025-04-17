using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Threading.Tasks;

namespace SWP391_M2_BL5_G4_SP25.Pages.Dashboard
{
	public class ListAccountModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public ListAccountModel(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public List<UserDto> Users { get; set; } = new List<UserDto>();

		public async Task OnGetAsync()
		{
			var users = await _userManager.Users.ToListAsync();

			Users = new List<UserDto>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				Users.Add(new UserDto
				{
					User = user,
					Roles = roles.ToList()
				});
			}
		}
		public async Task<IActionResult> OnPostUpdateIsDeleteAsync(int userId, bool isDelete)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				return NotFound();
			}

			user.isDelete = isDelete;
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				return RedirectToPage();
			}
			return Page();
		}

	}
}
