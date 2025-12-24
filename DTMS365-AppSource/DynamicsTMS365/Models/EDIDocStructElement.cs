using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocStructElement class for Master Doc Structure element
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/2/18
    /// </remarks>
    public class EDIDocStructElement
    {
        /// <summary>
        /// Property DSElementControl Control/ID as INT
        /// </summary>
        public int DSElementControl { get; set; }
        /// <summary>
        /// DSElementMDSSegControl Name Property as INT
        /// </summary>
        public int DSElementMDSSegControl { get; set; }
        /// <summary>
        /// MDSElementEDIDataTypeControl Property as INT
        /// </summary>
        public int DSElementEDIDataTypeControl { get; set; }
        /// <summary>
        /// DSElementElementControl Property as INT
        /// </summary>
        public int DSElementElementControl { get; set; }

        /// <summary>
        /// DSElementName Property as STRING
        /// </summary>
        public string DSElementName { get; set; }
        /// <summary>
        /// DSElementDesc Property as STRING
        /// </summary>
        public string DSElementDesc { get; set; }
        /// <summary>
        /// DSElementUsage Property as STRING
        /// </summary>
        public string DSElementUsage { get; set; }
        /// <summary>
        /// DSElementMinCount Property as INT
        /// </summary>
        public int DSElementMinCount { get; set; }
        /// <summary>
        /// DSElementMaxCount Property as int
        /// </summary>
        public int DSElementMaxCount { get; set; }
        /// <summary>
        /// DSElementSeqIndex Property as int
        /// </summary>
        public int DSElementSeqIndex { get; set; }
        /// <summary>
        /// DSElementDisabled Property as BOOLEAN
        /// </summary>
        public bool DSElementDisabled { get; set; }
        /// <summary>
        /// DSElementCreateDate Property as DateTime
        /// </summary>
        public DateTime DSElementCreateDate { get; set; }
        private string _DSElementCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// DSElementCreateUser Property as String
        /// </summary>
        public string DSElementCreateUser
        {
            get { return _DSElementCreateUser.Left(100); } //uses extension string method Left
            set { _DSElementCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// DSElementModDate Property as DateTime
        /// </summary>
        public DateTime DSElementModDate { get; set; }

        private string _DSElementModUser;
        /// <summary>
        /// DSLoopModUser Property as String
        /// </summary>
        public string DSElementModUser
        {
            get { return _DSElementModUser.Left(100); } //uses extension string method Left
            set { _DSElementModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _DSElementUpdated;
        /// <summary>
        /// DSElementUpdated Property as STRING
        /// </summary>
        public string DSElementUpdated
        {
            get
            {
                if (this._DSElementUpdated != null)
                {

                    return Convert.ToBase64String(this._DSElementUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DSElementUpdated = null;

                }

                else
                {

                    this._DSElementUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DSElementUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the DSLoopUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DSElementUpdated; }
        /// <summary>
        /// MDSSegMDSLoopControl Name Property as int
        /// </summary>
        public int DSSegDSLoopControl { get; set; }
        /// <summary>
        /// DSSegSegmentControl Name Property as int
        /// </summary>
        public int DSSegSegmentControl { get; set; }
    }
}