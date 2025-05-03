using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos;
using SWP391_M2_BL5_G4_SP25.DTO.UserDtos;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Data;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Category
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
	{
		private readonly MyDBContext _context;

		public CreateModel(MyDBContext context)
		{
			_context = context;
		}

		[BindProperty]
		public CreateCategoryDto Input { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingCategory = await _context.JobCategories
					.FirstOrDefaultAsync(c => c.CategoryName == Input.CategoryName);

			if (existingCategory != null)
			{
				ModelState.AddModelError("Input.CategoryName", "Category name already exists.");
				return Page();
			}

			var newCategory = new JobCategory
			{
				CategoryName = Input.CategoryName,
				Description = Input.Description,
				isDelete = false
			};

			_context.JobCategories.Add(newCategory);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
