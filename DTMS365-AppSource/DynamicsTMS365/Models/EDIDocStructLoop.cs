using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocStructLoops class for Master Doc Structure loops
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/2/18
    /// </remarks>
    public class EDIDocStructLoop
    {
        /// <summary>
        /// Property DSLoopControl Control/ID as INT
        /// </summary>
        public int DSLoopControl { get; set; }
        /// <summary>
        /// DSLoopTPDocControl Name Property as INT
        /// </summary>
        public int DSLoopTPDocControl { get; set; }
        /// <summary>
        /// DSLoopLoopControl Property as INT
        /// </summary>
        public int DSLoopLoopControl { get; set; }
        /// <summary>
        /// DSLoopParentLoopID Property as INT
        /// </summary>
        public int DSLoopParentLoopID { get; set; }
        /// <summary>
        /// DSLoopUsage Name Property as STRING
        /// </summary>
        public string DSLoopUsage { get; set; }
        /// <summary>
        /// DSLoopMinCount Name Property as int
        /// </summary>
        public int DSLoopMinCount { get; set; }
        /// <summary>
        /// DSLoopMaxCount Name Property as int
        /// </summary>
        public int DSLoopMaxCount { get; set; }
        /// <summary>
        /// DSLoopSeqIndex Property as int
        /// </summary>
        public int DSLoopSeqIndex { get; set; }
        /// <summary>
        /// DSLoopDisabled Property as BOOLEAN
        /// </summary>
        public bool DSLoopDisabled { get; set; }
        /// <summary>
        /// DSLoopCreateDate Property as DateTime
        /// </summary>
        public DateTime DSLoopCreateDate { get; set; }
        private string _DSLoopCreateUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string DSLoopCreateUser
        {
            get { return _DSLoopCreateUser.Left(100); } //uses extension string method Left
            set { _DSLoopCreateUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// DSLoopModDate Property as DateTime
        /// </summary>
        public DateTime DSLoopModDate { get; set; }

        private string _DSLoopModUser;
        /// <summary>
        /// DSLoopModUser Property as String
        /// </summary>
        public string DSLoopModUser
        {
            get { return _DSLoopModUser.Left(100); } //uses extension string method Left
            set { _DSLoopModUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _DSLoopUpdated;
        /// <summary>
        /// DSLoopUpdated Property as STRING
        /// </summary>
        public string DSLoopUpdated
        {
            get
            {
                if (this._DSLoopUpdated != null)
                {

                    return Convert.ToBase64String(this._DSLoopUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._DSLoopUpdated = null;

                }

                else
                {

                    this._DSLoopUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _DSLoopUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the DSLoopUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _DSLoopUpdated; }
    }
}