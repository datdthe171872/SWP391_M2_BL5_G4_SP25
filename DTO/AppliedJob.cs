using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.DTO
{
    public class AppliedJob
    {
        public int Id { get; set; }
        public Company Company {  get; set; }
        public JobCategory Category {  get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }
        public string Location {  get; set; }
        public string Exp {  get; set; }
        public int Salary {  get; set; }
        public string Skill {  get; set; }
        public string Jobtype {  get; set; }
        public DateTime PostedDate {  get; set; }
        public string Status {  get; set; }
        public int AppliedUser {  get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<Benefit> Benefits { get; set; }
        public List<Responsibility> Responsibilities { get; set;}
    }
}
