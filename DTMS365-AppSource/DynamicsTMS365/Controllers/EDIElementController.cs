using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
namespace DynamicsTMS365.Controllers
{

    /// <summary>
    /// EDIElementController for Elements Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/22/2018
    /// </Remarks>
    public class EDIElementController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIElementController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/22/18
        #endregion 

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDIElements Model
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Returns EDIElements</returns>
        /// <remarks>
        /// Modified by SRP on 2/22/18
        /// </remarks>
        private Models.EDIElement selectModelData(LTS.tblEDIElmnt d)
        {
            Models.EDIElement modelEDIElement = new Models.EDIElement();          
            //skipping values for reference to foreign keys added by SRP 23/2/2018
            List<string> skipEDIElements = new List<string> { "ElementUpdated",  "tblEDIDataType", "tblEDIValidationType",  "tblEDIFormattingFunction" };
            string sMsg = "";
            modelEDIElement = (Models.EDIElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIElement, d, skipEDIElements, ref sMsg);
            if (modelEDIElement != null) { modelEDIElement.setUpdated(d.ElementUpdated.ToArray()); }
            return modelEDIElement;
        }
            /// <summary>
            /// select LTSEDIElement
            /// </summary>
            /// <param name="d">EDIElement parameter</param>
            /// <returns>returns tblEDIElmnt</returns>
            /// <remarks>
            /// Modified by SRP on 2/22/18
            /// </remarks>
            private LTS.tblEDIElmnt selectLTSData(Models.EDIElement d)
            {
                LTS.tblEDIElmnt ltsEDIElmnts = new LTS.tblEDIElmnt();
                if (d != null)
                {
                    List<string> skipEDIElements = new List<string> { "ElementUpdated" };
                    string sMsg = "";
                    ltsEDIElmnts = (LTS.tblEDIElmnt)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIElmnts, d, skipEDIElements, ref sMsg);
                    if (ltsEDIElmnts != null)
                    {
                        byte[] bupdated = d.getUpdated();
                        ltsEDIElmnts.ElementUpdated = bupdated == null ? new byte[0] : bupdated;
                    }
                }
                return ltsEDIElmnts;
        }
        #endregion

        #region "Rest Services"

        /// <summary>
        /// Get EDISegment  Records
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 2/26/18
        /// </remarks>
        //[HttpGet, ActionName("GetRecord")]
        //public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIElement data)
        //{
        //    var response = new Models.Response();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        Models.EDIElement[] edielements = new Models.EDIElement[] { };
        //        DAL.NGLEDIelement dalData = new DAL.NGLEDIelement(Parameters);
        //        LTS.tblEDIElmnt oChanges = selectLTSData(data);
        //        LTS.tblEDIElmnt[] ltselements = dalData.GetEDIElmnt(oChanges.ElementControl);

        //        if (ltselements != null && ltselements.Count() > 0)
        //        {
        //            count = ltselements.Count();
        //            edielements = (from e in ltselements
        //                           select selectModelData(e)).ToArray();
        //            if (RecordCount > count) { count = RecordCount; }
        //        }
        //        //getting element records from tblEDIElmnt
        //        response = new Models.Response(edielements, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    return response;
        //}

        /// <summary>
        /// Get EDIElement Records
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 2/22/18
        /// </remarks>
        //[HttpGet, ActionName("GetRecords")]
        //public Models.Response GetRecords(string filter)
        //{
        //    var response = new Models.Response();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
        //        Models.EDIElement[] edielements = new Models.EDIElement[] { };
        //        DAL.NGLEDIelement oAn = new DAL.NGLEDIelement(Parameters);               
        //        LTS.tblEDIElmnt[] ltsEDIElmnts = oAn.GetEDIElmnts(ref RecordCount, f);
        //        if (ltsEDIElmnts != null && ltsEDIElmnts.Count() > 0)
        //        {
        //            count = ltsEDIElmnts.Count();
        //            edielements = (from e in ltsEDIElmnts
        //                            select selectModelData(e)).ToArray();                                    
        //            if (RecordCount > count) { count = RecordCount; }
        //        }
        //        //getting element records from tblEDIElmnt
        //        response = new Models.Response(edielements, count);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    // return the HTTP Response.
        //    return response;
        //}

        /// <summary>
        /// Saves Updates of EDIElement
        /// </summary>
        /// <param name="data">EDI Element</param>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Created By SRP on 2/22/18
        /// </remarks>
        //[HttpPost, ActionName("PostSave")]
        //public Models.Response PostSave([System.Web.Http.FromBody]Models.EDIElement data)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response();//new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DAL.NGLEDIelement dalData = new DAL.NGLEDIelement(Parameters);
        //        LTS.tblEDIElmnt oChanges = selectLTSData(data);
        //        //updates the edi elements
        //        LTS.tblEDIElmnt oData = dalData.UpdateEDIElmnt(oChanges);
        //        Models.EDIElement[] oRecords = new Models.EDIElement[1];
        //        oRecords[0] = selectModelData(oData);
        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    // return the HTTP Response.
        //    return response;
        //}

        /// <summary>
        /// Insert EDIElement data from Rest calls
        /// </summary>
        /// <param name="dt">EDIElement</param>
        /// <returns>Models.Response</returns>
        /// <remarks>Modified by SRP on 2/22/18</remarks>
        //[HttpPost, ActionName("SaveEDIElement")]
        //public Models.Response SaveEDIElement([System.Web.Http.FromBody]Models.EDIElement dt)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DAL.NGLEDIelement dalData = new DAL.NGLEDIelement(Parameters);
        //        LTS.tblEDIElmnt oChanges = selectLTSData(dt);
        //        //insert the new edi elements
        //        LTS.tblEDIElmnt oData = dalData.InsertEDIElmnt(oChanges);
        //        Models.EDIElement[] oRecords = new Models.EDIElement[1];
        //        oRecords[0] = selectModelData(oData);
        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    // return the HTTP Response.
        //    return response;
        //}

        /// <summary>
        /// Delete EDIElement  Record
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 3/7/18
        /// </remarks>
        //[HttpDelete, ActionName("DeleteRecord")]
        //public bool DeleteRecord([System.Web.Http.FromBody]Models.EDIElement data)
        //{
        //    bool response = new bool();
        //    /*if (!authenticateController(bool response)) { return response; }*/
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        Models.EDIElement[] ediSegments = new Models.EDIElement[] { };
        //        DAL.NGLEDIelement dalData = new DAL.NGLEDIelement(Parameters);
        //        LTS.tblEDIElmnt oChanges = selectLTSData(data);
        //        bool result = dalData.DeleteEDIElmnt(oChanges);

        //        //getting element records from tblEDIElmnt
        //        response = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error handler
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        //response.StatusCode = fault.StatusCode;
        //        //response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    return response;
        //}

        #endregion
    }
}