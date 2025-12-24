using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{
    public class tblUserSecurity
    {


        public int ProcedureControl { get; set; }

        public string ProcedureName { get; set; }

        public string ProcedureDescription { get; set; }

        public bool ProcedureHasAlert { get; set; }

        //public byte[] ProcedureUpdated { get; set; }

        public int ProcedureSecurityXrefControl { get; set; }

        public int ProcedureSecurityGroupXrefControl { get; set; }

        public bool ProcedureUserOverrideGroup { get; set; }

        public int ProcAlertUserXrefShowPopup { get; set; }

        public int ProcAlertUserXrefSendEmail { get; set; }

        public string strPopup { get; set; }

        public string strEmail { get; set; }


        //public Binary ProcedureUpdated { get; set; }


        private byte[] _ProcedureUpdated;
        /// <summary>
        /// ProcedureUpdated should be bound to UI, _ProcedureUpdated is only bound on the controller
        /// </summary>
        public string ProcedureUpdated
        {
            get
            {
                if (this._ProcedureUpdated != null) { return Convert.ToBase64String(this._ProcedureUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ProcedureUpdated = null; } else { this._ProcedureUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ProcedureUpdated = val; }
        public byte[] getUpdated() { return _ProcedureUpdated; }


        public static Models.tblProcedureList selectModelData(DTO.tblProcedureList d)
        {
            Models.tblProcedureList modelRecord = new Models.tblProcedureList();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "" };
                string sMsg = "";
                modelRecord = (Models.tblProcedureList)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ProcedureUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblStaticRouteCarr selectLTSData(Models.tblStaticRouteCarr d)
        {
            LTS.tblStaticRouteCarr LTSRecord = new LTS.tblStaticRouteCarr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "tblRouteTypeRefLane", "tblStaticRoute", "CarrierRefLane", "StaticRouteCarrUpdated", "tblStaticRouteEquips" };
                string sMsg = "";
                LTSRecord = (LTS.tblStaticRouteCarr)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.StaticRouteCarrUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }
    }
}