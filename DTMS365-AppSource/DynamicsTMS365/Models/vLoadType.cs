using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vLoadType
    {
        public int ID { get; set; }
        public string LoadType { get; set; }
        public string LoadTypeGroup { get; set; }
        public int rowguid { get; set; }
        private byte[] _LoadTypeUpdated;
        public string LoadTypeUpdated
        {
            get
            {
                if (this._LoadTypeUpdated != null)
                {

                    return Convert.ToBase64String(this._LoadTypeUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LoadTypeUpdated = null;

                }

                else
                {

                    this._LoadTypeUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LoadTypeUpdated = val; }
        public byte[] getUpdated() { return _LoadTypeUpdated; }
    }
}