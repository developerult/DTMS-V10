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

namespace DynamicsTMS365.WS
{
    /// <summary>
    /// Summary description for DTMSIntegration
    /// </summary>
    [WebService(Namespace = "http://www.dynamicstms365.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DTMSIntegration : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        // Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
        // With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
        // Should only be run For diagnostics Or In test systems.

        private string mstrLastError = "";

        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public string LastError()
        {
            return mstrLastError;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.ERPSetting GetERPSetting(string AuthorizationCode, int ERPSettingControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Setting";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using control # " + ERPSettingControl.ToString() + ".  ";
            DTO.ERPSetting oData = new DTO.ERPSetting();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetERPSetting(ERPSettingControl, DTMS.ConnectionString);
                if (oData != null && oData.ERPSettingControl != 0)
                {
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;

                }
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;

            } catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetERPSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);

            }

            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.ERPSetting[] GetERPSettings(string AuthorizationCode, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Settings";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data.  ";
            DTO.ERPSetting[] oData = new DTO.ERPSetting[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetERPSettings(DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPSettingControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetERPSettings Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.ERPSetting[] GetERPSettingsByLegalEntity(string AuthorizationCode, string LegalEntity, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Settings by Legal Entity";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + ".  ";
            DTO.ERPSetting[] oData = new DTO.ERPSetting[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetERPSettingsByLegalEntity(LegalEntity, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPSettingControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetERPSettingsByLegalEntity Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.ERPSetting[] GetERPSettingsByLegalEntityAndERPType(string AuthorizationCode, string LegalEntity, int ERPTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Settings by Legal Entity and ERP Type";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + " and ERP Type Control # " + ERPTypeControl.ToString() + ".  ";
            DTO.ERPSetting[] oData = new DTO.ERPSetting[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetERPSettingsByLegalEntityAndERPType(LegalEntity, ERPTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPSettingControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetERPSettingsByLegalEntityAndERPType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.Integration GetIntegration(string AuthorizationCode, int IntegrationControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Configuration";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using control # " + IntegrationControl.ToString() + ".  ";
            DTO.Integration oData = new DTO.Integration();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetIntegration(IntegrationControl, DTMS.ConnectionString);
                if (oData != null && oData.IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetIntegration Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.Integration[] GetIntegrations(string AuthorizationCode, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Settings";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data.  ";
            DTO.Integration[] oData = new DTO.Integration[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetIntegrations(DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetIntegrations Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.Integration[] GetIntegrationsByERPSetting(string AuthorizationCode, int ERPSettingControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Configuration by ERP Setting Control";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using ERP Setting Control: " + ERPSettingControl.ToString() + ".  ";
             DTO.Integration[] oData = new DTO.Integration[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetIntegrationsByERPSetting(ERPSettingControl, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPSettingControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetIntegrationsByERPSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.Integration[] GetIntegrationsByERPSettingAndIntegrationType(string AuthorizationCode, int ERPSettingControl, int IntegrationTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Configuration by ERP Setting Control and Integration Type Control";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using ERP Setting Control: " + ERPSettingControl.ToString() + ".  ";
             DTO.Integration[] oData = new DTO.Integration[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GetIntegrationsByERPSettingAndIntegrationType(ERPSettingControl, IntegrationTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPSettingControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GetIntegrationsByERPSettingAndIntegrationType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.vERPIntegrationSetting[] getvERPIntegrationSettings(string AuthorizationCode, string LegalEntity, int ERPTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Integration Settings View by Legal Entity and ERP Type";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + " and ERP Type Control # " + ERPTypeControl.ToString() + ".  ";
            DTO.vERPIntegrationSetting[] oData = new DTO.vERPIntegrationSetting[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.getvERPIntegrationSettings(LegalEntity, ERPTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.getvERPIntegrationSettings Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.vERPIntegrationSetting[] getvERPIntegrationSettingsByName(string AuthorizationCode, string LegalEntity, string ERPTypeName, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Integration Settings View by Legal Entity and ERP Type Name";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + " and ERP Type Name: " + ERPTypeName + ".  ";
            DTO.vERPIntegrationSetting[] oData = new DTO.vERPIntegrationSetting[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.getvERPIntegrationSettingsByName(LegalEntity, ERPTypeName, DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.getvERPIntegrationSettingsByName Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.vERPIntegrationSetting getvERPIntegrationSetting(string AuthorizationCode, string LegalEntity, int ERPTypeControl, int IntegrationTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Integration Setting View by Legal Entity and ERP Type and Integration Type Control";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + " and ERP Type Control # " + ERPTypeControl.ToString() + " and Integration Type Control # " + IntegrationTypeControl.ToString() + ".  ";
            DTO.vERPIntegrationSetting oData = new DTO.vERPIntegrationSetting();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.getvERPIntegrationSetting(LegalEntity, ERPTypeControl, IntegrationTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.getvERPIntegrationSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.vERPIntegrationSetting getvERPIntegrationSettingByName(string AuthorizationCode, string LegalEntity, string ERPTypeName, string IntegrationTypeName, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Integration Setting View by Legal Entity and ERP Type Name and Integration Type Name";
            string sErrorLogMsg = "Cannot " + sProcedureName + " data using Legal Entity: " + LegalEntity + " and ERP Type Name: " + ERPTypeName + " and Integration Type Name: " + IntegrationTypeName + ".  ";
            DTO.vERPIntegrationSetting oData = new DTO.vERPIntegrationSetting();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.getvERPIntegrationSettingByName(LegalEntity, ERPTypeName, IntegrationTypeName, DTMS.ConnectionString);
                if (oData != null && oData.IntegrationControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.getvERPIntegrationSettingByName Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }
        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.tblERPType GettblERPType(string AuthorizationCode, int ERPTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Type";
            string sErrorLogMsg = "Cannot " + sProcedureName + " using ERP Type Control # " + ERPTypeControl + ".  ";
            DTO.tblERPType oData = new DTO.tblERPType();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GettblERPType(ERPTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.ERPTypeControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GettblERPType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }

        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.tblERPType[] GettblERPTypes(string AuthorizationCode, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS ERP Types";
            string sErrorLogMsg = "Cannot " + sProcedureName + ".  ";
            DTO.tblERPType[] oData = new DTO.tblERPType[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GettblERPTypes(DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].ERPTypeControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GettblERPTypes Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }

        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.tblIntegrationType GettblIntegrationType(string AuthorizationCode, int IntegrationTypeControl, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Type";
            string sErrorLogMsg = "Cannot " + sProcedureName + " using Integration Type Control # " + IntegrationTypeControl.ToString() + ".  ";
            DTO.tblIntegrationType oData = new DTO.tblIntegrationType();
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GettblIntegrationType(IntegrationTypeControl, DTMS.ConnectionString);
                if (oData != null && oData.IntegrationTypeControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GettblIntegrationType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }

        /// <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.tblIntegrationType[] GettblIntegrationTypes(string AuthorizationCode, ref int RetVal, ref string ReturnMessage)
        {
            string sProcedureName = "Read Dynamics TMS Integration Types";
            string sErrorLogMsg = "Cannot " + sProcedureName + ".  ";
            DTO.tblIntegrationType[] oData = new DTO.tblIntegrationType[1];
            Ngl.FreightMaster.Integration.clsDTMSIntegration DTMS = new Ngl.FreightMaster.Integration.clsDTMSIntegration();
            // set the default value to false
            RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure;
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) { return oData; }
                Utilities.populateIntegrationObjectParameters(DTMS);
                oData = DTMS.GettblIntegrationTypes(DTMS.ConnectionString);
                if (oData != null && oData.Count() > 0 && oData[0].IntegrationTypeControl != 0)
                    RetVal = (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete;
                mstrLastError = DTMS.LastError;
                ReturnMessage = mstrLastError;
            }
            catch (Exception ex)
            {
                mstrLastError = ex.Message;
                ReturnMessage = mstrLastError;
                Utilities.LogException("DTMSIntegration.GettblIntegrationTypes Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName);
            }
            return oData;
        }

        /// <summary>
        ///     ''' Web Method used to read the global task parameters from the parameter table in the databaser
        ///     ''' </summary>
        ///     ''' <param name="AuthorizationCode"></param>
        ///     ''' <param name="ReturnMessage"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>
        ///     ''' Created by RHR for v-7.0.5.102 on 11/14/2016
        ///     '''   Web Method to read global task parameter so direct call to db is not required.
        ///     ''' </remarks>
        ///     ''' <c>ClassLibrary1.TraceExtension()</c>
        [WebMethod()]
        public DTO.GlobalTaskParameters GetGlobalTaskParameters(string AuthorizationCode, ref string ReturnMessage)
        {
            DTO.GlobalTaskParameters oRet = new DTO.GlobalTaskParameters();
            string sSource = "DTMSIntegration.GetGlobalTaskParameters";
            string sDataType = "Global Task Parameters";
            try
            {
                if (!Utilities.validateAuthCode(AuthorizationCode)) 
                {
                    ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.";
                    return oRet;
                }                
                oRet = Utilities.GetGlobalTaskParameters();
            }
            catch (Exception ex)
            {
                ReturnMessage = ex.Message;
                Utilities.LogResults(sSource, 10000, ex.Message +"\n" + ex.StackTrace, AuthorizationCode);
                Utilities.LogException(sSource, (int)Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " + sDataType + " data.  ", ex, AuthorizationCode, "Process " + sDataType + " Data Failure");
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
            return oRet;
        }
    }
}
