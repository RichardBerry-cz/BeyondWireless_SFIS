namespace BeyondWireless_SFIS.Models
{
    public class ModuleSerialInfo
    {
        public string PartNumber { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string SerialPart { get; set; } = string.Empty;
        public string Odf { get; set; } = string.Empty;

        public string Pid { get; set; } = string.Empty;

        public bool IsValid { get; set; } = false;
    }
}
