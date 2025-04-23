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
        public string Description { get; set; }
        public string Link { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        
        public bool isDelete { get; set; }
        public User User { get; set; }
        public List<Resume> Resumes { get; set; }
    }
}
