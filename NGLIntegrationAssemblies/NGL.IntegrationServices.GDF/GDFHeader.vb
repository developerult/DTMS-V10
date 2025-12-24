Imports System
Imports NGL.IntegrationServices.Utilities

Public Class GDFHeaders
    Implements IHeaders

    Private _Errors As String = ""
    Public Property Errors() As String Implements IHeaders.Errors
        Get
            Return _Errors
        End Get
        Set(ByVal value As String)
            _Errors = value
        End Set
    End Property

    Private _Items As List(Of Object)
    Public Property Items() As List(Of Object) Implements IHeaders.Items
        Get
            Return _Items
        End Get
        Set(ByVal value As List(Of Object))
            _Items = value
        End Set
    End Property

    Public Function ReadFromFile(ByVal fileName As String, Optional ByVal Details As List(Of Object) = Nothing) As List(Of Object) Implements IHeaders.ReadFromFile
        Items = New List(Of Object)
        Try

            Dim strData As String = ""
            Dim errMsg As String = ""
            Dim HRecords() As String = splitLines(readFileToEnd(fileName, errMsg))
            If Not HRecords Is Nothing AndAlso HRecords.Count > 0 Then
                For Each r In HRecords
                    Try
                        Dim strRow() As String = DecodeCSV(r)
                        If strRow.Length >= 29 Then
                            Dim H As New GDFHeader
                            With H
                                .PONumber = strRow(0)
                                .POVendor = strRow(1)
                                .POdate = strRow(2)
                                .POShipdate = strRow(3)
                                .POBuyer = strRow(4)
                                .POFrt = strRow(5)
                                .POTotalFrt = strRow(6)
                                .POTotalCost = strRow(7)
                                .POWgt = strRow(8)
                                .POCube = strRow(9)
                                .POQty = strRow(10)
                                .POPallets = strRow(11)
                                .POLines = strRow(12)
                                .POConfirm = strRow(13)
                                .PODefaultCustomer = strRow(14)
                                .PODefaultCarrier = strRow(15)
                                .POReqDate = strRow(16)
                                .POShipInstructions = strRow(17)
                                .POCooler = strRow(18)
                                .POFrozen = strRow(19)
                                .PODry = strRow(20)
                                .POTemp = strRow(21)
                                .POCarType = strRow(22)
                                .POShipVia = strRow(23)
                                .POShipViaType = strRow(24)
                                .POConsigneeNumber = strRow(25)
                                .POCustomerPO = strRow(26)
                                .POOtherCosts = strRow(27)
                                .POStatusFlag = strRow(28)
                            End With
                            If Not Details Is Nothing AndAlso Details.Count > 0 Then
                                Dim HDetails As List(Of Object) = (From d In Details Where d.SalesOrderNumber = H.PONumber).ToList()
                                H.Details = New GDFDetails
                                H.Details.Items = HDetails
                            End If
                            Items.Add(H)
                        Else
                            Errors &= "GDFHeader.ReadFromFile Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
                        End If
                    Catch ex As ApplicationException
                        Errors &= "GDFHeader.ReadFromFile parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
                    End Try
                Next
            Else
                Errors &= errMsg
            End If

        Catch ex As Exception
            Errors &= "GDFHeader.ReadFromFile Unexpected Error: " & ex.Message & vbCrLf & vbCrLf
        End Try
        Return Items

    End Function

    Public Function buildBookObjectData(ByVal vHeaders As IHeaders, _
                                        ByRef ErrMsg As String, _
                                        ByRef headerList As List(Of NGLBookWebService.clsBookHeaderObject), _
                                        ByRef detailList As List(Of NGLBookWebService.clsBookDetailObject)) As Boolean Implements IHeaders.buildBookObjectData
        Dim blnRet As Boolean = False

        Try

            For Each H As GDFHeader In vHeaders.Items

                Dim oH As New NGLBookWebService.clsBookHeaderObject
                With oH
                    .PONumber = H.PONumber
                    .POVendor = H.POVendor
                    .POdate = H.POdate
                    .POShipdate = H.POShipdate
                    .POBuyer = H.POBuyer
                    Short.TryParse(H.POFrt, .POFrt)
                    Double.TryParse(H.POTotalFrt, .POTotalFrt)
                    Double.TryParse(H.POTotalCost, .POTotalCost)
                    Double.TryParse(H.POWgt, .POWgt)
                    Double.TryParse(H.POCube, .POCube)
                    .POQty = CastToInteger(H.POQty, 0)
                    .POPallets = CastToInteger(H.POPallets, 0)
                    Double.TryParse(H.POLines, .POLines)
                    Boolean.TryParse(H.POConfirm, .POConfirm)
                    .PODefaultCustomer = H.PODefaultCustomer
                    .PODefaultCarrier = CastToInteger(H.PODefaultCarrier, 0)
                    Date.TryParse(H.POReqDate, .POReqDate)
                    .POShipInstructions = H.POShipInstructions
                    Boolean.TryParse(H.POCooler, .POCooler)
                    Boolean.TryParse(H.POFrozen, .POFrozen)
                    Boolean.TryParse(H.PODry, .PODry)
                    .POTemp = H.POTemp
                    .POCarType = H.POCarType
                    .POShipVia = H.POShipVia
                    .POShipViaType = H.POShipViaType
                    .POConsigneeNumber = H.POConsigneeNumber
                    .POCustomerPO = H.POCustomerPO
                    Double.TryParse(H.POOtherCosts, .POOtherCosts)
                    .POStatusFlag = CastToInteger(H.POStatusFlag, 0)
                End With

                headerList.Add(oH)
                'build the POItem data
                For Each D As GDFDetail In H.Details.Items
                    Dim oI As New NGLBookWebService.clsBookDetailObject
                    With oI
                        .ItemPONumber = D.ItemPONumber
                        Double.TryParse(D.FixOffInvAllow, .FixOffInvAllow)
                        Double.TryParse(D.FixFrtAllow, .FixFrtAllow)
                        .ItemNumber = D.ItemNumber
                        .QtyOrdered = CastToInteger(D.QtyOrdered, 0)
                        Double.TryParse(D.FreightCost, .FreightCost)
                        Double.TryParse(D.ItemCost, .ItemCost)
                        Double.TryParse(D.Weight, .Weight)
                        .Cube = CastToInteger(D.Cube, 0)
                        Short.TryParse(D.Pack, .Pack)
                        .Size = D.Size
                        .Description = D.Description
                        .Hazmat = D.Hazmat
                        .Brand = D.Brand
                        .CostCenter = D.CostCenter
                        .LotNumber = D.LotNumber
                        .LotExpirationDate = D.LotExpirationDate
                        .GTIN = D.GTIN
                        .CustItemNumber = D.CustItemNumber
                        .CustomerNumber = D.CustomerNumber
                    End With
                    detailList.Add(oI)
                Next

            Next
            blnRet = True
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function


    Public Overrides Function ToString() As String
        Dim strRet As String = ""
        For Each H In Items
            strRet &= H.ToString & vbCrLf & vbCrLf
        Next
        Return strRet
    End Function

