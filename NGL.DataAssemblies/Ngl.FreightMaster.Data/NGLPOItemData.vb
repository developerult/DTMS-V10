Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports System.Linq.Dynamic

Public Class NGLPOItemData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.POItems
        Me.LinqDB = db
        Me.SourceClass = "NGLPOItemData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.POItems
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
        Return GetPOItemFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetPOItemsFiltered()
    End Function

    Public Function GetPOItemFiltered(ByVal Control As Long) As DTO.POItem
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim POItem As DTO.POItem = (
                From d In db.POItems
                Where
                    d.POItemControl = Control
                Select New DTO.POItem With {.POItemControl = d.POItemControl _
                                      , .ItemPONumber = d.ItemPONumber _
                                      , .FixOffInvAllow = d.FixOffInvAllow _
                                      , .FixFrtAllow = d.FixFrtAllow _
                                      , .ItemNumber = d.ItemNumber _
                                      , .QtyOrdered = d.QtyOrdered _
                                      , .FreightCost = d.FreightCost _
                                      , .ItemCost = d.ItemCost _
                                      , .Weight = d.Weight _
                                      , .Cube = d.Cube _
                                      , .Pack = d.Pack _
                                      , .Size = d.Size _
                                      , .Description = d.Description _
                                      , .Hazmat = d.Hazmat _
                                      , .CreatedUser = d.CreatedUser _
                                      , .CreatedDate = d.CreatedDate _
                                      , .Brand = d.Brand _
                                      , .CostCenter = d.CostCenter _
                                      , .LotNumber = d.LotNumber _
                                      , .LotExpirationDate = d.LotExpirationDate _
                                      , .GTIN = d.GTIN _
                                      , .CustItemNumber = d.CustItemNumber _
                                      , .CustomerNumber = d.CustomerNumber _
                                      , .POOrderSequence = d.POOrderSequence _
                                      , .PalletType = d.PalletType _
                                     , .POItemHazmatTypeCode = d.POItemHazmatTypeCode _
                                     , .POItem49CFRCode = d.POItem49CFRCode _
                                     , .POItemIATACode = d.POItemIATACode _
                                     , .POItemDOTCode = d.POItemDOTCode _
                                     , .POItemMarineCode = d.POItemMarineCode _
                                     , .POItemNMFCClass = d.POItemNMFCClass _
                                     , .POItemFAKClass = d.POItemFAKClass _
                                     , .POItemLimitedQtyFlag = d.POItemLimitedQtyFlag _
                                     , .POItemPallets = d.POItemPallets _
                                     , .POItemTies = d.POItemTies _
                                     , .POItemHighs = d.POItemHighs _
                                     , .POItemQtyPalletPercentage = d.POItemQtyPalletPercentage _
                                     , .POItemQtyLength = d.POItemQtyLength _
                                     , .POItemQtyWidth = d.POItemQtyWidth _
                                     , .POItemQtyHeight = d.POItemQtyHeight _
                                     , .POItemStackable = d.POItemStackable _
                                     , .POItemLevelOfDensity = d.POItemLevelOfDensity _
                                           , .POItemCompLegalEntity = d.POItemCompLegalEntity _
                                   , .POItemCompAlphaCode = d.POItemCompAlphaCode _
                                   , .POItemNMFCSubClass = d.POItemNMFCSubClass _
                                   , .POItemUser1 = d.POItemUser1 _
                                   , .POItemUser2 = d.POItemUser2 _
                                   , .POItemUser3 = d.POItemUser3 _
                                   , .POItemUser4 = d.POItemUser4 _
                                      , .POItemUpdated = d.POItemUpdated.ToArray()}).First


                Return POItem

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

    Public Function GetPOItemsFiltered(Optional ByVal OrderNumber As String = "", Optional ByVal OrderSequence As Integer = 0, Optional ByVal CompNumber As Integer = 0) As DTO.POItem()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim POItems() As DTO.POItem = (
                From d In db.POItems
                Where
                    (d.ItemPONumber = OrderNumber) _
                    And
                    (d.POOrderSequence = OrderSequence) _
                    And
                    If(d.CustomerNumber, 0) = If(CompNumber = 0, If(d.CustomerNumber, 0), CompNumber)
                Order By d.POItemControl
                Select New DTO.POItem With {.POItemControl = d.POItemControl _
                                      , .ItemPONumber = d.ItemPONumber _
                                      , .FixOffInvAllow = d.FixOffInvAllow _
                                      , .FixFrtAllow = d.FixFrtAllow _
                                      , .ItemNumber = d.ItemNumber _
                                      , .QtyOrdered = d.QtyOrdered _
                                      , .FreightCost = d.FreightCost _
                                      , .ItemCost = d.ItemCost _
                                      , .Weight = d.Weight _
                                      , .Cube = d.Cube _
                                      , .Pack = d.Pack _
                                      , .Size = d.Size _
                                      , .Description = d.Description _
                                      , .Hazmat = d.Hazmat _
                                      , .CreatedUser = d.CreatedUser _
                                      , .CreatedDate = d.CreatedDate _
                                      , .Brand = d.Brand _
                                      , .CostCenter = d.CostCenter _
                                      , .LotNumber = d.LotNumber _
                                      , .LotExpirationDate = d.LotExpirationDate _
                                      , .GTIN = d.GTIN _
                                      , .CustItemNumber = d.CustItemNumber _
                                      , .CustomerNumber = d.CustomerNumber _
                                      , .POOrderSequence = d.POOrderSequence _
                                      , .PalletType = d.PalletType _
                                     , .POItemHazmatTypeCode = d.POItemHazmatTypeCode _
                                     , .POItem49CFRCode = d.POItem49CFRCode _
                                     , .POItemIATACode = d.POItemIATACode _
                                     , .POItemDOTCode = d.POItemDOTCode _
                                     , .POItemMarineCode = d.POItemMarineCode _
                                     , .POItemNMFCClass = d.POItemNMFCClass _
                                     , .POItemFAKClass = d.POItemFAKClass _
                                     , .POItemLimitedQtyFlag = d.POItemLimitedQtyFlag _
                                     , .POItemPallets = d.POItemPallets _
                                     , .POItemTies = d.POItemTies _
                                     , .POItemHighs = d.POItemHighs _
                                     , .POItemQtyPalletPercentage = d.POItemQtyPalletPercentage _
                                     , .POItemQtyLength = d.POItemQtyLength _
                                     , .POItemQtyWidth = d.POItemQtyWidth _
                                     , .POItemQtyHeight = d.POItemQtyHeight _
                                     , .POItemStackable = d.POItemStackable _
                                     , .POItemLevelOfDensity = d.POItemLevelOfDensity _
                                           , .POItemCompLegalEntity = d.POItemCompLegalEntity _
                                   , .POItemCompAlphaCode = d.POItemCompAlphaCode _
                                   , .POItemNMFCSubClass = d.POItemNMFCSubClass _
                                   , .POItemUser1 = d.POItemUser1 _
                                   , .POItemUser2 = d.POItemUser2 _
                                   , .POItemUser3 = d.POItemUser3 _
                                   , .POItemUser4 = d.POItemUser4 _
                                      , .POItemUpdated = d.POItemUpdated.ToArray()}).ToArray()
                Return POItems

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

    Public Function GetPOItemsFiltered365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vPOItem()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vPOItem
        Dim CompNumber = filters.CompNumberFrom
        Dim OrderNumber = filters.Data
        Dim OrderSequence = filters.ParentControl
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vPOItem)
                iQuery = (From d In db.vPOItems
                          Where
                              (d.ItemPONumber = OrderNumber) _
                              And
                              (d.POOrderSequence = OrderSequence) _
                              And
                              If(d.CustomerNumber, 0) = If(CompNumber = 0, If(d.CustomerNumber, 0), CompNumber)
                          Order By d.POItemControl
                          Select d)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPOItemsFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function


    Public Function GetPOItemsByParent(ByVal PohdrControl As Long) As LTS.vPOItem()
        If PohdrControl = 0 Then Return Nothing
        Dim oRet() As LTS.vPOItem
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oPOHDR = db.POHdrs.Where(Function(x) x.POHdrControl = PohdrControl).FirstOrDefault()
                Dim OrderNumber = oPOHDR.POHDROrderNumber
                Dim OrderSequence = oPOHDR.POHDROrderSequence
                Dim CompNumber = oPOHDR.POHDRDefaultCustomer
                oRet = db.vPOItems.Where(Function(x) x.ItemPONumber = OrderNumber And x.POOrderSequence = OrderSequence And x.CustomerNumber = CompNumber).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPOItemsByParent"), db)
            End Try
        End Using
        Return Nothing
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.POItem)
        'Create New Record
        Return New LTS.POItem With {.POItemControl = d.POItemControl _
                                      , .ItemPONumber = d.ItemPONumber _
                                      , .FixOffInvAllow = d.FixOffInvAllow _
                                      , .FixFrtAllow = d.FixFrtAllow _
                                      , .ItemNumber = d.ItemNumber _
                                      , .QtyOrdered = d.QtyOrdered _
                                      , .FreightCost = d.FreightCost _
                                      , .ItemCost = d.ItemCost _
                                      , .Weight = d.Weight _
                                      , .Cube = d.Cube _
                                      , .Pack = d.Pack _
                                      , .Size = d.Size _
                                      , .Description = d.Description _
                                      , .Hazmat = d.Hazmat _
                                      , .CreatedUser = d.CreatedUser _
                                      , .CreatedDate = d.CreatedDate _
                                      , .Brand = d.Brand _
                                      , .CostCenter = d.CostCenter _
                                      , .LotNumber = d.LotNumber _
                                      , .LotExpirationDate = d.LotExpirationDate _
                                      , .GTIN = d.GTIN _
                                      , .CustItemNumber = d.CustItemNumber _
                                      , .CustomerNumber = d.CustomerNumber _
                                      , .POOrderSequence = d.POOrderSequence _
                                      , .PalletType = d.PalletType _
                                     , .POItemHazmatTypeCode = d.POItemHazmatTypeCode _
                                     , .POItem49CFRCode = d.POItem49CFRCode _
                                     , .POItemIATACode = d.POItemIATACode _
                                     , .POItemDOTCode = d.POItemDOTCode _
                                     , .POItemMarineCode = d.POItemMarineCode _
                                     , .POItemNMFCClass = d.POItemNMFCClass _
                                     , .POItemFAKClass = d.POItemFAKClass _
                                     , .POItemLimitedQtyFlag = d.POItemLimitedQtyFlag _
                                     , .POItemPallets = d.POItemPallets _
                                     , .POItemTies = d.POItemTies _
                                     , .POItemHighs = d.POItemHighs _
                                     , .POItemQtyPalletPercentage = d.POItemQtyPalletPercentage _
                                     , .POItemQtyLength = d.POItemQtyLength _
                                     , .POItemQtyWidth = d.POItemQtyWidth _
                                     , .POItemQtyHeight = d.POItemQtyHeight _
                                     , .POItemStackable = d.POItemStackable _
                                     , .POItemLevelOfDensity = d.POItemLevelOfDensity _
                                   , .POItemCompLegalEntity = d.POItemCompLegalEntity _
                                   , .POItemCompAlphaCode = d.POItemCompAlphaCode _
                                   , .POItemNMFCSubClass = d.POItemNMFCSubClass _
                                   , .POItemUser1 = d.POItemUser1 _
                                   , .POItemUser2 = d.POItemUser2 _
                                   , .POItemUser3 = d.POItemUser3 _
                                   , .POItemUser4 = d.POItemUser4 _
                                      , .POItemUpdated = If(d.POItemUpdated Is Nothing, New Byte() {}, d.POItemUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetPOItemFiltered(Control:=CType(LinqTable, LTS.POItem).POItemControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.POItem = TryCast(LinqTable, LTS.POItem)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.POItems
                       Where d.POItemControl = source.POItemControl
                       Select New DTO.QuickSaveResults With {.Control = d.POItemControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.POItemUpdated.ToArray}).First

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


