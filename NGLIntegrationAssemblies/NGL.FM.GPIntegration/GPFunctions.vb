#Const StandAlone = True
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
'Imports NGL.Core
Imports GPObject = NGL.FM.GPIntegration

'NGL Imports
Imports NGL.FreightMaster
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGL.Core
Imports NGL.FM
Imports TMS = NGL.FreightMaster.Integration
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = NGL.FreightMaster.Data.LTS

'Public Class GPFunctions : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
''' <summary>
''' A Library of functions for interacting and calculating values based on GP Data
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.5.102 10/14/2016
'''   added logic to use more configuration settings for GP Functions
''' </remarks>
Public Class GPFunctions : Inherits FreightMaster.Core.NGLCommandLineBaseClass

    ''' <summary>
    ''' Constructor 
    ''' </summary>
    ''' <param name="c"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   clsDefaultIntegrationConfiguration is now required
    ''' </remarks>
    Public Sub New(ByRef c As clsDefaultIntegrationConfiguration)
        MyBase.New()
        _DefaultIntegrationConfiguration = c
    End Sub


    Private _DefaultIntegrationConfiguration As clsDefaultIntegrationConfiguration
    ''' <summary>
    ''' The Default Integration Configuration Object is now Required
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property DefaultIntegrationConfiguration() As clsDefaultIntegrationConfiguration
        Get
            Return _DefaultIntegrationConfiguration
        End Get
        Set(ByVal value As clsDefaultIntegrationConfiguration)
            _DefaultIntegrationConfiguration = value
        End Set
    End Property


    Public Function DetermineHazmat(ByVal TransactionNumber As String, ByVal TransactionType As String) As String

        Dim ReturnValue As String = "N"

        Try

            Select Case TransactionType

                Case "SOP"

                    ReturnValue = "N"

                Case "POP"

                    ReturnValue = "N"

                Case "INV"

                    ReturnValue = "N"

                Case Else

                    ReturnValue = "N"

            End Select

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Determine Hazmat Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw

        End Try

        Return ReturnValue

    End Function

    Public Function GetPorDAddress(ByVal EconnectStr As String, ByVal LocationID As String) As GPDataIntegrationSTructure.FromAddress

        Dim ReturnValue As New GPDataIntegrationSTructure.FromAddress

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            'Integrate Sales Order

            'Assume Order for now and assign SOPType = 2

            SQLCom.CommandText = "select LOCNCODE, LOCNDSCR, ADDRESS1, ADDRESS2, ADDRESS3, city, state, ZIPCODE, COUNTRY, PHONE1, FAXNUMBR from iv40700 where LOCNCODE = '" & LocationID & "'"

            TblRec = SQLCom.ExecuteReader

            TblRec.Read()

            If (TblRec.HasRows) Then

                ReturnValue.AddressID = LocationID
                ReturnValue.AddressName = TblRec("LOCNDSCR")
                ReturnValue.Address1 = TblRec("ADDRESS1")
                ReturnValue.Address2 = TblRec("ADDRESS2")
                ReturnValue.Address3 = TblRec("ADDRESS3")
                ReturnValue.City = TblRec("CITY")
                ReturnValue.State = TblRec("STATE")
                ReturnValue.ZipCode = TblRec("ZIPCODE")
                ReturnValue.Country = TblRec("COUNTRY")
                ReturnValue.Phone = TblRec("PHONE1")
                ReturnValue.Phone = TblRec("FAXNUMBR")

            Else

                ReturnValue.AddressID = "No Address"

            End If

            TblRec.Close()

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get PO or SOP Address Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
            ReturnValue.AddressID = "No Address"
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    'New function to update SOP order with cost / pound
    '2017-08-02
    Public Function CalculateSOPCostPerUnit(ByVal EconnectStr As String, ByVal OrderNumbers As GPDataIntegrationSTructure.SOPOrders) As List(Of GPDataIntegrationSTructure.OrderUnitPercentage)

        Dim ReturnValue As List(Of GPDataIntegrationSTructure.OrderUnitPercentage)
        Dim OrderInfo As New GPDataIntegrationSTructure.OrderUnitPercentage
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0
        Dim OrderText As String = ""

        Try

            'For Each Ord In OrderNumbers

            'If (OrderText = "") Then

            'OrderText = "'" & Ord.ToString & "'"

            'Else

            'OrderText = ", '" & Ord.ToString & "'"

            'End If

            'Next

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            'Integrate Sales Order

            'Assume Order for now and assign SOPType = 2

            SQLCom.CommandText = "select SOPNUMBE, sum(QUANTITY) as tot_qty, ( sum(QUANTITY) / (select sum(QUANTITY) as tot_qty from SOP30300 where SOPNUMBE in (" & OrderText & "))) as ord_percent  from SOP30300 where SOPNUMBE in (" & OrderText & ")  group by SOPNUMBE"

            TblRec = SQLCom.ExecuteReader

            TblRec.Read()

            If (TblRec.HasRows) Then

                OrderInfo.OrderNumber = Trim(TblRec(0).ToString)
                OrderInfo.OrderTotalQuantity = TblRec(1).ToString
                OrderInfo.OrderUnitPercentage = TblRec(2).ToString

#Disable Warning BC42104 ' Variable 'ReturnValue' is used before it has been assigned a value. A null reference exception could result at runtime.
                ReturnValue.Add(OrderInfo)
#Enable Warning BC42104 ' Variable 'ReturnValue' is used before it has been assigned a value. A null reference exception could result at runtime.

                OrderInfo = Nothing

            End If

            TblRec.Close()

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Calculate SOP Cost Per Order Error", Source & " Could not process any calculation requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
            OrderInfo.OrderNumber = "No SOP Orders"
            OrderInfo.OrderTotalQuantity = 0
            OrderInfo.OrderUnitPercentage = 0

            ReturnValue.Add(OrderInfo)

        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                    TblRec.Close()
                End If

            Catch ex1 As Exception

            End Try

            Try
                If Not SQLCom Is Nothing Then
                    SQLCom.Dispose()
                End If
            Catch ex2 As Exception

            End Try

            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then
                    SQLConn.Close()
                End If

                SQLConn.Dispose()

            Catch ex3 As Exception

            End Try

        End Try

        Return ReturnValue

    End Function


    Public Function GetTransporationMode(ByVal TransactionNumber As String, ByVal TransactionType As String) As GPEnumValues.TransMode

        Dim ReturnValue As GPEnumValues.TransMode = GPEnumValues.TransMode.Road

        Try

            Select Case TransactionType

                Case "SOP"

                    ReturnValue = GPEnumValues.TransMode.Road

                Case "POP"

                    ReturnValue = GPEnumValues.TransMode.Road

                Case "INV"

                    ReturnValue = GPEnumValues.TransMode.Road

                Case "INVTRN"

                    ReturnValue = GPEnumValues.TransMode.Road

                Case Else

                    ReturnValue = GPEnumValues.TransMode.Road

            End Select

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get Get Transportation Mode Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw

        End Try

        Return ReturnValue

    End Function

    'Added by SEM 2017-07-30
#Disable Warning BC42307 ' XML comment parameter 'SOPNumber' does not match a parameter on the corresponding 'function' statement.
    ''' <summary>
    ''' Get the notes for a PO
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="SOPNumber"></param>
    ''' <param name="ProcessToRun"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    Public Function GetNotes(ByVal EconnectSTr As String, ByVal OrderNumber As String, ByVal ProcessToRun As Integer, ByVal AdminEmail As String) As String
