using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Data;
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

		public List<string> Roles { get; set; } = new List<string>();

		[BindProperty(SupportsGet = true)]
		public UserSearchDto SearchInput { get; set; } = new UserSearchDto();

		public async Task OnGetAsync()
		{
			Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

			var usersQuery = _userManager.Users.AsQueryable();

			if (!string.IsNullOrWhiteSpace(SearchInput.SearchString))
			{
				string searchTermLower = SearchInput.SearchString.ToLower();
				usersQuery = usersQuery.Where(u =>
						u.FullName.ToLower().Contains(searchTermLower) ||
						u.Address.ToLower().Contains(searchTermLower) ||
						u.PhoneNumber.ToLower().Contains(searchTermLower));
			}

			var filteredUsers = await usersQuery.ToListAsync();

			Users = new List<UserDto>();

			if (!string.IsNullOrEmpty(SearchInput.SelectedRole))
			{
				var usersInRole = await _userManager.GetUsersInRoleAsync(SearchInput.SelectedRole);

				filteredUsers = filteredUsers.Where(u => usersInRole.Any(ur => ur.Id == u.Id)).ToList();
			}

			foreach (var user in filteredUsers)
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
