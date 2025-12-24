using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Model for Text Lookup Data for Workflow Setup page links to tblParProcOptTxtItem
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022
    /// </remarks>
    public class tblParProcOptTxtItem
    {

        public int ParProcOptTIControl { get; set; }
        public int ParProcOptTIParProcOptControl { get; set; }
        public string ParProcOptTIName { get; set; }
        public string ParProcOptTIDesc { get; set; }
        public string ParProcOptTIModUser { get; set; }
        public DateTime? ParProcOptTIModDate { get; set; }

        private byte[] _ParProcOptTIUpdated;

        /// <summary>
        /// ParProcOptTIUpdated should be bound to UI, _ParProcOptTIUpdated is only bound on the controller
        /// </summary>
        public string ParProcOptTIUpdated
        {
            get
            {
                if (this._ParProcOptTIUpdated != null) { return Convert.ToBase64String(this._ParProcOptTIUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ParProcOptTIUpdated = null; } else { this._ParProcOptTIUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ParProcOptTIUpdated = val; }
        public byte[] getUpdated() { return _ParProcOptTIUpdated; }


        public static Models.tblParProcOptTxtItem selectModelData(LTS.tblParProcOptTxtItem d)
        {
            Models.tblParProcOptTxtItem modelRecord = new Models.tblParProcOptTxtItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ParProcOptTIUpdated", "tblParProcessOption" };
                string sMsg = "";
                modelRecord = (Models.tblParProcOptTxtItem)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ParProcOptTIUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblParProcOptTxtItem selectLTSData(Models.tblParProcOptTxtItem d)
        {
            LTS.tblParProcOptTxtItem LTSRecord = new LTS.tblParProcOptTxtItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ParProcOptTIUpdated", "tblParProcessOption" };
                string sMsg = "";
                LTSRecord = (LTS.tblParProcOptTxtItem)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.ParProcOptTIUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }

    }
}