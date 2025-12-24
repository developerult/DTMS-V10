using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocSegmentElement class for Elements
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/27/18
    /// </remarks>
    public class EDIDocSegmentElement
    {
        /// <summary>
        /// Property DSEControl Control/ID as INT
        /// </summary>
        public int DSEControl { get; set; }
        /// <summary>
        /// DSEEDITControl Property as int
        /// </summary>
        public int DSEEDITControl { get; set; }
        /// <summary>
        /// DSESegmentControl Property as int
        /// </summary>
        public int DSESegmentControl { get; set; }
        /// <summary>
        /// DSEElementControl Name Property as int
        /// </summary>
        public int DSEElementControl { get; set; }
        /// <summary>
        /// DSEPosition Name Property as int
        /// </summary>
        public int DSEPosition { get; set; }
        /// <summary>
        /// DSEDefaultVal Name Property as STRING
        /// </summary>
        public string DSEDefaultVal { get; set; }
        /// <summary>
        /// DSEFormattingFnControl Property as int
        /// </summary>
        public int DSEFormattingFnControl { get; set; }
        /// <summary>
        /// DSESeqIndex Property as int
        /// </summary>
        public int DSESeqIndex { get; set; }
        /// <summary>
        /// DSEDisabled Property as BOOLEAN
        /// </summary>
        public bool DSEDisabled { get; set; }
        /// <summary>
        /// ElementCreateDate Property as DateTime
        /// </summary>
        public DateTime DSECreateDate { get; set; }
        private string _DSECreateUser; //Modified by SRP on 2/27/18
        /// <summary>
        /// DSECreateUser Property as String
        /// </summary>
        public string DSECreateUser
        {
            get { return _DSECreateUser.Left(100); } //uses extension string method Left
            set { _DSECreateUser = value.Left(100); }//Modified by SRP on 2/27/18
        }
        /// <summary>
        /// DSEModDate Property as DateTime
        /// </summary>
        public DateTime DSEModDate { get; set; }

        private string _DSEModUser;
        /// <summary>
        /// DSEModUser Property as String
        /// </summary>
        public string DSEModUser
        {
            get { return _DSEModUser.Left(100); } //uses extension string method Left
            set { _DSEModUser = value.Left(100); }// Modified by SRP on 2/27/18

        }


        private byte[] _DSEUpdated;
        /// <summary>
        /// DSEUpdated Property as STRING
        /// </summary>
        public string DSEUpdated
        {
            get
            {
                if (this._DSEUpdated != null)
                {

                    return Convert.ToBase64String(this._DSEUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DSEUpdated = null;

                }

                else
                {

                    this._DSEUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DSEUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DSEUpdated; }         
        /// <summary>
        /// EDITName Name Property as STRING
        /// </summary>
        public string EDITName { get; set; }
        /// <summary>
        /// EDITName Name Property as STRING
        /// </summary>
        public string SegmentName { get; set; }
        /// <summary>
        /// EDITControl Name Property as INT
        /// </summary>
        public int EDITControl { get; set; }
        /// <summary>
        /// SegmentControl Name Property as INT
        /// </summary>
        public int SegmentControl { get; set; }
        /// <summary>
        /// ElementName Name Property as INT
        /// </summary>
        public string ElementName { get; set; }
    }
}