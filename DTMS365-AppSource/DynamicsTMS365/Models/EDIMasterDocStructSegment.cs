using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIMasterDocStructSegment class for Master Doc Structure segments
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/28/18
    /// </remarks>
    public class EDIMasterDocStructSegment
    {
        /// <summary>
        /// Property MDSSegControl Control/ID as INT
        /// </summary>
        public int MDSSegControl { get; set; }
        /// <summary>
        /// MDSSegMDSLoopControl Name Property as INT
        /// </summary>
        public int MDSSegMDSLoopControl { get; set; }
        /// <summary>
        /// MDSSegSegmentControl Property as INT
        /// </summary>
        public int MDSSegSegmentControl { get; set; }
        /// <summary>
        /// MDSSegUsage Name Property as STRING
        /// </summary>
        public string MDSSegUsage { get; set; }
        /// <summary>
        /// MDSSegDesc Name Property as STRING
        /// </summary>
        public string MDSSegDesc { get; set; }
        /// <summary>
        /// MDSSegMinCount Name Property as int
        /// </summary>
        public int MDSSegMinCount { get; set; }
        /// <summary>
        /// MDSSegMaxCount Name Property as int
        /// </summary>
        public int MDSSegMaxCount { get; set; }
        /// <summary>
        /// MDSSegSeqIndex Name Property as int
        /// </summary>
        public int MDSSegSeqIndex { get; set; }
        /// <summary>
        /// MDSSegDisabled Property as BOOLEAN
        /// </summary>
        public bool MDSSegDisabled { get; set; }
        /// <summary>
        /// ElementCreateDate Property as DateTime
        /// </summary>
        public DateTime MDSSegCreateDate { get; set; }
        private string _MDSSegCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string MDSSegCreateUser
        {
            get { return _MDSSegCreateUser.Left(100); } //uses extension string method Left
            set { _MDSSegCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// ElementModDate Property as DateTime
        /// </summary>
        public DateTime MDSSegModDate { get; set; }

        private string _MDSSegModUser;
        /// <summary>
        /// ElementModUser Property as String
        /// </summary>
        public string MDSSegModUser
        {
            get { return _MDSSegModUser.Left(100); } //uses extension string method Left
            set { _MDSSegModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _MDSSegUpdated;
        /// <summary>
        /// MDSSegUpdated Property as STRING
        /// </summary>
        public string MDSSegUpdated
        {
            get
            {
                if (this._MDSSegUpdated != null)
                {

                    return Convert.ToBase64String(this._MDSSegUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._MDSSegUpdated = null;

                }

                else
                {

                    this._MDSSegUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _MDSSegUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _MDSSegUpdated; }
        /// <summary>
        /// SegmentControl Property as INT
        /// </summary>
        public int SegmentControl { get; set; }
        /// <summary>
        /// SegmentName Property as STRING
        /// </summary>
        public string SegmentName { get; set; }
    }
}