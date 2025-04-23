using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Linq;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Category
{
	public class IndexModel : PageModel
	{
		private readonly MyDBContext _context;

		public IndexModel(MyDBContext context)
		{
			_context = context;
		}

		public List<JobCategory> Categories { get; set; } = new List<JobCategory>();

		[BindProperty(SupportsGet = true)]
		public CategorySearchDto SearchInput { get; set; } = new CategorySearchDto();

		public async Task OnGetAsync()
		{
			var query = _context.JobCategories.Where(c => !c.isDelete);

			if (!string.IsNullOrWhiteSpace(SearchInput.SearchString))
			{
				query = query.Where(c => c.CategoryName.ToLower().Contains(SearchInput.SearchString.ToLower()) || 
				c.Description.ToLower().Contains(SearchInput.SearchString.ToLower()));
			}

			Categories = await query.ToListAsync();
		}

		public async Task<IActionResult> OnPostDeleteAsync(int id)
		{
			var category = await _context.JobCategories.FirstOrDefaultAsync(c => c.JobCategoryID == id && !c.isDelete);
			if (category == null)
			{
				return NotFound();
			}

			category.isDelete = true;
			_context.JobCategories.Update(category);
			await _context.SaveChangesAsync();

			return RedirectToPage();
		}

	}
}
