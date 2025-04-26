using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Requirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequirementID { get; set; }
        public int JobID { get; set; }

        [Required]
        public string Content { get; set; }
        public bool IsDelete { get; set; }
        public Job Job { get; set; }
    }
}
