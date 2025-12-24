using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocumentType class for Document Types
    /// </summary>
    /// <remarks>
    /// Created By SN on 2/14/18
    /// </remarks>
    public class EDIDocumentType
    {
        /// <summary>
        /// Property EDITControl Control/ID as INT
        /// </summary>
        public int EDITControl { get; set; }
        /// <summary>
        /// EDITName Name Property as STRING
        /// </summary>
        public string EDITName { get; set; }
        /// <summary>
        /// EDITDescription Property as string
        /// </summary>
        public string EDITDescription { get; set; }
        /// <summary>
        /// EDITDisabled Property as BOOLEAN
        /// </summary>
        public bool EDITDisabled { get; set; }
        /// <summary>
        /// EDITCreateDate Property as DateTime
        /// </summary>
        public DateTime EDITCreateDate { get; set; }
        private string _EDITCreateUser; //Modified by SN on 2/21/18
        /// <summary>
        /// EDITCreateUser Property as String
        /// </summary>
        public string EDITCreateUser {
            get { return _EDITCreateUser.Left(100); } //uses extension string method Left
            set { _EDITCreateUser = value.Left(100); }//Modified by SN on 2/21/18
        }
        /// <summary>
        /// EDITModDate Property as DateTime
        /// </summary>
        public DateTime EDITModDate { get; set; }

        private string _EDITModeUser;
        /// <summary>
        /// EDITModUser Property as String
        /// </summary>
        public string EDITModUser
        {
            get { return _EDITModeUser.Left(100); } //uses extension string method Left
            set { _EDITModeUser = value.Left(100); }// Modified by SN on 2/21/18

        }


        private byte[] _EDITUpdated;
        /// <summary>
        /// EDITUpdated Property as STRING
        /// </summary>
        public string EDITUpdated
        {
            get
            {
                if (this._EDITUpdated != null)
                {

                    return Convert.ToBase64String(this._EDITUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._EDITUpdated = null;

                }

                else
                {

                    this._EDITUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _EDITUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the EDIDocumentTypeUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _EDITUpdated; }
    }
}