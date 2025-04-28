using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO.CategoryDtos
{
	public class CategoryDto
	{
		public int JobCategoryID { get; set; }

		public string CategoryName { get; set; }

		public string Description { get; set; }
		public bool isDelete { get; set; }
	}
}
