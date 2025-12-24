using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class ZipCodes
    {
        public int ZipCodeControl { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
    }
}