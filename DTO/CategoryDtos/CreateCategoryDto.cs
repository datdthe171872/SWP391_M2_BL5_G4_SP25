using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos
{
	public class CreateCategoryDto
	{
		[Required]
		[StringLength(50)]
		[Display(Name = "Name")]
		public string CategoryName { get; set; }
		[Required]
		public string Description { get; set; }
	}
}
