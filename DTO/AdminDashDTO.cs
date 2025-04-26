namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class AdminDashDTO
    {
        public int JobCount { get; set; }
        public int CompanyCount { get; set; }
        public Dictionary<string, int> JobApplicationsWeekly { get; set; }
        public Dictionary<string, int> JobApplicationsMonthly { get; set; }
        public Dictionary<string, int> JobApplicationsYearly { get; set; }
    }
}
