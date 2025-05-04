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
    [Authorize(Roles = "Client")]
    public class EditModel : PageModel
	{
		private readonly MyDBContext _context;
		private readonly UploadImg _uploadImg;
		private readonly UserManager<User> _userManager;

		public EditModel(MyDBContext context, UploadImg uploadImg, UserManager<User> userManager)
		{
			_context = context;
			_uploadImg = uploadImg;
			_userManager = userManager;
		}
		[BindProperty]
		public EditCompanyDto Input { get; set; } = new EditCompanyDto();

		public HeaderDTO Header { get; set; } = new HeaderDTO();
		public async Task<IActionResult> OnGetAsync(int id)
		{
			Header.JobCategories = _context.JobCategories.Where(x => x.isDelete == false).ToList();
			var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }

            var clientProfile = await _context.ClientProfiles
					.FirstOrDefaultAsync(cp => cp.UserID == user.Id && !cp.isDelete);

			if (clientProfile == null)
			{
				TempData["StatusMessage"] = "You need to create a client profile first";
				return RedirectToPage("/ClientProfiles/Create");
			}

			var company = await _context.Companies
					.FirstOrDefaultAsync(c => c.CompanyID == id && !c.isDelete);

			if (company == null)
			{
				return NotFound();
			}

			if (company.ClientProfileID != clientProfile.ClientProfileID)
			{
				return Forbid();
			}

			Input.CompanyID = company.CompanyID;
			Input.CompanyName = company.CompanyName;
			Input.Email = company.Email;
			Input.Description = company.Description;
			Input.Link = company.Link;
			Input.Location = company.Location;
			Input.ExistingImagePath = company.Image;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("ImageFile");
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
				return RedirectToPage("/Client/Profile");
			}

			var company = await _context.Companies
					.FirstOrDefaultAsync(c => c.CompanyID == Input.CompanyID && !c.isDelete);

			if (company == null)
			{
				return NotFound();
			}

			if (company.ClientProfileID != clientProfile.ClientProfileID)
			{
				return Forbid();
			}

			//if (company.CompanyName.ToLower() != Input.CompanyName.ToLower())
			//{
			//    bool companyExists = await _context.Companies
			//        .AnyAsync(c => c.CompanyName.ToLower() == Input.CompanyName.ToLower() 
			//                && c.CompanyID != Input.CompanyID 
			//                && !c.isDelete);

			//    if (companyExists)
			//    {
			//        ModelState.AddModelError("Input.CompanyName", "A company with this name already exists.");
			//        return Page();
			//    }
			//}

			string imagePath = company.Image;
			if (Input.ImageFile != null)
			{
				imagePath = await _uploadImg.UploadAsync(Input.ImageFile, "uploads/companies");

				if (!string.IsNullOrEmpty(company.Image))
				{
				}
			}

			company.CompanyName = Input.CompanyName;
			company.Email = Input.Email;
			company.Description = Input.Description;
			company.Link = Input.Link;
			company.Location = Input.Location;
			company.Image = imagePath;

			await _context.SaveChangesAsync();

			return RedirectToPage("/Client/MyCompany");
		}
	}
}
