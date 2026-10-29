using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class ComPortItem
    {
        public string Display { get; set; } // For DisplayMember
        public string Value { get; set; }   // For ValueMember

        public override string ToString() => Display; // Ensures correct display in debug views
    }
}
