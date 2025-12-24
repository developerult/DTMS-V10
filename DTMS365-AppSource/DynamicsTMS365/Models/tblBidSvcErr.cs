using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 3/1/17 for v-8.0 Next Stop

namespace DynamicsTMS365.Models
{
    public class tblBidSvcErr
    {
        public int BidSvcErrControl { get; set; }
        public int BidSvcErrBidControl { get; set; }
        public string BidSvcErrErrorMessage { get; set; }
        public string BidSvcErrVendorErrorCode { get; set; }
        public string BidSvcErrVendorErrorMessage { get; set; }
        public string BidSvcErrCode { get; set; }
        public string BidSvcErrFieldName { get; set; }
        public string BidSvcErrMessage { get; set; }
        public DateTime? BidSvcErrModDate { get; set; }

        private string _BidSvcErrModUser;
        public string BidSvcErrModUser
        {
            get { return _BidSvcErrModUser.Left(100); } //uses extension string method Left
            set { _BidSvcErrModUser = value.Left(100); }
        }

        private byte[] _BidSvcErrUpdated;

        /// <summary>
        /// BidSvcErrUpdated should be bound to UI, _BidSvcErrUpdated is only bound on the controller
        /// </summary>
        public string BidSvcErrUpdated
        {
            get
            {
                if (this._BidSvcErrUpdated != null)
                {

                    return Convert.ToBase64String(this._BidSvcErrUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BidSvcErrUpdated = null;

                }

                else
                {

                    this._BidSvcErrUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BidSvcErrUpdated = val; }
        public byte[] getUpdated() { return _BidSvcErrUpdated; }



    }
}