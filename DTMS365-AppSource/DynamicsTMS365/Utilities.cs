using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Net;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using System.Text;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Web.UI;
using Ngl.FreightMaster.Data.DataTransferObjects;

//Imports NGL.FM.CarTar
//Imports DAL = NGL.FreightMaster.Data
//Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
//Imports BLL = NGL.FM.BLL

namespace DynamicsTMS365
{
    /// <summary>
    /// Global Static Utilities class
    /// </summary>
    /// <remarks>
    /// Modified by RHR for v-8.5.2.006 on 01/24/2023 added Change Password Page
    /// </remarks>
    public static class Utilities
    {
        internal static string LineBreak = "\r\n";
        internal static string ParagraphBreak = "\r\n\r\n";
        internal static string newLine = "[newline]";
        internal static string htmlLineBreak = "<br/>";
        internal static string htmlParagraphStart = "<p>";
        internal static string htmlParagraphEnd = "</p>";
        internal static string htmlHeader4Start = "<h4>";
        internal static string htmlHeader4End = "</h4>";
        // Modified by RHR for v-8.5.2.006 on 01/24/2023 added Change Password Page and financial page
        // Modified by RHR for v-8.5.2.007 on 04/18/2023 added ChartOfAccounts Page # 12
        // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic new pages 
        // TariffOrginRatesDistance = 162
        // TariffOrginRatesClass = 163,
        // TariffOrginRatesFlat = 164,
        // TariffOrginRatesUOM = 165
        public enum PageEnum
        {
            None = 0,
            ChangePassword = 3,
            Localization = 4,
            NSOpsStatic = 8,
            NextStopCarrier = 9,
            About = 10,
            Contact = 11,
            ChartOfAccounts = 12,
            CarrierRegistration = 13,
            FreeTrialRegistration = 14,
            RateShopping = 15,
            LoadBoardFinancial = 16,
            Settings = 17,
            All = 18,
            SignUp = 19,
            Home = 20,
            Login = 21,
            ForgotPassword = 22,
            RequestAnAccount = 23,
            Settlement = 24,
            Settled = 25,
            LoadPlanning = 26,
            RoleCenter = 27,
            CarrierMaintenance = 28,
            LegalEntitiyMaint = 29,
            CompanyMaint = 30,
            LaneMaint = 31,
            AccessorialFees = 32,
            SpotRate = 33,
            Parameters = 34,
            ConsolidationSum = 35,
            AlertsDashboard = 36,
            SchedulerTest = 37,
            Scheduler = 38,
            Tariff = 39,
            RegisterUser = 40,
            RegisterForFreeTrial = 41,
            CarrierAccessorialMaint = 42,
            CarrierAccessorialApproval = 43,
            LegalEntityCarrierMaint = 44,
            Tendered = 45,
            PageMaintenance = 46,
            LEUsers = 47,
            TariffServices = 48,
            TariffExceptions = 49,
            ManageSchedule = 50,
            CarrierScheduler = 51,
            TariffBreakPoints = 52,
            TariffFees = 53,
            TariffFuel = 54,
            TariffHDMs = 55,
            TariffNoDriveDays = 56,
            TariffRatesDistance = 57,  // Truck Load Distance Rates are default
            TariffRatesClass = 58,
            TariffRatesFlat = 59,
            TariffRatesUOM = 60,
            CarrierEquipCode = 61,
            LoadBoard = 62,
            RateIT = 63,
            FlatRateAccessorials = 64,
            RateITCarriers = 65,
            Dispatch = 66,
            DispatchReport = 67,
            BOL = 68,
            ChangeLoadStatus = 69,
            BookAccessorial = 70,
            BookPackage = 71,
            BookSpotRateData = 72,
            LoadBoardStops = 73,
            Reports = 74,
            CarrierSchedulerGrouped = 75,
            LoadBoardItems = 76,
            LoadBoardRevenue = 77,
            OrderPreview = 78,
            PackageDescriptions = 79,
            SystemInfo = 80,
            APMassEntryMaint = 81,
            QuickPrints = 82,
            EDILogs = 83,
            CompanyDetail = 84,
            LaneDetail = 85,
            CarrierDetail = 86,
            NGLAccounting = 87,
            NGLExpenses = 88,
            WhatsNew = 89,
            LoadBoardRevenueLoad = 90,
            KnowledgeBase = 91,
            LoadBoardLoadStatus = 92,
            ManageSecurity = 94, //Added By LVV on 5/20/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues 
            CompanyFinancial = 95, //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
            LoadBoardNotes = 96, //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board
            LoadBoardCarrierData = 97, //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162842 - Create Carrier Data Page
            LoadPLanningProtoType = 98,
            LELaneFee = 99,//Added By ManoRama On 13-JUL/20 for Lanefees
            LELaneProfileFees = 100, //Added By Pradeep on 7/24/20 - Lane Profile Fees
            LELanePreferredCarriers = 101, //Added by ManoRama for LELanePreferredCarriers ON 30-JUL-2020
            LocalReports = 102, //Added By LVV on 8/3/20 - Task #20200727155055 Report Viewer
            TestHTMLPage = 103, //Added by RHR Test Page          
            CompEDI = 104,//Added by ManoRama for Company EDI ON 12-AUG-2020
            CarrierEDI = 105, //Added by ManoRama for Carrier EDI ON 19-AUG-2020
            CarrierFuelIndexMaint = 106,//Added by ManoRama for Carrier Fuel Index ON 24-AUG-2020
            CarrierFuel = 107, //added by suhas for Carrier Fuel on 26/08/20
            NatFuelZones = 108,//Added by ManoRama for Carrier Fuel Index ON 24-AUG-2020s
            Claims = 109, //Added by RHR for v-8.3.0.001 on 08/29/2020 Claima Maintenance
            NGLSystemMaint = 110, //Added By LVV on 8/19/20 for v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
            CarrierEquipment = 111,//Added By manoRama On 01Sep2020 for Carrier Equipment changes
            StaticListMaint = 112, //added by suhas on 03-SEP-20 for static look up list maintenance
            GlobalListMaint = 113, //added by pradeep on 04-SEP-20 for global dynamic look up list maintenance 
            MapRoutePlanning = 114, //added by RHR on 09-SEP-20 for trimble maps route planning  
            DAT = 115, //Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration
            MasterCarrier = 116, //Added By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page
            LELaneTransLoad = 117, //Added By RHR on 02/05/2021 for v-8.4.0.001
            PODUpload = 118, //Added By RHR on 02/05/2021 for v-8.4.0.001
            CarrierAcceptRejectLoad = 119, //Added By RHR for v-8.4.0.002 on 04/26/2021
            CarrierBookAppt = 120, //Added By RHR for v-8.4.0.002 on 04/26/2021
            LoadBoardRevenueFees = 121, //Added By RHR for v-8.4.0.002 on 05/10/2021
            FavouritesDashboard = 122, //Added By CHA on 5/18/2021
            CompanyDashboard = 123, //Added By CHA on 5/18/2021
            CarrierDashboard = 124, //Added By CHA on 5/18/2021
            LaneDashboard = 125, //Added By CHA on 5/18/2021
            PlanningExecutionDashboard = 126, //Added By CHA on 5/18/2021
            TariffsDashboard = 127, //Added By CHA on 5/18/2021
            ShedulingDashboard = 128, //Added By CHA on 5/18/2021
            NEXTTrackDashboard = 129, //Added By CHA on 5/18/2021
            NGLAccountingDashboard = 130, //Added By CHA on 5/18/2021
            TaskManager = 131, //Added By RHR on 10/13/2021
            PickWorksheet = 132, //Added By RHR for v-8.5.0.001 on 10/14/2021
            PurchInvWorksheet = 133, //Added By RHR for v-8.5.0.001 on 10/14/2021
            ERPSettings = 134, //Added By RHR for v-8.5.0.001 on 10/14/2021
            WorkflowConfiguration = 135, //Added By RHR for v-8.5.0.001 on 10/14/2021
            MailLog = 136, //Added By RHR for v-8.5.0.001 on 10/14/2021
            IntegrationLog = 137, //Added By RHR for v-8.5.0.001 on 10/14/2021
            TaskManagerLog = 138, //Added By RHR for v-8.5.0.001 on 10/14/2021
            CarrierExposureShipment = 139, //Added By RHR for v-8.5.0.001 on 10/14/2021
            CarrierExposureAll = 140, //Added By RHR for v-8.5.0.001 on 10/14/2021
            OutdatedStagingOrders = 141, //Added By RHR for v-8.5.0.001 on 10/14/2021
            OutdatedNStatusOrders = 142, //Added By RHR for v-8.5.0.001 on 10/14/2021
            CarrierInsuranceExpired = 143, //Added By RHR for v-8.5.0.001 on 10/14/2021
            PreviewWaiting = 144, //Added By RHR for v-8.5.0.001 on 10/14/2021
            CarrierQualification = 145, //Added By RHR for v-8.5.0.001 on 10/14/2021
            LELaneHDMMaint = 146, //Added By RHR for v-8.5.0.001 on 10/14/2021
            CarrierProNumbers = 147, //Added By RHR for v-8.5.0.001 on 10/14/2021
            WorkflowLeadTimeConfiguration = 148, //Added By RHR for v-8.5.3.001 on 06/14/2022
            WorkflowSetup = 149, //Added By RHR for v-8.5.3.006 on 10/20/2022
            CarrierNoDriveDays = 150, //Added By RHR for v-8.5.3.006 on 01/06/2023
            CompanyCalendar = 151, //Added By RHR for v-8.5.3.006 on 01/06/2023
            RouteGuide = 152, //Added By RHR for v-8.5.3.006 on 01/06/2023
            LaneCalendar = 153, //Added By RHR for v-8.5.3.006 on 01/06/2023
            RouteGuideCarrier = 154, // Added by Kann on 01/16/2023
            RouteGuideEquip = 155,// Added by Kann on 01/16/2023
            SubscribedAlert = 156,
            UnSubscribedAlert = 157,
            LoadPlanningStops = 158,
            LoadPlanningQuotes = 159,
            LoadPlanningChangeSHID = 160,
            MultiPickCons = 161,
            TariffOrginRatesDistance = 162,  
            TariffOrginRatesClass = 163,
            TariffOrginRatesFlat = 164,
            TariffOrginRatesUOM = 165,
            RateShoppingQ = 166,
            tblSSOALEConfig = 167,
            tblSSOAConfig = 168,
            PODViewer = 169, //Added by RHR for v-8.5.4.003 on 10/24/2023
            BookingImages = 170, //Added by RHR for v-8.5.4.003 on 10/24/2023
            ERPIntegration = 171, //Added by RHR for v-8.5.4.004 on 11/13/2023 child page of ERPSettings
            LoadStatusBoard = 172 //Added by RHR for v-8.5.4.004 on 12/04/2023
        }
    
