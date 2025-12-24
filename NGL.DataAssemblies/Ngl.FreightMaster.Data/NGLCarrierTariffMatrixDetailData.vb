Imports System.ServiceModel

Public Class NGLCarrierTariffMatrixDetailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffMatrixDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierTariffMatrixDetailData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMatrixDetails
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
        Return GetCarrierTariffMatrixDetailFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierTariffMatrixDetailsFiltered()
    End Function

    Public Function GetCarrierTariffMatrixDetailFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierTariffMatrixDetail
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffMatrixDetail As DataTransferObjects.CarrierTariffMatrixDetail = (
                        From d In db.CarrierTariffMatrixDetails
                        Where
                        d.CarrTarMatDetControl = Control
                        Select New DataTransferObjects.CarrierTariffMatrixDetail With {.CarrTarMatDetControl = d.CarrTarMatDetControl _
                        , .CarrTarMatDetCarrTarMatControl = d.CarrTarMatDetCarrTarMatControl _
                        , .CarrTarMatDetID = d.CarrTarMatDetID _
                        , .CarrTarMatDetValue = If(d.CarrTarMatDetValue.HasValue, d.CarrTarMatDetValue.Value, 0) _
                        , .CarrTarMatDetModDate = d.CarrTarMatDetModDate _
                        , .CarrTarMatDetModUser = d.CarrTarMatDetModUser _
                        , .CarrTarMatDetUpdated = d.CarrTarMatDetUpdated.ToArray()}).First


                Return CarrierTariffMatrixDetail

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

    Public Function GetCarrierTariffMatrixDetailsFiltered(Optional ByVal CarrTarMatControl As Integer = 0) As DataTransferObjects.CarrierTariffMatrixDetail()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the details that match the criteria sorted by ID
                Dim CarrierTariffMatrixDetails() As DataTransferObjects.CarrierTariffMatrixDetail = (
                        From d In db.CarrierTariffMatrixDetails
                        Where
                        (d.CarrTarMatDetCarrTarMatControl = CarrTarMatControl)
                        Order By d.CarrTarMatDetID
                        Select New DataTransferObjects.CarrierTariffMatrixDetail With {.CarrTarMatDetControl = d.CarrTarMatDetControl _
                        , .CarrTarMatDetCarrTarMatControl = d.CarrTarMatDetCarrTarMatControl _
                        , .CarrTarMatDetID = d.CarrTarMatDetID _
                        , .CarrTarMatDetValue = If(d.CarrTarMatDetValue.HasValue, d.CarrTarMatDetValue.Value, 0) _
                        , .CarrTarMatDetModDate = d.CarrTarMatDetModDate _
                        , .CarrTarMatDetModUser = d.CarrTarMatDetModUser _
                        , .CarrTarMatDetUpdated = d.CarrTarMatDetUpdated.ToArray()}).ToArray()
                Return CarrierTariffMatrixDetails

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

    Friend Function AddNew(ByVal CarrTarMatControl As Integer, ByVal ID As Integer, ByVal Value As Decimal) As LTS.CarrierTariffMatrixDetail
        Dim nObject As New LTS.CarrierTariffMatrixDetail
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                nObject.CarrTarMatDetCarrTarMatControl = CarrTarMatControl
                nObject.CarrTarMatDetID = ID
                nObject.CarrTarMatDetValue = Value
                nObject.CarrTarMatDetModDate = Date.Now
                nObject.CarrTarMatDetModUser = Parameters.UserName
                nObject.CarrTarMatDetUpdated = New Byte() {}
                'modified by RHR 2/6/15 we use db not LinqDB because LinqDB connection will not be closed
                db.CarrierTariffMatrixDetails.InsertOnSubmit(nObject)
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
        Dim d = CType(oData, DataTransferObjects.CarrierTariffMatrixDetail)
        'Create New Record
        Return New LTS.CarrierTariffMatrixDetail With {.CarrTarMatDetControl = d.CarrTarMatDetControl _
            , .CarrTarMatDetCarrTarMatControl = d.CarrTarMatDetCarrTarMatControl _
            , .CarrTarMatDetID = d.CarrTarMatDetID _
            , .CarrTarMatDetValue = d.CarrTarMatDetValue _
            , .CarrTarMatDetModDate = Date.Now _
            , .CarrTarMatDetModUser = Parameters.UserName _
            , .CarrTarMatDetUpdated = If(d.CarrTarMatDetUpdated Is Nothing, New Byte() {}, d.CarrTarMatDetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierTariffMatrixDetailFiltered(Control:=CType(LinqTable, LTS.CarrierTariffMatrixDetail).CarrTarMatDetControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffMatrixDetail = TryCast(LinqTable, LTS.CarrierTariffMatrixDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMatrixDetails
                    Where d.CarrTarMatDetControl = source.CarrTarMatDetControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMatDetControl _
                        , .ModDate = d.CarrTarMatDetModDate _
                        , .ModUser = d.CarrTarMatDetModUser _
                        , .Updated = d.CarrTarMatDetUpdated.ToArray}).First

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