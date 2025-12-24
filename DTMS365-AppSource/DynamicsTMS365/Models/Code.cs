using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Codes
    {
        public string Code { get; set; }
        public string CodeDescription { get; set; }
        public string CodeType { get; set; }
        public byte[] _CodeUpdated { get; set; }
        public Guid rowguid { get; set; }

        public long? ID { get; set; }
        public string CodeUpdated
        {
            get
            {
                if (this._CodeUpdated != null)
                {

                    return Convert.ToBase64String(this._CodeUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CodeUpdated = null;

                }

                else
                {

                    this._CodeUpdated = Convert.FromBase64String(value);

                }

            }
        }
        public void setUpdated(byte[] val) { _CodeUpdated = val; }
        public byte[] getUpdated() { return _CodeUpdated; }
    
}
}