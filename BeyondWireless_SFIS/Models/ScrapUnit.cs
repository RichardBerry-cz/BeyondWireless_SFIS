using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class ScrapUnit
    {
        public int ScrapUnitId { get; set; }
        public int UnitId { get; set; }
        public DateTime DateScrapped { get; set; }
        public int UserId { get; set; }
    }

}
