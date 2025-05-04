using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Common;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Account
{
	[Authorize(Roles ="Admin")]
	public class IndexModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public IndexModel(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public List<UserDto> Users { get; set; } = new List<UserDto>();

		public List<string> Roles { get; set; } = new List<string>();

		[BindProperty(SupportsGet = true)]
		public UserSearchDto SearchInput { get; set; } = new UserSearchDto();

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;

		public PaginationInfo Pagination { get; set; } = new PaginationInfo { PageSize = 5 };

		public async Task OnGetAsync()
		{
			Roles = await _roleManager.Roles
					.Select(r => r.Name ?? string.Empty) 
					.ToListAsync();

			var usersQuery = _userManager.Users.AsQueryable();

			if (!string.IsNullOrWhiteSpace(SearchInput.SearchString))
			{
				string searchTermLower = SearchInput.SearchString.ToLower();
				usersQuery = usersQuery.Where(u =>
						u.FullName.ToLower().Contains(searchTermLower) ||
						(u.Address != null && u.Address.ToLower().Contains(searchTermLower)) || 
						(u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(searchTermLower))); 
			}

			if (!string.IsNullOrEmpty(SearchInput.SelectedRole))
			{
				var role = await _roleManager.FindByNameAsync(SearchInput.SelectedRole);
				if (role != null)
				{
					var userIdsInRole = await _userManager.GetUsersInRoleAsync(SearchInput.SelectedRole); 
					var userIds = userIdsInRole.Select(u => u.Id).ToList();
					usersQuery = usersQuery.Where(u => userIds.Contains(u.Id));
				}
			}

			Pagination.PageNumber = PageNumber;
			var totalCount = await usersQuery.CountAsync();
			Pagination.CalculatePagination(totalCount);

			var filteredUsers = await usersQuery
					.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
					.Take(Pagination.PageSize)
					.ToListAsync();

			Users = new List<UserDto>();
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
