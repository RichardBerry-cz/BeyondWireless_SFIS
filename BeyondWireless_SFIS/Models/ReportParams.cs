using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class ReportParams
    {
        public string SPName { get; set; }
        public int? CustomerOrderId { get; set; }
        public int? ProductId { get; set; }
        public int? StageId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
