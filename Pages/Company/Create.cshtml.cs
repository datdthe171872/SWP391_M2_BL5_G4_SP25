using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25.Pages.Company
{
	[Authorize(Roles ="Client")]
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

		public HeaderDTO Header { get; set; } =new HeaderDTO();
		public async Task<IActionResult> OnGet()
		{
			var user = await _userManager.GetUserAsync(User);
			Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete ==false).ToList();
			Header.User = user;
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }

            var clientProfile = await _context.ClientProfiles
					.FirstOrDefaultAsync(cp => cp.UserID == user.Id && !cp.isDelete);

			if (clientProfile == null)
			{
				TempData["StatusMessage"] = "You need to update a client profile first";
				return RedirectToPage("/Client/Profile");
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
			if (user.isDelete)
			{
				return RedirectToPage("/InActiveUser");
			}

			var clientProfile = await _context.ClientProfiles
					.FirstOrDefaultAsync(cp => cp.UserID == user.Id && !cp.isDelete);

			if (clientProfile == null)
			{
                TempData["StatusMessage"] = "You need to update a client profile first";
                return RedirectToPage("/Client/Profile");
            }

			bool companyExists = await _context.Companies
						.AnyAsync(c => c.CompanyName.ToLower() == Input.CompanyName.ToLower() && !c.isDelete);
			bool emailExist = await _context.Companies.AnyAsync(x=>x.Email.ToLower() == Input.Email.ToLower() && !x.isDelete);
			if (emailExist)
			{
                ModelState.AddModelError("Input.Email", "A company with this email already exists.");
                return Page();
            }
			if (companyExists)
			{
				ModelState.AddModelError("Input.CompanyName", "A company with this name already exists.");
				return Page();
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
			return RedirectToPage("/Client/MyCompany");
		}
	}
}
