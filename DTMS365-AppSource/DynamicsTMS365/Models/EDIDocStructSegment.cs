using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocStructSegment class for Master Doc Structure segment
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/2/18
    /// </remarks>
    public class EDIDocStructSegment
    {
        /// <summary>
        /// Property DSSegControl Control/ID as INT
        /// </summary>
        public int DSSegControl { get; set; }
        /// <summary>
        /// DSSegMDSLoopControl Name Property as INT
        /// </summary>
        public int DSSegDSLoopControl { get; set; }
        /// <summary>
        /// DSSegSegmentControl Property as INT
        /// </summary>
        public int DSSegSegmentControl { get; set; }
        /// <summary>
        /// DSSegDesc Property as STRING
        /// </summary>
        public string DSSegDesc { get; set; }
        
        /// <summary>
        /// DSSegUsage Property as STRING
        /// </summary>
        public string DSSegUsage { get; set; }
        /// <summary>
        /// DSSegMinCount Name Property as INT
        /// </summary>
        public int DSSegMinCount { get; set; }
        /// <summary>
        /// DSSegMaxCount Name Property as int
        /// </summary>
        public int DSSegMaxCount { get; set; }
        /// <summary>
        /// DSSegSeqIndex Property as int
        /// </summary>
        public int DSSegSeqIndex { get; set; }
        /// <summary>
        /// DSSegDisabled Property as BOOLEAN
        /// </summary>
        public bool DSSegDisabled { get; set; }
        /// <summary>
        /// DSSegCreateDate Property as DateTime
        /// </summary>
        public DateTime DSSegCreateDate { get; set; }
        private string _DSSegCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string DSSegCreateUser
        {
            get { return _DSSegCreateUser.Left(100); } //uses extension string method Left
            set { _DSSegCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// DSLoopModDate Property as DateTime
        /// </summary>
        public DateTime DSSegModDate { get; set; }

        private string _DSSegModUser;
        /// <summary>
        /// DSSegModUser Property as String
        /// </summary>
        public string DSSegModUser
        {
            get { return _DSSegModUser.Left(100); } //uses extension string method Left
            set { _DSSegModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _DSSegUpdated;
        /// <summary>
        /// DSSegUpdated Property as STRING
        /// </summary>
        public string DSSegUpdated
        {
            get
            {
                if (this._DSSegUpdated != null)
                {

                    return Convert.ToBase64String(this._DSSegUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DSSegUpdated = null;

                }

                else
                {

                    this._DSSegUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DSSegUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the DSLoopUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DSSegUpdated; }
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