        //Removed by RHR for v-8.2 on 12/19/2018 
        //we now use Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum
        //public enum BidType
        //{
        //    NextStop = 1,
        //    NGLTar = 2,
        //    P44 = 3,
        //    Spot = 4
        //}

        #region " NGL Widgets"

        /// <summary>
        /// Enumerated list of Group Sub Types associated with the cmGroupSubType table
        /// </summary>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/18/2018
        ///  added group sub types 58 to 63
        /// </remarks>
        public enum GroupSubType
        {
            Grid = 1,
            SpreadSheet = 2,
            ListView = 3,
            TreeList = 4,
            ScrollView = 5,
            AutoComplete = 6,
            ColorPicker = 7,
            ComboBox = 8,
            DatePicker = 9,
            DateTimePicker = 10,
            DropDownList = 11,
            Editor = 12,
            MaskedTextBox = 13,
            MultiSelect = 14,
            NumericTextBox = 15,
            kendoSwitch = 16,
            Button = 17,
            TabStrip = 18,
            GridFastTab = 19,
            FilterSelection = 20,
            FormFastTab = 22,
            FastTabAction = 23,
            EditorCustomTool = 24,
            EditorStandardTools = 25,
            StandardBorder = 26,
            FullPageBorder = 27,
            FloatBlockLeft = 28,
            TwoColumnData = 29,
            Line = 30,
            Header1 = 31,
            Header2 = 32,
            Header3 = 33,
            Header4 = 34,
            Div = 35,
            Span = 36,
            Paragraph = 37,
            Images = 38,
            Link = 39,
            PageFooter = 40,
            KendoIconButton = 41,
            KIcon = 42,
            HTMLSpace = 43,
            HTMLLineBreak = 44,
            FullPageFooter = 45,
            ToolTip = 46,
            GridToolBarTemplate = 47,
            KendoGridExportToolBar = 48,
            TimePicker = 49,
            NGLEditWindCtrl = 50,
            NGLAddDependentCtrl = 51,
            NGLSummaryDataCtrl = 52,
            NGLEditOnPageCtrl = 53,
            Checkbox = 54,
            FloatLeftTable = 55,
            TableHeader = 56,
            Label = 57,
            NGLWorkFlowOptionCtrl = 58,
            NGLWorkFlowGroup = 59,
            NGLWorkFlowOnOffSwitch = 60,
            NGLWorkFlowYesNoSwitch = 61,
            NGLWorkFlowSectionCtrl = 62,
            NGLPopupWindCtrl = 63,
            NGLErrWarnMsgLogCtrl = 64
        }


        public static DateTime? convertStringDateToDate(string value, DateTime? dtDefault = null)
        {
            DateTime? dtRet = null;
            DateTime dtReal;
            string sDateString = value;
            if (DateTime.TryParse(value, out dtReal))
            {
                return dtReal;
            }
            else
            {
                if (value.Trim().Length < 8)
                {
                    if (!dtRet.HasValue && dtDefault.HasValue)
                    {
                        return dtDefault;
                    }
                    else { return dtRet; }
                }

                sDateString = value.Substring(0, 4) + "/" + value.Substring(4, 2) + "/" + value.Substring(6, 2);
                if (DateTime.TryParse(sDateString, out dtReal))
                {
                    return dtReal;
                }
            }
            if (!dtRet.HasValue && dtDefault.HasValue) { 
                return dtDefault; 
            }
            else { return dtRet; }
                
        }

        public static DateTime? convertStringTimeToDate(string value, DateTime? dtDefault = null)
        {

            DateTime? dtRet = null;
            DateTime dtReal;
            string sDateString = value;
            if (DateTime.TryParse(value, out dtReal))
            {
                return dtReal;
            }
            else
            {
                if (value.Trim().Length < 4)
                {
                    if (value.Trim().Length == 1)
                    { value = value.PadLeft(2, '0'); }
                    if (value.Trim().Length == 3)
                    { value = value.PadLeft(4, '0'); }
                    value =  value.PadRight(4, '0');
                    //if (!dtRet.HasValue && dtDefault.HasValue)
                    //{
                    //    return dtDefault;
                    //}
                    //else { return dtRet; }
                }

                sDateString = value.Substring(0, 2) + ":" + value.Substring(2, 2);
                if (DateTime.TryParse(sDateString, out dtReal))
                {
                    return dtReal;
                }
            }
            if (!dtRet.HasValue && dtDefault.HasValue)
            {
                return dtDefault;
            }
            else { return dtRet; }
        }


