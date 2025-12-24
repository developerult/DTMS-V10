using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Overload for Kendo DropDownList when Control must be text not an integer
    /// Description is optional
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.2 on 09/25/2018 
    /// </remarks>
    public class vLookupText
    {
        //overload for Kendo DropDownList when Control must be text not an integer
        //Description is optional
        public string Name { get; set; }
        public string Control { get; set; }

        public string Description { get; set; }
    }
}