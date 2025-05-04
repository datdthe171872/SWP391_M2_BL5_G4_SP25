using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO.JobDtos;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Admin.Job
{
	public class DetailModel : PageModel
	{
		private readonly MyDBContext _context;

		public DetailModel(MyDBContext context)
		{
			_context = context;
		}

		[BindProperty]
		public JobDto Job { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			var job = await _context.Jobs
					.Include(j => j.Company)
					.FirstOrDefaultAsync(j => j.JobID == id);

			if (job == null)
			{
				return NotFound();
			}

			Job = new JobDto
			{
				JobID = job.JobID,
				CompanyID = job.CompanyID,
				JobCategoryID = job.JobCategoryID,
				Title = job.Title,
				Description = job.Description,
				Location = job.Location,
				Exp = job.Exp,
				Salary = job.Salary,
				SkillsRequired = job.SkillsRequired,
				JobType = job.JobType,
				PostDate = job.PostDate,
				Status = job.Status,
				isDelete = job.isDelete,
				Company = job.Company,
				JobCategory = job.JobCategory
			};

			return Page();
		}

		public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string newStatus)
		{
			var jobToUpdate = await _context.Jobs.FindAsync(id);
			if (jobToUpdate == null)
			{
				return NotFound();
			}

			jobToUpdate.Status = newStatus;
			await _context.SaveChangesAsync();

			return RedirectToPage(new { id = id });
		}
	}
}
