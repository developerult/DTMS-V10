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
    /// EDIDocSegmentElementController for EDIElements Rest API Controls
    /// </summary>
    /// <Remarks>
    /// Created By SRP on 2/27/2018
    /// </Remarks>
    public class EDIDocSegmentElementController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.EDIDocSegmentElementController";
        /// <summary>
        /// SourceClass Property for logging and error tracking
        /// </summary>
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;// Added by SRP on 02/27/18
        #endregion

        #region " Data Translation"
        /// <summary>
        /// Selecting a EDIDocSegmentElements Model
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Returns EDIDocSegmentElements</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        private Models.EDIDocSegmentElement selectModelData(LTS.tblEDIDocSegmentElement d)
        {
            Models.EDIDocSegmentElement modelEDIDocSegmentElements = new Models.EDIDocSegmentElement();

            //skipping values for reference to foreign keys added by SRP 27/2/2018
            List<string> skipEDIDocSegmentElement = new List<string> { "DSEUpdated", "tblEDIType", "tblEDIElmnt", "tblEDISegment", "tblEDIFormattingFunction" };
            string sMsg = "";
            modelEDIDocSegmentElements = (Models.EDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(modelEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
            if (modelEDIDocSegmentElements != null) { modelEDIDocSegmentElements.setUpdated(d.DSEUpdated.ToArray()); }
            return modelEDIDocSegmentElements;
        }
        /// <summary>
        /// select LTSEDIDocSegmentElement
        /// </summary>
        /// <param name="d">EDIDocSegmentElement parameter</param>
        /// <returns>returns tblEDIDocSegmentElement</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        private LTS.tblEDIDocSegmentElement selectLTSData(Models.EDIDocSegmentElement d)
        {
            LTS.tblEDIDocSegmentElement ltsEDIDocSegmentElements = new LTS.tblEDIDocSegmentElement();
            if (d != null)
            {
                List<string> skipEDIDocSegmentElement = new List<string> { "DSEUpdated" };
                string sMsg = "";
                ltsEDIDocSegmentElements = (LTS.tblEDIDocSegmentElement)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(ltsEDIDocSegmentElements, d, skipEDIDocSegmentElement, ref sMsg);
                if (ltsEDIDocSegmentElements != null)
                {
                    byte[] bupdated = d.getUpdated();
                    ltsEDIDocSegmentElements.DSEUpdated = bupdated == null ? new byte[0] : bupdated;
                }
            }
            return ltsEDIDocSegmentElements;
        }
        #endregion

        #region "Rest Services"

        /// <summary>
        /// Get EDIDocSegmentElement  Record
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 3/7/18
        /// </remarks>
        //[HttpGet, ActionName("GetRecord")]
        //public Models.Response GetRecord([System.Web.Http.FromBody]Models.EDIDocSegmentElement data)
        //{
        //    var response = new Models.Response();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        Models.EDIDocSegmentElement[] ediEDIDocSegmentElements = new Models.EDIDocSegmentElement[] { };
        //        DAL.NGLEDIDocSegmentElement dalData = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement oChanges = selectLTSData(data);
        //        LTS.tblEDIDocSegmentElement[] ltsEDIDocSegmentElement = dalData.GetEDIDocSegmentElement(oChanges.DSEControl);

        //        if (ltsEDIDocSegmentElement != null && ltsEDIDocSegmentElement.Count() > 0)
        //        {
        //            count = ltsEDIDocSegmentElement.Count();
        //            ediEDIDocSegmentElements = (from e in ltsEDIDocSegmentElement
        //                           select selectModelData(e)).ToArray();
        //            if (RecordCount > count) { count = RecordCount; }
        //        }
        //        //getting element records from tblEDIElmnt
        //        response = new Models.Response(ediEDIDocSegmentElements, count);
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
        /// Get EDIDocSegmentElement Records
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
        /// </remarks>
        //[HttpGet, ActionName("GetRecords")]
        //public Models.Response GetRecords(string filter,string doctype,string segment)
        //{
        //    var response = new Models.Response();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        DAL.Models.AllFilters f = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<DAL.Models.AllFilters>(filter);
        //        Models.EDIDocSegmentElement[] edidocsegmentelements = new Models.EDIDocSegmentElement[] { };
        //        DAL.NGLEDIDocSegmentElement oAn = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement[] ltsRet=null;
        //        if (Convert.ToInt32(doctype) != 0 && Convert.ToInt32(segment) != 0) 
        //            //ltsRet = oAn.GetEDIDocSegmentElements(Convert.ToInt32(doctype), Convert.ToInt32(segment)).Where(model => model.DSEEDITControl == Convert.ToInt32(doctype) && model.DSESegmentControl == Convert.ToInt32(segment)).ToArray();
        //        ltsRet = oAn.GetEDIDocSegElementsbyDocSeg(Convert.ToInt32(doctype), Convert.ToInt32(segment));
        //        else if (Convert.ToInt32(segment) != 0)
        //            //ltsRet = oAn.GetEDIDocSegmentElements(Convert.ToInt32(doctype), Convert.ToInt32(segment)).Where(model => model.DSEEDITControl == Convert.ToInt32(doctype) && model.DSESegmentControl == Convert.ToInt32(segment)).ToArray();
        //            ltsRet = oAn.GetEDIDocSegElementsbySeg(Convert.ToInt32(segment));

        //        //else
        //        //    ltsRet = null;
        //        if (ltsRet != null && ltsRet.Count() > 0)
        //        {
        //            count = ltsRet.Count();
        //            edidocsegmentelements = (from e in ltsRet
        //                            select selectModelData(e)).ToArray();
        //            if (RecordCount > count) { count = RecordCount; }
        //        }
        //        //getting element records from tblEDIDocSegmentElement
        //        response = new Models.Response(edidocsegmentelements, count);
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
        /// Get EDIDocSegmentElement Records
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 2/27/18
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
        //        Models.EDIDocSegmentElement[] edidocsegmentelements = new Models.EDIDocSegmentElement[] { };
        //        DAL.NGLEDIDocSegmentElement oAn = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement[] ltsRet = oAn.GetEDIDocSegmentElements(ref RecordCount, f);//.Where(model => model.DSEEDITControl == Convert.ToInt32(doctype) && model.DSESegmentControl == Convert.ToInt32(segment)).ToArray();
        //        if (ltsRet != null && ltsRet.Count() > 0)
        //        {
        //            count = ltsRet.Count();
        //            edidocsegmentelements = (from e in ltsRet
        //                                     select selectModelData(e)).ToArray();
        //            if (RecordCount > count) { count = RecordCount; }
        //        }
        //        //getting element records from tblEDIDocSegmentElement
        //        response = new Models.Response(edidocsegmentelements, count);
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
        /// Saves Updates of EDIDocSegmentElement
        /// </summary>
        /// <param name="data">EDIDocSegmentElement</param>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Created By SRP on 2/27/18
        /// </remarks>
        //[HttpPost, ActionName("PostSave")]
        //public Models.Response PostSave([System.Web.Http.FromBody]Models.EDIDocSegmentElement data)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DAL.NGLEDIDocSegmentElement dalData = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement oChanges = selectLTSData(data);
        //        //updates the edi docsegmentelements
        //        LTS.tblEDIDocSegmentElement oData = dalData.UpdateEDIDocSegmentElement(oChanges);
        //        Models.EDIDocSegmentElement[] oRecords = new Models.EDIDocSegmentElement[1];
        //        oRecords[0] = selectModelData(oData);
        //        response = new Models.Response(oRecords, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
        //        response.StatusCode = fault.StatusCode;
        //        response.Errors = fault.formatMessage();
        //        return response;
        //    }
        //    // return the HTTP Response.
        //    return response;
        //}

        /// <summary>
        /// Insert EDIDocSegmentElement data from Rest calls
        /// </summary>
        /// <param name="dt">EDIDocSegmentElement</param>
        /// <returns>Models.Response</returns>
        /// <remarks>Modified by SRP on 2/27/18</remarks>
        //[HttpPost, ActionName("SaveEDIDocSegmentElement")]
        //public Models.Response SaveEDIDocSegmentElement([System.Web.Http.FromBody]Models.EDIDocSegmentElement dt)
        //{
        //    // create a response message to send back
        //    var response = new Models.Response(); //new HttpResponseMessage();
        //    if (!authenticateController(ref response)) { return response; }
        //    try
        //    {
        //        DAL.NGLEDIDocSegmentElement dalData = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement oChanges = selectLTSData(dt);
        //        //insert the new edi docsegmentelement
        //        LTS.tblEDIDocSegmentElement oData = dalData.InsertEDIDocSegmentElement(oChanges);
        //        Models.EDIDocSegmentElement[] oRecords = new Models.EDIDocSegmentElement[1];
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
        //    return response;
        //}

        /// <summary>
        /// Delete EDIDocSegmentElement  Record
        /// </summary>
        /// <returns>Returns Model Response</returns>
        /// <remarks>
        /// Modified by SRP on 3/7/18
        /// </remarks>
        //[HttpDelete, ActionName("DeleteRecord")]
        //public bool DeleteRecord([System.Web.Http.FromBody]Models.EDIDocSegmentElement data)
        //{
        //    bool response = new bool();
        //    /*if (!authenticateController(bool response)) { return response; }*/
        //    try
        //    {
        //        int count = 0;
        //        int RecordCount = 0;
        //        Models.EDIDocSegmentElement[] ediEDIDocSegmentElements = new Models.EDIDocSegmentElement[] { };
        //        DAL.NGLEDIDocSegmentElement dalData = new DAL.NGLEDIDocSegmentElement(Parameters);
        //        LTS.tblEDIDocSegmentElement oChanges = selectLTSData(data);
        //        bool result = dalData.DeleteEDIDocSegmentElement(oChanges);

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