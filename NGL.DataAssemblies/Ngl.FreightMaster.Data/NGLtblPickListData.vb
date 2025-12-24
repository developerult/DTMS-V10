Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports System.Linq.Dynamic

Public Class NGLtblPickListData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.tblPickLists
        Me.LinqDB = db
        Me.SourceClass = "NGLtblPickListData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.tblPickLists
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblPickListFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblPickListsFiltered()
    End Function

    Public Function GettblPickListFiltered(ByVal Control As Long) As DTO.tblPickList
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblPickList As DTO.tblPickList = (
                From d In db.tblPickLists
                Where
                    d.PLControl = Control
                Select selectDTOData(d)).FirstOrDefault()
                Return tblPickList
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblPickListFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblPickListsFiltered(Optional ByVal dtTestDate As Nullable(Of Date) = Nothing,
                                            Optional ByVal intMaxRetry As Nullable(Of Integer) = Nothing,
                                            Optional ByVal strCompNumber As String = Nothing,
                                            Optional ByVal strOrderNumber As String = Nothing,
                                            Optional ByVal intOrderSequence As Nullable(Of Integer) = Nothing) As DTO.tblPickList()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim tblPickLists() As DTO.tblPickList = (
                From d In db.tblPickLists
                Where
                    (d.PLExported = 0) _
                    And (dtTestDate.HasValue = False OrElse d.PLExportDate.HasValue = False OrElse d.PLExportDate < dtTestDate.Value) _
                    And (intMaxRetry.HasValue = False OrElse d.PLExportRetry.HasValue = False OrElse d.PLExportRetry < intMaxRetry.Value) _
                    And (String.IsNullOrEmpty(strCompNumber) OrElse d.CompNumber = strCompNumber) _
                    And (String.IsNullOrEmpty(strOrderNumber) _
                      OrElse (d.BookCarrOrderNumber = strOrderNumber _
                      And (intOrderSequence.HasValue = False OrElse d.BookOrderSequence = intOrderSequence.Value)
                      )
                      )
                Order By d.PLControl
                Select selectDTOData(d)).ToArray()

                Return tblPickLists

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblPickListsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Returns the item details need for Pick List Status Update Web Services
    ''' We now call spGetExportPickDetailRows70 instead of spGetExportDetailRows70
    ''' because Pick Data needs Contracted Cost and AP Data needs Billed Cost
    ''' AP export procedures should still use GetExportDetailRows70
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100
    ''' Note: a duplicate instance of this method is available in thw NGLAPMassEntryData library
    ''' </remarks>
    Public Function GetExportPickDetailRows70(ByVal BookControl As Integer) As List(Of LTS.spGetExportPickDetailRows70Result)

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oReturnData = (From d In db.spGetExportPickDetailRows70(BookControl) Select d).ToList()

                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportPickDetailRows70"))
            End Try
        End Using
        Return Nothing
    End Function

#Region "LTS Methods"
    ''' <summary>
    ''' update the exported flags and details for the PLControl number provided 
    ''' </summary>
    ''' <param name="PLControl"></param>
    ''' <param name="ExportDate"></param>
    ''' <param name="Exported"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/15/2021 used replaces old xsd code in Integration DLL logic
    ''' </remarks>
    Public Function UpdatePickListExportStatus(ByVal PLControl As Long, ByVal ExportDate As Date, Optional ByVal Exported As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oPlData = db.tblPickLists.Where(Function(d) d.PLControl = PLControl).FirstOrDefault()
                If Not oPlData Is Nothing AndAlso oPlData.PLControl <> 0 Then
                    oPlData.PLExportDate = ExportDate
                    oPlData.PLExported = Exported
                    oPlData.PLExportRetry = oPlData.PLExportRetry + 1
                    db.SubmitChanges()
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateStatusPLExportStatus"))
            End Try
        End Using
        Return blnRet
    End Function


#End Region


#Region "LTS Methods"


#End Region


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblPickList)
        'Create New Record
        Return New LTS.tblPickList With {.PLControl = d.PLControl _
                                                , .PLExportDate = d.PLExportDate _
                                                , .PLExportRetry = d.PLExportRetry _
                                                , .PLExported = d.PLExported _
                                                , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                                , .BookConsPrefix = d.BookConsPrefix _
                                                , .CarrierNumber = d.CarrierNumber _
                                                , .BookRevTotalCost = d.BookRevTotalCost _
                                                , .LoadOrder = d.LoadOrder _
                                                , .BookDateLoad = d.BookDateLoad _
                                                , .BookDateRequired = d.BookDateRequired _
                                                , .BookLoadCom = d.BookLoadCom _
                                                , .BookProNumber = d.BookProNumber _
                                                , .BookRouteFinalCode = d.BookRouteFinalCode _
                                                , .BookRouteFinalDate = d.BookRouteFinalDate _
                                                , .BookTotalCases = d.BookTotalCases _
                                                , .BookTotalWgt = d.BookTotalWgt _
                                                , .BookTotalPL = d.BookTotalPL _
                                                , .BookTotalCube = d.BookTotalCube _
                                                , .BookTotalBFC = d.BookTotalBFC _
                                                , .BookStopNo = d.BookStopNo _
                                                , .CompName = d.CompName _
                                                , .CompNumber = d.CompNumber _
                                                , .BookTypeCode = d.BookTypeCode _
                                                , .BookDateOrdered = d.BookDateOrdered _
                                                , .BookOrigName = d.BookOrigName _
                                                , .BookOrigAddress1 = d.BookOrigAddress1 _
                                                , .BookOrigAddress2 = d.BookOrigAddress2 _
                                                , .BookOrigAddress3 = d.BookOrigAddress3 _
                                                , .BookOrigCity = d.BookOrigCity _
                                                , .BookOrigState = d.BookOrigState _
                                                , .BookOrigCountry = d.BookOrigCountry _
                                                , .BookOrigZip = d.BookOrigZip _
                                                , .BookDestName = d.BookDestName _
                                                , .BookDestAddress1 = d.BookDestAddress1 _
                                                , .BookDestAddress2 = d.BookDestAddress2 _
                                                , .BookDestAddress3 = d.BookDestAddress3 _
                                                , .BookDestCity = d.BookDestCity _
                                                , .BookDestState = d.BookDestState _
                                                , .BookDestCountry = d.BookDestCountry _
                                                , .BookDestZip = d.BookDestZip _
                                                , .BookLoadPONumber = d.BookLoadPONumber _
                                                , .CarrierName = d.CarrierName _
                                                , .LaneNumber = d.LaneNumber _
                                                , .CommCodeDescription = d.CommCodeDescription _
                                                , .BookMilesFrom = d.BookMilesFrom _
                                                , .BookCommCompControl = d.BookCommCompControl _
                                                , .BookRevCommCost = d.BookRevCommCost _
                                                , .BookRevGrossRevenue = d.BookRevGrossRevenue _
                                                , .BookFinCommStd = d.BookFinCommStd _
                                                , .BookDoNotInvoice = d.BookDoNotInvoice _
                                                , .BookOrderSequence = d.BookOrderSequence _
                                                , .CarrierEquipmentCodes = d.CarrierEquipmentCodes _
                                                , .BookCarrierTypeCode = d.BookCarrierTypeCode _
                                                , .BookWarehouseNumber = d.BookWarehouseNumber _
                                                , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
                                                , .CompNatNumber = d.CompNatNumber _
                                                , .BookTransType = d.BookTransType _
                                                , .BookShipCarrierNumber = d.BookShipCarrierNumber _
                                                , .LaneComments = d.LaneComments _
                                                , .FuelSurCharge = d.FuelSurCharge _
                                                , .BookRevCarrierCost = d.BookRevCarrierCost _
                                                , .BookRevOtherCost = d.BookRevOtherCost _
                                                , .BookRevNetCost = d.BookRevNetCost _
                                                , .BookRevFreightTax = d.BookRevFreightTax _
                                                , .BookFinServiceFee = d.BookFinServiceFee _
                                                , .BookRevLoadSavings = d.BookRevLoadSavings _
                                                , .TotalNonFuelFees = d.TotalNonFuelFees _
                                                , .BookPickNumber = d.BookPickNumber _
                                                , .BookPickupStopNumber = d.BookPickupStopNumber _
                                                , .BookRouteConsFlag = d.BookRouteConsFlag _
                                                , .BookAlternateAddressLaneNumber = d.BookAlternateAddressLaneNumber _
                                                , .BookSHID = d.BookSHID _
                                                , .BookShipCarrierDetails = d.BookShipCarrierDetails _
                                                , .BookShipCarrierName = d.BookShipCarrierName _
                                                , .BookRevNonTaxable = d.BookRevNonTaxable _
                                                , .BookWhseAuthorizationNo = d.BookWhseAuthorizationNo _
                                                , .BookControl = d.BookControl _
                                                , .PLUpdated = If(d.PLUpdated Is Nothing, New Byte() {}, d.PLUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblPickListFiltered(Control:=CType(LinqTable, LTS.tblPickList).PLControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.tblPickList = TryCast(LinqTable, LTS.tblPickList)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.tblPickLists
                       Where d.PLControl = source.PLControl
                       Select New DTO.QuickSaveResults With {.Control = d.PLControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.PLUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.tblPickList, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblPickList

        Return New DTO.tblPickList With {.PLControl = d.PLControl _
                                                , .PLExportDate = d.PLExportDate _
                                                , .PLExportRetry = d.PLExportRetry _
                                                , .PLExported = d.PLExported _
                                                , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                                , .BookConsPrefix = d.BookConsPrefix _
                                                , .CarrierNumber = d.CarrierNumber _
                                                , .BookRevTotalCost = d.BookRevTotalCost _
                                                , .LoadOrder = d.LoadOrder _
                                                , .BookDateLoad = d.BookDateLoad _
                                                , .BookDateRequired = d.BookDateRequired _
                                                , .BookLoadCom = d.BookLoadCom _
                                                , .BookProNumber = d.BookProNumber _
                                                , .BookRouteFinalCode = d.BookRouteFinalCode _
                                                , .BookRouteFinalDate = d.BookRouteFinalDate _
                                                , .BookTotalCases = d.BookTotalCases _
                                                , .BookTotalWgt = d.BookTotalWgt _
                                                , .BookTotalPL = d.BookTotalPL _
                                                , .BookTotalCube = d.BookTotalCube _
                                                , .BookTotalBFC = d.BookTotalBFC _
                                                , .BookStopNo = d.BookStopNo _
                                                , .CompName = d.CompName _
                                                , .CompNumber = d.CompNumber _
                                                , .BookTypeCode = d.BookTypeCode _
                                                , .BookDateOrdered = d.BookDateOrdered _
                                                , .BookOrigName = d.BookOrigName _
                                                , .BookOrigAddress1 = d.BookOrigAddress1 _
                                                , .BookOrigAddress2 = d.BookOrigAddress2 _
                                                , .BookOrigAddress3 = d.BookOrigAddress3 _
                                                , .BookOrigCity = d.BookOrigCity _
                                                , .BookOrigState = d.BookOrigState _
                                                , .BookOrigCountry = d.BookOrigCountry _
                                                , .BookOrigZip = d.BookOrigZip _
                                                , .BookDestName = d.BookDestName _
                                                , .BookDestAddress1 = d.BookDestAddress1 _
                                                , .BookDestAddress2 = d.BookDestAddress2 _
                                                , .BookDestAddress3 = d.BookDestAddress3 _
                                                , .BookDestCity = d.BookDestCity _
                                                , .BookDestState = d.BookDestState _
                                                , .BookDestCountry = d.BookDestCountry _
                                                , .BookDestZip = d.BookDestZip _
                                                , .BookLoadPONumber = d.BookLoadPONumber _
                                                , .CarrierName = d.CarrierName _
                                                , .LaneNumber = d.LaneNumber _
                                                , .CommCodeDescription = d.CommCodeDescription _
                                                , .BookMilesFrom = d.BookMilesFrom _
                                                , .BookCommCompControl = d.BookCommCompControl _
                                                , .BookRevCommCost = d.BookRevCommCost _
                                                , .BookRevGrossRevenue = d.BookRevGrossRevenue _
                                                , .BookFinCommStd = d.BookFinCommStd _
                                                , .BookDoNotInvoice = d.BookDoNotInvoice _
                                                , .BookOrderSequence = d.BookOrderSequence _
                                                , .CarrierEquipmentCodes = d.CarrierEquipmentCodes _
                                                , .BookCarrierTypeCode = d.BookCarrierTypeCode _
                                                , .BookWarehouseNumber = d.BookWarehouseNumber _
                                                , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
                                                , .CompNatNumber = d.CompNatNumber _
                                                , .BookTransType = d.BookTransType _
                                                , .BookShipCarrierNumber = d.BookShipCarrierNumber _
                                                , .LaneComments = d.LaneComments _
                                                , .FuelSurCharge = d.FuelSurCharge _
                                                , .BookRevCarrierCost = d.BookRevCarrierCost _
                                                , .BookRevOtherCost = d.BookRevOtherCost _
                                                , .BookRevNetCost = d.BookRevNetCost _
                                                , .BookRevFreightTax = d.BookRevFreightTax _
                                                , .BookFinServiceFee = d.BookFinServiceFee _
                                                , .BookRevLoadSavings = d.BookRevLoadSavings _
                                                , .TotalNonFuelFees = d.TotalNonFuelFees _
                                                , .BookPickNumber = d.BookPickNumber _
                                                , .BookPickupStopNumber = d.BookPickupStopNumber _
                                                , .BookRouteConsFlag = d.BookRouteConsFlag _
                                                , .BookAlternateAddressLaneNumber = d.BookAlternateAddressLaneNumber _
                                                , .BookSHID = d.BookSHID _
                                                , .BookShipCarrierDetails = d.BookShipCarrierDetails _
                                                , .BookShipCarrierName = d.BookShipCarrierName _
                                                , .BookRevNonTaxable = d.BookRevNonTaxable _
                                                , .BookWhseAuthorizationNo = d.BookWhseAuthorizationNo _
                                                , .BookControl = d.BookControl _
                                                , .PLUpdated = d.PLUpdated.ToArray() _
                                                , .Page = page _
                                                , .Pages = pagecount _
                                                , .RecordCount = recordcount _
                                                , .PageSize = pagesize}

    End Function


#End Region

End Class


