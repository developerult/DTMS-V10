using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    public class vtblHDM 
    {


        public string VariableCode { get; set; }
        public string CompName { get; set; }
        public int? HDMCompControl { get; set; }
        public bool? HDMActive { get; set; }
        public int? HDMLEAdminControl { get; set; }
       
        public string HDMModUser { get; set; }
        public string LegalEntity { get; set; }
        public DateTime? HDMModDate { get; set; }
        private int? _HDMVariableCode = null;
        public int? HDMVariableCode {
            get {
                if (!this._HDMVariableCode.HasValue)
                {
                    _HDMVariableCode = 1; // set default to Load Weight null or zero is not allowed
                }
                if (_HDMVariableCode.Value < 1)
                {
                    _HDMVariableCode = 1;
                }
                return _HDMVariableCode;
            }
            set {
                if (value.HasValue)
                {
                    _HDMVariableCode = value; 
                }
                else
                {
                    _HDMVariableCode = 1; // set default to Load Weight null or zero is not allowed
                }
                if (_HDMVariableCode.Value < 1)
                {
                    _HDMVariableCode = 1;
                }
            } 
        }
        public double? HDMVariable { get; set; }
        public decimal? HDMMinimum { get; set; }
        public string HDMDesc { get; set; }
        public string HDMName { get; set; }
        public int HDMCarrierControl { get; set; }
        public int HDMControl { get; set; }
        public decimal? HDMMaximum { get; set; }
        public string CarrierName { get; set; }

        private byte[] _HDMUpdated;

        public string HDMUpdated
        {
            get
            {
                if (this._HDMUpdated != null)
                {

                    return Convert.ToBase64String(this._HDMUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._HDMUpdated = null;

                }

                else
                {

                    this._HDMUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _HDMUpdated = val; }
        public byte[] getUpdated() { return _HDMUpdated; }


        public static Models.vtblHDM selectModelData(LTS.vtblHDM d)
        {
            Models.vtblHDM modelRecord = new Models.vtblHDM();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "HDMUpdated"};
                string sMsg = "";
                modelRecord = (Models.vtblHDM)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.HDMUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.vtblHDM selectLTSData(Models.vtblHDM d)
        {
            LTS.vtblHDM LTSRecord = new LTS.vtblHDM();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "HDMUpdated" };
                string sMsg = "";
                LTSRecord = (LTS.vtblHDM)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.HDMUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }

    }
}