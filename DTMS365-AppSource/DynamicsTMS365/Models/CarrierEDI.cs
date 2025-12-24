using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierEDI
    {
        public Carrier Carrier { get; set; }
        public bool CarrierEDIAcceptOn997 { get; set; }
        public bool CarrierEDIAcknowledgementRequested { get; set; }
        public string CarrierEDIBackupFolder { get; set; }
        public int? CarrierEDICarrierControl { get; set; }
        public string CarrierEDIComment { get; set; }
        public int CarrierEDIControl { get; set; }
        public string CarrierEDIDaysOfWeek { get; set; }
        public string CarrierEDIEmailAddress { get; set; }
        public bool CarrierEDIEmailNotificationOn { get; set; }
        public string CarrierEDIEndTime { get; set; }
        public string CarrierEDIFileNameBaseInbound { get; set; }
        public string CarrierEDIFileNameBaseOutbound { get; set; }
        public string CarrierEDIFTPBackupFolder { get; set; }
        public string CarrierEDIFTPInboundFolder { get; set; }
        public string CarrierEDIFTPOutboundFolder { get; set; }
        public string CarrierEDIFTPPassword { get; set; }
        public string CarrierEDIFTPServer { get; set; }
        public string CarrierEDIFTPUserName { get; set; }
        public int CarrierEDIGSSequence { get; set; }
        public string CarrierEDIInboundFolder { get; set; }

        public string _CarrierEDIInboundFolder { get { if (string.IsNullOrEmpty(this.CarrierEDIInboundFolder)) { this.CarrierEDIInboundFolder = @"C:\Data\Inbound"; } return this.CarrierEDIInboundFolder; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIInboundFolder = @"C:\Data\Inbound"; } } }
        public string _CarrierEDIBackupFolder { get { if (string.IsNullOrEmpty(this.CarrierEDIBackupFolder)) { return this.CarrierEDIBackupFolder = @"C:\Data\Backup"; } return this.CarrierEDIBackupFolder; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIBackupFolder = @"C:\Data\Backup"; } } }
        public string _CarrierEDILogFile { get { if (string.IsNullOrEmpty(this.CarrierEDILogFile)) { this.CarrierEDILogFile = @"C:\Data\EDILogFile.txt"; } return CarrierEDILogFile; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDILogFile = @"C:\Data\EDILogFile.txt"; } } }
        public string _CarrierEDIStartTime { get { if (string.IsNullOrEmpty(this.CarrierEDIStartTime)) { this.CarrierEDIStartTime = "09:00 AM"; } return CarrierEDIStartTime; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIStartTime = "09:00 AM"; } } }
        public string _CarrierEDIEndTime { get { if (string.IsNullOrEmpty(this.CarrierEDIEndTime)) { this.CarrierEDIEndTime = "05:00 PM"; } return CarrierEDIEndTime; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIEndTime = "05:00 PM"; } } }
        public string _CarrierEDIDaysOfWeek { get { if (string.IsNullOrEmpty(this.CarrierEDIDaysOfWeek)) { this.CarrierEDIDaysOfWeek = "CarrierEDIBackupFolder"; } return CarrierEDIDaysOfWeek; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIDaysOfWeek = "CarrierEDIBackupFolder"; } } }
        public string _CarrierEDIFileNameBaseOutbound { get { if (string.IsNullOrEmpty(this.CarrierEDIFileNameBaseOutbound)) { this.CarrierEDIFileNameBaseOutbound = "204OUT"; } return CarrierEDIFileNameBaseOutbound; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIFileNameBaseOutbound = "204OUT"; } } }
        public string _CarrierEDIFileNameBaseInbound { get { if (string.IsNullOrEmpty(this.CarrierEDIFileNameBaseInbound)) { this.CarrierEDIFileNameBaseInbound = "EDIIn"; } return CarrierEDIFileNameBaseInbound; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIFileNameBaseInbound = "EDIIn"; } } }
        public string _CarrierEDIWebServiceAuthKey { get { if (string.IsNullOrEmpty(this.CarrierEDIWebServiceAuthKey)) { this.CarrierEDIWebServiceAuthKey = "ABCDE"; } return CarrierEDIWebServiceAuthKey; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIWebServiceAuthKey = "ABCDE"; } } }
        public string _CarrierEDINGLEDIInputWebURL { get { if (string.IsNullOrEmpty(this.CarrierEDINGLEDIInputWebURL)) { this.CarrierEDINGLEDIInputWebURL = "http://localhost:4479/EDIInput.asmx"; } return CarrierEDINGLEDIInputWebURL; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDINGLEDIInputWebURL = "http://localhost:4479/EDIInput.asmx"; } } }
        public string _CarrierEDINGLEDI204OutputWebURL { get { if (string.IsNullOrEmpty(this.CarrierEDINGLEDI204OutputWebURL)) { this.CarrierEDINGLEDI204OutputWebURL = "http://localhost:4479/EDI204Output.asmx"; } return CarrierEDINGLEDI204OutputWebURL; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDINGLEDI204OutputWebURL = "http://localhost:4479/EDI204Output.asmx"; } } }
        public string _CarrierEDIOutboundFolder { get { if (string.IsNullOrEmpty(this.CarrierEDIOutboundFolder)) { this.CarrierEDIOutboundFolder = @"C:\Data\Outbound"; } return CarrierEDIOutboundFolder; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIOutboundFolder = @"C:\Data\Outbound"; } } }
        public string _CarrierEDIWebServiceURL { get { if (string.IsNullOrEmpty(this.CarrierEDIWebServiceURL)) { this.CarrierEDIWebServiceURL = "http://localhost:4479/EDIInput.asmx"; } return CarrierEDIWebServiceURL; } set { if (string.IsNullOrEmpty(value)) { this.CarrierEDIWebServiceURL = "http://localhost:4479/EDIInput.asmx"; } } }
        public string _CarrierEDISecurityQual { get { if (string.IsNullOrEmpty(this.CarrierEDISecurityQual)) { this.CarrierEDISecurityQual = "00"; } return CarrierEDISecurityQual; } set {  } }
   
        public string _CarrierEDIPartnerQual { get { if (string.IsNullOrEmpty(this.CarrierEDIPartnerQual)) { this.CarrierEDIPartnerQual = "02"; } return CarrierEDIPartnerQual; } set {  } }
          
        public char _CarrierEDITestCode { get { if (this.CarrierEDITestCode.ToString()=="\0") {  this.CarrierEDITestCode = 'P'; }return  this.CarrierEDITestCode; } set {   } }         
                    
        public int _CarrierEDIISASequence{ get{ if(this.CarrierEDIISASequence==0) this.CarrierEDIISASequence = 11111; return this.CarrierEDIISASequence; } set{  } }
        public int _CarrierEDISendMinutesOutbound { get { if (this.CarrierEDISendMinutesOutbound == 0) this.CarrierEDISendMinutesOutbound = 15; return CarrierEDISendMinutesOutbound; } set {  } }
        public int _CarrierEDIGSSequence { get { if (this.CarrierEDIGSSequence == 0) this.CarrierEDIGSSequence = 11111; return this.CarrierEDIGSSequence; } set { } } 
        
        public int CarrierEDIISASequence { get; set; }
        public DateTime? CarrierEDILastOutboundTransmission { get; set; }
        public string CarrierEDILogFile { get; set; }
        public string CarrierEDINGLEDI204OutputWebURL { get; set; }
        public string CarrierEDINGLEDIInputWebURL { get; set; }
        public string CarrierEDIOutboundFolder { get; set; }
        public string CarrierEDIPartnerCode { get; set; }
        public string CarrierEDIPartnerQual { get; set; }
        public string CarrierEDISecurityCode { get; set; }
        public string CarrierEDISecurityQual { get; set; }
        public int CarrierEDISendMinutesOutbound { get; set; }
        public string CarrierEDIStartTime { get; set; }
        public char CarrierEDITestCode { get; set; }
        public byte[] _CarrierEDIUpdated { get; set; }
        public string CarrierEDIWebServiceAuthKey { get; set; }
        public string CarrierEDIWebServiceURL { get; set; }
        public string CarrierEDIXaction { get; set; }

        public string CarrierEDIUpdated
        {
            get
            {
                if (this._CarrierEDIUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrierEDIUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrierEDIUpdated = null;

                }

                else
                {

                    this._CarrierEDIUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrierEDIUpdated = val; }
        public byte[] getUpdated() { return _CarrierEDIUpdated; }
    }
}