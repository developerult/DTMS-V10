using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Email
    {
        public string emailTo { get; set; }
        public string emailFrom { get; set; }
        public string emailCc { get; set; }
        public string emailBcc { get; set; }
        public string emailSubject { get; set; }
        public string emailBody { get; set; }

    }
}