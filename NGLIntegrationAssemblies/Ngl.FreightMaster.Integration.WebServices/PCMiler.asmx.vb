Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Ngl.Service.PCMiler64
'Imports Ngl.FreightMaster.PCMTest
'Imports Ngl.Interfaces

<System.Web.Services.WebService(Namespace:="http://nextgeneration.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class PCMiler
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function CityToLatLong(ByVal CityZip As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                strRet = oPCmiles.CityToLatLong(CityZip)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    <WebMethod()>
    Public Function cityStateZipLookup(ByVal postalCode As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As clsAddress()
        Dim arrRet As clsAddress()

        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                arrRet = oPCmiles.cityStateZipLookup(postalCode)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
#Disable Warning BC42104 ' Variable 'arrRet' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return arrRet
#Enable Warning BC42104 ' Variable 'arrRet' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    <WebMethod()>
    Public Function FullName(ByVal CityNameOrZipCode As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                strRet = oPCmiles.FullName(CityNameOrZipCode)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    <WebMethod()>
    Public Function getGeoCode(ByVal location As String,
                                ByRef dblLat As Double,
                                ByRef dblLong As Double,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                blnRet = oPCmiles.getGeoCode(location, dblLat, dblLong)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return blnRet
    End Function

    <WebMethod()>
    Public Function LatLongToCity(ByVal latlong As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                strRet = oPCmiles.LatLongToCity(latlong)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' Use PCM to calculate miles calculate miles. 
    ''' Modified to support extended calculation options like shortest 53Foot routing
    ''' </summary>
    ''' <param name="objOrig"></param>
    ''' <param name="objDest"></param>
    ''' <param name="Route_Type"></param>
    ''' <param name="Dist_Type"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="intBookControl"></param>
    ''' <param name="intLaneControl"></param>
    ''' <param name="strItemNumber"></param>
    ''' <param name="strItemType"></param>
    ''' <param name="dblAutoCorrectBadLaneZipCodes"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnBatch"></param>
    ''' <param name="arrBaddAddresses"></param>
    ''' <param name="DebugMode"></param>
    ''' <param name="LoggingOn"></param>
    ''' <param name="KeepLogDays"></param>
    ''' <param name="SaveOldLog"></param>
    ''' <param name="LogFileName"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.101 on 2/9/2017 
    '''   provides support for PCMSSetCalcTypeEx for use 
    '''   with extended calculation options like shortest 53Foot routing
    ''' </remarks>
    <WebMethod()>
    Public Function getPracticalMiles(ByVal objOrig As clsAddress,
                                ByVal objDest As clsAddress,
                                ByVal Route_Type As Integer,
                                ByVal Dist_Type As Integer,
                                ByVal intCompControl As Integer,
                                ByVal intBookControl As Integer,
                                ByVal intLaneControl As Integer,
                                ByVal strItemNumber As String,
                                ByVal strItemType As String,
                                ByVal dblAutoCorrectBadLaneZipCodes As Double,
                                ByVal dblBatchID As Double,
                                ByVal blnBatch As Boolean,
                                ByRef arrBaddAddresses() As clsPCMBadAddress,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                'Begin Modified by RHR for v-7.0.6.101 on 2/9/2017 
                Dim intPCMOptFlag As Integer = getPCMRoutingOption()
                Dim intPCMCalcType As Integer = convertNGLRouteTypeToPCMCalcType(Route_Type)
                Dim intPCMVelType As Integer = INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK
                oGlobalStopData = oPCmiles.getPracticalMilesEx(objOrig, objDest, Route_Type, Dist_Type, intPCMCalcType, intPCMOptFlag, intPCMVelType, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                'oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                'End Modified by RHR for v-7.0.6.101 on 2/9/2017 
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    ''' <summary>
    ''' Use PCM to calculate miles calculate miles. 
    ''' Modified to support extended calculation options like shortest 53Foot routing
    ''' This overload supports the Use Zip Only Global Parameter PCMilerUseZipOnly
    ''' </summary>
    ''' <param name="objOrig"></param>
    ''' <param name="objDest"></param>
    ''' <param name="Route_Type"></param>
    ''' <param name="Dist_Type"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="intBookControl"></param>
    ''' <param name="intLaneControl"></param>
    ''' <param name="strItemNumber"></param>
    ''' <param name="strItemType"></param>
    ''' <param name="dblAutoCorrectBadLaneZipCodes"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnBatch"></param>
    ''' <param name="arrBaddAddresses"></param>
    ''' <param name="DebugMode"></param>
    ''' <param name="LoggingOn"></param>
    ''' <param name="KeepLogDays"></param>
    ''' <param name="SaveOldLog"></param>
    ''' <param name="LogFileName"></param>
    ''' <param name="UseZipOnly"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.101 on 2/9/2017 
    '''   provides support for PCMSSetCalcTypeEx for use 
    '''   with extended calculation options like shortest 53Foot routing
    ''' </remarks>
    <WebMethod()>
    Public Function getPracticalMilesEX(ByVal objOrig As clsAddress,
                                ByVal objDest As clsAddress,
                                ByVal Route_Type As Integer,
                                ByVal Dist_Type As Integer,
                                ByVal intCompControl As Integer,
                                ByVal intBookControl As Integer,
                                ByVal intLaneControl As Integer,
                                ByVal strItemNumber As String,
                                ByVal strItemType As String,
                                ByVal dblAutoCorrectBadLaneZipCodes As Double,
                                ByVal dblBatchID As Double,
                                ByVal blnBatch As Boolean,
                                ByRef arrBaddAddresses() As clsPCMBadAddress,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByVal UseZipOnly As Boolean,
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly
                'Begin Modified by RHR for v-7.0.6.101 on 2/9/2017 
                Dim intPCMOptFlag As Integer = getPCMRoutingOption()
                Dim intPCMCalcType As Integer = convertNGLRouteTypeToPCMCalcType(Route_Type)
                Dim intPCMVelType As Integer = INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK
                oGlobalStopData = oPCmiles.getPracticalMilesEx(objOrig, objDest, Route_Type, Dist_Type, intPCMCalcType, intPCMOptFlag, intPCMVelType, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                'oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                'End Modified by RHR for v-7.0.6.101 on 2/9/2017 
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    <WebMethod()>
    Public Function Miles(ByVal Origin As String,
                                ByVal Destination As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As Single
        Dim sglRet As Single = 0
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                sglRet = oPCmiles.Miles(Origin, Destination)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return sglRet
    End Function

    <WebMethod()>
    Public Function zipcode(ByVal CityName As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                strRet = oPCmiles.zipcode(CityName)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    <WebMethod()>
    Public Function PCMReSync(ByVal arrStopData() As clsPCMDataStop,
                                ByVal strConsNumber As String,
                                ByVal dblBatchID As Double,
                                ByVal blnKeepStopNumbers As Boolean,
                                ByRef arrAllStops() As clsAllStop,
                                ByRef arrBaddAddresses() As clsPCMBadAddress,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    <WebMethod()>
    Public Function PCMReSyncEX(ByVal arrStopData() As clsPCMDataStop,
                                ByVal strConsNumber As String,
                                ByVal dblBatchID As Double,
                                ByVal blnKeepStopNumbers As Boolean,
                                ByRef arrAllStops() As clsAllStop,
                                ByRef arrBaddAddresses() As clsPCMBadAddress,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByVal UseZipOnly As Boolean,
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly
                oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    ''' <summary>
    ''' Use PCM to resequence stops and calculate miles.  Supports multi-pick functionality
    ''' Modified to support extended calculation options like shortest 53Foot routing
    ''' </summary>
    ''' <param name="oIFMStops"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnKeepStopNumbers"></param>
    ''' <param name="oPCMReportRecords"></param>
    ''' <param name="DebugMode"></param>
    ''' <param name="LoggingOn"></param>
    ''' <param name="KeepLogDays"></param>
    ''' <param name="SaveOldLog"></param>
    ''' <param name="LogFileName"></param>
    ''' <param name="UseZipOnly"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.101 on 2/9/2017 
    '''   provides support for PCMSSetCalcTypeEx for use 
    '''   with extended calculation options like shortest 53Foot routing
    ''' </remarks>
    <WebMethod()>
    Public Function PCMReSyncMultiStop(ByRef oIFMStops As clsFMStopData(),
                                ByVal dblBatchID As Double,
                                ByVal blnKeepStopNumbers As Boolean,
                                ByRef oPCMReportRecords As clsPCMReportRecord(),
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByVal UseZipOnly As Boolean,
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly
                'Begin Modified by RHR for v-7.0.6.101 on 2/9/2017 
                Dim intPCMOptFlag As Integer = getPCMRoutingOption()
                Dim intRouteType As Integer = 0
                If Not oIFMStops Is Nothing AndAlso oIFMStops.Count() > 0 Then
                    intRouteType = oIFMStops(0).RouteType
                End If
                Dim intPCMCalcType As Integer = convertNGLRouteTypeToPCMCalcType(intRouteType)
                Dim intPCMVelType As Integer = INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK
                oGlobalStopData = oPCmiles.PCMReSyncMultiStopEx(oIFMStops, intPCMCalcType, intPCMOptFlag, intPCMVelType, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
                'oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
                'End Modified by RHR for v-7.0.6.101 on 2/9/2017 
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    <WebMethod()>
    Public Function getRouteMiles(ByRef sRoute As clsSimpleStop(),
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As clsPCMReturn
        Dim oclsPCMReturn As clsPCMReturn = Nothing
        Try

            Using oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oclsPCMReturn = oPCmiles.getRouteMiles(sRoute)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oclsPCMReturn
    End Function


    <WebMethod()>
    Public Function PCMValidateAddress(ByVal strAddress As String,
                                ByVal DebugMode As Boolean,
                                ByVal LoggingOn As Boolean,
                                ByVal KeepLogDays As Boolean,
                                ByVal SaveOldLog As Boolean,
                                ByVal LogFileName As String,
                                ByRef LastError As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                blnRet = oPCmiles.PCMValidateAddress(strAddress)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Read the PCMilerRouteOption parameter from the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.101 on 2/9/2017 
    '''   provides support for PCMSSetCalcTypeEx for use 
    '''   with extended calculation options like shortest 53Foot routing
    ''' </remarks>
    Private Function getPCMRoutingOption() As Integer
        Dim intRet As Integer = 0
        Dim oQuery As Ngl.Core.Data.Query
        Dim objCon As System.Data.SqlClient.SqlConnection
        Try
            oQuery = New Ngl.Core.Data.Query(Utilities.GetConnectionString())
            objCon = New System.Data.SqlClient.SqlConnection(Utilities.GetConnectionString())
            Dim strSQL As String = "Select top 1 ParValue From dbo.Parameter where ParKey = 'PCMilerRouteOption'"
            Dim sResults = oQuery.getScalarValue(objCon, strSQL, 0)
            Integer.TryParse(sResults, intRet)
            'convert intRet from NGL option to PCM Calc option
            Select Case intRet
                Case INGL_Service_PCMiler.NGL_Opt_Flags.NGL_AVOIDTOLL
                    intRet = INGL_Service_PCMiler.PCMEX_Opt_Flags.OPT_AVOIDTOLL
                Case INGL_Service_PCMiler.NGL_Opt_Flags.NGL_FIFTYTHREE
                    intRet = INGL_Service_PCMiler.PCMEX_Opt_Flags.OPT_FIFTYTHREE
                Case INGL_Service_PCMiler.NGL_Opt_Flags.NGL_NATIONAL
                    intRet = INGL_Service_PCMiler.PCMEX_Opt_Flags.OPT_NATIONAL
                Case Else
                    intRet = 0
            End Select

        Catch ex As Exception
            'on error just return the default of zero = none
        Finally
            'close the db connection
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
#Disable Warning BC42104 ' Variable 'objCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not objCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'objCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                    If objCon.State = ConnectionState.Open Then
                        objCon.Close()
                    End If
                End If
                objCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return intRet
    End Function

    ''' <summary>
    ''' Converts the old NGL Route Type value to the new PCMEX_CALCTYPE where supported
    ''' supported values are Practical, shortest and Air.  All other values must use the 
    ''' legacy routing options
    ''' </summary>
    ''' <param name="Route_Type"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.101 on 2/9/20172/9/2017 
    '''   provides support for PCMSSetCalcTypeEx for use 
    '''   with extended calculation options like shortest 53Foot routing
    ''' </remarks>
    Private Function convertNGLRouteTypeToPCMCalcType(ByVal Route_Type As Integer) As Integer
        Dim intRet As Integer = 0
        Select Case Route_Type
            Case INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_PRACTICAL
                intRet = INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_PRACTICAL
            Case INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_SHORTEST
                intRet = INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_SHORTEST
            Case INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_AIR
                intRet = INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_AIR
            Case Else
                intRet = INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_NONE
        End Select

        Return intRet
    End Function


End Class