using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;


namespace DynamicsTMS365.Models
{
    public class vLBLoadStatus365
    {

        public int BookTrackControl { get; set; }

        public int BookTrackBookControl { get; set; }

        public string BookTrackDate { get; set; }

        public string BookTrackContact { get; set; }

        public string BookTrackComment { get; set; }

        public int? BookTrackStatus { get; set; }

        public string BookTrackCommentLocalized { get; set; }

        public string BookTrackCommentKeys { get; set; }

        public string BookTrackModUser { get; set; }

        private string BookTrackModDate { get; set; }        

        public string BookSHID { get; set; }

        public string BookConsPrefix { get; set; }

        public string BookCarrOrderNumber { get; set; }

        public string BookProNumber { get; set; }

        public string LoadStatusDesc { get; set; }

        public int? LoadStatusControl { get; set; }

        public int? LoadStatusCode { get; set; }


        private byte[] _BookTrackUpdated;

        /// <summary>
        /// BookFeesUpdated should be bound to UI, _BookFeesUpdated is only bound on the controller
        /// </summary>
        public string BookTrackUpdated
        {
            get
            {
                if (this._BookTrackUpdated != null) { return Convert.ToBase64String(this._BookTrackUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._BookTrackUpdated = null; } else { this._BookTrackUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _BookTrackUpdated = val; }
        public byte[] getUpdated() { return _BookTrackUpdated; }



        /// <summary>
        /// Transform LTS data to 365 Model data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.006 on 04/11/2024
        /// </remarks>
        public static Models.vLBLoadStatus365 selectModelData(LTS.vLBLoadStatus365 d)
        {
            Models.vLBLoadStatus365 modelRecord = new Models.vLBLoadStatus365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookTrackUpdated", "BookTrackDate", "BookTrackModDate" };
                string sMsg = "";
                modelRecord = (Models.vLBLoadStatus365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null)
                {
                    modelRecord.setUpdated(d.BookTrackUpdated.ToArray());
                    modelRecord.BookTrackDate = Utilities.convertDateToDateTimeString(d.BookTrackDate);
                    modelRecord.BookTrackModDate = Utilities.convertDateToDateTimeString(d.BookTrackModDate);

                }

            }

            return modelRecord;
        }

        /// <summary>
        /// Translate 365 Model data to LTS View data
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.4.006 on 04/11/2024
        /// </remarks>
        public static LTS.vLBLoadStatus365 selectLTSData(Models.vLBLoadStatus365 d)
        {
            LTS.vLBLoadStatus365 ltsRecord = new LTS.vLBLoadStatus365();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookTrackUpdated", "BookTrackDate", "BookTrackModDate" };
                string sMsg = "";
                ltsRecord = (LTS.vLBLoadStatus365)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.BookTrackUpdated = bupdated == null ? new byte[0] : bupdated;
                    ltsRecord.BookTrackDate = Utilities.convertStringToDateTime(d.BookTrackDate);
                    ltsRecord.BookTrackModDate = Utilities.convertStringToDateTime(d.BookTrackModDate);

                }
            }

            return ltsRecord;
        }

    }
}