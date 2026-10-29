using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BeyondWireless_SFIS.Models
{
    public class Unit
    {
        public int UnitId { get; set; } = -1;
        public string SerialNumber { get; set; } = string.Empty;
        public int CustomerOrderId { get; set; }
        public int StageId { get; set; }
        public bool IsFaulty { get; set; }
        public bool IsScrapped { get; set; }

        public bool IsStolen {  get; set; }
        public bool IsSold { get; set; } = false;
        public DateTime DateModified { get; set; }
        public bool IsBlocked { get; set; } 
        public string ReasonBlocked { get; set; }

        public int ProductId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public int NextStageId { get; set; }
        public string NextStageName { get; set; } = string.Empty;
        public int NextRequiredStageId { get; set; }
        public string NextRequiredStageName { get; set; } = string.Empty;
        public string Odf { get; set; } = string.Empty;
        public string SetSerialNumber { get; set; } = string.Empty;
        public bool CanRetest { get; set; } = false;

    }

}
