using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using Ngl.FreightMaster.Data;

namespace NGL.FM.CarTar
{

    public static class Util
    {

        #region "Constants & Enums"

        public static string ImportType_tmpCSVInterlinePoints = "tmpCSVInterlinePoints";
        public static string ImportType_tmpCSVCarrierRates = "tmpCSVCarrierRates";
        public static string ImportType_tmpCSVNonServicePoints = "tmpCSVNonServicePoints";

        public enum ImportExportTypes
        {
            ImportFromExcel = 0,
            ImportFromCSVRates = 1,
            ExportToExcel = 2,
            ImportFromCSVInterline = 3,
            ImportFromCSVNonService = 4
        }

        public static string XLSXFileType = ".xlsx";
        public static string NEWLINE = "<br/>";
        public static int DistanceEndColumnNum = 9;
        public static string DistanceEndCellLetter = "I";
        public static int FlatEndColumnNum = 9;
        public static string FlatEndCellLetter = "I";
        public static int ClassEndColumnNum = 18;
        public static string ClassEndCellLetter = "R";
        public static int UOMEndColumnNum = 18;
        public static string UOMEndCellLetter = "R";

        public static string CarrTarEquipMatTarRateTypeControlCell = "V5"; //"CarrTarEquipMatTarRateTypeControl"
        public static string TariffControlCell = "U2"; //"TariffControl"
        public static string CarrTarTempTypeCell = "V1"; //"CarrTarTempType"
        public static string CarrierControlCell = "U8"; //"CarrierControl"
        public static string DefWgtCell = "U9"; //"DefWgt"
        public static string CarrTarEquipMatTarBracketTypeControlCell = "V4"; //"CarrTarEquipMatTarBracketTypeControl"
        public static string CarrTarOutboundCell = "U11";  //"CarrTarOutbound"
        public static string CarrTarTariffTypeControlCell = "U13";   //"CarrTarTariffTypeControl"
        public static string CarrTarEquipMatClassTypeControlCell = "V3"; //"CarrTarEquipMatClassTypeControl"
        public static string CarrTarEquipMatCarrTarEquipControlCell = "V6"; //"CarrTarEquipMatCarrTarEquipControl"
        public static string CompanyControlCell = "U3"; //"CompanyControl"
        public static string CarrTarEquipMatNameCell = "B10"; //"CarrTarEquipMatName"
        public static string CarrTarEquipMatCarrTarMatBPControlCell = "V10"; // "CarrTarEquipMatCarrTarMatBPControl"
        public static string TariffNameCell = "U1"; // "TariffName"//TariffName
        public static string CompanyNameCell = "U4"; // "CompanyName 
        public static string CompanyCityCell = "U5";  //CompanyCity
        public static string CompanyStateCell = "U6";  // CompanyState
        public static string CompCountryCell = "V7";  //CompCountry
        public static string CarrierNameCell = "U7";  //CarrierName
        public static string CarrTarBPBracketTypeCell = "U10";  // CarrTarBPBracketType
        public static string CarrTarTariffModeTypeControlCell = "U12";  //  CarrTarTariffModeTypeControl
        public static string CarrTarTariffTypeCell = "U14";  //  //CarrTarTariffTypeCell
        public static string CarrTarEquipControlNameCell = "B9";  //CarrTarEquipControlName
        public static string CarrTarTempTypeNameCell = "B5"; //CarrTarTempTypeName
        public static string CarrTarTariffModeTypeControlNameCell = "B8"; //CarrTarTariffModeTypeControlName
        public static string CarrTarEquipMatTarRateTypeControlNameCell = "B11"; //CarrTarEquipMatTarRateTypeControlName
        public static string CarrTarEquipMatTarBracketTypeControlNameCell = "B12"; //CarrTarEquipMatTarBracketTypeControlName
        public static string CarrTarEquipMatClassTypeControlNameCell = "B13"; //CarrTarEquipMatClassTypeControlName

        public static string HeaderCell = "A1";
        public static string HeaderCompanyInfoCell = "A2";
        public static string HeaderTariffNameCell = "A3";
        public static string HeaderTariffName1Cell = "A4";
        public static string DefWgtEnglishCell = "B6";
        public static string DirectionEnglishCell = "B7";

        public static int RateStartRow = 17;
        public static string RatesStartCell = "A17";
        public static int BPHeaderRow = 16;

        public enum distanceRateColumns
        {
            country = 0,
            state = 1,
            city = 2,
            fromZip = 3,
            toZip = 4,
            lane = 5,
            minval = 6,
            transDays = 7,
            rate = 8
        }

        public static string DistColCountry = "A";
        public static string DistColState = "B";
        public static string DistColCity = "C";
        public static string DistColFromZip = "D";
        public static string DistColToZip = "E";
        public static string DistColLane = "F";
        public static string DistColMinval = "G";
        public static string DistColTransDays = "H";
        public static string DistColRate = "I";

        public enum flatRateColumns
        {
            country = 0,
            state = 1,
            city = 2,
            fromZip = 3,
            toZip = 4,
            lane = 5,
            minval = 6,
            transDays = 7,
            rate = 8
        }

