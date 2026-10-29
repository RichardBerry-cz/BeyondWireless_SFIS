using System.Collections.Generic;
using System.Drawing;

namespace BeyondWireless_SFIS.Shared
{
    public class Constants
    {
        public const int LoginTimeoutMinutes = 15;
       

#if DEBUG
        public static Color PrimaryAppColour = Color.Red;
        public static List<string> DespatchToEmailList = new List<string>() {"Richard Berry <richard@czelectronics.co.za>"};
        public static List<string> DespatchCcEmailList = new List<string>() {"Richard Berry <richard@czelectronics.co.za>"};
                
#else
        public static  Color PrimaryAppColour = Color.SteelBlue;

        public static List<string> DespatchToEmailList = new List<string>() { "Reinier Bekker <reinier@czelectronics.co.za>", };
        public static List<string> DespatchCcEmailList = new List<string>() {
                "TCL_operation <operation.sa@tcl.com>",
                "peiyu1.chen@tcl.com",
                "Ava <yanli13.zhang@tcl.com>",
                "JACK <zhengxuechao @tcl.com>",
                "Martine <martine@tcl.com>",
                "Accounts <accounts@czelectronics.co.za>",
                "Logistics.sa@tcl.com",
                "Warehouse.sa@tcl.com",
                "Linfeng11.wang@tcl.com",
                "Xiaochuang.cai@tcl.com",
                "Taozi.huang@tcl.com",
                "Wangjian6@tcl.com",
                "Sheridon Singh <sheridon@czelectronics.co.za>",
                "Willem Swart <willem@czelectronics.co.za>",
                "Sindisiwe Bilankulu <ReceivingFA2@czelectronics.co.za>",
                "Accounts <accounts@czelectronics.co.za>"
               

        };
#endif


        private static string _applicationVersion;

        public static string ApplicationVersion
        {
            get => _applicationVersion;
            set => _applicationVersion = value;

        }

        public static string emailAddress = "NoReply@czelectronics.co.za";
        public static string emailPassword = "NCZ313cEmailR";
    }
}
