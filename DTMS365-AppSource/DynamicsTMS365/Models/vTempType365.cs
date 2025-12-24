using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    public class vTempType365
    {
        public string CommCodeType { get; set; }
        public string CommCodeDescription { get; set; }
        public string ID { get; set; }
        public string TempType { get; set; }
        public int TariffTempType { get; set; }
        public string TariffTempTypeName { get; set; }
        public int DATEquipTypeControl { get; set; }
        public string DATEquipTypeName { get; set; }
        public int TempTypeBitPos { get; set; }

        private byte[] _TempTypeUpdated;
        public string TempTypeUpdated
        {
            get
            {
                if (this._TempTypeUpdated != null)
                {

                    return Convert.ToBase64String(this._TempTypeUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._TempTypeUpdated = null;

                }

                else
                {

                    this._TempTypeUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _TempTypeUpdated = val; }
        public byte[] getUpdated() { return _TempTypeUpdated; }


        public static Models.vTempType365 selectModelData(LTS.vTempType365 d)
        {
            Models.vTempType365 modelRecord = new Models.vTempType365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "TempTypeUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.vTempType365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                //When using Updated field with the DTO objects the data has already been converted to an array
                //so replace the LTL logic 
                //if (modelRecord != null) { modelRecord.setUpdated(d.LaneFeesUpdated.ToArray()); }
                // with the code below (just remove the ToArray
                if (modelRecord != null) { modelRecord.setUpdated(d.TempTypeUpdated.ToArray()); }
            }

            return modelRecord;
        }


        public static LTS.vTempType365 selectLTSData(Models.vTempType365 d)
        {
            LTS.vTempType365 LTSRecord = new LTS.vTempType365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "TempTypeUpdated", "rowguid" };
                string sMsg = "";
                LTSRecord = (LTS.vTempType365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.TempTypeUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }

            return LTSRecord;
        }


        public static LTS.TempType selectLTSTableData(Models.vTempType365 d)
        {
            LTS.TempType LTSRecord = new LTS.TempType();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "TempTypeUpdated", "rowguid", "TariffTempTypeName", "DATEquipTypeName" };
                string sMsg = "";
                LTSRecord = (LTS.TempType)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(LTSRecord, d, skipObjs, ref sMsg);
                if (LTSRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    LTSRecord.TempTypeUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }

            return LTSRecord;
        }

    }

    //Old Class Deprecated by RHR for v-8.5.2.007
    // Use vTempType365 instead
    public class vTempType
    {
        public string CommCodeType { get; set; }
        public string CommCodeDescription { get; set; }
        public string ID { get; set; }
        public string TempType { get; set; }
        public int rowguid { get; set; }
        public int TariffTempType { get; set; }
        public int DATEquipTypeControl { get; set; }
        public int TempTypeBitPos { get; set; }

        private byte[] _TempTypeUpdated;
        public string TempTypeUpdated
        {
            get
            {
                if (this._TempTypeUpdated != null)
                {

                    return Convert.ToBase64String(this._TempTypeUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._TempTypeUpdated = null;

                }

                else
                {

                    this._TempTypeUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _TempTypeUpdated = val; }
        public byte[] getUpdated() { return _TempTypeUpdated; }
    }
}