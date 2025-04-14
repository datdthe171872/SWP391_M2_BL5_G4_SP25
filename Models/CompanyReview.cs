using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class CompanyReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }

        public int CompanyID { get; set; }
        public int UserID { get; set; }
        
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public bool isDelete { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
    }
}
