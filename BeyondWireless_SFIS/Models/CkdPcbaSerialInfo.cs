namespace BeyondWireless_SFIS.Models
{
    public class CkdPcbaSerialInfo
    {
        public bool IsValid { get; set; }
        public string SerialNumber { get; set; }
        public string PartNumber { get; set; }
        public string Pid { get; set; }
        public string SerialPart { get; set; }
        public string DateCode { get; set; } // YYWW format
        public string Odf { get; set; }
        public string Model { get; set; }
    }
}
