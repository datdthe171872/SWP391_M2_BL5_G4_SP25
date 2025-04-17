using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.UserDtos
{
	public class CreateAccountDto
	{
		[Required]
		[StringLength(100)]
		[Display(Name = "Full Name")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		//[Required]
		//[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		//[DataType(DataType.Password)]
		//[Display(Name = "Password")]
		//public string Password { get; set; }

		//[DataType(DataType.Password)]
		//[Display(Name = "Confirm password")]
		//[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		//public string ConfirmPassword { get; set; }

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
		public bool IsActive { get; set; } = true;
	}
}
