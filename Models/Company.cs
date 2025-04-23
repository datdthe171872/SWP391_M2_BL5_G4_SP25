using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }

        public int ClientProfileID { get; set; }
        public int CompanyCategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        public bool isDelete { get; set; }
        public ClientProfile ClientProfile { get; set; }
        public CompanyCategories CompanyCategories { get; set; }
        public List<Job> Jobs { get; set; }
        public List<CompanyReview> CompanyReviews { get; set; }
    }
}
