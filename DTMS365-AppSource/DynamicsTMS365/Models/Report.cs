using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Report
    {
        public int ReportControl { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public int ReportReportMenuControl { get; set; }
        public int ReportMenuSequence { get; set; }

        public int ReportSecurityGroupXrefControl { get; set; }
        public int UserGroupsControl { get; set; }

    }
}