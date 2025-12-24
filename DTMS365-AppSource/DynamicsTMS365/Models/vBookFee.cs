using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{
    public class vBookFee
    {

        public string FeeAllocationTypeDesc { get; set; }
        public int BFTarBracketTypeControl { get; set; }
        public string BracketTypeName { get; set; }
        public string BracketTypeDesc { get; set; }
        public int BFAccessorialFeeCalcTypeControl { get; set; }
        public string FeeCalcTypeName { get; set; }
        public string FeeCalcTypeDesc { get; set; }
        public int BFAccessorialOverRideReasonControl { get; set; }
        public string FeeOverRideReasonName { get; set; }
        public string FeeAllocationTypeName { get; set; }
        public string FeeOverRideReasonDesc { get; set; }
        public string FeeDependencyTypeName { get; set; }
        public string FeeDependencyTypeDesc { get; set; }
        public string BFDependencyKey { get; set; }
        public DateTime? BFModDate { get; set; }
        public string BFModUser { get; set; }
        public string BookSHID { get; set; }
        public string BookCarrOrderNumber { get; set; }
        public string BookProNumber { get; set; }
        public string BookConsPrefix { get; set; }
        public int BFAccessorialDependencyTypeControl { get; set; }
        public int BFAccessorialFeeAllocationTypeControl { get; set; }
        public string BFBOLPlacement { get; set; }
        public string BFBOLText { get; set; }
        public int BFControl { get; set; }
        public int? BFBookControl { get; set; }
        public decimal? BFMinimum { get; set; }
        public decimal BFValue { get; set; }
        public double? BFVariable { get; set; }
        public int? BFAccessorialCode { get; set; }
        public string AccessorialName { get; set; }
        public int BFAccessorialFeeTypeControl { get; set; }
        public string FeeTypeDesc { get; set; }
        public bool BFOverRidden { get; set; }
        public int? BFVariableCode { get; set; }
        public string VariableCodeName { get; set; }
        public string VariableCodeDesc { get; set; }
        public bool BFVisible { get; set; }
        public bool BFAutoApprove { get; set; }
        public bool BFAllowCarrierUpdates { get; set; }
        public string BFCaption { get; set; }
        public string BFEDICode { get; set; }
        public bool BFTaxable { get; set; }
        public bool BFIsTax { get; set; }
        public int BFTaxSortOrder { get; set; }
        public bool BookFeesMissingFee { get; set; }
        //public Binary BookFeesUpdated { get; set; }

        private byte[] _BookFeesUpdated;

        /// <summary>
        /// BookFeesUpdated should be bound to UI, _BookFeesUpdated is only bound on the controller
        /// </summary>
        public string BookFeesUpdated
        {
            get
            {
                if (this._BookFeesUpdated != null) { return Convert.ToBase64String(this._BookFeesUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookFeesUpdated = null; } else { this._BookFeesUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookFeesUpdated = val; }
        public byte[] getUpdated() { return _BookFeesUpdated; }

        public static Models.vBookFee selectModelData(LTS.vBookFee d)
        {
            Models.vBookFee modelRecord = new Models.vBookFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookFeesUpdated" };
                string sMsg = "";
                modelRecord = (Models.vBookFee)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookFeesUpdated.ToArray()); }
            }
            return modelRecord;
        }

        public static LTS.vBookFee selectLTSData(Models.vBookFee d)
        {
           LTS.vBookFee ltsRecord = new LTS.vBookFee();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookFeesUpdated" };
                string sMsg = "";
                ltsRecord = (LTS.vBookFee)DTran.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookFeesUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsRecord;
        }

        public static DTO.BookFee selectDTOData(Models.vBookFee d)
        {
            DTO.BookFee oDTOFee = new DTO.BookFee();
            if (d != null)
            {

                oDTOFee.BookFeesAccessorialCode = d.BFAccessorialCode ?? 0;
                oDTOFee.BookFeesAccessorialDependencyTypeControl = d.BFAccessorialDependencyTypeControl;
                oDTOFee.BookFeesAccessorialFeeAllocationTypeControl = d.BFAccessorialFeeAllocationTypeControl;
                oDTOFee.BookFeesAccessorialFeeCalcTypeControl = d.BFAccessorialFeeCalcTypeControl;
                oDTOFee.BookFeesAccessorialFeeTypeControl = d.BFAccessorialFeeTypeControl;
                oDTOFee.BookFeesAccessorialOverRideReasonControl = d.BFAccessorialOverRideReasonControl;
                oDTOFee.BookFeesAllowCarrierUpdates = d.BFAllowCarrierUpdates;
                oDTOFee.BookFeesAutoApprove = d.BFAutoApprove;
                oDTOFee.BookFeesBOLPlacement = d.BFBOLPlacement;
                oDTOFee.BookFeesBOLText = d.BFBOLText;
                oDTOFee.BookFeesBookControl = d.BFBookControl ?? 0;
                oDTOFee.BookFeesCaption = d.BFCaption;
                oDTOFee.BookFeesControl = d.BFControl;
                oDTOFee.BookFeesDependencyKey = d.BFDependencyKey;
                oDTOFee.BookFeesEDICode = d.BFEDICode;
                oDTOFee.BookFeesIsTax = d.BFIsTax;
                oDTOFee.BookFeesMinimum = d.BFMinimum ?? 0;
                oDTOFee.BookFeesModDate = d.BFModDate;
                oDTOFee.BookFeesModUser = d.BFModUser;               
                oDTOFee.BookFeesOverRidden = d.BFOverRidden;
                oDTOFee.BookFeesTarBracketTypeControl = d.BFTarBracketTypeControl;
                oDTOFee.BookFeesTaxable = d.BFTaxable;
                oDTOFee.BookFeesTaxSortOrder = d.BFTaxSortOrder;
                oDTOFee.BookFeesValue = d.BFValue;
                oDTOFee.BookFeesVariable = d.BFVariable ?? 0;
                oDTOFee.BookFeesVariableCode = d.BFVariableCode ?? 0;
                oDTOFee.BookFeesVisible = d.BFVisible;
                oDTOFee.BookFeesMissingFee = d.BookFeesMissingFee;

                byte[] bupdated = d.getUpdated();
                oDTOFee.BookFeesUpdated = bupdated == null ? new byte[0] : bupdated;

            }
            return oDTOFee;
        }
    }
}