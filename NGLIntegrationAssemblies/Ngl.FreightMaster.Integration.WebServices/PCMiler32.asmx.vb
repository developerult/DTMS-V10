Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
'Imports Ngl.Service.PCMiler32
'Imports Ngl.FreightMaster.PCMTest
'Imports Ngl.Interfaces

<System.Web.Services.WebService(Namespace:="http://nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PCMiler32
    Inherits System.Web.Services.WebService

    '<WebMethod()> _
    'Public Function CityToLatLong(ByVal CityZip As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As String
    '    Dim strRet As String = ""
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            strRet = oPCmiles.CityToLatLong(CityZip)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return strRet
    'End Function

    '<WebMethod()> _
    'Public Function FullName(ByVal CityNameOrZipCode As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As String
    '    Dim strRet As String = ""
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            strRet = oPCmiles.FullName(CityNameOrZipCode)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return strRet
    'End Function

    '<WebMethod()> _
    'Public Function getGeoCode(ByVal location As String, _
    '                            ByRef dblLat As Double, _
    '                            ByRef dblLong As Double, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As Boolean
    '    Dim blnRet As Boolean = False
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            blnRet = oPCmiles.getGeoCode(location, dblLat, dblLong)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return blnRet
    'End Function

    '<WebMethod()> _
    'Public Function LatLongToCity(ByVal latlong As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As String
    '    Dim strRet As String = ""
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            strRet = oPCmiles.LatLongToCity(latlong)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return strRet
    'End Function

    '<WebMethod()> _
    'Public Function getPracticalMiles(ByVal objOrig As clsAddress, _
    '                            ByVal objDest As clsAddress, _
    '                            ByVal Route_Type As Integer, _
    '                            ByVal Dist_Type As Integer, _
    '                            ByVal intCompControl As Integer, _
    '                            ByVal intBookControl As Integer, _
    '                            ByVal intLaneControl As Integer, _
    '                            ByVal strItemNumber As String, _
    '                            ByVal strItemType As String, _
    '                            ByVal dblAutoCorrectBadLaneZipCodes As Double, _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnBatch As Boolean, _
    '                            ByRef arrBaddAddresses() As clsPCMBadAddress, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As clsGlobalStopData
    '    Dim oGlobalStopData As clsGlobalStopData = Nothing
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName

    '            oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oGlobalStopData
    'End Function

    '<WebMethod()> _
    'Public Function getPracticalMilesEX(ByVal objOrig As clsAddress, _
    '                            ByVal objDest As clsAddress, _
    '                            ByVal Route_Type As Integer, _
    '                            ByVal Dist_Type As Integer, _
    '                            ByVal intCompControl As Integer, _
    '                            ByVal intBookControl As Integer, _
    '                            ByVal intLaneControl As Integer, _
    '                            ByVal strItemNumber As String, _
    '                            ByVal strItemType As String, _
    '                            ByVal dblAutoCorrectBadLaneZipCodes As Double, _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnBatch As Boolean, _
    '                            ByRef arrBaddAddresses() As clsPCMBadAddress, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByVal UseZipOnly As Boolean, _
    '                            ByRef LastError As String) As clsGlobalStopData
    '    Dim oGlobalStopData As clsGlobalStopData = Nothing
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oPCmiles.UseZipOnly = UseZipOnly

    '            oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oGlobalStopData
    'End Function

    '<WebMethod()> _
    'Public Function Miles(ByVal Origin As String, _
    '                            ByVal Destination As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As Single
    '    Dim sglRet As Single = 0
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            sglRet = oPCmiles.Miles(Origin, Destination)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return sglRet
    'End Function

    '<WebMethod()> _
    'Public Function zipcode(ByVal CityName As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As String
    '    Dim strRet As String = ""
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            strRet = oPCmiles.zipcode(CityName)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return strRet
    'End Function

    '<WebMethod()> _
    'Public Function PCMReSync(ByVal arrStopData() As clsPCMDataStop, _
    '                            ByVal strConsNumber As String, _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnKeepStopNumbers As Boolean, _
    '                            ByRef arrAllStops() As clsAllStop, _
    '                            ByRef arrBaddAddresses() As clsPCMBadAddress, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As clsGlobalStopData
    '    Dim oGlobalStopData As clsGlobalStopData = Nothing
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oGlobalStopData
    'End Function

    '<WebMethod()> _
    'Public Function PCMReSyncEX(ByVal arrStopData() As clsPCMDataStop, _
    '                            ByVal strConsNumber As String, _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnKeepStopNumbers As Boolean, _
    '                            ByRef arrAllStops() As clsAllStop, _
    '                            ByRef arrBaddAddresses() As clsPCMBadAddress, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByVal UseZipOnly As Boolean, _
    '                            ByRef LastError As String) As clsGlobalStopData
    '    Dim oGlobalStopData As clsGlobalStopData = Nothing
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oPCmiles.UseZipOnly = UseZipOnly
    '            oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oGlobalStopData
    'End Function


    '<WebMethod()> _
    'Public Function PCMReSyncMultiStop(ByRef oIFMStops As clsFMStopData(), _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnKeepStopNumbers As Boolean, _
    '                            ByRef oPCMReportRecords As clsPCMReportRecord(), _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByVal UseZipOnly As Boolean, _
    '                            ByRef LastError As String) As clsGlobalStopData
    '    Dim oGlobalStopData As clsGlobalStopData = Nothing
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oPCmiles.UseZipOnly = UseZipOnly
    '            oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oGlobalStopData
    'End Function

    '<WebMethod()> _
    'Public Function getRouteMiles(ByRef sRoute As clsSimpleStop(), _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As clsPCMReturn
    '    Dim oclsPCMReturn As clsPCMReturn = Nothing
    '    Try

    '        Using oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oclsPCMReturn = oPCmiles.getRouteMiles(sRoute)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return oclsPCMReturn
    'End Function


    '<WebMethod()> _
    'Public Function PCMValidateAddress(ByVal strAddress As String, _
    '                            ByVal DebugMode As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal KeepLogDays As Boolean, _
    '                            ByVal SaveOldLog As Boolean, _
    '                            ByVal LogFileName As String, _
    '                            ByRef LastError As String) As Boolean
    '    Dim blnRet As Boolean = False
    '    Try
    '        Using oPCmiles As New PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = DebugMode
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            blnRet = oPCmiles.PCMValidateAddress(strAddress)
    '            LastError = oPCmiles.LastError
    '        End Using
    '    Catch ex As Exception
    '        LastError = ex.Message
    '    End Try
    '    Return blnRet
    'End Function


End Class