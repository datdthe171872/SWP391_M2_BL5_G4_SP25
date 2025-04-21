using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Company
{
    public class CompanyListModel : PageModel
    {
        private readonly MyDBContext _context;

        public CompanyListModel(MyDBContext context)
        {
            _context = context;
        }

        public IList<CompanyListDTO> Companies { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCompanies { get; set; }
        public int PageSize { get; } = 9; 

        public async Task OnGetAsync(int? pageNumber)
        {
            CurrentPage = pageNumber ?? 1;
            if (CurrentPage < 1) CurrentPage = 1;

            var query = _context.Companies
                .Where(c => !c.isDelete);

            TotalCompanies = await query.CountAsync();
            TotalPages = TotalCompanies == 0 ? 1 : (int)System.Math.Ceiling((double)TotalCompanies / PageSize);
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;

            var companies = await query
                .OrderBy(c => c.CompanyName)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            Companies = companies.Select(c => new CompanyListDTO
            {
                CompanyID = c.CompanyID,
                CompanyName = c.CompanyName,
                Description = c.Description ?? "No description available",
                Image = c.Image ?? "/assets/img/home-1/company/amazon.svg",
                Location = c.Location ?? "Unknown"
            }).ToList();
        }
    }
}
