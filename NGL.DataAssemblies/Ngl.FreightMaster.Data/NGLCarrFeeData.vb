Imports System.ServiceModel

Public Class NGLCarrFeeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrFees
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrFeeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrFees
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
        Return GetCarrFeeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrFeesFiltered()
    End Function

    Public Function GetCarrFeeFiltered(ByVal Control As Integer) As DataTransferObjects.CarrFee
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrFee As DataTransferObjects.CarrFee = (
                        From d In db.CarrFees
                        Where
                        d.CarrFeesControl = Control
                        Select New DataTransferObjects.CarrFee With {.CarrFeesControl = d.CarrFeesControl _
                        , .CarrFeesCarrierControl = If(d.CarrFeesCarrierControl.HasValue, d.CarrFeesCarrierControl.Value, 0) _
                        , .CarrFeesMinimum = If(d.CarrFeesMinimum.HasValue, d.CarrFeesMinimum.Value, 0) _
                        , .CarrFeesVariable = If(d.CarrFeesVariable.HasValue, d.CarrFeesVariable.Value, 0) _
                        , .CarrFeesAccessorialCode = If(d.CarrFeesAccessorialCode.HasValue, d.CarrFeesAccessorialCode.Value, 0) _
                        , .CarrFeesModDate = d.CarrFeesModDate _
                        , .CarrFeesModUser = d.CarrFeesModUser _
                        , .CarrFeesUpdated = d.CarrFeesUpdated.ToArray()}).First


                Return CarrFee

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

    Public Function GetCarrFeesFiltered(Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.CarrFee()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrFees() As DataTransferObjects.CarrFee = (
                        From d In db.CarrFees
                        Where
                        (d.CarrFeesCarrierControl = CarrierControl)
                        Order By d.CarrFeesControl
                        Select New DataTransferObjects.CarrFee With {.CarrFeesControl = d.CarrFeesControl _
                        , .CarrFeesCarrierControl = If(d.CarrFeesCarrierControl.HasValue, d.CarrFeesCarrierControl.Value, 0) _
                        , .CarrFeesMinimum = If(d.CarrFeesMinimum.HasValue, d.CarrFeesMinimum.Value, 0) _
                        , .CarrFeesVariable = If(d.CarrFeesVariable.HasValue, d.CarrFeesVariable.Value, 0) _
                        , .CarrFeesAccessorialCode = If(d.CarrFeesAccessorialCode.HasValue, d.CarrFeesAccessorialCode.Value, 0) _
                        , .CarrFeesModDate = d.CarrFeesModDate _
                        , .CarrFeesModUser = d.CarrFeesModUser _
                        , .CarrFeesUpdated = d.CarrFeesUpdated.ToArray()}).ToArray()
                Return CarrFees

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
        Dim d = CType(oData, DataTransferObjects.CarrFee)
        'Create New Record
        Return New LTS.CarrFee With {.CarrFeesControl = d.CarrFeesControl _
            , .CarrFeesCarrierControl = d.CarrFeesCarrierControl _
            , .CarrFeesMinimum = d.CarrFeesMinimum _
            , .CarrFeesVariable = d.CarrFeesVariable _
            , .CarrFeesAccessorialCode = d.CarrFeesAccessorialCode _
            , .CarrFeesModDate = Date.Now _
            , .CarrFeesModUser = Parameters.UserName _
            , .CarrFeesUpdated = If(d.CarrFeesUpdated Is Nothing, New Byte() {}, d.CarrFeesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrFeeFiltered(Control:=CType(LinqTable, LTS.CarrFee).CarrFeesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrFee = TryCast(LinqTable, LTS.CarrFee)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrFees
                    Where d.CarrFeesControl = source.CarrFeesControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrFeesControl _
                        , .ModDate = d.CarrFeesModDate _
                        , .ModUser = d.CarrFeesModUser _
                        , .Updated = d.CarrFeesUpdated.ToArray}).First

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

#End Region

End Class