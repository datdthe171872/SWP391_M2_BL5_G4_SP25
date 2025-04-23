using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Resume
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResumeID { get; set; }
        public int JobSeekerProfileID { get; set; }
        [Required]
        public string Link { get; set; }
        public bool IsDelete {  get; set; }
        public JobSeekerProfile JobSeekerProfile { get; set; }
    }
}
