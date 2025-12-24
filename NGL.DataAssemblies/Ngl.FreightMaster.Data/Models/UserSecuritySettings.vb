'------------------------------------------------------------------------------
' Created By RHR for v-8.0 on 3/10/2017
'   New Settings Model used to store old file user setting in the database
'------------------------------------------------------------------------------

Public Class UserSecuritySettings

    Private _CarrierControl() As Integer
    Public Property CarrierControl() As Integer
        Get
            Return _CarrierControl
        End Get
        Set
            _CarrierControl = Value
        End Set
    End Property

    Private _CompanyControl As Integer
    Public Property CompanyControl() As Integer
        Get
            Return _CompanyControl
        End Get
        Set
            _CompanyControl = Value
        End Set
    End Property

    Private _LaneControl As Integer
    Public Property LaneControl() As Integer
        Get
            Return _LaneControl
        End Get
        Set
            _LaneControl = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("2010-01-01")>
    Public Property OPStartDate() As Date
        Get
            Return CType(Me("OPStartDate"), Date)
        End Get
        Set
            Me("OPStartDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("2017-12-31")>
    Public Property OPEndDate() As Date
        Get
            Return CType(Me("OPEndDate"), Date)
        End Get
        Set
            Me("OPEndDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OPNatAcctNbr() As Integer
        Get
            Return CType(Me("OPNatAcctNbr"), Integer)
        End Get
        Set
            Me("OPNatAcctNbr") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OPCompNumber() As Integer
        Get
            Return CType(Me("OPCompNumber"), Integer)
        End Get
        Set
            Me("OPCompNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OPFreightType() As Integer
        Get
            Return CType(Me("OPFreightType"), Integer)
        End Get
        Set
            Me("OPFreightType") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property OPCreateUser() As String
        Get
            Return CType(Me("OPCreateUser"), String)
        End Get
        Set
            Me("OPCreateUser") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OPControl() As Long
        Get
            Return CType(Me("OPControl"), Long)
        End Get
        Set
            Me("OPControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OPFilterType() As Integer
        Get
            Return CType(Me("OPFilterType"), Integer)
        End Get
        Set
            Me("OPFilterType") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CompanyNumber() As String
        Get
            Return CType(Me("CompanyNumber"), String)
        End Get
        Set
            Me("CompanyNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CompanyName() As String
        Get
            Return CType(Me("CompanyName"), String)
        End Get
        Set
            Me("CompanyName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CarrierNumber() As String
        Get
            Return CType(Me("CarrierNumber"), String)
        End Get
        Set
            Me("CarrierNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CarrierName() As String
        Get
            Return CType(Me("CarrierName"), String)
        End Get
        Set
            Me("CarrierName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LaneNumber() As String
        Get
            Return CType(Me("LaneNumber"), String)
        End Get
        Set
            Me("LaneNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LaneName() As String
        Get
            Return CType(Me("LaneName"), String)
        End Get
        Set
            Me("LaneName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("""""")>
    Public Property Setting() As String
        Get
            Return CType(Me("Setting"), String)
        End Get
        Set
            Me("Setting") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property Theme() As String
        Get
            Return CType(Me("Theme"), String)
        End Get
        Set
            Me("Theme") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("en-US")>
    Public Property Language() As String
        Get
            Return CType(Me("Language"), String)
        End Get
        Set
            Me("Language") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property MenuStyle() As String
        Get
            Return CType(Me("MenuStyle"), String)
        End Get
        Set
            Me("MenuStyle") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccCheckEntryCarrierName() As String
        Get
            Return CType(Me("AccCheckEntryCarrierName"), String)
        End Get
        Set
            Me("AccCheckEntryCarrierName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccCheckEntryCarrierNumber() As String
        Get
            Return CType(Me("AccCheckEntryCarrierNumber"), String)
        End Get
        Set
            Me("AccCheckEntryCarrierNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OutlookbarWidth() As Double
        Get
            Return CType(Me("OutlookbarWidth"), Double)
        End Get
        Set
            Me("OutlookbarWidth") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property AccDetailEntryCarrierControl() As Integer
        Get
            Return CType(Me("AccDetailEntryCarrierControl"), Integer)
        End Get
        Set
            Me("AccDetailEntryCarrierControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccDetailEntryCarrierName() As String
        Get
            Return CType(Me("AccDetailEntryCarrierName"), String)
        End Get
        Set
            Me("AccDetailEntryCarrierName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccDetailEntryCarrierNumber() As String
        Get
            Return CType(Me("AccDetailEntryCarrierNumber"), String)
        End Get
        Set
            Me("AccDetailEntryCarrierNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property AccCheckEntryCarrierControl() As Integer
        Get
            Return CType(Me("AccCheckEntryCarrierControl"), Integer)
        End Get
        Set
            Me("AccCheckEntryCarrierControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadDetailDaysBack() As String
        Get
            Return CType(Me("LoadDetailDaysBack"), String)
        End Get
        Set
            Me("LoadDetailDaysBack") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property LoadDetailFilterOnComp() As Integer
        Get
            Return CType(Me("LoadDetailFilterOnComp"), Integer)
        End Get
        Set
            Me("LoadDetailFilterOnComp") = Value
        End Set
    End Property

    <Global.System.Configuration.ApplicationScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("<?xml version=""1.0"" encoding=""utf-16""?>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<ArrayOfString xmlns:xsi=""http://www.w3." &
        "org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <s" &
        "tring>en-US</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>es</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>fr</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>de" &
        "</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "</ArrayOfString>")>
    Public ReadOnly Property LanguageCollection() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("LanguageCollection"), Global.System.Collections.Specialized.StringCollection)
        End Get
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadDetailFilterOnCompName() As String
        Get
            Return CType(Me("LoadDetailFilterOnCompName"), String)
        End Get
        Set
            Me("LoadDetailFilterOnCompName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ConNumberSearch() As String
        Get
            Return CType(Me("ConNumberSearch"), String)
        End Get
        Set
            Me("ConNumberSearch") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadConsSummaryGridTheme() As String
        Get
            Return CType(Me("LoadConsSummaryGridTheme"), String)
        End Get
        Set
            Me("LoadConsSummaryGridTheme") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryLaneNumberFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryLaneNumberFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryLaneNumberFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryTransTypeFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryTransTypeFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryTransTypeFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryStartDateFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryStartDateFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryStartDateFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryStopDateFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryStopDateFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryStopDateFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigStartZipFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigStartZipFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigStartZipFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigStopZipFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigStopZipFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigStopZipFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestStartZipFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestStartZipFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestStartZipFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestStopZipFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestStopZipFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestStopZipFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryCompNumberFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryCompNumberFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryCompNumberFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigCityFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigCityFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigCityFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestCityFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestCityFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestCityFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigSt1Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigSt1Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigSt1Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigSt2Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigSt2Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigSt2Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigSt3Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigSt3Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigSt3Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryOrigSt4Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryOrigSt4Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryOrigSt4Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestSt1Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestSt1Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestSt1Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestSt2Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestSt2Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestSt2Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestSt3Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestSt3Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestSt3Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDestSt4Filter() As String
        Get
            Return CType(Me("LoadBoardSummaryDestSt4Filter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDestSt4Filter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryDefCustFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryDefCustFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryDefCustFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryUseLoadDateFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryUseLoadDateFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryUseLoadDateFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadBoardSummaryCompNameFilter() As String
        Get
            Return CType(Me("LoadBoardSummaryCompNameFilter"), String)
        End Get
        Set
            Me("LoadBoardSummaryCompNameFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APFRTBillMassEntryDaysBack() As String
        Get
            Return CType(Me("APFRTBillMassEntryDaysBack"), String)
        End Get
        Set
            Me("APFRTBillMassEntryDaysBack") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APFrtBillAuditApprovCarrierName() As String
        Get
            Return CType(Me("APFrtBillAuditApprovCarrierName"), String)
        End Get
        Set
            Me("APFrtBillAuditApprovCarrierName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APFrtBillAuditApprovCarrierNumber() As String
        Get
            Return CType(Me("APFrtBillAuditApprovCarrierNumber"), String)
        End Get
        Set
            Me("APFrtBillAuditApprovCarrierNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APFrtBillAuditApprovDaysBackFilter() As String
        Get
            Return CType(Me("APFrtBillAuditApprovDaysBackFilter"), String)
        End Get
        Set
            Me("APFrtBillAuditApprovDaysBackFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property APFrtBillAuditApprovCarrierControl() As Integer
        Get
            Return CType(Me("APFrtBillAuditApprovCarrierControl"), Integer)
        End Get
        Set
            Me("APFrtBillAuditApprovCarrierControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadDetailSearchString() As String
        Get
            Return CType(Me("LoadDetailSearchString"), String)
        End Get
        Set
            Me("LoadDetailSearchString") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadDetailIsWildCard() As Boolean
        Get
            Return CType(Me("LoadDetailIsWildCard"), Boolean)
        End Get
        Set
            Me("LoadDetailIsWildCard") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property LoadDetailBookLookUpNumbers() As Integer
        Get
            Return CType(Me("LoadDetailBookLookUpNumbers"), Integer)
        End Get
        Set
            Me("LoadDetailBookLookUpNumbers") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property OrderPreviewTabLastPaneNameUsed() As String
        Get
            Return CType(Me("OrderPreviewTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("OrderPreviewTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CompanyTabLastPaneNameUsed() As String
        Get
            Return CType(Me("CompanyTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("CompanyTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CarrierTabLastPaneNameUsed() As String
        Get
            Return CType(Me("CarrierTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("CarrierTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LaneTabLastPaneNameUsed() As String
        Get
            Return CType(Me("LaneTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("LaneTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadTabLastPaneNameUsed() As String
        Get
            Return CType(Me("LoadTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("LoadTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AuditingTabLastPaneNameUsed() As String
        Get
            Return CType(Me("AuditingTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("AuditingTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccountingTabLastPaneNameUsed() As String
        Get
            Return CType(Me("AccountingTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("AccountingTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property DashboardURL() As String
        Get
            Return CType(Me("DashboardURL"), String)
        End Get
        Set
            Me("DashboardURL") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property dpLoadBoardSummaryHideHeader() As Boolean
        Get
            Return CType(Me("dpLoadBoardSummaryHideHeader"), Boolean)
        End Get
        Set
            Me("dpLoadBoardSummaryHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("C:\PROGRAM FILES\NGL\FM BFC TARIFF Engine\EDITBFC.EXE")>
    Public Property BFCTariffEXEPath() As String
        Get
            Return CType(Me("BFCTariffEXEPath"), String)
        End Get
        Set
            Me("BFCTariffEXEPath") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property APFrtBillAuditApprovNormal() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovNormal"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovNormal") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property APFrtBillAuditApprovshowApproved() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovshowApproved"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovshowApproved") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property APFrtBillAuditApprovshowElectionic() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovshowElectionic"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovshowElectionic") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property APFrtBillAuditApprovshowMatched() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovshowMatched"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovshowMatched") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property APFrtBillAuditApprovshowAllErrors() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovshowAllErrors"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovshowAllErrors") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccDetailEntryPayCode() As String
        Get
            Return CType(Me("AccDetailEntryPayCode"), String)
        End Get
        Set
            Me("AccDetailEntryPayCode") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property UpgradeRequired() As Boolean
        Get
            Return CType(Me("UpgradeRequired"), Boolean)
        End Get
        Set
            Me("UpgradeRequired") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property AccountsPaidCarrierControl() As Integer
        Get
            Return CType(Me("AccountsPaidCarrierControl"), Integer)
        End Get
        Set
            Me("AccountsPaidCarrierControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccountsPaidCarrierNumber() As String
        Get
            Return CType(Me("AccountsPaidCarrierNumber"), String)
        End Get
        Set
            Me("AccountsPaidCarrierNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property AccountsPaidCarrierName() As String
        Get
            Return CType(Me("AccountsPaidCarrierName"), String)
        End Get
        Set
            Me("AccountsPaidCarrierName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property APCommDetailCommControl() As Integer
        Get
            Return CType(Me("APCommDetailCommControl"), Integer)
        End Get
        Set
            Me("APCommDetailCommControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APCommDetailCommNumber() As String
        Get
            Return CType(Me("APCommDetailCommNumber"), String)
        End Get
        Set
            Me("APCommDetailCommNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property APCommDetailCommName() As String
        Get
            Return CType(Me("APCommDetailCommName"), String)
        End Get
        Set
            Me("APCommDetailCommName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property ARMassEntryCompControl() As Integer
        Get
            Return CType(Me("ARMassEntryCompControl"), Integer)
        End Get
        Set
            Me("ARMassEntryCompControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ARMassEntryCompNumber() As String
        Get
            Return CType(Me("ARMassEntryCompNumber"), String)
        End Get
        Set
            Me("ARMassEntryCompNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ARMassEntryCompName() As String
        Get
            Return CType(Me("ARMassEntryCompName"), String)
        End Get
        Set
            Me("ARMassEntryCompName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property HelpTabLastPaneNameUsed() As String
        Get
            Return CType(Me("HelpTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("HelpTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ClaimTabLastPaneNameUsed() As String
        Get
            Return CType(Me("ClaimTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("ClaimTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property SystemTabLastPaneNameUsed() As String
        Get
            Return CType(Me("SystemTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("SystemTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadConsSummarysHideHeader() As Boolean
        Get
            Return CType(Me("LoadConsSummarysHideHeader"), Boolean)
        End Get
        Set
            Me("LoadConsSummarysHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property APFrtBillAuditApprovalHideHeader() As Boolean
        Get
            Return CType(Me("APFrtBillAuditApprovalHideHeader"), Boolean)
        End Get
        Set
            Me("APFrtBillAuditApprovalHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property ClaimControl() As Integer
        Get
            Return CType(Me("ClaimControl"), Integer)
        End Get
        Set
            Me("ClaimControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ClaimPro() As String
        Get
            Return CType(Me("ClaimPro"), String)
        End Get
        Set
            Me("ClaimPro") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ClaimFB() As String
        Get
            Return CType(Me("ClaimFB"), String)
        End Get
        Set
            Me("ClaimFB") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property ClaimCompControl() As Integer
        Get
            Return CType(Me("ClaimCompControl"), Integer)
        End Get
        Set
            Me("ClaimCompControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property ClaimCarrierControl() As Integer
        Get
            Return CType(Me("ClaimCarrierControl"), Integer)
        End Get
        Set
            Me("ClaimCarrierControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CheckCallsConSearch() As String
        Get
            Return CType(Me("CheckCallsConSearch"), String)
        End Get
        Set
            Me("CheckCallsConSearch") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AvailableServers() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("AvailableServers"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("AvailableServers") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AvailableDatabases() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("AvailableDatabases"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("AvailableDatabases") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AvailableWCFURLs() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("AvailableWCFURLs"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("AvailableWCFURLs") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AvailableAuthCodes() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("AvailableAuthCodes"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("AvailableAuthCodes") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property CarrAdHocControl() As Integer
        Get
            Return CType(Me("CarrAdHocControl"), Integer)
        End Get
        Set
            Me("CarrAdHocControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property CarrAdHocNumber() As Integer
        Get
            Return CType(Me("CarrAdHocNumber"), Integer)
        End Get
        Set
            Me("CarrAdHocNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property CarrAdHocName() As String
        Get
            Return CType(Me("CarrAdHocName"), String)
        End Get
        Set
            Me("CarrAdHocName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property MPConsolidationSearch() As String
        Get
            Return CType(Me("MPConsolidationSearch"), String)
        End Get
        Set
            Me("MPConsolidationSearch") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property dpLoadStatusBoardHideHeader() As Boolean
        Get
            Return CType(Me("dpLoadStatusBoardHideHeader"), Boolean)
        End Get
        Set
            Me("dpLoadStatusBoardHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property RecentReports() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("RecentReports"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("RecentReports") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property TileList() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("TileList"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("TileList") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property HotLinksList() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("HotLinksList"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("HotLinksList") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property FrtBillImportFilePath() As String
        Get
            Return CType(Me("FrtBillImportFilePath"), String)
        End Get
        Set
            Me("FrtBillImportFilePath") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property FrtBIllImportProcessAll() As Boolean
        Get
            Return CType(Me("FrtBIllImportProcessAll"), Boolean)
        End Get
        Set
            Me("FrtBIllImportProcessAll") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadBSummAutoHideSearchExpander() As Boolean
        Get
            Return CType(Me("LoadBSummAutoHideSearchExpander"), Boolean)
        End Get
        Set
            Me("LoadBSummAutoHideSearchExpander") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadMPConsHideHeader() As Boolean
        Get
            Return CType(Me("LoadMPConsHideHeader"), Boolean)
        End Get
        Set
            Me("LoadMPConsHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadMPConsHideInfoBar() As Boolean
        Get
            Return CType(Me("LoadMPConsHideInfoBar"), Boolean)
        End Get
        Set
            Me("LoadMPConsHideInfoBar") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LoadStatBrdAutoHideSearchExpander() As Boolean
        Get
            Return CType(Me("LoadStatBrdAutoHideSearchExpander"), Boolean)
        End Get
        Set
            Me("LoadStatBrdAutoHideSearchExpander") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("<?xml version=""1.0"" encoding=""utf-16""?>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<ArrayOfString xmlns:xsi=""http://www.w3." &
        "org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <s" &
        "tring>3</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "</ArrayOfString>")>
    Public Property LoadStatusBoardLaneTransExclusions() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("LoadStatusBoardLaneTransExclusions"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("LoadStatusBoardLaneTransExclusions") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("1000")>
    Public Property OPPageSize() As Integer
        Get
            Return CType(Me("OPPageSize"), Integer)
        End Get
        Set
            Me("OPPageSize") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("<?xml version=""1.0"" encoding=""utf-16""?>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<ArrayOfString xmlns:xsi=""http://www.w3." &
        "org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <s" &
        "tring>10</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>20</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>50</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>75</s" &
        "tring>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>100</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>200</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  <string>500</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
        "  <string>1000</string>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "</ArrayOfString>")>
    Public Property OPPageSizeList() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("OPPageSizeList"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("OPPageSizeList") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property RunReportsLocally() As Boolean
        Get
            Return CType(Me("RunReportsLocally"), Boolean)
        End Get
        Set
            Me("RunReportsLocally") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property dpCarrierTariffsHideHeader() As Boolean
        Get
            Return CType(Me("dpCarrierTariffsHideHeader"), Boolean)
        End Get
        Set
            Me("dpCarrierTariffsHideHeader") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property TariffTabLastPaneNameUsed() As String
        Get
            Return CType(Me("TariffTabLastPaneNameUsed"), String)
        End Get
        Set
            Me("TariffTabLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("30")>
    Public Property MaxClipboardItems() As Integer
        Get
            Return CType(Me("MaxClipboardItems"), Integer)
        End Get
        Set
            Me("MaxClipboardItems") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property FMWindowHeight() As Double
        Get
            Return CType(Me("FMWindowHeight"), Double)
        End Get
        Set
            Me("FMWindowHeight") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property FMWindowWidth() As Double
        Get
            Return CType(Me("FMWindowWidth"), Double)
        End Get
        Set
            Me("FMWindowWidth") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property SaveFMWindowSize() As Boolean
        Get
            Return CType(Me("SaveFMWindowSize"), Boolean)
        End Get
        Set
            Me("SaveFMWindowSize") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property BadAddressBatchID() As Double
        Get
            Return CType(Me("BadAddressBatchID"), Double)
        End Get
        Set
            Me("BadAddressBatchID") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("50")>
    Public Property BadAddressPageSize() As Integer
        Get
            Return CType(Me("BadAddressPageSize"), Integer)
        End Get
        Set
            Me("BadAddressPageSize") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("MAXIMIZED")>
    Public Property WindowState() As String
        Get
            Return CType(Me("WindowState"), String)
        End Get
        Set
            Me("WindowState") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LoadPlanningLastPaneNameUsed() As String
        Get
            Return CType(Me("LoadPlanningLastPaneNameUsed"), String)
        End Get
        Set
            Me("LoadPlanningLastPaneNameUsed") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property NewLoadDetailTabFromFind() As Boolean
        Get
            Return CType(Me("NewLoadDetailTabFromFind"), Boolean)
        End Get
        Set
            Me("NewLoadDetailTabFromFind") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenCompanyAsNewTab() As Boolean
        Get
            Return CType(Me("OpenCompanyAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenCompanyAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenCarrierAsNewTab() As Boolean
        Get
            Return CType(Me("OpenCarrierAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenCarrierAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenLaneAsNewTab() As Boolean
        Get
            Return CType(Me("OpenLaneAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenLaneAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenClaimAsNewTab() As Boolean
        Get
            Return CType(Me("OpenClaimAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenClaimAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property ShowNewTabFeatureInFindPane() As Boolean
        Get
            Return CType(Me("ShowNewTabFeatureInFindPane"), Boolean)
        End Get
        Set
            Me("ShowNewTabFeatureInFindPane") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property UseCustomScrollBar() As Boolean
        Get
            Return CType(Me("UseCustomScrollBar"), Boolean)
        End Get
        Set
            Me("UseCustomScrollBar") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("#FFF5F5F5")>
    Public Property CustomScrollBarColor() As Global.System.Windows.Media.Color
        Get
            Return CType(Me("CustomScrollBarColor"), Global.System.Windows.Media.Color)
        End Get
        Set
            Me("CustomScrollBarColor") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("#FFF5F5F5")>
    Public Property CustomScrollBarThumbsColor() As Global.System.Windows.Media.Color
        Get
            Return CType(Me("CustomScrollBarThumbsColor"), Global.System.Windows.Media.Color)
        End Get
        Set
            Me("CustomScrollBarThumbsColor") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("8")>
    Public Property CustomScrollBarCornerRadius() As Integer
        Get
            Return CType(Me("CustomScrollBarCornerRadius"), Integer)
        End Get
        Set
            Me("CustomScrollBarCornerRadius") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("C:\Ed\ed.exe")>
    Public Property ExportDocEXEPath() As String
        Get
            Return CType(Me("ExportDocEXEPath"), String)
        End Get
        Set
            Me("ExportDocEXEPath") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property RoutingGuideName() As String
        Get
            Return CType(Me("RoutingGuideName"), String)
        End Get
        Set
            Me("RoutingGuideName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property RoutingGuideNumber() As String
        Get
            Return CType(Me("RoutingGuideNumber"), String)
        End Get
        Set
            Me("RoutingGuideNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property RoutingGuideControl() As Integer
        Get
            Return CType(Me("RoutingGuideControl"), Integer)
        End Get
        Set
            Me("RoutingGuideControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenRoutingGuideAsNewTab() As Boolean
        Get
            Return CType(Me("OpenRoutingGuideAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenRoutingGuideAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property IsMassDeployment() As Boolean
        Get
            Return CType(Me("IsMassDeployment"), Boolean)
        End Get
        Set
            Me("IsMassDeployment") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("1000")>
    Public Property LPPageSize() As String
        Get
            Return CType(Me("LPPageSize"), String)
        End Get
        Set
            Me("LPPageSize") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPFilterType() As String
        Get
            Return CType(Me("LPFilterType"), String)
        End Get
        Set
            Me("LPFilterType") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPCompanyNumber() As String
        Get
            Return CType(Me("LPCompanyNumber"), String)
        End Get
        Set
            Me("LPCompanyNumber") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPCompanyName() As String
        Get
            Return CType(Me("LPCompanyName"), String)
        End Get
        Set
            Me("LPCompanyName") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("000000000")>
    Public Property LPOrigStartZip() As String
        Get
            Return CType(Me("LPOrigStartZip"), String)
        End Get
        Set
            Me("LPOrigStartZip") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("999999999")>
    Public Property LPOrigStopZip() As String
        Get
            Return CType(Me("LPOrigStopZip"), String)
        End Get
        Set
            Me("LPOrigStopZip") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("000000000")>
    Public Property LPDestStartZip() As String
        Get
            Return CType(Me("LPDestStartZip"), String)
        End Get
        Set
            Me("LPDestStartZip") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("999999999")>
    Public Property LPDestStopZip() As String
        Get
            Return CType(Me("LPDestStopZip"), String)
        End Get
        Set
            Me("LPDestStopZip") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPLaneNumberFilter() As String
        Get
            Return CType(Me("LPLaneNumberFilter"), String)
        End Get
        Set
            Me("LPLaneNumberFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPTransTypeFilter() As String
        Get
            Return CType(Me("LPTransTypeFilter"), String)
        End Get
        Set
            Me("LPTransTypeFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPStartDate() As String
        Get
            Return CType(Me("LPStartDate"), String)
        End Get
        Set
            Me("LPStartDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPStopDate() As String
        Get
            Return CType(Me("LPStopDate"), String)
        End Get
        Set
            Me("LPStopDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property LPOrigStateList() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("LPOrigStateList"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("LPOrigStateList") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property LPDestStateList() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("LPDestStateList"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("LPDestStateList") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPOrigCity() As String
        Get
            Return CType(Me("LPOrigCity"), String)
        End Get
        Set
            Me("LPOrigCity") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property LPDestCity() As String
        Get
            Return CType(Me("LPDestCity"), String)
        End Get
        Set
            Me("LPDestCity") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property LPUseLoadDate() As Boolean
        Get
            Return CType(Me("LPUseLoadDate"), Boolean)
        End Get
        Set
            Me("LPUseLoadDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property LPCarrierControlA() As Integer
        Get
            Return CType(Me("LPCarrierControlA"), Integer)
        End Get
        Set
            Me("LPCarrierControlA") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenConsolidationSumAsNewTab() As Boolean
        Get
            Return CType(Me("OpenConsolidationSumAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenConsolidationSumAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property OpenLoadPlanningAsNewTab() As Boolean
        Get
            Return CType(Me("OpenLoadPlanningAsNewTab"), Boolean)
        End Get
        Set
            Me("OpenLoadPlanningAsNewTab") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LPSummariesVisible() As Boolean
        Get
            Return CType(Me("LPSummariesVisible"), Boolean)
        End Get
        Set
            Me("LPSummariesVisible") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property LPDetailsVisible() As Boolean
        Get
            Return CType(Me("LPDetailsVisible"), Boolean)
        End Get
        Set
            Me("LPDetailsVisible") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property LPDetailsIsHorizontalView() As Boolean
        Get
            Return CType(Me("LPDetailsIsHorizontalView"), Boolean)
        End Get
        Set
            Me("LPDetailsIsHorizontalView") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property TileListFindPane() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("TileListFindPane"), Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("TileListFindPane") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property ShowDesktopAlerts() As Boolean
        Get
            Return CType(Me("ShowDesktopAlerts"), Boolean)
        End Get
        Set
            Me("ShowDesktopAlerts") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property ShowNotificationsPane() As Boolean
        Get
            Return CType(Me("ShowNotificationsPane"), Boolean)
        End Get
        Set
            Me("ShowNotificationsPane") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property ShowChatWindow() As Boolean
        Get
            Return CType(Me("ShowChatWindow"), Boolean)
        End Get
        Set
            Me("ShowChatWindow") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property ShowDashboardPane() As Boolean
        Get
            Return CType(Me("ShowDashboardPane"), Boolean)
        End Get
        Set
            Me("ShowDashboardPane") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property HideAAFilter() As Boolean
        Get
            Return CType(Me("HideAAFilter"), Boolean)
        End Get
        Set
            Me("HideAAFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("net.tcp://nglwcfdev704.nextgeneration.com:908")>
    Public Property TCPURL() As String
        Get
            Return CType(Me("TCPURL"), String)
        End Get
        Set
            Me("TCPURL") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("C:\Windows\Media\ding.wav")>
    Public Property SoundMediaFolder() As String
        Get
            Return CType(Me("SoundMediaFolder"), String)
        End Get
        Set
            Me("SoundMediaFolder") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("50")>
    Public Property CompCreditPageSize() As Integer
        Get
            Return CType(Me("CompCreditPageSize"), Integer)
        End Get
        Set
            Me("CompCreditPageSize") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property OrderEntryPrevDateLoad() As Date
        Get
            Return CType(Me("OrderEntryPrevDateLoad"), Date)
        End Get
        Set
            Me("OrderEntryPrevDateLoad") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OrderEntryCompControl() As Integer
        Get
            Return CType(Me("OrderEntryCompControl"), Integer)
        End Get
        Set
            Me("OrderEntryCompControl") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OrderEntryModeType() As Integer
        Get
            Return CType(Me("OrderEntryModeType"), Integer)
        End Get
        Set
            Me("OrderEntryModeType") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property ZebraPrinterIP() As String
        Get
            Return CType(Me("ZebraPrinterIP"), String)
        End Get
        Set
            Me("ZebraPrinterIP") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("9100")>
    Public Property ZebraPrinterPort() As String
        Get
            Return CType(Me("ZebraPrinterPort"), String)
        End Get
        Set
            Me("ZebraPrinterPort") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property OrderEntryTemp() As Integer
        Get
            Return CType(Me("OrderEntryTemp"), Integer)
        End Get
        Set
            Me("OrderEntryTemp") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("True")>
    Public Property ZebraLargeFont() As Boolean
        Get
            Return CType(Me("ZebraLargeFont"), Boolean)
        End Get
        Set
            Me("ZebraLargeFont") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("4")>
    Public Property SpotRateAllocFormual() As Integer
        Get
            Return CType(Me("SpotRateAllocFormual"), Integer)
        End Get
        Set
            Me("SpotRateAllocFormual") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("1")>
    Public Property SpotRateReason() As Integer
        Get
            Return CType(Me("SpotRateReason"), Integer)
        End Get
        Set
            Me("SpotRateReason") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("0")>
    Public Property SpotRateBFCAlloc() As Integer
        Get
            Return CType(Me("SpotRateBFCAlloc"), Integer)
        End Get
        Set
            Me("SpotRateBFCAlloc") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("10")>
    Public Property DesktopAlertFadeOutSeconds() As Integer
        Get
            Return CType(Me("DesktopAlertFadeOutSeconds"), Integer)
        End Get
        Set
            Me("DesktopAlertFadeOutSeconds") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property ZebraLargeFontBilling() As Boolean
        Get
            Return CType(Me("ZebraLargeFontBilling"), Boolean)
        End Get
        Set
            Me("ZebraLargeFontBilling") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LPRecalcOnDrop() As Boolean
        Get
            Return CType(Me("LPRecalcOnDrop"), Boolean)
        End Get
        Set
            Me("LPRecalcOnDrop") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property LPGetMilesOnDrop() As Boolean
        Get
            Return CType(Me("LPGetMilesOnDrop"), Boolean)
        End Get
        Set
            Me("LPGetMilesOnDrop") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AccCheckEntryDateStart() As Date
        Get
            Return CType(Me("AccCheckEntryDateStart"), Date)
        End Get
        Set
            Me("AccCheckEntryDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property AccCheckEntryDateEnd() As Date
        Get
            Return CType(Me("AccCheckEntryDateEnd"), Date)
        End Get
        Set
            Me("AccCheckEntryDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI210MaintDateStart() As Date
        Get
            Return CType(Me("EDI210MaintDateStart"), Date)
        End Get
        Set
            Me("EDI210MaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI210MaintDateEnd() As Date
        Get
            Return CType(Me("EDI210MaintDateEnd"), Date)
        End Get
        Set
            Me("EDI210MaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI820MaintDateStart() As Date
        Get
            Return CType(Me("EDI820MaintDateStart"), Date)
        End Get
        Set
            Me("EDI820MaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI820MaintDateEnd() As Date
        Get
            Return CType(Me("EDI820MaintDateEnd"), Date)
        End Get
        Set
            Me("EDI820MaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property EDI210MaintProFilter() As String
        Get
            Return CType(Me("EDI210MaintProFilter"), String)
        End Get
        Set
            Me("EDI210MaintProFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("")>
    Public Property EDI820MaintProFilter() As String
        Get
            Return CType(Me("EDI820MaintProFilter"), String)
        End Get
        Set
            Me("EDI820MaintProFilter") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI204MaintDateStart() As Date
        Get
            Return CType(Me("EDI204MaintDateStart"), Date)
        End Get
        Set
            Me("EDI204MaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI204MaintDateEnd() As Date
        Get
            Return CType(Me("EDI204MaintDateEnd"), Date)
        End Get
        Set
            Me("EDI204MaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI990MaintDateStart() As Date
        Get
            Return CType(Me("EDI990MaintDateStart"), Date)
        End Get
        Set
            Me("EDI990MaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI990MaintDateEnd() As Date
        Get
            Return CType(Me("EDI990MaintDateEnd"), Date)
        End Get
        Set
            Me("EDI990MaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI210InMaintDateStart() As Date
        Get
            Return CType(Me("EDI210InMaintDateStart"), Date)
        End Get
        Set
            Me("EDI210InMaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI210InMaintDateEnd() As Date
        Get
            Return CType(Me("EDI210InMaintDateEnd"), Date)
        End Get
        Set
            Me("EDI210InMaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("False")>
    Public Property EDI210InMaintblnUseInvoiceDate() As Boolean
        Get
            Return CType(Me("EDI210InMaintblnUseInvoiceDate"), Boolean)
        End Get
        Set
            Me("EDI210InMaintblnUseInvoiceDate") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI214MaintDateStart() As Date
        Get
            Return CType(Me("EDI214MaintDateStart"), Date)
        End Get
        Set
            Me("EDI214MaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property EDI214MaintDateEnd() As Date
        Get
            Return CType(Me("EDI214MaintDateEnd"), Date)
        End Get
        Set
            Me("EDI214MaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property DATMaintDateStart() As Date
        Get
            Return CType(Me("DATMaintDateStart"), Date)
        End Get
        Set
            Me("DATMaintDateStart") = Value
        End Set
    End Property

    <Global.System.Configuration.UserScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>
    Public Property DATMaintDateEnd() As Date
        Get
            Return CType(Me("DATMaintDateEnd"), Date)
        End Get
        Set
            Me("DATMaintDateEnd") = Value
        End Set
    End Property

    <Global.System.Configuration.ApplicationScopedSettingAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Configuration.DefaultSettingValueAttribute("03/02/2017")>
    Public ReadOnly Property VersionDate() As String
        Get
            Return CType(Me("VersionDate"), String)
        End Get
    End Property
End Class

Namespace My

    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>
    Friend Module MySettingsProperty

        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>
        Friend ReadOnly Property Settings() As Global.FreightMaster_TMS.MySettings
            Get
                Return Global.FreightMaster_TMS.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
