using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductShortCode { get; set; } = string.Empty;
        public int InspectionLevel { get; set; }
        public int BuildTypeId { get; set; }
        public string BuildTypeName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public bool HasSerialsPrinted { get; set; } = false;

        public string DisplayText => $"{PartNumber} - {ProductShortCode}";

        public string ExtendedDisplayText => $"{PartNumber} - {ProductName}";


    }

}
