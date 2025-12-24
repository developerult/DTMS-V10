
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class ApiError
    {
        public string HttpStatusCode { get; set; }
        public string HttpMessage { get; set; }
        public string ErrorMessage { get; set; }

        public List<Message> Errors { get; set; }
        public string SupportReferenceId { get; set; }
    }

    public class Message
    {
        public string Severity { get; set; }
        public string message { get; set; }
        public string diagnostic { get; set; }
        public string Source { get; set; }
    }

}