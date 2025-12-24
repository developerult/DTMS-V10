Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Ngl.Service.PCMiler64

Public Class TestPCMiler
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnRunTest_Click(sender As Object, e As EventArgs) Handles btnRunTest.Click
        Dim strLastError As String = ""
        lbResults.Items.Clear()
        'citystatelookup for from zip
        Dim oStops As New List(Of clsAddress)
        If Not String.IsNullOrWhiteSpace(Me.tbStop1Zip.Text) Then
            Dim oRet As clsAddress() = cityStateZipLookup(Me.tbStop1Zip.Text, strLastError)
            If Not String.IsNullOrWhiteSpace(strLastError) Then
                lbResults.Items.Add("Stop 1 city state lookup Error: " & strLastError)
            ElseIf Not oRet Is Nothing AndAlso oRet.Count() > 0 Then
                Dim intCt As Integer = 1
                oStops.Add(oRet(0))
                For Each oAddress As clsAddress In oRet
                    lbResults.Items.Add(String.Format("Stop 1 city state results {0}: {1} {2} {3}  ", intCt, oAddress.strCity, oAddress.strState, oAddress.strZip))
                    intCt += 1
                Next
            Else
                lbResults.Items.Add("Stop 1 Data Missing No Data Found")
            End If
        Else
            lbResults.Items.Add("No Stop 1 Zip Code")
        End If

        If Not String.IsNullOrWhiteSpace(Me.tbStop2Zip.Text) Then
            Dim oRet As clsAddress() = cityStateZipLookup(Me.tbStop2Zip.Text, strLastError)
            If Not String.IsNullOrWhiteSpace(strLastError) Then
                lbResults.Items.Add("Stop 2 city state lookup Error: " & strLastError)
            ElseIf Not oRet Is Nothing AndAlso oRet.Count() > 0 Then
                Dim intCt As Integer = 1
                oStops.Add(oRet(0))
                For Each oAddress As clsAddress In oRet
                    lbResults.Items.Add(String.Format("Stop 2 city state results {0}: {1} {2} {3}  ", intCt, oAddress.strCity, oAddress.strState, oAddress.strZip))
                    intCt += 1
                Next
            Else
                lbResults.Items.Add("Stop 2 Data Missing No Data Found")
            End If
        Else
            lbResults.Items.Add("No Stop 2 Zip Code")
        End If

        If Not String.IsNullOrWhiteSpace(Me.tbStop3Zip.Text) Then
            Dim oRet As clsAddress() = cityStateZipLookup(Me.tbStop3Zip.Text, strLastError)
            If Not String.IsNullOrWhiteSpace(strLastError) Then
                lbResults.Items.Add("Stop 3 city state lookup Error: " & strLastError)
            ElseIf Not oRet Is Nothing AndAlso oRet.Count() > 0 Then
                Dim intCt As Integer = 1
                oStops.Add(oRet(0))
                For Each oAddress As clsAddress In oRet
                    lbResults.Items.Add(String.Format("Stop 3 city state results {0}: {1} {2} {3}  ", intCt, oAddress.strCity, oAddress.strState, oAddress.strZip))
                    intCt += 1
                Next
            Else
                lbResults.Items.Add("Stop 3 Data Missing No Data Found")
            End If
        Else
            lbResults.Items.Add("No Stop 3 Zip Code")
        End If


        If Not String.IsNullOrWhiteSpace(Me.tbStop4Zip.Text) Then
            Dim oRet As clsAddress() = cityStateZipLookup(Me.tbStop4Zip.Text, strLastError)
            If Not String.IsNullOrWhiteSpace(strLastError) Then
                lbResults.Items.Add("Stop 4 city state lookup Error: " & strLastError)
                strLastError = ""
            ElseIf Not oRet Is Nothing AndAlso oRet.Count() > 0 Then
                Dim intCt As Integer = 1
                oStops.Add(oRet(0))
                For Each oAddress As clsAddress In oRet
                    lbResults.Items.Add(String.Format("Stop 4 city state results {0}: {1} {2} {3}  ", intCt, oAddress.strCity, oAddress.strState, oAddress.strZip))
                    intCt += 1
                Next
            Else
                lbResults.Items.Add("Stop 4 Data Missing No Data Found")
            End If
        Else
            lbResults.Items.Add("No Stop 4 Zip Code")
        End If
        'get lat long Test
        If Not oStops Is Nothing AndAlso oStops.Count() > 1 Then
            For Each oAddress As clsAddress In oStops
                Dim dblLat As Double
                Dim dblLong As Double
                Dim strLocation As String = String.Format("{0} {1} {2} ", oAddress.strCity, oAddress.strState, oAddress.strZip)
                If getGeoCode(strLocation, dblLat, dblLong, strLastError) Then
                    If Not String.IsNullOrWhiteSpace(strLastError) Then
                        lbResults.Items.Add("Get GEO Code Error: " & strLastError)
                        strLastError = ""
                    End If
                    lbResults.Items.Add(String.Format("Geo Code for Address {0}: Lat {1} Long {2}", strLocation, dblLat, dblLong))
                ElseIf Not String.IsNullOrWhiteSpace(strLastError) Then
                    lbResults.Items.Add("Get GEO Code Error for " & strLocation & " : " & strLastError)
                    strLastError = ""
                End If
            Next
        End If




        'Dim strFromZip = Me.tbFromZip.Text
        'Dim strToZip = Me.tbToZip.Text
        'Dim sglRet As Single = 0
        'Try
        '    Using oPCmiles As New PCMiles
        '        'Dim oPCmiles As New PCMiles
        '        oPCmiles.Debug = False
        '        oPCmiles.LoggingOn = False
        '        oPCmiles.KeepLogDays = 7
        '        oPCmiles.SaveOldLog = False
        '        oPCmiles.LogFileName = ""
        '        sglRet = oPCmiles.Miles(strFromZip, strToZip)
        '        Me.lblError.Text = oPCmiles.LastError
        '    End Using
        'Catch ex As Exception
        '    Me.lblError.Text = ex.Message
        'End Try
        'Me.tbMiles.Text = sglRet.ToString()
    End Sub



    Private Function CityToLatLong(ByVal CityZip As String, _
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                strRet = oPCmiles.CityToLatLong(CityZip)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    Private Function cityStateZipLookup(ByVal postalCode As String, _
                                ByRef LastError As String) As clsAddress()
        Dim arrRet As clsAddress()

        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
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

    Private Function FullName(ByVal CityNameOrZipCode As String, _
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                strRet = oPCmiles.FullName(CityNameOrZipCode)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    Private Function getGeoCode(ByVal location As String, _
                                ByRef dblLat As Double, _
                                ByRef dblLong As Double, _
                                ByRef LastError As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                blnRet = oPCmiles.getGeoCode(location, dblLat, dblLong)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return blnRet
    End Function

    Private Function LatLongToCity(ByVal latlong As String, _
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                strRet = oPCmiles.LatLongToCity(latlong)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    Private Function getPracticalMiles(ByVal objOrig As clsAddress, _
                                ByVal objDest As clsAddress, _
                                ByVal Route_Type As Integer, _
                                ByVal Dist_Type As Integer, _
                                ByVal intCompControl As Integer, _
                                ByVal intBookControl As Integer, _
                                ByVal intLaneControl As Integer, _
                                ByVal strItemNumber As String, _
                                ByVal strItemType As String, _
                                ByVal dblAutoCorrectBadLaneZipCodes As Double, _
                                ByVal dblBatchID As Double, _
                                ByVal blnBatch As Boolean, _
                                ByRef arrBaddAddresses() As clsPCMBadAddress, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""

                oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    Private Function getPracticalMilesEX(ByVal objOrig As clsAddress, _
                                ByVal objDest As clsAddress, _
                                ByVal Route_Type As Integer, _
                                ByVal Dist_Type As Integer, _
                                ByVal intCompControl As Integer, _
                                ByVal intBookControl As Integer, _
                                ByVal intLaneControl As Integer, _
                                ByVal strItemNumber As String, _
                                ByVal strItemType As String, _
                                ByVal dblAutoCorrectBadLaneZipCodes As Double, _
                                ByVal dblBatchID As Double, _
                                ByVal blnBatch As Boolean, _
                                ByRef arrBaddAddresses() As clsPCMBadAddress, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                oGlobalStopData = oPCmiles.getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    Private Function Miles(ByVal Origin As String, _
                                ByVal Destination As String, _
                                ByRef LastError As String) As Single
        Dim sglRet As Single = 0
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                sglRet = oPCmiles.Miles(Origin, Destination)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return sglRet
    End Function

    Private Function zipcode(ByVal CityName As String, _
                                ByRef LastError As String) As String
        Dim strRet As String = ""
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                strRet = oPCmiles.zipcode(CityName)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return strRet
    End Function

    Private Function PCMReSync(ByVal arrStopData() As clsPCMDataStop, _
                                ByVal strConsNumber As String, _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef arrAllStops() As clsAllStop, _
                                ByRef arrBaddAddresses() As clsPCMBadAddress, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    Private Function PCMReSyncEX(ByVal arrStopData() As clsPCMDataStop, _
                                ByVal strConsNumber As String, _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef arrAllStops() As clsAllStop, _
                                ByRef arrBaddAddresses() As clsPCMBadAddress, _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                oPCmiles.UseZipOnly = UseZipOnly
                oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBaddAddresses)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    Private Function PCMReSyncMultiStop(ByRef oIFMStops As clsFMStopData(), _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef oPCMReportRecords As clsPCMReportRecord(), _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                oPCmiles.UseZipOnly = UseZipOnly
                oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function

    Private Function getRouteMiles(ByRef sRoute As clsSimpleStop(), _
                                ByRef LastError As String) As clsPCMReturn
        Dim oclsPCMReturn As clsPCMReturn = Nothing
        Try

            Using oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                oclsPCMReturn = oPCmiles.getRouteMiles(sRoute)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oclsPCMReturn
    End Function

    Private Function PCMValidateAddress(ByVal strAddress As String, _
                                ByRef LastError As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Using oPCmiles As New PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = False
                oPCmiles.LoggingOn = False
                oPCmiles.KeepLogDays = 1
                oPCmiles.SaveOldLog = False
                oPCmiles.LogFileName = ""
                blnRet = oPCmiles.PCMValidateAddress(strAddress)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return blnRet
    End Function

End Class