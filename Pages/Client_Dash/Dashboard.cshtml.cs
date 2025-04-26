using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client_Dash
{

    public class DashboardModel : PageModel
    {
        private readonly MyDBContext _context;

        public DashboardModel(MyDBContext context)
        {
            _context = context;
        }

        public DashboardDTO Dashboard { get; set; }

        public async Task OnGetAsync()
        {

            var clientIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(clientIdString, out int clientId))
            {
                Dashboard = new DashboardDTO();
                return;
            }

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.ClientProfileID == clientId);

            if (company == null)
            {
                Dashboard = new DashboardDTO();
                return;
            }

            // Overview stats
            Dashboard = new DashboardDTO
            {
                OpenJobsCount = await _context.Jobs.CountAsync(j => j.CompanyID == company.CompanyID && j.Status == "Open"),
                TotalApplications = await _context.JobApplications.CountAsync(ja => _context.Jobs.Any(j => j.JobID == ja.JobID && j.CompanyID == company.ClientProfileID)),
                ShortlistedCount = await _context.JobApplications.CountAsync(ja => _context.Jobs.Any(j => j.JobID == ja.JobID && j.CompanyID == company.ClientProfileID) && ja.Status == "Shortlisted"),
                Jobs = await _context.Jobs
                    .Where(j => j.CompanyID == company.ClientProfileID)
                    .Include(j => j.JobApplications)
                    .Select(j => new JobDto
                    {
                        ID = j.JobID,
                        Title = j.Title,
                        Location = j.Location,
                        Salary = j.Salary.ToString(),
                        PostedDate = j.PostDate,
                        Status = j.Status,
                        ApplicationCount = j.JobApplications.Count
                    })
                    .ToListAsync()
            };
        }

        public async Task<IActionResult> OnPostCloseJobAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job != null)
            {
                job.Status = "Closed";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteApplicationAsync(int applicationId)
        {
            var application = await _context.JobApplications.FindAsync(applicationId);
            if (application != null)
            {
                _context.JobApplications.Remove(application);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}

