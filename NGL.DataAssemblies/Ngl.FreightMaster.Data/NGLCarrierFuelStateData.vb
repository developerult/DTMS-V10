Imports System.ServiceModel

Public Class NGLCarrierFuelStateData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierFuelStates
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierFuelStateData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierFuelStates
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
        Return GetCarrierFuelStateFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierFuelStatesFiltered()
    End Function

    Public Function GetCarrierFuelStateFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierFuelState
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierFuelState As DataTransferObjects.CarrierFuelState = (
                        From t In db.CarrierFuelStates
                        Where
                        (t.CarrierFuelStateControl = Control)
                        Select New DataTransferObjects.CarrierFuelState With {.CarrierFuelStateControl = t.CarrierFuelStateControl _
                        , .CarrierFuelStateFuelControl = t.CarrierFuelStateFuelControl _
                        , .CarrierFuelStatePercentage = If(t.CarrierFuelStatePercentage.HasValue, t.CarrierFuelStatePercentage.Value, 0) _
                        , .CarrierFuelStateEffective = t.CarrierFuelStateEffective _
                        , .CarrierFuelStateUpdated = t.CarrierFuelStateUpdated.ToArray()}).Single
                Return CarrierFuelState

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

    Public Function GetCarrierFuelStatesFiltered(Optional ByVal CarrierFuelControl As Integer = 0) As DataTransferObjects.CarrierFuelState()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierFuelStates() As DataTransferObjects.CarrierFuelState = (
                        From t In db.CarrierFuelStates
                        Where
                        t.CarrierFuelStateFuelControl = If(CarrierFuelControl = 0, t.CarrierFuelStateFuelControl, CarrierFuelControl)
                        Order By t.CarrierFuelStateEffective
                        Select New DataTransferObjects.CarrierFuelState With {.CarrierFuelStateControl = t.CarrierFuelStateControl _
                        , .CarrierFuelStateFuelControl = t.CarrierFuelStateFuelControl _
                        , .CarrierFuelStatePercentage = If(t.CarrierFuelStatePercentage.HasValue, t.CarrierFuelStatePercentage.Value, 0) _
                        , .CarrierFuelStateEffective = t.CarrierFuelStateEffective _
                        , .CarrierFuelStateUpdated = t.CarrierFuelStateUpdated.ToArray()}).ToArray()
                Return CarrierFuelStates

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
        Dim d = CType(oData, DataTransferObjects.CarrierFuelState)
        'Create New Record
        Return New LTS.CarrierFuelState With {.CarrierFuelStateControl = d.CarrierFuelStateControl _
            , .CarrierFuelStateFuelControl = d.CarrierFuelStateFuelControl _
            , .CarrierFuelStatePercentage = d.CarrierFuelStatePercentage _
            , .CarrierFuelStateEffective = d.CarrierFuelStateEffective _
            , .CarrierFuelStateUpdated = If(d.CarrierFuelStateUpdated Is Nothing, New Byte() {}, d.CarrierFuelStateUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFuelStateFiltered(Control:=CType(LinqTable, LTS.CarrierFuelState).CarrierFuelStateControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierFuelState = TryCast(LinqTable, LTS.CarrierFuelState)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CarrierFuelStates
                    Where d.CarrierFuelStateControl = source.CarrierFuelStateControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierFuelStateControl _
                        , .ModDate = Date.Now _
                        , .ModUser = Parameters.UserName _
                        , .Updated = d.CarrierFuelStateUpdated.ToArray}).First

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