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
    public class MyCompanyModel : PageModel
    {
        private readonly MyDBContext _dbContext;
        private readonly UserManager<User> _userManager;

        public MyCompanyModel(MyDBContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public List<Models.Company> Companies = new List<Models.Company>();

        public PaginationInfo Pagination { get; set; } = new PaginationInfo { PageSize = 2 };

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public CompanyManageInput Input { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Header.JobCategories = _dbContext.JobCategories.Where(x=>x.isDelete==false).ToList();
            var user = await _userManager.GetUserAsync(User);
            Header.User = user;
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            var clientProfile = _dbContext.ClientProfiles.FirstOrDefault(x => x.UserID == user.Id);
            if(clientProfile == null)
            {
                return Page();
            }
            Companies = _dbContext.Companies.Where(x=>x.ClientProfileID == clientProfile.ClientProfileID && x.isDelete ==false).ToList();
            Pagination.PageNumber = PageNumber;
            var total = Companies.Count();
            Pagination.CalculatePagination(total);
            Companies = Companies
                    .Skip((Pagination.PageNumber - 1) * Pagination.PageSize)
                    .Take(Pagination.PageSize)
                    .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var company = _dbContext.Companies.Include(x => x.ClientProfile).FirstOrDefault(x => x.CompanyID == Input.CompanyId);
            if(company == null)
            {
                return RedirectToPage("/Error");
            }
            if(user.Id != company.ClientProfile.UserID)
            {
                return RedirectToPage("/Error");
            }
            company.isDelete = true;
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                TempData["StatusMessage"] = "Dlete successfully!";
            }
            catch (Exception ex)
            {
                TempData["StatusMessage"] = "Delete Error";
            }


            return Page();
        }
    }
}