#Enable Warning BC42307 ' XML comment parameter 'SOPNumber' does not match a parameter on the corresponding 'function' statement.



        Dim SQLConn = New SqlConnection(EconnectSTr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim ReturnValue As String = ""
        Dim strVal As String = ""
        Dim strProcessToRun As String = "NA"

        Try

            'Integrate Sales Order
            Select Case ProcessToRun
                Case 1
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "SO"
                    strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsSOPNotes, OrderNumber))
                Case 2
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "PO"
                    strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsPONOtes, OrderNumber))
                Case 5
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "TO"
                    'strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalTOPallets, OrderNumber))

                Case Else
                    Log(Source & ".Get Notes Error: There was an invalid process to run for Notes")
                    strVal = ""

            End Select

            If Not String.IsNullOrWhiteSpace(strVal) And strVal <> "" Then

                SQLConn.Open()

                SQLCom.Connection = SQLConn

                SQLCom.CommandText = strVal
                'SQLCom.CommandText = "select SOPNUMBE From SOP10100 where SOPTYPE = 2 and sopnumbe = 'ORDST2225'"

                TblRec = SQLCom.ExecuteReader

                TblRec.Read()


                If (TblRec.HasRows) Then

                    ReturnValue = TblRec(0).ToString

                Else

                    ReturnValue = ""

                End If

                TblRec.Close()

                SQLCom.Dispose()
                SQLCom.Dispose()

            Else

                ReturnValue = ""

            End If

        Catch ex As ApplicationException
            ReturnValue = ""
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".Get Notes Error: Process " & strProcessToRun & " Notes Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".Get Notes Error: Unexpected " & strProcessToRun & " Notes Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Reads the comments from the database using the order/po number provided
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="ProcessToRun"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    Public Function GetComments(ByVal EconnectSTr As String, ByVal OrderNumber As String, ByVal ProcessToRun As Integer, ByVal AdminEmail As String) As String

        Dim ReturnValue As String = ""
        Dim strProcessToRun As String = "NA"

        Try

            'Integrate Sales Order
            Select Case ProcessToRun
                Case 1
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "SO"
                    ReturnValue = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsSOPComment, Trim(OrderNumber)))
                Case 2
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "PO"
                    ReturnValue = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsPOComment, Trim(OrderNumber)))
                Case 5
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "TO"
                    'strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalTOPallets, OrderNumber))

                Case Else
                    Log(Source & ".Get Notes Error: There was an invalid process to run for Notes")
                    ReturnValue = ""

            End Select



        Catch ex As ApplicationException
            ReturnValue = ""
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".Get Notes Error: Process " & strProcessToRun & " Notes Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".Get Notes Error: Unexpected " & strProcessToRun & " Notes Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function


    Public Function GetGPPMBatch() As String

        Dim BatchPrefix As String = String.Format(DefaultIntegrationConfiguration.GPPMBatchPrefix)
        Dim AddToBatchDate As Int16 = String.Format(DefaultIntegrationConfiguration.GPBatchDateToAdd)
        Dim BatchDate As Date = DateAdd(DateInterval.Day, AddToBatchDate, Now)

        Dim ReturnValue As String = ""
        Try

            ReturnValue = BatchPrefix & Year(BatchDate) & Right("0" & Month(BatchDate), 2) & Right("0" & Microsoft.VisualBasic.Day(BatchDate), 2) & Right("0" & Microsoft.VisualBasic.Minute(Now), 2)

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get GP Batch Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw

        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Calculate the Total Pallets for an order using the GPFunctionsTotalPOPallets or the GPFunctionsTotalSOPallets 
    ''' configuration query based on the ProcessToRun value: 1 = SO; 2 = PO
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="SOPNumber"></param>
    ''' <param name="ProcessToRun"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   added additional exception handlers
    ''' </remarks>
    Public Function CalculatePallets(ByVal EconnectSTr As String, ByVal SOPNumber As String, ByVal ProcessToRun As Integer, ByVal AdminEmail As String, ByVal SOPType As Integer) As Double

        Dim ReturnValue As Double = 0
        Dim strVal As String = ""
        Dim strProcessToRun As String = "NA"
        Try
            'Integrate Sales Order
            Select Case ProcessToRun
                Case 1

                    Select Case SOPType
                        Case 2

                            strProcessToRun = "SO"
                            strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOPallets, SOPNumber))


                        Case 3

                            strProcessToRun = "SO"
                            strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOConPallets, SOPNumber))

                        Case Else

                            Log(Source & ".CalculatePallets Error: There was an invalid SOP Type for Calculate Pallets")
                            strVal = "-1"

                    End Select
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                Case 2
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "PO"
                    strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalPOPallets, SOPNumber))
                Case 5
                    'Modified by RHR for v-7.0.5.102 10/14/2016
                    strProcessToRun = "TO"
                    strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalTOPallets, SOPNumber))

                Case Else
                    Log(Source & ".CalculatePallets Error: There was an invalid process to run for Calculate Pallets")
                    strVal = "-1"
            End Select
            If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)
        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".CalculatePallets Error: Process " & strProcessToRun & " Total Pallets Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".CalculatePallets Error: Unexpected " & strProcessToRun & " Total Pallets Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Calculate the Sales Order Quantity using the GPFunctionsTotalSOQuantity configuration query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="SOPNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   added additional exception handlers
    ''' </remarks>
    Public Function GetSOPQuantity(ByVal EconnectSTr As String, ByVal SOPNumber As String, ByVal SOPType As Integer) As Double

        Dim ReturnValue As Double = 0
        Try
            'Integrate Sales Order
            'Assume Order for now and assign SOPType = 2
            'Modified by RHR for v-7.0.5.102 10/14/2016

            Select Case SOPType
                Case 2
                    Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOQuantity, SOPNumber))
                    If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

                Case 3
                    Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOConQuantity, SOPNumber))
                    If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

                Case Else

            End Select

        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetSOPQuantity Error: Process SO Total Quantity Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetSOPQuantity Error: Unexpected SO Total Quantity Query Exception: " & ex.Message)
            Throw 'should not continue        
        End Try

        Return ReturnValue

    End Function

    Public Function GetInventoryTransferTotalQuantity(ByVal EconnectSTr As String, ByVal InvTransferID As String) As Double

        Dim ReturnValue As Double = 0
        Try
            'Integrate Sales Order
            'Assume Order for now and assign SOPType = 2
            'Modified by RHR for v-7.0.5.102 10/14/2016
            Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalTOWeight, InvTransferID))
            If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetInventoryTransferTotalQuantity Error: Process TO Total Quantity Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetInventoryTransferTotalQuantity Error: Unexpected TO Total Quantity Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Calculate the Purchse Order Quantity using the GPFunctionsTotalPOQuantity configuration query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="OrderNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   added additional exception handlers
    ''' </remarks>
    Public Function GetPOQuantity(ByVal EconnectSTr As String, ByVal OrderNumber As String) As Double

        Dim ReturnValue As Double = 0

        Try
            'Modified by RHR for v-7.0.5.102 10/14/2016
            Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalPOQuantity, OrderNumber))
            If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetPOQuantity Error: Process PO Total Quantity Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetPOQuantity Error: Unexpected PO Total Quantity Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Calculate the Total Sales Order Weight using the GPFunctionsTotalSOWeight configuration query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="SOPNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   added additional exception handlers
    ''' </remarks>
    Public Function GetSOPTotalWeight(ByVal EconnectSTr As String, ByVal SOPNumber As String, ByVal SOPType As Integer) As Double

        Dim ReturnValue As Double = 0
        Try
            'Integrate Sales Order
            'Assume Order for now and assign SOPType = 2
            'Modified by RHR for v-7.0.5.102 10/14/2016
            Select Case SOPType
                Case 2
                    Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOWeight, SOPNumber))
                    If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

                Case 3
                    Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalSOConWeight, SOPNumber))
                    If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

                Case Else


            End Select


        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetSOPTotalWeight Error: Process SO Total Weight Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetSOPTotalWeight Error: Unexpected SO Total Weight Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function

