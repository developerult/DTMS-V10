using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class cmPageDetail
    {
        //tree list fields
       public int? parentId { get; set; }

        private bool _expanded = true;
       public bool expanded {
            get { return _expanded; }
            set { _expanded = value; }
        }

        public bool PageDetAllowFilter { get; set; }
        public bool PageDetAllowPaging { get; set; }
        public bool PageDetAllowSort { get; set; }
        
        private string _PageDetAPIFilterID;
        public string PageDetAPIFilterID
        {
            get { return _PageDetAPIFilterID.Left(20); } //uses extension string method Left
            set { _PageDetAPIFilterID = value.Left(20); }
        }

        private string _PageDetAPIReference;
        public string PageDetAPIReference
        {
            get { return _PageDetAPIReference.Left(255); } //uses extension string method Left
            set { _PageDetAPIReference = value.Left(255); }
        }

        private string _PageDetAPISortKey;
        public string PageDetAPISortKey
        {
            get { return _PageDetAPISortKey.Left(20); } //uses extension string method Left
            set { _PageDetAPISortKey = value.Left(20); }
        }
        private string _PageDetAttributes;
        public string PageDetAttributes
        {
            get { return _PageDetAttributes.Left(255); } //uses extension string method Left
            set { _PageDetAttributes = value.Left(255); }
        }
        
        private string _PageDetCaption;
        public string PageDetCaption
        {
            get { return _PageDetCaption.Left(255); } //uses extension string method Left
            set { _PageDetCaption = value.Left(255); }
        }


        private string _PageDetCaptionLocal;
        public string PageDetCaptionLocal
        {
            get { return _PageDetCaptionLocal.Left(4000); } //uses extension string method Left
            set { _PageDetCaptionLocal = value.Left(4000); }
        }
        public int PageDetControl { get; set; }
        private string _PageDetCSSClass;
        public string PageDetCSSClass
        {
            get { return _PageDetCSSClass.Left(20); } //uses extension string method Left
            set { _PageDetCSSClass = value.Left(20); }
        }
        public int PageDetDataElmtControl { get; set; }
        private string _PageDetDesc;
        public string PageDetDesc
        {
            get { return _PageDetDesc.Left(255); } //uses extension string method Left
            set { _PageDetDesc = value.Left(255); }
        }
        public int PageDetElmtFieldControl { get; set; }
        public bool PageDetExpanded { get; set; }
        public string PageDetFieldFormatOverride { get; set; }
        public string PageDetFieldTemplateOverride { get; set; }

        public string PageDetWidgetAction { get; set; }
        public string PageDetWidgetActionKey { get; set; }

        public int PageDetFilterTypeControl { get; set; }

        private string _PageDetFKReference;
        public string PageDetFKReference
        {
            get { return _PageDetFKReference.Left(255); } //uses extension string method Left
            set { _PageDetFKReference = value.Left(255); }
        }
        public int PageDetGroupSubTypeControl { get; set; }
        public int PageDetGroupTypeControl { get; set; }

        public bool PageDetInsertOnly { get; set; }
        private string _PageDetMetaData;
        public string PageDetMetaData
        {
            get { return _PageDetMetaData.Left(4000); } //uses extension string method Left
            set { _PageDetMetaData = value.Left(4000); }
        }

        public DateTime? PageDetModDate { get; set; }
        private string _PageDetModUser;
        public string PageDetModUser
        {
            get { return _PageDetModUser.Left(100); } //uses extension string method Left
            set { _PageDetModUser = value.Left(100); }
        }

        private string _PageDetName;
        public string PageDetName
        {
            get { return _PageDetName.Left(255); } //uses extension string method Left
            set { _PageDetName = value.Left(255); }
        }
        public int PageDetOrientation { get; set; }
        public int PageDetPageControl { get; set; }
        public int PageDetPageTemplateControl { get; set; }
        public int PageDetParentID { get; set; }
        public bool PageDetReadOnly { get; set; }
        public bool PageDetRequired { get; set; }
        public int PageDetSequenceNo { get; set; }
        public int PageDetEditWndSeqNo { get; set; } //Added by LVV on 7/26/19
        public int PageDetAddWndSeqNo { get; set; } //Added by LVV on 7/26/19

        private string _PageDetTagIDReference;
        public string PageDetTagIDReference
        {
            get { return _PageDetTagIDReference.Left(255); } //uses extension string method Left
            set { _PageDetTagIDReference = value.Left(255); }
        }

        private byte[] _PageDetUpdated;

        /// <summary>
        /// PageDetUpdated should be bound to UI, _PageDetUpdated is only bound on the controller
        /// </summary>
        public string PageDetUpdated
        {
            get
            {
                if (this._PageDetUpdated != null)
                {

                    return Convert.ToBase64String(this._PageDetUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._PageDetUpdated = null;

                }

                else
                {

                    this._PageDetUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _PageDetUpdated = val; }
        public byte[] getUpdated() { return _PageDetUpdated; }
        public int PageDetUserSecurityControl { get; set; }
        public bool PageDetVisible { get; set; }
        public int PageDetEditWndVisibility { get; set; } //Added by LVV on 7/26/19
        public int PageDetAddWndVisibility { get; set; } //Added by LVV on 7/26/19
        public int PageDetWidth { get; set; }

        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            bool blnVal = true;
            int.TryParse(request["PageDetControl"] ?? "0", out intVal);
            this.PageDetControl = intVal;
            intVal = 0;
            int.TryParse(request["PageDetPageControl"] ?? "0", out intVal);
            this.PageDetPageControl = intVal;
            intVal = 0;
            int.TryParse(request["PageDetGroupTypeControl"] ?? "0", out intVal);
            this.PageDetGroupTypeControl = intVal;
            intVal = 0;
            int.TryParse(request["PageDetGroupSubTypeControl"] ?? "0", out intVal);
            this.PageDetGroupSubTypeControl = intVal;
            intVal = 0;
            this.PageDetName = request["PageDetName"] ?? "";
            this.PageDetDesc = request["PageDetDesc"] ?? "";
            this.PageDetCaption = request["PageDetCaption"] ?? "";
            this.PageDetCaptionLocal = request["PageDetCaptionLocal"] ?? "";
            int.TryParse(request["PageDetSequenceNo"] ?? "0", out intVal);
            this.PageDetSequenceNo = intVal;
            intVal = 0;
            int.TryParse(request["PageDetParentID"] ?? "0", out intVal);
            this.PageDetParentID = intVal;
            intVal = 0;
            int.TryParse(request["PageDetOrientation"] ?? "0", out intVal);
            this.PageDetOrientation = intVal;
            intVal = 0;
            int.TryParse(request["PageDetWidth"] ?? "0", out intVal);
            this.PageDetWidth = intVal;
            intVal = 0;
            bool.TryParse(request["PageDetAllowFilter"] ?? "false", out blnVal);
            this.PageDetAllowFilter = blnVal;
            blnVal = false;
            int.TryParse(request["PageDetFilterTypeControl"] ?? "0", out intVal);
            this.PageDetFilterTypeControl = intVal;
            intVal = 0;
            bool.TryParse(request["PageDetAllowSort"] ?? "false", out blnVal);
            this.PageDetAllowSort = blnVal;
            blnVal = false;
            bool.TryParse(request["PageDetAllowPaging"] ?? "0", out blnVal);
            this.PageDetAllowPaging = blnVal;
            blnVal = false;
            int.TryParse(request["PageDetUserSecurityControl"] ?? "0", out intVal);
            this.PageDetUserSecurityControl = intVal;
            intVal = 0;
            blnVal = true;
            bool.TryParse(request["PageDetVisible"] ?? "1", out blnVal);
            this.PageDetVisible = blnVal;
            blnVal = false;
            bool.TryParse(request["PageDetReadOnly"] ?? "0", out blnVal);
            this.PageDetReadOnly = blnVal;
            blnVal = false;
            int.TryParse(request["PageDetDataElmtControl"] ?? "0", out intVal);
            this.PageDetDataElmtControl = intVal;
            intVal = 0;
            int.TryParse(request["PageDetElmtFieldControl"] ?? "0", out intVal);
            this.PageDetElmtFieldControl = intVal;
            intVal = 0;
            int.TryParse(request["PageDetPageTemplateControl"] ?? "0", out intVal);
            this.PageDetPageTemplateControl = intVal;
            intVal = 0;
            blnVal = true;
            bool.TryParse(request["PageDetExpanded"] ?? "1", out blnVal);
            this.PageDetExpanded = blnVal;
            blnVal = false;
            this.PageDetMetaData = request["PageDetMetaData"] ?? "";
            this.PageDetFKReference = request["PageDetFKReference"] ?? "";
            this.PageDetTagIDReference = request["PageDetTagIDReference"] ?? "";
            this.PageDetModDate = string.IsNullOrEmpty(request["PageDetModDate"]) ? DateTime.Now : Convert.ToDateTime(request["PageDetModDate"]);
            this.PageDetModUser = request["PageDetModUser"] ?? "";
            this.PageDetUpdated = request["PageDetUpdated"] ?? "";
            blnVal = true;
            bool.TryParse(request["PageDetRequired"] ?? "1", out blnVal);
            this.PageDetRequired = blnVal;
            blnVal = false;
            this.PageDetFieldFormatOverride = request["PageDetFieldFormatOverride"] ?? "";
            blnVal = true;
            this.PageDetFieldTemplateOverride = request["PageDetFieldTemplateOverride"] ?? "";
            blnVal = true;
            bool.TryParse(request["PageDetInsertOnly"] ?? "1", out blnVal);
            this.PageDetInsertOnly = blnVal;
            blnVal = false;

        }
    }

    public class createPageDetailFromFieldFilters
    {
        public int PageDetPageControl { get; set; }
        public int PageDetParentID { get; set; }
        public int PageDetElmtFieldControl { get; set; }
    }

}