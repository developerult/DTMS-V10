using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// CompCarrierModel cl s for Company and Carrier
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/22/18
    /// </remarks>
    public class CompCarrierModel
    {
        /// <summary>
        /// CompControl Property   INT
        /// </summary>
        public int CompControl { get; set; }
        /// <summary>
        /// Property CompName Property   STRING
        /// </summary>
        public string CompName { get; set; }
        /// <summary>
        /// CompControl Property   INT
        /// </summary>
        public int CarrierControl { get; set; }
        /// <summary>
        /// TPDocCCEDIControl Property   STRING
        /// </summary>
        public string CarrierName { get; set; }
        /// <summary>
        /// CCEDIControl Property   INT
        /// </summary>
        public int CCEDIControl { get; set; }
    }
}