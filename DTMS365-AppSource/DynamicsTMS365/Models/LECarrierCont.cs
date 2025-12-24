using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class LECarrierCont
    {
        public int LECarContControl { get; set; }      
        public int LECarContNACXControl { get; set; }
        public string LECarContName { get; set; }
        public string LECarContTitle { get; set; }       
        public string LECarContPhone { get; set; }
        public string LECarContPhoneExt { get; set; }
        public string LECarContFax { get; set; }    
        public string LECarCont800 { get; set; }
        public string LECarContEmail { get; set; }
        public Boolean LECarContDefault { get; set; }

        public DateTime? LECarContModDate { get; set; }

        private string _LECarContModUser;
        public string LECarContModUser
        {
            get { return _LECarContModUser.Left(100); } //uses extension string method Left
            set { _LECarContModUser = value.Left(100); }
        }

        private byte[] _LECarContUpdated;

        /// <summary>
        /// LECarContUpdated should be bound to UI, _LECarContUpdated is only bound on the controller
        /// </summary>
        public string LECarContUpdated
        {
            get
            {
                if (this._LECarContUpdated != null)
                {

                    return Convert.ToBase64String(this._LECarContUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LECarContUpdated = null;

                }

                else
                {

                    this._LECarContUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LECarContUpdated = val; }
        public byte[] getUpdated() { return _LECarContUpdated; }


    }
}