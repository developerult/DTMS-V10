Imports System.Data.Linq
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.FreightMaster.Data.Utilities
Imports System.Linq.Dynamic
Imports P44 = Ngl.FM.P44

Imports LTTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum
Imports LTSCEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum
Imports BidTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum
Imports BSCEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidStatusCodeEnum
Imports CHR = Ngl.FM.CHRAPI
Imports UPS = Ngl.FM.UPSAPI
Imports JTS = Ngl.FM.JTSAPI
Imports Map = Ngl.API.Mapping
Imports SerilogTracing
Imports Ngl.FreightMaster.Data.LTS
'Class Added by LVV 5/19/16 for v-7.0.5.110 DAT
Public Class NGLLoadTenderData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        'Me.LinqTable = db.tblLoadTenders
        'Me.LinqDB = db
        Me.SourceClass = "NGLLoadTenderData"
    End Sub

#End Region

#Region "Delegates"
    'Modified by RHR for v-7.0.5.103 on 02/19/2017
    Public Delegate Sub CreateNP44RateRequestDelegate(ByRef oP44Proxy As P44.P44Proxy, ByVal oP44Data As P44.RateRequest, ByVal LoadTenderControl As Integer)
    'Public Delegate Sub CreateP44RateRequestNoBookDelegate(ByVal oP44Request As P44.RateRequest, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
    'Public Delegate Sub CreateP44RateRequestOrderDelegate(ByVal order As DAL.Models.RateRequestOrder, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)

#End Region


#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASIntegrationDataContext(ConnectionString)
                _LinqTable = db.tblLoadTenders
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

#Region "DEPRECIATED"

    ''' <summary>
    ''' DEPRECIATED
    ''' Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
    ''' Next Stop Posted Loads that have not yet received any bids from the provided Carrier
    ''' Basically, all the available loads this Carrier does not have any active bids on
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 2/6/17 for v-8.0 Next Stop
    ''' DEPRECIATED By LVV on 10/16/18
    ''' </remarks>
    Public Function GetvNSCarrAvailableLoads(ByRef RecordCount As Integer,
                                         ByVal CarrierControl As Integer,
                                         Optional ByVal filterWhere As String = "",
                                         Optional ByVal sortExpression As String = "",
                                         Optional ByVal page As Integer = 1,
                                         Optional ByVal pagesize As Integer = 1000,
                                         Optional ByVal skip As Integer = 0,
                                         Optional ByVal take As Integer = 0) As LTS.vNSAvailablePendingLoad()
        throwDepreciatedException("This version of " & buildProcedureName("GetvNSCarrAvailableLoads") & " has been depreciated. Please use the AllFilters overload.")
        Return Nothing
    End Function

    ''' <summary>
    ''' DEPRECIATED
    ''' Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
    ''' Next Stop Posted Loads that have not yet received any bids from any Carrier
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 2/6/17 for v-8.0 Next Stop
    ''' DEPRECIATED By LVV on 10/16/18
    ''' </remarks>
    Public Function GetvNSOpsPendingLoads(ByRef RecordCount As Integer,
                                         Optional ByVal filterWhere As String = "",
                                         Optional ByVal sortExpression As String = "",
                                         Optional ByVal page As Integer = 1,
                                         Optional ByVal pagesize As Integer = 1000,
                                         Optional ByVal skip As Integer = 0,
                                         Optional ByVal take As Integer = 0) As LTS.vNSAvailablePendingLoad()
        throwDepreciatedException("This version of " & buildProcedureName("GetvNSOpsPendingLoads") & " has been depreciated. Please use the AllFilters overload.")
        Return Nothing
    End Function

    ''' <summary>
    ''' DEPRECIATED
    ''' Returns an array of LTS.vNSLoadsWActiveBid data objects representing active 
    ''' Next Stop Posted Loads that have received at least one bid
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns>LTS.vNSLoadsWActiveBid()</returns>
    ''' <remarks>
    ''' Added By LVV on 1/9/17 for v-8.0 Next Stop
    ''' DEPRECIATED By LVV on 11/6/18
    ''' </remarks>
    Public Function GetvNSLoadsWActiveBid(ByRef RecordCount As Integer,
                                          Optional ByVal filterWhere As String = "",
                                          Optional ByVal sortExpression As String = "",
                                          Optional ByVal page As Integer = 1,
                                          Optional ByVal pagesize As Integer = 1000,
                                          Optional ByVal skip As Integer = 0,
                                          Optional ByVal take As Integer = 0) As LTS.vNSLoadsWActiveBid()
        throwDepreciatedException("This version of " & buildProcedureName("GetvNSLoadsWActiveBid") & " has been depreciated. Please use the AllFilters overload.")
        Return Nothing
    End Function

    ''''' <summary>
    ''''' Returns an array of LTS.vNSLoadsWActiveBid data objects representing active 
    ''''' Next Stop Posted Loads that have received at least one bid
    ''''' </summary>
    ''''' <param name="RecordCount"></param>
    ''''' <param name="sortExpression"></param>
    ''''' <param name="page"></param>
    ''''' <param name="pagesize"></param>
    ''''' <param name="skip"></param>
    ''''' <param name="take"></param>
    ''''' <returns>LTS.vNSLoadsWActiveBid()</returns>
    ''''' <remarks>
    ''''' Added By LVV on 1/9/17 for v-8.0 Next Stop
    ''''' </remarks>
    ''Public Function GetvNSLoadsWActiveBid(ByRef RecordCount As Integer,
    ''                                      Optional ByVal filterWhere As String = "",
    ''                                      Optional ByVal sortExpression As String = "",
    ''                                      Optional ByVal page As Integer = 1,
    ''                                      Optional ByVal pagesize As Integer = 1000,
    ''                                      Optional ByVal skip As Integer = 0,
    ''                                      Optional ByVal take As Integer = 0) As LTS.vNSLoadsWActiveBid()
    ''    Dim oRetData As LTS.vNSLoadsWActiveBid()
    ''    Using db As New NGLMASIntegrationDataContext(ConnectionString)
    ''        Try
    ''            Dim intPageCount As Integer = 1
    ''            Dim oQuery = From t In db.vNSLoadsWActiveBids Select t
    ''            '"(CarrTarDiscountMinValue < 75) And (CarrTarDiscountWgtLimit > 50)"
    ''            '"(BidStatus = 1) And (BidCarrierControl = CarrierControl)"
    ''            If oQuery Is Nothing Then Return Nothing
    ''            If Not String.IsNullOrEmpty(filterWhere) Then
    ''                oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
    ''            End If
    ''            RecordCount = oQuery.Count()
    ''            If RecordCount < 1 Then Return Nothing
    ''            If take <> 0 Then
    ''                pagesize = take
    ''            Else
    ''                'calculate based on page and pagesize
    ''                If pagesize < 1 Then pagesize = 1
    ''                If RecordCount < 1 Then RecordCount = 1
    ''                If page < 1 Then page = 1
    ''                skip = (page - 1) * pagesize
    ''            End If
    ''            If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
    ''            oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()
    ''            Return oRetData
    ''        Catch ex As Exception
    ''            ManageLinqDataExceptions(ex, buildProcedureName("GetvNSLoadsWActiveBid"))
    ''        End Try
    ''        Return Nothing
    ''    End Using
    ''End Function

    ''' <summary>
    ''' DEPRECIATED
    ''' Returns an array of LTS.tblLoadTender objects where LTType is NEXTStop
    ''' and Archived is true.
    ''' Additional filtering may be applied through the filterWhere parameter
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' DEPRECIATED By LVV on 11/7/18
    ''' </remarks>
    Public Function GetNSHisoricalLoads(ByRef RecordCount As Integer,
                                     Optional ByVal filterWhere As String = "",
                                      Optional ByVal sortExpression As String = "",
                                      Optional ByVal page As Integer = 1,
                                      Optional ByVal pagesize As Integer = 1000,
                                      Optional ByVal skip As Integer = 0,
                                      Optional ByVal take As Integer = 0) As LTS.tblLoadTender()
        throwDepreciatedException("This version of " & buildProcedureName("GetNSHisoricalLoads") & " has been depreciated. Please use the AllFilters overload.")
        Return Nothing
    End Function

    ''''' <summary>
    ''''' Returns an array of LTS.tblLoadTender objects where LTType is NEXTStop
    ''''' and Archived is true.
    ''''' Additional filtering may be applied through the filterWhere parameter
    ''''' </summary>
    ''''' <param name="RecordCount"></param>
    ''''' <param name="filterWhere"></param>
    ''''' <param name="sortExpression"></param>
    ''''' <param name="page"></param>
    ''''' <param name="pagesize"></param>
    ''''' <param name="skip"></param>
    ''''' <param name="take"></param>
    ''''' <returns></returns>
    ''Public Function GetNSHisoricalLoads(ByRef RecordCount As Integer,
    ''                                 Optional ByVal filterWhere As String = "",
    ''                                  Optional ByVal sortExpression As String = "",
    ''                                  Optional ByVal page As Integer = 1,
    ''                                  Optional ByVal pagesize As Integer = 1000,
    ''                                  Optional ByVal skip As Integer = 0,
    ''                                  Optional ByVal take As Integer = 0) As LTS.tblLoadTender()
    ''    Dim oRetData As LTS.tblLoadTender()
    ''    Using db As New NGLMASIntegrationDataContext(ConnectionString)
    ''        Try
    ''            Dim intPageCount As Integer = 1
    ''            Dim oQuery = From t In db.tblLoadTenders
    ''                         Where (t.LTLoadTenderTypeControl = LTTypeEnum.NextStop AndAlso t.LTArchived = True)
    ''                         Select t
    ''            '"(CarrTarDiscountMinValue < 75) And (CarrTarDiscountWgtLimit > 50)"
    ''            '"(BidStatus = 1) And (BidCarrierControl = CarrierControl)"
    ''            If oQuery Is Nothing Then Return Nothing
    ''            If Not String.IsNullOrEmpty(filterWhere) Then
    ''                oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
    ''            End If
    ''            RecordCount = oQuery.Count()
    ''            If RecordCount < 1 Then Return Nothing
    ''            If take <> 0 Then
    ''                pagesize = take
    ''            Else
    ''                'calculate based on page and pagesize
    ''                If pagesize < 1 Then pagesize = 1
    ''                If RecordCount < 1 Then RecordCount = 1
    ''                If page < 1 Then page = 1
    ''                skip = (page - 1) * pagesize
    ''            End If
    ''            If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
    ''            oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()
    ''            Return oRetData
    ''        Catch ex As Exception
    ''            ManageLinqDataExceptions(ex, buildProcedureName("GetNSHisoricalLoads"))
    ''        End Try
    ''        Return Nothing
    ''    End Using
    ''End Function

#End Region

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetLoadTenderFiltered(LoadTenderControl:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLoadTenders()
    End Function

    ''' <summary>
    ''' Gets the tblLoadTender record where LoadTenderControl = Control if it is provided
    ''' Else returns the first record 
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <returns>DTO.tblLoadTender</returns>
    ''' <remarks>
    ''' Added by LVV 5/19/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLoadTenderFiltered(Optional ByVal LoadTenderControl As Integer = 0) As DTO.tblLoadTender
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LoadTender As DTO.tblLoadTender = (
                From d In db.tblLoadTenders
                Where
                    (d.LoadTenderControl = If(LoadTenderControl = 0, d.LoadTenderControl, LoadTenderControl))
                Select selectDTOData(d, db)).FirstOrDefault()
                Return LoadTender
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenderFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns the most recent record for the provided BookControl
    ''' (Selects the First record ordered by LoadTenderControl desc)
    ''' Option to additionally filter by LoadTenderType
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns>DTO.tblLoadTender</returns>
    ''' <remarks>
    ''' Added by LVV 5/19/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLoadTenderFiltered(Optional ByVal BookSHID As String = "",
                                          Optional ByVal BookControl As Integer = 0,
                                          Optional ByVal intLoadTenderType As Integer = 0,
                                          Optional ByVal intStatusCode As Integer = 0,
                                          Optional ByVal Archived As Boolean? = Nothing) As DTO.tblLoadTender
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LoadTender As DTO.tblLoadTender = (
                From d In db.tblLoadTenders
                Where
                    (d.LTBookSHID = If(String.IsNullOrWhiteSpace(BookSHID), d.LTBookSHID, BookSHID)) _
                    And
                    (d.LTBookControl = If(BookControl = 0, d.LTBookControl, BookControl)) _
                    And
                    (d.LTLoadTenderTypeControl = If(intLoadTenderType = 0, d.LTLoadTenderTypeControl, intLoadTenderType)) _
                    And
                    (d.LTStatusCode = If(intStatusCode = 0, d.LTStatusCode, intStatusCode)) _
                    And
                    (d.LTArchived = If(Archived Is Nothing, d.LTArchived, Archived))
                Order By d.LoadTenderControl Descending
                Select selectDTOData(d, db)).FirstOrDefault()
                Return LoadTender
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenderFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets historical quotes
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 10/24/2018 for v-8.3
    '''     Fixed the user/company security on Rate shopping so quotes can be shared by multiple users.
    '''     Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
    ''' Modified by RHR for v-8.5.4.002 on 07/19/2023 added rule to filter by LTPosterUserControl when access is restricted 
    '''     on the Rate Shop Q page
    ''' </remarks>
    Public Function GetHistoricalQuotes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vHistoricalQuote()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vHistoricalQuote
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
                Dim blnRestrict As Boolean = False
                If (Sec.isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
                    blnRestrict = True
                End If
                'Modified by LVV on 10/24/2018 for v-8.3 -- Fixed the user/company security on Rate shopping so quotes can be shared by multiple users. Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
                'Get the USC of users assigned to the logged in users LE and add them to the filters list
                Dim flist = filters.FilterValues.ToList()
                Dim users As Integer()
                'Modified by RHR for v-8.5.4.002 on 07/19/2023
                If blnRestrict Then
                    users = New Integer() {Parameters.UserControl}
                Else
                    users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                End If

                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vHistoricalQuote)
                iQuery = db.vHistoricalQuotes
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                'Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ") AND (LTPosterUserControl = " & Me.Parameters.UserControl & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetHistoricalQuotes"))
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Gets historical quotes
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 10/24/2018 for v-8.3
    '''     Fixed the user/company security on Rate shopping so quotes can be shared by multiple users.
    '''     Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
    ''' Modified by RHR for v-8.5.4.002 on 07/19/2023 added rule to filter by LTPosterUserControl when access is restricted 
    '''     on the Rate Shop Q page
    ''' </remarks>
    Public Function GetExportBids(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vExportBid()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vExportBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter

                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
                Dim blnRestrict As Boolean = False
                If (Sec.isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
                    blnRestrict = True
                End If
                'Modified by LVV on 10/24/2018 for v-8.3 -- Fixed the user/company security on Rate shopping so quotes can be shared by multiple users. Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
                'Get the USC of users assigned to the logged in users LE and add them to the filters list
                Dim flist = filters.FilterValues.ToList()
                Dim users As Integer()
                'Modified by RHR for v-8.5.4.002 on 07/19/2023
                If blnRestrict Then
                    users = New Integer() {Parameters.UserControl}
                Else
                    users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                End If

                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vExportBid)
                iQuery = db.vExportBids.Where(Function(x) x.BidSelectedForExport = True)  '  this filter is included in the view
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportBids"))
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetExportQuoteBids(ByVal iLoadTenderControl) As LTS.vExportBid()
        Dim filters As New Models.AllFilters()
        filters.take = 1000
        Dim RecordCount As Integer = 1000
        Dim oRet() As LTS.vExportBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter

                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
                Dim blnRestrict As Boolean = False
                If (Sec.isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
                    blnRestrict = True
                End If
                'Modified by LVV on 10/24/2018 for v-8.3 -- Fixed the user/company security on Rate shopping so quotes can be shared by multiple users. Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
                'Get the USC of users assigned to the logged in users LE and add them to the filters list
                Dim flist = filters.FilterValues.ToList()
                Dim users As Integer()
                'Modified by RHR for v-8.5.4.002 on 07/19/2023
                If blnRestrict Then
                    users = New Integer() {Parameters.UserControl}
                Else
                    users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                End If

                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                flist.Add(New Models.FilterDetails With {.filterName = "LoadTenderControl", .filterValueFrom = iLoadTenderControl})

                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vExportBid)
                iQuery = db.vExportBids  '.Where(Function(x) x.LoadTenderControl = iLoadTenderControl)
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetExportQuoteBids"))
            End Try
        End Using
        Return oRet
    End Function




    ''' <summary>
    ''' Get a list of LoadTender records by user(s) where the Export flag is true.
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 07/19/2023
    ''' </remarks>
    Public Function GetFilteredSelectedHistoricalQuotes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vHistoricalQuote()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vHistoricalQuote
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
                Dim blnRestrict As Boolean = False
                If (Sec.isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
                    blnRestrict = True
                End If
                Dim flist = filters.FilterValues.ToList()
                Dim users As Integer()
                If blnRestrict Then
                    users = New Integer() {Parameters.UserControl}
                Else
                    users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                End If

                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vHistoricalQuote)
                iQuery = db.vHistoricalQuotes.Where(Function(x) x.LTSelectedForExport = 1)
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                'Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ") AND (LTPosterUserControl = " & Me.Parameters.UserControl & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFilteredSelectedHistoricalQuotes"))
            End Try
        End Using
        Return oRet
    End Function



    ''' <summary>
    ''' Update the export flag for all of the filtered loads/bids for the current user or group of LE users based on restriction logic
    ''' Sales Rep users are restricted by default
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <param name="blnSelected"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 07/19/2023
    ''' </remarks>
    Public Function UpdateAllBidSelectedForExportForAllHistoricalQuotes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, ByVal blnSelected As Boolean) As Boolean
        Dim blnRet As Boolean = False
        If filters Is Nothing Then Return False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
                Dim blnRestrict As Boolean = False
                If (Sec.isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
                    blnRestrict = True
                End If
                Dim flist = filters.FilterValues.ToList()
                Dim users As Integer()
                If blnRestrict Then
                    users = New Integer() {Parameters.UserControl}
                Else
                    users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                End If
                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vHistoricalQuote)
                iQuery = db.vHistoricalQuotes
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                'Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ") AND (LTPosterUserControl = " & Me.Parameters.UserControl & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                Dim oData() As LTS.vHistoricalQuote = iQuery.ToArray()
                If (Not oData Is Nothing AndAlso oData.Count() > 0) Then
                    For Each d As LTS.vHistoricalQuote In oData
                        db.spUpdateAllLoadTenderBidExportFlags(d.LoadTenderControl, blnSelected)
                    Next
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateAllBidSelectedForExportForAllHistoricalQuotes"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Update the export flag for all the bids assigned to iLoadTenderControl key using blnSelected flag
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="blnSelected"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 07/19/2023
    ''' </remarks>
    Public Function UpdateAllBidSelectedForExportForHistoricalQuote(ByVal iLoadTenderControl As Integer, ByVal blnSelected As Boolean) As Boolean
        Dim blnRet As Boolean = False
        If iLoadTenderControl = 0 Then Return False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                db.spUpdateAllLoadTenderBidExportFlags(iLoadTenderControl, blnSelected)
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateAllBidSelectedForExportForHistoricalQuote"))
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' returns an array of API dispatched loads filterd by the users LECompControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 03/03/2018
    '''     limited functionality used by rate shopping,  the LECompControl user filter may not work as expected when bound to booking records?
    '''     because the actual bookcustcompcontrol number may be different than the LECompControl number.  
    '''     the All page should be used to look up the actual order.
    ''' Modified by LVV on 10/24/2018 for v-8.3
    '''     Fixed the user/company security on Rate shopping so quotes can be shared by multiple users.
    '''     Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
    ''' </remarks>
    Public Function GetAPIDispatchedLoads(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vAPIDispatchedLoad()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vAPIDispatchedLoad
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                'Modified by LVV on 10/24/2018 for v-8.3 -- Fixed the user/company security on Rate shopping so quotes can be shared by multiple users. Show all records where the USC is in a list of users assigned to the LE – get the logged in users LEAControl
                'Get the USC of users assigned to the logged in users LE and add them to the filters list
                Dim flist = filters.FilterValues.ToList()
                Dim users = db.udfGetAssignedUsersForLE(Parameters.UserLEControl).Select(Function(y) y.USC.Value).ToArray()
                If users?.Length < 1 Then users = New Integer() {Parameters.UserControl}
                For Each usc In users
                    flist.Add(New Models.FilterDetails With {.filterName = "LTPosterUserControl", .filterValueFrom = usc})
                Next
                filters.FilterValues = flist.ToArray()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vAPIDispatchedLoad)
                iQuery = db.vAPIDispatchedLoads
                Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ")"
                ''Dim filterWhere = " (LTBookCustCompControl = " & Sec.getLECompControl() & ") AND (LTPosterUserControl = " & Me.Parameters.UserControl & ")"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPIDispatchedLoads"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' returns an array of RateRequestItem,  typically filtered by LoadTenderControl, caller must inforce user security
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 03/03/2018
    ''' </remarks>
    Public Function GetRateRequestItems(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vRateRequestItem()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vRateRequestItem
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vRateRequestItem)
                iQuery = db.vRateRequestItems
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRateRequestItems"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' get an array of NGLAPIAccessorials for bid by Load Tender Control
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 04/11/2018
    '''     uses vAPILoadTenderFee to return a distinct list of fees linked to the tblNGLAPICodes
    ''' </remarks>
    Public Function GetAccessorialsByLoadTender(ByVal intLoadTenderControl As Integer) As Models.NGLAPIAccessorial()
        Dim oRet() As Models.NGLAPIAccessorial
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = GetAccessorialsByLoadTender(intLoadTenderControl, db)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAccessorialsByLoadTender"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' wrapper method to allow bid accessorials to be returned inside other queries 
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    Private Function GetAccessorialsByLoadTender(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext) As Models.NGLAPIAccessorial()
        Dim oRet() As Models.NGLAPIAccessorial
        If intLoadTenderControl = 0 Then Return oRet
        If db Is Nothing Then Return oRet
        If Not db.tblLoadTenders.Any(Function(x) x.LoadTenderControl = intLoadTenderControl) Then Return oRet
        oRet = (From d In db.vAPILoadTenderFees Where d.LoadTenderControl = intLoadTenderControl Select New Models.NGLAPIAccessorial With {.Control = d.NACControl, .Code = d.NACCode, .Name = d.NACName, .Desc = d.NACDesc}).ToArray()
        Return oRet
    End Function

    Public Function GettblNGLAPICode(ByVal NACControl As Integer) As LTS.tblNGLAPICode
        Dim oRet As LTS.tblNGLAPICode
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblNGLAPICodes.Where(Function(x) x.NACControl = NACControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblNGLAPICode"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the most recent LoadTenderControl for the provided filters
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <param name="intLoadTenderType"></param>
    ''' <param name="intStatusCode"></param>
    ''' <param name="Archived"></param>
    ''' <returns>Integer</returns>
    ''' <remarks>
    ''' Added by LVV 7/13/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLTControlFiltered(Optional ByVal BookSHID As String = "",
                                         Optional ByVal intLoadTenderType As Integer = 0,
                                         Optional ByVal intStatusCode As Integer = 0,
                                         Optional ByVal Archived As Boolean? = Nothing) As Integer
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LoadTender As Integer = (
                From d In db.tblLoadTenders
                Where
                    (d.LTBookSHID = If(String.IsNullOrWhiteSpace(BookSHID), d.LTBookSHID, BookSHID)) _
                    And
                    (d.LTLoadTenderTypeControl = If(intLoadTenderType = 0, d.LTLoadTenderTypeControl, intLoadTenderType)) _
                    And
                    (d.LTStatusCode = If(intStatusCode = 0, d.LTStatusCode, intStatusCode)) _
                    And
                    (d.LTArchived = If(Archived Is Nothing, d.LTArchived, Archived))
                Order By d.LoadTenderControl Descending
                Select d.LoadTenderControl).FirstOrDefault()
                Return LoadTender
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLTControlFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns the most recent record for the provided BookControl
    ''' (Selects the First record ordered by LoadTenderControl desc)
    ''' Option to additionally filter by LoadTenderType and Status Codes
    ''' </summary>
    ''' <param name="statusCodes"></param>
    ''' <param name="BookSHID"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="intLoadTenderType"></param>
    ''' <param name="Archived"></param>
    ''' <returns>DTO.tblLoadTender</returns>
    ''' <remarks>
    ''' Added by LVV on 2/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetLTFilteredByStatusCodes(ByVal statusCodes As Integer(),
                                               Optional ByVal BookSHID As String = "",
                                               Optional ByVal BookControl As Integer = 0,
                                               Optional ByVal intLoadTenderType As Integer = 0,
                                               Optional ByVal Archived As Boolean? = Nothing) As DTO.tblLoadTender
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LoadTender As DTO.tblLoadTender = (
                From d In db.tblLoadTenders
                Where
                    (d.LTBookSHID = If(String.IsNullOrWhiteSpace(BookSHID), d.LTBookSHID, BookSHID)) _
                    And
                    (d.LTBookControl = If(BookControl = 0, d.LTBookControl, BookControl)) _
                    And
                    (d.LTLoadTenderTypeControl = If(intLoadTenderType = 0, d.LTLoadTenderTypeControl, intLoadTenderType)) _
                    And
                    (statusCodes.Contains(d.LTStatusCode)) _
                    And
                    (d.LTArchived = If(Archived Is Nothing, d.LTArchived, Archived))
                Order By d.LoadTenderControl Descending
                Select selectDTOData(d, db)).FirstOrDefault()
                Return LoadTender
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLTFilteredByStatusCodes"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' GetLoadTenders
    ''' </summary>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 5/19/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLoadTenders(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblLoadTender()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Try
                    intRecordCount = getScalarInteger("select COUNT(dbo.tblLoadTender.LoadTenderControl) from dbo.tblLoadTender")
                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the contacts that match the criteria
                Dim LoadTenders() As DTO.tblLoadTender = (
                From d In db.tblLoadTenders
                Order By d.LoadTenderControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return LoadTenders
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenders"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets LoadTenders filtered by LoadTenderType
    ''' </summary>
    ''' <param name="intLoadTenderType"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns>DTO.tblLoadTender()</returns>
    ''' <remarks>
    ''' Added by LVV 5/19/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetLoadTendersFiltered(ByVal intLoadTenderType As Integer, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblLoadTender()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Try
                    intRecordCount = getScalarInteger("select COUNT(dbo.tblLoadTender.LoadTenderControl) from dbo.tblLoadTender")
                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the contacts that match the criteria
                Dim LoadTenders() As DTO.tblLoadTender = (
                From d In db.tblLoadTenders
                Where
                    (d.LTLoadTenderTypeControl = If(intLoadTenderType = 0, d.LTLoadTenderTypeControl, intLoadTenderType))
                Order By d.LoadTenderControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return LoadTenders
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTendersFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Saves the DATRefID, Poster name and time, and status code for the 
    ''' specified LTControl after a successful Post to DAT
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="strAssetID"></param>
    ''' <param name="UserName"></param>
    ''' <param name="LTStatusCode"></param>
    ''' <remarks>
    ''' Added by LVV 5/25/16 for v-7.0.5.110 DAT
    ''' Modified by LVV on 1/13/17 for v-8.0 Next Stop
    ''' Added param for Status Code so can be used for multiple load boards
    ''' </remarks>
    Public Sub updatePostResults(ByVal LTControl As Integer, ByVal strAssetID As String, ByVal UserName As String, ByVal LTStatusCode As Integer)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim dtNow = Date.Now
                Try
                    Dim oLTS = (From d In db.tblLoadTenders Where d.LoadTenderControl = LTControl).FirstOrDefault()
                    If Not oLTS Is Nothing AndAlso oLTS.LoadTenderControl > 0 Then
                        With oLTS
                            .LTDATRefID = strAssetID
                            .LTDATPoster = UserName
                            .LTTenderedDate = dtNow
                            .LTArchived = False
                            .LTStatusCode = LTStatusCode
                            .LTModUser = UserName
                            .LTModDate = dtNow
                        End With
                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Ignore errors when updating
                End Try
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updatePostResults"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Saves the error message and status code for the specified LTControl
    ''' after an unsuccessful Post attempt to DAT
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="Message"></param>
    ''' <param name="UserName"></param>
    ''' <remarks>
    ''' Added by LVV 5/25/16 for v-7.0.5.110 DAT
    ''' I could probably change the name of this eventually to make it more general
    ''' considering all it does is save the Error status code and a message. This
    ''' could be used for more than DAT but I am leaving for now until I have time
    ''' to deal with it. LVV 6/15/16
    ''' </remarks>
    Public Sub updateDATPostResultsError(ByVal LTControl As Integer, ByVal Message As String, ByVal UserName As String)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim dtNow = Date.Now
                Try
                    Dim oLTS = (From d In db.tblLoadTenders Where d.LoadTenderControl = LTControl).FirstOrDefault()
                    If Not oLTS Is Nothing AndAlso oLTS.LoadTenderControl > 0 Then
                        With oLTS
                            .LTStatusCode = LTSCEnum.DATError
                            .LTMessage = Message
                            .LTModUser = UserName
                            .LTModDate = dtNow
                        End With

                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Ignore errors when updating
                End Try

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateDATPostResultsError"))
            End Try

        End Using
    End Sub

    ''' <summary>
    ''' A method to update various fields for a given record. 
    ''' The default of 99999 for Status Code is because there is no code that large
    ''' so this is the same as if a nullable was nothing. I might change it later
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="UserName"></param>
    ''' <param name="Message"></param>
    ''' <param name="StatusCode"></param>
    ''' <param name="Archived"></param>
    ''' <remarks>
    ''' Added by LVV 6/17/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Sub updateLoadTender(ByVal LTControl As Integer,
                                Optional ByVal UserName As String = "",
                                Optional ByVal Message As String = "",
                                Optional ByVal StatusCode As Integer = 99999,
                                Optional ByVal Archived As Boolean? = Nothing,
                                Optional ByVal Expired As Boolean? = Nothing,
                                Optional ByVal TotalWgt As Integer? = Nothing,
                                Optional ByVal TotalCube As Double? = Nothing,
                                Optional ByVal DATComment1 As String = Nothing,
                                Optional ByVal DATComment2 As String = Nothing,
                                Optional ByVal TotalPL As Integer? = Nothing,
                                Optional ByVal TotalCases As Double? = Nothing,
                                Optional ByVal TotalMiles As Double? = Nothing)
        Using operation = Logger.StartActivity("NGLLoadTenderData.updateLoadTender with LTControl: {LoadTenderControl}, UserName: {UserName}, TotalPL: {TotalPL}, TotalCases: {TotalCases}, TotalMiles: {TotalMiles}", LTControl, UserName, TotalPL, TotalCases, TotalMiles)

            Dim oLTS As LTS.tblLoadTender
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try
                    Dim dtNow = Date.Now
                    Try

                        oLTS = (From d In db.tblLoadTenders Where d.LoadTenderControl = LTControl).FirstOrDefault()
                        If Not oLTS Is Nothing AndAlso oLTS.LoadTenderControl > 0 Then
                            With oLTS
                                If Not String.IsNullOrEmpty(Message) Then
                                    If Not String.IsNullOrWhiteSpace(.LTMessage) Then
                                        .LTMessage += (" " + Message)
                                    Else
                                        .LTMessage = Message
                                    End If
                                End If
                                If StatusCode <> 99999 Then
                                    .LTStatusCode = StatusCode
                                End If
                                If Not Archived Is Nothing Then
                                    .LTArchived = Archived
                                End If
                                If Not Expired Is Nothing Then
                                    .LTExpired = Expired
                                End If
                                If Not String.IsNullOrWhiteSpace(UserName) Then
                                    .LTModUser = UserName
                                    .LTModDate = dtNow
                                End If
                                If Not TotalWgt Is Nothing Then
                                    .LTBookTotalWgt = TotalWgt
                                End If
                                If Not TotalCube Is Nothing Then
                                    .LTBookTotalCube = TotalCube
                                End If
                                If Not String.IsNullOrWhiteSpace(DATComment1) Then
                                    .LTDATComment1 = DATComment1
                                End If
                                If Not String.IsNullOrWhiteSpace(DATComment2) Then
                                    .LTDATComment2 = DATComment2
                                End If
                                If Not TotalPL Is Nothing Then
                                    .LTBookTotalPL = TotalPL
                                End If
                                If Not TotalCases Is Nothing Then
                                    .LTBookTotalCases = TotalCases
                                End If
                                If Not TotalMiles Is Nothing Then
                                    .LTBookTotalMiles = TotalMiles
                                End If

                            End With

                            db.SubmitChanges()
                        End If
                    Catch ex As Exception
                        'Ignore errors when updating
                        Logger.Error(ex, "Exception in NGLLoadTenderData.updateLoadTender")
                    End Try

                Catch ex As Exception
                    Logger.Error(ex, "Exception in NGLLoadTenderData.updateLoadTender")
                    'ManageLinqDataExceptions(ex, buildProcedureName("updateDATPostResults"))
                End Try

            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Gets all records with Load Tender Type 5 (DAT) based on filter parameter. Also filtered where 
    ''' the LTTenderedDate (date Posted) is null OR between the parameters StartDate and EndDate.
    ''' </summary>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="scFilter"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 6/9/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetDATDataScreen(ByVal StartDate As Date, ByVal EndDate As Date, ByVal scFilter As Integer) As DTO.tblLoadTender()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000

                Try
                    intRecordCount = getScalarInteger("select COUNT(dbo.tblLoadTender.LoadTenderControl) from dbo.tblLoadTender")
                Catch ex As Exception
                    'ignore any record count errors
                End Try

                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim LTs As New List(Of DTO.tblLoadTender)
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)
                'Gets all active records that matches the provided criteria

                Select Case scFilter
                    Case 1
                        'Posted or Error
                        LTs = (
                            From d In db.tblLoadTenders
                            Where
                                (d.LTLoadTenderTypeControl = LTTypeEnum.DAT) _
                                And
                                ((d.LTArchived = False And d.LTStatusCode = LTSCEnum.DATPosted) Or (d.LTArchived = True And d.LTStatusCode = LTSCEnum.DATError)) _
                                And
                                ((d.LTTenderedDate Is Nothing) _
                                Or (d.LTTenderedDate >= StartDate And d.LTTenderedDate <= EndDate))
                            Order By d.LoadTenderControl Descending
                            Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).ToList()
                    Case 2
                        'Deleted
                        LTs = (
                            From d In db.tblLoadTenders
                            Where
                                (d.LTLoadTenderTypeControl = LTTypeEnum.DAT) _
                                And
                                ((d.LTStatusCode = LTSCEnum.DATDeleted)) _
                                And
                                ((d.LTTenderedDate Is Nothing) _
                                Or (d.LTTenderedDate >= StartDate And d.LTTenderedDate <= EndDate))
                            Order By d.LoadTenderControl Descending
                            Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).ToList()
                    Case 3
                        'Expired
                        LTs = (
                            From d In db.tblLoadTenders
                            Where
                                (d.LTLoadTenderTypeControl = LTTypeEnum.DAT) _
                                And
                                ((d.LTStatusCode = LTSCEnum.DATExpired)) _
                                And
                                ((d.LTTenderedDate Is Nothing) _
                                Or (d.LTTenderedDate >= StartDate And d.LTTenderedDate <= EndDate))
                            Order By d.LoadTenderControl Descending
                            Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).ToList()
                End Select

                Return LTs.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATDataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Save Messages and logs associated with a Load Tender Quote request using NGL Tariff data.  
    ''' Callers of this method should not save CarrierCostResults.log records to the tblLoadTenderLog
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="enmLoadTenderTypeControl"></param>
    ''' <param name="oCarrierCostResults"></param>
    ''' <remarks>
    ''' Created and Modified by RHR for v-8.5.3.001 on 05/31/2022 added logic for tblLoadTenderLog records  
    ''' </remarks>
    Public Sub saveLoadTenderCarrierCostMessages(iLoadTenderControl As Integer,
                                      enmLoadTenderTypeControl As NGLLoadTenderTypes,
                                      oCarrierCostResults As DTO.CarrierCostResults)
        'enmLoadTenderTypeControl is expected to be one of the following
        'DAT = 5,NextStop = 6,P44 = 7,RateQuote = 8,SpotRate = 9
        Dim dtModDate As Date = Date.Now()
        Dim sModUser As String = Me.Parameters.UserName
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oNGLMessage As New List(Of LTS.tblNGLMessageRefBook)
                saveLoadTenderCarrierCostLogs(iLoadTenderControl, oCarrierCostResults)
                If Not oCarrierCostResults.Messages Is Nothing AndAlso oCarrierCostResults.Messages.Count > 0 Then
                    'Note: CarrierCostResults.Messages are a list of message localized value keys where the key string
                    '       like M_FinalizedCannotChangeCarrier  the key is always the 
                    '      is stored in a dictionary and the values are a list of NGLMessages which represent a parameter 
                    '       array of posible values used to format the localized value parameters
                    '       Typically the CarriercostResults do not have any parameters
                    '       but if they do it is formatted like this: String.Format(sLog, p.ToArray()))
                    '       To save the record in the database each NMKeyString may or may not have multiple records
                    '       
                    For Each kvp As KeyValuePair(Of String, List(Of DTO.NGLMessage)) In oCarrierCostResults.Messages
                        Dim lMessages = kvp.Value
                        'lMessages may be null if so we still need the  kvp.Key
                        If Not lMessages Is Nothing AndAlso lMessages.Count > 0 Then
                            For Each msg In lMessages
                                'The Property Message ( msg.Message) holds any parameter array values                                '   
                                oNGLMessage.Add(New LTS.tblNGLMessageRefBook() With {.NMMTRefControl = iLoadTenderControl,
                                                .NMNMTControl = CInt(enmLoadTenderTypeControl),
                                                .NMKeyString = kvp.Key,
                                                .NMMessage = msg.Message,
                                                .NMErrorDetails = msg.ErrorDetails,
                                                .NMErrorMessage = msg.ErrorMessage,
                                                .NMErrorReason = msg.ErrorReason,
                                                .NMMTRefAlphaControl = msg.AlphaCode,
                                                .NMMTRefName = "LoadTenderControl",
                                                .NMModDate = dtModDate,
                                                .NMModUser = sModUser})
                            Next
                        Else
                            'just add the default record
                            oNGLMessage.Add(New LTS.tblNGLMessageRefBook() With {.NMMTRefControl = iLoadTenderControl,
                                                .NMNMTControl = CInt(enmLoadTenderTypeControl),
                                                .NMKeyString = kvp.Key,
                                                .NMMessage = "",
                                                .NMErrorDetails = "",
                                                .NMErrorMessage = "",
                                                .NMErrorReason = "",
                                                .NMMTRefAlphaControl = "",
                                                .NMMTRefName = "LoadTenderControl",
                                                .NMModDate = dtModDate,
                                                .NMModUser = sModUser})
                        End If
                    Next
                End If

                If Not oNGLMessage Is Nothing AndAlso oNGLMessage.Count() > 0 Then
                    db.tblNGLMessageRefBooks.InsertAllOnSubmit(oNGLMessage)
                End If
                db.SubmitChanges() 'If Not Res.Messages Is Nothing AndAlso Res.Messages.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, Res.Messages)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("saveLoadTenderMessages"))
            End Try
        End Using


    End Sub

    ''' <summary>
    ''' Save logs associated with a Load Tender Quote request using NGL Tariff data.  
    ''' Callers of this method should not save CarrierCostResults.log records to the tblLoadTenderLog
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="oCarrierCostResults"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.001 on 05/31/2022 added logic for tblLoadTenderLog records 
    ''' </remarks>
    Public Sub saveLoadTenderCarrierCostLogs(iLoadTenderControl As Integer,
                                      oCarrierCostResults As DTO.CarrierCostResults)

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            If Not oCarrierCostResults.Log Is Nothing AndAlso oCarrierCostResults.Log.Count > 0 Then
                If iLoadTenderControl <> 0 AndAlso db.tblLoadTenders.Any(Function(x) x.LoadTenderControl = iLoadTenderControl) Then
                    Dim oLTLogData As New NGLLoadTenderLogData(Me.Parameters)
                    For Each lg In oCarrierCostResults.Log.Where(Function(x) x.bLogged = False)
                        Try
                            lg.bLogged = True
                            Dim oRecord As New LTS.tblLoadTenderLog() With {.LTLogLoadTenderControl = iLoadTenderControl,
                                .LTLogMessage = lg.Message,
                                .LTLogDetails = lg.ErrorDetails,
                                .LTLogReason = lg.ErrorReason}
                            oLTLogData.InsertOrUpdate(oRecord)
                        Catch ex As Exception
                            ' if write to log fails we just continue 
                        End Try
                    Next
                End If
            End If
        End Using
    End Sub

    Public Function readLoadTenderCarrierCostMessages(iLoadTenderControl As Integer, Optional enmLoadTenderTypeControl As NGLLoadTenderTypes = NGLLoadTenderTypes.None) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oNGLMessage As New List(Of LTS.tblNGLMessageRefBook)
                If enmLoadTenderTypeControl = NGLLoadTenderTypes.None Then
                    oNGLMessage = db.tblNGLMessageRefBooks.Where(Function(x) x.NMMTRefControl = iLoadTenderControl).ToList()
                Else
                    oNGLMessage = db.tblNGLMessageRefBooks.Where(Function(x) x.NMMTRefControl = iLoadTenderControl And x.NMNMTControl = CInt(enmLoadTenderTypeControl)).ToList()
                End If

                If Not oNGLMessage Is Nothing AndAlso oNGLMessage.Count() > 0 Then
                    oRet.Success = True
                    Dim oNGLMessages As New Dictionary(Of String, List(Of DTO.NGLMessage))
                    For Each oMsg In oNGLMessage.OrderBy(Function(x) x.NMKeyString)
                        If oMsg.NMKeyString = "Log" Then
                            oRet.AddLog(oMsg.NMMessage)
                        Else
                            oRet.AddMessage(DTO.WCFResults.MessageType.Messages, oMsg.NMKeyString, New DTO.NGLMessage(oMsg.NMMessage, oMsg.NMControl, oMsg.NMMTRefName, oMsg.NMMTRefControl))
                        End If
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("readLoadTenderCarrierCostMessages"))
            End Try
        End Using
        Return oRet
    End Function

    Public Function deleteLoadTenderCarrierCostMessages(iLoadTenderControl As Integer, Optional enmLoadTenderTypeControl As NGLLoadTenderTypes = NGLLoadTenderTypes.None) As Boolean
        Dim blnRet As Boolean = True
        Dim oMessages As New DTO.WCFResults()
        oMessages.Success = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oNGLMessage As New List(Of LTS.tblNGLMessageRefBook)
                If enmLoadTenderTypeControl = NGLLoadTenderTypes.None Then
                    oNGLMessage = db.tblNGLMessageRefBooks.Where(Function(x) x.NMMTRefControl = iLoadTenderControl).ToList()
                Else
                    oNGLMessage = db.tblNGLMessageRefBooks.Where(Function(x) x.NMMTRefControl = iLoadTenderControl And x.NMNMTControl = CInt(enmLoadTenderTypeControl)).ToList()
                End If

                If Not oNGLMessage Is Nothing AndAlso oNGLMessage.Count() > 0 Then
                    For Each oMsg In oNGLMessage
                        db.tblNGLMessageRefBooks.DeleteOnSubmit(oMsg)
                    Next
                End If
                db.SubmitChanges(ConflictMode.ContinueOnConflict)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("deleteLoadTenderCarrierCostMessages"))
                blnRet = False
            End Try
        End Using
        Return blnRet
    End Function


#Region "DAT 365"

    ''' <summary>
    ''' Gets all DAT records which are currently posted
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration</remarks>
    Public Function GetDATPosted(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vDATPosted()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vDATPosted
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vDATPosted)
                iQuery = db.vDATPosteds
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATPosted"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets all DAT records which have been deleted
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration</remarks>
    Public Function GetDATDeleted(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vDATDeleted()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vDATDeleted
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vDATDeleted)
                iQuery = db.vDATDeleteds
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATDeleted"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets all DAT records which are expired
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration</remarks>
    Public Function GetDATExpired(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vDATExpired()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vDATExpired
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vDATExpired)
                iQuery = db.vDATExpireds
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATExpired"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets all DAT records which have errors
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration</remarks>
    Public Function GetDATErrors(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vDATError()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vDATError
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vDATError)
                iQuery = db.vDATErrors
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATErrors"), db)
            End Try
        End Using
        Return Nothing
    End Function

#End Region


    ''' <summary>
    '''  Gets all records with Load Tender Type 5 (DAT) based on SHID
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 7/1/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Public Function GetDATDataScreenFiltered(ByVal SHID As String) As DTO.tblLoadTender()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LTs As New List(Of DTO.tblLoadTender)
                'Gets all records that matches the provided criteria

                LTs = (
                    From d In db.tblLoadTenders
                    Where
                        (d.LTLoadTenderTypeControl = LTTypeEnum.DAT) _
                      And
                        (d.LTBookSHID = SHID)
                    Order By d.LoadTenderControl Descending
                    Select selectDTOData(d, db)).ToList()

                Return LTs.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATDataScreenFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Calls udfGetDATStopData and returns only a few fields
    ''' from the return table in a WCFResults object
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns>DTO.WCFResults</returns>
    ''' <remarks>
    ''' Added by LVV 7/1/16 for v-7.0.5.110 DAT
    ''' Added BookTotalPL float, BookTotalCases int
    ''' </remarks>
    Public Function GetDATStopData(ByVal BookControl As Integer) As DTO.WCFResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.WCFResults

                Dim ltsRes = (From d In db.udfGetDATStopData(BookControl) Select d).FirstOrDefault()

                If Not ltsRes Is Nothing Then
                    wcfRet.updateKeyFields("BookTotalWgt", ltsRes.BookTotalWgt)
                    wcfRet.updateKeyFields("BookTotalCube", ltsRes.BookTotalCube)
                    wcfRet.updateKeyFields("DATComment1", ltsRes.DATComment1)
                    wcfRet.updateKeyFields("DATComment2", ltsRes.DATComment2)
                    wcfRet.updateKeyFields("BookTotalPL", ltsRes.BookTotalPL)
                    wcfRet.updateKeyFields("BookTotalCases", ltsRes.BookTotalCases)
                    wcfRet.updateKeyFields("BookMilesFrom", ltsRes.BookMilesFrom)

                    wcfRet.Success = True
                Else
                    wcfRet.Success = False
                End If
                Return wcfRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATStopData"))
            End Try
            Return Nothing
        End Using

    End Function

    Public Function GetOrderNumberFromLoadTender(ByVal iLoadTenderControl As Integer) As String
        Dim sRet As String = "Unavailable Order Number"
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                sRet = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = iLoadTenderControl).Select(Function(x) x.LTBookCarrOrderNumber).FirstOrDefault()
            Catch ex As Exception
                'Do Nothing
            End Try

            Return sRet
        End Using
    End Function
    Public Function InsertLoadBoardRecords(ByVal BookControl As Integer, ByVal BookSHID As String, ByVal LTTypeControl As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum) As DTO.WCFResults
        Dim wcfRet As New DTO.WCFResults
        Dim ltsRes As LTS.spInsertLoadBoardRecordsResult
        Using operation = Logger.StartActivity("InsertLoadBoardRecords(BookControl: {BookControl}, BookSHID: {BookSHID}, LTTypeControl: {LTTypeControl}", BookControl, BookSHID, LTTypeControl)

            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try


                    ltsRes = (From d In db.spInsertLoadBoardRecords(BookControl, BookSHID, CInt(LTTypeControl)) Select d).FirstOrDefault()

                    If Not ltsRes Is Nothing Then
                        If ltsRes.LoadTenderControl <> 0 AndAlso Not ltsRes.LoadTenderControl Is Nothing Then
                            wcfRet.updateKeyFields("LoadTenderControl", ltsRes.LoadTenderControl)
                            wcfRet.Success = True
                            'get the SHID number assigned,  we use this in the 
                            wcfRet.updateKeyFields("BookSHID", ltsRes.BookSHID)
                            'get the CNS number assigned,  we use this in the 
                            wcfRet.updateKeyFields("BookConsPrefix", ltsRes.BookConsPrefix)
                        End If

                        If ltsRes.ErrNumber <> 0 Then
                            Dim p As New List(Of String)
                            If ltsRes.ErrNumber = 1 Then
                                p.Add(ltsRes.ParamName)
                                p.Add(ltsRes.ParamValue)
                                wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p.ToArray())
                            End If
                            If ltsRes.ErrNumber = 3 Then
                                wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey)
                            End If
                            wcfRet.updateKeyFields("RetMsg", ltsRes.RetMsg)
                            wcfRet.Success = False
                        End If

                        Return wcfRet
                    End If

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("InsertLoadBoardRecords"))
                End Try

                Return Nothing
            End Using

        End Using
    End Function




    ''' <summary>
    ''' use the P44RateRequest and the RateRequestOrder data to insert a record into the tblLoadTender table
    ''' </summary>
    ''' <param name="oP44Request"></param>
    ''' <param name="order"></param>
    ''' <param name="BookSHiD"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 06/28/2023
    '''     added logic to support temperature selection when Rate Shopping 
    ''' </remarks>
    Private Function createLoadTenderQuote(ByVal oP44Request As P44.RateRequest,
                                           ByRef order As Models.RateRequestOrder,
                                         Optional ByVal BookSHiD As String = "Quote",
                                         Optional ByVal BookCarrOrderNumber As String = "Pending",
                                         Optional ByVal BookControl As Integer = 0,
                                         Optional ByVal BookConsPrefix As String = "Sample") As LTS.tblLoadTender
        Dim oRet As New LTS.tblLoadTender()
        Using operation = Logger.StartActivity("createLoadTenderQuote(oP44Request: {oP44Request}, order: {order}, BookSHiD: {BookSHiD}, BookCarrOrderNumber: {BookCarrOrderNumber}, BookControl: {BookControl}, BookConsPrefix: {BookConsPrefix}", oP44Request, order, BookSHiD, BookCarrOrderNumber, BookControl, BookConsPrefix)
            Dim Sec As New NGLSecurityDataProvider(Parameters)
            With oRet
                .LTLoadTenderTypeControl = Utilities.NGLLoadTenderTypes.RateQuote
                .LTBookCustCompControl = Sec.getLECompControl()
                .LTPosterUserControl = Parameters.UserControl
                .LTCarrierControl = 0
                .LTCarrierSCAC = ""
                .LTCarrierNumber = 0
                .LTCarrierName = ""
                .LTBookControl = BookControl
                .LTBookProNumber = ""
                .LTBookConsPrefix = BookConsPrefix
                .LTBookSHID = BookSHiD
                .LTBookStopNo = 1
                .LTBookTotalMiles = 0
                .LTBookCarrOrderNumber = BookCarrOrderNumber
                .LTBookOrderSequence = 0
                .LTBookRouteFinalCode = ""
                .LTBookTransactionPurpose = Nothing
                If (Not oP44Request.lineItems Is Nothing AndAlso oP44Request.lineItems.Count() > 0) Then
                    .LTBookTotalCases = oP44Request.lineItems.Sum(Function(x) x.numPieces)
                    .LTBookTotalWgt = oP44Request.lineItems.Sum(Function(x) x.weight)
                    .LTBookTotalPL = oP44Request.lineItems.Sum(Function(x) x.palletCount)
                Else
                    .LTBookTotalCases = 1
                    .LTBookTotalWgt = 1
                    .LTBookTotalPL = 1
                End If

                .LTBookTotalCube = Conversion.Int(oP44Request.cubicFeet)
                .LTBookDateLoad = Utilities.returnDateFromString(oP44Request.shipDate)
                .LTBookDateRequired = Utilities.returnDateFromString(oP44Request.deliveryDate, Utilities.returnDateFromString(oP44Request.shipDate).AddDays(3))
                If (Not oP44Request.origin Is Nothing) Then
                    .LTBookOrigName = oP44Request.origin.companyName
                    .LTBookOrigAddress1 = oP44Request.origin.address1
                    .LTBookOrigAddress2 = oP44Request.origin.address2
                    .LTBookOrigAddress3 = oP44Request.origin.address3
                    .LTBookOrigCity = oP44Request.origin.city
                    .LTBookOrigState = oP44Request.origin.stateName
                    .LTBookOrigCountry = oP44Request.origin.country
                    .LTBookOrigZip = oP44Request.origin.postalCode
                Else
                    .LTBookOrigName = "N/A"
                    .LTBookOrigAddress1 = ""
                    .LTBookOrigAddress2 = ""
                    .LTBookOrigAddress3 = ""
                    .LTBookOrigCity = ""
                    .LTBookOrigState = ""
                    .LTBookOrigCountry = ""
                    .LTBookOrigZip = ""
                End If
                If (Not oP44Request.destination Is Nothing) Then
                    .LTBookDestName = oP44Request.destination.companyName
                    .LTBookDestAddress1 = oP44Request.destination.address1
                    .LTBookDestAddress2 = oP44Request.destination.address2
                    .LTBookDestAddress3 = oP44Request.destination.address3
                    .LTBookDestCity = oP44Request.destination.city
                    .LTBookDestState = oP44Request.destination.stateName
                    .LTBookDestCountry = oP44Request.destination.country
                    .LTBookDestZip = oP44Request.destination.postalCode
                Else
                    .LTBookDestName = "N/A"
                    .LTBookDestAddress1 = ""
                    .LTBookDestAddress2 = ""
                    .LTBookDestAddress3 = ""
                    .LTBookDestCity = ""
                    .LTBookDestState = ""
                    .LTBookDestCountry = ""
                    .LTBookDestZip = ""
                End If
                ' Begin Modified by RHR for v-8.5.4.001 on 06/28/2023
                order.prepareTemperatureSettings()
                .LTBookLoadCom = order.CommCodeType
                .LTCommCodeDescription = order.CommCodeDescription
                ' End Modified by RHR for v-8.5.4.001 on 06/28/2023
                .LTLaneComments = Nothing
                .LTLaneOriginAddressUse = 0
                .LTBookRouteConsFlag = 1
                .LTBookRevTotalCost = 0
                .LTModDate = Date.Now()
                .LTModUser = Me.Parameters.UserName
            End With
        End Using

        Return oRet
    End Function

    ''' <summary>
    ''' Creates a tblLoadTenderBook using the P44 Quoted information, caller must save to db
    ''' </summary>
    ''' <param name="oP44Request"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="BookSHiD"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 3/1/2018
    '''   Creates a tblLoadTenderBook using the P44 Quoted information
    '''   Does not save.  the caller must save the changes to the database
    ''' </remarks>
    Private Function createLoadTenderBook(ByVal oP44Request As P44.RateRequest,
                                          ByVal LoadTenderControl As Integer,
                                         Optional ByVal BookSHiD As String = "Quote",
                                         Optional ByVal BookCarrOrderNumber As String = "Pending",
                                         Optional ByVal BookControl As Integer = 0,
                                         Optional ByVal BookConsPrefix As String = "Sample") As LTS.tblLoadTenderBook
        Dim oRet As New LTS.tblLoadTenderBook()
        Using operation = Logger.StartActivity("createLoadTenderBook(oP44Request: {oP44Request}, LoadTenderControl: {LoadTenderControl}, BookSHiD: {BookSHiD}, BookCarrOrderNumber: {BookCarrOrderNumber}, BookControl: {BookControl}, BookConsPrefix: {BookConsPrefix}", oP44Request, LoadTenderControl, BookSHiD, BookCarrOrderNumber, BookControl, BookConsPrefix)
            Try
                If LoadTenderControl = 0 Then Return Nothing
                If oP44Request Is Nothing Then Return Nothing

                With oRet
                    .LTBookLoadTenderControl = LoadTenderControl
                    .LTBookBookControl = BookControl
                    .LTBookProNumber = ""
                    .LTBookConsPrefix = BookConsPrefix
                    .LTBookSHID = BookSHiD
                    .LTBookOrigCompControl = 0
                    If (Not oP44Request.origin Is Nothing) Then
                        .LTBookOrigName = oP44Request.origin.companyName
                        .LTBookOrigAddress1 = oP44Request.origin.address1
                        .LTBookOrigAddress2 = oP44Request.origin.address2
                        .LTBookOrigAddress3 = oP44Request.origin.address3
                        .LTBookOrigCity = oP44Request.origin.city
                        .LTBookOrigState = oP44Request.origin.stateName
                        .LTBookOrigCountry = oP44Request.origin.country
                        .LTBookOrigZip = oP44Request.origin.postalCode
                    Else
                        .LTBookOrigName = "N/A"
                        .LTBookOrigAddress1 = ""
                        .LTBookOrigAddress2 = ""
                        .LTBookOrigAddress3 = ""
                        .LTBookOrigCity = ""
                        .LTBookOrigState = ""
                        .LTBookOrigCountry = ""
                        .LTBookOrigZip = ""
                    End If
                    .LTBookDestCompControl = 0
                    If (Not oP44Request.destination Is Nothing) Then
                        .LTBookDestName = oP44Request.destination.companyName
                        .LTBookDestAddress1 = oP44Request.destination.address1
                        .LTBookDestAddress2 = oP44Request.destination.address2
                        .LTBookDestAddress3 = oP44Request.destination.address3
                        .LTBookDestCity = oP44Request.destination.city
                        .LTBookDestState = oP44Request.destination.stateName
                        .LTBookDestCountry = oP44Request.destination.country
                        .LTBookDestZip = oP44Request.destination.postalCode
                    Else
                        .LTBookDestName = "N/A"
                        .LTBookDestAddress1 = ""
                        .LTBookDestAddress2 = ""
                        .LTBookDestAddress3 = ""
                        .LTBookDestCity = ""
                        .LTBookDestState = ""
                        .LTBookDestCountry = ""
                        .LTBookDestZip = ""
                    End If
                    .LTBookDateOrdered = returnDateFromString(oP44Request.shipDate)
                    .LTBookDateLoad = returnDateFromString(oP44Request.shipDate)
                    .LTBookDateRequired = returnDateFromString(oP44Request.deliveryDate, If(.LTBookDateLoad.HasValue, .LTBookDateLoad.Value.AddDays(3), Date.Now.AddDays(3)))
                    If (Not oP44Request.lineItems Is Nothing AndAlso oP44Request.lineItems.Count() > 0) Then
                        .LTBookTotalCases = oP44Request.lineItems.Sum(Function(x) x.numPieces)
                        .LTBookTotalWgt = oP44Request.lineItems.Sum(Function(x) x.weight)
                        .LTBookTotalPL = oP44Request.lineItems.Sum(Function(x) x.palletCount)
                    Else
                        .LTBookTotalCases = 1
                        .LTBookTotalWgt = 1
                        .LTBookTotalPL = 1
                    End If
                    .LTBookTotalCube = Conversion.Int(oP44Request.cubicFeet)
                    .LTBookTotalPX = .LTBookTotalPL
                    .LTBookTotalBFC = 0
                    .LTBookTranCode = "N"
                    .LTBookPayCode = "N"
                    .LTBookTypeCode = ""
                    .LTBookBookLoadCom = "D"
                    .LTBookBookLoadComments = "Quote"
                    .LTBookStopNo = 1
                    .LTBookPickupStopNumber = 1
                    .LTBookOrigStopNumber = 1
                    .LTBookDestStopNumber = 1
                    .LTBookPickNumber = 1
                    .LTBookItemDetailDescription = ""
                    .LTBookCarrOrderNumber = BookCarrOrderNumber
                    .LTBookOrderSequence = 0
                    .LTBookMilesFrom = 0
                    .LTBookRevLaneBenchMiles = 0
                    .LTBookComCode = "D"
                    .LTBookTransType = ""
                    .LTBookRouteConsFlag = 1
                    .LTBookAllowInterlinePoints = 1
                    .LTBookModDate = Date.Now()
                    .LTBookModUser = Me.Parameters.UserName
                    '.LTBookOrigContactName = 
                End With


            Catch ex As Exception
                Return Nothing
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Create one tblLoadTenderItem instance for each  P44 Quoted item,  caller must save to db
    ''' </summary>
    ''' <param name="oP44Request"></param>
    ''' <param name="LTBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 3/1/2018
    '''   Creates an array of tblLoadTenderItem using the P44 Quoted item information
    '''   Does not save.  the caller must save the changes to the database
    ''' </remarks>
    Private Function createLoadTenderItems(ByVal oP44Request As P44.RateRequest,
                                          ByVal LTBookControl As Integer) As LTS.tblLoadTenderItem()
        Dim oRet As New List(Of LTS.tblLoadTenderItem)
        Using operation = Logger.StartActivity("createLoadTenderItems(oP44Request: {oP44Request}, LTBookControl: {LTBookControl}", oP44Request, LTBookControl)
            Try
                Dim oLookUpData As New NGLLookupDataProvider(Parameters)
                Dim oPalletTypes = oLookUpData.GetViewLookupStaticList(NGLLookupDataProvider.StaticLists.PalletType, NGLLookupDataProvider.ListSortType.Control)
                If LTBookControl = 0 Then Return Nothing
                If oP44Request Is Nothing Then Return Nothing
                If oP44Request.lineItems Is Nothing OrElse oP44Request.lineItems.Count() < 1 Then Return Nothing
                'weight = i.Weight.ToString(), weightUnit = i.WeightUnit, freightClass = i.FreightClass, palletCount = i.PalletCount, numPieces = i.NumPieces, description = i.Description, length = i.Length, width = i.Width, height = i.Height, packageType = i.PackageType, nmfcItem = i.NMFCItem, nmfcSub = i.NMFCSub, stackable = i.Stackable
                Dim itmCt As Integer = 0
                For Each lItm In oP44Request.lineItems
                    Dim tItm = New LTS.tblLoadTenderItem
                    itmCt += 1
                    With tItm
                        .LTItemLTBookControl = LTBookControl
                        .LTItemItemNumber = "QI-" & itmCt.ToString()
                        .LTItemQtyOrdered = lItm.numPieces
                        .LTItemWeight = lItm.weight
                        .LTItemFAKClass = lItm.freightClass
                        .LTItemCube = 0
                        .LTItemPallets = lItm.palletCount
                        If oPalletTypes.Any(Function(x) x.Description = lItm.packageType) Then
                            .LTItemPalletTypeID = oPalletTypes.Where(Function(x) x.Description = lItm.packageType).Select(Function(x) x.Control).FirstOrDefault()
                        Else
                            .LTItemPalletTypeID = 1 'normal
                        End If
                        .LTItemQtyLength = lItm.length
                        .LTItemNMFCClass = lItm.nmfcItem
                        .LTItemNMFCSubClass = lItm.nmfcSub
                        .LTItemDescription = lItm.description
                        .LTItemQtyWidth = lItm.width
                        .LTItemQtyHeight = lItm.height
                        .LTItemStackable = lItm.stackable
                        '.LTItemHazmat = lItm.h
                    End With
                    oRet.Add(tItm)
                Next

            Catch ex As Exception
                'do not fail to show save the quote just because the items could not be saved.
                Return Nothing
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function getPalletTypeID(ByVal sPkgType As String) As Integer
        Dim iRet As Integer = 1
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                iRet = db.PalletTypes.Where(Function(x) x.PalletTypeDescription = sPkgType).Select(Function(x) x.ID).FirstOrDefault()
            Catch ex As Exception
                'do nothing just return the default
            End Try
        End Using
        Return iRet
    End Function

    ''' <summary>
    ''' Save the company, lane and booking informaiton.  Must be called inside a db using statement.
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="db"></param>
    ''' <param name="intLoadTenderTransTypeControl"></param>    ''' 
    ''' <param name="sErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 04/06/2018
    '''     Process Flow
    '''     1. check the intLoadTenderTransTypeControl flag if zero default to 1
    '''     2. create orign comp on outbound (1) or transfer (2) 
    '''     3. create destinination comp on inbound (3) or transfer (2)
    '''     4. create the lane
    '''     5. create the booking record
    ''' </remarks>
    Private Function createBookingFromTender(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext, ByVal intLoadTenderTransTypeControl As Integer, ByRef sErrMsg As String) As Integer
        Dim intBookControl As Integer = 0
        Using operation = Logger.StartActivity("createBookingFromTender(intLoadTenderControl: {intLoadTenderControl}, db: {db}, intLoadTenderTransTypeControl: {intLoadTenderTransTypeControl}", intLoadTenderControl, db, intLoadTenderTransTypeControl)

            If intLoadTenderControl = 0 Or db Is Nothing Then Return False
            If intLoadTenderTransTypeControl < 1 Or intLoadTenderTransTypeControl > 3 Then intLoadTenderTransTypeControl = 1
            Dim intOrigCompControl As Integer = 0
            Dim intDestCompControl As Integer = 0
            Dim intLaneControl As Integer = 0


            'check if a booking record exists by calling spUpdateBookFromLoadTender,  if this works we have a booking so we do not need to create a new one.
            Logger.Information("calling spUpdateBookFromLoadTender(intLoadTenderControl: {intLoadTenderControl}, Parameters.UserName: {UserName})", intLoadTenderControl, Parameters.UserName)
            Dim oResult = db.spUpdateBookFromLoadTender(intLoadTenderControl, Parameters.UserName).FirstOrDefault()
            If Not oResult Is Nothing AndAlso oResult.ErrNumber = 0 AndAlso oResult.BookControl <> 0 Then
                operation.Complete()
                Return oResult.BookControl.Value
            End If
            If intLoadTenderTransTypeControl = 1 Or intLoadTenderTransTypeControl = 2 Then
                Logger.Information("calling spCreateCompFromLoadTenderOrigin(intLoadTenderControl: {intLoadTenderControl}, Parameters.UserName: {UserName})", intLoadTenderControl, Parameters.UserName)
                Dim oRet = db.spCreateCompFromLoadTenderOrigin(intLoadTenderControl, Parameters.UserName).FirstOrDefault()
                If Not oRet Is Nothing Then
                    intOrigCompControl = oRet.CompControl
                End If
                If intOrigCompControl = 0 Then
                    'The system could not generate a new {0}
                    Logger.Error("throwCreateNewDependentRecordFailed(Booking Record because creation Of the ship from company information failed.)")
                    throwCreateNewDependentRecordFailed("Booking Record because creation Of the ship from company information failed.")
                End If
            End If

            If intLoadTenderTransTypeControl = 2 Or intLoadTenderTransTypeControl = 3 Then
                Dim oRet = db.spCreateCompFromLoadTenderDest(intLoadTenderControl, Parameters.UserName).FirstOrDefault()
                Logger.Information("calling spCreateCompFromLoadTenderDest(intLoadTenderControl: {intLoadTenderControl}, Parameters.UserName: {UserName})", intLoadTenderControl, Parameters.UserName)
                If Not oRet Is Nothing Then
                    intDestCompControl = oRet.CompControl
                End If
                If intDestCompControl = 0 Then
                    'The system could not generate a new {0}
                    throwCreateNewDependentRecordFailed("Booking Record because creation Of the ship To company information failed.")
                End If
            End If
            Logger.Information("calling spCreateLaneFromLoadTender(intLoadTenderControl: {intLoadTenderControl}, Parameters.UserName: {Parameters.UserName})")
            Dim oLaneRet = db.spCreateLaneFromLoadTender(intLoadTenderControl, Parameters.UserName).FirstOrDefault()
            If Not oLaneRet Is Nothing Then
                intLaneControl = oLaneRet.LaneControl
            End If
            If intLaneControl = 0 Then
                'The system could not generate a new {0}
                Logger.Error("throwCreateNewDependentRecordFailed(Booking Record because creation Of the lane information failed.)")
                throwCreateNewDependentRecordFailed("Booking Record because creation Of the lane information failed.")
            End If
            Dim iLTBookControl As Integer = db.tblLoadTenderBooks.Where(Function(x) x.LTBookLoadTenderControl = intLoadTenderControl).Select(Function(x) x.LTBookControl).FirstOrDefault()
            Logger.Information("Getting LTBookControl from tblLoadTenderBooks where LTBookLoadTenderControl = {intLoadTenderControl}", intLoadTenderControl)
            If iLTBookControl = 0 Then
                'The system could not generate a new {0}
                Logger.Error("throwCreateNewDependentRecordFailed(Booking Record because the dispatched booking And item details are Not available.)")
                throwCreateNewDependentRecordFailed("Booking Record because the dispatched booking And item details are Not available.")
            End If
            Dim oBookRet = db.spCreateBookFromLoadTender(iLTBookControl, intLaneControl, intOrigCompControl, intDestCompControl, Parameters.UserName).FirstOrDefault()
            Logger.Information("calling spCreateBookFromLoadTender(iLTBookControl: {iLTBookControl}, intLaneControl: {intLaneControl}, intOrigCompControl: {intOrigCompControl}, intDestCompControl: {intDestCompControl}, Parameters.UserName: {UserName})", iLTBookControl, intLaneControl, intOrigCompControl, intDestCompControl)
            If Not oBookRet Is Nothing Then
                If oBookRet.ErrNumber <> 0 Then
                    'The system could not generate a new {0}
                    Logger.Error("throwCreateNewDependentRecordFailed(Booking Record because creation Of the record failed For the following reason: {oBookRet.RetMsg})", oBookRet.RetMsg)
                    throwCreateNewDependentRecordFailed("Booking Record because creation Of the record failed For the following reason: " & oBookRet.RetMsg)
                End If
                intBookControl = oBookRet.BookControl
            End If
            If intBookControl = 0 Then
                'The system could not generate a new {0}
                Logger.Error("throwCreateNewDependentRecordFailed(Booking Record because creation Of the record failed.)")
                throwCreateNewDependentRecordFailed("Booking Record because creation of the record failed.")
            End If
        End Using

        Return intBookControl
    End Function

    Public Function createBookingFromTender(ByVal intLoadTenderControl As Integer, ByVal intLoadTenderTransTypeControl As Integer, ByRef sErrMsg As String) As Integer
        Dim iRet As Integer = 0
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                iRet = createBookingFromTender(intLoadTenderControl, db, intLoadTenderTransTypeControl, sErrMsg)
            Catch ex As FaultException
                Logger.Error(ex, "FaultException in createBookingFromTender")
                Throw
            Catch ex As Exception
                Logger.Error(ex, "Exception in createBookingFromTender")
                ManageLinqDataExceptions(ex, buildProcedureName("createBookingFromTender"))
            End Try
        End Using
        Return iRet
    End Function

    ''' <summary>
    ''' The calling procedure must tender/accept the load with spot rate logic using the bookcontrol number that is returned
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="oDispatch"></param>
    ''' <param name="sErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 04/04/2018
    '''     save the dispatch data, creates company, lane and booking information
    '''     retuns the new BookControl number.
    '''     the caller must generate a spot rate 
    ''' Modified by RHR for v-8.2.0.119 on 09/24/19 map carrier pro to shid if provided
    ''' </remarks>
    Public Function dispatchLoadTender(ByVal intLoadTenderControl As Integer, ByVal oDispatch As Models.Dispatch, Optional ByRef sErrMsg As String = "", Optional eLoadTenderType As LTTypeEnum = LTTypeEnum.P44) As Integer
        Dim iRet As Integer = 0
        'Me.BookDependencyResult = DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(0, source.BookItemBookLoadControl)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = intLoadTenderControl).FirstOrDefault()
                If oLoadTender Is Nothing Then Return 0
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                Dim intCompControl As Integer = Sec.getLECompControl()
                Dim oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, oDispatch.VendorSCAC).FirstOrDefault()
                Dim oShipCarrierData As LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult
                If oDispatch.ProviderSCAC <> oDispatch.VendorSCAC Then
                    oShipCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, oDispatch.ProviderSCAC).FirstOrDefault()
                End If
                If oCarrierData Is Nothing Then Return 0
                Dim sToParse As String = ""
                Dim dtParsed As Date = Date.Now()
                With oLoadTender
                    .LTLoadTenderTypeControl = eLoadTenderType 'dispatched 
                    .LTCarrierControl = oCarrierData.CarrierControl
                    .LTCarrierName = oCarrierData.CarrierName
                    .LTCarrierNumber = oCarrierData.CarrierNumber
                    .LTCarrierSCAC = oDispatch.VendorSCAC
                    If Not oShipCarrierData Is Nothing AndAlso oShipCarrierData.CarrierControl <> 0 Then
                        .LTBookShipCarrierName = oShipCarrierData.CarrierName
                        .LTBookShipCarrierNumber = oShipCarrierData.CarrierNumber
                    ElseIf oDispatch.ProviderSCAC <> oDispatch.VendorSCAC Then
                        .LTBookShipCarrierName = oDispatch.ProviderSCAC
                        .LTBookShipCarrierNumber = oDispatch.ProviderSCAC
                    End If
                    .LTBookShipCarrierProNumber = oDispatch.CarrierProNumber
                    'update the editable fields from the dispatch page
                    .LTBookOrigAddress1 = oDispatch.Origin.Address1
                    .LTBookOrigName = oDispatch.Origin.Name
                    .LTBookOrigContactName = oDispatch.Origin.Contact.ContactName
                    .LTBookOrigPhone = oDispatch.Origin.Contact.ContactPhone
                    .LTBookOrigContactEmail = oDispatch.Origin.Contact.ContactEmail
                    .LTBookDestAddress1 = oDispatch.Destination.Address1
                    .LTBookDestName = oDispatch.Destination.Name
                    .LTBookDestContactName = oDispatch.Destination.Contact.ContactName
                    .LTBookDestPhone = oDispatch.Destination.Contact.ContactPhone
                    .LTBookDestContactEmail = oDispatch.Destination.Contact.ContactEmail
                    .LTBookDateLoad = oDispatch.PickupDate
                    .LTBookCarrBookDate = oDispatch.PickupDate
                    sToParse = oDispatch.PickupDate.ToString("yyyy-MM-dd") & " " & oDispatch.PickupStartTime
                    If Date.TryParse(sToParse, dtParsed) Then
                        .LTBookCarrBookTime = dtParsed
                    End If
                    sToParse = oDispatch.PickupDate.ToString("yyyy-MM-dd") & " " & oDispatch.PickupEndTime
                    If Date.TryParse(sToParse, dtParsed) Then
                        .LTBookCarrBookEndTime = dtParsed
                    End If

                    .LTBookDateRequired = oDispatch.DeliveryDate
                    .LTBookCarrPODate = oDispatch.DeliveryDate
                    sToParse = oDispatch.DeliveryDate.ToString("yyyy-MM-dd") & " " & oDispatch.DeliveryStartTime
                    If Date.TryParse(sToParse, dtParsed) Then
                        .LTBookCarrPOTime = dtParsed
                    End If
                    sToParse = oDispatch.DeliveryDate.ToString("yyyy-MM-dd") & " " & oDispatch.DeliveryEndTime
                    If Date.TryParse(sToParse, dtParsed) Then
                        .LTBookCarrPOEndTime = dtParsed
                    End If

                    .LTSIBILLOFLADING = oDispatch.BillOfLading
                    .LTSIPURCHASEORDER = oDispatch.PONumber
                    .LTBookLoadPONumber = oDispatch.PONumber
                    'Modified by RHR for v-8.2.0.119 on 09/24/19 map carrier pro to shid if provided
                    If Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                        oDispatch.SHID = oDispatch.CarrierProNumber
                    End If
                    .LTBookSHID = oDispatch.SHID
                    .LTSICUSTOMERREFERENCE = oDispatch.OrderNumber
                    .LTBookCarrOrderNumber = oDispatch.OrderNumber
                    .LTSIPICKUP = oDispatch.PickupNumber
                    .LTMessage = oDispatch.sShipIDs
                    .LTSIPRO = oDispatch.CarrierProNumber
                    .LTSISYSTEMGENERATED = oDispatch.SystemGeneratedNbr
                    .LTSIEXTERNAL = oDispatch.EXTERNALNbr
                    .LTLTTTControl = oDispatch.LoadTenderTransTypeControl
                    If Not oDispatch.EmergencyContact Is Nothing Then
                        If oDispatch.LoadTenderTransTypeControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                            .LTBookDestEmergencyContactPhone = If(oDispatch.EmergencyContact.ContactPhone, "")
                            .LTBookDestEmergencyContactName = If(oDispatch.EmergencyContact.ContactName, "")
                            .LTLaneOriginAddressUse = True
                        Else
                            .LTBookOrigEmergencyContactPhone = If(oDispatch.EmergencyContact.ContactPhone, "")
                            .LTBookOrigEmergencyContactName = If(oDispatch.EmergencyContact.ContactName, "")
                            .LTLaneOriginAddressUse = False
                        End If

                    End If
                    .LTLAPIPickupNote = oDispatch.PickupNote
                    .LTLaneComments = oDispatch.DeliveryNote
                    .LTLaneCommentsConfidential = oDispatch.ConfidentialNote
                    .LTLLaneLengthUnit = oDispatch.LengthUnit
                    .LTLLaneWeightUnit = oDispatch.WeightUnit
                    .LTLAPIQuoteNumber = oDispatch.QuoteNumber
                    .LTLinearFeet = oDispatch.LinearFeet
                    'Response Data Returned
                    .LTSICapacityProviderBolUrl = oDispatch.RespCapacityProviderBolUrl
                    .LTSIPackingVisualizationUrl = oDispatch.RespPackingVisualizationUrl
                    .LTSIPickupNote = oDispatch.RespPickupNote
                    .LTSIPickupDateTime = oDispatch.RespPickupDateTime
                    .LTModDate = Date.Now
                    .LTModUser = Parameters.UserName
                End With
                'insert any info messages
                If Not oDispatch.InfoMessages Is Nothing AndAlso oDispatch.InfoMessages.Count() > 0 Then
                    For Each iMsg As Models.APIMessage In oDispatch.InfoMessages
                        insertLoadTenderMessage(oDispatch.LoadTenderControl, NGLMessageKeyRef.NGLAPIInfoMessages, oDispatch.SHID, iMsg.Severity, iMsg.Message, iMsg.Diagnostic, iMsg.Source, "Information", "")
                    Next
                End If
                'insert any error messages
                If Not oDispatch.Errors Is Nothing AndAlso oDispatch.Errors.Count() > 0 Then
                    For Each iMsg As Models.APIMessage In oDispatch.Errors
                        insertLoadTenderMessage(oDispatch.LoadTenderControl, NGLMessageKeyRef.NGLAPIInfoMessages, oDispatch.SHID, iMsg.Severity, iMsg.Message, iMsg.Diagnostic, iMsg.Source, oDispatch.ErrorCode, oDispatch.ErrorMessage)
                    Next
                End If
                'update the bid status codes
                Dim oBids = db.tblBids.Where(Function(x) x.BidLoadTenderControl = intLoadTenderControl).ToList()
                If Not oBids Is Nothing AndAlso oBids.Count > 0 Then
                    For Each b In oBids
                        'Modified by RHR for v-8.1 on 4/16/2018 we now use the BidControl to select the correct bid record more accurate than SCAC
                        'If b.BidVendor = oDispatch.VendorSCAC AndAlso b.BidCarrierSCAC = oDispatch.ProviderSCAC Then
                        '    b.BidStatusCode = BSCEnum.OpsAccept
                        'Else
                        '    b.BidStatusCode = BSCEnum.OpsReject
                        'End If
                        If b.BidControl = oDispatch.BidControl Then
                            b.BidStatusCode = BSCEnum.OpsAccept
                        Else
                            b.BidStatusCode = BSCEnum.OpsReject
                        End If
                    Next
                End If
                db.SubmitChanges()

                iRet = createBookingFromTender(intLoadTenderControl, db, oDispatch.LoadTenderTransTypeControl, sErrMsg)

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("dispatchLoadTender"))
            End Try
        End Using
        Return iRet
    End Function


#Region "New Load Tender for API Methods"

    ''' <summary>
    ''' Get the tblLoadTender information 
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.007 on 03/28/2023 new method for API Library
    ''' </remarks>
    Public Function getLoadTenderData(ByVal intLoadTenderControl As Integer) As LTS.tblLoadTender
        Dim oRet As New LTS.tblLoadTender()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = intLoadTenderControl).FirstOrDefault()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getLoadTenderData"))
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetLoadTenderBidCarrier(ByRef intCompControl As Integer, ByVal VendorSCAC As String) As LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult
        Dim oRet As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                If (intCompControl = 0) Then
                    Dim Sec As New NGLSecurityDataProvider(Parameters)
                    intCompControl = Sec.getLECompControl()
                End If
                oRet = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, VendorSCAC).FirstOrDefault()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenderBidCarrier"))
            End Try
        End Using
        Return oRet
    End Function

    Public Sub UpdateBidStatusCodes(ByVal intLoadTenderControl As Integer, ByVal intBidControl As Integer)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oBids = db.tblBids.Where(Function(x) x.BidLoadTenderControl = intLoadTenderControl).ToList()
                If Not oBids Is Nothing AndAlso oBids.Count > 0 Then
                    For Each b In oBids
                        'Modified by RHR for v-8.1 on 4/16/2018 we now use the BidControl to select the correct bid record more accurate than SCAC
                        'If b.BidVendor = oDispatch.VendorSCAC AndAlso b.BidCarrierSCAC = oDispatch.ProviderSCAC Then
                        '    b.BidStatusCode = BSCEnum.OpsAccept
                        'Else
                        '    b.BidStatusCode = BSCEnum.OpsReject
                        'End If
                        If b.BidControl = intBidControl Then
                            b.BidStatusCode = BSCEnum.OpsAccept
                        Else
                            b.BidStatusCode = BSCEnum.OpsReject
                        End If
                    Next
                End If
                db.SubmitChanges()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBidStatusCodes"))
            End Try
        End Using
        Return
    End Sub

    Public Function updateLoadTenderBid(ByVal oData As LTS.tblLoadTender) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                db.tblLoadTenders.Attach(oData, True)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("updateLoadTenderBid"))
            End Try
        End Using
        Return blnRet
    End Function




#End Region
    ''' <summary>
    ''' Reads the bid data and formats into a Dispatch Model 
    ''' </summary>
    ''' <param name="intBidControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 12/22/2018 
    '''     added logic to populate new fields to assist with dispatching 
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Lookup the default carrier contact using modified GetDefaultContactForCarrier()
    '''   and add those fields to the Dispatch Model
    ''' Modified by RHR for v-8.2 on 6/30/2019 
    '''  added logic to support new properties and default values for 
    '''  DAL.Models.Dispatch.AutoAcceptOnDispatch and 
    '''  DAL.Models.Dispatch.mailLoadTenderSheet flags
    '''  Default Values are:
    '''  Spot Rate:
    '''     AutoAcceptOnDispatch = True
    '''     EmailLoadTenderSheet = True
    '''  Post to DAT (dispatching dialog And switches are Not visible And must use defaults)
    '''     AutoAcceptOnDispatch = False
    '''     EmailLoadTenderSheet = False
    '''  Post to NEXTStop (dispatching dialog And switches are Not visible And must use defaults) 
    '''     AutoAcceptOnDispatch = False
    '''     EmailLoadTenderSheet = False
    '''  API Rate Selected:
    '''     AutoAcceptOnDispatch = True
    '''     EmailLoadTenderSheet = False
    '''  Tariff Rate Selected: 
    '''     AutoAcceptOnDispatch = False
    '''     EmailLoadTenderSheet = True
    ''' </remarks>
    Public Function getBidToDispatch(ByVal intBidControl As Integer) As Models.Dispatch
        Dim oRet As New Models.Dispatch
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.tblBid)(Function(t As LTS.tblBid) t.tblLoadTender)
                'db.LoadOptions = oDLO
                Dim oBid As LTS.tblBid = db.tblBids.Where(Function(x) x.BidControl = intBidControl).FirstOrDefault()
                If oBid Is Nothing OrElse oBid.BidLoadTenderControl = 0 Then Return Nothing
                Dim oLaneValues = db.spGetLaneLTDispatchValues(oBid.BidLoadTenderControl, True, Parameters.UserName).FirstOrDefault()
                Dim otblLoadTender As LTS.tblLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = oBid.BidLoadTenderControl).FirstOrDefault()

                ' Begin Modified by RHR for v-8.2 on 12/22/2018 
                oRet.BookControl = If(otblLoadTender.LTBookControl, 0)
                oRet.ModeTypeControl = oBid.BidBookModeTypeControl
                oRet.CarrierControl = oBid.BidCarrierControl
                oRet.CarrTarEquipMatControl = oBid.BidBookCarrTarEquipMatControl
                oRet.CarrTarEquipControl = oBid.BidBookCarrTarEquipControl
                oRet.FuelVariable = oBid.BidFuelVariable 'TODO: we need to find a way to populate this when the bid is created
                oRet.FuelUOM = oBid.BidFuelUOM 'TODO: we need to find a way to populate this when the bid is created
                oRet.DispatchLoadTenderType = otblLoadTender.LTLoadTenderTypeControl
                ' End Modified by RHR for v-8.2 on 12/22/2018 
                oRet.ProviderSCAC = oBid.BidCarrierSCAC
                oRet.VendorSCAC = oBid.BidVendor
                oRet.QuoteNumber = oBid.BidQuoteNumber
                oRet.SHID = If(String.IsNullOrWhiteSpace(oBid.BidSHID), If(String.IsNullOrWhiteSpace(otblLoadTender.LTBookSHID), otblLoadTender.LTBookConsPrefix, otblLoadTender.LTBookSHID), oBid.BidSHID)
                oRet.OrderNumber = otblLoadTender.LTBookCarrOrderNumber
                'Modified by RHR for v-8.2 on 12/6/2018 
                '  added DispatchBidType which maps to [BidBidTypeControl]
                oRet.BidControl = oBid.BidControl
                oRet.DispatchBidType = oBid.BidBidTypeControl
                Select Case oRet.DispatchBidType
                    Case BidTypeEnum.NextStop
                        oRet.AutoAcceptOnDispatch = False
                        oRet.EmailLoadTenderSheet = False
                    Case BidTypeEnum.P44
                        oRet.AutoAcceptOnDispatch = True
                        oRet.EmailLoadTenderSheet = False
                    Case BidTypeEnum.Spot
                        oRet.AutoAcceptOnDispatch = True
                        oRet.EmailLoadTenderSheet = True

                    Case Else
                        oRet.AutoAcceptOnDispatch = False
                        oRet.EmailLoadTenderSheet = True
                End Select
                oRet.LoadTenderControl = oBid.BidLoadTenderControl
                oRet.Origin = GetAddress(otblLoadTender, True)
                oRet.Destination = GetAddress(otblLoadTender, False)
                oRet.Requestor = oRet.Origin
                If Not otblLoadTender.LTBookDateLoad.HasValue Then
                    throwFieldRequiredException("Load Date")
                End If
                oRet.PickupDate = otblLoadTender.LTBookDateLoad
                If Not otblLoadTender.LTBookDateRequired.HasValue Then
                    throwFieldRequiredException("Required Date")
                End If
                oRet.DeliveryDate = otblLoadTender.LTBookDateRequired
                If Not otblLoadTender.LTBookTotalWgt.HasValue Then
                    throwFieldRequiredException("Total Weight")
                End If
                oRet.TotalWgt = otblLoadTender.LTBookTotalWgt
                oRet.TotalQty = If(otblLoadTender.LTBookTotalCases, 1)
                oRet.TotalPlts = If(otblLoadTender.LTBookTotalPL, 1)
                oRet.TotalCube = If(otblLoadTender.LTBookTotalCube, 0)
                oRet.PONumber = otblLoadTender.LTBookLoadPONumber
                oRet.WeightUnit = If(String.IsNullOrWhiteSpace(otblLoadTender.LTLLaneWeightUnit), "lbs", otblLoadTender.LTLLaneWeightUnit)
                oRet.LengthUnit = If(String.IsNullOrWhiteSpace(otblLoadTender.LTLLaneLengthUnit), "in", otblLoadTender.LTLLaneLengthUnit)
                oRet.LineHaul = oBid.BidLineHaul
                oRet.Fuel = oBid.BidFuelTotal
                oRet.TotalCost = oBid.BidTotalCost
                If otblLoadTender.LTLTTTControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                    oRet.EmergencyContact = GetContact(otblLoadTender, False, True)
                Else
                    oRet.EmergencyContact = GetContact(otblLoadTender, True, True)
                End If
                oRet.PickupNote = otblLoadTender.LTLAPIPickupNote
                If Not oLaneValues Is Nothing Then
                    oRet.DeliveryNote = If(String.IsNullOrWhiteSpace(oLaneValues.LTLaneComments), otblLoadTender.LTLaneComments, oLaneValues.LTLaneComments)
                    oRet.ConfidentialNote = If(String.IsNullOrWhiteSpace(oLaneValues.LTLaneCommentsConfidential), otblLoadTender.LTLaneCommentsConfidential, oLaneValues.LTLaneCommentsConfidential)
                    oRet.PickupStartTime = If(oLaneValues.LTBookCarrBookTime.HasValue, oLaneValues.LTBookCarrBookTime.Value.ToString("HH:mm"), If(otblLoadTender.LTBookCarrBookTime.HasValue, otblLoadTender.LTBookCarrBookTime.Value.ToString("HH:mm"), "08:00"))
                    oRet.PickupEndTime = If(oLaneValues.LTBookCarrBookEndTime.HasValue, oLaneValues.LTBookCarrBookEndTime.Value.ToString("HH:mm"), If(otblLoadTender.LTBookCarrBookEndTime.HasValue, otblLoadTender.LTBookCarrBookEndTime.Value.ToString("HH:mm"), "17:00"))
                    oRet.DeliveryStartTime = If(oLaneValues.LTBookCarrPOTime.HasValue, oLaneValues.LTBookCarrPOTime.Value.ToString("HH:mm"), If(otblLoadTender.LTBookCarrPOTime.HasValue, otblLoadTender.LTBookCarrPOTime.Value.ToString("HH:mm"), "08:00"))
                    oRet.DeliveryEndTime = If(oLaneValues.LTBookCarrPOEndTime.HasValue, oLaneValues.LTBookCarrPOEndTime.Value.ToString("HH:mm"), If(otblLoadTender.LTBookCarrPOEndTime.HasValue, otblLoadTender.LTBookCarrPOEndTime.Value.ToString("HH:mm"), "17:00"))
                    If oRet.Origin.Contact Is Nothing Then
                        oRet.Origin.Contact = New Models.Contact()
                    End If
                    If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactName) Then oRet.Origin.Contact.ContactName = oLaneValues.LTBookOrigContactName
                    If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactPhone) Then oRet.Origin.Contact.ContactPhone = oLaneValues.LTBookOrigPhone
                    If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactEmail) Then oRet.Origin.Contact.ContactEmail = oLaneValues.LTBookOrigContactEmail

                    If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactName) Then oRet.Destination.Contact.ContactName = oLaneValues.LTBookDestContactName
                    If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactPhone) Then oRet.Destination.Contact.ContactPhone = oLaneValues.LTBookDestPhone
                    If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactEmail) Then oRet.Destination.Contact.ContactEmail = oLaneValues.LTBookDestContactEmail

                    If oRet.EmergencyContact Is Nothing Then oRet.EmergencyContact = New Models.Contact()
                    If otblLoadTender.LTLTTTControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                        If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactName) Then oRet.EmergencyContact.ContactName = oLaneValues.LTBookDestEmergencyContactName
                        If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactPhone) Then oRet.EmergencyContact.ContactPhone = oLaneValues.LTBookDestEmergencyContactPhone
                    Else
                        If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactName) Then oRet.EmergencyContact.ContactName = oLaneValues.LTBookOrigEmergencyContactName
                        If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactPhone) Then oRet.EmergencyContact.ContactPhone = oLaneValues.LTBookOrigEmergencyContactPhone
                    End If
                End If
                Dim lAccessorials As New List(Of String)
                oRet.Items = GetItems(oBid.BidLoadTenderControl, db, lAccessorials)
                If (lAccessorials Is Nothing OrElse lAccessorials.Count() < 1) Then
                    oRet.Accessorials = GetAccessorials(oBid.BidLoadTenderControl, db)
                Else
                    oRet.Accessorials = lAccessorials.ToArray()
                End If


                'Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
                'lookup the default carrier contact and add those fields to the Model
                Dim oCarrier As New NGLCarrierContData(Parameters)
                Dim cont = oCarrier.GetDefaultContactForCarrier(oBid.BidCarrierControl)
                oRet.CarrierContact = New Models.Contact
                If Not cont Is Nothing Then
                    oRet.CarrierContact.ContactControl = cont.CarrierContControl
                    oRet.CarrierContact.ContactCarrierControl = cont.CarrierContCarrierControl
                    oRet.CarrierContact.ContactLECarControl = cont.CarrierContLECarControl
                    oRet.CarrierContact.ContactDefault = cont.CarrierContactDefault
                    oRet.CarrierContact.ContactScheduler = cont.CarrierContSchedContact
                    oRet.CarrierContact.ContactName = cont.CarrierContName
                    oRet.CarrierContact.ContactEmail = cont.CarrierContactEMail
                    oRet.CarrierContact.ContactTitle = cont.CarrierContTitle
                    oRet.CarrierContact.ContactPhone = cont.CarrierContactPhone
                    oRet.CarrierContact.ContactPhoneExt = cont.CarrierContPhoneExt
                    oRet.CarrierContact.ContactFax = cont.CarrierContactFax
                    oRet.CarrierContact.Contact800 = cont.CarrierContact800
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getBidToDispatch"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' ** Deprecated **
    ''' Gets BOL To Print by using the BookControl to get the LoadTender record, else uses the Book table directly
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 7/2/19 to call getBOLDispatchDataNoLT if there is no
    '''  LoadTender record and instead uses the Book, Lane, Comp, etc. tables
    ''' Deprecated by LVV on 8/9/2019
    '''  I don't see why we need this anymore since we are supposed to always go through the new method GetDispatchAndBOLReportData(), which calls the new stored procedures.
    '''  This method is no longer called from anywhere
    ''' </remarks>
    Public Function getBOLToPrintByBookControl(ByVal intBookControl As Integer) As Models.Dispatch
        throwDepreciatedException("This version of " & buildProcedureName("getBOLToPrintByBookControl") & " has been Deprecated. Please use the method GetDispatchAndBOLReportData() instead")
        Return Nothing

    End Function

    ''' <summary>
    ''' Stil being called from BOLController method GetBOL() - not sure why we still have this
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <returns></returns>
    Public Function getBOLToPrint(ByVal intLoadTenderControl As Integer) As Models.Dispatch
        Dim oRet As New Models.Dispatch
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                If intLoadTenderControl <> 0 Then
                    oRet = getBOLDispatchData(intLoadTenderControl, db)
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getBOLToPrint"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Called from above method getBOLToPrint(), which is stil being called from BOLController method GetBOL() - not sure why we still have this
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    Private Function getBOLDispatchData(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext) As Models.Dispatch
        Dim oRet As New Models.Dispatch
        Using operation = Logger.StartActivity("getBOLDispatchData(intLoadTenderControl: {intLoadTenderControl})", intLoadTenderControl)
            Dim otblLoadTender As LTS.tblLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = intLoadTenderControl).FirstOrDefault()
            If otblLoadTender Is Nothing OrElse otblLoadTender.LoadTenderControl = 0 Then Return Nothing
            Dim strCarrierSCAC = otblLoadTender.LTCarrierSCAC
            Dim oBid As LTS.tblBid = db.tblBids.Where(Function(x) x.BidLoadTenderControl = intLoadTenderControl And x.BidCarrierSCAC = strCarrierSCAC).FirstOrDefault()
            If oBid Is Nothing OrElse oBid.BidControl = 0 Then Return Nothing
            Dim oLaneValues = db.spGetLaneLTDispatchValues(intLoadTenderControl, True, Parameters.UserName).FirstOrDefault()
            'update the bill to company information 
            db.spUpdateLoadTenderBillToComp(intLoadTenderControl, Parameters.UserName)
            oRet.BOLLegalText = DirectCast(New NGLLegalEntityAdminData(Me.Parameters), NGLLegalEntityAdminData).getBOLLegalText()
            Dim strCarrier As String = strCarrierSCAC & " " & otblLoadTender.LTCarrierName
            If Not String.IsNullOrWhiteSpace(otblLoadTender.LTBookShipCarrierName) Then
                strCarrier = otblLoadTender.LTBookShipCarrierName
            End If

            oRet.ProviderSCAC = strCarrier
            oRet.VendorSCAC = oBid.BidVendor
            oRet.QuoteNumber = otblLoadTender.LTLAPIQuoteNumber
            oRet.SHID = If(String.IsNullOrWhiteSpace(otblLoadTender.LTBookSHID), otblLoadTender.LTBookConsPrefix, otblLoadTender.LTBookSHID)
            oRet.OrderNumber = otblLoadTender.LTBookCarrOrderNumber
            oRet.PONumber = otblLoadTender.LTSIPURCHASEORDER
            oRet.PickupNumber = otblLoadTender.LTSIPICKUP
            oRet.PickupNote = otblLoadTender.LTLAPIPickupNote
            oRet.DeliveryNote = otblLoadTender.LTLaneComments
            oRet.CarrierProNumber = otblLoadTender.LTSIPRO
            oRet.BidControl = oBid.BidControl
            oRet.LoadTenderControl = intLoadTenderControl
            oRet.Origin = GetAddress(otblLoadTender, True)
            oRet.Destination = GetAddress(otblLoadTender, False)
            oRet.Requestor = GetBillToAddress(otblLoadTender, oRet.Origin)
            oRet.PickupDate = otblLoadTender.LTBookDateLoad
            oRet.DeliveryDate = otblLoadTender.LTBookDateRequired
            oRet.TotalWgt = otblLoadTender.LTBookTotalWgt
            oRet.TotalQty = otblLoadTender.LTBookTotalCases
            oRet.TotalPlts = otblLoadTender.LTBookTotalPL
            oRet.TotalCube = otblLoadTender.LTBookTotalCube

            oRet.WeightUnit = otblLoadTender.LTLLaneWeightUnit
            oRet.LengthUnit = otblLoadTender.LTLLaneLengthUnit
            oRet.LineHaul = oBid.BidLineHaul
            oRet.Fuel = oBid.BidFuelTotal
            oRet.TotalCost = oBid.BidTotalCost
            If otblLoadTender.LTLTTTControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                oRet.EmergencyContact = GetContact(otblLoadTender, False, True)
            Else
                oRet.EmergencyContact = GetContact(otblLoadTender, True, True)
            End If
            If Not oLaneValues Is Nothing Then
                oRet.DeliveryNote = oLaneValues.LTLaneComments
                oRet.ConfidentialNote = oLaneValues.LTLaneCommentsConfidential
                oRet.PickupStartTime = If(oLaneValues.LTBookCarrBookTime.HasValue, oLaneValues.LTBookCarrBookTime.Value.ToString("HH:mm"), "08:00")
                oRet.PickupEndTime = If(oLaneValues.LTBookCarrBookEndTime.HasValue, oLaneValues.LTBookCarrBookEndTime.Value.ToString("HH:mm"), "17:00")
                oRet.DeliveryStartTime = If(oLaneValues.LTBookCarrPOTime.HasValue, oLaneValues.LTBookCarrPOTime.Value.ToString("HH:mm"), "08:00")
                oRet.DeliveryEndTime = If(oLaneValues.LTBookCarrPOEndTime.HasValue, oLaneValues.LTBookCarrPOEndTime.Value.ToString("HH:mm"), "17:00")

                If oRet.Origin.Contact Is Nothing Then
                    oRet.Origin.Contact = New Models.Contact()
                End If
                If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactName) Then oRet.Origin.Contact.ContactName = oLaneValues.LTBookOrigContactName
                If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactPhone) Then oRet.Origin.Contact.ContactPhone = oLaneValues.LTBookOrigPhone
                If String.IsNullOrWhiteSpace(oRet.Origin.Contact.ContactEmail) Then oRet.Origin.Contact.ContactEmail = oLaneValues.LTBookOrigContactEmail

                If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactName) Then oRet.Destination.Contact.ContactName = oLaneValues.LTBookDestContactName
                If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactPhone) Then oRet.Destination.Contact.ContactPhone = oLaneValues.LTBookDestPhone
                If String.IsNullOrWhiteSpace(oRet.Destination.Contact.ContactEmail) Then oRet.Destination.Contact.ContactEmail = oLaneValues.LTBookDestContactEmail

                If oRet.EmergencyContact Is Nothing Then oRet.EmergencyContact = New Models.Contact()
                If otblLoadTender.LTLTTTControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                    If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactName) Then oRet.EmergencyContact.ContactName = oLaneValues.LTBookDestEmergencyContactName
                    If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactPhone) Then oRet.EmergencyContact.ContactPhone = oLaneValues.LTBookDestEmergencyContactPhone

                Else
                    If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactName) Then oRet.EmergencyContact.ContactName = oLaneValues.LTBookOrigEmergencyContactName
                    If String.IsNullOrWhiteSpace(oRet.EmergencyContact.ContactPhone) Then oRet.EmergencyContact.ContactPhone = oLaneValues.LTBookOrigEmergencyContactPhone

                End If

            End If
            Dim lAccessorials As New List(Of String)
            oRet.Items = GetItems(intLoadTenderControl, db, lAccessorials)
            If (lAccessorials Is Nothing OrElse lAccessorials.Count() < 1) Then
                oRet.Accessorials = GetAccessorials(intLoadTenderControl, db)
            Else
                oRet.Accessorials = lAccessorials.ToArray()
            End If

            Logger.Information("otblLoadTender.LTCommCodeDescription: {otblLoadTender.LTCommCodeDescription}", otblLoadTender.LTCommCodeDescription)
            oRet.TempTypeDescription = otblLoadTender.LTCommCodeDescription
        End Using

        Return oRet
    End Function

#Region "Get BOL Report Data No LoadTender"

    ''' <summary>
    ''' Method to return data for both the BOL and Dispatch HTML Reports
    ''' This way any changes to mapping can be done in the stored procedures which consequently makes deploying any changes much easier
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <param name="blnIsBOL"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/17/19 for v-8.2
    ''' Modified by RHR for v-8.2.0.117 on 8/8/19
    '''   Added new logic to read dispath data
    '''   supports multi-pick and multi-stop
    ''' </remarks>
    Public Function GetDispatchAndBOLReportData(ByVal intBookControl As Integer, ByVal blnIsBOL As Boolean) As Models.Dispatch()
        Dim oRet As New List(Of Models.Dispatch)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
                Dim reports = db.spGetDispatchAndBOLReportData(intBookControl, Parameters.UserLEControl, blnIsBOL).ToArray()
                If reports?.Count > 0 Then
                    For Each r In reports
                        Dim d As New Models.Dispatch()
                        'ID INFO
                        d.ProviderSCAC = r.CarrierSCAC
                        d.VendorSCAC = r.VendorSCAC
                        d.CarrierName = r.CarrierName
                        d.SHID = r.SHID
                        d.OrderNumber = r.OrderNumbers
                        d.ItemOrderNumbers = r.BookItemOrderNumbers
                        d.PONumber = r.BookLoadPONumbers
                        d.CarrierProNumber = r.CarrierProNumber
                        d.QuoteNumber = r.QuoteNumber
                        d.PickupNumber = r.PickupNumber
                        d.BillOfLading = r.BillOfLading 'NEW
                        'LEGAL TEXT
                        d.BOLLegalText = r.BOLLegalText
                        d.DispatchLegalText = r.DispatchLegalText
                        'PLTS/WGT/ETC.
                        d.TotalWgt = If(r.TotalWgt.HasValue, r.TotalWgt.Value, 0)
                        d.TotalQty = If(r.TotalQty.HasValue, r.TotalQty.Value, 0)
                        d.TotalPlts = Math.Ceiling(If(r.TotalPlts.HasValue, r.TotalPlts.Value, 0))
                        d.TotalCube = If(r.TotalCube.HasValue, r.TotalCube.Value, 0)
                        d.WeightUnit = r.WeightUnit
                        d.LengthUnit = r.LengthUnit
                        d.TempTypeDescription = r.TempTypeDescription
                        'DATES
                        d.PickupDate = If(r.PickupDate.HasValue, r.PickupDate.Value, Date.MinValue)
                        d.DeliveryDate = If(r.DeliveryDate.HasValue, r.DeliveryDate.Value, Date.MinValue)
                        d.PickupStartTime = If(r.PickupStartTime.HasValue, r.PickupStartTime.Value.ToString("HH:mm"), "08:00")
                        d.PickupEndTime = If(r.PickupEndTime.HasValue, r.PickupEndTime.Value.ToString("HH:mm"), "17:00")
                        d.DeliveryStartTime = If(r.DeliveryStartTime.HasValue, r.DeliveryStartTime.Value.ToString("HH:mm"), "08:00")
                        d.DeliveryEndTime = If(r.DeliveryEndTime.HasValue, r.DeliveryEndTime.Value.ToString("HH:mm"), "17:00")
                        'NOTES
                        d.PickupNote = r.PickupNotes
                        d.DeliveryNote = r.DeliveryNotes
                        d.ConfidentialNote = r.ConfidentialNotes
                        'COSTS
                        d.LineHaul = If(r.LineHaul.HasValue, r.LineHaul.Value, 0)
                        d.Fuel = If(r.TotalFuel.HasValue, r.TotalFuel.Value, 0)
                        d.TotalCost = If(r.TotalCost.HasValue, r.TotalCost.Value, 0).ToString()
                        d.OtherCost = If(r.OtherCost.HasValue, r.OtherCost.Value, 0)
                        'CONTROLS
                        d.BidControl = If(r.BidControl.HasValue, r.BidControl.Value, 0)
                        d.LoadTenderControl = If(r.LoadTenderControl.HasValue, r.LoadTenderControl.Value, 0)
                        'ORIGIN
                        d.Origin = New Models.AddressBook()
                        d.Origin.Name = r.OrigName
                        d.Origin.Address1 = r.OrigAddress1
                        d.Origin.Address2 = r.OrigAddress2
                        d.Origin.Address3 = r.OrigAddress3
                        d.Origin.City = r.OrigCity
                        d.Origin.State = r.OrigState
                        d.Origin.Zip = r.OrigZip
                        d.Origin.Country = r.OrigCountry
                        d.Origin.Contact = New Models.Contact()
                        d.Origin.Contact.ContactName = r.OrigContactNames
                        d.Origin.Contact.ContactEmail = r.OrigContactEmails
                        d.Origin.Contact.ContactPhone = r.OrigContactPhones
                        'DESTINATION
                        d.Destination = New Models.AddressBook()
                        d.Destination.Name = r.DestName
                        d.Destination.Address1 = r.DestAddress1
                        d.Destination.Address2 = r.DestAddress2
                        d.Destination.Address3 = r.DestAddress3
                        d.Destination.City = r.DestCity
                        d.Destination.State = r.DestState
                        d.Destination.Zip = r.DestZip
                        d.Destination.Country = r.DestCountry
                        d.Destination.Contact = New Models.Contact()
                        d.Destination.Contact.ContactName = r.DestContactNames
                        d.Destination.Contact.ContactEmail = r.DestContactEmails
                        d.Destination.Contact.ContactPhone = r.DestContactPhones
                        'REQUESTOR
                        d.Requestor = New Models.AddressBook()
                        d.Requestor.Name = r.BillToName
                        d.Requestor.Address1 = r.BillToAddress1
                        d.Requestor.Address2 = r.BillToAddress2
                        d.Requestor.Address3 = r.BillToAddress3
                        d.Requestor.City = r.BillToCity
                        d.Requestor.State = r.BillToState
                        d.Requestor.Zip = r.BillToZip
                        d.Requestor.Country = r.BillToCountry
                        d.Requestor.Contact = New Models.Contact()
                        d.Requestor.Contact = d.Origin.Contact
                        d.Requestor.Contact.ContactPhone = r.BillToCompPhone ' Overwrite the phone number with the company phone number because YOLO
                        'EMERGENCY CONTACT
                        d.EmergencyContact = New Models.Contact()
                        If r.LTLTTTControl = NGLLookupDataProvider.LoadTenderTransType.Inbound Then
                            d.EmergencyContact.ContactName = r.DestEmergencyContactNames
                            d.EmergencyContact.ContactPhone = r.DestEmergencyContactPhones
                        Else
                            d.EmergencyContact.ContactName = r.OrigEmergencyContactNames
                            d.EmergencyContact.ContactPhone = r.OrigEmergencyContactPhones
                        End If
                        'ITEMS
                        Dim lItems As New List(Of Models.Item)
                        Dim sOrderNumbersFilter As String = ""
                        Dim sOrderNbrsSplit = r.OrderNumbers.Split(",")
                        If sOrderNbrsSplit?.Count() > 0 Then
                            Dim sDelimiter As String = ""
                            For Each sOnbr In sOrderNbrsSplit
                                sOrderNumbersFilter &= sDelimiter & "'" & sOnbr & "'"
                                sDelimiter = ", "
                            Next
                            Dim items = db.spGetDispatchAndBOLReportItemData(sOrderNumbersFilter, Parameters.UserLEControl, blnIsBOL).ToArray()
                            If items?.Count > 0 Then
                                For Each i In items
                                    Dim itm As New Models.Item()
                                    With itm
                                        .ItemNumber = i.ItemNumber
                                        .ItemDesc = i.ItemDesc
                                        .ItemFreightClass = i.ItemFreightClass
                                        .ItemWgt = If(i.ItemWgt.HasValue, i.ItemWgt.Value, 0)
                                        .ItemTotalPackages = If(i.ItemTotalPackages.HasValue, i.ItemTotalPackages.Value, 0)
                                        .ItemPackageType = i.ItemPackageType
                                        .ItemPieces = If(i.ItemPieces.HasValue, i.ItemPieces.Value, 0)
                                        .ItemStackable = If(i.ItemStackable.HasValue, i.ItemStackable.Value, False)
                                        .ItemLength = If(i.ItemLength.HasValue, i.ItemLength.Value, 0)
                                        .ItemWidth = If(i.ItemWidth.HasValue, i.ItemWidth.Value, 0)
                                        .ItemHeight = If(i.ItemHeight.HasValue, i.ItemHeight.Value, 0)
                                        .ItemNMFCItemCode = i.ItemNMFCItemCode
                                        .ItemNMFCSubCode = i.ItemNMFCSubCode
                                        .ItemIsHazmat = i.ItemIsHazmat
                                        .ItemHazmatID = i.ItemHazmatID
                                        .ItemHazmatClass = i.ItemHazmatClass
                                        .ItemHazmatPackingGroup = i.ItemHazmatPackingGroup
                                        .ItemHazmatProperShipName = i.ItemHazmatProperShipName
                                        .ItemCube = If(i.ItemCube.HasValue, i.ItemCube.Value, 0)
                                    End With
                                    lItems.Add(itm)
                                Next
                            End If
                        End If
                        d.Items = lItems.ToArray()
                        'ACCESSORIALS
                        Dim lAccessorials As New List(Of String)
                        If Not String.IsNullOrWhiteSpace(sOrderNumbersFilter) Then
                            Dim accessorials = db.spGetDispatchAndBOLReportAccessorialData(sOrderNumbersFilter, Parameters.UserLEControl, blnIsBOL).ToArray()
                            If accessorials?.Count > 0 Then
                                For Each a In accessorials

                                    If blnIsBOL Then
                                        lAccessorials.Add(a.Name)
                                    Else
                                        lAccessorials.Add(a.Name & ": " & FormatCurrency(a.Val))
                                    End If

                                Next
                            End If
                        End If
                        d.Accessorials = lAccessorials.ToArray()


                        'Book Accesorials
                        Dim filters As New Models.AllFilters With {.ParentControl = intBookControl}
                        Dim ct As Integer = 0
                        Dim TotalFees As Integer
                        Dim oAItem As New Models.Item()
                        Dim oAccs As LTS.vBookAccessorial() = oBookAccDAL.GetBookAccessorials(filters, ct)
                        If oAccs IsNot Nothing AndAlso oAccs.Any() Then
                            oAItem = New Models.Item()

                            ' Map each vBookAccessorial to BookAccessorial
                            For Each acc As LTS.vBookAccessorial In oAccs
                                Dim bAcc As New BookAccessorial()
                                bAcc.BookAcssControl = acc.BookAcssControl
                                bAcc.BookAcssBookControl = acc.BookAcssBookControl
                                bAcc.BookAcssNACControl = acc.BookAcssNACControl
                                bAcc.BookAcssValue = acc.BookAcssValue
                                oAItem.oBookAccessorial.Add(bAcc)
                                TotalFees += acc.BookAcssValue
                            Next
                        End If
                        d.Fees = TotalFees

                        oRet.Add(d)
                    Next
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDispatchAndBOLReportData"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function



    ''' <summary>
    ''' ** Deprecated **
    ''' Uses the Book, Lane, Comp, etc. tables to get the BOL Data when no LoadTender record exists
    ''' Variation on getBOLDispatchData()
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 7/2/19 for v-8.2
    ''' Deprecated by LVV on 8/9/2019
    '''  I don't see why we need this anymore since we are supposed to always go through the new method GetDispatchAndBOLReportData(), which calls the new stored procedures.
    '''  This method is no longer called from anywhere
    ''' </remarks>
    Private Function getBOLDispatchDataNoLT(ByVal intBookControl As Integer) As Models.Dispatch
        throwDepreciatedException("This version of " & buildProcedureName("getBOLDispatchDataNoLT") & " has been Deprecated. Please use the method GetDispatchAndBOLReportData() instead")
        Return Nothing

    End Function

    ''' <summary>
    ''' ** Deprecated **
    ''' Uses the BookItem, BookPackage, BookAccessorial, BookFees, etc. tables to get the BOL Item Data when no LoadTender record exists
    ''' Variation on GetItems()
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <param name="db"></param>
    ''' <param name="sAccessorial"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 7/2/19 for v-8.2
    ''' Deprecated by LVV on 8/9/2019
    '''  I don't see why we need this anymore since we are supposed to always go through the new method GetDispatchAndBOLReportData(), which calls the new stored procedures.
    '''  This method is no longer called from anywhere
    ''' </remarks>
    Public Function GetItemsNoLT(ByVal intBookControl As Integer, ByRef db As NGLMASIntegrationDataContext, Optional ByRef sAccessorial As List(Of String) = Nothing) As Models.Item()
        throwDepreciatedException("This version of " & buildProcedureName("GetItemsNoLT") & " has been Deprecated. Please use the method GetDispatchAndBOLReportData() instead")
        Return Nothing

    End Function

    ''' <summary>
    ''' ** Deprecated **
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.117 on 8/2/2019
    '''  added new logic to get data when load tender is missing
    ''' Deprecated by LVV on 8/9/2019
    '''  I don't see why we need this anymore since we are supposed to always go through the new method GetDispatchAndBOLReportData(), which calls the new stored procedures.
    '''  This method is no longer called from anywhere
    ''' </remarks>
    Public Function getDispatchDataToPrintByBookControl(ByVal intBookControl As Integer) As Models.Dispatch
        throwDepreciatedException("This version of " & buildProcedureName("getDispatchDataToPrintByBookControl") & " has been Deprecated. Please use the method GetDispatchAndBOLReportData() instead")
        Return Nothing

    End Function

    ''' <summary>
    ''' ** Deprecated **
    ''' Reverse engineered method dispatchLoadTender()
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="db"></param>
    ''' <returns>
    ''' Deprecated by LVV on 8/9/2019
    '''  I don't see why we need this anymore since we are supposed to always go through the new method GetDispatchAndBOLReportData(), which calls the new stored procedures.
    '''  This method is no longer called from anywhere
    ''' </returns>
    Public Function getDispatchData(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext) As Models.Dispatch
        throwDepreciatedException("This version of " & buildProcedureName("getDispatchData") & " has been Deprecated. Please use the method GetDispatchAndBOLReportData() instead")
        Return Nothing

    End Function

#End Region

    Public Function GetContact(ByRef oLT As LTS.tblLoadTender, Optional ByVal blnOrigin As Boolean = True, Optional ByVal blnEmergency As Boolean = False) As Models.Contact
        Dim oRet As New Models.Contact
        If oLT Is Nothing Then Return oRet
        With oLT
            If Not blnEmergency Then
                oRet.ContactName = If(blnOrigin = True, .LTBookOrigContactName, .LTBookDestContactName)
                oRet.ContactPhone = If(blnOrigin = True, .LTBookOrigPhone, .LTBookDestPhone)
                oRet.ContactEmail = If(blnOrigin = True, .LTBookOrigContactEmail, .LTBookDestContactEmail)
            Else
                oRet.ContactName = If(blnOrigin = True, .LTBookOrigEmergencyContactName, .LTBookDestEmergencyContactName)
                oRet.ContactPhone = If(blnOrigin = True, .LTBookOrigEmergencyContactPhone, .LTBookDestEmergencyContactPhone)
            End If
        End With
        Return oRet
    End Function

    Public Function GetAddress(ByRef oLT As LTS.tblLoadTender, Optional ByVal blnOrigin As Boolean = True) As Models.AddressBook
        Dim oRet As New Models.AddressBook
        If oLT Is Nothing Then Return oRet
        With oLT
            oRet.Address1 = If(blnOrigin = True, .LTBookOrigAddress1, .LTBookDestAddress1)
            oRet.Address2 = If(blnOrigin = True, .LTBookOrigAddress2, .LTBookDestAddress2)
            oRet.Address3 = If(blnOrigin = True, .LTBookOrigAddress3, .LTBookDestAddress3)
            oRet.City = If(blnOrigin = True, .LTBookOrigCity, .LTBookDestCity)
            oRet.Contact = GetContact(oLT, blnOrigin)
            oRet.Country = If(blnOrigin = True, .LTBookOrigCountry, .LTBookDestCountry)
            oRet.Name = If(blnOrigin = True, .LTBookOrigName, .LTBookDestName)
            oRet.State = If(blnOrigin = True, .LTBookOrigState, .LTBookDestState)
            oRet.Zip = If(blnOrigin = True, .LTBookOrigZip, .LTBookDestZip)
        End With

        Return oRet
    End Function

    Public Function GetBillToAddress(ByRef oLT As LTS.tblLoadTender, ByRef Orig As Models.AddressBook) As Models.AddressBook
        Dim oRet As New Models.AddressBook
        If oLT Is Nothing Then Return oRet
        With oLT
            oRet.Contact = Orig.Contact
            If String.IsNullOrWhiteSpace(oLT.LTBillToCompNumber) Or String.IsNullOrWhiteSpace(oLT.LTBillToCompAddress1) Then
                oRet.Address1 = Orig.Address1
                oRet.Address2 = Orig.Address2
                oRet.Address3 = Orig.Address3
                oRet.City = Orig.City
                oRet.Country = Orig.Country
                oRet.Name = Orig.Name
                oRet.State = Orig.State
                oRet.Zip = Orig.Zip
            Else
                oRet.Address1 = .LTBillToCompAddress1
                oRet.Address2 = .LTBillToCompAddress2
                oRet.City = .LTBillToCompCity
                oRet.Country = .LTBillToCompCountry
                oRet.Name = .LTBillToCompName
                oRet.State = .LTBillToCompState
                oRet.Zip = .LTBillToCompZip
            End If
        End With

        Return oRet
    End Function

    Public Function GetAccessorials(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext) As String()
        Dim oRet As String()
        Using Logger.StartActivity("GetAccessorials(intloadTenderControl: {intLoadTenderControl})", intLoadTenderControl)
            Dim oFees As Models.NGLAPIAccessorial() = GetAccessorialsByLoadTender(intLoadTenderControl, db)
            If Not oFees Is Nothing AndAlso oFees.Count < 1 Then Return oRet
            oRet = oFees.Select(Function(x) x.Code).ToArray()
        End Using

        Return oRet
    End Function

    ''' <summary>
    ''' Used for dispatching only.  This method reads from BookPackage if records exist 
    ''' or from  tblLoadTenderItems if no records are found in BookPackage 
    ''' </summary>
    ''' <param name="intLoadTenderControl"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 3/1/2019
    '''   added new logic to read from BookPackage by default if a booking record exists
    '''   because when a bookcontrol is found we log the original item details in tblLoadTenderItems
    '''   and not the packages,  Note for this to work the tblLoadTenderBooks must be properly populated
    ''' </remarks>
    Public Function GetItems(ByVal intLoadTenderControl As Integer, ByRef db As NGLMASIntegrationDataContext, Optional ByRef sAccessorial As List(Of String) = Nothing) As Models.Item()
        Dim oRet As New List(Of Models.Item)
        Using Logger.StartActivity("GetItems(intLoadTenderControl: {intLoadTenderControl})", intLoadTenderControl)
            Dim intLTBookControls() As Integer = db.tblLoadTenderBooks.Where(Function(x) x.LTBookLoadTenderControl = intLoadTenderControl).Select(Function(x) x.LTBookControl).ToArray()
            Dim oLoadTenderBooks() As LTS.tblLoadTenderBook = db.tblLoadTenderBooks.Where(Function(x) x.LTBookLoadTenderControl = intLoadTenderControl).ToArray()
            If oLoadTenderBooks Is Nothing OrElse oLoadTenderBooks.Count < 1 Then Return oRet.ToArray() 'no items
            Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
            Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
            Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
            Dim oPkgs As New List(Of LTS.vBookPackage)
            Dim oItems As New List(Of LTS.tblLoadTenderItem)
            If sAccessorial Is Nothing Then sAccessorial = New List(Of String)
            Dim blnUsingPkgs As Boolean = False
            Dim blnUsingLoadTenderItems = False
            Dim oAItem As New Models.Item()
            For Each oBook In oLoadTenderBooks
                If Not blnUsingLoadTenderItems AndAlso oBook.LTBookBookControl <> 0 Then
                    Dim filters As New Models.AllFilters With {.ParentControl = oBook.LTBookBookControl}
                    Dim ct As Integer
                    Dim oThesePkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
                    If Not oThesePkgs Is Nothing AndAlso oThesePkgs.Count() > 0 Then
                        blnUsingPkgs = True 'once we start using packages we can no longer read from tblLoadTenderItems
                        oPkgs.AddRange(oThesePkgs)
                    End If
                    ct = 0
                    filters = New Models.AllFilters With {.ParentControl = oBook.LTBookBookControl} 'we must clear the filter to be sure we have good data
                    Dim oAccs As LTS.vBookAccessorial() = oBookAccDAL.GetBookAccessorials(filters, ct)
                    If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                        oBookAccessorial.AddRange(oAccs)
                        sAccessorial.AddRange(oAccs.Select(Function(x) x.NACCode).ToList())
                    End If

                    If oAccs IsNot Nothing AndAlso oAccs.Any() Then
                        oAItem = New Models.Item()

                        ' Map each vBookAccessorial to BookAccessorial
                        For Each acc As LTS.vBookAccessorial In oAccs
                            Dim bAcc As New BookAccessorial()
                            bAcc.BookAcssControl = acc.BookAcssControl
                            bAcc.BookAcssBookControl = acc.BookAcssBookControl
                            bAcc.BookAcssNACControl = acc.BookAcssNACControl
                            bAcc.BookAcssValue = acc.BookAcssValue
                            oAItem.oBookAccessorial.Add(bAcc)
                        Next


                    End If
                End If
                If Not blnUsingPkgs Then
                    Dim iLTBookControl = oBook.LTBookControl
                    If iLTBookControl <> 0 Then
                        'we assume rate shopping no booking so just read the LoadTenderItem table
                        Dim oTheseItems() As LTS.tblLoadTenderItem = db.tblLoadTenderItems.Where(Function(x) intLTBookControls.Contains(x.LTItemLTBookControl)).ToArray
                        If Not oTheseItems Is Nothing AndAlso oTheseItems.Count > 0 Then
                            blnUsingLoadTenderItems = True 'once we start using LoadTenderItems we can no longer read from BookPackages
                            oItems.AddRange(oTheseItems)
                        End If
                    End If
                End If
            Next
            If blnUsingPkgs Then
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    oRet = (From i In oPkgs Select New Models.Item With {
                       .ItemWgt = i.BookPkgWeight.ToString(),
                       .ItemWeightUnit = "lbs",
                       .ItemFreightClass = i.BookPkgFAKClass,
                       .ItemTotalPackages = i.BookPkgCount,
                       .ItemPieces = i.BookPkgCount, 'NOTE: THE NEW SP WILL RETURN A DIFFERENT OBJECT WHICH WILL HAVE ITEM QTY
                       .ItemDesc = i.BookPkgDescription,
                       .ItemLength = i.BookPkgLength,
                       .ItemWidth = i.BookPkgWidth,
                       .ItemHeight = i.BookPkgHeight,
                       .ItemPackageType = i.PackageType,
                       .ItemNMFCItemCode = i.BookPkgNMFCClass,
                       .ItemNMFCSubCode = i.BookPkgNMFCSubClass,
                       .ItemStackable = i.BookPkgStackable,
                       .oBookAccessorial = oBookAccessorial.Select(Function(acc) New BookAccessorial With {
                            .BookAcssControl = acc.BookAcssControl,
                            .BookAcssBookControl = acc.BookAcssBookControl,
                            .BookAcssNACControl = acc.BookAcssNACControl,
                            .BookAcssValue = acc.BookAcssValue
                        }).ToList()
                   }).ToList()
                End If
                'oRet.Add(oAItem)
            Else
                If Not oItems Is Nothing AndAlso oItems.Count > 0 Then
                    Dim oLookUpData As New NGLLookupDataProvider(Parameters)
                    Dim oPalletTypes = oLookUpData.GetViewLookupStaticList(NGLLookupDataProvider.StaticLists.PalletType, NGLLookupDataProvider.ListSortType.Control)
                    For Each itm In oItems
                        Dim oItem = New Models.Item
                        With oItem
                            .ItemDesc = itm.LTItemDescription
                            .ItemFreightClass = itm.LTItemFAKClass
                            'todo: add look up for hazmat
                            '.ItemHazmatClass = getHazmatClass(itm.LTItemBookHazControl)
                            Try
                                .ItemHeight = System.Convert.ToDecimal(itm.LTItemQtyHeight)
                            Catch ex As System.OverflowException
                                .ItemHeight = 12
                            End Try
                            Try
                                .ItemLength = System.Convert.ToDecimal(itm.LTItemQtyLength)
                            Catch ex As System.OverflowException
                                .ItemLength = 12
                            End Try
                            .ItemNMFCItemCode = itm.LTItemNMFCClass
                            .ItemNMFCSubCode = itm.LTItemNMFCSubClass
                            .ItemNumber = itm.LTItemItemNumber
                            If oPalletTypes.Any(Function(x) x.Control = itm.LTItemPalletTypeID) Then
                                .ItemPackageType = oPalletTypes.Where(Function(x) x.Control = itm.LTItemPalletTypeID).Select(Function(x) x.Description).FirstOrDefault()
                            Else
                                .ItemPackageType = "PLT"
                            End If
                            .ItemPieces = If(itm.LTItemQtyOrdered, 1)
                            .ItemStackable = itm.LTItemStackable
                            Try
                                .ItemTotalPackages = System.Convert.ToInt32(itm.LTItemPallets)
                            Catch ex As System.OverflowException
                                .ItemTotalPackages = 1
                            End Try
                            Try
                                .ItemWgt = System.Convert.ToDecimal(If(itm.LTItemWeight, 1))
                            Catch ex As Exception
                                .ItemWgt = 10
                            End Try
                            Try
                                .ItemWidth = System.Convert.ToDecimal(itm.LTItemQtyWidth)
                            Catch ex As System.OverflowException
                                .ItemWidth = 12
                            End Try
                        End With
                        oItem.oBookAccessorial = oBookAccessorial.Select(Function(acc) New BookAccessorial With {
                            .BookAcssControl = acc.BookAcssControl,
                            .BookAcssBookControl = acc.BookAcssBookControl,
                            .BookAcssNACControl = acc.BookAcssNACControl,
                            .BookAcssValue = acc.BookAcssValue
                        }).ToList()
                        oRet.Add(oItem)
                    Next
                End If
                'oRet.Add(oAItem)
            End If
        End Using

        Return oRet.ToArray()
    End Function

    ''' <summary>
    ''' Called from BOLController method GetBOLAdditionalServices()
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.119 on 10/01/2019
    '''     we now call spGetBOLAccessorialString which expects a BookControl number
    ''' </remarks>
    Public Function GetBOLAccessorialString(ByVal iBookControl As Integer) As String
        Dim sRet As String = ""
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oRet = db.spGetBOLAccessorialString(iBookControl).FirstOrDefault()
                If Not oRet Is Nothing Then
                    sRet = oRet.BOLAccessorialString
                End If

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBOLAccessorialString"), db)
            End Try
        End Using
        Return sRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="iNMNMTControl"></param>
    ''' <param name="sSHID"></param>
    ''' <param name="sSeverity"></param>
    ''' <param name="sMessage"></param>
    ''' <param name="sDiagnostic"></param>
    ''' <param name="sSource"></param>
    ''' <param name="sErrorCode"></param>
    ''' <param name="sErrorMessage"></param>
    ''' <param name="sErrMsg"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2  on 8/8/2019 
    '''     activated spAddLoadTenderMessage used to rie the messages to tblNGLMessage
    ''' </remarks>
    Public Sub insertLoadTenderMessage(ByVal iLoadTenderControl As Integer,
                                       ByVal iNMNMTControl As Utilities.NGLMessageKeyRef,
                                       ByVal sSHID As String,
                                       ByVal sSeverity As String,
                                       ByVal sMessage As String,
                                       ByVal sDiagnostic As String,
                                       ByVal sSource As String,
                                       ByVal sErrorCode As String,
                                       ByVal sErrorMessage As String,
                                       Optional ByRef sErrMsg As String = "")

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                db.spAddLoadTenderMessage(iLoadTenderControl, iNMNMTControl, sSHID, sSeverity, sMessage, sDiagnostic, sSource, sErrorCode, sErrorMessage, Parameters.UserName)
            Catch ex As Exception
                sErrMsg = String.Concat("Insert Error: ", buildProcedureName("insertLoadTenderMessage"), " ", ex.Message)
                Utilities.SaveAppError(sErrMsg, Me.Parameters)
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Add the quotes to the bid table
    ''' </summary>
    ''' <param name="ltControl"></param>
    ''' <param name="oResponse"></param>
    Public Sub InsertLTRateQuoteBids(ByVal ltControl As Integer, ByVal oResponse As List(Of P44.rateQuoteResponse))
        Using operation = Logger.StartActivity("InsertLTRateQuoteBids(ltControl: {ltControl}, oResponse: {@oResponse})", ltControl, oResponse)
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try
                    Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = ltControl).FirstOrDefault()
                    Dim sRetMSg As String = ""
                    If Not CreateNGLAPIBid(oResponse, oLoadTender.LoadTenderControl, oLoadTender.LTBookSHID, oLoadTender.LTBookOrigState, oLoadTender.LTBookDestState, 0, sRetMSg, 0, oLoadTender.LTBookDateRequired, BSCEnum.Quoted) Then
                        throwUnExpectedFaultException(sRetMSg)
                    End If
                Catch ex As FaultException
                    Logger.Error(ex, "InsertLTRateQuoteBids")
                    Throw
                Catch ex As Exception
                    Logger.Error(ex, "InsertLTRateQuoteBids")
                    ManageLinqDataExceptions(ex, buildProcedureName("InsertLTRateQuoteBids"))
                End Try
            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Wrapper method
    ''' Inserts a new Rate Shopping Quote into the tblLoadTender and dependent child tables using the 
    ''' NGL API settings and quote request
    ''' </summary>
    ''' <param name="oP44Request"></param>
    ''' <param name="BookSHiD"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 06/28/2023
    '''     added logic to support temperature selection when Rate Shopping 
    ''' </remarks>
    Public Function InsertLoadTenderRateQuote(ByVal oP44Request As P44.RateRequest,
                                              ByVal BookSHiD As String,
                                              ByVal BookCarrOrderNumber As String,
                                              ByRef order As Models.RateRequestOrder,
                                              Optional ByVal BookControl As Integer = 0,
                                              Optional ByVal BookConsPrefix As String = "") As Integer
        Using Logger.StartActivity("InsertLoadTenderRateQuote(P44RateRequest: {@P44RateRequest}, SHID: {SHID}, OrderNumber: {OrderNumber}, BookControl: {BookControl}, BookConsPrefix: {BookConsPrefix}", oP44Request, BookSHiD, BookCarrOrderNumber, BookControl, BookConsPrefix)
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Return InsertLoadTenderRateQuote(db, oP44Request, BookSHiD, BookCarrOrderNumber, order, BookControl, BookConsPrefix)
            End Using
        End Using

    End Function

    ''' <summary>
    ''' Inserts a new Rate Shopping Quote into the tblLoadTender and dependent child tables using the 
    ''' NGL API settings and quote request
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="oP44Request"></param>
    ''' <param name="BookSHiD"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 06/28/2023
    '''     added logic to support temperature selection when Rate Shopping 
    ''' </remarks>
    Public Function InsertLoadTenderRateQuote(ByRef db As NGLMASIntegrationDataContext,
                                              ByVal oP44Request As P44.RateRequest,
                                              ByVal BookSHiD As String,
                                              ByVal BookCarrOrderNumber As String,
                                              ByRef order As Models.RateRequestOrder,
                                              Optional ByVal BookControl As Integer = 0,
                                              Optional ByVal BookConsPrefix As String = "") As Integer
        Dim oLoadTender As New LTS.tblLoadTender()
        Using Logger.StartActivity("InsertLoadTenderRateQuote: SHID:{SHID}, OrderNumber: {OrderNumber}", BookSHiD, BookCarrOrderNumber)
            Try
                oLoadTender = createLoadTenderQuote(oP44Request, order)
                If oLoadTender Is Nothing Then Return 0
                db.tblLoadTenders.InsertOnSubmit(oLoadTender)
                db.SubmitChanges()
                If Not oLoadTender.LoadTenderControl = 0 Then
                    Try
                        Dim oLoadTenderBook = createLoadTenderBook(oP44Request, oLoadTender.LoadTenderControl, BookSHiD, BookCarrOrderNumber, BookControl, BookConsPrefix)
                        If Not oLoadTenderBook Is Nothing Then
                            db.tblLoadTenderBooks.InsertOnSubmit(oLoadTenderBook)
                            db.SubmitChanges()
                            If Not oLoadTenderBook.LTBookControl = 0 Then
                                'insert the item details
                                Try
                                    Dim oLoadTenderItems() = createLoadTenderItems(oP44Request, oLoadTenderBook.LTBookControl)
                                    If Not oLoadTenderItems Is Nothing AndAlso oLoadTenderItems.Count > 0 Then
                                        db.tblLoadTenderItems.InsertAllOnSubmit(oLoadTenderItems)
                                        db.SubmitChanges()
                                    End If
                                Catch ex As Exception
                                    Logger.Error(ex, "InsertLoadTenderRateQuote")
                                    'do not fail just because we cannot create a LoadTenderItem records for the quote just save the error to the log
                                    Dim sMsg As String = "Warning! " & buildProcedureName("InsertLoadTenderRateQuote") & " could not save load tender booking items because: " & ex.Message
                                    SaveAppError(sMsg, Parameters)
                                End Try
                                'now save the accessorial
                                If Not oP44Request.accessorials Is Nothing AndAlso oP44Request.accessorials.Count() > 0 Then
                                    Try
                                        Dim lAccessorial As New List(Of LTS.tblLoadTenderBookAPIFee)
                                        Dim intCodes As New List(Of Integer) From {NGLLookupDataProvider.NGLAPICodeTypes.ChargeCodes, NGLLookupDataProvider.NGLAPICodeTypes.DeliveryAccessorials, NGLLookupDataProvider.NGLAPICodeTypes.NonSpecificAccessorials, NGLLookupDataProvider.NGLAPICodeTypes.PickUpAccessorials}
                                        For Each acc In oP44Request.accessorials
                                            'check for a valid code
                                            Dim intNACControl As Integer = db.tblNGLAPICodes.Where(Function(x) x.NACCode = acc And intCodes.Contains(x.NACNACodeTypeControl)).Select(Function(x) x.NACControl).FirstOrDefault()
                                            Logger.Information("Accessorial Code: {AccessorialCode} NACControl: {NACControl}", acc, intNACControl)
                                            If intNACControl <> 0 Then
                                                'we have a valid code so add it to the database
                                                lAccessorial.Add(New LTS.tblLoadTenderBookAPIFee With {.BookFeesModDate = Date.Now(), .BookFeesModUser = Parameters.UserName, .LTBAFLTBookControl = oLoadTenderBook.LTBookControl, .LTBAFNACControl = intNACControl})
                                            Else
                                                Logger.Warning("Invalid Accessorial Code: {AccessorialCode}", acc)
                                            End If

                                        Next
                                        If Not lAccessorial Is Nothing AndAlso lAccessorial.Count > 0 Then
                                            db.tblLoadTenderBookAPIFees.InsertAllOnSubmit(lAccessorial)
                                            db.SubmitChanges()
                                        End If
                                    Catch ex As Exception
                                        Logger.Error(ex, "InsertLoadTenderRateQuote")
                                        'do not fail just because we cannot create a accessorial records for the quote just save the error to the log
                                        Dim sMsg As String = "Warning! " & buildProcedureName("InsertLoadTenderRateQuote") & " could not save accessorial fees because: " & ex.Message
                                        SaveAppError(sMsg, Parameters)
                                    End Try
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        'do not fail just because we cannot create a LoadTenderBook record for the quote
                        Dim sMsg As String = "Warning! " & buildProcedureName("InsertLoadTenderRateQuote") & " could not save load tender booking because: " & ex.Message
                        SaveAppError(sMsg, Parameters)
                    End Try
                End If
                Return oLoadTender.LoadTenderControl
            Catch ex As FaultException
                Logger.Error(ex, "InsertLoadTenderRateQuote")
                Throw
            Catch ex As Exception
                Logger.Error(ex, "InsertLoadTenderRateQuote")
                ManageLinqDataExceptions(ex, buildProcedureName("InsertLoadTenderRateQuote"))
            End Try
        End Using
        Return 0
    End Function

#Region "Load Tender Logs "


#End Region

#Region "NEXTStop"

    ''' <summary>
    ''' Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
    ''' Next Stop Posted Loads that have not yet received any bids
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns>LTS.vNSAvailablePendingLoad()</returns>
    ''' <remarks>
    ''' Added By LVV on 12/23/16 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetvNSAvailablePendingLoads(ByRef RecordCount As Integer,
                                                Optional ByVal filterWhere As String = "",
                                                Optional ByVal sortExpression As String = "",
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000,
                                                Optional ByVal skip As Integer = 0,
                                                Optional ByVal take As Integer = 0) As LTS.vNSAvailablePendingLoad()
        Dim oRetData As LTS.vNSAvailablePendingLoad()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1
                Dim oQuery = From t In db.vNSAvailablePendingLoads Select t
                '"(CarrTarDiscountMinValue < 75) And (CarrTarDiscountWgtLimit > 50)"
                '"(BidStatus = 1) And (BidCarrierControl = CarrierControl)"
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing
                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()
                Return oRetData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNSAvailablePendingLoads"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
    ''' Next Stop Posted Loads that have not yet received any bids from the provided Carrier
    ''' Basically, all the available loads this Carrier does not have any active bids on
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filters"></param>
    ''' <returns>LTS.vNSAvailablePendingLoad()</returns>
    ''' <remarks>
    ''' Added By LVV on 10/16/18
    '''  Created an overload for use by content management
    '''  Plus this is better code anyway
    ''' </remarks>
    Public Function GetvNSCarrAvailableLoads(ByRef RecordCount As Integer, ByVal CarrierControl As Integer, ByVal filters As Models.AllFilters) As LTS.vNSAvailablePendingLoad()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vNSAvailablePendingLoad
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vNSAvailablePendingLoad)
                iQuery = From t In db.vNSAvailablePendingLoads
                         Where (
                                 (Not db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode = BSCEnum.Active And x.BidCarrierControl = CarrierControl)) _
                                 Or
                                 (
                                    (db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode <> BSCEnum.Active And x.BidCarrierControl = CarrierControl)) _
                                    And
                                    (Not db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode = BSCEnum.Active And x.BidCarrierControl = CarrierControl))
                                )
                             )
                         Select t
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNSCarrAvailableLoads"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an array of LTS.vNSAvailablePendingLoad data objects representing active 
    ''' Next Stop Posted Loads that have not yet received any bids from any Carrier
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>vNSAvailablePendingLoad</returns>
    ''' <remarks>
    ''' Added By LVV on 10/16/18
    '''  Created an overload for use by content management
    '''  Plus this is better code anyway
    ''' </remarks>
    Public Function GetvNSOpsPendingLoads(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNSAvailablePendingLoad()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vNSAvailablePendingLoad
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vNSAvailablePendingLoad)
                iQuery = From t In db.vNSAvailablePendingLoads
                         Where (
                             (Not db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode = BSCEnum.Active)) _
                             Or
                             (
                                 (db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode <> BSCEnum.Active)) _
                                 And
                                 (Not db.tblBids.Any(Function(x) x.BidBidTypeControl = BidTypeEnum.NextStop And x.BidLoadTenderControl = t.LoadTenderControl And x.BidStatusCode = BSCEnum.Active))
                                 )
                             )
                         Select t
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNSOpsPendingLoads"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an array of LTS.vNSLoadsWActiveBid data objects representing loads 
    ''' that have active bids (details grid of NSCarPendingBidsGrid)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>vNSLoadsWActiveBid</returns>
    ''' <remarks>
    ''' Added By LVV on 11/6/18
    '''  Created an overload for use by content management
    '''  Plus this is better code anyway
    ''' </remarks>
    Public Function GetvNSLoadsWActiveBid(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNSLoadsWActiveBid()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vNSLoadsWActiveBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vNSLoadsWActiveBid)
                iQuery = From t In db.vNSLoadsWActiveBids Select t
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvNSLoadsWActiveBid"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an array of LTS.vNSHisoricalLoad objects (where LTType is NEXTStop and Archived is true).
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>tblLoadTender</returns>
    ''' <remarks>
    ''' Added By LVV on 11/7/18
    '''  Created an overload for use by content management
    '''  Plus this is better code anyway
    ''' </remarks>
    Public Function GetNSHisoricalLoads(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNSHisoricalLoad()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vNSHisoricalLoad
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vNSHisoricalLoad)
                iQuery = From t In db.vNSHisoricalLoads Select t
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNSHisoricalLoads"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function DeleteNextStopLoad(ByVal LTControl As Integer, ByVal StatusCode As Integer, BidStatusCode As Integer) As DTO.DATResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.DATResults
                Dim ltsRes = (From d In db.spDeleteNextStopLoad(LTControl, StatusCode, BidStatusCode) Select d).FirstOrDefault()
                If Not ltsRes Is Nothing Then
                    wcfRet.Success = True
                    If ltsRes.ErrNumber <> 0 Then
                        Dim p As New List(Of String)
                        If ltsRes.ErrNumber = 1 Then
                            p.Add(ltsRes.ParamName)
                            p.Add(ltsRes.ParamValue)
                            wcfRet.AddMessage(DataTransferObjects.DATResults.MessageType.Warnings, ltsRes.ErrKey, p.ToArray())
                        End If
                        wcfRet.Success = False
                    End If
                    Return wcfRet
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteNextStopLoad"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function AcceptNextStopBid(ByVal LTControl As Integer, ByVal BidControl As Integer) As DTO.WCFResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim wcfRet As New DTO.WCFResults
                Dim ltsRes = (From d In db.spAcceptNextStopBid(LTControl, BidControl) Select d).FirstOrDefault()
                If Not ltsRes Is Nothing Then
                    wcfRet.Success = True
                    If ltsRes.ErrNumber <> 0 Then
                        Dim p As New List(Of String)
                        If ltsRes.ErrNumber = 1 Then
                            p.Add(ltsRes.ParamName)
                            p.Add(ltsRes.ParamValue)
                            wcfRet.AddMessage(DataTransferObjects.WCFResults.MessageType.Warnings, ltsRes.ErrKey, p.ToArray())
                        End If
                        wcfRet.Success = False
                    End If
                    Return wcfRet
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AcceptNextStopBid"))
            End Try
            Return Nothing
        End Using
    End Function

#End Region

    Public Function GetLoadTenderTypeName(ByVal LTTypeControl As Integer) As String
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim LTName As String = ""

                LTName = (
                    From d In db.tblLoadTenderTypes
                    Where
                        (d.LoadTenderTypeControl = LTTypeControl)
                    Select d.LoadTenderTypeName).FirstOrDefault()

                Return LTName

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenderTypeName"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Delegate Sub CreateP44BidDelegate(ByVal BookControl As Integer, ByVal LoadTenderControl As Integer, ByVal SHID As String, ByVal SSOAAct As SSOAAccount, ByVal strMsg As String)
    Public Delegate Sub ProcessP44RateRequestDelegate(ByRef oP44Proxy As P44.P44Proxy, ByVal oP44Data As P44.RateRequest, ByVal LoadTenderControl As Integer, ByVal strMsg As String)

    Public Sub CreateP44BidAsync(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.P44,
                                 Optional ByRef strMsg As String = "")
        'Dim fetcher As New CreateP44BidDelegate(AddressOf Me.ExecCreateP44Bid)
        ' Launch thread
        Using Logger.StartActivity("CreateP44BidAsync(BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}, SSOAAct: {SSOAAct}, strMsg: {strMsg})", BookControl, LoadTenderControl, SHID, SSOAAct, strMsg)

            ExecCreateP44Bid(BookControl, LoadTenderControl, SHID, SSOAAct, strMsg)
        End Using

    End Sub

    Private Sub ExecCreateP44Bid(ByVal BookControl As Integer, ByVal LoadTenderControl As Integer, ByVal SHID As String, ByVal SSOAAct As Utilities.SSOAAccount, ByVal strMsg As String)
        Using Logger.StartActivity("ExecCreateP44Bid(BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}, SSOAAct: {SSOAAct}, strMsg: {strMsg})", BookControl, LoadTenderControl, SHID, SSOAAct, strMsg)

            Try
                Dim s = "CreateP44BidAsync Warning: "

                Dim success = CreateP44Bid(BookControl, LoadTenderControl, SHID, SSOAAct, strMsg)

                If String.IsNullOrWhiteSpace(strMsg) Then
                    updateLoadTender(LoadTenderControl, Message:=strMsg)
                    If Not success Then s = "CreateP44BidAsync Error: "
                    SaveAppError(s & strMsg, Parameters)
                    ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                    Logger.Error(s & strMsg)
                    'response.AddLog("Read P44 API Bid Failure: " & strMsg)
                    'saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.P44, response)
                End If
            Catch ex As Exception
                'ignore all errors for async processing at this time
                'we could log as system alert message
                Logger.Error(ex, "ExecCreateP44Bid")
                SaveAppError("CreateP44BidAsync Error: " & ex.Message, Parameters)
                Try
                    'response.AddLog("Read API Bid Failure: " & ex.Message)
                    'saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.P44, response)
                Catch e As Exception
                    SaveAppError("CreateP44BidAsync Error: " & e.Message, Parameters)
                End Try
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' use CreateP44Bid when generating rates from an existing order,
    ''' Use CreateP44Quote when an existing order does not exist, 
    ''' CreateP44Bid expects the caller to create a tblLoadTender Record 
    ''' and pass in the the LoadTenderControl to use
    ''' to create a record in tblBids (and tblBidCostAdj and tblBidSvcErr)
    ''' for each quote returned from P44
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SSOAAct"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rules:
    ''' If P44 q.deliveryDate is nothing then use BookDateRequired
    ''' If P44 q.quoteDate is nothing then use current date
    ''' If P44 q.interLine comes back null or with the word false BidInterline is false
    ''' In all other cases BidInterline is true
    ''' TODO:
    ''' Figure out how to get the linehaul out of the P44 data
    ''' Figure out how to get FuelUOM out of P44 data
    ''' Modified by RHR for v-8.2 on 12/11/2018
    '''   added logic to use the P44AccountGroup and moved logic to 
    '''   read SSOA and create P44 proxy to shared functions that can be called 
    '''   by other procedures
    '''   returns false on error and inserts error message into strMsg
    ''' </remarks>
    Public Function CreateP44Bid(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.P44,
                                 Optional ByRef strMsg As String = "") As Boolean

        Using Logger.StartActivity("CreateP44Bid(BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}", BookControl, LoadTenderControl, SHID)

            Dim oLTLs As LTS.vBookRevenue() = DirectCast(Me.NDPBaseClassFactory("NGLBookRevenueData", False), NGLBookRevenueData).GetLTLvBookRevenues(BookControl)
            If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then Return True 'nothing to do but we did not fail
            Dim origST = oLTLs(0).BookOrigState
            Dim destST = oLTLs(0).BookDestState
            Dim dtNow = Date.Now
            Dim oP44Proxy As P44.P44Proxy
            Dim oP44Data As P44.RateRequest
            oP44Data = CopyLTLDataToP44Data(oLTLs)
            If oP44Data Is Nothing Then Return True 'nothing to do but we did not fail

            Dim P44AccountGroup As String = "NGLOPS" 'set default to NGLOPS but each user should have their own settings
            If Not createP44Proxy(oP44Proxy, P44AccountGroup, SSOAAct, strMsg) Then
                Return False
            End If
            Dim intVersion = GetParValue("APIRateQuoteVersion", 0) 'Added by LVV on 2/15/19
            oP44Data.loginGroupKey = P44AccountGroup
            'Modified by RHR for v-8.5.4.005 on 03/19/2024
            'Fix mapping for accesorial fees.
            If (Not oP44Data.accessorials Is Nothing AndAlso oP44Data.accessorials.Count() > 0) Then
                Dim lFees As New List(Of String)

                For Each sFee As String In oP44Data.accessorials
                    If sFee = "PRISON" Then
                        lFees.Add("PRISDEL")
                    Else
                        lFees.Add(sFee)
                    End If
                Next
                oP44Data.accessorials = lFees.ToArray()
            End If
            Dim oResponse As List(Of P44.rateQuoteResponse) = oP44Proxy.GetRateQuotes(oP44Data, intVersion)
            If oResponse Is Nothing OrElse oResponse.Count() < 1 Then
                strMsg += "No NGL API rates are available"
                Return True 'no rates available
            End If
            'save the P44 quotes
            Return CreateNGLAPIBid(oResponse, LoadTenderControl, SHID, origST, destST, oLTLs(0).BookCustCompControl, strMsg, BookControl, oLTLs(0).BookDateRequired)
        End Using

    End Function

    ''' <summary>
    ''' Use CreateP44Quote when an existing order does not exist, 
    ''' use CreateP44Bid when generating rates from an existing order
    ''' CreateP44Quote Creates a tblLoadTender Record and returns the LoadTenderControl
    ''' It also generates a P44Proxy and P44 RateRequest used to process quotes from P44
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oP44Proxy"></param>
    ''' <param name="oP44Data"></param>
    ''' <param name="SSOAAct"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 12/11/2018
    '''   migrating SaveQuote logic from P44RateQuoteController
    '''   to run in DataAccess Layer
    '''   the caller must execute NgLDATBLL.CreateNGLTariffBidNoBookAsync to get the tariffs
    '''   next the caller must execute ProcessP44RateRequest so we 
    '''   share the oP44Proxy and oP44Data by reference
    '''   LoadTenderControl is returned by ref for the new Load Tender record
    ''' Modified by RHR for v-8.3.0.002 on 10/20/2020
    '''     added logic to then caller to  only call CreateNGLTariffBidNoBookAsync when we are not using APIs
    '''     modified CreateRateRequestOrderQuote to pass blnUseP44API by reference so we 
    '''     can get the modified value back, modified  the test for createP44Proxy to set
    '''     blnGenerateP44Proxy to false -- this maps to blnUseP44API
    ''' Modified by RHR for v-8.5.3.001 on 05/27/2022 added logic for tblLoadTenderLog records
    '''     removed optional parameters all references must use all parameters
    '''Modified by RHR for v-8.5.4.001 on 06/28/2023
    ''' added logic to support temperature selection when Rate Shopping 
    ''' </remarks>
    Public Function CreateRateRequestOrderQuote(ByRef order As Models.RateRequestOrder,
                                   ByRef LoadTenderControl As Integer,
                                   ByRef oP44Proxy As P44.P44Proxy,
                                   ByRef oP44Data As P44.RateRequest,
                                   ByVal SSOAAct As Utilities.SSOAAccount,
                                   ByRef strMsg As String,
                                   ByRef blnGenerateP44Proxy As Boolean,
                                   ByRef oLTLogData As NGLLoadTenderLogData) As Boolean
        Using Logger.StartActivity("CreateRateRequestOrderQuote(Order: {Order}, LoadTenderControl: {LoadTenderControl}, SSOAAct: {SSOAAct}, strMsg: {strMsg}, blnGenerateP44Proxy: {blnGenerateP44Proxy})", order, LoadTenderControl, SSOAAct, strMsg, blnGenerateP44Proxy)
            If oLTLogData Is Nothing Then oLTLogData = New NGLLoadTenderLogData(Me.Parameters)
            'Default values updated when dispatched
            Dim SHID = "SHID"
            Dim OrderNbr = "SO123"
            Dim BookControl = 0
            Dim CNS = "CNS"
            Dim P44AccountGroup As String = "NGLOPS" 'set default to NGLOPS but each user should have their own settings
            If blnGenerateP44Proxy Then

                If Not createP44Proxy(oP44Proxy, P44AccountGroup, SSOAAct, strMsg) Then
                    oLTLogData.AddToCollection(strMsg)
                    'Return False
                    blnGenerateP44Proxy = False
                Else
                    oLTLogData.AddToCollection("P44 Proxy Ready")
                End If

            End If
            Dim oItems = New List(Of Models.RateRequestItem)
            oLTLogData.AddToCollection("Fill Rate Request Item Details")
            fillRateRequestItems(order, oItems)

            oLTLogData.AddToCollection("Get Rate Request")
            oP44Data = getRateRequest(order, oItems, P44AccountGroup)
            oLTLogData.AddToCollection("Create Blank Load Tender Record")
            'Modified by RHR for v-8.5.4.001 on 06/28/2023
            '   added logic to support temperature selection when Rate Shopping 
            LoadTenderControl = InsertLoadTenderRateQuote(oP44Data, SHID, OrderNbr, order, BookControl, CNS)
        End Using

        Return True

    End Function


    Public Function DispatchToP44(ByRef d As Models.Dispatch,
                                   Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.P44,
                                   Optional ByRef strMsg As String = "") As DTO.WCFResults

        Dim oRet As New DTO.WCFResults()
        oRet.Success = False

        Dim P44WebServiceUrl As String
        Dim P44WebServiceLogin As String
        Dim P44WebServicePassword As String
        Dim P44AccountGroup As String
        'Get Login Credentials
        oRet = readSSOASettings(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword, P44AccountGroup, SSOAAct, strMsg)
        If oRet.Success = False Then Return oRet
        Dim oP44Dispatch = New Ngl.FM.P44.Dispatch()
        'Create mapping for P44
        oRet = CopyDALDispatchToP44Dispatch(oP44Dispatch, d, strMsg)
        If oRet.Success = False Then Return oRet
        'dispatch the load to P44
        Dim oP44Dispatching = New Ngl.FM.P44.P44Dispatch()
        Dim strSI As String = ""
        Dim oResults As Ngl.FM.P44.Message = oP44Dispatching.Dispatch(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword, P44AccountGroup, oP44Dispatch, strSI)
        If Not String.IsNullOrWhiteSpace(oResults.Diagnostic) AndAlso oResults.Diagnostic <> "Success!" Then
            'we have a problem so log the issue and return false
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
            Return oRet
        Else
            'convert the P44 Dispatch to Model Dispatch
            oRet = CopyP44DispatchToDALDispatch(d, oP44Dispatch, strMsg)
            If oRet.Success = False Then Return oRet
            oRet.Data = d
            If Not String.IsNullOrWhiteSpace(oResults.message) Then
                Select Case oResults.Severity
                    Case Ngl.FM.P44.SeverityEnum.ERROR
                        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
                    Case Ngl.FM.P44.SeverityEnum.WARNING
                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {oResults.message})
                End Select
            End If
            'return success.
            oRet.Success = True
        End If

        Return oRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oP44Dispatch"></param>
    ''' <param name="d"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
    ''' </remarks>
    Public Function CopyDALDispatchToP44Dispatch(ByRef oP44Dispatch As Ngl.FM.P44.Dispatch, ByVal d As Models.Dispatch, ByRef strMsg As String) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = True
        Dim skipObjs As New List(Of String) From {"Origin",
                                                  "Destination",
                                                  "Items",
                                                  "Accessorials",
                                                  "Requestor",
                                                  "EmergencyContact",
                                                  "InfoMessages",
                                                  "Errors"}
        oP44Dispatch = CopyMatchingFields(oP44Dispatch, d, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then

            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Origin Data
        skipObjs = New List(Of String) From {"Contact"}
        oP44Dispatch.Origin = New P44.AddressBook()
        oP44Dispatch.Origin = CopyMatchingFields(oP44Dispatch.Origin, d.Origin, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then

            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        If Not oP44Dispatch.Origin Is Nothing Then
            oP44Dispatch.Origin.Contact = New P44.Contact()
            oP44Dispatch.Origin.Contact = CopyMatchingFields(oP44Dispatch.Origin.Contact, d.Origin.Contact, skipObjs, strMsg)
            If (Not String.IsNullOrWhiteSpace(strMsg)) Then

                oRet.Success = False
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                Return oRet
            End If

        End If
        'Dest Data
        skipObjs = New List(Of String) From {"Contact"}
        oP44Dispatch.Destination = New P44.AddressBook
        oP44Dispatch.Destination = CopyMatchingFields(oP44Dispatch.Destination, d.Destination, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then

            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        If Not oP44Dispatch.Destination Is Nothing Then
            oP44Dispatch.Destination.Contact = New P44.Contact()
            oP44Dispatch.Destination.Contact = CopyMatchingFields(oP44Dispatch.Destination.Contact, d.Destination.Contact, skipObjs, strMsg)
            If (Not String.IsNullOrWhiteSpace(strMsg)) Then

                oRet.Success = False
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                Return oRet
            End If

        End If
        'Requestor Data
        skipObjs = New List(Of String) From {"Contact"}
        oP44Dispatch.Requestor = New P44.AddressBook()
        oP44Dispatch.Requestor = CopyMatchingFields(oP44Dispatch.Requestor, d.Requestor, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then

            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        If Not oP44Dispatch.Requestor Is Nothing Then
            oP44Dispatch.Requestor.Contact = New P44.Contact()
            oP44Dispatch.Requestor.Contact = CopyMatchingFields(oP44Dispatch.Requestor.Contact, d.Requestor.Contact, skipObjs, strMsg)
            If (Not String.IsNullOrWhiteSpace(strMsg)) Then

                oRet.Success = False
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                Return oRet
            End If

        End If
        'EmergencyContact Data
        skipObjs = New List(Of String)
        oP44Dispatch.EmergencyContact = New P44.Contact()
        oP44Dispatch.EmergencyContact = CopyMatchingFields(oP44Dispatch.EmergencyContact, d.EmergencyContact, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then

            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Items Data
        skipObjs = New List(Of String)
        '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
        Dim dblGlobalAPIUseNMFCCodes As Double
        dblGlobalAPIUseNMFCCodes = GetParValue("GlobalAPIUseNMFCCodes", 0)
        If Not d.Items Is Nothing AndAlso d.Items.Count() > 0 Then
            Dim oP44Items As New List(Of Ngl.FM.P44.Item)
            'skipObjs = New List(Of String) From {"ItemDimensions"}
            For Each i In d.Items
                If dblGlobalAPIUseNMFCCodes = 0 Then
                    '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
                    i.ItemNMFCItemCode = ""
                    i.ItemNMFCSubCode = ""
                End If
                Dim oP44Item = New Ngl.FM.P44.Item()
                oP44Items.Add(CopyMatchingFields(oP44Item, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then

                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            If Not oP44Items Is Nothing AndAlso oP44Items.Count() > 0 Then
                oP44Dispatch.Items = oP44Items.ToArray()
            End If
        End If
        'Accessorials Data
        oP44Dispatch.Accessorials = d.Accessorials
        'InfoMessages Data
        skipObjs = New List(Of String)
        If Not d.InfoMessages Is Nothing AndAlso d.InfoMessages.Count() > 0 Then
            Dim oP44InfoMessages As New List(Of Ngl.FM.P44.APIMessage)

            For Each i In d.InfoMessages
                Dim oP44APIMessage = New Ngl.FM.P44.APIMessage()
                oP44InfoMessages.Add(CopyMatchingFields(oP44APIMessage, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then

                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            If Not oP44InfoMessages Is Nothing AndAlso oP44InfoMessages.Count() > 0 Then
                oP44Dispatch.InfoMessages = oP44InfoMessages.ToArray()
            End If
        End If
        'Errors Data
        skipObjs = New List(Of String)
        If Not d.Errors Is Nothing AndAlso d.Errors.Count() > 0 Then
            Dim oP44Errors As New List(Of Ngl.FM.P44.APIMessage)

            For Each i In d.Errors
                Dim oP44APIErrors = New Ngl.FM.P44.APIMessage()
                oP44Errors.Add(CopyMatchingFields(oP44APIErrors, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then
                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            If Not oP44Errors Is Nothing AndAlso oP44Errors.Count() > 0 Then
                oP44Dispatch.Errors = oP44Errors.ToArray()
            End If
        End If



        Return oRet
    End Function


    Public Function CopyP44DispatchToDALDispatch(ByRef oDALDispatch As Models.Dispatch, ByVal oP44Dispatch As Ngl.FM.P44.Dispatch, ByRef strMsg As String) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = True

        Dim skipObjs As New List(Of String) From {"Origin",
                                                  "Destination",
                                                  "Items",
                                                  "Accessorials",
                                                  "Requestor",
                                                  "EmergencyContact",
                                                  "InfoMessages",
                                                  "Errors"}
        oDALDispatch = CopyMatchingFields(oDALDispatch, oP44Dispatch, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Origin Data
        skipObjs = New List(Of String) From {"Contact"}
        oDALDispatch.Origin = CopyMatchingFields(oDALDispatch.Origin, oP44Dispatch.Origin, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        oDALDispatch.Origin.Contact = CopyMatchingFields(oDALDispatch.Origin.Contact, oP44Dispatch.Origin.Contact, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Dest Data
        skipObjs = New List(Of String) From {"Contact"}
        oDALDispatch.Destination = CopyMatchingFields(oDALDispatch.Destination, oP44Dispatch.Destination, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        oDALDispatch.Destination.Contact = CopyMatchingFields(oDALDispatch.Destination.Contact, oP44Dispatch.Destination.Contact, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Requestor Data
        skipObjs = New List(Of String) From {"Contact"}
        oDALDispatch.Requestor = CopyMatchingFields(oDALDispatch.Requestor, oP44Dispatch.Requestor, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        skipObjs = New List(Of String)
        oDALDispatch.Requestor.Contact = CopyMatchingFields(oDALDispatch.Requestor.Contact, oP44Dispatch.Requestor.Contact, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'EmergencyContact Data
        skipObjs = New List(Of String)
        oDALDispatch.EmergencyContact = CopyMatchingFields(oDALDispatch.EmergencyContact, oP44Dispatch.EmergencyContact, skipObjs, strMsg)
        If (Not String.IsNullOrWhiteSpace(strMsg)) Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
            Return oRet
        End If
        'Items Data
        skipObjs = New List(Of String)
        If Not oP44Dispatch.Items Is Nothing AndAlso oP44Dispatch.Items.Count() > 0 Then
            Dim oModelsItems As New List(Of Models.Item)
            skipObjs = New List(Of String) From {"ItemDimensions"}
            For Each i In oP44Dispatch.Items
                Dim oModelsItem = New Models.Item
                oModelsItems.Add(CopyMatchingFields(oModelsItem, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then
                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            oDALDispatch.Items = oModelsItems.ToArray()
        End If
        'Accessorials Data
        oDALDispatch.Accessorials = oP44Dispatch.Accessorials
        'InfoMessages Data
        skipObjs = New List(Of String)
        If Not oP44Dispatch.InfoMessages Is Nothing AndAlso oP44Dispatch.InfoMessages.Count() > 0 Then
            Dim oModelsInfoMessages As New List(Of Models.APIMessage)

            For Each i In oP44Dispatch.InfoMessages
                Dim oModelsAPIMessage = New Models.APIMessage
                oModelsInfoMessages.Add(CopyMatchingFields(oModelsAPIMessage, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then
                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            oDALDispatch.InfoMessages = oModelsInfoMessages.ToArray()
        End If
        'Errors Data
        skipObjs = New List(Of String)
        If Not oP44Dispatch.Errors Is Nothing AndAlso oP44Dispatch.Errors.Count() > 0 Then
            Dim oModelsErrors As New List(Of Models.APIMessage)

            For Each i In oP44Dispatch.Errors
                Dim oModelsAPIMessage = New Models.APIMessage
                oModelsErrors.Add(CopyMatchingFields(oModelsAPIMessage, i, skipObjs, strMsg))
                If (Not String.IsNullOrWhiteSpace(strMsg)) Then
                    oRet.Success = False
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, {strMsg})
                    Return oRet
                End If
            Next
            oDALDispatch.Errors = oModelsErrors.ToArray()
        End If



        Return oRet
    End Function

    Public Sub ProcessP44RateRequestAsync(ByRef oP44Proxy As P44.P44Proxy,
                                         ByVal oP44Data As P44.RateRequest,
                                         ByVal LoadTenderControl As Integer,
                                         Optional ByRef strMsg As String = "")
        ' Launch thread

        ExecProcessP44RateRequest(oP44Proxy, oP44Data, LoadTenderControl, strMsg)
    End Sub

    Private Sub ExecProcessP44RateRequest(ByRef oP44Proxy As P44.P44Proxy, ByVal oP44Data As P44.RateRequest, ByVal LoadTenderControl As Integer, ByVal strMsg As String)
        Dim response = New DTO.CarrierCostResults()
        Try
            Dim s = "ProcessP44RateRequestAsync Warning: "

            Dim success = ProcessP44RateRequest(oP44Proxy, oP44Data, LoadTenderControl, strMsg)

            If String.IsNullOrWhiteSpace(strMsg) Then
                updateLoadTender(LoadTenderControl, Message:=strMsg)
                If Not success Then s = "ProcessP44RateRequestAsync Error: "
                SaveAppError(s & strMsg, Parameters)
                ' Modified by RHR for v-8.3.0.002 on 12/17/2020

                response.AddLog("Read P44 API Bid Failure: " & strMsg)
                saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.P44, response)
            End If
        Catch ex As Exception
            'ignore all errors for async processing at this time
            'we could log as system alert message
            SaveAppError("ProcessP44RateRequestAsync Error: " & ex.Message, Parameters)
            Try
                response.AddLog("Read P44 API Bid Failure: " & ex.Message)
                saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.P44, response)
            Catch e As Exception
                SaveAppError("ProcessP44RateRequestAsync Error: " & e.Message, Parameters)
            End Try
        End Try
    End Sub


    Public Function ProcessP44RateRequest(ByRef oP44Proxy As P44.P44Proxy,
                                         ByVal oP44Data As P44.RateRequest,
                                         ByVal LoadTenderControl As Integer,
                                         Optional ByRef strMsg As String = "") As Boolean
        Using Logger.StartActivity("ProcessP44RateRequest(LoadTenderControl: {LoadTenderControl})", LoadTenderControl)

            Dim intVersion = GetParValue("APIRateQuoteVersion", 0) 'Added by 'LVV on 2/15/19
            'Modified by RHR for v-8.5.4.005 on 03/19/2024
            'Fix mapping for accesorial fees.
            If (Not oP44Data.accessorials Is Nothing AndAlso oP44Data.accessorials.Count() > 0) Then
                Dim lFees As New List(Of String)

                For Each sFee As String In oP44Data.accessorials
                    If sFee = "PRISON" Then
                        lFees.Add("PRISDEL")
                    Else
                        lFees.Add(sFee)
                    End If
                Next
                oP44Data.accessorials = lFees.ToArray()
            End If

            Dim oResponse As List(Of P44.rateQuoteResponse) = oP44Proxy.GetRateQuotes(oP44Data, intVersion)
            If oResponse Is Nothing OrElse oResponse.Count() < 1 Then
                strMsg += "No NGL API rates are available"
                Return False 'no rates available
            End If
            'If oResponse Is Nothing OrElse oResponse.Count() < 1 Then
            '    strMsg += "No NGL API rates are available"
            '    Return True 'no rates available
            'End If
            InsertLTRateQuoteBids(LoadTenderControl, oResponse)
            Return True
        End Using

    End Function


    Public Sub ProcessP44RateRequestAsync(ByRef oP44Proxy As P44.P44Proxy,
                                         ByVal oP44Data As P44.RateRequest,
                                         ByVal LoadTenderControl As Integer)


    End Sub


    Public Sub ExecProcessP44RateRequest(ByRef oP44Proxy As P44.P44Proxy,
                                         ByVal oP44Data As P44.RateRequest,
                                         ByVal LoadTenderControl As Integer)

        Dim intVersion = GetParValue("APIRateQuoteVersion", 0) 'Added by LVV on 2/15/19
        Dim oResponse As List(Of P44.rateQuoteResponse) = oP44Proxy.GetRateQuotes(oP44Data, intVersion)
        If oResponse Is Nothing OrElse oResponse.Count() < 1 Then Return
        InsertLTRateQuoteBids(LoadTenderControl, oResponse)
        Return
    End Sub

    Friend Function calcUnitsPerPallet(ByVal iUnits As Integer, ByVal dPallets As Decimal) As Integer
        Dim iRet As Integer = iUnits
        If iUnits > dPallets And dPallets > 1 Then
            iRet = Math.Ceiling(iUnits / Math.Ceiling(dPallets))
        End If

        Return iRet
    End Function

    Friend Function calcLinearSpace(ByVal intCT As Integer, ByVal dLength As Decimal, Optional ByVal sUnitType As String = "In") As Integer
        Dim iRet = 0
        If sUnitType = "In" Then
            iRet = Math.Ceiling((dLength / 12)) * intCT
        End If
        Return iRet
    End Function

    Friend Function calcVolume(ByVal dLength As Decimal, ByVal dWidth As Decimal, ByVal dHeight As Decimal, Optional ByVal sUnitType As String = "In") As Integer
        Dim iRet = 0
        If sUnitType = "In" Then
            iRet = Math.Ceiling((dLength / 12)) * Math.Ceiling((dWidth / 12)) * Math.Ceiling((dHeight / 12))
        End If
        Return iRet
    End Function


#Region "Begin UPS API Logic"


    ''' <summary>
    '''  Retrieve a Rate Request for a single shipment LTL or Truckload
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Created  By RHR for v-8.5.1.001 on 01/25/2022 
    '''     1. get the token (note: we should be able to cache the token and the expired date
    '''     2. Call the API GetRateQuotes method using the oAPILoadData
    '''     3. handle errors and validate results
    '''     4. Call the InsertAPIQuoteBids (copy logic from InsertLTRateQuoteBids)
    ''' TODO:  
    '''     1. map item cost to RateRequestItem data replace .declaredValue = 1000,
    '''     2. map TMS Pallet Type to replace .packagingCode = "PLT" We need a cross refence with each API
    '''     3. Find out the best way to map .productName = itm.ItemNumber to something other than ItemNumber
    '''     4. Create a cross reference and replace .temperatureSensitive = "Dry", with the correct UPS Code for the item
    '''     5. Create a setup or parameter to create a cross reference and replace .temperatureUnit = "Fahrenheit", with the correct UPS Code for the item
    '''     6. Map .requiredTemperatureHigh = 85, and .requiredTemperatureLow = 35, to the correct temperature settings in TMS
    '''     7. Create a new way to replace the following:
    '''         (a) .unitsPerPallet = calcUnitsPerPallet(itm.Quantity,itm.PalletCount),
    '''         (b) .unitWeight = 0
    '''         (c) .unitVolume = 0,
    '''         (d) .hazardousEmergencyPhone = "5555555555",
    '''         (e) .upc = "",    
    '''         (f) .sku = "",
    '''         (g) .plu = ""
    ''' </remarks>
    Public Function ProcessUPSRateRequest(ByVal order As Models.RateRequestOrder,
                                          ByVal LoadTenderControl As Integer,
                                          ByVal oLEConfig As LTS.tblSSOALEConfig,
                                          ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                          Optional ByRef strMsg As String = "") As Boolean

        Return False 'not finished

        Dim oResponse As New UPS.UPSQuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oUPSAPI As New UPS.UPSAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)

        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0

        If order.Accessorials Is Nothing Then
            order.Accessorials = New String() {}
        End If
        Dim blnContinue = oLT.GetInfoForLTRateQuoteTariffBids(LoadTenderControl, order.Accessorials, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)

        If Not blnContinue Then Return False 'Nothing to do
        If order.ShipDate < Date.Now.AddDays(1) Then Return False ' TODO add message to Error Logs  invalid ship date

        If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
            'this should never happen unless there is a design bug.
            throwInvalidOperatonException("Shipping information is missing stop data")
            Return False
        End If

        'Read the default config settings
        Dim sMode As String = "Parcel"
        Dim sEquipment As String = "Van"
        Dim sLocationID As String = "C377465" 'default for Tree Top
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000


        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LocationID")) Then sLocationID = lCompConfig.Where(Function(x) x.SSOACName = "LocationID").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If

        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        Dim TotalCases = 1
        Dim dblTotalWeight As Double = 1
        Dim TotalPL = 1
        Dim TotalLen = 48
        Dim TotalWidth = 42
        Dim TotalHeight = 48
        Dim TotalCube = 0
        Dim oRateRequest = New UPS.RateRequest()
        Dim oOrigSpecialReq As UPS.UPSSpecialRequirement = New UPS.UPSSpecialRequirement()
        Dim oOrigRefs As New List(Of UPS.UPSReferenceNumbers)  '[] = new UPSReferenceNumbers[1];
        UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.SHID, order.Stops(0).SHID, oOrigRefs)
        UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.CON, order.Stops(0).BookCarrOrderNumber, oOrigRefs)
        UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.CRID, order.Stops(0).BookProNumber, oOrigRefs)
        UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.PU, order.BookConsPrefix, oOrigRefs)
        UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.DEL, order.Stops(0).BookCarrOrderNumber, oOrigRefs)
        oRateRequest.oOrigin = New UPS.UPSAddress() With {
            .locationName = order.Pickup.CompName,
            .address1 = order.Pickup.CompAddress1,
            .city = order.Pickup.CompCity,
            .stateProvinceCode = order.Pickup.CompState,
            .countryCode = "US",
            .postalCode = order.Pickup.CompPostalCode,
            .specialRequirement = oOrigSpecialReq,
            .customerLocationId = order.Pickup.CompName,
            .referenceNumbers = oOrigRefs.ToArray()
        }
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        iCompControl = order.BookCustCompControl
        'get a default FAK class
        Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)

        oRateRequest.lStops = New List(Of UPS.UPSAddress)
        oRateRequest.lItems = New List(Of UPS.UPSItem)
        Dim iTmp As Integer = 0

        For Each s In order.Stops
            Dim oItems = New List(Of Models.RateRequestItem)
            oLT.fillRateRequestItems(s, oItems)
            If Not oItems Is Nothing AndAlso oItems.Count() > 0 Then
                dblTotalWeight = oItems.Sum(Function(x) x.Weight)
                '**************** Begin Validate Mode and weight  ***************************

                Dim blnShipLTL As Boolean = False
                If dblTotalWeight < dLowLTLWeight Then
                    oRateRequest.setPostMessageOnlyFlag(True)
                    oRateRequest.AddMessage(UPS.UPSAPI.MessageEnum.E_WeightTooLowForLTL, " The UPS settings require shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for UPS.", "", "BookTotalWgt")
                    Exit For
                End If
                If dblTotalWeight <= dHighLTLWeight Then
                    sMode = "Parcel"
                Else
                    oRateRequest.setPostMessageOnlyFlag(True)
                    oRateRequest.AddMessage(UPS.UPSAPI.MessageEnum.E_WeightTooHighForLTL, " The UPS settings require shipments to have a weight less than " & dHighLTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for UPS.", "", "BookTotalWgt")
                    Exit For
                End If

                '****************** End Validate Mode and weight *******************************
                Dim UPSStop = New UPS.UPSAddress() With {
                    .locationName = s.CompName,
                    .address1 = s.CompAddress1,
                    .city = s.CompCity,
                    .stateProvinceCode = s.CompState,
                    .countryCode = "US",
                    .postalCode = s.CompPostalCode,
                    .specialRequirement = oOrigSpecialReq,
                    .customerLocationId = s.CompName,
                    .referenceNumbers = oOrigRefs.ToArray()
                }
                oRateRequest.lStops.Add(UPSStop)
                oRateRequest.setShipDate(order.ShipDate)
                oRateRequest.setMode(sEquipment, 1, sMode)

                TotalCases = oItems.Sum(Function(x) x.NumPieces)
                TotalPL = oItems.Sum(Function(x) x.PalletCount)
                TotalLen = oItems.Sum(Function(x) x.Length)
                TotalWidth = oItems.Sum(Function(x) x.Width)
                TotalHeight = oItems.Sum(Function(x) x.Height)

                For Each itm As Models.RateRequestItem In oItems
                    Dim oItem = New UPS.UPSItem() With {
                        .description = If(String.IsNullOrWhiteSpace(itm.Description), "misc products", itm.Description),
                        .freightClass = If(Integer.TryParse(itm.FreightClass, iTmp), iTmp, 100),
                        .actualWeight = If(Integer.TryParse(itm.Weight.ToString(), iTmp), iTmp, 100),
                        .weightUnit = If(String.IsNullOrWhiteSpace(itm.WeightUnit), "Pounds", itm.WeightUnit),
                        .length = itm.Length,
                        .width = itm.Width,
                        .height = itm.Height,
                        .pallets = itm.PalletCount,
                        .pieces = itm.NumPieces,
                        .palletSpaces = itm.PalletCount,
                        .packagingCode = "PLT",
                        .productName = itm.ItemNumber,
                        .declaredValue = 1000,
                        .temperatureSensitive = "Dry",
                        .temperatureUnit = "Fahrenheit",
                        .requiredTemperatureHigh = 85,
                        .requiredTemperatureLow = 35,
                        .isStackable = If(itm.Stackable, "true", "false"),
                        .referenceNumbers = oOrigRefs.ToArray()
                    }
                    oRateRequest.lItems.Add(oItem)

                Next
            End If
        Next

        Return GetUPSRates(oRateRequest, LoadTenderControl, oLEConfig, lCompConfig, strMsg)

    End Function


    Function GetUPSRates(ByRef oRateRequest As UPS.RateRequest,
                         ByVal LoadTenderControl As Integer,
                         ByVal oLEConfig As LTS.tblSSOALEConfig,
                         ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                         Optional ByRef strMsg As String = "") As Boolean



        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oUPSAPI As New UPS.UPSAPI(bUseTLS12)

        Dim oResponse As New UPS.UPSQuoteResponse()
        If oLEConfig Is Nothing OrElse String.IsNullOrWhiteSpace(oLEConfig.SSOALEClientID) Then
            Return False
        End If
        Dim sclient_id As String = oLEConfig.SSOALEClientID
        Dim sclient_secret As String = oLEConfig.SSOALEClientSecret
        Dim saudience As String = oLEConfig.SSOALELoginURL
        Dim sgrant_type As String = oLEConfig.SSOALEAuthCode
        Dim sDataURL As String = oLEConfig.SSOALEDataURL

        Dim arrUPSServices As New List(Of String)

        Dim sCarrierNumber As String = "0"
        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfig.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "01" And x.SSOACValue = "1")) Then arrUPSServices.Add("01")
            If (lCompConfig.Any(Function(x) x.SSOACName = "02" And x.SSOACValue = "1")) Then arrUPSServices.Add("02")
            If (lCompConfig.Any(Function(x) x.SSOACName = "03" And x.SSOACValue = "1")) Then arrUPSServices.Add("03")
            If (lCompConfig.Any(Function(x) x.SSOACName = "07" And x.SSOACValue = "1")) Then arrUPSServices.Add("07")
            If (lCompConfig.Any(Function(x) x.SSOACName = "08" And x.SSOACValue = "1")) Then arrUPSServices.Add("08")
            If (lCompConfig.Any(Function(x) x.SSOACName = "11" And x.SSOACValue = "1")) Then arrUPSServices.Add("11")
            If (lCompConfig.Any(Function(x) x.SSOACName = "12" And x.SSOACValue = "1")) Then arrUPSServices.Add("12")
            If (lCompConfig.Any(Function(x) x.SSOACName = "13" And x.SSOACValue = "1")) Then arrUPSServices.Add("13")
            If (lCompConfig.Any(Function(x) x.SSOACName = "14" And x.SSOACValue = "1")) Then arrUPSServices.Add("14")
            If (lCompConfig.Any(Function(x) x.SSOACName = "54" And x.SSOACValue = "1")) Then arrUPSServices.Add("54")
            If (lCompConfig.Any(Function(x) x.SSOACName = "59" And x.SSOACValue = "1")) Then arrUPSServices.Add("59")
            If (lCompConfig.Any(Function(x) x.SSOACName = "65" And x.SSOACValue = "1")) Then arrUPSServices.Add("65")
        End If

        Dim dShipDate As Date?
        If Not oRateRequest Is Nothing Then
            dShipDate = oRateRequest.getNGLShipDate()
        End If
        If oRateRequest.getPostMessageOnlyFlag() = True Then
            oResponse.postMessagesOnly = True
            Dim oMessages = oRateRequest.GetMessages()
            For Each msg In oMessages
                oResponse.AddMessage(msg)
            Next
        Else
            If Not dShipDate.HasValue OrElse dShipDate.Value <= Date.Now.AddDays(1) Then
                Dim sShipDateTxt As String = "Missing"
                If dShipDate.HasValue Then
                    sShipDateTxt = dShipDate.Value.ToString()
                End If
                oResponse.postMessagesOnly = True
                oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_InvalidShipDate, "UPS requires more advanced notice to rate this load shipping on " & sShipDateTxt, "", "LoadDate")
            End If
        End If


        If Not oResponse.postMessagesOnly Then
            If arrUPSServices Is Nothing OrElse arrUPSServices.Count() < 1 Then
                arrUPSServices = New List(Of String)
                arrUPSServices.Add("03") ' default is ground
            End If
            For Each sMode As String In arrUPSServices
                oRateRequest.setServiceMode(sMode)
                oResponse = oUPSAPI.getHTTPRateRequest(sgrant_type, sclient_id, sclient_secret, oRateRequest, sDataURL)
                'Dim oRes As UPS.UPSTokenData = oUPSAPI.getToken(sclient_id, sclient_secret, saudience, sgrant_type)
                'If Not oRes Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oRes.access_token) Then
                '    oResponse = oUPSAPI.getHTTPRateRequest(oRes.access_token, "", "", oRateRequest, sDataURL)
                'End If


                If oResponse Is Nothing Then
                    oResponse = New UPS.UPSQuoteResponse()
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid", "", "")
                    Exit For
                    'ElseIf oResponse.RateResponse Is Nothing OrElse oResponse.RateResponse.Count() < 1 Then
                ElseIf oResponse.postMessagesOnly Then
                    Exit For
                ElseIf oResponse.RateResponse Is Nothing Then
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_NoRatesFound, "No UPS API rates are available", "", "")
                End If
                If sCarrierNumber = "0" Then
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_InvalidCarrierNumber, "Fix the API SSOA Config setting for CarrierNumber to save rates", "", "CarrierNumber")
                    Exit For
                End If
                ' we now insert a quote with error messages even if one is not available 
                ' using the postMessagesOnly flag with a zero cost.  This logic will help
                ' users track issues with API rating
                InsertUPSRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
            Next

        Else
            ' we now insert a quote with error messages even if one is not available 
            ' using the postMessagesOnly flag with a zero cost.  This logic will help
            ' users track issues with API rating
            InsertUPSRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
        End If


        Return True
    End Function


    ''' <summary>
    ''' use CreateUPSBid when generating rates from an existing order,
    ''' Use CreateUPSQuote when an existing order does not exist, 
    ''' CreateUPSBid expects the caller to create a tblLoadTender Record 
    ''' and pass in the the LoadTenderControl to use
    ''' to create a record in tblBids (and tblBidCostAdj and tblBidSvcErr)
    ''' for each quote returned from P44
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SSOAAct"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rules:
    ''' If P44 q.deliveryDate is nothing then use BookDateRequired
    ''' If P44 q.quoteDate is nothing then use current date
    ''' If P44 q.interLine comes back null or with the word false BidInterline is false
    ''' In all other cases BidInterline is true
    ''' TODO:
    ''' Figure out how to get the linehaul out of the P44 data
    ''' Figure out how to get FuelUOM out of P44 data
    ''' Modified by RHR for v-8.2 on 12/11/2018
    '''   added logic to use the P44AccountGroup and moved logic to 
    '''   read SSOA and create P44 proxy to shared functions that can be called 
    '''   by other procedures
    '''   returns false on error and inserts error message into strMsg
    ''' </remarks>
    Public Function CreateUPSBid(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 ByVal oLEConfig As LTS.tblSSOALEConfig,
                                 ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                 Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.UPSAPI,
                                 Optional ByRef strMsg As String = "") As Boolean

        'Return False 'not finished

        Dim oLTLs As LTS.vBookRevenue() = DirectCast(Me.NDPBaseClassFactory("NGLBookRevenueData", False), NGLBookRevenueData).GetLTLvBookRevenues(BookControl)
        If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then Return True 'nothing to do but we did not fail
        Dim origST = oLTLs(0).BookOrigState
        Dim destST = oLTLs(0).BookDestState
        Dim dtNow = Date.Now
        Dim oRateRequest As UPS.RateRequest = CopyBookDataToUPSData(oLTLs, lCompConfig)
        ' CopyBookDataToUPSData()
        If oRateRequest Is Nothing Then Return True 'nothing to do but we did not fail

        Return GetUPSRates(oRateRequest, LoadTenderControl, oLEConfig, lCompConfig, strMsg)

    End Function


    Protected Function CopyBookDataToUPSData(ByRef oBook() As LTS.vBookRevenue, ByVal lCompConfig As List(Of LTS.tblSSOAConfig), Optional ByVal timeOut As Integer = 20, Optional ByVal sDefFrtClass As String = "70") As UPS.RateRequest
        Dim oRet As New UPS.RateRequest()
        If ((oBook Is Nothing) OrElse oBook.Count() < 1 OrElse (oBook(0).BookControl = 0)) Then Return Nothing

        Dim oResponse As New UPS.UPSQuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oUPSAPI As New UPS.UPSAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)


        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        Dim dtShip As Date = Date.Now.AddDays(2) 'UPS rates need 2 days 
        Dim dtRequired As Date = dtShip.AddDays(4)
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        Dim oAccs As LTS.vBookAccessorial()

        'Read the default config settings
        Dim sMode As String = "TL"
        Dim sEquipment As String = "Van"
        Dim sLocationID As String = "C377465" 'default for Tree Top\
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000

        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LocationID")) Then sLocationID = lCompConfig.Where(Function(x) x.SSOACName = "LocationID").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        'Note: in v-8.5.1 we only support one booking record
        'For Each book In oBook
        Dim book As LTS.vBookRevenue = oBook(0)

        If Not book Is Nothing AndAlso book.BookControl <> 0 AndAlso Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 Then
            iCompControl = book.BookCustCompControl
            '**************** Begin Validate Mode and weight  ***************************

            Dim blnShipLTL As Boolean = False
            Dim dblTotalWeight As Double = 0
            If book.BookTotalWgt.HasValue Then dblTotalWeight = book.BookTotalWgt.Value
            If dblTotalWeight < dLowLTLWeight Then
                oRet.setPostMessageOnlyFlag(True)
                oRet.AddMessage(UPS.UPSAPI.MessageEnum.E_WeightTooLowForLTL, " The UPS settings require LTL shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for UPS.", "", "BookTotalWgt")
                Return oRet
            End If
            If dblTotalWeight <= dHighLTLWeight Then
                sMode = "Parcel"
            Else
                oRet.setPostMessageOnlyFlag(True)
                oRet.AddMessage(UPS.UPSAPI.MessageEnum.E_WeightTooHighForLTL, " The UPS settings require shipments to have a weight less than " & dHighLTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for UPS.", "", "BookTotalWgt")
                Return oRet
            End If

            '****************** End Validate Mode and weight *******************************
            'get a default FAK class
            Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)
            'get the earliest load date
            If book.BookDateLoad.HasValue Then dtShip = book.BookDateLoad.Value
            'get the latest delivery date
            If book.BookDateRequired.HasValue AndAlso book.BookDateRequired.Value > dtRequired Then dtRequired = book.BookDateRequired.Value
            Dim filters As New Models.AllFilters With {.ParentControl = book.BookControl}
            Dim ct As Integer

            ct = 0
            filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
            oAccs = oBookAccDAL.GetBookAccessorials(filters, ct)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                oBookAccessorial.AddRange(oAccs)
                sAccessorial.AddRange(oAccs.Select(Function(x) x.NACCode).ToList())
            End If
            'Modified by RHR for v-8.5.3.005 on 09/21/2022 added logic to get the item details if they exists 
            If book.BookLoads Is Nothing OrElse book.BookLoads.Count() < 1 OrElse book.BookLoads(0).BookItems Is Nothing OrElse book.BookLoads(0).BookItems.Count() < 1 Then
                Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    oItems.AddRange(oPkgs)
                End If
            Else
                For Each bl In book.BookLoads
                    If Not bl Is Nothing AndAlso Not bl.BookItems Is Nothing AndAlso bl.BookItems.Count() > 0 Then
                        For Each bi As LTS.BookItem In bl.BookItems
                            Dim oPkg As New LTS.vBookPackage()
                            With oPkg
                                .BookPkgDescription = bi.BookItemDescription
                                .BookPkgFAKClass = bi.BookItemFAKClass
                                .BookPkgWeight = If(bi.BookItemWeight, 10)
                                .BookPkgLength = bi.BookItemQtyLength
                                .BookPkgWidth = bi.BookItemQtyWidth
                                .BookPkgHeight = bi.BookItemQtyHeight
                                .BookPkgCount = CInt(bi.BookItemPallets)
                                .PackageType = "PLT"
                                .BookPkgStackable = bi.BookItemStackable
                            End With
                            oItems.Add(oPkg)
                        Next
                    End If
                Next
            End If

        End If
        'Next
        If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

        Dim oOrigSpecialReq As UPS.UPSSpecialRequirement = New UPS.UPSSpecialRequirement()
        Dim oOrigRefs As New List(Of UPS.UPSReferenceNumbers)  '[] = new UPSReferenceNumbers[1];


        With oRet
            .customerCode = sLocationID
            .setMode(sEquipment, 1, sMode)
            .lStops = New List(Of UPS.UPSAddress)
            .lItems = New List(Of UPS.UPSItem)
            .setShipDate(dtShip.ToShortDateString)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                For Each a As LTS.vBookAccessorial In oAccs
                    If a.AccessorialCode = 48 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.liftGate)
                    If a.AccessorialCode = 17 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.insideDelivery)
                    If a.AccessorialCode = 122 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.insidePickup)
                    If a.AccessorialCode = 68 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.residentialNonCommercial)
                    If a.AccessorialCode = 132 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.residentialNonCommercial)
                    If a.AccessorialCode = 72 Then oOrigSpecialReq.setSpecialRequrement(UPS.UPSAPI.SpecialRequirement.constructionSite)
                Next
            End If
            Dim sCNS As String = If(String.IsNullOrWhiteSpace(oBook(0).BookConsPrefix), oBook(0).BookProNumber, oBook(0).BookConsPrefix)
            Dim sSHID As String = If(String.IsNullOrWhiteSpace(oBook(0).BookSHID), sCNS, oBook(0).BookSHID)
            Dim sDel As String = If(String.IsNullOrWhiteSpace(oBook(0).BookLoads(0).BookLoadPONumber), sCNS, oBook(0).BookLoads(0).BookLoadPONumber)
            UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.PU, sCNS, oOrigRefs)
            UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.SHID, sSHID, oOrigRefs)
            UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.DEL, sDel, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookCarrOrderNumber)) Then UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.CON, oBook(0).BookCarrOrderNumber, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookProNumber)) Then UPS.UPSReferenceNumbers.addReferenceNumber(UPS.UPSAPI.RefNumbers.CRID, oBook(0).BookProNumber, oOrigRefs)


            .oRefs = oOrigRefs.ToArray()
            .oOrigin = New UPS.UPSAddress() With {
                .locationName = oBook(0).BookOrigName,
                .address1 = oBook(0).BookOrigAddress1,
                .city = oBook(0).BookOrigCity,
                .stateProvinceCode = oBook(0).BookOrigState,
                .countryCode = "US",
                .postalCode = oBook(0).BookOrigZip,
                .specialRequirement = oOrigSpecialReq,
                .customerLocationId = oBook(0).BookOrigName,
                .referenceNumbers = oOrigRefs.ToArray()
            }

            Dim UPSStop = New UPS.UPSAddress() With {
                .locationName = oBook(0).BookDestName,
                .address1 = oBook(0).BookDestAddress1,
                .city = oBook(0).BookDestCity,
                .stateProvinceCode = oBook(0).BookDestState,
                .countryCode = "US",
                .postalCode = oBook(0).BookDestZip,
                .specialRequirement = oOrigSpecialReq,
                .customerLocationId = oBook(0).BookDestName,
                .referenceNumbers = oOrigRefs.ToArray()
            }
            .lStops.Add(UPSStop)
            Dim iTmp As Integer = 0
            Dim ideclaredValueTotal As Integer = 0
            For Each ITM In oItems
                Dim iactualWeight = If(CInt(ITM.BookPkgWeight) > 0, CInt(ITM.BookPkgWeight), 100)
                Console.WriteLine(iactualWeight)
            Next
            Dim lineItems As List(Of UPS.UPSItem) = (From i In oItems Select New UPS.UPSItem() With {
                    .description = If(String.IsNullOrWhiteSpace(i.BookPkgDescription), "misc products", i.BookPkgDescription),
                    .freightClass = If(Integer.TryParse(i.BookPkgFAKClass, iTmp), iTmp, 100),
                    .actualWeight = If(CInt(i.BookPkgWeight) > 0, CInt(i.BookPkgWeight), 100),
                    .weightUnit = "Pounds",
                    .length = i.BookPkgLength,
                    .width = i.BookPkgWidth,
                    .height = i.BookPkgHeight,
                    .pallets = i.BookPkgCount,
                    .pieces = i.BookPkgCount,
                    .palletSpaces = i.BookPkgCount,
                    .packagingCode = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                    .productName = "goods",
                    .declaredValue = 1000,
                    .isStackable = If(String.IsNullOrWhiteSpace(i.BookPkgStackable), "false", i.BookPkgStackable.ToString().ToLower()),
                    .referenceNumbers = oOrigRefs.ToArray()
                }).ToList()
            .lItems = lineItems
            .declaredValue = lineItems.Sum(Function(x) x.declaredValue)
        End With

        Return oRet
    End Function




    ''' <summary>
    ''' Add the quotes to the bid table
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks>
    ''' TODO: modify logic for .BidErrors = Left(oResponse.concateMessages, 3999), we use this field for reponse data
    ''' </remarks>
    Public Function InsertUPSRateQuoteBids(ByVal LoadTenderControl As Integer, ByVal oResponse As UPS.UPSQuoteResponse, ByVal sCarrierNumber As String, Optional sFreightClass As String = "Parcel") As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)

            Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = LoadTenderControl).FirstOrDefault()
            Dim sRetMSg As String = ""
            'If Not CreateNGLAPIBid(oResponse, oLoadTender.LoadTenderControl, oLoadTender.LTBookSHID, oLoadTender.LTBookOrigState, oLoadTender.LTBookDestState, 0, sRetMSg, 0, oLoadTender.LTBookDateRequired, BSCEnum.Quoted) Then
            '    throwUnExpectedFaultException(sRetMSg)
            'End If
            'Begin 

            Dim SHID As String = oLoadTender.LTBookSHID
            Dim origST As String = oLoadTender.LTBookOrigState
            Dim destST As String = oLoadTender.LTBookDestState
            Dim intCompControl As Integer = 0
            Dim strMsg As String = ""
            Dim BookControl As Integer = 0
            Dim DefaultRequiredDate As Date? = oLoadTender.LTBookDateRequired
            Dim bStatusCode As BSCEnum = BSCEnum.Active



            If LoadTenderControl = 0 Then
                Dim lDetails As New List(Of String) From {"Load Tendered Reference", " cannot be found and "}
                throwInvalidKeyParentRequiredException(lDetails)
                Return False
            End If

            Dim sSCAC = "UPS" 'oLoadTender.LTCarrierSCAC
            Dim iCarrierControl As Integer = 0
            Dim sLTCarrierName = "UPS"
            Dim sCNS = oLoadTender.LTBookConsPrefix
            Dim sOrderNbr = oLoadTender.LTBookCarrOrderNumber
            Dim sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

            Dim dtNow As Date = Date.Now
            Dim dtDefaultRequiredDate As Date = If(DefaultRequiredDate, dtNow.AddDays(3)) ' TODO: add transit time
            Dim blnInvalidData As Boolean = False
            'If oResponse Is Nothing OrElse oResponse.RateResponse Is Nothing OrElse oResponse.RateResponse.Count() < 1 Then
            If oResponse Is Nothing OrElse oResponse.RateResponse Is Nothing Then
                oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim iCarrierNumber As Integer = 0
            If Not Integer.TryParse(sCarrierNumber, iCarrierNumber) Then
                oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim oCarrier = db.CarrierRefIntegrations.Where(Function(x) x.CarrierNumber = iCarrierNumber).FirstOrDefault()
            If oCarrier Is Nothing OrElse oCarrier.CarrierControl = 0 Then
                oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            Else
                sSCAC = oCarrier.CarrierSCAC
                sLTCarrierName = oCarrier.CarrierName
                sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                iCarrierControl = oCarrier.CarrierControl
            End If

            Try
                'Modified by RHR for v-8.5.4.001 on 07/06/2023 new logic to read Carrier Costing Parameters
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                If intCompControl = 0 Then
                    intCompControl = Sec.getLECompControl()
                End If
                Dim lCarrier As List(Of Integer) = Sec.RestrictedCarriersForSalesReps()
                If Not blnInvalidData And Not oResponse.postMessagesOnly Then
                    'For Each q As UPS.UPSQuoteSummary In oResponse.RateResponse
                    Dim q As UPS.UPSQuoteSummary = oResponse.RateResponse
                    sSCAC = oCarrier.CarrierSCAC
                    sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", oCarrier.CarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

                    Dim oCarrierData As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
                    If Not String.IsNullOrEmpty(sSCAC) Then
                        'get the carrier data
                        oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, sSCAC).FirstOrDefault()
                    End If
                    If oCarrierData Is Nothing OrElse oCarrierData.CarrierControl = 0 Then
                        oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                        blnInvalidData = True
                        Return False 'note we need to test this logic
                        'Continue For 'We still want to try to create any other bids -- the strMsg results should be logged by the caller
                    End If

                    If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(oCarrierData.CarrierControl))) Then
                        oResponse.AddMessage(UPS.UPSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                        blnInvalidData = True
                        Return False 'note we need to test this logic
                    End If

                    Dim fuelTotal As Decimal = 0
                    Dim fuelVar As Decimal = 0
                    Dim dLIneHaul As Decimal = 0
                    Dim Adjs As New List(Of LTS.tblBidCostAdj)
                    Dim iBidAdjustmentCount As Integer = 0
                    Dim dTotalAdjs As Decimal = 0
                    If Not q Is Nothing AndAlso Not q.RatedShipment Is Nothing Then
                        If Not q.RatedShipment.ItemizedCharges Is Nothing AndAlso q.RatedShipment.ItemizedCharges.Count() > 0 Then
                            For Each a As UPS.UPSResponseItemizedCharge In q.RatedShipment.ItemizedCharges
                                Dim sCodeValue As String = a.Code
                                Dim iCode As Integer = 0
                                Integer.TryParse(sCodeValue, iCode)
                                'Modified by RHR for v-8.5.4.001 removed <> 375 this is fuel
                                '   Added new logic to assign the Cost Adjustment Type
                                'If iCode > 0 AndAlso iCode <> 375 Then
                                If iCode > 0 Then
                                    iBidAdjustmentCount += 1
                                    dTotalAdjs += a.MonetaryValue
                                    Dim sEDICode As String = UPS.UPSAPI.getUPSChargeCodeEDICode(iCode)
                                    If sEDICode = "FUE" Then
                                        fuelTotal += a.MonetaryValue
                                        fuelVar = 1
                                    End If

                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                  .BidCostAdjWeight = q.RatedShipment.BillingWeight.Weight,
                                                                  .BidCostAdjAmount = a.MonetaryValue,
                                                                  .BidCostAdjRate = 1,
                                                                  .BidCostAdjDescCode = sEDICode,
                                                                  .BidCostAdjDesc = UPS.UPSAPI.getUPSChargeCodeDesc(iCode),
                                                                  .BidCostAdjTypeControl = UPS.UPSAPI.getUPSChargeCodeCostAdjType(iCode),
                                                                  .BidCostAdjModDate = dtNow,
                                                                  .BidCostAdjModUser = Me.Parameters.UserName})
                                End If
                            Next
                        End If
                        If q.RatedShipment IsNot Nothing AndAlso q.RatedShipment.BaseServiceCharge IsNot Nothing Then
                            dLIneHaul = q.RatedShipment.BaseServiceCharge.MonetaryValue
                        End If
                        If dLIneHaul = 0 AndAlso q.RatedShipment IsNot Nothing AndAlso q.RatedShipment.TransportationCharges IsNot Nothing Then
                            dLIneHaul = q.RatedShipment.TransportationCharges.MonetaryValue
                        End If
                        If q.totalCharge = 0 Then
                            q.totalCharge = q.RatedShipment.TotalCharges.MonetaryValue
                        End If
                        If dLIneHaul = 0 Then
                            If dTotalAdjs <> 0 Then
                                dLIneHaul = q.totalCharge - dTotalAdjs
                            Else
                                dLIneHaul = q.totalCharge
                            End If
                        End If
                        Dim dblCarrierCostUpcharge As Double = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(iCarrierControl, intCompControl)
                        Dim sServiceType = Ngl.FM.UPSAPI.UPSAPI.getUPSServiceModeDesc(oResponse.RateResponse.RatedShipment.Service.Code)
                        Dim dblBidCustLineHaul = dLIneHaul + (dLIneHaul * dblCarrierCostUpcharge)
                        Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                  .BidCostAdjWeight = q.RatedShipment.BillingWeight.Weight,
                                                                  .BidCostAdjAmount = dblBidCustLineHaul,
                                                                  .BidCostAdjRate = 1,
                                                                  .BidCostAdjDescCode = "UPLF",
                                                                  .BidCostAdjDesc = "Customer Line Haul Charges",
                                                                  .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                                                                  .BidCostAdjModDate = dtNow,
                                                                  .BidCostAdjModUser = Me.Parameters.UserName})

                        Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                  .BidCostAdjWeight = q.RatedShipment.BillingWeight.Weight,
                                                                  .BidCostAdjAmount = dLIneHaul,
                                                                  .BidCostAdjRate = 1,
                                                                  .BidCostAdjDescCode = "400",
                                                                  .BidCostAdjDesc = sServiceType & " Charges",
                                                                  .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierLineHaul,
                                                                  .BidCostAdjModDate = dtNow,
                                                                  .BidCostAdjModUser = Me.Parameters.UserName})
                        Dim strErrs = "" ' sb.ToString()
                        Dim strInfos = "" 'sb.ToString()
                        Dim strWarnings = ""
                        Dim blnInterline As Boolean = False
                        Dim bArchived = 0
                        'TODO: add logic for minimim transit days and max and Min delivery date
                        ' Also we need to apply new Lead Time calculation we should not just change the delivery date?
                        Dim dtLoad As Date = If(oLoadTender.LTBookDateLoad, Date.Now.AddDays(1))
                        Dim dtBidDeliveryDate As Date? = dtLoad.AddDays(q.transit.maximumTransitDays)
                        Dim dtBidQuoteDate As Date? = dtNow
                        Dim dtBidExpirationDate As Date = dtNow

                        Dim dTransitDays As Double = Ngl.FM.UPSAPI.UPSAPI.getUPSTransitDaysByServiceMode(oResponse.RateResponse.RatedShipment.Service.Code)
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                .BidBidTypeControl = BidTypeEnum.UPSAPI,
                                                .BidCarrierControl = oCarrierData.CarrierControl,
                                                .BidCarrierNumber = If(oCarrierData.CarrierNumber, 0),
                                                .BidCarrierName = Left(oCarrierData.CarrierName, 40),
                                                .BidCarrierSCAC = Left(sSCAC, 4),
                                                .BidSHID = SHID,
                                                .BidTotalCost = q.totalCharge,
                                                .BidLineHaul = dLIneHaul,
                                                .BidFuelTotal = fuelTotal,
                                                .BidFuelVariable = fuelVar,
                                                .BidFuelUOM = "Flat Rate",
                                                .BidOrigState = Left(origST, 2),
                                                .BidDestState = Left(destST, 2),
                                                .BidPosted = dtNow,
                                                .BidStatusCode = bStatusCode,
                                                .BidArchived = bArchived,
                                                .BidMode = q.transportModeType,
                                                .BidErrorCount = 0,
                                                .BidErrors = Left(strErrs, 3999),
                                                .BidWarnings = Left(strWarnings, 3999),
                                                .BidInfos = Left(strInfos, 3999),
                                                .BidInterLine = blnInterline,
                                                .BidQuoteNumber = Left(q.quoteId, 100),
                                                .BidTransitTime = dTransitDays,
                                                .BidDeliveryDate = dtBidDeliveryDate,
                                                .BidQuoteDate = dtBidQuoteDate,
                                                .BidTotalWeight = oLoadTender.LTBookTotalWgt,
                                                .BidDetailTotal = 0,
                                                .BidDetailTransitTime = 0,
                                                .BidAdjustments = q.totalCharge - dLIneHaul, 'difference between line haul and total cost 
                                                .BidAdjustmentCount = iBidAdjustmentCount,
                                                .BidVendor = Left(sSCAC, 20),
                                                .BidContractID = Left(q.quoteId, 50),
                                                .BidServiceType = sServiceType,
                                                .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                .BidComments = "",
                                                .BidExpires = dtBidExpirationDate,
                                                .BidCustLineHaul = dblBidCustLineHaul,
                                                .BidCustTotalCost = (q.totalCharge - dLIneHaul) + dblBidCustLineHaul,
                                                .BidModDate = dtNow,
                                                .BidModUser = Me.Parameters.UserName}

                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        Dim oT = db.tblBidCostAdjs
                        For Each adj In Adjs
                            adj.BidCostAdjBidControl = bidCtrl
                            oT.InsertOnSubmit(adj)
                        Next

                        Dim lMessages As List(Of UPS.UPSMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            'Reset blnInvalidData flag to false because we have already logged the messages
                            blnInvalidData = False
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                            .BidSvcErrBidControl = bidCtrl,
                            .BidSvcErrErrorMessage = msg.Message,
                            .BidSvcErrVendorErrorCode = msg.VendorErrorCode,
                            .BidSvcErrModDate = dtNow,
                            .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    Else
                        blnInvalidData = True

                    End If
                End If

                If blnInvalidData Or oResponse.postMessagesOnly Then
                    Dim lMessages As List(Of UPS.UPSMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                    If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.UPSAPI,
                                                            .BidCarrierControl = iCarrierControl,
                                                            .BidCarrierNumber = iCarrierNumber,
                                                            .BidCarrierName = Left(sLTCarrierName, 40),
                                                            .BidCarrierSCAC = Left(sSCAC, 4),
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = 0,
                                                            .BidLineHaul = 0,
                                                            .BidFuelTotal = 0,
                                                            .BidFuelVariable = 0,
                                                            .BidFuelUOM = "NA",
                                                            .BidOrigState = Left(origST, 2),
                                                            .BidDestState = Left(destST, 2),
                                                            .BidPosted = dtNow,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = False,
                                                            .BidMode = "NA",
                                                            .BidErrorCount = lMessages.Count(),
                                                            .BidErrors = Left(oResponse.concateMessages, 3999),
                                                            .BidVendor = Left(sSCAC, 20),
                                                            .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                            .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                            .BidModDate = dtNow,
                                                            .BidModUser = Me.Parameters.UserName}

                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                    .BidSvcErrBidControl = bidCtrl,
                                    .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                    .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                    .BidSvcErrFieldName = msg.FieldName,
                                    .BidSvcErrModDate = dtNow,
                                    .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    End If

                End If

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertUPSRateQuoteBids"))
            End Try
            Return False
        End Using
    End Function

#End Region

#Region "Begin JTS API Logic"

    ''' <summary>
    '''  Retrieve a Rate Request for a single shipment LTL or Truckload
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Created  By RHR for v-8.5.1.001 on 01/25/2022 
    '''     1. get the token (note: we should be able to cache the token and the expired date
    '''     2. Call the API GetRateQuotes method using the oAPILoadData
    '''     3. handle errors and validate results
    '''     4. Call the InsertAPIQuoteBids (copy logic from InsertLTRateQuoteBids)
    ''' TODO:  
    '''     1. map item cost to RateRequestItem data replace .declaredValue = 1000,
    '''     2. map TMS Pallet Type to replace .packagingCode = "PLT" We need a cross refence with each API
    '''     3. Find out the best way to map .productName = itm.ItemNumber to something other than ItemNumber
    '''     4. Create a cross reference and replace .temperatureSensitive = "Dry", with the correct JTS Code for the item
    '''     5. Create a setup or parameter to create a cross reference and replace .temperatureUnit = "Fahrenheit", with the correct JTS Code for the item
    '''     6. Map .requiredTemperatureHigh = 85, and .requiredTemperatureLow = 35, to the correct temperature settings in TMS
    '''     7. Create a new way to replace the following:
    '''         (a) .unitsPerPallet = calcUnitsPerPallet(itm.Quantity,itm.PalletCount),
    '''         (b) .unitWeight = 0
    '''         (c) .unitVolume = 0,
    '''         (d) .hazardousEmergencyPhone = "5555555555",
    '''         (e) .upc = "",    
    '''         (f) .sku = "",
    '''         (g) .plu = ""
    ''' </remarks>
    Public Function ProcessJTSRateRequest(ByVal order As Models.RateRequestOrder,
                                          ByVal LoadTenderControl As Integer,
                                          ByVal oLEConfig As LTS.tblSSOALEConfig,
                                          ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                          Optional ByRef strMsg As String = "") As Boolean

        Dim oResponse As New JTS.JTSQuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oJTSAPI As New JTS.JTSAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)

        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0

        If order.Accessorials Is Nothing Then
            order.Accessorials = New String() {}
        End If
        Dim blnContinue = oLT.GetInfoForLTRateQuoteTariffBids(LoadTenderControl, order.Accessorials, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)

        If Not blnContinue Then Return False 'Nothing to do
        If order.ShipDate < Date.Now.AddDays(1) Then Return False ' TODO add message to Error Logs  invalid ship date

        If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
            'this should never happen unless there is a design bug.
            throwInvalidOperatonException("Shipping information is missing stop data")
            Return False
        End If

        'Read the default config settings
        Dim sMode As String = "TL"
        Dim sEquipment As String = "Van"
        Dim sLocationID As String = "C377465" 'default for Tree Top
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000

        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LocationID")) Then sLocationID = lCompConfig.Where(Function(x) x.SSOACName = "LocationID").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If

        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        Dim TotalCases = 1
        Dim dblTotalWeight As Double = 1
        Dim TotalPL = 1
        Dim TotalLen = 48
        Dim TotalWidth = 42
        Dim TotalHeight = 48
        Dim TotalCube = 0
        Dim oRateRequest = New JTS.RateRequest()
        Dim oOrigSpecialReq As JTS.JTSSpecialRequirement = New JTS.JTSSpecialRequirement()
        Dim oOrigRefs As New List(Of JTS.JTSReferenceNumbers)  '[] = new JTSReferenceNumbers[1];
        JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.SHID, order.Stops(0).SHID, oOrigRefs)
        JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.CON, order.Stops(0).BookCarrOrderNumber, oOrigRefs)
        JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.CRID, order.Stops(0).BookProNumber, oOrigRefs)
        JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.PU, order.BookConsPrefix, oOrigRefs)
        JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.DEL, order.Stops(0).BookCarrOrderNumber, oOrigRefs)
        oRateRequest.oOrigin = New JTS.JTSAddress() With {
            .locationName = order.Pickup.CompName,
            .address1 = order.Pickup.CompAddress1,
            .city = order.Pickup.CompCity,
            .stateProvinceCode = order.Pickup.CompState,
            .countryCode = "US",
            .postalCode = order.Pickup.CompPostalCode,
            .customerLocationId = order.Pickup.CompName,
            .referenceNumbers = oOrigRefs.ToArray()
        }
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        iCompControl = order.BookCustCompControl
        'get a default FAK class
        Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)

        oRateRequest.lStops = New List(Of JTS.JTSAddress)
        oRateRequest.lItems = New List(Of JTS.JTSItem)
        Dim iTmp As Integer = 0

        For Each s In order.Stops
            Dim oItems = New List(Of Models.RateRequestItem)
            oLT.fillRateRequestItems(s, oItems)
            If Not oItems Is Nothing AndAlso oItems.Count() > 0 Then
                dblTotalWeight = oItems.Sum(Function(x) x.Weight)
                '**************** Begin Validate Mode and weight  ***************************

                Dim blnShipLTL As Boolean = False
                If dblTotalWeight < dLowLTLWeight Then
                    oRateRequest.setPostMessageOnlyFlag(True)
                    oRateRequest.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooLowForLTL, " The JTS settings require LTL shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                    Exit For
                End If
                If dblTotalWeight <= dHighLTLWeight Then
                    sMode = "LTL"
                Else
                    'try truckload
                    If dblTotalWeight < dLowTLWeight Then
                        'check if we send the LTL and TL warning  or just the  warning
                        If dHighLTLWeight > 0 Then
                            'we are between LTL and Truckload
                            oRateRequest.setPostMessageOnlyFlag(True)
                            oRateRequest.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooHighForLTL, " The JTS settings require LTL shipments to have a weight less than " & dHighLTLWeight.ToString() & " or a Truckload weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                            Exit For
                        Else 'just send the Truckload warning
                            oRateRequest.setPostMessageOnlyFlag(True)
                            oRateRequest.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooLowForTL, " The JTS settings require Truckload shipments to have a weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                            Exit For
                        End If
                    ElseIf dblTotalWeight > dHighTLWeight Then
                        oRateRequest.setPostMessageOnlyFlag(True)
                        oRateRequest.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooHighForLTL, " The JTS settings require Truckload shipments to have a weight less than " & dHighTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS. ", "", "BookTotalWgt")
                        Exit For
                    End If
                End If

                '****************** End Validate Mode and weight *******************************
                Dim JTSStop = New JTS.JTSAddress() With {
                    .locationName = s.CompName,
                    .address1 = s.CompAddress1,
                    .city = s.CompCity,
                    .stateProvinceCode = s.CompState,
                    .countryCode = "US",
                    .postalCode = s.CompPostalCode,
                    .customerLocationId = s.CompName,
                    .referenceNumbers = oOrigRefs.ToArray()
                }
                oRateRequest.lStops.Add(JTSStop)
                oRateRequest.setShipDate(order.ShipDate)
                oRateRequest.setMode(sEquipment, 1, sMode)

                TotalCases = oItems.Sum(Function(x) x.NumPieces)
                TotalPL = oItems.Sum(Function(x) x.PalletCount)
                TotalLen = oItems.Sum(Function(x) x.Length)
                TotalWidth = oItems.Sum(Function(x) x.Width)
                TotalHeight = oItems.Sum(Function(x) x.Height)

                For Each itm As Models.RateRequestItem In oItems
                    Dim iInvRecNo As Integer = getJTSRecNo(itm.FreightClass)
                    Dim oItem = New JTS.JTSItem() With {.height = CInt(itm.Height).ToString(), .weight = CInt(itm.Weight).ToString(), .palletCount = itm.PalletCount, .itemClass = itm.FreightClass, .itemInvRecNo = iInvRecNo}
                    '    .length = itm.Length,
                    '    .width = itm.Width,
                    '    .height = itm.Height,
                    '    .pallets = itm.PalletCount
                    '}
                    'Dim oItem = New JTS.JTSItem() With {
                    '.Description = If(String.IsNullOrWhiteSpace(itm.Description), "misc products", itm.Description),
                    '.freightClass = If(Integer.TryParse(itm.FreightClass, iTmp), iTmp, 100),
                    '.actualWeight = If(Integer.TryParse(itm.Weight.ToString(), iTmp), iTmp, 100),
                    '.weightUnit = If(String.IsNullOrWhiteSpace(itm.WeightUnit), "Pounds", itm.WeightUnit),
                    '.length = itm.Length,
                    '.width = itm.Width,
                    '.height = itm.Height,
                    '.pallets = itm.PalletCount,
                    '.pieces = itm.NumPieces,
                    '.palletSpaces = itm.PalletCount,
                    '.packagingCode = "PLT",
                    '.productName = itm.ItemNumber,
                    '.declaredValue = 1000,
                    '.temperatureSensitive = "Dry",
                    '.temperatureUnit = "Fahrenheit",
                    '.requiredTemperatureHigh = 85,
                    '.requiredTemperatureLow = 35,
                    '.isStackable = If(itm.Stackable, "true", "false"),
                    '.referenceNumbers = oOrigRefs.ToArray()
                    '}
                    oRateRequest.lItems.Add(oItem)

                Next
            End If
        Next

        Return GetJTSRates(oRateRequest, LoadTenderControl, oLEConfig, lCompConfig, strMsg)

    End Function


    Function GetJTSRates(ByRef oRateRequest As JTS.RateRequest,
                         ByVal LoadTenderControl As Integer,
                         ByVal oLEConfig As LTS.tblSSOALEConfig,
                         ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                         Optional ByRef strMsg As String = "") As Boolean



        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oJTSAPI As New JTS.JTSAPI(bUseTLS12)

        Dim oResponse As New JTS.JTSQuoteResponse()
        If oLEConfig Is Nothing OrElse String.IsNullOrWhiteSpace(oLEConfig.SSOALEClientID) Then
            Return False
        End If
        Dim sclient_id As String = oLEConfig.SSOALEClientID
        Dim sclient_secret As String = oLEConfig.SSOALEClientSecret
        Dim saudience As String = oLEConfig.SSOALELoginURL
        Dim sgrant_type As String = oLEConfig.SSOALEAuthCode
        Dim sDataURL As String = oLEConfig.SSOALEDataURL

        Dim sCarrierNumber As String = "0"
        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfig.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If

        Dim dShipDate As Date?
        If Not oRateRequest Is Nothing Then
            dShipDate = oRateRequest.getNGLShipDate()
        End If
        If oRateRequest.getPostMessageOnlyFlag() = True Then
            oResponse.postMessagesOnly = True
            Dim oMessages = oRateRequest.GetMessages()
            For Each msg In oMessages
                oResponse.AddMessage(msg)
            Next
        Else
            If Not dShipDate.HasValue OrElse dShipDate.Value <= Date.Now.AddDays(1) Then
                Dim sShipDateTxt As String = "Missing"
                If dShipDate.HasValue Then
                    sShipDateTxt = dShipDate.Value.ToString()
                End If
                oResponse.postMessagesOnly = True
                oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_InvalidShipDate, "JTS requires more advanced notice to rate this load shipping on " & sShipDateTxt, "", "LoadDate")
            End If
        End If


        If Not oResponse.postMessagesOnly Then
            Try



                'Dim oRes As JTS.JTSTokenData = oJTSAPI.getToken(sclient_id, sclient_secret, saudience, sgrant_type)
                'If Not oRes Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oRes.access_token) Then
                oResponse = oJTSAPI.getHTTPRateRequest(sgrant_type, oRateRequest, sDataURL)
                'End If


                If oResponse Is Nothing Then
                    oResponse = New JTS.JTSQuoteResponse()
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid", "", "")
                ElseIf oResponse.rates Is Nothing OrElse oResponse.rates.Count() < 1 Then
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_NoRatesFound, "No JTS API rates are available", "", "")
                End If
                If sCarrierNumber = "0" Then
                    oResponse.postMessagesOnly = True
                    oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_InvalidCarrierNumber, "Fix the API SSOA Config setting for CarrierNumber to save rates", "", "CarrierNumber")
                End If
            Catch ex As Exception
                oResponse = New JTS.JTSQuoteResponse()
                oResponse.postMessagesOnly = True
                oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your API Vendor.  The actual Error is: " & ex.Message, "", "")

            End Try
        End If

        ' we now insert a quote with error messages even if one is not available 
        ' using the postMessagesOnly flag with a zero cost.  This logic will help
        ' users track issues with API rating
        InsertJTSRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
        Return True
    End Function



    ''' <summary>
    ''' use CreateP44Bid when generating rates from an existing order,
    ''' Use CreateP44Quote when an existing order does not exist, 
    ''' CreateP44Bid expects the caller to create a tblLoadTender Record 
    ''' and pass in the the LoadTenderControl to use
    ''' to create a record in tblBids (and tblBidCostAdj and tblBidSvcErr)
    ''' for each quote returned from P44
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SSOAAct"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rules:
    ''' If P44 q.deliveryDate is nothing then use BookDateRequired
    ''' If P44 q.quoteDate is nothing then use current date
    ''' If P44 q.interLine comes back null or with the word false BidInterline is false
    ''' In all other cases BidInterline is true
    ''' TODO:
    ''' Figure out how to get the linehaul out of the P44 data
    ''' Figure out how to get FuelUOM out of P44 data
    ''' Modified by RHR for v-8.2 on 12/11/2018
    '''   added logic to use the P44AccountGroup and moved logic to 
    '''   read SSOA and create P44 proxy to shared functions that can be called 
    '''   by other procedures
    '''   returns false on error and inserts error message into strMsg
    ''' </remarks>
    Public Function CreateJTSBid(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 ByVal oLEConfig As LTS.tblSSOALEConfig,
                                 ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                 Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.JTSAPI,
                                 Optional ByRef strMsg As String = "") As Boolean
        Dim oLTLs As LTS.vBookRevenue() = DirectCast(Me.NDPBaseClassFactory("NGLBookRevenueData", False), NGLBookRevenueData).GetLTLvBookRevenues(BookControl)
        If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then Return True 'nothing to do but we did not fail
        Dim origST = oLTLs(0).BookOrigState
        Dim destST = oLTLs(0).BookDestState
        Dim dtNow = Date.Now
        Dim oRateRequest As JTS.RateRequest = CopyBookDataToJTSData(oLTLs, lCompConfig)
        ' CopyBookDataToJTSData()
        If oRateRequest Is Nothing Then Return True 'nothing to do but we did not fail

        Return GetJTSRates(oRateRequest, LoadTenderControl, oLEConfig, lCompConfig, strMsg)

    End Function

    Public Function getJTSRecNo(sFak As String) As Integer

        'Dim dClass50RecNo As Double = 64428
        'Dim dClass55RecNo As Double = 64429
        'Dim dClass60RecNo As Double = 64430
        'Dim dClass65RecNo As Double = 64431
        'Dim dClass70RecNo As Double = 64432
        'Dim dClass77_5RecNo As Double = 64433
        'Dim dClass85RecNo As Double = 64434
        'Dim dClass92_5RecNo As Double = 64435
        'Dim dClass100RecNo As Double = 64436
        'Dim dClass125RecNo As Double = 64437
        'Dim dClass175RecNo As Double = 64438
        'Dim dClass250RecNo As Double = 64439
        'Dim dClass300RecNo As Double = 64440
        Dim iRet As Integer = 64436
        Select Case sFak
            Case "50"
                iRet = 64428
            Case "55"
                iRet = 64429
            Case "60"
                iRet = 64430
            Case "65"
                iRet = 64431
            Case "70"
                iRet = 64432
            Case "77.5"
                iRet = 64433
            Case "85"
                iRet = 64434
            Case "92.5"
                iRet = 64435
            Case "100"
                iRet = 64436
            Case "125"
                iRet = 64437
            Case "175"
                iRet = 64438
            Case "250"
                iRet = 64439
            Case "300"
                iRet = 64440
            Case Else
                iRet = 64436

        End Select
        Return iRet
    End Function


    Protected Function CopyBookDataToJTSData(ByRef oBook() As LTS.vBookRevenue, ByVal lCompConfig As List(Of LTS.tblSSOAConfig), Optional ByVal timeOut As Integer = 20, Optional ByVal sDefFrtClass As String = "70") As JTS.RateRequest
        Dim oRet As New JTS.RateRequest()
        If ((oBook Is Nothing) OrElse oBook.Count() < 1 OrElse (oBook(0).BookControl = 0)) Then Return Nothing

        Dim oResponse As New JTS.JTSQuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oJTSAPI As New JTS.JTSAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)


        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0
        Dim iCompControl As Integer = 0
        Dim dDefaultFreightClass As Double = 100
        Dim dtShip As Date = Date.Now.AddDays(2) 'JTS rates need 2 days 
        Dim dtRequired As Date = dtShip.AddDays(4)
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        Dim oAccs As LTS.vBookAccessorial()

        'Read the default config settings
        Dim sMode As String = "LTL"
        Dim sEquipment As String = "Van"
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000
        Dim iRecNo As Integer = 64436 'mapping for FAK 100 is default



        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        'Note: in v-8.5.1 we only support one booking record
        'For Each book In oBook
        Dim book As LTS.vBookRevenue = oBook(0)

        If Not book Is Nothing AndAlso book.BookControl <> 0 AndAlso Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 Then
            iCompControl = book.BookCustCompControl
            '**************** Begin Validate Mode and weight  ***************************

            Dim blnShipLTL As Boolean = False
            Dim dblTotalWeight As Double = 0
            If book.BookTotalWgt.HasValue Then dblTotalWeight = Math.Ceiling(book.BookTotalWgt.Value)
            If dblTotalWeight < dLowLTLWeight Then
                oRet.setPostMessageOnlyFlag(True)
                oRet.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooLowForLTL, " The JTS settings require LTL shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                Return oRet
            End If
            If dblTotalWeight <= dHighLTLWeight Then
                sMode = "LTL"
            Else
                'try truckload
                If dblTotalWeight < dLowTLWeight Then
                    'check if we send the LTL and TL warning  or just the  warning
                    If dHighLTLWeight > 0 Then
                        'we are between LTL and Truckload
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooHighForLTL, " The JTS settings require LTL shipments to have a weight less than " & dHighLTLWeight.ToString() & " or a Truckload weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                        Return oRet
                    Else 'just send the Truckload warning
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooLowForTL, " The JTS settings require Truckload shipments to have a weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS.", "", "BookTotalWgt")
                        Return oRet
                    End If
                ElseIf dblTotalWeight > dHighTLWeight Then
                    oRet.setPostMessageOnlyFlag(True)
                    oRet.AddMessage(JTS.JTSAPI.MessageEnum.E_WeightTooHighForLTL, " The JTS settings require Truckload shipments to have a weight less than " & dHighTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for JTS. ", "", "BookTotalWgt")
                    Return oRet
                End If
            End If

            '****************** End Validate Mode and weight *******************************
            'get a default FAK class
            Integer.TryParse(GetParText("UseFAKDefault", iCompControl), dDefaultFreightClass)
            'get the earliest load date
            If book.BookDateLoad.HasValue Then dtShip = book.BookDateLoad.Value
            'get the latest delivery date
            If book.BookDateRequired.HasValue AndAlso book.BookDateRequired.Value > dtRequired Then dtRequired = book.BookDateRequired.Value
            Dim filters As New Models.AllFilters With {.ParentControl = book.BookControl}
            Dim ct As Integer
            Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
            If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                oItems.AddRange(oPkgs)
            End If
            ct = 0
            filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
            oAccs = oBookAccDAL.GetBookAccessorials(filters, ct)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                ' we save the fees so when the load is tendered or dispatched we can validate that the fees are added to the order
                ' some carrier API do not provide a list of fees.
                ' See further down in the procedure for custom maping to SpecialRequrement for CHR
                oBookAccessorial.AddRange(oAccs)
                sAccessorial.AddRange(oAccs.Select(Function(x) x.NACCode).ToList())
                oRet.oAccessorials = sAccessorial.ToArray()
                oRet.oFees = (From d In oAccs Select New JTS.JTSFees With {.BookAcssControl = d.BookAcssControl,
                                          .BookAcssNACControl = d.BookAcssNACControl,
                                          .BookAcssValue = d.BookAcssValue,
                                                  .NACCode = d.NACCode,
                                                  .NACName = d.NACName,
                                                  .AccessorialCode = d.AccessorialCode,
                                                  .AccessorialName = d.AccessorialName
                                        }).ToArray()
            End If

            'For Each bl In book.BookLoads
            '    If Not bl Is Nothing AndAlso Not bl.BookItems Is Nothing AndAlso bl.BookItems.Count() > 0 Then
            '        oItems.AddRange(bl.BookItems)
            '    End If
            'Next
        End If
        'Next
        If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

        Dim oOrigSpecialReq As JTS.JTSSpecialRequirement = New JTS.JTSSpecialRequirement()
        Dim oOrigRefs As New List(Of JTS.JTSReferenceNumbers)  '[] = new JTSReferenceNumbers[1];


        With oRet
            '.setMode(JTSAPI.EquipTypevan, 1, sMode)
            .lStops = New List(Of JTS.JTSAddress)
            .lItems = New List(Of JTS.JTSItem)
            .setShipDate(dtShip.ToShortDateString)
            'ToDo: add more accessorial fees to special req
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                For Each a As LTS.vBookAccessorial In oAccs
                    If a.AccessorialCode = 67 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.accessorialNeedsAppointmentNotification)
                    If a.AccessorialCode = 48 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.accessorialNeedsLiftGateRequired)
                    If a.AccessorialCode = 17 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.accessorialNeedsInsideDelivery)
                    'If a.AccessorialCode = 122 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.insidePickup)
                    If a.AccessorialCode = 68 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.accessorialNeedsResidentialDelivery)
                    If a.AccessorialCode = 132 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.accessorialNeedsResidentialDelivery)
                    'If a.AccessorialCode = 72 Then oOrigSpecialReq.setSpecialRequrement(JTS.JTSAPI.SpecialRequirement.constructionSite)
                Next
                oRet.oSpecial = oOrigSpecialReq
            End If
            Dim sCNS As String = If(String.IsNullOrWhiteSpace(oBook(0).BookConsPrefix), oBook(0).BookProNumber, oBook(0).BookConsPrefix)
            Dim sSHID As String = If(String.IsNullOrWhiteSpace(oBook(0).BookSHID), sCNS, oBook(0).BookSHID)
            Dim sDel As String = If(String.IsNullOrWhiteSpace(oBook(0).BookLoads(0).BookLoadPONumber), sCNS, oBook(0).BookLoads(0).BookLoadPONumber)
            JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.PU, sCNS, oOrigRefs)
            JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.SHID, sSHID, oOrigRefs)
            JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.DEL, sDel, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookCarrOrderNumber)) Then JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.CON, oBook(0).BookCarrOrderNumber, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookProNumber)) Then JTS.JTSReferenceNumbers.addReferenceNumber(JTS.JTSAPI.RefNumbers.CRID, oBook(0).BookProNumber, oOrigRefs)


            .oRefs = oOrigRefs.ToArray()
            .oOrigin = New JTS.JTSAddress() With {
                .locationName = oBook(0).BookOrigName,
                .address1 = oBook(0).BookOrigAddress1,
                .city = oBook(0).BookOrigCity,
                .stateProvinceCode = oBook(0).BookOrigState,
                .countryCode = "US",
                .postalCode = oBook(0).BookOrigZip,
                .customerLocationId = oBook(0).BookOrigName,
                .referenceNumbers = oOrigRefs.ToArray()
            }

            Dim JTSStop = New JTS.JTSAddress() With {
                .locationName = oBook(0).BookDestName,
                .address1 = oBook(0).BookDestAddress1,
                .city = oBook(0).BookDestCity,
                .stateProvinceCode = oBook(0).BookDestState,
                .countryCode = "US",
                .postalCode = oBook(0).BookDestZip
            }
            .lStops.Add(JTSStop)
            Dim iTmp As Integer = 0
            Dim ideclaredValueTotal As Integer = 0
            For Each ITM In oItems
                Dim iactualWeight = If(CInt(ITM.BookPkgWeight) > 0, Math.Ceiling(ITM.BookPkgWeight), 100)
                Console.WriteLine(iactualWeight)
            Next

            'Dim lineItems As List(Of JTS.JTSItem) = (From i In oItems Select New JTS.JTSItem() With {
            '                                                              .itemInvRecNo = getJTSRecNo(i.BookPkgFAKClass),
            '                                                              .itemClass = i.BookPkgFAKClass,
            '                                                              .height = i.BookPkgHeight,
            '                                                              .weight = i.BookPkgWeight,
            '                                                              .palletSizeId = 0,
            '                                                              .palletCount = i.BookPkgCount}).ToList()
            Dim lineItems As List(Of JTS.JTSItem) = (From i In oItems Select New JTS.JTSItem() With {
                                                                          .itemInvRecNo = getJTSRecNo(i.BookPkgFAKClass),
                                                                          .itemClass = i.BookPkgFAKClass,
                                                                          .height = i.BookPkgHeight,
                                                                          .weight = If(CInt(i.BookPkgWeight) > 0, Math.Ceiling(i.BookPkgWeight), 10),
                                                                          .palletSizeId = 0,
                                                                          .palletCount = i.BookPkgCount}).ToList()
            .lItems = lineItems

        End With

        Return oRet
    End Function



    ''' <summary>
    ''' Add the quotes to the bid table
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 07/07/2023 
    '''     added new logic to assign the Cost Adjustment Type 
    '''     added new logic to calculate the customer upcharge precent
    ''' </remarks>
    Public Function InsertJTSRateQuoteBids(ByVal LoadTenderControl As Integer, ByVal oResponse As JTS.JTSQuoteResponse, ByVal sCarrierNumber As String, Optional sFreightClass As String = "Truck") As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)

            Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = LoadTenderControl).FirstOrDefault()
            Dim sRetMSg As String = ""
            'If Not CreateNGLAPIBid(oResponse, oLoadTender.LoadTenderControl, oLoadTender.LTBookSHID, oLoadTender.LTBookOrigState, oLoadTender.LTBookDestState, 0, sRetMSg, 0, oLoadTender.LTBookDateRequired, BSCEnum.Quoted) Then
            '    throwUnExpectedFaultException(sRetMSg)
            'End If
            'Begin 

            Dim SHID As String = oLoadTender.LTBookSHID
            Dim origST As String = oLoadTender.LTBookOrigState
            Dim destST As String = oLoadTender.LTBookDestState
            Dim intCompControl As Integer = 0
            Dim strMsg As String = ""
            Dim BookControl As Integer = 0
            Dim DefaultRequiredDate As Date? = oLoadTender.LTBookDateRequired
            Dim bStatusCode As BSCEnum = BSCEnum.Active



            If LoadTenderControl = 0 Then
                Dim lDetails As New List(Of String) From {"Load Tendered Reference", " cannot be found and "}
                throwInvalidKeyParentRequiredException(lDetails)
                Return False
            End If

            Dim sSCAC = oLoadTender.LTCarrierSCAC
            Dim iCarrierControl As Integer = 0
            Dim sLTCarrierName = "JTS"
            Dim sCNS = oLoadTender.LTBookConsPrefix
            Dim sOrderNbr = oLoadTender.LTBookCarrOrderNumber
            Dim sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

            Dim dtNow As Date = Date.Now
            Dim dtDefaultRequiredDate As Date = If(DefaultRequiredDate, dtNow.AddDays(3)) ' TODO: add transit time
            Dim blnInvalidData As Boolean = False
            If oResponse Is Nothing OrElse oResponse.rates Is Nothing OrElse oResponse.rates.Count() < 1 Then
                oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim iCarrierNumber As Integer = 0
            If Not Integer.TryParse(sCarrierNumber, iCarrierNumber) Then
                oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim oCarrier = db.CarrierRefIntegrations.Where(Function(x) x.CarrierNumber = iCarrierNumber).FirstOrDefault()
            If oCarrier Is Nothing OrElse oCarrier.CarrierControl = 0 Then
                oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            Else
                sSCAC = oCarrier.CarrierSCAC
                sLTCarrierName = oCarrier.CarrierName
                sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                iCarrierControl = oCarrier.CarrierControl
            End If


            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                If intCompControl = 0 Then
                    intCompControl = Sec.getLECompControl()
                End If
                Dim lCarrier As List(Of Integer) = Sec.RestrictedCarriersForSalesReps()
                If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(iCarrierControl))) Then
                    oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                    blnInvalidData = True
                    Return False 'note we need to test this logic
                End If

                If Not blnInvalidData And Not oResponse.postMessagesOnly Then
                    For Each q As JTS.JTSQuoteSummary In oResponse.rates
                        'sSCAC = q.carrierSCAC
                        sLoadDetails = String.Format("JTL using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", oCarrier.CarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

                        'Dim oCarrierData As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
                        'If Not String.IsNullOrEmpty(sSCAC) Then
                        '    'get the carrier data
                        '    oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, sSCAC).FirstOrDefault()
                        'End If
                        'If oCarrierData Is Nothing OrElse oCarrierData.CarrierControl = 0 Then
                        '    'oResponse.AddMessage(JTS.JTSAPI.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                        '    blnInvalidData = True
                        '    Continue For 'We still want to try to create any other bids -- the strMsg results should be logged by the caller
                        'End If

                        Dim fuelTotal As Decimal = 0
                        Dim fuelVar As Decimal = 0
                        Dim dLIneHaul As Decimal = 0
                        Dim Adjs As New List(Of LTS.tblBidCostAdj)
                        Dim iBidAdjustmentCount As Integer = 0
                        ' Begin Modified by RHR for v-8.5.4.001 on 07/07/2023
                        '   added new logic to assign the Cost Adjustment Type
                        '   added new logic to calculate the customer upcharge precent
                        Dim dblCarrierCostUpcharge As Double = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(iCarrierControl, intCompControl)
                        Dim decBidCustLineHaul As Decimal = q.baseRate + (q.baseRate * dblCarrierCostUpcharge)
                        Dim decCustomerTotalCharges As Decimal = (q.totalRate - q.baseRate) + decBidCustLineHaul
                        Dim oBidAdj As New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = q.baseRate,
                                                                           .BidCostAdjRate = q.baseRate,
                                                                           .BidCostAdjDescCode = "Carrier Line Haul",
                                                                           .BidCostAdjDesc = "Carrier Line Haul",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierLineHaul,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        Adjs.Add(oBidAdj)
                        oBidAdj = New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = q.baseRate,
                                                                           .BidCostAdjRate = q.baseRate,
                                                                           .BidCostAdjDescCode = "Customer Line Haul",
                                                                           .BidCostAdjDesc = "Customer Line Haul",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        'Adjs.Add(oBidAdj)
                        oBidAdj = New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = q.fuelSurcharge,
                                                                           .BidCostAdjRate = q.fuelSurcharge,
                                                                           .BidCostAdjDescCode = "Fuel",
                                                                           .BidCostAdjDesc = "Fuel",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Fuel,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        Adjs.Add(oBidAdj)
                        oBidAdj = New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = q.accessorialTotal,
                                                                           .BidCostAdjRate = q.accessorialTotal,
                                                                           .BidCostAdjDescCode = "MSC",
                                                                           .BidCostAdjDesc = "Fees",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        Adjs.Add(oBidAdj)
                        oBidAdj = New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = q.totalRate,
                                                                           .BidCostAdjRate = q.totalRate,
                                                                           .BidCostAdjDescCode = "Carrier Total Charges",
                                                                           .BidCostAdjDesc = "Carrier Total Charges",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierTotalCost,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        Adjs.Add(oBidAdj)
                        oBidAdj = New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                           .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                           .BidCostAdjAmount = decCustomerTotalCharges,
                                                                           .BidCostAdjRate = decCustomerTotalCharges,
                                                                           .BidCostAdjDescCode = "Customer Total Charges",
                                                                           .BidCostAdjDesc = "Customer Total Charges",
                                                                           .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerTotalCost,
                                                                           .BidCostAdjModDate = dtNow,
                                                                           .BidCostAdjModUser = Me.Parameters.UserName}
                        Adjs.Add(oBidAdj)

                        Dim strErrs = "" ' sb.ToString()
                        Dim strInfos = "" 'sb.ToString()
                        Dim strWarnings = ""
                        Dim blnInterline As Boolean = False
                        Dim bArchived = 0
                        'TODO: add logic for minimim transit days and max and Min delivery date
                        Dim dtLoad As Date = If(oLoadTender.LTBookDateLoad, Date.Now.AddDays(1))
                        Dim dtBidDeliveryDate As Date? '= dtLoad.AddDays(q.transit.maximumTransitDays)
                        Dim dtBidQuoteDate As Date? = dtNow
                        Dim dtBidExpirationDate As Date = dtNow
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                        .BidBidTypeControl = BidTypeEnum.JTSAPI,
                                                        .BidCarrierControl = iCarrierControl,
                                                        .BidCarrierNumber = iCarrierNumber,
                                                        .BidCarrierName = q.carrierName,
                                                        .BidCarrierSCAC = Left(q.carrierSCAC, 4),
                                                        .BidSHID = SHID,
                                                        .BidTotalCost = q.totalRate,
                                                        .BidLineHaul = q.baseRate,
                                                        .BidFuelTotal = q.fuelSurcharge,
                                                        .BidFuelVariable = 0,
                                                        .BidFuelUOM = "",
                                                        .BidOrigState = Left(origST, 2),
                                                        .BidDestState = Left(destST, 2),
                                                        .BidPosted = dtNow,
                                                        .BidStatusCode = bStatusCode,
                                                        .BidArchived = bArchived,
                                                        .BidMode = 3, ' q.transportModeType,
                                                        .BidErrorCount = 0,
                                                        .BidErrors = Left(strErrs, 3999),
                                                        .BidWarnings = Left(strWarnings, 3999),
                                                        .BidInfos = Left(strInfos, 3999),
                                                        .BidInterLine = blnInterline,
                                                        .BidQuoteNumber = Left(q.quoteId, 100),
                                                        .BidTransitTime = q.transitDays,
                                                        .BidDeliveryDate = dtBidDeliveryDate,
                                                        .BidQuoteDate = dtBidQuoteDate,
                                                        .BidTotalWeight = oLoadTender.LTBookTotalWgt,
                                                        .BidDetailTotal = 0,
                                                        .BidDetailTransitTime = 0,
                                                        .BidAdjustments = q.totalRate - q.baseRate, 'difference between line haul and total cost 
                                                        .BidAdjustmentCount = iBidAdjustmentCount,
                                                        .BidVendor = Left(sSCAC, 20),
                                                        .BidContractID = Left(q.quoteId, 50),
                                                        .BidServiceType = "LTL", ' Left(q.transportModeType, 50),
                                                        .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                        .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                        .BidComments = "",
                                                        .BidExpires = dtBidExpirationDate,
                                                        .BidCustLineHaul = decBidCustLineHaul,
                                                        .BidCustTotalCost = decCustomerTotalCharges,
                                                        .BidModDate = dtNow,
                                                        .BidModUser = Me.Parameters.UserName}
                        ' End Modified by RHR for v-8.5.4.001 on 07/07/2023
                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        Dim oT = db.tblBidCostAdjs
                        For Each adj In Adjs
                            adj.BidCostAdjBidControl = bidCtrl
                            oT.InsertOnSubmit(adj)
                        Next

                        Dim lMessages As List(Of JTS.JTSMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            'Reset blnInvalidData flag to false because we have already logged the messages
                            blnInvalidData = False
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                .BidSvcErrBidControl = bidCtrl,
                                .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                .BidSvcErrMessage = msg.Details,
                                .BidSvcErrFieldName = msg.FieldName,
                                .BidSvcErrModDate = dtNow,
                                .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    Next
                End If

                If blnInvalidData Or oResponse.postMessagesOnly Then
                    Dim lMessages As List(Of JTS.JTSMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                    If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.JTSAPI,
                                                            .BidCarrierControl = iCarrierControl,
                                                            .BidCarrierNumber = iCarrierNumber,
                                                            .BidCarrierName = Left(sLTCarrierName, 40),
                                                            .BidCarrierSCAC = Left(sSCAC, 4),
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = 0,
                                                            .BidLineHaul = 0,
                                                            .BidFuelTotal = 0,
                                                            .BidFuelVariable = 0,
                                                            .BidFuelUOM = "NA",
                                                            .BidOrigState = Left(origST, 2),
                                                            .BidDestState = Left(destST, 2),
                                                            .BidPosted = dtNow,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = False,
                                                            .BidMode = "NA",
                                                            .BidErrorCount = lMessages.Count(),
                                                            .BidErrors = Left(oResponse.concateMessages, 3999),
                                                            .BidVendor = Left(sSCAC, 20),
                                                            .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                            .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                            .BidModDate = dtNow,
                                                            .BidModUser = Me.Parameters.UserName}

                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                    .BidSvcErrBidControl = bidCtrl,
                                    .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                    .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                    .BidSvcErrFieldName = msg.FieldName,
                                    .BidSvcErrModDate = dtNow,
                                    .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    End If

                End If

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertJTSRateQuoteBids"))
            End Try
            Return False
        End Using
    End Function


#End Region

#Region "Begin CHR API Logic"

    ''' <summary>
    '''  Retrieve a Rate Request for a single shipment LTL or Truckload
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Created  By RHR for v-8.5.1.001 on 01/25/2022 
    '''     1. get the token (note: we should be able to cache the token and the expired date
    '''     2. Call the API GetRateQuotes method using the oAPILoadData
    '''     3. handle errors and validate results
    '''     4. Call the InsertAPIQuoteBids (copy logic from InsertLTRateQuoteBids)
    ''' TODO:  
    '''     1. map item cost to RateRequestItem data replace .declaredValue = 1000,
    '''     2. map TMS Pallet Type to replace .packagingCode = "PLT" We need a cross refence with each API
    '''     3. Find out the best way to map .productName = itm.ItemNumber to something other than ItemNumber
    '''     4. Create a cross reference and replace .temperatureSensitive = "Dry", with the correct CHR Code for the item
    '''     5. Create a setup or parameter to create a cross reference and replace .temperatureUnit = "Fahrenheit", with the correct CHR Code for the item
    '''     6. Map .requiredTemperatureHigh = 85, and .requiredTemperatureLow = 35, to the correct temperature settings in TMS
    '''     7. Create a new way to replace the following:
    '''         (a) .unitsPerPallet = calcUnitsPerPallet(itm.Quantity,itm.PalletCount),
    '''         (b) .unitWeight = 0
    '''         (c) .unitVolume = 0,
    '''         (d) .hazardousEmergencyPhone = "5555555555",
    '''         (e) .upc = "",    
    '''         (f) .sku = "",
    '''         (g) .plu = ""
    '''         
    ''' Modified by RHR for v-8.5.4.002 on 09/22/2023 moved code to CHRAPI
    '''     majority of processing is moved to API classes
    ''' </remarks>
    Public Function ProcessCHRRateRequest(ByVal order As Models.RateRequestOrder,
                                          ByVal LoadTenderControl As Integer,
                                          ByVal oLEConfig As LTS.tblSSOALEConfig,
                                          ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                          Optional ByRef strMsg As String = "") As Boolean

        Dim oResponse As New Map.QuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oChrAPI As New CHR.CHRAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)

        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0

        If order.Accessorials Is Nothing Then
            order.Accessorials = New String() {}
        End If
        Dim blnContinue = oLT.GetInfoForLTRateQuoteTariffBids(LoadTenderControl, order.Accessorials, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)

        If Not blnContinue Then Return False 'Nothing to do
        If order.ShipDate < Date.Now.AddDays(1) Then Return False ' TODO add message to Error Logs  invalid ship date

        If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
            'this should never happen unless there is a design bug.
            throwInvalidOperatonException("Shipping information is missing stop data")
            Return False
        End If

        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        iCompControl = order.BookCustCompControl
        'get a default FAK class
        Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)

        Dim sCarrierNumber As String = "0"
        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfig.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Dim sMsg As String = ""
        Dim skipObjs As New List(Of String) From {"Packages", "Pickup", "Stops"}
        Dim ChrOrder As Map.RateRequestOrder = Map.RateRequestOrder.selectMapData(order, sMsg, skipObjs)
        Dim ChrLEConfig As Map.SSOALEConfig = New Map.SSOALEConfig(oLEConfig.SSOALEClientID, oLEConfig.SSOALEClientSecret, oLEConfig.SSOALELoginURL, oLEConfig.SSOALEAuthCode, oLEConfig.SSOALEDataURL)
        Dim sRetMsg As String = ""
        Dim ChrSSOAConfigs As List(Of Map.SSOAConfig) = (From e In lCompConfig Select Map.SSOAConfig.selectMapData(e, sRetMsg)).ToList()
        'the two lines of code need to be replace they are just to get past compile errors
        Dim lSpecialFees As New List(Of Map.RateRequest.SpecialRequirement)
        Dim oAPIResponse = oChrAPI.ProcessRateRequest(ChrOrder, LoadTenderControl, ChrLEConfig, ChrSSOAConfigs, lSpecialFees, strMsg, iDefaultFreightClass)
        If oResponse.success Then
            ' we now insert a quote with error messages even if one is not available 
            ' using the postMessagesOnly flag with a zero cost.  This logic will help
            ' users track issues with API rating
            InsertCHRRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
        End If

        Return oResponse.success

    End Function

    ''' <summary>
    ''' Depreciated do not user logic moved to API Components on 09/22/2023
    ''' </summary>
    ''' <param name="oRateRequest"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oLEConfig"></param>
    ''' <param name="lCompConfig"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    Function GetCHRRates(ByRef oRateRequest As CHR.CHRRateRequest,
                         ByVal LoadTenderControl As Integer,
                         ByVal oLEConfig As LTS.tblSSOALEConfig,
                         ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                         Optional ByRef strMsg As String = "") As Boolean

        throwDepreciatedException("This version of " & buildProcedureName("GetCHRRates") & " has been depreciated. Please use the methods in the API module.")
        Return Nothing

    End Function


    ''' <summary>
    ''' use CreateP44Bid when generating rates from an existing order,
    ''' Use CreateP44Quote when an existing order does not exist, 
    ''' CreateP44Bid expects the caller to create a tblLoadTender Record 
    ''' and pass in the the LoadTenderControl to use
    ''' to create a record in tblBids (and tblBidCostAdj and tblBidSvcErr)
    ''' for each quote returned from P44
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SSOAAct"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rules:
    ''' If P44 q.deliveryDate is nothing then use BookDateRequired
    ''' If P44 q.quoteDate is nothing then use current date
    ''' If P44 q.interLine comes back null or with the word false BidInterline is false
    ''' In all other cases BidInterline is true
    ''' TODO:
    ''' Figure out how to get the linehaul out of the P44 data
    ''' Figure out how to get FuelUOM out of P44 data
    ''' Modified by RHR for v-8.2 on 12/11/2018
    '''   added logic to use the P44AccountGroup and moved logic to 
    '''   read SSOA and create P44 proxy to shared functions that can be called 
    '''   by other procedures
    '''   returns false on error and inserts error message into strMsg
    ''' Modified by RHR for v-8.5.4.002 on 09/22/2023 moved code to CHRAPI
    '''     majority of processing is moved to API classes
    ''' </remarks>
    Public Function CreateCHRBid(ByVal BookControl As Integer,
                                 ByVal LoadTenderControl As Integer,
                                 ByVal SHID As String,
                                 ByVal oLEConfig As LTS.tblSSOALEConfig,
                                 ByVal lCompConfig As List(Of LTS.tblSSOAConfig),
                                 Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.CHRAPI,
                                 Optional ByRef strMsg As String = "") As Boolean
        Dim oLTLs As LTS.vBookRevenue() = DirectCast(Me.NDPBaseClassFactory("NGLBookRevenueData", False), NGLBookRevenueData).GetLTLvBookRevenues(BookControl)
        If oLTLs Is Nothing OrElse oLTLs.Count() < 1 Then Return True 'nothing to do but we did not fail
        Dim origST = oLTLs(0).BookOrigState
        Dim destST = oLTLs(0).BookDestState
        Dim dtNow = Date.Now
        ' Dim oRateRequest As CHR.CHRRateRequest = CopyBookDataToCHRData(oLTLs, lCompConfig)
        Dim oRateRequest As Map.RateRequest = CopyBookDataToAPIData(oLTLs, lCompConfig)

        ' CopyBookDataToCHRData()
        If oRateRequest Is Nothing Then Return True 'nothing to do but we did not fail

        Dim oResponse As New Map.QuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oChrAPI As New CHR.CHRAPI(bUseTLS12)
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        iCompControl = oLTLs(0).BookCustCompControl
        'get a default FAK class
        Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)

        Dim sCarrierNumber As String = "0"
        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "CarrierNumber")) Then sCarrierNumber = lCompConfig.Where(Function(x) x.SSOACName = "CarrierNumber").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Dim ChrLEConfig As Map.SSOALEConfig = New Map.SSOALEConfig(oLEConfig.SSOALEClientID, oLEConfig.SSOALEClientSecret, oLEConfig.SSOALELoginURL, oLEConfig.SSOALEAuthCode, oLEConfig.SSOALEDataURL)
        Dim sRetMsg As String = ""
        Dim ChrSSOAConfigs As List(Of Map.SSOAConfig) = (From e In lCompConfig Select Map.SSOAConfig.selectMapData(e, sRetMsg)).ToList()
        'the line of code below needs to be replace they are just to get past compile errors

        Dim oAPIResponse = oChrAPI.GetBid(oRateRequest, LoadTenderControl, ChrLEConfig, ChrSSOAConfigs, strMsg, iDefaultFreightClass)
        If oResponse.success Then
            ' we now insert a quote with error messages even if one is not available 
            ' using the postMessagesOnly flag with a zero cost.  This logic will help
            ' users track issues with API rating
            InsertCHRRateQuoteBids(LoadTenderControl, oResponse, sCarrierNumber)
        End If

        Return oResponse.success

    End Function

    Protected Function CopyBookDataToCHRData(ByRef oBook() As LTS.vBookRevenue, ByVal lCompConfig As List(Of LTS.tblSSOAConfig), Optional ByVal timeOut As Integer = 20, Optional ByVal sDefFrtClass As String = "70") As CHR.CHRRateRequest
        Dim oRet As New CHR.CHRRateRequest()
        If ((oBook Is Nothing) OrElse oBook.Count() < 1 OrElse (oBook(0).BookControl = 0)) Then Return Nothing

        Dim oResponse As New CHR.CHRQuoteResponse()
        Dim intVersion = GetParValue("APIRateQuoteVersion", 0)
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim oChrAPI As New CHR.CHRAPI(bUseTLS12)
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)


        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0
        Dim iCompControl As Integer = 0
        Dim iDefaultFreightClass As Integer = 100
        Dim dtShip As Date = Date.Now.AddDays(2) 'CHR rates need 2 days 
        Dim dtRequired As Date = dtShip.AddDays(4)
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        Dim oAccs As LTS.vBookAccessorial()

        'Read the default config settings
        Dim sMode As String = "TL"
        Dim sEquipment As String = "Van"
        Dim sLocationID As String = "C377465" 'default for Tree Top\
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000

        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LocationID")) Then sLocationID = lCompConfig.Where(Function(x) x.SSOACName = "LocationID").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        'Note: in v-8.5.1 we only support one booking record
        'For Each book In oBook
        Dim book As LTS.vBookRevenue = oBook(0)

        If Not book Is Nothing AndAlso book.BookControl <> 0 AndAlso Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 Then
            iCompControl = book.BookCustCompControl
            '**************** Begin Validate Mode and weight  ***************************

            Dim blnShipLTL As Boolean = False
            Dim dblTotalWeight As Double = 0
            If book.BookTotalWgt.HasValue Then dblTotalWeight = book.BookTotalWgt.Value
            If dblTotalWeight < dLowLTLWeight Then
                oRet.setPostMessageOnlyFlag(True)
                oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooLowForLTL, " The CHR settings require LTL shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                Return oRet
            End If
            If dblTotalWeight <= dHighLTLWeight Then
                sMode = "LTL"
            Else
                'try truckload
                If dblTotalWeight < dLowTLWeight Then
                    'check if we send the LTL and TL warning  or just the  warning
                    If dHighLTLWeight > 0 Then
                        'we are between LTL and Truckload
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooHighForLTL, " The CHR settings require LTL shipments to have a weight less than " & dHighLTLWeight.ToString() & " or a Truckload weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                        Return oRet
                    Else 'just send the Truckload warning
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooLowForTL, " The CHR settings require Truckload shipments to have a weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                        Return oRet
                    End If
                ElseIf dblTotalWeight > dHighTLWeight Then
                    oRet.setPostMessageOnlyFlag(True)
                    oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooHighForLTL, " The CHR settings require Truckload shipments to have a weight less than " & dHighTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR. ", "", "BookTotalWgt")
                    Return oRet
                End If
            End If

            '****************** End Validate Mode and weight *******************************
            'get a default FAK class
            Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)
            'get the earliest load date
            If book.BookDateLoad.HasValue Then dtShip = book.BookDateLoad.Value
            'get the latest delivery date
            If book.BookDateRequired.HasValue AndAlso book.BookDateRequired.Value > dtRequired Then dtRequired = book.BookDateRequired.Value
            Dim filters As New Models.AllFilters With {.ParentControl = book.BookControl}
            Dim ct As Integer

            ct = 0
            filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
            oAccs = oBookAccDAL.GetBookAccessorials(filters, ct)
            Dim oTMSFees() As LTS.tblAccessorial = NGLtblAccessorialObjData.GetAllAccessorials()
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                ' Modified by RHR for v-8.5.3.005 on 09/22/2022 added new logic to validate accessorial codes with CHR mapping
                Dim sApprovedAcc As New List(Of String)
                Dim oApprovedCHRFees As New List(Of Map.Fees)
                For Each fee As LTS.vBookAccessorial In oAccs
                    Dim sCHRFeeCode As String = ""
                    If Not String.IsNullOrWhiteSpace(fee.NACCode) Then
                        sCHRFeeCode = CHR.CHRAPI.getCHRFeeCodeMapping(fee.NACCode)
                    End If
                    If String.IsNullOrWhiteSpace(sCHRFeeCode) Then
                        Dim sEDICode = oTMSFees.Where(Function(x) x.AccessorialCode = fee.AccessorialCode).Select(Function(y) y.AccessorialEDICode).FirstOrDefault()
                        If Not String.IsNullOrWhiteSpace(sEDICode) Then
                            sCHRFeeCode = CHR.CHRAPI.getCHRFeeCodeMapping(sEDICode)
                        End If
                    End If
                    If Not String.IsNullOrWhiteSpace(sCHRFeeCode) Then
                        sApprovedAcc.Add(sCHRFeeCode)
                        oApprovedCHRFees.Add(New Map.Fees With {.BookAcssControl = fee.BookAcssControl,
                                          .BookAcssNACControl = fee.BookAcssNACControl,
                                          .BookAcssValue = fee.BookAcssValue,
                                                  .NACCode = fee.NACCode,
                                                  .NACName = fee.NACName,
                                                  .AccessorialCode = fee.AccessorialCode,
                                                  .AccessorialName = fee.AccessorialName
                                        })
                    End If
                Next

                oRet.oAccessorials = sApprovedAcc.ToArray()
                oRet.oFees = oApprovedCHRFees.ToArray()
            End If
            'Modified by RHR for v-8.5.3.005 on 09/21/2022 added logic to get the item details if they exists 
            'Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
            'If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
            '    oItems.AddRange(oPkgs)
            'End If
            If book.BookLoads Is Nothing OrElse book.BookLoads.Count() < 1 OrElse book.BookLoads(0).BookItems Is Nothing OrElse book.BookLoads(0).BookItems.Count() < 1 Then
                Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    oItems.AddRange(oPkgs)
                End If
            Else
                For Each bl In book.BookLoads
                    If Not bl Is Nothing AndAlso Not bl.BookItems Is Nothing AndAlso bl.BookItems.Count() > 0 Then
                        For Each bi As LTS.BookItem In bl.BookItems
                            Dim oPkg As New LTS.vBookPackage()
                            With oPkg
                                .BookPkgDescription = bi.BookItemDescription
                                .BookPkgFAKClass = bi.BookItemFAKClass
                                .BookPkgWeight = If(bi.BookItemWeight, 10)
                                .BookPkgLength = bi.BookItemQtyLength
                                .BookPkgWidth = bi.BookItemQtyWidth
                                .BookPkgHeight = bi.BookItemQtyHeight
                                .BookPkgCount = CInt(bi.BookItemPallets)
                                .PackageType = "PLT"
                                .BookPkgStackable = bi.BookItemStackable
                            End With
                            oItems.Add(oPkg)
                        Next
                    End If
                Next
            End If
        End If
        'Next
        If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

        Dim oOrigSpecialReq As CHR.CHRSpecialRequirement = New CHR.CHRSpecialRequirement()
        Dim oOrigRefs As New List(Of CHR.CHRReferenceNumbers)  '[] = new CHRReferenceNumbers[1];


        With oRet
            .customerCode = sLocationID
            .setMode(sEquipment, 1, sMode)
            .lStops = New List(Of CHR.CHRAddress)
            .lItems = New List(Of CHR.CHRItem)
            .setShipDate(dtShip.ToShortDateString)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                For Each a As LTS.vBookAccessorial In oAccs
                    If a.AccessorialCode = 48 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.liftGate)
                    If a.AccessorialCode = 17 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.insideDelivery)
                    If a.AccessorialCode = 122 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.insidePickup)
                    If a.AccessorialCode = 68 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.residentialNonCommercial)
                    If a.AccessorialCode = 132 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.residentialNonCommercial)
                    If a.AccessorialCode = 72 Then oOrigSpecialReq.setSpecialRequrement(CHR.CHRAPI.SpecialRequirement.constructionSite)
                Next
            End If
            Dim sCNS As String = If(String.IsNullOrWhiteSpace(oBook(0).BookConsPrefix), oBook(0).BookProNumber, oBook(0).BookConsPrefix)
            Dim sSHID As String = If(String.IsNullOrWhiteSpace(oBook(0).BookSHID), sCNS, oBook(0).BookSHID)
            Dim sDel As String = If(String.IsNullOrWhiteSpace(oBook(0).BookLoads(0).BookLoadPONumber), sCNS, oBook(0).BookLoads(0).BookLoadPONumber)
            CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.PU, sCNS, oOrigRefs)
            CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.SHID, sSHID, oOrigRefs)
            CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.DEL, sDel, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookCarrOrderNumber)) Then CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.CON, oBook(0).BookCarrOrderNumber, oOrigRefs)
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookProNumber)) Then CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.CRID, oBook(0).BookProNumber, oOrigRefs)


            .oRefs = oOrigRefs.ToArray()
            .oOrigin = New CHR.CHRAddress() With {
            .locationName = oBook(0).BookOrigName,
            .address1 = oBook(0).BookOrigAddress1,
            .city = oBook(0).BookOrigCity,
            .stateProvinceCode = oBook(0).BookOrigState,
            .countryCode = "US",
            .postalCode = oBook(0).BookOrigZip,
            .specialRequirement = oOrigSpecialReq,
            .customerLocationId = oBook(0).BookOrigName,
            .referenceNumbers = oOrigRefs.ToArray()
        }

            Dim CHRStop = New CHR.CHRAddress() With {
            .locationName = oBook(0).BookDestName,
            .address1 = oBook(0).BookDestAddress1,
            .city = oBook(0).BookDestCity,
            .stateProvinceCode = oBook(0).BookDestState,
            .countryCode = "US",
            .postalCode = oBook(0).BookDestZip,
            .specialRequirement = oOrigSpecialReq,
            .customerLocationId = oBook(0).BookDestName,
            .referenceNumbers = oOrigRefs.ToArray()
        }
            .lStops.Add(CHRStop)
            Dim iTmp As Integer = 0
            Dim ideclaredValueTotal As Integer = 0
            For Each ITM In oItems
                Dim iactualWeight = If(CInt(ITM.BookPkgWeight) > 0, CInt(ITM.BookPkgWeight), 100)
                Console.WriteLine(iactualWeight)
            Next
            Dim lineItems As List(Of CHR.CHRItem) = (From i In oItems Select New CHR.CHRItem() With {
                .description = If(String.IsNullOrWhiteSpace(i.BookPkgDescription), "misc products", i.BookPkgDescription),
                .freightClass = If(Integer.TryParse(i.BookPkgFAKClass, iTmp), iTmp, 100),
                .actualWeight = If(CInt(i.BookPkgWeight) > 0, CInt(i.BookPkgWeight), 100),
                .weightUnit = "Pounds",
                .length = i.BookPkgLength,
                .width = i.BookPkgWidth,
                .height = i.BookPkgHeight,
                .pallets = i.BookPkgCount,
                .pieces = i.BookPkgCount,
                .palletSpaces = i.BookPkgCount,
                .packagingCode = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                .productName = "goods",
                .declaredValue = 1000,
                .isStackable = If(String.IsNullOrWhiteSpace(i.BookPkgStackable), "false", i.BookPkgStackable.ToString().ToLower()),
                .referenceNumbers = oOrigRefs.ToArray()
            }).ToList()
            .lItems = lineItems
            .declaredValue = lineItems.Sum(Function(x) x.declaredValue)
        End With

        Return oRet
    End Function

    ''' <summary>
    ''' Copies Book table data to the NGL API RateRequest object
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="lCompConfig"></param>
    ''' <param name="iDefaultFreightClass"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.002 on 09/28/2023 
    '''     This version uses one booking and does not support multi stops    
    ''' </remarks>
    Protected Function CopyBookDataToAPIData(ByRef oBook() As LTS.vBookRevenue, ByVal lCompConfig As List(Of LTS.tblSSOAConfig), Optional ByVal iDefaultFreightClass As Integer = 100) As Map.RateRequest
        Dim oRet As New Map.RateRequest()
        If ((oBook Is Nothing) OrElse oBook.Count() < 1 OrElse (oBook(0).BookControl = 0)) Then Return Nothing

        Dim oResponse As New Map.QuoteResponse()
        Dim oLT As New NGLLoadTenderData(Parameters)
        Dim oBid As New NGLBidData(Parameters)
        Dim lRevs As New List(Of DTO.BookRevenue)


        'get the accessorials and compcontrols
        Dim lAccessorial As New List(Of DTO.BookFee)
        Dim origCompControl As Integer = 0
        Dim destCompControl As Integer = 0
        Dim iCompControl As Integer = 0
        Dim dtShip As Date = Date.Now.AddDays(2) 'CHR rates need 2 days 
        Dim dtRequired As Date = dtShip.AddDays(4)
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        Dim oAccs As LTS.vBookAccessorial()

        'Read the default config settings
        Dim sMode As String = "TL"
        Dim sEquipment As String = "Van"
        Dim sLocationID As String = "C377465" 'default for Tree Top\
        Dim sLowLTLWeight As String = "500"
        Dim sHighLTLWeight As String = "5000"
        Dim sLowTLWeight As String = "5000"
        Dim sHighTLWeight As String = "45000"
        Dim dLowLTLWeight As Double = 500
        Dim dHighLTLWeight As Double = 5000
        Dim dLowTLWeight As Double = 5000
        Dim dHighTLWeight As Double = 45000

        If Not lCompConfig Is Nothing AndAlso lCompConfig.Count() > 0 Then
            If (lCompConfig.Any(Function(x) x.SSOACName = "Mode")) Then sMode = lCompConfig.Where(Function(x) x.SSOACName = "Mode").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "Equipment")) Then sEquipment = lCompConfig.Where(Function(x) x.SSOACName = "Equipment").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LocationID")) Then sLocationID = lCompConfig.Where(Function(x) x.SSOACName = "LocationID").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowLTLWeight")) Then sLowLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighLTLWeight")) Then sHighLTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighLTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "LowTLWeight")) Then sLowTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "LowTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
            If (lCompConfig.Any(Function(x) x.SSOACName = "HighTLWeight")) Then sHighTLWeight = lCompConfig.Where(Function(x) x.SSOACName = "HighTLWeight").Select(Function(d) d.SSOACValue).FirstOrDefault()
        End If
        Double.TryParse(sLowLTLWeight, dLowLTLWeight)
        Double.TryParse(sHighLTLWeight, dHighLTLWeight)
        Double.TryParse(sLowTLWeight, dLowTLWeight)
        Double.TryParse(sHighTLWeight, dHighTLWeight)

        'Note: in v-8.5.1 we only support one booking record
        'For Each book In oBook
        Dim book As LTS.vBookRevenue = oBook(0)

        If Not book Is Nothing AndAlso book.BookControl <> 0 AndAlso Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 Then
            iCompControl = book.BookCustCompControl
            '**************** Begin Validate Mode and weight  ***************************

            Dim blnShipLTL As Boolean = False
            Dim dblTotalWeight As Double = 0
            If book.BookTotalWgt.HasValue Then dblTotalWeight = book.BookTotalWgt.Value
            If dblTotalWeight < dLowLTLWeight Then
                oRet.setPostMessageOnlyFlag(True)
                oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooLowForLTL, " The CHR settings require LTL shipments to have a weight of at least " & dLowLTLWeight.ToString() & ".  The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                Return oRet
            End If
            If dblTotalWeight <= dHighLTLWeight Then
                sMode = "LTL"
            Else
                'try truckload
                If dblTotalWeight < dLowTLWeight Then
                    'check if we send the LTL and TL warning  or just the  warning
                    If dHighLTLWeight > 0 Then
                        'we are between LTL and Truckload
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooHighForLTL, " The CHR settings require LTL shipments to have a weight less than " & dHighLTLWeight.ToString() & " or a Truckload weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                        Return oRet
                    Else 'just send the Truckload warning
                        oRet.setPostMessageOnlyFlag(True)
                        oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooLowForTL, " The CHR settings require Truckload shipments to have a weight of at least " & dLowTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR.", "", "BookTotalWgt")
                        Return oRet
                    End If
                ElseIf dblTotalWeight > dHighTLWeight Then
                    oRet.setPostMessageOnlyFlag(True)
                    oRet.AddMessage(Map.APIMessage.MessageEnum.E_WeightTooHighForLTL, " The CHR settings require Truckload shipments to have a weight less than " & dHighTLWeight.ToString() & ". The current weight, " & dblTotalWeight.ToString() & ", is not valid for CHR. ", "", "BookTotalWgt")
                    Return oRet
                End If
            End If

            '****************** End Validate Mode and weight *******************************
            'get a default FAK class
            Integer.TryParse(GetParText("UseFAKDefault", iCompControl), iDefaultFreightClass)
            'get the earliest load date
            If book.BookDateLoad.HasValue Then dtShip = book.BookDateLoad.Value
            'get the latest delivery date
            If book.BookDateRequired.HasValue AndAlso book.BookDateRequired.Value > dtRequired Then dtRequired = book.BookDateRequired.Value
            Dim filters As New Models.AllFilters With {.ParentControl = book.BookControl}
            Dim ct As Integer

            ct = 0
            filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
            oAccs = oBookAccDAL.GetBookAccessorials(filters, ct)
            Dim oTMSFees() As LTS.tblAccessorial = NGLtblAccessorialObjData.GetAllAccessorials()
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                ' Modified by RHR for v-8.5.3.005 on 09/22/2022 added new logic to validate accessorial codes with CHR mapping
                Dim sApprovedAcc As New List(Of String)
                Dim oApprovedCHRFees As New List(Of Map.Fees)
                For Each fee As LTS.vBookAccessorial In oAccs
                    Dim sCHRFeeCode As String = ""
                    If Not String.IsNullOrWhiteSpace(fee.NACCode) Then
                        sCHRFeeCode = CHR.CHRAPI.getCHRFeeCodeMapping(fee.NACCode)
                    End If
                    If String.IsNullOrWhiteSpace(sCHRFeeCode) Then
                        Dim sEDICode = oTMSFees.Where(Function(x) x.AccessorialCode = fee.AccessorialCode).Select(Function(y) y.AccessorialEDICode).FirstOrDefault()
                        If Not String.IsNullOrWhiteSpace(sEDICode) Then
                            sCHRFeeCode = CHR.CHRAPI.getCHRFeeCodeMapping(sEDICode)
                        End If
                    End If
                    If Not String.IsNullOrWhiteSpace(sCHRFeeCode) Then
                        sApprovedAcc.Add(sCHRFeeCode)
                        oApprovedCHRFees.Add(New Map.Fees With {.BookAcssControl = fee.BookAcssControl,
                                          .BookAcssNACControl = fee.BookAcssNACControl,
                                          .BookAcssValue = fee.BookAcssValue,
                                                  .NACCode = fee.NACCode,
                                                  .NACName = fee.NACName,
                                                  .AccessorialCode = fee.AccessorialCode,
                                                  .AccessorialName = fee.AccessorialName
                                        })
                    End If
                Next

                oRet.oAccessorials = sApprovedAcc.ToList()
                oRet.oFees = oApprovedCHRFees.ToList()
            End If
            'Modified by RHR for v-8.5.3.005 on 09/21/2022 added logic to get the item details if they exists 
            'Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
            'If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
            '    oItems.AddRange(oPkgs)
            'End If
            If book.BookLoads Is Nothing OrElse book.BookLoads.Count() < 1 OrElse book.BookLoads(0).BookItems Is Nothing OrElse book.BookLoads(0).BookItems.Count() < 1 Then
                Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    oItems.AddRange(oPkgs)
                End If
            Else
                For Each bl In book.BookLoads
                    If Not bl Is Nothing AndAlso Not bl.BookItems Is Nothing AndAlso bl.BookItems.Count() > 0 Then
                        For Each bi As LTS.BookItem In bl.BookItems
                            Dim oPkg As New LTS.vBookPackage()
                            With oPkg
                                .BookPkgDescription = bi.BookItemDescription
                                .BookPkgFAKClass = bi.BookItemFAKClass
                                .BookPkgWeight = If(bi.BookItemWeight, 10)
                                .BookPkgLength = bi.BookItemQtyLength
                                .BookPkgWidth = bi.BookItemQtyWidth
                                .BookPkgHeight = bi.BookItemQtyHeight
                                .BookPkgCount = CInt(bi.BookItemPallets)
                                .PackageType = "PLT"
                                .BookPkgStackable = bi.BookItemStackable
                            End With
                            oItems.Add(oPkg)
                        Next
                    End If
                Next
            End If
        End If
        'Next
        If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

        With oRet
            .customerCode = sLocationID
            .lStops = New List(Of Map.AddressBook)
            .lItems = New List(Of Map.RateRequestItem)
            .setShipDate(dtShip.ToShortDateString)
            If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                For Each a As LTS.vBookAccessorial In oAccs
                    .addSpecialRequirements(a.AccessorialCode)
                Next
            End If
            Dim sCNS As String = If(String.IsNullOrWhiteSpace(oBook(0).BookConsPrefix), oBook(0).BookProNumber, oBook(0).BookConsPrefix)
            Dim sSHID As String = If(String.IsNullOrWhiteSpace(oBook(0).BookSHID), sCNS, oBook(0).BookSHID)
            Dim sDel As String = If(String.IsNullOrWhiteSpace(oBook(0).BookLoads(0).BookLoadPONumber), sCNS, oBook(0).BookLoads(0).BookLoadPONumber)

            Dim lReferencesNumbers As New List(Of Map.ReferenceNumber)
            lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.SHID, sSHID))
            lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.CNS, sCNS))
            lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.PU, sCNS))
            lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.DEL, sDel))

            If (Not String.IsNullOrWhiteSpace(oBook(0).BookCarrOrderNumber)) Then
                lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.ORD, oBook(0).BookCarrOrderNumber))
            End If
            If (Not String.IsNullOrWhiteSpace(oBook(0).BookProNumber)) Then
                lReferencesNumbers.Add(New Map.ReferenceNumber(Map.ReferenceNumber.RefNumber.TMSPRO, oBook(0).BookProNumber))
            End If
            .lReferencesNumbers = lReferencesNumbers
            .oOrigin = New Map.AddressBook() With {
            .Name = oBook(0).BookOrigName,
            .Address1 = oBook(0).BookOrigAddress1,
            .City = oBook(0).BookOrigCity,
            .State = oBook(0).BookOrigState,
            .Country = "US",
            .Zip = oBook(0).BookOrigZip,
            .LocationCode = oBook(0).BookOrigName,
            .lSpecialReqs = oRet.lSpecialReqs,
            .lReferencesNumbers = lReferencesNumbers
            }

            Dim oStop = New Map.AddressBook() With {
            .Name = oBook(0).BookDestName,
            .Address1 = oBook(0).BookDestAddress1,
            .City = oBook(0).BookDestCity,
            .State = oBook(0).BookDestState,
            .Country = "US",
            .Zip = oBook(0).BookDestZip,
            .LocationCode = oBook(0).BookOrigName,
            .lSpecialReqs = oRet.lSpecialReqs,
            .lReferencesNumbers = lReferencesNumbers
            }

            .lStops.Add(oStop)
            Dim iTmp As Integer = 0
            Dim ideclaredValueTotal As Integer = 0
            For Each ITM In oItems
                Dim iactualWeight = If(CInt(ITM.BookPkgWeight) > 0, CInt(ITM.BookPkgWeight), 100)
                Console.WriteLine(iactualWeight)
            Next
            Dim lineItems As List(Of Map.RateRequestItem) = (From i In oItems Select New Map.RateRequestItem() With {
                .Description = If(String.IsNullOrWhiteSpace(i.BookPkgDescription), "misc products", i.BookPkgDescription),
                .FreightClass = If(Integer.TryParse(i.BookPkgFAKClass, iTmp), iTmp, 100),
                .Weight = If(CInt(i.BookPkgWeight) > 0, CInt(i.BookPkgWeight), 100),
                .WeightUnit = "Pounds",
                .Length = i.BookPkgLength,
                .Width = i.BookPkgWidth,
                .Height = i.BookPkgHeight,
                .PalletCount = i.BookPkgCount,
                .Pieces = i.BookPkgCount,
                .PackageType = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                .ItemNumber = "goods",
                .ItemCost = 1000,
                .Stackable = i.BookPkgStackable
            }).ToList()

            .lItems = lineItems
            .declaredValue = lineItems.Sum(Function(x) x.ItemCost)
        End With

        Return oRet
    End Function

    ''' <summary>
    ''' Add the quotes to the bid table
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 07/07/2023 
    '''     added new logic to assign the Cost Adjustment Type 
    '''         added new logic to calculate the customer upcharge precent
    ''' </remarks>
    Public Function InsertCHRRateQuoteBids(ByVal LoadTenderControl As Integer, ByVal oResponse As Map.QuoteResponse, ByVal sCarrierNumber As String, Optional sFreightClass As String = "Truck") As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)

            Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = LoadTenderControl).FirstOrDefault()
            Dim sRetMSg As String = ""
            'If Not CreateNGLAPIBid(oResponse, oLoadTender.LoadTenderControl, oLoadTender.LTBookSHID, oLoadTender.LTBookOrigState, oLoadTender.LTBookDestState, 0, sRetMSg, 0, oLoadTender.LTBookDateRequired, BSCEnum.Quoted) Then
            '    throwUnExpectedFaultException(sRetMSg)
            'End If
            'Begin 

            Dim SHID As String = oLoadTender.LTBookSHID
            Dim origST As String = oLoadTender.LTBookOrigState
            Dim destST As String = oLoadTender.LTBookDestState
            Dim intCompControl As Integer = 0
            Dim strMsg As String = ""
            Dim BookControl As Integer = 0
            Dim DefaultRequiredDate As Date? = oLoadTender.LTBookDateRequired
            Dim bStatusCode As BSCEnum = BSCEnum.Active



            If LoadTenderControl = 0 Then
                Dim lDetails As New List(Of String) From {"Load Tendered Reference", " cannot be found and "}
                throwInvalidKeyParentRequiredException(lDetails)
                Return False
            End If

            Dim sSCAC = oLoadTender.LTCarrierSCAC
            Dim iCarrierControl As Integer = 0
            Dim sLTCarrierName = "CHR"
            Dim sCNS = oLoadTender.LTBookConsPrefix
            Dim sOrderNbr = oLoadTender.LTBookCarrOrderNumber
            Dim sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

            Dim dtNow As Date = Date.Now
            Dim dtDefaultRequiredDate As Date = If(DefaultRequiredDate, dtNow.AddDays(3)) ' TODO: add transit time
            Dim blnInvalidData As Boolean = False

            If oResponse Is Nothing OrElse oResponse.quoteSummaries Is Nothing OrElse oResponse.quoteSummaries.Count() < 1 Then
                If oResponse Is Nothing Then oResponse = New Map.QuoteResponse()
                oResponse.AddMessage(Map.APIMessage.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim iCarrierNumber As Integer = 0
            If Not Integer.TryParse(sCarrierNumber, iCarrierNumber) Then
                oResponse.AddMessage(Map.APIMessage.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            End If
            Dim oCarrier = db.CarrierRefIntegrations.Where(Function(x) x.CarrierNumber = iCarrierNumber).FirstOrDefault()
            If oCarrier Is Nothing OrElse oCarrier.CarrierControl = 0 Then
                oResponse.AddMessage(Map.APIMessage.MessageEnum.E_InvalidCarrierNumber, sLoadDetails, "", "")
                blnInvalidData = True
            Else
                sSCAC = oCarrier.CarrierSCAC
                sLTCarrierName = oCarrier.CarrierName
                sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)
                iCarrierControl = oCarrier.CarrierControl
            End If

            Try
                Dim Sec As New NGLSecurityDataProvider(Parameters)
                If intCompControl = 0 Then

                    intCompControl = Sec.getLECompControl()
                End If
                Dim lCarrier As List(Of Integer) = Sec.RestrictedCarriersForSalesReps()
                If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(iCarrierControl))) Then
                    oResponse.AddMessage(Map.APIMessage.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                    blnInvalidData = True
                    Return False 'note we need to test this logic
                End If
                If Not blnInvalidData And Not oResponse.postMessagesOnly Then
                    For Each q As Map.QuoteSummary In oResponse.quoteSummaries
                        sSCAC = oCarrier.CarrierSCAC
                        sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", oCarrier.CarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

                        Dim oCarrierData As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
                        If Not String.IsNullOrEmpty(sSCAC) Then
                            'get the carrier data
                            oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, sSCAC).FirstOrDefault()
                        End If
                        If oCarrierData Is Nothing OrElse oCarrierData.CarrierControl = 0 Then
                            oResponse.AddMessage(Map.APIMessage.MessageEnum.E_NoRatesFound, sLoadDetails, "", "")
                            blnInvalidData = True
                            Continue For 'We still want to try to create any other bids -- the strMsg results should be logged by the caller
                        End If

                        Dim fuelTotal As Decimal = 0
                        Dim fuelVar As Decimal = 0
                        Dim dLIneHaul As Decimal = 0
                        Dim Adjs As New List(Of LTS.tblBidCostAdj)
                        Dim iBidAdjustmentCount As Integer = 0
                        Dim sTMSFeeAccessorialEDICode = ""
                        ' Begin Modified by RHR for v-8.5.4.001 on 07/07/2023
                        '   added new logic to assign the Cost Adjustment Type
                        '   added new logic to calculate the customer upcharge precent
                        Dim eCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial
                        Dim dblCarrierCostUpcharge As Decimal = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(iCarrierControl, intCompControl)
                        Dim decBidCustLineHaul As Decimal = 0
                        If Not q Is Nothing AndAlso Not q.rates Is Nothing AndAlso q.rates.Count() > 0 Then
                            iBidAdjustmentCount = q.rates.Count()
                            For Each a As Map.Rate In q.rates
                                ' CHR Rate Code Values map to TMS EDI codes as defined below in the Select Case
                                Dim sCodeValue As String = a.rateCodeValue
                                Dim sAdjDesc As String = ""
                                eCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial
                                Select Case a.rateCode
                                    Case "400" '- Line Haul
                                        sAdjDesc = "Carrier Line Haul"
                                        dLIneHaul = dLIneHaul + a.totalRate
                                        eCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierLineHaul
                                    Case "405" 'Fuel Surchage
                                        sAdjDesc = "Fuel Surcharge"
                                        fuelTotal = fuelTotal + a.totalRate
                                        eCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Fuel
                                    Case "260"
                                        sAdjDesc = "Delivery Surcharge"
                                        sCodeValue = "MSC"
                                    Case "IDL" ' Inside Delivery
                                        sAdjDesc = "Inside Delivery"
                                    Case "IPU" 'Inside Pickup 
                                        sAdjDesc = "Inside Pickup"
                                    Case "LFT" 'Lift Gate Or Forklift Service
                                        sAdjDesc = "Lift Gate Or Forklift Service"
                                    Case "DET" 'Detention
                                        sAdjDesc = "Detention"
                                        sCodeValue = "DTU"
                                    Case "DLA" '- Delivery Limited Access
                                        sAdjDesc = "Delivery Limited Access"
                                        sCodeValue = "EXD"
                                    Case "PLA" 'Pickup Limited Access 
                                        sAdjDesc = "Pickup Limited Access"
                                        sCodeValue = "HCP"
                                    Case "Res" 'Residential Delivery Fee
                                        sAdjDesc = "Residential Delivery Fee"
                                    Case "REP" 'Residential Pickup Fee
                                        sAdjDesc = "Residential Pickup Fee"
                                    Case "CSD" 'Construction Site Delivery
                                        sAdjDesc = "Construction Site Delivery"
                                        sCodeValue = "ACH"
                                    Case Else
                                        sAdjDesc = "Misc"
                                        sCodeValue = "MSC"
                                End Select

                                If a.isOptional = True Then
                                    oResponse.AddMessage(Map.APIMessage.MessageEnum.E_OptionalCharge, sCodeValue & " " & a.totalRate.ToString("c"), "NA", sAdjDesc)
                                Else

                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                               .BidCostAdjWeight = a.quantity,
                                                                               .BidCostAdjAmount = a.totalRate,
                                                                               .BidCostAdjRate = a.unitRate,
                                                                               .BidCostAdjDescCode = sCodeValue,
                                                                               .BidCostAdjTypeControl = eCostAdjTypeControl,
                                                                               .BidCostAdjDesc = sAdjDesc,
                                                                               .BidCostAdjModDate = dtNow,
                                                                               .BidCostAdjModUser = Me.Parameters.UserName})
                                    'Most Common
                                    '400 - Line Haul, 405 - Fuel Surchage, 260 - Deliver Surchage, IDL - Inside Delivery, IPU - Inside Pickup LFT - Lift Gate Or Forklift Service, DET - Detention, DLA - Delivery Limited Access, PLA - Pickup Limited Access RES - Residential Delivery Fee, REP - Residential Pickup Fee, CSD - Construction Site Delivery here.
                                End If

                            Next
                            decBidCustLineHaul = dLIneHaul + (dLIneHaul * dblCarrierCostUpcharge)
                            Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                               .BidCostAdjWeight = oLoadTender.LTBookTotalWgt,
                                                                               .BidCostAdjAmount = decBidCustLineHaul,
                                                                               .BidCostAdjRate = 1,
                                                                               .BidCostAdjDescCode = "UPLF",
                                                                               .BidCostAdjDesc = "Customer Line Haul Charges",
                                                                               .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                                                                               .BidCostAdjModDate = dtNow,
                                                                               .BidCostAdjModUser = Me.Parameters.UserName})

                        End If
                        Dim strErrs = "" ' sb.ToString()
                        Dim strInfos = "" 'sb.ToString()
                        Dim strWarnings = ""
                        Dim blnInterline As Boolean = False
                        Dim bArchived = 0
                        'TODO: add logic for minimim transit days and max and Min delivery date
                        Dim dtLoad As Date = If(oLoadTender.LTBookDateLoad, Date.Now.AddDays(1))
                        Dim dtBidDeliveryDate As Date? = If(oLoadTender.LTBookDateRequired, Date.Now.AddDays(5))
                        Dim dblTransitDays As Integer = 5
                        If Not q.transit Is Nothing AndAlso q.transit.maximumTransitDays > 0 Then
                            dblTransitDays = q.transit.maximumTransitDays
                        End If
                        dtBidDeliveryDate = dtLoad.AddDays(dblTransitDays)
                        Dim dtBidQuoteDate As Date? = dtNow
                        Dim dtBidExpirationDate As Date = dtNow
                        Dim decBidCustTotalCost As Decimal = decBidCustLineHaul + (q.totalCharge - dLIneHaul)
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                            .BidCarrierControl = oCarrierData.CarrierControl,
                                                            .BidCarrierNumber = If(oCarrierData.CarrierNumber, 0),
                                                            .BidCarrierName = Left(oCarrierData.CarrierName, 40),
                                                            .BidCarrierSCAC = Left(sSCAC, 4),
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = q.totalCharge,
                                                            .BidLineHaul = dLIneHaul,
                                                            .BidFuelTotal = fuelTotal,
                                                            .BidFuelVariable = fuelVar,
                                                            .BidFuelUOM = "Flat Rate",
                                                            .BidOrigState = Left(origST, 2),
                                                            .BidDestState = Left(destST, 2),
                                                            .BidPosted = dtNow,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = bArchived,
                                                            .BidMode = q.transportModeType,
                                                            .BidErrorCount = 0,
                                                            .BidErrors = Left(strErrs, 3999),
                                                            .BidWarnings = Left(strWarnings, 3999),
                                                            .BidInfos = Left(strInfos, 3999),
                                                            .BidInterLine = blnInterline,
                                                            .BidQuoteNumber = Left(q.quoteId, 100),
                                                            .BidTransitTime = dblTransitDays,
                                                            .BidDeliveryDate = dtBidDeliveryDate,
                                                            .BidQuoteDate = dtBidQuoteDate,
                                                            .BidTotalWeight = oLoadTender.LTBookTotalWgt,
                                                            .BidDetailTotal = 0,
                                                            .BidDetailTransitTime = 0,
                                                            .BidAdjustments = q.totalCharge - dLIneHaul, 'difference between line haul and total cost 
                                                            .BidAdjustmentCount = iBidAdjustmentCount,
                                                            .BidVendor = Left(sSCAC, 20),
                                                            .BidContractID = Left(q.quoteId, 50),
                                                            .BidServiceType = Left(q.transportModeType, 50),
                                                            .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                            .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                            .BidComments = "",
                                                            .BidExpires = dtBidExpirationDate,
                                                            .BidCustLineHaul = decBidCustLineHaul,
                                                            .BidCustTotalCost = decBidCustTotalCost,
                                                            .BidModDate = dtNow,
                                                            .BidModUser = Me.Parameters.UserName}
                        ' End Modified by RHR for v-8.5.4.001 on 07/07/2023

                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        Dim oT = db.tblBidCostAdjs
                        For Each adj In Adjs
                            adj.BidCostAdjBidControl = bidCtrl
                            oT.InsertOnSubmit(adj)
                        Next

                        Dim lMessages As List(Of Map.APIMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            'Reset blnInvalidData flag to false because we have already logged the messages
                            blnInvalidData = False
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                    .BidSvcErrBidControl = bidCtrl,
                                    .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                    .BidSvcErrMessage = msg.Details,
                                    .BidSvcErrFieldName = msg.FieldName,
                                    .BidSvcErrModDate = dtNow,
                                    .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    Next
                End If

                If blnInvalidData Or oResponse.postMessagesOnly Then
                    Dim lMessages As List(Of Map.APIMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                    If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                        Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                                .BidCarrierControl = iCarrierControl,
                                                                .BidCarrierNumber = iCarrierNumber,
                                                                .BidCarrierName = Left(sLTCarrierName, 40),
                                                                .BidCarrierSCAC = Left(sSCAC, 4),
                                                                .BidSHID = SHID,
                                                                .BidTotalCost = 0,
                                                                .BidLineHaul = 0,
                                                                .BidFuelTotal = 0,
                                                                .BidFuelVariable = 0,
                                                                .BidFuelUOM = "NA",
                                                                .BidOrigState = Left(origST, 2),
                                                                .BidDestState = Left(destST, 2),
                                                                .BidPosted = dtNow,
                                                                .BidStatusCode = bStatusCode,
                                                                .BidArchived = False,
                                                                .BidMode = "NA",
                                                                .BidErrorCount = lMessages.Count(),
                                                                .BidErrors = Left(oResponse.concateMessages, 3999),
                                                                .BidVendor = Left(sSCAC, 20),
                                                                .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                                .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                                .BidCustLineHaul = 0,
                                                                .BidCustTotalCost = 0,
                                                                .BidModDate = dtNow,
                                                                .BidModUser = Me.Parameters.UserName}

                        Dim oTable = db.tblBids
                        oTable.InsertOnSubmit(oBid)
                        db.SubmitChanges()
                        Dim bidCtrl = oBid.BidControl

                        If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                            Dim oTbl = db.tblBidSvcErrs
                            For Each msg In lMessages
                                msg.bLogged = True
                                'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                        .BidSvcErrBidControl = bidCtrl,
                                        .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                        .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                        .BidSvcErrFieldName = msg.FieldName,
                                        .BidSvcErrModDate = dtNow,
                                        .BidSvcErrModUser = Me.Parameters.UserName}
                                oTbl.InsertOnSubmit(oBidErr)
                            Next
                        End If

                        db.SubmitChanges()
                    End If

                End If

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertCHRRateQuoteBids"))
            End Try
            Return False
        End Using
    End Function


#End Region



    ''' <summary>
    ''' Used for LTL shipping where multiple stops are not support.
    ''' Please use the overloaded version that processes items by stops 
    ''' If building a non-LTL Load
    ''' This method is for backward compatibility only.
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="oItems"></param>
    Public Sub fillRateRequestItems(ByVal order As Models.RateRequestOrder, ByRef oItems As List(Of Models.RateRequestItem))
        If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
            Return
        End If

        If oItems Is Nothing Then
            oItems = New List(Of Models.RateRequestItem)()
        End If

        For Each oStop As Models.RateRequestStop In order.Stops

            If oStop.Items IsNot Nothing AndAlso oStop.Items.Count() > 0 Then
                For Each itm In oStop.Items
                    If itm.NumPieces < 1 Then itm.Quantity = "1"
                Next
                oItems.AddRange(oStop.Items)
            End If
        Next

        If oItems Is Nothing OrElse oItems.Count() < 1 OrElse oItems(0) Is Nothing Then
            oItems = New List(Of Models.RateRequestItem)()
            Dim item As Models.RateRequestItem = New Models.RateRequestItem() With {
                    .FreightClass = "100",
                    .ItemNumber = "NA",
                    .Weight = 500,
                    .PalletCount = 1,
                    .Quantity = "1",
                    .Pieces = "1",
                    .NumPieces = 1
                }
            oItems.Add(item)
        End If
    End Sub

    ''' <summary>
    ''' fills the item details for each stop uses defaults if no items found 
    ''' (FreightClass = 100; Weight = 500; PalletCount = 1; Quantity = 1)
    ''' </summary>
    ''' <param name="oStop"></param>
    ''' <param name="oItems"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 1/30/2019 to support multi-Stop Rate shopping
    ''' </remarks>
    Public Sub fillRateRequestItems(ByVal oStop As Models.RateRequestStop, ByRef oItems As List(Of Models.RateRequestItem))


        If oItems Is Nothing Then
            oItems = New List(Of Models.RateRequestItem)()
        End If

        If Not oStop Is Nothing AndAlso oStop.Items IsNot Nothing AndAlso oStop.Items.Count() > 0 Then
            oItems.AddRange(oStop.Items)
        End If


        If oItems Is Nothing OrElse oItems.Count() < 1 OrElse oItems(0) Is Nothing Then
            oItems = New List(Of Models.RateRequestItem)()
            Dim item As Models.RateRequestItem = New Models.RateRequestItem() With {
                    .FreightClass = "100",
                    .ItemNumber = "NA",
                    .Weight = 500,
                    .PalletCount = 1,
                    .Quantity = "1"
                }
            oItems.Add(item)
        End If
    End Sub

    Public Function readSSOASettings(ByRef P44WebServiceUrl As String,
                                         ByRef P44WebServiceLogin As String,
                                         ByRef P44WebServicePassword As String,
                                         ByRef P44AccountGroup As String,
                                         Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.P44,
                                         Optional ByRef strMsg As String = "") As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = True
        Dim oSSOA As LTS.vSSOA = DirectCast(Me.NDPBaseClassFactory("NGLtblSingleSignOnAccountData", False), NGLtblSingleSignOnAccountData).GetSSOADataForCurrentUser(SSOAAct)
        If oSSOA Is Nothing OrElse oSSOA.SSOAControl = 0 Then
            oRet.Success = False
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_NotFoundCarrierAPISSOAByUser, {Me.Parameters.UserName})
            strMsg += oRet.getErrorsAsSingleStr(" ", False)
            Return oRet
        End If
        P44WebServiceUrl = oSSOA.SSOALoginURL ' example: "http: //test.p-44.com"
        P44WebServiceLogin = oSSOA.SSOASecurityXrefUserName ' example:"rramsey@nextgeneration.com"
        P44WebServicePassword = DTran.Decrypt(oSSOA.SSOASecurityXrefPassword, "NGL") ' example: "NGL2016!"
        P44AccountGroup = oSSOA.SSOASecurityXrefReferenceID
        Return oRet
    End Function

    Public Function createP44Proxy(ByRef oP44Proxy As P44.P44Proxy,
                                       ByRef P44AccountGroup As String,
                                        Optional ByVal SSOAAct As Utilities.SSOAAccount = Utilities.SSOAAccount.P44,
                                        Optional ByRef strMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        Dim P44WebServiceUrl As String
        Dim P44WebServiceLogin As String
        Dim P44WebServicePassword As String
        Dim oResult As DTO.WCFResults = readSSOASettings(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword, P44AccountGroup, SSOAAct, strMsg)
        If oResult.Success = False Then Return False
        Dim bUseTLS12 As Boolean = If(GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        oP44Proxy = New P44.P44Proxy(P44WebServiceUrl, P44WebServiceLogin, P44WebServicePassword, bUseTLS12)
        If oP44Proxy Is Nothing Then
            strMsg += String.Format("Cannot connect to API Proxy; check if the current user {0} has an account to access the Carrier API interface", Me.Parameters.UserName)
            Return False
        Else
            Return True
        End If

    End Function

    ''' <summary>
    ''' Create  NGL API Bid records as children of the LoadTender data.  the caller must create the tblLoadTender record
    ''' </summary>
    ''' <param name="oResponse"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SHID"></param>
    ''' <param name="origST"></param>
    ''' <param name="destST"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="strMsg"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="DefaultRequiredDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 02/06/2018 for v-8.1
    '''   generic NGL API Bid create method that does not require a booking record
    '''   the tblLoadtender Record must still exist.
    ''' Modified by RHR for v-8.1 on 03/26/18
    '''     added logic for Line 
    ''' Modified by RHR for v-8.2.1 on 10/20/2019
    '''     added new API codes to use for Line Haul -- GFC + Air + LH + OCEAN
    ''' Modified by RHR for v-8.5.4.001 on 07/07/2023 
    '''     added new logic to assign the Cost Adjustment Type 
    '''     added new logic to calculate the customer upcharge precent
    ''' </remarks>
    Public Function CreateNGLAPIBid(ByVal oResponse As List(Of P44.rateQuoteResponse),
                                     ByVal LoadTenderControl As Integer,
                                     ByVal SHID As String,
                                     ByVal origST As String,
                                     ByVal destST As String,
                                     ByVal intCompControl As Integer,
                                     Optional ByRef strMsg As String = "",
                                     Optional ByVal BookControl As Integer = 0,
                                     Optional ByVal DefaultRequiredDate As Date? = Nothing,
                                     Optional ByVal bStatusCode As BSCEnum = BSCEnum.Active) As Boolean
        Using Logger.StartActivity("CreateNGLAPIBid(LoadTenderControl: {LoadTenderControl})", LoadTenderControl)
            If LoadTenderControl = 0 Then
                Dim lDetails As New List(Of String) From {"Load Tendered Reference", " cannot be found and "}
                throwInvalidKeyParentRequiredException(lDetails)
                Return False
            End If

            Dim dtNow As Date = Date.Now
            Dim dtDefaultRequiredDate As Date = If(DefaultRequiredDate, dtNow.AddDays(3))
            Dim blnInvalidData As Boolean = False
            'default message if needed
            'Dim sLoadDetails = String.Format("Using Carrier {0},  LoadTenderControl {1}, CNS {2}, SCAC {3}, Order No. {4}", sLTCarrierName, LoadTenderControl, sCNS, sSCAC, sOrderNbr)

            Dim P44RetMsg As New P44.rateQuoteResponse() With {.mode = P44.P44Proxy.translateMode("LTL"), .scac = "TEMP", .vendor = "TEMP"}
            If oResponse Is Nothing OrElse oResponse.Count() < 1 Then
                If oResponse Is Nothing Then oResponse = New List(Of P44.rateQuoteResponse)()
                P44RetMsg.postMessagesOnly = True
                P44RetMsg.AddMessage(P44.MessageEnum.E_NoRatesFound, "NGL API", "No Rates Found", "")
                oResponse.Add(P44RetMsg)
                blnInvalidData = True
            End If
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try

                    If (oResponse(0).postMessagesOnly = False) Then
                        Dim Sec As New NGLSecurityDataProvider(Parameters)
                        If intCompControl = 0 Then
                            intCompControl = Sec.getLECompControl()
                        End If
                        Dim lCarrier As List(Of Integer) = Sec.RestrictedCarriersForSalesReps()
                        For Each q As P44.rateQuoteResponse In oResponse
                            Dim sFreightClass As String = "NA"
                            Dim sSCAC As String = If(String.IsNullOrWhiteSpace(q.scac), q.vendor, q.scac)
                            Dim oCarrierData As New LTS.spGetBidCarrierBySCACUsingCompLegalEntityResult()
                            If Not String.IsNullOrEmpty(sSCAC) Then
                                'get the carrier data
                                oCarrierData = db.spGetBidCarrierBySCACUsingCompLegalEntity(intCompControl, sSCAC).FirstOrDefault()
                            End If
                            If oCarrierData Is Nothing OrElse oCarrierData.CarrierControl = 0 Then
                                Dim sMsg = String.Format("Could not create NGL API Bid for LT with BookControl {0} and LoadTenderControl {1}. No Carrier data found using CompControl {2} and SCAC {3}. Source: NGLLoadTenderData.CreateNGLAPIBid (spGetBidCarrierBySCACUsingCompLegalEntity). ", BookControl, LoadTenderControl, intCompControl, sSCAC)
                                q.AddMessage(P44.MessageEnum.E_InvalidCarrierNumber, sMsg, "Invalid Carrier Configuration for API", "")
                                Continue For 'We still want to try to create any other bids -- the strMsg results should be logged by the caller
                            End If
                            If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(oCarrierData.CarrierControl))) Then
                                'skip
                                Continue For
                            End If

                            Dim fuelTotal As Decimal = 0
                            Dim fuelVar As Decimal = 0
                            Dim dLIneHaul As Decimal = 0
                            Dim Adjs As New List(Of LTS.tblBidCostAdj)
                            If Not q.rateDetail.rateAdjustments Is Nothing AndAlso q.rateDetail.rateAdjustments.Length > 0 Then
                                'Modified by RHR for v-8.2.1 on 10/20/2019  added GFC
                                'line haul = ITEM + DEF + DSC + GFC + Air + LH + OCEAN
                                'Modified by RHR for v-8.5.4.001 on 07/07/2023
                                '   added new logic to assign the Cost Adjustment Type
                                Dim CostAdjType As Integer = NGLLookupDataProvider.CostAdjType.Accessorial
                                For Each a As P44.rateAdjustment In q.rateDetail.rateAdjustments
                                    If a.descriptionCode = "FSC" Then
                                        fuelTotal = a.amount
                                        fuelVar = a.rate
                                        CostAdjType = NGLLookupDataProvider.CostAdjType.Fuel
                                    Else

                                        If a.descriptionCode = "GFC" _
                                            Or a.descriptionCode = "Air" _
                                            Or a.descriptionCode = "LH" _
                                            Or a.descriptionCode = "OCEAN" _
                                            Or a.descriptionCode = "ITEM" _
                                            Or a.descriptionCode = "DEF" Then
                                            CostAdjType = NGLLookupDataProvider.CostAdjType.Service
                                            dLIneHaul += a.amount
                                        ElseIf a.descriptionCode = "DSC" Or a.amount < 0 Then
                                            CostAdjType = NGLLookupDataProvider.CostAdjType.Discount
                                            dLIneHaul += a.amount
                                        Else
                                            CostAdjType = NGLLookupDataProvider.CostAdjType.Accessorial
                                            sFreightClass = a.freightClass
                                        End If
                                    End If
                                    Dim oBidAdj As New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = a.freightClass,
                                                                               .BidCostAdjWeight = a.weight,
                                                                               .BidCostAdjDesc = a.description,
                                                                               .BidCostAdjDescCode = a.descriptionCode,
                                                                               .BidCostAdjAmount = a.amount,
                                                                               .BidCostAdjRate = a.rate,
                                                                               .BidCostAdjTypeControl = CostAdjType,
                                                                               .BidCostAdjModDate = dtNow,
                                                                               .BidCostAdjModUser = Me.Parameters.UserName}
                                    Adjs.Add(oBidAdj)
                                Next
                            End If

                            Dim sb = New System.Text.StringBuilder()
                            Dim sSpacer = ""
                            Dim Errs As New List(Of LTS.tblBidSvcErr)
                            If Not q.errors Is Nothing AndAlso q.errors.Length > 0 Then
                                For Each se In q.errors
                                    Dim oBidErr As New LTS.tblBidSvcErr With {.BidSvcErrErrorMessage = se.errorMessage,
                                                                          .BidSvcErrVendorErrorCode = se.vendorErrorCode,
                                                                          .BidSvcErrVendorErrorMessage = se.vendorErrorMessage,
                                                                          .BidSvcErrCode = se.errorCode,
                                                                          .BidSvcErrFieldName = se.fieldName,
                                                                          .BidSvcErrMessage = se.message,
                                                                          .BidSvcErrModDate = dtNow,
                                                                          .BidSvcErrModUser = Me.Parameters.UserName}
                                    Errs.Add(oBidErr)
                                    sb.Append(sSpacer)
                                    sb.Append(se.message)
                                    sSpacer = "; AND, "
                                Next
                            End If
                            Dim strErrs = sb.ToString()

                            sb.Clear()
                            sSpacer = ""
                            If Not q.infos Is Nothing AndAlso q.infos.Length > 0 Then
                                For Each si In q.infos
                                    sb.Append(sSpacer)
                                    sb.Append(si.infoMessage)
                                    sSpacer = "; AND, "
                                Next
                            End If
                            Dim strInfos = sb.ToString()
                            sb.Clear()
                            sSpacer = ""
                            If Not q.warnings Is Nothing AndAlso q.warnings.Length > 0 Then
                                For Each sw In q.warnings
                                    sb.Append(sSpacer)
                                    sb.Append(sw.warningMessage)
                                    sSpacer = "; AND, "
                                Next
                            End If
                            Dim strWarnings = sb.ToString()
                            sb.Clear()

                            Dim blnInterline As Boolean = True
                            If String.IsNullOrWhiteSpace(q.interLine) Then
                                blnInterline = False
                            Else
                                If q.interLine.Trim.ToUpper() = "FALSE" OrElse q.interLine.Trim.ToUpper() = "DIRECT" OrElse q.interLine.Trim.ToUpper() = "UNSPECIFIED" Then
                                    blnInterline = False
                                End If
                            End If


                            Dim bArchived = 0


                            'If (q.errors.Length > 0 AndAlso q.rateDetail.total = 0) Then
                            '    bStatusCode = BSCEnum.BidError
                            '    bArchived = 1
                            'End If
                            Dim dtBidDeliveryDate As Date? = dtDefaultRequiredDate
                            If Not String.IsNullOrWhiteSpace(q.deliveryDate) Then
                                If Not Date.TryParse(q.deliveryDate, dtBidDeliveryDate) Then dtBidDeliveryDate = dtDefaultRequiredDate
                            End If
                            Dim dtBidQuoteDate As Date? = dtNow
                            If Not String.IsNullOrWhiteSpace(q.quoteDate) Then
                                If Not Date.TryParse(q.quoteDate, dtBidQuoteDate) Then dtBidQuoteDate = dtNow
                            End If
                            Dim dtBidExpirationDate As Date = dtNow
                            If Not String.IsNullOrWhiteSpace(q.expirationDate) Then
                                If Not Date.TryParse(q.expirationDate, dtBidExpirationDate) Then dtBidExpirationDate = dtNow.ToShortDateString()
                            End If

                            'Begin Modified by RHR for v-8.5.4.001 on 07/07/2023
                            '   added new logic to calculate the customer upcharge precent
                            Dim dblCarrierCostUpcharge As Double = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(oCarrierData.CarrierControl, intCompControl)
                            Dim dblBidCustLineHaul = dLIneHaul + (dLIneHaul * dblCarrierCostUpcharge)
                            ' we now need to add the line haul and customer line haul to the adjustments
                            Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                      .BidCostAdjWeight = q.totalWeight,
                                                                      .BidCostAdjAmount = dblBidCustLineHaul,
                                                                      .BidCostAdjRate = 1,
                                                                      .BidCostAdjDescCode = "UPLF",
                                                                      .BidCostAdjDesc = "Customer Line Haul Charges",
                                                                      .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                                                                      .BidCostAdjModDate = dtNow,
                                                                      .BidCostAdjModUser = Me.Parameters.UserName})

                            Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = sFreightClass,
                                                                      .BidCostAdjWeight = q.totalWeight,
                                                                      .BidCostAdjAmount = dLIneHaul,
                                                                      .BidCostAdjRate = 1,
                                                                      .BidCostAdjDescCode = "400",
                                                                      .BidCostAdjDesc = "Carrier Line Haul Charges",
                                                                      .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierLineHaul,
                                                                      .BidCostAdjModDate = dtNow,
                                                                      .BidCostAdjModUser = Me.Parameters.UserName})

                            Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.P44,
                                                            .BidCarrierControl = oCarrierData.CarrierControl,
                                                            .BidCarrierNumber = If(oCarrierData.CarrierNumber, 0),
                                                            .BidCarrierName = Left(oCarrierData.CarrierName, 40),
                                                            .BidCarrierSCAC = Left(If(String.IsNullOrWhiteSpace(q.scac), q.vendor, q.scac), 4),
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = q.rateDetail.total,
                                                            .BidLineHaul = dLIneHaul,
                                                            .BidFuelTotal = fuelTotal,
                                                            .BidFuelVariable = fuelVar,
                                                            .BidFuelUOM = "Flat Rate",
                                                            .BidOrigState = Left(origST, 2),
                                                            .BidDestState = Left(destST, 2),
                                                            .BidPosted = dtNow,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = bArchived,
                                                            .BidMode = P44.P44Proxy.transportationModeTranslation(q.mode),
                                                            .BidErrorCount = If(q.errors?.Length > 0, q.errors.Length, 0),
                                                            .BidErrors = Left(strErrs, 3999),
                                                            .BidWarnings = Left(strWarnings, 3999),
                                                            .BidInfos = Left(strInfos, 3999),
                                                            .BidInterLine = blnInterline,
                                                            .BidQuoteNumber = Left(q.quoteNumber, 100),
                                                            .BidTransitTime = q.transitTime,
                                                            .BidDeliveryDate = dtBidDeliveryDate,
                                                            .BidQuoteDate = dtBidQuoteDate,
                                                            .BidTotalWeight = q.totalWeight,
                                                            .BidDetailTotal = q.rateDetail.total,
                                                            .BidDetailTransitTime = q.rateDetail.transitTime,
                                                            .BidAdjustments = 0, 'difference between line haul and total cost 
                                                            .BidAdjustmentCount = If(q.rateDetail.rateAdjustments?.Length > 0, q.rateDetail.rateAdjustments.Length, 0),
                                                            .BidVendor = Left(If(String.IsNullOrWhiteSpace(q.vendor), q.scac, q.vendor), 20),
                                                            .BidContractID = Left(q.contractId, 50),
                                                            .BidServiceType = Left(q.serviceType, 50),
                                                            .BidTotalPlts = q.totalPallets,
                                                            .BidTotalQty = q.totalPieces,
                                                            .BidComments = Left(q.carrierNote, 255),
                                                            .BidExpires = dtBidExpirationDate,
                                                            .BidCustLineHaul = dblBidCustLineHaul,
                                                            .BidCustTotalCost = (q.rateDetail.total - dLIneHaul) + dblBidCustLineHaul,
                                                            .BidModDate = dtNow,
                                                            .BidModUser = Me.Parameters.UserName}
                            'End Modified by RHR for v-8.5.4.001 on 07/07/2023
                            Dim oTable = db.tblBids
                            oTable.InsertOnSubmit(oBid)
                            db.SubmitChanges()
                            Dim bidCtrl = oBid.BidControl

                            Dim oT = db.tblBidCostAdjs
                            For Each adj In Adjs
                                adj.BidCostAdjBidControl = bidCtrl
                                oT.InsertOnSubmit(adj)
                            Next
                            Dim oTbl = db.tblBidSvcErrs
                            For Each e In Errs
                                e.BidSvcErrBidControl = bidCtrl
                                oTbl.InsertOnSubmit(e)
                            Next

                            Dim lMessages As List(Of P44.P44Message) = q.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                            If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                                'Reset blnInvalidData flag to false because we have already logged the messages
                                blnInvalidData = False
                                For Each msg In lMessages
                                    msg.bLogged = True
                                    Dim oBidErr As New LTS.tblBidSvcErr With {
                                    .BidSvcErrBidControl = bidCtrl,
                                    .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                    .BidSvcErrMessage = msg.Details,
                                    .BidSvcErrFieldName = msg.FieldName,
                                    .BidSvcErrModDate = dtNow,
                                    .BidSvcErrModUser = Me.Parameters.UserName}
                                    oTbl.InsertOnSubmit(oBidErr)
                                Next
                            End If


                            db.SubmitChanges()
                        Next
                    Else
                        'get the ngl dat carrier dat
                        Dim iCarrNumber As Integer = CInt(GetParValue("DATCarrierNumber", intCompControl))
                        Dim icarriercontrol = db.CarrierRefIntegrations.Where(Function(x) x.CarrierNumber = iCarrNumber).Select(Function(y) y.CarrierControl).FirstOrDefault()
                        If blnInvalidData Or oResponse.Any(Function(x) x.postMessagesOnly = True) Then
                            For Each res In oResponse.Where(Function(x) x.postMessagesOnly = True).ToArray()
                                Dim lMessages As List(Of P44.P44Message) = res.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                                If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                                    Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                        .BidBidTypeControl = BidTypeEnum.CHRAPI,
                                                                        .BidCarrierControl = icarriercontrol,
                                                                        .BidCarrierNumber = iCarrNumber,
                                                                        .BidCarrierName = "NGL API",
                                                                        .BidCarrierSCAC = "TEMP",
                                                                        .BidSHID = SHID,
                                                                        .BidTotalCost = 0,
                                                                        .BidLineHaul = 0,
                                                                        .BidFuelTotal = 0,
                                                                        .BidFuelVariable = 0,
                                                                        .BidFuelUOM = "NA",
                                                                        .BidOrigState = Left(origST, 2),
                                                                        .BidDestState = Left(destST, 2),
                                                                        .BidPosted = dtNow,
                                                                        .BidStatusCode = bStatusCode,
                                                                        .BidArchived = False,
                                                                        .BidMode = "NA",
                                                                        .BidErrorCount = lMessages.Count(),
                                                                        .BidErrors = Left(res.concateMessages, 3999),
                                                                        .BidVendor = "TEMP",
                                                                        .BidTotalPlts = 0,
                                                                        .BidTotalQty = 0,
                                                                        .BidModDate = dtNow,
                                                                        .BidModUser = Me.Parameters.UserName}

                                    Dim oTable = db.tblBids
                                    oTable.InsertOnSubmit(oBid)
                                    db.SubmitChanges()
                                    Dim bidCtrl = oBid.BidControl

                                    If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                                        Dim oTbl = db.tblBidSvcErrs
                                        For Each msg In lMessages
                                            msg.bLogged = True
                                            'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                            Dim oBidErr As New LTS.tblBidSvcErr With {
                                                .BidSvcErrBidControl = bidCtrl,
                                                .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                                .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                                .BidSvcErrFieldName = msg.FieldName,
                                                .BidSvcErrModDate = dtNow,
                                                .BidSvcErrModUser = Me.Parameters.UserName}
                                            oTbl.InsertOnSubmit(oBidErr)
                                        Next
                                    End If

                                    db.SubmitChanges()
                                End If
                            Next
                        End If

                    End If
                    Return True
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("CreateNGLAPIBid"))
                End Try
                Return True
            End Using
        End Using

    End Function

    ''' <summary>
    ''' Gets the BookControl from tblLoadTender using the param LTControl
    ''' Uses this to see if any of the following fields has changed since the
    ''' LoadTender was created by comparing values in the Book table to the
    ''' LoadTenderBook table
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns>LTS.spHasLoadTenderChangedResult</returns>
    ''' <remarks>
    ''' Created By LVV 8/23/17 v-8.0 TMS 365
    ''' </remarks>
    Public Function HasLoadTenderChanged(ByVal LoadTendControl As Integer) As LTS.spHasLoadTenderChangedResult
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Return (From d In db.spHasLoadTenderChanged(LoadTendControl) Select d).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("HasLoadTenderChanged"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetNextCNSNumberByLE(ByVal LEAdminControl As Integer, Optional ByVal LEAdminLegalEntity As String = "") As String
        Dim strConsNumber As String = ""
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oResults = db.spGetNextConsNumberByLE(LEAdminControl, LEAdminLegalEntity).FirstOrDefault()
                If Not oResults Is Nothing Then
                    strConsNumber = oResults.BookConsNumber
                End If
            Catch ex As Exception
                Utilities.SaveAppError(ex, Parameters)
                'do nothing just returns an empty string
            End Try
        End Using
        Return strConsNumber
    End Function

    ''' <summary>
    ''' Gets the Origin and Destination CompControls if they match an existing company
    ''' Also gets the book fees
    ''' Returns True if at least one company was valid (aka continue getting tariff bids)
    ''' Returns False is neither company was valid (aka exit get tariff bids)
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="accessorials"></param>
    ''' <param name="OrigCompControl"></param>
    ''' <param name="DestCompControl"></param>
    ''' <param name="lAccessorial"></param>
    ''' <param name="AccessorialValues"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 12/12/2018
    '''   added logic to support AccessorialValues as default costs for fees
    '''   typically used to support fees like flat rate fuel for tariffs
    ''' Modified by RHR for v-8.5.4.002 on 07/20/2023 compcontrol is no longer required
    ''' </remarks>
    Public Function GetInfoForLTRateQuoteTariffBids(ByVal LoadTenderControl As Integer, ByVal accessorials As String(), ByRef OrigCompControl As Integer, ByRef DestCompControl As Integer, ByRef lAccessorial As List(Of DTO.BookFee), Optional ByVal AccessorialValues As String() = Nothing) As Boolean
        Using operation = Logger.StartActivity("GetInfoForLTRateQuoteTariffBids(LoadTenderControl: {LoadTenderControl}, accessorials: {@Accessorials}, OrigCompControl: {OrigCompControl}, DestCompControl: {DestCompControl}, lAccessorial: {@lAccessorial}, AccessorialValues: {@AccessorialValues}", LoadTenderControl, accessorials, OrigCompControl, DestCompControl, lAccessorial, AccessorialValues)
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Dim blnRet As Boolean = False
                Try
                    Logger.Information("execute spGetCompFromLoadTenderOrigin and spGetCompFromLoadTenderDest to get the Origin and Destination CompControls for LoadTenderControl: " & LoadTenderControl & " and UserName: " & Parameters.UserName, "GetInfoForLTRateQuoteTariffBids")
                    Dim spOrig = (From d In db.spGetCompFromLoadTenderOrigin(LoadTenderControl, Parameters.UserName) Select d).FirstOrDefault()
                    If spOrig.CompControl.HasValue Then
                        OrigCompControl = spOrig.CompControl.Value
                        Logger.Information("Origin CompControl: " & OrigCompControl, "GetInfoForLTRateQuoteTariffBids")
                    End If

                    Logger.Information("execute spGetCompFromLoadTenderDest to get the Destination CompControl for LoadTenderControl: " & LoadTenderControl & " and UserName: " & Parameters.UserName, "GetInfoForLTRateQuoteTariffBids")
                    Dim spDest = (From d In db.spGetCompFromLoadTenderDest(LoadTenderControl, Parameters.UserName) Select d).FirstOrDefault()
                    If spDest.CompControl.HasValue Then DestCompControl = spDest.CompControl.Value

                    'If OrigCompControl <> 0 OrElse DestCompControl <> 0 Then
                    blnRet = True

                    'Only do this part if there is a possibility of having a tariff (aka at least one valid company)
                    If Not accessorials Is Nothing AndAlso accessorials.Count() > 0 Then
                        Try
                            For i As Integer = 0 To accessorials.Count() - 1
                                Logger.Information("execute spGetAccessorialFromNGLAPICode to get the AccessorialCode for Accessorial: {@0} ", accessorials(i))
                                Dim spRes = (From d In db.spGetAccessorialFromNGLAPICode(accessorials(i)) Select d).FirstOrDefault()
                                If spRes.AccessorialCode.HasValue AndAlso spRes.AccessorialCode.Value <> 0 Then
                                    'we have a valid code so add it to the list
                                    If spRes.AccessorialCode.HasValue Then

                                        Dim dValue As Decimal = 0
                                        Logger.Information("MapAccessorialValueOverride for Accessorial: {@0} ", spRes.AccessorialCode.Value)
                                        dValue = MapAccessorialValueOverride(spRes.AccessorialCode.Value) 'This is a hack override if for some reason it does not come from the original order
                                        Logger.Information("MapAccessorialValueOverride for Accessorial: {@0} returned value: {@1} ", spRes.AccessorialCode.Value, dValue)
                                        If Not AccessorialValues Is Nothing AndAlso AccessorialValues.Count() > i Then
                                            'try to add the string value to the decimal value if it is a valid number
                                            Decimal.TryParse(AccessorialValues(i), dValue)
                                            Logger.Information("If not AccessorialValeus and ValuesCount > i ({2}) AccessorialValues for Accessorial: {@0} returned value: {@1} ", spRes.AccessorialCode.Value, dValue, i)
                                        End If
                                        Logger.Information("Add Accessorial to the lAccessorial list for Accessorial: {0}, Type Tariff, Caption: {1}, dVAlue: {2} ", spRes.AccessorialCode.Value, spRes.AccessorialCaption, dValue)
                                        lAccessorial.Add(New DTO.BookFee With {
                                                    .BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff,  'the method is called tariff, so it is always tariff
                                                    .BookFeesAccessorialCode = spRes.AccessorialCode.Value,
                                                    .BookFeesCaption = spRes.AccessorialCaption,
                                                    .BookFeesMinimum = dValue,
                                                    .BookFeesValue = dValue,
                                                    .BookFeesModDate = Date.Now(),
                                                    .BookFeesModUser = Parameters.UserName})
                                    End If

                                End If
                            Next
                            'Removed by RHR for v-8.5.4.005 on 02/06/2024 we do not need to call spGetAccessorialFromNGLAPICode twice
                            '   this is  bad or duplicate code that was never removed.
                            'For Each acc In accessorials
                            '    Dim spRes = (From d In db.spGetAccessorialFromNGLAPICode(acc) Select d).FirstOrDefault()
                            '    If spRes.AccessorialCode.HasValue AndAlso spRes.AccessorialCode.Value <> 0 Then
                            '        'we have a valid code so add it to the list
                            '        lAccessorial.Add(New DTO.BookFee With {.BookFeesAccessorialCode = spRes.AccessorialCode.Value, .BookFeesCaption = spRes.AccessorialCaption, .BookFeesModDate = Date.Now(), .BookFeesModUser = Parameters.UserName})
                            '    End If
                            'Next
                        Catch ex As Exception
                            Logger.Error(ex, "Error in GetInfoForLTRateQuoteTariffBids")
                            'do not fail just because we cannot create a accessorial records for the quote just save the error to the log
                            'Dim sMsg As String = "Warning! " & buildProcedureName("GetInfoForLTRateQuoteTariffBids") & " could not process accessorial fees because: " & ex.Message & ". No messate to user, quote generation was not suspended."
                            'SaveAppError(sMsg, Parameters)
                        End Try
                    End If
                    'End If
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetInfoForLTRateQuoteTariffBids")
                    'ManageLinqDataExceptions(ex, buildProcedureName("GetInfoForLTRateQuoteTariffBids"))
                End Try
                Return blnRet
            End Using

        End Using
    End Function

    Public Function MapAccessorialValueOverride(ByVal acc As String) As Decimal
        If (acc = "FSC") Then
            Return 2
        End If

    End Function

    ''' <summary>
    ''' Removes the RateShopping Historical Quotes for the current user LTBookControl = 0 LTPosterUserControl = current user
    ''' </summary>
    ''' <returns></returns>
    Public Function DeleteRateShoppingQuotes() As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Dim iUserControl As Integer = Me.Parameters.UserControl
            If iUserControl = 0 Then Return False
            Try
                db.spDeleteRateShoppingHistQuotes(iUserControl)
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteRateShoppingQuotes"))
            End Try
            Return blnRet
        End Using
    End Function

#End Region

#Region "Protected Functions"

#Region "CHR API Methods"


    Private Function getCHRRateRequest(ByVal order As Models.RateRequestOrder, ByRef oItems As List(Of Models.RateRequestItem), ByRef customerCode As String) As CHR.CHRRateRequest
        Dim dtShipDate As DateTime = DateTime.Now
        DateTime.TryParse(order.ShipDate, dtShipDate)
        Dim dtdeliveryDate As DateTime = DateTime.Now.AddDays(1)
        DateTime.TryParse(order.DeliveryDate, dtdeliveryDate)
        'Note: CHR uses CHRSpecialRequirement for some accessorials like liftGate et...

        Dim lRefs = New List(Of CHR.CHRReferenceNumbers)
        CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.SHID, order.BookConsPrefix, lRefs)
        CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.CRID, order.Pickup.BookCarrOrderNumber, lRefs)
        CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.MBOL, order.Pickup.SHID, lRefs)
        CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.MBOL, order.Pickup.SHID, lRefs)

        Dim oDest As New CHR.CHRAddress() With {
                .locationName = order.CompName,
                .address1 = order.Stops(0).CompAddress1,
                .city = order.Stops(0).CompCity,
                .stateProvinceCode = Left(order.Stops(0).CompState, 2),
                .countryCode = "US",
                .postalCode = order.Stops(0).CompPostalCode,
                .referenceNumbers = lRefs.ToArray()
             }

        Dim lStops As New List(Of CHR.CHRAddress)
        lStops.Add(oDest)

        Dim lItemRefs = New List(Of CHR.CHRReferenceNumbers)
        'For Each item In oItems
        'CHR.CHRReferenceNumbers.addReferenceNumber(CHR.CHRAPI.RefNumbers.CRID, , lRefs)

        ' Next

        'oRefs.Add()
        Dim iTmp As Integer = 0
        Dim oRequest As CHR.CHRRateRequest = New CHR.CHRRateRequest() With {
                .oRefs = lRefs.ToArray(),
             .oAccessorials = order.Accessorials,
             .oOrigin = New CHR.CHRAddress() With {
                .locationName = order.CompName,
                .address1 = order.Pickup.CompAddress1,
                .city = order.Pickup.CompCity,
                .stateProvinceCode = Left(order.Pickup.CompState, 2),
                .countryCode = "US",
                .postalCode = order.Pickup.CompPostalCode,
                .referenceNumbers = lRefs.ToArray()
                },
            .lStops = lStops,
            .lItems = (From i In oItems Select New CHR.CHRItem() With {
                                            .referenceNumbers = lRefs.ToArray(),
                                            .description = If(String.IsNullOrWhiteSpace(i.Description), "misc products", i.Description),
                                            .freightClass = If(Integer.TryParse(i.FreightClass, iTmp), iTmp, 100),
                                            .actualWeight = If(Integer.TryParse(i.Weight.ToString(), iTmp), iTmp, 100),
                                            .weightUnit = If(String.IsNullOrWhiteSpace(i.WeightUnit), "Pounds", i.WeightUnit),
                                            .length = i.Length,
                                            .width = i.Width,
                                            .height = i.Height,
                                            .pallets = i.PalletCount,
                                            .pieces = i.NumPieces,
                                            .palletSpaces = i.PalletCount,
                                            .packagingCode = If(String.IsNullOrWhiteSpace(i.PackageType), "PLT", i.PackageType),
                                            .productName = If(String.IsNullOrWhiteSpace(i.Code), "Goods", i.Code)}).ToList(),
            .customerCode = customerCode,
            .declaredValue = 1000
            }
        'oRefs = oRefs[0] = New CHRReferenceNumbers { type = "DEL", value = "DeliverNumber1" };
        '.timeout = 10,
        '.shipDate = dtShipDate.ToShortDateString(),
        '.deliveryDate = dtdeliveryDate.ToShortDateString(),
        '.returnMultiple = True,
        '.loginGroupKey = P44AccountGroup,
        '.destination = New P44.addressInfo() With {
        '    .address1 = order.Stops(0).CompAddress1,
        '    .address2 = order.Stops(0).CompAddress2,
        '    .address3 = order.Stops(0).CompAddress3,
        '    .city = order.Stops(0).CompCity,
        '    .stateName = order.Stops(0).CompState,
        '    .country = order.Stops(0).CompCountry,
        '    .postalCode = order.Stops(0).CompPostalCode,
        '    .companyName = order.Stops(0).CompName
        '},

        '.accessorials = order.Accessorials,
        '.lineItems = (From i In oItems Select New P44.rateQuoteLineImpl() With {
        '    .weight = i.Weight.ToString(),
        '    .weightUnit = i.WeightUnit,
        '    .freightClass = i.FreightClass,
        '    .palletCount = i.PalletCount,
        '    .numPieces = i.NumPieces,
        '    .description = i.Description,
        '    .length = i.Length,
        '    .width = i.Width,
        '    .height = i.Height,
        '    .packageType = i.PackageType,
        '    .nmfcItem = i.NMFCItem,
        '    .nmfcSub = i.NMFCSub,
        '    .stackable = i.Stackable
        '}).ToArray()
        '}
        Return oRequest
    End Function


#End Region

    ''' <summary>
    ''' Generate p44 rate request for rate shopping when no order is available
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="oItems"></param>
    ''' <param name="P44AccountGroup"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
    ''' </remarks>
    Private Function getRateRequest(ByVal order As Models.RateRequestOrder, ByRef oItems As List(Of Models.RateRequestItem), ByRef P44AccountGroup As String) As P44.RateRequest

        Dim oRequest As P44.RateRequest
        Using operation = Logger.StartActivity("getRateRequest(Order: {@Order}, oItems: {@Items}, P44AccountGroup: {P44AccountGroup}", order, oItems, P44AccountGroup)
            Dim dtShipDate As DateTime = DateTime.Now
            DateTime.TryParse(order.ShipDate, dtShipDate)
            Dim dtdeliveryDate As DateTime = DateTime.Now.AddDays(1)
            DateTime.TryParse(order.DeliveryDate, dtdeliveryDate)
            Dim dblGlobalAPIUseNMFCCodes As Double
            ' Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
            dblGlobalAPIUseNMFCCodes = GetParValue("GlobalAPIUseNMFCCodes", 0) 'use global setting
            oRequest = New P44.RateRequest() With {
               .fetchAllGuaranteed = True,
               .timeout = 10,
               .shipDate = dtShipDate.ToShortDateString(),
               .deliveryDate = dtdeliveryDate.ToShortDateString(),
               .returnMultiple = True,
               .loginGroupKey = P44AccountGroup,
               .destination = New P44.addressInfo() With {
                   .address1 = order.Stops(0).CompAddress1,
                   .address2 = order.Stops(0).CompAddress2,
                   .address3 = order.Stops(0).CompAddress3,
                   .city = order.Stops(0).CompCity,
                   .stateName = order.Stops(0).CompState,
                   .country = order.Stops(0).CompCountry,
                   .postalCode = order.Stops(0).CompPostalCode,
                   .companyName = order.Stops(0).CompName
               },
               .origin = New P44.addressInfo() With {
                   .address1 = order.Pickup.CompAddress1,
                   .address2 = order.Pickup.CompAddress2,
                   .address3 = order.Pickup.CompAddress3,
                   .city = order.Pickup.CompCity,
                   .stateName = order.Pickup.CompState,
                   .country = order.Pickup.CompCountry,
                   .postalCode = order.Pickup.CompPostalCode,
                   .companyName = order.Pickup.CompName
               },
               .accessorials = order.Accessorials,
               .lineItems = (From i In oItems Select New P44.rateQuoteLineImpl() With {
                   .weight = i.Weight.ToString(),
                   .weightUnit = i.WeightUnit,
                   .freightClass = i.FreightClass,
                   .palletCount = i.PalletCount,
                   .numPieces = i.NumPieces,
                   .description = i.Description,
                   .length = i.Length,
                   .width = i.Width,
                   .height = i.Height,
                   .packageType = i.PackageType,
                    .nmfcItem = If(dblGlobalAPIUseNMFCCodes = 0, "", i.NMFCItem), '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
                    .nmfcSub = If(dblGlobalAPIUseNMFCCodes = 0, "", i.NMFCSub), '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
                   .stackable = i.Stackable
               }).ToArray()
           }
        End Using

        Return oRequest
    End Function


    ''' <summary>
    ''' Read the booking data and bookpackage data into the API object data
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="timeOut"></param>
    ''' <param name="sDefFrtClass"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 2/26/2018 for v-8.2
    '''   we now map vBookPackage for each order to the API Items
    '''   not the bookItem data
    '''  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
    ''' </remarks>
    Protected Function CopyLTLDataToP44Data(ByRef oBook() As LTS.vBookRevenue, Optional ByVal timeOut As Integer = 20, Optional ByVal sDefFrtClass As String = "70") As P44.RateRequest
        Dim oRet As New P44.RateRequest()
        If ((oBook Is Nothing) OrElse oBook.Count() < 1 OrElse (oBook(0).BookControl = 0)) Then Return Nothing

        Dim dtShip As Date = Date.MaxValue
        Dim dtRequired As Date = Date.MinValue
        Dim oBookPkgDAL = New NGLBookPackage(Me.Parameters)
        Dim oItems As New List(Of LTS.vBookPackage)
        Dim oBookAccDAL = New NGLBookAccessorial(Me.Parameters)
        Dim oBookAccessorial As New List(Of LTS.vBookAccessorial)
        Dim sAccessorial As New List(Of String)
        '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
        Dim dblGlobalAPIUseNMFCCodes As Double
        dblGlobalAPIUseNMFCCodes = GetParValue("GlobalAPIUseNMFCCodes", oBook(0).BookCustCompControl)
        For Each book In oBook
            If Not book Is Nothing AndAlso book.BookControl <> 0 AndAlso Not book.BookLoads Is Nothing AndAlso book.BookLoads.Count() > 0 Then
                'get the earliest load date
                If book.BookDateLoad.HasValue AndAlso book.BookDateLoad.Value < dtShip Then dtShip = book.BookDateLoad.Value
                'get the latest delivery date
                If book.BookDateRequired.HasValue AndAlso book.BookDateRequired.Value > dtRequired Then dtRequired = book.BookDateRequired.Value
                Dim filters As New Models.AllFilters With {.ParentControl = book.BookControl}
                Dim ct As Integer
                Dim oPkgs As LTS.vBookPackage() = oBookPkgDAL.GetBookPackages(filters, ct, False)
                If Not oPkgs Is Nothing AndAlso oPkgs.Count() > 0 Then
                    oItems.AddRange(oPkgs)
                End If
                ct = 0
                filters = New Models.AllFilters With {.ParentControl = book.BookControl} 'we must clear the filter to be sure we have good data
                Dim oAccs As LTS.vBookAccessorial() = oBookAccDAL.GetBookAccessorials(filters, ct)
                If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                    oBookAccessorial.AddRange(oAccs)
                    sAccessorial.AddRange(oAccs.Select(Function(x) x.NACCode).ToList())
                End If

                'For Each bl In book.BookLoads
                '    If Not bl Is Nothing AndAlso Not bl.BookItems Is Nothing AndAlso bl.BookItems.Count() > 0 Then
                '        oItems.AddRange(bl.BookItems)
                '    End If
                'Next
            End If
        Next
        If dtShip = Date.MaxValue Then dtShip = Date.Now 'set to today
        If dtRequired = Date.MinValue Then dtRequired = Date.Now.AddDays(5) 'set to 5 days from now

        With oRet
            .timeout = timeOut
            .shipDate = dtShip.ToShortDateString
            .deliveryDate = dtRequired.ToShortDateString
            .returnMultiple = True
            .destination = New P44.addressInfo() With {
                    .address1 = oBook(0).BookDestAddress1,
                    .address2 = oBook(0).BookDestAddress2,
                    .address3 = oBook(0).BookDestAddress3,
                    .city = oBook(0).BookDestCity,
                    .stateName = oBook(0).BookDestState,
                    .country = oBook(0).BookDestCountry,
                    .postalCode = oBook(0).BookDestZip,
                    .companyName = oBook(0).BookDestName
                }
            .origin = New P44.addressInfo() With {
                    .address1 = oBook(0).BookOrigAddress1,
                    .address2 = oBook(0).BookOrigAddress2,
                    .address3 = oBook(0).BookOrigAddress3,
                    .city = oBook(0).BookOrigCity,
                    .stateName = oBook(0).BookOrigState,
                    .country = oBook(0).BookOrigCountry,
                    .postalCode = oBook(0).BookOrigZip,
                    .companyName = oBook(0).BookOrigName
                }
            If Not sAccessorial Is Nothing AndAlso sAccessorial.Count() > 0 Then
                .accessorials = sAccessorial.ToArray()
            End If
            'old code removed by RHR on 2/26/19,  we do not use the item details for line items in API data
            '.lineItems = (From i In oItems Select New P44.rateQuoteLineImpl With {.weight = i.BookItemWeight.ToString(), .weightUnit = "lbs", .freightClass = If(String.IsNullOrEmpty(i.BookItemFAKClass), sDefFrtClass, i.BookItemFAKClass), .palletCount = CInt(i.BookItemPallets), .numPieces = i.BookItemQtyOrdered, .description = i.BookItemDescription}).ToArray()

            .lineItems = (From i In oItems Select New P44.rateQuoteLineImpl() With {
                        .weight = i.BookPkgWeight.ToString(),
                        .weightUnit = "lbs",
                        .freightClass = i.BookPkgFAKClass,
                        .palletCount = i.BookPkgCount,
                        .numPieces = i.BookPkgCount,
                        .description = i.BookPkgDescription,
                        .length = i.BookPkgLength,
                        .width = i.BookPkgWidth,
                        .height = i.BookPkgHeight,
                        .packageType = i.PackageType,
                        .nmfcItem = If(dblGlobalAPIUseNMFCCodes = 0, "", i.BookPkgNMFCClass), '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
                        .nmfcSub = If(dblGlobalAPIUseNMFCCodes = 0, "", i.BookPkgNMFCSubClass), '  Modified by RHR for v-8.5.3.003 on 07/27/2022 add ability to turn on or off NMFC codes for API.
                        .stackable = i.BookPkgStackable
                    }).ToArray()
        End With

        Return oRet
    End Function

    Protected Function GetLTLOrderData(ByVal BookControl As Integer) As LTS.vBookRevenue()
        Dim oData = DirectCast(Me.NDPBaseClassFactory("NGLBookRevenueData", False), NGLBookRevenueData).GetLTLvBookRevenues(BookControl)
        Return Nothing
    End Function

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblLoadTender)
        Dim skipObjs As New List(Of String) From {"UserSecurityControl", "SSOALoginURL", "Database", "DBServer", "ConnectionString", "UserName", "DATFeature", "SSOAUserName", "SSOAPassword", "TokenString", "TokenExpiresDate", "LTUpdated", "LTModDate", "LTModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.tblLoadTender
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .LTModDate = Date.Now
            .LTModUser = Me.Parameters.UserName
            .LTUpdated = If(d.LTUpdated Is Nothing, New Byte() {}, d.LTUpdated)
        End With
        Return oLTS
    End Function


    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLoadTenderFiltered(LoadTenderControl:=CType(LinqTable, LTS.tblLoadTender).LoadTenderControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As New DTO.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.tblLoadTender = TryCast(LinqTable, LTS.tblLoadTender)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblLoadTenders
                       Where d.LoadTenderControl = source.LoadTenderControl
                       Select New DTO.QuickSaveResults With {.Control = d.LoadTenderControl _
                                                         , .ModDate = d.LTModDate _
                                                         , .ModUser = d.LTModUser _
                                                         , .Updated = d.LTUpdated.ToArray}).First

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.tblLoadTender, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblLoadTender
        Dim oDTO As New DTO.tblLoadTender
        Dim skipObjs As New List(Of String) From {"UserSecurityControl", "SSOALoginURL", "Database", "DBServer", "ConnectionString", "UserName", "DATFeature", "SSOAUserName", "SSOAPassword", "TokenString", "TokenExpiresDate", "LTUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .LTUpdated = d.LTUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

#End Region

End Class


