using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Responsibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponsibilityID { get; set; }
        public int JobID { get; set; }
            
        [Required]
        public string Content { get; set; }
        public bool IsDelete { get; set; }
        public Job Job { get; set; }

    }
}
