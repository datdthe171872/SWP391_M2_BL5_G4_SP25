using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDBContext _dBContext;
        private readonly UserManager<User> _userManager;

        public IndexModel(MyDBContext dBContext, UserManager<User> userManager)
        {
            this._dBContext = dBContext;
            this._userManager = userManager;
        }

        public List<JobCategory> JobCategories { get; set; } 
        public HeaderDTO Header { get; set; } = new HeaderDTO();

        public async Task<IActionResult> OnGetAsync()
        {
            JobCategories = _dBContext.JobCategories.Where(x=>x.isDelete==false).ToList();
            Header.JobCategories = JobCategories;
            return Page();
        }

    }
}
