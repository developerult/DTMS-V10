Imports System.Data.Linq
Imports System.ServiceModel

Public Class NGLCarrierTariffRowData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffMatrixes
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierTariffRowData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMatrixes
                Me.LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrierTariffRowFiltered(Control:=Control)
    End Function

    ''' <summary>
    ''' this method do not return any records because the Carrier Tariff Control number is required.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrierTariffRowFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierTariffRow
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffMatrix)(Function(t As LTS.CarrierTariffMatrix) t.CarrierTariffMatrixDetails)
                db.LoadOptions = oDLO

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffRow As DataTransferObjects.CarrierTariffRow = (
                        From d In db.CarrierTariffMatrixes
                        Where
                        d.CarrTarMatControl = Control
                        Select New DataTransferObjects.CarrierTariffRow With {.CarrTarMatControl = d.CarrTarMatControl,
                        .CarrTarMatCarrTarControl = d.CarrTarMatCarrTarControl,
                        .ZipFrom = d.CarrTarMatFromZip,
                        .ZipTo = d.CarrTarMatToZip,
                        .ExportFlag = d.CarrTarMatExptFlag,
                        .MinVal = d.CarrTarMatMin,
                        .MaxDays = d.CarrTarMatMaxDays,
                        .CarrTarMatModUser = d.CarrTarMatModUser,
                        .CarrTarMatModDate = d.CarrTarMatModDate,
                        .CarrTarMatUpdated = d.CarrTarMatUpdated.ToArray(),
                        .CarrierTariffMatrixDetails = (
                        From b In d.CarrierTariffMatrixDetails
                        Select New DataTransferObjects.CarrierTariffMatrixDetail With {
                        .CarrTarMatDetControl = b.CarrTarMatDetControl,
                        .CarrTarMatDetCarrTarMatControl = b.CarrTarMatDetCarrTarMatControl,
                        .CarrTarMatDetID = b.CarrTarMatDetID,
                        .CarrTarMatDetValue = If(b.CarrTarMatDetValue.HasValue, b.CarrTarMatDetValue, 0),
                        .CarrTarMatDetModUser = b.CarrTarMatDetModUser,
                        .CarrTarMatDetModDate = b.CarrTarMatDetModDate,
                        .CarrTarMatDetUpdated = b.CarrTarMatDetUpdated.ToArray()}).ToList()}).First

                For Each TariffDetail As DataTransferObjects.CarrierTariffMatrixDetail In CarrierTariffRow.CarrierTariffMatrixDetails
                    With TariffDetail
                        Select Case .CarrTarMatDetID
                            Case 1
                                CarrierTariffRow.DetControl1 = .CarrTarMatDetControl
                                CarrierTariffRow.Break1 = .CarrTarMatDetValue
                            Case 2
                                CarrierTariffRow.DetControl2 = .CarrTarMatDetControl
                                CarrierTariffRow.Break2 = .CarrTarMatDetValue
                            Case 3
                                CarrierTariffRow.DetControl3 = .CarrTarMatDetControl
                                CarrierTariffRow.Break3 = .CarrTarMatDetValue
                            Case 4
                                CarrierTariffRow.DetControl4 = .CarrTarMatDetControl
                                CarrierTariffRow.Break4 = .CarrTarMatDetValue
                            Case 5
                                CarrierTariffRow.DetControl5 = .CarrTarMatDetControl
                                CarrierTariffRow.Break5 = .CarrTarMatDetValue
                            Case 6
                                CarrierTariffRow.DetControl6 = .CarrTarMatDetControl
                                CarrierTariffRow.Break6 = .CarrTarMatDetValue
                            Case 7
                                CarrierTariffRow.DetControl7 = .CarrTarMatDetControl
                                CarrierTariffRow.Break7 = .CarrTarMatDetValue
                            Case 8
                                CarrierTariffRow.DetControl8 = .CarrTarMatDetControl
                                CarrierTariffRow.Break8 = .CarrTarMatDetValue
                            Case 9
                                CarrierTariffRow.DetControl9 = .CarrTarMatDetControl
                                CarrierTariffRow.Break9 = .CarrTarMatDetValue
                            Case 10
                                CarrierTariffRow.DetControl10 = .CarrTarMatDetControl
                                CarrierTariffRow.Break10 = .CarrTarMatDetValue
                            Case 11
                                CarrierTariffRow.DetControl11 = .CarrTarMatDetControl
                                CarrierTariffRow.Break11 = .CarrTarMatDetValue
                        End Select
                    End With
                Next

                Return CarrierTariffRow

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The only filter allowed is the Carrier Tariff Control number
    ''' </summary>
    ''' <param name="CarrTarMatCarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierTariffRowsFiltered(ByVal CarrTarMatCarrTarControl As Integer,
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffRow()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Count the records
                Dim rows = From d In db.CarrierTariffMatrixes
                        Where
                        d.CarrTarMatCarrTarControl = CarrTarMatCarrTarControl
                        Select Control = d.CarrTarMatControl
                intRecordCount = rows.Count
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffMatrix)(Function(t As LTS.CarrierTariffMatrix) t.CarrierTariffMatrixDetails)
                db.LoadOptions = oDLO

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffRow() As DataTransferObjects.CarrierTariffRow = (
                        From d In db.CarrierTariffMatrixes
                        Where
                        d.CarrTarMatCarrTarControl = CarrTarMatCarrTarControl
                        Select New DataTransferObjects.CarrierTariffRow With {.CarrTarMatControl = d.CarrTarMatControl,
                        .CarrTarMatCarrTarControl = d.CarrTarMatCarrTarControl,
                        .ZipFrom = d.CarrTarMatFromZip,
                        .ZipTo = d.CarrTarMatToZip,
                        .ExportFlag = d.CarrTarMatExptFlag,
                        .MinVal = d.CarrTarMatMin,
                        .MaxDays = d.CarrTarMatMaxDays,
                        .CarrTarMatModUser = d.CarrTarMatModUser,
                        .CarrTarMatModDate = d.CarrTarMatModDate,
                        .Page = page,
                        .Pages = intPageCount,
                        .CarrTarMatUpdated = d.CarrTarMatUpdated.ToArray(),
                        .CarrierTariffMatrixDetails = (
                        From b In d.CarrierTariffMatrixDetails
                        Select New DataTransferObjects.CarrierTariffMatrixDetail With {
                        .CarrTarMatDetControl = b.CarrTarMatDetControl,
                        .CarrTarMatDetCarrTarMatControl = b.CarrTarMatDetCarrTarMatControl,
                        .CarrTarMatDetID = b.CarrTarMatDetID,
                        .CarrTarMatDetValue = If(b.CarrTarMatDetValue.HasValue, b.CarrTarMatDetValue, 0),
                        .CarrTarMatDetModUser = b.CarrTarMatDetModUser,
                        .CarrTarMatDetModDate = b.CarrTarMatDetModDate,
                        .CarrTarMatDetUpdated = b.CarrTarMatDetUpdated.ToArray()}).ToList()}).Skip(intSkip).Take(pagesize).ToArray()

                For Each TariffRow As DataTransferObjects.CarrierTariffRow In CarrierTariffRow
                    For Each TariffDetail As DataTransferObjects.CarrierTariffMatrixDetail In TariffRow.CarrierTariffMatrixDetails
                        With TariffDetail
                            Select Case .CarrTarMatDetID
                                Case 1
                                    TariffRow.DetControl1 = .CarrTarMatDetControl
                                    TariffRow.Break1 = .CarrTarMatDetValue
                                Case 2
                                    TariffRow.DetControl2 = .CarrTarMatDetControl
                                    TariffRow.Break2 = .CarrTarMatDetValue
                                Case 3
                                    TariffRow.DetControl3 = .CarrTarMatDetControl
                                    TariffRow.Break3 = .CarrTarMatDetValue
                                Case 4
                                    TariffRow.DetControl4 = .CarrTarMatDetControl
                                    TariffRow.Break4 = .CarrTarMatDetValue
                                Case 5
                                    TariffRow.DetControl5 = .CarrTarMatDetControl
                                    TariffRow.Break5 = .CarrTarMatDetValue
                                Case 6
                                    TariffRow.DetControl6 = .CarrTarMatDetControl
                                    TariffRow.Break6 = .CarrTarMatDetValue
                                Case 7
                                    TariffRow.DetControl7 = .CarrTarMatDetControl
                                    TariffRow.Break7 = .CarrTarMatDetValue
                                Case 8
                                    TariffRow.DetControl8 = .CarrTarMatDetControl
                                    TariffRow.Break8 = .CarrTarMatDetValue
                                Case 9
                                    TariffRow.DetControl9 = .CarrTarMatDetControl
                                    TariffRow.Break9 = .CarrTarMatDetValue
                                Case 10
                                    TariffRow.DetControl10 = .CarrTarMatDetControl
                                    TariffRow.Break10 = .CarrTarMatDetValue
                                Case 11
                                    TariffRow.DetControl11 = .CarrTarMatDetControl
                                    TariffRow.Break11 = .CarrTarMatDetValue
                            End Select
                        End With
                    Next
                Next

                Return CarrierTariffRow

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function isZipRangeInUse(ByVal CarrTarControl As Integer,
                                    ByVal FromZip As String,
                                    ByVal ToZip As String,
                                    ByRef ReturnMsg As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim CarrierTariffMatrix = (From d In db.CarrierTariffMatrixes
                        Where
                        d.CarrTarMatCarrTarControl = CarrTarControl _
                        And
                        d.CarrTarMatFromZip <= FromZip _
                        And
                        d.CarrTarMatToZip >= ToZip _
                        And
                        (
                            (
                                (FromZip >= d.CarrTarMatFromZip And FromZip <= d.CarrTarMatToZip) _
                                Or
                                (ToZip >= d.CarrTarMatFromZip And ToZip <= d.CarrTarMatToZip)
                                ) _
                            Or
                            (FromZip <= d.CarrTarMatFromZip And ToZip >= d.CarrTarMatFromZip) _
                            Or
                            (FromZip <= d.CarrTarMatToZip And ToZip >= d.CarrTarMatToZip)
                            )
                        Select d).ToList

                If Not CarrierTariffMatrix Is Nothing AndAlso CarrierTariffMatrix.Count > 0 Then
                    ReturnMsg = "The selected postal code range is already in use see tariff for From: " & CarrierTariffMatrix(0).CarrTarMatFromZip & " To: " & CarrierTariffMatrix(0).CarrTarMatToZip
                    blnRet = True
                End If


            Catch ex As FaultException
                Throw
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'do nothing this is the desired result (no records)
            Catch ex As Exception
                Throw
            End Try

            Return blnRet

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierTariffRow)
        'Create New Record
        Return New LTS.CarrierTariffMatrix With {.CarrTarMatControl = d.CarrTarMatControl,
            .CarrTarMatCarrTarControl = d.CarrTarMatCarrTarControl,
            .CarrTarMatFromZip = d.ZipFrom,
            .CarrTarMatToZip = d.ZipTo,
            .CarrTarMatExptFlag = d.ExportFlag,
            .CarrTarMatMin = d.MinVal,
            .CarrTarMatMaxDays = d.MaxDays,
            .CarrTarMatModUser = Parameters.UserName,
            .CarrTarMatModDate = Date.Now,
            .CarrTarMatUpdated = If(d.CarrTarMatUpdated Is Nothing, New Byte() {}, d.CarrTarMatUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierTariffRowFiltered(Control:=CType(LinqTable, LTS.CarrierTariffMatrix).CarrTarMatControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffMatrix = TryCast(LinqTable, LTS.CarrierTariffMatrix)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMatrixes
                    Where d.CarrTarMatControl = source.CarrTarMatControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMatControl _
                        , .ModDate = d.CarrTarMatModDate _
                        , .ModUser = d.CarrTarMatModUser _
                        , .Updated = d.CarrTarMatUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oData, DataTransferObjects.CarrierTariffRow)

            If .CarrTarMatCarrTarControl = 0 Then
                Utilities.SaveAppError("Cannot save new Carrier Tariff Matrix data. A Tariff Control Number is required.  Please create your Carrier Tariff Header information first.", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If
            'Check if the data code range is already in use
            Dim strMsg As String = ""
            If isZipRangeInUse(.CarrTarMatCarrTarControl, .ZipFrom, .ZipTo, strMsg) Then
                Utilities.SaveAppError("Cannot save new Carrier Tariff Matrix data.  " & strMsg, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If

        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oData, DataTransferObjects.CarrierTariffRow)

            If .CarrTarMatCarrTarControl = 0 Then
                Utilities.SaveAppError("Cannot save new Carrier Tariff Matrix data. A Tariff Control Number is required.  Please create your Carrier Tariff Header information first.", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If
            'Check if the data code range is already in use
            Dim strMsg As String = ""
            If isZipRangeInUse(.CarrTarMatCarrTarControl, .ZipFrom, .ZipTo, strMsg) Then
                Utilities.SaveAppError("Cannot save new Carrier Tariff Matrix data.  " & strMsg, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
            End If

        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        Dim oRow As DataTransferObjects.CarrierTariffRow = TryCast(oData, DataTransferObjects.CarrierTariffRow)
        If oRow Is Nothing Then Return 'bad data cannot add details
        With CType(LinqTable, LTS.CarrierTariffMatrix)
            Dim oDetails As New List(Of LTS.CarrierTariffMatrixDetail)
            'update the values
            For intID As Integer = 1 To 11
                Dim oDetail As New LTS.CarrierTariffMatrixDetail
                With oDetail
                    .CarrTarMatDetCarrTarMatControl = oRow.CarrTarMatControl
                    .CarrTarMatDetModDate = Date.Now
                    .CarrTarMatDetModUser = Parameters.UserName
                    .CarrTarMatDetUpdated = New Byte() {}
                    Select Case intID
                        Case 1
                            .CarrTarMatDetID = 1
                            .CarrTarMatDetValue = oRow.Break1
                        Case 2
                            .CarrTarMatDetID = 2
                            .CarrTarMatDetValue = oRow.Break2
                        Case 3
                            .CarrTarMatDetID = 3
                            .CarrTarMatDetValue = oRow.Break3
                        Case 4
                            .CarrTarMatDetID = 4
                            .CarrTarMatDetValue = oRow.Break4
                        Case 5
                            .CarrTarMatDetID = 5
                            .CarrTarMatDetValue = oRow.Break5
                        Case 6
                            .CarrTarMatDetID = 6
                            .CarrTarMatDetValue = oRow.Break6
                        Case 7
                            .CarrTarMatDetID = 7
                            .CarrTarMatDetValue = oRow.Break7
                        Case 8
                            .CarrTarMatDetID = 8
                            oDetail.CarrTarMatDetValue = oRow.Break8
                        Case 9
                            .CarrTarMatDetID = 9
                            .CarrTarMatDetValue = oRow.Break9
                        Case 10
                            .CarrTarMatDetID = 10
                            .CarrTarMatDetValue = oRow.Break10
                        Case 11
                            .CarrTarMatDetID = 11
                            .CarrTarMatDetValue = oRow.Break11
                    End Select
                End With
                oDetails.Add(oDetail)
            Next
            'Add the matrix Detail Records to the matrix
            .CarrierTariffMatrixDetails.AddRange(oDetails)
        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierTariffMatrixDetails.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierTariffMatrix).CarrierTariffMatrixDetails)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any updated contact records  inserts or deletes are automatic and handled elsewhere
            .CarrierTariffMatrixDetails.AttachAll(GetCarrierTariffMatrixDetailChanges(oData), True)
        End With
    End Sub

    Protected Function GetCarrierTariffMatrixDetailChanges(ByVal source As DataTransferObjects.CarrierTariffRow) As List(Of LTS.CarrierTariffMatrixDetail)

        If source Is Nothing Then Return New List(Of LTS.CarrierTariffMatrixDetail)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Select the Detail records                    
                Dim intCarrTarMatDetControls As Integer() = {source.DetControl1, source.DetControl2, source.DetControl3, source.DetControl4, source.DetControl5, source.DetControl6, source.DetControl7, source.DetControl8, source.DetControl9, source.DetControl10, source.DetControl11}

                Dim oDetails As List(Of LTS.CarrierTariffMatrixDetail) = (From d In db.CarrierTariffMatrixDetails
                        Where intCarrTarMatDetControls.Contains(d.CarrTarMatDetControl)
                        Select New LTS.CarrierTariffMatrixDetail _
                        With {.CarrTarMatDetControl = d.CarrTarMatDetControl,
                        .CarrTarMatDetCarrTarMatControl = d.CarrTarMatDetCarrTarMatControl,
                        .CarrTarMatDetID = d.CarrTarMatDetID,
                        .CarrTarMatDetUpdated = d.CarrTarMatDetUpdated,
                        .CarrTarMatDetModDate = Date.Now,
                        .CarrTarMatDetModUser = Parameters.UserName}).ToList

                If oDetails Is Nothing OrElse oDetails.Count < 11 Then
                    addMissingCarrierTariffMatrixDetails(oDetails, source)
                Else
                    'just update the values
                    For Each oDetail As LTS.CarrierTariffMatrixDetail In oDetails
                        Select Case oDetail.CarrTarMatDetID
                            Case 1
                                oDetail.CarrTarMatDetValue = source.Break1
                            Case 2
                                oDetail.CarrTarMatDetValue = source.Break2
                            Case 3
                                oDetail.CarrTarMatDetValue = source.Break3
                            Case 4
                                oDetail.CarrTarMatDetValue = source.Break4
                            Case 5
                                oDetail.CarrTarMatDetValue = source.Break5
                            Case 6
                                oDetail.CarrTarMatDetValue = source.Break6
                            Case 7
                                oDetail.CarrTarMatDetValue = source.Break7
                            Case 8
                                oDetail.CarrTarMatDetValue = source.Break8
                            Case 9
                                oDetail.CarrTarMatDetValue = source.Break9
                            Case 10
                                oDetail.CarrTarMatDetValue = source.Break10
                            Case 11
                                oDetail.CarrTarMatDetValue = source.Break11
                        End Select
                    Next
                End If

                Return oDetails
            Catch ex As FaultException
                Throw
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using

    End Function

    Protected Sub addMissingCarrierTariffMatrixDetails(ByRef oDetails As List(Of LTS.CarrierTariffMatrixDetail), ByVal parent As DataTransferObjects.CarrierTariffRow)

        Dim oCarrTarMatDetData As New NGLCarrierTariffMatrixDetailData(Me.Parameters)

        If oDetails Is Nothing Then oDetails = New List(Of LTS.CarrierTariffMatrixDetail)
        If oDetails.Count = 0 Then
            'all rows are missing so we need to create 11 new rows            
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 1, parent.Break1))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 2, parent.Break2))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 3, parent.Break3))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 4, parent.Break4))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 5, parent.Break5))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 6, parent.Break6))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 7, parent.Break7))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 8, parent.Break8))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 9, parent.Break9))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 10, parent.Break10))
            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 11, parent.Break11))
        Else
            'we need to add any missing details
            For intID As Integer = 1 To 11
                Dim blnFound As Boolean = False
                Dim localID As Integer = intID
                Try
                    Dim oData = From d In oDetails Where d.CarrTarMatDetID = localID Select d
                    If Not oData Is Nothing Then blnFound = True
                Catch ex As Exception
                    'do nothing
                End Try
                If Not blnFound Then
                    Select Case intID
                        Case 1
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 1, parent.Break1))
                        Case 2
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 2, parent.Break2))
                        Case 3
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 3, parent.Break3))
                        Case 4
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 4, parent.Break4))
                        Case 5
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 5, parent.Break5))
                        Case 6
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 6, parent.Break6))
                        Case 7
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 7, parent.Break7))
                        Case 8
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 8, parent.Break8))
                        Case 9
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 9, parent.Break9))
                        Case 10
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 10, parent.Break10))
                        Case 11
                            oDetails.Add(oCarrTarMatDetData.AddNew(parent.CarrTarMatControl, 11, parent.Break11))
                    End Select
                End If
            Next
        End If

    End Sub

#End Region

End Class