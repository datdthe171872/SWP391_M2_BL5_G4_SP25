using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Skills
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillID { get; set; }

        public int JobID { get; set; }

        [Required]
        [StringLength(100)]
        public string SkillName { get; set; }

        public string ProficiencyLevel { get; set; } // "Beginner", "Intermediate", "Advanced"
        public bool isDelete { get; set; }
        public Job Job { get; set; }
    }
}
