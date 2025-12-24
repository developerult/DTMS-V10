using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Carrier
    {

        public int Control { get; set; }

        public int? Number { get; set; }

        public string Name { get; set; }

        public string CarrierSCAC { get; set; }
        public DateTime? CarrierModDate { get; set; }
        public string CarrierModUser { get; set; }

    }
}