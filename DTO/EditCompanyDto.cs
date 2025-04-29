using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
	public class EditCompanyDto
	{
		public int CompanyID { get; set; }

		[Required(ErrorMessage = "Company name is required")]
		[StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
		[Display(Name = "Company Name")]
		public string CompanyName { get; set; }

		[EmailAddress(ErrorMessage = "Invalid email address")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Website Link")]
		public string Link { get; set; }

		[Display(Name = "Location")]
		[StringLength(255)]
		public string Location { get; set; }

		[Display(Name = "Company Logo")]
		[ValidateNever]
		public IFormFile ImageFile { get; set; }

		public string ExistingImagePath { get; set; }
	}
}
