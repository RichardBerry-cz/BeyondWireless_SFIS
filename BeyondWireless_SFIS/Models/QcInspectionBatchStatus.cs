using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class QcInspectionBatchStatus
    {
        public int CustomerOrderId { get; set; }
        public string Odf { get; set; } = string.Empty;
        public int InspectionLevel { get; set; }
        public int BatchSize { get; set; }
        public int SampleSize { get; set; } 
        public int SampledCount { get; set; }
        public int DefectCount { get; set; }
        public int CriticalDefectLimit { get; set; }
        public int MajorDefectLimit { get; set; }
        public int MinorDefectLimit { get; set; }
 
    }
}
