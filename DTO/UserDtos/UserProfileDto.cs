using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.UserDtos
{
	public class UserProfileDto
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Fullname is required")]
		[Display(Name = "Full Name")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Email not correct")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Address")]
		public string Address { get; set; }

		[Display(Name = "Phone Number")]
		[Phone(ErrorMessage = "Phone Number is not correct")]
		public string PhoneNumber { get; set; }

	}
}
