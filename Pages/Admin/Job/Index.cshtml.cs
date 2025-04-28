using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.JobDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Job
{
	public class IndexModel : PageModel
	{
		private readonly MyDBContext _context;

		public IndexModel(MyDBContext context)
		{
			_context = context;
		}

		public List<JobDto> Jobs { get; set; } = new List<JobDto>();

		public async Task OnGetAsync()
		{
			var query = _context.Jobs
					.Include(j => j.Company)
					.Include(j => j.JobCategory)
					.Where(j => !j.isDelete)
					.Select(j => new JobDto
					{
						JobID = j.JobID,
						CompanyID = j.CompanyID,
						JobCategoryID = j.JobCategoryID,
						Title = j.Title,
						Description = j.Description,
						Location = j.Location, 
						Exp = j.Exp,
						Salary = j.Salary,
						SkillsRequired = string.Join(", ", j.Requirements.Select(r => r.Content)), 
						JobType = j.JobType,
						Status = j.Status,
						Company = j.Company,
						JobCategory = j.JobCategory
					});

			Jobs = await query.ToListAsync();
		}

		public async Task<IActionResult> OnPostUpdateStatusAsync(int jobId, string newStatus)
		{
			var job = await _context.Jobs.FindAsync(jobId);
			if (job == null)
			{
				return NotFound();
			}

			job.Status = newStatus;
			await _context.SaveChangesAsync();

			return RedirectToPage();
		}
	}
}
