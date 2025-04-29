using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using SWP391_M2_BL5_G4_SP25.Common;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25.Pages.Client_Dash
{
    public class AdminDashModel : PageModel
    {
        private readonly AdminDashService _adminDashService;
        public AdminDashModel(AdminDashService adminDashService)
        {
            _adminDashService = adminDashService;
        }

        public int JobCount { get; set; }
        public int CompanyCount { get; set; }
        public Dictionary<string, int> DailyApplications { get; set; }
        public Dictionary<string, int> MonthlyApplications { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
        public List<int> AvailableYears { get; set; }
        public bool IsLoading { get; set; } = true;
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int? year, int? month)
        {
            try
            {
                // Fetch counts
                JobCount = await _adminDashService.GetJobCountAsync();
                CompanyCount = await _adminDashService.GetCompanyCountAsync();

                // Get available years from JobApplications
                AvailableYears = await _adminDashService.GetAvailableYearsAsync();
                if (!AvailableYears.Any())
                {
                    AvailableYears = new List<int> { DateTime.Now.Year }; // Fallback to current year
                }

                // Set default year and month if not provided
                SelectedYear = year ?? AvailableYears.Max(); // Default to latest year
                SelectedMonth = month ?? DateTime.Now.Month; // Default to current month

                // Fetch chart data for the selected year and month
                DailyApplications = await _adminDashService.GetJobApplicationsByDayAsync(SelectedYear, SelectedMonth);
                MonthlyApplications = await _adminDashService.GetJobApplicationsByMonthAsync(SelectedYear);

                // Fallback data if empty
                if (DailyApplications.Values.All(v => v == 0))
                {
                    DailyApplications = new Dictionary<string, int>
                {
                    { "01", 5 }, { "02", 3 }, { "03", 7 }, { "04", 2 }, { "05", 4 }, { "06", 6 }, { "07", 1 },
                    { "08", 3 }, { "09", 5 }, { "10", 2 }, { "11", 4 }, { "12", 6 }, { "13", 1 }, { "14", 3 },
                    { "15", 5 }, { "16", 2 }, { "17", 4 }, { "18", 6 }, { "19", 1 }, { "20", 3 }, { "21", 5 },
                    { "22", 2 }, { "23", 4 }, { "24", 6 }, { "25", 1 }, { "26", 3 }, { "27", 5 }, { "28", 2 }
                };
                }
                if (MonthlyApplications.Values.All(v => v == 0))
                {
                    MonthlyApplications = new Dictionary<string, int>
                {
                    { "Jan", 10 }, { "Feb", 15 }, { "Mar", 8 }, { "Apr", 12 }, { "May", 5 },
                    { "Jun", 7 }, { "Jul", 9 }, { "Aug", 11 }, { "Sep", 6 }, { "Oct", 4 },
                    { "Nov", 13 }, { "Dec", 8 }
                };
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load dashboard data.";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}



