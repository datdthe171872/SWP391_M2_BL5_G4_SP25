using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client
{
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

        public List<JobCategory> Categories { get; set; } =new List<JobCategory>();
        public List<SWP391_M2_BL5_G4_SP25.Models.Company> Companies { get; set; } = new List<Models.Company>();


        public async Task<IActionResult> OnGetAsync()
        {
            var user = _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var clientProfile = _context.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if (clientProfile == null)
            {
               return Page();
            }
            Categories = _context.JobCategories.ToList();
            Companies = _context.Companies.Where(x => x.ClientProfileID == clientProfile.ClientProfileID).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Client/AppliedJob");
        }
    }
}
