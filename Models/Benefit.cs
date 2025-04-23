using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Benefit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenefitID { get; set; }
        public int JobID { get; set; }

        [Required]
        public string Content { get; set; }
        public bool IsDelete { get; set; }
        public Job Job { get; set; }
    }
}
