using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Procedure
    {
        public int ProcedureControl { get; set; }
        public string ProcedureName { get; set; }
        public string ProcedureDescription { get; set; }
        public bool ProcedureHasAlert { get; set; }

        public int ProcedureSecurityGroupXrefControl { get; set; }
        public int UserGroupsControl { get; set; }

    }
}