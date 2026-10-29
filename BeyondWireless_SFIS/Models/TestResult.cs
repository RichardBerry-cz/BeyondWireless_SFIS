using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class TestResult
    {
        public int TestResultId { get; set; }
        public int TestTypeId {  get; set; }
        public string TestTypeName { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string TestDate { get; set; }
        public int StageId { get; set; }
        public bool IsPass { get; set; }
        public string DefectDescription { get; set; } = string.Empty; //Not used anymore Now in Defects List
        public int DefectId { get; set; }  //Not used anymore Now in Defects List
        public string Position { get; set; } = string.Empty;  //Not used anymore Now in Defects List
        public int UserId { get; set; }
        public bool IsRepaired { get; set; } = false;

        public List<UnitDefect> Defects { get; set; } = new List<UnitDefect>();
    }

    public class UnitDefect
    {
        public int TestResultId { get; set; } = -1;
        public int TestResultDetailId { get; set; } = -1;
        public int DefectId { get; set; } = -1;
        public string DefectName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsRepaired { get; set; } = false;
    }

}
