Imports System.ServiceModel
Imports Ngl.Core.Utility
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLLoadStatusBoardData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vLoadStatusBoards
        Me.LinqDB = db
        Me.SourceClass = "NGLLoadStatusBoardData"
    End Sub

#End Region

#Region " Properties "

    Private _RecalcTotals As Boolean = False

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vLoadStatusBoards
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _BookDependencyResult As LTS.spUpdateBookDependenciesResult
    Public Property BookDependencyResult() As LTS.spUpdateBookDependenciesResult
        Get
            Return _BookDependencyResult
        End Get
        Set(ByVal value As LTS.spUpdateBookDependenciesResult)
            _BookDependencyResult = value
        End Set
    End Property

    Private _LastProcedureName As String
    Public Property LastProcedureName() As String
        Get
            Return _LastProcedureName
        End Get
        Set(ByVal value As String)
            _LastProcedureName = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetvLoadStatusBoardFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetvLoadStatusBoardsByCNS()
    End Function

    'Public Function GetvLoadStatusBoardFiltered(Optional ByVal Control As Integer = 0) As DTO.vLoadStatusBoard
    '    Using db As New NGLMasBookDataContext(ConnectionString)
    '        Try
    '            Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
    '            'Get the newest record that matches the provided criteria                
    '            Dim vLoadStatusBoard As DTO.vLoadStatusBoard = ( _
    '            From d In db.vLoadStatusBoards _
    '            Where _
    '                (d.BookControl = If(Control = 0, d.BookControl, Control)) _
    '                And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl)) _
    '            Order By d.BookControl Descending _
    '            Select New DTO.vLoadStatusBoard With {.BookControl = d.BookControl _
    '                                        , .BookProNumber = d.BookProNumber _
    '                                        , .BookDateLoad = d.BookDateLoad _
    '                                        , .BookDateRequired = d.BookDateRequired _
    '                                        , .BookConsPrefix = d.BookConsPrefix _
    '                                        , .BookCustCompControl = d.BookCustCompControl _
    '                                        , .CompName = d.CompName _
    '                                        , .CompNumber = d.CompNumber _
    '                                        , .BookODControl = d.BookODControl _
    '                                        , .BookCarrierControl = d.BookCarrierControl _
    '                                        , .CarrierName = d.CarrierName _
    '                                        , .CarrierNumber = d.CarrierNumber _
    '                                        , .BookCarrierContControl = d.BookCarrierContControl _
    '                                        , .BookCarrierContact = d.BookCarrierContact _
    '                                        , .BookCarrierContactPhone = d.BookCarrierContactPhone _
    '                                        , .BookOrigName = d.BookOrigName _
    '                                        , .BookOrigAddress1 = d.BookOrigAddress1 _
    '                                        , .BookOrigCity = d.BookOrigCity _
    '                                        , .BookOrigState = d.BookOrigState _
    '                                        , .BookOrigZip = d.BookOrigZip _
    '                                        , .BookDestName = d.BookDestName _
    '                                        , .BookDestAddress1 = d.BookDestAddress1 _
    '                                        , .BookDestCity = d.BookDestCity _
    '                                        , .BookDestState = d.BookDestState _
    '                                        , .BookDestZip = d.BookDestZip _
    '                                        , .BookTotalCases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0) _
    '                                        , .BookTotalWgt = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0) _
    '                                        , .BookTotalPL = If(d.BookTotalPL.HasValue, d.BookTotalPL, 0) _
    '                                        , .BookTotalCube = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0) _
    '                                        , .BookTotalPX = If(d.BookTotalPX.HasValue, d.BookTotalPX, 0) _
    '                                        , .BookTotalBFC = If(d.BookTotalBFC.HasValue, d.BookTotalBFC, 0) _
    '                                        , .BookCarrActDate = d.BookCarrActDate _
    '                                        , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
    '                                        , .BookTranCode = d.BookTranCode _
    '                                        , .BookPayCode = d.BookPayCode _
    '                                        , .BookTypeCode = d.BookTypeCode _
    '                                        , .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo, 0) _
    '                                        , .BookModDate = d.BookModDate _
    '                                        , .BookModUser = d.BookModUser _
    '                                        , .BookRevBilledBFC = If(d.BookRevBilledBFC.HasValue, d.BookRevBilledBFC, 0) _
    '                                        , .BookRevCarrierCost = If(d.BookRevCarrierCost.HasValue, d.BookRevCarrierCost, 0) _
    '                                        , .BookRevOtherCost = If(d.BookRevOtherCost.HasValue, d.BookRevOtherCost, 0) _
    '                                        , .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0) _
    '                                        , .BookMilesFrom = If(d.BookMilesFrom.HasValue, d.BookMilesFrom, 0) _
    '                                        , .BookRouteConsFlag = d.BookRouteConsFlag _
    '                                        , .BookCarrOrderNumber = d.BookCarrOrderNumber _
    '                                        , .BookOrderSequence = d.BookOrderSequence _
    '                                        , .BookLockAllCosts = d.BookLockAllCosts _
    '                                        , .BookLockBFCCost = d.BookLockBFCCost _
    '                                        , .BookPickupStopNumber = d.BookPickupStopNumber _
    '                                        , .BookOrigStopNumber = d.BookOrigStopNumber _
    '                                        , .BookDestStopNumber = d.BookDestStopNumber _
    '                                        , .BookOrigMiles = d.BookOrigMiles _
    '                                        , .BookDestMiles = d.BookDestMiles _
    '                                        , .BookPickNumber = d.BookPickNumber _
    '                                        , .BookDateOrdered = d.BookDateOrdered _
    '                                        , .BookTransType = d.BookTransType}).First

    '            Return vLoadStatusBoard

    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return Nothing

    '    End Using
    'End Function

    Public Function GetvLoadStatusBoardFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.vLoadStatusBoard
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Get the newest record that matches the provided criteria                
                Return (
                    From d In db.vLoadStatusBoards
                        Where
                            (d.BookControl = If(Control = 0, d.BookControl, Control)) _
                            And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvLoadStatusBoardFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetvLoadStatusBoardsFiltered(ByVal Filters As String) As DataTransferObjects.vLoadStatusBoard()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oLoadStatusBoards As New List(Of vLoadStatusBoard)

        Try
            Dim strComp As String = ""
            strComp &= "( " _
                       & "(isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) > 0" _
                       & "	AND " _
                       & " CompNumber In (SELECT dbo.UserAdmin.UserAdminCompControl FROM dbo.UserAdmin	Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "')" _
                       & ")" _
                       & " OR " _
                       & " isnull((SELECT top 1 isnull(dbo.UserAdmin.UserAdminCompControl,0) FROM dbo.UserAdmin Where dbo.UserAdmin.UserAdminUserName = '" & Me.Parameters.UserName & "'),0) = 0" _
                       & ") "

            If Not String.IsNullOrEmpty(Filters) AndAlso Filters.Trim.Length > 1 Then
                strComp &= " AND "
            End If
            Dim strSQL As String = "SELECT Top 500 * From vLoadStatusBoard  " _
                                   & " WHERE  " & strComp & Filters

            'Dim strSQL As String = "SELECT Top 500 BookControl, BookDateLoad, BookDateRequired, BookOrigZip, BookOrigCity, BookOrigState, BookConsPrefix, BookProNumber, " _
            '                        & " BookCustCompControl, CompName, BookOrigName, BookDestName, BookDestCity, BookDestState, BookDestZip, BookTotalCases, BookTotalWgt, " _
            '                         & " BookTotalPL, BookTotalCube, BookTotalPX, BookTotalBFC, BookCarrActDate, BookFinARInvoiceDate, BookODControl, BookCarrierControl, CarrierName, BookTranCode, " _
            '                         & " BookPayCode, BookModDate, BookModUser, BookStopNo, BookRevBilledBFC, BookRevCarrierCost, BookRevOtherCost, BookRevTotalCost, " _
            '                         & " BookMilesFrom , BookRouteConsFlag,BookCarrierContact , BookCarrierContactPhone,BookTypeCode,BookCarrOrderNumber " _
            '                         & " FROM Book, Comp, Carrier " _
            '                         & " WHERE Book.BookCustCompControl = Comp.CompControl and Book.BookCarrierControl = Carrier.CarrierControl " & strComp & " and " & Filters

            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New DataTransferObjects.vLoadStatusBoard
                    With oItem
                        .BookControl = DataTransformation.getDataRowValue(oRow, "BookControl", 0)
                        .BookDateLoad = DataTransformation.getDataRowValue(oRow, "BookDateLoad")
                        .BookDateRequired = DataTransformation.getDataRowValue(oRow, "BookDateRequired")
                        .BookOrigZip = DataTransformation.getDataRowValue(oRow, "BookOrigZip", "")
                        .BookOrigCity = DataTransformation.getDataRowValue(oRow, "BookOrigCity", "")
                        .BookOrigState = DataTransformation.getDataRowValue(oRow, "BookOrigState", "")
                        .BookConsPrefix = DataTransformation.getDataRowValue(oRow, "BookConsPrefix", "")
                        .BookProNumber = DataTransformation.getDataRowValue(oRow, "BookProNumber", "")
                        .BookCustCompControl = DataTransformation.getDataRowValue(oRow, "BookCustCompControl", 0)
                        .CompName = DataTransformation.getDataRowValue(oRow, "CompName", "")
                        .BookOrigName = DataTransformation.getDataRowValue(oRow, "BookOrigName", "")
                        .BookDestName = DataTransformation.getDataRowValue(oRow, "BookDestName", "")
                        .BookDestCity = DataTransformation.getDataRowValue(oRow, "BookDestCity", "")
                        .BookDestState = DataTransformation.getDataRowValue(oRow, "BookDestState", "")
                        .BookDestZip = DataTransformation.getDataRowValue(oRow, "BookDestZip", "")
                        .BookTotalCases = DataTransformation.getDataRowValue(oRow, "BookTotalCases", 0)
                        .BookTotalWgt = DataTransformation.getDataRowValue(oRow, "BookTotalWgt", 0)
                        .BookTotalPL = DataTransformation.getDataRowValue(oRow, "BookTotalPL", 0)
                        .BookTotalCube = DataTransformation.getDataRowValue(oRow, "BookTotalCube", 0)
                        .BookTotalPX = DataTransformation.getDataRowValue(oRow, "BookTotalPX", 0)
                        .BookTotalBFC = DataTransformation.getDataRowValue(oRow, "BookTotalBFC", 0)
                        .BookCarrActDate = DataTransformation.getDataRowValue(oRow, "BookCarrActDate")
                        .BookFinARInvoiceDate = DataTransformation.getDataRowValue(oRow, "BookFinARInvoiceDate")
                        .BookODControl = DataTransformation.getDataRowValue(oRow, "BookODControl", 0)
                        .BookCarrierControl = DataTransformation.getDataRowValue(oRow, "BookCarrierControl", 0)
                        .CarrierName = DataTransformation.getDataRowValue(oRow, "CarrierName", "")
                        .BookTranCode = DataTransformation.getDataRowValue(oRow, "BookTranCode", "")
                        .BookPayCode = DataTransformation.getDataRowValue(oRow, "BookPayCode", "")
                        .BookStopNo = DataTransformation.getDataRowValue(oRow, "BookStopNo", 0)
                        .BookModDate = DataTransformation.getDataRowValue(oRow, "BookModDate")
                        .BookModUser = DataTransformation.getDataRowValue(oRow, "BookModUser", "")
                        .BookRevBilledBFC = DataTransformation.getDataRowValue(oRow, "BookRevBilledBFC", 0)
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookRevCarrierCost = DataTransformation.getDataRowValue(oRow, "BookRevCarrierCost", 0)
                        .BookRevOtherCost = DataTransformation.getDataRowValue(oRow, "BookRevOtherCost", 0)
                        .BookRevTotalCost = DataTransformation.getDataRowValue(oRow, "BookRevTotalCost", 0)
                        .BookMilesFrom = DataTransformation.getDataRowValue(oRow, "BookMilesFrom", 0)
                        .BookRouteConsFlag = DataTransformation.getDataRowValue(oRow, "BookRouteConsFlag", 0)
                        .BookCarrierContact = DataTransformation.getDataRowValue(oRow, "BookCarrierContact", "")
                        .BookCarrierContactPhone = DataTransformation.getDataRowValue(oRow, "BookCarrierContactPhone", "")
                        .BookTypeCode = DataTransformation.getDataRowValue(oRow, "BookTypeCode", "")
                        .BookCarrOrderNumber = DataTransformation.getDataRowValue(oRow, "BookCarrOrderNumber", "")
                        .BookCarrierContControl = DataTransformation.getDataRowValue(oRow, "BookCarrierContControl", 0)
                        .BookOrderSequence = DataTransformation.getDataRowValue(oRow, "BookOrderSequence", 0)
                        .BookOrigAddress1 = DataTransformation.getDataRowValue(oRow, "BookOrigAddress1", "")
                        .BookDestAddress1 = DataTransformation.getDataRowValue(oRow, "BookDestAddress1", "")
                        .CompNumber = DataTransformation.getDataRowValue(oRow, "CompNumber", 0)
                        .CarrierNumber = DataTransformation.getDataRowValue(oRow, "CarrierNumber", 0)
                        .BookLockAllCosts = DataTransformation.getDataRowValue(oRow, "BookLockAllCosts", False)
                        .BookLockBFCCost = DataTransformation.getDataRowValue(oRow, "BookLockBFCCost", False)
                        .BookPickupStopNumber = DataTransformation.getDataRowValue(oRow, "BookPickupStopNumber", 0)
                        .BookOrigStopNumber = DataTransformation.getDataRowValue(oRow, "BookOrigStopNumber", 0)
                        .BookDestStopNumber = DataTransformation.getDataRowValue(oRow, "BookDestStopNumber", 0)
                        .BookOrigMiles = DataTransformation.getDataRowValue(oRow, "BookOrigMiles", 0)
                        .BookDestMiles = DataTransformation.getDataRowValue(oRow, "BookDestMiles", 0)
                        .BookPickNumber = DataTransformation.getDataRowValue(oRow, "BookPickNumber", 0)
                        .BookTransType = DataTransformation.getDataRowValue(oRow, "BookTransType", 0)
                        .BookDateOrdered = DataTransformation.getDataRowValue(oRow, "BookDateOrdered")
                        .BookSHID = DataTransformation.getDataRowValue(oRow, "BookSHID")
                        .BookExpDelDateTime = DataTransformation.getDataRowValue(oRow, "BookExpDelDateTime")
                        .BookMustLeaveByDateTime = DataTransformation.getDataRowValue(oRow, "BookMustLeaveByDateTime")
                        .BookOutOfRouteMiles = DataTransformation.getDataRowValue(oRow, "BookOutOfRouteMiles")
                        .BookSpotRateAllocationFormula = DataTransformation.getDataRowValue(oRow, "BookSpotRateAllocationFormula")
                        .BookSpotRateAutoCalcBFC = DataTransformation.getDataRowValue(oRow, "BookSpotRateAutoCalcBFC")
                        .BookSpotRateUseCarrierFuelAddendum = DataTransformation.getDataRowValue(oRow, "BookSpotRateUseCarrierFuelAddendum")
                        .BookSpotRateBFCAllocationFormula = DataTransformation.getDataRowValue(oRow, "BookSpotRateBFCAllocationFormula")
                        .BookSpotRateTotalUnallocatedBFC = DataTransformation.getDataRowValue(oRow, "BookSpotRateTotalUnallocatedBFC")
                        .BookSpotRateTotalUnallocatedLineHaul = DataTransformation.getDataRowValue(oRow, "BookSpotRateTotalUnallocatedLineHaul")
                        .BookSpotRateUseFuelAddendum = DataTransformation.getDataRowValue(oRow, "BookSpotRateUseFuelAddendum")
                        .BookRevLaneBenchMiles = DataTransformation.getDataRowValue(oRow, "BookRevLaneBenchMiles")
                        .BookRevLoadMiles = DataTransformation.getDataRowValue(oRow, "BookRevLoadMiles")
                        .BookCarrTarControl = DataTransformation.getDataRowValue(oRow, "BookCarrTarControl")
                        .BookCarrTarName = DataTransformation.getDataRowValue(oRow, "BookCarrTarName")
                        .BookCarrTarEquipControl = DataTransformation.getDataRowValue(oRow, "BookCarrTarEquipControl")
                        .BookShipCarrierProControl = DataTransformation.getDataRowValue(oRow, "BookShipCarrierProControl")
                    End With
                    oLoadStatusBoards.Add(oItem)
                Next
            Else
                oLoadStatusBoards.Add(New DataTransferObjects.vLoadStatusBoard)
            End If
            Return oLoadStatusBoards.ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function

    'Public Function GetvLoadStatusBoardsByCNS(Optional ByVal BookConsPrefix As String = "") As DTO.vLoadStatusBoard()
    '    Using db As New NGLMasBookDataContext(ConnectionString)
    '        Try
    '            Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

    '            'db.Log = New DebugTextWriter
    '            'Get the newest record that matches the provided criteria
    '            Dim vLoadStatusBoards() As DTO.vLoadStatusBoard = ( _
    '            From d In db.vLoadStatusBoards _
    '            Where _
    '                (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix = BookConsPrefix) _
    '                And _
    '                (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl)) _
    '            Order By d.BookConsPrefix Descending, d.BookStopNo _
    '            Select New DTO.vLoadStatusBoard With {.BookControl = d.BookControl _
    '                                        , .BookProNumber = d.BookProNumber _
    '                                        , .BookDateLoad = d.BookDateLoad _
    '                                        , .BookDateRequired = d.BookDateRequired _
    '                                        , .BookConsPrefix = d.BookConsPrefix _
    '                                        , .BookCustCompControl = d.BookCustCompControl _
    '                                        , .CompName = d.CompName _
    '                                        , .CompNumber = d.CompNumber _
    '                                        , .BookODControl = d.BookODControl _
    '                                        , .BookCarrierControl = d.BookCarrierControl _
    '                                        , .CarrierName = d.CarrierName _
    '                                        , .CarrierNumber = d.CarrierNumber _
    '                                        , .BookCarrierContControl = d.BookCarrierContControl _
    '                                        , .BookCarrierContact = d.BookCarrierContact _
    '                                        , .BookCarrierContactPhone = d.BookCarrierContactPhone _
    '                                        , .BookOrigName = d.BookOrigName _
    '                                        , .BookOrigAddress1 = d.BookOrigAddress1 _
    '                                        , .BookOrigCity = d.BookOrigCity _
    '                                        , .BookOrigState = d.BookOrigState _
    '                                        , .BookOrigZip = d.BookOrigZip _
    '                                        , .BookDestName = d.BookDestName _
    '                                        , .BookDestAddress1 = d.BookDestAddress1 _
    '                                        , .BookDestCity = d.BookDestCity _
    '                                        , .BookDestState = d.BookDestState _
    '                                        , .BookDestZip = d.BookDestZip _
    '                                        , .BookTotalCases = If(d.BookTotalCases.HasValue, d.BookTotalCases, 0) _
    '                                        , .BookTotalWgt = If(d.BookTotalWgt.HasValue, d.BookTotalWgt, 0) _
    '                                        , .BookTotalPL = If(d.BookTotalPL.HasValue, d.BookTotalPL, 0) _
    '                                        , .BookTotalCube = If(d.BookTotalCube.HasValue, d.BookTotalCube, 0) _
    '                                        , .BookTotalPX = If(d.BookTotalPX.HasValue, d.BookTotalPX, 0) _
    '                                        , .BookTotalBFC = If(d.BookTotalBFC.HasValue, d.BookTotalBFC, 0) _
    '                                        , .BookCarrActDate = d.BookCarrActDate _
    '                                        , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
    '                                        , .BookTranCode = d.BookTranCode _
    '                                        , .BookPayCode = d.BookPayCode _
    '                                        , .BookTypeCode = d.BookTypeCode _
    '                                        , .BookStopNo = If(d.BookStopNo.HasValue, d.BookStopNo, 0) _
    '                                        , .BookModDate = d.BookModDate _
    '                                        , .BookModUser = d.BookModUser _
    '                                        , .BookRevBilledBFC = If(d.BookRevBilledBFC.HasValue, d.BookRevBilledBFC, 0) _
    '                                        , .BookRevCarrierCost = If(d.BookRevCarrierCost.HasValue, d.BookRevCarrierCost, 0) _
    '                                        , .BookRevOtherCost = If(d.BookRevOtherCost.HasValue, d.BookRevOtherCost, 0) _
    '                                        , .BookRevTotalCost = If(d.BookRevTotalCost.HasValue, d.BookRevTotalCost, 0) _
    '                                        , .BookMilesFrom = If(d.BookMilesFrom.HasValue, d.BookMilesFrom, 0) _
    '                                        , .BookRouteConsFlag = d.BookRouteConsFlag _
    '                                        , .BookCarrOrderNumber = d.BookCarrOrderNumber _
    '                                        , .BookOrderSequence = d.BookOrderSequence _
    '                                        , .BookLockAllCosts = d.BookLockAllCosts _
    '                                        , .BookLockBFCCost = d.BookLockBFCCost _
    '                                        , .BookPickupStopNumber = d.BookPickupStopNumber _
    '                                        , .BookOrigStopNumber = d.BookOrigStopNumber _
    '                                        , .BookDestStopNumber = d.BookDestStopNumber _
    '                                        , .BookOrigMiles = d.BookOrigMiles _
    '                                        , .BookDestMiles = d.BookDestMiles _
    '                                        , .BookPickNumber = d.BookPickNumber _
    '                                        , .BookDateOrdered = d.BookDateOrdered _
    '                                        , .BookTransType = d.BookTransType}).Take(500).ToArray()

    '            Return vLoadStatusBoards

    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return Nothing

    '    End Using
    'End Function

    Public Function GetvLoadStatusBoardsByCNS(Optional ByVal BookConsPrefix As String = "") As DataTransferObjects.vLoadStatusBoard()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Return (
                    From d In db.vLoadStatusBoards
                        Where
                            (BookConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookConsPrefix) OrElse d.BookConsPrefix = BookConsPrefix) _
                            And
                            (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.BookCustCompControl))
                        Order By d.BookConsPrefix Descending, d.BookStopNo
                        Select selectDTOData(d, db)).Take(500).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvLoadStatusBoardsByCNS"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated vLoadStatusBoard record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Dim source As DataTransferObjects.vLoadStatusBoard = TryCast(oData, DataTransferObjects.vLoadStatusBoard)
        If source Is Nothing Then Return Nothing
        SaveChanges(oData)
        Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(source.BookControl, 0)
        Me.LastProcedureName = "spUpdateBookDependencies"
        Return Nothing
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs and return the updated QuickSaveResults record
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DataTransferObjects.QuickSaveResults
        Return Update(oData, oLinqTable)
    End Function

    ''' <summary>
    ''' This override does not call the standard cleanup methods and returns nothing.
    ''' The caller must check the BookDependencyResult properties (ReturnBookControl,MustRecalculate,RetMsg, and ErrNumber) 
    ''' , recalculate costs 
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Update(oData, oLinqTable)
    End Sub

