using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class QcInspectionItemViewModel
    {
        public int QcCheckListItemId { get; set; } 
        public int SequenceNumber { get; set; } // For ordering
        public string ItemName { get; set; }
        //public string AcceptanceCriteria { get; set; }
        public bool IsPass { get; set; } // For user to check
        public string Comment { get; set; } // Optional comment
       
    }
}
