using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.Test.Core
{
    public class AccessorialOverrider
    {

        public AccessorialOverrider()
        {

        }

        private const string API_FFE = "FRZF";
        private const string API_HMB = "HMBY";
        private const string API_CHR = "RBTW";
        private const string API_GTZ = "GTZT";
        private const string API_ESTES = "EXLA";

        /// <summary>
        /// If returns empty list, then caller should ignore it. Else caller should use these hard-coded override values
        /// </summary>
        /// <param name="ApiSource"></param>
        /// <returns></returns>
        public List<AccessorialMinimalEntity> HackIt(string ApiSource)
        {
            List<AccessorialMinimalEntity> newAccList = new List<AccessorialMinimalEntity>();

            switch (ApiSource)
            {
                //Override
                case API_FFE:
                    //Could override here
                    break;

                case API_HMB:
                    //Could override here
                    break;

                case API_CHR:
                    //Could override here
                    break;

                case API_GTZ:
                    newAccList = new List<AccessorialMinimalEntity>()
                    {
                        new AccessorialMinimalEntity() {EdiCode = "UND", NumericValue = 35.00M, Description= "Driver Unload"},
                        new AccessorialMinimalEntity() {EdiCode = "RES", NumericValue = 0, Description= "Residential Pickup or Delivery"},
                        new AccessorialMinimalEntity() {EdiCode = "EXD", NumericValue = 120.00M, Description= "School Pickup or Delviery"},
                        new AccessorialMinimalEntity() {EdiCode = "ACH", NumericValue = 120.00M, Description= "Prison Pickup or Delivery"},
                        new AccessorialMinimalEntity() {EdiCode = "MNC", NumericValue = 0.00M, Description= "Delivery Appointment"},
                        new AccessorialMinimalEntity() {EdiCode = "NFY", NumericValue = 0, Description= "Notify Before Delivery"},
                        new AccessorialMinimalEntity() {EdiCode = "IDL", NumericValue = 35.00M, Description= "Inside Delivery"},
                        new AccessorialMinimalEntity() {EdiCode = "LFT", NumericValue = 35.00M, Description= "Liftgate Pickup or Delivery"}
                    };
                    break;

                case API_ESTES:
                    //Could override here
                    break;

            }

            return newAccList;
        }

    }


    public class AccessorialMinimalEntity
    {
        public string EdiCode { get; set; }

        public decimal NumericValue { get; set; }
        public string Description { get; set; }


    }
}