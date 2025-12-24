using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDILoop class for Loop
    /// </summary>
    /// <remarks>
    /// Created By SRP on 2/26/18
    /// </remarks>
    public class EDILoop
    {
        /// <summary>
        /// Property LoopControl Control/ID as INT
        /// </summary>
        public int LoopControl { get; set; }
        /// <summary>
        /// LoopName Name Property as STRING
        /// </summary>
        public string LoopName { get; set; }
        /// <summary>
        /// LoopDesc Property as string
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string LoopDesc { get; set; }
        /// <summary>
        /// LoopMinCount Property as INT
        /// </summary>
        public string LoopMinCount { get; set; }
        /// <summary>
        /// LoopMaxCount Name Property as INT
        /// </summary>
        public string LoopMaxCount { get; set; }

        /// <summary>
        /// LoopDisabled Property as BOOLEAN
        /// </summary>
        public bool LoopDisabled { get; set; }
        /// <summary>
        /// LoopCreateDate Property as DateTime
        /// </summary>
        public DateTime LoopCreateDate { get; set; }
        private string _LoopCreateDate;
        //Modified by SRP on 2/26/18
        /// <summary>
        /// LoopCreateUser Property as String
        /// </summary>
        public string LoopCreateUser
        {
            get { return _LoopCreateDate.Left(100); } //uses extension string method Left
            set { _LoopCreateDate = value.Left(100); }//Modified by SRP on 2/26/18
        }
        /// <summary>
        /// LoopModDate Property as DateTime
        /// </summary>
        public DateTime LoopModDate { get; set; }

        private string _LoopModUser;
        /// <summary>
        /// LoopModUser Property as String
        /// </summary>
        public string LoopModUser
        {
            get { return _LoopModUser.Left(100); } //uses extension string method Left
            set { _LoopModUser = value.Left(100); }// Modified by SRP on 2/26/18

        }


        private byte[] _LoopUpdated;
        /// <summary>
        /// LoopUpdated Property as STRING
        /// </summary>
        public string LoopUpdated
        {
            get
            {
                if (this._LoopUpdated != null)
                {

                    return Convert.ToBase64String(this._LoopUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LoopUpdated = null;

                }

                else
                {

                    this._LoopUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _LoopUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the SegmentUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _LoopUpdated; }
    }
}