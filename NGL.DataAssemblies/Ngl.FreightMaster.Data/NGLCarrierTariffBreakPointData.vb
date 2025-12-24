Imports System.ServiceModel

Public Class NGLCarrierTariffBreakPointData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffBreakPoints
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierTariffBreakPointData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffBreakPoints
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
        Return GetCarrierTariffBreakPointFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierTariffBreakPointsFiltered()
    End Function

    Public Function GetCarrierTariffBreakPointFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierTariffBreakPoint
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffBreakPoint As DataTransferObjects.CarrierTariffBreakPoint = (
                        From d In db.CarrierTariffBreakPoints
                        Where
                        d.CarrTarBPControl = Control
                        Select New DataTransferObjects.CarrierTariffBreakPoint With {.CarrTarBPControl = d.CarrTarBPControl _
                        , .CarrTarBPCarrTarControl = d.CarrTarBPCarrTarControl _
                        , .CarrTarBPID = d.CarrTarBPID _
                        , .CarrTarBPValue = If(d.CarrTarBPValue.HasValue, d.CarrTarBPValue.Value, 0) _
                        , .CarrTarBPModDate = d.CarrTarBPModDate _
                        , .CarrTarBPModUser = d.CarrTarBPModUser _
                        , .CarrTarBPUpdated = d.CarrTarBPUpdated.ToArray()}).First


                Return CarrierTariffBreakPoint

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

    Public Function GetCarrierTariffBreakPointsFiltered(Optional ByVal CarrTarControl As Integer = 0) As DataTransferObjects.CarrierTariffBreakPoint()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the details that match the criteria sorted by ID
                Dim CarrierTariffBreakPoints() As DataTransferObjects.CarrierTariffBreakPoint = (
                        From d In db.CarrierTariffBreakPoints
                        Where
                        (d.CarrTarBPCarrTarControl = CarrTarControl)
                        Order By d.CarrTarBPID
                        Select New DataTransferObjects.CarrierTariffBreakPoint With {.CarrTarBPControl = d.CarrTarBPControl _
                        , .CarrTarBPCarrTarControl = d.CarrTarBPCarrTarControl _
                        , .CarrTarBPID = d.CarrTarBPID _
                        , .CarrTarBPValue = If(d.CarrTarBPValue.HasValue, d.CarrTarBPValue.Value, 0) _
                        , .CarrTarBPModDate = d.CarrTarBPModDate _
                        , .CarrTarBPModUser = d.CarrTarBPModUser _
                        , .CarrTarBPUpdated = d.CarrTarBPUpdated.ToArray()}).ToArray()
                Return CarrierTariffBreakPoints

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

    Friend Function AddNew(ByVal CarrTarControl As Integer, ByVal ID As Integer, ByVal Value As Decimal) As LTS.CarrierTariffBreakPoint
        Dim nObject As New LTS.CarrierTariffBreakPoint
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                nObject.CarrTarBPCarrTarControl = CarrTarControl
                nObject.CarrTarBPID = ID
                nObject.CarrTarBPValue = Value
                nObject.CarrTarBPModDate = Date.Now
                nObject.CarrTarBPModUser = Parameters.UserName
                nObject.CarrTarBPUpdated = New Byte() {}
                'modified by RHR 2/6/15 we use db not LinqDB because LinqDB connection will not be closed
                db.CarrierTariffBreakPoints.InsertOnSubmit(nObject)
                db.SubmitChanges()
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

            Return nObject

        End Using

    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierTariffBreakPoint)
        'Create New Record
        Return New LTS.CarrierTariffBreakPoint With {.CarrTarBPControl = d.CarrTarBPControl _
            , .CarrTarBPCarrTarControl = d.CarrTarBPCarrTarControl _
            , .CarrTarBPID = d.CarrTarBPID _
            , .CarrTarBPValue = d.CarrTarBPValue _
            , .CarrTarBPModDate = Date.Now _
            , .CarrTarBPModUser = Parameters.UserName _
            , .CarrTarBPUpdated = If(d.CarrTarBPUpdated Is Nothing, New Byte() {}, d.CarrTarBPUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierTariffBreakPointFiltered(Control:=CType(LinqTable, LTS.CarrierTariffBreakPoint).CarrTarBPControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffBreakPoint = TryCast(LinqTable, LTS.CarrierTariffBreakPoint)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffBreakPoints
                    Where d.CarrTarBPControl = source.CarrTarBPControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarBPControl _
                        , .ModDate = d.CarrTarBPModDate _
                        , .ModUser = d.CarrTarBPModUser _
                        , .Updated = d.CarrTarBPUpdated.ToArray}).First

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