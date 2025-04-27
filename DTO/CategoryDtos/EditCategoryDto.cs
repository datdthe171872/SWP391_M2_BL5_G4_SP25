using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos
{
	public class EditCategoryDto
	{
		public int JobCategoryID { get; set; }
		[Required]
		[StringLength(50)]
		[Display(Name = "Name")]
		public string CategoryName { get; set; }
		[Required]
		public string Description { get; set; }
	}
}
