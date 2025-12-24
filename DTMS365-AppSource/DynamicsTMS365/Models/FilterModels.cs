using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class AuditFilters
    {
        public string CarrierDDLValue { get; set; }
        public int APAuditFltrsDDLValue { get; set; }
        public DateTime? APReceivedDateFrom { get; set; }
        public DateTime? APReceivedDateTo { get; set; }
    }

    public class OrderPreviewFilters
    {
        public bool NatAcctChecked { get; set; }
        public bool CompChecked { get; set; }
        public string NatAcctDDLValue { get; set; }
        public string CompDDLValue { get; set; }
        public int FrtTypDDLValue { get; set; }
    }

    public class SelectContactFilters
    {
        public int ContactType { get; set; }
        public int Control { get; set; }
    }

}