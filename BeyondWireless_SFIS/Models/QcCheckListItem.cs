using System;

namespace BeyondWireless_SFIS.Models
{
    public class QcCheckListItem
    {
        public int QcCheckListItemId { get; set; }
        public int QcInspectionTypeId { get; set; }
        public string InspectionTypeName { get; set; }
        public int SequenceNumber { get; set; }
        public string ItemName { get; set; }
        public string AcceptanceCriteria { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedByName { get; set; }
        public int ModifiedBy { get; set; }
    }
}
