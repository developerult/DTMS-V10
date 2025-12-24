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
    /// Model for Value Lookup Data for Workflow Setup page links to tblParProcessOption
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.3.006 on 10/20/2022
    /// </remarks>
    public class tblParProcOptValItem
    {

        public int ParProcOptVIControl { get; set; }
        public int ParProcOptVIParProcOptControl { get; set; }
        public string ParProcOptVIName { get; set; }
        public string ParProcOptVIDesc { get; set; }
        public string ParProcOptVIModUser { get; set; }
        public DateTime? ParProcOptVIModDate { get; set; }

        private byte[] _ParProcOptVIUpdated;

        /// <summary>
        /// ParProcOptVIUpdated should be bound to UI, _ParProcOptVIUpdated is only bound on the controller
        /// </summary>
        public string ParProcOptVIUpdated
        {
            get
            {
                if (this._ParProcOptVIUpdated != null) { return Convert.ToBase64String(this._ParProcOptVIUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ParProcOptVIUpdated = null; } else { this._ParProcOptVIUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ParProcOptVIUpdated = val; }
        public byte[] getUpdated() { return _ParProcOptVIUpdated; }


        public static Models.tblParProcOptValItem selectModelData(LTS.tblParProcOptValItem d)
        {
            Models.tblParProcOptValItem modelRecord = new Models.tblParProcOptValItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ParProcOptVIUpdated", "tblParProcessOption" };
                string sMsg = "";
                modelRecord = (Models.tblParProcOptValItem)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ParProcOptVIUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblParProcOptValItem selectLTSData(Models.tblParProcOptValItem d)
        {
            LTS.tblParProcOptValItem LTSRecord = new LTS.tblParProcOptValItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> {"ParProcOptVIUpdated", "tblParProcessOption" };
                string sMsg = "";
                LTSRecord = (LTS.tblParProcOptValItem)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.ParProcOptVIUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }

    }
}