using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    public class tblServiceToken
    {


        public DateTime? ServiceTokenModDate { get; set; }
        public DateTime? ServiceTokenExpirationDate { get; set; }
        public string ServiceTokenUserName { get; set; }
        public string ServiceTokenNotificationEMailAddressCc { get; set; }
        public string ServiceTokenNotificationEMailAddress { get; set; }
        public int ServiceTokenBookTrackStatus { get; set; }
        public string ServiceTokenBookTrackComment { get; set; }
        public bool ServiceTokenSendEmail { get; set; }
        public string ServiceTokenCode { get; set; }
        public int ServiceTokenAltKeyControl { get; set; }
        public int ServiceTokenLaneControl { get; set; }
        public int ServiceTokenCarrierContControl { get; set; }
        public int ServiceTokenCarrierControl { get; set; }
        public int ServiceTokenCompControl { get; set; }
        public int ServiceTokenBookControl { get; set; }
        public string ServiceToken { get; set; }
        public int ServiceTokenServiceTypeControl { get; set; }
        public int ServiceTokenControl { get; set; }
        public string ServiceTokenModUser { get; set; }

        private byte[] _ServiceTokenUpdated;

        /// <summary>
        /// ServiceTokenUpdated should be bound to UI, _ServiceTokenUpdated is only bound on the controller
        /// </summary>
        public string ServiceTokenUpdated
        {
            get
            {
                if (this._ServiceTokenUpdated != null) { return Convert.ToBase64String(this._ServiceTokenUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._ServiceTokenUpdated = null; } else { this._ServiceTokenUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _ServiceTokenUpdated = val; }
        public byte[] getUpdated() { return _ServiceTokenUpdated; }


        public static Models.tblServiceToken selectModelData(LTS.tblServiceToken d)
        {
            Models.tblServiceToken modelRecord = new Models.tblServiceToken();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ServiceTokenUpdated","tblServiceType" };
                string sMsg = "";
                modelRecord = (Models.tblServiceToken)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ServiceTokenUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static Models.tblServiceToken selectModelData(DTO.tblServiceToken d)
        {
            Models.tblServiceToken modelRecord = new Models.tblServiceToken();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ServiceTokenUpdated", "tblServiceType" };
                string sMsg = "";
                modelRecord = (Models.tblServiceToken)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.ServiceTokenUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.tblServiceToken selectLTSData(Models.tblServiceToken d)
        {
            LTS.tblServiceToken ltsRecord = new LTS.tblServiceToken();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ServiceTokenUpdated", "tblServiceType" };
                string sMsg = "";
                ltsRecord = (LTS.tblServiceToken)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.ServiceTokenUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        public static DTO.tblServiceToken selectDTOData(Models.tblServiceToken d)
        {
            DTO.tblServiceToken DTORecord = new DTO.tblServiceToken();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ServiceTokenUpdated", "tblServiceType" };
                string sMsg = "";
                DTORecord = (DTO.tblServiceToken)DTran.CopyMatchingFields(DTORecord, d, skipObjs, ref sMsg);
                if (DTORecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    DTORecord.ServiceTokenUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return DTORecord;
        }


    }
}