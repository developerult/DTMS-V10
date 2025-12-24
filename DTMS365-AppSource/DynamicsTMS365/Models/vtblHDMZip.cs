using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{
    public class vtblHDMZip
    {

        //[Column(Storage = "_HDMZipControl", AutoSync = AutoSync.Always, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int HDMZipControl { get; set; }
        //[Column(Storage = "_HDMZipHDMControl", DbType = "Int NOT NULL", UpdateCheck = UpdateCheck.Never)]
        public int HDMZipHDMControl { get; set; }
        //[Column(Storage = "_HDMZipFrom", DbType = "NVarChar(20) NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public string HDMZipFrom { get; set; }
        //[Column(Storage = "_HDMZipTo", DbType = "NVarChar(20)", UpdateCheck = UpdateCheck.Never)]
        public string HDMZipTo { get; set; }
        //[Column(Storage = "_HDMZipCity", DbType = "NVarChar(25)", UpdateCheck = UpdateCheck.Never)]
        public string HDMZipCity { get; set; }
        //[Column(Storage = "_HDMZipState", DbType = "NVarChar(8)", UpdateCheck = UpdateCheck.Never)]
        public string HDMZipState { get; set; }
        //[Column(Storage = "_HDMZipCountry", DbType = "NVarChar(30)", UpdateCheck = UpdateCheck.Never)]
        public string HDMZipCountry { get; set; }
        //[Column(Storage = "_HDMZipModDate", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public DateTime? HDMZipModDate { get; set; }
        //[Column(Storage = "_HDMZipModUser", DbType = "NVarChar(100)", UpdateCheck = UpdateCheck.Never)]
        public string HDMZipModUser { get; set; }
        //[Column(Storage = "_HDMZipUpdated", AutoSync = AutoSync.Always, DbType = "rowversion", CanBeNull = true, IsDbGenerated = true, IsVersion = true, UpdateCheck = UpdateCheck.Never)]
        private byte[] _HDMZipUpdated { get; set; }

        public string HDMZipUpdated
        {
            get
            {
                if (this._HDMZipUpdated != null)
                {

                    return Convert.ToBase64String(this._HDMZipUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._HDMZipUpdated = null;

                }

                else
                {

                    this._HDMZipUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _HDMZipUpdated = val; }
        public byte[] getUpdated() { return _HDMZipUpdated; }


        public static Models.vtblHDMZip selectModelData(LTS.vtblHDMZip d)
        {
            Models.vtblHDMZip modelRecord = new Models.vtblHDMZip();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "HDMZipUpdated" };
                string sMsg = "";
                modelRecord = (Models.vtblHDMZip)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.HDMZipUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.vtblHDMZip selectLTSData(Models.vtblHDMZip d)
        {
            LTS.vtblHDMZip LTSRecord = new LTS.vtblHDMZip();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "HDMZipUpdated" };
                string sMsg = "";
                LTSRecord = (LTS.vtblHDMZip)DTran.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.HDMZipUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return LTSRecord;
        }

    }
}