using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.UserDtos
{
	public class EditAccountDto
	{
		public int Id { get; set; }	

		[Required]
		[StringLength(100)]
		[Display(Name = "Full Name")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[StringLength(255)]
		[Display(Name = "Address")]
		public string? Address { get; set; }

		[Phone]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Role")]
		[Required]
		public string Role { get; set; }

		[Display(Name = "Status")]
		public bool IsDelete { get; set; } = true;
	}
}
