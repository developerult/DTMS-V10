Imports System.Collections.ObjectModel
Imports PCMComService = NGL.Service.PCMiler64

'Imports System.Collections.

Public Class Form2

    Private _BadAddresses As New ObservableCollection(Of clsAddress)
    Public Property BadAddresses() As ObservableCollection(Of clsAddress)
        Get
            If _BadAddresses Is Nothing Then
                _BadAddresses = New ObservableCollection(Of clsAddress)
            End If
            Return _BadAddresses
        End Get
        Set(ByVal value As ObservableCollection(Of clsAddress))
            _BadAddresses = value
        End Set
    End Property

    Private _Stops As New ObservableCollection(Of clsAddress)
    Public Property Stops() As ObservableCollection(Of clsAddress)
        Get
            If _Stops Is Nothing Then
                _Stops = New ObservableCollection(Of clsAddress)
            End If
            Return _Stops
        End Get
        Set(ByVal value As ObservableCollection(Of clsAddress))
            _Stops = value
        End Set
    End Property

    Private _Solution As List(Of clsFMStopData)
    Public Property Solution() As List(Of clsFMStopData)
        Get
            If _Solution Is Nothing Then
                _Solution = New List(Of clsFMStopData)
            End If
            Return _Solution
        End Get
        Set(ByVal value As List(Of clsFMStopData))
            _Solution = value
        End Set
    End Property


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



    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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


        'Me.tbOrigAddress.Text = My.Settings.OrigAddress
        'Me.tbOrigCity.Text = My.Settings.OrigCity
        'Me.tbOrigState.Text = My.Settings.OrigState
        'Me.tbOrigZip.Text = My.Settings.OrigZip

        Me.tbStopAddress.Text = My.Settings.DestAddress
        Me.tbStopCity.Text = My.Settings.DestCity
        Me.tbStopState.Text = My.Settings.DestState
        Me.tbStopZip.Text = My.Settings.DestZip

        Me.chkUseCom.Checked = My.Settings.UseCom

        Dim oPickup As New clsAddress With {.strAddress = My.Settings.OrigAddress, .strCity = My.Settings.OrigCity, .strState = My.Settings.OrigState, .strZip = My.Settings.OrigZip}
        Stops.Add(oPickup)

        SetupGrids()

    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub SetupGrids()
        'Stops
        Me.dgvStops.AutoGenerateColumns = False
        Me.dgvStops.AutoSize = True
        Me.dgvStops.DataSource = Stops
        Me.dgvStops.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strAddress", .Name = "Street Address", .ReadOnly = True})
        Me.dgvStops.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strCity", .Name = "City", .ReadOnly = True})
        Me.dgvStops.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strState", .Name = "State/Province", .ReadOnly = True})
        Me.dgvStops.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strZip", .Name = "Postal Code", .ReadOnly = True})
        'Bad Addresses
        Me.dgvBadAddress.AutoGenerateColumns = False
        Me.dgvBadAddress.AutoSize = True
        Me.dgvBadAddress.DataSource = BadAddresses
        Me.dgvBadAddress.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strAddress", .Name = "Street Address", .ReadOnly = True})
        Me.dgvBadAddress.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strCity", .Name = "City", .ReadOnly = True})
        Me.dgvBadAddress.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strState", .Name = "State/Province", .ReadOnly = True})
        Me.dgvBadAddress.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "strZip", .Name = "Postal Code", .ReadOnly = True})
        'Solution
        Me.dgvSolution.AutoGenerateColumns = False
        Me.dgvSolution.AutoSize = True
        Me.dgvSolution.DataSource = Solution
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "StopNumber", .Name = "Stop #", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SeqNumber", .Name = "Seq #", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "StopName", .Name = "Name", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "LegMiles", .Name = "Leg Miles", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "TotalMiles", .Name = "Total Miles", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Street", .Name = "Street Address", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "City", .Name = "City", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "State", .Name = "State", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Zip", .Name = "Postal Code", .ReadOnly = True})
        Me.dgvSolution.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "AddressValid", .Name = "Valid", .ReadOnly = True})


    End Sub

    Private Sub updateBaddAddressInfo(ByVal lBaddAddresses As List(Of clsPCMBadAddress), ByVal FailedAddressMessage As String)
        Dim strMsg As String = FailedAddressMessage
        If Not lBaddAddresses Is Nothing AndAlso lBaddAddresses.Count() > 0 Then
            For Each b In lBaddAddresses

                strMsg &= b.Message

                BadAddresses.Add(New clsAddress With {.strAddress = b.objPCMOrig.strAddress, _
                                                      .strCity = b.objPCMOrig.strCity, _
                                                      .strState = b.objPCMOrig.strState, _
                                                      .strZip = b.objPCMOrig.strZip})

                BadAddresses.Add(New clsAddress With {.strAddress = b.objPCMDest.strAddress, _
                                                      .strCity = b.objPCMDest.strCity, _
                                                      .strState = b.objPCMDest.strState, _
                                                      .strZip = b.objPCMDest.strZip})


            Next
        End If
        Me.lblPCMBadAddressMsg.Text = strMsg
    End Sub

    Private Sub btnAddStop_Click(sender As Object, e As EventArgs) Handles btnAddStop.Click
        addStop(New clsAddress() With {.strAddress = Me.tbStopAddress.Text, .strCity = Me.tbStopCity.Text, .strState = Me.tbStopState.Text, .strZip = Me.tbStopZip.Text})
    End Sub

    Private Sub addStop(ByRef oStop As clsAddress)
        If oStop Is Nothing Then Return
        Me.dgvStops.DataSource = Nothing
        Me.Stops.Add(oStop)
        Me.dgvStops.DataSource = Stops
        Me.dgvStops.Refresh()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clearSolution()
    End Sub

    Private Sub clearSolution()
        Me.dgvStops.DataSource = Nothing
        Me.dgvStops.Refresh()
        Me.dgvBadAddress.DataSource = Nothing
        Me.dgvBadAddress.Refresh()
        Me.dgvSolution.DataSource = Nothing
        Me.dgvSolution.Refresh()
        Me.Stops = New ObservableCollection(Of clsAddress)
        Me.BadAddresses = New ObservableCollection(Of clsAddress)
        Me.Solution = New List(Of clsFMStopData)
        Me.lblErrors.Text = ""
        Me.lblPCMBadAddressMsg.Text = ""
    End Sub


    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        PCMReSyncMultiStop()
    End Sub

    Public Function PCMReSyncMultiStopCom(ByRef oIFMStops As clsFMStopData(), _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef oPCMReportRecords As clsPCMReportRecord(), _
                                ByVal DebugMode As Boolean, _
                                ByVal LoggingOn As Boolean, _
                                ByVal KeepLogDays As Boolean, _
                                ByVal SaveOldLog As Boolean, _
                                ByVal LogFileName As String, _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Dim strMsg As String
        Dim strskip As New List(Of String)
        Try
            If oIFMStops Is Nothing OrElse oIFMStops.Count() < 1 Then Return oGlobalStopData
            Dim oComIFMStops As New List(Of PCMComService.clsFMStopData)
            For Each s In oIFMStops
                Dim oComStop As New PCMComService.clsFMStopData
                oComStop = Trans.CopyMatchingFields(oComStop, s, strskip, strMsg)
                oComIFMStops.Add(oComStop)
            Next
            Dim oComIFMStopsArr = oComIFMStops.ToArray()
            Dim oCOMPCMReportRecords As PCMComService.clsPCMReportRecord()
            Dim oComResults As PCMComService.clsGlobalStopData
            Using oPCmiles As New PCMComService.PCMiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly

                oComResults = oPCmiles.PCMReSyncMultiStop(oComIFMStopsArr, dblBatchID, blnKeepStopNumbers, oCOMPCMReportRecords)
                LastError = oPCmiles.LastError
            End Using
            If Not oComIFMStopsArr Is Nothing AndAlso oComIFMStopsArr.count() > 0 Then
                Dim oLocalIFMStops As New List(Of clsFMStopData)
                For Each s In oComIFMStopsArr
                    Dim oLocalStop As New clsFMStopData
                    oLocalStop = Trans.CopyMatchingFields(oLocalStop, s, strskip, strMsg)
                    oLocalIFMStops.Add(oLocalStop)
                Next
                If Not oLocalIFMStops Is Nothing AndAlso oLocalIFMStops.Count() > 0 Then
                    oIFMStops = oLocalIFMStops.ToArray()
                End If
            End If
            If Not oCOMPCMReportRecords Is Nothing AndAlso oCOMPCMReportRecords.Count() > 0 Then
                Dim oLocalPCMReportRecords As New List(Of clsPCMReportRecord)
                For Each s In oCOMPCMReportRecords
                    Dim oLocalReport As New clsPCMReportRecord
                   
                    oLocalReport = Trans.CopyMatchingFields(oLocalReport, s, strskip, strMsg)
                    oLocalPCMReportRecords.Add(oLocalReport)
                Next
                If Not oLocalPCMReportRecords Is Nothing AndAlso oLocalPCMReportRecords.Count() > 0 Then
                    oPCMReportRecords = oLocalPCMReportRecords.ToArray()
                End If
            End If
            If Not oComResults Is Nothing Then
                oGlobalStopData = Trans.CopyMatchingFields(oGlobalStopData, oComResults, strskip, strMsg)
            End If

        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function



    Public Function PCMReSyncMultiStopLocal(ByRef oIFMStops As clsFMStopData(), _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef oPCMReportRecords As clsPCMReportRecord(), _
                                ByVal DebugMode As Boolean, _
                                ByVal LoggingOn As Boolean, _
                                ByVal KeepLogDays As Boolean, _
                                ByVal SaveOldLog As Boolean, _
                                ByVal LogFileName As String, _
                                ByVal UseZipOnly As Boolean, _
                                ByRef LastError As String) As clsGlobalStopData
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            Dim oPCmiles As New PCMiles()
            With oPCmiles
                'Dim oPCmiles As New PCMiles
                oPCmiles.Debug = DebugMode
                oPCmiles.LoggingOn = LoggingOn
                oPCmiles.KeepLogDays = KeepLogDays
                oPCmiles.SaveOldLog = SaveOldLog
                oPCmiles.LogFileName = LogFileName
                oPCmiles.UseZipOnly = UseZipOnly
                oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
                LastError = oPCmiles.LastError
            End With
        Catch ex As Exception
            LastError = ex.Message
        End Try
        Return oGlobalStopData
    End Function


    Private Sub PCMReSyncMultiStop(Optional ByVal blnKeepStopNumbers As Boolean = False)
        
        Dim intCompControl As Integer

        Dim oFMStopData As New List(Of clsFMStopData)
        'Dim oBadAddresses As New clsPCMBadAddresses
        Dim oPCMReportRecords As clsPCMReportRecord()
        Dim intPCMilerRouteType As Integer = Me.cboRouteType.SelectedValue
        Dim intPCMilerDistanceType As Integer = Me.cboDistanceType.SelectedValue
        Me.Cursor = Cursors.WaitCursor
        Try
            If Stops Is Nothing OrElse Stops.Count() < 2 Then Return


            'get the PCMiler settings for the first stop normally this is the origin we can only have one set of values
            intCompControl = 0
            Dim blnFirst As Boolean = True
            Dim intCount As Integer = 0
            For Each oStop In Stops
                Dim oFMStop As New clsFMStopData
                With oFMStop
                    .BookControl = 0
                    .BookCustCompControl = 0
                    .BookODControl = 0
                    .BookProNumber = ""
                    .City = oStop.strCity
                    .DistType = intPCMilerDistanceType
                    If blnFirst Then
                        .LocationisOrigin = True
                    Else
                        .LocationisOrigin = False
                    End If
                    .RouteType = intPCMilerRouteType
                    .State = oStop.strState
                    .StopNumber = intCount + 1
                    .Street = oStop.strAddress
                    .Zip = oStop.strZip
                End With
                oFMStopData.Add(oFMStop)
                intCount += 1
            Next
            'send  the data to PC Miler
            'get the parameter settings
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            Dim LastError As String = ""
            If oFMStopData Is Nothing OrElse oFMStopData.Count() < 1 Then Return
            Dim arrFMStopData As clsFMStopData() = oFMStopData.ToArray()
            Dim oReturn As clsGlobalStopData
            If Me.chkUseCom.Checked = True Then
                oReturn = PCMReSyncMultiStopCom(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            Else
                oReturn = PCMReSyncMultiStopLocal(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            End If

            If Not String.IsNullOrEmpty(LastError) Then Me.lblErrors.Text = LastError
            If Not arrFMStopData Is Nothing AndAlso arrFMStopData.Count() > 0 Then
                Solution = arrFMStopData.ToList()
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.DataSource = Solution
                Me.dgvSolution.Refresh()
                UpdateBadAddressList(Solution)
            Else
                BadAddresses = New ObservableCollection(Of clsAddress)
                Me.dgvBadAddress.DataSource = Nothing
                Me.dgvBadAddress.Refresh()
                Me.Solution = New List(Of clsFMStopData)
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.Refresh()
            End If



        Catch ex As Exception
            Me.lblErrors.Text = ex.Message
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub


    Private Sub UpdateBadAddressList(ByVal oStops As List(Of clsFMStopData))

        BadAddresses = New ObservableCollection(Of clsAddress)
        Me.dgvBadAddress.DataSource = Nothing
        Dim oBadStops As List(Of clsFMStopData) = (From d In oStops Where d.LogBadAddress = True Select d).ToList

        If oBadStops Is Nothing OrElse oBadStops.Count < 1 Then Return
        Dim strMsg As String = ""
        For Each oStop As clsFMStopData In oBadStops
            strMsg &= oStop.Warning
            BadAddresses.Add(New clsAddress With {.strAddress = oStop.PCMilerStreet, _
                                                     .strCity = oStop.PCMilerCity, _
                                                     .strState = oStop.PCMilerState, _
                                                     .strZip = oStop.PCMilerZip})

        Next
        Me.dgvBadAddress.DataSource = BadAddresses
        Me.dgvBadAddress.Refresh()
        
        Me.lblPCMBadAddressMsg.Text = strMsg

    End Sub

    Private Sub addFMStop(ByRef oFMStopData As List(Of clsFMStopData), ByRef oStop As clsAddress, ByVal oBook As clsBookData)

        If oFMStopData Is Nothing Then oFMStopData = New List(Of clsFMStopData)
        Dim oStopData As New clsFMStopData

        oStopData.AddressValid = False
        oStopData.BookControl = oBook.BookControl
        oStopData.BookCustCompControl = oBook.BookCustCompControl
        oStopData.BookLoadControl = oBook.BookLoadControl
        oStopData.BookODControl = oBook.BookODControl
        oStopData.BookProNumber = oBook.BookProNumber
        oStopData.City = oStop.strCity
        oStopData.DistType = Me.cboDistanceType.SelectedValue
        oStopData.LaneOriginAddressUse = oBook.LaneOriginAddressUse
        oStopData.LegCost =	0.0
        oStopData.LegESTCHG =	0.0
        oStopData.LegMiles =	0.0
        oStopData.LegTime =	
        oStopData.LegTolls =	0.0
        oStopData.LocationisOrigin = oBook.LocationisOrigin
        oStopData.LogBadAddress = oBook.LogBadAddress
        oStopData.Matched	= False
        oStopData.PCMilerCity =	Nothing
        oStopData.PCMilerState =	Nothing
        oStopData.PCMilerStreet =	Nothing
        oStopData.PCMilerZip =	Nothing
        oStopData.RouteType = Me.cboRouteType.SelectedValue
        oStopData.SeqNumber = oBook.SeqNumber
        oStopData.State = oStop.strState
        oStopData.StopName =	Nothing
        oStopData.StopNumber = oBook.StopNumber
        oStopData.Street = oStop.strAddress
        oStopData.TotalCost =	0.0
        oStopData.TotalESTCHG =	0.0
        oStopData.TotalMiles =	0.0
        oStopData.TotalTime =	
        oStopData.TotalTolls =	0.0
        oStopData.Warning =	Nothing
        oStopData.Zip = oStop.strZip

        oFMStopData.Add(oStopData)
    End Sub


    Private Sub btnCATest_Click(sender As Object, e As EventArgs) Handles btnCATest.Click
        Me.Cursor = Cursors.WaitCursor
        Dim blnKeepStopNumbers As Boolean = False
        Dim intCompControl As Integer

        Dim oFMStopData As New List(Of clsFMStopData)
        Dim oBooks As New List(Of clsBookData)

        Try
        clearSolution()
        'Add first test stop data to solution
        Dim oBook As New clsBookData With {.BookControl = 573245, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 49990, _
                                           .BookProNumber = "HCS253768", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = False, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 2}
        oBooks.Add(oBook)
        Dim oAddress As New clsAddress() With {.strAddress = "4767  27TH ST  SE", _
                                               .strCity = "CALGARY", _
                                               .strState = "AB", _
                                               .strZip = "T2B 3M5"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)
        'Add second test stop data to solution
        oBook = New clsBookData With {.BookControl = 573252, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 50578, _
                                           .BookProNumber = "HCS253775", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = False, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 2}
        oBooks.Add(oBook)
        oAddress = New clsAddress() With {.strAddress = "2525 29TH STREET NE", _
                                               .strCity = "CALGARY", _
                                               .strState = "AB", _
                                               .strZip = "T1Y 7B5"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)
        'Add third test stop data to solution
        oBook = New clsBookData With {.BookControl = 573182, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 52400, _
                                           .BookProNumber = "HCS253705", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = False, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 3}
        oBooks.Add(oBook)
        oAddress = New clsAddress() With {.strAddress = "71 Thornhill Drive", _
                                               .strCity = "DARTMOUTH", _
                                               .strState = "NS", _
                                               .strZip = "B3B 1R9"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)
        'Add fourth test stop data to solution
        oBook = New clsBookData With {.BookControl = 573245, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 49990, _
                                           .BookProNumber = "HCS253768", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = True, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 4}
        oBooks.Add(oBook)
        oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                               .strCity = "Lunenburg", _
                                               .strState = "NS", _
                                               .strZip = "B0J 2C0"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)

        'Add fifth test stop data to solution
        oBook = New clsBookData With {.BookControl = 573182, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 52400, _
                                           .BookProNumber = "HCS253705", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = True, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 4}
        oBooks.Add(oBook)
        oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                               .strCity = "Lunenburg", _
                                               .strState = "NS", _
                                               .strZip = "B0J 2C0"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)

        'Add sixth test stop data to solution for single pick test
        oBook = New clsBookData With {.BookControl = 573252, _
                                           .BookCustCompControl = 11875, _
                                           .BookLoadControl = 0, _
                                           .BookODControl = 50578, _
                                           .BookProNumber = "HCS253775", _
                                           .LaneOriginAddressUse = True, _
                                           .LocationisOrigin = True, _
                                           .LogBadAddress = False, _
                                           .SeqNumber = 0, _
                                           .StopNumber = 4}
        oBooks.Add(oBook)
        oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                               .strCity = "Lunenburg", _
                                               .strState = "NS", _
                                               .strZip = "B0J 2C0"}
        addStop(oAddress)
        addFMStop(oFMStopData, oAddress, oBook)


        'Dim oBadAddresses As New clsPCMBadAddresses
        Dim oPCMReportRecords As clsPCMReportRecord()
        Dim intPCMilerRouteType As Integer = Me.cboRouteType.SelectedValue
        Dim intPCMilerDistanceType As Integer = Me.cboDistanceType.SelectedValue

            If Stops Is Nothing OrElse Stops.Count() < 2 Then Return
            intCompControl = 11875
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            Dim LastError As String = ""
            If oFMStopData Is Nothing OrElse oFMStopData.Count() < 1 Then Return
            Dim arrFMStopData As clsFMStopData() = oFMStopData.ToArray()
            Dim oReturn As clsGlobalStopData
            If Me.chkUseCom.Checked = True Then
                oReturn = PCMReSyncMultiStopCom(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            Else
                oReturn = PCMReSyncMultiStopLocal(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            End If

            If Not String.IsNullOrEmpty(LastError) Then Me.lblErrors.Text = LastError
            If Not arrFMStopData Is Nothing AndAlso arrFMStopData.Count() > 0 Then
                Solution = arrFMStopData.ToList()
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.DataSource = Solution
                Me.dgvSolution.Refresh()
                UpdateBadAddressList(Solution)
            Else
                BadAddresses = New ObservableCollection(Of clsAddress)
                Me.dgvBadAddress.DataSource = Nothing
                Me.dgvBadAddress.Refresh()
                Me.Solution = New List(Of clsFMStopData)
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.Refresh()
            End If



        Catch ex As Exception
            Me.lblErrors.Text = ex.Message
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCA2PickTest_Click(sender As Object, e As EventArgs) Handles btnCA2PickTest.Click

        Me.Cursor = Cursors.WaitCursor
        Dim blnKeepStopNumbers As Boolean = False
        Dim intCompControl As Integer

        Dim oFMStopData As New List(Of clsFMStopData)
        Dim oBooks As New List(Of clsBookData)

        Try
            clearSolution()
            'Add first test stop data to solution
            Dim oBook As New clsBookData With {.BookControl = 573245, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 49990, _
                                               .BookProNumber = "HCS253768", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 2}
            oBooks.Add(oBook)
            Dim oAddress As New clsAddress() With {.strAddress = "4767  27TH ST  SE", _
                                                   .strCity = "CALGARY", _
                                                   .strState = "AB", _
                                                   .strZip = "T2B 3M5"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)
            'Add second test stop data to solution
            oBook = New clsBookData With {.BookControl = 573252, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 50578, _
                                               .BookProNumber = "HCS253775", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 2}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "2525 29TH STREET NE", _
                                                   .strCity = "CALGARY", _
                                                   .strState = "AB", _
                                                   .strZip = "T1Y 7B5"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)
            'Add third test stop data to solution
            oBook = New clsBookData With {.BookControl = 573182, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 52400, _
                                               .BookProNumber = "HCS253705", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 3}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "71 Thornhill Drive", _
                                                   .strCity = "DARTMOUTH", _
                                                   .strState = "NS", _
                                                   .strZip = "B3B 1R9"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)
            'Add fourth test stop data to solution
            oBook = New clsBookData With {.BookControl = 573245, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 49990, _
                                               .BookProNumber = "HCS253768", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 4}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                                   .strCity = "Lunenburg", _
                                                   .strState = "NS", _
                                                   .strZip = "B0J 2C0"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)

            'Add fifth test stop data to solution
            oBook = New clsBookData With {.BookControl = 573182, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 52400, _
                                               .BookProNumber = "HCS253705", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 4}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                                   .strCity = "Lunenburg", _
                                                   .strState = "NS", _
                                                   .strZip = "B0J 2C0"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)

            'Add sixth test stop data to solution for multi-pick test
            oBook = New clsBookData With {.BookControl = 573252, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 50578, _
                                               .BookProNumber = "HCS253775", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 4}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "2660 Meadowpine Blvd.", _
                                                   .strCity = "Mississauga", _
                                                   .strState = "ON", _
                                                   .strZip = "L5N 7E6"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)


            'Dim oBadAddresses As New clsPCMBadAddresses
            Dim oPCMReportRecords As clsPCMReportRecord()
            Dim intPCMilerRouteType As Integer = Me.cboRouteType.SelectedValue
            Dim intPCMilerDistanceType As Integer = Me.cboDistanceType.SelectedValue

            If Stops Is Nothing OrElse Stops.Count() < 2 Then Return
            intCompControl = 11875
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            Dim LastError As String = ""
            If oFMStopData Is Nothing OrElse oFMStopData.Count() < 1 Then Return
            Dim arrFMStopData As clsFMStopData() = oFMStopData.ToArray()
            Dim oReturn As clsGlobalStopData
            If Me.chkUseCom.Checked = True Then
                oReturn = PCMReSyncMultiStopCom(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            Else
                oReturn = PCMReSyncMultiStopLocal(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            End If

            If Not String.IsNullOrEmpty(LastError) Then Me.lblErrors.Text = LastError
            If Not arrFMStopData Is Nothing AndAlso arrFMStopData.Count() > 0 Then
                Solution = arrFMStopData.ToList()
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.DataSource = Solution
                Me.dgvSolution.Refresh()
                UpdateBadAddressList(Solution)
            Else
                BadAddresses = New ObservableCollection(Of clsAddress)
                Me.dgvBadAddress.DataSource = Nothing
                Me.dgvBadAddress.Refresh()
                Me.Solution = New List(Of clsFMStopData)
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.Refresh()
            End If



        Catch ex As Exception
            Me.lblErrors.Text = ex.Message
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub



    Private Sub btnCAKeepStopTest_Click(sender As Object, e As EventArgs) Handles btnCAKeepStopTest.Click

        Me.Cursor = Cursors.WaitCursor
        Dim blnKeepStopNumbers As Boolean = True
        Dim intCompControl As Integer

        Dim oFMStopData As New List(Of clsFMStopData)
        Dim oBooks As New List(Of clsBookData)

        Try
            clearSolution()
            'Add first test stop data to solution for multi-pick test
            Dim oBook As New clsBookData With {.BookControl = 573252, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 50578, _
                                               .BookProNumber = "HCS253775", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 1}
            oBooks.Add(oBook)
            Dim oAddress As New clsAddress() With {.strAddress = "2660 Meadowpine Blvd.", _
                                                   .strCity = "Mississauga", _
                                                   .strState = "ON", _
                                                   .strZip = "L5N 7E6"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)

            'Add second test stop data to solution
            oBook = New clsBookData With {.BookControl = 573182, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 52400, _
                                               .BookProNumber = "HCS253705", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 2}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "71 Thornhill Drive", _
                                                   .strCity = "DARTMOUTH", _
                                                   .strState = "NS", _
                                                   .strZip = "B3B 1R9"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)

            'Add third test stop data to solution
            oBook = New clsBookData With {.BookControl = 573245, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 49990, _
                                               .BookProNumber = "HCS253768", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 3}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                                   .strCity = "Lunenburg", _
                                                   .strState = "NS", _
                                                   .strZip = "B0J 2C0"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)

            'Add fourth test stop data to solution
            oBook = New clsBookData With {.BookControl = 573182, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 52400, _
                                               .BookProNumber = "HCS253705", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = True, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 3}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "100 Battery Point", _
                                                   .strCity = "Lunenburg", _
                                                   .strState = "NS", _
                                                   .strZip = "B0J 2C0"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)


            'Add fifth test stop data to solution
            oBook = New clsBookData With {.BookControl = 573245, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 49990, _
                                               .BookProNumber = "HCS253768", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 4}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "4767  27TH ST  SE", _
                                                   .strCity = "CALGARY", _
                                                   .strState = "AB", _
                                                   .strZip = "T2B 3M5"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)
            'Add sixth test stop data to solution
            oBook = New clsBookData With {.BookControl = 573252, _
                                               .BookCustCompControl = 11875, _
                                               .BookLoadControl = 0, _
                                               .BookODControl = 50578, _
                                               .BookProNumber = "HCS253775", _
                                               .LaneOriginAddressUse = True, _
                                               .LocationisOrigin = False, _
                                               .LogBadAddress = False, _
                                               .SeqNumber = 0, _
                                               .StopNumber = 5}
            oBooks.Add(oBook)
            oAddress = New clsAddress() With {.strAddress = "2525 29TH STREET NE", _
                                                   .strCity = "CALGARY", _
                                                   .strState = "AB", _
                                                   .strZip = "T1Y 7B5"}
            addStop(oAddress)
            addFMStop(oFMStopData, oAddress, oBook)




            'Dim oBadAddresses As New clsPCMBadAddresses
            Dim oPCMReportRecords As clsPCMReportRecord()
            Dim intPCMilerRouteType As Integer = Me.cboRouteType.SelectedValue
            Dim intPCMilerDistanceType As Integer = Me.cboDistanceType.SelectedValue

            If Stops Is Nothing OrElse Stops.Count() < 2 Then Return
            intCompControl = 11875
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            Dim LastError As String = ""
            If oFMStopData Is Nothing OrElse oFMStopData.Count() < 1 Then Return
            Dim arrFMStopData As clsFMStopData() = oFMStopData.ToArray()
            Dim oReturn As clsGlobalStopData
            If Me.chkUseCom.Checked = True Then
                oReturn = PCMReSyncMultiStopCom(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            Else
                oReturn = PCMReSyncMultiStopLocal(arrFMStopData, 1, blnKeepStopNumbers, oPCMReportRecords, True, False, 0, False, "", False, LastError)
            End If

            If Not String.IsNullOrEmpty(LastError) Then Me.lblErrors.Text = LastError
            If Not arrFMStopData Is Nothing AndAlso arrFMStopData.Count() > 0 Then
                Solution = arrFMStopData.ToList()
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.DataSource = Solution
                Me.dgvSolution.Refresh()
                UpdateBadAddressList(Solution)
            Else
                BadAddresses = New ObservableCollection(Of clsAddress)
                Me.dgvBadAddress.DataSource = Nothing
                Me.dgvBadAddress.Refresh()
                Me.Solution = New List(Of clsFMStopData)
                Me.dgvSolution.DataSource = Nothing
                Me.dgvSolution.Refresh()
            End If



        Catch ex As Exception
            Me.lblErrors.Text = ex.Message
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

End Class