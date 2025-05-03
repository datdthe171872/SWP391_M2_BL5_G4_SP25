using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Constants;
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

        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync(int id)
        {
			Header.JobCategories = _context.JobCategories.Where(x => x.isDelete == false).ToList();
			var job = await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Where(j => j.JobID == id && !j.isDelete && j.Status == StatusJob.OPEN)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return NotFound();
            }

            JobDetail = new JobDetailDTO
            {
                JobID = job.JobID,
                Title = job.Title,
                CompanyName = job.Company?.CompanyName ?? "Unknown",
                Location = job.Location,
                JobType = job.JobType,
                PostDate = job.PostDate,
                Salary = job.Salary,
                Description = job.Description,
                CategoryName = job.JobCategory?.CategoryName ?? "Uncategorized",
                Exp = job.Exp,
                SkillsRequired = job.SkillsRequired,
                Gender = "Both"
            };

            return Page();
        }
    }
}
