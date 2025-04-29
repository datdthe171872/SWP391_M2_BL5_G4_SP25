using Microsoft.AspNetCore.Mvc;

namespace SWP391_M2_BL5_G4_SP25.DTO.JobDtos
{
	public class JobSearchDto
	{
		public string? SearchTerm { get; set; }
		public string? Status { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
