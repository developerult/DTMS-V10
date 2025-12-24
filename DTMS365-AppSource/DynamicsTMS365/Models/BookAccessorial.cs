using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class BookAccessorial
    {
        public int AccessorialCode { get; set; }
        public int BookAcssBookControl { get; set; }
        public int BookAcssControl { get; set; }
        public decimal BookAcssValue { get; set; }
        public int BookAcssNACControl { get; set; }
        public string NACCode { get; set; }
        public string NACName { get; set; }
        public string BookAcssModDate { get; set; }
        public string BookAcssModUser { get; set; }


        private byte[] _BookAcssUpdated;

        /// <summary>
        /// BookAcssUpdated should be bound to UI, _BookAcssUpdated is only bound on the controller
        /// </summary>
        public string BookAcssUpdated
        {
            get
            {
                if (this._BookAcssUpdated != null)
                {

                    return Convert.ToBase64String(this._BookAcssUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BookAcssUpdated = null;

                }

                else
                {

                    this._BookAcssUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BookAcssUpdated = val; }
        public byte[] getUpdated() { return _BookAcssUpdated; }


    }
}