using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class APMassEntryMsg
    {
        public int APMMsgControl { get; set; }
        public int APMMsgAPControl { get; set; }
        public int APMMsgLoadStatusControl { get; set; }
        public string APMMsgMessage { get; set; }
        public string APMMsgSource { get; set; }
        public DateTime? APMMsgCreateDate { get; set; }
        public bool APMMsgResolved { get; set; }
        public string APMMsgModUser { get; set; }
        public DateTime? APMMsgModDate { get; set; }
        public string LoadStatusDesc { get; set; }
    }
}