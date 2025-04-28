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
    [Authorize(Roles = "Client")]
    public class AppliedJobSeekerModel : PageModel
    {
        private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public AppliedJobSeekerModel(MyDBContext dbContext, IWebHostEnvironment env,UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _env = env;
            _userManager = userManager;
        }

        public List<AppliedJobSeeker> JobSeekers { get; set; } = new List<AppliedJobSeeker>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo { PageSize = 5 };
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int JobId { get; set; }

        [BindProperty]
        public ManageAppliedSeekerInput Input { get; set; } = new ManageAppliedSeekerInput();


        public async Task<IActionResult> OnGetAsync()
        {
            var job = _dbContext.Jobs.FirstOrDefault(x => x.JobID == JobId);
            if (job == null)
            {
                return Page();
            }
            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.CompanyID == job.CompanyID);
            var clientProfile = await _dbContext.ClientProfiles.FirstOrDefaultAsync(x => x.ClientProfileID == company.ClientProfileID);

            var userLogin = await _userManager.GetUserAsync(User);
            if(clientProfile.UserID != userLogin.Id)
            {
                return Unauthorized();
            }

            var applications = _dbContext.JobApplications.Where(x=>x.JobID == job.JobID).ToList();
            if(applications.Count == 0 || applications == null)
            {
                return Page();
            }
            foreach (var item in applications)
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == item.UserID);
                if (user == null)
                {
                    continue;
                }
                var seekerProfile = _dbContext.JobSeekerProfiles.FirstOrDefault(x => x.UserID == user.Id);
                if(seekerProfile == null)
                {
                    continue;
                }
                var path = Path.Combine(_env.WebRootPath, item.CVFile.Substring(1));
                var seeker = new AppliedJobSeeker
                {
                    JobApplicationId = item.JobApplicationID,
                    Name = user.FullName,
                    AppliedDate = item.ApplicationDate,
                    CV = item.CVFile,
                    Location = user.Address,
                    Logo = seekerProfile.Logo,
                    Status = item.Status,
                    FileExist = System.IO.File.Exists(path),
                };
                JobSeekers.Add(seeker);

            }
            //pagination
            Pagination.PageNumber = PageNumber;
            var total = JobSeekers.Count;
            Pagination.CalculatePagination(total);
            JobSeekers = JobSeekers
                    .Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                    .Take(Pagination.PageSize)
                    .ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var jobapplication = _dbContext.JobApplications.FirstOrDefault(x => x.JobApplicationID == Input.JobApplicationId);
            if (jobapplication == null)
            {
                return Page();
            }
            if (string.IsNullOrEmpty(Input.Type))
            {
                return Page();
            }
            if(Input.Type == "Confirm")
            {
                jobapplication.Status = "Confirm";
            }
            else
            {
                jobapplication.Status = "Reject";
            }
            
            try
            {
                await _dbContext.SaveChangesAsync();
                TempData["StatusMessage"] = "Change successfully!";
            }
            catch (Exception ex)
            {
                TempData["StatusMessage"] = "Error";
            }
            return await OnGetAsync();
        }
    }
}
