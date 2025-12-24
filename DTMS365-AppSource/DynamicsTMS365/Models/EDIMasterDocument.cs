using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIMasterDocument class for Document
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/27/18
    /// </remarks>
    public class EDIMasterDocument
    {
        /// <summary>
        /// Property MasterDocControl Control/ID as INT
        /// </summary>
        public int MasterDocControl { get; set; }
        /// <summary>
        /// MasterDocEDITControl Property as INT
        /// </summary>
        public int MasterDocEDITControl { get; set; }
        /// <summary>
        /// MasterDocInbound Property as BOOLEAN
        /// </summary>
        public bool MasterDocInbound { get; set; }
        /// <summary>
        /// MasterDocPublished Property as BOOLEAN
        /// </summary>
        public bool MasterDocPublished { get; set; }
        /// <summary>
        /// MasterDocDisabled Property as BOOLEAN
        /// </summary>
        public bool MasterDocDisabled { get; set; }
        /// <summary>
        /// MasterDocCreateDate Property as DateTime
        /// </summary>
        public DateTime MasterDocCreateDate { get; set; }
        private string _MasterDocCreateUser; //Modified by SRP on 2/27/18
        /// <summary>
        /// MasterDocCreateUser Property { get; set; }
        /// </summary>
        public string MasterDocCreateUser
        {
            get { return _MasterDocCreateUser.Left(100); } //uses extension string method Left
            set { _MasterDocCreateUser = value.Left(100); }//Modified by SRP on 2/27/18
        }
        /// <summary>
        /// MasterDocModDate Property as DateTime
        /// </summary>
        public DateTime MasterDocModDate { get; set; }

        private string _MasterDocModUser;
        /// <summary>
        /// MasterDocModUser Property { get; set; }
        /// </summary>
        public string MasterDocModUser
        {
            get { return _MasterDocModUser.Left(100); } //uses extension string method Left
            set { _MasterDocModUser = value.Left(100); }// Modified by SRP on 2/27/18

        }


        private byte[] _MasterDocModUpdated;
        /// <summary>
        /// MasterDocModUpdated Property { get; set; }
        /// </summary>
        public string MasterDocModUpdated
        {
            get
            {
                if (this._MasterDocModUpdated != null)
                {

                    return Convert.ToBase64String(this._MasterDocModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._MasterDocModUpdated = null;

                }

                else
                {

                    this._MasterDocModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _MasterDocModUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _MasterDocModUpdated; }
        /// <summary>
        /// Property EDITControl Control/ID as INT
        /// </summary>
        public int EDITControl { get; set; }

        /// <summary>
        /// Property EDITName { get; set; }
        /// </summary>
        public string EDITName { get; set; }
        /// <summary>
        /// Property EDITDescription { get; set; }
        /// </summary>
        public string EDITDescription { get; set; }
        /// <summary>
        /// EDITDisabled Property as BOOLEAN
        /// </summary>
        public bool EDITDisabled { get; set; }


        public string LoopName { get; set; }


        public int LoopMinCount { get; set; }


        public int LoopMaxCount { get; set; }


        public string SegmentName { get; set; }


        public int SegmentMinCount { get; set; }


        public int SegmentMaxCount { get; set; }


        public string ElementName { get; set; }


        public int ElementEDIDataTypeControl { get; set; }


        public int ElementMinLength { get; set; }


        public int ElementMaxLength { get; set; }


        public string DSEDefaultVal { get; set; }


        public int ElementValidationTypeControl { get; set; }


        public int ElementFormattingFnControl { get; set; }

        public string SegmentDesc { get; set; }

        public string ElementDesc { get; set; }

        public string MDSElementUsage { get; set; }

        public string MDSSegUsage { get; set; }

        public string DataMapFieldTable { get; set; }


        public string DataMapFieldColumn { get; set; }

        public string MDSAttrNotes { get; set; }
        public int MDSLoopLoopControl { get; set; }


        public int MDSLoopParentLoopID { get; set; }

        public int MDSLoopSeqIndex { get; set; }

        public int MDSSegSeqIndex { get; set; }

        public string EditDescription { get; set; }

        public string Version { get; set; }

		public string MasterDocType { get; set; }

        public string MappingDetails { get; set; }

    }
}