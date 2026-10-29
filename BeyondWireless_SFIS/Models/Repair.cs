using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class Repair
    {
        public int RepairId { get; set; }
        public int UnitId { get; set; }
        public int TestResultId { get; set; }
        public int TestResultDetailId { get; set; }
        public int DefectId { get; set; }
        public string RepairDescription { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int RepairTypeId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime RepairDate { get; set; }
        public int UserId { get; set; }
    }

}
