using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIMasterDocStructElement class for Master Doc Structure elements
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/28/18
    /// </remarks>
    public class EDIMasterDocStructElement
    {
        /// <summary>
        /// Property MDSElementControl Control/ID as INT
        /// </summary>
        public int MDSElementControl { get; set; }
        /// <summary>
        /// MDSElementMDSSegControl Name Property as INT
        /// </summary>
        public int MDSElementMDSSegControl { get; set; }
        /// <summary>
        /// MDSElementEDIDataTypeControl Property as INT
        /// </summary>
        public int MDSElementEDIDataTypeControl { get; set; }
        /// <summary>
        /// MDSElementDesc Property as STRING
        /// </summary>
        public string MDSElementDesc { get; set; }
        /// <summary>
        /// MDSElementName Property as STRING
        /// </summary>
        public string MDSElementName { get; set; }
        /// <summary>
        /// MDSElementUsage Name Property as STRING
        /// </summary>
        public string MDSElementUsage { get; set; }
        /// <summary>
        /// MDSElementMinCount Name Property as int
        /// </summary>
        public int MDSElementMinCount { get; set; }
        /// <summary>
        /// MDSElementMaxCount Name Property as int
        /// </summary>
        public int MDSElementMaxCount { get; set; }
        /// <summary>
        /// MDSElementSeqIndex Name Property as int
        /// </summary>
        public int MDSElementSeqIndex { get; set; }
        /// <summary>
        /// MDSElementDisabled Property as BOOLEAN
        /// </summary>
        public bool MDSElementDisabled { get; set; }
        /// <summary>
        /// MDSElementCreateDate Property as DateTime
        /// </summary>
        public DateTime MDSElementCreateDate { get; set; }
        private string _MDSElementCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string MDSElementCreateUser
        {
            get { return _MDSElementCreateUser.Left(100); } //uses extension string method Left
            set { _MDSElementCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// ElementModDate Property as DateTime
        /// </summary>
        public DateTime MDSElementModDate { get; set; }

        private string _MDSElementModUser;
        /// <summary>
        /// ElementModUser Property as String
        /// </summary>
        public string MDSElementModUser
        {
            get { return _MDSElementModUser.Left(100); } //uses extension string method Left
            set { _MDSElementModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _MDSElementUpdated;
        /// <summary>
        /// MDSSegUpdated Property as STRING
        /// </summary>
        public string MDSElementUpdated
        {
            get
            {
                if (this._MDSElementUpdated != null)
                {

                    return Convert.ToBase64String(this._MDSElementUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._MDSElementUpdated = null;

                }

                else
                {

                    this._MDSElementUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _MDSElementUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _MDSElementUpdated; }
        /// <summary>
        /// MDSSegMDSLoopControl Name Property as int
        /// </summary>
        public int MDSSegMDSLoopControl { get; set; }
        /// <summary>
        /// MDSSegSegmentControl Name Property as int
        /// </summary>
        public int MDSSegSegmentControl { get; set; }
    }
}