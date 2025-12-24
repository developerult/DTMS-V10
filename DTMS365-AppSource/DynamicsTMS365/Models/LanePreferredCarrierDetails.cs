using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LanePreferredCarrierDetails
    {
        public int LLTCDControl { get; set; }
        public decimal MaxAllowedCost { get; set; }
        public int MaxCases { get; set; }
        public int MaxCube { get; set; }
        public double MaxPL { get; set; }
        public double MaxWgt { get; set; }
        public decimal MinAllowedCost { get; set; }
        public int ModeTypeControl { get; set; }
        public int TempType { get; set; }
        public double LLTCMaxWgt {            get {                return this.MaxWgt;                    }            set { } }
        public int LLTCMaxCases { get { return this.MaxCases; } set { } }
        public int LLTCMaxCube { get { return this.MaxCube; } set { } }
        public double LLTCMaxPL { get { return this.MaxPL; } set { } }
        public decimal LLTCMinAllowedCost { get { return this.MinAllowedCost; } set { } }
        public decimal LLTCMaxAllowedCost { get { return this.MaxAllowedCost; } set { } }
        public string LLTCModeTypeName
        {
            get
            {
                if (this.ModeTypeControl != 0)
                {
                    if (this.ModeTypeControl == 1)
                    {
                        return "Air";
                    }
                    else if (this.ModeTypeControl == 2)
                    {
                        return "Rail";
                    }
                    else if (this.ModeTypeControl == 3)
                    {
                        return "Road";
                    }
                    else if (this.ModeTypeControl == 4)
                    {
                        return "Sea";
                    }
                    else if (this.ModeTypeControl == 5)
                    {
                        return "Service";
                    }
                }
                return string.Empty;
            }

            set { }
        }

        public string LLTCTempTypeName
        {
            get
            {
                if (this.TempType == 1)
                {
                    return "Dry";
                }
                else if (this.TempType == 2)
                {
                    return "Frozen";
                }
                else if (this.TempType == 3)
                {
                    return "Cooler";
                }
                else if (this.TempType == 4)
                {
                    return "Any";
                }
                
                return string.Empty;
            }set { }
        }


    }
}