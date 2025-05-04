using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Common;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
    [Authorize(Roles ="Client")]
    public class AppliedJobModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public AppliedJobModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<JobCategory> jobCategory { get; set; } =new List<JobCategory>();
        public List<AppliedJob> appliedJobs { get; set; } = new List<AppliedJob>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo {PageSize=5};
        
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public FilterJobInput Input {  get; set; }

        [BindProperty]
        public ManageAppliedJobInput InputManage { get; set; }

        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            Header.User = user;
            jobCategory = _context.JobCategories.Where(x=>x.isDelete==false).ToList();
            Header.JobCategories = jobCategory;
            var clientprofile = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if (clientprofile == null)
            {
                return Page();
            }
            var companies = _context.Companies.Where(x => x.ClientProfileID == clientprofile.ClientProfileID).ToList();
            if (companies.Count == 0|| companies == null)
            {
                return Page();
            }
            var myJobs = _context.Jobs
                .Where(x => companies.Contains(x.Company) && x.isDelete == false)
                .OrderByDescending(x=>x.PostDate)
                .Include(x => x.Company)
                .Include(x=>x.JobCategory)
                .Include(x=>x.Responsibilities)
                .Include(x=>x.Benefits)
                .Include(x=>x.Requirements)
                .ToList();
            if(myJobs.Count == 0 || myJobs == null) return Page();
            foreach (var item in myJobs)
            {
                var appliedUserCount= _context.JobApplications.Where(x=>x.JobID == item.JobID).Count(); 
                var appliedJob = new AppliedJob
                {
                    Id = item.JobID,
                    Requirements = item.Requirements,
                    Benefits = item.Benefits,
                    Category = item.JobCategory,
                    Company = item.Company,
                    Description = item.Description,
                    Exp = item.Exp,
                    Location = item.Location,
                    Responsibilities = item.Responsibilities,
                    Salary = item.Salary,
                    Skill  = item.SkillsRequired,
                    Status = item.Status,
                    Title = item.Title,
                    Jobtype = item.JobType,
                    PostedDate = item.PostDate,
                    AppliedUser = appliedUserCount,
                };
                appliedJobs.Add(appliedJob);
            }
            //filter 
            //searchtitle
            if(!string.IsNullOrEmpty(Input.SearchTitle))
            {
                appliedJobs = appliedJobs.Where(x=>x.Title.Contains(Input.SearchTitle)).ToList();
            }
            //
            if (!string.IsNullOrEmpty(Input.CategoryId.ToString()))
            {
                appliedJobs = appliedJobs.Where(x=>x.Category.JobCategoryID.Equals(Input.CategoryId)).ToList();
            }
            //
            if (!string.IsNullOrEmpty(Input.Order) && Input.Order.Equals("oldest"))
            {
                appliedJobs = appliedJobs.OrderBy(x => x.PostedDate).ToList();
            }
            //
            if (!string.IsNullOrEmpty(Input.Method))
            {
                appliedJobs = appliedJobs.Where(x=>x.Jobtype.Equals(Input.Method)).ToList();
            }
            //pagination
            Pagination.PageNumber = PageNumber;
            var total = appliedJobs.Count();
            Pagination.CalculatePagination(total);
            appliedJobs = appliedJobs
                    .Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                    .Take(Pagination.PageSize)
                    .ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var job = _context.Jobs.Include(x=>x.Company).FirstOrDefault(x => x.JobID == InputManage.JobId);

            if (job == null)
            {
                return Page();
            }
            var clientProfile = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);

            if (clientProfile == null)
            {
                return RedirectToPage("/Error");
            }
            if(job.Company.ClientProfileID!= clientProfile.ClientProfileID)
            {
                return RedirectToPage("/Error");
            }


            if (InputManage.Type == "Delete")
            {
                job.isDelete = true;
                TempData["StatusMessage"] = "Delete job successfully!";
            }
            else
            {
                job.Status = "End";
                TempData["StatusMessage"] = "End job successfully!";
            }
            try
            {
                var result = await _context.SaveChangesAsync();
               
            }catch (Exception ex)
            {
                TempData["StatusMessage"] = "End  Error";
            }
            return await OnGetAsync();
        }
    }
}
