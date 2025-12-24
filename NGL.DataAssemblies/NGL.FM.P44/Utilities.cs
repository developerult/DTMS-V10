using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
using P44M = P44SDK.V4.Model;


namespace NGL.FM.P44
{
    public class Utilities
    {
        #region "Shared"
    
        internal static P44M.Address.CountryEnum getCountryEnum(string s)
        {
            var strCountry = s.Trim().ToUpper();
            P44M.Address.CountryEnum retVal = P44M.Address.CountryEnum.US; //Set return value to US by default 
            if (string.IsNullOrWhiteSpace(strCountry)) { return retVal; } //If country is null return US by default
            if (strCountry == "US" || strCountry == "USA") { retVal = P44M.Address.CountryEnum.US; }
            if (strCountry == "CA" || strCountry == "CAN") { retVal = P44M.Address.CountryEnum.CA; }
            if (strCountry == "MX" || strCountry == "MEX") { retVal = P44M.Address.CountryEnum.MX; }
            return retVal;
        }

        internal static P44M.LineItem.FreightClassEnum getFreightClassEnum(string s, P44M.LineItem.FreightClassEnum def)
        {
            P44M.LineItem.FreightClassEnum retVal = def; //Set return value to parameter def by default
            switch (s)
            {
                case "50":
                    retVal = P44M.LineItem.FreightClassEnum._50;
                    break;
                case "55":
                    retVal = P44M.LineItem.FreightClassEnum._55;
                    break;
                case "60":
                    retVal = P44M.LineItem.FreightClassEnum._60;
                    break;
                case "65":
                    retVal = P44M.LineItem.FreightClassEnum._65;
                    break;
                case "70":
                    retVal = P44M.LineItem.FreightClassEnum._70;
                    break;
                case "77.5":
                    retVal = P44M.LineItem.FreightClassEnum._775;
                    break;
                case "85":
                    retVal = P44M.LineItem.FreightClassEnum._85;
                    break;
                case "92.5":
                    retVal = P44M.LineItem.FreightClassEnum._925;
                    break;
                case "100":
                    retVal = P44M.LineItem.FreightClassEnum._100;
                    break;
                case "110":
                    retVal = P44M.LineItem.FreightClassEnum._110;
                    break;
                case "125":
                    retVal = P44M.LineItem.FreightClassEnum._125;
                    break;
                case "150":
                    retVal = P44M.LineItem.FreightClassEnum._150;
                    break;
                case "175":
                    retVal = P44M.LineItem.FreightClassEnum._175;
                    break;
                case "200":
                    retVal = P44M.LineItem.FreightClassEnum._200;
                    break;
                case "250":
                    retVal = P44M.LineItem.FreightClassEnum._250;
                    break;
                case "300":
                    retVal = P44M.LineItem.FreightClassEnum._300;
                    break;
                case "400":
                    retVal = P44M.LineItem.FreightClassEnum._400;
                    break;
                case "500":
                    retVal = P44M.LineItem.FreightClassEnum._500;
                    break;
            }
            return retVal;
        }

        internal static P44M.LineItem.PackageTypeEnum getPackageTypeEnum(string s)
        {
            var strPackage = s.Trim().ToUpper();
            P44M.LineItem.PackageTypeEnum retVal = P44M.LineItem.PackageTypeEnum.PLT; //Set return value to PLT by default    
            switch (strPackage)
            {
                case "PLT":
                    retVal = P44M.LineItem.PackageTypeEnum.PLT;
                    break;
                case "BAG":
                    retVal = P44M.LineItem.PackageTypeEnum.BAG;
                    break;
                case "BALE":
                    retVal = P44M.LineItem.PackageTypeEnum.BALE;
                    break;
                case "BOX":
                    retVal = P44M.LineItem.PackageTypeEnum.BOX;
                    break;
                case "BUCKET":
                    retVal = P44M.LineItem.PackageTypeEnum.BUCKET;
                    break;
                case "PAIL":
                    retVal = P44M.LineItem.PackageTypeEnum.PAIL;
                    break;
                case "BUNDLE":
                    retVal = P44M.LineItem.PackageTypeEnum.BUNDLE;
                    break;
                case "CAN":
                    retVal = P44M.LineItem.PackageTypeEnum.CAN;
                    break;
                case "CARTON":
                    retVal = P44M.LineItem.PackageTypeEnum.CARTON;
                    break;
                case "CASE":
                    retVal = P44M.LineItem.PackageTypeEnum.CASE;
                    break;
                case "COIL":
                    retVal = P44M.LineItem.PackageTypeEnum.COIL;
                    break;
                case "CRATE":
                    retVal = P44M.LineItem.PackageTypeEnum.CRATE;
                    break;
                case "CYLINDER":
                    retVal = P44M.LineItem.PackageTypeEnum.CYLINDER;
                    break;
                case "DRUM":
                    retVal = P44M.LineItem.PackageTypeEnum.DRUM;
                    break;
                case "PIECES":
                    retVal = P44M.LineItem.PackageTypeEnum.PIECES;
                    break;
                case "REEL":
                    retVal = P44M.LineItem.PackageTypeEnum.REEL;
                    break;
                case "ROLL":
                    retVal = P44M.LineItem.PackageTypeEnum.ROLL;
                    break;
                case "SKID":
                    retVal = P44M.LineItem.PackageTypeEnum.SKID;
                    break;
                case "TUBE":
                    retVal = P44M.LineItem.PackageTypeEnum.TUBE;
                    break;
            }
            return retVal;
        }

