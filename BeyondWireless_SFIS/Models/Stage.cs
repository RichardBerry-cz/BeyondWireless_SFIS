using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondWireless_SFIS.Models
{
    public class Stage
    {
        public int StageId { get; set; }
        public int BuildTypeId { get; set; }
        public int SequenceNumber { get; set; }
        public string StageName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int TestTypeId { get; set; }

        public string DisplayText => $"{StageName} {GetBuildTypeSuffix(BuildTypeId)}";

        private string GetBuildTypeSuffix(int buildTypeId)
        {
           switch (buildTypeId)
            {
                case 1:
                    return "(CKD)";
   
                case 2:
                    return "(SKD)";
                case 3:
                    return "(CBU)";
                default:
                    return string.Empty;
            };
        }
    }

}
