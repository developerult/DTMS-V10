using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class cmPage
    {
        public int PageControl { get; set; }

        private string _PageName;
        public string PageName
        {
            get { return _PageName.Left(255); } //uses extension string method Left
            set { _PageName = value.Left(255); }
        }
        private string _PageDesc;
        public string PageDesc
        {
            get { return _PageDesc.Left(255); } //uses extension string method Left
            set { _PageDesc = value.Left(255); }
        }
        private string _PageCaption;
        public string PageCaption
        {
            get { return _PageCaption.Left(4000); } //uses extension string method Left
            set { _PageCaption = value.Left(4000); }
        }
        private string _PageCaptionLocal;
        public string PageCaptionLocal
        {
            get { return _PageCaptionLocal.Left(4000); } //uses extension string method Left
            set { _PageCaptionLocal = value.Left(4000); }
        }
        public int PageFormControl { get; set; }
        private string _PageFormName;
        public string PageFormName
        {
            get { return _PageFormName.Left(255); } //uses extension string method Left
            set { _PageFormName = value.Left(255); }
        }
        private string _PageFormDesc;
        public string PageFormDesc
        {
            get { return _PageFormDesc.Left(255); } //uses extension string method Left
            set { _PageFormDesc = value.Left(255); }
        }
        public bool PageDataSource { get; set; }
        public bool PageSortable { get; set; }
        public bool PagePageable { get; set; }
        public bool PageGroupable { get; set; }
        public bool PageEditable { get; set; }
        public int PageDataElmtControl { get; set; }
        public int PageElmtFieldControl { get; set; }
        public int PageAutoRefreshSec { get; set; }

        //Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
        private string _PageFooterMsg;
        public string PageFooterMsg
        {
            get { return _PageFooterMsg.Left(4000); } //uses extension string method Left
            set { _PageFooterMsg = value.Left(4000); }
        }

        public DateTime? PageModDate { get; set; }
        private string _PageModUser;
        public string PageModUser
        {
            get { return _PageModUser.Left(100); } //uses extension string method Left
            set { _PageModUser = value.Left(100); }
        }

        private byte[] _PageUpdated;

        /// <summary>
        /// PageUpdated should be bound to UI, _PageUpdated is only bound on the controller
        /// </summary>
        public string PageUpdated
        {
            get
            {
                if (this._PageUpdated != null) { return Convert.ToBase64String(this._PageUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._PageUpdated = null; }
                else { this._PageUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _PageUpdated = val; }
        public byte[] getUpdated() { return _PageUpdated; }


        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            bool blnVal = true;
            int.TryParse(request["PageControl"] ?? "0", out intVal);
            this.PageControl = intVal;
            intVal = 0;
            this.PageName = request["PageName"] ?? "";
            this.PageDesc = request["PageDesc"] ?? "";
            this.PageCaption = request["PageCaption"] ?? "";
            this.PageCaptionLocal = request["PageCaptionLocal"] ?? "";
            int.TryParse(request["PageFormControl"] ?? "0", out intVal);
            this.PageFormControl = intVal;
            intVal = 0;
            blnVal = false;
            bool.TryParse(request["PageDataSource"] ?? "0", out blnVal);
            this.PageDataSource = blnVal;
            blnVal = false;
            bool.TryParse(request["PageSortable"] ?? "0", out blnVal);
            this.PageSortable = blnVal;
            blnVal = false;
            bool.TryParse(request["PagePageable"] ?? "0", out blnVal);
            this.PagePageable = blnVal;
            blnVal = false;
            bool.TryParse(request["PageGroupable"] ?? "0", out blnVal);
            this.PageGroupable = blnVal;
            blnVal = true;
            bool.TryParse(request["PageEditable"] ?? "1", out blnVal);
            this.PageEditable = blnVal;
            blnVal = false;
            int.TryParse(request["PageDataElmtControl"] ?? "0", out intVal);
            this.PageDataElmtControl = intVal;
            intVal = 0;
            int.TryParse(request["PageElmtFieldControl"] ?? "0", out intVal);
            this.PageElmtFieldControl = intVal;
            intVal = 0;
            int.TryParse(request["PageAutoRefreshSec"] ?? "0", out intVal);
            this.PageAutoRefreshSec = intVal;
            this.PageModDate = string.IsNullOrEmpty(request["PageModDate"]) ? DateTime.Now : Convert.ToDateTime(request["PageModDate"]);
            this.PageModUser = request["PageModUser"] ?? "";
            this.PageUpdated = request["PageUpdated"] ?? "";
        }
    }
}