        /// <summary>
        /// Containers designed to hold other controls and may be  
        /// placed as a primary container on a page or window
        /// does not require a specific type of parent
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2 on 10/18/2018
        ///  added NGLPopupWindCtrl & NGLWorkFlowOptionCtrl
        /// </remarks>
        public static bool isGroupSubTypeAContainer(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.Grid
                        || Value == (int)Utilities.GroupSubType.SpreadSheet
                        || Value == (int)Utilities.GroupSubType.TreeList
                        || Value == (int)Utilities.GroupSubType.ScrollView
                        || Value == (int)Utilities.GroupSubType.TabStrip
                        || Value == (int)Utilities.GroupSubType.StandardBorder
                        || Value == (int)Utilities.GroupSubType.FullPageBorder
                        || Value == (int)Utilities.GroupSubType.TwoColumnData
                        || Value == (int)Utilities.GroupSubType.PageFooter
                        || Value == (int)Utilities.GroupSubType.FullPageFooter
                        || Value == (int)Utilities.GroupSubType.NGLEditWindCtrl
                        || Value == (int)Utilities.GroupSubType.NGLAddDependentCtrl
                        || Value == (int)Utilities.GroupSubType.NGLSummaryDataCtrl
                        || Value == (int)Utilities.GroupSubType.NGLEditOnPageCtrl
                        || Value == (int)Utilities.GroupSubType.NGLPopupWindCtrl
                        || Value == (int)Utilities.GroupSubType.NGLWorkFlowOptionCtrl
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isGroupSubTypeAnHTMLTag(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.Line
                        || Value == (int)Utilities.GroupSubType.Header1
                        || Value == (int)Utilities.GroupSubType.Header2
                        || Value == (int)Utilities.GroupSubType.Header3
                        || Value == (int)Utilities.GroupSubType.Header4
                        || Value == (int)Utilities.GroupSubType.Div
                        || Value == (int)Utilities.GroupSubType.Span
                        || Value == (int)Utilities.GroupSubType.Paragraph
                        || Value == (int)Utilities.GroupSubType.Images
                        || Value == (int)Utilities.GroupSubType.Link
                        || Value == (int)Utilities.GroupSubType.HTMLSpace
                        || Value == (int)Utilities.GroupSubType.HTMLLineBreak
                        || Value == (int)Utilities.GroupSubType.Label
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isGroupSubTypeAnActionItem(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.Button
                        || Value == (int)Utilities.GroupSubType.EditorCustomTool
                        || Value == (int)Utilities.GroupSubType.EditorStandardTools
                        || Value == (int)Utilities.GroupSubType.KendoIconButton
                        || Value == (int)Utilities.GroupSubType.KIcon
                        || Value == (int)Utilities.GroupSubType.ToolTip
                        || Value == (int)Utilities.GroupSubType.GridToolBarTemplate
                        || Value == (int)Utilities.GroupSubType.KendoGridExportToolBar
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isGroupSubTypeADataEntryItem(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.AutoComplete
                        || Value == (int)Utilities.GroupSubType.ColorPicker
                        || Value == (int)Utilities.GroupSubType.DatePicker
                        || Value == (int)Utilities.GroupSubType.DateTimePicker
                        || Value == (int)Utilities.GroupSubType.Editor
                        || Value == (int)Utilities.GroupSubType.MaskedTextBox
                        || Value == (int)Utilities.GroupSubType.NumericTextBox
                        || Value == (int)Utilities.GroupSubType.kendoSwitch
                        || Value == (int)Utilities.GroupSubType.TimePicker
                        || Value == (int)Utilities.GroupSubType.Checkbox
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isGroupSubTypeASelectionList(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.ListView
                        || Value == (int)Utilities.GroupSubType.ComboBox
                        || Value == (int)Utilities.GroupSubType.DropDownList
                        || Value == (int)Utilities.GroupSubType.MultiSelect
                        || Value == (int)Utilities.GroupSubType.DropDownList
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isGroupSubTypeAPageSpecificItem(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.FloatBlockLeft
                        || Value == (int)Utilities.GroupSubType.FloatLeftTable
                        || Value == (int)Utilities.GroupSubType.TableHeader
                        )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isGroupSubTypeAFastTab(int Value)
        {
            if (Value == (int)Utilities.GroupSubType.GridFastTab
                        || Value == (int)Utilities.GroupSubType.FormFastTab
                        || Value == (int)Utilities.GroupSubType.FastTabAction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the sub type is a work flow option component
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLWorkFlowOptionCtrl 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLWorkFlowOptionCtrl(int Value)
        {

            if (Value == (int)Utilities.GroupSubType.NGLWorkFlowGroup
                        || Value == (int)Utilities.GroupSubType.NGLWorkFlowOnOffSwitch
                        || Value == (int)Utilities.GroupSubType.NGLWorkFlowYesNoSwitch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the sub type is can be included on the NGLWorkFlowGroup
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLWorkFlowGroup 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLWorkFlowGroup(int Value)
        {

            if (Value == (int)Utilities.GroupSubType.NGLWorkFlowOnOffSwitch
                || Value == (int)Utilities.GroupSubType.NGLWorkFlowYesNoSwitch
                || Value == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the sub type is can be included on the NGLWorkFlowOnOffSwitch
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLWorkFlowOnOffSwitch 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLWorkFlowOnOffSwitch(int Value)
        {

            if (Value == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the sub type is can be included on the NGLWorkFlowYesNoSwitch
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLWorkFlowYesNoSwitch 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLWorkFlowYesNoSwitch(int Value)
        {

            if (Value == (int)Utilities.GroupSubType.NGLWorkFlowSectionCtrl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the sub type is can be included on the NGLWorkFlowSectionCtrl
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLWorkFlowSectionCtrl 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLWorkFlowSectionCtrl(int Value)
        {

            bool bRet = false;
            if (isGroupSubTypeAnHTMLTag(Value)) { return true; }
            if (isGroupSubTypeADataEntryItem(Value)) { return true; }
            if (isGroupSubTypeAFastTab(Value)) { return true; }
            if (Value == (int)Utilities.GroupSubType.DropDownList) { return true; }
            return bRet;
        }

        /// <summary>
        /// Determines if the sub type is can be included on the NGLPopupWindCtrl
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 10/18/2018
        ///   controls that may be included as children to an NGLPopupWindCtrl 
        /// </remarks>
        public static bool isGroupSubTypeAllowedOnNGLPopupWindCtrl(int Value)
        {
            bool bRet = false;
            if (isGroupSubTypeAContainer(Value)) { return true; }
            if (isGroupSubTypeAPageSpecificItem(Value)) { return true; }
            if (isGroupSubTypeAFastTab(Value)) { return true; }
            if (isGroupSubTypeAnHTMLTag(Value)) { return true; }
            if (isGroupSubTypeADataEntryItem(Value)) { return true; }
            if (Value == (int)Utilities.GroupSubType.DropDownList) { return true; }
            if (Value == (int)Utilities.GroupSubType.Button) { return true; }
            return bRet;
        }

        public static bool isGroupSubTypeAllowedOnNGLSummaryDataCtrl(int Value)
        {
            bool bRet = true;
            if (isGroupSubTypeAContainer(Value)) { return false; }
            if (isGroupSubTypeAnHTMLTag(Value)) { return false; }
            if (isGroupSubTypeAnActionItem(Value)) { return false; }
            if (isGroupSubTypeAPageSpecificItem(Value)) { return false; }
            if (isGroupSubTypeAFastTab(Value)) { return false; }

            return bRet;
        }

        public static bool isGroupSubTypeAllowedOnNGLEditOnPageCtrl(int Value)
        {
            bool bRet = false;
            if (isGroupSubTypeAnHTMLTag(Value)) { return true; }
            if (isGroupSubTypeAPageSpecificItem(Value)) { return true; }
            if (isGroupSubTypeADataEntryItem(Value)) { return true; }
            if (Value == (int)Utilities.GroupSubType.DropDownList) { return true; }
            return bRet;
        }

        public static bool isGroupSubTypeAllowedOnNGLEditWindCtrl(int Value)
        {
            bool bRet = false;

            if (isGroupSubTypeAPageSpecificItem(Value)) { return true; }
            if (isGroupSubTypeAnHTMLTag(Value)) { return true; }
            if (isGroupSubTypeADataEntryItem(Value)) { return true; }
            if (Value == (int)Utilities.GroupSubType.DropDownList) { return true; }
            if (Value == (int)Utilities.GroupSubType.Button) { return true; }
            return bRet;
        }

        #endregion

        private static DAL.WCFParameters _DALWCFParameters;
        public static DAL.WCFParameters DALWCFParameters
        {
            get
            {
                if (_DALWCFParameters == null)
                {

                    _DALWCFParameters = new DAL.WCFParameters
                    {
                        Database = System.Configuration.ConfigurationManager.AppSettings["Database"],
                        DBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"],
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString,
                        WCFAuthCode = "NGLSystem",
                        UserName = "",
                        ValidateAccess = false
                    };
                }
                return _DALWCFParameters;
            }
            set { _DALWCFParameters = value; }
        }

        public static DAL.WCFParameters getUserParameters()
        {
            System.Security.Principal.IIdentity oUser = HttpContext.Current.User.Identity;

            DAL.WCFParameters par = new DAL.WCFParameters
            {
                Database = System.Configuration.ConfigurationManager.AppSettings["Database"],
                DBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"],
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString,
                WCFAuthCode = "NGLSystem",
                UserName = oUser.Name,
                ValidateAccess = false
            };

            return par;
        }

        public static DAL.WCFParameters authenticateUser(DAL.WCFParameters par)
        {

            return par;
        }

        private static DynamicsTMS365.TMSApp.clsGlobalTMSLookup _globalvLookup;
        /// <summary>
        /// Global Lookup lists stored in Application State Memory
        /// </summary>
        /// <remarks>
        /// Created by RHR for v-8.0 on 2/21/2017
        /// </remarks>
        public static DynamicsTMS365.TMSApp.clsGlobalTMSLookup globalvLookup
        {
            get
            {
                if (_globalvLookup == null)
                {
                    _globalvLookup = new DynamicsTMS365.TMSApp.clsGlobalTMSLookup();
                }
                return _globalvLookup;
            }
            set
            {
                _globalvLookup = value;
            }
        }

        private static DynamicsTMS365.TMSApp.clsUserLists _UserLists;
        /// <summary>
        /// Global User Lookup lists container stored in Application State Memory
        /// </summary>
        /// <remarks>
        /// Created by RHR for v-8.0 on 2/21/2017
        /// </remarks>
        public static DynamicsTMS365.TMSApp.clsUserLists UserLists
        {
            get
            {
                if (_UserLists == null)
                {
                    _UserLists = new DynamicsTMS365.TMSApp.clsUserLists();
                }
                return _UserLists;
            }
            set
            {
                _UserLists = value;
            }
        }



        private static Dictionary<int, DAL.Models.SSOResults> _GlobalSSOResultsByUser;
        public static Dictionary<int, DAL.Models.SSOResults> GlobalSSOResultsByUser
        {
            get
            {
                if (_GlobalSSOResultsByUser == null)
                {
                    _GlobalSSOResultsByUser = new Dictionary<int, DAL.Models.SSOResults>();
                }
                return _GlobalSSOResultsByUser;
            }
            set { _GlobalSSOResultsByUser = value; }
        }

        private static Dictionary<int, Models.GenericResult> _GlobalMenuTreeByUser;
        public static Dictionary<int, Models.GenericResult> GlobalMenuTreeByUser
        {
            get
            {
                if (_GlobalMenuTreeByUser == null)
                {
                    _GlobalMenuTreeByUser = new Dictionary<int, Models.GenericResult>();
                }
                return _GlobalMenuTreeByUser;
            }
            set { _GlobalMenuTreeByUser = value; }
        }

        private static Dictionary<string, LTS.cmLocalizeKeyValuePair> _GlobalLocalizedMsgs;
        public static Dictionary<string, LTS.cmLocalizeKeyValuePair> GlobalLocalizedMsgs
        {
            get
            {
                if (_GlobalLocalizedMsgs == null)
                {
                    _GlobalLocalizedMsgs = new Dictionary<string, LTS.cmLocalizeKeyValuePair>();
                }
                return _GlobalLocalizedMsgs;
            }
            set { _GlobalLocalizedMsgs = value; }
        }


        public static string formatNGLMessageForDisplay(string m, List<DTO.NGLMessage> p)
        {
            if (string.IsNullOrWhiteSpace(m)) return m;
            string strRet = getLocalizedMsg(m, null);
            try
            {
                if (p != null && p.Count() > 0)
                {
                    var sList = (from d in p select d.Message).ToList();
                    if (sList != null && sList.Count() > 0)
                    {
                        if (strRet.Contains("{"))
                        {
                            strRet = string.Format(strRet, sList.ToArray());
                        } else
                        {
                            strRet = string.Concat(strRet, ", ", string.Join(", ", sList));
                        }

                    }

                }
            }
            catch (FormatException ex)
            {
                SaveSysError(ex.Message, "DynamicsTMS365.Utilities.formatNGLMessageForDisplay:  Invalid Message Format; the message [ " + m + " ] may require missing parameters");
            }
            catch (Exception ex)
            {
                throw;
            }
            return strRet;
        }

        public static void addCarrierCostResultMessagesToResponse(ref Models.Response response, ref DTO.CarrierCostResults carCostResp, string sPageTitle)
        {
            /*
            response.Err = result.Err.ToArray();
            response.ErrTitle = result.ErrTitle;
            response.Warn = result.Warn.ToArray();
            response.WarningTitle = result.WarningTitle;
            response.Message = result.Message.ToArray();
            response.MsgTitle = result.MsgTitle;
            response.Log = result.Log.ToArray();
            response.LogTitle = result.LogTitle; */
            //public List<DTO.NGLMessage> TempMsg = new List<DTO.NGLMessage>();

            string sRespTitle = "";
            // Process WCF Errors
            response.Err = null;
            // Process Warnings
            sRespTitle = "";
            response.Warn = null;
            // Process Messages
            sRespTitle = "";
            response.Message = convertNGLMsgDictToResponseData("", "Messages", sPageTitle, carCostResp.Messages, ref sRespTitle, ref response);
            bool blnShowlog = true;
            if (carCostResp.Messages != null && carCostResp.Messages.Count() > 0)
            {
                foreach (var oMsgs in carCostResp.Messages)
                {
                    if (oMsgs.Key == "M_InvalidLineHaulCannotRateLoad")
                    {
                        response.Data = new bool[] { true };
                        blnShowlog = false;
                        break;
                    }
                }
            }
            response.MsgTitle = sRespTitle;
            if (blnShowlog)
            {                                   
                // Process Logs --> just convert list to array            
                response.Log = carCostResp.Log.ToArray();
                response.LogTitle = sPageTitle + " Logs";
                response.Messages = "<ul style='text-align:left;'>";
                foreach (DTO.NGLMessage oMsg in carCostResp.Log){
               
                    if (!string.IsNullOrWhiteSpace(oMsg.Message)){
                        response.Messages += " <li>  " + oMsg.Message + "</li> ";
                    }
                
                }
                response.Messages += "</ul>";
            }
        }

        public static string convertWCFMessagesToString(DTO.WCFResults wcfRes)
        {

            string sRet = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (wcfRes != null )
            {               
                if (wcfRes.Messages != null && wcfRes.Messages.Count() > 0)
                {
                    foreach (var oMsgs in wcfRes.Messages)
                    {
                        sb.Append(formatNGLMessageForDisplay(oMsgs.Key, oMsgs.Value.ToList()));
                        sb.Append("\n\r<br /> ");
                    }
                }
                if (wcfRes.Warnings != null && wcfRes.Warnings.Count() > 0)
                {
                    foreach (var oMsgs in wcfRes.Warnings)
                    {
                        sb.Append(formatNGLMessageForDisplay(oMsgs.Key, oMsgs.Value.ToList()));
                        sb.Append("\n\r<br /> ");
                    }
                }
                if (wcfRes.Errors != null && wcfRes.Errors.Count() > 0)
                {
                    foreach (var oMsgs in wcfRes.Errors)
                    {
                        sb.Append(formatNGLMessageForDisplay(oMsgs.Key, oMsgs.Value.ToList()));
                        sb.Append("\n\r<br /> ");
                    }
                }
                sRet = sb.ToString();
            }
            
            return sRet;
        }




        /// <summary>
        /// Copies WCF result Errors, Warnings, Messages and Logs to a Models.Response object.
        /// Compatible with NGLErrWarnMsgLogCtrl widget
        /// </summary>
        /// <param name="response"></param>
        /// <param name="wcfRes"></param>
        /// <param name="sPageTitle"></param>
        /// <remarks> 
        /// Created by RHR for v-8.3.0.002 on 12/21/2020
        /// </remarks>
        public static void addWCFMessagesToResponse(ref Models.Response response, ref DTO.WCFResults wcfRes, string sPageTitle)
        {
            /*
            response.Err = result.Err.ToArray();
            response.ErrTitle = result.ErrTitle;
            response.Warn = result.Warn.ToArray();
            response.WarningTitle = result.WarningTitle;
            response.Message = result.Message.ToArray();
            response.MsgTitle = result.MsgTitle;
            response.Log = result.Log.ToArray();
            response.LogTitle = result.LogTitle; */
            //public List<DTO.NGLMessage> TempMsg = new List<DTO.NGLMessage>();


            string sRespTitle = "";
            // Process WCF Errors
            response.Err = convertNGLMsgDictToResponseData("", "Errors", sPageTitle, wcfRes.Errors, ref sRespTitle, ref response);
            response.ErrTitle = sRespTitle;
            // Process Warnings
            sRespTitle = "";
            response.Warn = convertNGLMsgDictToResponseData("", "Warnings", sPageTitle, wcfRes.Warnings, ref sRespTitle, ref response);
            response.WarningTitle = sRespTitle;
            // Process Messages
            sRespTitle = "";
            response.Message = convertNGLMsgDictToResponseData("", "Messages", sPageTitle, wcfRes.Messages, ref sRespTitle, ref response);
            response.MsgTitle = sRespTitle;
            // Process Logs --> just convert list to array            
            response.Log = wcfRes.Log.ToArray();
            response.LogTitle = sPageTitle + " Logs";
        }

        /// <summary>
        /// Copies DAL ResultObject data to Models.Response object.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="oMRes"></param>
        /// <param name="sPageTitle"></param>
        /// <remarks>
        /// Created by RHR for v-8.4.0.003 on 09/12/2021
        /// Modified by RHR for v-8.5.3.006 on 01/04/2023 added logic to set success to false on error or warning 
        ///     and to populate error and message strings if empty when error or messages are reported.
        /// </remarks>
        public static void addResultObjectMessagesToResponse(ref Models.Response response, ref Ngl.FreightMaster.Data.Models.ResultObject oMRes, string sPageTitle, bool bFalseOnError = false, bool bFalseOnWarning = false)
        {
           
            string sRespTitle = sPageTitle + " Errors"; //default for Errors
            // Process WCF Errors
           
            response.Err = oMRes.Err.ToArray(); // convertNGLMsgDictToResponseData("", "Errors", sPageTitle, oMRes.Errors, ref sRespTitle, ref response);
            if (response.Err.Count() > 0 )
            {
                // Modified by RHR for v-8.5.3.006 on 01/04/2023 
                if (bFalseOnError) { oMRes.Success = false; }
                if (string.IsNullOrWhiteSpace(oMRes.ErrMsg))
                {
                    foreach (DTO.NGLMessage oError in response.Err)
                    {
                        oMRes.ErrMsg = string.Concat(oMRes.ErrMsg, oError.Message);
                    }
                }
            }
            response.Errors = oMRes.ErrMsg;
            if (!string.IsNullOrWhiteSpace(oMRes.ErrTitle)) { sRespTitle = oMRes.ErrTitle;  }
            response.ErrTitle =  sRespTitle;
            // Process Warnings          
            sRespTitle = sPageTitle + " Warnings"; //default for Warnings
            response.Warn = oMRes.Warn.ToArray();
            if (response.Warn.Count() > 0 )
            {
                // Modified by RHR for v-8.5.3.006 on 01/04/2023 
                if (bFalseOnWarning) { oMRes.Success = false; }                
                if (string.IsNullOrWhiteSpace(oMRes.WarningMsg))
                {
                    foreach (DTO.NGLMessage oMsg in response.Warn)
                    {
                        oMRes.WarningMsg = string.Concat(oMRes.WarningMsg, oMsg.Message);
                    }
                }
            }
            response.Warnings = oMRes.WarningMsg;
            if (!string.IsNullOrWhiteSpace(oMRes.WarningTitle)) { sRespTitle = oMRes.WarningTitle; }
            response.WarningTitle = sRespTitle;
            // Process Messages
            sRespTitle = sPageTitle + " Messages"; //default for Messages
            
            response.Message = oMRes.Message.ToArray();
            if (response.Message.Count() > 0 )
            {
                // Modified by RHR for v-8.5.3.006 on 01/04/2023 
                if (string.IsNullOrWhiteSpace(oMRes.Msg))
                {
                    foreach (DTO.NGLMessage oMsg in response.Message)
                    {
                        oMRes.Msg = string.Concat(oMRes.Msg, oMsg.Message);
                    }
                }
            }
            response.Messages = oMRes.Msg;
            if (!string.IsNullOrWhiteSpace(oMRes.MsgTitle)) { sRespTitle = oMRes.MsgTitle; }
            response.MsgTitle = sRespTitle;
            // Process Logs
            sRespTitle = sPageTitle + " Logs"; //default for Messages
            response.Log = oMRes.Log.ToArray();
            if (!string.IsNullOrWhiteSpace(oMRes.LogTitle)) { sRespTitle = oMRes.LogTitle; }
            response.LogTitle = sRespTitle;
            
        }

        /// <summary>
        /// Converts a WCF Message Dictionary to a Response Array
        /// </summary>
        /// <param name="sMsgTitle"></param>
        /// <param name="sTitle"></param>
        /// <param name="sPageTitle"></param>
        /// <param name="nglMsgDict"></param>
        /// <param name="sRespTitle"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.3.0.002 on 12/21/2020
        /// WCF Errors, Messages and Warnings store messages in a dictionary
        /// Models.Response stores messages as a list of NGL Messages
        /// We need to convert the WCF data to a list of NGL Messages
        /// one Dictionary object in WCF = one NGLMessage in Resonse
        /// The dictionary contains a key (localized message key) and an 
        /// Optional list of string format details stored in the NGLMessage.Message property
        /// </remarks>
        public static DTO.NGLMessage[] convertNGLMsgDictToResponseData(string sMsgTitle, string sTitle, string sPageTitle,  Dictionary<string, List<DTO.NGLMessage>> nglMsgDict, ref string sRespTitle, ref Models.Response response)
        {
            List<DTO.NGLMessage> lNGLMsg = new List<DTO.NGLMessage>();
            if (string.IsNullOrWhiteSpace(sMsgTitle))
            {
                sRespTitle = sPageTitle + " " + sTitle;
            }
            else
            {
                sRespTitle = sMsgTitle;
            }
            if (nglMsgDict != null && nglMsgDict.Count() > 0)
            {
                foreach (var oMsgs in nglMsgDict)
                {
                    string sMessage = formatNGLMessageForDisplay(oMsgs.Key, oMsgs.Value.ToList());
                    if (response != null && !string.IsNullOrWhiteSpace(sMessage))
                    {
                        if (sTitle == "Messages")
                        {
                            response.Messages = string.Concat(response.Message, "\n<br>", sMessage);
                        } else if( sTitle == "Errors") {
                            response.Errors = string.Concat(response.Errors, "\n<br>", sMessage);
                        }
                        else if (sTitle == "Warnings")
                        {
                            response.Warnings = string.Concat(response.Warnings, "\n<br>", sMessage);
                        }
                    }                 
                    lNGLMsg.Add(new DTO.NGLMessage(sMessage));

                }
            }
            return lNGLMsg.ToArray();
        }

        public static string formatLocalizedMsg(string skey, List<string> DetailsList, Object Culture = null)
        {
            string sMsg = "";
            if (!string.IsNullOrWhiteSpace(skey))
            {
                sMsg = Utilities.getLocalizedMsg(skey, Culture);
            }
            if (DetailsList != null && DetailsList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(sMsg))
                {
                    if (sMsg.Contains("{0}"))
                    {
                        try
                        {
                            sMsg = string.Format(sMsg, DetailsList.ToArray());
                        }
                        catch (Exception ex)
                        {
                            //return the message without any { or }
                            sMsg = sMsg.Replace("{", "");
                            sMsg = sMsg.Replace("}", "");
                        }
                    }
                    else
                    {
                        sMsg = string.Concat(sMsg, ": " + String.Join(",", DetailsList));
                    }

                }
                else
                {
                    sMsg = String.Join(",", DetailsList);
                }
            }


            return sMsg;
        }


        /// <summary>
        /// Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
        /// </summary>
        /// <param name="oResults"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string formatWCFResultErrors(DTO.WCFResults oResults, Object Culture = null)
        {
            string sMsg = null;
            try
            {
                if (oResults == null) return sMsg;

                if (oResults.Errors != null && oResults.Errors.Count() > 0)
                {
                    foreach (var item in oResults.Errors)
                    {
                        string skey = item.Key;
                        List<DTO.NGLMessage> details = item.Value;
                        List<string> DetailList = new List<string>();
                        if (details != null && details.Count() > 0)
                        {
                            foreach (DTO.NGLMessage m in details)
                            {
                                DetailList.Add(m.Message);
                            }
                        }
                        sMsg += string.Concat(LineBreak, htmlLineBreak, formatLocalizedMsg(skey, DetailList, Culture));
                    }
                }
            }
            catch (Exception ex)
            {
                //do nothing cannot process messages
            }

            return sMsg;
        }

        /// <summary>
        /// Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
        /// </summary>
        /// <param name="oResults"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string formatWCFResultMessages(DTO.WCFResults oResults, Object Culture = null)
        {
            string sMsg = "";
            try
            {
                if (oResults == null) return sMsg;

                if (oResults.Messages != null && oResults.Messages.Count() > 0)
                {
                    foreach (var item in oResults.Messages)
                    {
                        string skey = item.Key;
                        List<DTO.NGLMessage> details = item.Value;
                        List<string> DetailList = new List<string>();
                        if (details != null && details.Count() > 0)
                        {
                            foreach (DTO.NGLMessage m in details)
                            {
                                DetailList.Add(m.Message);
                            }
                        }
                        sMsg += formatLocalizedMsg(skey, DetailList, Culture);
                    }
                }
            }
            catch (Exception ex)
            {
                //do nothing cannot process messages
            }

            return sMsg;
        }

        /// <summary>
        /// Deprecated in v-8.3.0.002 we now use addWCFMessagesToResponse and the NGLErrWarnMsgLogCtrl widget
        /// </summary>
        /// <param name="oResults"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public static string formatWCFResultWarnings(DTO.WCFResults oResults, Object Culture = null)
        {
            string sMsg = "";
            try
            {
                if (oResults == null) return sMsg;
                if (oResults.Warnings != null && oResults.Warnings.Count() > 0)
                {
                    foreach (var item in oResults.Warnings)
                    {
                        string skey = item.Key;
                        List<DTO.NGLMessage> details = item.Value;
                        List<string> DetailList = new List<string>();
                        if (details != null && details.Count() > 0)
                        {
                            foreach (DTO.NGLMessage m in details)
                            {
                                DetailList.Add(m.Message);
                            }
                        }
                        sMsg += formatLocalizedMsg(skey, DetailList, Culture);
                    }
                }
            }
            catch (Exception ex)
            {
                //do nothing cannot process messages
            }

            return sMsg;
        }
        public static string getLocalizedMsg(string skey, Object Culture = null)
        {
            if (string.IsNullOrWhiteSpace(skey)) { return ""; }
            string sRet = skey;
            try
            {
                if (GlobalLocalizedMsgs == null || GlobalLocalizedMsgs.Count() < 1)
                {
                    loadLocalizedMsgs();
                }
                if (GlobalLocalizedMsgs.ContainsKey(skey))
                {
                    LTS.cmLocalizeKeyValuePair oLocal = GlobalLocalizedMsgs[skey];
                    return GetLocalizedString(oLocal.cmLocalValue, oLocal.cmLocalValueLocal, Culture);
                }
                else
                {
                    LTS.cmLocalizeKeyValuePair oLocal = findLocalizedMsg(skey);
                    if (oLocal != null)
                    {
                        return GetLocalizedString(oLocal.cmLocalValue, oLocal.cmLocalValueLocal, Culture);
                    }

                }
            }
            catch (Exception ex)
            {
                SaveSysError(ex.Message, "DynamicsTMS365.Utilities.getLocalizedMsg");
            }
            return sRet;
        }

        public static void loadLocalizedMsgs()
        {
            try
            {
                GlobalLocalizedMsgs = new Dictionary<string, LTS.cmLocalizeKeyValuePair>();
                DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(DALWCFParameters);
                //List<LTS.cmLocalizeKeyValuePair> oRecords = new List<LTS.cmLocalizeKeyValuePair>();
                List<LTS.cmLocalizeKeyValuePair> oRecords = dalData.GetAll();
                if (oRecords != null && oRecords.Count() > 0)
                {
                    foreach (LTS.cmLocalizeKeyValuePair row in oRecords)
                    {
                        if (!GlobalLocalizedMsgs.ContainsKey(row.cmLocalKey))
                        {
                            GlobalLocalizedMsgs.Add(row.cmLocalKey, row);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SaveSysError(ex.Message, "DynamicsTMS365.Utilities.loadLocalizedMsgs");
            }
        }

        public static LTS.cmLocalizeKeyValuePair findLocalizedMsg(string sKey)
        {

            LTS.cmLocalizeKeyValuePair oRet = null;
            try
            {
                if (GlobalLocalizedMsgs == null)
                {
                    loadLocalizedMsgs();
                }
                if (GlobalLocalizedMsgs.ContainsKey(sKey))
                {
                    oRet = GlobalLocalizedMsgs[sKey];
                }
                else
                {
                    DAL.NGLcmLocalizeKeyValuePairData dalData = new DAL.NGLcmLocalizeKeyValuePairData(DALWCFParameters);
                    oRet = dalData.FindRecordByKey(sKey);
                    if (oRet != null && oRet.cmLocalControl != 0)
                    {
                        GlobalLocalizedMsgs.Add(sKey, oRet);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveSysError(ex.Message, "DynamicsTMS365.Utilities.findLocalizedMsg");
            }

            return oRet;
        }

        /// <summary>
        /// Calculates the page and pageSize using skip and take
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        public static void getPageFromSkipTake(int skip, int take, ref int page, ref int pagesize)
        {
            page = skip;
            if (skip >= take)
            {
                int tpage = (skip / take) + 1;
                page = tpage;
            }
            pagesize = take;
        }

        public static void SaveSysError(string Message, string errorProcedure, string record = "", int errorNumber = 0, int errorSeverity = 0, int errorState = 0, int errorLineNber = 0)
        {
            try
            {
                DAL.NGLSystemDataProvider dalSysData = new DAL.NGLSystemDataProvider(Utilities.DALWCFParameters);
                dalSysData.CreateSystemErrorByMessage(Message, Utilities.DALWCFParameters.UserName, errorProcedure, record, errorNumber, errorSeverity, errorState, errorLineNber);

            }
            catch (Exception)
            {
                //we ignore all errors while saving application error data
            }
        }

        public static void SaveAppError(string Message)
        {
            try
            {
                DAL.NGLSystemDataProvider dalSysData = new DAL.NGLSystemDataProvider(Utilities.DALWCFParameters);
                dalSysData.CreateAppErrorByMessage(Message, Utilities.DALWCFParameters.UserName);
                //we ignore all errors while saving application error data
            }
            catch
            {
                //do nothing
            }

        }

        public static string GetLocalizedString(string Caption, string CaptionLocal, Object Culture)
        {
            return Caption;
        }

        public static FaultExceptionEventArgs ManageExceptions(ref Exception ex)
        {
            FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
            var t = ex.GetType();

            if (ex.GetType() == typeof(FaultException<DAL.SqlFaultInfo>))
            {
                //object val = ((FaultException<DAL.SqlFaultInfo>)ex).Detail.DetailsList
                if ((((FaultException<DAL.SqlFaultInfo>)ex).Detail != null) && (((FaultException<DAL.SqlFaultInfo>)ex).Detail.DetailsList != null) && (((FaultException<DAL.SqlFaultInfo>)ex).Detail.DetailsList.Count() > 0))
                {
                    fault = new FaultExceptionEventArgs(ex, false, null, ((FaultException<DAL.SqlFaultInfo>)ex).Reason.ToString(), ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Message, ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Details, ((FaultException<DAL.SqlFaultInfo>)ex).Detail.DetailsList);
                }
                else
                {
                    fault = new FaultExceptionEventArgs(ex, false, null, ((FaultException<DAL.SqlFaultInfo>)ex).Reason.ToString(), ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Message, ((FaultException<DAL.SqlFaultInfo>)ex).Detail.Details);
                }

                fault.StatusCode = HttpStatusCode.InternalServerError;
            }
            else if (ex.GetType() == typeof(TimeoutException))
            {
                fault = new FaultExceptionEventArgs(ex, false, null, "Timedout", "OperationTimedout");
                fault.StatusCode = HttpStatusCode.RequestTimeout;
            }
            else if (ex.GetType() == typeof(System.ServiceModel.ProtocolException))
            {
                if ((DALWCFParameters != null))
                    SaveAppError(ex.ToString());
                fault = new FaultExceptionEventArgs(ex, false, null, Utilities.getLocalizedMsg("E_WCFCommunicationProtocolFailure"), Utilities.getLocalizedMsg("E_WCFCommunicationSaveItemFailure"));
                fault.StatusCode = HttpStatusCode.BadRequest;
            }
            else if (ex.GetType() == typeof(System.ServiceModel.CommunicationException))
            {
                if ((DALWCFParameters != null))
                    SaveAppError(ex.ToString());
                fault = new FaultExceptionEventArgs(ex, false, null, Utilities.getLocalizedMsg("E_WCFCommunicationSaveDataFailure"), Utilities.getLocalizedMsg("E_WCFCommunicationSaveItemFailure"));
                fault.StatusCode = HttpStatusCode.InternalServerError;
            }
            else if (ex.GetType() == typeof(FaultException<DAL.ConflictFaultInfo>))
            {
                //Op                if ((DALWCFParameters != null))
                //SaveAppError(ex.ToString());
                FaultException < DAL.ConflictFaultInfo> oSQLConflict = (FaultException<DAL.ConflictFaultInfo>)ex;
                string sConflictData = oSQLConflict.Detail.Message;
                sConflictData = sConflictData.Replace(Environment.NewLine, "<br />");

                fault = new FaultExceptionEventArgs(ex, false, null, Utilities.getLocalizedMsg(oSQLConflict.Reason.ToString()), sConflictData);
                fault.StatusCode = HttpStatusCode.InternalServerError;

            }
            else
            {
                if ((DALWCFParameters != null))
                    SaveAppError(ex.ToString());
                fault = new FaultExceptionEventArgs(ex, false, null, Utilities.getLocalizedMsg("E_UnExpected"), ex.Message);
                fault.StatusCode = HttpStatusCode.ServiceUnavailable;
            }
            // Modified by RHR for v-8.5.3.006 added message formating for Access Denied
            if (fault.Reason == "E_AccessDenied")
            {
                fault.GetLocalReasonAndMessage();

            } else if (fault.Reason == "E_AccessGranted")
            {
                fault.GetLocalReasonAndMessage();
            }
                return fault;
        }

        public static FaultExceptionEventArgs BuildException(string sDetail, List<string> sDetailList, string sReason = "E_DataValidationFailure")
        {
            FaultExceptionEventArgs fault = new FaultExceptionEventArgs(null, false, null, sReason, "", sDetail, sDetailList);

            return fault;
        }

        /// <summary>
        /// Generate a random password of characters, numbers, and special characters
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.2.006 on 03/03/2023 added new security rules to match highest level of security setting 3
        /// </remarks>
        public static string GenerateRandomPassword()
        {
            Random random = new Random();

            
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(RandomString(3, true, ref random));
            builder.Append(random.Next(1000,9999).ToString());
            builder.Append(RandomString(3, false, ref random));
            builder.Append("^");
            return builder.ToString();

            //string password = string.Empty;
            //int iUselower = 0;
            //for (int i = 0; i < 10; i++)
            //{
                
            //    if (i%2 == 0)
            //    {
            //        //add a random integer for all even numbers
            //        password = String.Concat(password, random.Next(10).ToString());
            //    }
            //    else
            //    {
            //        //add a random string for all odd numbers
            //        if (iUselower < 3)
            //        {
            //            //the first 3 letters are upper case letters all others are lower case
            //            password = String.Concat(password, RandomString(1, false));
            //        } else
            //        {
            //            password = String.Concat(password, RandomString(1,true));
            //        }
            //    }             
             
            //}
            ////add a special character
            //password = String.Concat(password, "^");
            //return password;
        }

        /// <summary>
        /// Creates a random string for size of characters
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 03/03/2023 
        ///   the caller must create the random object so each call gets a new set of characters
        /// </remarks>
        public static string RandomString(int size, bool lowerCase, ref Random random)
        {
           System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

       

        public static string getUserTheme(int UserControl)
        {
            string strRet = "";
            if (GlobalSSOResultsByUser.ContainsKey(UserControl))
            {
                DAL.Models.SSOResults ssoRes = GlobalSSOResultsByUser[UserControl]; //Modified By LVV on 4/9/20 - bug fix null reference exception
                if (ssoRes != null) { strRet = ssoRes.UserTheme365; }
            }
            if (string.IsNullOrWhiteSpace(strRet)) { strRet = "blueopal"; }
            return strRet;
        }

        /// <summary>
        /// reverse the changes made by ngl.encodeNGLReservedCharacters in client 
        /// </summary>
        /// <param name="sVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.2.006 on 2023-01-20 added dot character
        /// </remarks>
        public static string decodeNGLReservedCharacters(string sVal)
        {

            string asciichar = (Convert.ToChar(200)).ToString();
            sVal = sVal.Replace(asciichar, "\\");

            asciichar = (Convert.ToChar(201)).ToString();
            sVal = sVal.Replace(asciichar, "/");

            asciichar = (Convert.ToChar(202)).ToString();
            sVal = sVal.Replace(asciichar, ".");
            return sVal;
        }

        /// <summary>
        /// old phone number logic to remove all non numeric characters (do no use)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.5.4.006 on 05/21/2024
        ///  removed this logic just return the source string
        /// </remarks>
        public static string removeNonNumericText(string source)
        {
            return source;

            //if (!string.IsNullOrWhiteSpace(source))
            //{
            //    return System.Text.RegularExpressions.Regex.Replace(source, "[^.0-9]", "");
            //}
            //else { return source; }

        }

        public static string cleanFormatOrTemplateData(string source, string sDefault)
        {
            if (sDefault == null) { sDefault = ""; }
            string sRet = string.IsNullOrWhiteSpace(source) ? sDefault : source;



            //all format or template data must be encapsulated in double quotes
            if (sRet != null && sRet.Length > 0)
            {
                //remove any leading and trailing double quotes so we can test for internal double quotes that need to be escaped
                if (sRet.Substring(0, 1) == "\"") { sRet = sRet.Substring(1); }
                if (sRet.Substring(sRet.Length - 1, 1) == "\"") sRet = sRet.Substring(0, sRet.Length - 1);
                //escape any internal double quotes
                if (sRet.Contains("\"")) { sRet = sRet.Replace("\"", "\\\""); }
                //add leading and trailing double quotes
                if (sRet.Length > 3 && sRet.Substring(0, 3).ToLower() != "fun") { sRet = "\"" + sRet + "\""; }
                //if (sRet.Substring(0, 1) != "\"") { sRet = "\"" + sRet; }
                //if (sRet.Substring(sRet.Length - 1, 1) != "\"") { sRet = sRet + "\""; }
            }

            return sRet;
        }

        #region " Date Time validation Methods"

        /// <summary>
        /// Create a new timespan using timeInput if valid or returns 12 midnight if not valid
        /// </summary>
        /// <param name="timeInput"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 12/28/2022 
        ///     function used to convert timeInput string to a TimeSpan value
        /// </remarks>
        public static TimeSpan getNewTimeSpan(string timeInput)
        {
            TimeSpan tsRet = new TimeSpan(0, 0, 0);
            if (!string.IsNullOrWhiteSpace(timeInput))
            {
                TimeSpan.TryParse(timeInput, out tsRet);
            }
            return tsRet;
        }

        /// <summary>
        /// returns a new datetime using date value of dtVal and hours minues and seconds of timeInput string if it is a valid date or timespan 
        /// </summary>
        /// <param name="timeInput"></param>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 12/28/2022 
        ///     function used to add timeInput to dtVal as new datetime
        /// </remarks>
        public static DateTime? updateTimeSpan(string timeInput, DateTime? dtVal)
        {
            if (dtVal.HasValue)
            {
                TimeSpan tsRet = new TimeSpan(0, 0, 0);
                DateTime dtTime = dtVal.Value;
                
                if (!string.IsNullOrWhiteSpace(timeInput))
                {
                    if (DateTime.TryParse(timeInput, out dtTime))
                    {
                        tsRet = new TimeSpan( dtTime.Hour, dtTime.Minute, dtTime.Second);
                    } else
                    {
                        TimeSpan.TryParse(timeInput, out tsRet);
                    }                   
                }
                dtVal = dtVal.Value.Date + tsRet;
            }
            
            return dtVal;
        }

        /// <summary>
        /// returns a new datetime using date value of dtVal and   hours minues and seconds of dtTime 
        /// </summary>
        /// <param name="dtTime"></param>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 12/28/2022 
        ///     function used to add dtTime to dtVal as new datetime
        /// </remarks>
        public static DateTime? updateTimeSpan(DateTime? dtTime, DateTime? dtVal)
        {
            if (dtVal.HasValue)
            {
                TimeSpan tsRet = new TimeSpan(0, 0, 0);

                if (dtTime.HasValue)
                {
                    tsRet = new TimeSpan(dtTime.Value.Hour, dtTime.Value.Minute, dtTime.Value.Second);                  
                }
                dtVal = dtVal.Value.Date + tsRet;
            }

            return dtVal;
        }

        /// <summary>
        /// Returns true if timeInput is a valud timespan
        /// </summary>
        /// <param name="timeInput"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.2.006 on 12/28/2022 
        ///     function used to test if a timeInput string is a vlaid TimeSpan
        /// </remarks>
        public static bool isTimeValid(string timeInput)
        {
            bool blnRet = false;
            TimeSpan tsRet = new TimeSpan(0, 0, 0);
            if (!string.IsNullOrWhiteSpace(timeInput))
            {
                if (TimeSpan.TryParse(timeInput, out tsRet))
                {
                    blnRet = true;
                }
            }
            return blnRet;
        }

        #endregion

        /// <summary>
        /// Convert Nullable datetime to US Standard Short Date String
        /// </summary>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        ///     Overload uses nullable date 
        /// </remarks>
        public static string convertDateToShortDateString(DateTime? dtVal)
        {
           return dtVal.HasValue? dtVal.Value.ToString("MM/dd/yyyy") : "";
        }

        /// <summary>
        ///  Convert Nullable datetime to US Standard Short Date String
        /// </summary>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        /// </remarks>
        public static string convertDateToShortDateString(DateTime dtVal)
        {
            return dtVal.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Convert Nullable datetime to US Standard Date and Time String MM/dd/yyyy h:mm tt
        /// </summary>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        ///     Overload uses nullable date 
        /// </remarks>
        public static string convertDateToDateTimeString(DateTime? dtVal)
        {
            return dtVal.HasValue ? dtVal.Value.ToString("MM/dd/yyyy h:mm:ss tt") : "";
        }

        /// <summary>
        ///  Convert Nullable datetime to US Standard Short Date String
        /// </summary>
        /// <param name="dtVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        /// </remarks>
        public static string convertDateToDateTimeString(DateTime dtVal)
        {
            return dtVal.ToString("MM/dd/yyyy h:mm:ss tt");
        }

        /// <summary>
        /// Convert a string to nullable datetime value
        /// </summary>
        /// <param name="sVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        /// </remarks>
        public static DateTime? convertStringToNullDateTime(string sVal)
        {
            DateTime? dtRet = null;
            DateTime dtTest = DateTime.Now;
            if (string.IsNullOrWhiteSpace(sVal) || sVal.Length < 8)
            {
                return dtRet;
            } else
            {
                if (DateTime.TryParse(sVal, out dtTest))
                {
                    dtRet = dtTest;
                }
            }
            return dtRet;
        }

        /// <summary>
        /// Convert a string to datetime value use the current date and time as default if string is not valid
        /// </summary>
        /// <param name="sVal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.5.3.007 on 2023-02-15
        /// </remarks>
        public static DateTime convertStringToDateTime(string sVal)
        {
            DateTime dtRet = DateTime.Now;
            DateTime dtTest = DateTime.Now;
            if (string.IsNullOrWhiteSpace(sVal) || sVal.Length < 8)
            {
                return dtRet;
            }
            else
            {
                if (DateTime.TryParse(sVal, out dtTest))
                {
                    dtRet = dtTest;
                }
            }
            return dtRet;
           
        }

        #region " Web Service Replacement Methods"

        /// <summary>
        /// Wrapper method to populate Integration settings from web config
        /// </summary>
        /// <param name="oImportExport"></param> 
        /// <remarks>
        ///  Created by RHR for v-8.5.0.001 on 10/08/2021
        /// </remarks>
        internal static void populateIntegrationObjectParameters( Ngl.FreightMaster.Integration.clsImportExport oImportExport)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NGLMASPROD"].ConnectionString;
            {

                //check Ngl.FreightMaster.Integration to determin if we need to read parameter settings
                // for now we set to empty string
                // all email logic in Ngl.FreightMaster.Integration  should use new wrapper methods
                // validate we are using parameter setting in Ngl.FreightMaster.Integration 
                //oImportExport.AdminEmail = Utilities.GetSetting("AdminEMail");
                //oImportExport.FromEmail = Utilities.GetSetting("EMailFrom");
                //oImportExport.GroupEmail = Utilities.GetSetting("GroupEMail");
                //oImportExport.Retry = Utilities.GetSetting("Retries");
                //oImportExport.SMTPServer = Utilities.GetSetting("SmtpMailServer");
                //oImportExport.Debug = Utilities.GetDebugMode();
                DTO.GlobalTaskParameters oRet = Utilities.GetGlobalTaskParameters();
                oImportExport.AdminEmail = oRet.GlobalAdminEmail;
                oImportExport.FromEmail = oRet.GlobalFromEmail;
                oImportExport.GroupEmail = oRet.GlobalGroupEmail;
                oImportExport.Retry = oRet.GlobalAutoRetry;
                oImportExport.SMTPServer = oRet.GlobalSMTPServer;
                oImportExport.Debug = oRet.GlobalDebugMode;
                oImportExport.DBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
                oImportExport.Database = System.Configuration.ConfigurationManager.AppSettings["Database"];
                oImportExport.ConnectionString = connectionString;
                oImportExport.AuthorizationCode = System.Configuration.ConfigurationManager.AppSettings["FMWSAuthCode"];
                oImportExport.WCFAuthCode = System.Configuration.ConfigurationManager.AppSettings["FMWCFAuthCode"];
                oImportExport.WCFURL = System.Configuration.ConfigurationManager.AppSettings["FMWCFServiceURL"];
                oImportExport.WCFTCPURL = System.Configuration.ConfigurationManager.AppSettings["FMWCFNETTCPServiceURL"];
            }
        }

        internal static DTO.GlobalTaskParameters GetGlobalTaskParameters()
        {
            DTO.GlobalTaskParameters oRet = new DTO.GlobalTaskParameters();
            try
            {
                Ngl.FreightMaster.Data.WCFParameters WCFParameters = Utilities.DALWCFParameters;
                Ngl.FreightMaster.Data.NGLSystemDataProvider oSystem = new Ngl.FreightMaster.Data.NGLSystemDataProvider(Utilities.DALWCFParameters);
                oRet = oSystem.GetGlobalTaskParameters();
            }
            catch (Exception ex)
            {
                //ignore errors just return defauls
            }                
            return oRet;
        }


        /// <summary>
        /// new web service validation logic
        /// </summary>
        /// <param name="AuthorizationCode"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Created by RHR for v-8.5.0.001 on 10/08/2021 
        ///    current logic uses web config settings but this should be 
        ///    updated to use Bearer Token logic 
        /// </remarks>
        internal static bool validateAuthCode(string AuthorizationCode)
        {
            try
            {
                if (AuthorizationCode == System.Configuration.ConfigurationManager.AppSettings["FMWSAuthCode"])
                    return true;
                else
                    throw new ApplicationException("Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        internal static void LogException(string sSubject, int Result, string logMessage, Exception ex, string AuthorizationCode, string strHeader = "")
        {
            LogResults(sSubject, Result, logMessage + ex.ToString(), AuthorizationCode);
            try
            {
                string strMsg = "<p>" + logMessage + "</p>\n";
                if (strHeader.Trim().Length > 0)
                    strMsg = "<h2>" + strHeader + "\n</h2>" + strMsg;
                strMsg += "<hr />\n" ;
                strMsg += ex.ToString();
                strMsg += "\n<hr />\n" + "\n<p>Using Authorization Code: " + AuthorizationCode + "</p>";
                DAL.NGLEmailData oMail = new DAL.NGLEmailData(DALWCFParameters);
                // new logic supports empty send to address.  The system will use TMS Admin Parameter Email
                oMail.GenerateEmail("", "", sSubject, strMsg, "", "", "", "");
             }
            catch (Exception e)
            {
                //ignore any log or email exceptions
            }
        }


        internal static void LogResults(string ModuleName, int Result, string LastError, string AuthorizationCode)
        {
            try
            {
                string sLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sLogFile, true))
                {
                    sw.WriteLine(string.Format("{0},{1},{2},{3},{4}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), ModuleName, Result, LastError, AuthorizationCode));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion



        #region " File IO Functions"

        /// <summary>
        /// adds a timestamp and extension to a filename, do not include an extension as part of sFilenameBase
        /// </summary>
        /// <param name="sFilenameBase"></param>
        /// <param name="sExtension"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.2 on 08/23/2018
        /// </remarks>
        public static string timeStampFileName(string sFilenameBase, string sExtension)
        {
            if (string.IsNullOrEmpty(sExtension)) { sExtension = ".txt"; } else if (sExtension.Substring(0, 1) != ".") { sExtension = "." + sExtension; }
            string sFilename = sFilenameBase + sExtension;
            try
            {

                DateTime dt = DateTime.Now;
                System.Globalization.DateTimeFormatInfo dfi = new System.Globalization.DateTimeFormatInfo();
                dfi.DateSeparator = "-";
                dfi.TimeSeparator = "-";
                sFilename = string.Concat(sFilenameBase, "-", dt.Month.ToString(), "-", dt.Day.ToString(), "-", dt.Year.ToString(), "-", dt.Hour.ToString(), "-", dt.Minute.ToString(), "-", dt.Second.ToString(), sExtension);
                return sFilename;
            }
            catch (Exception exc)
            {
                return sFilename;
            }

        }

        /// <summary>
        /// Reads a Parameter Value for the master company assigned to the provided/users Legal Entity
        /// </summary>
        /// <param name="ParKey"></param>
        /// <param name="LEControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 05/07/2021 get par val by LE
        /// </remarks>
        public static double GetParValueByLegalEntity(string ParKey, int LEControl)
        {
            double dblRet = 0;
            try
            {
                DAL.NGLParameterData dalParData = new DAL.NGLParameterData(Utilities.DALWCFParameters);
                dblRet = dalParData.GetParValueByLegalEntity(ParKey, LEControl);

            }
            catch (Exception)
            {
                //we ignore all errors while saving application error data
            }
            return dblRet;
        }

        /// <summary>
        /// Reads a Parameter Text for the master company assigned to the provided/users Legal Entity
        /// </summary>
        /// <param name="ParKey"></param>
        /// <param name="LEControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Created by RHR for v-8.4.0.002 on 05/07/2021 get par val by LE
        /// </remarks>
        public static string GetParTextByLegalEntity(string ParKey, int LEControl)
        {
            string sRet = "";
            try
            {
                DAL.NGLParameterData dalParData = new DAL.NGLParameterData(Utilities.DALWCFParameters);
                sRet = dalParData.GetParTextByLegalEntity(ParKey, LEControl);

            }
            catch (Exception)
            {
                //we ignore all errors while saving application error data
            }
            return sRet;
        }

        public static bool SavePageSetting(string UserPSName,string UserPSValue, int UserPSPageControl, int UserPSUserSecurityControl)
        {
            bool bRet = true;
            try
            {
                DAL.NGLUserPageSettingData dalUserPSData = new DAL.NGLUserPageSettingData(Utilities.DALWCFParameters);
                LTS.tblUserPageSetting oRecord = new LTS.tblUserPageSetting() {UserPSUserSecurityControl = UserPSUserSecurityControl,UserPSPageControl = UserPSPageControl, UserPSName = UserPSName,UserPSMetaData = UserPSValue };
                bRet = dalUserPSData.SaveCurrentUserPageSetting(oRecord);

            }
            catch (Exception)
            {
                //we ignore all errors while saving application error data
            }
            return bRet;
        }

        public static string ReadPageSetting(string UserPSName,int UserPSPageControl, int UserPSUserSecurityControl)
        {
            string sRet = "";
            try
            {
                DAL.NGLUserPageSettingData dalUserPSData = new DAL.NGLUserPageSettingData(Utilities.DALWCFParameters);
                LTS.tblUserPageSetting[] lPageSettings = dalUserPSData.GetPageSettings(UserPSPageControl, UserPSName, UserPSUserSecurityControl);
                if (lPageSettings != null && lPageSettings.Length > 0)
                {
                    foreach (var lPageSetting in lPageSettings)
                    {
                        if (lPageSetting != null && lPageSetting.UserPSName == UserPSName && !string.IsNullOrWhiteSpace(lPageSetting.UserPSMetaData))
                        {
                            sRet = lPageSetting.UserPSMetaData;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //we ignore all errors while saving application error data
            }   
            return sRet;
        }
        #endregion

    }
}