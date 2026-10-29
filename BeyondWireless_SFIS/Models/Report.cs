using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string SPName { get; set; }
        public bool RequiresProductId { get; set; }
        public bool RequiresCustomerOrderId { get; set; }
        public bool RequiresDateRange { get; set; }
        public bool RequiresStageId { get; set; }
        public bool IsActive { get; set; }

    }
}
