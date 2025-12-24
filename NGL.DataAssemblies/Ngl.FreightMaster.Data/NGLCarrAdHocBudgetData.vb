Imports System.ServiceModel

Public Class NGLCarrAdHocBudgetData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrAdHocBudgets
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrAdHocBudgetData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrAdHocBudgets
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
        Return GetCarrAdHocBudgetFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrAdHocBudgetsFiltered()
    End Function

    Public Function GetCarrAdHocBudgetFiltered(ByVal Control As Integer) As DataTransferObjects.CarrAdHocBudget
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrAdHocBudget As DataTransferObjects.CarrAdHocBudget = (
                        From d In db.CarrAdHocBudgets
                        Where
                        d.CarrAdHocBudControl = Control
                        Select New DataTransferObjects.CarrAdHocBudget With {.CarrAdHocBudControl = d.CarrAdHocBudControl,
                        .CarrAdHocBudCarrAdHocControl = d.CarrAdHocBudCarrAdHocControl,
                        .CarrAdHocBudModDate = d.CarrAdHocBudModDate,
                        .CarrAdHocBudModUser = d.CarrAdHocBudModUser,
                        .CarrAdHocBudExpMo1 = If(d.CarrAdHocBudExpMo1.HasValue, d.CarrAdHocBudExpMo1.Value, 0),
                        .CarrAdHocBudExpMo2 = If(d.CarrAdHocBudExpMo2.HasValue, d.CarrAdHocBudExpMo2.Value, 0),
                        .CarrAdHocBudExpMo3 = If(d.CarrAdHocBudExpMo3.HasValue, d.CarrAdHocBudExpMo3.Value, 0),
                        .CarrAdHocBudExpMo4 = If(d.CarrAdHocBudExpMo4.HasValue, d.CarrAdHocBudExpMo4.Value, 0),
                        .CarrAdHocBudExpMo5 = If(d.CarrAdHocBudExpMo5.HasValue, d.CarrAdHocBudExpMo5.Value, 0),
                        .CarrAdHocBudExpMo6 = If(d.CarrAdHocBudExpMo6.HasValue, d.CarrAdHocBudExpMo6.Value, 0),
                        .CarrAdHocBudExpMo7 = If(d.CarrAdHocBudExpMo7.HasValue, d.CarrAdHocBudExpMo7.Value, 0),
                        .CarrAdHocBudExpMo8 = If(d.CarrAdHocBudExpMo8.HasValue, d.CarrAdHocBudExpMo8.Value, 0),
                        .CarrAdHocBudExpMo9 = If(d.CarrAdHocBudExpMo9.HasValue, d.CarrAdHocBudExpMo9.Value, 0),
                        .CarrAdHocBudExpMo10 = If(d.CarrAdHocBudExpMo10.HasValue, d.CarrAdHocBudExpMo10.Value, 0),
                        .CarrAdHocBudExpMo11 = If(d.CarrAdHocBudExpMo11.HasValue, d.CarrAdHocBudExpMo11.Value, 0),
                        .CarrAdHocBudExpMo12 = If(d.CarrAdHocBudExpMo12.HasValue, d.CarrAdHocBudExpMo12.Value, 0),
                        .CarrAdHocBudExpTotal = If(d.CarrAdHocBudExpTotal.HasValue, d.CarrAdHocBudExpTotal.Value, 0),
                        .CarrAdHocBudActMo1 = If(d.CarrAdHocBudActMo1.HasValue, d.CarrAdHocBudActMo1.Value, 0),
                        .CarrAdHocBudActMo2 = If(d.CarrAdHocBudActMo2.HasValue, d.CarrAdHocBudActMo2.Value, 0),
                        .CarrAdHocBudActMo3 = If(d.CarrAdHocBudActMo3.HasValue, d.CarrAdHocBudActMo3.Value, 0),
                        .CarrAdHocBudActMo4 = If(d.CarrAdHocBudActMo4.HasValue, d.CarrAdHocBudActMo4.Value, 0),
                        .CarrAdHocBudActMo5 = If(d.CarrAdHocBudActMo5.HasValue, d.CarrAdHocBudActMo5.Value, 0),
                        .CarrAdHocBudActMo6 = If(d.CarrAdHocBudActMo6.HasValue, d.CarrAdHocBudActMo6.Value, 0),
                        .CarrAdHocBudActMo7 = If(d.CarrAdHocBudActMo7.HasValue, d.CarrAdHocBudActMo7.Value, 0),
                        .CarrAdHocBudActMo8 = If(d.CarrAdHocBudActMo8.HasValue, d.CarrAdHocBudActMo8.Value, 0),
                        .CarrAdHocBudActMo9 = If(d.CarrAdHocBudActMo9.HasValue, d.CarrAdHocBudActMo9.Value, 0),
                        .CarrAdHocBudActMo10 = If(d.CarrAdHocBudActMo10.HasValue, d.CarrAdHocBudActMo10.Value, 0),
                        .CarrAdHocBudActMo11 = If(d.CarrAdHocBudActMo11.HasValue, d.CarrAdHocBudActMo11.Value, 0),
                        .CarrAdHocBudActMo12 = If(d.CarrAdHocBudActMo12.HasValue, d.CarrAdHocBudActMo12.Value, 0),
                        .CarrAdHocBudActTotal = If(d.CarrAdHocBudActTotal.HasValue, d.CarrAdHocBudActTotal.Value, 0),
                        .CarrAdHocBudgetUpdated = d.CarrAdHocBudgetUpdated.ToArray()}).First


                Return CarrAdHocBudget

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

    Public Function GetCarrAdHocBudgetsFiltered(Optional ByVal CarrAdHocControl As Integer = 0) As DataTransferObjects.CarrAdHocBudget()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrAdHocBudgets() As DataTransferObjects.CarrAdHocBudget = (
                        From d In db.CarrAdHocBudgets
                        Where
                        (d.CarrAdHocBudCarrAdHocControl = If(CarrAdHocControl = 0, d.CarrAdHocBudCarrAdHocControl, CarrAdHocControl))
                        Order By d.CarrAdHocBudControl
                        Select New DataTransferObjects.CarrAdHocBudget With {.CarrAdHocBudControl = d.CarrAdHocBudControl,
                        .CarrAdHocBudCarrAdHocControl = d.CarrAdHocBudCarrAdHocControl,
                        .CarrAdHocBudModDate = d.CarrAdHocBudModDate,
                        .CarrAdHocBudModUser = d.CarrAdHocBudModUser,
                        .CarrAdHocBudExpMo1 = If(d.CarrAdHocBudExpMo1.HasValue, d.CarrAdHocBudExpMo1.Value, 0),
                        .CarrAdHocBudExpMo2 = If(d.CarrAdHocBudExpMo2.HasValue, d.CarrAdHocBudExpMo2.Value, 0),
                        .CarrAdHocBudExpMo3 = If(d.CarrAdHocBudExpMo3.HasValue, d.CarrAdHocBudExpMo3.Value, 0),
                        .CarrAdHocBudExpMo4 = If(d.CarrAdHocBudExpMo4.HasValue, d.CarrAdHocBudExpMo4.Value, 0),
                        .CarrAdHocBudExpMo5 = If(d.CarrAdHocBudExpMo5.HasValue, d.CarrAdHocBudExpMo5.Value, 0),
                        .CarrAdHocBudExpMo6 = If(d.CarrAdHocBudExpMo6.HasValue, d.CarrAdHocBudExpMo6.Value, 0),
                        .CarrAdHocBudExpMo7 = If(d.CarrAdHocBudExpMo7.HasValue, d.CarrAdHocBudExpMo7.Value, 0),
                        .CarrAdHocBudExpMo8 = If(d.CarrAdHocBudExpMo8.HasValue, d.CarrAdHocBudExpMo8.Value, 0),
                        .CarrAdHocBudExpMo9 = If(d.CarrAdHocBudExpMo9.HasValue, d.CarrAdHocBudExpMo9.Value, 0),
                        .CarrAdHocBudExpMo10 = If(d.CarrAdHocBudExpMo10.HasValue, d.CarrAdHocBudExpMo10.Value, 0),
                        .CarrAdHocBudExpMo11 = If(d.CarrAdHocBudExpMo11.HasValue, d.CarrAdHocBudExpMo11.Value, 0),
                        .CarrAdHocBudExpMo12 = If(d.CarrAdHocBudExpMo12.HasValue, d.CarrAdHocBudExpMo12.Value, 0),
                        .CarrAdHocBudExpTotal = If(d.CarrAdHocBudExpTotal.HasValue, d.CarrAdHocBudExpTotal.Value, 0),
                        .CarrAdHocBudActMo1 = If(d.CarrAdHocBudActMo1.HasValue, d.CarrAdHocBudActMo1.Value, 0),
                        .CarrAdHocBudActMo2 = If(d.CarrAdHocBudActMo2.HasValue, d.CarrAdHocBudActMo2.Value, 0),
                        .CarrAdHocBudActMo3 = If(d.CarrAdHocBudActMo3.HasValue, d.CarrAdHocBudActMo3.Value, 0),
                        .CarrAdHocBudActMo4 = If(d.CarrAdHocBudActMo4.HasValue, d.CarrAdHocBudActMo4.Value, 0),
                        .CarrAdHocBudActMo5 = If(d.CarrAdHocBudActMo5.HasValue, d.CarrAdHocBudActMo5.Value, 0),
                        .CarrAdHocBudActMo6 = If(d.CarrAdHocBudActMo6.HasValue, d.CarrAdHocBudActMo6.Value, 0),
                        .CarrAdHocBudActMo7 = If(d.CarrAdHocBudActMo7.HasValue, d.CarrAdHocBudActMo7.Value, 0),
                        .CarrAdHocBudActMo8 = If(d.CarrAdHocBudActMo8.HasValue, d.CarrAdHocBudActMo8.Value, 0),
                        .CarrAdHocBudActMo9 = If(d.CarrAdHocBudActMo9.HasValue, d.CarrAdHocBudActMo9.Value, 0),
                        .CarrAdHocBudActMo10 = If(d.CarrAdHocBudActMo10.HasValue, d.CarrAdHocBudActMo10.Value, 0),
                        .CarrAdHocBudActMo11 = If(d.CarrAdHocBudActMo11.HasValue, d.CarrAdHocBudActMo11.Value, 0),
                        .CarrAdHocBudActMo12 = If(d.CarrAdHocBudActMo12.HasValue, d.CarrAdHocBudActMo12.Value, 0),
                        .CarrAdHocBudActTotal = If(d.CarrAdHocBudActTotal.HasValue, d.CarrAdHocBudActTotal.Value, 0),
                        .CarrAdHocBudgetUpdated = d.CarrAdHocBudgetUpdated.ToArray()}).ToArray()
                Return CarrAdHocBudgets

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
        Dim d = CType(oData, DataTransferObjects.CarrAdHocBudget)
        'Create New Record
        Return New LTS.CarrAdHocBudget With {.CarrAdHocBudControl = d.CarrAdHocBudControl,
            .CarrAdHocBudCarrAdHocControl = d.CarrAdHocBudCarrAdHocControl,
            .CarrAdHocBudModDate = Date.Now,
            .CarrAdHocBudModUser = Parameters.UserName,
            .CarrAdHocBudExpMo1 = d.CarrAdHocBudExpMo1,
            .CarrAdHocBudExpMo2 = d.CarrAdHocBudExpMo2,
            .CarrAdHocBudExpMo3 = d.CarrAdHocBudExpMo3,
            .CarrAdHocBudExpMo4 = d.CarrAdHocBudExpMo4,
            .CarrAdHocBudExpMo5 = d.CarrAdHocBudExpMo5,
            .CarrAdHocBudExpMo6 = d.CarrAdHocBudExpMo6,
            .CarrAdHocBudExpMo7 = d.CarrAdHocBudExpMo7,
            .CarrAdHocBudExpMo8 = d.CarrAdHocBudExpMo8,
            .CarrAdHocBudExpMo9 = d.CarrAdHocBudExpMo9,
            .CarrAdHocBudExpMo10 = d.CarrAdHocBudExpMo10,
            .CarrAdHocBudExpMo11 = d.CarrAdHocBudExpMo11,
            .CarrAdHocBudExpMo12 = d.CarrAdHocBudExpMo12,
            .CarrAdHocBudExpTotal = d.CarrAdHocBudExpTotal,
            .CarrAdHocBudActMo1 = d.CarrAdHocBudActMo1,
            .CarrAdHocBudActMo2 = d.CarrAdHocBudActMo2,
            .CarrAdHocBudActMo3 = d.CarrAdHocBudActMo3,
            .CarrAdHocBudActMo4 = d.CarrAdHocBudActMo4,
            .CarrAdHocBudActMo5 = d.CarrAdHocBudActMo5,
            .CarrAdHocBudActMo6 = d.CarrAdHocBudActMo6,
            .CarrAdHocBudActMo7 = d.CarrAdHocBudActMo7,
            .CarrAdHocBudActMo8 = d.CarrAdHocBudActMo8,
            .CarrAdHocBudActMo9 = d.CarrAdHocBudActMo9,
            .CarrAdHocBudActMo10 = d.CarrAdHocBudActMo10,
            .CarrAdHocBudActMo11 = d.CarrAdHocBudActMo11,
            .CarrAdHocBudActMo12 = d.CarrAdHocBudActMo12,
            .CarrAdHocBudActTotal = d.CarrAdHocBudActTotal,
            .CarrAdHocBudgetUpdated = If(d.CarrAdHocBudgetUpdated Is Nothing, New Byte() {}, d.CarrAdHocBudgetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrAdHocBudgetFiltered(Control:=CType(LinqTable, LTS.CarrAdHocBudget).CarrAdHocBudControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrAdHocBudget = TryCast(LinqTable, LTS.CarrAdHocBudget)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrAdHocBudgets
                    Where d.CarrAdHocBudControl = source.CarrAdHocBudControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrAdHocBudControl _
                        , .ModDate = d.CarrAdHocBudModDate _
                        , .ModUser = d.CarrAdHocBudModUser _
                        , .Updated = d.CarrAdHocBudgetUpdated.ToArray}).First

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