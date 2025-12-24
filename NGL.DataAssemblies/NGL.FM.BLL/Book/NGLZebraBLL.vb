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

Public Class NGLZebraBLL : Inherits BLLBaseClass

#Region "Constructors"

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLZebraBLL"
    End Sub

#End Region

#Region "Properties"

    Private _oShipLabels As DTO.BookShipLabel()
    Public Property oShipLabels() As DTO.BookShipLabel()
        Get
            Return _oShipLabels
        End Get
        Set(ByVal value As DTO.BookShipLabel())
            _oShipLabels = value
        End Set
    End Property

    Private _oBillLabels As DTO.BookRevenue()
    Public Property oBillLabels() As DTO.BookRevenue()
        Get
            Return _oBillLabels
        End Get
        Set(ByVal value As DTO.BookRevenue())
            _oBillLabels = value
        End Set
    End Property

#End Region

#Region "DAL Wrapper Methods"

    Public Function GetCarrierVLookup(ByVal CarrierControl As Integer) As DTO.vLookupList
        Return NGLCarrierData.GetCarrierVLookup(CarrierControl)
    End Function

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Retrieves the selected label data, creates the label formats, and sends them to the printer.
    ''' printLabelType: 0 = print shipping labels only, 1 = print billing labels only, 2 = print both shipping and billing labels, 3 = no labels were selected to print
    ''' </summary>
    ''' <param name="multOrders"></param>
    ''' <param name="multBillOrders"></param> 
    ''' <param name="printLabelType"></param>
    ''' <param name="IPAddress"></param>
    ''' <param name="Port"></param>
    ''' <remarks></remarks>
    Public Function sendLabels(ByVal multOrders As DTO.BookShipLabel(),
                          ByVal multBillOrders As DTO.BookRevenue(),
                          ByVal printLabelType As Integer,
                          ByVal IPAddress As String,
                          ByVal Port As Integer,
                          ByVal LargeFontShip As Boolean,
                          ByVal LargeFontBill As Boolean) As DTO.ZebraResult
        Me.oShipLabels = multOrders
        Me.oBillLabels = multBillOrders
        Dim printType As DAL.Utilities.PrintLabelType = [Enum].Parse(GetType(DAL.Utilities.PrintLabelType), printLabelType)
        Dim oZebra As New DTO.ZebraResult
        Select Case printType
            Case FreightMaster.Data.Utilities.PrintLabelType.PrintPalletLabels
                CreateShippingLabelLayout(oZebra, LargeFontShip)
                If oZebra.Success = False Then
                    Return oZebra
                End If
                SendZPLToPrinter(oZebra, IPAddress, Port)
            Case FreightMaster.Data.Utilities.PrintLabelType.PrintBillLabels
                CreateBillingLabelLayout(oZebra, LargeFontBill)
                If oZebra.Success = False Then
                    Return oZebra
                End If
                SendZPLToPrinter(oZebra, IPAddress, Port)
            Case FreightMaster.Data.Utilities.PrintLabelType.PrintBothPalletAndBills
                CreateShippingLabelLayout(oZebra, LargeFontShip)
                If oZebra.Success = False Then
                    Return oZebra
                End If
                CreateBillingLabelLayout(oZebra, LargeFontBill)
                If oZebra.Success = False Then
                    Return oZebra
                End If
                SendZPLToPrinter(oZebra, IPAddress, Port)
            Case FreightMaster.Data.Utilities.PrintLabelType.PrintLabelNotSelected
                oZebra.AddMessage(FreightMaster.Data.DataTransferObjects.ZebraResult.MessageEnum.M_AtLeastOneLabelRequired)
        End Select

        Return oZebra

    End Function

    ''' <summary>
    ''' Assmebles the master list of individual label format strings
    ''' into one giant ZPL command to send to the printer
    ''' </summary>
    ''' <param name="labels"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function assembleZPL(ByVal labels As List(Of String)) As String
        Dim printerCode As String = ""
        For Each zpl In labels
            printerCode = printerCode & vbCrLf & zpl
        Next
        Return printerCode
    End Function

    ''' <summary>
    ''' Assmebles the master list of individual label format strings
    ''' into one giant ZPL command to send to the printer, but
    ''' uses a space " " as a seperator instead of vbCrLf
    ''' </summary>
    ''' <param name="labels"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function assembleZPLSpace(ByVal labels As List(Of String)) As String
        Dim printerCode As String = ""
        For Each zpl In labels
            printerCode = printerCode & " " & zpl
        Next
        Return printerCode
    End Function

    ''' <summary>
    ''' Sends the formatted ZPL command string (label format)
    ''' to the Zebra printer via TCP connection
    ''' </summary>
    ''' <param name="zebra"></param>
    ''' <param name="IPAddress"></param>
    ''' <param name="Port"></param>
    ''' <remarks></remarks>
    Public Sub SendZPLToPrinter(ByRef zebra As DTO.ZebraResult, ByVal IPAddress As String, ByVal Port As Integer)
        'Concatonate list of ZPL label strings into one ZPL command string
        Dim strZPLCommand = assembleZPL(zebra.ZPLCMDStrings)
        Try
            'this will code to send the ZPL command string to the printer
            Using tcp As New Net.Sockets.TcpClient()
                Dim ar As IAsyncResult = tcp.BeginConnect(IPAddress, Port, Nothing, Nothing)
                Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                Try
                    If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), False) Then
                        tcp.Close()
                        Throw New TimeoutException()
                    End If
                Catch ex As TimeoutException
                    zebra.Success = False
                    If String.IsNullOrEmpty(IPAddress) Then
                        IPAddress = "unknown"
                    End If
                    zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_CannotConnectToZebraPrinter, {IPAddress, Port.ToString()})
                    Return 
                End Try

                'Write ZPL String to Connection
                Dim writer As New System.IO.StreamWriter(tcp.GetStream())
                writer.Write(strZPLCommand)
                writer.Flush()

                'Close Connection
                writer.Close()

                tcp.Close()
                Try
                    tcp.EndConnect(ar)
                Catch ex As Exception

                End Try
            End Using

        Catch ex As Exception
            zebra.Success = False
            zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_UnableToPrint, {ex.ToString()})
        End Try

    End Sub

    Public Sub SendZPLToPrinterString(ByVal zpl As String, ByVal IPAddress As String, ByVal Port As Integer)
        Try
            'this will code to send the ZPL command string to the printer
            Using tcp As New Net.Sockets.TcpClient()
                Dim ar As IAsyncResult = tcp.BeginConnect(IPAddress, Port, Nothing, Nothing)
                Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                Try
                    If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), False) Then
                        tcp.Close()
                        Throw New TimeoutException()
                    End If
                    'Write ZPL String to Connection
                    Dim writer As New System.IO.StreamWriter(tcp.GetStream())
                    writer.Write(zpl)
                    writer.Flush()

                    'Close Connection
                    writer.Close()

                    'tcp.Close()
                    tcp.EndConnect(ar)
                    'tcp.Close()

                Catch ex As Exception
                    '    wh.Close()
                End Try
            End Using

        Catch ex As Exception

        End Try

    End Sub

    Public Sub SendZPLToPrinterLoop(ByRef zebra As DTO.ZebraResult, ByVal IPAddress As String, ByVal Port As Integer)
        For Each zpl In zebra.ZPLCMDStrings
            Try
                'this will code to send the ZPL command string to the printer
                Using tcp As New Net.Sockets.TcpClient()
                    Dim ar As IAsyncResult = tcp.BeginConnect(IPAddress, Port, Nothing, Nothing)
                    Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                    Try
                        If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), False) Then
                            tcp.Close()
                            Throw New TimeoutException()
                        End If
                        'Write ZPL String to Connection
                        Dim writer As New System.IO.StreamWriter(tcp.GetStream())
                        writer.Write(zpl)
                        writer.Flush()

                        'Close Connection
                        writer.Close()

                        'tcp.Close()
                        tcp.EndConnect(ar)
                        'tcp.Close()

                    Catch ex As Exception
                        '    wh.Close()
                    End Try
                End Using

            Catch ex As Exception
                zebra.Success = False
                zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_CannotConnectToZebraPrinter, {IPAddress, Port.ToString()})
            End Try
        Next

    End Sub

    Public Sub SendZPLToPrinterInnerLoop(ByRef zebra As DTO.ZebraResult, ByVal IPAddress As String, ByVal Port As Integer)
        Try
            'this will code to send the ZPL command string to the printer
            Using tcp As New Net.Sockets.TcpClient()
                Dim ar As IAsyncResult = tcp.BeginConnect(IPAddress, Port, Nothing, Nothing)
                Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                Try
                    If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), False) Then
                        tcp.Close()
                        Throw New TimeoutException()
                    End If
                    'Write ZPL String to Connection
                    Dim writer As New System.IO.StreamWriter(tcp.GetStream())
                    For Each zpl In zebra.ZPLCMDStrings
                        writer.Write(zpl)
                        writer.Flush()
                    Next
                    'Close Connection
                    writer.Close()

                    'tcp.Close()
                    tcp.EndConnect(ar)
                    'tcp.Close()

                Catch ex As Exception
                    '    wh.Close()
                End Try
            End Using

        Catch ex As Exception
            zebra.Success = False
            zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_CannotConnectToZebraPrinter, {IPAddress, Port.ToString()})
        End Try
    End Sub

    Public Sub SendZPLToPrinterSpace(ByRef zebra As DTO.ZebraResult, ByVal IPAddress As String, ByVal Port As Integer)
        'Concatonate list of ZPL label strings into one ZPL command string
        Dim strZPLCommand = assembleZPLSpace(zebra.ZPLCMDStrings)
        Try
            'this will code to send the ZPL command string to the printer
            Using tcp As New Net.Sockets.TcpClient()
                Dim ar As IAsyncResult = tcp.BeginConnect(IPAddress, Port, Nothing, Nothing)
                Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                Try
                    If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5), False) Then
                        tcp.Close()
                        Throw New TimeoutException()
                    End If
                Catch ex As TimeoutException
                    zebra.Success = False
                    zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_CannotConnectToZebraPrinter, {IPAddress, Port.ToString()})
                End Try

                'Write ZPL String to Connection
                Dim writer As New System.IO.StreamWriter(tcp.GetStream())
                writer.Write(strZPLCommand)
                writer.Flush()

                'Close Connection
                writer.Close()

                tcp.Close()
                Try
                    tcp.EndConnect(ar)
                Catch ex As Exception

                End Try
            End Using

        Catch ex As Exception
            zebra.Success = False
            zebra.AddMessage(DTO.ZebraResult.MessageEnum.M_UnableToPrint, {ex.ToString()})
        End Try

    End Sub

    ''' <summary>
    ''' Loops through each item in the list of BookShipLabelDetails
    ''' and creates an OrderNumber string by concatonating several fields.
    ''' Adds each OrderNumber string to an array which is then returned.
    ''' OrderNumber = BookCarrOrderNumber-BookOrderSequence
    ''' </summary>
    ''' <param name="details"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getOrderNumberString(ByVal details As List(Of DTO.BookShipLabelDetail)) As String()
        Dim ret As New List(Of String)
        Dim temp As String = ""
        Dim sep As String = ""

        For Each d In details
            temp = d.BookCarrOrderNumber & "-" & d.BookOrderSequence
            ret.Add(temp)
        Next
        Return ret.ToArray()
    End Function

    ''' <summary>
    ''' Loops through the BookShipLabel data stored in the NGLZebraBLL property oShipLabels
    ''' and inserts the data into the shipping label layout. Adds each ZPL string to the ZPLCMDStrings list
    ''' property of the DTO.ZebraResult object, as well as any errors to the Messages property.
    ''' </summary>
    ''' <param name="zebra"></param>
    ''' <remarks></remarks>
    Public Sub CreateShippingLabelLayout(ByRef zebra As DTO.ZebraResult, ByVal LargeFont As Boolean)
        'Success defaults to True unless and exception is caught
        zebra.Success = True
        For Each l In Me.oShipLabels
            Try
                Dim DateLoad = l.DateLoad()
                Dim Weight = l.TotalWgt()
                Weight = Math.Round(Weight, 0)
                Dim TotalPL = l.TotalPL()
                Dim orders1 As String = ""
                Dim orders2 As String = ""
                Dim orderNums = getOrderNumberString(l.BookShipLabelDetails)
                Select Case orderNums.Length
                    Case 1
                        orders1 = orderNums(0)
                    Case 2
                        orders1 = orderNums(0) & ", " & orderNums(1)
                    Case 3
                        orders1 = orderNums(0) & ", " & orderNums(1) & ","
                        orders2 = orderNums(2)
                    Case Else
                        '4 or more
                        'there are too many order numbers?
                        orders1 = orderNums(0) & ", " & orderNums(1) & ","
                        orders2 = orderNums(2) & ", " & orderNums(3)
                End Select

                If LargeFont = False Then
                    'If the box is unchecked, use the default ngl label format
                    '*Note: This label was designed to work with the data sizes in the ngl system.
                    Dim zplCode As String = "^XA^LH0,0^XZ" & vbCrLf _
                                            & "^XA" & vbCrLf _
                                            & "^PW812" & vbCrLf _
                                            & "^LL1218" & vbCrLf _
                                            & "^LS0" & vbCrLf _
                                            & "^FT43,71^A0N,23,24^FH\^FD" & l.BookOrigAddress1 & "^FS" & vbCrLf _
                                            & "^FT43,108^A0N,23,24^FH\^FD" & l.BookOrigCity & " " & l.BookOrigState & " " & l.BookOrigZip & "^FS" & vbCrLf _
                                            & "^FT43,146^A0N,23,24^FH\^FD" & l.BookOrigCountry & "^FS" & vbCrLf _
                                            & "^FT135,285^A0N,28,28^FH\^FD" & l.BookDestName & "^FS" & vbCrLf _
                                            & "^FT135,333^A0N,28,28^FH\^FD" & l.BookDestAddress1 & "^FS" & vbCrLf _
                                            & "^FT135,380^A0N,28,28^FH\^FD" & l.BookDestAddress2 & "^FS" & vbCrLf _
                                            & "^FT135,438^A0N,39,38^FH\^FD" & l.BookDestCity & "^FS" & vbCrLf _
                                            & "^FT135,500^A0N,39,38^FH\^FD" & l.BookDestState & "^FS" & vbCrLf _
                                            & "^FT135,550^A0N,28,28^FH\^FD" & l.BookDestCity & " " & l.BookDestState & " " & l.BookDestZip & "^FS" & vbCrLf _
                                            & "^FT135,597^A0N,28,28^FH\^FD" & l.BookDestCountry & "^FS" & vbCrLf _
                                            & "^FT43,1038^A0N,34,33^FH\^FD" & l.CarrierName & "^FS" & vbCrLf _
                                            & "^FT43,1086^A0N,28,28^FH\^FD" & DateLoad & "^FS" & vbCrLf _
                                            & "^FT43,1131^A0N,28,28^FH\^FDWeight: " & Weight & " LBS^FS" & vbCrLf _
                                            & "^FT43,653^A0N,28,28^FH\^FDCNS# " & l.BookConsPrefix & "^FS" & vbCrLf _
                                            & "^FT43,697^A0N,28,28^FH\^FDOrder# " & orders1 & "^FS" & vbCrLf _
                                            & "^FT634,142^A0N,39,38^FH\^FD" & TotalPL & "^FS" & vbCrLf _
                                            & "^BY2,3,160^FT43,973^B3N,N,,Y,N" & vbCrLf _
                                            & "^FD" & l.BookSHID & "^FS" & vbCrLf _
                                            & "^FT133,741^A0N,28,28^FH\^FD" & orders2 & "^FS" & vbCrLf _
                                            & "^FT553,82^A0N,34,33^FH\^FDTOTAL PALLETS:^FS" & vbCrLf _
                                            & "^FT43,241^A0N,34,33^FH\^FDSHIP^FS" & vbCrLf _
                                            & "^FT43,283^A0N,34,33^FH\^FD TO:^FS" & vbCrLf _
                                            & "^FT43,802^A0N,28,28^FH\^FDShipment ID^FS" & vbCrLf _
                                            & "^PQ" & l.PrintQty & ",1,0,Y^XZ"
                    zebra.AddZPL(zplCode)
                Else
                    'If the box is checked, use the ShieldsFont label instead
                    '*Note: Our system allows for larger data sizes than Shields old program. As a result, with the larger font
                    'sizes it could create the possibility for text runoff or overwriting.
                    Dim zplCodeShieldsFont As String = "^XA^LH0,0^XZ" & vbCrLf _
                                           & "^XA" & vbCrLf _
                                           & "^PW812" & vbCrLf _
                                           & "^LL1218" & vbCrLf _
                                           & "^LS0" & vbCrLf _
                                           & "^FT40,71^A0N,30,20^FH\^FD" & l.BookOrigAddress1 & "^FS" & vbCrLf _
                                           & "^FT40,108^A0N,30,20^FH\^FD" & l.BookOrigCity & " " & l.BookOrigState & " " & l.BookOrigZip & "^FS" & vbCrLf _
                                           & "^FT40,146^A0N,30,20^FH\^FD" & l.BookOrigCountry & "^FS" & vbCrLf _
                                           & "^FT553,82^A0N,30,30^FH\^FDTOTAL PALLETS:^FS" & vbCrLf _
                                           & "^FT634,150^A0N,75,90^FH\^FD" & TotalPL & "^FS" & vbCrLf _
                                           & "^FT24,225^A0N,50,60^FH\^FDSHIP^FS" & vbCrLf _
                                           & "^FT28,274^A0N,50,60^FH\^FD TO:^FS" & vbCrLf _
                                           & "^FT135,270^A0N,28,28^FH\^FD" & l.BookDestName & "^FS" & vbCrLf _
                                           & "^FT135,310^A0N,28,28^FH\^FD" & l.BookDestAddress1 & "^FS" & vbCrLf _
                                           & "^FT135,350-^A0N,25,35^FH\^FD" & l.BookDestAddress2 & "^FS" & vbCrLf _
                                           & "^FT135,430^A0N,75,60^FH\^FD" & l.BookDestCity & "^FS" & vbCrLf _
                                           & "^FT135,510^A0N,75,90^FH\^FD" & l.BookDestState & "^FS" & vbCrLf _
                                           & "^FT135,550^A0N,30,30^FH\^FD" & l.BookDestCity & " " & l.BookDestState & " " & l.BookDestZip & "^FS" & vbCrLf _
                                           & "^FT135,590^A0N,30,30^FH\^FD" & l.BookDestCountry & "^FS" & vbCrLf _
                                           & "^FT40,653^A0N,28,28^FH\^FDCNS# " & l.BookConsPrefix & "^FS" & vbCrLf _
                                           & "^FT40,690^A0N,28,28^FH\^FDOrder# " & orders1 & "^FS" & vbCrLf _
                                           & "^FT122,727^A0N,28,28^FH\^FD" & orders2 & "^FS" & vbCrLf _
                                           & "^FT40,790^A0N,28,28^FH\^FDShipment ID^FS" & vbCrLf _
                                           & "^FT40,960^BY2,3,160^B3N,N,,N,N" & vbCrLf _
                                           & "^FD" & l.BookSHID & "^FS" & vbCrLf _
                                           & "^FT45,1030^A0N,75,60^FD" & l.BookSHID & "^FS" & vbCrLf _
                                           & "^FT43,1030^A0N,75,60^FD" & l.BookSHID & "^FS" & vbCrLf _
                                           & "^FT45,1028^A0N,75,60^FD" & l.BookSHID & "^FS" & vbCrLf _
                                           & "^FT40,1083^A0N,55,60^FH\^FD" & l.CarrierName & "^FS" & vbCrLf _
                                           & "^FT38,1083^A0N,55,60^FH\^FD" & l.CarrierName & "^FS" & vbCrLf _
                                           & "^FT40,1081^A0N,55,60^FH\^FD" & l.CarrierName & "^FS" & vbCrLf _
                                           & "^FT40,1128^A0N,28,28^FH\^FD" & DateLoad & "^FS" & vbCrLf _
                                           & "^FT40,1168^A0N,28,28^FH\^FDWeight: " & Weight & " LBS^FS" & vbCrLf _
                                           & "^PQ" & l.PrintQty & ",1,0,Y^XZ"
                    zebra.AddZPL(zplCodeShieldsFont)
                End If
            Catch ex As Exception
                zebra.AddMessage(FreightMaster.Data.DataTransferObjects.ZebraResult.MessageEnum.M_UnableToCreateLabelString)
                zebra.Success = False
                Return
            End Try
        Next

    End Sub

    ''' <summary>
    ''' Loops through the BookRevenue data stored in the NGLZebraBLL property oBillLabels
    ''' and inserts the data into the billing label layout. Adds each ZPL string to the ZPLCMDStrings list
    ''' property of the DTO.ZebraResult object, as well as any errors to the Messages property.
    ''' </summary>
    ''' <param name="zebra"></param>
    ''' <remarks></remarks>
    Public Sub CreateBillingLabelLayout(ByRef zebra As DTO.ZebraResult, ByVal LargeFont As Boolean)
        'Success defaults to True unless and exception is caught
        zebra.Success = True
        For Each b In Me.oBillLabels
            Try
                Dim Weight = b.BookTotalWgt
                Weight = Math.Round(Weight, 0)
                Dim Carrier = GetCarrierVLookup(b.BookCarrierControl)

                If LargeFont = False Then
                    Dim zplCode As String = "^XA^LH0,0^XZ" & vbCrLf _
                                            & "^XA" & vbCrLf _
                                            & "^PW812" & vbCrLf _
                                            & "^LL0223" & vbCrLf _
                                            & "^LS0" & vbCrLf _
                                            & "^FT25,46^A0N,20,19^FH\^FDCNS#: " & b.BookConsPrefix & "^FS" & vbCrLf _
                                            & "^FT25,121^A0N,20,19^FH\^FDCarrier: " & Carrier.Name & "^FS" & vbCrLf _
                                            & "^FT25,158^A0N,20,19^FH\^FDShipment ID: " & b.BookSHID & "^FS" & vbCrLf _
                                            & "^FT25,196^A0N,20,19^FH\^FDConsignee: " & b.BookDestName & "^FS" & vbCrLf _
                                            & "^FT488,83^A0N,20,19^FH\^FDTotal Pallets: " & b.BookTotalPL & "^FS" & vbCrLf _
                                            & "^FT488,121^A0N,20,19^FH\^FDTotal Cost: $" & Math.Round(b.BookTotalBFC, 2) & "^FS" & vbCrLf _
                                            & "^FT488,46^A0N,20,19^FH\^FDShip Date: " & b.BookDateLoad & "^FS" & vbCrLf _
                                            & "^FT488,158^A0N,20,19^FH\^FDTotal Wt: " & Weight & " LBS^FS" & vbCrLf _
                                            & "^FT25,83^A0N,20,19^FH\^FDOrder#: " & b.BookCarrOrderNumber & "-" & b.BookOrderSequence & "^FS" & vbCrLf _
                                            & "^PQ1,0,1,Y^XZ"
                    zebra.AddZPL(zplCode)
                Else
                    Dim zplCodeShieldsFont As String = "^XA^LH0,0^XZ" & vbCrLf _
                                           & "^XA" & vbCrLf _
                                           & "^PW812" & vbCrLf _
                                           & "^LL0223" & vbCrLf _
                                           & "^LS0" & vbCrLf _
                                           & "^FT25,46^A0N,28,27^FH\^FDCNS#: " & b.BookConsPrefix & "^FS" & vbCrLf _
                                           & "^FT25,121^A0N,31,30^FH\^FDCarrier: " & Carrier.Name & "^FS" & vbCrLf _
                                           & "^FT25,120^A0N,31,30^FH\^FDCarrier: " & Carrier.Name & "^FS" & vbCrLf _
                                           & "^FT24,121^A0N,31,30^FH\^FDCarrier: " & Carrier.Name & "^FS" & vbCrLf _
                                           & "^FT25,158^A0N,31,30^FH\^FDShipment ID: " & b.BookSHID & "^FS" & vbCrLf _
                                           & "^FT25,157^A0N,31,30^FH\^FDShipment ID: " & b.BookSHID & "^FS" & vbCrLf _
                                           & "^FT24,158^A0N,31,30^FH\^FDShipment ID: " & b.BookSHID & "^FS" & vbCrLf _
                                           & "^FT25,196^A0N,28,27^FH\^FDConsignee: " & b.BookDestName & "^FS" & vbCrLf _
                                           & "^FT486,83^A0N,28,27^FH\^FDTotal Pallets: " & b.BookTotalPL & "^FS" & vbCrLf _
                                           & "^FT486,82^A0N,28,27^FH\^FDTotal Pallets: " & b.BookTotalPL & "^FS" & vbCrLf _
                                           & "^FT485,83^A0N,28,27^FH\^FDTotal Pallets: " & b.BookTotalPL & "^FS" & vbCrLf _
                                           & "^FT486,121^A0N,28,27^FH\^FDTotal Cost: $" & Math.Round(b.BookTotalBFC, 2) & "^FS" & vbCrLf _
                                           & "^FT486,46^A0N,26,25^FH\^FDShip Date: " & b.BookDateLoad & "^FS" & vbCrLf _
                                           & "^FT486,158^A0N,28,27^FH\^FDTotal Wt: " & Weight & " LBS^FS" & vbCrLf _
                                           & "^FT25,83^A0N,31,30^FH\^FDOrder#: " & b.BookCarrOrderNumber & "-" & b.BookOrderSequence & "^FS" & vbCrLf _
                                           & "^FT25,82^A0N,31,30^FH\^FDOrder#: " & b.BookCarrOrderNumber & "-" & b.BookOrderSequence & "^FS" & vbCrLf _
                                           & "^FT24,83^A0N,31,30^FH\^FDOrder#: " & b.BookCarrOrderNumber & "-" & b.BookOrderSequence & "^FS" & vbCrLf _
                                           & "^PQ1,0,1,Y^XZ"
                    zebra.AddZPL(zplCodeShieldsFont)
                End If

            Catch ex As Exception
                zebra.AddMessage(FreightMaster.Data.DataTransferObjects.ZebraResult.MessageEnum.M_UnableToCreateLabelString)
                zebra.Success = False
                Return
            End Try
        Next
    End Sub



