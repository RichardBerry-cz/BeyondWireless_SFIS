using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class User
    {
        public int UserId { get; set; } = -1;
        public string FullName { get; set; }
        public int AuthorityLevel { get; set; }
        public string RFIDSerial { get; set; }
        public bool IsActive { get; set; }
    }
}
