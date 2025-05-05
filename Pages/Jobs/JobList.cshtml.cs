using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Jobs
{
    public class JobListModel : PageModel
    {
        private readonly MyDBContext _context;

        public JobListModel(MyDBContext context)
        {
            _context = context;
        }

        public IList<Job> Jobs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalJobs { get; set; }
        public int PageSize { get; } = 10;
        public string SearchTerm { get; set; }
        public string SelectedLocation { get; set; }
        public int? SelectedCategory { get; set; }
        public string SelectedJobType { get; set; }
        public string SelectedExperience { get; set; }
        public string SelectedSalaryRange { get; set; }
        public IList<string> Locations { get; set; }
        public IList<JobCategory> Categories { get; set; }
        public IList<string> JobTypes { get; set; }
        public IList<string> Experiences { get; set; }
        public IList<string> SalaryRanges { get; set; }

        public async Task OnGetAsync(int? pageNumber, string search, string location, int? category, string jobType, string experience, string salary)
        {
            CurrentPage = pageNumber ?? 1;
            if (CurrentPage < 1) CurrentPage = 1;

            SearchTerm = search;
            SelectedLocation = location;
            SelectedCategory = category;
            SelectedJobType = jobType;
            SelectedExperience = experience;
            SelectedSalaryRange = salary;

           

            Locations = await _context.Jobs
                .Where(j => !j.isDelete)
                .Select(j => j.Location)
                .Distinct()
                .ToListAsync();

            Categories = await _context.JobCategories
                .Where(c => !c.isDelete)
                .ToListAsync();



            JobTypes = await _context.Jobs
                .Where(j => !j.isDelete)
                .Select(j => j.JobType)
                .Distinct()
                .ToListAsync();

            Experiences = await _context.Jobs
                .Where(j => !j.isDelete)
                .Select(j => j.Exp)
                .Distinct()
                .ToListAsync();

            SalaryRanges = new List<string>
            {
                "0 - 50000",
                "50000 - 75000",
                "75000 - 100000",
                "100000 - 150000",
                "150000+"
            };

            
            var query = _context.Jobs
                .Include(j => j.Company)
                .Where(j => !j.isDelete && j.Status == "Open");

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(j => j.Title.Contains(search) || j.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location == location);
            }

            if (category.HasValue)
            {
                query = query.Where(j => j.JobCategoryID == category);
            }


            if (!string.IsNullOrEmpty(jobType))
            {
                query = query.Where(j => j.JobType == jobType);
            }

            if (!string.IsNullOrEmpty(experience))
            {
                query = query.Where(j => j.Exp == experience);
            }

            if (!string.IsNullOrEmpty(salary))
            {
                var range = salary.Split('-').Select(s => s.Trim()).ToArray();
                int minSalary = int.Parse(range[0]);
                int? maxSalary = range[1] == "150000+" ? null : int.Parse(range[1]);
                query = maxSalary.HasValue
                    ? query.Where(j => j.Salary >= minSalary && j.Salary <= maxSalary)
                    : query.Where(j => j.Salary >= minSalary);
            }

            
            TotalJobs = await query.CountAsync();
            TotalPages = TotalJobs == 0 ? 1 : (int)Math.Ceiling((double)TotalJobs / PageSize);
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;

            Jobs = await query
                .OrderByDescending(j => j.PostDate)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
