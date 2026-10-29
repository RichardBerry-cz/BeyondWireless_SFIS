using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class ScrapItem
    {
        public int ScrapItemId { get; set; }
        public int OrderId { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public DateTime DateScrapped { get; set; }
        public int UserId { get; set; }
    }

}
