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
            _userManager = userManager;
        }

        public DashboardDTO Dashboard { get; set; }
        public string UserName { get; set; }
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AcceptedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public List<JobApplication> Applications { get; set; }
        public string SearchTerm { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public List<Job> ShortlistedJobs { get; set; }
        public HeaderDTO Header { get; set; } = new HeaderDTO();
        public async Task<IActionResult> OnGetAsync(string searchTerm, string status, string date)
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
                // Set default welcome message
                UserName = "Guest";

                // Get all applications with related data
                var query = _context.JobApplications
                    .Include(ja => ja.User)
                    .Include(ja => ja.Job)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(ja => 
                        ja.Job.Title.Contains(searchTerm) || 
                        ja.User.Email.Contains(searchTerm));
                    SearchTerm = searchTerm;
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(ja => ja.Status == status);
                    Status = status;
                }

                if (!string.IsNullOrEmpty(date))
                {
                    var filterDate = DateTime.Parse(date);
                    query = query.Where(ja => ja.ApplicationDate.Date == filterDate.Date);
                    Date = date;
                }

                // Get filtered applications
                Applications = await query
                    .OrderByDescending(ja => ja.ApplicationDate)
                    .ToListAsync();

                // Calculate statistics
                TotalApplications = Applications.Count;
                PendingApplications = Applications.Count(a => a.Status == "Pending");
                AcceptedApplications = Applications.Count(a => a.Status == "Accepted");
                RejectedApplications = Applications.Count(a => a.Status == "Rejected");

                return Page();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in OnGetAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Set default values
                Applications = new List<JobApplication>();
                TotalApplications = 0;
                PendingApplications = 0;
                AcceptedApplications = 0;
                RejectedApplications = 0;
                
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
                // Log the error
                Console.WriteLine($"Error in OnPostDeleteApplicationAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = "An error occurred while deleting the application.";
                return RedirectToPage();
            }
        }
    }
}

