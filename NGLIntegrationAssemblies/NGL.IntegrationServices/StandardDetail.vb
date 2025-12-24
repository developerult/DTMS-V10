Imports System
Imports NGL.IntegrationServices.Utilities

Public Class StandardDetails
    Implements IDetails
     
    Private _Errors As String = ""
    Public Property Errors() As String Implements IDetails.Errors
        Get
            Return _Errors
        End Get
        Set(ByVal value As String)
            _Errors = value
        End Set
    End Property
      
    Private _Items As List(Of Object)
    Public Property Items() As List(Of Object) Implements IDetails.Items
        Get
            Return _Items
        End Get
        Set(ByVal value As List(Of Object))
            _Items = value
        End Set
    End Property


    Public ReadOnly Property Count() As Integer
        Get
            If Items Is Nothing Then
                Return 0
            Else
                Return Items.Count
            End If
        End Get
    End Property

    Public ReadOnly Property SQLItemNumberList() As String
        Get
            If Items Is Nothing Then
                Return "''"
            Else
                Dim strRet As String = ""
                Dim strspacer As String = ""
                For Each d In Items
                    strRet &= strspacer & "'" & d.ItemNumber & "'"
                    strspacer = ","
                Next
                Return strRet
            End If
        End Get
    End Property

    Public Function ReadFromString(ByVal Data As String) As List(Of Object) Implements IDetails.ReadFromString
        Items = New List(Of Object)
        Try
            Dim strDetails As List(Of String) = readDataToList(Data)
            If Not strDetails Is Nothing AndAlso strDetails.Count > 0 Then
                For Each r As String In strDetails
                    Try
                        Dim strRow() As String = DecodeCSV(r)
                        If strRow.Length >= 20 Then
                            Dim D As New StandardDetail
                            With D
                                .ItemPONumber = strRow(0)
                                .FixOffInvAllow = strRow(1)
                                .FixFrtAllow = strRow(2)
                                .ItemNumber = strRow(3)
                                .QtyOrdered = strRow(4)
                                .FreightCost = strRow(5)
                                .ItemCost = strRow(6)
                                .Weight = strRow(7)
                                .Cube = strRow(8)
                                .Pack = strRow(9)
                                .Size = strRow(10)
                                .Description = strRow(11)
                                .Hazmat = strRow(12)
                                .Brand = strRow(13)
                                .CostCenter = strRow(14)
                                .LotNumber = strRow(15)
                                .LotExpirationDate = strRow(16)
                                .GTIN = strRow(17)
                                .CustItemNumber = strRow(18)
                                .CustomerNumber = strRow(19)
                            End With
                            Items.Add(D)
                        Else
                            Errors &= "StandardDetail.ReadFromString Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
                        End If
                    Catch ex As ApplicationException
                        Errors &= "StandardDetail.ReadFromString parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
                    End Try
                Next
            End If
        Catch ex As Exception
            Errors &= "StandardDetail.ReadFromString Unexpected Error: " & ex.Message & vbCrLf & vbCrLf

        End Try

        Return Items
    End Function

    'Public Function ReadFromString(ByVal Data As String) As List(Of StandardDetail)
    '    Items = New List(Of StandardDetail)
    '    Try
    '        Dim strDetails() As String = splitLines(Data)
    '        If Not strDetails Is Nothing AndAlso strDetails.Length > 0 Then
    '            For Each r As String In strDetails
    '                Try
    '                    Dim strRow() As String = DecodeCSV(r)
    '                    If strRow.Length >= 8 Then
    '                        Dim D As New StandardDetail
    '                        With D
    '                            .SalesOrderNumber = strRow(0)
    '                            .ItemNumber = strRow(1)
    '                            .QuantityOrdered = strRow(2)
    '                            .Cost = strRow(3)
    '                            .Description = strRow(4)
    '                            .RequiredByDate = strRow(5)
    '                            .Weight = strRow(6)
    '                            .BillOfLadingDescription = strRow(7)
    '                        End With
    '                        Items.Add(D)
    '                    Else
    '                        Errors &= "StandardDetail.ReadFromString Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
    '                    End If
    '                Catch ex As ApplicationException
    '                    Errors &= "StandardDetail.ReadFromString parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
    '                End Try
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Errors &= "StandardDetail.ReadFromString Unexpected Error: " & ex.Message & vbCrLf & vbCrLf

    '    End Try

    '    Return Items
    'End Function


    'Public Function ReadFromFile(ByVal fileName As String, Optional ByVal Details As List(Of Object) = Nothing) As List(Of Object) Implements IDetails.ReadFromFile
    '    Items = New List(Of Object)
    '    Try

    '        Dim strData As String = ""
    '        Dim errMsg As String = ""
    '        Dim DRecords As List(Of String) = readFileToList(fileName, errMsg)
    '        If Not DRecords Is Nothing AndAlso DRecords.Count > 0 Then
    '            For Each r In DRecords
    '                Try
    '                    Dim strRow() As String = DecodeCSV(r)
    '                    If strRow.Length >= 8 Then
    '                        Dim D As New StandardDetail
    '                        With D
    '                            .ItemPONumber = strRow(0)
    '                            .FixOffInvAllow = strRow(1)
    '                            .FixFrtAllow = strRow(2)
    '                            .ItemNumber = strRow(3)
    '                            .QtyOrdered = strRow(4)
    '                            .FreightCost = strRow(5)
    '                            .ItemCost = strRow(6)
    '                            .Weight = strRow(7)
    '                            .Cube = strRow(8)
    '                            .Pack = strRow(9)
    '                            .Size = strRow(10)
    '                            .Description = strRow(11)
    '                            .Hazmat = strRow(12)
    '                            .Brand = strRow(13)
    '                            .CostCenter = strRow(14)
    '                            .LotNumber = strRow(15)
    '                            .LotExpirationDate = strRow(16)
    '                            .GTIN = strRow(17)
    '                            .CustItemNumber = strRow(18)
    '                            .CustomerNumber = strRow(19)
    '                        End With
    '                        Items.Add(D)
    '                    Else
    '                        Errors &= "StandardDetail.ReadFromFile Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
    '                    End If
    '                Catch ex As ApplicationException
    '                    Errors &= "StandardDetail.ReadFromFile parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
    '                End Try
    '            Next
    '        Else
    '            Errors &= errMsg
    '        End If

    '    Catch ex As Exception
    '        Errors &= "StandardDetail.ReadFromFile Unexpected Error: " & ex.Message & vbCrLf & vbCrLf
    '    End Try
    '    Return Items

    'End Function

    Public Function ReadFromFile(ByVal fileName As String, Optional ByVal Details As List(Of Object) = Nothing) As List(Of Object) Implements IDetails.ReadFromFile
        Items = New List(Of Object)
        Try

            Dim strData As String = ""
            Dim errMsg As String = ""
            Dim DRecords() As String = splitLines(readFileToEnd(fileName, errMsg))
            If Not DRecords Is Nothing AndAlso DRecords.Count > 0 Then
                For Each r In DRecords
                    Try
                        If r.Trim.Length = 0 Then Continue For
                        Dim strRow() As String = DecodeCSV(r)
                        If strRow.Length >= 20 Then
                            Dim D As New StandardDetail
                            With D
                                .ItemPONumber = strRow(0)
                                .FixOffInvAllow = strRow(1)
                                .FixFrtAllow = strRow(2)
                                .ItemNumber = strRow(3)
                                .QtyOrdered = strRow(4)
                                .FreightCost = strRow(5)
                                .ItemCost = strRow(6)
                                .Weight = strRow(7)
                                .Cube = strRow(8)
                                .Pack = strRow(9)
                                .Size = strRow(10)
                                .Description = strRow(11)
                                .Hazmat = strRow(12)
                                .Brand = strRow(13)
                                .CostCenter = strRow(14)
                                .LotNumber = strRow(15)
                                .LotExpirationDate = strRow(16)
                                .GTIN = strRow(17)
                                .CustItemNumber = strRow(18)
                                .CustomerNumber = strRow(19)
                            End With
                            Items.Add(D)
                        Else
                            Errors &= "StandardDetail.ReadFromFile Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
                        End If
                    Catch ex As ApplicationException
                        Errors &= "StandardDetail.ReadFromFile parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
                    End Try
                Next
            Else
                Errors &= errMsg
            End If

        Catch ex As Exception
            Errors &= "StandardDetail.ReadFromFile Unexpected Error: " & ex.Message & vbCrLf & vbCrLf
        End Try
        Return Items

    End Function
 
