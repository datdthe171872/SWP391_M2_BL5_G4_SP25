using Microsoft.AspNetCore.Builder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobID { get; set; }

        public int CompanyID { get; set; }
        public int? JobCategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(100)]
        public string Location { get; set; }
        public string Exp { get; set; }
        public int Salary { get; set; }
        [StringLength(200)]
        public string SkillsRequired { get; set; }
        [Required]
        public string JobType { get; set; } // "FullTime", "PartTime", "Contract", "Internship"

        public DateTime PostDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "OPEN"; // "Open", "Closed","Pending"
        public bool isDelete { get; set; }
        public Company Company { get; set; }
        public JobCategory JobCategory { get; set; }
        public List<JobApplication> JobApplications { get; set; }
        public List<Responsibility> Responsibilities { get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<Benefit> Benefits { get; set; }
    }
}