#Disable Warning BC42307 ' XML comment parameter 'SOPNumber' does not match a parameter on the corresponding 'function' statement.
    ''' <summary>
    ''' Calculate the Total inventory transfer Weight using the GPFunctionsTotalTOWeight configuration query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="SOPNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by SEM for v-7.0.6.103 Update 
    '''   added logic for inventory transfer order
    ''' </remarks>
    Public Function GetInventoryTransferTotalWeight(ByVal EconnectSTr As String, ByVal InvTransferID As String) As Double
#Enable Warning BC42307 ' XML comment parameter 'SOPNumber' does not match a parameter on the corresponding 'function' statement.

        Dim ReturnValue As Double = 0
        Try
            'Integrate Sales Order
            'Assume Order for now and assign SOPType = 2
            'Modified by RHR for v-7.0.5.102 10/14/2016
            Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalTOWeight, InvTransferID))
            If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetInventoryTransferTotalWeight Error: Process TO Total Weight Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetInventoryTransferTotalWeight Error: Unexpected TO Total Weight Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Calculate the Total Purchse Order Weight using the GPFunctionsTotalPOWeight configuration query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="OrderNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   added additional exception handlers
    ''' </remarks>
    Public Function GetPOTotalWeight(ByVal EconnectSTr As String, ByVal OrderNumber As String) As Double

        Dim ReturnValue As Double = 0
        Try
            'Integrate Sales Order
            'Assume Order for now and assign SOPType = 2
            'Modified by RHR for v-7.0.5.102 10/14/2016
            Dim strVal = getScalarString(EconnectSTr, String.Format(DefaultIntegrationConfiguration.GPFunctionsTotalPOWeight, OrderNumber))
            If Not String.IsNullOrWhiteSpace(strVal) Then Double.TryParse(strVal, ReturnValue)

        Catch ex As ApplicationException
            ReturnValue = -1
            Dim strMsg As String = ex.Message
            If Not ex.InnerException Is Nothing Then strMsg &= " -- inner -- " & ex.InnerException.Message
            Me.Log(Source & ".GetPOTotalWeight Error: Process PO Total Weight Query Exception: " & strMsg)
        Catch ex As Exception
            Me.Log(Source & ".GetPOTotalWeight Error: Unexpected PO Total Weight Query Exception: " & ex.Message)
            Throw 'should not continue
        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Depricated by RHR v-7.0.5.102 on 10/17/2016  we now calculate the weight inside the item details query
    ''' </summary>
    ''' <param name="EconnectSTr"></param>
    ''' <param name="ItemNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/17/2016
    ''' </remarks>
    Public Function GetItemWeight(ByVal EconnectSTr As String, ByVal ItemNumber As String) As Double

        Dim ReturnValue As Double = 0

        Dim SQLConn = New SqlConnection(EconnectSTr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            'Integrate Sales Order

            'Assume Order for now and assign SOPType = 2

            SQLCom.CommandText = "select ITEMSHWT from IV00101 where ITEMNMBR = '" + ItemNumber + "'"

            TblRec = SQLCom.ExecuteReader

            TblRec.Read()

            If (TblRec.HasRows) Then

                Double.TryParse(TblRec("ITEMSHWT").ToString, ReturnValue)

            Else

                ReturnValue = -1

            End If

            TblRec.Close()

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get Item Weight Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    Public Function FindVendor(ByVal sSource As String, ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal VendorID As String) As GPObject.GPDataIntegrationSTructure.VendorFound

        Dim ReturnValue As New GPObject.GPDataIntegrationSTructure.VendorFound

        ReturnValue.GPCompany = "NA"
        ReturnValue.VendorID = "NA"

        Dim ErrorString As String = ""
        Dim DatabaseName As String = ""
        Dim EconnectStr As String = ""
        Dim VendorFound As Boolean = False

        Try

            EconnectStr = sSource

            Dim SQLConn = New SqlConnection(EconnectStr)
            Dim SQLCom As New SqlCommand
            Dim TblRec As SqlDataReader

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            SQLCom.CommandText = "select * from PM00200 where VENDORID = '" + VendorID + "'"

            TblRec = SQLCom.ExecuteReader

            TblRec.Read()

            If (TblRec.HasRows) Then

                If (ReturnValue.GPCompany = "NA") Then

                    ReturnValue.VendorID = Trim(TblRec("VENDORID").ToString)

                    ReturnValue.GPCompany = "Found"

                    VendorFound = True

                End If

            End If

            TblRec.Close()

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Find Vendor Data Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw

        Finally
            Me.closeLog(0)

        End Try

        Try

            If (Not VendorFound) Then

                If Me.Verbose Then Log("Begin Process Data ")
                'save the ERPFrieghtAccountIndex for later use

                'get a list of nav settings
                Dim oConfig As New UserConfiguration()
                With oConfig
                    .AdminEmail = Me.AdminEmail
                    .AutoRetry = Me.AutoRetry
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .Debug = Me.Debug
                    .FromEmail = Me.FromEmail
                    .GroupEmail = Me.GroupEmail
                    .LogFile = Me.LogFile
                    .SMTPServer = Me.SMTPServer
                    .UserName = "System Download"
                    .WSAuthCode = "NGLSystem"
                    .WCFAuthCode = "NGLSystem"
                    .WCFURL = ""
                    .WCFTCPURL = ""
                    .ConnectionString = Me.ConnectionString

                End With

                Dim oSettings As tmsintegrationsettings.vERPIntegrationSetting()

#Disable Warning BC42104 ' Variable 'oSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
#Enable Warning BC42104 ' Variable 'oSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
                    'group the settings by Legal Entity
                    Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                    If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                        For Each legal In sLegals
                            Dim lLegalSettings As tmsintegrationsettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                            If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                                Me.LegalEntity = legal

                                If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
                                    EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & legal.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
                                Else
                                    EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & legal.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
                                End If
                                'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & legal.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"

                                Dim SQLConn = New SqlConnection(EconnectStr)
                                Dim SQLCom As New SqlCommand
                                Dim TblRec As SqlDataReader

                                SQLConn.Open()

                                SQLCom.Connection = SQLConn

                                SQLCom.CommandText = "select * from PM00200 where VENDORID = '" + VendorID + "'"

                                TblRec = SQLCom.ExecuteReader

                                TblRec.Read()

                                If (TblRec.HasRows) Then

                                    If (ReturnValue.GPCompany = "NA") Then

                                        ReturnValue.VendorID = Trim(TblRec("VENDORID").ToString)

                                        ReturnValue.GPCompany = legal

                                    End If

                                End If

                                TblRec.Close()

                                SQLCom.Dispose()

                                SQLConn.Dispose()

                            End If

                        Next

                    End If

                End If

            End If

            If Me.Verbose Then Log("Process Data Complete")
            'TODO: add additional error handlers as needed

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected GP Process Data Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw

        Finally
            Me.closeLog(0)

        End Try

        Return ReturnValue

    End Function

    Public Function InsertUserLink(ByVal EconnectSTr As String, ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting) As Boolean

        Dim ReturnValue As Boolean = True

        Dim SQLConn = New SqlConnection(EconnectSTr)
        Dim SQLCom As New SqlCommand
        'Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try

            If (Not String.IsNullOrWhiteSpace(TMSSetting.IntegrationTypeName)) Then

                SQLConn.Open()

                SQLCom.Connection = SQLConn

                'Integrate Sales Order

                'Assume Order for now and assign SOPType = 2

                SQLCom.CommandText = "insert into DYNAMICS.dbo.SY08140 (USERID, SEQNUMBR, TYPEID, CmdID, CmdFormID, CmdDictID, DSPLNAME, ScbTargetStringOne) "
                SQLCom.CommandText = SQLCom.CommandText & " select usr.USERID, isnull(maxid.max_id,1), 2, 0, 0, 0, '" & TMSSetting.IntegrationTypeName.ToString & "', '" & TMSSetting.TMSURI.ToString & "' from DYNAMICS.dbo.SY01400 usr left outer join (select userid, MAX(SEQNUMBR) + 1 as max_id  from sy08140 group by userid) maxid on maxid.USERID = usr.USERID where usr.USERID not in (select userid from DYNAMICS.dbo.SY08140 where DSPLNAME = '" & TMSSetting.IntegrationTypeName.ToString & "')"

                SQLCom.Dispose()

                SQLConn.Dispose()

            End If

        Catch ex As Exception

            ReturnValue = False

            LogError(Source & " Error!  Unexpected GP Insert User Links", Source & ".  The actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


    'Public Function GetFrieghtAccountIndex() As Integer

    '    Dim ReturnValue As Integer = 520

    '    Return ReturnValue

    'End Function

    Public Function GetTimeToRun(ByVal LastRunDate As Date) As Boolean

        Dim ReturnValue As Boolean = True

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Returns the Trans Type for the provided Transaction Type 0 = PO or SO = 1  and TransactionNumber 
    ''' TransactionMasterID and TransactionLineNumber  are not being used
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="TransactionType"></param>
    ''' <param name="TransactionNumber"></param>
    ''' <param name="TransactionLineNumber"></param>
    ''' <param name="TransactionMasterID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 11/21/2016
    '''   added logic to use sql strings stored in configuration settings
    '''   added default for Return value to use OutboundVendorDelivered
    ''' </remarks>
    Public Function GetShippingMethod(ByVal EconnectStr As String, ByVal TransactionType As Int16, ByVal TransactionNumber As String, ByVal TransactionLineNumber As Integer, ByVal TransactionMasterID As String, ByVal SOPType As Integer) As Short
        'Modified by RHR for v-7.0.5.102 11/21/2016
        Dim ReturnValue As Short = GPEnumValues.FrieghtType.OutboundVendorDelivered
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try
            SQLConn.Open()
            SQLCom.Connection = SQLConn
            'Integrate Sales Order
            'Ship Method Type 0 - pickup, 1 - Delivery
            '3 = Vendor Origin Dock-CPU - SOP - Customer Pickup
            '4 = Outbound Vendor Delivered - SOP - Deliver
            '5 = Inbound Vendor Delivered - PO - Delivered Pricing, vendor pays freight
            '8 = Inbound FOB Vendor Dock - 
            'Assume Order for now and assign SOPType = 2

            Select Case TransactionType
                Case 0
                    'set up default
                    ReturnValue = GPEnumValues.FrieghtType.InboundFOBVendorDock

                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetPOShippingMethod, Trim(TransactionNumber))
                    'Modified by RHR for v-7.0.5.102 11/21/2016


                Case 5
                    'Modified by SEM for v-7.0.6.103 Update
                    'adding transfer order
                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetTOShippingMethod, Trim(TransactionNumber))
                    'SQLCom.CommandText = "select sopln.SHIPMTHD, sh.SHMTHDSC, sh.SHIPTYPE from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD  where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '" & Trim(TransactionNumber) & "'"

                Case Else
                    'Modified by RHR for v-7.0.5.102 11/21/2016
                    'assume Sales Order by default
                    Select Case SOPType
                        Case 2
                            SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetSOShippingMethod, Trim(TransactionNumber))

                        Case 3
                            SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetSOConShippingMethod, Trim(TransactionNumber))

                    End Select

                    'SQLCom.CommandText = "select sopln.SHIPMTHD, sh.SHMTHDSC, sh.SHIPTYPE from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD  where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '" & Trim(TransactionNumber) & "'"
            End Select
            TblRec = SQLCom.ExecuteReader
            TblRec.Read()
            If (TblRec.HasRows) Then
                Dim LineCols = TblRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()
                If (LineCols.Contains("TransType")) Then
                    TryParseInt(TblRec("TransType").ToString(), ReturnValue)
                End If
            End If


        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get Shipping Method Error", Source & " Could not process the shipping method for Shipping Transaction " & Trim(TransactionNumber) & "; using " & ReturnValue.ToString() & " as the default; the actual error is:  " & ex.Message, AdminEmail)

        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Returns the Temperature for the Transaction Type 0= PO or SO = 1 provided in TransactionNumber 
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="TransactionType"></param>
    ''' <param name="TransactionNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    '''   we now use sql strings stored in configuration settings to look up temperature
    '''   the default for Return value is D for dry
    ''' </remarks>
    Public Function GetTemp(ByVal EconnectStr As String, ByVal TransactionType As Int16, ByVal TransactionNumber As String, ByVal SOPType As Integer) As String

        Dim ReturnValue As String = "D"
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try
            SQLConn.Open()
            SQLCom.Connection = SQLConn

            'Assume Order for now and assign SOPType = 2
            'Dim SOPType As Integer = 2
            Select Case TransactionType
                Case 0

                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetPOTemp, Trim(TransactionNumber))

                'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                'Adding logic GP Inventory Transfer Order

                Case 5
                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetTOTemp, Trim(TransactionNumber))

                Case Else
                    'assume Sales Order by default
                    Select Case SOPType
                        Case 3
                            SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetSOConTemp, Trim(TransactionNumber))
                        Case Else
                            SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetSOTemp, Trim(TransactionNumber))
                    End Select


            End Select
            TblRec = SQLCom.ExecuteReader
            TblRec.Read()
            If (TblRec.HasRows) Then
                Dim LineCols = TblRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()
                If (LineCols.Contains("SOTemp")) Then
                    ReturnValue = TblRec("SOTemp").ToString()
                End If
            End If
        Catch ex As Exception
            LogError(Source & " Error! Unexpected GP Get Temperature Error", Source & " Could not process the get temperature request for Shipping Transaction " & Trim(TransactionNumber) & "; using default value of D for Dry; the actual error is:  " & ex.Message, AdminEmail)
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


    'Added by SEM 2017-08-04
    Public Function GetItemTemp(ByVal EconnectStr As String, ByVal TransactionType As Int16, ByVal TransactionNumber As String, ByVal ItemNumber As String) As String

        Dim ReturnValue As String = "D"
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try
            SQLConn.Open()
            SQLCom.Connection = SQLConn

            'Assume Order for now and assign SOPType = 2
            Dim SOPType As Integer = 2
            Select Case TransactionType
                Case 0
                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetPOTemp, Trim(TransactionNumber))


                'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                'Adding logic GP Inventory Transfer Order

                Case 5
                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetTOTemp, Trim(TransactionNumber))

                Case Else
                    'assume Sales Order by default
                    SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionGetItemTemp, Trim(TransactionNumber), Trim(ItemNumber))
            End Select
            TblRec = SQLCom.ExecuteReader
            TblRec.Read()
            If (TblRec.HasRows) Then
                Dim LineCols = TblRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()
                If (LineCols.Contains("SOTemp")) Then
                    ReturnValue = TblRec("SOTemp").ToString()
                End If
            End If
        Catch ex As Exception
            LogError(Source & " Error! Unexpected GP Get Item Temperature Error", Source & " Could not process the get item temperature request for Shipping Transaction " & Trim(TransactionNumber) & "; using default value of D for Dry; the actual error is:  " & ex.Message, AdminEmail)
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


