using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Resumes
{
    [Authorize(Roles = "JobSeeker")]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(UserManager<User> userManager, MyDBContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Resume Resume { get; set; }

        [BindProperty]
        public IFormFile CvFile { get; set; }
        public HeaderDTO Header { get; set; } =new HeaderDTO();
        [BindProperty(SupportsGet =true)]
        public int id {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            Header.User = user;
            Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete==false).ToList();
            var profile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserID == user.Id);

            if (profile == null)
            {
                return RedirectToPage("/Resumes/Index");
            }
            Resume = _context.Resumes.FirstOrDefault(x => x.ResumeID == id && x.IsDelete ==false);
            if(Resume == null)
            {
                return RedirectToPage("/Resumes/Index");
            }
            if(Resume.JobSeekerProfileID != profile.JobSeekerProfileID)
            {
                return RedirectToPage("/Error");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);


            var existingProfile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserID == user.Id && !p.isDelete);

            if (existingProfile == null)
            {
                return RedirectToPage("/Resumes/Index");
            }
            var resume = await _context.Resumes.FirstOrDefaultAsync(x=>x.ResumeID == id && x.IsDelete ==false); 
            if (resume == null)
            {
                return RedirectToPage("/Error");
            }


            if (CvFile != null)
            {
                if (!CvFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("CvFile", "Chỉ chấp nhận file PDF.");
                    return Page();
                }

                if (CvFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("CvFile", "File CV không được vượt quá 5MB.");
                    return Page();
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/resumes");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{CvFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CvFile.CopyToAsync(stream);
                }

                //// Delete old file
                //if (!string.IsNullOrEmpty(existingProfile.Link))
                //{
                //    var oldFilePath = Path.Combine(_environment.WebRootPath, existingProfile.Link.TrimStart('/'));
                //    if (System.IO.File.Exists(oldFilePath))
                //    {
                //        System.IO.File.Delete(oldFilePath);
                //    }
                //}

                resume.Link = $"/uploads/resumes/{uniqueFileName}";
            }

            //existingProfile.Description = Description;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Cập nhật CV thành công!";
            return RedirectToPage("/Resumes/Index");
        }
    }
} 