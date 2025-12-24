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
Imports NGLCoreComm = NGL.Core.Communication


Public Class clsAcceptRejectCodeInfo : Inherits BLLBaseClass


    Public Sub New(ByVal oParameters As DAL.WCFParameters, _
                   ByVal enmAcceptRejectMode As NGL.FM.BLL.NGLBookBLL.AcceptRejectModeEnum, _
                   ByVal enmAcceptRejectCode As NGL.FM.BLL.NGLBookBLL.AcceptRejectEnum, _
                   ByVal sBookRouteFinalCode As String)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLBookBLL"
        AcceptRejectCode = enmAcceptRejectCode
        AcceptRejectMode = enmAcceptRejectMode
        BookRouteFinalCode = sBookRouteFinalCode
    End Sub

    

    Private _AcceptRejectMode As NGL.FM.BLL.NGLBookBLL.AcceptRejectModeEnum
    Public Property AcceptRejectMode() As NGL.FM.BLL.NGLBookBLL.AcceptRejectModeEnum
        Get
            Return _AcceptRejectMode
        End Get
        Set(ByVal value As NGL.FM.BLL.NGLBookBLL.AcceptRejectModeEnum)
            _AcceptRejectMode = value
        End Set
    End Property

    Private _AcceptRejectCode As NGL.FM.BLL.NGLBookBLL.AcceptRejectEnum
    Public Property AcceptRejectCode() As NGL.FM.BLL.NGLBookBLL.AcceptRejectEnum
        Get
            Return _AcceptRejectCode
        End Get
        Set(ByVal value As NGL.FM.BLL.NGLBookBLL.AcceptRejectEnum)
            _AcceptRejectCode = value
        End Set
    End Property

    Private _BookRouteFinalCode As String
    Public Property BookRouteFinalCode() As String
        Get
            Return _BookRouteFinalCode
        End Get
        Set(ByVal value As String)
            _BookRouteFinalCode = value
        End Set
    End Property

    Private _BookTranCode As String
    Public Property BookTranCode() As String
        Get
            Return _BookTranCode
        End Get
        Set(ByVal value As String)
            _BookTranCode = value
        End Set
    End Property

    Private _LoadStatusCode As Integer
    Public Property LoadStatusCode() As Integer
        Get
            Return _LoadStatusCode
        End Get
        Set(ByVal value As Integer)
            _LoadStatusCode = value
        End Set
    End Property

    Private _LoadStatusControl As Integer
    Public Property LoadStatusControl() As Integer
        Get
            Return _LoadStatusControl
        End Get
        Set(ByVal value As Integer)
            _LoadStatusControl = value
        End Set
    End Property

    Private _EmailLocalizationType As DAL.Utilities.EmailLocalizationTypesEnum
    Public Property EmailLocalizationType() As DAL.Utilities.EmailLocalizationTypesEnum
        Get
            Return _EmailLocalizationType
        End Get
        Set(ByVal value As DAL.Utilities.EmailLocalizationTypesEnum)
            _EmailLocalizationType = value
        End Set
    End Property

    Private _AcceptRejectMsg As String
    Public Property AcceptRejectMsg() As String
        Get
            Return _AcceptRejectMsg
        End Get
        Set(ByVal value As String)
            _AcceptRejectMsg = value
        End Set
    End Property

    Private _InvalidAcceptRejectCode As Boolean = False
    Public Property InvalidAcceptRejectCode() As Boolean
        Get
            Return _InvalidAcceptRejectCode
        End Get
        Set(ByVal value As Boolean)
            _InvalidAcceptRejectCode = value
        End Set
    End Property



    ''' <summary>
    ''' Updates the LoadStatus properties based on the AcceptRejectCode.  If Accepted the BookTranCode is set to PB Else the BookTranCode is set to N
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub updateAcceptRejectCodeInfo()

        Select Case AcceptRejectCode
            Case NGLBookBLL.AcceptRejectEnum.Accepted
                BookTranCode = "PB" 'New BookTranCode is always PC on Accepted

                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.MANUAL Then
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadManuallyAccepted
                Else
                    If BookRouteFinalCode = "NS" Then
                        EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadChangesAccepted
                    Else
                        EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadAccepted
                    End If
                End If
                AcceptRejectMsg = "Accepted"
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.EDI Then
                    LoadStatusCode = 990
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "EDI: Load Accepted by Carrier via 990 or 997", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.EDI))
                ElseIf AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.WEB Then
                    LoadStatusCode = 19
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "NEXTrack: Load Accepted By Carrier", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.NEXTrack))
                Else 'Manual
                    LoadStatusCode = 119
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Accepted via Fax, Email or Phone", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))
                End If
            Case NGLBookBLL.AcceptRejectEnum.Rejected
                BookTranCode = "N" 'New BookTranCode is always N on Reject
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.MANUAL Then
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadManuallyRejected
                Else
                    If BookRouteFinalCode = "NS" Then
                        EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadChangesRejected
                    Else
                        EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadRejected
                    End If
                End If
                AcceptRejectMsg = "Rejected"
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.EDI Then
                    LoadStatusCode = -990
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "EDI: Load Rejected by Carrier via 990", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.EDI))
                ElseIf AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.WEB Then
                    LoadStatusCode = 21
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "NEXTrack: Load Rejected By Carrier", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.NEXTrack))
                Else
                    LoadStatusCode = 121
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Rejected via Fax, Email or Phone", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))
                End If
            Case NGLBookBLL.AcceptRejectEnum.Expired
                BookTranCode = "N" 'New BookTranCode is always N on Expired
                EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadExpired
                AcceptRejectMsg = "Expired"
                LoadStatusCode = 20
                If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Load Expired", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.None))
            Case NGLBookBLL.AcceptRejectEnum.Unfinalize
                BookTranCode = "P" 'New BookTranCode is always P on Unfinalize
                EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadUnfinalized
                AcceptRejectMsg = "Unfinalized"
                LoadStatusCode = 2000
                If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Load Unfinalized", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))
            Case NGLBookBLL.AcceptRejectEnum.Dropped
                BookTranCode = "N" 'New BookTranCode is always N when Dropped
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.MANUAL Then
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadManuallyDropped
                Else
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadDropped
                End If
                AcceptRejectMsg = "Dropped"
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.EDI Then
                    LoadStatusCode = -99021
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "EDI: Load Dropped by Carrier via 990", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.EDI))
                ElseIf AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.WEB Then
                    LoadStatusCode = 221
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "NEXTrack: Load Dropped By Carrier", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.NEXTrack))
                Else
                    LoadStatusCode = 321
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Dropped via Fax, Email or Phone", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))
                End If
            Case NGLBookBLL.AcceptRejectEnum.Unassigned
                BookTranCode = "N" 'New BookTranCode is always N on Unassigned
                EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadUnassigned
                AcceptRejectMsg = "Unassigned"
                LoadStatusCode = 421
                If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Unassigned and returned to N status", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))
            Case NGLBookBLL.AcceptRejectEnum.Tendered
                BookTranCode = "PC" 'BookTranCode is always PC on Tendered
                AcceptRejectMsg = "Tendered"
                If AcceptRejectMode = NGLBookBLL.AcceptRejectModeEnum.MANUAL Then
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadManuallyTendered
                    LoadStatusCode = 150
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then
                        LoadStatusControl = NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Tendered by TMS", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual)
                        'ByVal LoadStatusCode As Integer, ByVal LoadStatusControl As Integer
                        LoadStatusControl = storeLoadStatusControl(LoadStatusCode, LoadStatusControl)
                    End If

                Else
                    EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadAutoTendered
                    LoadStatusCode = 50
                    If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "SYSTEM: Load Auto Tendered by TMS", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.System))
                End If
            Case NGLBookBLL.AcceptRejectEnum.ModifyUnaccepted
                BookTranCode = "P" 'New BookTranCode is always P on Modify
                EmailLocalizationType = DAL.Utilities.EmailLocalizationTypesEnum.LoadModifyUnaccepted
                AcceptRejectMsg = "Unaccepted Load Modified"
                LoadStatusCode = 2001
                If Not tryGetLoadStatusControl(LoadStatusCode, LoadStatusControl) Then LoadStatusControl = storeLoadStatusControl(LoadStatusCode, NGLLoadStatusCodeData.GetLoadStatusControl(LoadStatusCode, "Manual: Load Unassigned and returned to N status", DAL.NGLLookupDataProvider.LoadStatusCodeTypes.Manual))

            Case Else
                InvalidAcceptRejectCode = True

        End Select

    End Sub


End Class
