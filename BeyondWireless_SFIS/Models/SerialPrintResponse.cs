using System;

namespace BeyondWireless_SFIS.Models
{
    public class SerialPrintResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public SerialPrintRange PrintRange { get; set; } = new SerialPrintRange();

        // Helper property for quick success checking
        public bool IsSuccess => StatusCode == 0;
    }

    public class SerialPrintRange
    {
        public int PrintID { get; set; }
        public int CustomerOrderId { get; set; }
        public string Odf { get; set; } = string.Empty;
        public string Pid { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string DateCode { get; set; } = string.Empty;
        public int FirstSerial { get; set; } = -1;
        public int LastSerial { get; set; } = -1;
        public string PcbaPartNumber { get; set; } = string.Empty;
        public string FirstFullSerialNumber { get; set; } = string.Empty;
        public string LastFullSerialNumber { get; set; } = string.Empty;
        public DateTime DatePrinted { get; set; } = DateTime.MinValue;
        public int UserID { get; set; } 
        public int PrintQuantity { get; set; } = -1;

    }
}
