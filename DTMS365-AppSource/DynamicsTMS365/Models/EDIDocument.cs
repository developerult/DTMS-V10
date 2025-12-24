using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIDocument cl s for EDIDocument
    /// </summary>
    /// <remarks>
    /// Created by SRP on 3/22/18
    /// </remarks>
    public class EDIDocument
    {
        /// <summary>
        /// Property EDITControl Control/ID   INT
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Always)]
        public int EDITControl { get; set; }
        /// <summary>
        /// EDITName Property   STRING
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Always)]
        public string EDITName { get; set; }
        /// <summary>
        /// CarrierName Property   STRING
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Always)]
        public string CarrierName { get; set; }
        /// <summary>
        /// CompName Property   STRING
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Always)]
        public string CompName { get; set; }
        /// <summary>
        /// Property CompControl INT
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Always)]
        public int CompControl { get; set; }
        /// <summary>
        /// Property CarrierControl  INT
        /// </summary>
        public int CarrierControl { get; set; }
        /// <summary>
        /// Property TPDocCCEDIControl  INT
        /// </summary>
        public int TPDocCCEDIControl { get; set; }
        /// <summary>
        /// Property TPDocControl  INT
        /// </summary>
        public int TPDocControl { get; set; }
        /// <summary>
        /// Property TPDocInbound  BOOLEN
        /// </summary>
        public bool TPDocInbound { get; set; }
        /// <summary>
        /// Property TPDocCreateUser  STRING
        /// </summary>
        public string TPDocCreateUser { get; set; }
        /// <summary>
        /// Property TPDocInbound  DateTime
        /// </summary>
        public DateTime TPDocCreateDate { get; set; }
        /// <summary>
        /// Property TPDocDisabled  BOOLEAN
        /// </summary>
        public bool TPDocDisabled { get; set; }
        /// <summary>
        /// Property TPDocPublished  BOOLEAN
        /// </summary>
        public bool TPDocPublished { get; set; }
        //private byte[] _TPDocModUpdated;
        /// <summary>
        /// LoopUpdated Property as STRING
        /// </summary>
        public string TPDocModUpdated { get; set; }
        //private byte[] _TPDocModUpdated;
        /// <summary>
        /// MasterDocControl Property as INT
        /// </summary>
        public int MasterDocControl { get; set; }
        //{
        //    get
        //    {
        //        if (this._TPDocModUpdated != null)
        //        {

        //            return Convert.ToBase64String(this._TPDocModUpdated);

        //        }
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //        {

        //            this._TPDocModUpdated = null;

        //        }

        //        else
        //        {

        //            this._TPDocModUpdated = Convert.FromBase64String(value);

        //        }

        //    }
        //}
    }
}