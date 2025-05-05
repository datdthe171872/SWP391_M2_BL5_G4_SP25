using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWP391_M2_BL5_G4_SP25.Constants;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
    [Authorize(Roles = "Client")]
    public class CreateJobModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public CreateJobModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CreateJobInput Input { get; set; }

        public List<JobCategory> Categories { get; set; } = new List<JobCategory>();
        public List<Models.Company> Companies { get; set; } = new List<Models.Company>();

        public HeaderDTO HeaderDTO { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }

            Categories = await _context.JobCategories.Where(x => x.isDelete == false).ToListAsync();
            HeaderDTO.JobCategories = Categories;
            HeaderDTO.User = user;
            var clientProfile = await _context.ClientProfiles.FirstOrDefaultAsync(x => x.UserID == user.Id);

            if (clientProfile == null)
            {
                return Page();
            }

            Companies = await _context.Companies.Where(x => x.ClientProfileID == clientProfile.ClientProfileID && x.isDelete == false).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            if (string.IsNullOrEmpty(Input.Company.ToString()))
            {
                return BadRequest();
            }

            var job = new Job
            {
                CompanyID = Input.Company,
                Title = Input.Title,
                Description = Input.Description,
                Location = Input.Location,
                Exp = Input.Exp,
                Salary = Input.Salary,
                SkillsRequired = Input.Skill,
                PostDate = DateTime.Now,
                Status = StatusJob.WAIT,
                JobType = Input.Jobtype,
                JobCategoryID = Input.Category,
            };
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            if (!Input.Requirements.IsNullOrEmpty())
            {
                // 2. Thêm requirements
                foreach (var req in Input.Requirements)
                {
                    if (!string.IsNullOrWhiteSpace(req))
                    {
                        _context.Requirements.Add(new Requirement
                        {
                            JobID = job.JobID,
                            Content = req,
                            IsDelete = false
                        });
                    }
                }
            }

            if (!Input.Benefits.IsNullOrEmpty())
            {
                // 3. Thêm benefits
                foreach (var benefit in Input.Benefits)
                {
                    if (!string.IsNullOrWhiteSpace(benefit))
                    {
                        _context.Benefits.Add(new Benefit
                        {
                            JobID = job.JobID,
                            Content = benefit,
                            IsDelete = false
                        });
                    }
                }
            }

            if (!Input.Responsibilities.IsNullOrEmpty())
            {
                // 4. Thêm responsibilities
                foreach (var resp in Input.Responsibilities)
                {
                    if (!string.IsNullOrWhiteSpace(resp))
                    {
                        _context.Responsibilities.Add(new Responsibility
                        {
                            JobID = job.JobID,
                            Content = resp,
                            IsDelete = false
                        });
                    }
                }
            }


            await _context.SaveChangesAsync();

            return RedirectToPage("/Client/MyJob");
        }
    }
}
