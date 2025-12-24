Imports System.ServiceModel

Public Class NGLCarrTarMatBPDetData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffMatrixBPDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarMatBPDetData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMatrixBPDetails
                _LinqDB = db
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
        Return GetCarrTarMatBPDetFiltered(Control)
    End Function

    ''' <summary>
    ''' This method is not allowed because a parameter is required
    ''' We may change the logic for the GetRecordsFiltered method to use 
    ''' a generic criteria object as the parameter in the future
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarMatBPDetFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarMatBPDet
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrTarMatBPDet As DataTransferObjects.CarrTarMatBPDet = (
                        From d In db.CarrierTariffMatrixBPDetails
                        Where
                        d.CarrTarMatBPDetControl = Control
                        Select selectDTOData(d, db)).First


                Return CarrTarMatBPDet

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
    ''' This method does not use paging because only 10 records are ever returned
    ''' </summary>
    ''' <param name="CarrTarMatBPDetCarrTarMatBPControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarMatBPDetsFiltered(ByVal CarrTarMatBPDetCarrTarMatBPControl As Integer) As DataTransferObjects.CarrTarMatBPDet()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the details that match the CarrTarMatBPDetID sorted by ID
                Dim CarrierTariffMatrixBPDetails() As DataTransferObjects.CarrTarMatBPDet = (
                        From d In db.CarrierTariffMatrixBPDetails
                        Where
                        (d.CarrTarMatBPDetCarrTarMatBPControl = CarrTarMatBPDetCarrTarMatBPControl)
                        Order By d.CarrTarMatBPDetID
                        Select selectDTOData(d, db)).ToArray()

                Return CarrierTariffMatrixBPDetails

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarMatBPDet)
        'Create New Record
        Return New LTS.CarrierTariffMatrixBPDetail With {.CarrTarMatBPDetControl = d.CarrTarMatBPDetControl _
            , .CarrTarMatBPDetCarrTarMatBPControl = d.CarrTarMatBPDetCarrTarMatBPControl _
            , .CarrTarMatBPDetName = d.CarrTarMatBPDetName _
            , .CarrTarMatBPDetDesc = d.CarrTarMatBPDetDesc _
            , .CarrTarMatBPDetID = d.CarrTarMatBPDetID _
            , .CarrTarMatBPDetValue = d.CarrTarMatBPDetValue _
            , .CarrTarMatBPDetModDate = d.CarrTarMatBPDetModDate _
            , .CarrTarMatBPDetModUser = d.CarrTarMatBPDetModUser _
            , .CarrTarMatBPDetUpdated = If(d.CarrTarMatBPDetUpdated Is Nothing, New Byte() {}, d.CarrTarMatBPDetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarMatBPDetFiltered(Control:=DirectCast(LinqTable, LTS.CarrierTariffMatrixBPDetail).CarrTarMatBPDetControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffMatrixBPDetail = TryCast(LinqTable, LTS.CarrierTariffMatrixBPDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMatrixBPDetails
                    Where d.CarrTarMatBPDetControl = source.CarrTarMatBPDetControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMatBPDetControl _
                        , .ModDate = d.CarrTarMatBPDetModDate _
                        , .ModUser = d.CarrTarMatBPDetModUser _
                        , .Updated = d.CarrTarMatBPDetUpdated.ToArray}).First

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
        'Check if the data already exists only one allowed
        With DirectCast(oData, DataTransferObjects.CarrTarMatBPDet)
            Try
                Dim CarrierTariffEquipMatrixDet As DataTransferObjects.CarrTarMatBPDet = (
                        From t In DirectCast(oDB, NGLMASCarrierDataContext).CarrierTariffMatrixBPDetails
                        Where
                        (t.CarrTarMatBPDetCarrTarMatBPControl = .CarrTarMatBPDetCarrTarMatBPControl _
                         And
                         t.CarrTarMatBPDetID = .CarrTarMatBPDetID)
                        Select New DataTransferObjects.CarrTarMatBPDet With {.CarrTarMatBPDetControl = t.CarrTarMatBPDetControl}).First

                If Not CarrierTariffEquipMatrixDet Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Carrier Matrix Break Point Detail data.  The Break Point Column, " & .CarrTarMatBPDetID & ",  is already assigned for the selected break point record.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With DirectCast(oData, DataTransferObjects.CarrTarMatBPDet)
            Try
                'Get the newest record that matches the provided criteria
                Dim oRecord As DataTransferObjects.CarrTarMatBPDet = (
                        From t In DirectCast(oDB, NGLMASCarrierDataContext).CarrierTariffMatrixBPDetails
                        Where
                        (t.CarrTarMatBPDetControl <> .CarrTarMatBPDetControl) _
                        And
                        (t.CarrTarMatBPDetCarrTarMatBPControl = .CarrTarMatBPDetCarrTarMatBPControl _
                         And
                         t.CarrTarMatBPDetID = .CarrTarMatBPDetID)
                        Select New DataTransferObjects.CarrTarMatBPDet With {.CarrTarMatBPDetControl = t.CarrTarMatBPDetControl}).First

                If Not oRecord Is Nothing Then
                    Utilities.SaveAppError("Cannot save Carrier Matrix Break Point Detail changes.  The Break Point Column, " & .CarrTarMatBPDetID & ",  is already assigned to the selected matrix break point record.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffMatrixBPDetail, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarMatBPDet
        Return New DataTransferObjects.CarrTarMatBPDet With {.CarrTarMatBPDetControl = d.CarrTarMatBPDetControl _
            , .CarrTarMatBPDetCarrTarMatBPControl = d.CarrTarMatBPDetCarrTarMatBPControl _
            , .CarrTarMatBPDetName = d.CarrTarMatBPDetName _
            , .CarrTarMatBPDetDesc = d.CarrTarMatBPDetDesc _
            , .CarrTarMatBPDetID = d.CarrTarMatBPDetID _
            , .CarrTarMatBPDetValue = d.CarrTarMatBPDetValue _
            , .CarrTarMatBPDetModDate = d.CarrTarMatBPDetModDate _
            , .CarrTarMatBPDetModUser = d.CarrTarMatBPDetModUser _
            , .CarrTarMatBPDetUpdated = d.CarrTarMatBPDetUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

#End Region

End Class