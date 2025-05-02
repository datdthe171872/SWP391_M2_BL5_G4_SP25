namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class DashboardDTO {
        public int OpenJobsCount { get; set; }
        public int TotalApplications { get; set; }
        public int ShortlistedCount { get; set; }
        public int TotalJobViews { get; set; }
        public IList<JobDto> Jobs { get; set; }
    }
}
