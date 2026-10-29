namespace BeyondWireless_SFIS.Shared
{
    public  class ConnectionStrings
    {

        //public const string bwDb = @"Server=Rberry-LAPTOP\SS2022D;Database=TCL;User Id=rb; Password=rb;Connect Timeout=10";
        //public const string bwDb = @"Server=Kishan-LT\MSSQLSERVER01;Database=TCL;User Id=rb; Password=rb;TrustServerCertificate=true;Connect Timeout=10";
#if DEBUG

        public const string bwDb = "Server=192.168.16.164,1433;Database=BeyondWireless;User Id=Warehouse; Password=Password123#;Connect Timeout=10";
        public const string referenceDb = "Server=192.168.16.164,1433;Database=Reference;User Id=Warehouse; Password=Password123#;Connect Timeout=10";
#else
        public const string bwDb = "Server=192.168.16.164,1433;Database=BeyondWireless;User Id=Warehouse; Password=Password123#;Connect Timeout=10";
        public const string referenceDb = "Server=192.168.16.164,1433;Database=Reference;User Id=Warehouse; Password=Password123#;Connect Timeout=10";
#endif
    }
}
