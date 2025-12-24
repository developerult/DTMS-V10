Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar
Imports NGL.FreightMaster.Data
Imports Microsoft.VisualBasic.Logging
Imports Newtonsoft.Json
Imports System.Runtime.Serialization
Imports Serilog
Imports Serilog.Context

Public MustInherit Class BLLBaseClass


#Region " Constructors "
    Public Sub New()
        MyBase.New()
        Logger = Logger.ForContext(Of BLLBaseClass)


    End Sub

#End Region

#Region " Properties"

    Private _SourceClass As String = "BLLBaseClass"
    Public Logger As Serilog.ILogger = Serilog.Log.Logger

    Public Overridable Property SourceClass() As String
        Get
            Return _SourceClass
        End Get
        Set(ByVal value As String)
            _SourceClass = value
        End Set
    End Property

    Private _Parameters As DAL.WCFParameters
    Public Property Parameters() As DAL.WCFParameters
        Get
            Return _Parameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _Parameters = value
            If (Not _Parameters Is Nothing) Then
                LogContext.PushProperty("NGLUsername", _Parameters.UserName)
                LogContext.PushProperty("NGLUSATUserID", _Parameters.USATUserID)
            End If
        End Set
    End Property

    Private _ConnectionString As String = ""  ' is not available via class libraries My.Settings.NGLMAS 'the default value uses the configuration setting NGLMAS
    Public Property ConnectionString() As String
        Get
            If Len(Trim(_ConnectionString)) < 5 Then
                If Len(Trim(_Parameters.ConnectionString)) < 5 Then
                    _ConnectionString = String.Format("Server={0}; Database={1}; Integrated Security=SSPI;TrustServerCertificate=true;", _Parameters.DBServer.Trim, _Parameters.Database.Trim)
                Else
                    _ConnectionString = _Parameters.ConnectionString
                End If
            End If
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _ClassRef As New StoredReferences
    ''' <summary>
    ''' Stores a list of object References used by the Base Class Factories
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClassRef() As StoredReferences
        Get
            If _ClassRef Is Nothing Then _ClassRef = New StoredReferences
            Return _ClassRef
        End Get
        Set(ByVal value As StoredReferences)
            _ClassRef = value
        End Set
    End Property


#End Region

#Region " Enums"

    Public Enum sysErrorSeverity As Integer
        Information = 0
        Warning
        Unexpected
        Critical
    End Enum

    Public Enum sysErrorState As Integer
        UserLevelFault = 0
        ServerLevelFault
        SystemLevelFault
    End Enum

#End Region

#Region " BLL Data Object Properties"

    Private _BookBLL As NGLBookBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property BookBLL() As NGLBookBLL
        Get
            'If _BookBLL Is Nothing Then
            _BookBLL = New NGLBookBLL(Parameters)

            'End If
            Return _BookBLL
        End Get
        Set(value As NGLBookBLL)
            _BookBLL = value
        End Set
    End Property

    Private _BookRevenueBLL As NGLBookRevenueBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property BookRevenueBLL() As NGLBookRevenueBLL
        Get
            'If _BookRevenueBLL Is Nothing Then
            _BookRevenueBLL = New NGLBookRevenueBLL(Parameters)

            'End If
            Return _BookRevenueBLL
        End Get
        Set(value As NGLBookRevenueBLL)
            _BookRevenueBLL = value
        End Set
    End Property

    Private _BookFeesBLL As NGLBookFeesBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property BookFeesBLL() As NGLBookFeesBLL
        Get
           ' If _BookFeesBLL Is Nothing Then
                _BookFeesBLL = New NGLBookFeesBLL(Parameters)

            'End If
            Return _BookFeesBLL
        End Get
        Set(value As NGLBookFeesBLL)
            _BookFeesBLL = value
        End Set
    End Property

    Private _BatchBLL As NGLBatchBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property BatchBLL() As NGLBatchBLL
        Get
            'If _BatchBLL Is Nothing Then
                _BatchBLL = New NGLBatchBLL(Parameters)

            'End If
            Return _BatchBLL
        End Get
        Set(value As NGLBatchBLL)
            _BatchBLL = value
        End Set
    End Property

    Private _OrderImportBLL As NGLOrderImportBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property OrderImportBLL() As NGLOrderImportBLL
        Get
         '  If _OrderImportBLL Is Nothing Then
                _OrderImportBLL = New NGLOrderImportBLL(Parameters)

       '     End If
            Return _OrderImportBLL
        End Get
        Set(value As NGLOrderImportBLL)
            _OrderImportBLL = value
        End Set
    End Property

    Private _LaneTransLoadXrefDetBLL As NGLLaneTransLoadXrefDetBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property LaneTransLoadXrefDetBLL() As NGLLaneTransLoadXrefDetBLL
        Get
            If _LaneTransLoadXrefDetBLL Is Nothing Then
                _LaneTransLoadXrefDetBLL = New NGLLaneTransLoadXrefDetBLL(Parameters)

            End If
            Return _LaneTransLoadXrefDetBLL
        End Get
        Set(value As NGLLaneTransLoadXrefDetBLL)
            _LaneTransLoadXrefDetBLL = value
        End Set
    End Property


    Private _CarrierBLL As NGLCarrierBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property CarrierBLL() As NGLCarrierBLL
        Get
       '     If _CarrierBLL Is Nothing Then
                _CarrierBLL = New NGLCarrierBLL(Parameters)

       '     End If
            Return _CarrierBLL
        End Get
        Set(value As NGLCarrierBLL)
            _CarrierBLL = value
        End Set
    End Property

    Private _PCMilerBLL As NGLPCMilerBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property PCMilerBLL() As NGLPCMilerBLL
        Get
            If _PCMilerBLL Is Nothing Then
                _PCMilerBLL = New NGLPCMilerBLL(Parameters)

            End If
            Return _PCMilerBLL
        End Get
        Set(value As NGLPCMilerBLL)
            _PCMilerBLL = value
        End Set
    End Property

    'Added by LVV 5/23/16 for v-7.0.5.110 DAT
    Private _NGLDATBLL As NGLDATBLL
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLDATBLL() As NGLDATBLL
        Get
            If _NGLDATBLL Is Nothing Then
                _NGLDATBLL = New NGLDATBLL(Parameters)

            End If
            Return _NGLDATBLL
        End Get
        Set(value As NGLDATBLL)
            _NGLDATBLL = value
        End Set
    End Property

#End Region

#Region " TAR Data Object Properties"


    Private _TARBookRev As TAR.BookRev
    <JsonIgnore, IgnoreDataMember>
    Public Property TARBookRev() As TAR.BookRev
        Get
            'If _TARBookRev Is Nothing Then
            _TARBookRev = New TAR.BookRev(Me.Parameters)
            'End If
            Return _TARBookRev
        End Get
        Set(value As TAR.BookRev)
            _TARBookRev = value
        End Set
    End Property

    Private _TARIntegration As TAR.Integration
    <JsonIgnore, IgnoreDataMember>
    Public Property TARIntegration() As TAR.Integration
        Get
            If _TARIntegration Is Nothing Then
                _TARIntegration = New TAR.Integration(Me.Parameters)
            End If
            Return _TARIntegration
        End Get
        Set(value As TAR.Integration)
            _TARIntegration = value
        End Set
    End Property


    Private _CarrTarExportExcel As TAR.CarrTarExportExcel
    <JsonIgnore, IgnoreDataMember>
    Public Property TARExportExcel() As TAR.CarrTarExportExcel
        Get
            If _CarrTarExportExcel Is Nothing Then
                _CarrTarExportExcel = New TAR.CarrTarExportExcel(Me.Parameters)
            End If
            Return _CarrTarExportExcel
        End Get
        Set(value As TAR.CarrTarExportExcel)
            _CarrTarExportExcel = value
        End Set
    End Property

    Private _CarrTarImportExcel As TAR.CarrTarImportExcel
    <JsonIgnore, IgnoreDataMember>
    Public Property TARImportExcel() As TAR.CarrTarImportExcel
        Get
            If _CarrTarImportExcel Is Nothing Then
                _CarrTarImportExcel = New TAR.CarrTarImportExcel(Me.Parameters)
            End If
            Return _CarrTarImportExcel
        End Get
        Set(value As TAR.CarrTarImportExcel)
            _CarrTarImportExcel = value
        End Set
    End Property

    Private _CarrTarImportCSV As TAR.CarrTarImportRatesCSV
    <JsonIgnore, IgnoreDataMember>
    Public Property TARImportRatesCSV() As TAR.CarrTarImportRatesCSV
        Get
            If _CarrTarImportCSV Is Nothing Then
                _CarrTarImportCSV = New TAR.CarrTarImportRatesCSV(Me.Parameters)
            End If
            Return _CarrTarImportCSV
        End Get
        Set(value As TAR.CarrTarImportRatesCSV)
            _CarrTarImportCSV = value
        End Set
    End Property

    Private _CarrTarCopy As TAR.CarrTarCopyContract
    <JsonIgnore, IgnoreDataMember>
    Public Property CarrTarCopy() As TAR.CarrTarCopyContract
        Get
            If _CarrTarCopy Is Nothing Then
                _CarrTarCopy = New TAR.CarrTarCopyContract(Me.Parameters)
                '_CarrTarCopy = DirectCast(TarBaseClassFactory("CarrTarCopyContract"), TAR.CarrTarCopyContract)
            End If
            Return _CarrTarCopy
        End Get
        Set(value As TAR.CarrTarCopyContract)
            _CarrTarCopy = value
        End Set
    End Property

    Private _CarrTarImportCSVInterline As TAR.CarrTarImportInterlinePointsCSV
    <JsonIgnore, IgnoreDataMember>
    Public Property TARImportInterlinePointsCSV() As TAR.CarrTarImportInterlinePointsCSV
        Get
            If _CarrTarImportCSVInterline Is Nothing Then
                _CarrTarImportCSVInterline = New TAR.CarrTarImportInterlinePointsCSV(Me.Parameters)
                '_CarrTarImportCSVInterline = DirectCast(TarBaseClassFactory("CarrTarImportInterlinePointsCSV"), TAR.CarrTarImportInterlinePointsCSV)
            End If
            Return _CarrTarImportCSVInterline
        End Get
        Set(value As TAR.CarrTarImportInterlinePointsCSV)
            _CarrTarImportCSVInterline = value
        End Set
    End Property

    Private _CarrTarImportCSVNonService As TAR.CarrTarImportNonServicePointsCSV
    <JsonIgnore, IgnoreDataMember>
    Public Property TARImportNonServcPointsCSV() As TAR.CarrTarImportNonServicePointsCSV
        Get
            If _CarrTarImportCSVNonService Is Nothing Then
                _CarrTarImportCSVNonService = New TAR.CarrTarImportNonServicePointsCSV(Me.Parameters)
                '_CarrTarImportCSVNonService = DirectCast(TarBaseClassFactory("CarrTarImportNonServicePointsCSV"), TAR.CarrTarImportNonServicePointsCSV)
            End If
            Return _CarrTarImportCSVNonService
        End Get
        Set(value As TAR.CarrTarImportNonServicePointsCSV)
            _CarrTarImportCSVNonService = value
        End Set
    End Property

