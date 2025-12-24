using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    //Modified by RHR for v - 8.4.0.003 on 11 / 2 / 2021
    public class GenericResult
    {
        public int Control { get; set; }
        public int ErrNumber { get; set; }
        public string RetMsg { get; set; }

        public DateTime? dtField { get; set; }

        public string strField { get; set; }
        public string strField2 { get; set; }
        public string strField3 { get; set; }
        public string strField4 { get; set; }

        public bool blnField { get; set; }
        public bool blnField1 { get; set; }
        public bool blnField2 { get; set; }

        public int intField1 { get; set; }
        public int intField2 { get; set; }
        public int intField3 { get; set; }

        public decimal decField1 { get; set; }

        public long longField1 { get; set; }

        public int[] intArray { get; set; }

        public string[] strArray { get; set; }
        public string sToken { get; set; } //Modified by RHR for v - 8.4.0.003 on 11 / 2 / 2021

        public Ngl.FreightMaster.Data.DataTransferObjects.NGLMessage[] log { get; set; }

    }
}