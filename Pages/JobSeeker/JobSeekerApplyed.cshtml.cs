using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SWP391_M2_BL5_G4_SP25.Pages.JobSeeker
{
    public class JobSeekerApplyedModel : PageModel
    {
        private readonly MyDBContext _context;

        public JobSeekerApplyedModel(MyDBContext context)
        {
            _context = context;
        }

        public List<AppliedJobViewModel> AppliedJobs { get; set; }
        public string SearchTerm { get; set; }
        public string CategoryFilter { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(string searchTerm, string categoryFilter, int pageIndex = 1)
        {
            SearchTerm = searchTerm;
            CategoryFilter = categoryFilter;
            PageIndex = pageIndex;

            // Lấy UserID của JobSeeker hiện tại
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Truy vấn danh sách công việc đã ứng tuyển
            var query = from ja in _context.JobApplications
                        join j in _context.Jobs on ja.JobID equals j.JobID
                        join c in _context.Companies on j.CompanyID equals c.CompanyID
                        where ja.UserID == userId && !ja.isDelete && !j.isDelete && !c.isDelete
                        select new AppliedJobViewModel
                        {
                            JobApplicationID = ja.JobApplicationID,
                            JobTitle = j.Title,
                            CompanyName = c.CompanyName,
                            CompanyLogo = c.Image,
                            Location = j.Location,
                            JobType = j.JobType,
                            ApplicationDate = ja.ApplicationDate,
                            Status = ja.Status,
                            SkillsRequired = j.SkillsRequired.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                        };

            // Áp dụng tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.JobTitle.Contains(searchTerm) || x.CompanyName.Contains(searchTerm));
            }

            // Áp dụng lọc theo loại công việc
            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Category")
            {
                query = query.Where(x => x.JobType == categoryFilter);
            }

            // Tính tổng số trang
            var totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            // Lấy dữ liệu phân trang
            AppliedJobs = await query
                .OrderByDescending(x => x.ApplicationDate)
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }

    public class AppliedJobViewModel
    {
        public int JobApplicationID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public List<string> SkillsRequired { get; set; }
    }
}

