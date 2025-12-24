using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDITPDocument class for Document
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/1/18
    /// </remarks>
    public class EDITPDocument
    {
        /// <summary>
        /// Property TPDocControl Control/ID as INT
        /// </summary>
        
        public int TPDocControl { get; set; }
        /// <summary>
        /// TPDocEDITControl Property as INT
        /// </summary>
        
        public int TPDocEDITControl { get; set; }
        /// <summary>
        /// TPDocCCEDIControl Property as INT
        /// </summary>
        
        public int TPDocCCEDIControl { get; set; }
        /// <summary>
        /// TPDocInbound Property as BOOLEAN
        /// </summary>
       
        public bool TPDocInbound { get; set; }
        /// <summary>
        /// TPDocPublished Property as BOOLEAN
        /// </summary>
        
        public bool TPDocPublished { get; set; }
        /// <summary>
        /// TPDocDisabled Property as BOOLEAN
        /// </summary>
        
        public bool TPDocDisabled { get; set; }
        /// <summary>
        /// TPDocCreateDate Property as DateTime
        /// </summary>
        
        public DateTime TPDocCreateDate { get; set; }
        
        private string _TPDocCreateUser; //Modified by SRP on 3/1/18
        /// <summary>
        /// TPDocCreateUser Property as String
        /// </summary>
        
        public string TPDocCreateUser
        {
            get { return _TPDocCreateUser.Left(100); } //uses extension string method Left
            set { _TPDocCreateUser = value.Left(100); }//Modified by SRP on 3/1/18
        }
        /// <summary>
        /// TPDocModDate Property as DateTime
        /// </summary>
        
        public DateTime TPDocModDate { get; set; }
        
        private string _TPDocModUser;
        /// <summary>
        /// TPDocModUser Property as String
        /// </summary>
        
        public string TPDocModUser
        {
            get { return _TPDocModUser.Left(100); } //uses extension string method Left
            set { _TPDocModUser = value.Left(100); }// Modified by SRP on 3/1/18

        }


        private byte[] _TPDocModUpdated;
        /// <summary>
        /// TPDocModUpdated Property { get; set; }
        /// </summary>
        public string TPDocModUpdated
        {
            get
            {
                if (this._TPDocModUpdated != null)
                {

                    return Convert.ToBase64String(this._TPDocModUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._TPDocModUpdated = null;

                }

                else
                {


                    this._TPDocModUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _TPDocModUpdated = val; }


        /// <summary>
        /// getUpdated Method for getting the TPDocModUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _TPDocModUpdated; }
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

        public string DSElementUsage { get; set; }

        public string DSSegUsage { get; set; }

        public string DataMapFieldTable { get; set; }


        public string DataMapFieldColumn { get; set; }

        public string DSAttrNotes { get; set; }

        public int DSLoopLoopControl { get; set; }


        public int DSLoopParentLoopID { get; set; }

        public int DSLoopSeqIndex { get; set; }

        public int DSSegSeqIndex { get; set; }

        public string EDITName { get; set; }

        public string EditDescription { get; set; }

        public string Version { get; set; }

        public string CarrierName { get; set; }

        public string CompName { get; set; }

		public string TPDocType { get; set; }

        public string MappingDetails { get; set; }

    }
}