using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
	public class CreateCompanyDto
	{
		[Required]
		[StringLength(100)]
		public string CompanyName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public string? Description { get; set; }

		public string? Link { get; set; }

		[StringLength(255)]
		public string? Location { get; set; }

		public IFormFile? ImageFile { get; set; }
	}
}
