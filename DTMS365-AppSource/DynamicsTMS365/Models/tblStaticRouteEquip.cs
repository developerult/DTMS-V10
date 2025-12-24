using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    public class tblStaticRouteEquip
    {
        
        //[Column(Storage = "_StaticRouteEquipControl", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int StaticRouteEquipControl { get; set; }
        //[Column(Storage = "_StaticRouteEquipStaticRouteCarrControl", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int StaticRouteEquipStaticRouteCarrControl { get; set; }
        //[Column(Storage = "_StaticRouteEquipCarrierTruckControl", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int StaticRouteEquipCarrierTruckControl { get; set; }
        //[Column(Storage = "_StaticRouteEquipCarrierTruckDescription", DbType = "NVarChar(255)", UpdateCheck = UpdateCheck.Never)]
        public string StaticRouteEquipCarrierTruckDescription { get; set; }
        //[Column(Storage = "_StaticRouteEquipName", DbType = "NVarChar(50)", UpdateCheck = UpdateCheck.Never)]
        public string StaticRouteEquipName { get; set; }
        //[Column(Storage = "_StaticRouteEquipDescription", DbType = "NVarChar(255)", UpdateCheck = UpdateCheck.Never)]
        public string StaticRouteEquipDescription { get; set; }
        //[Column(Storage = "_StaticRouteEquipModDate", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? StaticRouteEquipModDate { get; set; }
        //[Column(Storage = "_StaticRouteEquipModUser", DbType = "NVarChar(100)", UpdateCheck = UpdateCheck.Never)]
        public string StaticRouteEquipModUser { get; set; }
        //[Column(Storage = "_StaticRouteEquipUpdated", AutoSync = AutoSync.Always, DbType = "rowversion", IsDbGenerated = true, IsVersion = true, UpdateCheck = UpdateCheck.Never)]
        //public Binary StaticRouteEquipUpdated { get; set; }
        //[Association(Name = "CarrierTruckRefLane_tblStaticRouteEquip", Storage = "_CarrierTruckRefLane", ThisKey = "StaticRouteEquipCarrierTruckControl", OtherKey = "CarrierTruckControl", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        // public CarrierTruckRefLane CarrierTruckRefLane { get; set; }
        //[Association(Name = "tblStaticRouteCarr_tblStaticRouteEquip", Storage = "_tblStaticRouteCarr", ThisKey = "StaticRouteEquipStaticRouteCarrControl", OtherKey = "StaticRouteCarrControl", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        //public tblStaticRouteCarr tblStaticRouteCarr { get; set; }


        private byte[] _StaticRouteEquipUpdated;
        /// <summary>
        /// StaticRouteEquipUpdated should be bound to UI, _StaticRouteEquipUpdated is only bound on the controller
        /// </summary>
        public string StaticRouteEquipUpdated
        {
            get
            {
                if (this._StaticRouteEquipUpdated != null) { return Convert.ToBase64String(this._StaticRouteEquipUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._StaticRouteEquipUpdated = null; } else { this._StaticRouteEquipUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _StaticRouteEquipUpdated = val; }
        public byte[] getUpdated() { return _StaticRouteEquipUpdated; }


        public static Models.tblStaticRouteEquip selectModelData(LTS.tblStaticRouteEquip d)
        {
            Models.tblStaticRouteEquip modelRecord = new Models.tblStaticRouteEquip();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierTruckRefLane", "tblStaticRouteCarr", "StaticRouteEquipUpdated"};
                string sMsg = "";
                modelRecord = (Models.tblStaticRouteEquip)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.StaticRouteEquipUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblStaticRouteEquip selectLTSData(Models.tblStaticRouteEquip d)
        {
            LTS.tblStaticRouteEquip LTSRecord = new LTS.tblStaticRouteEquip();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrierTruckRefLane", "tblStaticRouteCarr",  "StaticRouteEquipUpdated" };
                string sMsg = "";
                LTSRecord = (LTS.tblStaticRouteEquip)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.StaticRouteEquipUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }
    }
}