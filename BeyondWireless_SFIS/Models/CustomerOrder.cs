using System;

namespace BeyondWireless_SFIS.Models
{
    public class CustomerOrder
    {
        public int CustomerOrderId { get; set; }
        public string CustomerOrderNumber { get; set; } = string.Empty;
        public int CustomerOrderStatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int BuildTypeId { get; set; }
        public string Odf { get; set; } = string.Empty;
        public string PanelOdf { get; set; } = string.Empty;
        public int QuantityOrdered { get; set; }
        public int QuantityDespatched { get; set; }
        public int PcbasDespatched { get; set; } = 0;
        public DateTime DateLoaded { get; set; }
        public string BCAssemblyOrder { get; set; } = string.Empty;
        public DateTime DateModified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool LabelPrintingActive { get; set; } = false;
        public DateTime ExpectedKitArrivalDate { get; set; }
        public DateTime ExpectedPanelArrivalDate { get; set; }

        public string BuildType { get; set; } = string.Empty;        
        public string PartNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string PID { get; set; } = string.Empty;

        public string DisplayText => $"{Odf} - {ProductName} ({QuantityOrdered})";
    }

}
