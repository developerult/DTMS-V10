using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Ngl.FreightMaster.Integration;
using System.Xml.Serialization;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
//using Ngl.FreightMaster.Integration.Configuration;
using LTS = Ngl.FreightMaster.Data.LTS;
using DAL = Ngl.FreightMaster.Data;

namespace DynamicsTMS365.WS
{
    /// <summary>
    /// Summary description for DTMSERPIntegration
    /// </summary>
    [WebService(Namespace = "http://www.dynamicstms365.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DTMSERPIntegration : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

 
        // Note: replace all instances of   ''' <c>ClassLibrary1.TraceExtension()</c> 
        // With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.    
        // Must Add Reference to ClassLibrary1 project if it is not already added
        // Should only be run For diagnostics Or In test systems.


        private string mstrLastError = "";
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public string LastError()
        {
            return mstrLastError;
        }


        /// <summary>
        ///     ''' Imports Company Data using the 70 interface  Contacts are Suggested but Calendar data is optional
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="CompanyHeaders"></param>
        ///     ''' <param name="CompanyContacts"></param>
        ///     ''' <param name="CompanyCalendar"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsIntegrationUpdateResults ProcessCompanyData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsCompanyHeaderObject70[] CompanyHeaders, Ngl.FreightMaster.Integration.clsCompanyContactObject70[] CompanyContacts, Ngl.FreightMaster.Integration.clsCompanyCalendarObject70[] CompanyCalendar, ref string ReturnMessage)
        {
            clsIntegrationUpdateResults oRes = new clsIntegrationUpdateResults();
            oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessCompanyData70";
            string sDataType = "Company";
            try
            {
                if (CompanyHeaders == null || CompanyHeaders.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    oRes.ReturnValue = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return oRes;
                }
                List<Ngl.FreightMaster.Integration.clsCompanyHeaderObject70> lCompHeaders = CompanyHeaders.ToList();
                List<Ngl.FreightMaster.Integration.clsCompanyContactObject70> lCompContacts = new List<Ngl.FreightMaster.Integration.clsCompanyContactObject70>();
                if (CompanyContacts != null && CompanyContacts.Count() > 0)
                    lCompContacts = CompanyContacts.ToList();

                List<Ngl.FreightMaster.Integration.clsCompanyCalendarObject70> lCompCals = new List<Ngl.FreightMaster.Integration.clsCompanyCalendarObject70>();
                if (CompanyCalendar != null && CompanyCalendar.Count() > 0)
                    lCompCals = CompanyCalendar.ToList();

                Ngl.FreightMaster.Integration.clsCompany company = new Ngl.FreightMaster.Integration.clsCompany();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataValidationFailure;
                    return oRes;
                }
                Utilities.populateIntegrationObjectParameters(company);
                oRes = company.ProcessObjectData70(lCompHeaders, lCompContacts, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, lCompCals);
                ReturnMessage = company.LastError;
                Utilities.LogResults(sSource, (int)oRes.ReturnValue, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)oRes.ReturnValue, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return oRes;
        }



        /// <summary>
        ///     ''' Imports Carrier Data using the 70 interface Contacts Suggested Calendar is optional.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="CarrierHeaders"></param>
        ///     ''' <param name="CarrierContacts"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsIntegrationUpdateResults ProcessCarrierData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsCarrierHeaderObject70[] CarrierHeaders, Ngl.FreightMaster.Integration.clsCarrierContactObject70[] CarrierContacts, ref string ReturnMessage)
        {
            clsIntegrationUpdateResults oRes = new clsIntegrationUpdateResults();
            oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessCarrierData70";
            string sDataType = "Company";
            try
            {
                if (CarrierHeaders == null || CarrierHeaders.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return oRes;
                }

                List<Ngl.FreightMaster.Integration.clsCarrierHeaderObject70> lCarrierHeaders = CarrierHeaders.ToList();
                List<Ngl.FreightMaster.Integration.clsCarrierContactObject70> lCarrierContacts = new List<Ngl.FreightMaster.Integration.clsCarrierContactObject70>();
                if (CarrierContacts != null && CarrierContacts.Count() > 0)
                { lCarrierContacts = CarrierContacts.ToList(); }
                Ngl.FreightMaster.Integration.clsCarrier carrier = new Ngl.FreightMaster.Integration.clsCarrier();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataValidationFailure;
                    return oRes;
                }
                Utilities.populateIntegrationObjectParameters(carrier);
                oRes = carrier.ProcessObjectData70(lCarrierHeaders, lCarrierContacts, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = carrier.LastError;
                Utilities.LogResults(sSource, (int)oRes.ReturnValue, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)oRes.ReturnValue, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return oRes;
        }



        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool doesLaneExist(string AuthorizationCode, string sLaneNumber, ref string ReturnMessage)
        {
            bool blnRet = false;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.doesLaneExist";
            string sDataType = "Lane";
            try
            {
                Ngl.FreightMaster.Integration.clsLane lane = new Ngl.FreightMaster.Integration.clsLane();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    ReturnMessage = "Invalie Authorization Code " + AuthorizationCode;
                    return false;
                }
                Utilities.populateIntegrationObjectParameters(lane);
                blnRet = lane.doesLaneExist(sLaneNumber);
                ReturnMessage = lane.LastError;
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, 0, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return blnRet;
        }

        /// <summary>
        ///     ''' Imports Lane Data using the 70 inteface calendar data is optional
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="Lanes"></param>
        ///     ''' <param name="Calendar"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsIntegrationUpdateResults ProcessLaneData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsLaneObject70[] Lanes, Ngl.FreightMaster.Integration.clsLaneCalendarObject70[] Calendar, ref string ReturnMessage)
        {
            clsIntegrationUpdateResults oRes = new clsIntegrationUpdateResults();
            oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessLaneData70";
            string sDataType = "Lane";
            try
            {
                if (Lanes == null || Lanes.Length == 0)
                {
                    ReturnMessage = "No Lanes";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return oRes;
                }
                Ngl.FreightMaster.Integration.clsLane lane = new Ngl.FreightMaster.Integration.clsLane();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataValidationFailure;
                    return oRes;
                }
                Utilities.populateIntegrationObjectParameters(lane);

                List<Ngl.FreightMaster.Integration.clsLaneObject70> lLanes = Lanes.ToList();
                List<Ngl.FreightMaster.Integration.clsLaneCalendarObject70> lLaneCals = new List<Ngl.FreightMaster.Integration.clsLaneCalendarObject70>();
                if (Calendar != null && Calendar.Count() > 0)
                { lLaneCals = Calendar.ToList(); }
                oRes = lane.ProcessObjectData70(lLanes, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, lLaneCals);
                ReturnMessage = lane.LastError;
                Utilities.LogResults(sSource, (int)oRes.ReturnValue, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)oRes.ReturnValue, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return oRes;
        }


        /// <summary>
        ///     ''' Imports Lane Data using the 80 inteface calendar data is optional
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="Lanes"></param>
        ///     ''' <param name="Calendar"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsIntegrationUpdateResults ProcessLaneData80(string AuthorizationCode, Ngl.FreightMaster.Integration.clsLaneObject80[] Lanes, Ngl.FreightMaster.Integration.clsLaneCalendarObject80[] Calendar, ref string ReturnMessage)
        {
            clsIntegrationUpdateResults oRes = new clsIntegrationUpdateResults();
            oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessLaneData80";
            string sDataType = "Lane";
            try
            {
                if (Lanes == null || Lanes.Length == 0)
                {
                    ReturnMessage = "No Lanes";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return oRes;
                }
                Ngl.FreightMaster.Integration.clsLane lane = new Ngl.FreightMaster.Integration.clsLane();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataValidationFailure;
                    return oRes;
                }
                Utilities.populateIntegrationObjectParameters(lane);

                List<Ngl.FreightMaster.Integration.clsLaneObject80> lLanes = Lanes.ToList();
                List<Ngl.FreightMaster.Integration.clsLaneCalendarObject80> lLaneCals = new List<Ngl.FreightMaster.Integration.clsLaneCalendarObject80>();
                if (Calendar != null && Calendar.Count() > 0)
                { lLaneCals = Calendar.ToList(); }
                oRes = lane.ProcessObjectData80(lLanes, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, lLaneCals);
                ReturnMessage = lane.LastError;
                Utilities.LogResults(sSource, (int)oRes.ReturnValue, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                oRes.ReturnValue = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n"  + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)oRes.ReturnValue, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return oRes;
        }



        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessPalletTypeData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsPalletTypeObject[] PalletTypes, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessPalletTypeData70";
            string sDataType = "Pallet Type";
            try
            {
                if (PalletTypes == null || PalletTypes.Length == 0)
                {
                    ReturnMessage = "No Data";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsPalletType dataObject = new Ngl.FreightMaster.Integration.clsPalletType();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(dataObject);
                {
                   
                    result = (int)dataObject.ProcessObjectData(PalletTypes.ToList(), System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                    ReturnMessage = dataObject.LastError;
                    Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }


        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessHazmatData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsHazmatObject[] Hazmats, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessHazmatData70";
            string sDataType = "Hazmat";
            try
            {
                if (Hazmats == null || Hazmats.Length == 0)
                {
                    ReturnMessage = "No Data";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsHazmat dataObject = new Ngl.FreightMaster.Integration.clsHazmat();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(dataObject);
                {
                    
                    result = (int)dataObject.ProcessData(Hazmats.ToList(), System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                    ReturnMessage = dataObject.LastError;
                    Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }



        /// <summary>
        ///     ''' Add functionality to save records in clsBook
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="OrderHeaders"></param>
        ///     ''' <param name="OrderDetails"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessBookData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsBookHeaderObject70[] OrderHeaders, Ngl.FreightMaster.Integration.clsBookDetailObject70[] OrderDetails, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessBookData70";
            string sDataType = "Book";
            try
            {
                if (OrderHeaders == null || OrderHeaders.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                List<Ngl.FreightMaster.Integration.clsBookHeaderObject70> lOrderHeaders = OrderHeaders.ToList();
                List<Ngl.FreightMaster.Integration.clsBookDetailObject70> lOrderDetails = new List<Ngl.FreightMaster.Integration.clsBookDetailObject70>();
                if (OrderDetails != null && OrderDetails.Length > 0)
                { lOrderDetails = OrderDetails.ToList(); }
                result = (int)book.ProcessObjectData(lOrderHeaders, lOrderDetails, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = book.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }


        /// <summary>
        ///     ''' version 705 integration includes support for new ChangeNo key fields for header and footer references
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="OrderHeaders"></param>
        ///     ''' <param name="OrderDetails"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR v-7.0.5.100 07/21/2016
        ///     ''' Added ChangeNo field to  match header records with item detail records
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessBookData705(string AuthorizationCode, Ngl.FreightMaster.Integration.clsBookHeaderObject705[] OrderHeaders, Ngl.FreightMaster.Integration.clsBookDetailObject705[] OrderDetails, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessBookData705";
            string sDataType = "Book";
            try
            {
                if (OrderHeaders == null || OrderHeaders.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                List<Ngl.FreightMaster.Integration.clsBookHeaderObject705> lOrderHeaders = OrderHeaders.ToList();
                List<Ngl.FreightMaster.Integration.clsBookDetailObject705> lOrderDetails = new List<Ngl.FreightMaster.Integration.clsBookDetailObject705>();
                if (OrderDetails != null && OrderDetails.Length > 0)
                { lOrderDetails = OrderDetails.ToList(); }
                result = (int)book.ProcessObjectData(lOrderHeaders, lOrderDetails, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = book.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }


        /// <summary>
        ///     ''' version 80 integration includes support for new NAV fields
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="OrderHeaders"></param>
        ///     ''' <param name="OrderDetails"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR v-8.2 09/11/2018
        ///     '''     Supports changes implemented in NAV Integration
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessBookData80(string AuthorizationCode, Ngl.FreightMaster.Integration.clsBookHeaderObject80[] OrderHeaders, Ngl.FreightMaster.Integration.clsBookDetailObject80[] OrderDetails, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessBookData80";
            string sDataType = "Book";
            try
            {
                if (OrderHeaders == null || OrderHeaders.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                List<Ngl.FreightMaster.Integration.clsBookHeaderObject80> lOrderHeaders = OrderHeaders.ToList();
                List<Ngl.FreightMaster.Integration.clsBookDetailObject80> lOrderDetails = new List<Ngl.FreightMaster.Integration.clsBookDetailObject80>();
                if (OrderDetails != null && OrderDetails.Length > 0)
                { lOrderDetails = OrderDetails.ToList(); }
                result = (int)book.ProcessObjectData(lOrderHeaders, lOrderDetails, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = book.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        /// <summary>
        ///     ''' Look up existing order in TMS by Order Number (prvided) and update the POHDR status using the delete code
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="OrderNumber"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-8.2.1.007 on 11/15/2020 
        ///     '''     part of the CPF requirement for backward compatibility     
        ///     ''' </remarks>
        [WebMethod()]
        public int ProcessDeleteByOrderNumber(string AuthorizationCode, string OrderNumber, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessDeleteByOrderNumber";
            string sDataType = "Book";
            try
            {
                if (string.IsNullOrWhiteSpace(OrderNumber))
                {
                    ReturnMessage = "Missing Order Number";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataConnectionFailure;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                result = (int)book.ProcessDeleteByOrderNumber(OrderNumber, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = book.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process delete booking request by order number.  ", ex, AuthorizationCode, "Process process delete booking request by order number failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }




        /// <summary>
        ///     ''' Returns cost per pound
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="BookCarrOrderNumber"></param>
        ///     ''' <param name="BookOrderSequence"></param>
        ///     ''' <param name="CompAlphaCode"></param>
        ///     ''' <param name="CompLegalEntity"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR on 11/27/2017 for v-7.0.6.105
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public double GetCostPerPoundForOrder(string AuthorizationCode, string BookCarrOrderNumber, int BookOrderSequence, string CompAlphaCode, string CompLegalEntity)
        {
            string sSource = "DTMSERPIntegration.GetCostPerPoundForOrder";
            double result = 0;
            try
            {
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return -1;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                result = book.GetCostPerPoundForOrder(BookCarrOrderNumber, BookOrderSequence, CompAlphaCode, CompLegalEntity);
            }
            catch (Exception ex)
            {
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, 0, "Cannot Get Cost Per Pound For Order.  ", ex, AuthorizationCode, "Process Cost Per Pound Failure");
            }
            return result;
        }

        /// <summary>
        ///     ''' Returns cost per pound
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="BookCarrOrderNumber"></param>
        ///     ''' <param name="BookOrderSequence"></param>
        ///     ''' <param name="CompNumber"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR on 11/27/2017 for v-7.0.6.105
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public double GetCostPerPoundForOrderByCompNumber(string AuthorizationCode, string BookCarrOrderNumber, int BookOrderSequence, int CompNumber)
        {
            string sSource = "DTMSERPIntegration.GetCostPerPoundForOrder";
            double result = 0;
            try
            {
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return -1;
                Utilities.populateIntegrationObjectParameters(book);
                //these settings do not appear to be used so we are not setting them
                // ValidateOrderUniqueness now maintained in IMPORTORDERUNIQUENESS parameter
                //book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification");
                //book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness");
                result = book.GetCostPerPoundForOrder(BookCarrOrderNumber, BookOrderSequence, "", "", CompNumber);
            }
            catch (Exception ex)
            {
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, 0, "Cannot Get Cost Per Pound For Order.  ", ex, AuthorizationCode, "Process Cost Per Pound Failure");
            }
            return result;
        }



        /// <summary>
        ///     ''' Request Pick List Status Update data using the 70 interface.  AutoConfirmation is typically false.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="MaxRetry"></param>
        ///     ''' <param name="RetryMinutes"></param>
        ///     ''' <param name="CompLegalEntity"></param>
        ///     ''' <param name="MaxRowsReturned"></param>
        ///     ''' <param name="AutoConfirmation"></param>
        ///     ''' <param name="RetVal"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsPickListData70 GetPickListData70(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsPickListData70 pl = new clsPickListData70();
            Ngl.FreightMaster.Integration.clsPickList picklist = new Ngl.FreightMaster.Integration.clsPickList();
            Ngl.FreightMaster.Integration.clsPickListObject70[] Headers = new Ngl.FreightMaster.Integration.clsPickListObject70[1];
            Ngl.FreightMaster.Integration.clsPickDetailObject70[] Details = new Ngl.FreightMaster.Integration.clsPickDetailObject70[1];
            Ngl.FreightMaster.Integration.clsPickListFeeObject70[] Fees = new Ngl.FreightMaster.Integration.clsPickListFeeObject70[1];

            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetPickListData70";
            string sDataType = "Pick List";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;

            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return pl;
                Utilities.populateIntegrationObjectParameters(picklist);
                picklist.MaxRowsReturned = MaxRowsReturned;
                picklist.AutoConfirmation = AutoConfirmation;
                RetVal = (int)picklist.readObjectData70(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, ref Fees, ref Details);
                mstrLastError = picklist.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                pl.Headers = Headers;
                pl.Details = Details;
                pl.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot process " + sDataType + " data using " + strCriteria + ".  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return pl;
        }


        /// <summary>
        ///     ''' Request Pick List Status Update data using the 80 interface.  AutoConfirmation is typically false.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="MaxRetry"></param>
        ///     ''' <param name="RetryMinutes"></param>
        ///     ''' <param name="CompLegalEntity"></param>
        ///     ''' <param name="MaxRowsReturned"></param>
        ///     ''' <param name="AutoConfirmation"></param>
        ///     ''' <param name="RetVal"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR v-8.2.0.117 7/17/2019
        ///     '''   replaces the 70 version Of the data
        ///     '''   includes support for BookItemOrderNumber 
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsPickListData80 GetPickListData80(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsPickListData80 pl = new clsPickListData80();
            Ngl.FreightMaster.Integration.clsPickList picklist = new Ngl.FreightMaster.Integration.clsPickList();
            Ngl.FreightMaster.Integration.clsPickListObject80[] Headers = new Ngl.FreightMaster.Integration.clsPickListObject80[1];
            Ngl.FreightMaster.Integration.clsPickDetailObject80[] Details = new Ngl.FreightMaster.Integration.clsPickDetailObject80[1];
            Ngl.FreightMaster.Integration.clsPickListFeeObject80[] Fees =new Ngl.FreightMaster.Integration.clsPickListFeeObject80[1];

            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetPickListData80";
            string sDataType = "Pick List";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;

            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return pl;
                Utilities.populateIntegrationObjectParameters(picklist);
                picklist.MaxRowsReturned = MaxRowsReturned;
                picklist.AutoConfirmation = AutoConfirmation;
                RetVal = (int)picklist.readObjectData80(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, ref Fees, ref Details);
                mstrLastError = picklist.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                pl.Headers = Headers;
                pl.Details = Details;
                pl.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot process " + sDataType + " data using " + strCriteria + ".  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return pl;
        }




        /// <summary>
        ///Request Pick List Status Update data using the 80 interface.  AutoConfirmation is typically false.
        ///</summary>
        ///<param name="AuthorizationCode"></param>
        ///<param name="MaxRetry"></param>
        ///<param name="RetryMinutes"></param>
        ///<param name="CompLegalEntity"></param>
        ///<param name="MaxRowsReturned"></param>
        ///<param name="AutoConfirmation"></param>
        ///<param name="RetVal"></param>
        ///<param name="ReturnMessage"></param>
        ///<returns></returns>
        ///<remarks>
        ///Created by by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
        ///</remarks>
        ///<c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsPickListData85 GetPickListData85(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsPickListData85 pl = new clsPickListData85();
            Ngl.FreightMaster.Integration.clsPickList picklist = new Ngl.FreightMaster.Integration.clsPickList();
            Ngl.FreightMaster.Integration.clsPickListObject85[] Headers = new Ngl.FreightMaster.Integration.clsPickListObject85[1];
            Ngl.FreightMaster.Integration.clsPickDetailObject85[] Details = new Ngl.FreightMaster.Integration.clsPickDetailObject85[1];
            Ngl.FreightMaster.Integration.clsPickListFeeObject85[] Fees = new Ngl.FreightMaster.Integration.clsPickListFeeObject85[1];

            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetPickListData85";
            string sDataType = "Pick List";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;

            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return pl;
                Utilities.populateIntegrationObjectParameters(picklist);
                picklist.MaxRowsReturned = MaxRowsReturned;
                picklist.AutoConfirmation = AutoConfirmation;
                RetVal = (int)picklist.readObjectData85(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, ref Fees, ref Details);
                mstrLastError = picklist.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                pl.Headers = Headers;
                pl.Details = Details;
                pl.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot process " + sDataType + " data using " + strCriteria + ".  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return pl;
        }


        /// <summary>
        ///     ''' Update the Pick List record as recieved for the PLControl number provided. 
        ///     ''' Returns true for success and false for failure; ReturnMessage may contain details about failures.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="PlControl"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool ConfirmPickListExport70(string AuthorizationCode, long PlControl, ref string ReturnMessage)
        {
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ConfirmPickListExport70";
            string sDataType = "Pick List Confirmation";
            bool result = false;

            Ngl.FreightMaster.Integration.clsPickList picklist = new Ngl.FreightMaster.Integration.clsPickList();
            string strCriteria = "";
            strCriteria += " Pl Control = " + PlControl.ToString();
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return false;
                Utilities.populateIntegrationObjectParameters(picklist);
                result = picklist.confirmExport(System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, PlControl);
                if (!result)
                    ReturnMessage = picklist.LastError;
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, 0, "Cannot process " + sDataType + " data using " + strCriteria + ".  ", ex, AuthorizationCode, "A Duplicate Picklist Export Record is Possible");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }




        /// <summary>
        ///     ''' Request AP Export data using the 70 interface.  AutoConfirmation is typically false.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="MaxRetry"></param>
        ///     ''' <param name="RetryMinutes"></param>
        ///     ''' <param name="CompLegalEntity"></param>
        ///     ''' <param name="MaxRowsReturned"></param>
        ///     ''' <param name="AutoConfirmation"></param>
        ///     ''' <param name="RetVal"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsAPExportData70 GetAPData70(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsAPExportData70 ap = new clsAPExportData70();
            Ngl.FreightMaster.Integration.clsAPExport apExport = new Ngl.FreightMaster.Integration.clsAPExport();
            Ngl.FreightMaster.Integration.clsAPExportObject70[] Headers = new Ngl.FreightMaster.Integration.clsAPExportObject70[1];
            Ngl.FreightMaster.Integration.clsAPExportDetailObject70[] Details = new Ngl.FreightMaster.Integration.clsAPExportDetailObject70[1];
            Ngl.FreightMaster.Integration.clsAPExportFeeObject70[] Fees = new Ngl.FreightMaster.Integration.clsAPExportFeeObject70[1];

            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetAPData70";
            string sDataType = "AP";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;


            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return ap;
                Utilities.populateIntegrationObjectParameters(apExport);
                {
                    var withBlock = apExport;
                    withBlock.MaxRowsReturned = MaxRowsReturned;
                    withBlock.AutoConfirmation = AutoConfirmation;
                }
                RetVal = (int)apExport.readObjectData70(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity,ref Fees,ref Details);
                mstrLastError = apExport.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                ap.Headers = Headers;
                ap.Details = Details;
                ap.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot get AP Export Object data with details using " + strCriteria + ".  ", ex, AuthorizationCode, "Export AP Data Failure");
            }
            return ap;
        }


        /// <summary>
        ///     ''' Request AP Export data using the 70 interface.  AutoConfirmation is typically false.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="MaxRetry"></param>
        ///     ''' <param name="RetryMinutes"></param>
        ///     ''' <param name="CompLegalEntity"></param>
        ///     ''' <param name="MaxRowsReturned"></param>
        ///     ''' <param name="AutoConfirmation"></param>
        ///     ''' <param name="RetVal"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks> 
        ///     ''' Created by RHR v-8.2.0.117 7/17/2019
        ///     '''   replaces the 70 version Of the data
        ///     '''   includes support for BookItemOrderNumber 
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public clsAPExportData80 GetAPData80(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsAPExportData80 ap = new clsAPExportData80();
            Ngl.FreightMaster.Integration.clsAPExport apExport = new Ngl.FreightMaster.Integration.clsAPExport();
            Ngl.FreightMaster.Integration.clsAPExportObject80[] Headers = new Ngl.FreightMaster.Integration.clsAPExportObject80[1];
            Ngl.FreightMaster.Integration.clsAPExportDetailObject80[] Details = new Ngl.FreightMaster.Integration.clsAPExportDetailObject80[1];
            Ngl.FreightMaster.Integration.clsAPExportFeeObject80[] Fees = new Ngl.FreightMaster.Integration.clsAPExportFeeObject80[1];

            //set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetAPData80";
            string sDataType = "AP";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;


            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return ap;
                Utilities.populateIntegrationObjectParameters(apExport);
                {
                    var withBlock = apExport;
                    withBlock.MaxRowsReturned = MaxRowsReturned;
                    withBlock.AutoConfirmation = AutoConfirmation;
                }
                RetVal = (int)apExport.readObjectData80(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, ref Fees, ref Details);
                mstrLastError = apExport.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                ap.Headers = Headers;
                ap.Details = Details;
                ap.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot get AP Export Object data with details using " + strCriteria + ".  ", ex, AuthorizationCode, "Export AP Data Failure");
            }
            return ap;
        }

        [WebMethod()]
        public clsAPExportData85 GetAPData85(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, bool AutoConfirmation, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            clsAPExportData85 ap = new clsAPExportData85();
            Ngl.FreightMaster.Integration.clsAPExport apExport = new Ngl.FreightMaster.Integration.clsAPExport();
            Ngl.FreightMaster.Integration.clsAPExportObject85[] Headers = new Ngl.FreightMaster.Integration.clsAPExportObject85[1];
            Ngl.FreightMaster.Integration.clsAPExportDetailObject85[] Details = new Ngl.FreightMaster.Integration.clsAPExportDetailObject85[1];
            Ngl.FreightMaster.Integration.clsAPExportFeeObject85[] Fees = new Ngl.FreightMaster.Integration.clsAPExportFeeObject85[1];

            //set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetAPData85";
            string sDataType = "AP";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " AutoConfirmation = " + AutoConfirmation.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;


            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return ap;
                Utilities.populateIntegrationObjectParameters(apExport);
                {
                    var withBlock = apExport;
                    withBlock.MaxRowsReturned = MaxRowsReturned;
                    withBlock.AutoConfirmation = AutoConfirmation;
                }
                RetVal = (int)apExport.readObjectData85(ref Headers, System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, ref Fees, ref Details);
                mstrLastError = apExport.LastError;
                ReturnMessage = mstrLastError;
                Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode);
                ap.Headers = Headers;
                ap.Details = Details;
                ap.Fees = Fees;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot get AP Export Object data with details using " + strCriteria + ".  ", ex, AuthorizationCode, "Export AP Data Failure");
            }
            return ap;
        }


        /// <summary>
        ///     ''' Update the AP Export record as recieved for the APControl number provided.
        ///     ''' Returns true for success and false for failure; ReturnMessage may contain details about failures.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="APControl"></param>
        ///     ''' <param name="LastError"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool ConfirmAPExport70(string AuthorizationCode, int APControl, ref string ReturnMessage)
        {
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ConfirmAPExport70";
            string sDataType = "AP Export Confirmation";
            Ngl.FreightMaster.Integration.clsAPExport apExport = new Ngl.FreightMaster.Integration.clsAPExport();
            bool result = false;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return false;
                Utilities.populateIntegrationObjectParameters(apExport);
                result = apExport.confirmExportEx(System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, APControl);
                mstrLastError = apExport.LastError;
                Utilities.LogResults(sSource, 0, apExport.LastError, AuthorizationCode);
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, 0, "Cannot process " + sDataType + "  data using APControl Number " + APControl.ToString() + ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible");
            }
            return result;
        }


        /// <summary>
        ///     ''' Represents the AP Export data aggregated into a single invvoice for the entire freight bill.
        ///     ''' </summary>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/11/2016
        ///     '''  currently used by the GP Integration Service
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public APExportRecordsAggregated[] GetAPExportRecordsAggregated(string AuthorizationCode, int MaxRetry, int RetryMinutes, string CompLegalEntity, int MaxRowsReturned, ref int RetVal, ref string ReturnMessage)
        {
            if (CompLegalEntity == "")
                CompLegalEntity = null;

            List<APExportRecordsAggregated> ap = new List<APExportRecordsAggregated>();
            List<LTS.spGetAPExportRecordsAggregatedResult> APResults = new List<LTS.spGetAPExportRecordsAggregatedResult>();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetAPExportRecordsAggregated";
            string sDataType = "AP";
            string strCriteria = "";
            strCriteria += " MaxRetry = " + MaxRetry.ToString();
            strCriteria += " RetryMinutes = " + RetryMinutes.ToString();
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            if (!string.IsNullOrEmpty(CompLegalEntity))
                strCriteria += " CompLegalEntity = " + CompLegalEntity;

            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                { return ap.ToArray(); }
                

                DAL.NGLAPMassEntryData oNGLAPDataLib = new DAL.NGLAPMassEntryData(Utilities.DALWCFParameters);
                APResults = oNGLAPDataLib.GetAPExportRecordsAggregated(MaxRetry, RetryMinutes, CompLegalEntity, MaxRowsReturned);
                if (APResults != null && APResults.Count() > 0)
                {
                    List<string> skipObjects = new List<string>() { "" };
                    foreach (LTS.spGetAPExportRecordsAggregatedResult apr in APResults)
                    {
                        string strMsg = "";
                        APExportRecordsAggregated nap = new APExportRecordsAggregated();
                        nap = (APExportRecordsAggregated)Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nap, apr, skipObjects,ref strMsg);
                        if (!string.IsNullOrWhiteSpace(strMsg))
                        {
                            ReturnMessage += strMsg;
                            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors;
                        }
                        else
                            ap.Add(nap);
                    }
                    if ((RetVal == (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors))
                        Utilities.LogResults(sSource, RetVal, ReturnMessage, AuthorizationCode);
                    else
                        RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    if (ap != null && ap.Count() > 0)
                        ReturnMessage = "Success! " + ap.Count().ToString() + " AP records were selected for processing";
                    else
                        ReturnMessage = "No AP records are available for processing";
                }
                else
                {
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    ReturnMessage = "No AP records are available for processing";
                }

                Utilities.LogResults(sSource, RetVal, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, RetVal, "Cannot Get AP Export Aggregated Records using " + strCriteria + ".  ", ex, AuthorizationCode, "Get AP Export Records Aggregated Failure");
            }
            return ap.ToArray();
        }

        /// <summary>
        ///     ''' Updates  the AP Export status flags based on thge SHID, Freight Bill Number.
        ///     ''' Typically the value of APExport Flag is true  the APExport date is required.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="BookSHID"></param>
        ///     ''' <param name="APBillNumber"></param>
        ///     ''' <param name="APExportFlag"></param>
        ///     ''' <param name="APExportDate"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/11/2016
        ///     '''  currently used by the GP Integration Service
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool UpdateAPExportStatus(string AuthorizationCode, string BookSHID, string APBillNumber, bool APExportFlag, DateTime APExportDate, ref string ReturnMessage)
        {
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.UpdateAPExportStatus";
            string sDataType = "AP Export Status";
            // Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
            bool result = false;
            string strCriteria = "";
            strCriteria += " BookSHID = " + BookSHID;
            strCriteria += " APBillNumber = " + APBillNumber;
            strCriteria += " APExportFlag = " + APExportFlag.ToString();
            strCriteria += " APExportDate = " + APExportDate.ToString();
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                { return false; }               
                DAL.NGLAPMassEntryData oNGLAPDataLib = new DAL.NGLAPMassEntryData(Utilities.DALWCFParameters);
                result = oNGLAPDataLib.UpdateStatus(BookSHID, APBillNumber, APExportFlag, APExportDate, null/* TODO Change to default(_) if this is not a reference type */);

                Utilities.LogResults(sSource, 0, "Success!", AuthorizationCode);
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException(sSource, 0, "Cannot update " + sDataType + "  data using  " + strCriteria + ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible");
            }
            return result;
        }



        /// <summary>
        ///     ''' Imports Payable data useing the 70 inteface.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="Payables"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int ProcessPayableData70(string AuthorizationCode, Ngl.FreightMaster.Integration.clsPayablesObject70[] Payables, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.ProcessPayableData70";
            string sDataType = "Payable";
            try
            {
                if (Payables == null || Payables.Length == 0)
                {
                    ReturnMessage = "Empty Header";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsPayables oPayables = new Ngl.FreightMaster.Integration.clsPayables();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(oPayables);
                result = (int)oPayables.ProcessObjectData70(Payables.ToList(), System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = oPayables.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        /// <summary>
        ///     ''' Imports Payable data using the 705 inteface and Freight Bill Number as the key
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="Payables"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/11/2016
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public int UpdatePayablesByFreightBill(string AuthorizationCode, Ngl.FreightMaster.Integration.clsPayablesObject705[] Payables, ref string ReturnMessage)
        {
            int result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.UpdatePayablesByFreightBill";
            string sDataType = "Payable";
            try
            {
                if (Payables == null || Payables.Length == 0)
                {
                    ReturnMessage = "Empty Payables Data Nothing to Do";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    result = (int)Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                    return result;
                }
                Ngl.FreightMaster.Integration.clsPayables oPayables = new Ngl.FreightMaster.Integration.clsPayables();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return result;
                Utilities.populateIntegrationObjectParameters(oPayables);
                result = (int)oPayables.UpdatePayablesByFreightBill(Payables.ToList(), System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString);
                ReturnMessage = oPayables.LastError;
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, result, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public Ngl.FreightMaster.Integration.APUnpaidFreightBills[] GetUnpaidFreightBills(string AuthorizationCode, string LegalEntity, int MaxRowsReturned, ref int WSResult, ref string LastError)
        {
            List<Ngl.FreightMaster.Integration.APUnpaidFreightBills> lUnpaid = new List<Ngl.FreightMaster.Integration.APUnpaidFreightBills>();
            Ngl.FreightMaster.Integration.clsAPExport apExport = new Ngl.FreightMaster.Integration.clsAPExport();
            // set the default value to false
            WSResult = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            string strCriteria = "";
            strCriteria += " MaxRowsReturned = " + MaxRowsReturned.ToString();
            strCriteria += " LegalEntity = " + LegalEntity;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode))
                    return lUnpaid.ToArray();
                Utilities.populateIntegrationObjectParameters(apExport);
                {
                    var withBlock = apExport;
                    withBlock.MaxRowsReturned = MaxRowsReturned;
                }
                lUnpaid = apExport.GetUnpaidFreightBills(System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString, ref LegalEntity);
                if (lUnpaid != null)
                { WSResult = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete; }
                mstrLastError = apExport.LastError;
                LastError = mstrLastError;
                Utilities.LogResults("DTMSERPIntegration.GetUnpaidFreightBills", WSResult, apExport.LastError, AuthorizationCode);
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                LastError = mstrLastError;
                Utilities.LogException("DTMSERPIntegration.GetUnpaidFreightBills Failure", WSResult, "Cannot get unpaid freight bills using " + strCriteria + ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure");
            }
            return lUnpaid.ToArray();
        }



        /// <summary>
        ///     ''' Web Method to get the company abbreviation using the company number or the company alpha code.  
        ///     ''' set the company number to zero to use the alpha code
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="compNumber"></param>
        ///     ''' <param name="compAlphaCode"></param>
        ///     ''' <param name="sDefault"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/11/2016
        ///     '''  currently used by the GP Integration Service
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public string GetCompAbrevByNumberOrAlpha(string AuthorizationCode, int compNumber, string compAlphaCode, string sDefault, ref string ReturnMessage)
        {
            string strRet = sDefault;
            ReturnMessage = "";
            string sSource = "DTMSERPIntegration.GetCompAbrevByNumberOrAlpha";
            string sDataType = "Comp Abrev";
            try
            {
                if (compNumber == 0 & string.IsNullOrWhiteSpace(compAlphaCode))
                {
                    ReturnMessage = "Missing Company Reference";
                    Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode);
                    return strRet;
                }
                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.";
                    return strRet;
                }
                Utilities.populateIntegrationObjectParameters(book);

                strRet = book.GetCompAbrevByNumberOrAlpha(compNumber, compAlphaCode, sDefault, ref ReturnMessage);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return strRet;
        }

        /// <summary>
        ///     ''' Web Method used to test if the order information has changed
        ///     ''' using the Header data and the total items.  Used to determine 
        ///     ''' if we are importing a new order or a modified order.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="oHeader"></param>
        ///     ''' <param name="TotalItems"></param>
        ///     ''' <param name="strChanges"></param>
        ///     ''' <param name="TestTransType"></param>
        ///     ''' <param name="TestModeType"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/11/2016
        ///     '''   currently used by the GP Integration Service
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool HasOrderChanged(string AuthorizationCode, Ngl.FreightMaster.Integration.clsBookHeaderObject705 oHeader, int TotalItems, ref string strChanges, bool TestTransType, bool TestModeType, ref string ReturnMessage)
        {
            bool blnRet = false;
            string sSource = "DTMSERPIntegration.HasOrderChanged";
            string sDataType = "Order";
            try
            {
                if (oHeader == null)
                {
                    ReturnMessage = "Order header information is missing";
                    return false;
                }

                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.";
                    return blnRet;
                }
                Utilities.populateIntegrationObjectParameters(book);

                blnRet = book.HasOrderChanged(oHeader, TotalItems,ref strChanges, TestTransType, TestModeType);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return blnRet;
        }


        /// <summary>
        ///     ''' Web Method used to test if the order information has changed
        ///     ''' using the Header data and the total items.  Used to determine 
        ///     ''' if we are importing a new order or a modified order.
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="oHeader"></param>
        ///     ''' <param name="TotalItems"></param>
        ///     ''' <param name="strChanges"></param>
        ///     ''' <param name="TestTransType"></param>
        ///     ''' <param name="TestModeType"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-8.x on 05/15/2018
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public bool HasOrderChanged80(string AuthorizationCode, Ngl.FreightMaster.Integration.clsBookHeaderObject80 oHeader, int TotalItems, ref string strChanges, bool TestTransType, bool TestModeType, ref string ReturnMessage)
        {
            bool blnRet = false;
            string sSource = "DTMSERPIntegration.HasOrderChanged";
            string sDataType = "Order";
            try
            {
                if (oHeader == null)
                {
                    ReturnMessage = "Order header information is missing";
                    return false;
                }

                Ngl.FreightMaster.Integration.clsBook book = new Ngl.FreightMaster.Integration.clsBook();
                if (!Utilities.validateAuthCode(AuthorizationCode))
                {
                    ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.";
                    return blnRet;
                }
                Utilities.populateIntegrationObjectParameters(book);

                blnRet = book.HasOrderChanged(oHeader, TotalItems,ref strChanges, TestTransType, TestModeType);
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message + "\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
            }
            finally
            {
                try
                {
                    mstrLastError = ReturnMessage;
                }
                catch (Exception ex)
                {
                }
            }
            return blnRet;
        }

    }


    [Serializable()]
    public class clsPickListData
    {
        public Ngl.FreightMaster.Integration.clsPickListObject[] Headers;
        public Ngl.FreightMaster.Integration.clsPickDetailObject[] Details;
    }

    [Serializable()]
    public class clsPickListData60
    {
        public Ngl.FreightMaster.Integration.clsPickListObject60[] Headers;
        public Ngl.FreightMaster.Integration.clsPickDetailObject60[] Details;
    }


    [Serializable()]
    public class clsPickListData70
    {
        public Ngl.FreightMaster.Integration.clsPickListObject70[] Headers;
        public Ngl.FreightMaster.Integration.clsPickDetailObject70[] Details;
        public Ngl.FreightMaster.Integration.clsPickListFeeObject70[] Fees;
    }



    /// <summary>
    /// Pick Worksheet return data for web services v-8.0
    /// </summary>
    /// <remarks>
    /// Created by RHR v-8.2.0.117 7/17/2019 
    /// replaces the 70 version Of the data, includes support for BookItemOrderNumber
    /// </remarks>
    [Serializable()]
    public class clsPickListData80
    {
        public Ngl.FreightMaster.Integration.clsPickListObject80[] Headers;
        public Ngl.FreightMaster.Integration.clsPickDetailObject80[] Details;
        public Ngl.FreightMaster.Integration.clsPickListFeeObject80[] Fees;
    }

    /// <summary>
    /// Pick Worksheet return data for web services
    /// </summary>
    /// <remarks>
    /// Created by by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
    /// </remarks>
    [Serializable()]
    public class clsPickListData85
    {
        public Ngl.FreightMaster.Integration.clsPickListObject85[] Headers;
        public Ngl.FreightMaster.Integration.clsPickDetailObject85[] Details;
        public Ngl.FreightMaster.Integration.clsPickListFeeObject85[] Fees;
    }


    [Serializable()]
    public class clsAPExportData
    {
        public Ngl.FreightMaster.Integration.clsAPExportObject[] Headers;
        public Ngl.FreightMaster.Integration.clsAPExportDetailObject[] Details;
    }


    [Serializable()]
    public class clsAPExportData70
    {
        public Ngl.FreightMaster.Integration.clsAPExportObject70[] Headers;
        public Ngl.FreightMaster.Integration.clsAPExportDetailObject70[] Details;
        public Ngl.FreightMaster.Integration.clsAPExportFeeObject70[] Fees;
    }


    [Serializable()]
    public class clsAPExportData80
    {
        public Ngl.FreightMaster.Integration.clsAPExportObject80[] Headers;
        public Ngl.FreightMaster.Integration.clsAPExportDetailObject80[] Details;
        public Ngl.FreightMaster.Integration.clsAPExportFeeObject80[] Fees;
    }

    [Serializable()]
    public class clsAPExportData85
    {
        public Ngl.FreightMaster.Integration.clsAPExportObject85[] Headers;
        public Ngl.FreightMaster.Integration.clsAPExportDetailObject85[] Details;
        public Ngl.FreightMaster.Integration.clsAPExportFeeObject85[] Fees;
    }





}
