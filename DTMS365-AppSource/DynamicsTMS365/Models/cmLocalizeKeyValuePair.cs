using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{

    public class cmLocalizeKeyValuePair
    {

        public int cmLocalControl { get; set; }
        private string _cmLocalKey;
        public string cmLocalKey { 
            get { return _cmLocalKey.Left(100);} //uses extension string method Left
            set{_cmLocalKey = value.Left(100);} 
        }        

        public string cmLocalValue{ get; set; } 
        public string cmLocalValueLocal { get; set; } 
        public DateTime? cmLocalModDate { get; set; } 
        private string _cmLocalModUser;
        public string cmLocalModUser { 
            get { return _cmLocalModUser.Left(100);} //uses extension string method Left
            set{_cmLocalModUser = value.Left(100);} 
        }
    
    //public byte[] cmLocalUpdated]		ROWVERSION  { get; set; } //NULL,
        private byte[] _cmLocalUpdated;
        
        /// <summary>
        /// cmLocalUpdated should be bound to UI, _cmLocalUpdated is only bound on the controller
        /// </summary>
        public string cmLocalUpdated
        {
            get 
            {
                if (this._cmLocalUpdated != null)
                {

                    return Convert.ToBase64String(this._cmLocalUpdated);

                }
                return string.Empty;           
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._cmLocalUpdated = null;

                }

                else
                {

                    this._cmLocalUpdated = Convert.FromBase64String(value);

                }
            
            }
        }

        public void setUpdated(byte[] val) { _cmLocalUpdated = val; }
        public byte[] getUpdated() { return  _cmLocalUpdated; }

        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            int.TryParse(request["cmLocalControl"] ?? "0", out intVal);
            this.cmLocalControl = intVal;
            this.cmLocalKey = request["cmLocalKey"] ?? "";
            this.cmLocalValue = request["cmLocalValue"] ?? "";
            this.cmLocalValueLocal = request["cmLocalValueLocal"] ?? "";
            this.cmLocalModDate = string.IsNullOrEmpty(request["cmLocalModDate"]) ? DateTime.Now : Convert.ToDateTime(request["cmLocalModDate"]);
            this.cmLocalModUser = request["cmLocalModUser"] ?? "";
            this.cmLocalUpdated = request["cmLocalUpdated"] ?? "";


        }
        
    }
}