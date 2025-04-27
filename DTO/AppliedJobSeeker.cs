using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class AppliedJobSeeker
    {
        public int JobApplicationId { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime AppliedDate { get; set; }
        public string CV {  get; set; }
        public string Status { get; set; }
        public bool FileExist { get; set; }
    }
}
