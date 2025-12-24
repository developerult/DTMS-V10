using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class AcceptorReject
    {

        public int BookControl { get; set; }
        public int CarrierControl { get; set; }
        public int CarrierContControl { get; set; }
        public int AcceptRejectCode { get; set; } //Accepted = 0,1 = Rejected,2 = Expired,3 =  Unfinalize,4 = Tendered,5 = Dropped,6 = Unassigned,7 = ModifyUnaccepted
        public bool SendEmail { get; set; }
        public string BookTrackComment { get; set; }
        public int BookTrackStatus { get; set; }
        public string NotificationEMailAddress { get; set; }
        public string NotificationEMailAddressCc { get; set; }
        public int AcceptRejectMode { get; set; } //values 0=Manual, 1 = EDI, 2 = web, 3 = system, 4 = token, 5 = DAT
        public string UserName { get; set; }
                                       //Optional ByRef results As DTO.WCFResults = Nothing,
                                       //Optional ByVal intValidationFlags As Int64 = 0,
                                       //Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost
    }
}