using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class ClientProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientProfileID { get; set; }

        public int UserID { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Logo { get; set; }
        public bool isDelete { get; set; }

        public User User { get; set; }
        public Company Company { get; set; }
    }
}
