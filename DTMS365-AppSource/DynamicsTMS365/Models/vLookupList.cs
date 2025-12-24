using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Generic Lookup List Model
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.0 on 2/21/2017
    /// </remarks>
    public class vLookupList
    {
        public int Control { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class vLookupListCriteria
    {
        public int id { get; set; }
        public int sortKey { get; set; }
         public  object criteria { get; set; }
    }
}