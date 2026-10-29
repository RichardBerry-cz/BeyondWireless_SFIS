using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class Defect
    {
        public int DefectId { get; set; }
        public int TestTypeId { get; set; }
        public string DefectName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DefectGroupId { get; set; } = -1;
    }
}
