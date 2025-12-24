Imports PCMComService = NGL.Service.PCMiler64

Public Class Form1

    Private _RouteTypes As Dictionary(Of Integer, String)
    Public Property RouteTypes() As Dictionary(Of Integer, String)
        Get
            Return _RouteTypes
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _RouteTypes = value
        End Set
    End Property

    Private _DistanceTypes As Dictionary(Of Integer, String)
    Public Property DistanceTypes() As Dictionary(Of Integer, String)
        Get
            Return _DistanceTypes
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _DistanceTypes = value
        End Set
    End Property

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        With My.Settings
            .OrigAddress = Me.tbOrigAddress.Text
            .OrigCity = Me.tbOrigCity.Text
            .OrigState = Me.tbOrigState.Text
            .OrigZip = Me.tbOrigZip.Text

            .DestAddress = Me.tbDestAddress.Text
            .DestCity = Me.tbDestCity.Text
            .DestState = Me.tbDestState.Text
            .DestZip = Me.tbDestZip.Text

            .Route_Type = Me.cboRouteType.SelectedValue
            .Dist_Type = Me.cboDistanceType.SelectedValue
            .UseCom = Me.chkUseCom.Checked

        End With
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        RouteTypes = New Dictionary(Of Integer, String) From { _
            {0, "PRACTICAL"}, _
            {1, "SHORTEST"}, _
            {2, "NATIONAL"}, _
            {3, "AVOIDTOLL"}, _
            {4, "AIR"}, _
            {6, "53FOOT"} _
        }

        DistanceTypes = New Dictionary(Of Integer, String) From { _
            {0, "MILES"}, _
            {1, "KILOMETERS"} _
        }


        Me.lblErrors.Text = ""
        Me.lblPCMBadAddressMsg.Text = ""

        Me.cboRouteType.DataSource = New BindingSource(RouteTypes, Nothing)
        Me.cboRouteType.ValueMember = "Key"
        Me.cboRouteType.DisplayMember = "Value"
        Me.cboRouteType.SelectedValue = My.Settings.Route_Type

        Me.cboDistanceType.DataSource = New BindingSource(DistanceTypes, Nothing)
        Me.cboDistanceType.ValueMember = "Key"
        Me.cboDistanceType.DisplayMember = "Value"
        Me.cboDistanceType.SelectedValue = My.Settings.Dist_Type


        Me.tbOrigAddress.Text = My.Settings.OrigAddress
        Me.tbOrigCity.Text = My.Settings.OrigCity
        Me.tbOrigState.Text = My.Settings.OrigState
        Me.tbOrigZip.Text = My.Settings.OrigZip

        Me.tbDestAddress.Text = My.Settings.DestAddress
        Me.tbDestCity.Text = My.Settings.DestCity
        Me.tbDestState.Text = My.Settings.DestState
        Me.tbDestZip.Text = My.Settings.DestZip

        Me.chkUseCom.Checked = My.Settings.UseCom

    End Sub

    Public Function getPracticalMilesCOM(ByVal objOrig As clsAddress, _
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
                                ByRef lBaddAddresses As List(Of clsPCMBadAddress), _
                                ByVal DebugMode As Boolean, _
                                ByVal LoggingOn As Boolean, _
                                ByVal KeepLogDays As Boolean, _
                                ByVal SaveOldLog As Boolean, _
                                ByVal LogFileName As String, _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As PCMComService.clsGlobalStopData = Nothing
        Dim oOrigCom As New PCMComService.clsAddress With {.strAddress = objOrig.strAddress, .strCity = objOrig.strCity, .strState = objOrig.strState, .strZip = objOrig.strZip}
        Dim oDestCom As New PCMComService.clsAddress With {.strAddress = objDest.strAddress, .strCity = objDest.strCity, .strState = objDest.strState, .strZip = objDest.strZip}
        Dim oArrBaddAddressesCom() As PCMComService.clsPCMBadAddress
        Dim oListBadAddress As New List(Of clsPCMBadAddress)
        Dim oRet As New clsGlobalStopData
        Try
            Using oPCmiles As New PCMComService.PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly

                oGlobalStopData = oPCmiles.getPracticalMiles(oOrigCom, oDestCom, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, oArrBaddAddressesCom)
                LastError = oPCmiles.LastError
            End Using
        Catch ex As Exception
            LastError = ex.Message
        End Try
        If Not oGlobalStopData Is Nothing Then
            With oGlobalStopData
                oRet.AutoCorrectBadLaneZipCodes = .AutoCorrectBadLaneZipCodes
                oRet.BadAddressCount = .BadAddressCount
                oRet.DestZip = .DestZip
                oRet.FailedAddressMessage = .FailedAddressMessage
                oRet.OriginZip = .OriginZip
                oRet.TotalMiles = .TotalMiles
            End With
            If Not oArrBaddAddressesCom Is Nothing AndAlso oArrBaddAddressesCom.Count() <> 0 Then
                For Each a In oArrBaddAddressesCom
                    Dim oNewBadAddress As New clsPCMBadAddress()
                    With oNewBadAddress
                        .BatchID = a.BatchID
                        .BookControl = a.BookControl
                        .LaneControl = a.LaneControl
                        .Message = a.Message
                        If Not a.objDest Is Nothing Then
                            .objDest = New clsAddress() With {.strAddress = a.objDest.strAddress, .strCity = a.objDest.strCity, .strState = a.objDest.strCity, .strZip = a.objDest.strZip}
                        End If
                        If Not a.objOrig Is Nothing Then
                            .objOrig = New clsAddress() With {.strAddress = a.objOrig.strAddress, .strCity = a.objOrig.strCity, .strState = a.objOrig.strCity, .strZip = a.objOrig.strZip}
                        End If

                        If Not a.objPCMDest Is Nothing Then
                            .objPCMDest = New clsAddress() With {.strAddress = a.objPCMDest.strAddress, .strCity = a.objPCMDest.strCity, .strState = a.objPCMDest.strCity, .strZip = a.objPCMDest.strZip}
                        End If
                        If Not a.objPCMOrig Is Nothing Then
                            .objPCMOrig = New clsAddress() With {.strAddress = a.objPCMOrig.strAddress, .strCity = a.objPCMOrig.strCity, .strState = a.objPCMOrig.strCity, .strZip = a.objPCMOrig.strZip}
                        End If
                    End With
                    lBaddAddresses.Add(oNewBadAddress)
                Next
            End If
        End If
        Return oRet
    End Function


    Public Function getPracticalMilesLocal(ByVal objOrig As clsAddress, _
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
                                ByRef lBaddAddresses As List(Of clsPCMBadAddress), _
                                ByVal DebugMode As Boolean, _
                                ByVal LoggingOn As Boolean, _
                                ByVal KeepLogDays As Boolean, _
                                ByVal SaveOldLog As Boolean, _
                                ByVal LogFileName As String, _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Dim arrBaddAddresses() As clsPCMBadAddress
        Try
            Dim oPCmiles As New PCMiles()
            With oPCmiles
                'Dim oPCmiles As New PCMiles
                .Debug = DebugMode
                .LoggingOn = LoggingOn
                .KeepLogDays = KeepLogDays
                .SaveOldLog = SaveOldLog
                .LogFileName = LogFileName
                .UseZipOnly = UseZipOnly

                oGlobalStopData = .getPracticalMiles(objOrig, objDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBaddAddresses)
                LastError = oPCmiles.LastError
                If Not oPCmiles.PCMBuffers Is Nothing AndAlso oPCmiles.PCMBuffers.Count() > 0 Then
                   
                    Me.tbPCMReport.Text = String.Join(vbCrLf, oPCmiles.PCMBuffers)
                End If
            End With
        Catch ex As Exception
            LastError = ex.Message
        End Try
       
        If Not arrBaddAddresses Is Nothing AndAlso arrBaddAddresses.Count() <> 0 Then
            For Each a In arrBaddAddresses
                Dim oNewBadAddress As New clsPCMBadAddress()
                With oNewBadAddress
                    .BatchID = a.BatchID
                    .BookControl = a.BookControl
                    .LaneControl = a.LaneControl
                    .Message = a.Message
                    If Not a.objDest Is Nothing Then
                        .objDest = New clsAddress() With {.strAddress = a.objDest.strAddress, .strCity = a.objDest.strCity, .strState = a.objDest.strCity, .strZip = a.objDest.strZip}
                    End If
                    If Not a.objOrig Is Nothing Then
                        .objOrig = New clsAddress() With {.strAddress = a.objOrig.strAddress, .strCity = a.objOrig.strCity, .strState = a.objOrig.strCity, .strZip = a.objOrig.strZip}
                    End If

                    If Not a.objPCMDest Is Nothing Then
                        .objPCMDest = New clsAddress() With {.strAddress = a.objPCMDest.strAddress, .strCity = a.objPCMDest.strCity, .strState = a.objPCMDest.strCity, .strZip = a.objPCMDest.strZip}
                    End If
                    If Not a.objPCMOrig Is Nothing Then
                        .objPCMOrig = New clsAddress() With {.strAddress = a.objPCMOrig.strAddress, .strCity = a.objPCMOrig.strCity, .strState = a.objPCMOrig.strCity, .strZip = a.objPCMOrig.strZip}
                    End If
                End With
                lBaddAddresses.Add(oNewBadAddress)
            Next
        End If

        Return oGlobalStopData
    End Function




    Private Sub btnGetMiles_Click(sender As Object, e As EventArgs) Handles btnGetMiles.Click

        Try
            Me.Cursor = Cursors.WaitCursor


            Me.tbMiles.Text = ""
            Me.lblErrors.Text = ""
            Me.lblPCMBadAddressMsg.Text = ""

            Me.tbPCMOrigAddress.Text = ""
            Me.tbPCMOrigCity.Text = ""
            Me.tbPCMOrigState.Text = ""
            Me.tbPCMOrigZip.Text = ""

            Me.tbPCMDestAddress.Text = ""
            Me.tbPCMDestCity.Text = ""
            Me.tbPCMDestState.Text = ""
            Me.tbPCMDestZip.Text = ""

            Dim objOrig As New clsAddress With {.strAddress = Me.tbOrigAddress.Text, .strCity = Me.tbOrigCity.Text, .strState = Me.tbOrigState.Text, .strZip = Me.tbOrigZip.Text}
            Dim objDest As New clsAddress With {.strAddress = Me.tbDestAddress.Text, .strCity = Me.tbDestCity.Text, .strState = Me.tbDestState.Text, .strZip = Me.tbDestZip.Text}
            Dim lBaddAddresses As New List(Of clsPCMBadAddress)
            Dim sLastError As String
            Dim intRouteType As Integer = Me.cboRouteType.SelectedValue
            If intRouteType < 0 Or intRouteType > 6 Then intRouteType = 0
            Dim intDistType As Integer = Me.cboDistanceType.SelectedValue
            If intDistType < 0 Or intDistType > 1 Then intDistType = 0
            Dim oreturn As New clsGlobalStopData()
            If Me.chkUseCom.Checked = True Then
                oreturn = getPracticalMilesCOM(objOrig, objDest, intRouteType, intDistType, 0, 0, 0, "", "", 0, 1, False, lBaddAddresses, True, False, False, False, "", False, sLastError)
            Else
                oreturn = getPracticalMilesLocal(objOrig, objDest, intRouteType, intDistType, 0, 0, 0, "", "", 0, 1, False, lBaddAddresses, True, False, False, False, "", False, sLastError)
            End If

            If Not oreturn Is Nothing Then
                Me.lblErrors.Text = sLastError
                Me.tbMiles.Text = oreturn.TotalMiles.ToString()
                If Not lBaddAddresses Is Nothing AndAlso lBaddAddresses.Count() > 0 Then

                    Me.lblPCMBadAddressMsg.Text = oreturn.FailedAddressMessage & vbCrLf & lBaddAddresses(0).Message
                    With lBaddAddresses(0)
                        Me.tbPCMOrigAddress.Text = .objPCMOrig.strAddress
                        Me.tbPCMOrigCity.Text = .objPCMOrig.strCity
                        Me.tbPCMOrigState.Text = .objPCMOrig.strState
                        Me.tbPCMOrigZip.Text = .objPCMOrig.strZip

                        Me.tbPCMDestAddress.Text = .objPCMDest.strAddress
                        Me.tbPCMDestCity.Text = .objPCMDest.strCity
                        Me.tbPCMDestState.Text = .objPCMDest.strState
                        Me.tbPCMDestZip.Text = .objPCMDest.strZip

                    End With
                End If

            End If
        Catch ex As Exception
            Me.lblErrors.Text = ex.Message

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnResequence_Click(sender As Object, e As EventArgs) Handles btnResequence.Click
        Dim oForm = Form2
        oForm.ShowDialog()

    End Sub

    Private Sub btnShop_Click(sender As Object, e As EventArgs) Handles btnShop.Click
        Dim oForm = Form3
        oForm.ShowDialog()
    End Sub
End Class
