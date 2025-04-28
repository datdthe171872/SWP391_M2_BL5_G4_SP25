using SWP391_M2_BL5_G4_SP25.Models;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.JobDtos
{
	public class JobDto
	{
		public int JobID { get; set; }
		public int CompanyID { get; set; }
		public int? JobCategoryID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Exp { get; set; }
		public int Salary { get; set; }
		public string SkillsRequired { get; set; }
		public string JobType { get; set; } // "FullTime", "PartTime", "Contract", "Internship"
		public DateTime PostDate { get; set; } = DateTime.Now;
		public string Status { get; set; } = "OPEN";
		public bool isDelete { get; set; }
		public Company Company { get; set; }
		public JobCategory JobCategory { get; set; }
	}
}