#Disable Warning BC42307 ' XML comment parameter 'TransactionNumber' does not match a parameter on the corresponding 'function' statement.
    ''' <summary>
    ''' Returns the Temperature for the Transaction Type 0= PO or SO = 1 provided in TransactionNumber 
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="TransactionType"></param>
    ''' <param name="TransactionNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    '''   we now use sql strings stored in configuration settings to look up temperature
    '''   the default for Return value is D for dry
    ''' </remarks>
    Public Function GetMaxSOPDexID(ByVal EconnectStr As String, ByVal TransactionType As Int16, ByVal CurrentDexID As Long) As Long
#Enable Warning BC42307 ' XML comment parameter 'TransactionNumber' does not match a parameter on the corresponding 'function' statement.

        Dim ReturnValue As Long = 0
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim QStr As String = ""

        Try
            SQLConn.Open()
            SQLCom.Connection = SQLConn

            'Assume Order for now and assign SOPType = 2
            Select Case TransactionType
                Case 0
                    'QStr = String.Format(DefaultIntegrationConfiguration.GPFunctionGetNextSOPInvDexID, CurrentDexID)

                'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                'Adding logic GP Inventory Transfer Order

                Case 5
                    'SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetTOTemp, Trim(TransactionNumber))

                Case Else
                    'assume Sales Order by default
                    'SQLCom.CommandText = String.Format(DefaultIntegrationConfiguration.GPFunctionsGetSOTemp, Trim(TransactionNumber))
            End Select

            If (QStr <> "") Then

                SQLCom.CommandText = QStr

                TblRec = SQLCom.ExecuteReader
                TblRec.Read()

                If (TblRec.HasRows) Then
                    ReturnValue = TblRec(0).ToString()
                End If

                TblRec.Close()

            End If

        Catch ex As Exception
            LogError(Source & " Error! Unexpected GP Get Temperature Error", Source & " Could not process the get temperature request for Shipping Transaction " & Trim(CurrentDexID) & "; using default value of D for Dry; the actual error is:  " & ex.Message, AdminEmail)
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


    Public Function CalculateLane(ByVal EconnectStr As String, ByVal TransactionType As Int16, ByVal TransactionNumber As String, ByVal TransactionLineNumber As Integer, ByVal TransactionMasterID As String, ByVal ProAbb As String, ByVal SOPType As Integer, Optional ByRef SErrors As String = "") As String

        Dim ReturnValue As String = ""

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            'Integrate Sales Order

            Select Case TransactionType

                Case 1

                    'Assume Order for now and assign SOPType = 2

                    Select Case SOPType
                        Case 2

                            SQLCom.CommandText = "select hd.CUSTNMBR, ln.LOCNCODE, ln.PRSTADCD from SOP10100 hd inner join SOP10200 ln on ln.SOPNUMBE = hd.SOPNUMBE where hd.SOPTYPE = " & SOPType & " and hd.SOPNUMBE = '" & Trim(TransactionNumber) & "'  and ln.LNITMSEQ = " & TransactionLineNumber

                        Case 3

                            SQLCom.CommandText = "select hd.CUSTNMBR, ln.LOCNCODE, ln.PRSTADCD from SOP30200 hd inner join SOP30300 ln on ln.SOPNUMBE = hd.SOPNUMBE where hd.SOPTYPE = " & SOPType & " and hd.SOPNUMBE = '" & Trim(TransactionNumber) & "'  and ln.LNITMSEQ = " & TransactionLineNumber

                    End Select

                    TblRec = SQLCom.ExecuteReader

                    TblRec.Read()

                    If (TblRec.HasRows) Then

                        'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                        ReturnValue = ProAbb & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)

                        TblRec.Close()

                    Else

                        TblRec.Close()

                        Select Case SOPType
                            Case 2
                                SQLCom.CommandText = "select top 1 LNITMSEQ from SOP10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & TransactionNumber & "'"

                            Case 3
                                SQLCom.CommandText = "select top 1 LNITMSEQ from SOP30300 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & TransactionNumber & "'"

                        End Select

                        TblRec = SQLCom.ExecuteReader

                        TblRec.Read()

                        If (TblRec.HasRows) Then

                            TransactionLineNumber = TblRec(0).ToString

                        Else

                            ReturnValue = "Error"

                            SErrors = "For SOP Order #:  " & TransactionNumber & "  and Line #:  " & TransactionLineNumber

                        End If

                        TblRec.Close()

                        If (ReturnValue <> "Error") Then

                            Select Case SOPType
                                Case 2
                                    SQLCom.CommandText = "select hd.CUSTNMBR, ln.LOCNCODE, ln.PRSTADCD from SOP10100 hd inner join SOP10200 ln on ln.SOPNUMBE = hd.SOPNUMBE where hd.SOPTYPE = " & SOPType & " and hd.SOPNUMBE = '" & Trim(TransactionNumber) & "'  and ln.LNITMSEQ = " & TransactionLineNumber

                                Case 3
                                    SQLCom.CommandText = "select hd.CUSTNMBR, ln.LOCNCODE, ln.PRSTADCD from SOP30200 hd inner join SOP30300 ln on ln.SOPNUMBE = hd.SOPNUMBE where hd.SOPTYPE = " & SOPType & " and hd.SOPNUMBE = '" & Trim(TransactionNumber) & "'  and ln.LNITMSEQ = " & TransactionLineNumber

                            End Select


                            TblRec = SQLCom.ExecuteReader

                            TblRec.Read()

                            If (TblRec.HasRows) Then

                                'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                                ReturnValue = ProAbb & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)

                            Else

                                ReturnValue = "Error"

                                SErrors = "For SOP Order #:  " & TransactionNumber & "  and Line #:  " & TransactionLineNumber

                            End If

                            TblRec.Close()

                        End If

                        TblRec.Close()

                    End If

                Case 2

                    'Assume Order for now and assign SOPType = 2

                    SQLCom.CommandText = "select PRSTADCD, VENDORID from pop10100 where PONUMBER = '" & TransactionNumber & "'"

                    TblRec = SQLCom.ExecuteReader

                    TblRec.Read()

                    If (TblRec.HasRows) Then

                        'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("VENDORID").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                        'Modified by SEM 2017-07-26, removed Location code
                        ReturnValue = Trim(TblRec("PRSTADCD").ToString) & "-" & Trim(TblRec("VENDORID").ToString) & "-" & ProAbb

                        TblRec.Close()

                    Else

                        TblRec.Close()

                        SQLCom.CommandText = "select top 1 ADRSCODE, VENDORID from pop10110 where PONUMBER = '" & TransactionNumber & "' and POLNESTA in (1, 2, 3)"

                        TblRec = SQLCom.ExecuteReader

                        TblRec.Read()

                        If (TblRec.HasRows) Then

                            'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                            ReturnValue = Trim(TblRec("ADRSCODE").ToString) & "-" & Trim(TblRec("VENDORID").ToString) & "-" & ProAbb

                        Else

                            ReturnValue = "Error"

                            SErrors = "For PO Order #:  " & TransactionNumber

                        End If

                        TblRec.Close()

                    End If

                'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                'Adding Lane for GP Inventory Transfer Order

                Case 5

                    SQLCom.CommandText = "select top 1 'IVTRSFR' as ID, ln.TRNSFLOC, ln.TRNSTLOC from SVC00700 hd inner join SVC00701 ln on ln.ORDDOCID = hd.ORDDOCID where hd.ORDDOCID = '" & Trim(TransactionNumber) & "'"

                    TblRec = SQLCom.ExecuteReader

                    TblRec.Read()

                    If (TblRec.HasRows) Then

                        'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                        ReturnValue = ProAbb & "-" & Trim(TblRec("TRNSFLOC").ToString) & "-" & Trim(TblRec("TRNSTLOC").ToString)

                        TblRec.Close()

                    Else

                        ReturnValue = "Error"

                        SErrors = "For Inventory Transfer Order #:  " & TransactionNumber

                        TblRec.Close()

                    End If

                Case Else

                    ReturnValue = "Error"

                    SErrors = "There was an invalid transaction type passed into Calcuate Lane"

            End Select

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected Calculate Lane Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="ErrorString"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 11/28/2016
    '''   added GPFunctionsGetPayableCreditGL configuration setting
    ''' </remarks>
    Public Function GetPayableCreditGL(ByVal EconnectStr As String, ByRef ErrorString As String) As String

        Dim ReturnValue As String = ""

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim Counter As Integer = 0

        Try

            ErrorString = ""

            SQLConn.Open()

            SQLCom.Connection = SQLConn

            'Integrate Sales Order

            'Modified by RHR for v-7.0.5.102 11/28/2016
            'SQLCom.CommandText = "SELECT rtrim(gl.ACTNUMST) as gl_code from SY01100 post inner join gl00105  gl on gl.ACTINDX = post.ACTINDX where SERIES = 4 and SEQNUMBR = 200"
            SQLCom.CommandText = DefaultIntegrationConfiguration.GPFunctionsGetPayableCreditGL
            TblRec = SQLCom.ExecuteReader

            TblRec.Read()

            If (TblRec.HasRows) Then

                'ReturnValue = Trim(TblRec("LOCNCODE").ToString) & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)
                ReturnValue = TblRec("gl_code").ToString

                TblRec.Close()

            Else

                SQLCom.CommandText = "select top 1 rtrim(gl.ACTNUMST) as gl_code from GL00100 glinfo inner join gl00105 gl on gl.ACTINDX = glinfo.ACTINDX where glinfo.ACTIVE = 1 order by glinfo.DEX_ROW_ID"

                TblRec.Read()

                If (TblRec.HasRows) Then

                    ReturnValue = TblRec("gl_ocde").ToString

                Else

                    ErrorString = "There were no records returned in the Freight Invoice Payables Credit GL Account."

                    ReturnValue = "Error"

                End If

                TblRec.Close()

            End If

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected Get Payables Credit GL Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Executes a SQL string and returns a single value as a string.  The sql query must
    ''' be constructed to return a single value in a single row
    ''' </summary>
    ''' <param name="sConnection"></param>
    ''' <param name="strSQL"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/17/2016
    '''   unified method for executiong scaler query
    '''   Throws Application exception on SQL Error 
    '''   re throws all other exceptions
    '''   caller should handle applicaiton exception and log the error and return a default value
    '''   caller should re throw all other exceptions
    ''' </remarks>
    Public Function getScalarString(ByVal sConnection As String, ByVal strSQL As String) As String

        Dim oQuery As New Data.Query(sConnection)
        Dim oCon As New System.Data.SqlClient.SqlConnection(sConnection)
        Dim strRet As String = ""
        Try
            oCon.Open()
            'If oCon is passed before it has been opened the funtion will create a new connection.
            strRet = oQuery.getScalarValue(oCon, strSQL, 1)
        Catch ex As System.Data.SqlClient.SqlException
            Throw New ApplicationException(ex.Message, ex)
        Catch ex As Exception
            Throw
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
        Return strRet
    End Function


    ''' <summary>
    ''' Tries to converts a string value to double then round to integer. Default is zero if string cannot be converted.  All errors are ignored.
    ''' </summary>
    ''' <param name="sVal"></param>
    ''' <param name="intVal"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/16/2016
    '''  used to convert the string value of any number to an integer 
    ''' </remarks>
    Public Shared Function TryParseInt(ByVal sVal As String, ByRef intVal As Integer) As Boolean
        Dim blnRet As Boolean = False
        intVal = 0
        Dim dblVal As Double = 0
        Try
            If Double.TryParse(sVal, dblVal) Then
                intVal = CInt(dblVal)
                blnRet = True
            End If
        Catch ex As Exception
            'do nothing
        End Try

        Return blnRet
    End Function

    Public Shared Function TryParseDate(ByVal sDate As String, ByVal d As Date) As Date
        Date.TryParse(sDate, d)
        Return d
    End Function

    ''' <summary>
    ''' Validates that sDate is a valid date but not 1900-01-01 if not use default.  If both string are not dates return an empty string
    ''' </summary>
    ''' <param name="sDate"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 11/29/2017
    '''   use to allow docdate as default for shipdate and other date values
    ''' </remarks>
    Public Shared Function ValidateGPDateString(ByVal sDate As String, ByVal sDefault As String) As String
        Dim dtTest As Date = Date.Now
        Dim dtToReturn As Date = Date.Now
        Dim sReturn As String = "" 'empty string by default if no dates work
        Dim sRet As String = "" 'empty string 
        Dim blnDateFound As Boolean = False
        If String.IsNullOrEmpty(sDate) OrElse sDate = "1900-01-01 00:00:00.000" Then ' do not use use default
            sDate = sDefault
        Else
            'if the date string is not empty and it is not a date we still use the default
            If Not Date.TryParse(sDate, dtTest) Then
                sDate = sDefault
            Else
                blnDateFound = True
                sReturn = dtTest.ToString()
            End If
        End If
        'we are using the default so run a second test to be sure this is a valid date.
        If Not blnDateFound AndAlso Date.TryParse(sDate, dtTest) Then
            sReturn = dtTest.ToString()
        End If
        Return sReturn
    End Function

    Public Function GetSOPRequiredShipDate(ByVal EconnectStr As String, ByVal TransactionNumber As String, ByVal SOPType As Integer, ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal ADminemail As String) As String

        Dim ReturnValue As String = ""

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try
            SQLConn.Open()
            SQLCom.Connection = SQLConn

            'Assume Order for now and assign SOPType = 2
            'Dim SOPType As Integer = 2
            Select Case SOPType

                Case 2

                    SQLCom.CommandText = String.Format(c.GPSOPOrdRequiredShipDate, TransactionNumber)

                'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                'Adding logic GP Inventory Transfer Order

                Case 3
                    SQLCom.CommandText = String.Format(c.GPSOPShipConfirmRequiredShipDate, TransactionNumber)

                Case Else
                    'assume Sales Order by default
                    ReturnValue = ""

            End Select

            TblRec = SQLCom.ExecuteReader
            TblRec.Read()
            If (TblRec.HasRows) Then

                ReturnValue = TblRec(0).ToString

            Else

                ReturnValue = ""

            End If

            TblRec.Close()
        Catch ex As Exception
            LogError(Source & " Error! Unexpected GP Get Required Ship Date Error", Source & " Could not process the get required ship date request for Shipping Transaction " & Trim(TransactionNumber) & "; the actual error is:  " & ex.Message, ADminemail)
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try

        End Try


        Return ReturnValue

    End Function

    Private Function getTMSIntegrationSettings(ByVal c As clsDefaultIntegrationConfiguration, ByRef oSettings As tmsintegrationsettings.vERPIntegrationSetting()) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oSettingObject As New tmsintegrationsettings.DTMSIntegration()
            oSettingObject.Url = c.TMSSettingsURI
            Dim ReturnMessage As String
            Dim ERPTypeName As String = "GP"
            Dim RetVal As Integer
#Disable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            oSettings = oSettingObject.getvERPIntegrationSettingsByName(c.TMSSettingsAuthCode, c.TMSRunLegalEntity, ERPTypeName, RetVal, ReturnMessage)
#Enable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If RetVal <> TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not read Integration Settings information:  " & ReturnMessage)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Read Integration Settings Error", "Error Data Validation Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                    Case Else
                        LogError("Read Integration Settings Error", "Error Integration Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                End Select
            Else
                blnRet = True
            End If

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read Integration Settings Error", Source & " Unexpected Read Integration Settings Error! Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
            blnRet = False
        End Try
        Return blnRet

    End Function

End Class