        public static string FlatColCountry = "A";
        public static string FlatColState = "B";
        public static string FlatColCity = "C";
        public static string FlatColFromZip = "D";
        public static string FlatColToZip = "E";
        public static string FlatColLane = "F";
        public static string FlatColMinval = "G";
        public static string FlatColTransDays = "H";
        public static string FlatColRate = "I";

        public enum ClassRateColumns
        {
            country = 0,
            state = 1,
            city = 2,
            fromZip = 3,
            toZip = 4,
            lane = 5,
            classCol = 6,
            minval = 7,
            transDays = 8,
            BP1 = 9,
            BP2 = 10,
            BP3 = 11,
            BP4 = 12,
            BP5 = 13,
            BP6 = 14,
            BP7 = 15,
            BP8 = 16,
            BP9 = 17
        }

        public static string ClassColCountry = "A";
        public static string ClassColState = "B";
        public static string ClassColCity = "C";
        public static string ClassColFromZip = "D";
        public static string ClassColToZip = "E";
        public static string ClassColLane = "F";
        public static string ClassColClass = "G";
        public static string ClassColMinval = "H";
        public static string ClassColTransDays = "I";
        public static string ClassColBP1 = "J";
        public static string ClassColBP2 = "K";
        public static string ClassColBP3 = "L";
        public static string ClassColBP4 = "M";
        public static string ClassColBP5 = "N";
        public static string ClassColBP6 = "O";
        public static string ClassColBP7 = "P";
        public static string ClassColBP8 = "Q";
        public static string ClassColBP9 = "R";


        public enum UOMRateColumns
        {
            country = 0,
            state = 1,
            city = 2,
            fromZip = 3,
            toZip = 4,
            lane = 5,
            classCol = 6,
            minval = 7,
            transDays = 8,
            BP1 = 9,
            BP2 = 10,
            BP3 = 11,
            BP4 = 12,
            BP5 = 13,
            BP6 = 14,
            BP7 = 15,
            BP8 = 16,
            BP9 = 17
        }
        public static string UOMColCountry = "A";
        public static string UOMColState = "B";
        public static string UOMColCity = "C";
        public static string UOMColFromZip = "D";
        public static string UOMColToZip = "E";
        public static string UOMColLane = "F";
        public static string UOMColClass = "G";
        public static string UOMColMinval = "H";
        public static string UOMColTransDays = "I";
        public static string UOMColBP1 = "J";
        public static string UOMColBP2 = "K";
        public static string UOMColBP3 = "L";
        public static string UOMColBP4 = "M";
        public static string UOMColBP5 = "N";
        public static string UOMColBP6 = "O";
        public static string UOMColBP7 = "P";
        public static string UOMColBP8 = "Q";
        public static string UOMColBP9 = "R";

        public static string BPColBP1 = "J";
        public static string BPColBP2 = "K";
        public static string BPColBP3 = "L";
        public static string BPColBP4 = "M";
        public static string BPColBP5 = "N";
        public static string BPColBP6 = "O";
        public static string BPColBP7 = "P";
        public static string BPColBP8 = "Q";
        public static string BPColBP9 = "R";
        #endregion

        #region " Static - Global Properties"

        private static string _RateShopClassCode = "100";
        public static string RateShopClassCode
        {
            get { return _RateShopClassCode; }
            set { _RateShopClassCode = value; } 
        }

        


        #endregion

        #region "static methods"

        public static DTO.vLookupList getvLookupItem(List<DTO.vLookupList> list, DAL.Utilities.LookUpListEnum item, int control)
        {
            DTO.vLookupList resultItem = (from d in list where d.Control == control select d).FirstOrDefault();
 
            return resultItem;
        }

        /// <summary>
        /// formats the provided sql fault info into 
        /// (a) a list of strings to be localized on the client (sFaultInfo) and 
        /// (b) a not localized string used for logs and serverside messages
        /// </summary>
        /// <param name="sqlEx"></param>
        /// <param name="sFaultInfo"></param>
        /// <returns>
        /// string
        /// </returns>
        /// <remarks>
        /// Created by RHR v-7.0.5.100 06/02/2016
        ///     standard message format logic to be used by all carrier tariff error handlers 
        ///     that catch sql fault info data
        /// </remarks>
        public static string formatFaultInfo(FaultException<DAL.SqlFaultInfo> sqlEx, ref List<string> sFaultInfo)
        {
            string strRet = "";
            if (sqlEx == null) { return ""; }
            if (sqlEx.Message != "E_NoData")// don't throw the exception if no data was found. just return false (not interline).  this should not happen
            {
                string strMsg = sqlEx.Detail.Message;
                string strReason = sqlEx.Reason.ToString();
                string NotLocalizedMessage = "";
                string NotLocalizedDetails = "";
                string NotLocalizedReason = "";
                string sDetails = DAL.SqlFaultInfo.getAlertInfoNotLocalized(strMsg
                    , sqlEx.Detail.Details
                    , sqlEx.Detail.DetailsList
                    , strReason
                    , ref NotLocalizedMessage
                    , ref NotLocalizedDetails
                    , ref NotLocalizedReason);

                sFaultInfo = new List<string> { strReason, strMsg, sDetails };
                strRet = sqlEx.Detail.ToString(strReason);
            }
            return strRet;
        }

        #endregion


    }

}
