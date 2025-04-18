using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class JobSeekerProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobSeekerProfileID { get; set; }

        public int UserID { get; set; }

        [StringLength(255)]
        public string Logo { get; set; }
        public string CV { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; } // lieasence
        public bool isDelete { get; set; }
        public User User { get; set; }
    }
}
