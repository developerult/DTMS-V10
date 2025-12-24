using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Parameter 
    {
                  
        public string ParKey { get; set; }
        public string ParDescription { get; set; }
        //public Binary ParOLE { get; set; }
        public string ParText { get; set; }
        public double ParValue { get; set; }
        public int ParCategoryControl { get; set; }
        public bool ParIsGlobal { get; set; }
        //special field not normally used  should be flagged as visible false and readonly:
        public string msrepl_tran_version { get; set; }
        public string ParOLE { get; set; }
        public string rowguid { get; set; }

        private byte[] _ParUpdated;
        public string ParUpdated
        {
            get
            {
                if (this._ParUpdated != null)
                {

                    return Convert.ToBase64String(this._ParUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._ParUpdated = null;

                }

                else
                {

                    this._ParUpdated = Convert.FromBase64String(value);

                }

            }
        }


        public void setUpdated(byte[] val) { _ParUpdated = val; }
        public byte[] getUpdated() { return _ParUpdated; }


        public void fillFromRequest(HttpRequest request)
        {
            int intVal = 0;
            bool blnVal = true;
            double dblVal = 0;
            int.TryParse(request["ParCategoryControl"] ?? "0", out intVal);
            this.ParCategoryControl = intVal;
            this.ParDescription = request["ParDescription"] ?? "";
            this.ParKey = request["ParKey"] ?? "";
            this.ParText = request["ParText"] ?? "";
            blnVal = true;
            bool.TryParse(request["ParIsGlobal"] ?? "true", out blnVal);
            this.ParIsGlobal = blnVal;            
            this.ParUpdated = request["ParUpdated"] ?? "";
        }
    }
}