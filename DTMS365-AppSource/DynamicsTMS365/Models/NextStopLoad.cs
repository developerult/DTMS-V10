using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added By LVV on 12/23/16 for v-8.0 Next Stop

namespace DynamicsTMS365.Models
{
    public class NextStopLoad
    {
        public int LoadTenderControl { get; set; }
        public int LTLoadTenderTypeControl { get; set; }
        public int LTBookControl { get; set; }
        public string LTBookProNumber { get; set; }
        public string LTBookConsPrefix { get; set; }
        public string LTBookSHID { get; set; }
        public string LTBookCarrOrderNumber { get; set; }
        public int? LTBookOrderSequence { get; set; }      
        public int? LTBookTotalCases { get; set; }
        public double? LTBookTotalWgt { get; set; }
        public double? LTBookTotalPL { get; set; }
        public int? LTBookTotalCube { get; set; }
        public double? LTBookTotalMiles { get; set; }
        public DateTime? LTBookDateLoad { get; set; }
        public DateTime? LTBookDateRequired { get; set; }
        public string LTBookOrigName { get; set; }
        public string LTBookOrigAddress1 { get; set; }
        public string LTBookOrigAddress2 { get; set; }
        public string LTBookOrigAddress3 { get; set; }
        public string LTBookOrigCity { get; set; }
        public string LTBookOrigState { get; set; }
        public string LTBookOrigCountry { get; set; }
        public string LTBookOrigZip { get; set; }
        public string LTBookDestName { get; set; }
        public string LTBookDestAddress1 { get; set; }
        public string LTBookDestAddress2 { get; set; }
        public string LTBookDestAddress3 { get; set; }
        public string LTBookDestCity { get; set; }
        public string LTBookDestState { get; set; }
        public string LTBookDestCountry { get; set; }
        public string LTBookDestZip { get; set; }
        public int? LTStatusCode { get; set; }
        public string LTMessage { get; set; }
        public Boolean LTArchived { get; set; }
        public DateTime? LTTenderedDate { get; set; }
        public DateTime? LTExpires { get; set; }
        public Boolean LTExpired { get; set; }
        public string LTNotes { get; set; }

        public DateTime? LTModDate { get; set; }

        private string _LTModUser;
        public string LTModUser
        {
            get { return _LTModUser.Left(100); } //uses extension string method Left
            set { _LTModUser = value.Left(100); }
        }

        private byte[] _LTUpdated;

        /// <summary>
        /// LTUpdated should be bound to UI, _LTUpdated is only bound on the controller
        /// </summary>
        public string LTUpdated
        {
            get
            {
                if (this._LTUpdated != null)
                {

                    return Convert.ToBase64String(this._LTUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LTUpdated = null;

                }

                else
                {

                    this._LTUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LTUpdated = val; }
        public byte[] getUpdated() { return _LTUpdated; }


        public void fillFromRequestDelete(HttpRequest request)
        {
            int intVal = 0;
            int.TryParse(request["LoadTenderControl"] ?? "0", out intVal);
            this.LoadTenderControl = intVal;
            intVal = 0;
            int.TryParse(request["LTBookControl"] ?? "0", out intVal);
            this.LTBookControl = intVal;
            this.LTBookSHID = request["LTBookSHID"] ?? "";
        }

    }
}