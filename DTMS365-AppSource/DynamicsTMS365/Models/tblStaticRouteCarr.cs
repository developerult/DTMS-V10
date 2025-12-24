using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
                 
    public class tblStaticRouteCarr
    {


        //public tblRouteTypeRefLane tblRouteTypeRefLane { get; set; }
       // public tblStaticRoute tblStaticRoute { get; set; }
       // public CarrierRefLane CarrierRefLane { get; set; }
        //public EntitySet<tblStaticRouteEquip> tblStaticRouteEquips { get; set; }
        public string StaticRouteCarrURI { get; set; }
        public string StaticRouteStateFilter { get; set; }
        //public Binary StaticRouteCarrUpdated { get; set; }
        public string StaticRouteCarrModUser { get; set; }
        public DateTime? StaticRouteCarrModDate { get; set; }
        public bool StaticRouteCarrAutoAcceptLoads { get; set; }
        public bool StaticRouteCarrRequireAutoTenderApproval { get; set; }
        public int StaticRouteCarrRouteSequence { get; set; }
        public int StaticRouteCarrTransType { get; set; }
        public bool StaticRouteCarrHazmatFlag { get; set; }
        public int StaticRouteCarrMaxStops { get; set; }
        public int StaticRouteCarrTendLeadTime { get; set; }
        public bool StaticRouteCarrAutoTenderFlag { get; set; }
        public int StaticRouteCarrRouteTypeCode { get; set; }
        public string StaticRouteCarrDescription { get; set; }
        public string StaticRouteCarrName { get; set; }
        public string StaticRouteCarrCarrierName { get; set; }
        public int StaticRouteCarrCarrierNumber { get; set; }
        public int StaticRouteCarrCarrierControl { get; set; }
        public int StaticRouteCarrStaticRouteControl { get; set; }
        public int StaticRouteCarrControl { get; set; }


        //public Binary StaticRouteCarrUpdated { get; set; }


        private byte[] _StaticRouteCarrUpdated;
        /// <summary>
        /// StaticRouteCarrUpdated should be bound to UI, _StaticRouteCarrUpdated is only bound on the controller
        /// </summary>
        public string StaticRouteCarrUpdated
        {
            get
            {
                if (this._StaticRouteCarrUpdated != null) { return Convert.ToBase64String(this._StaticRouteCarrUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._StaticRouteCarrUpdated = null; } else { this._StaticRouteCarrUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _StaticRouteCarrUpdated = val; }
        public byte[] getUpdated() { return _StaticRouteCarrUpdated; }


        public static Models.tblStaticRouteCarr selectModelData(LTS.tblStaticRouteCarr d)
        {
            Models.tblStaticRouteCarr modelRecord = new Models.tblStaticRouteCarr();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "tblRouteTypeRefLane","tblStaticRoute","CarrierRefLane", "StaticRouteCarrUpdated", "tblStaticRouteEquips" };
                string sMsg = "";
                modelRecord = (Models.tblStaticRouteCarr)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.StaticRouteCarrUpdated.ToArray()); }
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