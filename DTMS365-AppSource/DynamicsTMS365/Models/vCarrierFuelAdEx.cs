using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using DTran = Ngl.Core.Utility.DataTransformation;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// A model of the CarrFuelAdExCarrFuelAd view
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.4.005 on 03/28/2024 added new Carrier Specific Zone logic
    /// </remarks>
    public class vCarrierFuelAdEx
    {



        public int CarrFuelAdExCarrFuelAdContol { get; set; }

        public int CarrFuelAdExControl { get; set; }

        public string CarrFuelAdExEffDate { get; set; }

        public string CarrFuelAdExModDate { get; set; }

        public string CarrFuelAdExModUser { get; set; }

        public decimal? CarrFuelAdExPercent { get; set; }

        public decimal? CarrFuelAdExRatePerMile { get; set; }

        public string CarrFuelAdExState { get; set; }

        private bool _CarrFuelAdExUseExceptionDefault = true;
        public bool? CarrFuelAdExUseExceptionDefault
        {
            get
            {
                return _CarrFuelAdExUseExceptionDefault;
            }
            set
            {
                _CarrFuelAdExUseExceptionDefault = value ?? true;
            } 
        }

        private bool _CarrFuelAdExCalcAvgWithNatAverage = false;
        public bool? CarrFuelAdExCalcAvgWithNatAverage
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithNatAverage;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithNatAverage = value ?? false;
            }
        }
        private bool _CarrFuelAdExCalcAvgWithZone1 = false;
        public bool? CarrFuelAdExCalcAvgWithZone1
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone1;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone1 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone2 = false;
        public bool? CarrFuelAdExCalcAvgWithZone2
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone2;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone2 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone3 = false;
        public bool? CarrFuelAdExCalcAvgWithZone3
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone3;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone3 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone4 = false;
        public bool? CarrFuelAdExCalcAvgWithZone4
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone4;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone4 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone5 = false;
        public bool? CarrFuelAdExCalcAvgWithZone5
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone5;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone5 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone6 = false;
        public bool? CarrFuelAdExCalcAvgWithZone6
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone6;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone6 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone7 = false;
        public bool? CarrFuelAdExCalcAvgWithZone7
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone7;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone7 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone8 = false;
        public bool? CarrFuelAdExCalcAvgWithZone8
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone8;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone8 = value ?? false;
            }
        }

        private bool _CarrFuelAdExCalcAvgWithZone9 = false;
        public bool? CarrFuelAdExCalcAvgWithZone9
        {
            get
            {
                return _CarrFuelAdExCalcAvgWithZone9;
            }
            set
            {
                _CarrFuelAdExCalcAvgWithZone9 = value ?? false;
            }
        }

        public string CalcNatAverageLabel{ get; set; }

       public string CalcNatAverageValue{ get; set; }

       public string CalcAvgWithZone1Label{ get; set; }

       public string CalcAvgWithZone1Value{ get; set; }

       public string CalcAvgWithZone2Label{ get; set; }

       public string CalcAvgWithZone2Value{ get; set; }

       public string CalcAvgWithZone3Label{ get; set; }

       public string CalcAvgWithZone3Value{ get; set; }

       public string CalcAvgWithZone4Label{ get; set; }

       public string CalcAvgWithZone4Value{ get; set; }

       public string CalcAvgWithZone5Label{ get; set; }

       public string CalcAvgWithZone5Value{ get; set; }

       public string CalcAvgWithZone6Label{ get; set; }

       public string CalcAvgWithZone6Value{ get; set; }

       public string CalcAvgWithZone7Label{ get; set; }

       public string CalcAvgWithZone7Value{ get; set; }

       public string CalcAvgWithZone8Label{ get; set; }

       public string CalcAvgWithZone8Value{ get; set; }

       public string CalcAvgWithZone9Label{ get; set; }

       public string CalcAvgWithZone9Value{ get; set; }


        private byte[] _CarrFuelAdExUpdated;


        public string CarrFuelAdExUpdated
        {
            get
            {
                if (this._CarrFuelAdExUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrFuelAdExUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrFuelAdExUpdated = null;

                }

                else
                {

                    this._CarrFuelAdExUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrFuelAdExUpdated = val; }
        public byte[] getUpdated() { return _CarrFuelAdExUpdated; }


        public static Models.vCarrierFuelAdEx selectModelData(LTS.vCarrierFuelAdEx d)
        {
            Models.vCarrierFuelAdEx modelRecord = new Models.vCarrierFuelAdEx();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrFuelAdExUpdated", "CarrierFuelAddendum", "CarrFuelAdExModDate", "CarrFuelAdExEffDate" };
                string sMsg = "";
                modelRecord = (Models.vCarrierFuelAdEx)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    modelRecord.setUpdated(d.CarrFuelAdExUpdated.ToArray());
                    modelRecord.CarrFuelAdExEffDate = Utilities.convertDateToShortDateString(d.CarrFuelAdExEffDate);
                    modelRecord.CarrFuelAdExModDate = Utilities.convertDateToDateTimeString(d.CarrFuelAdExModDate);
                }
            }

            return modelRecord;
        }


        private LTS.vCarrierFuelAdEx selectLTSData(Models.vCarrierFuelAdEx d)
        {
            LTS.vCarrierFuelAdEx ltsRecord = new LTS.vCarrierFuelAdEx();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "CarrFuelAdExUpdated", "CarrierFuelAddendum", "CarrFuelAdExModDate", "CarrFuelAdExEffDate" };
                string sMsg = "";
                ltsRecord = (LTS.vCarrierFuelAdEx)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsRecord, d, skipObjs, ref sMsg);
                if (ltsRecord != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsRecord.CarrFuelAdExUpdated = bupdated == null ? new byte[0] : bupdated;
                    ltsRecord.CarrFuelAdExModDate = Utilities.convertStringToDateTime(d.CarrFuelAdExModDate);
                    ltsRecord.CarrFuelAdExEffDate = Utilities.convertStringToDateTime(d.CarrFuelAdExEffDate);


                }
            }

            return ltsRecord;
        }
    }
}