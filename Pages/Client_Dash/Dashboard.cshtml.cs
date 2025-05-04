using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using SWP391_M2_BL5_G4_SP25.Common;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client_Dash
{
    [Authorize(Roles = "Client")]
    public class DashboardModel : PageModel
    {
        private readonly MyDBContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardModel(MyDBContext context, UserManager<User> userManager)
        {
            _context = context;
            MonthlyStats = new MonthlyStats();
            YearlyStats = new YearlyStats();
            Applications = new List<JobApplication>();
            AvailableYears = new List<int>();
            SearchTerm = string.Empty;
            Status = string.Empty;
            Date = string.Empty;
            _userManager = userManager;
        }

        public required DashboardDTO Dashboard { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public List<JobApplication> Applications { get; set; }
        public MonthlyStats MonthlyStats { get; set; }
        public YearlyStats YearlyStats { get; set; }
        public List<int> AvailableYears { get; set; }
        public string SearchTerm { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public List<Job> ShortlistedJobs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? year, int? month)
        {
            Header.JobCategories = _context.JobCategories.Where(x=>x.isDelete ==false).ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user.isDelete)
            {
                return RedirectToPage("/InActiveUser");
            }
            Header.User = user;
            try
            {
                UserName = "Guest";

                var applications = await _context.JobApplications
                    .Include(ja => ja.User)
                    .Include(ja => ja.Job)
                    .OrderByDescending(ja => ja.ApplicationDate)
                    .ToListAsync();

                // Get available years from applications
                AvailableYears = applications.Select(a => a.ApplicationDate.Year).Distinct().OrderBy(y => y).ToList();

                // Set selected year and month
                MonthlyStats = new MonthlyStats
                {
                    SelectedYear = year ?? DateTime.Now.Year,
                    SelectedMonth = month ?? DateTime.Now.Month
                };

                YearlyStats = new YearlyStats
                {
                    SelectedYear = year ?? DateTime.Now.Year
                };

                // Calculate total statistics
                TotalApplications = applications.Count;
                PendingApplications = applications.Count(a => a.Status == "Pending");
                AcceptedApplications = applications.Count(a => a.Status == "Comfirm");
                RejectedApplications = applications.Count(a => a.Status == "Reject");

                // Calculate monthly statistics
                for (int m = 1; m <= 12; m++)
                {
                    var monthApplications = applications.Where(a => 
                        a.ApplicationDate.Year == MonthlyStats.SelectedYear && 
                        a.ApplicationDate.Month == m);
                    
                    MonthlyStats.Pending.Add(monthApplications.Count(a => a.Status == "Pending"));
                    MonthlyStats.Comfirm.Add(monthApplications.Count(a => a.Status == "Comfirm"));
                    MonthlyStats.Reject.Add(monthApplications.Count(a => a.Status == "Reject"));
                }

                // Calculate yearly statistics
                YearlyStats.Years = AvailableYears;

                foreach (var y in AvailableYears)
                {
                    var yearApplications = applications.Where(a => a.ApplicationDate.Year == y);
                    YearlyStats.Pending.Add(yearApplications.Count(a => a.Status == "Pending"));
                    YearlyStats.Comfirm.Add(yearApplications.Count(a => a.Status == "Comfirm"));
                    YearlyStats.Reject.Add(yearApplications.Count(a => a.Status == "Reject"));
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGetAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Set default values
                TotalApplications = 0;
                PendingApplications = 0;
                AcceptedApplications = 0;
                RejectedApplications = 0;
                MonthlyStats = new MonthlyStats();
                YearlyStats = new YearlyStats();
                
                return Page();
            }
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
            try
            {
                var application = await _context.JobApplications
                    .FirstOrDefaultAsync(ja => ja.JobApplicationID == applicationId);

                if (application != null)
                {
                    _context.JobApplications.Remove(application);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Application deleted successfully.";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPostDeleteApplicationAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = "An error occurred while deleting the application.";
                return RedirectToPage();
            }
        }
    }
}

