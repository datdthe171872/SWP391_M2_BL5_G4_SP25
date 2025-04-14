using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class JobCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public string Description { get; set; }
        public bool isDelete { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
