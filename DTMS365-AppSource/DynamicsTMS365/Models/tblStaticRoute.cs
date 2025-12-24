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
    /// Model for Route Guide parent data
    /// </summary>
	/// <remarks>
	/// Created by RHR for v-8.5.3.006 on 01/10/2023
	/// </remarks>
    public class tblStaticRoute
    {

        public string StaticRouteURI { get; set; }
        public bool StaticRoutePlaceOnHold { get; set; }
        public bool StaticRouteFillLargestFirst { get; set; }
        public string StaticRouteModUser { get; set; }
        public DateTime? StaticRouteModDate { get; set; }
        public bool StaticRouteRequireAutoTenderApproval { get; set; }
        public int StaticRouteCapacityPreference { get; set; }
        public bool StaticRouteSplitOversizedLoads { get; set; }
        public int StaticRouteGuideDateSelectionDaysAfter { get; set; }
        public int StaticRouteGuideDateSelectionDaysBefore { get; set; }
        public bool StaticRouteUseShipDateFlag { get; set; }
        public bool StaticRouteAutoTenderFlag { get; set; }
        public int StaticRouteCompNumber { get; set; }
        public string StaticRouteCompName { get; set; }
        public string StaticRouteNatName { get; set; }
        public int StaticRouteNatNumber { get; set; }
        public int StaticRouteCompControl { get; set; }
        public string StaticRouteDescription { get; set; }
        public string StaticRouteName { get; set; }
        public string StaticRouteNumber { get; set; }
        public int StaticRouteControl { get; set; }


        private byte[] _StaticRouteUpdated;
        /// <summary>
        /// StaticRouteUpdated should be bound to UI, _StaticRouteUpdated is only bound on the controller
        /// </summary>
        public string StaticRouteUpdated
        {
            get
            {
                if (this._StaticRouteUpdated != null) { return Convert.ToBase64String(this._StaticRouteUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._StaticRouteUpdated = null; } else { this._StaticRouteUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _StaticRouteUpdated = val; }
        public byte[] getUpdated() { return _StaticRouteUpdated; }


        public static Models.tblStaticRoute selectModelData(LTS.tblStaticRoute d)
        {
            Models.tblStaticRoute modelRecord = new Models.tblStaticRoute();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "tblStaticRouteCarrs", "StaticRouteUpdated" };
                string sMsg = "";
                modelRecord = (Models.tblStaticRoute)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.StaticRouteUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblStaticRoute selectLTSData(Models.tblStaticRoute d)
        {
            LTS.tblStaticRoute LTSRecord = new LTS.tblStaticRoute();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "tblStaticRouteCarrs", "StaticRouteUpdated" };
                string sMsg = "";
                LTSRecord = (LTS.tblStaticRoute)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.StaticRouteUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }
    }
}