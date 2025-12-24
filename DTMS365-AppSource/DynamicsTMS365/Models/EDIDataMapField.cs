using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDataMapField class for DataMap
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/27/18
    /// </remarks>
    public class EDIDataMapField
    {
        /// <summary>
        /// Property DataMapFieldControl Control/ID as INT
        /// </summary>
        public int DataMapFieldControl { get; set; }
        /// <summary>
        /// DataMapFieldEDITControl Property as int
        /// </summary>
        public int DataMapFieldEDITControl { get; set; }
        /// <summary>
        /// DataMapFieldEDIDataTypeControl Property as int
        /// </summary>
        public int DataMapFieldEDIDataTypeControl { get; set; }
        /// <summary>
        /// DataMapFieldTable Name Property as STRING
        /// </summary>
        public string DataMapFieldTable { get; set; }
        /// <summary>
        /// DataMapFieldColumn Name Property as STRING
        /// </summary>
        public string DataMapFieldColumn { get; set; }
        /// <summary>
        /// DataMapFieldDisabled Property as BOOLEAN
        /// </summary>
        public bool DataMapFieldDisabled { get; set; }
        /// <summary>
        /// DataMapFieldCreateDate Property as DateTime
        /// </summary>
        public DateTime DataMapFieldCreateDate { get; set; }
        private string _DataMapFieldCreateUser; //Modified by SRP on 2/27/18
        /// <summary>
        /// DataMapFieldCreateUser Property as String
        /// </summary>
        public string DataMapFieldCreateUser
        {
            get { return _DataMapFieldCreateUser.Left(100); } //uses extension string method Left
            set { _DataMapFieldCreateUser = value.Left(100); }//Modified by SRP on 2/27/18
        }
        /// <summary>
        /// DataMapFieldModDate Property as DateTime
        /// </summary>
        public DateTime DataMapFieldModDate { get; set; }

        private string _DataMapFieldModUser;
        /// <summary>
        /// DataMapFieldModUser Property as String
        /// </summary>
        public string DataMapFieldModUser
        {
            get { return _DataMapFieldModUser.Left(100); } //uses extension string method Left
            set { _DataMapFieldModUser = value.Left(100); }// Modified by SRP on 2/27/18

        }


        private byte[] _DataMapFieldModUpdated;
        /// <summary>
        /// DataMapFieldModUpdated Property as STRING
        /// </summary>
        public string DataMapFieldModUpdated
        {
            get
            {
                if (this._DataMapFieldModUpdated != null)
                {

                    return Convert.ToBase64String(this._DataMapFieldModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DataMapFieldModUpdated = null;

                }

                else
                {

                    this._DataMapFieldModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DataMapFieldModUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DataMapFieldModUpdated; }
    }
}