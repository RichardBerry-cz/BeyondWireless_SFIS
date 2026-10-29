namespace BeyondWireless_SFIS.Models
{
    public class SerialPrintOrder
    {
        public int CustomerOrderId { get; set; }
        public string Odf { get; set; }
        public string Model { get; set; }
        public int QuantityToPrint { get; set; }  
        public int QuantityOrdered { get; set; }

        public string DisplayText => $"{Odf} - {Model}";


    }
}
