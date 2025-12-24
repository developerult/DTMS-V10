using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Form
    {
        public int FormControl { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public int FormFormMenuControl { get; set; }
        public int FormMenuSequence { get; set; }

        public int FormSecurityGroupXrefControl { get; set; }
        public int UserGroupsControl { get; set; }

    }
}