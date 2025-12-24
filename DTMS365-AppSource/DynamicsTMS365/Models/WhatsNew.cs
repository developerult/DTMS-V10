using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class WhatsNew
    {
        public int WhatsNewControl { get; set; }
        public string WhatsNewVersion { get; set; }
        public string VersionDescription { get; set; }
        public DateTime? VersionDate { get; set; }
        public int WhatsNewFeatureTypeControl { get; set; }
        public string FeatureTypeName { get; set; }
        public string FeatureTypeDesc { get; set; }
        public string WhatsNewTitle { get; set; }
        public string WhatsNewTitleLocal { get; set; }
        public string WhatsNewNote { get; set; }
        public string WhatsNewNoteLocal { get; set; }
        public int WhatsNewSeqNo { get; set; }
        public int WhatsNewParentID { get; set; }
        public DateTime? WhatsNewModDate { get; set; }

        private string _WhatsNewModUser;
        public string WhatsNewModUser
        {
            get { return _WhatsNewModUser.Left(100); } //uses extension string method Left
            set { _WhatsNewModUser = value.Left(100); }
        }


        private byte[] _WhatsNewUpdated;

        /// <summary>
        /// WhatsNewUpdated should be bound to UI, _WhatsNewUpdated is only bound on the controller
        /// </summary>
        public string WhatsNewUpdated
        {
            get
            {
                if (this._WhatsNewUpdated != null) { return Convert.ToBase64String(this._WhatsNewUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._WhatsNewUpdated = null; } else { this._WhatsNewUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _WhatsNewUpdated = val; }
        public byte[] getUpdated() { return _WhatsNewUpdated; }

    }

    public class WhatsNewItem
    {
        public string Version { get; set; }
        public int FeatureType { get; set; }
        public int SequenceNo { get; set; }
        public string Title { get; set; }     
        public NoteItem[] Notes { get; set; }
    }

    public class NoteItem
    {
        public int Index { get; set; }
        public string Note { get; set; }
        public int SequenceNo { get; set; }
    }

}