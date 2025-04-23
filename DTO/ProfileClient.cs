using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class ProfileClient
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Img { get; set; }
        public string Link { get; set; }

        public DateTime dob { get; set; }

        public string Desciption { get; set; }
    }
}
