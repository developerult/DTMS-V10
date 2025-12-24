using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIMDocStructLoop class for Master Doc Structure loop
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/28/18
    /// </remarks>
    public class EDIMDocStructLoop
    {
        /// <summary>
        /// Property MDSLoopControl Control/ID as INT
        /// </summary>
        public int MDSLoopControl { get; set; }
        /// <summary>
        /// MDSLoopMasterDocControl Name Property as INT
        /// </summary>
        public int MDSLoopMasterDocControl { get; set; }
        /// <summary>
        /// MDSLoopLoopControl Property as INT
        /// </summary>
        public int MDSLoopLoopControl { get; set; }
        /// <summary>
        /// MDSLoopParentLoopID Name Property as int
        /// </summary>
        public int MDSLoopParentLoopID { get; set; }
        /// <summary>
        /// MDSLoopUsage Name Property as STRING
        /// </summary>
        public string MDSLoopUsage { get; set; }
        /// <summary>
        /// MDSLoopMinCount Name Property as int
        /// </summary>
        public int MDSLoopMinCount { get; set; }
        /// <summary>
        /// MDSLoopMaxCount Name Property as int
        /// </summary>
        public int MDSLoopMaxCount { get; set; }
        /// <summary>
        /// MDSLoopSeqIndex Name Property as int
        /// </summary>
        public int MDSLoopSeqIndex { get; set; }
        /// <summary>
        /// MDSLoopDisabled Property as BOOLEAN
        /// </summary>
        public bool MDSLoopDisabled { get; set; }
        /// <summary>
        /// MDSLoopModDate Property as DateTime
        /// </summary>
        public DateTime MDSLoopModDate { get; set; }
        private string _MDSLoopModUser; //Modified by SRP on 2/28/18
        /// <summary>
        /// MDSLoopModUser Property as String
        /// </summary>
        public string MDSLoopModUser
        {
            get { return _MDSLoopModUser.Left(100); } //uses extension string method Left
            set { _MDSLoopModUser = value.Left(100); }//Modified by SRP on 2/28/18
        }
        /// <summary>
        /// MDSLoopCreateDate Property as DateTime
        /// </summary>
        public DateTime MDSLoopCreateDate { get; set; }

        private string _MDSLoopCreateUser;
        /// <summary>
        /// MDSLoopCreateUser Property as String
        /// </summary>
        public string MDSLoopCreateUser
        {
            get { return _MDSLoopCreateUser.Left(100); } //uses extension string method Left
            set { _MDSLoopCreateUser = value.Left(100); }// Modified by SRP on 2/28/18

        }


        private byte[] _MDSLoopUpdated;
        /// <summary>
        /// MDSLoopUpdated Property as STRING
        /// </summary>
        public string MDSLoopUpdated
        {
            get
            {
                if (this._MDSLoopUpdated != null)
                {

                    return Convert.ToBase64String(this._MDSLoopUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._MDSLoopUpdated = null;

                }

                else
                {

                    this._MDSLoopUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _MDSLoopUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _MDSLoopUpdated; }
    }
}