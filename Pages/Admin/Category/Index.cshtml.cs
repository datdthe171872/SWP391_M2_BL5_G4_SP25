using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Common;
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

		public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

		[BindProperty(SupportsGet = true)]
		public CategorySearchDto SearchInput { get; set; } = new CategorySearchDto();

		public PaginationInfo Pagination { get; set; } = new PaginationInfo { PageSize = 2 };

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;

		public async Task OnGetAsync()
		{
			var query = _context.JobCategories.Where(c => !c.isDelete);

			if (!string.IsNullOrWhiteSpace(SearchInput.SearchString))
			{
				query = query.Where(c => c.CategoryName.ToLower().Contains(SearchInput.SearchString.ToLower()) ||
				c.Description.ToLower().Contains(SearchInput.SearchString.ToLower()));
			}
			Pagination.PageNumber = PageNumber;
			var totalCount = await query.CountAsync();
			Pagination.CalculatePagination(totalCount);
			Categories = await query
					.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
					.Take(Pagination.PageSize)
					.Select(c => new CategoryDto
					{
						JobCategoryID = c.JobCategoryID,
						CategoryName = c.CategoryName,
						Description = c.Description,
						isDelete = c.isDelete
					})
					.ToListAsync();
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
