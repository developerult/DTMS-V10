using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LanePreferredCarrier
    {
        public Lane Lane { get; set; }      
        public bool LLTCAllowAutoAssignment { get; set; }
        public int LLTCCarrierContControl { get; set; }
        public int LLTCCarrierControl { get; set; }  
        public LimitLaneToCarrierDetails[] Details { get; set; }
        public string LLTCCarrierName { get; set; }
        public int LLTCCarrierNumber { get; set; }
        public int LLTCControl { get; set; }
        public bool LLTCIgnoreTariff { get; set; }
        public int LLTCLaneControl { get; set; }
        public decimal LLTCMaxAllowedCost { get; set; }
        public int LLTCMaxCases { get; set; }
        public int LLTCMaxCube { get; set; }
        public double LLTCMaxPL { get; set; }
        public double LLTCMaxWgt { get; set; }
        public decimal LLTCMinAllowedCost { get; set; }
        public DateTime? LLTCModDate { get; set; }
        public int LLTCModeTypeControl { get; set; }
        public string LLTCModUser { get; set; }
        public bool LLTCSActive { get; set; }
        public int LLTCSequenceNumber { get; set; }
        public int LLTCTariffControl { get; set; }
        public string LLTCTariffEquip { get; set; }
        public string LLTCTariffName { get; set; }
        public int LLTCTempType { get; set; }

        private byte[] _LimitLaneToCarrierUpdated;

        public string LLTCModeTypeName
        {
            get
            {
                if(this.LLTCModeTypeControl != 0)
                {
                    if (this.LLTCModeTypeControl==1)
                    {
                        return "Air";
                    }
                    else if (this.LLTCModeTypeControl == 2)
                    {
                        return "Rail";
                    }
                    else if (this.LLTCModeTypeControl == 3)
                    {
                        return "Road";
                    }
                    else if (this.LLTCModeTypeControl == 4)
                    {
                        return "Sea";
                    }
                    else if (this.LLTCModeTypeControl == 5)
                    {
                        return "Service";
                    }
                }
                return string.Empty;
            }

                set { } }

        public string LLTCTempTypeName
        {
            get
            {
                if (this.LLTCTempType == 1)
                {
                    return "Dry";
                }
                else if (this.LLTCTempType == 2)
                {
                    return "Frozen";
                }
                else if (this.LLTCTempType == 3)
                {
                    return "Cooler";
                }
                else if (this.LLTCTempType == 4)
                {
                    return "Any";
                }

                return string.Empty;
            }
            set { }
        }
        public string LimitLaneToCarrierUpdated
        {
            get
            {
                if (this._LimitLaneToCarrierUpdated != null)
                {

                    return Convert.ToBase64String(this._LimitLaneToCarrierUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LimitLaneToCarrierUpdated = null;

                }

                else
                {

                    this._LimitLaneToCarrierUpdated = Convert.FromBase64String(value);

                }

            }
        }
       
        public string CarrierContact800 { get; set; }

        public bool CarrierContactDefault { get; set; }
       
        public string CarrierContactEMail { get; set; }
       
        public string CarrierContactFax { get; set; }
      
        public string CarrierContactPhone { get; set; }

        public string CarrierContName { get; set; }
    
        public string CarrierContPhoneExt { get; set; }

        public bool CompRestrictCarrierSelection { get; set; }   
    
        public bool CompWarnOnRestrictedCarrierSelection { get; set; }
        public void setUpdated(byte[] val) { _LimitLaneToCarrierUpdated = val; }
        public byte[] getUpdated() { return _LimitLaneToCarrierUpdated; }
    }
}