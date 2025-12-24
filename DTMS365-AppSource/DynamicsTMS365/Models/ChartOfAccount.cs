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
    /// General Ledger Chart of Accounts
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.2.007 on 04/18/2023
    /// </remarks>
    public class ChartOfAccount
    {



        public int ID { get; set; }
        public string AcctNo { get; set; }
        public string AcctDescription { get; set; }
        public string AcctType { get; set; }
        public string AcctLine { get; set; }
        public string AcctLineNumber { get; set; }
        public string AcctLinNumberSub { get; set; }
        private byte[] _AcctUpdated;

        public string AcctUpdated
        {
            get
            {
                if (this._AcctUpdated != null)
                {

                    return Convert.ToBase64String(this._AcctUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._AcctUpdated = null;

                }

                else
                {

                    this._AcctUpdated = Convert.FromBase64String(value);

                }

            }
        }


        public void setUpdated(byte[] val) { _AcctUpdated = val; }
        public byte[] getUpdated() { return _AcctUpdated; }


        public static Models.ChartOfAccount selectModelData(DTO.ChartOfAccount d)
        {
            Models.ChartOfAccount modelRecord = new Models.ChartOfAccount();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "AcctUpdated", "rowguid" };
                string sMsg = "";
                modelRecord = (Models.ChartOfAccount)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.AcctUpdated.ToArray());
                }
            }
            return modelRecord;
        }

        public static DTO.ChartOfAccount selectDTOData(Models.ChartOfAccount d)
        {
            DTO.ChartOfAccount dtoRecord = new DTO.ChartOfAccount();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "AcctUpdated", "rowguid" };
                string sMsg = "";
                dtoRecord = (DTO.ChartOfAccount)DTran.CopyMatchingFields(dtoRecord, d, skipObjs, ref sMsg);
                if (dtoRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    dtoRecord.AcctUpdated = bupdated == null ? new byte[0] : bupdated;

                }
            }
            return dtoRecord;
        }

    }
}