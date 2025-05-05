using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Constants;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Jobs
{
    public class DetailsModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public JobDetailDTO JobDetail { get; set; } =new JobDetailDTO();
        public IList<JobSeekerProfile> UserResumes { get; set; } = new List<JobSeekerProfile>();
        public HeaderDTO Header { get; set; } = new HeaderDTO();

        [BindProperty(SupportsGet = true)]
        public int JobId {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete==false).ToList();
            var user = await _userManager.GetUserAsync(User);
            Header.User = user;
            

            var job = await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Where(j => j.JobID == JobId && !j.isDelete && j.Status == StatusJob.OPEN)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return NotFound();
            }

            var requirements = await _context.Requirements
                .Where(r => r.JobID == JobId && !r.IsDelete)
                .Select(r => r.Content)
                .ToListAsync();
            var requirementsContent = requirements.Any() ? string.Join("<br>", requirements) : "No requirements available.";

            var responsibilities = await _context.Responsibilities
                .Where(r => r.JobID == JobId && !r.IsDelete)
                .Select(r => r.Content)
                .ToListAsync();
            var responsibilitiesContent = responsibilities.Any() ? string.Join("<br>", responsibilities) : "No responsibilities available.";

            JobDetail = new JobDetailDTO
            {
                JobID = job.JobID,
                Title = job.Title,
                CompanyName = job.Company?.CompanyName ?? "Unknown",
                CompanyID = job.CompanyID,
                Location = job.Location,
                JobType = job.JobType,
                PostDate = job.PostDate,
                Salary = job.Salary,
                Description = job.Description,
                CategoryName = job.JobCategory?.CategoryName ?? "Uncategorized",
                Exp = job.Exp,
                SkillsRequired = job.SkillsRequired,
                Gender = "Both",
                Requirements = requirementsContent, 
                Responsibilities = responsibilitiesContent 
            };

            if(user != null)
            {
                var query = _context.JobSeekerProfiles
                .Include(r => r.Resumes.Where(r => !r.IsDelete))
                .Where(p => p.UserID == user.Id && !p.isDelete);

                UserResumes = await query.ToListAsync();
                // Check if the user has already applied for this job
            }
            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string cvLink)
        {

            // Get the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }
            if (!User.IsInRole("JobSeeker"))
            {
                return RedirectToPage("/Error");
            }

            // Validate the job exists
            var job = await _context.Jobs
                .Where(j => j.JobID == JobId && !j.isDelete && j.Status == StatusJob.OPEN)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return RedirectToPage("/Error");
            }
            bool hasappli = await _context.JobApplications
                    .AnyAsync(ja => ja.JobID == JobId && ja.UserID == user.Id && !ja.isDelete);
            if (hasappli)
            {
                ModelState.AddModelError(string.Empty, "you have already apply this job");
                return await OnGetAsync();
            }
            // Create a new job application
            var jobApplication = new JobApplication
            {
                JobID = JobId,
                UserID = user.Id,
                CoverLetter = "",
                CVFile = cvLink,
                ApplicationDate = DateTime.UtcNow,
                Status = StatusJobApply.PENDING
            };

            // Save the application to the database
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            // Redirect to a confirmation page or back to the job details page

            return RedirectToPage("/JobSeeker/JobSeekerApplyed");
        }
    }
}