Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.Configuration
Imports Ngl.Interfaces
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
Imports BLL = Ngl.FM.BLL
Imports PCM = Ngl.FreightMaster.PCMiler
Imports System.ServiceModel

<Serializable()>
Public Class clsLane : Inherits clsDownload


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String,
            ByVal from_email As String,
            ByVal group_email As String,
            ByVal auto_retry As Integer,
            ByVal smtp_server As String,
            ByVal db_server As String,
            ByVal database_catalog As String,
            ByVal auth_code As String,
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region

    Private _oWCfParameters As DAL.WCFParameters
    Public Property oWCFParameters() As DAL.WCFParameters
        Get
            If (_oWCfParameters Is Nothing) Then
                _oWCfParameters = New DAL.WCFParameters()
                With _oWCfParameters
                    .UserName = "System Download"
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .ConnectionString = Me.ConnectionString
                    .WCFAuthCode = "NGLSystem"
                    .ValidateAccess = False
                End With
            End If
            Return _oWCfParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _oWCfParameters = value
        End Set
    End Property

    Private _oPCMiler As BLL.NGLPCMilerBLL
    Public Property oPCMiler() As BLL.NGLPCMilerBLL
        Get
            If _oPCMiler Is Nothing Then
                _oPCMiler = New BLL.NGLPCMilerBLL(oWCFParameters)
            End If
            Return _oPCMiler
        End Get
        Set(ByVal value As BLL.NGLPCMilerBLL)
            _oPCMiler = value
        End Set
    End Property


    Protected mblnInsertRecord As Boolean = True
    Protected mstrLaneNumbers As String = ""
    Protected mblnPCMilerExpected As Boolean = False

    Public Function getDataSet() As LaneData
        Return New LaneData
    End Function

    ''' <summary>
    ''' Depricated, we now use Trimble logic call calcLatLong80 
    ''' </summary>
    ''' <param name="LaneNumber"></param>
    ''' <param name="LaneLattitude"></param>
    ''' <param name="LaneLongitude"></param>
    ''' <param name="LaneOrigZip"></param>
    ''' <param name="LaneDestZip"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 depricated
    '''     Call calcMiles80 instead
    ''' </remarks>
    Private Sub calcLatLong70(ByVal LaneNumber As String, ByRef LaneLattitude As Double, ByRef LaneLongitude As Double, ByVal LaneOrigZip As String, ByVal LaneDestZip As String, ByVal blnOriginUse As Boolean)
        Dim blnGetGeoCode As Boolean = False
        Dim strlocation As String = ""
        'Modified by RHR for v-8.4.0.003  wrapper to call 80 version
        If blnOriginUse Then
            strlocation = zipClean(LaneOrigZip)
        Else
            strlocation = zipClean(LaneDestZip)
        End If
        calcLatLong80(LaneNumber, LaneLattitude, LaneLongitude, strlocation)
        Return

        'Try
        '    'Test if a zip code is possible and the 
        '    'if we are using the origin or destination zip (blnOriginUse = LaneOriginAddressUse)
        '    If blnOriginUse Then
        '        blnGetGeoCode = True
        '        strlocation = zipClean(LaneOrigZip)
        '    Else
        '        blnGetGeoCode = True
        '        strlocation = zipClean(LaneDestZip)
        '    End If
        '    If blnGetGeoCode Then
        '        If UsePCMiler Then
        '            Dim blnGetLatLong As Boolean
        '            Dim strPCMLastError As String

        '            Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles ' Ngl.Service.PCMiler.PCMiles
        '                blnGetLatLong = oPCmiles.getGeoCode(strlocation, LaneLattitude, LaneLongitude)
        '                strPCMLastError = oPCmiles.LastError
        '            End Using

        '            If Not blnGetLatLong Or Not String.IsNullOrEmpty(strPCMLastError) Then
        '                ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calLatLong (import not affected).  There was a PC Miler error while attempting to calculate lat-long for lane number " & LaneNumber & ".<br />" & vbCrLf & strPCMLastError & "<br />" & vbCrLf
        '                Log("NGL.FreightMaster.Integration.clsLane.calLatLong ( PC Miler ) Warning!")
        '            Else
        '                If Debug Then Log("Lat/Long = " & LaneLattitude.ToString & "/" & LaneLongitude.ToString)
        '            End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calLatLong (import not affected), could not calculate lat-long for lane number " & LaneNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
        '    Log("NGL.FreightMaster.Integration.clsLane.calLatLong Unexpected Error: " & ex.ToString)
        'End Try

    End Sub

    ''' <summary>
    ''' Wrapper for calcLatLong80 used to generate the address details
    ''' </summary>
    ''' <param name="LaneNumber"></param>
    ''' <param name="LaneLattitude"></param>
    ''' <param name="LaneLongitude"></param>
    ''' <param name="sStreet"></param>
    ''' <param name="sCity"></param>
    ''' <param name="sState"></param>
    ''' <param name="sZip"></param>
    ''' <param name="sCountry"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021 
    ''' </remarks>
    Private Function calcLatLong80(ByVal LaneNumber As String, ByRef LaneLattitude As Double, ByRef LaneLongitude As Double, ByVal sStreet As String, ByVal sCity As String, ByVal sState As String, ByVal sZip As String, ByVal sCountry As String, Optional ByRef strPCMLastError As String = "") As Boolean
        Dim sAddress = oPCMiler.concateAddress(sStreet, sCity, sState, sZip, sCountry)
        Return calcLatLong80(LaneNumber, LaneLattitude, LaneLongitude, sAddress, strPCMLastError)
    End Function

    ''' <summary>
    ''' Gets Lat Long for address use oPCMiler.concateAddress to generate a valid address
    ''' </summary>
    ''' <param name="LaneNumber"></param>
    ''' <param name="LaneLattitude"></param>
    ''' <param name="LaneLongitude"></param>
    ''' <param name="sAddress"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021 
    ''' </remarks>
    Private Function calcLatLong80(ByVal LaneNumber As String, ByRef LaneLattitude As Double, ByRef LaneLongitude As Double, ByVal sAddress As String, Optional ByRef strPCMLastError As String = "") As Boolean
        Dim blnGetGeoCode As Boolean = False
        Try
            blnGetGeoCode = oPCMiler.getGeoCode(sAddress, LaneLattitude, LaneLongitude, strPCMLastError, False, "")
            If Not blnGetGeoCode Or Not String.IsNullOrEmpty(strPCMLastError) Then
                ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calLatLong80 (import not affected).  There was a PC Miler error while attempting to calculate lat-long for lane number " & LaneNumber & ".<br />" & vbCrLf & strPCMLastError & "<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsLane.calLatLong ( PC Miler ) Warning: " & strPCMLastError)
            Else
                If Debug Then Log("Lat/Long = " & LaneLattitude.ToString & "/" & LaneLongitude.ToString)
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calLatLong (import not affected), could not calculate lat-long for lane number " & LaneNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calLatLong Unexpected Error: " & ex.ToString)
        End Try
        Return blnGetGeoCode
    End Function


    ''' <summary>
    ''' Depricated, we now use Trimble logic call calcMiles80 
    ''' </summary>
    ''' <param name="OriginAddress"></param>
    ''' <param name="DestinationAddress"></param>
    ''' <param name="LaneBenchMiles"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <param name="blnUseKilometers"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 depricated
    '''     Call calcMiles80 instead
    ''' </remarks>
    Protected Sub calcMiles70(ByVal OriginAddress As DAL.LaneIntegrationAddressData,
                                   ByVal DestinationAddress As DAL.LaneIntegrationAddressData,
                                   ByRef LaneBenchMiles As Integer, ByVal LaneNumber As String,
                                   ByVal blnOriginUse As Boolean, ByVal blnUseKilometers As Boolean)

        calcMiles80(OriginAddress, DestinationAddress, LaneBenchMiles, LaneNumber, blnOriginUse)
        Return
        'the code below is no longer used.  left as a reference to the original logic
        'Dim dMiles As Double = 0
        'Dim RouteType As Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Route_Type = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_PRACTICAL
        'Dim DistType As Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_MILES
        'If blnUseKilometers Then DistType = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO
        'If UsePCMiler Then
        '    Try
        '        Dim oOrig As New Ngl.Service.PCMiler64.clsAddress
        '        Dim oDest As New Ngl.Service.PCMiler64.clsAddress

        '        oOrig.strAddress = OriginAddress.Address1
        '        oOrig.strCity = OriginAddress.City
        '        oOrig.strState = OriginAddress.State
        '        oOrig.strZip = zipClean(OriginAddress.Zip)

        '        oDest.strAddress = DestinationAddress.Address1
        '        oDest.strCity = DestinationAddress.City
        '        oDest.strState = DestinationAddress.State
        '        oDest.strZip = zipClean(DestinationAddress.Zip)

        '        Dim strLastPCMError As String = ""
        '        Dim arrBadAddresses() As Ngl.Service.PCMiler64.clsPCMBadAddress

        '        Dim oGlobalStopData As New Ngl.Service.PCMiler64.clsGlobalStopData

        '        Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles
        '            If blnOriginUse Then
        '                oGlobalStopData = oPCmiles.getPracticalMiles(oDest, oOrig, RouteType, DistType, 0, 0, 0, LaneNumber, "Lane Number", 0, 0, True, arrBadAddresses)
        '            Else
        '                oGlobalStopData = oPCmiles.getPracticalMiles(oOrig, oDest, RouteType, DistType, 0, 0, 0, LaneNumber, "Lane Number", 0, 0, True, arrBadAddresses)
        '            End If
        '            strLastPCMError = oPCmiles.LastError
        '        End Using
        '        If Not oGlobalStopData Is Nothing AndAlso String.IsNullOrEmpty(oGlobalStopData.FailedAddressMessage) Then
        '            LaneBenchMiles = CInt((oGlobalStopData.TotalMiles))
        '            If Debug Then Log("Miles = " & oGlobalStopData.TotalMiles.ToString)
        '        ElseIf Not oGlobalStopData Is Nothing Then
        '            Log("Calc Miles Failure (Import Not Affected): " & oGlobalStopData.FailedAddressMessage)
        '        Else
        '            Log("PCMiler Error (Import Not Affected): " & strLastPCMError)
        '            GroupEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calcMiles, Error (Import Not Affected): " & strLastPCMError & ".  The miles for the lane number, " & LaneNumber & " must be calculated manually.<br />" & vbCrLf
        '        End If

        '    Catch ex As System.Runtime.Serialization.SerializationException
        '        'this is thrown if we cannot reference one of the serializeable objects 
        '        'normally when one or more components are not properly installed
        '        ITEmailMsg &= "<br />" & Source & " Warning: There was a serialization exception in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & "Check that all components have been properly installed<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
        '        Log("NGL.FreightMaster.Integration.clsLane.calcMiles serialization exception: " & ex.ToString)
        '    Catch ex As Exception
        '        GroupEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & ex.Message & ".<br />" & vbCrLf
        '        ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
        '        Log("NGL.FreightMaster.Integration.clsLane.calcMiles Unexpected Error: " & ex.ToString)
        '    End Try
        'End If

    End Sub

    ''' <summary>
    ''' New Calc Miles for Trimble Wrappert to convert Lane address to PCM address
    ''' </summary>
    ''' <param name="OriginAddress"></param>
    ''' <param name="DestinationAddress"></param>
    ''' <param name="LaneBenchMiles"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021
    ''' </remarks>
    Protected Sub calcMiles80(ByVal OriginAddress As DAL.LaneIntegrationAddressData,
                                   ByVal DestinationAddress As DAL.LaneIntegrationAddressData,
                                   ByRef LaneBenchMiles As Integer, ByVal LaneNumber As String,
                                   ByVal blnOriginUse As Boolean)
        Dim oOrig As New PCM.clsAddress
        Dim oDest As New PCM.clsAddress
        oOrig.strAddress = OriginAddress.Address1
        oOrig.strCity = OriginAddress.City
        oOrig.strState = OriginAddress.State
        oOrig.strZip = zipClean(OriginAddress.Zip)

        oDest.strAddress = DestinationAddress.Address1
        oDest.strCity = DestinationAddress.City
        oDest.strState = DestinationAddress.State
        oDest.strZip = zipClean(DestinationAddress.Zip)
        calcMiles80(oOrig, oDest, LaneBenchMiles, LaneNumber, blnOriginUse)
        Return
    End Sub

    ''' <summary>
    ''' New Calc Miles for Trimble using PCM address
    ''' </summary>
    ''' <param name="oOrig"></param>
    ''' <param name="oDest"></param>
    ''' <param name="LaneBenchMiles"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021
    ''' </remarks>
    Protected Sub calcMiles80(ByVal oOrig As PCM.clsAddress,
                                   ByVal oDest As PCM.clsAddress,
                                   ByRef LaneBenchMiles As Integer,
                                   ByVal LaneNumber As String,
                                   ByVal blnOriginUse As Boolean)

        Try
            Dim strLastPCMError As String = ""
            Dim oBadAddresses As New PCM.clsPCMBadAddresses()

            Dim oAllStops As New PCM.clsAllStops()
            If blnOriginUse Then
                oAllStops = oPCMiler.GetPracticalMiles(oDest, oOrig, -1, -1, 0, 0, 0, LaneNumber, "Lane Number", 0, 0, False, oBadAddresses, False, "", strLastPCMError)
            Else
                oAllStops = oPCMiler.GetPracticalMiles(oOrig, oDest, -1, -1, 0, 0, 0, LaneNumber, "Lane Number", 0, 0, False, oBadAddresses, False, "", strLastPCMError)
            End If

            If Not oAllStops Is Nothing AndAlso String.IsNullOrEmpty(oAllStops.FailedAddressMessage) Then
                LaneBenchMiles = CInt((oAllStops.TotalMiles))
                If Debug Then Log("Miles = " & oAllStops.TotalMiles.ToString)
            ElseIf Not oAllStops Is Nothing Then
                Log("Calc Miles Failure (Import Not Affected): " & oAllStops.FailedAddressMessage)
            Else
                Log("PCMiler Error (Import Not Affected): " & strLastPCMError)
                GroupEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calcMiles, Error (Import Not Affected): " & strLastPCMError & ".  The miles for the lane number, " & LaneNumber & " must be calculated manually.<br />" & vbCrLf
            End If

        Catch ex As System.Runtime.Serialization.SerializationException
            'this is thrown if we cannot reference one of the serializeable objects 
            'normally when one or more components are not properly installed
            ITEmailMsg &= "<br />" & Source & " Warning: There was a serialization exception in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & "Check that all components have been properly installed<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calcMiles serialization exception: " & ex.ToString)
        Catch ex As Exception
            GroupEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & ex.Message & ".<br />" & vbCrLf
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & LaneNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calcMiles Unexpected Error: " & ex.ToString)
        End Try


    End Sub

    ''' <summary>
    ''' Process Lane Import Data
    ''' </summary>
    ''' <param name="oItem"></param>
    ''' <param name="oLaneWCFData"></param>
    ''' <param name="oAllowUpdatePar"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 call new CalcLatLong80 method for trimble API
    ''' </remarks>
    Public Function ProcessLaneHeader80(ByRef oItem As clsLaneObject80,
                                        ByRef oLaneWCFData As DAL.NGLLaneData,
                                        ByRef oAllowUpdatePar As AllowUpdateParameters) As ProcessLaneHeaderResult
        'Dim intRet As ProcessDataReturnValues
        Dim dtVal As Date
        Dim result As New ProcessLaneHeaderResult
        result.successFlag = True

        Try
            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            'Modified by RHR 2/26/14 Lane numbers are unique and we do not need to use the Legal Entity filter
            Dim oRow As DTO.Lane = oLaneWCFData.GetLaneFilteredByLaneNumber(oItem.LaneNumber)
            Dim insertFlag As Boolean

            Dim LaneCompControl As Integer = 0

            If oRow Is Nothing Then oRow = New DTO.Lane
            If oRow.LaneControl = 0 Then
                insertFlag = True
                If Not oLaneWCFData.ValidateLaneBeforeInsert(LaneCompControl,
                                                                 oItem.LaneCompNumber,
                                                                 oItem.LaneNumber,
                                                                 oItem.LaneName,
                                                                 oItem.LaneLegalEntity,
                                                                 oItem.LaneCompAlphaCode,
                                                                 strValMsg) Then
                    result.processLaneHeaderFailed(oItem.LaneName, oItem.LaneNumber, oItem.LaneCompAlphaCode, oItem.LaneLegalEntity)
                    Me.LastError = "Cannot insert new lane where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & strValMsg & "."
                    AddToGroupEmailMsg(Me.LastError)
                    Return result
                End If
                'Modified by RHR 2/26/14 LaneCompControl is required
                If LaneCompControl = 0 Then
                    'there was a problem reading the company data and we cannot continue
                    result.processLaneHeaderFailed(oItem.LaneName, oItem.LaneNumber, oItem.LaneCompAlphaCode, oItem.LaneLegalEntity)
                    AddToGroupEmailMsg("Cannot insert new lane where " & buildDataKeyString(result.dicInvalidKeys) & " because: The Company Control Number could not be identified.")
                    Return result
                Else
                    'save the comp control for this new lane it is a required field
                    oRow.LaneCompControl = LaneCompControl
                    oRow.LaneName = oItem.LaneName
                    oRow.LaneNumber = oItem.LaneNumber
                    oRow.LaneLegalEntity = oItem.LaneLegalEntity
                End If
            Else
                insertFlag = False
                LaneCompControl = oRow.LaneCompControl
                oItem.LaneNumber = oRow.LaneNumber
                'oItem.LaneCompAlphaCode = oRow.LaneCompAlphaCode
                oItem.LaneLegalEntity = oRow.LaneLegalEntity
                oItem.LaneName = oRow.LaneName
            End If
            oAllowUpdatePar.insertFlag = insertFlag
            result.insertFlag = insertFlag
            If String.IsNullOrEmpty(oItem.LaneOrigLegalEntity) Then oItem.LaneOrigLegalEntity = oItem.LaneLegalEntity
            If String.IsNullOrEmpty(oItem.LaneDestLegalEntity) Then oItem.LaneDestLegalEntity = oItem.LaneLegalEntity
            Dim inboundAddressOrig As New DAL.LaneIntegrationAddressData
            With inboundAddressOrig
                .CompNumber = oItem.LaneOrigCompNumber
                .LegalEntity = oItem.LaneOrigLegalEntity
                .CompAlphaCode = oItem.LaneOrigCompAlphaCode
                .CompName = oItem.LaneOrigName
                .Address1 = oItem.LaneOrigAddress1
                .Address2 = oItem.LaneOrigAddress2
                .Address3 = oItem.LaneOrigAddress3
                .City = oItem.LaneOrigCity
                .Country = oItem.LaneOrigCountry
                .State = oItem.LaneOrigState
                .Zip = oItem.LaneOrigZip
                .ContactPhone = oItem.LaneOrigContactPhone
                .ContactPhoneExt = oItem.LaneOrigContactPhoneExt
                .ContactFax = oItem.LaneOrigContactFax
                .ContactEmail = oItem.LaneOrigContactEmail
            End With
            Dim inboundAddressDest As New DAL.LaneIntegrationAddressData
            With inboundAddressDest
                .CompNumber = oItem.LaneDestCompNumber
                .LegalEntity = oItem.LaneDestLegalEntity
                .CompAlphaCode = oItem.LaneDestCompAlphaCode
                .CompName = oItem.LaneDestName
                .Address1 = oItem.LaneDestAddress1
                .Address2 = oItem.LaneDestAddress2
                .Address3 = oItem.LaneDestAddress3
                .City = oItem.LaneDestCity
                .Country = oItem.LaneDestCountry
                .State = oItem.LaneDestState
                .Zip = oItem.LaneDestZip
                .ContactPhone = oItem.LaneDestContactPhone
                .ContactPhoneExt = oItem.LaneDestContactPhoneExt
                .ContactFax = oItem.LaneDestContactFax
                .ContactEmail = oItem.LaneDestContactEmail
            End With
            Dim updatedAddressOrig As New DAL.LaneIntegrationAddressData
            Dim updatedAddressDest As New DAL.LaneIntegrationAddressData

            Dim currentAddressOrig As New DAL.LaneIntegrationAddressData
            Dim currentAddressDest As New DAL.LaneIntegrationAddressData
            If insertFlag = False Then
                With currentAddressOrig
                    .CompName = oRow.LaneOrigName
                    .Address1 = oRow.LaneOrigAddress1
                    .Address2 = oRow.LaneOrigAddress2
                    .Address3 = oRow.LaneOrigAddress3
                    .City = oRow.LaneOrigCity
                    .Country = oRow.LaneOrigCountry
                    .State = oRow.LaneOrigState
                    .Zip = oRow.LaneOrigZip
                    .ContactPhone = oRow.LaneOrigContactPhone
                    .ContactPhoneExt = oRow.LaneOrigContactPhoneExt
                    .ContactFax = oRow.LaneOrigContactFax
                    .ContactEmail = oRow.LaneOrigContactEmail
                End With
                With currentAddressDest
                    .CompName = oRow.LaneDestName
                    .Address1 = oRow.LaneDestAddress1
                    .Address2 = oRow.LaneDestAddress2
                    .Address3 = oRow.LaneDestAddress3
                    .City = oRow.LaneDestCity
                    .Country = oRow.LaneDestCountry
                    .State = oRow.LaneDestState
                    .Zip = oRow.LaneDestZip
                    .ContactPhone = oRow.LaneDestContactPhone
                    .ContactPhoneExt = oRow.LaneDestContactPhoneExt
                    .ContactFax = oRow.LaneDestContactFax
                    .ContactEmail = oRow.LaneDestContactEmail
                End With
                updatedAddressOrig = oLaneWCFData.LookupLaneAddressInformation(inboundAddressOrig, currentAddressOrig)
                updatedAddressDest = oLaneWCFData.LookupLaneAddressInformation(inboundAddressDest, currentAddressDest)
            Else
                updatedAddressOrig = oLaneWCFData.LookupLaneAddressInformation(inboundAddressOrig, Nothing)
                updatedAddressDest = oLaneWCFData.LookupLaneAddressInformation(inboundAddressDest, Nothing)
            End If

            With oRow
                'Removed by RHR 2/26/14 oRow.LaneNumber, oRow.LaneName,oRow.LaneLegalEntity, and oRow.LaneCompControl are 
                'never updated and must be configured above for new lanes
                '.LaneNumber = oItem.LaneNumber
                '.LaneName = oItem.LaneName
                ' If AllowUpdate("LaneLegalEntity", oAllowUpdatePar) Then .LaneLegalEntity = oItem.LaneLegalEntity
                If AllowUpdate("LaneNumberMaster", oAllowUpdatePar) Then .LaneNumberMaster = oItem.LaneNumberMaster
                If AllowUpdate("LaneNameMaster", oAllowUpdatePar) Then .LaneNameMaster = oItem.LaneNameMaster

                If AllowUpdate("LaneDefaultCarrierUse", oAllowUpdatePar) Then .LaneDefaultCarrierUse = oItem.LaneDefaultCarrierUse
                'Do I have to check allowUpdate for either LaneDefaultCarrierControl or LAneDefaultCarrierNumber in here somewhere?
                If oItem.LaneDefaultCarrierNumber > 0 Then
                    Dim nCarrierControl = oLaneWCFData.GetCarrierControl(oItem.LaneDefaultCarrierNumber)
                    If nCarrierControl > 0 Then
                        .LaneDefaultCarrierControl = nCarrierControl
                    End If
                End If

                If AllowUpdate("LaneOrigName", oAllowUpdatePar) Then .LaneOrigName = updatedAddressOrig.CompName
                If AllowUpdate("LaneOrigCompNumber", oAllowUpdatePar) Then .LaneOrigCompControl = updatedAddressOrig.CompControl
                If AllowUpdate("LaneOrigAddress1", oAllowUpdatePar) Then .LaneOrigAddress1 = updatedAddressOrig.Address1
                If AllowUpdate("LaneOrigAddress2", oAllowUpdatePar) Then .LaneOrigAddress2 = updatedAddressOrig.Address2
                If AllowUpdate("LaneOrigAddress3", oAllowUpdatePar) Then .LaneOrigAddress3 = updatedAddressOrig.Address3
                If AllowUpdate("LaneOrigCity", oAllowUpdatePar) Then .LaneOrigCity = updatedAddressOrig.City
                If AllowUpdate("LaneOrigCountry", oAllowUpdatePar) Then .LaneOrigCountry = updatedAddressOrig.Country
                If AllowUpdate("LaneOrigState", oAllowUpdatePar) Then .LaneOrigState = updatedAddressOrig.State
                If AllowUpdate("LaneOrigZip", oAllowUpdatePar) Then .LaneOrigZip = updatedAddressOrig.Zip
                If AllowUpdate("LaneOrigContactPhone", oAllowUpdatePar) Then .LaneOrigContactPhone = updatedAddressOrig.ContactPhone
                If AllowUpdate("LaneOrigContactPhoneExt", oAllowUpdatePar) Then .LaneOrigContactPhoneExt = updatedAddressOrig.ContactPhoneExt
                If AllowUpdate("LaneOrigContactFax", oAllowUpdatePar) Then .LaneOrigContactFax = updatedAddressOrig.ContactFax
                If AllowUpdate("LaneDestName", oAllowUpdatePar) Then .LaneDestName = updatedAddressDest.CompName
                If AllowUpdate("LaneDestCompNumber", oAllowUpdatePar) Then .LaneDestCompControl = updatedAddressDest.CompControl
                If AllowUpdate("LaneDestAddress1", oAllowUpdatePar) Then .LaneDestAddress1 = updatedAddressDest.Address1
                If AllowUpdate("LaneDestAddress2", oAllowUpdatePar) Then .LaneDestAddress2 = updatedAddressDest.Address2
                If AllowUpdate("LaneDestAddress3", oAllowUpdatePar) Then .LaneDestAddress3 = updatedAddressDest.Address3
                If AllowUpdate("LaneDestCity", oAllowUpdatePar) Then .LaneDestCity = updatedAddressDest.City
                If AllowUpdate("LaneDestCountry", oAllowUpdatePar) Then .LaneDestCountry = updatedAddressDest.Country
                If AllowUpdate("LaneDestState", oAllowUpdatePar) Then .LaneDestState = updatedAddressDest.State
                If AllowUpdate("LaneDestZip", oAllowUpdatePar) Then .LaneDestZip = updatedAddressDest.Zip
                If AllowUpdate("LaneDestContactPhone", oAllowUpdatePar) Then .LaneDestContactPhone = updatedAddressDest.ContactPhone
                If AllowUpdate("LaneDestContactPhoneExt", oAllowUpdatePar) Then .LaneDestContactPhoneExt = updatedAddressDest.ContactPhoneExt
                If AllowUpdate("LaneDestContactFax", oAllowUpdatePar) Then .LaneDestContactFax = updatedAddressDest.ContactFax
                If AllowUpdate("LaneOriginAddressUse", oAllowUpdatePar) Then .LaneOriginAddressUse = oItem.LaneOriginAddressUse

                If insertFlag _
                Or (AllowUpdate("LaneLatitude", oAllowUpdatePar) And AllowUpdate("LaneLongitude", oAllowUpdatePar)) _
                And (updatedAddressOrig.HasAddressChanged Or updatedAddressDest.HasAddressChanged) _
                Then
                    'Modified by RHR for v-8.4.0.003 on 07/19/2021 call new CalcLatLong80 method for trimble API
                    If oItem.LaneOriginAddressUse Then
                        calcLatLong80(oItem.LaneNumber, oItem.LaneLatitude, oItem.LaneLongitude, oItem.LaneOrigAddress1, oItem.LaneOrigCity, oItem.LaneOrigState, oItem.LaneOrigZip, oItem.LaneOrigCountry)
                    Else
                        calcLatLong80(oItem.LaneNumber, oItem.LaneLatitude, oItem.LaneLongitude, oItem.LaneDestAddress1, oItem.LaneDestCity, oItem.LaneDestState, oItem.LaneDestZip, oItem.LaneDestCountry)
                    End If
                    'Reomved 07/19/2021 calcLatLong70(oItem.LaneNumber, oItem.LaneLatitude, oItem.LaneLongitude, oItem.LaneOrigZip, oItem.LaneDestZip, oItem.LaneOriginAddressUse)
                    .LaneLatitude = oItem.LaneLatitude
                    .LaneLongitude = oItem.LaneLongitude
                    If AllowUpdate("LaneBenchMiles", oAllowUpdatePar) Then
                        calcMiles80(updatedAddressOrig, updatedAddressDest, oItem.LaneBenchMiles, oItem.LaneNumber, oItem.LaneOriginAddressUse)
                        .LaneBenchMiles = oItem.LaneBenchMiles
                    End If
                End If

                If AllowUpdate("LaneConsigneeNumber", oAllowUpdatePar) Then .LaneConsigneeNumber = oItem.LaneConsigneeNumber
                If AllowUpdate("LaneRecMinIn", oAllowUpdatePar) Then .LaneRecMinIn = oItem.LaneRecMinIn
                If AllowUpdate("LaneRecMinUnload", oAllowUpdatePar) Then .LaneRecMinUnload = oItem.LaneRecMinUnload
                If AllowUpdate("LaneRecMinOut", oAllowUpdatePar) Then .LaneRecMinOut = oItem.LaneRecMinOut
                If AllowUpdate("LaneAppt", oAllowUpdatePar) Then .LaneAppt = oItem.LaneAppt
                If AllowUpdate("LanePalletExchange", oAllowUpdatePar) Then .LanePalletExchange = oItem.LanePalletExchange
                If AllowUpdate("LanePalletType", oAllowUpdatePar) Then .LanePalletType = oItem.LanePalletType
                If AllowUpdate("LaneBFC", oAllowUpdatePar) Then .LaneBFC = oItem.LaneBFC
                If AllowUpdate("LaneBFCType", oAllowUpdatePar) Then .LaneBFCType = oItem.LaneBFCType
                If validateDateWS(oItem.LaneRecHourStart, dtVal) Then
                    .LaneRecHourStart = exportDateToString(dtVal.ToString)
                End If
                If validateDateWS(oItem.LaneRecHourStop, dtVal) Then
                    .LaneRecHourStop = exportDateToString(dtVal.ToString)
                End If
                If validateDateWS(oItem.LaneDestHourStart, dtVal) Then
                    .LaneDestHourStart = exportDateToString(dtVal.ToString)
                End If
                If validateDateWS(oItem.LaneDestHourStop, dtVal) Then
                    .LaneDestHourStop = exportDateToString(dtVal.ToString)
                End If
                If AllowUpdate("LaneComments", oAllowUpdatePar) Then .LaneComments = oItem.LaneComments
                If AllowUpdate("LaneCommentsConfidential", oAllowUpdatePar) Then .LaneCommentsConfidential = oItem.LaneCommentsConfidential

                'may need to perform a cross reference lookup for the controlNumber; check old logic (tempType and TransType)
                If AllowUpdate("LaneTempType", oAllowUpdatePar) Then .LaneTempType = oItem.LaneTempType
                If AllowUpdate("LaneTransType", oAllowUpdatePar) Then .LaneTransType = oItem.LaneTransType
                If AllowUpdate("LanePrimaryBuyer", oAllowUpdatePar) Then .LanePrimaryBuyer = oItem.LanePrimaryBuyer
                If AllowUpdate("LaneAptDelivery", oAllowUpdatePar) Then .LaneAptDelivery = oItem.LaneAptDelivery
                If AllowUpdate("LaneCarrierEquipmentCodes", oAllowUpdatePar) Then .LaneCarrierEquipmentCodes = oItem.LaneCarrierEquipmentCodes
                If AllowUpdate("LaneChepGLID", oAllowUpdatePar) Then .LaneChepGLID = oItem.LaneChepGLID
                If AllowUpdate("LaneCarrierTypeCode", oAllowUpdatePar) Then .LaneCarrierTypeCode = oItem.LaneCarrierTypeCode
                If AllowUpdate("LanePickUpMon", oAllowUpdatePar) Then .LanePickUpMon = oItem.LanePickUpMon
                If AllowUpdate("LanePickUpTue", oAllowUpdatePar) Then .LanePickUpTue = oItem.LanePickUpTue
                If AllowUpdate("LanePickUpWed", oAllowUpdatePar) Then .LanePickUpWed = oItem.LanePickUpWed
                If AllowUpdate("LanePickUpThu", oAllowUpdatePar) Then .LanePickUpThu = oItem.LanePickUpThu
                If AllowUpdate("LanePickUpFri", oAllowUpdatePar) Then .LanePickUpFri = oItem.LanePickUpFri
                If AllowUpdate("LanePickUpSat", oAllowUpdatePar) Then .LanePickUpSat = oItem.LanePickUpSat
                If AllowUpdate("LanePickUpSun", oAllowUpdatePar) Then .LanePickUpSun = oItem.LanePickUpSun
                If AllowUpdate("LaneDropOffMon", oAllowUpdatePar) Then .LaneDropOffMon = oItem.LaneDropOffMon
                If AllowUpdate("LaneDropOffTue", oAllowUpdatePar) Then .LaneDropOffTue = oItem.LaneDropOffTue
                If AllowUpdate("LaneDropOffWed", oAllowUpdatePar) Then .LaneDropOffWed = oItem.LaneDropOffWed
                If AllowUpdate("LaneDropOffThu", oAllowUpdatePar) Then .LaneDropOffThu = oItem.LaneDropOffThu
                If AllowUpdate("LaneDropOffFri", oAllowUpdatePar) Then .LaneDropOffFri = oItem.LaneDropOffFri
                If AllowUpdate("LaneDropOffSat", oAllowUpdatePar) Then .LaneDropOffSat = oItem.LaneDropOffSat
                If AllowUpdate("LaneDropOffSun", oAllowUpdatePar) Then .LaneDropOffSun = oItem.LaneDropOffSun
                '60
                If AllowUpdate("LaneDefaultRouteSequence", oAllowUpdatePar) Then .LaneDefaultRouteSequence = oItem.LaneDefaultRouteSequence
                If AllowUpdate("LaneRouteGuideNumber", oAllowUpdatePar) Then .LaneRouteGuideNumber = oItem.LaneRouteGuideNumber
                If AllowUpdate("LaneIsCrossDockFacility", oAllowUpdatePar) Then .LaneIsCrossDockFacility = oItem.LaneIsCrossDockFacility
                '70

                If AllowUpdate("LaneRequiredOnTimeServiceLevel", oAllowUpdatePar) Then .LaneRequiredOnTimeServiceLevel = oItem.LaneRequiredOnTimeServiceLevel
                If AllowUpdate("LaneCalcOnTimeServiceLevel", oAllowUpdatePar) Then .LaneCalcOnTimeServiceLevel = oItem.LaneCalcOnTimeServiceLevel
                If AllowUpdate("LaneCalcOnTimeNoMonthsUsed", oAllowUpdatePar) Then .LaneCalcOnTimeNoMonthsUsed = oItem.LaneCalcOnTimeNoMonthsUsed
                If AllowUpdate("LaneModeTypeControl", oAllowUpdatePar) Then .LaneModeTypeControl = If(oItem.LaneModeTypeControl = 0, 3, oItem.LaneModeTypeControl)
                If AllowUpdate("LaneUser1", oAllowUpdatePar) Then .LaneUser1 = oItem.LaneUser1
                If AllowUpdate("LaneUser2", oAllowUpdatePar) Then .LaneUser2 = oItem.LaneUser2
                If AllowUpdate("LaneUser3", oAllowUpdatePar) Then .LaneUser3 = oItem.LaneUser3
                If AllowUpdate("LaneUser4", oAllowUpdatePar) Then .LaneUser4 = oItem.LaneUser4

                '80
                If AllowUpdate("LaneOrigContactEmail", oAllowUpdatePar) Then .LaneOrigContactEmail = updatedAddressOrig.ContactEmail
                If AllowUpdate("LaneDestContactEmail", oAllowUpdatePar) Then .LaneDestContactEmail = updatedAddressDest.ContactEmail

            End With
            'Update or Create new Record
            If insertFlag Then
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Created
                oRow = oLaneWCFData.CreateRecord(oRow)
                result.successFlag = True
            Else
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Updated
                oRow = oLaneWCFData.UpdateRecord(oRow)
                result.successFlag = True
            End If

            If Not oRow Is Nothing Then result.LaneControl = oRow.LaneControl
            If result.successFlag Then
                Dim intLaneControl As Integer = oRow.LaneControl
                If intLaneControl <> 0 Then
                    'Modified by RHR 2/26/14 we now use the lane control not the lane comp control for these methods
                    oLaneWCFData.saveBrokerData70(oItem.BrokerName, oItem.BrokerNumber, intLaneControl)
                    UpdateRouteGuideControl70(oItem.LaneNumber, intLaneControl)

                End If
            End If
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            result.processLaneHeaderFailed(oItem.LaneName, oItem.LaneNumber, oItem.LaneCompAlphaCode, oItem.LaneLegalEntity)
            If ex.Detail.Message = "E_InvalidKeyFilterMetaData" Then
                AddToGroupEmailMsg(String.Format("Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.", ex.Detail.DetailsList.ToArray()))
            ElseIf ex.Detail.Message = "E_InvalidRecordKeyField" Then
                AddToGroupEmailMsg(String.Format("The '{0}' is required and cannot be empty.", ex.Detail.DetailsList.ToArray()))
            Else
                AddToGroupEmailMsg("Could not import Lane where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & ex.Detail.ToString(ex.Reason.ToString()) & ".")
            End If
        Catch ex As Exception
            result.processLaneHeaderFailed(oItem.LaneName, oItem.LaneNumber, oItem.LaneCompAlphaCode, oItem.LaneLegalEntity)
            AddToGroupEmailMsg("Could not import Lane where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & ex.ToString() & ".")
        End Try

        Return result

    End Function


    Public Function ProcessObjectData70(
                        ByVal oLanes As List(Of clsLaneObject70),
                        ByVal strConnection As String,
                        Optional ByVal oCalendars As List(Of clsLaneCalendarObject70) = Nothing) As clsIntegrationUpdateResults
        Dim oLanes80 As New List(Of clsLaneObject80)
        Dim oCalendars80 As New List(Of clsLaneCalendarObject80)
        Dim finalResult As New clsIntegrationUpdateResults
        Dim strMsg As String = ""
        If Not oLanes Is Nothing AndAlso oLanes.Count() > 0 Then
            For Each o In oLanes
                Dim strSkip As New List(Of String) From {""}
                Dim newLane = New clsLaneObject80
                strMsg = ""
                CopyMatchingFields(newLane, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oLanes80.Add(newLane)
            Next
        Else
            finalResult.ReturnValue = ProcessDataReturnValues.nglDataIntegrationComplete
            Return finalResult
        End If

        If Not oCalendars Is Nothing AndAlso oCalendars.Count() > 0 Then
            For Each o In oCalendars
                Dim strSkip As New List(Of String) From {""}
                Dim newCalendar = New clsLaneCalendarObject80
                strMsg = ""
                CopyMatchingFields(newCalendar, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oCalendars80.Add(newCalendar)
            Next
        End If

        Return ProcessObjectData80(oLanes80, strConnection, oCalendars80)


    End Function


    Public Function ProcessObjectData80(
                        ByVal oLanes As List(Of clsLaneObject80),
                        ByVal strConnection As String,
                        Optional ByVal oCalendars As List(Of clsLaneCalendarObject80) = Nothing) As clsIntegrationUpdateResults

        Dim finalResult As New clsIntegrationUpdateResults
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Dim dtVal As Date
        Dim resultObject = New ProcessLaneHeaderResult
        Try
            oWCFParameters.ConnectionString = strConnection

            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oLaneWCFData = New DAL.NGLLaneData(oWCFParameters)
            Dim oLaneCalWCFData = New DAL.NGLLaneCalData(oWCFParameters)
            Dim dictionaryList As New List(Of Dictionary(Of String, String))
            Dim successFlag As Boolean = False

            'Create a new allow update parameter object
            Dim oAllowUpdatePar As New AllowUpdateParameters With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Lane}
            'Header
            For Each oItem As clsLaneObject80 In oLanes
                If Not oItem Is Nothing Then
                    resultObject = ProcessLaneHeader80(oItem, oLaneWCFData, oAllowUpdatePar)
                    If resultObject.successFlag Then
                        'Modified  by RHR 7/15/2015 we always return the control numbers for testing and debugging 
                        'not just on insert
                        'If resultObject.insertFlag Then finalResult.ControlNumbers.Add(resultObject.LaneControl)
                        finalResult.ControlNumbers.Add(resultObject.LaneControl)
                        'at least one lane record was processed so set success = true
                        successFlag = True
                    Else
                        dictionaryList.Add(resultObject.dicInvalidKeys)
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                    intRet = resultObject.intRet
                Else
                    AddToGroupEmailMsg("One of the lane header records was null or empty and could not be processed.")
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Next
            If successFlag = False Then
                'this only happens if all lanes fail to import (no need to continue with details)
                finalResult.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
                Return finalResult
            End If
            'Calendar
            If Not oCalendars Is Nothing AndAlso oCalendars.Count > 0 Then
                For Each oCalItem As clsLaneCalendarObject80 In oCalendars
                    If Not oCalItem Is Nothing Then
                        Try
                            oAllowUpdatePar.insertFlag = False
                            Dim blnCanImport As Boolean = True
                            Dim strLogMsg As String = ""
                            For Each oDictionaryItem As Dictionary(Of String, String) In dictionaryList
                                'Rules:
                                '1. if any Lane Number matches we cannot import unless it is 0
                                '2. If the Lane Alpha Code and the Lane Legal Entity match we cannot import
                                If oDictionaryItem("Lane Number") <> 0 AndAlso oDictionaryItem("Lane Number") = oCalItem.LaneNumber Then
                                    blnCanImport = False
                                    strLogMsg = "Lane Number = " & oCalItem.LaneNumber
                                    Exit For
                                ElseIf oDictionaryItem("Lane Alpha Code") = oCalItem.LaneCompAlphaCode And oDictionaryItem("Lane Legal Entity") = oCalItem.LaneLegalEntity Then
                                    blnCanImport = False
                                    strLogMsg = "Lane Alpha Code = " & oCalItem.LaneCompAlphaCode
                                    Exit For
                                End If
                            Next
                            If blnCanImport Then
                                If validateDateWS(oCalItem.StartTime, dtVal) Then
                                    oCalItem.StartTime = exportDateToString(dtVal.ToString)
                                Else
                                    oCalItem.StartTime = ""
                                End If
                                If validateDateWS(oCalItem.EndTime, dtVal) Then
                                    oCalItem.EndTime = exportDateToString(dtVal.ToString)
                                Else
                                    oCalItem.EndTime = ""
                                End If
                                oLaneCalWCFData.InsertOrUpdateLaneCal70(oCalItem.LaneLegalEntity, oCalItem.LaneNumber, oCalItem.LaneCompAlphaCode, oCalItem.Month, AllowUpdate("Month", oAllowUpdatePar), oCalItem.Day, AllowUpdate("Day", oAllowUpdatePar), oCalItem.Open, AllowUpdate("Open", oAllowUpdatePar), oCalItem.StartTime, AllowUpdate("StartTime", oAllowUpdatePar), oCalItem.EndTime, AllowUpdate("EndTime", oAllowUpdatePar), oCalItem.IsHoliday, AllowUpdate("IsHoliday", oAllowUpdatePar), oCalItem.ApplyToOrigin, AllowUpdate("ApplyToOrigin", oAllowUpdatePar))
                            Else
                                Log("Skipping detail record where " & strLogMsg & " because header record failed to import")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As FaultException(Of DAL.SqlFaultInfo)
                            If ex.Detail.Message = "E_ServerMsgDetails" AndAlso Not ex.Detail.DetailsList Is Nothing AndAlso ex.Detail.DetailsList.Count > 0 Then
                                AddToGroupEmailMsg(String.Format("Update Lane Calendar information error. Server Message: {0}", ex.Detail.DetailsList))
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            Else
                                AddToGroupEmailMsg("Invalid or missing lane calendar information")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            'modify to add to either group or IT emailMsg and let finally code do the actual sending (do this for all LogExceptions)
                            AddToGroupEmailMsg("Invalid or missing lane calendar information")
                            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                        End Try
                    Else
                        AddToGroupEmailMsg("One of the lane calendar records was null or empty and could not be processed.")
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                Next
            End If

        Catch ex As Exception
            AddToITEmailMsg("Lane import system error")
            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        Finally
            If GroupEmailMsg.Trim.Length > 0 Then
                LogGroupEmailError("Process Lane Data Warning", "The following errors or warnings were reported some lane records may not have been processed correctly." & GroupEmailMsg)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogAdminEmailError("Process Lane Data Failure", "The following errors or warnings were reported some lane records may not have been processed correctly." & ITEmailMsg)
            End If
        End Try

        finalResult.ReturnValue = intRet
        Return finalResult

    End Function


    Public Function ProcessObjectData(
                        ByVal oLanes() As clsLaneObject,
                        ByVal strConnection As String,
                        Optional ByVal oCalendars() As clsLaneCalendarObject = Nothing) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New LaneData.LaneHeaderDataTable
        Dim oCTable As New LaneData.LaneCalendarDataTable
        Dim dtVal As Date
        Try
            For Each oItem As clsLaneObject In oLanes
                Dim oRow As LaneData.LaneHeaderRow = oHTable.NewLaneHeaderRow
                With oRow
                    .LaneNumber = Left(oItem.LaneNumber, 50)
                    .LaneName = Left(oItem.LaneName, 50)
                    .LaneNumberMaster = Left(oItem.LaneNumberMaster, 50)
                    .LaneNameMaster = Left(oItem.LaneNameMaster, 50)
                    .LaneCompNumber = Left(oItem.LaneCompNumber, 50)
                    .LaneDefaultCarrierUse = oItem.LaneDefaultCarrierUse
                    .LaneDefaultCarrierNumber = oItem.LaneDefaultCarrierNumber
                    .LaneOrigCompNumber = Left(oItem.LaneOrigCompNumber, 50)
                    .LaneOrigName = Left(oItem.LaneOrigName, 40)
                    .LaneOrigAddress1 = Left(oItem.LaneOrigAddress1, 40)
                    .LaneOrigAddress2 = Left(oItem.LaneOrigAddress2, 40)
                    .LaneOrigAddress3 = Left(oItem.LaneOrigAddress3, 40)
                    .LaneOrigCity = Left(oItem.LaneOrigCity, 25)
                    .LaneOrigCountry = Left(oItem.LaneOrigCountry, 30)
                    .LaneOrigState = Left(oItem.LaneOrigState, 2)
                    .LaneOrigZip = Left(oItem.LaneOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    .LaneOrigContactPhone = Left(oItem.LaneOrigContactPhone, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
                    .LaneOrigContactPhoneExt = Left(oItem.LaneOrigContactPhoneExt, 50)
                    .LaneOrigContactFax = Left(oItem.LaneOrigContactFax, 15)
                    .LaneDestCompNumber = Left(oItem.LaneDestCompNumber, 50)
                    .LaneDestName = Left(oItem.LaneDestName, 40)
                    .LaneDestAddress1 = Left(oItem.LaneDestAddress1, 40)
                    .LaneDestAddress2 = Left(oItem.LaneDestAddress2, 40)
                    .LaneDestAddress3 = Left(oItem.LaneDestAddress3, 40)
                    .LaneDestCity = Left(oItem.LaneDestCity, 25)
                    .LaneDestCountry = Left(oItem.LaneDestCountry, 30)
                    .LaneDestState = Left(oItem.LaneDestState, 2)
                    .LaneDestZip = Left(oItem.LaneDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    .LaneDestContactPhone = Left(oItem.LaneDestContactPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                    .LaneDestContactPhoneExt = Left(oItem.LaneDestContactPhoneExt, 50)
                    .LaneDestContactFax = Left(oItem.LaneDestContactFax, 15)
                    .LaneConsigneeNumber = Left(oItem.LaneConsigneeNumber, 15)
                    .LaneRecMinIn = oItem.LaneRecMinIn
                    .LaneRecMinUnload = oItem.LaneRecMinUnload
                    .LaneRecMinOut = oItem.LaneRecMinOut
                    .LaneAppt = oItem.LaneAppt
                    .LanePalletExchange = oItem.LanePalletExchange
                    .LanePalletType = Left(oItem.LanePalletType, 1)
                    .LaneBenchMiles = oItem.LaneBenchMiles
                    .LaneBFC = oItem.LaneBFC
                    .LaneBFCType = Left(oItem.LaneBFCType, 10)
                    If validateDateWS(oItem.LaneRecHourStart, dtVal) Then
                        .LaneRecHourStart = exportDateToString(dtVal.ToString)
                    End If
                    If validateDateWS(oItem.LaneRecHourStop, dtVal) Then
                        .LaneRecHourStop = exportDateToString(dtVal.ToString)
                    End If
                    If validateDateWS(oItem.LaneDestHourStart, dtVal) Then
                        .LaneDestHourStart = exportDateToString(dtVal.ToString)
                    End If
                    If validateDateWS(oItem.LaneDestHourStop, dtVal) Then
                        .LaneDestHourStop = exportDateToString(dtVal.ToString)
                    End If
                    .LaneComments = Left(oItem.LaneComments, 255)
                    .LaneCommentsConfidential = Left(oItem.LaneCommentsConfidential, 255)
                    .LaneLatitude = oItem.LaneLatitude
                    .LaneLongitude = oItem.LaneLongitude
                    .LaneTempType = oItem.LaneTempType
                    .LaneTransType = oItem.LaneTransType
                    .LanePrimaryBuyer = Left(oItem.LanePrimaryBuyer, 50)
                    .LaneAptDelivery = oItem.LaneAptDelivery
                    .BrokerNumber = Left(oItem.BrokerNumber, 50)
                    .BrokerName = Left(oItem.BrokerName, 30)
                    .LaneOriginAddressUse = oItem.LaneOriginAddressUse
                    .LaneCarrierEquipmentCodes = Left(oItem.LaneCarrierEquipmentCodes, 50)
                    .LaneChepGLID = Left(oItem.LaneChepGLID, 50)
                    .LaneCarrierTypeCode = Left(oItem.LaneCarrierTypeCode, 50)
                    .LanePickUpMon = oItem.LanePickUpMon
                    .LanePickUpTue = oItem.LanePickUpTue
                    .LanePickUpWed = oItem.LanePickUpWed
                    .LanePickUpThu = oItem.LanePickUpThu
                    .LanePickUpFri = oItem.LanePickUpFri
                    .LanePickUpSat = oItem.LanePickUpSat
                    .LanePickUpSun = oItem.LanePickUpSun
                    .LaneDropOffMon = oItem.LaneDropOffMon
                    .LaneDropOffTue = oItem.LaneDropOffTue
                    .LaneDropOffWed = oItem.LaneDropOffWed
                    .LaneDropOffThu = oItem.LaneDropOffThu
                    .LaneDropOffFri = oItem.LaneDropOffFri
                    .LaneDropOffSat = oItem.LaneDropOffSat
                    .LaneDropOffSun = oItem.LaneDropOffSun
                End With
                oHTable.AddLaneHeaderRow(oRow)
            Next
            Try
                If Not oCalendars Is Nothing Then
                    For Each oItem As clsLaneCalendarObject In oCalendars
                        Dim oRow As LaneData.LaneCalendarRow = oCTable.NewLaneCalendarRow
                        With oRow
                            .ApplyToOrigin = oItem.ApplyToOrigin
                            .Day = oItem.Day
                            If ValidateTimeWS(oItem.StartTime) Then
                                .StartTime = oItem.StartTime
                            End If
                            .IsHoliday = oItem.IsHoliday
                            .LaneNumber = Left(oItem.LaneNumber, 50)
                            .Month = oItem.Month
                            .Open = oItem.Open
                            If ValidateTimeWS(oItem.EndTime) Then
                                .EndTime = oItem.EndTime
                            End If
                        End With
                        oCTable.AddLaneCalendarRow(oRow)
                    Next
                End If
            Catch ex As Exception
                LogException("Process Object Data Warning (import not affected)", "Invalid or missing lane calendar information", AdminEmail, ex, "NGL.FreightMaster.Integration.clsLane.ProcessObjectData Warning (import not affected).")
            End Try

            intRet = ProcessData(oHTable, strConnection, oCTable)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Lane import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsLane.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function

    Public Function ProcessData(ByVal oLanes As LaneData.LaneHeaderDataTable,
                                         ByVal strConnection As String,
                                         Optional ByVal oCalendar As LaneData.LaneCalendarDataTable = Nothing) As ProcessDataReturnValues


        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim blnOriginUse As Boolean = False
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsLane.ProcessData"
        Dim strHeaderTable As String = "Lane"
        Dim strItemTable As String = ""
        Dim strCalendarTable As String = "LaneCal"
        Me.HeaderName = "Lane"
        Me.CalendarName = "Lane Calendar"
        Me.ImportTypeKey = IntegrationTypes.Lane
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Lane Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        'Hours and days of operation
        Dim oCals As New clsImportFields
        If Not buildCalendarCollection(oCals) Then Return ProcessDataReturnValues.nglDataIntegrationFailure

        Try

            'Import the Header Records
            If importHeaderRecords(oLanes, oFields) Then
                'Now Import the Calendar
                'Check if the calendar file exists
                If Not oCalendar Is Nothing AndAlso oCalendar.Count > 0 Then
                    importCalendarRecords(oCalendar, oCals)
                Else
                    Log("Calendar information was not provided.")
                End If
            End If
            strTitle = "Process Data Complete"

            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Warning", "The following errors or warnings were reported some lane records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Failure", "The following errors or warnings were reported some lane records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If ITNoLaneEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Orders With Missing Lanes Found", "The following book records were found in the No Lane staging table and have now been processed due to updates to the lane data." & ITNoLaneEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalCalendarRecords > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalCalendarRecords & " " & strCalendarTable & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Or Me.CalendarErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    'If Me.ItemErrors > 0 Then
                    '    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the Import Error Report for more information."
                    'End If
                    If Me.CalendarErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.CalendarErrors & " " & Me.CalendarName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            LogException("Process Lane Data Failure", "Could not process the requested lane data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsLane.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet


    End Function


    Public Function ProcessObjectData(ByVal oLanes As List(Of clsLaneObject60),
                                         ByVal strConnection As String,
                                         Optional ByVal oCalendar As List(Of clsLaneCalendarObject60) = Nothing) As ProcessDataReturnValues


        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim blnOriginUse As Boolean = False
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsLane.ProcessData"
        Dim strHeaderTable As String = "Lane"
        Dim strItemTable As String = ""
        Dim strCalendarTable As String = "LaneCal"
        Me.HeaderName = "Lane"
        Me.CalendarName = "Lane Calendar"
        Me.ImportTypeKey = IntegrationTypes.Lane
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Lane Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection60(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        'Hours and days of operation
        Dim oCals As New clsImportFields
        If Not buildCalendarCollection(oCals) Then Return ProcessDataReturnValues.nglDataIntegrationFailure

        Try

            'Import the Header Records
            If importHeaderRecords(oLanes, oFields) Then
                'Now Import the Calendar
                'Check if the calendar file exists
                If Not oCalendar Is Nothing AndAlso oCalendar.Count > 0 Then
                    importCalendarRecords(oCalendar, oCals)
                Else
                    Log("Calendar information was not provided.")
                End If
            End If
            strTitle = "Process Data Complete"

            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Warning", "The following errors or warnings were reported some lane records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Failure", "The following errors or warnings were reported some lane records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If ITNoLaneEmailMsg.Trim.Length > 0 Then
                LogError("Process Lane Data Orders With Missing Lanes Found", "The following book records were found in the No Lane staging table and have now been processed due to updates to the lane data." & ITNoLaneEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalCalendarRecords > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalCalendarRecords & " " & strCalendarTable & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Or Me.CalendarErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    'If Me.ItemErrors > 0 Then
                    '    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the Import Error Report for more information."
                    'End If
                    If Me.CalendarErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.CalendarErrors & " " & Me.CalendarName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            LogException("Process Lane Data Failure", "Could not process the requested lane data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsLane.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet


    End Function

    ''' <summary>
    ''' calculate miles for older versions of TMS web services
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <param name="blnUseKilometers"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 added logic for Trimble API
    '''     we now call CalcMiles80 to get the miles.
    ''' </remarks>
    Protected Function calcMiles(
        ByRef oFields As clsImportFields,
        ByVal blnOriginUse As Boolean,
        ByVal blnUseKilometers As Boolean) As Single

        Dim sglmiles As Single = 0
        'Dim RouteType As Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Route_Type = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_PRACTICAL
        'Dim DistType As Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_MILES
        'If blnUseKilometers Then DistType = Ngl.Service.PCMiler64.INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO
        'If UsePCMiler Then
        If Debug Then Log("Use Origin = " & oFields("LaneOrigZip").Use.ToString & " ; Use Dest = " & oFields("LaneDestZip").Use.ToString)
        Try
            If mblnInsertRecord OrElse (oFields("LaneOrigZip").Use = True _
                        AndAlso oFields("LaneDestZip").Use = True _
                        AndAlso oFields("LaneBenchMiles").Use = False) Then

                'an origin and dest zip code are possible and the LaneBenchMiles 
                'is not provided by the imported record
                'so get the miles using PCMiler
                'Begin Modified by RHR for v-8.4.0.003 on 07/19/2021  we use PCM Address now
                'Dim oOrig As New Ngl.Service.PCMiler64.clsAddress
                'Dim oDest As New Ngl.Service.PCMiler64.clsAddress
                Dim oOrig As New PCM.clsAddress
                Dim oDest As New PCM.clsAddress
                'End Modified by RHR for v-8.4.0.003 on 07/19/2021 
                If mblnInsertRecord OrElse oFields("LaneOrigAddress1").Use Then
                    oOrig.strAddress = oFields("LaneOrigAddress1").Value
                End If
                If mblnInsertRecord OrElse oFields("LaneOrigCity").Use Then
                    oOrig.strCity = oFields("LaneOrigCity").Value
                End If
                If mblnInsertRecord OrElse oFields("LaneOrigState").Use Then
                    oOrig.strState = oFields("LaneOrigState").Value
                End If
                oOrig.strZip = zipClean(oFields("LaneOrigZip").Value)


                If mblnInsertRecord OrElse oFields("LaneDestAddress1").Use Then
                    oDest.strAddress = oFields("LaneDestAddress1").Value
                End If
                If mblnInsertRecord OrElse oFields("LaneDestCity").Use Then
                    oDest.strCity = oFields("LaneDestCity").Value
                End If
                If mblnInsertRecord OrElse oFields("LaneDestState").Use Then
                    oDest.strState = oFields("LaneDestState").Value
                End If
                oDest.strZip = zipClean(oFields("LaneDestZip").Value)
                'Begin Modified by RHR for v-8.4.0.003 on 07/19/2021
                Dim iMiles As Integer = 0
                calcMiles80(oOrig, oDest, iMiles, oFields("LaneNumber").Value, blnOriginUse)
                oFields("LaneBenchMiles").Value = iMiles.ToString()
                oFields("LaneBenchMiles").Use = True
                If Debug Then Log("Miles = " & iMiles.ToString())
                'Removed on 07/19/2021
                'Dim strLastPCMError As String = ""
                'Dim arrBaddAddresses() As Ngl.Service.PCMiler64.clsPCMBadAddress

                'Dim oGlobalStopData As New Ngl.Service.PCMiler64.clsGlobalStopData
                'Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles
                '    If blnOriginUse Then
                '        oGlobalStopData = oPCmiles.getPracticalMiles(oDest, oOrig, RouteType, DistType, 0, 0, 0, oFields("LaneNumber").Value, "Lane Number", 0, 0, True, arrBaddAddresses)
                '    Else
                '        oGlobalStopData = oPCmiles.getPracticalMiles(oOrig, oDest, RouteType, DistType, 0, 0, 0, oFields("LaneNumber").Value, "Lane Number", 0, 0, True, arrBaddAddresses)
                '    End If
                '    strLastPCMError = oPCmiles.LastError
                'End Using
                'If Not oGlobalStopData Is Nothing AndAlso String.IsNullOrEmpty(oGlobalStopData.FailedAddressMessage) Then
                '    oFields("LaneBenchMiles").Value = oGlobalStopData.TotalMiles.ToString
                '    oFields("LaneBenchMiles").Use = True
                '    If Debug Then Log("Miles = " & oGlobalStopData.TotalMiles.ToString)
                'ElseIf Not oGlobalStopData Is Nothing Then
                '    Log("Calc Miles Failure (Import Not Affected): " & oGlobalStopData.FailedAddressMessage)
                'Else
                '    Log("PCMiler Error (Import Not Affected): " & strLastPCMError)
                '    GroupEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calcMiles, Error (Import Not Affected): " & strLastPCMError & ".  The miles for the lane number, " & oFields("LaneNumber").Value & " must be calculated manually.<br />" & vbCrLf
                'End If
                'End Modified by RHR for v-8.4.0.003 on 07/19/2021


            End If
        Catch ex As System.Runtime.Serialization.SerializationException
            'this is thrown if we cannot reference one of the serializeable objects 
            'normally when one or more components are not properly installed
            ITEmailMsg &= "<br />" & Source & " Warning: There was a serialization exception in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & "Check that all components have been properly installed<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calcMiles serialization exception: " & ex.ToString)
        Catch ex As Exception
            GroupEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & ex.Message & ".<br />" & vbCrLf
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calcMiles (import not affected), could not calculate miles for lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calcMiles Unexpected Error: " & ex.ToString)
        End Try

        Return sglmiles


    End Function

    ''' <summary>
    ''' Calc Lat Long for older web servies limited to zip code only 
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 call new CalcLatLong80 method for trimble API
    ''' </remarks>
    Private Sub calcLatLong(
        ByRef oFields As clsImportFields,
        ByVal blnOriginUse As Boolean)

        Dim blnGetGeoCode As Boolean = False
        Dim strlocation As String = ""
        Try
            'Test if a zip code is possible and the 
            'the import flags for lat and long are false
            'we only modify if the import flag is false else
            'the source controls the Lat Long
            If blnOriginUse Then
                If mblnInsertRecord OrElse (oFields("LaneOrigZip").Use = True _
                AndAlso oFields("LaneLatitude").Use = False _
                AndAlso oFields("LaneLongitude").Use = False) Then
                    blnGetGeoCode = True
                    strlocation = zipClean(oFields("LaneOrigZip").Value)
                End If
            ElseIf mblnInsertRecord OrElse (oFields("LaneDestZip").Use = True _
                AndAlso oFields("LaneLatitude").Use = False _
                AndAlso oFields("LaneLongitude").Use = False) Then
                blnGetGeoCode = True
                strlocation = zipClean(oFields("LaneDestZip").Value)
            End If
            If blnGetGeoCode Then
                Dim dblLat As Double = 0
                Dim dblLong As Double = 0
                Dim strPCMLastError As String = ""
                ' Begin Modified by RHR for v-8.4.0.003 on 07/19/2021 call new CalcLatLong80 method for trimble API
                If calcLatLong80(oFields("LaneNumber").Value, dblLat, dblLong, strlocation, strPCMLastError) Then
                    oFields("LaneLatitude").Value = CStr(dblLat)
                    oFields("LaneLatitude").Use = True
                    oFields("LaneLongitude").Value = CStr(dblLong)
                    oFields("LaneLongitude").Use = True
                    If Debug Then Log("Lat/Long = " & dblLat.ToString & "/" & dblLong.ToString)
                End If
                ' End Modified by RHR for v-8.4.0.003 on 07/19/2021 
                'Removed Modified by RHR for v-8.4.0.003 on 07/19/2021
                'If UsePCMiler Then
                '    Dim blnGetLatLong As Boolean

                '    Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles ' Ngl.Service.PCMiler.PCMiles
                '        blnGetLatLong = oPCmiles.getGeoCode(strlocation, dblLat, dblLong)
                '        strPCMLastError = oPCmiles.LastError
                '    End Using

                '    If Not blnGetLatLong Or Not String.IsNullOrEmpty(strPCMLastError) Then
                '        ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsLane.calLatLong (import not affected).  There was a PC Miler error while attempting to calculate lat-long for lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & strPCMLastError & "<br />" & vbCrLf
                '        Log("NGL.FreightMaster.Integration.clsLane.calLatLong ( PC Miler ) Warning!")
                '    Else
                '        oFields("LaneLatitude").Value = CStr(dblLat)
                '        oFields("LaneLatitude").Use = True
                '        oFields("LaneLongitude").Value = CStr(dblLong)
                '        oFields("LaneLongitude").Use = True
                '        If Debug Then Log("Lat/Long = " & dblLat.ToString & "/" & dblLong.ToString)
                '    End If
                'End If
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsLane.calLatLong (import not affected), could not calculate lat-long for lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.calLatLong Unexpected Error: " & ex.ToString)
        End Try

    End Sub
    ''' <summary>
    ''' This version of doesLaneExist has been depreciated as of v-4.7.3 of FreightMaster use new overload below
    ''' </summary>
    ''' <param name="oKeyField"></param>
    ''' <param name="oAltKey"></param>
    ''' <param name="blnOriginUse"></param>
    ''' <param name="blnUseConsigneeNumber"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function doesLaneExist(ByRef oKeyField As clsImportField,
                                        ByRef oAltKey As clsImportField,
                                        ByRef blnOriginUse As Boolean,
                                        ByRef blnUseConsigneeNumber As Boolean,
                                        ByVal strHeaderTable As String) As Integer
        Dim intRet As Integer = -1

        Try
            blnOriginUse = False
            Dim strSQL As String = "Select LaneOriginAddressUse,"
            Dim strValues As String = " Where "
            If Trim(oAltKey.Value).ToString.ToUpper = "NULL" Then
                blnUseConsigneeNumber = False
                strSQL &= oKeyField.Name
                strValues &= oKeyField.Name & " = " & oKeyField.Value
            ElseIf stripQuotes(oAltKey.Value).Trim.Length > 0 Then
                blnUseConsigneeNumber = True
                strSQL &= oAltKey.Name
                strValues &= oAltKey.Name & " = " & oAltKey.Value
            Else
                blnUseConsigneeNumber = False
                strSQL &= oKeyField.Name
                strValues &= oKeyField.Name & " = " & oKeyField.Value
            End If

            strSQL &= " From " & strHeaderTable & strValues
            'If mblnDebug Then
            '    ITEmailMsg &= "<br />" & Source & " Information: NGL.FreightMaster.Integration.clsLane.doeslaneexist sql query <hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
            'End If

            Dim intRetryCt As Integer = 0
            Do
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                Dim drTemp As SqlDataReader
                intRetryCt += 1
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With

                        If drTemp.HasRows Then
                            drTemp.Read()
                            blnOriginUse = CBool(nz(drTemp.Item("LaneOriginAddressUse"), 0))
                            drTemp.Close()
                            Return 1
                        Else
                            blnOriginUse = False
                            Return 0
                        End If
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oKeyField.Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, attempted to validate Lane number " & oKeyField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Catch ex As Exception

                    End Try
                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, could not process Lane number " & oKeyField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
        End Try
        Return intRet
    End Function

    ''' <summary>
    ''' Return empty string or lane number if found
    ''' </summary>
    ''' <param name="OrigAdd"></param>
    ''' <param name="DestAdd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 5/11/2017 for v-7.0.6.105
    '''   returns a lane number using the address information and inbound flag
    '''   or an empty string if the lane does not exist
    ''' Modified by RHR for v-7.0.6.105 on 6/14/2017
    '''   added default comp numbers for inbound and outbound lanes
    ''' </remarks>
    Public Function doesLaneExist(ByVal OrigAdd As clsAddressInfo, ByVal DestAdd As clsAddressInfo, ByVal DefaultOutboundCompNumber As Integer, ByVal DefaultInboundCompNumber As Integer, Optional ByVal Inbound As Boolean = False) As String
        Dim strLaneNumber As String = ""

        Dim sbWhere As New System.Text.StringBuilder()
        Dim sCompNumber As String = "0"
        Dim intLaneOriginAddressUse As String = 0
        If Inbound Then
            intLaneOriginAddressUse = 1
            If DefaultInboundCompNumber = 0 Then
                sCompNumber = If(String.IsNullOrWhiteSpace(DestAdd.CompNumber) = True, "0", DestAdd.CompNumber)
            Else
                sCompNumber = DefaultInboundCompNumber.ToString()
            End If
        Else
            If DefaultOutboundCompNumber = 0 Then
                sCompNumber = If(String.IsNullOrWhiteSpace(OrigAdd.CompNumber) = True, "0", OrigAdd.CompNumber)
            Else
                sCompNumber = DefaultOutboundCompNumber.ToString()
            End If
        End If

        sbWhere.Append(String.Format(" LaneCompControl = (Select top 1 compcontrol from comp where compnumber = '{0}')", sCompNumber))
        sbWhere.Append(String.Format(" And LaneOriginAddressUse = {0}", intLaneOriginAddressUse))
        sbWhere.Append(String.Format(" And LaneOrigAddress1 = '{0}'", OrigAdd.Addr1))
        sbWhere.Append(String.Format(" And LaneOrigCity = '{0}'", OrigAdd.City))
        sbWhere.Append(String.Format(" And LaneOrigState = '{0}'", OrigAdd.State))
        sbWhere.Append(String.Format(" And LaneOrigZip = '{0}'", OrigAdd.Zip))
        sbWhere.Append(String.Format(" And LaneDestAddress1 = '{0}'", DestAdd.Addr1))
        sbWhere.Append(String.Format(" And LaneDestCity = '{0}'", DestAdd.City))
        sbWhere.Append(String.Format(" And LaneDestState = '{0}'", DestAdd.State))
        sbWhere.Append(String.Format(" And LaneDestZip = '{0}'", DestAdd.Zip))

        Dim strSQL As String = String.Format("SELECT Top 1 LaneNumber FROM dbo.Lane WHERE {0} ", sbWhere.ToString())

        Using conn As New SqlConnection(Me.ConnectionString)
            Dim cmd As New SqlCommand(strSQL, conn)
            cmd.CommandTimeout = 3000
            cmd.CommandType = CommandType.Text
            Try
                conn.Open()
                strLaneNumber = cmd.ExecuteScalar()
            Catch ex As Exception
                ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, attempted to validate Lane number without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
            End Try
        End Using
        Return strLaneNumber
    End Function

    ''' <summary>
    ''' Generates a new Lane nunmber using a sequence number in position 2 of 4
    ''' Like 1-2-3-4 where 1 = NGLComp# 2 = Sequence# 3 = CustOrVend# 4 = Warehouse#
    ''' </summary>
    ''' <param name="sCompanyNumber"></param>
    ''' <param name="sCustLocationID"></param>
    ''' <param name="sCompanyLocationID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/11/2017
    ''' </remarks>
    Public Function getNextLaneNumberInSequence(ByVal sCompanyNumber As String, ByVal sCustLocationID As String, ByVal sCompanyLocationID As String) As String
        Dim strLaneNumber As String = ""
        Dim strWhere As String = String.Format("LaneNumber Like '{0}-%-{1}-{2}'", sCompanyNumber, sCustLocationID, sCompanyLocationID)
        Dim strLaneNumbers As New List(Of String)

        Dim strSQL As String = String.Format("SELECT Distinct LaneNumber FROM dbo.Lane WHERE {0} ", strWhere)

        Using conn As New SqlConnection(Me.ConnectionString)
            Dim cmd As New SqlCommand(strSQL, conn)
            cmd.CommandTimeout = 3000
            cmd.CommandType = CommandType.Text
            Try
                conn.Open()
                Using RDR = cmd.ExecuteReader()
                    If RDR.HasRows Then
                        Do While RDR.Read
                            strLaneNumbers.Add(RDR.Item("LaneNumber").ToString())
                        Loop
                    End If
                End Using
            Catch ex As Exception
                ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, attempted to validate Lane number without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
            End Try
        End Using
        Dim intNextSequence As Integer = 0
        If Not strLaneNumbers Is Nothing AndAlso strLaneNumbers.Count() > 0 Then
            For Each nbr In strLaneNumbers
                Dim sElems() As String = nbr.Split("-")
                If Not sElems Is Nothing AndAlso sElems.Count > 1 Then
                    Dim strNextSequence = sElems(1)
                    Dim intThisSequence As Integer = 0
                    If Integer.TryParse(strNextSequence, intThisSequence) Then
                        If intThisSequence > intNextSequence Then intNextSequence = intThisSequence
                    End If
                End If
            Next
        End If
        strLaneNumber = String.Format("{0}-{1}-{2}-{3}", sCompanyNumber, intNextSequence, sCustLocationID, sCompanyLocationID)
        Return strLaneNumber
    End Function

    ''' <summary>
    ''' This new overloaded form of doesLaneExist replaces the deprecated version (above)
    ''' </summary>
    ''' <param name="oKeyField"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function doesLaneExist(ByRef oKeyField As clsImportField, ByVal strHeaderTable As String) As Integer
        Dim intRet As Integer = -1

        Try
            Dim strSQL As String = String.Format("SELECT Top 1 LaneControl FROM {0} WHERE {1} = {2}", strHeaderTable, oKeyField.Name, oKeyField.Value)

            Dim intRetryCt As Integer = 0
            Do
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                intRetryCt += 1
                Dim intLaneControl As Integer = 0
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            intLaneControl = .ExecuteScalar
                        End With
                        Return intLaneControl
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oKeyField.Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, attempted to validate Lane number " & oKeyField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally

                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, could not process Lane number " & oKeyField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
        End Try
        Return intRet
    End Function

    Public Function doesLaneExist(ByVal sLaneNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Try
            Dim strSQL As String = String.Format("SELECT Count(*) FROM {0} WHERE {1} = '{2}'", "dbo.Lane", "LaneNumber", sLaneNumber)

            Dim intCount As Integer = 0
            'check the active db connection
            If Me.openConnection Then
                With cmdObj
                    .Connection = DBCon
                    .CommandTimeout = 300
                    .CommandText = strSQL
                    .CommandType = CommandType.Text
                    intCount = .ExecuteScalar
                End With
                If intCount <> 0 Then blnRet = True
            End If
        Catch ex As Exception
            'do nothing
        Finally

            Try
                cmdObj.Cancel()
            Catch ex As Exception

            End Try
            Me.closeConnection()
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' This new overloaded form of doesLaneExist replaces the depreciated version (above)
    ''' </summary>
    ''' <param name="oKeyField"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isAddressTheSame(ByRef oFields As clsImportFields, ByRef oKeyField As clsImportField, ByVal strHeaderTable As String) As Integer
        Dim intRet As Integer = -1

        Try
            Dim strSQL As String = String.Format("SELECT top 1 LaneControl FROM {0} WHERE {1} = {2}", strHeaderTable, oKeyField.Name, oKeyField.Value)
            strSQL &= " AND (LaneOrigAddress1 = " & oFields("LaneOrigAddress1").Value
            strSQL &= " AND LaneOrigCity = " & oFields("LaneOrigCity").Value
            strSQL &= " AND LaneOrigState = " & oFields("LaneOrigState").Value
            strSQL &= " AND LaneOrigZip = " & oFields("LaneOrigZip").Value
            strSQL &= " AND LaneDestAddress1 = " & oFields("LaneDestAddress1").Value
            strSQL &= " AND LaneDestCity = " & oFields("LaneDestCity").Value
            strSQL &= " AND LaneDestState = " & oFields("LaneDestState").Value
            strSQL &= " AND LaneDestZip = " & oFields("LaneDestZip").Value & ") "
            Dim intRetryCt As Integer = 0
            Do
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                intRetryCt += 1
                Dim intLaneControl As Integer = 0
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            intLaneControl = .ExecuteScalar
                        End With
                        Return intLaneControl
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsLane.isAddressNew: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane address could not be validated correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.isAddressNew Warning (Import Not Affected)!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.isAddressNew Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.isAddressNew, attempted to validate Lane address " & oKeyField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.isAddressNew Warning (Import Not Affected)!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.isAddressNew Warning Retry = " & intRetryCt.ToString)
                    End If
                Finally

                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.doesLaneExist, could not process Lane number " & oKeyField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.doesLaneExist Failed!" & readExceptionMessage(ex))
        End Try
        Return intRet
    End Function
    ''' <summary>
    ''' This version of saveLaneData has been depreciated and is no longer value use the new varsion below as of v-4.7.3
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="blnInsertRecord"></param>
    ''' <param name="blnUseConsigneeNumber"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function saveLaneData(ByRef oFields As clsImportFields,
                                        ByVal blnInsertRecord As Boolean,
                                        ByVal blnUseConsigneeNumber As Boolean,
                                        ByVal strHeaderTable As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Try
            If blnInsertRecord Then
                'build execute string to insert record into Header table
                strSQL = "Insert Into " & strHeaderTable & " ("
                strValues = " Values ( "
                blnFirstField = True
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Name <> "BrokerName" _
                        And oFields(intct).Name <> "BrokerNumber" Then
                        'And oFields(intct).Use Then
                        'Note on insert new lane we no longer check the .use flag we insert all data provided except for 
                        'LaneBenchMiles, LaneLatitude Or LaneLongitude
                        If (oFields(intct).Name = "LaneBenchMiles" _
                            Or oFields(intct).Name = "LaneLatitude" _
                            Or oFields(intct).Name = "LaneLongitude") _
                            And oFields(intct).Use = False Then
                            'skip this field
                        Else
                            If blnFirstField Then
                                blnFirstField = False
                            Else
                                strSQL &= " , "
                                strValues &= " , "
                            End If
                            strSQL &= oFields(intct).Name
                            strValues &= oFields(intct).Value
                        End If
                    End If
                Next
                strSQL &= ",LaneModUser,LaneModDate "
                strValues &= ",'System Download','" & mstrCreatedDate & "'"
                strSQL = strSQL & " ) " & strValues & " ) "
                'debug.print " Insert Header SQL = " & strSQL
            Else
                'build sql string to update current record
                strSQL = "Update " & strHeaderTable & " Set "
                strValues = " Where "
                blnFirstField = True
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Name <> "BrokerName" _
                            And oFields(intct).Name <> "BrokerNumber" _
                            And oFields(intct).Use Then
                        If blnFirstField Then
                            blnFirstField = False
                        Else
                            strSQL &= " , "
                        End If
                        If blnUseConsigneeNumber Then
                            If oFields(intct).Key = "LaneConsigneeNumber" Then
                                strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                            End If
                        Else
                            If oFields(intct).Key = "LaneNumber" Then
                                strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                            End If
                        End If
                        strSQL &= oFields(intct).Name & " = " & oFields(intct).Value
                    End If
                Next
                strSQL &= ", LaneModUser = 'System Update', LaneModDate  = '" & mstrCreatedDate & "'"
                strSQL &= " From " & strHeaderTable & strValues
                'Debug.Print " Update Header SQL = " & strSQL
            End If
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Try
                    'check the active db connection
                    If Me.openConnection() Then
                        cmd = New SqlCommand(strSQL, Me.DBCon)
                        cmd.ExecuteNonQuery()
                        processNoLanePOs(oFields("LaneNumber").Value)
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveLaneData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oFields("LaneNumber").Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.saveLaneData Failed!")
                        Else
                            Log("saveLaneData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As System.Data.SqlClient.SqlException
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveLaneData, attempted to save Lane number " & oFields("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.saveLaneData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("saveLaneData Failure Retry = " & intRetryCt.ToString)
                    End If
                Catch ex As Exception
                    Throw
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsLane.saveLaneData: Could not write to  Lane table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    ''' <summary>
    ''' This overloaded version of saveLaneData replaces the previous version as of FreightMaster v-4.7.3.  The blnUseConsigneeNumber flag is no longer used.
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="blnInsertRecord"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function saveLaneData(ByRef oFields As clsImportFields,
                                        ByVal blnInsertRecord As Boolean,
                                        ByVal strHeaderTable As String) As Boolean
        Dim Ret As Boolean = False
        Dim sbSQL As New System.Text.StringBuilder(2500)
        Dim sbValues As New System.Text.StringBuilder(2500)
        Dim strFieldSeperator As String = ""
        Try
            If blnInsertRecord Then
                'build execute string to insert record into Header table
                sbSQL.AppendFormat("Insert Into {0} (", strHeaderTable)
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Name <> "BrokerName" _
                        And oFields(intct).Name <> "BrokerNumber" Then
                        'And oFields(intct).Use Then
                        'Note on insert new lane we no longer check the .use flag we insert all data provided except for 
                        'LaneBenchMiles, LaneLatitude Or LaneLongitude
                        If Not ((oFields(intct).Name = "LaneBenchMiles" _
                            Or oFields(intct).Name = "LaneLatitude" _
                            Or oFields(intct).Name = "LaneLongitude") _
                            And oFields(intct).Use = False) Then
                            sbSQL.AppendFormat("{0}{1}", strFieldSeperator, oFields(intct).Name)
                            sbValues.AppendFormat("{0}{1}", strFieldSeperator, oFields(intct).Value)
                            strFieldSeperator = ","
                        End If
                        'Modified by RHR the above code does not insert miles and lat long in new lanes 
                        'this is not acceptable and must be reversed.
                    End If
                Next
                sbValues.AppendFormat(",'System Download','{0}'", mstrCreatedDate)
                sbSQL.AppendFormat(",LaneModUser,LaneModDate ) Values ({0})", sbValues.ToString)
                'debug.print " Insert Header SQL = " & sbSQL.tostring
            Else
                'build sql string to update current record
                sbSQL.AppendFormat("Update {0} Set ", strHeaderTable)
                For intct As Integer = 1 To oFields.Count
                    If oFields(intct).Name <> "BrokerName" _
                            And oFields(intct).Name <> "BrokerNumber" _
                            And oFields(intct).Use Then
                        If oFields(intct).Key = "LaneNumber" Then
                            sbValues.AppendFormat("{0} = {1}", oFields(intct).Name, oFields(intct).Value)
                        End If
                        sbSQL.AppendFormat("{0}{1} = {2}", strFieldSeperator, oFields(intct).Name, oFields(intct).Value)
                        strFieldSeperator = ","
                    End If
                Next
                sbSQL.AppendFormat(", LaneModUser = 'System Update', LaneModDate  = '{0}' From {1} Where {2}", mstrCreatedDate, strHeaderTable, sbValues.ToString)
            End If
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Try
                    'check the active db connection
                    If Me.openConnection() Then
                        cmd = New SqlCommand(sbSQL.ToString, Me.DBCon)
                        cmd.ExecuteNonQuery()
                        processNoLanePOs(oFields("LaneNumber").Value)
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveLaneData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oFields("LaneNumber").Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.saveLaneData Failed!")
                        Else
                            Log("saveLaneData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As System.Data.SqlClient.SqlException
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveLaneData, attempted to save Lane number " & oFields("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & sbSQL.ToString & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.saveLaneData Failed!" & readExceptionMessage(ex))
                        Exit Do
                    Else
                        Log("saveLaneData Failure Retry = " & intRetryCt.ToString)
                    End If

                Catch ex As Exception
                    Throw
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsLane.saveLaneData: Could not write to  Lane table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function


    Private Function saveBrokerData(ByRef oFields As clsImportFields,
                                        ByVal intLaneControl As Integer) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Try
            'build execute string to insert record as default
            strSQL = "Insert Into LaneSec (LaneSecBrokerNumber,LaneSecBrokerName,LaneSecLaneControl)"
            strSQL &= " Values (" & oFields("BrokerNumber").Value & "," & oFields("BrokerName").Value & "," & intLaneControl & ")"
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                Dim drTemp As SqlDataReader
                Try
                    'check the active db connection
                    If Me.openConnection() Then

                        'test if a lanesec file exists
                        Dim strQry As String = "Select LaneSecControl From LaneSec Where LaneSecLaneControl = " & intLaneControl
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strQry
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With

                        If drTemp.HasRows Then
                            strSQL = "Update LaneSec Set LaneSecBrokerNumber = " & oFields("BrokerNumber").Value
                            strSQL &= " ,LaneSecBrokerName = " & oFields("BrokerName").Value
                            strSQL = strSQL & " Where LaneSecLaneControl = " & intLaneControl
                        End If
                        Try
                            drTemp.Close()
                        Catch ex As Exception

                        End Try
                        cmdObj.CommandText = strSQL
                        cmdObj.ExecuteScalar()
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveBrokerData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oFields("LaneNumber").Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.saveBrokerData Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.saveBrokerData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveBrokerData, attempted to save broker data for Lane number " & oFields("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.saveBrokerData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.saveBrokerData Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Catch ex As Exception

                    End Try
                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveBrokerData, could not save broker data for Lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.saveBrokerData Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    Private Function UpdateRouteGuideControl(ByRef oFields As clsImportFields,
                                        ByVal intLaneControl As Integer) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""

        Try
            'build execute string 
            strSQL = "exec dbo.spUpdateLaneRouteGuideControl " & intLaneControl.ToString

            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                Try
                    'check the active db connection
                    If Me.openConnection() Then
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            .ExecuteNonQuery()
                        End With
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oFields("LaneNumber").Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl, attempted to save broker data for Lane number " & oFields("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally

                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl, could not save broker data for Lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    ''' <summary>
    ''' Executes the stored procedure spUpdateLaneRouteGuideControl passing intLaneControl as the parameter
    ''' </summary>
    ''' <param name="LaneNumber"></param>
    ''' <param name="intLaneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateRouteGuideControl70(ByVal LaneNumber As String, ByVal intLaneControl As Integer) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""

        Try
            'build execute string 
            strSQL = "exec dbo.spUpdateLaneRouteGuideControl " & intLaneControl.ToString

            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                Try
                    'check the active db connection
                    If Me.openConnection() Then
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            .ExecuteNonQuery()
                        End With
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & LaneNumber & " could not be processed correctly.<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl, attempted to save broker data for Lane number " & LaneNumber & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally

                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl, could not save broker data for Lane number " & LaneNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.UpdateRouteGuideControl Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function


    ''' <summary>
    ''' This method splits and verifies the special codes and saves them to the database. It is currently turned off because of issues with the 
    ''' Extended stored procedures.  
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="intLaneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function saveSpecialCodes(ByRef oFields As clsImportFields,
                                        ByVal intLaneControl As Integer) As Boolean
        Return True

        'Dim Ret As Boolean = False
        'Dim strSQL As String = ""
        'Dim strValues As String = ""
        'Dim blnFirstField As Boolean = True
        'Try
        '    'if special codes exist add them to the database we ignore any errors or messages 
        '    'returned by the stored procedure durring this process
        '    'Test code to add special codes
        '    'oFields("LaneCarrierEquipmentCodes").Value = "CC-FR"
        '    If Len(Trim(oFields("LaneCarrierEquipmentCodes").Value)) > 1 AndAlso oFields("LaneCarrierEquipmentCodes").Value <> "NULL" Then
        '        strSQL = "DECLARE @Value float " _
        '            & " DECLARE @Message nvarchar(4000) " _
        '            & " EXECUTE [dbo].[nglspUpdateLaneCodesTable] " _
        '            & oFields("LaneCarrierEquipmentCodes").Value _
        '            & "," & intLaneControl.ToString _
        '            & " ,@Value OUTPUT " _
        '            & " ,@Message OUTPUT"

        '        Dim intRetryCt As Integer = 0
        '        Do
        '            intRetryCt += 1
        '            Dim cmdObj As New System.Data.SqlClient.SqlCommand
        '            Try
        '                'check the active db connection
        '                If Me.openConnection() Then
        '                    cmdObj.Connection = DBCon
        '                    cmdObj.CommandText = strSQL
        '                    cmdObj.ExecuteNonQuery()
        '                    Ret = True
        '                    Exit Do
        '                Else
        '                    If intRetryCt > Me.Retry Then
        '                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveSpecialCodes: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oFields("LaneNumber").Value & " could not be processed correctly.<br />" & vbCrLf
        '                        Log("NGL.FreightMaster.Integration.clsLane.saveSpecialCodes Failed!")
        '                    Else
        '                        Log("NGL.FreightMaster.Integration.clsLane.saveSpecialCodes Open DB Connection Failure Retry = " & intRetryCt.ToString)
        '                    End If
        '                End If
        '            Catch ex As Exception
        '                If intRetryCt > Me.Retry Then
        '                    ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveSpecialCodes, attempted to save special codes data for Lane number " & oFields("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
        '                    Log("NGL.FreightMaster.Integration.clsLane.saveSpecialCodes Failed!" & readExceptionMessage(ex))
        '                Else
        '                    Log("NGL.FreightMaster.Integration.clsLane.saveSpecialCodes Failure Retry = " & intRetryCt.ToString)
        '                End If
        '            Finally
        '                Try
        '                    cmdObj.Cancel()
        '                Catch ex As Exception

        '                End Try
        '            End Try
        '            'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
        '        Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        '    End If
        'Catch ex As Exception
        '    ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.saveSpecialCodes, could not save special codes data for Lane number " & oFields("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
        '    Log("NGL.FreightMaster.Integration.clsLane.saveSpecialCodes Failed!" & readExceptionMessage(ex))
        'End Try
        'Return Ret
    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("LaneNumber", "LaneNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '0
                .Add("LaneName", "LaneName", clsImportField.DataTypeID.gcvdtString, 50, False) '1
                .Add("LaneNumberMaster", "LaneNumberMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '2
                .Add("LaneNameMaster", "LaneNameMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '3
                .Add("LaneCompNumber", "LaneCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '4
                .Add("LaneDefaultCarrierUse", "LaneDefaultCarrierUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '5
                .Add("LaneDefaultCarrierNumber", "LaneDefaultCarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '6
                .Add("LaneOrigCompNumber", "LaneOrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '7
                .Add("LaneOrigName", "LaneOrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '8
                .Add("LaneOrigAddress1", "LaneOrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '9
                .Add("LaneOrigAddress2", "LaneOrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '10
                .Add("LaneOrigAddress3", "LaneOrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '11
                .Add("LaneOrigCity", "LaneOrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '12
                .Add("LaneOrigState", "LaneOrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '13
                .Add("LaneOrigCountry", "LaneOrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '14
                .Add("LaneOrigZip", "LaneOrigZip", clsImportField.DataTypeID.gcvdtString, 50, True) '15
                .Add("LaneOrigContactPhone", "LaneOrigContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '16  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneOrigContactPhoneExt", "LaneOrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '17
                .Add("LaneOrigContactFax", "LaneOrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '18
                .Add("LaneDestCompNumber", "LaneDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '19
                .Add("LaneDestName", "LaneDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '20
                .Add("LaneDestAddress1", "LaneDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '21
                .Add("LaneDestAddress2", "LaneDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '22
                .Add("LaneDestAddress3", "LaneDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '23
                .Add("LaneDestCity", "LaneDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '24
                .Add("LaneDestState", "LaneDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '25
                .Add("LaneDestCountry", "LaneDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '26
                .Add("LaneDestZip", "LaneDestZip", clsImportField.DataTypeID.gcvdtString, 20, True) '27 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneDestContactPhone", "LaneDestContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '28 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneDestContactPhoneExt", "LaneDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '29
                .Add("LaneDestContactFax", "LaneDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '30
                .Add("LaneConsigneeNumber", "LaneConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '31
                .Add("LaneRecMinIn", "LaneRecMinIn", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '32
                .Add("LaneRecMinUnload", "LaneRecMinUnload", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '33
                .Add("LaneRecMinOut", "LaneRecMinOut", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '34
                .Add("LaneAppt", "LaneAppt", clsImportField.DataTypeID.gcvdtBit, 2, True) '35
                .Add("LanePalletExchange", "LanePalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '36
                .Add("LanePalletType", "LanePalletType", clsImportField.DataTypeID.gcvdtString, 1, True) '37
                .Add("LaneBenchMiles", "LaneBenchMiles", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '38
                .Add("LaneBFC", "LaneBFC", clsImportField.DataTypeID.gcvdtFloat, 11, True) '39
                .Add("LaneBFCType", "LaneBFCType", clsImportField.DataTypeID.gcvdtString, 10, True) '40
                .Add("LaneRecHourStart", "LaneRecHourStart", clsImportField.DataTypeID.gcvdtDate, 22, True) '41
                .Add("LaneRecHourStop", "LaneRecHourStop", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("LaneDestHourStart", "LaneDestHourStart", clsImportField.DataTypeID.gcvdtDate, 22, True) '43
                .Add("LaneDestHourStop", "LaneDestHourStop", clsImportField.DataTypeID.gcvdtDate, 22, True) '44
                .Add("LaneComments", "LaneComments", clsImportField.DataTypeID.gcvdtString, 255, True) '45
                .Add("LaneCommentsConfidential", "LaneCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '46
                .Add("LaneLatitude", "LaneLatitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '47
                .Add("LaneLongitude", "LaneLongitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '48
                .Add("LaneTempType", "LaneTempType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '49
                .Add("LaneTransType", "LaneTransType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '50
                .Add("LanePrimaryBuyer", "LanePrimaryBuyer", clsImportField.DataTypeID.gcvdtString, 50, True) '51
                .Add("LaneAptDelivery", "LaneAptDelivery", clsImportField.DataTypeID.gcvdtBit, 2, True) '52
                .Add("BrokerNumber", "BrokerNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '53
                .Add("BrokerName", "BrokerName", clsImportField.DataTypeID.gcvdtString, 30, True) '54
                .Add("LaneOriginAddressUse", "LaneOriginAddressUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '55
                .Add("LaneCarrierEquipmentCodes", "LaneCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '56
                .Add("LaneChepGLID", "LaneChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '57
                .Add("LaneCarrierTypeCode", "LaneCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '58
                .Add("LanePickUpMon", "LanePickUpMon", clsImportField.DataTypeID.gcvdtBit, 2, False) '59
                .Add("LanePickUpTue", "LanePickUpTue", clsImportField.DataTypeID.gcvdtBit, 2, False) '60
                .Add("LanePickUpWed", "LanePickUpWed", clsImportField.DataTypeID.gcvdtBit, 2, False) '61
                .Add("LanePickUpThu", "LanePickUpThu", clsImportField.DataTypeID.gcvdtBit, 2, False) '62
                .Add("LanePickUpFri", "LanePickUpFri", clsImportField.DataTypeID.gcvdtBit, 2, False) '63
                .Add("LanePickUpSat", "LanePickUpSat", clsImportField.DataTypeID.gcvdtBit, 2, False) '64
                .Add("LanePickUpSun", "LanePickUpSun", clsImportField.DataTypeID.gcvdtBit, 2, False) '65
                .Add("LaneDropOffMon", "LaneDropOffMon", clsImportField.DataTypeID.gcvdtBit, 2, False) '66
                .Add("LaneDropOffTue", "LaneDropOffTue", clsImportField.DataTypeID.gcvdtBit, 2, False) '67
                .Add("LaneDropOffWed", "LaneDropOffWed", clsImportField.DataTypeID.gcvdtBit, 2, False) '68
                .Add("LaneDropOffThu", "LaneDropOffThu", clsImportField.DataTypeID.gcvdtBit, 2, False) '69
                .Add("LaneDropOffFri", "LaneDropOffFri", clsImportField.DataTypeID.gcvdtBit, 2, False) '70
                .Add("LaneDropOffSat", "LaneDropOffSat", clsImportField.DataTypeID.gcvdtBit, 2, False) '71
                .Add("LaneDropOffSun", "LaneDropOffSun", clsImportField.DataTypeID.gcvdtBit, 2, False) '72
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "LaneNumber" Or oFields(ct).Name = "CarrierEquipmentCodes" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Lane)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection60(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("LaneNumber", "LaneNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '0
                .Add("LaneName", "LaneName", clsImportField.DataTypeID.gcvdtString, 50, False) '1
                .Add("LaneNumberMaster", "LaneNumberMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '2
                .Add("LaneNameMaster", "LaneNameMaster", clsImportField.DataTypeID.gcvdtString, 50, True) '3
                .Add("LaneCompNumber", "LaneCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '4
                .Add("LaneDefaultCarrierUse", "LaneDefaultCarrierUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '5
                .Add("LaneDefaultCarrierNumber", "LaneDefaultCarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '6
                .Add("LaneOrigCompNumber", "LaneOrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '7
                .Add("LaneOrigName", "LaneOrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '8
                .Add("LaneOrigAddress1", "LaneOrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '9
                .Add("LaneOrigAddress2", "LaneOrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '10
                .Add("LaneOrigAddress3", "LaneOrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '11
                .Add("LaneOrigCity", "LaneOrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '12
                .Add("LaneOrigState", "LaneOrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '13
                .Add("LaneOrigCountry", "LaneOrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '14
                .Add("LaneOrigZip", "LaneOrigZip", clsImportField.DataTypeID.gcvdtString, 50, True) '15
                .Add("LaneOrigContactPhone", "LaneOrigContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '16 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneOrigContactPhoneExt", "LaneOrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '17 
                .Add("LaneOrigContactFax", "LaneOrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '18
                .Add("LaneDestCompNumber", "LaneDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '19
                .Add("LaneDestName", "LaneDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '20
                .Add("LaneDestAddress1", "LaneDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '21
                .Add("LaneDestAddress2", "LaneDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '22
                .Add("LaneDestAddress3", "LaneDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '23
                .Add("LaneDestCity", "LaneDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '24
                .Add("LaneDestState", "LaneDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '25
                .Add("LaneDestCountry", "LaneDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '26
                .Add("LaneDestZip", "LaneDestZip", clsImportField.DataTypeID.gcvdtString, 20, True) '27 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneDestContactPhone", "LaneDestContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '28 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("LaneDestContactPhoneExt", "LaneDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '29
                .Add("LaneDestContactFax", "LaneDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '30
                .Add("LaneConsigneeNumber", "LaneConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '31
                .Add("LaneRecMinIn", "LaneRecMinIn", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '32
                .Add("LaneRecMinUnload", "LaneRecMinUnload", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '33
                .Add("LaneRecMinOut", "LaneRecMinOut", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '34
                .Add("LaneAppt", "LaneAppt", clsImportField.DataTypeID.gcvdtBit, 2, True) '35
                .Add("LanePalletExchange", "LanePalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '36
                .Add("LanePalletType", "LanePalletType", clsImportField.DataTypeID.gcvdtString, 1, True) '37
                .Add("LaneBenchMiles", "LaneBenchMiles", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '38
                .Add("LaneBFC", "LaneBFC", clsImportField.DataTypeID.gcvdtFloat, 11, True) '39
                .Add("LaneBFCType", "LaneBFCType", clsImportField.DataTypeID.gcvdtString, 10, True) '40
                .Add("LaneRecHourStart", "LaneRecHourStart", clsImportField.DataTypeID.gcvdtDate, 22, True) '41
                .Add("LaneRecHourStop", "LaneRecHourStop", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("LaneDestHourStart", "LaneDestHourStart", clsImportField.DataTypeID.gcvdtDate, 22, True) '43
                .Add("LaneDestHourStop", "LaneDestHourStop", clsImportField.DataTypeID.gcvdtDate, 22, True) '44
                .Add("LaneComments", "LaneComments", clsImportField.DataTypeID.gcvdtString, 255, True) '45
                .Add("LaneCommentsConfidential", "LaneCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '46
                .Add("LaneLatitude", "LaneLatitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '47
                .Add("LaneLongitude", "LaneLongitude", clsImportField.DataTypeID.gcvdtFloat, 11, True) '48
                .Add("LaneTempType", "LaneTempType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '49
                .Add("LaneTransType", "LaneTransType", clsImportField.DataTypeID.gcvdtSmallInt, 6, True) '50
                .Add("LanePrimaryBuyer", "LanePrimaryBuyer", clsImportField.DataTypeID.gcvdtString, 50, True) '51
                .Add("LaneAptDelivery", "LaneAptDelivery", clsImportField.DataTypeID.gcvdtBit, 2, True) '52
                .Add("BrokerNumber", "BrokerNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '53
                .Add("BrokerName", "BrokerName", clsImportField.DataTypeID.gcvdtString, 30, True) '54
                .Add("LaneOriginAddressUse", "LaneOriginAddressUse", clsImportField.DataTypeID.gcvdtBit, 2, True) '55
                .Add("LaneCarrierEquipmentCodes", "LaneCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '56
                .Add("LaneChepGLID", "LaneChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '57
                .Add("LaneCarrierTypeCode", "LaneCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '58
                .Add("LanePickUpMon", "LanePickUpMon", clsImportField.DataTypeID.gcvdtBit, 2, False) '59
                .Add("LanePickUpTue", "LanePickUpTue", clsImportField.DataTypeID.gcvdtBit, 2, False) '60
                .Add("LanePickUpWed", "LanePickUpWed", clsImportField.DataTypeID.gcvdtBit, 2, False) '61
                .Add("LanePickUpThu", "LanePickUpThu", clsImportField.DataTypeID.gcvdtBit, 2, False) '62
                .Add("LanePickUpFri", "LanePickUpFri", clsImportField.DataTypeID.gcvdtBit, 2, False) '63
                .Add("LanePickUpSat", "LanePickUpSat", clsImportField.DataTypeID.gcvdtBit, 2, False) '64
                .Add("LanePickUpSun", "LanePickUpSun", clsImportField.DataTypeID.gcvdtBit, 2, False) '65
                .Add("LaneDropOffMon", "LaneDropOffMon", clsImportField.DataTypeID.gcvdtBit, 2, False) '66
                .Add("LaneDropOffTue", "LaneDropOffTue", clsImportField.DataTypeID.gcvdtBit, 2, False) '67
                .Add("LaneDropOffWed", "LaneDropOffWed", clsImportField.DataTypeID.gcvdtBit, 2, False) '68
                .Add("LaneDropOffThu", "LaneDropOffThu", clsImportField.DataTypeID.gcvdtBit, 2, False) '69
                .Add("LaneDropOffFri", "LaneDropOffFri", clsImportField.DataTypeID.gcvdtBit, 2, False) '70
                .Add("LaneDropOffSat", "LaneDropOffSat", clsImportField.DataTypeID.gcvdtBit, 2, False) '71
                .Add("LaneDropOffSun", "LaneDropOffSun", clsImportField.DataTypeID.gcvdtBit, 2, False) '72
                'New Fields added to v-5.2
                .Add("LaneDefaultRouteSequence", "LaneDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
                .Add("LaneRouteGuideNumber", "LaneRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74
                .Add("LaneIsCrossDockFacility", "LaneIsCrossDockFacility", clsImportField.DataTypeID.gcvdtBit, 2, True) '75
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "LaneNumber" Or oFields(ct).Name = "CarrierEquipmentCodes" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Lane)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildCalendarCollection(ByRef oCalendar As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oCalendar
                .Add("LaneCalLaneControl", "LaneCalLaneControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "LaneNumber", "LaneControl")
                .Add("LaneNumber", "LaneNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("Month", "LaneCalMonth", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("Day", "LaneCalDay", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("Open", "LaneCalOpen", clsImportField.DataTypeID.gcvdtBit, 2, False)
                .Add("StartTime", "LaneCalStartTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("EndTime", "LaneCalEndTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("IsHoliday", "LaneCalIsHoliday", clsImportField.DataTypeID.gcvdtBit, 2, False)
                .Add("ApplyToOrigin", "LaneCalApplyToOrigin", clsImportField.DataTypeID.gcvdtBit, 2, False, clsImportField.PKValue.gcPK)
            End With
            Log("Lane Calendar Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oCalendar.Count
                Dim blnUseField As Boolean = True
                Try
                    If oCalendar(ct).Name = "LaneCalLaneControl" Or oCalendar(ct).Name = "LaneNumber" Or oCalendar(ct).Name = "LaneCalMonth" Or oCalendar(ct).Name = "LaneCalDay" Or oCalendar(ct).Name = "LaneCalApplyToOrigin" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oCalendar(ct).Name, IntegrationTypes.Lane)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oCalendar(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.buildCalendarCollection, could not build the calendar collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.buildCalendarCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importCalendarRecords(
        ByRef oCalendar As LaneData.LaneCalendarDataTable,
        ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsLane.importCalendarRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                CalendarErrors = 0
                TotalCalendarRecords = 0
                Try

                    Try
                        Dim lngMax As Long = oCalendar.Count
                        Log("Importing " & lngMax & " Lane Calendar Records.")
                        For Each oRow As LaneData.LaneCalendarRow In oCalendar
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
                            'Get the parent table key information
                            If blnDataValidated Then blnDataValidated = lookupFKValues(oItems,
                                                                                                strErrorMessage,
                                                                                                "Lane",
                                                                                                strSource,
                                                                                                "Lane calendar record for Lane number " & oItems("LaneNumber").Value)
                            'test if the record already exists.
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oItems,
                                                                                                strErrorMessage,
                                                                                                blnInsertRecord,
                                                                                                "Lane calendar record for Lane number " & oItems("LaneNumber").Value,
                                                                                                "LaneCal")
                            If Not blnDataValidated Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", "Lane Calendar")
                                CalendarErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "LaneCal", "", "") Then
                                    TotalCalendarRecords += 1
                                End If
                            End If
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importCalendarRecords; attempted to import lane calendar records for lane number" & oItems("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failed!" & readExceptionMessage(ex))
                        Me.CalendarErrors += 1
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            Me.CalendarErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importCalendarRecords; Could not import lane calendar records for lane number" & oItems("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importCalendarRecords(
        ByRef oCalendar As List(Of clsLaneCalendarObject60),
        ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            If oCalendar Is Nothing OrElse oCalendar.Count < 1 Then Return False

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsLane.importCalendarRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                CalendarErrors = 0
                TotalCalendarRecords = 0
                Try
                    Dim lngMax As Long = oCalendar.Count
                    Log("Importing " & lngMax & " Lane Calendar Records.")
                    For Each oRow As clsLaneCalendarObject60 In oCalendar
                        strErrorMessage = ""
                        blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
                        'Get the parent table key information
                        If blnDataValidated Then blnDataValidated = lookupFKValues(oItems,
                                                                                            strErrorMessage,
                                                                                            "Lane",
                                                                                            strSource,
                                                                                            "Lane calendar record for Lane number " & oItems("LaneNumber").Value)
                        'test if the record already exists.
                        If blnDataValidated Then blnDataValidated = doesRecordExist(oItems,
                                                                                            strErrorMessage,
                                                                                            blnInsertRecord,
                                                                                            "Lane calendar record for Lane number " & oItems("LaneNumber").Value,
                                                                                            "LaneCal")
                        If Not blnDataValidated Then
                            addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", "Lane Calendar")
                            CalendarErrors += 1
                        Else
                            'Save the changes
                            If saveData(oItems, blnInsertRecord, "LaneCal", "", "") Then
                                TotalCalendarRecords += 1
                            End If
                        End If
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importCalendarRecords; attempted to import lane calendar records for lane number" & oItems("LaneNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failed!" & readExceptionMessage(ex))
                        Me.CalendarErrors += 1
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            Me.CalendarErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importCalendarRecords; Could not import lane calendar records for lane number" & oItems("LaneNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.importCalendarRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importHeaderRecords(
                    ByRef oLanes As LaneData.LaneHeaderDataTable,
                    ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Dim blnOriginUse As Boolean = False
        Try

            'now get the Carrier Header Records
            Dim strSource As String = "clsLane.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""

            Dim intRetryCt As Integer = 0

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    Try
                        Dim intHeaderFieldCt As Integer = 55 'actual number of fields              
                        'set the default for the Use LaneBenchMiles flag
                        Dim blnUseBenchMilesDefault As Boolean = oFields("LaneBenchMiles").Use
                        'set the default for the use LaneLatitude and LaneLongitude values
                        Dim blnUseLaneLat As Boolean = oFields("LaneLatitude").Use
                        Dim blnUseLaneLong As Boolean = oFields("LaneLongitude").Use
                        Dim lngMax As Long = oLanes.Count
                        Log("Importing " & lngMax & " Lane Header Records.")
                        'Do Until objImportRS.EOF


                        For Each oRow As LaneData.LaneHeaderRow In oLanes
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes.  the 
                            'LaneCompNumber field's name is changed to LaneCompControl below
                            oFields("LaneCompNumber").Name = "LaneCompNumber"
                            oFields("LaneCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("LaneCompNumber").Length = 50
                            oFields("LaneCompNumber").Null = True
                            oFields("LaneOrigCompNumber").Name = "LaneOrigCompNumber"
                            oFields("LaneOrigCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("LaneOrigCompNumber").Length = 50
                            oFields("LaneOrigCompNumber").Null = True
                            oFields("LaneDestCompNumber").Name = "LaneDestCompNumber"
                            oFields("LaneDestCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("LaneDestCompNumber").Length = 50
                            oFields("LaneDestCompNumber").Null = True
                            oFields("LaneDefaultCarrierNumber").Name = "LaneDefaultCarrierNumber"
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'As of v-4.7.3 we expect the LaneOriginAddressUse to be provided as part of the download.  The default should always be false.
                            blnOriginUse = False
                            Boolean.TryParse(oFields("LaneOriginAddressUse").Value, blnOriginUse)
                            'test if the record already exists. 
                            Select Case doesLaneExist(oFields("LaneNumber"), "Lane")
                                Case -1
                                    blnDataValidated = False
                                    strErrorMessage = "Could not check for existing lane record.  The lane number " & oFields("LaneNumber").Value & " has not been downloaded."
                                Case 0 'no records found so insert a new one
                                    mblnInsertRecord = True
                                Case Else
                                    mblnInsertRecord = False
                            End Select
                            'Look up the Company Control Number
                            If blnDataValidated AndAlso Not lookupCompControlByAlphaCode(oFields("LaneCompNumber"), "LaneCompControl") Then
                                blnDataValidated = False
                                strErrorMessage = "Invalid reference to Company Number " & oFields("LaneCompNumber").Value
                            End If
                            'Look up the Default Carrier Control Number LaneDefaultCarrierNumber
                            If blnDataValidated AndAlso Not lookupDefaultCarrier(oFields("LaneDefaultCarrierNumber"), "LaneDefaultCarrierControl") Then
                                blnDataValidated = False
                                strErrorMessage = "Invalid reference to the Default Carrier Number " & oFields("LaneDefaultCarrierNumber").Value
                            End If

                            'Now we need to lookup the origin address and name values if a LaneOrigCompNumber is provided
                            If blnDataValidated AndAlso Not lookupCompAddress(oFields("LaneOrigCompNumber"),
                                    oFields("LaneOrigName"),
                                    oFields("LaneOrigAddress1"),
                                    oFields("LaneOrigAddress2"),
                                    oFields("LaneOrigAddress3"),
                                    oFields("LaneOrigCity"),
                                    oFields("LaneOrigState"),
                                    oFields("LaneOrigCountry"),
                                    oFields("LaneOrigZip"),
                                    oFields("LaneOrigContactPhone"),
                                    oFields("LaneOrigContactFax"),
                                    "LaneOrigCompControl") Then
                                blnDataValidated = False
                                strErrorMessage = "Invalid reference to Origin Company Number " & oFields("LaneOrigCompNumber").Value
                            End If

                            'Now we need to lookup the destination address and name values if a LaneDestCompNumber is provided
                            If blnDataValidated AndAlso Not lookupCompAddress(oFields("LaneDestCompNumber"),
                                    oFields("LaneDestName"),
                                    oFields("LaneDestAddress1"),
                                    oFields("LaneDestAddress2"),
                                    oFields("LaneDestAddress3"),
                                    oFields("LaneDestCity"),
                                    oFields("LaneDestState"),
                                    oFields("LaneDestCountry"),
                                    oFields("LaneDestZip"),
                                    oFields("LaneDestContactPhone"),
                                    oFields("LaneDestContactFax"),
                                    "LaneDestCompControl") Then
                                blnDataValidated = False
                                strErrorMessage = "Invalid reference to Destination Company Number " & oFields("LaneDestCompNumber").Value
                            End If

                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                                Return False
                            Else
                                'Save the use values for Bench Miles, Lat and Long because calcLatLong and calcMiles changes them
                                blnUseBenchMilesDefault = oFields("LaneBenchMiles").Use
                                blnUseLaneLat = oFields("LaneLatitude").Use
                                blnUseLaneLong = oFields("LaneLongitude").Use
                                'Check if the address has changed
                                If mblnInsertRecord OrElse (isAddressTheSame(oFields, oFields("LaneNumber"), "Lane") = 0) Then
                                    'Get the Lat Long and miles if needed
                                    calcLatLong(oFields, False)
                                    calcMiles(oFields, blnOriginUse, False)
                                End If
                                'Save the changes to the main table
                                If Not saveLaneData(oFields, mblnInsertRecord, "Lane") Then
                                    strErrorMessage = "Unable to save lane data to database download failed for lane number " & oFields("LaneNumber").Value
                                    addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                    RecordErrors += 1
                                Else
                                    TotalRecords += 1
                                    'Get the LaneControlNumber Back
                                    Dim intLaneControl = getLaneControl(oFields("LaneNumber"))
                                    If intLaneControl > 0 Then
                                        'Now add/update the Broker Number and Broker Name Fields to the LaneSec Table
                                        saveBrokerData(oFields, intLaneControl)
                                        UpdateRouteGuideControl(oFields, intLaneControl)
                                        saveSpecialCodes(oFields, intLaneControl)
                                    End If

                                End If
                                'We need to reset the Use Bench Miles and Lat and Long values
                                oFields("LaneBenchMiles").Use = blnUseBenchMilesDefault
                                oFields("LaneLatitude").Use = blnUseLaneLat
                                oFields("LaneLongitude").Use = blnUseLaneLong
                            End If
                            'Debug.Print strSQL
                            mblnInsertRecord = False
                            strErrorMessage = ""
                            blnDataValidated = False
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally
                        'Object dispoded of in sub routines via Using statement
                        'Try
                        '    oPCmiles.Dispose()
                        'Catch ex As Exception

                        'End Try
                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importHeaderRecords, attempted to import lane header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importHeaderRecords(
                    ByRef oLanes As List(Of clsLaneObject60),
                    ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Dim blnOriginUse As Boolean = False
        Try
            If oLanes Is Nothing OrElse oLanes.Count < 1 Then
                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsLane.importHeaderRecords failed to process Lane records because the list is empty<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords failed to process Lane records because the list is empty")
                Return False
            End If

            Dim strSource As String = "clsLane.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""

            Dim intRetryCt As Integer = 0

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    'set the default for the Use LaneBenchMiles flag
                    Dim blnUseBenchMilesDefault As Boolean = oFields("LaneBenchMiles").Use
                    'set the default for the use LaneLatitude and LaneLongitude values
                    Dim blnUseLaneLat As Boolean = oFields("LaneLatitude").Use
                    Dim blnUseLaneLong As Boolean = oFields("LaneLongitude").Use
                    Dim lngMax As Long = oLanes.Count
                    Log("Importing " & lngMax & " Lane Header Records.")
                    For Each oRow As clsLaneObject60 In oLanes
                        'Reset the data types and values to defaults for the following fields 
                        'at the top of each loop to handle alpha vs numeric data changes.  the 
                        'LaneCompNumber field's name is changed to LaneCompControl below
                        oFields("LaneCompNumber").Name = "LaneCompNumber"
                        oFields("LaneCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("LaneCompNumber").Length = 50
                        oFields("LaneCompNumber").Null = True
                        oFields("LaneOrigCompNumber").Name = "LaneOrigCompNumber"
                        oFields("LaneOrigCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("LaneOrigCompNumber").Length = 50
                        oFields("LaneOrigCompNumber").Null = True
                        oFields("LaneDestCompNumber").Name = "LaneDestCompNumber"
                        oFields("LaneDestCompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("LaneDestCompNumber").Length = 50
                        oFields("LaneDestCompNumber").Null = True
                        oFields("LaneDefaultCarrierNumber").Name = "LaneDefaultCarrierNumber"
                        strErrorMessage = ""
                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                        blnOriginUse = False
                        Boolean.TryParse(oFields("LaneOriginAddressUse").Value, blnOriginUse)
                        'test if the record already exists. 
                        Select Case doesLaneExist(oFields("LaneNumber"), "Lane")
                            Case -1
                                blnDataValidated = False
                                strErrorMessage = "Could not check for existing lane record.  The lane number " & oFields("LaneNumber").Value & " has not been downloaded."
                            Case 0 'no records found so insert a new one
                                mblnInsertRecord = True
                            Case Else
                                mblnInsertRecord = False
                        End Select
                        'Look up the Company Control Number
                        If blnDataValidated AndAlso Not lookupCompControlByAlphaCode(oFields("LaneCompNumber"), "LaneCompControl") Then
                            blnDataValidated = False
                            strErrorMessage = "Invalid reference to Company Number " & oFields("LaneCompNumber").Value
                        End If
                        'Look up the Default Carrier Control Number LaneDefaultCarrierNumber
                        If blnDataValidated AndAlso Not lookupDefaultCarrier(oFields("LaneDefaultCarrierNumber"), "LaneDefaultCarrierControl") Then
                            blnDataValidated = False
                            strErrorMessage = "Invalid reference to the Default Carrier Number " & oFields("LaneDefaultCarrierNumber").Value
                        End If

                        'Now we need to lookup the origin address and name values if a LaneOrigCompNumber is provided
                        If blnDataValidated AndAlso Not lookupCompAddress(oFields("LaneOrigCompNumber"),
                                oFields("LaneOrigName"),
                                oFields("LaneOrigAddress1"),
                                oFields("LaneOrigAddress2"),
                                oFields("LaneOrigAddress3"),
                                oFields("LaneOrigCity"),
                                oFields("LaneOrigState"),
                                oFields("LaneOrigCountry"),
                                oFields("LaneOrigZip"),
                                oFields("LaneOrigContactPhone"),
                                oFields("LaneOrigContactFax"),
                                "LaneOrigCompControl") Then
                            blnDataValidated = False
                            strErrorMessage = "Invalid reference to Origin Company Number " & oFields("LaneOrigCompNumber").Value
                        End If

                        'Now we need to lookup the destination address and name values if a LaneDestCompNumber is provided
                        If blnDataValidated AndAlso Not lookupCompAddress(oFields("LaneDestCompNumber"),
                                oFields("LaneDestName"),
                                oFields("LaneDestAddress1"),
                                oFields("LaneDestAddress2"),
                                oFields("LaneDestAddress3"),
                                oFields("LaneDestCity"),
                                oFields("LaneDestState"),
                                oFields("LaneDestCountry"),
                                oFields("LaneDestZip"),
                                oFields("LaneDestContactPhone"),
                                oFields("LaneDestContactFax"),
                                "LaneDestCompControl") Then
                            blnDataValidated = False
                            strErrorMessage = "Invalid reference to Destination Company Number " & oFields("LaneDestCompNumber").Value
                        End If

                        If Not blnDataValidated Then
                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                            RecordErrors += 1
                            Return False
                        Else
                            'Save the use values for Bench Miles, Lat and Long because calcLatLong and calcMiles changes them
                            blnUseBenchMilesDefault = oFields("LaneBenchMiles").Use
                            blnUseLaneLat = oFields("LaneLatitude").Use
                            blnUseLaneLong = oFields("LaneLongitude").Use
                            'Check if the address has changed
                            If mblnInsertRecord OrElse (isAddressTheSame(oFields, oFields("LaneNumber"), "Lane") = 0) Then
                                'Get the Lat Long and miles if needed
                                calcLatLong(oFields, False)
                                calcMiles(oFields, blnOriginUse, False)
                            End If
                            'Save the changes to the main table
                            If Not saveLaneData(oFields, mblnInsertRecord, "Lane") Then
                                strErrorMessage = "Unable to save lane data to database download failed for lane number " & oFields("LaneNumber").Value
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                TotalRecords += 1
                                'Get the LaneControlNumber Back
                                Dim intLaneControl = getLaneControl(oFields("LaneNumber"))
                                If intLaneControl > 0 Then
                                    'Now add/update the Broker Number and Broker Name Fields to the LaneSec Table
                                    saveBrokerData(oFields, intLaneControl)
                                    UpdateRouteGuideControl(oFields, intLaneControl)
                                    saveSpecialCodes(oFields, intLaneControl)
                                End If

                            End If
                            'We need to reset the Use Bench Miles and Lat and Long values
                            oFields("LaneBenchMiles").Use = blnUseBenchMilesDefault
                            oFields("LaneLatitude").Use = blnUseLaneLat
                            oFields("LaneLongitude").Use = blnUseLaneLong
                        End If
                        'Debug.Print strSQL
                        mblnInsertRecord = False
                        strErrorMessage = ""
                        blnDataValidated = False
                    Next
                    Return True

                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importHeaderRecords, attempted to import lane header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function


    Public Sub processNoLanePOs(ByVal strLaneNumber As String)
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim strNoLaneOrdersSelected As String = ""
        Try
            Dim strSQL As String = "Select * From dbo.POHNoLanes Where "
            If Me.AuthorizationCode.Trim.Length > 0 Then
                strSQL &= "POHNLAUTHCode = '" & Me.AuthorizationCode.Trim & "' AND "
            End If
            strSQL &= "POHNLvendor = " & strLaneNumber
            'check the active db connection
            If Me.openConnection Then
                With cmdObj
                    .Connection = DBCon
                    .CommandTimeout = 300
                    .CommandText = strSQL
                End With
                Dim adapter As New SqlClient.SqlDataAdapter(cmdObj)
                Dim oTable As New DataTable
                adapter.Fill(oTable)
                Dim oBook As New clsBook
                Dim POHNLControls As New List(Of Integer)
                Dim dsBook As BookData = oBook.getDataSet
                Dim dtBook As New BookData.BookHeaderDataTable
                Dim dtBookDetail As New BookData.BookDetailDataTable
                For Each oRow As DataRow In oTable.Rows
                    Dim drBook As BookData.BookHeaderRow = dtBook.NewRow
                    POHNLControls.Add(oRow.Item("POHNLControl"))
                    assignRowToRow(oRow, "POHNLOrderNumber", drBook, "PONumber", "")
                    assignRowToRow(oRow, "POHNLvendor", drBook, "POVendor", "")
                    assignRowToRow(oRow, "POHNLPOdate", drBook, "POdate", DBNull.Value)
                    assignRowToRow(oRow, "POHNLShipdate", drBook, "POShipdate", DBNull.Value)
                    assignRowToRow(oRow, "POHNLBuyer", drBook, "POBuyer", "")
                    assignRowToRow(oRow, "POHNLFrt", drBook, "POFrt", 0)
                    assignRowToRow(oRow, "POHNLTotalFrt", drBook, "POTotalFrt", 0)
                    assignRowToRow(oRow, "POHNLTotalCost", drBook, "POTotalCost", 0)
                    assignRowToRow(oRow, "POHNLWgt", drBook, "POWgt", 0)
                    assignRowToRow(oRow, "POHNLCube", drBook, "POCube", 0)
                    assignRowToRow(oRow, "POHNLQty", drBook, "POQty", 0)
                    assignRowToRow(oRow, "POHNLPallets", drBook, "POPallets", 0)
                    assignRowToRow(oRow, "POHNLLines", drBook, "POLines", 0)
                    assignRowToRow(oRow, "POHNLConfirm", drBook, "POConfirm", 0)
                    assignRowToRow(oRow, "POHNLDefaultCustomer", drBook, "PODefaultCustomer", "")
                    assignRowToRow(oRow, "POHNLDefaultCarrier", drBook, "PODefaultCarrier", 0)
                    assignRowToRow(oRow, "POHNLReqDate", drBook, "POReqDate", DBNull.Value)
                    assignRowToRow(oRow, "POHNLShipInstructions", drBook, "POShipInstructions", "")
                    assignRowToRow(oRow, "POHNLCooler", drBook, "POCooler", 0)
                    assignRowToRow(oRow, "POHNLFrozen", drBook, "POFrozen", 0)
                    assignRowToRow(oRow, "POHNLDry", drBook, "PODry", 0)
                    assignRowToRow(oRow, "POHNLTemp", drBook, "POTemp", "")
                    assignRowToRow(oRow, "POHNLCarType", drBook, "POCarType", "")
                    assignRowToRow(oRow, "POHNLShipVia", drBook, "POShipVia", "")
                    assignRowToRow(oRow, "POHNLShipViaType", drBook, "POShipViaType", "")
                    drBook.POConsigneeNumber = ""
                    assignRowToRow(oRow, "POHNLnumber", drBook, "POCustomerPO", "")
                    assignRowToRow(oRow, "POHNLOtherCost", drBook, "POOtherCosts", 0)
                    assignRowToRow(oRow, "POHNLStatusFlag", drBook, "POStatusFlag", 0)
                    strNoLaneOrdersSelected &= "<br />Missing lanes resolved for order number " & drBook.PONumber & " for company " & drBook.PODefaultCustomer & " using lane number " & drBook.POVendor & " <br />" & vbCrLf & vbCrLf

                    dtBook.AddBookHeaderRow(drBook)
                    dtBook.AcceptChanges()
                    'Get the Item Details
                    strSQL = "Select * From dbo.POINoLanes Where POIPOHNLControl = " & oRow.Item("POHNLControl")
                    With cmdObj
                        .Connection = DBCon
                        .CommandTimeout = 300
                        .CommandText = strSQL
                    End With
                    Dim oItemAdapter As New SqlClient.SqlDataAdapter(cmdObj)
                    Dim oItemTable As New DataTable
                    adapter.Fill(oItemTable)
                    For Each oIRows As DataRow In oItemTable.Rows
                        Dim drBookDetail As BookData.BookDetailRow = dtBookDetail.NewRow
                        assignRowToRow(oIRows, "ItemPONumber", drBookDetail, "ItemPONumber", "")
                        assignRowToRow(oIRows, "FixOffInvAllow", drBookDetail, "FixOffInvAllow", 0)
                        assignRowToRow(oIRows, "FixFrtAllow", drBookDetail, "FixFrtAllow", 0)
                        assignRowToRow(oIRows, "ItemNumber", drBookDetail, "ItemNumber", "")
                        assignRowToRow(oIRows, "QtyOrdered", drBookDetail, "QtyOrdered", 0)
                        assignRowToRow(oIRows, "FreightCost", drBookDetail, "FreightCost", 0)
                        assignRowToRow(oIRows, "ItemCost", drBookDetail, "ItemCost", 0)
                        assignRowToRow(oIRows, "Weight", drBookDetail, "Weight", 0)
                        assignRowToRow(oIRows, "Cube", drBookDetail, "Cube", 0)
                        assignRowToRow(oIRows, "Pack", drBookDetail, "Pack", 0)
                        assignRowToRow(oIRows, "Size", drBookDetail, "Size", "")
                        assignRowToRow(oIRows, "Description", drBookDetail, "Description", "")
                        assignRowToRow(oIRows, "Hazmat", drBookDetail, "Hazmat", "")
                        assignRowToRow(oIRows, "Brand", drBookDetail, "Brand", "")
                        assignRowToRow(oIRows, "CostCenter", drBookDetail, "CostCenter", "")
                        assignRowToRow(oIRows, "LotNumber", drBookDetail, "LotNumber", "")
                        assignRowToRow(oIRows, "LotExpirationDate", drBookDetail, "LotExpirationDate", DBNull.Value)
                        assignRowToRow(oIRows, "GTIN", drBookDetail, "GTIN", "")
                        assignRowToRow(oIRows, "CustItemNumber", drBookDetail, "CustItemNumber", "")
                        assignRowToRow(oIRows, "CustomerNumber", drBookDetail, "CustomerNumber", "")
                        dtBookDetail.AddBookDetailRow(drBookDetail)
                        dtBookDetail.AcceptChanges()
                    Next
                Next
                If POHNLControls.Count > 0 Then
                    Dim intRet As Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    With oBook
                        .AdminEmail = Me.AdminEmail
                        .GroupEmail = Me.GroupEmail
                        .SMTPServer = Me.SMTPServer
                        .Retry = Me.Retry
                        .DBServer = Me.DBServer
                        .Database = Me.Database
                        .Debug = Me.Debug
                        .FromEmail = Me.FromEmail
                        .AuthorizationCode = Me.AuthorizationCode
                        intRet = .ProcessData(dtBook, dtBookDetail, Me.DBConnection)
                    End With
                    If intRet = ProcessDataReturnValues.nglDataIntegrationComplete Then
                        ITNoLaneEmailMsg &= strNoLaneOrdersSelected
                        Log("NGL.FreightMaster.Integration.clsLane.ProcessNoLanePOs missing lanes resolved.")

                        Dim strSep As String = ""
                        strSQL = "Delete From dbo.POHNoLanes Where POHNLControl IN ("
                        For Each intControl As Integer In POHNLControls
                            strSQL &= strSep & intControl.ToString
                            strSep = ","
                        Next
                        strSQL &= ")"
                        With cmdObj
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .ExecuteNonQuery()
                        End With
                    End If
                End If
            Else
                ITEmailMsg &= "<br />" & Source & " Error (Import Not Affected): NGL.FreightMaster.Integration.clsLane.processNoLanePOs: Open database connection failure, attempted to create a database connection without success.  POs with missing lane number, " & strLaneNumber & ", could not be processed. Please resend the lane data.<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsLane.processNoLanePOs Error!")
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Error (Import Not Affected): NGL.FreightMaster.Integration.clsLane.processNoLanePOs; POs with missing lane number, " & strLaneNumber & ", could not be processed correctly. Please resend the lane data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsLane.processNoLanePOs Error!")
        Finally
            Try
                cmdObj.Cancel()
            Catch ex As Exception

            End Try
        End Try

    End Sub
End Class


Public Class ProcessLaneHeaderResult


    Private _dicInvalidKeys As New Dictionary(Of String, String)
    Public Property dicInvalidKeys() As Dictionary(Of String, String)
        Get
            If _dicInvalidKeys Is Nothing Then _dicInvalidKeys = New Dictionary(Of String, String)
            Return _dicInvalidKeys
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _dicInvalidKeys = value
        End Set
    End Property

    Private _LaneControl As Integer = 0
    Public Property LaneControl() As Integer
        Get
            Return _LaneControl
        End Get
        Set(ByVal value As Integer)
            _LaneControl = value
        End Set
    End Property

    Private _insertFlag As Boolean = False
    Public Property insertFlag() As Boolean
        Get
            Return _insertFlag
        End Get
        Set(ByVal value As Boolean)
            _insertFlag = value
        End Set
    End Property

    Private _successFlag As Boolean
    Public Property successFlag() As Boolean
        Get
            Return _successFlag
        End Get
        Set(ByVal value As Boolean)
            _successFlag = value
        End Set
    End Property

    Private _intRet As ProcessDataReturnValues
    Public Property intRet() As ProcessDataReturnValues
        Get
            Return _intRet
        End Get
        Set(ByVal value As ProcessDataReturnValues)
            _intRet = value
        End Set
    End Property

    Public Sub processLaneHeaderFailed(ByVal LaneName As String, ByVal LaneNumber As String, ByVal LaneCompAlphaCode As String, ByVal LaneLegalEntity As String)
        If Me.dicInvalidKeys Is Nothing OrElse Me.dicInvalidKeys.Count < 1 Then
            'add the key fields to the dictionary
            Me.dicInvalidKeys = New Dictionary(Of String, String) From {{"Lane Name", LaneName}, {"Lane Number", LaneNumber}, {"Lane Comp Alpha Code", LaneCompAlphaCode}, {"Lane Legal Entity", LaneLegalEntity}}
        End If
        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        successFlag = False
    End Sub

End Class