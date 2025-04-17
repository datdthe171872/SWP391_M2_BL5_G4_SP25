using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Threading.Tasks;

namespace SWP391_M2_BL5_G4_SP25.Pages.Dashboard
{
	public class ListAccountModel : PageModel
	{
		private readonly UserManager<User> _userManager;

		public ListAccountModel(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public IEnumerable<User> Users { get; set; }

		public async Task OnGetAsync()
		{
			Users = await _userManager.Users.Where(u => !u.isDelete).ToListAsync();
		}
	}
}