#End Region

#Region " NGL Data Object Properties"

    Private _NGLSystemData As DAL.NGLSystemDataProvider
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLSystemData As DAL.NGLSystemDataProvider
        Get
            If _NGLSystemData Is Nothing Then
                If Not Parameters Is Nothing Then
                    Dim blnValidateAccess = Parameters.ValidateAccess
                    Parameters.ValidateAccess = False
                    _NGLSystemData = New DAL.NGLSystemDataProvider(Parameters)
                    Parameters.ValidateAccess = blnValidateAccess
                End If
            End If
            Return _NGLSystemData
        End Get
        Set(value As DAL.NGLSystemDataProvider)
            _NGLSystemData = value
        End Set
    End Property

    Private _NGLBookData As DAL.NGLBookData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookData() As DAL.NGLBookData
        Get
            If _NGLBookData Is Nothing Then
                _NGLBookData = New DAL.NGLBookData(Parameters)
                '_NGLBookData = DirectCast(NDPBaseClassFactory("NGLBookData"), DAL.NGLBookData)
            End If
            Return _NGLBookData
        End Get
        Set(value As DAL.NGLBookData)
            _NGLBookData = value
        End Set
    End Property

    Private _NGLvBookConsData As DAL.NGLvBookConsData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLvBookConsData() As DAL.NGLvBookConsData
        Get
            If _NGLvBookConsData Is Nothing Then
                _NGLvBookConsData = New DAL.NGLvBookConsData(Parameters)
                '_NGLvBookConsData = DirectCast(NDPBaseClassFactory("NGLvBookConsData"), DAL.NGLvBookConsData)
            End If
            Return _NGLvBookConsData
        End Get
        Set(value As DAL.NGLvBookConsData)
            _NGLvBookConsData = value
        End Set
    End Property

    Private _NGLvBookMultiPickData As DAL.NGLvBookMultiPickData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLvBookMultiPickData() As DAL.NGLvBookMultiPickData
        Get
            If _NGLvBookMultiPickData Is Nothing Then
                _NGLvBookMultiPickData = New DAL.NGLvBookMultiPickData(Parameters)
                '_NGLvBookMultiPickData = DirectCast(NDPBaseClassFactory("NGLvBookMultiPickData"), DAL.NGLvBookMultiPickData)
            End If
            Return _NGLvBookMultiPickData
        End Get
        Set(value As DAL.NGLvBookMultiPickData)
            _NGLvBookMultiPickData = value
        End Set
    End Property

    Private _NGLBookItemData As DAL.NGLBookItemData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookItemData() As DAL.NGLBookItemData
        Get
            If _NGLBookItemData Is Nothing Then
                _NGLBookItemData = New DAL.NGLBookItemData(Parameters)
                '_NGLBookItemData = DirectCast(NDPBaseClassFactory("NGLBookItemData"), DAL.NGLBookItemData)
            End If
            Return _NGLBookItemData
        End Get
        Set(value As DAL.NGLBookItemData)
            _NGLBookItemData = value
        End Set
    End Property

    Private _NGLBookTrackData As DAL.NGLBookTrackData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookTrackData() As DAL.NGLBookTrackData
        Get
            If _NGLBookTrackData Is Nothing Then
                _NGLBookTrackData = New DAL.NGLBookTrackData(Parameters)
                '_NGLBookTrackData = DirectCast(NDPBaseClassFactory("NGLBookTrackData"), DAL.NGLBookTrackData)
            End If
            Return _NGLBookTrackData
        End Get
        Set(value As DAL.NGLBookTrackData)
            _NGLBookTrackData = value
        End Set
    End Property

    Private _NGLBookNoteData As DAL.NGLBookNoteData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookNoteData() As DAL.NGLBookNoteData
        Get
            If _NGLBookNoteData Is Nothing Then
                _NGLBookNoteData = New DAL.NGLBookNoteData(Parameters)
                '_NGLBookNoteData = DirectCast(NDPBaseClassFactory("NGLBookNoteData"), DAL.NGLBookNoteData)
            End If
            Return _NGLBookNoteData
        End Get
        Set(value As DAL.NGLBookNoteData)
            _NGLBookNoteData = value
        End Set
    End Property

    Private _NGLBookLoadData As DAL.NGLBookLoadData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookLoadData() As DAL.NGLBookLoadData
        Get
            If _NGLBookLoadData Is Nothing Then
                _NGLBookLoadData = New DAL.NGLBookLoadData(Parameters)
                '_NGLBookLoadData = DirectCast(NDPBaseClassFactory("NGLBookLoadData"), DAL.NGLBookLoadData)
            End If
            Return _NGLBookLoadData
        End Get
        Set(value As DAL.NGLBookLoadData)
            _NGLBookLoadData = value
        End Set
    End Property

    Private _NGLBookRevenueData As DAL.NGLBookRevenueData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookRevenueData() As DAL.NGLBookRevenueData
        Get
            If _NGLBookRevenueData Is Nothing Then
                _NGLBookRevenueData = New DAL.NGLBookRevenueData(Parameters)
            End If
            Return _NGLBookRevenueData
        End Get
        Set(value As DAL.NGLBookRevenueData)
            _NGLBookRevenueData = value
        End Set
    End Property

    Private _NGLSecurityData As DAL.NGLSecurityDataProvider
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLSecurityData() As DAL.NGLSecurityDataProvider
        Get
            If _NGLSecurityData Is Nothing Then
                _NGLSecurityData = New DAL.NGLSecurityDataProvider(Me.Parameters)
            End If
            Return _NGLSecurityData
        End Get
        Set(value As DAL.NGLSecurityDataProvider)
            _NGLSecurityData = value
        End Set
    End Property

    Private _NGLLoadPlanningTruckData As DAL.NGLLoadPlanningTruckData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLoadPlanningTruckData() As DAL.NGLLoadPlanningTruckData
        Get
            If _NGLLoadPlanningTruckData Is Nothing Then
                _NGLLoadPlanningTruckData = New DAL.NGLLoadPlanningTruckData(Parameters)
            End If
            Return _NGLLoadPlanningTruckData
        End Get
        Set(value As DAL.NGLLoadPlanningTruckData)
            _NGLLoadPlanningTruckData = value
        End Set
    End Property

    Private _NGLFlatFileIntegrationData As DAL.NGLFlatFileImport
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLFlatFileImport() As DAL.NGLFlatFileImport
        Get
            If _NGLFlatFileIntegrationData Is Nothing Then
                _NGLFlatFileIntegrationData = New DAL.NGLFlatFileImport(Parameters)
            End If
            Return _NGLFlatFileIntegrationData
        End Get
        Set(ByVal value As DAL.NGLFlatFileImport)
            _NGLFlatFileIntegrationData = value
        End Set
    End Property

    Private _NGLNewBookingsForSolutionData As DAL.NGLNewBookingsForSolutionData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLNewBookingsForSolutionData() As DAL.NGLNewBookingsForSolutionData
        Get
            If _NGLNewBookingsForSolutionData Is Nothing Then
                _NGLNewBookingsForSolutionData = New DAL.NGLNewBookingsForSolutionData(Parameters)
                '_NGLNewBookingsForSolutionData = DirectCast(NDPBaseClassFactory("NGLNewBookingsForSolutionData"), DAL.NGLNewBookingsForSolutionData)
            End If
            Return _NGLNewBookingsForSolutionData
        End Get
        Set(value As DAL.NGLNewBookingsForSolutionData)
            _NGLNewBookingsForSolutionData = value
        End Set
    End Property

    Private _NGLCarrTarEquipMatData As DAL.NGLCarrTarEquipMatData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrTarEquipMatData() As DAL.NGLCarrTarEquipMatData
        Get
            If _NGLCarrTarEquipMatData Is Nothing Then
                _NGLCarrTarEquipMatData = New DAL.NGLCarrTarEquipMatData(Parameters)
                '_NGLCarrTarEquipMatData = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipMatData"), DAL.NGLCarrTarEquipMatData)
            End If
            Return _NGLCarrTarEquipMatData
        End Get
        Set(value As DAL.NGLCarrTarEquipMatData)
            _NGLCarrTarEquipMatData = value
        End Set
    End Property

    Private _NGLBookCarrierData As DAL.NGLBookCarrierData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookCarrierData() As DAL.NGLBookCarrierData
        Get
            If _NGLBookCarrierData Is Nothing Then
                _NGLBookCarrierData = New DAL.NGLBookCarrierData(Parameters)
                '_NGLBookCarrierData = DirectCast(NDPBaseClassFactory("NGLBookCarrierData"), DAL.NGLBookCarrierData)
            End If
            Return _NGLBookCarrierData
        End Get
        Set(value As DAL.NGLBookCarrierData)
            _NGLBookCarrierData = value
        End Set
    End Property

    Private _NGLCarrierDropLoadData As DAL.NGLCarrierDropLoadData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierDropLoadData() As DAL.NGLCarrierDropLoadData
        Get
            If _NGLCarrierDropLoadData Is Nothing Then
                _NGLCarrierDropLoadData = New DAL.NGLCarrierDropLoadData(Parameters)
                '_NGLCarrierDropLoadData = DirectCast(NDPBaseClassFactory("NGLCarrierDropLoadData"), DAL.NGLCarrierDropLoadData)
            End If
            Return _NGLCarrierDropLoadData
        End Get
        Set(value As DAL.NGLCarrierDropLoadData)
            _NGLCarrierDropLoadData = value
        End Set
    End Property

    Private _NGLBookFinancialData As DAL.NGLBookFinancialData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookFinancialData() As DAL.NGLBookFinancialData
        Get
            If _NGLBookFinancialData Is Nothing Then
                _NGLBookFinancialData = New DAL.NGLBookFinancialData(Parameters)
                '_NGLBookFinancialData = DirectCast(NDPBaseClassFactory("NGLBookFinancialData"), DAL.NGLBookFinancialData)
            End If
            Return _NGLBookFinancialData
        End Get
        Set(value As DAL.NGLBookFinancialData)
            _NGLBookFinancialData = value
        End Set
    End Property

    Private _NGLBookLoadDetailData As DAL.NGLBookLoadDetailData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookLoadDetailData() As DAL.NGLBookLoadDetailData
        Get
            If _NGLBookLoadDetailData Is Nothing Then
                _NGLBookLoadDetailData = New DAL.NGLBookLoadDetailData(Parameters)
                '_NGLBookLoadDetailData = DirectCast(NDPBaseClassFactory("NGLBookLoadDetailData"), DAL.NGLBookLoadDetailData)
            End If
            Return _NGLBookLoadDetailData
        End Get
        Set(value As DAL.NGLBookLoadDetailData)
            _NGLBookLoadDetailData = value
        End Set
    End Property

    Private _NGLLoadStatusBoardData As DAL.NGLLoadStatusBoardData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLoadStatusBoardData() As DAL.NGLLoadStatusBoardData
        Get
            If _NGLLoadStatusBoardData Is Nothing Then
                _NGLLoadStatusBoardData = New DAL.NGLLoadStatusBoardData(Parameters)
                '_NGLLoadStatusBoardData = DirectCast(NDPBaseClassFactory("NGLLoadStatusBoardData"), DAL.NGLLoadStatusBoardData)
            End If
            Return _NGLLoadStatusBoardData
        End Get
        Set(value As DAL.NGLLoadStatusBoardData)
            _NGLLoadStatusBoardData = value
        End Set
    End Property

    Private _NGLLoadStatusCodeData As DAL.NGLLoadStatusCodeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLoadStatusCodeData() As DAL.NGLLoadStatusCodeData
        Get
            If _NGLLoadStatusCodeData Is Nothing Then
                _NGLLoadStatusCodeData = New DAL.NGLLoadStatusCodeData(Parameters)
                '_NGLLoadStatusCodeData = DirectCast(NDPBaseClassFactory("NGLLoadStatusCodeData"), DAL.NGLLoadStatusCodeData)
            End If
            Return _NGLLoadStatusCodeData
        End Get
        Set(value As DAL.NGLLoadStatusCodeData)
            _NGLLoadStatusCodeData = value
        End Set
    End Property

    Private _NGLBookFeeData As DAL.NGLBookFeeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookFeeData() As DAL.NGLBookFeeData
        Get
            If _NGLBookFeeData Is Nothing Then
                _NGLBookFeeData = New DAL.NGLBookFeeData(Parameters)
                '_NGLBookFeeData = DirectCast(NDPBaseClassFactory("NGLBookFeeData"), DAL.NGLBookFeeData)
            End If
            Return _NGLBookFeeData
        End Get
        Set(value As DAL.NGLBookFeeData)
            _NGLBookFeeData = value
        End Set
    End Property

    Private _NGLBookFeePendingData As DAL.NGLBookFeePendingData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBookFeePendingData() As DAL.NGLBookFeePendingData
        Get
            If _NGLBookFeePendingData Is Nothing Then
                _NGLBookFeePendingData = New DAL.NGLBookFeePendingData(Parameters)
                '_NGLBookFeePendingData = DirectCast(NDPBaseClassFactory("NGLBookFeePendingData"), DAL.NGLBookFeePendingData)
            End If
            Return _NGLBookFeePendingData
        End Get
        Set(value As DAL.NGLBookFeePendingData)
            _NGLBookFeePendingData = value
        End Set
    End Property

    Private _NGLCarrierData As DAL.NGLCarrierData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierData() As DAL.NGLCarrierData
        Get
            If _NGLCarrierData Is Nothing Then
                _NGLCarrierData = New DAL.NGLCarrierData(Parameters)
                '_NGLCarrierData = DirectCast(NDPBaseClassFactory("NGLCarrierData"), DAL.NGLCarrierData)
            End If
            Return _NGLCarrierData
        End Get
        Set(value As DAL.NGLCarrierData)
            _NGLCarrierData = value
        End Set
    End Property

    Private _NGLCarrierEquipCodeData As DAL.NGLCarrierEquipCodeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierEquipCodeData() As DAL.NGLCarrierEquipCodeData
        Get
            If _NGLCarrierEquipCodeData Is Nothing Then
                _NGLCarrierEquipCodeData = New DAL.NGLCarrierEquipCodeData(Parameters)
                '_NGLCarrierEquipCodeData = DirectCast(NDPBaseClassFactory("NGLCarrierEquipCodeData"), DAL.NGLCarrierEquipCodeData)
            End If
            Return _NGLCarrierEquipCodeData
        End Get
        Set(value As DAL.NGLCarrierEquipCodeData)
            _NGLCarrierEquipCodeData = value
        End Set
    End Property

    Private _NGLCarrierFuelData As DAL.NGLCarrierFuelData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierFuelData() As DAL.NGLCarrierFuelData
        Get
            If _NGLCarrierFuelData Is Nothing Then
                _NGLCarrierFuelData = New DAL.NGLCarrierFuelData(Parameters)
                '_NGLCarrierFuelData = DirectCast(NDPBaseClassFactory("NGLCarrierFuelData"), DAL.NGLCarrierFuelData)
            End If
            Return _NGLCarrierFuelData
        End Get
        Set(value As DAL.NGLCarrierFuelData)
            _NGLCarrierFuelData = value
        End Set
    End Property

    Private _NGLCarrierFuelAddendumData As DAL.NGLCarrierFuelAddendumData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierFuelAddendumData() As DAL.NGLCarrierFuelAddendumData
        Get
            If _NGLCarrierFuelAddendumData Is Nothing Then
                _NGLCarrierFuelAddendumData = New DAL.NGLCarrierFuelAddendumData(Parameters)
                '_NGLCarrierFuelAddendumData = DirectCast(NDPBaseClassFactory("NGLCarrierFuelAddendumData"), DAL.NGLCarrierFuelAddendumData)
            End If
            Return _NGLCarrierFuelAddendumData
        End Get
        Set(value As DAL.NGLCarrierFuelAddendumData)
            _NGLCarrierFuelAddendumData = value
        End Set
    End Property

    Private _NGLCarrierFuelAdExData As DAL.NGLCarrierFuelAdExData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierFuelAdExData() As DAL.NGLCarrierFuelAdExData

        Get
            If _NGLCarrierFuelAdExData Is Nothing Then
                _NGLCarrierFuelAdExData = New DAL.NGLCarrierFuelAdExData(Parameters)
                '_NGLCarrierFuelAdExData = DirectCast(NDPBaseClassFactory("NGLCarrierFuelAdExData"), DAL.NGLCarrierFuelAdExData)
            End If
            Return _NGLCarrierFuelAdExData
        End Get
        Set(value As DAL.NGLCarrierFuelAdExData)
            _NGLCarrierFuelAdExData = value
        End Set
    End Property

    Private _NGLCarrierFuelAdRateData As DAL.NGLCarrierFuelAdRateData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierFuelAdRateData() As DAL.NGLCarrierFuelAdRateData
        Get
            If _NGLCarrierFuelAdRateData Is Nothing Then
                _NGLCarrierFuelAdRateData = New DAL.NGLCarrierFuelAdRateData(Parameters)
                '_NGLCarrierFuelAdRateData = DirectCast(NDPBaseClassFactory("NGLCarrierFuelAdRateData"), DAL.NGLCarrierFuelAdRateData)
            End If
            Return _NGLCarrierFuelAdRateData
        End Get
        Set(value As DAL.NGLCarrierFuelAdRateData)
            _NGLCarrierFuelAdRateData = value
        End Set
    End Property

    Private _NGLCarrierFuelStateData As DAL.NGLCarrierFuelStateData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierFuelStateData() As DAL.NGLCarrierFuelStateData
        Get
            If _NGLCarrierFuelStateData Is Nothing Then
                _NGLCarrierFuelStateData = New DAL.NGLCarrierFuelStateData(Parameters)
                '_NGLCarrierFuelStateData = DirectCast(NDPBaseClassFactory("NGLCarrierFuelStateData"), DAL.NGLCarrierFuelStateData)
            End If
            Return _NGLCarrierFuelStateData
        End Get
        Set(value As DAL.NGLCarrierFuelStateData)
            _NGLCarrierFuelStateData = value
        End Set
    End Property

    Private _NGLCarrFeeData As DAL.NGLCarrFeeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrFeeData() As DAL.NGLCarrFeeData
        Get
            If _NGLCarrFeeData Is Nothing Then
                _NGLCarrFeeData = New DAL.NGLCarrFeeData(Parameters)
                '_NGLCarrFeeData = DirectCast(NDPBaseClassFactory("NGLCarrFeeData"), DAL.NGLCarrFeeData)
            End If
            Return _NGLCarrFeeData
        End Get
        Set(value As DAL.NGLCarrFeeData)
            _NGLCarrFeeData = value
        End Set
    End Property

    Private _NGLCarrierContData As DAL.NGLCarrierContData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierContData() As DAL.NGLCarrierContData
        Get
            If _NGLCarrierContData Is Nothing Then
                _NGLCarrierContData = New DAL.NGLCarrierContData(Parameters)
                '_NGLCarrierContData = DirectCast(NDPBaseClassFactory("NGLCarrierContData"), DAL.NGLCarrierContData)
            End If
            Return _NGLCarrierContData
        End Get
        Set(value As DAL.NGLCarrierContData)
            _NGLCarrierContData = value
        End Set
    End Property

    Private _NGLCarrTarContractData As DAL.NGLCarrTarContractData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrTarContractData() As DAL.NGLCarrTarContractData
        Get
            If _NGLCarrTarContractData Is Nothing Then
                _NGLCarrTarContractData = New DAL.NGLCarrTarContractData(Parameters)
                '_NGLCarrTarContractData = DirectCast(NDPBaseClassFactory("NGLCarrTarContractData"), DAL.NGLCarrTarContractData)
            End If
            Return _NGLCarrTarContractData
        End Get
        Set(value As DAL.NGLCarrTarContractData)
            _NGLCarrTarContractData = value
        End Set
    End Property

    Private _NGLCarrTarFeeData As DAL.NGLCarrTarFeeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrTarFeeData() As DAL.NGLCarrTarFeeData
        Get
            If _NGLCarrTarFeeData Is Nothing Then
                _NGLCarrTarFeeData = New DAL.NGLCarrTarFeeData(Parameters)
                '_NGLCarrTarFeeData = DirectCast(NDPBaseClassFactory("NGLCarrTarFeeData"), DAL.NGLCarrTarFeeData)
            End If
            Return _NGLCarrTarFeeData
        End Get
        Set(value As DAL.NGLCarrTarFeeData)
            _NGLCarrTarFeeData = value
        End Set
    End Property

    Private _NGLCarrTarEquipData As DAL.NGLCarrTarEquipData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrTarEquipData() As DAL.NGLCarrTarEquipData
        Get
            If _NGLCarrTarEquipData Is Nothing Then
                _NGLCarrTarEquipData = New DAL.NGLCarrTarEquipData(Parameters)
                '_NGLCarrTarEquipData = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipData"), DAL.NGLCarrTarEquipData)
            End If
            Return _NGLCarrTarEquipData
        End Get
        Set(value As DAL.NGLCarrTarEquipData)
            _NGLCarrTarEquipData = value
        End Set
    End Property

    Private _NGLCarrierProNumberData As DAL.NGLCarrierProNumberData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierProNumberData() As DAL.NGLCarrierProNumberData
        Get
            If _NGLCarrierProNumberData Is Nothing Then
                _NGLCarrierProNumberData = New DAL.NGLCarrierProNumberData(Parameters)
                '_NGLCarrierProNumberData = DirectCast(NDPBaseClassFactory("NGLCarrierProNumberData"), DAL.NGLCarrierProNumberData)
            End If
            Return _NGLCarrierProNumberData
        End Get
        Set(value As DAL.NGLCarrierProNumberData)
            _NGLCarrierProNumberData = value
        End Set
    End Property

    Private _NGLCompData As DAL.NGLCompData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCompData() As DAL.NGLCompData
        Get
            If _NGLCompData Is Nothing Then
                _NGLCompData = New DAL.NGLCompData(Parameters)
                '_NGLCompData = DirectCast(NDPBaseClassFactory("NGLCompData"), DAL.NGLCompData)
            End If
            Return _NGLCompData
        End Get
        Set(value As DAL.NGLCompData)
            _NGLCompData = value
        End Set
    End Property

    Private _NGLCompContData As DAL.NGLCompContData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCompContData() As DAL.NGLCompContData
        Get
            If _NGLCompContData Is Nothing Then
                _NGLCompContData = New DAL.NGLCompContData(Parameters)
                '_NGLCompContData = DirectCast(NDPBaseClassFactory("NGLCompContData"), DAL.NGLCompContData)
            End If
            Return _NGLCompContData
        End Get
        Set(value As DAL.NGLCompContData)
            _NGLCompContData = value
        End Set
    End Property

    Private _NGLAMSAppointmentData As DAL.NGLAMSAppointmentData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAMSAppointmentData() As DAL.NGLAMSAppointmentData
        Get
            If _NGLAMSAppointmentData Is Nothing Then
                _NGLAMSAppointmentData = New DAL.NGLAMSAppointmentData(Parameters)
                '_NGLAMSAppointmentData = DirectCast(NDPBaseClassFactory("NGLAMSAppointmentData"), DAL.NGLAMSAppointmentData)
            End If
            Return _NGLAMSAppointmentData
        End Get
        Set(value As DAL.NGLAMSAppointmentData)
            _NGLAMSAppointmentData = value
        End Set
    End Property

    Private _NGLAPMassEntryMsgData As DAL.NGLAPMassEntryMsg
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAPMassEntryMsgData() As DAL.NGLAPMassEntryMsg
        Get
            If _NGLAPMassEntryMsgData Is Nothing Then
                _NGLAPMassEntryMsgData = New DAL.NGLAPMassEntryMsg(Parameters)
                '_NGLAPMassEntryMsgData = DirectCast(NGLLinkDataBaseClassFactory("NGLAPMassEntryMsg"), DAL.NGLAPMassEntryMsg)
            End If
            Return _NGLAPMassEntryMsgData
        End Get
        Set(value As DAL.NGLAPMassEntryMsg)
            _NGLAPMassEntryMsgData = value
        End Set
    End Property

    Private _NGLAPMassEntryData As DAL.NGLAPMassEntryData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAPMassEntryData() As DAL.NGLAPMassEntryData
        Get
            If _NGLAPMassEntryData Is Nothing Then
                _NGLAPMassEntryData = New DAL.NGLAPMassEntryData(Parameters)
                '_NGLAPMassEntryData = DirectCast(NGLLinkDataBaseClassFactory("NGLAPMassEntryData"), DAL.NGLAPMassEntryData)
            End If
            Return _NGLAPMassEntryData
        End Get
        Set(value As DAL.NGLAPMassEntryData)
            _NGLAPMassEntryData = value
        End Set
    End Property

    Private _NGLAPMassEntryFeesData As DAL.NGLAPMassEntryFees
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAPMassEntryFeesData() As DAL.NGLAPMassEntryFees
        Get
            If _NGLAPMassEntryFeesData Is Nothing Then
                _NGLAPMassEntryFeesData = New DAL.NGLAPMassEntryFees(Parameters)
                '_NGLAPMassEntryFeesData = DirectCast(NGLLinkDataBaseClassFactory("NGLAPMassEntryFees"), DAL.NGLAPMassEntryFees)
            End If
            Return _NGLAPMassEntryFeesData
        End Get
        Set(value As DAL.NGLAPMassEntryFees)
            _NGLAPMassEntryFeesData = value
        End Set
    End Property

    Private _NGLAPMassEntryHistoriesData As DAL.NGLAPMassEntryHistories
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAPMassEntryHistoriesData() As DAL.NGLAPMassEntryHistories
        Get
            If _NGLAPMassEntryHistoriesData Is Nothing Then
                _NGLAPMassEntryHistoriesData = New DAL.NGLAPMassEntryHistories(Parameters)
                '_NGLAPMassEntryHistoriesData = DirectCast(NGLLinkDataBaseClassFactory("NGLAPMassEntryHistories"), DAL.NGLAPMassEntryHistories)
            End If
            Return _NGLAPMassEntryHistoriesData
        End Get
        Set(value As DAL.NGLAPMassEntryHistories)
            _NGLAPMassEntryHistoriesData = value
        End Set
    End Property

    Private _NGLAPMassEntryHistoryFeesData As DAL.NGLAPMassEntryHistoryFees
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLAPMassEntryHistoryFeesData() As DAL.NGLAPMassEntryHistoryFees
        Get
            If _NGLAPMassEntryHistoryFeesData Is Nothing Then
                _NGLAPMassEntryHistoryFeesData = New DAL.NGLAPMassEntryHistoryFees(Parameters)
                '_NGLAPMassEntryHistoryFeesData = DirectCast(NGLLinkDataBaseClassFactory("NGLAPMassEntryHistoryFees"), DAL.NGLAPMassEntryHistoryFees)
            End If
            Return _NGLAPMassEntryHistoryFeesData
        End Get
        Set(value As DAL.NGLAPMassEntryHistoryFees)
            _NGLAPMassEntryHistoryFeesData = value
        End Set
    End Property

    Private _NGLcmLocalizeKeyValuePairData As DAL.NGLcmLocalizeKeyValuePairData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLcmLocalizeKeyValuePairData() As DAL.NGLcmLocalizeKeyValuePairData
        Get
            If _NGLcmLocalizeKeyValuePairData Is Nothing Then
                _NGLcmLocalizeKeyValuePairData = New NGLcmLocalizeKeyValuePairData(Parameters)
                '_NGLcmLocalizeKeyValuePairData = DirectCast(NGLLinkDataBaseClassFactory("NGLcmLocalizeKeyValuePairData"), DAL.NGLcmLocalizeKeyValuePairData)
            End If
            Return _NGLcmLocalizeKeyValuePairData
        End Get
        Set(value As DAL.NGLcmLocalizeKeyValuePairData)
            _NGLcmLocalizeKeyValuePairData = value
        End Set
    End Property

    Private _NGLtblSingleSignOnAccountData As DAL.NGLtblSingleSignOnAccountData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLtblSingleSignOnAccountData() As DAL.NGLtblSingleSignOnAccountData

        Get
            If _NGLtblSingleSignOnAccountData Is Nothing Then
                _NGLtblSingleSignOnAccountData = New DAL.NGLtblSingleSignOnAccountData(Parameters)
                '_NGLtblSingleSignOnAccountData = DirectCast(NDPBaseClassFactory("NGLtblSingleSignOnAccountData"), DAL.NGLtblSingleSignOnAccountData)
            End If
            Return _NGLtblSingleSignOnAccountData
        End Get
        Set(value As DAL.NGLtblSingleSignOnAccountData)
            _NGLtblSingleSignOnAccountData = value
        End Set
    End Property


    Private _NGLtblServiceTokenData As DAL.NGLtblServiceTokenData
    ''' <summary>
    ''' Instance of DAL.NGLtblServiceTokenData property auto generated on demand
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 05/04/2021
    ''' </remarks>
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLtblServiceTokenData() As DAL.NGLtblServiceTokenData
        Get
            If _NGLtblServiceTokenData Is Nothing Then
                _NGLtblServiceTokenData = New DAL.NGLtblServiceTokenData(Parameters)
                '_NGLtblServiceTokenData = DirectCast(NDPBaseClassFactory("NGLtblServiceTokenData"), DAL.NGLtblServiceTokenData)
            End If
            Return _NGLtblServiceTokenData
        End Get
        Set(value As DAL.NGLtblServiceTokenData)
            _NGLtblServiceTokenData = value
        End Set
    End Property

    Private _NGLLegalEntityCarrierData As DAL.NGLLegalEntityCarrierData
    ''' <summary>
    ''' Instance of DAL.NGLLegalEntityCarrierData property auto generated on demand
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 05/04/2021
    ''' </remarks>
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLegalEntityCarrierData() As DAL.NGLLegalEntityCarrierData
        Get
            If _NGLLegalEntityCarrierData Is Nothing Then
                _NGLLegalEntityCarrierData = New DAL.NGLLegalEntityCarrierData(Parameters)
                '_NGLLegalEntityCarrierData = DirectCast(NGLLinkDataBaseClassFactory("NGLLegalEntityCarrierData"), DAL.NGLLegalEntityCarrierData)
            End If
            Return _NGLLegalEntityCarrierData
        End Get
        Set(value As DAL.NGLLegalEntityCarrierData)
            _NGLLegalEntityCarrierData = value
        End Set
    End Property

    Private _NGLBatchProcessData As DAL.NGLBatchProcessDataProvider
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBatchProcessData() As DAL.NGLBatchProcessDataProvider
        Get
            If _NGLBatchProcessData Is Nothing Then
                _NGLBatchProcessData = New DAL.NGLBatchProcessDataProvider(Parameters)
                '_NGLBatchProcessData = DirectCast(NDPBaseClassFactory("NGLBatchProcessDataProvider"), DAL.NGLBatchProcessDataProvider)
            End If
            Return _NGLBatchProcessData
        End Get
        Set(value As DAL.NGLBatchProcessDataProvider)
            _NGLBatchProcessData = value
        End Set
    End Property

    Private _NGLLaneData As DAL.NGLLaneData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLaneData() As DAL.NGLLaneData
        Get
            If _NGLLaneData Is Nothing Then
                _NGLLaneData = New DAL.NGLLaneData(Parameters)
                '_NGLLaneData = DirectCast(NDPBaseClassFactory("NGLLaneData"), DAL.NGLLaneData)
            End If
            Return _NGLLaneData
        End Get
        Set(value As DAL.NGLLaneData)
            _NGLLaneData = value
        End Set
    End Property

    Private _NGLLaneFeeData As DAL.NGLLaneFeeData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLaneFeeData() As DAL.NGLLaneFeeData
        Get
            If _NGLLaneFeeData Is Nothing Then
                _NGLLaneFeeData = New DAL.NGLLaneFeeData(Parameters)
                '_NGLLaneFeeData = DirectCast(NDPBaseClassFactory("NGLLaneFeeData"), DAL.NGLLaneFeeData)
            End If
            Return _NGLLaneFeeData
        End Get
        Set(value As DAL.NGLLaneFeeData)
            _NGLLaneFeeData = value
        End Set
    End Property

    Private _NGLLaneTransLoadXrefData As DAL.NGLLaneTransLoadXrefData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLaneTransLoadXrefData() As DAL.NGLLaneTransLoadXrefData
        Get
            If _NGLLaneTransLoadXrefData Is Nothing Then
                _NGLLaneTransLoadXrefData = New DAL.NGLLaneTransLoadXrefData(Parameters)
                '_NGLLaneTransLoadXrefData = DirectCast(NDPBaseClassFactory("NGLLaneTransLoadXrefData"), DAL.NGLLaneTransLoadXrefData)
            End If
            Return _NGLLaneTransLoadXrefData
        End Get
        Set(value As DAL.NGLLaneTransLoadXrefData)
            _NGLLaneTransLoadXrefData = value
        End Set
    End Property

    Private _NGLLaneTransLoadXrefDetData As DAL.NGLLaneTransLoadXrefDetData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLaneTransLoadXrefDetData() As DAL.NGLLaneTransLoadXrefDetData
        Get
            If _NGLLaneTransLoadXrefDetData Is Nothing Then
                _NGLLaneTransLoadXrefDetData = New DAL.NGLLaneTransLoadXrefDetData(Parameters)
                '_NGLLaneTransLoadXrefDetData = DirectCast(NDPBaseClassFactory("NGLLaneTransLoadXrefDetData"), DAL.NGLLaneTransLoadXrefDetData)
            End If
            Return _NGLLaneTransLoadXrefDetData
        End Get
        Set(value As DAL.NGLLaneTransLoadXrefDetData)
            _NGLLaneTransLoadXrefDetData = value
        End Set
    End Property

    Private _NGLtblStaticRouteData As DAL.NGLtblStaticRouteData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLtblStaticRouteData() As DAL.NGLtblStaticRouteData
        Get
            If _NGLtblStaticRouteData Is Nothing Then
                _NGLtblStaticRouteData = New DAL.NGLtblStaticRouteData(Parameters)
                '_NGLtblStaticRouteData = DirectCast(NDPBaseClassFactory("NGLtblStaticRouteData"), DAL.NGLtblStaticRouteData)
            End If
            Return _NGLtblStaticRouteData
        End Get
        Set(value As DAL.NGLtblStaticRouteData)
            _NGLtblStaticRouteData = value
        End Set
    End Property

    Private _NGLPOHdrData As DAL.NGLPOHdrData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLPOHdrData() As DAL.NGLPOHdrData
        Get
            If _NGLPOHdrData Is Nothing Then
                _NGLPOHdrData = New NGLPOHdrData(Parameters)
                '_NGLPOHdrData = DirectCast(NDPBaseClassFactory("NGLPOHdrData"), DAL.NGLPOHdrData)
            End If
            Return _NGLPOHdrData
        End Get
        Set(value As DAL.NGLPOHdrData)
            _NGLPOHdrData = value
        End Set
    End Property

    Private _NGLPOItemsData As DAL.NGLPOItemData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLPOItemsData() As DAL.NGLPOItemData
        Get
            If _NGLPOItemsData Is Nothing Then
                _NGLPOItemsData = New NGLPOItemData(Parameters)
                '_NGLPOItemsData = DirectCast(NDPBaseClassFactory("NGLPOItemData"), DAL.NGLPOItemData)
            End If
            Return _NGLPOItemsData
        End Get
        Set(value As DAL.NGLPOItemData)
            _NGLPOItemsData = value
        End Set
    End Property

    Private _NGLLookupData As DAL.NGLLookupDataProvider
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLookupData() As DAL.NGLLookupDataProvider
        Get
            If _NGLLookupData Is Nothing Then
                If (Parameters IsNot Nothing) Then
                    _NGLLookupData = New NGLLookupDataProvider(Parameters)

                Else
                    Throw New System.InvalidOperationException("Parameters Are Required")

                End If
            End If
            Return _NGLLookupData
        End Get
        Set(value As DAL.NGLLookupDataProvider)
            _NGLLookupData = value
        End Set
    End Property

    Private _NGLEmailData As DAL.NGLEmailData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLEmailData() As DAL.NGLEmailData
        Get
            If _NGLEmailData Is Nothing Then
                _NGLEmailData = New DAL.NGLEmailData(Parameters)
                '_NGLEmailData = DirectCast(NDPBaseClassFactory("NGLEmailData"), DAL.NGLEmailData)
            End If
            Return _NGLEmailData
        End Get
        Set(value As DAL.NGLEmailData)
            _NGLEmailData = value
        End Set
    End Property


    Private _NGLParameterData As DAL.NGLParameterData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLParameterData() As DAL.NGLParameterData
        Get
            If _NGLParameterData Is Nothing Then
                _NGLParameterData = New DAL.NGLParameterData(Parameters)
                '_NGLParameterData = DirectCast(NDPBaseClassFactory("NGLParameterData"), DAL.NGLParameterData)
            End If
            Return _NGLParameterData
        End Get
        Set(value As DAL.NGLParameterData)
            _NGLParameterData = value
        End Set
    End Property

    'NGLtblAlertMessageData
    Private _NGLtblAlertMessageData As DAL.NGLtblAlertMessageData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLtblAlertMessageData() As DAL.NGLtblAlertMessageData
        Get
            If _NGLtblAlertMessageData Is Nothing Then
                _NGLtblAlertMessageData = New DAL.NGLtblAlertMessageData(Parameters)
                '_NGLtblAlertMessageData = DirectCast(NDPBaseClassFactory("NGLtblAlertMessageData"), DAL.NGLtblAlertMessageData)
            End If
            Return _NGLtblAlertMessageData
        End Get
        Set(value As DAL.NGLtblAlertMessageData)
            _NGLtblAlertMessageData = value
        End Set
    End Property

    Private _NGLtblAccessorialData As DAL.NGLtblAccessorialData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLtblAccessorialData() As DAL.NGLtblAccessorialData
        Get
            If _NGLtblAccessorialData Is Nothing Then
                _NGLtblAccessorialData = New DAL.NGLtblAccessorialData(Parameters)
                '_NGLtblAccessorialData = DirectCast(NDPBaseClassFactory("NGLtblAccessorialData"), DAL.NGLtblAccessorialData)
            End If
            Return _NGLtblAccessorialData
        End Get
        Set(value As DAL.NGLtblAccessorialData)
            _NGLtblAccessorialData = value
        End Set
    End Property

    Private _NGLCompParameterRefSystemData As DAL.NGLCompParameterRefSystemData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCompParameterRefSystemData() As DAL.NGLCompParameterRefSystemData
        Get
            If _NGLCompParameterRefSystemData Is Nothing Then
                _NGLCompParameterRefSystemData = New DAL.NGLCompParameterRefSystemData(Parameters)

            End If
            Return _NGLCompParameterRefSystemData
        End Get
        Set(value As DAL.NGLCompParameterRefSystemData)
            _NGLCompParameterRefSystemData = value
        End Set
    End Property

    'Added by LVV on 4/12/16 for v-7.0.5.1 EDI Sim Testing Tool
    Private _NGLCarrierEDIData As DAL.NGLCarrierEDIData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCarrierEDIData() As DAL.NGLCarrierEDIData
        Get
            If _NGLCarrierEDIData Is Nothing Then
                _NGLCarrierEDIData = New DAL.NGLCarrierEDIData(Parameters)

            End If
            Return _NGLCarrierEDIData
        End Get
        Set(value As DAL.NGLCarrierEDIData)
            _NGLCarrierEDIData = value
        End Set
    End Property

    Private _NGLEDIData As DAL.NGLEDIData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLEDIData() As DAL.NGLEDIData
        Get
            If _NGLEDIData Is Nothing Then
                _NGLEDIData = New DAL.NGLEDIData(Parameters)

            End If
            Return _NGLEDIData
        End Get
        Set(value As DAL.NGLEDIData)
            _NGLEDIData = value
        End Set
    End Property


    'Added by LVV 5/23/16 for v-7.0.5.110 DAT
    Private _NGLSSOAData As DAL.NGLtblSingleSignOnAccountData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLSSOAData() As DAL.NGLtblSingleSignOnAccountData
        Get
            If _NGLSSOAData Is Nothing Then
                _NGLSSOAData = New DAL.NGLtblSingleSignOnAccountData(Parameters)

            End If
            Return _NGLSSOAData
        End Get
        Set(value As DAL.NGLtblSingleSignOnAccountData)
            _NGLSSOAData = value
        End Set
    End Property

    'Added by LVV 6/30/16 for v-7.0.5.110 DAT
    Private _NGLLoadTenderData As DAL.NGLLoadTenderData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLoadTenderData() As DAL.NGLLoadTenderData
        Get
            If _NGLLoadTenderData Is Nothing Then
                _NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)

            End If
            Return _NGLLoadTenderData
        End Get
        Set(value As DAL.NGLLoadTenderData)
            _NGLLoadTenderData = value
        End Set
    End Property

    'Added by LVV 7/11/16 for v-7.0.5.110 DAT
    Private _NGLSystemLogData As DAL.NGLSystemLogData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLSystemLogData() As DAL.NGLSystemLogData
        Get
            If _NGLSystemLogData Is Nothing Then
                _NGLSystemLogData = New DAL.NGLSystemLogData(Parameters)

            End If
            Return _NGLSystemLogData
        End Get
        Set(value As DAL.NGLSystemLogData)
            _NGLSystemLogData = value
        End Set
    End Property

    'Added by LVV 6/13/19
    Private _NGLCompParameterData As DAL.NGLCompParameterData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLCompParameterData() As DAL.NGLCompParameterData

        Get
            If _NGLCompParameterData Is Nothing Then
                _NGLCompParameterData = New DAL.NGLCompParameterData(Parameters)

            End If
            Return _NGLCompParameterData
        End Get
        Set(value As DAL.NGLCompParameterData)
            _NGLCompParameterData = value
        End Set
    End Property


    Private _NGLBidData As DAL.NGLBidData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLBidData() As DAL.NGLBidData
        Get
            If _NGLBidData Is Nothing Then
                _NGLBidData = New DAL.NGLBidData(Parameters)

            End If
            Return _NGLBidData
        End Get
        Set(value As DAL.NGLBidData)
            _NGLBidData = value
        End Set
    End Property

    'NGLLegalEntityCarrierData

    Private _NGLLegalEntityCarrierObjData As DAL.NGLLegalEntityCarrierData
    <JsonIgnore, IgnoreDataMember>
    Public Property NGLLegalEntityCarrierObjData() As DAL.NGLLegalEntityCarrierData
        Get
            If _NGLLegalEntityCarrierObjData Is Nothing Then
                _NGLLegalEntityCarrierObjData = New DAL.NGLLegalEntityCarrierData(Parameters)

            End If
            Return _NGLLegalEntityCarrierObjData
        End Get
        Set(value As NGLLegalEntityCarrierData)
            _NGLLegalEntityCarrierObjData = value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Overloads Function incrementID(ByRef seed As Integer) As Integer
        seed += 1
        Return seed
    End Function

    Protected Function sourcePath(caller As String) As String
        Return buildProcedureName(caller)
    End Function

    Protected Overridable Function buildProcedureName(ByVal caller As String) As String
        Return SourceClass & "." & caller
    End Function

    ''' <summary>
    ''' a dictionary of previously constructed objects is stored in a global module level property in Utilites called dictTARBaseClasses 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function TarBaseClassFactory(source As String, Optional ByVal blnAlwaysCreateNew As Boolean = False) As TAR.TarBaseClass
        Try
            Logger.Verbose("BLLBaseClass.TarBaseClassFactory: {0}, alwaysCreateNew: {1} ", source, blnAlwaysCreateNew)
            If (Parameters IsNot Nothing) Then
                Try
                    Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " Parameters are not null")
                    If Not blnAlwaysCreateNew Then
                        Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " not alwaysCreateNew")
                        If Not ClassRef.dictTARBaseClasses Is Nothing AndAlso ClassRef.dictTARBaseClasses.Count > 0 Then
                            If ClassRef.dictTARBaseClasses.ContainsKey(source) Then
                                Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " dictTARBaseClasses contains key")
                                Return ClassRef.dictTARBaseClasses(source)
                            End If
                        End If
                    End If
                    Dim typename As String = "NGL.FM.CarTar." + source

                    Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " typename: " & typename)
                    Dim t As Type = GetType(NGL.FM.CarTar.TarBaseClass).Assembly.[GetType](typename)
                    Logger.Verbose("BLLBaseClass.TarBaseClassFactory: {0}, Attempting to Activate type: {@1} ", source, t)
                    Dim newC As TAR.TarBaseClass = TryCast(Activator.CreateInstance(t, New Object() {Parameters}), TAR.TarBaseClass)
                    If newC Is Nothing Then
                        Logger.Error("BLLBaseClass.TarBaseClassFactory: " & source & " newC is nothing")
                        ' throwInvalidClassException(New List(Of String) From {source, "TarBaseClass"})
                    End If
                    If Not blnAlwaysCreateNew Then
                        'if we are not always creating a new instance then add the instance to the collection
                        If ClassRef.dictTARBaseClasses Is Nothing Then
                            Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " dictTARBaseClasses is nothing")
                            ClassRef.dictTARBaseClasses = New Dictionary(Of String, TAR.TarBaseClass)
                        End If
                        If Not ClassRef.dictTARBaseClasses.ContainsKey(source) Then
                            Logger.Verbose("BLLBaseClass.TarBaseClassFactory: " & source & " dictTARBaseClasses does not contain key")
                            ClassRef.dictTARBaseClasses.Add(source, newC)
                        End If
                    End If
                    Logger.Verbose("BLLBaseClass.TarBaseClassFactory: new class of type {0} created", newC.GetType().Name)
                    Return newC
                Catch exc As InvalidCastException
                    Logger.Error(exc, "BLLBaseClass.TarBaseClassFactory: " & source & " InvalidCastException")
                    '   Throw
                Catch ex As FaultException
                    Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " FaultException")
                    'Throw
                Catch ex As System.NullReferenceException
                    Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " NullReferenceException")
                    'throwInvalidClassException(New List(Of String) From {source, "TarBaseClass"})
                Catch ex As System.ArgumentNullException
                    Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " ArgumentNullException")
                    'throwInvalidClassException(New List(Of String) From {source, "TarBaseClass"})
                Catch ex As System.MissingMethodException
                    Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " MissingMethodException")
                    'throwInvalidClassException(New List(Of String) From {source, "TarBaseClass"})
                Catch ex As Exception
                    Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " Exception")
                    'throwUnExpectedFaultException(ex, buildProcedureName("TarBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                End Try
            Else
                Logger.Warning("BLLBaseClass.TarBaseClassFactory: " & source & " Parameters are null")
                'throwInvalidOperatonException("Parameters Are Required")
            End If
        Catch ex As FaultException
            Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " FaultException")
            'Throw
        Catch ex As Exception
            Logger.Error(ex, "BLLBaseClass.TarBaseClassFactory: " & source & " Exception")
            'throwUnExpectedFaultException(ex, buildProcedureName("TarBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' a dictionary of previously constructed objects is stored in a global module level property in Utilites called dictNDPBaseClasses 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NDPBaseClassFactory(source As String, Optional ByVal blnAlwaysCreateNew As Boolean = False) As DAL.NDPBaseClass
        Try
            If (Parameters IsNot Nothing) Then
                Try
                    If Not blnAlwaysCreateNew Then
                        If Not ClassRef.dictNDPBaseClasses Is Nothing AndAlso ClassRef.dictNDPBaseClasses.Count > 0 Then
                            If ClassRef.dictNDPBaseClasses.ContainsKey(source) Then Return ClassRef.dictNDPBaseClasses(source)
                        End If
                    End If
                    Dim typename As String = "Ngl.FreightMaster.Data." + source
                    Dim t As Type = GetType(NGL.FreightMaster.Data.NDPBaseClass).Assembly.[GetType](typename)
                    'if we get here we need to create a new instance and then add it to the dictionary
                    Dim newC As DAL.NDPBaseClass = TryCast(Activator.CreateInstance(t, New Object() {Parameters}), DAL.NDPBaseClass)
                    If newC Is Nothing Then
                        throwInvalidClassException(New List(Of String) From {source, "NDPBaseClass"})
                    End If
                    If Not blnAlwaysCreateNew Then
                        'if we are not always creating a new instance then add the instance to the collection
                        If ClassRef.dictNDPBaseClasses Is Nothing Then ClassRef.dictNDPBaseClasses = New Dictionary(Of String, DAL.NDPBaseClass)
                        If Not ClassRef.dictNDPBaseClasses.ContainsKey(source) Then ClassRef.dictNDPBaseClasses.Add(source, newC)
                    End If

                    Return newC

                Catch exc As InvalidCastException
                    Throw
                Catch ex As FaultException
                    Throw
                Catch ex As System.NullReferenceException
                    throwInvalidClassException(New List(Of String) From {source, "NDPBaseClass"})
                Catch ex As System.ArgumentNullException
                    throwInvalidClassException(New List(Of String) From {source, "NDPBaseClass"})
                Catch ex As System.MissingMethodException
                    throwInvalidClassException(New List(Of String) From {source, "NDPBaseClass"})
                Catch ex As System.Reflection.TargetInvocationException
                    If Not ex.InnerException Is Nothing Then
                        If TypeOf ex.InnerException Is FaultException Then
                            Throw ex.InnerException
                        End If
                        throwUnExpectedFaultException(ex.InnerException, buildProcedureName("NDPBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                    Else
                        throwUnExpectedFaultException(ex, buildProcedureName("NDPBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                    End If

                Catch ex As Exception
                    throwUnExpectedFaultException(ex, buildProcedureName("NDPBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                End Try
            Else
                throwInvalidOperatonException("Parameters Are Required")
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("NDPBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' a dictionary of previously constructed objects is stored in a global module level property in Utilites called dictBLLBaseClasses 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' As of 8/25/14 we always create a new instance of the objects when using the BaseClass due to a problem with state between calls to WCF
    ''' </remarks>
    Public Function BLLBaseClassFactory(source As String, Optional ByVal blnAlwaysCreateNew As Boolean = False) As BLLBaseClass
        Try

            If (Parameters IsNot Nothing) Then
                Try
                    If Not blnAlwaysCreateNew Then
                        If Not ClassRef.dictBLLBaseClasses Is Nothing AndAlso ClassRef.dictBLLBaseClasses.Count > 0 Then
                            If ClassRef.dictBLLBaseClasses.ContainsKey(source) Then Return ClassRef.dictBLLBaseClasses(source)
                        End If
                    End If
                    Dim typename As String = "NGL.FM.BLL." + source
                    Dim t As Type = GetType(NGL.FM.BLL.BLLBaseClass).Assembly.[GetType](typename)

                    Dim newC As BLLBaseClass = TryCast(Activator.CreateInstance(t, New Object() {Parameters}), BLLBaseClass)
                    If newC Is Nothing Then
                        throwInvalidClassException(New List(Of String) From {source, "BLLBaseClass"})
                    End If
                    newC.ClassRef = Me.ClassRef
                    If Not blnAlwaysCreateNew Then
                        'if we are not always creating a new instance then add the instance to the collection
                        If ClassRef.dictBLLBaseClasses Is Nothing Then ClassRef.dictBLLBaseClasses = New Dictionary(Of String, BLLBaseClass)
                        If Not ClassRef.dictBLLBaseClasses.ContainsKey(source) Then ClassRef.dictBLLBaseClasses.Add(source, newC)
                    End If
                    Return newC

                Catch exc As InvalidCastException
                    Throw
                Catch ex As FaultException
                    Throw
                Catch ex As System.NullReferenceException
                    throwInvalidClassException(New List(Of String) From {source, "BLLBaseClass"})
                Catch ex As System.ArgumentNullException
                    throwInvalidClassException(New List(Of String) From {source, "BLLBaseClass"})
                Catch ex As System.MissingMethodException
                    throwInvalidClassException(New List(Of String) From {source, "BLLBaseClass"})
                Catch ex As Exception
                    throwUnExpectedFaultException(ex, buildProcedureName("BLLBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                End Try
            Else
                throwInvalidOperatonException("Parameters Are Required")
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("BLLBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return Nothing
    End Function


    ''' <summary>
    ''' a dictionary of previously constructed objects is stored in a global module level property in Utilites called dictNGLLinkDataBaseClasses 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NGLLinkDataBaseClassFactory(source As String, Optional ByVal blnAlwaysCreateNew As Boolean = False) As DAL.NGLLinkDataBaseClass
        Try
            If (Parameters IsNot Nothing) Then
                Try
                    If Not blnAlwaysCreateNew Then
                        If Not ClassRef.dictNGLLinkDataBaseClasses Is Nothing AndAlso ClassRef.dictNGLLinkDataBaseClasses.Count > 0 Then
                            If ClassRef.dictNGLLinkDataBaseClasses.ContainsKey(source) Then Return ClassRef.dictNGLLinkDataBaseClasses(source)
                        End If
                    End If
                    Dim typename As String = "Ngl.FreightMaster.Data." + source
                    Dim t As Type = GetType(NGL.FreightMaster.Data.NGLLinkDataBaseClass).Assembly.[GetType](typename)
                    'if we get here we need to create a new instance and then add it to the dictionary
                    Dim newC As DAL.NGLLinkDataBaseClass = TryCast(Activator.CreateInstance(t, New Object() {Parameters}), DAL.NGLLinkDataBaseClass)
                    If newC Is Nothing Then
                        throwInvalidClassException(New List(Of String) From {source, "NGLLinkDataBaseClass"})
                    End If
                    If Not blnAlwaysCreateNew Then
                        'if we are not always creating a new instance then add the instance to the collection
                        If ClassRef.dictNGLLinkDataBaseClasses Is Nothing Then ClassRef.dictNGLLinkDataBaseClasses = New Dictionary(Of String, DAL.NGLLinkDataBaseClass)
                        If Not ClassRef.dictNGLLinkDataBaseClasses.ContainsKey(source) Then ClassRef.dictNGLLinkDataBaseClasses.Add(source, newC)
                    End If

                    Return newC

                Catch exc As InvalidCastException
                    Throw
                Catch ex As FaultException
                    Throw
                Catch ex As System.NullReferenceException
                    throwInvalidClassException(New List(Of String) From {source, "NGLLinkDataBaseClass"})
                Catch ex As System.ArgumentNullException
                    throwInvalidClassException(New List(Of String) From {source, "NGLLinkDataBaseClass"})
                Catch ex As System.MissingMethodException
                    throwInvalidClassException(New List(Of String) From {source, "NGLLinkDataBaseClass"})
                Catch ex As System.Reflection.TargetInvocationException
                    If Not ex.InnerException Is Nothing Then
                        If TypeOf ex.InnerException Is FaultException Then
                            Throw ex.InnerException
                        End If
                        throwUnExpectedFaultException(ex.InnerException, buildProcedureName("NGLLinkDataBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                    Else
                        throwUnExpectedFaultException(ex, buildProcedureName("NGLLinkDataBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                    End If

                Catch ex As Exception
                    throwUnExpectedFaultException(ex, buildProcedureName("NGLLinkDataBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
                End Try
            Else
                throwInvalidOperatonException("Parameters Are Required")
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("NGLLinkDataBaseClassFactory"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return Nothing
    End Function


    Protected Sub populateSampleParameterData()

        If Parameters Is Nothing Then
            Parameters = New DAL.WCFParameters()
        End If
        Parameters.Database = "NGLMAS2013DEV"
        Parameters.DBServer = "NGLRDP06D"
        Parameters.UserName = "ngl" & vbCr & "ramsey"
        Parameters.WCFAuthCode = "WCFDEV"
        Parameters.WCFServiceURL = "http://nglwcfdev70.nextgeneration.com"
        Parameters.ConnectionString = "Data Source=nglrdp06d;Initial Catalog=NGLMAS2013DEV;User ID=nglweb;Password=5529"
        Parameters.ProcedureControl = 0
        Parameters.ProcedureName = ""
        Parameters.FormControl = 0
        Parameters.FormName = ""
    End Sub

    Public Sub SaveAppError(Message As String)
        Try
            NGLSystemData.CreateAppErrorByMessage(Message, Parameters.UserName)
            'we ignore all errors while saving application error data
        Catch ex As Exception
        End Try

    End Sub

    Public Sub SaveSysError(Message As String, errorProcedure As String, Optional record As String = "", Optional errorNumber As Integer = 0, Optional errorSeverity As Integer = 0, Optional errorState As Integer = 0, _
        Optional errorLineNber As Integer = 0)

        Try
            Logger.Error("BLLBaseClass.SaveSysError: {Message}, {errorProcedure}, {record}, {errorNumber}, {errorSeverity}, {errorState}, {errorLineNber}, {UserName}", Message, errorProcedure, record, errorNumber, errorSeverity, errorState, errorLineNber, Parameters.UserName)

            'we ignore all errors while saving application error data
        Catch ex As Exception
        End Try


    End Sub

    Public Sub SaveSysError(Message As String, errorProcedure As String, ByVal record As String, ByVal errorNumber As Integer, ByVal errorSeverity As sysErrorSeverity, ByVal errorState As sysErrorState, ByVal errorLineNber As Integer)

        Try

            Logger.Error("BLLBaseClass.SaveSysError: {Message}, {errorProcedure}, {record}, {errorNumber}, {errorSeverity}, {errorState}, {errorLineNber}, {UserName}", Message, errorProcedure, record, errorNumber, errorSeverity, errorState, errorLineNber, Parameters.UserName)


        Catch ex As Exception
        End Try


    End Sub

    ''' <summary>
    ''' When calling executeNGLStoredProcedure the following parameters
    ''' are required by the stored procedure but are implemented in the WCF service
    ''' so they should not be included in the ProcPars array:
    ''' @UserName NVARCHAR (100), @RetMsg NVARCHAR (4000) OUTPUT, @ErrNumber INT OUTPUT
    ''' </summary>
    ''' <param name="BatchName"></param>
    ''' <param name="ProcName"></param>
    ''' <param name="ProcPars"></param>
    ''' <returns></returns>
    Protected Function executeNGLStoredProcedure(BatchName As String, ProcName As String, ProcPars As DTO.NGLStoredProcedureParameter()) As Boolean
        Try

            Return NGLBatchProcessData.executeNGLStoredProcedure(BatchName, ProcName, ProcPars)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Dim errMsg As String = String.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString())
            SaveSysError(errMsg, sourcePath("executeNGLStoredProcedure"), ProcName)
        Catch ex As Exception
            Logger.Error(ex, "BLLBaseClass.executeNGLStoredProcedure: {BatchName}, {ProcName}, {ProcPars}", BatchName, ProcName, ProcPars)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' When calling executeNGLStoredProcedure the following parameters
    ''' are required by the stored procedure but are implemented in the WCF service
    ''' so they should not be included in the ProcPars array:
    ''' @UserName NVARCHAR (100), @RetMsg NVARCHAR (4000) OUTPUT, @ErrNumber INT OUTPUT
    ''' </summary>
    ''' <param name="ProcName"></param>
    ''' <param name="ProcPars"></param>
    ''' <returns></returns>
    Protected Function executeNGLStoredProcedure(ProcName As String, ProcPars As List(Of DTO.NGLStoredProcedureParameter)) As Boolean
        Try

            Return NGLBatchProcessData.executeNGLStoredProcedure(ProcName, ProcName, ProcPars.ToArray())
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Dim errMsg As String = String.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString())
            SaveSysError(errMsg, sourcePath("executeNGLStoredProcedure"), ProcName)
        Catch ex As Exception
            Logger.Error(ex, "BLLBaseClass.executeNGLStoredProcedure: {ProcName}, {ProcPars}", ProcName, ProcPars)
        End Try
        Return False
    End Function

    Protected Function executeSQL(strSQL As String) As Boolean
        Try
            Return NGLBatchProcessData.executeSQL(strSQL)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Dim errMsg As String = String.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString())

            SaveSysError(errMsg, sourcePath("executeSQL"), strSQL)
        Catch ex As Exception
            Logger.Error(ex, "BLLBaseClass.executeSQL: {strSQL}", strSQL)
        End Try
        Return False
    End Function

    Protected Function getScalarInteger(strSQL As String) As Integer
        Dim intRet As Integer = 0
        Try
            intRet = NGLBatchProcessData.returnScalarInteger(strSQL)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Dim errMsg As String = String.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString())

            SaveSysError(errMsg, sourcePath("getScalarInteger"), strSQL)
        Catch ex As Exception
            Throw
        End Try
        Return intRet
    End Function

    Protected Function getScalarString(strSQL As String) As String
        Dim strRet As String = ""
        Try
            strRet = NGLBatchProcessData.returnScalarString(strSQL)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            Dim errMsg As String = String.Format("Reason: {0} Message: {1} Details: {2}  Trace: {3}", sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.ToString())

            SaveSysError(errMsg, sourcePath("getScalarString"), strSQL)
        Catch ex As Exception
            Throw
        End Try
        Return strRet
    End Function

    Public Function getLocalizedValueByKey(ByVal sKey As String, ByVal sDefault As String) As String
        Dim strRet As String = sDefault
        Try
            strRet = NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey(sKey, sDefault)
        Catch ex As Exception
            Logger.Warning(ex, "BLLBaseClass.getLocalizedValueByKey: {sKey}, {sDefault}", sKey, sDefault)
        End Try
        Return strRet
    End Function

#End Region

#Region " Fault Exception Methods"

    ''' <summary>
    ''' Throws a Fault Exception and optionally
    ''' Saves an Application Error Log and/or
    ''' A System Error Log
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Details"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="Reason"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <param name="pars"></param>
    ''' <remarks>
    ''' Modified by RHR 02/08/2016 v-7.0.5.0
    '''   the CreateSystemErrorByMessage was not sending the full message to the system alert
    '''   parameters because of localization issues.
    ''' </remarks>
    Public Overridable Sub throwFaultException(Message As String, Details As String, DetailsList As List(Of String), Reason As String, Optional sendAppErr As Boolean = True, Optional sendSystemError As Boolean = False, Optional pars As DAL.sysErrorParameters = Nothing)
        Try
            Dim strNotLocalizedMessage As String = ""
            Dim strNotLocalizedDetails As String = ""
            Dim strNotLocalizedReason As String = ""

            Dim strDetails As String = DAL.SqlFaultInfo.getAlertInfoNotLocalized(Message, Details, DetailsList, Reason, strNotLocalizedMessage, strNotLocalizedDetails, strNotLocalizedReason)

            Dim strFullMessage As String = String.Concat("Reason: ", strNotLocalizedReason, " Message: ", strNotLocalizedMessage, " Details: ", strDetails)
            If sendAppErr Then NGLSystemData.CreateAppErrorByMessage(strFullMessage, Me.Parameters.UserName)
            If sendSystemError Then
                If pars Is Nothing Then
                    pars = New DAL.sysErrorParameters() With {.Message = String.Concat(" Reason: ", strNotLocalizedReason, " Message: ", strNotLocalizedMessage, " "), .Record = strDetails}
                End If
                With pars
                    NGLSystemData.CreateSystemErrorByMessage(.Message, Parameters.UserName, .Procedure, .Record, .Number, .Severity, .ErrState, .LineNber)
                End With


            End If
        Catch ex As Exception
            'do nothing when saving log records
        End Try

        Throw New FaultException(Of DAL.SqlFaultInfo)(New DAL.SqlFaultInfo With {.Message = Message, .Details = Details, .DetailsList = DetailsList}, New FaultReason(Reason))

    End Sub

    ''' <summary>
    ''' Throws a Fault Exception and optionally creates an Application Error Log
    ''' </summary>
    ''' <param name="enmMsg"></param>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="enmReason"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFaultException(enmMsg As DAL.SqlFaultInfo.FaultInfoMsgs, _
                                               enmDetails As DAL.SqlFaultInfo.FaultDetailsKey, _
                                               DetailsList As List(Of String), _
                                               enmReason As DAL.SqlFaultInfo.FaultReasons, _
                                               Optional sendAppErr As Boolean = True, _
                                               Optional sendSystemError As Boolean = False)
        Try
            Dim Message = DAL.SqlFaultInfo.getFaultMessage(enmMsg)
            Dim Reason = DAL.SqlFaultInfo.getFaultReason(enmReason)
            Dim Details = DAL.SqlFaultInfo.getFaultDetailsKey(enmDetails)
            throwFaultException(Message, Details, DetailsList, Reason, sendAppErr, sendSystemError)

        Catch ex As FaultException
            Logger.Error(ex, "BLLBaseClass.throwFaultException: {enmMsg}, {enmDetails}, {DetailsList}, {enmReason}, {sendAppErr}, {sendSystemError}", enmMsg, enmDetails, DetailsList, enmReason, sendAppErr, sendSystemError)
        Catch ex As Exception
            Logger.Error(ex, "BLLBaseClass.throwFaultException: {enmMsg}, {enmDetails}, {DetailsList}, {enmReason}, {sendAppErr}, {sendSystemError}", enmMsg, enmDetails, DetailsList, enmReason, sendAppErr, sendSystemError)

        End Try
    End Sub

    ''' <summary>
    ''' Throws an E_FieldRequired Fault Exception.  The actual field name must be included in the FieldName parameter
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFieldRequiredException(ByVal FieldName As String, _
                                               Optional sendAppErr As Boolean = True, _
                                               Optional sendSystemError As Boolean = False)
        Try
            Dim DetailsList As New List(Of String) From {FieldName}
            Dim Message = DAL.SqlFaultInfo.getFaultMessage(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_InvalidRecordKeyField)
            Dim Reason = DAL.SqlFaultInfo.getFaultReason(FreightMaster.Data.SqlFaultInfo.FaultReasons.E_DataValidationFailure)
            Dim Details = DAL.SqlFaultInfo.getFaultDetailsKey(FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_FieldRequired)
            throwFaultException(Message, Details, DetailsList, Reason, sendAppErr, sendSystemError)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("throwFalutExcepton"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Throws an E_UnExpected Fault Exception and creates a system Error Message Log
    ''' </summary>
    ''' <param name="execp"></param>
    ''' <param name="Procedure"></param>
    ''' <param name="enmErrState"></param>
    ''' <param name="enmSeverity"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwUnExpectedFaultException(ByVal execp As Exception,
                                                          Procedure As String,
                                                          Optional enmErrState As DAL.sysErrorParameters.sysErrorState = DAL.sysErrorParameters.sysErrorState.UserLevelFault,
                                                          Optional enmSeverity As DAL.sysErrorParameters.sysErrorSeverity = DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        Try
            Dim sysError = New DAL.sysErrorParameters With {.ErrState = enmErrState,
                                                            .Message = execp.ToString,
                                                            .Procedure = Procedure,
                                                            .Severity = enmSeverity}
            Dim Message = DAL.SqlFaultInfo.getFaultMessage(DAL.SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG)
            Dim Reason = DAL.SqlFaultInfo.getFaultReason(DAL.SqlFaultInfo.FaultReasons.E_UnExpected)
            Dim Details = DAL.SqlFaultInfo.getFaultDetailsKey(DAL.SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails)
            throwFaultException(Message, Details, New List(Of String) From {execp.Message}, Reason, False, True, sysError)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwFaultException("E_UnExpectedMSG", execp.Message, Nothing, "E_UnExpected", True, True)
        End Try
    End Sub

    ''' <summary>
    ''' Throws an E_InvalidOperationException Fault Exception for E_InvalidClassException 
    ''' Optionally creates application and system error logs
    ''' </summary>
    ''' <param name="DetailsList"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwInvalidClassException(ByVal DetailsList As List(Of String), Optional sendAppErr As Boolean = True, Optional sendSystemError As Boolean = True)
        throwFaultException(DAL.SqlFaultInfo.FaultInfoMsgs.E_InvalidOperationException, DAL.SqlFaultInfo.FaultDetailsKey.E_InvalidClassException, DetailsList, DAL.SqlFaultInfo.FaultReasons.E_DataAccessFailure, sendAppErr, sendSystemError)
    End Sub

    ''' <summary>
    ''' Throws a system error for InvalidOperation with a server message
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwInvalidOperatonException(ByVal Message As String, Optional sendAppErr As Boolean = True, Optional sendSystemError As Boolean = True)
        Dim DetailsList As New List(Of String) From {Message}
        throwFaultException(DAL.SqlFaultInfo.FaultInfoMsgs.E_InvalidOperationException, DAL.SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, DetailsList, DAL.SqlFaultInfo.FaultReasons.E_DataAccessFailure, sendAppErr, sendSystemError)
    End Sub

    Public Overridable Sub logSystemError(ByVal pars As DAL.sysErrorParameters, ByVal Message As String, ByVal Record As String)
        If pars Is Nothing Then
            pars = New DAL.sysErrorParameters() With {.Message = Message, .Record = Record}
        End If
        With pars
            NGLSystemData.CreateSystemErrorByMessage(.Message, Parameters.UserName, .Procedure, .Record, .Number, .Severity, .ErrState, .LineNber)
        End With
    End Sub

    Public Overridable Sub logSystemError(ByVal execp As Exception,
                                          Procedure As String,
                                          Optional ByVal Record As String = "",
                                          Optional enmErrState As DAL.sysErrorParameters.sysErrorState = DAL.sysErrorParameters.sysErrorState.UserLevelFault,
                                          Optional enmSeverity As DAL.sysErrorParameters.sysErrorSeverity = DAL.sysErrorParameters.sysErrorSeverity.Unexpected)


        Dim pars As New DAL.sysErrorParameters With {.ErrState = enmErrState,
                                                     .Message = execp.ToString,
                                                     .Procedure = Procedure,
                                                     .Record = Record,
                                                     .Severity = enmSeverity}
        logSystemError(pars, "", "")

    End Sub

#End Region

End Class
