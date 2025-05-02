using SWP391_M2_BL5_G4_SP25.DTO;

namespace SWP391_M2_BL5_G4_SP25.Service
{
    public class DashboardService
    {
        public Task<AdminDashDTO> GetDashboardDataAsync()
        {
            // Mock data (replace with API/database call later)
            return Task.FromResult(new AdminDashDTO
            {
                JobCount = 25,
                CompanyCount = 10,
                JobApplicationsWeekly = new Dictionary<string, int>
                {
                    { "Mon", 5 }, { "Tue", 8 }, { "Wed", 12 }, { "Thu", 10 }, { "Fri", 15 }, { "Sat", 7 }, { "Sun", 3 }
                },
                JobApplicationsMonthly = new Dictionary<string, int>
                {
                    { "Jan", 50 }, { "Feb", 60 }, { "Mar", 70 }, { "Apr", 55 }, { "May", 80 }, { "Jun", 65 },
                    { "Jul", 75 }, { "Aug", 90 }, { "Sep", 85 }, { "Oct", 70 }, { "Nov", 60 }, { "Dec", 50 }
                },
                JobApplicationsYearly = new Dictionary<string, int>
                {
                    { "2020", 500 }, { "2021", 600 }, { "2022", 700 }, { "2023", 800 }, { "2024", 900 }, { "2025", 400 }
                }
            });
        }
    }
}
