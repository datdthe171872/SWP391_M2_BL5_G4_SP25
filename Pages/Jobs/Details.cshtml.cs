using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Jobs
{
    public class DetailsModel : PageModel
    {
        private readonly MyDBContext _context;

        public DetailsModel(MyDBContext context)
        {
            _context = context;
        }

        public JobDetailDTO JobDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Where(j => j.JobID == id && !j.isDelete && j.Status == "Open")
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return NotFound();
            }

            var requirements = await _context.Requirements
                .Where(r => r.JobID == id && !r.IsDelete)
                .Select(r => r.Content)
                .ToListAsync();
            var requirementsContent = requirements.Any() ? string.Join("<br>", requirements) : "No requirements available.";

            var responsibilities = await _context.Responsibilities
                .Where(r => r.JobID == id && !r.IsDelete)
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

            return Page();
        }
    }
}