using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierLegalAccessorialXref
    {
        public int CLAXControl { get; set; }
        public int LEAdminControl { get; set; }
        public int CarrierControl { get; set; }
        public int AccessorialCode { get; set; }    
        private string _Caption;
        public string Caption
        {
            get { return _Caption.Left(50); } //uses extension string method Left
            set { _Caption = value.Left(50); }
        }
        private string _EDICode;
        public string EDICode
        {
            get { return _EDICode.Left(20); } //uses extension string method Left
            set { _EDICode = value.Left(20); }
        }       
        public bool AutoApprove { get; set; }
        public bool AllowCarrierUpdates { get; set; }
        public bool AccessorialVisible { get; set; }
        public double ApproveToleranceLow { get; set; }
        public double ApproveToleranceHigh { get; set; }
        public double ApproveTolerancePerLow { get; set; }
        public double ApproveTolerancePerHigh { get; set; }
        public decimal AverageValue { get; set; }
        public bool DynamicAverageValue { get; set; }
    }
}