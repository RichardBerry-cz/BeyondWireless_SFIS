using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class UnitHistory
    {
        public int UnitHistoryId { get; set; }
        public int UnitId { get; set; }
        public int StageId { get; set; }
        public DateTime MovementDate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

}