End Class

Public Class GDFHeader


    Public Details As GDFDetails
    Public PONumber As String
    Public POVendor As String
    Public POdate As String
    Public POShipdate As String
    Public POBuyer As String
    Public POFrt As String
    Public POTotalFrt As String
    Public POTotalCost As String
    Public POWgt As String
    Public POCube As String
    Public POQty As String
    Public POPallets As String
    Public POLines As String
    Public POConfirm As String
    Public PODefaultCustomer As String
    Public PODefaultCarrier As String
    Public POReqDate As String
    Public POShipInstructions As String
    Public POCooler As String
    Public POFrozen As String
    Public PODry As String
    Public POTemp As String
    Public POCarType As String
    Public POShipVia As String
    Public POShipViaType As String
    Public POConsigneeNumber As String
    Public POCustomerPO As String
    Public POOtherCosts As String
    Public POStatusFlag As String

    'Public ReadOnly Property dtOrderDate() As Date
    '    Get
    '        Return CastToDate(OrderDate, Date.Now.ToShortDateString)
    '    End Get
    'End Property

    'Public Function LaneNumber(ByVal Company As String) As String
    '    Dim strRet As String = Company.Trim & "-" & CustomerNumber.Trim
    '    If Not String.IsNullOrEmpty(LocationNumber.Trim) Then
    '        strRet &= "-" & LocationNumber.Trim
    '    End If
    '    Return strRet
    'End Function

    'Public Function TotalWeight() As Double
    '    Dim TWeight = Details.Items.Sum(Function(d) d.dblTotalWeight)
    '    Return CastToDouble(TWeight.ToString)
    'End Function

    'Public Function TotalCost() As Double
    '    Dim TCost = Details.Items.Sum(Function(d) (d.dblCost * d.intQty))
    '    Return CastToDouble(TCost.ToString)
    'End Function

    'Public Function TotalQty() As Integer
    '    Dim TQty = Details.Items.Sum(Function(d) d.intQty)
    '    Return CastToInteger(TQty.ToString)
    'End Function

    'Public Function TotalPLTS() As Integer
    '    Dim Pallets As List(Of Object) = (From i In Details.Items Where i.ItemNumber.ToUpper = "PALLETS" Select i).ToList
    '    Dim TPlts = Pallets.Sum(Function(p) p.intQty)
    '    Return CastToInteger(TPlts.ToString)
    'End Function

    'Public Function Temp() As String
    '    Return Details.getTempTypeText()
    'End Function

    'Public Overrides Function ToString() As String
    '    Dim strRet As String = SalesOrderNumber & " | " _
    '                           & OrderDate & " | " _
    '                           & PONumber & " | " _
    '                           & CustomerNumber & " | " _
    '                           & LocationNumber & " | " _
    '                           & DestinationName & " | " _
    '                           & DestinationAddress1 & " | " _
    '                           & DestinationAddress2 & " | " _
    '                           & DestinationAddress3 & " | " _
    '                           & DestinationCity & " | " _
    '                           & DestinationState & " | " _
    '                           & DestinationZip & " | " _
    '                           & Latitude & " | " _
    '                           & Longitude & " | " _
    '                           & ShipVia & " | "
    '    For Each d In Details.Items
    '        strRet &= vbTab & d.ToString
    '    Next

    '    Return strRet
    'End Function

End Class