#End Region

    '** SHIELDS LARGE FONT LAYOUT OPTIONS BEFORE 9/4/15 **

    'Dim zplCodeShieldsFont As String = "^XA^LH0,0^XZ" & vbCrLf _
    '                                        & "^XA" & vbCrLf _
    '                                        & "^PW812" & vbCrLf _
    '                                        & "^LL1218" & vbCrLf _
    '                                        & "^LS0" & vbCrLf _
    '                                        & "^FT40,71^A0N,30,16^FH\^FD" & l.BookOrigAddress1 & "^FS" & vbCrLf _
    '                                        & "^FT40,108^A0N,30,16^FH\^FD" & l.BookOrigCity & " " & l.BookOrigState & " " & l.BookOrigZip & "^FS" & vbCrLf _
    '                                        & "^FT40,146^A0N,30,16^FH\^FD" & l.BookOrigCountry & "^FS" & vbCrLf _
    '                                        & "^FT553,82^A0N,25,35^FH\^FDTOTAL PALLETS:^FS" & vbCrLf _
    '                                        & "^FT634,150^A0N,75,90^FH\^FD" & TotalPL & "^FS" & vbCrLf _
    '                                        & "^FT24,225^A0N,50,60^FH\^FDSHIP^FS" & vbCrLf _
    '                                        & "^FT28,274^A0N,50,60^FH\^FD TO:^FS" & vbCrLf _
    '                                        & "^FT135,270^A0N,25,35^FH\^FD" & l.BookDestName & "^FS" & vbCrLf _
    '                                        & "^FT135,310^A0N,25,35^FH\^FD" & l.BookDestAddress1 & "^FS" & vbCrLf _
    '                                        & "^FT135,350-^A0N,25,35^FH\^FD" & l.BookDestAddress2 & "^FS" & vbCrLf _
    '                                        & "^FT135,430^A0N,75,60^FH\^FD" & l.BookDestCity & "^FS" & vbCrLf _
    '                                        & "^FT135,510^A0N,75,90^FH\^FD" & l.BookDestState & "^FS" & vbCrLf _
    '                                        & "^FT135,550^A0N,25,35^FH\^FD" & l.BookDestCity & " " & l.BookDestState & " " & l.BookDestZip & "^FS" & vbCrLf _
    '                                        & "^FT135,590^A0N,25,35^FH\^FD" & l.BookDestCountry & "^FS" & vbCrLf _
    '                                        & "^FT40,653^A0N,28,28^FH\^FDCNS# " & l.BookConsPrefix & "^FS" & vbCrLf _
    '                                        & "^FT40,690^A0N,28,28^FH\^FDOrder# " & orders1 & "^FS" & vbCrLf _
    '                                        & "^FT122,727^A0N,28,28^FH\^FD" & orders2 & "^FS" & vbCrLf _
    '                                        & "^FT40,790^A0N,28,28^FH\^FDShipment ID^FS" & vbCrLf _
    '                                        & "^FT40,960^BY2,3,160^B3N,N,,N,N" & vbCrLf _
    '                                        & "^FD" & l.BookSHID & "^FS" & vbCrLf _
    '                                        & "^FT45,1000^A0N,35,50^FD" & l.BookSHID & "^FS" & vbCrLf _
    '                                        & "^FT40,1060^A0N,55,60^FH\^FD" & l.CarrierName & "^FS" & vbCrLf _
    '                                        & "^FT40,1105^A0N,25,35^FH\^FD" & DateLoad & "^FS" & vbCrLf _
    '                                        & "^FT40,1145^A0N,25,35^FH\^FDWeight: " & Weight & " LBS^FS" & vbCrLf _
    '                                        & "^PQ" & l.PrintQty & ",1,0,Y^XZ"


    'Dim zplCodeLargeFont As String = "^XA^LH0,0^XZ" & vbCrLf _
    '                                                 & "^XA" & vbCrLf _
    '                                                 & "^PW812" & vbCrLf _
    '                                                 & "^LL0223" & vbCrLf _
    '                                                 & "^LS0" & vbCrLf _
    '                                                 & "^FT25,46^A0N,25,24^FH\^FDCNS#: " & b.BookConsPrefix & "^FS" & vbCrLf _
    '                                                 & "^FT25,121^A0N,25,24^FH\^FDCarrier: " & Carrier.Name & "^FS" & vbCrLf _
    '                                                 & "^FT25,158^A0N,25,24^FH\^FDShipment ID: " & b.BookSHID & "^FS" & vbCrLf _
    '                                                 & "^FT25,196^A0N,25,24^FH\^FDConsignee: " & b.BookDestName & "^FS" & vbCrLf _
    '                                                 & "^FT488,83^A0N,25,24^FH\^FDTotal Pallets: " & b.BookTotalPL & "^FS" & vbCrLf _
    '                                                 & "^FT488,121^A0N,25,24^FH\^FDTotal Cost: $" & Math.Round(b.BookTotalBFC, 2) & "^FS" & vbCrLf _
    '                                                 & "^FT488,46^A0N,25,24^FH\^FDShip Date: " & b.BookDateLoad & "^FS" & vbCrLf _
    '                                                 & "^FT488,158^A0N,25,24^FH\^FDTotal Wt: " & Weight & " LBS^FS" & vbCrLf _
    '                                                 & "^FT25,83^A0N,25,24^FH\^FDOrder#: " & b.BookCarrOrderNumber & "-" & b.BookOrderSequence & "^FS" & vbCrLf _
    '                                                 & "^PQ1,0,1,Y^XZ"

End Class


