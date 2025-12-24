using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;
using Ngl.FreightMaster.Integration;
using System.Text.RegularExpressions;

namespace DynamicsTMS365.Controllers
{
    public class EDILogBookTrackController : NGLControllerBase
    {
        #region " Constructors "

        /// <summary> Initializes the Page property by calling the base class constructor </summary>
        public EDILogBookTrackController() : base(Utilities.PageEnum.EDILogs){ }

        #endregion

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDILogBookTrackController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " Data Translation"

        private Models.BookTrack selectModelData(LTS.vBookTrack d)
        {
            Models.BookTrack modelRecord = new Models.BookTrack();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "BookTrackUpdated", "_BookTrackDetails", "BookTrackDetails", "_Book", "Book" };
                string sMsg = "";
                modelRecord = (Models.BookTrack)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { modelRecord.setUpdated(d.BookTrackUpdated.ToArray()); }
            }
            return modelRecord;
        }

        #endregion


        #region " REST Services"

        /// POST 	/API/objectcontroller{data}  : Create a new object or Update a the current object if the control number exists
        /// GET 	/API/objectcontroller/{id}   : Get the object information where the control number = "id"
        /// PUT 	/API/objectcontroller/{data} : Update the object information stored in data
        /// DELETE	/API/objectcontroller/{id}   : Delete object where the control number = "id"

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords()
        {
            return GetRecords("");
        }

        [HttpGet, ActionName("GetRecords")]
        public Models.Response GetRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateFilter(ref response, filter)) { return response; } //Verfiy that the filters object is not null
            return GetAllRecords(filter);
        }

        [HttpGet, ActionName("GetAllRecords")]
        public Models.Response GetAllRecords(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                if (!string.IsNullOrWhiteSpace(filter)) { savePageFilters(filter, "EDIBookTrackGridFilter"); } //save the page filter for the next time the page loads
                DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);

                List<string> shids = new List<string>();
                string sEDIData = f.Data;

                if (string.IsNullOrWhiteSpace(sEDIData.Trim())) { return response; }
                sEDIData = sEDIData.Replace("\n", ""); //strip out line breaks in case the user added them via paste

                string strISA = sEDIData.Left(106);
                string strSegSep = strISA.Substring(strISA.Length - 1);
                string[] sISAs = Regex.Split(sEDIData, @"\" + strSegSep + @"ISA\*"); //split any ISA sections           
                string strISAHeader = ""; //now we have a list of ISAs so split each ISAs by GS
                clsEDIISA oISA;
                ////clsEDIGS oGS;
                foreach (var s in sISAs)
                {
                    if (s.Left(4) == "ISA*") { strISAHeader = s.Left(105); } else { strISAHeader = "ISA*" + s.Left(101); } //Add the ISA to the array we have to add the ISA* string back in if it does not exist
                    oISA = new clsEDIISA(strISAHeader);
                    oISA.SegmentTerminator = strSegSep;
                    string[] sIEAs = Regex.Split(s, @"\" + strSegSep + @"IEA\*"); //strip off the IEA segment               
                    string[] sGSs = Regex.Split(sIEAs[0], @"\" + strSegSep + @"GS\*"); //get the GS data
                    for (int i = 1; i <= sGSs.Length - 1; i++) //we skip the first item because it holds the ISA data
                    {
                        string sg = sGSs[i];
                        if (sg.Left(2) == "FA") { } //997
                        else if (sg.Left(2) == "QM") { //214                            
                            string[] sGEs = Regex.Split(sg, @"\" + strSegSep + @"GE\*"); //strip off the GE segment                           
                            string[] sSTs = Regex.Split(sGEs[0], @"\" + strSegSep + @"ST\*"); //split the records by ST
                            ////oGS = new clsEDIGS("GS*" + sSTs[0]);
                            for (int ist = 1; ist <= sSTs.Length - 1; ist++)
                            {
                                string st = sSTs[ist];
                                string[] sSEs = Regex.Split(st, @"\" + strSegSep + @"SE\*");
                                if (sSEs.Length > 1)
                                {
                                    clsEDISE oSE = new clsEDISE("SE*" + sSEs[1]);
                                    int intElements = 0;
                                    if (int.TryParse(oSE.SE01, out intElements))
                                    {                                        
                                        string[] s200s = Regex.Split(sSEs[0], @"\" + strSegSep + @"LX\*"); //Split out the 200 loops                                       
                                        string[] s100s = Regex.Split(s200s[0], @"\" + strSegSep + @"N1\*"); //the s200s(0) refers to the first part of the segment including any 100 loops so get the 100 loops
                                        clsEDI214 o214 = new clsEDI214(s100s[0], strSegSep, null);
                                        if (!string.IsNullOrWhiteSpace(o214.B10.B1002.Trim())) { shids.Add(o214.B10.B1002.Trim()); }
                                    }
                                }
                            }
                        }
                        //else if (sg.Left(2) == "GF") { EDIXaction = "990"; }
                        else if (sg.Left(2) == "IM") { //210
                            string[] sGEs = Regex.Split(sg, @"\" + strSegSep + @"GE\*"); //strip off the GE segment                           
                            string[] sSTs = Regex.Split(sGEs[0], @"\" + strSegSep + @"ST\*"); //split the records by ST                                                                                                                                                                                          
                            ////oGS = new clsEDIGS("GS*" + sSTs[0]);
                            for (int ist = 1; ist <= sSTs.Length - 1; ist++)
                            {
                                string st = sSTs[ist];
                                string[] sSEs = Regex.Split(st, @"\" + strSegSep + @"SE\*");
                                if (sSEs.Length > 1)
                                {
                                    clsEDISE oSE = new clsEDISE("SE*" + sSEs[1]);
                                    int intElements = 0;
                                    if (int.TryParse(oSE.SE01, out intElements))
                                    {                                        
                                        string[] sG62s = Regex.Split(sSEs[0], @"\" + strSegSep + @"G62\*"); //Split the B3 and C3 and N9 Loop using the G62 segment (G62 element is required)                                      
                                        if (sG62s.Length > 1) 
                                        {                                           
                                            string[] sN9s = Regex.Split(sG62s[0], @"\" + strSegSep + @"N9\*"); //Item 0 has the B3, C3 and N9 Loop segments. Split out the N9 Loop segment                                           
                                            clsEDI210 o210 = new clsEDI210(sN9s[0], strSegSep, null); //The first item in the sN9s list contains the B3 and C3 data. We use this to create the 210 object
                                            if (!string.IsNullOrWhiteSpace(o210.B3.B303.Trim())) { shids.Add(o210.B3.B303.Trim()); }                                                                                                                               
                                        }
                                    }
                                }
                            }
                        }
                        //else if (sg.Left(2) == "RA") { EDIXaction = "820"; }
                        //else if (sg.Left(2) == "SM") { EDIXaction = "204"; }
                    }
                }

                Models.BookTrack[] records = new Models.BookTrack[] { };
                int RecordCount = 0;
                int count = 0;
                LTS.vBookTrack[] oData = NGLBookTrackData.GetBookTracksBySHIDs(ref RecordCount, f, shids.ToArray());
                if (oData != null && oData.Count() > 0)
                {
                    count = oData.Count();
                    records = (from e in oData select selectModelData(e)).ToArray();
                    if (RecordCount > count) { count = RecordCount; }
                }else { return response; }
                response = new Models.Response(records, count);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        #endregion
    }
}