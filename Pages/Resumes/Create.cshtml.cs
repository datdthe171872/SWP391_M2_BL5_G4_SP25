using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Resumes
{
    [Authorize(Roles = "JobSeeker")] 
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(UserManager<User> userManager, MyDBContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public IFormFile CvFile { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["Error"] = "Vui lòng đăng nhập để upload CV.";
                return Page();
            }

            if (CvFile == null || CvFile.Length == 0)
            {
                ViewData["Error"] = "Vui lòng chọn file CV.";
                return Page();
            }

            if (!CvFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                ViewData["Error"] = "Chỉ chấp nhận file PDF.";
                return Page();
            }
            if (CvFile.Length > 5 * 1024 * 1024)
            {
                ViewData["Error"] = "File CV không được vượt quá 5MB.";
                return Page();
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads/resumes");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{CvFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await CvFile.CopyToAsync(stream);
            }

            // Check if user already has a JobSeekerProfile
            var existingProfile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserID == user.Id && !p.isDelete);

            if (existingProfile != null)
            {
                // Update existing profile
                existingProfile.Description = Description;
                existingProfile.Link = $"/uploads/resumes/{uniqueFileName}";
                _context.JobSeekerProfiles.Update(existingProfile);
            }
            else
            {
                // Create new profile
                var jobSeekerProfile = new JobSeekerProfile
                {
                    UserID = user.Id,
                    Logo = "default.jpg",
                    Description = Description,
                    Link = $"/uploads/resumes/{uniqueFileName}",
                    Dob = DateTime.Now,
                    isDelete = false
                };
                _context.JobSeekerProfiles.Add(jobSeekerProfile);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Upload CV thành công!";
            return RedirectToPage("/Resumes/Index");
        }
    }
} 