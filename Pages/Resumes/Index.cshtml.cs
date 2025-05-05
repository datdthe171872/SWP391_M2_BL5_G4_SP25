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

        public List<Resume> resumes { get; set; } =new List<Resume>();
        public HeaderDTO Header { get; set; } =new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Header.User = user;
            Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete==false).ToList();
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            var seekerProfile = _context.JobSeekerProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if (seekerProfile == null)
            {
                return RedirectToPage("/JobSeeker/JobSeekerProfile");
            }
            resumes = _context.Resumes.Where(x=>x.JobSeekerProfileID == seekerProfile.JobSeekerProfileID).ToList();

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
            var resume = _context.Resumes.FirstOrDefault(x => x.ResumeID == id && x.IsDelete == false);
            if (resume == null)
            {
                return RedirectToPage("/Error");
            }

            if (resume != null)
            {
                // Delete the physical file
                if (!string.IsNullOrEmpty(resume.Link))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, resume.Link.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Delete from database
                _context.Resumes.Remove(resume);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Xóa CV thành công!";
            }

            return RedirectToPage();
        }
    }
} 