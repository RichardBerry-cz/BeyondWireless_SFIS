namespace BeyondWireless_SFIS.Models
{
    public class RepairType
    {
        public int RepairTypeId { get; set; }
        public int TestTypeId { get; set; }
        public string RepairDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
