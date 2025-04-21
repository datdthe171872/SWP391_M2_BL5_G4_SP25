using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Company
{
    public class DetailsModel : PageModel
    {
        private readonly MyDBContext _context;

        public DetailsModel(MyDBContext context)
        {
            _context = context;
        }

        public CompanyDetailDTO CompanyDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var company = await _context.Companies
                .Where(c => c.CompanyID == id && !c.isDelete)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return NotFound();
            }

            CompanyDetail = new CompanyDetailDTO
            {
                CompanyID = company.CompanyID,
                CompanyName = company.CompanyName,
                Description = company.Description ?? "No description available",
                Image = company.Image ?? "/assets/img/home-1/company/default.svg",
                Location = company.Location ?? "Unknown",
                //Email = company.Email ?? "Not provided",
                //Phone = company.Phone ?? "Not provided",
                //Website = company.Website ?? "Not provided",
                //FoundedDate = company.FoundedDate,
                //Industry = company.Industry ?? "Not specified",
                //EmployeeCount = company.EmployeeCount
            };

            return Page();
        }
    }
}
