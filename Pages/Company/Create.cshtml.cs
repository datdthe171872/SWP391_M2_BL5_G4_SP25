using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25.Pages.Company
{
	public class CreateModel : PageModel
	{
		private readonly MyDBContext _context;
		private readonly UploadImg _uploadImg;
		private readonly UserManager<User> _userManager;

		public CreateModel(MyDBContext context, UploadImg uploadImg, UserManager<User> userManager)
		{
			_context = context;
			_uploadImg = uploadImg;
			_userManager = userManager;
		}

		[BindProperty]
		public CreateCompanyDto Input { get; set; } = new CreateCompanyDto();

		public async Task<IActionResult> OnGet()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToPage("/Account/Login");
			}

			var clientProfile = await _context.ClientProfiles
					.FirstOrDefaultAsync(cp => cp.UserID == user.Id && !cp.isDelete);

			if (clientProfile == null)
			{
				TempData["StatusMessage"] = "You need to create a client profile first";
				return RedirectToPage("/ClientProfiles/Create");
			}

			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToPage("/Account/Login");
			}

			var clientProfile = await _context.ClientProfiles
					.FirstOrDefaultAsync(cp => cp.UserID == user.Id && !cp.isDelete);

			if (clientProfile == null)
			{
				TempData["StatusMessage"] = "You need to create a client profile first";
				return RedirectToPage("/ClientProfiles/Create");
			}

			string imagePath = null;
			if (Input.ImageFile != null)
			{
				imagePath = await _uploadImg.UploadAsync(Input.ImageFile, "uploads/companies");
			}

			var company = new Models.Company
			{
				CompanyName = Input.CompanyName,
				Email = Input.Email,
				Description = Input.Description,
				Link = Input.Link,
				Location = Input.Location,
				Image = imagePath,
				ClientProfileID = clientProfile.ClientProfileID, 
				isDelete = false
			};

			_context.Companies.Add(company);
			await _context.SaveChangesAsync();
			return RedirectToPage("CompanyList");
		}
	}
}
