namespace SWP391_M2_BL5_G4_SP25.Models.Enum
{
    public static class EnumExp
    {
        public const string NoExperience = "NO EXP";          // Không yêu cầu kinh nghiệm
        public const string LessThan1Year = "LESS THAN 1 YEARS";         // Dưới 1 năm
        public const string From1To2Years = "FROM 1 TO 2 YEARS";         // 1 đến 2 năm
        public const string From2To5Years = "FROM 2 TO 5 YEARS";         // 2 đến 5 năm
        public const string MoreThan5Years = "MORE THAN 5 YEARS";        // Trên 5 năm
    }
    public enum EnumStatus
    {
        Open = 0,       // Đang tuyển
        Closed = 1,     // Đã đóng
        Pending = 2     // Chờ duyệt hoặc đang xử lý
    }
}