End Class

Public Class StandardDetail
    'Public SalesOrderNumber As String
    'Public ItemNumber As String
    'Public QuantityOrdered As String
    'Public Cost As String
    'Public Description As String
    'Public RequiredByDate As String
    'Public Weight As String
    'Public BillOfLadingDescription As String

    Public ItemPONumber As String
    Public FixOffInvAllow As String
    Public FixFrtAllow As String
    Public ItemNumber As String
    Public QtyOrdered As String
    Public FreightCost As String
    Public ItemCost As String
    Public Weight As String
    Public Cube As String
    Public Pack As String
    Public Size As String
    Public Description As String
    Public Hazmat As String
    Public Brand As String
    Public CostCenter As String
    Public LotNumber As String
    Public LotExpirationDate As String
    Public GTIN As String
    Public CustItemNumber As String
    Public CustomerNumber As String


    'Public ReadOnly Property dtRequiredByDate() As Date
    '    Get
    '        Return CastToDate(RequiredByDate, Date.Now.ToShortDateString)
    '    End Get
    'End Property

    'Public ReadOnly Property dblItemWeight() As Double
    '    Get
    '        Return CastToDouble(Weight.ToString)
    '    End Get
    'End Property

    'Public ReadOnly Property dblCost() As Double
    '    Get
    '        Return CastToDouble(Cost)
    '    End Get
    'End Property


    'Public ReadOnly Property intQty() As Integer
    '    Get
    '        Return CastToInteger(QuantityOrdered)
    '    End Get
    'End Property

    Public ReadOnly Property dblTotalWeight() As Integer
        Get
            Return CastToDouble(QtyOrdered) * CastToDouble(Weight.ToString)
        End Get
    End Property


    'Public Overrides Function ToString() As String
    '    Dim strRet As String = SalesOrderNumber & " | " _
    '                           & ItemNumber & " | " _
    '                           & QuantityOrdered & " | " _
    '                           & Cost & " | " _
    '                           & Description & " | " _
    '                           & RequiredByDate & " | " _
    '                           & Weight & " | " _
    '                           & BillOfLadingDescription & " | "

    '    Return strRet
    'End Function
End Class
