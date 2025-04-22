using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class RegisterInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool RoleType { get; set; }
        [Required]
        public string Fullname { get; set; }
        public string Password { get; set; }
    }
}
