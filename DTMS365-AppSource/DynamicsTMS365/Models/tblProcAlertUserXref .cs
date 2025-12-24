using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{
    public class tblProcAlertUserXref
    {

        public int ProcAlertUserXrefControl { get; set; }

        public int UserSecurityControl { get; set; }

        public int ProcedureControl { get; set; }

        public List<tblProcedureList> tblProcedureLists { get; set; }

        public List<tblUserSecurity> tblUserSecuritys { get; set; }

        public int ProcAlertUserXrefShowPopup { get; set; }

        public int ProcAlertUserXrefSendEmail { get; set; }


        //private byte[] _ProcedureUpdated;
        /// <summary>
        /// ProcedureUpdated should be bound to UI, _ProcedureUpdated is only bound on the controller
        /// </summary>
        //public string ProcedureUpdated
        //{
        //    get
        //    {
        //        if (this._ProcedureUpdated != null) { return Convert.ToBase64String(this._ProcedureUpdated); }
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value)) { this._ProcedureUpdated = null; } else { this._ProcedureUpdated = Convert.FromBase64String(value); }
        //    }
        //}

        //public void setUpdated(byte[] val) { 
        //    _ProcedureUpdated = val; 
        //}
        //public byte[] getUpdated() { return _ProcedureUpdated; }


        public static Models.tblProcedureList selectModelData(DTO.tblProcedureList d)
        {
            Models.tblProcedureList modelRecord = new Models.tblProcedureList();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ProcAlertUserXrefControl" };
                string sMsg = "";
                modelRecord = (Models.tblProcedureList)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
               // if (modelRecord != null && d.ProcedureUpdated != null) { modelRecord.setUpdated(d.ProcedureUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static DTO.tblProcedureList selectDTOData(Models.tblProcedureList d)
        {
            DTO.tblProcedureList DTORecord = new DTO.tblProcedureList();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "ProcAlertUserXrefControl" };
                string sMsg = "";
                DTORecord = (DTO.tblProcedureList)DTran.CopyMatchingFields(DTORecord, d, skipObjs, ref sMsg);
                //if (DTORecord != null)
                //{
                //    byte[] bupdated = d.getUpdated();
                //    DTORecord.ProcedureUpdated = bupdated == null ? new byte[0] : bupdated;
                //}
            }
            return DTORecord;
        }
    }
}