using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class CompContact
    {
        public int CompContControl { get; set; }
        public int CompContCompControl { get; set; }
        public string CompContName { get; set; }
        public string CompContTitle { get; set; }
        public string CompCont800 { get; set; }
        public string CompContPhone { get; set; }
        public string CompContPhoneExt { get; set; }
        public string CompContFax { get; set; }
        public string CompContEmail { get; set; }
        public bool CompContTender { get; set; }


        private byte[] _CompContUpdated;

        /// <summary>
        /// CompContUpdated should be bound to UI, _CompContUpdated is only bound on the controller
        /// </summary>
        public string CompContUpdated
        {
            get
            {
                if (this._CompContUpdated != null)
                {

                    return Convert.ToBase64String(this._CompContUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CompContUpdated = null;

                }

                else
                {

                    this._CompContUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CompContUpdated = val; }
        public byte[] getUpdated() { return _CompContUpdated; }


    }
}