#End Region

#Region "Shared Methods"

    Friend Shared Function selectDTOData(ByVal d As LTS.vLoadStatusBoard, ByRef db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.vLoadStatusBoard
        Dim oDTO As New DataTransferObjects.vLoadStatusBoard
        Dim skipObjs As New List(Of String) From {"CarrierName",
                "CarrierNumber",
                "CompName",
                "CompNumber",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Sub updateLTSWithDTO(ByRef d As DataTransferObjects.vLoadStatusBoard, ByRef l As LTS.vLoadStatusBoard, ByVal UserName As String)

        Dim skipObjs As New List(Of String) From {"CarrierName",
                "CarrierNumber",
                "CompName",
                "CompNumber",
                "BookModDate",
                "BookModUser",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        l = CopyMatchingFields(l, d, skipObjs)
        'add custom formatting
        With l
            .BookModDate = Date.Now()
            .BookModUser = UserName
        End With
    End Sub

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.vLoadStatusBoard, ByVal UserName As String) As LTS.vLoadStatusBoard
        Dim oLTS As New LTS.vLoadStatusBoard
        updateLTSWithDTO(d, oLTS, UserName)
        Return oLTS
    End Function

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim source As LTS.vLoadStatusBoard = TryCast(LinqTable, LTS.vLoadStatusBoard)
        If source Is Nothing Then Return Nothing
        Return GetvLoadStatusBoardFiltered(Control:=source.BookControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.vLoadStatusBoard = TryCast(LinqTable, LTS.vLoadStatusBoard)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.BookControl)
    End Function

    Public Function QuickSaveResults(ByVal BookControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                ret = (From d In db.vLoadStatusBoards
                    Where d.BookControl = BookControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookControl _
                        , .ModDate = d.BookModDate _
                        , .ModUser = d.BookModUser}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    'Private Sub SaveChanges(ByVal oData As DTO.vLoadStatusBoard)
    '    Using LinqDB
    '        'Note: the ValidateData Function must throw a FaultException error on failure
    '        ValidateUpdatedRecord(LinqDB, oData)
    '        With oData
    '            Try
    '                'Open the existing Record
    '                Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).vLoadStatusBoards Where e.BookControl = .BookControl Select e).First
    '                If d Is Nothing Then
    '                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
    '                Else
    '                    'Check for conflicts
    '                    If .BookModDate <> d.BookModDate Then
    '                        'the data may have changed so check each field for conflicts
    '                        Dim ConflictData As New List(Of KeyValuePair(Of String, String))
    '                        Dim blnConflictFound As Boolean = False
    '                        addToConflicts("BookProNumber", .BookProNumber, d.BookProNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDateLoad", .BookDateLoad, d.BookDateLoad, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDateRequired", .BookDateRequired, d.BookDateRequired, ConflictData, blnConflictFound)
    '                        addToConflicts("BookConsPrefix", .BookConsPrefix, d.BookConsPrefix, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCustCompControl", .BookCustCompControl, d.BookCustCompControl, ConflictData, blnConflictFound)
    '                        addToConflicts("BookODControl", .BookODControl, d.BookODControl, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrierControl", .BookCarrierControl, d.BookCarrierControl, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrierContControl", .BookCarrierContControl, d.BookCarrierContControl, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrierContact", .BookCarrierContact, d.BookCarrierContact, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrierContactPhone", .BookCarrierContactPhone, d.BookCarrierContactPhone, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigName", .BookOrigName, d.BookOrigName, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigAddress1", .BookOrigAddress1, d.BookOrigAddress1, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigCity", .BookOrigCity, d.BookOrigCity, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigState", .BookOrigState, d.BookOrigState, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigZip", .BookOrigZip, d.BookOrigZip, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestName", .BookDestName, d.BookDestName, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestAddress1", .BookDestAddress1, d.BookDestAddress1, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestCity", .BookDestCity, d.BookDestCity, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestState", .BookDestState, d.BookDestState, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestZip", .BookDestZip, d.BookDestZip, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalCases", .BookTotalCases, d.BookTotalCases, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalWgt", .BookTotalWgt, d.BookTotalWgt, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalPL", .BookTotalPL, d.BookTotalPL, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalCube", .BookTotalCube, d.BookTotalCube, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalPX", .BookTotalPX, d.BookTotalPX, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTotalBFC", .BookTotalBFC, d.BookTotalBFC, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrActDate", .BookCarrActDate, d.BookCarrActDate, ConflictData, blnConflictFound)
    '                        addToConflicts("BookFinARInvoiceDate", .BookFinARInvoiceDate, d.BookFinARInvoiceDate, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTranCode", .BookTranCode, d.BookTranCode, ConflictData, blnConflictFound)
    '                        addToConflicts("BookPayCode", .BookPayCode, d.BookPayCode, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTypeCode", .BookTypeCode, d.BookTypeCode, ConflictData, blnConflictFound)
    '                        addToConflicts("BookStopNo", .BookStopNo, d.BookStopNo, ConflictData, blnConflictFound)
    '                        addToConflicts("BookRevBilledBFC", .BookRevBilledBFC, d.BookRevBilledBFC, ConflictData, blnConflictFound)
    '                        addToConflicts("BookRevCarrierCost", .BookRevCarrierCost, d.BookRevCarrierCost, ConflictData, blnConflictFound)
    '                        addToConflicts("BookRevOtherCost", .BookRevOtherCost, d.BookRevOtherCost, ConflictData, blnConflictFound)
    '                        addToConflicts("BookRevTotalCost", .BookRevTotalCost, d.BookRevTotalCost, ConflictData, blnConflictFound)
    '                        addToConflicts("BookMilesFrom", .BookMilesFrom, d.BookMilesFrom, ConflictData, blnConflictFound)
    '                        addToConflicts("BookRouteConsFlag", .BookRouteConsFlag, d.BookRouteConsFlag, ConflictData, blnConflictFound)
    '                        addToConflicts("BookCarrOrderNumber", .BookCarrOrderNumber, d.BookCarrOrderNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrderSequence", .BookOrderSequence, d.BookOrderSequence, ConflictData, blnConflictFound)
    '                        addToConflicts("BookMilesFrom", .BookMilesFrom, d.BookMilesFrom, ConflictData, blnConflictFound)
    '                        addToConflicts("BookLockAllCosts", .BookLockAllCosts, d.BookLockAllCosts, ConflictData, blnConflictFound)
    '                        addToConflicts("BookLockBFCCost", .BookLockBFCCost, d.BookLockBFCCost, ConflictData, blnConflictFound)
    '                        addToConflicts("BookPickupStopNumber", .BookPickupStopNumber, d.BookPickupStopNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigStopNumber", .BookOrigStopNumber, d.BookOrigStopNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestStopNumber", .BookDestStopNumber, d.BookDestStopNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookOrigMiles", .BookOrigMiles, d.BookOrigMiles, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDestMiles", .BookDestMiles, d.BookDestMiles, ConflictData, blnConflictFound)
    '                        addToConflicts("BookPickNumber", .BookPickNumber, d.BookPickNumber, ConflictData, blnConflictFound)
    '                        addToConflicts("BookTransType", .BookTransType, d.BookTransType, ConflictData, blnConflictFound)
    '                        addToConflicts("BookDateOrdered", .BookDateOrdered, d.BookDateOrdered, ConflictData, blnConflictFound)


    '                        If blnConflictFound Then
    '                            'We only add the mod date and mod user if one or more other fields have been modified
    '                            addToConflicts("BookModDate", .BookModDate, d.BookModDate, ConflictData, blnConflictFound)
    '                            addToConflicts("BookModUser", .BookModUser, d.BookModUser, ConflictData, blnConflictFound)
    '                            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
    '                            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
    '                            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
    '                        End If
    '                    End If
    '                    'Update the table data
    '                    d.BookProNumber = .BookProNumber
    '                    d.BookDateLoad = .BookDateLoad
    '                    d.BookDateRequired = .BookDateRequired
    '                    d.BookConsPrefix = .BookConsPrefix
    '                    d.BookCustCompControl = .BookCustCompControl
    '                    d.BookODControl = .BookODControl
    '                    d.BookCarrierControl = .BookCarrierControl
    '                    d.BookCarrierContControl = .BookCarrierContControl
    '                    d.BookCarrierContact = .BookCarrierContact
    '                    d.BookCarrierContactPhone = .BookCarrierContactPhone
    '                    d.BookOrigName = .BookOrigName
    '                    d.BookOrigAddress1 = .BookOrigAddress1
    '                    d.BookOrigCity = .BookOrigCity
    '                    d.BookOrigState = .BookOrigState
    '                    d.BookOrigZip = .BookOrigZip
    '                    d.BookDestName = .BookDestName
    '                    d.BookDestAddress1 = .BookDestAddress1
    '                    d.BookDestCity = .BookDestCity
    '                    d.BookDestState = .BookDestState
    '                    d.BookDestZip = .BookDestZip
    '                    d.BookTotalCases = .BookTotalCases
    '                    d.BookTotalWgt = .BookTotalWgt
    '                    d.BookTotalPL = .BookTotalPL
    '                    d.BookTotalCube = .BookTotalCube
    '                    d.BookTotalPX = .BookTotalPX
    '                    d.BookTotalBFC = .BookTotalBFC
    '                    d.BookCarrActDate = .BookCarrActDate
    '                    d.BookFinARInvoiceDate = .BookFinARInvoiceDate
    '                    d.BookTranCode = .BookTranCode
    '                    d.BookPayCode = .BookPayCode
    '                    d.BookTypeCode = .BookTypeCode
    '                    d.BookOrigStopNumber = If(.BookOrigStopNumber < 1, 1, .BookOrigStopNumber)
    '                    d.BookDestStopNumber = calculateBookDestStopNumber(d.BookStopNo, .BookStopNo, d.BookDestStopNumber)
    '                    d.BookStopNo = If(.BookStopNo < 1, 1, .BookStopNo)
    '                    d.BookRevBilledBFC = .BookRevBilledBFC
    '                    d.BookRevCarrierCost = .BookRevCarrierCost
    '                    d.BookRevOtherCost = .BookRevOtherCost
    '                    d.BookRevTotalCost = .BookRevTotalCost
    '                    d.BookMilesFrom = .BookMilesFrom
    '                    d.BookRouteConsFlag = .BookRouteConsFlag
    '                    d.BookCarrOrderNumber = .BookCarrOrderNumber
    '                    d.BookOrderSequence = .BookOrderSequence
    '                    d.BookLockAllCosts = .BookLockAllCosts
    '                    d.BookLockBFCCost = .BookLockBFCCost
    '                    d.BookPickupStopNumber = If(.BookPickupStopNumber < 1, 1, .BookPickupStopNumber)
    '                    d.BookOrigMiles = .BookOrigMiles
    '                    d.BookDestMiles = .BookDestMiles
    '                    d.BookPickNumber = If(.BookPickNumber < 1, 1, .BookPickNumber)
    '                    d.BookTransType = .BookTransType
    '                    d.BookDateOrdered = .BookDateOrdered
    '                    'update the mod date and mod user                       
    '                    d.BookModDate = Date.Now
    '                    d.BookModUser = Me.Parameters.UserName
    '                End If
    '                LinqDB.SubmitChanges()
    '            Catch ex As FaultException
    '                Throw
    '            Catch ex As SqlException
    '                Utilities.SaveAppError(ex.Message, Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
    '            Catch conflictEx As ChangeConflictException
    '                Try
    '                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
    '                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
    '                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
    '                Catch ex As FaultException
    '                    Throw
    '                Catch ex As Exception
    '                    Utilities.SaveAppError(ex.Message, Me.Parameters)
    '                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
    '                End Try
    '            Catch ex As InvalidOperationException
    '                Utilities.SaveAppError(ex.Message, Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
    '            Catch ex As Exception
    '                Utilities.SaveAppError(ex.Message, Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
    '            End Try

    '        End With
    '    End Using
    'End Sub

    Private Sub SaveChanges(ByVal oData As DataTransferObjects.vLoadStatusBoard)
        If Not oData Is Nothing Then
            Using LinqDB
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                Try
                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).vLoadStatusBoards Where e.BookControl = oData.BookControl Select e).FirstOrDefault()
                    If d Is Nothing OrElse d.BookControl = 0 Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, oData.BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            'Modified by LVV 10/10/14 we now use reflection via CheckForDataConflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim oSkip As New List(Of String) From {"CarrierName",
                                    "CarrierNumber",
                                    "CompanyName",
                                    "CompanyNumber",
                                    "BookModDate",
                                    "BookModUser"}
                            Dim blnConflictFound As Boolean = CheckForDataConflicts(oData, d, oSkip, ConflictData)
                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", oData.BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", oData.BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        'Update the table data
                        updateLTSWithDTO(oData, d, Me.Parameters.UserName)
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SaveChanges"))
                End Try
            End Using
        End If
    End Sub

#End Region

End Class