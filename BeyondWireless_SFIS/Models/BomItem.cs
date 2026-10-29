using System;

namespace BeyondWireless_SFIS.Models
{
    public class BomItem
    {
        public int ProductId { get; set; }
        public string ComponentPartNumber { get; set; } = string.Empty;
        public string ComponentDescription { get; set; } = string.Empty;
        public decimal UsageQuantity { get; set; }
        public string Position { get; set; } = string.Empty;
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        public string DisplayText => $"{Position} - {ComponentDescription}";
    }

}
