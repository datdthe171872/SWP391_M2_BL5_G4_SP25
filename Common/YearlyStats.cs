using System.Collections.Generic;

namespace SWP391_M2_BL5_G4_SP25.Common
{
    public class YearlyStats
    {
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        public List<int> Years { get; set; } = new List<int>();
        public List<int> Pending { get; set; } = new List<int>();
        public List<int> Comfirm { get; set; } = new List<int>();
        public List<int> Reject { get; set; } = new List<int>();
    }
} 