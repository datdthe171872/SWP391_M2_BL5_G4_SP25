using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Common;
using SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos;
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

		[BindProperty(SupportsGet = true)]
		public JobSearchDto SearchInput { get; set; } = new JobSearchDto();

		public PaginationInfo Pagination { get; set; } = new PaginationInfo { PageSize = 2 };

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;

		public async Task OnGetAsync()
		{
			var query = _context.Jobs
					.Include(j => j.Company)
					.Include(j => j.JobCategory)
					.Where(j => !j.isDelete)
					;
			if (!string.IsNullOrEmpty(SearchInput.SearchTerm))
			{
				query = query.Where(j =>
						j.Title.Contains(SearchInput.SearchTerm) ||
						j.Location.Contains(SearchInput.SearchTerm));
			}

			if (!string.IsNullOrEmpty(SearchInput.Status))
			{
				query = query.Where(j => j.Status == SearchInput.Status);
			}

			if (SearchInput.StartDate.HasValue)
			{
				query = query.Where(j => j.PostDate >= SearchInput.StartDate.Value.Date);
			}

			if (SearchInput.EndDate.HasValue)
			{
				query = query.Where(j => j.PostDate <= SearchInput.EndDate.Value.Date);
			}
			Pagination.PageNumber = PageNumber;
			var totalCount = await query.CountAsync();
			Pagination.CalculatePagination(totalCount);
			Jobs = await query
							.Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
							.Take(Pagination.PageSize).Select(j => new JobDto
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
							}).ToListAsync();
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
