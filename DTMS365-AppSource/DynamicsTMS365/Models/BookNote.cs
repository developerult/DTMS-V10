using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board
    public class BookNote
    {
        public int BookNotesControl { get; set; }
        public int BookNotesBookControl { get; set; }
        public string BookNotesVisable1 { get; set; }
        public string BookNotesVisable2 { get; set; }
        public string BookNotesVisable3 { get; set; }
        public string BookNotesVisable4 { get; set; }
        public string BookNotesVisable5 { get; set; }
        public string BookNotesConfidential1 { get; set; }
        public string BookNotesConfidential2 { get; set; }
        public string BookNotesConfidential3 { get; set; }
        public string BookNotesConfidential4 { get; set; }
        public string BookNotesConfidential5 { get; set; }
        public string BookNotesBookUser1 { get; set; }
        public string BookNotesBookUser2 { get; set; }
        public string BookNotesBookUser3 { get; set; }
        public string BookNotesBookUser4 { get; set; }

        public DateTime? BookNotesModDate { get; set; }

        private string _BookNotesModUser;
        public string BookNotesModUser
        {
            get { return _BookNotesModUser.Left(100); } //uses extension string method Left
            set { _BookNotesModUser = value.Left(100); }
        }

        private byte[] _BookNotesUpdated;

        /// <summary>
        /// BookNotesUpdated should be bound to UI, _BookNotesUpdated is only bound on the controller
        /// </summary>
        public string BookNotesUpdated
        {
            get
            {
                if (this._BookNotesUpdated != null) { return Convert.ToBase64String(this._BookNotesUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookNotesUpdated = null; } else { this._BookNotesUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookNotesUpdated = val; }
        public byte[] getUpdated() { return _BookNotesUpdated; }

        public string BookSHID { get; set; }
        public string BookConsPrefix { get; set; }
        public string BookProNumber { get; set; }    
        public string BookCarrOrderNumber { get; set; }
        public int BookOrderSequence { get; set; }
    }
}