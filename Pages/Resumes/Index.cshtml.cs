using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Resumes
{
    [Authorize(Roles = "JobSeeker")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(UserManager<User> userManager, MyDBContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }

        public IList<JobSeekerProfile> JobSeekerProfiles { get; set; }
        public string SearchDescription { get; set; }
        public DateTime? SearchDob { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchDescription, DateTime? searchDob)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var query = _context.JobSeekerProfiles
                .Where(p => p.UserID == user.Id && !p.isDelete);

            if (!string.IsNullOrEmpty(searchDescription))
            {
                query = query.Where(p => p.Description != null && p.Description.Contains(searchDescription));
            }

            if (searchDob.HasValue)
            {
                query = query.Where(p => p.Dob.Date == searchDob.Value.Date);
            }

            JobSeekerProfiles = await query.OrderByDescending(p => p.JobSeekerProfileID).ToListAsync();
            
            // Debug information
            foreach (var profile in JobSeekerProfiles)
            {
                Console.WriteLine($"Profile ID: {profile.JobSeekerProfileID}, User ID: {profile.UserID}");
            }

            SearchDescription = searchDescription;
            SearchDob = searchDob;

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var profile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.JobSeekerProfileID == id && p.UserID == user.Id && !p.isDelete);

            if (profile != null)
            {
                // Delete the physical file
                if (!string.IsNullOrEmpty(profile.Link))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, profile.Link.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Delete from database
                _context.JobSeekerProfiles.Remove(profile);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Xóa CV thành công!";
            }

            return RedirectToPage();
        }
    }
} 