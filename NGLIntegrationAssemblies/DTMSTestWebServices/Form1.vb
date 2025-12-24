Public Class Form1

    Private _BookWebMethod As Integer
    Public Property BookWebMethod() As Integer
        Get
            Return _BookWebMethod
        End Get
        Set(ByVal value As Integer)
            _BookWebMethod = value
        End Set
    End Property

    Public Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.tbAuthCode.Text = My.Settings.AuthCode
        Me.tbBookObjectWSURL.Text = My.Settings.DTMSTestWebServices_NGLBookObjectWS_BookObject.ToString()
        Me.tbLaneObjectWSURL.Text = My.Settings.DTMSTestWebServices_NGLLaneObjectWS_LaneObject.ToString()
        Me.tbERPObjectWSURL.Text = My.Settings.DTMSTestWebServices_NGLDTMSERPIntegrationWS_DTMSERPIntegration.ToString()
        'hide/remove unused tabs
        Me.TabControl1.TabPages.Remove(TabPage3)
        Me.TabControl1.TabPages.Remove(TabPage4)
        Me.TabControl1.TabPages.Remove(TabPage5)
        Me.TabControl1.TabPages.Remove(TabPage6)
        Me.TabControl1.TabPages.Remove(TabPage7)
        Me.TabControl1.TabPages.Remove(TabPage8)

    End Sub

    Private Sub btnBookRun_Click(sender As Object, e As EventArgs) Handles btnBookRun.Click
        Me.tbBookResults.Text = "Running"
        System.Threading.Thread.Sleep(1000)
        Select Case BookWebMethod

            Case 1 ' ProcessData 
                ProcessData()
            Case 2 'ProcessDataEx 
                ProcessDataEx()
            Case 3 'ProcessData60 
                ProcessData60()
            Case 4 'ProcessData70 
                ProcessData70()
            Case 5 'ProcessBookData70 
                ProcessBookData70()
            Case 6 'ProcessBookData705 
                ProcessBookData705()
            Case Else
                ProcessBookData80()
        End Select
        'Dim xbval = Me.gbBookProcessDataOptions.
        SaveURLSettings()

    End Sub

    Public Sub SaveURLSettings()
        My.Settings.DTMSTestWebServices_NGLBookObjectWS_BookObject = Me.tbBookObjectWSURL.Text
        My.Settings.AuthCode = Me.tbAuthCode.Text
        My.Settings.DTMSTestWebServices_NGLLaneObjectWS_LaneObject = Me.tbLaneObjectWSURL.Text
        My.Settings.DTMSTestWebServices_NGLDTMSERPIntegrationWS_DTMSERPIntegration = Me.tbERPObjectWSURL.Text
    End Sub



    Private Sub ProcessBookData80()
        Dim oBookHeader As New NGLDTMSERPIntegrationWS.clsBookHeaderObject80()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLDTMSERPIntegrationWS.clsBookDetailObject80()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .POItemPallets = 1
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLDTMSERPIntegrationWS.clsBookHeaderObject80 = New NGLDTMSERPIntegrationWS.clsBookHeaderObject80(0) {oBookHeader}

        Dim oBookItems() As NGLDTMSERPIntegrationWS.clsBookDetailObject80 = New NGLDTMSERPIntegrationWS.clsBookDetailObject80(0) {oBookDetail}

        Dim oIntegrationObject As NGLDTMSERPIntegrationWS.DTMSERPIntegration = New NGLDTMSERPIntegrationWS.DTMSERPIntegration()

        oIntegrationObject.Url = Me.tbERPObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oIntegrationObject.ProcessBookData80(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessBookData705()
        Dim oBookHeader As New NGLDTMSERPIntegrationWS.clsBookHeaderObject705()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLDTMSERPIntegrationWS.clsBookDetailObject705()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .POItemPallets = 1
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLDTMSERPIntegrationWS.clsBookHeaderObject705 = New NGLDTMSERPIntegrationWS.clsBookHeaderObject705(0) {oBookHeader}

        Dim oBookItems() As NGLDTMSERPIntegrationWS.clsBookDetailObject705 = New NGLDTMSERPIntegrationWS.clsBookDetailObject705(0) {oBookDetail}

        Dim oIntegrationObject As NGLDTMSERPIntegrationWS.DTMSERPIntegration = New NGLDTMSERPIntegrationWS.DTMSERPIntegration()

        oIntegrationObject.Url = Me.tbERPObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oIntegrationObject.ProcessBookData705(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessBookData70()
        Dim oBookHeader As New NGLDTMSERPIntegrationWS.clsBookHeaderObject70()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLDTMSERPIntegrationWS.clsBookDetailObject70()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .POItemPallets = 1
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLDTMSERPIntegrationWS.clsBookHeaderObject70 = New NGLDTMSERPIntegrationWS.clsBookHeaderObject70(0) {oBookHeader}

        Dim oBookItems() As NGLDTMSERPIntegrationWS.clsBookDetailObject70 = New NGLDTMSERPIntegrationWS.clsBookDetailObject70(0) {oBookDetail}

        Dim oIntegrationObject As NGLDTMSERPIntegrationWS.DTMSERPIntegration = New NGLDTMSERPIntegrationWS.DTMSERPIntegration()

        oIntegrationObject.Url = Me.tbERPObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oIntegrationObject.ProcessBookData70(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessData70()
        Dim oBookHeader As New NGLBookObjectWS.clsBookHeaderObject70()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLBookObjectWS.clsBookDetailObject70()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .POItemPallets = 1
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLBookObjectWS.clsBookHeaderObject70 = New NGLBookObjectWS.clsBookHeaderObject70(0) {oBookHeader}

        Dim oBookItems() As NGLBookObjectWS.clsBookDetailObject70 = New NGLBookObjectWS.clsBookDetailObject70(0) {oBookDetail}

        Dim oBookObject As NGLBookObjectWS.BookObject = New NGLBookObjectWS.BookObject()

        oBookObject.Url = Me.tbBookObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oBookObject.ProcessData70(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessData60()
        Dim oBookHeader As New NGLBookObjectWS.clsBookHeaderObject60()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLBookObjectWS.clsBookDetailObject60()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .POItemPallets = 1
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLBookObjectWS.clsBookHeaderObject60 = New NGLBookObjectWS.clsBookHeaderObject60(0) {oBookHeader}

        Dim oBookItems() As NGLBookObjectWS.clsBookDetailObject60 = New NGLBookObjectWS.clsBookDetailObject60(0) {oBookDetail}

        Dim oBookObject As NGLBookObjectWS.BookObject = New NGLBookObjectWS.BookObject()

        oBookObject.Url = Me.tbBookObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oBookObject.ProcessData60(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessDataEx()
        Dim oBookHeader As New NGLBookObjectWS.clsBookHeaderObject()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLBookObjectWS.clsBookDetailObject()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLBookObjectWS.clsBookHeaderObject = New NGLBookObjectWS.clsBookHeaderObject(0) {oBookHeader}

        Dim oBookItems() As NGLBookObjectWS.clsBookDetailObject = New NGLBookObjectWS.clsBookDetailObject(0) {oBookDetail}

        Dim oBookObject As NGLBookObjectWS.BookObject = New NGLBookObjectWS.BookObject()

        oBookObject.Url = Me.tbBookObjectWSURL.Text
        Dim ReturnMessage As String = ""
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oBookObject.ProcessDataEx(WebAuthCode, oBookHeaders, oBookItems, ReturnMessage)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub ProcessData()
        Dim oBookHeader As New NGLBookObjectWS.clsBookHeaderObject()
        With oBookHeader
            .PONumber = Me.tbBookOrderNo.Text
            .POVendor = Me.tbBookLaneNo.Text
            .POdate = Date.Now()
            .POShipdate = Date.Now()
            .POWgt = 100
            .POCube = 100
            .POQty = 100
            .POPallets = 1
            .PODefaultCustomer = Me.tbBookCompNo.Text
            .POReqDate = Date.Now()
            .POShipInstructions = "Test"
            .POCooler = False
            .POFrozen = False
            .PODry = True
            .POTemp = "D"
            .POStatusFlag = 0
            .POOrderSequence = 0
        End With
        Dim oBookDetail = New NGLBookObjectWS.clsBookDetailObject()
        With oBookDetail
            .Description = "test"
            .FreightCost = 100
            .ItemCost = 100
            .ItemNumber = "Test 1"
            .ItemPONumber = Me.tbBookOrderNo.Text()
            .PalletType = "N"
            .QtyOrdered = 100
            .Weight = 100
            .CustomerNumber = Me.tbBookCompNo.Text
            .Cube = 100
        End With
        'add the data to an array
        Dim oBookHeaders() As NGLBookObjectWS.clsBookHeaderObject = New NGLBookObjectWS.clsBookHeaderObject(0) {oBookHeader}

        Dim oBookItems() As NGLBookObjectWS.clsBookDetailObject = New NGLBookObjectWS.clsBookDetailObject(0) {oBookDetail}

        Dim oBookObject As NGLBookObjectWS.BookObject = New NGLBookObjectWS.BookObject()

        oBookObject.Url = Me.tbBookObjectWSURL.Text
        Dim ReturnMessage As String = "No Details Available"
        Dim WebAuthCode As String = Me.tbAuthCode.Text
        Dim NGLRet As Integer = oBookObject.ProcessData(WebAuthCode, oBookHeaders, oBookItems)

        Select Case NGLRet
            Case WebServiceReturnValues.nglDataConnectionFailure
                Me.tbBookResults.Text = "Database Connection Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationFailure
                Me.tbBookResults.Text = "Data Integration Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                Me.tbBookResults.Text = "Some Errors " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataValidationFailure
                Me.tbBookResults.Text = "Data Validation Failure Error " & vbCrLf & ReturnMessage
            Case WebServiceReturnValues.nglDataIntegrationComplete
                Me.tbBookResults.Text = "Success! Book Data imported." & vbCrLf & "Return Message: " & ReturnMessage
            Case Else
                Me.tbBookResults.Text = "Invalid Return Value."
        End Select
    End Sub

    Private Sub rbBookProcessData_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessData.CheckedChanged
        BookWebMethod = 1
    End Sub

    Private Sub rbProcessDataEx_CheckedChanged(sender As Object, e As EventArgs) Handles rbProcessDataEx.CheckedChanged
        BookWebMethod = 2
    End Sub

    Private Sub rbBookProcessData60_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessData60.CheckedChanged
        BookWebMethod = 3

    End Sub

    Private Sub rbBookProcessData70_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessData70.CheckedChanged
        BookWebMethod = 4
    End Sub

    Private Sub rbBookProcessBookData70_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessBookData70.CheckedChanged
        BookWebMethod = 5
    End Sub

    Private Sub rbBookProcessBookData705_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessBookData705.CheckedChanged
        BookWebMethod = 6
    End Sub

    Private Sub rbBookProcessBookData80_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookProcessBookData80.CheckedChanged
        BookWebMethod = 7
    End Sub
End Class
