using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class vPOHdr
    {
        public string POHdrControl { get; set; }
        public string POHDRnumber { get; set; }
        public string POHDROrderNumber { get; set; }
        public int POHDROrderSequence { get; set; }
        public string POHDRAPGLNumber { get; set; }
        public string POHDRBuyer { get; set; }
        public string POHdrCarrBLNumber { get; set; }
        public string POHDRCarrierEquipmentCodes { get; set; }
        public string POHDRCarrierTypeCode { get; set; }
        public string POHDRCarType { get; set; }
        public string POHDRChepGLID { get; set; }
        public string POHDRComments { get; set; }
        public string POHDRCommentsConfidential { get; set; }
        public string POHDRCompAlphaCode { get; set; }
        public string POHDRCompLegalEntity { get; set; }
        public bool POHDRConfirm { get; set; }
        public bool? POHDRCooler { get; set; }
        public int? POHDRCube { get; set; }
        public int? POHDRDefaultCarrier { get; set; }
        public int? POHDRDefaultCustomer { get; set; }
        public string POHDRDefaultCustomerName { get; set; }
        public int POHDRDefaultRouteSequence { get; set; }
        public string POHDRDestAddress1 { get; set; }
        public string POHDRDestAddress2 { get; set; }
        public string POHDRDestAddress3 { get; set; }
        public string POHDRDestCity { get; set; }
        public string POHDRDestCompNumber { get; set; }
        public string POHDRDestContactEmail { get; set; }
        public string POHDRDestContactFax { get; set; }
        public string POHDRDestContactPhone { get; set; }
        public string POHDRDestContactPhoneExt { get; set; }
        public string POHDRDestCountry { get; set; }
        public string POHDRDestName { get; set; }
        public string POHDRDestState { get; set; }
        public string POHDRDestZip { get; set; }
        public int? POHDRDry { get; set; }
        public bool? POHDRFrozen { get; set; }
        public byte? POHDRFrt { get; set; }
        public bool POHDRHoldLoad { get; set; }
        public bool POHDRInbound { get; set; }
        public double? POHDRLines { get; set; }
        public int POHDRModeTypeControl { get; set; }
        public string POHDROrigAddress1 { get; set; }
        public string POHDROrigAddress2 { get; set; }
        public string POHDROrigAddress3 { get; set; }
        public string POHDROrigCity { get; set; }
        public string POHDROrigCompNumber { get; set; }
        public string POHDROrigContactEmail { get; set; }
        public string POHDROrigContactFax { get; set; }
        public string POHDROrigContactPhone { get; set; }
        public string POHDROrigContactPhoneExt { get; set; }
        public string POHDROrigCountry { get; set; }
        public string POHDROrigName { get; set; }
        public string POHDROrigState { get; set; }
        public string POHDROrigZip { get; set; }
        public double? POHDROtherCost { get; set; }
        public bool POHDRPalletExchange { get; set; }
        public string POHDRPalletPositions { get; set; }
        public int? POHDRPallets { get; set; }
        public string POHDRPalletType { get; set; }
        public string POHDRPRONumber { get; set; }
        public int? POHDRQty { get; set; }
        public string POHDRRouteGuideNumber { get; set; }
        public DateTime? POHDRShipdate { get; set; }
        public string POHDRShipInstructions { get; set; }
        public string POHDRShipVia { get; set; }
        public string POHDRShipViaType { get; set; }
        public byte POHDRSortOrder { get; set; }
        public int? POHDRStatusFlag { get; set; }
        public string POHDRTemp { get; set; }
        public double? POHDRTotalCost { get; set; }
        public double? POHDRTotalFrt { get; set; }
        public string POHDRUser1 { get; set; }
        public string POHDRUser2 { get; set; }
        public string POHDRUser3 { get; set; }
        public string POHDRUser4 { get; set; }
        public string POHDRvendor { get; set; }
        public double? POHDRWgt { get; set; }
        public string POHdrWhseAuthorizationNo { get; set; }
        public string POHDRWhseReleaseNo { get; set; }
        public DateTime? POHDRActDelDate { get; set; }
        public string POHDRActDelTime { get; set; }
        public DateTime? POHDRActPUDate { get; set; }
        public string POHDRActPUTime { get; set; }
        public DateTime? POHDRScheduleDelDate { get; set; }
        public string POHDRScheduleDelTime { get; set; }
        public DateTime? POHDRSchedulePUDate { get; set; }
        public string POHDRSchedulePUTime { get; set; }
        public DateTime? POHDRMustLeaveByDateTime { get; set; }
        public DateTime? POHDRPOdate { get; set; }
        public DateTime? POHDRReqDate { get; set; }

        public DateTime? POHDRCreateDate { get; set; }
        public string POHDRCreateUser { get; set; }
        public string POHDRModVerify { get; set; }

        private string _POHDRModUser;
        public string POHDRModUser
        {
            get { return _POHDRModUser.Left(100); } //uses extension string method Left
            set { _POHDRModUser = value.Left(100); }
        }

        public string POItemOrderNumbers { get; set; }

        public string TransType { get; set; }

        private byte[] _POHdrUpdated;

        /// <summary>
        /// POHdrUpdated should be bound to UI, _POHdrUpdated is only bound on the controller
        /// </summary>
        public string POHdrUpdated
        {
            get
            {
                if (this._POHdrUpdated != null) { return Convert.ToBase64String(this._POHdrUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._POHdrUpdated = null; } else { this._POHdrUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _POHdrUpdated = val; }
        public byte[] getUpdated() { return _POHdrUpdated; }


    }
}