namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class JobDetailDTO
    {
        public int JobID { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public DateTime PostDate { get; set; }
        public int Salary { get; set; }
        public string Description { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public string Skills { get; set; }
        public string Benefits { get; set; }
        public int Vacancy { get; set; }
        public string Exp { get; set; }
        public DateTime Deadline { get; set; }
        public string SkillsRequired { get; set; }
        public string Gender { get; set; } = "Both";
        public string CategoryName { get; set; }
        public string CompanyLogo { get; set; }
    }
}
