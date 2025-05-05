using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
    [Authorize(Roles = "Client")]
    public class UpdateJobModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public UpdateJobModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UpdateJobInput Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public int JobId { get; set; }
        public List<JobCategory> Categories { get; set; } = new List<JobCategory>();
        public List<Models.Company> Companies { get; set; } = new List<Models.Company>();

        public List<Requirement> Requirements { get; set; } = new List<Requirement>();
        public List<Responsibility> Responsibilities { get; set; } = new List<Responsibility>();
        public List<Benefit> Benefits { get; set; }=new List<Benefit>();
        public HeaderDTO HeaderDTO { get; set; } =new HeaderDTO();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            HeaderDTO.User = user;
            var clientProfile = await _context.ClientProfiles.FirstOrDefaultAsync(x => x.UserID == user.Id);

            if (clientProfile == null)
            {
                return Unauthorized();
            }
            
            var job = await _context.Jobs
                .Include(x=>x.Requirements)
                .Include(x=>x.Responsibilities)
                .Include(x=>x.Benefits)
                .FirstOrDefaultAsync(x=>x.JobID == JobId);
            if (job == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyID == job.CompanyID && x.isDelete == false);

            if(company.ClientProfileID != clientProfile.ClientProfileID)
            {
                return Unauthorized();
            }

            Categories = await _context.JobCategories.Where(x=>x.isDelete==false).ToListAsync();
            HeaderDTO.JobCategories = Categories;
            Companies = await _context.Companies.Where(x => x.ClientProfileID == clientProfile.ClientProfileID).ToListAsync();
            Requirements = job.Requirements;
            Benefits = job.Benefits;
            Responsibilities = job.Responsibilities;


            Input = new UpdateJobInput
            {
                Company = job.CompanyID,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Exp = job.Exp,
                Salary = job.Salary,
                Skill = job.SkillsRequired,
                Jobtype = job.JobType,
                Category = job.JobCategoryID
            };

            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
            var job = await _context.Jobs.FirstOrDefaultAsync(x=>x.JobID==JobId);
            job.CompanyID = Input.Company;
            job.Title = Input.Title;
            job.Description = Input.Description;
            job.Location = Input.Location;
            job.Exp = Input.Exp;
            job.Salary = Input.Salary;
            job.SkillsRequired = Input.Skill;
            job.JobType = Input.Jobtype;
            job.JobCategoryID = Input.Category;



            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            //requirement
            var existingRequirements = _context.Requirements.Where(r => r.JobID == job.JobID).ToList();
            
            foreach (var req in existingRequirements)
            {
                if(Input.Requirements == null)
                {
                    _context.Requirements.Remove(req);
                }
                else
                {
                    if (!Input.Requirements.Any(r => r.RequirementID == req.RequirementID))
                    {
                        _context.Requirements.Remove(req);
                    }
                }
                
            }

            // 2. Update hoặc Add mới
            if(Input.Requirements != null)
            {
                foreach (var reqInput in Input.Requirements)
                {
                    if (reqInput.RequirementID != 0) // Update
                    {
                        var existing = existingRequirements.FirstOrDefault(r => r.RequirementID == reqInput.RequirementID);
                        if (existing != null)
                        {
                            existing.Content = reqInput.Content;
                            _context.Requirements.Update(existing);
                        }
                    }
                    else // Insert mới
                    {
                        var newReq = new Requirement
                        {
                            JobID = job.JobID,
                            Content = reqInput.Content,
                            IsDelete = false
                        };
                        _context.Requirements.Add(newReq);
                    }
                }

            }

            //responsibility
            var existResponsibilities = _context.Responsibilities.Where(r=>r.JobID == job.JobID).ToList();
            foreach (var res in existResponsibilities)
            {
                if(Input.Responsibilities == null)
                {
                    _context.Responsibilities.Remove(res);
                }
                else
                {
                    if (!Input.Responsibilities.Any(r => r.ResponsibilityID == res.ResponsibilityID))
                    {
                        _context.Responsibilities.Remove(res);
                    }
                }
                
            }

            if(Input.Responsibilities != null)
            {
                foreach (var resInput in Input.Responsibilities)
                {
                    if (resInput.ResponsibilityID != 0)
                    {
                        var existing = existResponsibilities.FirstOrDefault(r => r.ResponsibilityID == resInput.ResponsibilityID);
                        if (existing != null)
                        {
                            existing.Content = resInput.Content;
                            _context.Responsibilities.Update(existing);
                        }
                    }
                    else
                    {
                        var newRes = new Responsibility
                        {
                            JobID = job.JobID,
                            Content = resInput.Content,
                            IsDelete = false
                        };
                        _context.Responsibilities.Add(newRes);
                    }
                }
            }
            

            //benefit
            var existBenefits = _context.Benefits.Where(x=>x.JobID == job.JobID).ToList();
            foreach (var item in existBenefits)
            {
                if (Input.Benefits == null)
                {
                    _context.Benefits.Remove(item);
                }
                else
                {
                    if (!Input.Benefits.Any(x => x.BenefitID == item.BenefitID))
                    {
                        _context.Benefits.Remove(item);
                    }
                }
            }

            if(Input.Benefits != null)
            {
                foreach (var benefitInput in Input.Benefits)
                {
                    if (benefitInput.BenefitID != 0)
                    {
                        var existing = existBenefits.FirstOrDefault(x => x.BenefitID == benefitInput.BenefitID);
                        if (existing != null)
                        {
                            existing.Content = benefitInput.Content;
                            _context.Benefits.Update(existing);
                        }
                    }
                    else
                    {
                        var newBenefit = new Benefit
                        {
                            JobID = job.JobID,
                            Content = benefitInput.Content,
                            IsDelete = false
                        };
                        _context.Benefits.Add(newBenefit);
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToPage("/Client/MyJob");
        }
    }
}
