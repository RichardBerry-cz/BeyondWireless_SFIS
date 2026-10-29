namespace BeyondWireless_SFIS.Models
{
    public class TestResultDefectViewModel
    {
        public int TestResultId { get; set; }
        public int TestResultDetailId { get; set; }
        public string TestDate { get; set; } = string.Empty;
        public string TestTypeName { get; set; } = string.Empty;
        public int DefectId { get; set; } = -1;
        public string DefectName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int TestTypeId { get; set; }
        public bool IsRepaired { get; set; }
    }
}