        internal static P44M.LineItemHazmatDetail.PackingGroupEnum getPackingGroupEnum(string s)
        {
            //Set return value to I by default
            P44M.LineItemHazmatDetail.PackingGroupEnum retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;

            var strPacking = s.Trim().ToUpper();

            switch (strPacking)
            {
                case "I":
                    retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.I;
                    break;
                case "II":
                    retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.II;
                    break;
                case "III":
                    retVal = P44M.LineItemHazmatDetail.PackingGroupEnum.III;
                    break;
            }

            return retVal;
        }

        internal static string getSeverityEnumString(P44M.Message.SeverityEnum e)
        {
            string strRet = "";
            switch (e)
            {
                case P44M.Message.SeverityEnum.ERROR:
                    strRet = "ERROR";
                    break;
                case P44M.Message.SeverityEnum.WARNING:
                    strRet = "WARNING";
                    break;
                case P44M.Message.SeverityEnum.INFO:
                    strRet = "INFO";
                    break;
                default:
                    strRet = "";
                    break;
            }
            return strRet;
        }

        internal static string getSourceEnumString(P44M.Message.SourceEnum e)
        {
            string strRet = "";
            switch (e)
            {
                case P44M.Message.SourceEnum.SYSTEM:
                    strRet = "SYSTEM";
                    break;
                case P44M.Message.SourceEnum.CAPACITYPROVIDER:
                    strRet = "CAPACITY PROVIDER";
                    break;
                default:
                    strRet = "";
                    break;
            }
            return strRet;
        }

        internal static string getStatusEnumStringDesc(P44M.RequestedAccessorialService.StatusEnum e)
        {
            string strRet = "";
            switch (e)
            {
                case P44M.RequestedAccessorialService.StatusEnum.ACCEPTED:
                    strRet = "ACCEPTED - requested accessorial accepted and included in quote total with a provided charge breakdown";
                    break;
                case P44M.RequestedAccessorialService.StatusEnum.ACCEPTEDUNITEMIZED:
                    strRet = "ACCEPTED UNITEMIZED - requested accessorial probably included in quote total, but no verifiable charge breakdown";
                    break;
                case P44M.RequestedAccessorialService.StatusEnum.UNACCEPTED:
                    strRet = "UNACCEPTED - requested accessorial not accepted and not reflected in quote total";
                    break;
                default:
                    strRet = "";
                    break;
            }
            return strRet;
        }

        internal static string formatDispatchTimeStringForAPI(string sTime, string sDefault)
        {
            string sRet = sDefault;
            DateTime dtParsed = DateTime.Now;
            string sToParse = string.Format("{0} {1}", "2018-01-01", sTime);
            if (DateTime.TryParse(sToParse, out dtParsed)) { sRet = dtParsed.ToString("HH:mm"); }
            return sRet;
        }

        /// <summary>
        /// Takes string sDate and parses it to a datetime - if that fails use the date sDefault
        /// Returns that datetime in the string format "yyyy-MM-dd"
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="sDefault"></param>
        /// <returns></returns>
        internal static string formatDateStringForAPI(string sDate, DateTime sDefault)
        {
            DateTime dt;
            if (!DateTime.TryParse(sDate, out dt)) { dt = sDefault; }
            string sRet = dt.ToString("yyyy-MM-dd");
            return sRet;
        }

        internal static string formatShortDateTime(DateTime? dtVal)
        {
            if (dtVal.HasValue)
            { return string.Format("{0} {1}", dtVal.Value.ToShortDateString(), dtVal.Value.ToShortTimeString()); }
            else { return ""; }
        }

        #endregion


    }
}