using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIMasterDocStructElmntAttribute class for Master Doc Structure element attribute
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/28/18
    /// </remarks>
    public class EDIMasterDocStructElmntAttribute
    {
        /// <summary>
        /// Property MDSAttrControl Control/ID as INT
        /// </summary>
        public int MDSAttrControl { get; set; }
        /// <summary>
        /// MDSAttrMDSElementControl Name Property as INT
        /// </summary>
        public int MDSAttrMDSElementControl { get; set; }
        /// <summary>
        /// MDSAttrQualifyingElementControl Property as INT
        /// </summary>
        public int MDSAttrQualifyingElementControl { get; set; }
        /// <summary>
        /// MDSAttrQualifyingValue Property as STRING
        /// </summary>
        public string MDSAttrQualifyingValue { get; set; }
        /// <summary>
        /// MDSAttrUsage Property as STRING
        /// </summary>
        public string MDSAttrUsage { get; set; }
        /// <summary>
        /// MDSAttrNotes Property as STRING
        /// </summary>
        public string MDSAttrNotes { get; set; }
        /// <summary>
        /// MDSAttrMinDataLength Name Property as int
        /// </summary>
        public int MDSAttrMinDataLength { get; set; }
        /// <summary>
        /// MDSAttrMaxDataLength Name Property as int
        /// </summary>
        public int MDSAttrMaxDataLength { get; set; }
        /// <summary>
        /// MDSAttrDefaultVal Name Property as STRING
        /// </summary>
        public string MDSAttrDefaultVal { get; set; }
        /// <summary>
        /// MDSAttrTransformationTypeControl Name Property as int
        /// </summary>
        public int MDSAttrTransformationTypeControl { get; set; }
        /// <summary>
        /// MDSAttrValidationTypeControl Name Property as int
        /// </summary>
        public int MDSAttrValidationTypeControl { get; set; }
        /// <summary>
        /// MDSAttrFormattingFnControl Name Property as int
        /// </summary>
        public int MDSAttrFormattingFnControl { get; set; }
        /// <summary>
        /// MDSAttrDataMapFieldControl Name Property as int
        /// </summary>
        public int MDSAttrDataMapFieldControl { get; set; }
        /// <summary>
        /// MDSAttrDataMapFieldColumnControl Name Property as int
        /// </summary>
        public int MDSAttrDataMapFieldColumnControl { get; set; }
        /// <summary>
        /// MDSAttrDisabled Property as BOOLEAN
        /// </summary>
        public bool MDSAttrDisabled { get; set; }
        /// <summary>
        /// ElementCreateDate Property as DateTime
        /// </summary>
        public DateTime MDSAttrCreateDate { get; set; }
        private string _MDSAttrCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string MDSAttrCreateUser
        {
            get { return _MDSAttrCreateUser.Left(100); } //uses extension string method Left
            set { _MDSAttrCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// MDSAttrModDate Property as DateTime
        /// </summary>
        public DateTime MDSAttrModDate { get; set; }

        private string _MDSAttrModUser;
        /// <summary>
        /// ElementModUser Property as String
        /// </summary>
        public string MDSAttrModUser
        {
            get { return _MDSAttrModUser.Left(100); } //uses extension string method Left
            set { _MDSAttrModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _MDSAttrModUpdated;
        /// <summary>
        /// MDSSegUpdated Property as STRING
        /// </summary>
        public string MDSAttrModUpdated
        {
            get
            {
                if (this._MDSAttrModUpdated != null)
                {

                    return Convert.ToBase64String(this._MDSAttrModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._MDSAttrModUpdated = null;

                }

                else
                {

                    this._MDSAttrModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _MDSAttrModUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _MDSAttrModUpdated; }
    }
}