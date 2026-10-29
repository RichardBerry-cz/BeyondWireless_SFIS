using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class QcInspectionResult
    {
        public int QcInspectionResultId { get; set; }
        public int QcCheckListItemId { get; set; }
        public int UnitId { get; set; }
        public bool IsPass { get; set; }
        public string Comment { get; set; }
        public bool? ReworkPass { get; set; }
        public string ReworkComment { get; set; }
    }
}
