Imports System.ServiceModel

Public Class NGLCarrTarEquipMatDetData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffEquipMatrixDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarEquipMatDetData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffEquipMatrixDetails
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
        Return GetCarrTarEquipMatDetFiltered(Control)
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

    Public Function GetCarrTarEquipMatDetFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarEquipMatDet
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrTarEquipMatDet As DataTransferObjects.CarrTarEquipMatDet = (
                        From d In db.CarrierTariffEquipMatrixDetails
                        Where
                        d.CarrTarEquipMatDetControl = Control
                        Select selectDTOData(d, db)).First


                Return CarrTarEquipMatDet

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
    ''' <param name="CarrTarEquipMatControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarEquipMatDetsFiltered(ByVal CarrTarEquipMatControl As Integer) As DataTransferObjects.CarrTarEquipMatDet()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the details that match the CarrTarEquipMatDetID sorted by ID
                Dim CarrierTariffEquipMatrixDetails() As DataTransferObjects.CarrTarEquipMatDet = (
                        From d In db.CarrierTariffEquipMatrixDetails
                        Where
                        (d.CarrTarEquipMatDetCarrTarEquipMatControl = CarrTarEquipMatControl)
                        Order By d.CarrTarEquipMatDetID
                        Select selectDTOData(d, db)).ToArray()

                Return CarrierTariffEquipMatrixDetails

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
        Dim d = CType(oData, DataTransferObjects.CarrTarEquipMatDet)
        'Create New Record
        Return New LTS.CarrierTariffEquipMatrixDetail With {.CarrTarEquipMatDetControl = d.CarrTarEquipMatDetControl _
            , .CarrTarEquipMatDetCarrTarEquipMatControl = d.CarrTarEquipMatDetCarrTarEquipMatControl _
            , .CarrTarEquipMatDetID = d.CarrTarEquipMatDetID _
            , .CarrTarEquipMatDetValue = d.CarrTarEquipMatDetValue _
            , .CarrTarEquipMatDetModDate = Date.Now _
            , .CarrTarEquipMatDetModUser = Parameters.UserName _
            , .CarrTarEquipMatDetUpdated = If(d.CarrTarEquipMatDetUpdated Is Nothing, New Byte() {}, d.CarrTarEquipMatDetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarEquipMatDetFiltered(Control:=DirectCast(LinqTable, LTS.CarrierTariffEquipMatrixDetail).CarrTarEquipMatDetControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariffEquipMatrixDetail = TryCast(LinqTable, LTS.CarrierTariffEquipMatrixDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffEquipMatrixDetails
                    Where d.CarrTarEquipMatDetControl = source.CarrTarEquipMatDetControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarEquipMatDetControl _
                        , .ModDate = d.CarrTarEquipMatDetModDate _
                        , .ModUser = d.CarrTarEquipMatDetModUser _
                        , .Updated = d.CarrTarEquipMatDetUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffEquipMatrixDetail, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarEquipMatDet
        Return New DataTransferObjects.CarrTarEquipMatDet With {.CarrTarEquipMatDetControl = d.CarrTarEquipMatDetControl _
            , .CarrTarEquipMatDetCarrTarEquipMatControl = d.CarrTarEquipMatDetCarrTarEquipMatControl _
            , .CarrTarEquipMatDetID = d.CarrTarEquipMatDetID _
            , .CarrTarEquipMatDetValue = d.CarrTarEquipMatDetValue _
            , .CarrTarEquipMatDetModDate = d.CarrTarEquipMatDetModDate _
            , .CarrTarEquipMatDetModUser = d.CarrTarEquipMatDetModUser _
            , .CarrTarEquipMatDetUpdated = d.CarrTarEquipMatDetUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function


#End Region

End Class