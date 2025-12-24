using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocStructElmntAttribute class for Master Doc Structure element attributes
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/2/18
    /// </remarks>
    public class EDIDocStructElmntAttribute
    {
        /// <summary>
        /// Property DSAttrControl Control/ID as INT
        /// </summary>
        public int DSAttrControl { get; set; }
        /// <summary>
        /// DSAttrMDSElementControl Name Property as INT
        /// </summary>
        public int DSAttrDSElementControl { get; set; }
        /// <summary>
        /// DSAttrQualifyingElementControl Property as INT
        /// </summary>
        public int DSAttrQualifyingElementControl { get; set; }
        /// <summary>
        /// DSAttrQualifyingValue Property as STRING
        /// </summary>
        public string DSAttrQualifyingValue { get; set; }
        /// <summary>
        /// DSAttrUsage Property as STRING
        /// </summary>
        public string DSAttrUsage { get; set; }
        /// <summary>
        /// DSAttrNotes Property as STRING
        /// </summary>
        public string DSAttrNotes { get; set; }
        /// <summary>
        /// DSAttrMinDataLength Property as INT
        /// </summary>
        public int DSAttrMinDataLength { get; set; }
        /// <summary>
        /// DSAttrMaxDataLength Property as int
        /// </summary>
        public int DSAttrMaxDataLength { get; set; }
        /// <summary>
        /// DSAttrDefaultVal Property as int
        /// </summary>
        public string DSAttrDefaultVal { get; set; }

        /// <summary>
        /// DSAttrTransformationTypeControl Property as INT
        /// </summary>
        public int DSAttrTransformationTypeControl { get; set; }
        /// <summary>
        /// DSAttrValidationTypeControl Property as int
        /// </summary>
        public int DSAttrValidationTypeControl { get; set; }
        /// <summary>
        /// DSAttrFormattingFnControl Property as int
        /// </summary>
        public int DSAttrFormattingFnControl { get; set; }
        /// <summary>
        /// DSAttrDataMapFieldControl Property as int
        /// </summary>
        public int DSAttrDataMapFieldControl { get; set; }
        /// <summary>
        /// DSAttrDataMapFieldColumnControl Property as int
        /// </summary>
        public int DSAttrDataMapFieldColumnControl { get; set; }
        /// <summary>
        /// DSAttrDisabled Property as BOOLEAN
        /// </summary>
        public bool DSAttrDisabled { get; set; }
        /// <summary>
        /// DSAttrCreateDate Property as DateTime
        /// </summary>
        public DateTime DSAttrCreateDate { get; set; }
        private string _DSAttrCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// DSElementCreateUser Property as String
        /// </summary>
        public string DSAttrCreateUser
        {
            get { return _DSAttrCreateUser.Left(100); } //uses extension string method Left
            set { _DSAttrCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// DSAttrModDate Property as DateTime
        /// </summary>
        public DateTime DSAttrModDate { get; set; }

        private string _DSAttrModUser;
        /// <summary>
        /// DSLoopModUser Property as String
        /// </summary>
        public string DSAttrModUser
        {
            get { return _DSAttrModUser.Left(100); } //uses extension string method Left
            set { _DSAttrModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _DSAttrModUpdated;
        /// <summary>
        /// DSElementUpdated Property as STRING
        /// </summary>
        public string DSAttrModUpdated
        {
            get
            {
                if (this._DSAttrModUpdated != null)
                {

                    return Convert.ToBase64String(this._DSAttrModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DSAttrModUpdated = null;

                }

                else
                {

                    this._DSAttrModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DSAttrModUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the DSLoopUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DSAttrModUpdated; }
    }
}