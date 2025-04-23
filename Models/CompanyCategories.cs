using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class CompanyCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool isDelete { get; set; }
        public List<Company> Companies { get; set; }
    }
}
