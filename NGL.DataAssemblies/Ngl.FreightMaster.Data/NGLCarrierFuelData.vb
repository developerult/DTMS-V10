Imports System.Data.Linq
Imports System.ServiceModel
Imports NGL.Core.ChangeTracker

Public Class NGLCarrierFuelData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierFuels
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierFuelData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierFuels
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
        Return GetCarrierFuelFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierFuelsFiltered()
    End Function

    Public Function GetCarrierFuelFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.CarrierFuel
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierFuel)(Function(t As LTS.CarrierFuel) t.CarrierFuelStates)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim CarrierFuel As DataTransferObjects.CarrierFuel = (
                        From t In db.CarrierFuels
                        Where
                        (t.CarrierFuelControl = If(Control = 0, t.CarrierFuelControl, Control))
                        Select New DataTransferObjects.CarrierFuel With {.CarrierFuelControl = t.CarrierFuelControl _
                        , .CarrierFuelCarrierControl = t.CarrierFuelCarrierControl _
                        , .CarrierFuelState = t.CarrierFuelState _
                        , .CarrierFuelStatePercent = If(t.CarrierFuelStatePercent.HasValue, t.CarrierFuelStatePercent.Value, 0) _
                        , .CarrierFuelEffectiveDate = t.CarrierFuelEffectiveDate _
                        , .CarrierFuelUpdated = t.CarrierFuelUpdated.ToArray() _
                        , .CarrierFuelStates = (
                        From d In t.CarrierFuelStates
                        Select New DataTransferObjects.CarrierFuelState With {.CarrierFuelStateControl = d.CarrierFuelStateControl _
                        , .CarrierFuelStateFuelControl = d.CarrierFuelStateFuelControl _
                        , .CarrierFuelStatePercentage = If(d.CarrierFuelStatePercentage.HasValue, d.CarrierFuelStatePercentage.Value, 0) _
                        , .CarrierFuelStateEffective = d.CarrierFuelStateEffective _
                        , .CarrierFuelStateUpdated = d.CarrierFuelStateUpdated.ToArray()}).ToList()}).First

                Return CarrierFuel

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

    Public Function GetCarrierFuelsFiltered(Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.CarrierFuel()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierFuels() As DataTransferObjects.CarrierFuel = (
                        From t In db.CarrierFuels
                        Where
                        (t.CarrierFuelCarrierControl = If(CarrierControl = 0, t.CarrierFuelCarrierControl, CarrierControl))
                        Order By t.CarrierFuelControl
                        Select New DataTransferObjects.CarrierFuel With {.CarrierFuelControl = t.CarrierFuelControl _
                        , .CarrierFuelCarrierControl = t.CarrierFuelCarrierControl _
                        , .CarrierFuelState = t.CarrierFuelState _
                        , .CarrierFuelStatePercent = If(t.CarrierFuelStatePercent.HasValue, t.CarrierFuelStatePercent.Value, 0) _
                        , .CarrierFuelEffectiveDate = t.CarrierFuelEffectiveDate _
                        , .CarrierFuelUpdated = t.CarrierFuelUpdated.ToArray()}).ToArray()
                Return CarrierFuels

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
        Dim d = CType(oData, DataTransferObjects.CarrierFuel)
        'Create New Record
        Return New LTS.CarrierFuel With {.CarrierFuelControl = d.CarrierFuelControl _
            , .CarrierFuelCarrierControl = d.CarrierFuelCarrierControl _
            , .CarrierFuelState = d.CarrierFuelState _
            , .CarrierFuelStatePercent = d.CarrierFuelStatePercent _
            , .CarrierFuelEffectiveDate = d.CarrierFuelEffectiveDate _
            , .CarrierFuelUpdated = If(d.CarrierFuelUpdated Is Nothing, New Byte() {}, d.CarrierFuelUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFuelFiltered(Control:=CType(LinqTable, LTS.CarrierFuel).CarrierFuelControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierFuel = TryCast(LinqTable, LTS.CarrierFuel)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CarrierFuels
                    Where d.CarrierFuelControl = source.CarrierFuelControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierFuelControl _
                        , .ModDate = Date.Now _
                        , .ModUser = Parameters.UserName _
                        , .Updated = d.CarrierFuelUpdated.ToArray}).First

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

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.CarrierFuel)
            'Add CarrierFuel contact Records
            .CarrierFuelStates.AddRange(
                From d In CType(oData, DataTransferObjects.CarrierFuel).CarrierFuelStates
                                           Select New LTS.CarrierFuelState With {.CarrierFuelStateControl = d.CarrierFuelStateControl _
                                           , .CarrierFuelStateFuelControl = d.CarrierFuelStateFuelControl _
                                           , .CarrierFuelStatePercentage = d.CarrierFuelStatePercentage _
                                           , .CarrierFuelStateEffective = d.CarrierFuelStateEffective _
                                           , .CarrierFuelStateUpdated = If(d.CarrierFuelStateUpdated Is Nothing, New Byte() {}, d.CarrierFuelStateUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierFuelStates.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierFuel).CarrierFuelStates)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted contact records 
            .CarrierFuelStates.InsertAllOnSubmit(GetCarrierFuelStateChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .CarrierFuelStates.AttachAll(GetCarrierFuelStateChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedCarrierFuelStates = GetCarrierFuelStateChanges(oData, TrackingInfo.Deleted)
            .CarrierFuelStates.AttachAll(deletedCarrierFuelStates, True)
            .CarrierFuelStates.DeleteAllOnSubmit(deletedCarrierFuelStates)
        End With
    End Sub

    Protected Function GetCarrierFuelStateChanges(ByVal source As DataTransferObjects.CarrierFuel, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierFuelState)

        Dim details As IEnumerable(Of LTS.CarrierFuelState) = (
                From d In source.CarrierFuelStates
                Where d.TrackingState = changeType
                Select New LTS.CarrierFuelState With {.CarrierFuelStateControl = d.CarrierFuelStateControl _
                , .CarrierFuelStateFuelControl = d.CarrierFuelStateFuelControl _
                , .CarrierFuelStatePercentage = d.CarrierFuelStatePercentage _
                , .CarrierFuelStateEffective = d.CarrierFuelStateEffective _
                , .CarrierFuelStateUpdated = If(d.CarrierFuelStateUpdated Is Nothing, New Byte() {}, d.CarrierFuelStateUpdated)})
        Return details.ToList()
    End Function

#End Region

End Class