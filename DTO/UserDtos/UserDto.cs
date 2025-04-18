using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.DTO.UserDtos
{
	public class UserDto
	{
		public User User { get; set; }
		public List<string> Roles { get; set; }
	}
}
