using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Category
{
	public class EditModel : PageModel
	{
		private readonly MyDBContext _context;

		public EditModel(MyDBContext context)
		{
			_context = context;
		}

		[BindProperty]
		public EditCategoryDto Category { get; set; } = new EditCategoryDto();

		public async Task OnGetAsync(int id)
		{
			var category = await _context.JobCategories
			.FirstOrDefaultAsync(c => c.JobCategoryID == id && !c.isDelete);
			if (category == null)
			{
				return;
			}
			Category = new EditCategoryDto
			{
				JobCategoryID = category.JobCategoryID,
				CategoryName = category.CategoryName,
				Description = category.Description
			};
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var category = await _context.JobCategories
					.FirstOrDefaultAsync(c => c.JobCategoryID == Category.JobCategoryID && !c.isDelete);

			if (category == null)
			{
				return NotFound();
			}

			category.CategoryName = Category.CategoryName;
			category.Description = Category.Description;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
				return Page();
			}

			return RedirectToPage("./Index");
		}
	}
}
