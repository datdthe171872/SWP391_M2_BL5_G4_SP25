using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class JobApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobApplicationID { get; set; }

        public int JobID { get; set; }
        public int UserID { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string CoverLetter { get; set; }
        public string CVFile { get; set; }
        public string Status { get; set; } = "Pending"; // "Pending", "Reviewed", "Accepted", "Rejected"
        public bool isDelete { get; set; }
        public Job Job { get; set; }
        public User User { get; set; }
    }
}
