using System;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Common;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Service
{
    public class AdminDashService
    {
        private readonly MyDBContext _dbContext;
        public AdminDashService(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetJobCountAsync()
        {
            return await _dbContext.Jobs.CountAsync();
        }

        public async Task<int> GetCompanyCountAsync()
        {
            return await _dbContext.Companies.CountAsync();
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            return await _dbContext.JobApplications
                .Select(a => a.ApplicationDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetJobApplicationsByDayAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var applications = await _dbContext.JobApplications
                .Where(a => a.ApplicationDate.Year == year && a.ApplicationDate.Month == month)
                .GroupBy(a => a.ApplicationDate.Day)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new Dictionary<string, int>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                var key = day.ToString("D2"); // Format as "01", "02", etc.
                var count = applications.FirstOrDefault(a => a.Day == day)?.Count ?? 0;
                result[key] = count;
            }
            return result;
        }

        public async Task<Dictionary<string, int>> GetJobApplicationsByMonthAsync(int year)
        {
            var applications = await _dbContext.JobApplications
                .Where(a => a.ApplicationDate.Year == year)
                .GroupBy(a => a.ApplicationDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new Dictionary<string, int>();
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int month = 1; month <= 12; month++)
            {
                var count = applications.FirstOrDefault(a => a.Month == month)?.Count ?? 0;
                result[months[month - 1]] = count;
            }
            return result;
        }
    }
}




