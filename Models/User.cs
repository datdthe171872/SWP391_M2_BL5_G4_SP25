using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class User : IdentityUser<int>
    {

       
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool isDelete { get; set; }
        public ClientProfile ClientProfile { get; set; }
        public JobSeekerProfile JobSeekerProfile { get; set; }
        public List<JobApplication> JobApplications { get; set; }
        public List<CompanyReview> CompanyReviews { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
