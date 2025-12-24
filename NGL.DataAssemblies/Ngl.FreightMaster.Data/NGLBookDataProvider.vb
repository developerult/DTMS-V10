Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ServiceModel
Imports System.Data.SqlDbType
Imports System.Threading
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Windows.Forms
Imports System.IO.IsolatedStorage
Imports System.Runtime.InteropServices
Imports Ngl.FreightMaster.Data.LTS
Imports System.Web.UI.WebControls
Imports System.Text.RegularExpressions
Imports Serilog

'Imports System.Windows.Forms.Keys


Public Class NGLNewPOsForSolutionData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vNewPOsForSolutions
        Me.LinqDB = db
        Me.SourceClass = "NGLNewPOsForSolutionData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vNewPOsForSolutions
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
        Return GetNewPOFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing 'Cannot execuete without parameters
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetFirstRecord(ByVal LowerControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal FromDate As Date,
                                                 ByVal ToDate As Date,
                                                 ByVal UseLoadDate As Boolean,
                                                 ByVal NatAccountNumber As Integer,
                                                 ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DTran.formatStartDateFilter(FromDate)
                ToDate = DTran.formatEndDateFilter(ToDate)
                'For the test we use the book table and the bookload data is ignored.  
                'The final query will need to use a view that selects the book load data because
                'We need the temperature.
                Dim NewPO As New DTO.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                NewPO = (
                    From d In db.vNewPOsForSolutions
                    Where (LowerControl = 0 OrElse d.SolutionDetailPOHdrControl >= LowerControl) _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                    And
                    (If(UseLoadDate,
                            (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                            (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                    And
                    (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                    Order By d.SolutionDetailPOHdrControl
                    Select selectDTOData(d)).FirstOrDefault()
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                     ByVal FKControl As Long,
                                                     ByVal FromDate As Date,
                                                     ByVal ToDate As Date,
                                                     ByVal UseLoadDate As Boolean,
                                                     ByVal NatAccountNumber As Integer,
                                                     ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DTran.formatStartDateFilter(FromDate)
                ToDate = DTran.formatEndDateFilter(ToDate)
                Dim NewPO As New DTO.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                NewPO = (
                    From d In db.vNewPOsForSolutions
                    Where d.SolutionDetailPOHdrControl < CurrentControl _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                    And
                    (If(UseLoadDate,
                            (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                            (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                    And
                    (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                    Order By d.SolutionDetailPOHdrControl Descending
                    Select selectDTOData(d)).FirstOrDefault()
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetNextRecord(ByVal CurrentControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                ByVal UseLoadDate As Boolean,
                                                ByVal NatAccountNumber As Integer,
                                                ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim NewPO As New DTO.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                NewPO = (
                       From d In db.vNewPOsForSolutions
                       Where d.SolutionDetailPOHdrControl > CurrentControl _
                       And
                       (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                        And
                        (If(UseLoadDate,
                                (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad < ToDate.AddDays(1)),
                                (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired < ToDate.AddDays(1))
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                       Order By d.SolutionDetailPOHdrControl
                       Select selectDTOData(d)).FirstOrDefault()
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetLastRecord(ByVal UpperControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                ByVal UseLoadDate As Boolean,
                                                ByVal NatAccountNumber As Integer,
                                                ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim NewPO As New DTO.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If UpperControl <> 0 Then
                    NewPO = (
                        From d In db.vNewPOsForSolutions
                        Where d.SolutionDetailPOHdrControl >= UpperControl _
                        And
                        (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                        And
                        (If(UseLoadDate,
                                (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad < ToDate.AddDays(1)),
                                (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired < ToDate.AddDays(1))
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                        Order By d.SolutionDetailPOHdrControl
                        Select selectDTOData(d)).FirstOrDefault()

                Else
                    NewPO = (
                        From d In db.vNewPOsForSolutions
                        Where
                        (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                        And
                        (If(UseLoadDate,
                                (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad < ToDate.AddDays(1)),
                                (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired < ToDate.AddDays(1))
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                        Order By d.SolutionDetailPOHdrControl Descending
                        Select selectDTOData(d)).FirstOrDefault()
                End If
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetNewPOFiltered(ByVal Control As Long) As DTO.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim NewPO As DTO.tblSolutionDetail = (From d In db.vNewPOsForSolutions Where d.SolutionDetailPOHdrControl = Control Select selectDTOData(d)).FirstOrDefault()
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewPOFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Return tblSolutionDetail for orders in No Pro and New Lane status
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.3.007 on 2023-01-30 new 365 version for web Load Planning page
    '''     added support for New Lane
    ''' </remarks>
    Public Function GetNewPOFiltered365(ByVal Control As Long) As DTO.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim oPOData As LTS.vNewPOsForSolution365 = db.vNewPOsForSolution365s.Where(Function(x) x.POHdrControl = Control).FirstOrDefault()
                Dim NewPO As DTO.tblSolutionDetail = CopyvNewPOsForSolutionToSolutionDetailData(oPOData)
                Return NewPO
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewPOFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Get new  POs 
    ''' </summary>
    ''' <param name="Filter"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.6.101 on 01/31/2016
    '''   added logic to allow zero as the company control number so all orders are visible
    ''' </remarks>
    Public Function GetNewPOsFiltered(ByVal Filter As DTO.LoadPlanningTruckDataFilter) As DTO.tblSolutionDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Filter.StartDateFilter = DTran.formatStartDateFilter(Filter.StartDateFilter)
                Filter.StopDateFilter = DTran.formatEndDateFilter(Filter.StopDateFilter)
                'For this release the comp control number is requried and must be provided by the caller so we do not need to apply company level restrictions
                'Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                Dim OrigStates As New List(Of String)
                Dim blnUseOrigStateFilter As Boolean = False
                Dim DestStates As New List(Of String)
                Dim blnUseDestStateFilter As Boolean = False
                populateStateListsFromFilter(Filter, OrigStates, blnUseOrigStateFilter, DestStates, blnUseDestStateFilter)

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Try
                    Dim qNewPOs = (From d In db.vNewPOsForSolutions
                                   Where
                            (Filter.CompControlFilter = 0 OrElse d.SolutionDetailCompControl = Filter.CompControlFilter) _
                            And If(Filter.UseLoadDateFilter = True,
                                   If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value <= Filter.StopDateFilter, False),
                                   If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value <= Filter.StopDateFilter, False)) _
                       And (d.SolutionDetailOrigZip.Trim >= If(Filter.OrigStartZipFilter = String.Empty, "0", Filter.OrigStartZipFilter)) _
                       And (d.SolutionDetailOrigZip.Trim <= If(Filter.OrigStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.OrigStopZipFilter)) _
                       And (d.SolutionDetailDestZip.Trim >= If(Filter.DestStartZipFilter = String.Empty, "0", Filter.DestStartZipFilter)) _
                       And (d.SolutionDetailDestZip.Trim <= If(Filter.DestStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.DestStopZipFilter)) _
                       And If(Filter.OrigCityFilter.Trim = String.Empty, True, (d.SolutionDetailOrigCity.Trim.ToUpper = Filter.OrigCityFilter.Trim.ToUpper)) _
                       And If(Filter.DestCityFilter.Trim = String.Empty, True, (d.SolutionDetailDestCity.Trim.ToUpper = Filter.DestCityFilter.Trim.ToUpper)) _
                       And (blnUseOrigStateFilter = False _
                             OrElse (OrigStates.Contains(d.SolutionDetailOrigState))) _
                       And (blnUseDestStateFilter = False _
                             OrElse (DestStates.Contains(d.SolutionDetailDestState))) _
                       And ((Filter.BookTransTypeFilter.Trim Is Nothing OrElse Filter.BookTransTypeFilter.Trim Is String.Empty) _
                             OrElse d.SolutionDetailTransType.ToUpper = Filter.BookTransTypeFilter.ToUpper)
                                   Select d.SolutionDetailPOHdrControl).ToArray()


                    If Not qNewPOs Is Nothing AndAlso qNewPOs.Count > 0 Then intRecordCount = qNewPOs.Count
                Catch ex As Exception
                    'ignore any record count errors when counting the available new bookings
                End Try


                If Filter.PageSize < 1 Then Filter.PageSize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If Filter.Page < 1 Then Filter.Page = 1
                If intRecordCount > Filter.PageSize Then intPageCount = ((intRecordCount - 1) \ Filter.PageSize) + 1
                Dim intSkip As Integer = (Filter.Page - 1) * Filter.PageSize

                'Return all the pohdr records that match the criteria sorted by name
                Dim NewPOs() As DTO.tblSolutionDetail = (
                    From d In db.vNewPOsForSolutions
                    Where
                    (Filter.CompControlFilter = 0 OrElse d.SolutionDetailCompControl = Filter.CompControlFilter) _
                     And If(Filter.UseLoadDateFilter = True,
                                   If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value <= Filter.StopDateFilter, False),
                                   If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value <= Filter.StopDateFilter, False)) _
                       And (d.SolutionDetailOrigZip.Trim >= If(Filter.OrigStartZipFilter = String.Empty, "0", Filter.OrigStartZipFilter)) _
                       And (d.SolutionDetailOrigZip.Trim <= If(Filter.OrigStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.OrigStopZipFilter)) _
                       And (d.SolutionDetailDestZip.Trim >= If(Filter.DestStartZipFilter = String.Empty, "0", Filter.DestStartZipFilter)) _
                       And (d.SolutionDetailDestZip.Trim <= If(Filter.DestStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.DestStopZipFilter)) _
                       And If(Filter.OrigCityFilter.Trim = String.Empty, True, (d.SolutionDetailOrigCity.Trim.ToUpper = Filter.OrigCityFilter.Trim.ToUpper)) _
                       And If(Filter.DestCityFilter.Trim = String.Empty, True, (d.SolutionDetailDestCity.Trim.ToUpper = Filter.DestCityFilter.Trim.ToUpper)) _
                       And (blnUseOrigStateFilter = False _
                             OrElse (OrigStates.Contains(d.SolutionDetailOrigState))) _
                       And (blnUseDestStateFilter = False _
                             OrElse (DestStates.Contains(d.SolutionDetailDestState))) _
                       And ((Filter.BookTransTypeFilter.Trim Is Nothing OrElse Filter.BookTransTypeFilter.Trim Is String.Empty) _
                             OrElse d.SolutionDetailTransType.ToUpper = Filter.BookTransTypeFilter.ToUpper)
                    Order By d.SolutionDetailPOHdrControl Descending
                    Select selectDTOData(d, Filter.Page, intPageCount, intRecordCount, Filter.PageSize)).Skip(intSkip).Take(Filter.PageSize).ToArray()
                Return NewPOs
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewPOsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Friend Function selectDTOData(ByVal d As LTS.vNewPOsForSolution, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionDetail
        Dim oDTO As New DTO.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailUpdated = New Byte() {}
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region

#Region "LTS & 365 Updates"

    ''' <summary>
    ''' Read new PROs from Order Preview 
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.007 on 01/17/2023
    ''' </remarks>
    Public Function GetNewPOsFiltered365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As DTO.tblSolutionDetail()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As DTO.tblSolutionDetail
        Dim oPOData() As LTS.vNewPOsForSolution365
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim leComps As Integer() = db.vLEComp365RefBooks.Where(Function(t) t.LEAdminControl = Parameters.UserLEControl And t.UserSecurityControl = Parameters.UserControl).Select(Function(x) x.CompControl).ToArray()
                If leComps Is Nothing OrElse leComps.Count < 1 Then Return Nothing
                Dim OrigStates As New List(Of String)
                Dim blnUseOrigStateFilter As Boolean = False
                Dim DestStates As New List(Of String)
                Dim blnUseDestStateFilter As Boolean = False
                populateStateListsFromFilter(filters, OrigStates, blnUseOrigStateFilter, DestStates, blnUseDestStateFilter)
                Dim iQuery As IQueryable(Of LTS.vNewPOsForSolution365)
                iQuery = (From t In db.vNewPOsForSolution365s
                          Where (leComps.Contains(t.CompControl)) And (blnUseOrigStateFilter = False OrElse OrigStates.Contains(t.BookOrigState)) And (blnUseDestStateFilter = False OrElse DestStates.Contains(t.BookDestState))
                          Select t)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oPOData = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                oRet = (From d In oPOData Select CopyvNewPOsForSolutionToSolutionDetailData(d)).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewPOsFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' Map vNewPOsForSolution365 to tblSolutionDetail data
    ''' </summary>
    ''' <param name="vNewPOs"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.007 on 01/17/2023
    ''' </remarks>
    Protected Function CopyvNewPOsForSolutionToSolutionDetailData(ByVal vNewPOs As LTS.vNewPOsForSolution365) As DTO.tblSolutionDetail
        Dim oSolutionDetails As New DTO.tblSolutionDetail()
        With vNewPOs
            oSolutionDetails.SolutionDetailControl = 0
            oSolutionDetails.SolutionDetailSolutionTruckControl = .BookCarrTruckControl
            oSolutionDetails.SolutionDetailBookControl = .BookControl
            oSolutionDetails.SolutionDetailPOHdrControl = .POHdrControl
            oSolutionDetails.SolutionDetailBookLoadControl = .BookLoadControl
            oSolutionDetails.SolutionDetailProNumber = .BookProNumber
            oSolutionDetails.SolutionDetailPONumber = .BookLoadPONumber
            oSolutionDetails.SolutionDetailOrderNumber = .BookCarrOrderNumber
            oSolutionDetails.SolutionDetailOrderSequence = .BookOrderSequence
            oSolutionDetails.SolutionDetailCom = .BookLoadCom
            oSolutionDetails.SolutionDetailConsPrefix = .BookConsPrefix
            oSolutionDetails.SolutionDetailCompControl = If(.CompControl, 0)
            oSolutionDetails.SolutionDetailCompNumber = If(.POHDRDefaultCustomer, 0)
            oSolutionDetails.SolutionDetailCompName = .CompName
            oSolutionDetails.SolutionDetailCompNatNumber = If(.CompNatNumber, 0)
            oSolutionDetails.SolutionDetailCompNatName = .CompNatName
            oSolutionDetails.SolutionDetailODControl = If(.LaneControl, 0)
            oSolutionDetails.SolutionDetailCarrierControl = If(.CarrierControl, 0)
            oSolutionDetails.SolutionDetailCarrierNumber = .POHDRDefaultCarrier
            oSolutionDetails.SolutionDetailCarrierName = .CarrierName
            oSolutionDetails.SolutionDetailOrigCompControl = If(.BookOrigCompControl, 0)
            oSolutionDetails.SolutionDetailOrigName = .BookOrigName
            oSolutionDetails.SolutionDetailOrigAddress1 = .BookOrigAddress1
            oSolutionDetails.SolutionDetailOrigAddress2 = .BookOrigAddress2
            oSolutionDetails.SolutionDetailOrigAddress3 = .BookOrigAddress3
            oSolutionDetails.SolutionDetailOrigCity = .BookOrigCity
            oSolutionDetails.SolutionDetailOrigState = .BookOrigState
            oSolutionDetails.SolutionDetailOrigCountry = .BookOrigCountry
            oSolutionDetails.SolutionDetailOrigZip = .BookOrigZip
            oSolutionDetails.SolutionDetailDestCompControl = If(.BookDestCompControl, 0)
            oSolutionDetails.SolutionDetailDestName = .BookDestName
            oSolutionDetails.SolutionDetailDestAddress1 = .BookDestAddress1
            oSolutionDetails.SolutionDetailDestAddress2 = .BookDestAddress2
            oSolutionDetails.SolutionDetailDestAddress3 = .BookDestAddress3
            oSolutionDetails.SolutionDetailDestCity = .BookDestCity
            oSolutionDetails.SolutionDetailDestState = .BookDestState
            oSolutionDetails.SolutionDetailDestCountry = .BookDestCountry
            oSolutionDetails.SolutionDetailDestZip = .BookDestZip
            oSolutionDetails.SolutionDetailDateOrdered = .BookDateOrdered
            oSolutionDetails.SolutionDetailDateLoad = .BookDateLoad
            oSolutionDetails.SolutionDetailDateRequired = .BookDateRequired
            oSolutionDetails.SolutionDetailTotalCases = If(.BookTotalCases, 0)
            oSolutionDetails.SolutionDetailTotalWgt = If(.BookTotalWgt, 0)
            oSolutionDetails.SolutionDetailTotalPL = If(.BookTotalPL, 0)
            oSolutionDetails.SolutionDetailTotalCube = If(.BookTotalCube, 0)
            oSolutionDetails.SolutionDetailTotalPX = .BookTotalPX
            oSolutionDetails.SolutionDetailTotalBFC = If(.BookRevBilledBFC, 0)
            oSolutionDetails.SolutionDetailTranCode = .BookTranCode
            oSolutionDetails.SolutionDetailPayCode = .BookPayCode
            oSolutionDetails.SolutionDetailTypeCode = .BookTypeCode
            oSolutionDetails.SolutionDetailStopNo = .BookStopNo
            oSolutionDetails.SolutionDetailModDate = .BookModDate
            oSolutionDetails.SolutionDetailModUser = .BookModUser
            oSolutionDetails.SolutionDetailMilesFrom = .LaneBenchMiles
            oSolutionDetails.SolutionDetailHoldLoad = .BookHoldLoad
            oSolutionDetails.SolutionDetailTransType = .BookTransType
            oSolutionDetails.SolutionDetailDateRequested = .BookDateRequested
            oSolutionDetails.SolutionDetailCarrierEquipmentCodes = .BookCarrierEquipmentCodes
            oSolutionDetails.SolutionDetailRouteTypeCode = If(.BookRouteTypeCode, 0)
            oSolutionDetails.SolutionDetailIsHazmat = .IsHazmat
            oSolutionDetails.SolutionDetailInbound = .nbound
            oSolutionDetails.SolutionDetailRouteGuideNumber = .BookRouteGuideNumber
            oSolutionDetails.SolutionDetailLaneNumber = .LaneNumber
            oSolutionDetails.SolutionDetailLaneName = .LaneName
            oSolutionDetails.SolutionDetailBookNotes = .BookNotesVisable1
        End With
        Return oSolutionDetails

    End Function

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow records to be added 
        Utilities.SaveAppError("Cannot add data.  Records cannot be created using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow records to be updated 
        Utilities.SaveAppError("Cannot save data.  Records cannot be modified using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow Route Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub


#End Region

End Class

Public Class NGLtblSolutionData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblSolutions
        Me.LinqDB = db
        Me.SourceClass = "NGLtblSolutionData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.tblSolutions
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
        Return GettblSolutionFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetFirstRecord(ByVal LowerControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal FromDate As Date,
                                                 ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                tblSolution = (
                    From d In db.tblSolutions
                    Where (LowerControl = 0 OrElse d.SolutionControl >= LowerControl) _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                    And
                    (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                        If(UseCreateDate,
                            (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                            If(UseCommittedDate,
                                (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                        True
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                    Order By d.SolutionControl
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                     ByVal FKControl As Long,
                                                     ByVal FromDate As Date,
                                                     ByVal ToDate As Date,
                                                     ByVal UseCreateDate As Boolean,
                                                     ByVal UseCommittedDate As Boolean,
                                                     ByVal UseArchivedDate As Boolean,
                                                     ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber


                tblSolution = (
                    From d In db.tblSolutions
                    Where d.SolutionControl < CurrentControl _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                    And
                    (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                        If(UseCreateDate,
                            (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                            If(UseCommittedDate,
                                (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                        True
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                    Order By d.SolutionControl Descending
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetNextRecord(ByVal CurrentControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber


                tblSolution = (
                       From d In db.tblSolutions
                       Where d.SolutionControl > CurrentControl _
                       And
                       (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                       Order By d.SolutionControl
                       Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetLastRecord(ByVal UpperControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If UpperControl <> 0 Then
                    tblSolution = (
                        From d In db.tblSolutions
                        Where d.SolutionControl >= UpperControl _
                        And
                        (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                        Order By d.SolutionControl
                        Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Else
                    tblSolution = (
                        From d In db.tblSolutions
                        Where
                        (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                        Order By d.SolutionControl Descending
                        Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                End If

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSolutionFiltered(ByVal Control As Integer) As DTO.tblSolution
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblSolution As DTO.tblSolution = (
                    From d In db.tblSolutions
                    Where
                        d.SolutionControl = Control
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                    , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                    , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                    , .SolutionName = d.SolutionName _
                                                    , .SolutionDescription = d.SolutionDescription _
                                                    , .SolutionCompControl = d.SolutionCompControl _
                                                    , .SolutionCompNumber = d.SolutionCompNumber _
                                                    , .SolutionCompName = d.SolutionCompName _
                                                    , .SolutionCreateDate = d.SolutionCreateDate _
                                                    , .SolutionTotalCases = d.SolutionTotalCases _
                                                    , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                    , .SolutionTotalPL = d.SolutionTotalPL _
                                                    , .SolutionTotalCube = d.SolutionTotalCube _
                                                    , .SolutionTotalPX = d.SolutionTotalPX _
                                                    , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                    , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                    , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                    , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                    , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                    , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                    , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                    , .SolutionTotalCost = d.SolutionTotalCost _
                                                    , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                    , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                    , .SolutionCommitted = d.SolutionCommitted _
                                                    , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                    , .SolutionArchived = d.SolutionArchived _
                                                    , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                    , .SolutionModDate = d.SolutionModDate _
                                                    , .SolutionModUser = d.SolutionModUser _
                                                    , .SolutionUpdated = d.SolutionUpdated.ToArray()}).First


                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function gettblsolutionsfiltered(ByVal CompanyControl As Integer,
                                          ByVal FromDate As Date,
                                          ByVal ToDate As Date,
                                          ByVal UseCreateDate As Boolean,
                                          ByVal UseCommittedDate As Boolean,
                                          ByVal UseArchivedDate As Boolean,
                                          ByVal NatAccountNumber As Integer,
                                          Optional ByVal page As Integer = 1,
                                          Optional ByVal pagesize As Integer = 1000) As DTO.tblSolution()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'the above writes output to the Immediate window
                'Dim qTest = db.tblSolutions.ToList()

                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                intRecordCount = (From d In db.tblSolutions
                                  Where (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = CompanyControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                                      And
                                      (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                                          If(UseCreateDate,
                                              (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                              If(UseCommittedDate,
                                                  (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                                  (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                                          True
                                          )
                                      ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber)))) _
                                      And
                                      (d.SolutionModUser = Parameters.UserName)
                                  Select d.SolutionControl).Count
                'This query fails because queries with local collections are not supported.
                'Dim tblSolutions() As DTO.tblSolution = ( _
                '    From d In db.tblSolutions _
                '    Where qRet.Contains(d.SolutionControl) _
                '    Order By d.SolutionControl _

                'If Not qRet Is Nothing AndAlso qRet.Count > 0 Then

                'intRecordCount = qRet.Count

                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Dim oControls = qRet.ToList()
                'Return all the records that match the criteria sorted by name
                Dim tblSolutions() As DTO.tblSolution = (
                    From d In db.tblSolutions
                    Where (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = CompanyControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                       And
                       (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                           If(UseCreateDate,
                               (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                               If(UseCommittedDate,
                                   (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                   (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                           True
                           )
                       ) _
                       And
                       (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber)))) _
                       And
                       (d.SolutionModUser = Parameters.UserName)
                    Order By d.SolutionControl
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                    , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                    , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                    , .SolutionName = d.SolutionName _
                                                    , .SolutionDescription = d.SolutionDescription _
                                                    , .SolutionCompControl = d.SolutionCompControl _
                                                    , .SolutionCompNumber = d.SolutionCompNumber _
                                                    , .SolutionCompName = d.SolutionCompName _
                                                    , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                    , .SolutionCompNatName = d.SolutionCompNatName _
                                                    , .SolutionCreateDate = d.SolutionCreateDate _
                                                    , .SolutionTotalCases = d.SolutionTotalCases _
                                                    , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                    , .SolutionTotalPL = d.SolutionTotalPL _
                                                    , .SolutionTotalCube = d.SolutionTotalCube _
                                                    , .SolutionTotalPX = d.SolutionTotalPX _
                                                    , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                    , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                    , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                    , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                    , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                    , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                    , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                    , .SolutionTotalCost = d.SolutionTotalCost _
                                                    , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                    , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                    , .SolutionCommitted = d.SolutionCommitted _
                                                    , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                    , .SolutionArchived = d.SolutionArchived _
                                                    , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                    , .SolutionModDate = d.SolutionModDate _
                                                    , .SolutionModUser = d.SolutionModUser _
                                                    , .SolutionUpdated = d.SolutionUpdated.ToArray() _
                                                    , .Page = page _
                                                    , .Pages = intPageCount _
                                                    , .RecordCount = intRecordCount _
                                                    , .PageSize = pagesize}).Skip(intSkip).Take(pagesize).ToArray()

                Return tblSolutions
                'Else
                'Return Nothing
                'End If

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub UpdateBookFromLoadPlanning_Old(ByVal BookProNumber As String, ByVal dictData As Dictionary(Of String, String))
        Dim strProcName As String = "dbo.spMassUpdateLoadPlanningPro"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookProNumber", BookProNumber)
        Dim intDefault As Integer = 0
        Dim dtDefault As Date
        Dim boolDefault As Boolean
        For Each item In dictData
            Select Case item.Key
                Case "BookDateLoad"
                    If Date.TryParse(item.Value, dtDefault) Then
                        oCmd.Parameters.AddWithValue("@BookDateLoad", dtDefault)
                    End If
                Case "BookDateRequired"
                    If Date.TryParse(item.Value, dtDefault) Then
                        oCmd.Parameters.AddWithValue("@BookDateRequired", dtDefault)
                    End If
                Case "BookCarrActDate"
                    If Date.TryParse(item.Value, dtDefault) Then
                        oCmd.Parameters.AddWithValue("@BookCarrActDate", dtDefault)
                    End If
                Case "BookShipCarrierProNumber"
                    oCmd.Parameters.AddWithValue("@BookShipCarrierProNumber", Left(item.Value, 20))
                Case "BookShipCarrierName"
                    oCmd.Parameters.AddWithValue("@BookShipCarrierName", Left(item.Value, 60))
                Case "BookShipCarrierNumber"
                    oCmd.Parameters.AddWithValue("@BookShipCarrierNumber", Left(item.Value, 80))
                Case "BookCustomerApprovalTransmitted"
                    If Boolean.TryParse(item.Value, boolDefault) Then
                        oCmd.Parameters.AddWithValue("@BookCustomerApprovalTransmitted", If(boolDefault = True, "1", "0"))
                    End If
                Case "BookCustomerApprovalRecieved"
                    If Boolean.TryParse(item.Value, boolDefault) Then
                        oCmd.Parameters.AddWithValue("@BookCustomerApprovalRecieved", If(boolDefault = True, "1", "0"))
                    End If
                Case "BookCarrTrailerNo"
                    oCmd.Parameters.AddWithValue("@BookCarrTrailerNo", Left(item.Value, 50))
                Case "BookCarrSealNo"
                    oCmd.Parameters.AddWithValue("@BookCarrSealNo", Left(item.Value, 50))
                Case "BookCarrDriverNo"
                    oCmd.Parameters.AddWithValue("@BookCarrDriverNo", Left(item.Value, 50))
                Case "BookCarrDriverName"
                    oCmd.Parameters.AddWithValue("@BookCarrDriverName", Left(item.Value, 50))
                Case "BookCarrRouteNo"
                    oCmd.Parameters.AddWithValue("@BookCarrRouteNo", Left(item.Value, 50))
                Case "BookCarrTripNo"
                    oCmd.Parameters.AddWithValue("@BookCarrTripNo", Left(item.Value, 50))
                Case "BookTrackContact"
                    oCmd.Parameters.AddWithValue("@BookTrackContact", Left(item.Value, 50))
                Case "BookTrackComment"
                    oCmd.Parameters.AddWithValue("@BookTrackComment", Left(item.Value, 255))
                Case "BookTrackStatus"
                    oCmd.Parameters.AddWithValue("@BookTrackStatus", item.Value)
                Case "BookRouteConsFlag"
                    If Boolean.TryParse(item.Value, boolDefault) Then
                        oCmd.Parameters.AddWithValue("@BookRouteConsFlag", If(boolDefault = True, "1", "0"))
                    End If
                Case "BookShipCarrierProNumberRaw"
                    oCmd.Parameters.AddWithValue("@BookShipCarrierProNumberRaw", Left(item.Value, 20))
                Case "BookShipCarrierProControl"
                    If Integer.TryParse(item.Value, intDefault) Then
                        oCmd.Parameters.AddWithValue("@BookShipCarrierProControl", intDefault)
                    End If
                Case "BookRouteTypeCode"
                    oCmd.Parameters.AddWithValue("@BookRouteTypeCode", item.Value)
                Case "BookCarrBLNumber"
                    oCmd.Parameters.AddWithValue("@BookCarrBLNumber", Left(item.Value, 20))
            End Select
        Next
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub


    Public Function UpdateBookFromLoadPlanning(ByVal BookProNumber As String, ByVal dictData As Dictionary(Of String, String)) As String
        Dim strMessage As String = ""
        Dim strDetails As String = ""
        Dim strSolutionTruckKey As String = ""
        Dim BookDateLoad As Date?
        Dim BookDateRequired As Date?
        Dim BookCarrActDate As Date?
        Dim BookShipCarrierProNumber As String = Nothing 'NVARCHAR (20) = NULL, 
        Dim BookShipCarrierName As String = Nothing ' NVARCHAR (60) = NULL, 
        Dim BookShipCarrierNumber As String = Nothing ' NVARCHAR (80) = NULL, 
        Dim BookCustomerApprovalTransmitted As Boolean?
        Dim BookCustomerApprovalRecieved As Boolean?
        Dim BookCarrTrailerNo As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookCarrSealNo As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookCarrDriverNo As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookCarrDriverName As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookCarrRouteNo As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookCarrTripNo As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookTrackContact As String = Nothing ' NVARCHAR (50) = NULL, 
        Dim BookTrackComment As String = Nothing ' NVARCHAR (255) = NULL, 
        Dim BookTrackStatus As Integer?
        Dim BookRouteConsFlag As Boolean?
        Dim BookShipCarrierProNumberRaw As String = Nothing ' nvarchar(20) = NULL,
        Dim BookShipCarrierProControl As Integer?
        Dim BookRouteTypeCode As Integer?
        Dim BookCarrBLNumber As String = Nothing ' NVARCHAR(20) = Null,
        Dim intDefault As Integer = 0
        Dim dtDefault As Date
        Dim boolDefault As Boolean
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                For Each item In dictData
                    Select Case item.Key
                        Case "BookDateLoad"
                            If Date.TryParse(item.Value, dtDefault) Then BookDateLoad = dtDefault
                        Case "BookDateRequired"
                            If Date.TryParse(item.Value, dtDefault) Then BookDateRequired = dtDefault
                        Case "BookCarrActDate"
                            If Date.TryParse(item.Value, dtDefault) Then BookCarrActDate = dtDefault
                        Case "BookShipCarrierProNumber"
                            BookShipCarrierProNumber = Left(item.Value, 20)
                        Case "BookShipCarrierName"
                            BookShipCarrierName = Left(item.Value, 60)
                        Case "BookShipCarrierNumber"
                            BookShipCarrierNumber = Left(item.Value, 80)
                        Case "BookCustomerApprovalTransmitted"
                            If Boolean.TryParse(item.Value, boolDefault) Then BookCustomerApprovalTransmitted = boolDefault
                        Case "BookCustomerApprovalRecieved"
                            If Boolean.TryParse(item.Value, boolDefault) Then BookCustomerApprovalRecieved = boolDefault
                        Case "BookCarrTrailerNo"
                            BookCarrTrailerNo = Left(item.Value, 50)
                        Case "BookCarrSealNo"
                            BookCarrSealNo = Left(item.Value, 50)
                        Case "BookCarrDriverNo"
                            BookCarrDriverNo = Left(item.Value, 50)
                        Case "BookCarrDriverName"
                            BookCarrDriverName = Left(item.Value, 50)
                        Case "BookCarrRouteNo"
                            BookCarrRouteNo = Left(item.Value, 50)
                        Case "BookCarrTripNo"
                            BookCarrTripNo = Left(item.Value, 50)
                        Case "BookTrackContact"
                            BookTrackContact = Left(item.Value, 50)
                        Case "BookTrackComment"
                            BookTrackComment = Left(item.Value, 255)
                        Case "BookTrackStatus"
                            If Integer.TryParse(item.Value, intDefault) Then BookTrackStatus = intDefault
                        Case "BookRouteConsFlag"
                            If Boolean.TryParse(item.Value, boolDefault) Then BookRouteConsFlag = boolDefault
                        Case "BookShipCarrierProNumberRaw"
                            BookShipCarrierProNumberRaw = Left(item.Value, 20)
                        Case "BookShipCarrierProControl"
                            If Integer.TryParse(item.Value, intDefault) Then BookShipCarrierProControl = intDefault
                        Case "BookRouteTypeCode"
                            If Integer.TryParse(item.Value, intDefault) Then BookRouteTypeCode = intDefault
                        Case "BookCarrBLNumber"
                            BookCarrBLNumber = Left(item.Value, 20)
                    End Select
                Next
                Dim oResult = db.spUpdateBookFromLoadPlanning(BookProNumber, BookDateLoad, BookDateRequired, BookCarrActDate, BookShipCarrierProNumber, BookShipCarrierName, BookShipCarrierNumber, BookCustomerApprovalTransmitted, BookCustomerApprovalRecieved, BookCarrTrailerNo, BookCarrSealNo, BookCarrDriverNo, BookCarrDriverName, BookCarrRouteNo, BookCarrTripNo, BookTrackContact, BookTrackComment, BookTrackStatus, BookRouteConsFlag, BookShipCarrierProNumber, BookShipCarrierProControl, BookRouteTypeCode, BookCarrBLNumber, Me.Parameters.UserName).FirstOrDefault()
                If Not oResult Is Nothing Then
                    strSolutionTruckKey = oResult.SolutionTruckKey
                    If oResult.ErrNumber > 0 And oResult.ErrNumber < 10 Then
                        'NGL customer error message
                        strMessage = "E_DataValidationFailure"
                        strDetails = oResult.RetMsg
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_ProcessProcedureFailure"))
                    ElseIf oResult.ErrNumber > 9 Then
                        'SQL error message
                        throwSQLFaultException(oResult.RetMsg)
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookFromLoadPlanning"))
            End Try
        End Using
        Return strSolutionTruckKey
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSolution)
        'Create New Record
        Return New LTS.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = If(d.SolutionCreateDate.HasValue, d.SolutionCreateDate, Date.Now) _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = Date.Now _
                                                        , .SolutionModUser = Parameters.UserName _
                                                        , .SolutionUpdated = If(d.SolutionUpdated Is Nothing, New Byte() {}, d.SolutionUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblSolutionFiltered(Control:=CType(LinqTable, LTS.tblSolution).SolutionControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSolution = TryCast(LinqTable, LTS.tblSolution)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSolutions
                       Where d.SolutionControl = source.SolutionControl
                       Select New DTO.QuickSaveResults With {.Control = d.SolutionControl _
                                                                , .ModDate = d.SolutionModDate _
                                                                , .ModUser = d.SolutionModUser _
                                                                , .Updated = d.SolutionUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLtblSolutionTruckData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblSolutionTrucks
        Me.LinqDB = db
        Me.SourceClass = "NGLtblSolutionTruckData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.tblSolutionTrucks
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
        Return GettblSolutionTruckFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced. 
    ''' The FKControl parameter is a reference to the Solution Control Number.
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetFirstRecord(ByVal LowerControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionTruck As New DTO.tblSolutionTruck
                tblSolutionTruck = (
                    From d In db.tblSolutionTrucks
                    Where (LowerControl = 0 OrElse d.SolutionTruckControl >= LowerControl) _
                    And
                    (d.SolutionTruckSolutionControl = FKControl) _
                    And
                    (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                    Order By d.SolutionTruckControl
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl parameter is the seed for the previous record.
    ''' The FKControl parameter is a reference to the Solution Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                    ByVal FKControl As Long,
                                                    ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionTruck As New DTO.tblSolutionTruck
                tblSolutionTruck = (
                    From d In db.tblSolutionTrucks
                    Where d.SolutionTruckControl < CurrentControl _
                    And
                    (d.SolutionTruckSolutionControl = FKControl) _
                    And
                    (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                    Order By d.SolutionTruckControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced.
    ''' The FKControl parameter is a reference to the Solution Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetNextRecord(ByVal CurrentControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionTruck As New DTO.tblSolutionTruck
                tblSolutionTruck = (
                   From d In db.tblSolutionTrucks
                   Where d.SolutionTruckControl > CurrentControl _
                    And
                    (d.SolutionTruckSolutionControl = FKControl) _
                    And
                    (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                   Order By d.SolutionTruckControl
                   Select selectDTOData(d, db)).FirstOrDefault()
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be providced.
    ''' The FKControl parameter is a reference to the Solution Truck Control Number.
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetLastRecord(ByVal UpperControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionTruck As New DTO.tblSolutionTruck
                If UpperControl <> 0 Then
                    tblSolutionTruck = (
                        From d In db.tblSolutionTrucks
                        Where d.SolutionTruckControl >= UpperControl _
                        And
                        (d.SolutionTruckSolutionControl = FKControl) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                        Order By d.SolutionTruckControl
                        Select selectDTOData(d, db)).FirstOrDefault()
                Else
                    tblSolutionTruck = (
                        From d In db.tblSolutionTrucks
                        Where (d.SolutionTruckSolutionControl = FKControl) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                        Order By d.SolutionTruckControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
                End If
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSolutionTruckFiltered(ByVal Control As Long) As DTO.tblSolutionTruck
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblSolutionTruck As DTO.tblSolutionTruck = (
                    From d In db.tblSolutionTrucks
                    Where
                        d.SolutionTruckControl = Control
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSolutionTruckFiltered"))
            End Try

            Return Nothing

        End Using
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SolutionControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 8/11/2016
    '''   added skip and take parameters to support
    '''   interface with TMS 365 pages
    ''' </remarks>
    Public Function GettblSolutionTrucksFiltered(ByVal SolutionControl As Long,
                                                ByVal RouteTypeCode As Integer,
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000,
                                                Optional ByVal skip As Integer = 0,
                                                Optional ByVal take As Integer = 0) As DTO.tblSolutionTruck()

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                intRecordCount = (From d In db.tblSolutionTrucks
                                  Where (d.SolutionTruckSolutionControl = SolutionControl) _
                                      And
                                      (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                                  Select d.SolutionTruckControl).Count

                If skip <> 0 And take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1


                'Return all the records that match the criteria sorted by name
                Dim tblSolutionTrucks() As DTO.tblSolutionTruck = (
                    From d In db.tblSolutionTrucks
                    Where (d.SolutionTruckSolutionControl = SolutionControl) _
                        And
                        (RouteTypeCode = 0 OrElse d.SolutionTruckRouteTypeCode = RouteTypeCode)
                    Order By d.SolutionTruckControl Descending
                    Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(skip).Take(pagesize).ToArray()

                Return tblSolutionTrucks


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("gettblSolutionTrucksFiltered"))
            End Try

            Return Nothing

        End Using
    End Function
    Friend Shared Function selectDTOData(ByVal d As LTS.tblSolutionTruck, ByVal db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckUpdated", "SolutionTruckStaticRouteControl", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckUpdated = d.SolutionTruckUpdated.ToArray()
            .SolutionTruckStaticRouteControl = db.udfGetStaticRouteNumber(d.SolutionTruckStaticRouteControl)
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSolutionTruck)
        Dim skipObjs As New List(Of String) From {"SolutionTruckUpdated", "SolutionTruckModDate", "SolutionTruckModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.tblSolutionTruck
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .SolutionTruckModDate = Date.Now
            .SolutionTruckModUser = Me.Parameters.UserName
            .SolutionTruckUpdated = If(d.SolutionTruckUpdated Is Nothing, New Byte() {}, d.SolutionTruckUpdated)
        End With
        Return oLTS

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblSolutionTruckFiltered(Control:=CType(LinqTable, LTS.tblSolutionTruck).SolutionTruckControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults

        Dim ret As New DTO.QuickSaveResults()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSolutionTruck = TryCast(LinqTable, LTS.tblSolutionTruck)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSolutionTrucks
                       Where d.SolutionTruckControl = source.SolutionTruckControl
                       Select New DTO.QuickSaveResults With {.Control = d.SolutionTruckControl _
                                                                , .ModDate = d.SolutionTruckModDate _
                                                                , .ModUser = d.SolutionTruckModUser _
                                                                , .Updated = d.SolutionTruckUpdated.ToArray}).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function


#End Region

End Class

Public Class NGLtblSolutionDetailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblSolutionDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLtblSolutionDetailData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.tblSolutionDetails
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
        Return GettblSolutionDetailFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced. 
    ''' The FKControl parameter is a reference to the Solution Truck Control Number.
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long,
                                                 ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionDetail As New DTO.tblSolutionDetail
                tblSolutionDetail = (
                    From d In db.tblSolutionDetails
                    Where (LowerControl = 0 OrElse d.SolutionDetailControl >= LowerControl) _
                    And
                    (d.SolutionDetailSolutionTruckControl = FKControl)
                    Order By d.SolutionDetailControl
                    Select selectDTOData(d)).FirstOrDefault()

                Return tblSolutionDetail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl parameter is the seed for the previous record.
    ''' The FKControl parameter is a reference to the Solution Truck Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                    ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionDetail As New DTO.tblSolutionDetail
                tblSolutionDetail = (
                    From d In db.tblSolutionDetails
                    Where d.SolutionDetailControl < CurrentControl _
                    And
                    (d.SolutionDetailSolutionTruckControl = FKControl)
                    Order By d.SolutionDetailBookControl Descending
                    Select selectDTOData(d)).FirstOrDefault()
                Return tblSolutionDetail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced.
    ''' The FKControl parameter is a reference to the Solution Truck Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long,
                                                 ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'For the test we use the book table and the bookload data is ignored.  
                'The final query will need to use a view that selects the book load data because
                'We need the temperature.
                Dim tblSolutionDetail As New DTO.tblSolutionDetail
                tblSolutionDetail = (
                   From d In db.tblSolutionDetails
                   Where d.SolutionDetailControl > CurrentControl _
                    And
                    (d.SolutionDetailSolutionTruckControl = FKControl)
                   Order By d.SolutionDetailBookControl
                   Select selectDTOData(d)).FirstOrDefault()
                Return tblSolutionDetail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be providced.
    ''' The FKControl parameter is a reference to the Solution Truck Control Number.
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long,
                                                 ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolutionDetail As New DTO.tblSolutionDetail
                If UpperControl <> 0 Then
                    tblSolutionDetail = (
                        From d In db.tblSolutionDetails
                        Where d.SolutionDetailControl >= UpperControl _
                        And
                        (d.SolutionDetailSolutionTruckControl = FKControl)
                        Order By d.SolutionDetailBookControl
                        Select selectDTOData(d)).FirstOrDefault()
                Else
                    tblSolutionDetail = (
                        From d In db.tblSolutionDetails
                        Where (d.SolutionDetailSolutionTruckControl = FKControl)
                        Order By d.SolutionDetailBookControl Descending
                        Select selectDTOData(d)).FirstOrDefault()
                End If
                Return tblSolutionDetail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblSolutionDetailFiltered(ByVal Control As Long) As DTO.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblSolutionDetail As DTO.tblSolutionDetail = (
                    From d In db.tblSolutionDetails
                    Where
                        d.SolutionDetailControl = Control
                    Select selectDTOData(d)).FirstOrDefault()
                Return tblSolutionDetail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSolutionDetailFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblSolutionDetailsFiltered(ByVal SolutionTruckControl As Long,
                                               Optional ByVal page As Integer = 1,
                                               Optional ByVal pagesize As Integer = 1000) As DTO.tblSolutionDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                intRecordCount = db.tblSolutionDetails.Count(Function(x) x.SolutionDetailSolutionTruckControl = SolutionTruckControl)
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the records that match the criteria sorted by name
                Dim tblSolutionDetails() As DTO.tblSolutionDetail = (
                    From d In db.tblSolutionDetails
                    Where (d.SolutionDetailSolutionTruckControl = SolutionTruckControl)
                    Order By d.SolutionDetailControl Descending
                    Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblSolutionDetails
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblSolutionDetailsFiltered"))
            End Try
            Return Nothing

        End Using
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.tblSolutionDetail, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionDetail
        Dim oDTO As New DTO.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailUpdated = d.SolutionDetailUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSolutionDetail)
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "SolutionDetailBookModDate", "SolutionDetailBookModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.tblSolutionDetail
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .SolutionDetailBookModDate = Date.Now
            .SolutionDetailBookModUser = Me.Parameters.UserName
            .SolutionDetailUpdated = If(d.SolutionDetailUpdated Is Nothing, New Byte() {}, d.SolutionDetailUpdated)
        End With
        Return oLTS
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblSolutionDetailFiltered(Control:=CType(LinqTable, LTS.tblSolutionDetail).SolutionDetailControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults

        Dim ret As New DTO.QuickSaveResults()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSolutionDetail = TryCast(LinqTable, LTS.tblSolutionDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSolutionDetails
                       Where d.SolutionDetailControl = source.SolutionDetailControl
                       Select New DTO.QuickSaveResults With {.Control = d.SolutionDetailControl _
                                                                , .ModDate = d.SolutionDetailModDate _
                                                                , .ModUser = d.SolutionDetailModUser _
                                                                , .Updated = d.SolutionDetailUpdated.ToArray}).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function


#End Region

End Class

Public Class NGLLoadPlanningData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblSolutions
        Me.LinqDB = db
        Me.SourceClass = "NGLLoadPlanningData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.tblSolutions
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
        Return GettblSolutionFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetFirstRecord(ByVal LowerControl As Long,
                                                 ByVal FKControl As Long,
                                                 ByVal FromDate As Date,
                                                 ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                tblSolution = (
                    From d In db.tblSolutions
                    Where (LowerControl = 0 OrElse d.SolutionControl >= LowerControl) _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                    And
                    (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                        If(UseCreateDate,
                            (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                            If(UseCommittedDate,
                                (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                        True
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                    Order By d.SolutionControl
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                     ByVal FKControl As Long,
                                                     ByVal FromDate As Date,
                                                     ByVal ToDate As Date,
                                                     ByVal UseCreateDate As Boolean,
                                                     ByVal UseCommittedDate As Boolean,
                                                     ByVal UseArchivedDate As Boolean,
                                                     ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber


                tblSolution = (
                    From d In db.tblSolutions
                    Where d.SolutionControl < CurrentControl _
                    And
                    (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                    And
                    (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                        If(UseCreateDate,
                            (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                            If(UseCommittedDate,
                                (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                        True
                        )
                    ) _
                    And
                    (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                    Order By d.SolutionControl Descending
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl is the seed control value.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetNextRecord(ByVal CurrentControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber


                tblSolution = (
                       From d In db.tblSolutions
                       Where d.SolutionControl > CurrentControl _
                       And
                       (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                       Order By d.SolutionControl
                       Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be provided.
    ''' The FKControl parameter is a reference to the Comp Control Number.  If a NatAccountNumber is provided the 
    ''' FKcontrol is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseCreateDate"></param>
    ''' <param name="UseCommittedDate"></param>
    ''' <param name="UseArchivedDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetLastRecord(ByVal UpperControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                 ByVal UseCreateDate As Boolean,
                                                 ByVal UseCommittedDate As Boolean,
                                                 ByVal UseArchivedDate As Boolean,
                                                 ByVal NatAccountNumber As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim tblSolution As New DTO.tblSolution
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If UpperControl <> 0 Then
                    tblSolution = (
                        From d In db.tblSolutions
                        Where d.SolutionControl >= UpperControl _
                        And
                        (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                        Order By d.SolutionControl
                        Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                Else
                    tblSolution = (
                        From d In db.tblSolutions
                        Where
                        (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber))))
                        Order By d.SolutionControl Descending
                        Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = d.SolutionCreateDate _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = d.SolutionModDate _
                                                        , .SolutionModUser = d.SolutionModUser _
                                                        , .SolutionUpdated = d.SolutionUpdated.ToArray()}).FirstOrDefault

                End If

                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSolutionFiltered(ByVal Control As Integer) As DTO.tblSolution
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblSolution As DTO.tblSolution = (
                    From d In db.tblSolutions
                    Where
                        d.SolutionControl = Control
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                    , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                    , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                    , .SolutionName = d.SolutionName _
                                                    , .SolutionDescription = d.SolutionDescription _
                                                    , .SolutionCompControl = d.SolutionCompControl _
                                                    , .SolutionCompNumber = d.SolutionCompNumber _
                                                    , .SolutionCompName = d.SolutionCompName _
                                                    , .SolutionCreateDate = d.SolutionCreateDate _
                                                    , .SolutionTotalCases = d.SolutionTotalCases _
                                                    , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                    , .SolutionTotalPL = d.SolutionTotalPL _
                                                    , .SolutionTotalCube = d.SolutionTotalCube _
                                                    , .SolutionTotalPX = d.SolutionTotalPX _
                                                    , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                    , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                    , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                    , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                    , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                    , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                    , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                    , .SolutionTotalCost = d.SolutionTotalCost _
                                                    , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                    , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                    , .SolutionCommitted = d.SolutionCommitted _
                                                    , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                    , .SolutionArchived = d.SolutionArchived _
                                                    , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                    , .SolutionModDate = d.SolutionModDate _
                                                    , .SolutionModUser = d.SolutionModUser _
                                                    , .SolutionUpdated = d.SolutionUpdated.ToArray()}).First


                Return tblSolution

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblSolutionsFiltered(ByVal CompanyControl As Integer,
                                          ByVal FromDate As Date,
                                          ByVal ToDate As Date,
                                          ByVal UseCreateDate As Boolean,
                                          ByVal UseCommittedDate As Boolean,
                                          ByVal UseArchivedDate As Boolean,
                                          ByVal NatAccountNumber As Integer,
                                          Optional ByVal page As Integer = 1,
                                          Optional ByVal pagesize As Integer = 1000) As DTO.tblSolution()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                intRecordCount = (From d In db.tblSolutions
                                  Where (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = CompanyControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                                      And
                                      (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                                          If(UseCreateDate,
                                              (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                              If(UseCommittedDate,
                                                  (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                                  (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                                          True
                                          )
                                      ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber)))) _
                                      And
                                      (d.SolutionModUser = Parameters.UserName)
                                  Select d.SolutionControl).Count

                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the records that match the criteria sorted by name
                Dim tblSolutions() As DTO.tblSolution = (
                    From d In db.tblSolutions
                    Where (NatAccountNumber <> 0 OrElse (d.SolutionCompControl = CompanyControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionCompControl)))) _
                        And
                        (If(UseCreateDate Or UseCommittedDate Or UseArchivedDate,
                            If(UseCreateDate,
                                (d.SolutionCreateDate >= FromDate And d.SolutionCreateDate < ToDate.AddDays(1)),
                                If(UseCommittedDate,
                                    (d.SolutionCommittedDate >= FromDate And d.SolutionCommittedDate < ToDate.AddDays(1)),
                                    (d.SolutionArchivedDate >= FromDate And d.SolutionArchivedDate < ToDate.AddDays(1)))),
                            True
                            )
                        ) _
                        And
                        (NatAccountNumber = 0 OrElse (d.SolutionCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionCompNatNumber)))) _
                        And
                        (d.SolutionModUser = Parameters.UserName)
                    Order By d.SolutionControl
                    Select New DTO.tblSolution With {.SolutionControl = d.SolutionControl _
                                                    , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                    , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                    , .SolutionName = d.SolutionName _
                                                    , .SolutionDescription = d.SolutionDescription _
                                                    , .SolutionCompControl = d.SolutionCompControl _
                                                    , .SolutionCompNumber = d.SolutionCompNumber _
                                                    , .SolutionCompName = d.SolutionCompName _
                                                    , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                    , .SolutionCompNatName = d.SolutionCompNatName _
                                                    , .SolutionCreateDate = d.SolutionCreateDate _
                                                    , .SolutionTotalCases = d.SolutionTotalCases _
                                                    , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                    , .SolutionTotalPL = d.SolutionTotalPL _
                                                    , .SolutionTotalCube = d.SolutionTotalCube _
                                                    , .SolutionTotalPX = d.SolutionTotalPX _
                                                    , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                    , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                    , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                    , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                    , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                    , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                    , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                    , .SolutionTotalCost = d.SolutionTotalCost _
                                                    , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                    , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                    , .SolutionCommitted = d.SolutionCommitted _
                                                    , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                    , .SolutionArchived = d.SolutionArchived _
                                                    , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                    , .SolutionModDate = d.SolutionModDate _
                                                    , .SolutionModUser = d.SolutionModUser _
                                                    , .SolutionUpdated = d.SolutionUpdated.ToArray() _
                                                    , .Page = page _
                                                    , .Pages = intPageCount _
                                                    , .RecordCount = intRecordCount _
                                                    , .PageSize = pagesize}).Skip(intSkip).Take(pagesize).ToArray()
                Return tblSolutions

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSolution)
        If d.SolutionAttributeTypeControl = 0 Then
            Try
                'try to get the default attrubute type control for this data object
                d.SolutionAttributeTypeControl = New NGLLookupDataProvider(Me.Parameters).GetAttributeTypeControl("tblSolution", "Load Planning Solution Attributes")
            Catch ex As FaultException
                'ignore any fault exceptions just leave the value at zero
            Catch ex As Exception
                Throw
            End Try
        End If
        'Create New Record
        Return New LTS.tblSolution With {.SolutionControl = d.SolutionControl _
                                                        , .SolutionAttributeControl = d.SolutionAttributeControl _
                                                        , .SolutionAttributeTypeControl = d.SolutionAttributeTypeControl _
                                                        , .SolutionName = d.SolutionName _
                                                        , .SolutionDescription = d.SolutionDescription _
                                                        , .SolutionCompControl = d.SolutionCompControl _
                                                        , .SolutionCompNumber = d.SolutionCompNumber _
                                                        , .SolutionCompName = d.SolutionCompName _
                                                        , .SolutionCompNatNumber = d.SolutionCompNatNumber _
                                                        , .SolutionCompNatName = d.SolutionCompNatName _
                                                        , .SolutionCreateDate = If(d.SolutionCreateDate.HasValue, d.SolutionCreateDate, Date.Now) _
                                                        , .SolutionTotalCases = d.SolutionTotalCases _
                                                        , .SolutionTotalWgt = d.SolutionTotalWgt _
                                                        , .SolutionTotalPL = d.SolutionTotalPL _
                                                        , .SolutionTotalCube = d.SolutionTotalCube _
                                                        , .SolutionTotalPX = d.SolutionTotalPX _
                                                        , .SolutionTotalBFC = d.SolutionTotalBFC _
                                                        , .SolutionTotalOrders = d.SolutionTotalOrders _
                                                        , .SolutionTotalTLCost = d.SolutionTotalTLCost _
                                                        , .SolutionTotalMPCost = d.SolutionTotalMPCost _
                                                        , .SolutionTotalPoolCost = d.SolutionTotalPoolCost _
                                                        , .SolutionTotalLTLPoolCost = d.SolutionTotalLTLPoolCost _
                                                        , .SolutionTotalLTLCost = d.SolutionTotalLTLCost _
                                                        , .SolutionTotalCost = d.SolutionTotalCost _
                                                        , .SolutionTotalTrucks = d.SolutionTotalTrucks _
                                                        , .SolutionTotalMiles = d.SolutionTotalMiles _
                                                        , .SolutionCommitted = d.SolutionCommitted _
                                                        , .SolutionCommittedDate = d.SolutionCommittedDate _
                                                        , .SolutionArchived = d.SolutionArchived _
                                                        , .SolutionArchivedDate = d.SolutionArchivedDate _
                                                        , .SolutionModDate = Date.Now _
                                                        , .SolutionModUser = Parameters.UserName _
                                                        , .SolutionUpdated = If(d.SolutionUpdated Is Nothing, New Byte() {}, d.SolutionUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblSolutionFiltered(Control:=CType(LinqTable, LTS.tblSolution).SolutionControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSolution = TryCast(LinqTable, LTS.tblSolution)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSolutions
                       Where d.SolutionControl = source.SolutionControl
                       Select New DTO.QuickSaveResults With {.Control = d.SolutionControl _
                                                                , .ModDate = d.SolutionModDate _
                                                                , .ModUser = d.SolutionModUser _
                                                                , .Updated = d.SolutionUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLLoadPlanningTruckData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblSolutionTrucks
        Me.LinqDB = db
        Me.SourceClass = "NGLLoadPlanningTruckData"
    End Sub

#End Region

#Region " Properties "

    Public Shared mSharedLoadsBeingRouted As Boolean = False
    Public Shared mSharedLastOrderNumber As String = "New Process No Order Number"
    Public Shared mSharedOrdersToRoute As New List(Of DTO.tblSolutionDetail)
    Public Shared mSharedPadLock As New Object

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.tblSolutionTrucks
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _TempCNSSeed As Integer = 0
    ''' <summary>
    ''' Generates a temporary CNS number seed value
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.x on 04/15/2021
    ''' </remarks>
    Public ReadOnly Property TempCNSSeed As String
        Get
            If _TempCNSSeed > 40000000 Then
                _TempCNSSeed = 0
            End If
            _TempCNSSeed += 1
            Return _TempCNSSeed.ToString
        End Get
    End Property

    Private _OrdersNotLoaded As List(Of DTO.tblSolutionDetail)
    Public Property OrdersNotLoaded As List(Of DTO.tblSolutionDetail)
        Get
            If _OrdersNotLoaded Is Nothing Then _OrdersNotLoaded = New List(Of DTO.tblSolutionDetail)
            Return _OrdersNotLoaded
        End Get
        Set(value As List(Of DTO.tblSolutionDetail))
            _OrdersNotLoaded = value
        End Set
    End Property

    Private _TrucksReadyToTender As List(Of DTO.tblSolutionTruck)
    Public Property TrucksReadyToTender As List(Of DTO.tblSolutionTruck)
        Get
            Return _TrucksReadyToTender
        End Get
        Set(value As List(Of DTO.tblSolutionTruck))
            _TrucksReadyToTender = value
        End Set
    End Property

    Private _ListAvailableEquipment As New List(Of DTO.CarriersForRoute)
    Public Property ListAvailableEquipment As List(Of DTO.CarriersForRoute)
        Get
            If _ListAvailableEquipment Is Nothing Then _ListAvailableEquipment = New List(Of DTO.CarriersForRoute)
            Return _ListAvailableEquipment
        End Get
        Set(value As List(Of DTO.CarriersForRoute))
            _ListAvailableEquipment = value
        End Set
    End Property

    Private _StaticRouteData As DTO.tblStaticRoute
    Public Property StaticRouteData As DTO.tblStaticRoute
        Get
            Return _StaticRouteData
        End Get
        Set(value As DTO.tblStaticRoute)
            _StaticRouteData = value
        End Set
    End Property

    'Public gblnFillLargestFirst As Boolean = False
    'Public ListAvailableEquipment As New List(Of clsTruck2)
    'Public gblnOptimizeCapacity As Boolean = False
#End Region

#Region "Delegates"

    Public Delegate Sub ProcessDataDelegate(ByRef lOrders As List(Of DTO.tblSolutionDetail), ByVal OptimizeCapacity As Boolean)
    Public Delegate Sub ProcessUnRoutedDataDelegate(ByRef lOrders As List(Of DTO.tblSolutionDetail))

    'Delegate Function
    Public Delegate Sub SaveLoadPlanningTruckMessagesDelegate(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal Messages As Dictionary(Of String, List(Of DTO.NGLMessage)))
    Public Delegate Sub DeleteLoadPlanningTruckMessagesDelegate(ByVal CompControl As Integer, ByVal TruckKey As String)
    Public Delegate Sub ClearLoadPlanningTruckCostOrMilesDelegate(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal ClearCosts As Boolean, ByVal ClearMiles As Boolean)

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetLoadPlanningTruckFiltered(ByVal CompControl As Integer, ByVal TruckKey As String, Optional ByVal blnReadNGLMessages As Boolean = True) As DTO.tblSolutionTruck
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the first record that matches the criteria in TruckFilter
                Dim tblSolutionTruck As DTO.tblSolutionTruck = (
                    From d In db.spGetLoadPlanningTruckRecord(CompControl, TruckKey)
                    Select selectDTOData(d, db, CompControl)).FirstOrDefault()
                If Not tblSolutionTruck Is Nothing AndAlso Not String.IsNullOrWhiteSpace(tblSolutionTruck.SolutionTruckConsPrefix) Then
                    'get the details
                    Dim Details As List(Of DTO.tblSolutionDetail) = (
                        From d In db.spGetLoadPlanningBookData(tblSolutionTruck.SolutionTruckConsPrefix, tblSolutionTruck.SolutionTruckRouteConsFlag, tblSolutionTruck.SolutionTruckCarrierTruckControl, tblSolutionTruck.SolutionTruckCarrierControl)
                        Select selectDTODetailData(d)).ToList()
                    If Not Details Is Nothing Then tblSolutionTruck.SolutionDetails = Details
                    If blnReadNGLMessages Then
                        'Notes:
                        'database key NMMTRefControl maps to CompControl
                        'database key NMMTRefAlphaControl maps to TruckKey
                        Dim oMessages = db.tblNGLMessageRefBooks.Where(Function(x) x.NMNMTControl = Utilities.NGLMessageKeyRef.LoadPlanningTruck And x.NMMTRefControl = CompControl And x.NMMTRefAlphaControl = TruckKey).ToList()
                        If Not oMessages Is Nothing AndAlso oMessages.Count() > 0 Then
                            tblSolutionTruck.addMessages(oMessages)
                        End If
                    End If
                End If
                Return tblSolutionTruck
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadPlanningTruckFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLoadPlanningTrucksFiltered(ByVal TruckFilter As DTO.LoadPlanningTruckDataFilter, Optional ByVal blnReadNGLMessages As Boolean = True) As DTO.tblSolutionTruck()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria in TruckFilter
                Dim tblSolutionTrucks() As DTO.tblSolutionTruck = (
                    From d In db.spGetLoadPlanningTruckData(TruckFilter.CompControlFilter, TruckFilter.CarrierControlFilter, TruckFilter.StartDateFilter, TruckFilter.StopDateFilter, TruckFilter.OrigStartZipFilter, TruckFilter.OrigStopZipFilter, TruckFilter.DestStartZipFilter, TruckFilter.DestStopZipFilter, TruckFilter.OrigCityFilter, TruckFilter.DestCityFilter, TruckFilter.OrigSt1Filter, TruckFilter.OrigSt2Filter, TruckFilter.OrigSt3Filter, TruckFilter.OrigSt4Filter, TruckFilter.DestSt1Filter, TruckFilter.DestSt2Filter, TruckFilter.DestSt3Filter, TruckFilter.DestSt4Filter, TruckFilter.UseLoadDateFilter, TruckFilter.BookTransTypeFilter, TruckFilter.BookConsPrefixFilter, TruckFilter.LaneNumberFilter, TruckFilter.BookTranCodeFilter, TruckFilter.Page, TruckFilter.PageSize)
                    Select selectDTOData(d, db, TruckFilter)).ToArray()
                If Not tblSolutionTrucks Is Nothing AndAlso tblSolutionTrucks.Count > 0 Then
                    For Each tblSolutionTruck In tblSolutionTrucks
                        If Not tblSolutionTruck Is Nothing Then
                            'get the details
                            Dim Details As List(Of DTO.tblSolutionDetail) = (
                                From d In db.spGetLoadPlanningBookData(tblSolutionTruck.SolutionTruckConsPrefix, tblSolutionTruck.SolutionTruckRouteConsFlag, tblSolutionTruck.SolutionTruckCarrierTruckControl, tblSolutionTruck.SolutionTruckCarrierControl)
                                Select selectDTODetailData(d)).ToList()
                            If Not Details Is Nothing Then tblSolutionTruck.SolutionDetails = Details
                            If blnReadNGLMessages Then
                                'Notes:
                                'database key NMMTRefControl maps to TruckFilter.CompControlFilter 
                                'database key NMMTRefAlphaControl maps to tblSolutionTruck.SolutionTruckKey
                                Dim oMessages = db.tblNGLMessageRefBooks.Where(Function(x) x.NMNMTControl = Utilities.NGLMessageKeyRef.LoadPlanningTruck And x.NMMTRefControl = TruckFilter.CompControlFilter And x.NMMTRefAlphaControl = tblSolutionTruck.SolutionTruckKey).ToList()

                                If Not oMessages Is Nothing AndAlso oMessages.Count() > 0 Then
                                    tblSolutionTruck.addMessages(oMessages)
                                End If
                            End If
                        End If
                    Next
                End If
                Return tblSolutionTrucks
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadPlanningTrucksFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Sub ClearLoadPlanningTruckCostOrMilesAsync(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal ClearCosts As Boolean, ByVal ClearMiles As Boolean)
        Dim fetcher As New ClearLoadPlanningTruckCostOrMilesDelegate(AddressOf Me.ExecClearLoadPlanningTruckCostOrMiles)
        ' Launch thread
        fetcher.BeginInvoke(CompControl, TruckKey, ClearCosts, ClearMiles, Nothing, Nothing)
    End Sub

    Private Sub ExecClearLoadPlanningTruckCostOrMiles(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal ClearCosts As Boolean, ByVal ClearMiles As Boolean)
        Try
            ClearLoadPlanningTruckCostOrMiles(CompControl, TruckKey, ClearCosts, ClearMiles)
        Catch ex As Exception
            'ignore all errors for async processing at this time
            'we could log as system alert message
        End Try

    End Sub

    Public Sub ClearLoadPlanningTruckCostOrMiles(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal ClearCosts As Boolean, Optional ByVal ClearMiles As Boolean = False)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                db.spClearLoadPlanningTruckCostOrMiles(CompControl, TruckKey, ClearCosts, ClearMiles)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ClearLoadPlanningTruckCostOrMiles"))
            End Try
        End Using
    End Sub
    Public Sub saveLoadPlanningTruckMessagesAsync(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal Messages As Dictionary(Of String, List(Of DTO.NGLMessage)))
        Dim fetcher As New SaveLoadPlanningTruckMessagesDelegate(AddressOf Me.ExecsaveLoadPlanningTruckMessages)
        ' Launch thread
        fetcher.BeginInvoke(CompControl, TruckKey, Messages, Nothing, Nothing)
    End Sub

    Private Sub ExecsaveLoadPlanningTruckMessages(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal Messages As Dictionary(Of String, List(Of DTO.NGLMessage)))
        Try
            saveLoadPlanningTruckMessages(CompControl, TruckKey, Messages)
        Catch ex As Exception
            'ignore all errors for async processing at this time
            'we could log as system alert message
        End Try

    End Sub

    Public Function saveLoadPlanningTruckMessages(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal Messages As Dictionary(Of String, List(Of DTO.NGLMessage))) As Boolean
        Dim blnRet As Boolean = True
        Dim tblMessages As New List(Of LTS.tblNGLMessageRefBook)
        If Not Messages Is Nothing AndAlso Messages.Count() > 0 Then
            For Each d In Messages
                If Not d.Value Is Nothing AndAlso d.Value.Count > 0 Then
                    For Each m In d.Value
                        Dim ltsMsg As New LTS.tblNGLMessageRefBook
                        With ltsMsg
                            .NMNMTControl = Utilities.NGLMessageKeyRef.LoadPlanningTruck 'always set the message type to LoadPlanningTruck
                            .NMMTRefControl = CompControl 'always set the ref control to the provided company control number
                            .NMMTRefAlphaControl = TruckKey 'always set the alpa control to the provided truck key
                            Dim eMessageKey As Utilities.NGLMessageKeyRef = Utilities.NGLMessageKeyRef.LoadPlanningTruck
                            .NMMTRefName = eMessageKey.ToString()
                            .NMKeyString = d.Key
                            .NMMessage = m.Message
                            .NMErrorReason = m.ErrorReason
                            .NMErrorDetails = m.ErrorDetails
                            .NMErrorMessage = m.ErrorMessage
                            .NMModDate = Date.Now()
                            .NMModUser = Me.Parameters.UserName
                        End With
                        tblMessages.Add(ltsMsg)
                    Next
                Else
                    'just create one record with the key and no messages
                    Dim ltsMsg As New LTS.tblNGLMessageRefBook
                    With ltsMsg
                        .NMNMTControl = Utilities.NGLMessageKeyRef.LoadPlanningTruck 'always set the message type to LoadPlanningTruck
                        .NMMTRefControl = CompControl 'always set the ref control to the provided company control number
                        .NMMTRefAlphaControl = TruckKey 'always set the alpa control to the provided truck key
                        Dim eMessageKey As Utilities.NGLMessageKeyRef = Utilities.NGLMessageKeyRef.LoadPlanningTruck
                        .NMMTRefName = eMessageKey.ToString()
                        .NMKeyString = d.Key
                        .NMModDate = Date.Now()
                        .NMModUser = Me.Parameters.UserName
                    End With
                    tblMessages.Add(ltsMsg)
                End If
            Next
        Else
            Return True 'nothing to save so just return success
        End If

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'now insert each tblmessage record into the database
                db.tblNGLMessageRefBooks.InsertAllOnSubmit(tblMessages)
                db.SubmitChanges()
            Catch ex As Exception
                blnRet = False
                ManageLinqDataExceptions(ex, buildProcedureName("saveLoadPlanningTruckMessages"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Sub deleteLoadPlanningTruckMessagesAsync(ByVal CompControl As Integer, ByVal TruckKey As String)
        Dim fetcher As New DeleteLoadPlanningTruckMessagesDelegate(AddressOf Me.ExecdeleteLoadPlanningTruckMessages)
        ' Launch thread
        fetcher.BeginInvoke(CompControl, TruckKey, Nothing, Nothing)
    End Sub

    Private Sub ExecdeleteLoadPlanningTruckMessages(ByVal CompControl As Integer, ByVal TruckKey As String)
        Try
            deleteLoadPlanningTruckMessages(CompControl, TruckKey)
        Catch ex As Exception
            'ignore all errors for async processing at this time
            'we could log as system alert message
        End Try

    End Sub

    Public Sub deleteLoadPlanningTruckMessages(ByVal CompControl As Integer, ByVal TruckKey As String)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'now insert each tblmessage record into the database
                db.spDeletetblNGLMessages(Utilities.NGLMessageKeyRef.LoadPlanningTruck, CompControl, TruckKey)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("deleteLoadPlanningTruckMessages"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Return all the records that match the criteria in TruckFilter where all orders on the truck have the same route guide control number.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="CarrierTruckControl"></param>
    ''' <param name="LoadDate"></param>
    ''' <param name="TransType"></param>
    ''' <param name="TranCode"></param>
    ''' <param name="RouteGuideControl"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMatchingRoutedLoads(ByVal CompControl As Integer,
                                               ByVal CarrierTruckControl As Integer,
                                               ByVal LoadDate As Date,
                                               ByVal TransType As String,
                                               ByVal TranCode As String,
                                               ByVal RouteGuideControl As Integer,
                                               ByVal RouteTypeCode As Integer) As DTO.tblSolutionTruck()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria in TruckFilter
                Dim tblSolutionTrucks() As DTO.tblSolutionTruck = (
                    From d In db.spGetMatchingRoutedLoads(CompControl,
                                                          CarrierTruckControl,
                                                          LoadDate,
                                                          TransType,
                                                          TranCode,
                                                          RouteGuideControl,
                                                          RouteTypeCode)
                    Select selectDTOData(d, db, CompControl)).ToArray()
                If Not tblSolutionTrucks Is Nothing AndAlso tblSolutionTrucks.Count > 0 Then
                    For Each tblSolutionTruck In tblSolutionTrucks
                        If Not tblSolutionTruck Is Nothing Then
                            'get the details
                            Dim Details As List(Of DTO.tblSolutionDetail) = (
                                From d In db.spGetLoadPlanningBookData(tblSolutionTruck.SolutionTruckConsPrefix, tblSolutionTruck.SolutionTruckRouteConsFlag, tblSolutionTruck.SolutionTruckCarrierTruckControl, tblSolutionTruck.SolutionTruckCarrierControl)
                                Select selectDTODetailData(d)).ToList()
                            If Not Details Is Nothing Then tblSolutionTruck.SolutionDetails = Details
                        End If
                    Next
                End If
                Return tblSolutionTrucks
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMatchingRoutedLoads"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetCarriersForRoute(ByVal CompControl As Integer,
                                               ByVal RouteGuideControl As Integer,
                                               ByVal RouteTypeCode As Integer,
                                               ByVal TransType As String,
                                               ByVal StateFilter As String,
                                               ByVal LoadDate As Date,
                                               ByVal RequiredDate As Date,
                                               ByVal Cases As Double,
                                               ByVal Wgt As Double,
                                               ByVal Cubes As Double,
                                               ByVal Plts As Integer,
                                               ByVal TempType As String,
                                               ByVal IsHazmat As Boolean,
                                               Optional ByVal ActiveCarriersOnly As Boolean = True) As DTO.CarriersForRoute()


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Return all the records that match the criteria in TruckFilter
                Dim CarriersForRoute() As DTO.CarriersForRoute = (
                    From d In db.spGetCarriersForRoute(CompControl,
                                                       RouteGuideControl,
                                                       RouteTypeCode,
                                                       TransType,
                                                       StateFilter,
                                                       LoadDate,
                                                       RequiredDate,
                                                       Cases,
                                                       Wgt,
                                                       Cubes,
                                                       Plts,
                                                       TempType,
                                                       IsHazmat,
                                                       ActiveCarriersOnly)
                    Select New DTO.CarriersForRoute With {
                        .StaticRouteControl = d.StaticRouteControl _
                         , .StaticRouteNumber = d.StaticRouteNumber _
                         , .StaticRouteCompControl = d.StaticRouteCompControl _
                         , .StaticRouteAutoTenderFlag = d.StaticRouteAutoTenderFlag _
                         , .StaticRouteUseShipDateFlag = d.StaticRouteUseShipDateFlag _
                         , .StaticRouteGuideDateSelectionDaysBefore = d.StaticRouteGuideDateSelectionDaysBefore _
                         , .StaticRouteGuideDateSelectionDaysAfter = d.StaticRouteGuideDateSelectionDaysAfter _
                         , .StaticRouteSplitOversizedLoads = d.StaticRouteSplitOversizedLoads _
                         , .StaticRouteCapacityPreference = d.StaticRouteCapacityPreference _
                         , .StaticRouteRequireAutoTenderApproval = d.StaticRouteRequireAutoTenderApproval _
                         , .StaticRouteFillLargestFirst = d.StaticRouteFillLargestFirst _
                         , .StaticRoutePlaceOnHold = d.StaticRoutePlaceOnHold _
                         , .StaticRouteCarrCarrierControl = d.StaticRouteCarrCarrierControl _
                         , .StaticRouteCarrCarrierName = d.StaticRouteCarrCarrierName _
                         , .StaticRouteCarrName = d.StaticRouteCarrName _
                         , .StaticRouteCarrRouteTypeCode = d.StaticRouteCarrRouteTypeCode _
                         , .StaticRouteCarrAutoTenderFlag = d.StaticRouteCarrAutoTenderFlag _
                         , .StaticRouteCarrTendLeadTime = d.StaticRouteCarrTendLeadTime _
                         , .StaticRouteCarrMaxStops = d.StaticRouteCarrMaxStops _
                         , .StaticRouteCarrHazmatFlag = d.StaticRouteCarrHazmatFlag _
                         , .StaticRouteCarrTransType = d.StaticRouteCarrTransType _
                         , .StaticRouteCarrRouteSequence = d.StaticRouteCarrRouteSequence _
                         , .StaticRouteCarrRequireAutoTenderApproval = d.StaticRouteCarrRequireAutoTenderApproval _
                         , .StaticRouteCarrAutoAcceptLoads = d.StaticRouteCarrAutoAcceptLoads _
                         , .StaticRouteStateFilter = d.StaticRouteStateFilter _
                         , .StaticRouteCarrControl = d.StaticRouteCarrControl _
                         , .StaticRouteEquipControl = d.StaticRouteEquipControl _
                         , .StaticRouteEquipCarrierTruckControl = d.StaticRouteEquipCarrierTruckControl _
                         , .StaticRouteEquipName = d.StaticRouteEquipName _
                         , .CarrierSCAC = d.CarrierSCAC _
                         , .CarrierActive = d.CarrierActive _
                         , .CarrierIgnoreTariff = d.CarrierIgnoreTariff _
                         , .CarrierAutoFinalize = d.CarrierAutoFinalize _
                         , .CarrierTruckEquipment = d.CarrierTruckEquipment _
                         , .CarrierTruckFAK = d.CarrierTruckFAK _
                         , .CarrierTruckPUMon = d.CarrierTruckPUMon _
                         , .CarrierTruckPUTue = d.CarrierTruckPUTue _
                         , .CarrierTruckPUWed = d.CarrierTruckPUWed _
                         , .CarrierTruckPUThu = d.CarrierTruckPUThu _
                         , .CarrierTruckPUFri = d.CarrierTruckPUFri _
                         , .CarrierTruckPUSat = d.CarrierTruckPUSat _
                         , .CarrierTruckPUSun = d.CarrierTruckPUSun _
                         , .CarrierTruckDLMon = d.CarrierTruckDLMon _
                         , .CarrierTruckDLTue = d.CarrierTruckDLTue _
                         , .CarrierTruckDLWed = d.CarrierTruckDLWed _
                         , .CarrierTruckDLThu = d.CarrierTruckDLThu _
                         , .CarrierTruckDLFri = d.CarrierTruckDLFri _
                         , .CarrierTruckDLSat = d.CarrierTruckDLSat _
                         , .CarrierTruckDLSun = d.CarrierTruckDLSun _
                         , .CarrierTruckMaxCases = d.CarrierTruckMaxCases _
                         , .CarrierTruckMinCases = d.CarrierTruckMinCases _
                         , .CarrierTruckSplitCases = d.CarrierTruckSplitCases _
                         , .CarrierTruckMaxWgt = d.CarrierTruckMaxWgt _
                         , .CarrierTruckMinWgt = d.CarrierTruckMinWgt _
                         , .CarrierTruckSplitWgt = d.CarrierTruckSplitWgt _
                         , .CarrierTruckMaxCubes = d.CarrierTruckMaxCubes _
                         , .CarrierTruckMinCubes = d.CarrierTruckMinCubes _
                         , .CarrierTruckSplitCubes = d.CarrierTruckSplitCubes _
                         , .CarrierTruckMaxPlts = d.CarrierTruckMaxPlts _
                         , .CarrierTruckMinPlts = d.CarrierTruckMinPlts _
                         , .CarrierTruckSplitPlts = d.CarrierTruckSplitPlts _
                         , .CarrierTruckTrucksAvailable = d.CarrierTruckTrucksAvailable _
                         , .CarrierTruckMaxLoadsByWeek = d.CarrierTruckMaxLoadsByWeek _
                         , .CarrierTruckMaxLoadsByMonth = d.CarrierTruckMaxLoadsByMonth _
                         , .CarrierTruckTotalLoadsForWeek = d.CarrierTruckTotalLoadsForWeek _
                         , .CarrierTruckTotalLoadsForMonth = d.CarrierTruckTotalLoadsForMonth _
                         , .CarrierTruckTempType = d.CarrierTruckTempType _
                         , .CarrierTruckHazmat = d.CarrierTruckHazmat
                         }).ToArray()






                Return CarriersForRoute


            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Friend Shared Function SelectDTOData(ByVal d As LTS.spGetLoadPlanningTruckData365Result, ByVal db As NGLMasBookDataContext, ByVal CompControl As Integer, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckControl",
                                                      "SolutionTruckUpdated",
                                                      "SolutionTruckCapacityPreference",
                                                      "SolutionTruckCompControl",
                                                      "SolutionTruckKey",
                                                      "SolutionTruckAttributeControl",
                                                      "SolutionTruckAttributeTypeControl",
                                                      "SolutionTruckSolutionControl",
                                                      "SolutionTruckCarrierEquipmentCodes",
                                                      "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckControl = 0
            .SolutionTruckUpdated = New Byte() {}
            .SolutionTruckCapacityPreference = db.udfGetRouteCapacityPreference(d.SolutionTruckStaticRouteControl)
            .SolutionTruckCompControl = CompControl
            .SolutionTruckKey = d.SolutionTruckCarrierControl.ToString & "-" & d.SolutionTruckConsPrefix.Trim & "-" & If(d.SolutionTruckRouteConsFlag, "1", "0") & "-" & d.SolutionTruckRouteTypeCode & "-" & d.SolutionTruckCarrierTruckControl.ToString()
            .SolutionTruckAttributeControl = 0
            .SolutionTruckAttributeTypeControl = 0
            .SolutionTruckSolutionControl = 0
            .SolutionTruckCarrierEquipmentCodes = ""
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.spGetLoadPlanningTruckRecordResult, ByVal db As NGLMasBookDataContext, ByVal CompControl As Integer, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckControl",
                                                      "SolutionTruckUpdated",
                                                      "SolutionTruckCapacityPreference",
                                                      "SolutionTruckCompControl",
                                                      "SolutionTruckKey",
                                                      "SolutionTruckAttributeControl",
                                                      "SolutionTruckAttributeTypeControl",
                                                      "SolutionTruckSolutionControl",
                                                      "SolutionTruckCarrierEquipmentCodes",
                                                      "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckControl = 0
            .SolutionTruckUpdated = New Byte() {}
            .SolutionTruckCapacityPreference = db.udfGetRouteCapacityPreference(d.SolutionTruckStaticRouteControl)
            .SolutionTruckCompControl = CompControl
            .SolutionTruckKey = d.SolutionTruckCarrierControl.ToString & "-" & d.SolutionTruckConsPrefix.Trim & "-" & If(d.SolutionTruckRouteConsFlag, "1", "0") & "-" & d.SolutionTruckRouteTypeCode & "-" & d.SolutionTruckCarrierTruckControl.ToString()
            .SolutionTruckAttributeControl = 0
            .SolutionTruckAttributeTypeControl = 0
            .SolutionTruckSolutionControl = 0
            .SolutionTruckCarrierEquipmentCodes = ""
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.spGetLoadPlanningTruckDataResult, ByVal db As NGLMasBookDataContext, ByVal TruckFilter As DTO.LoadPlanningTruckDataFilter, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckControl",
                                                      "SolutionTruckUpdated",
                                                      "SolutionTruckCapacityPreference",
                                                      "SolutionTruckCompControl",
                                                      "SolutionTruckKey",
                                                      "SolutionTruckAttributeControl",
                                                      "SolutionTruckAttributeTypeControl",
                                                      "SolutionTruckSolutionControl",
                                                      "SolutionTruckCarrierEquipmentCodes",
                                                      "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckControl = 0
            .SolutionTruckUpdated = New Byte() {}
            .SolutionTruckCapacityPreference = db.udfGetRouteCapacityPreference(d.SolutionTruckStaticRouteControl)
            .SolutionTruckCompControl = TruckFilter.CompControlFilter
            .SolutionTruckKey = d.SolutionTruckCarrierControl.ToString & "-" & d.SolutionTruckConsPrefix.Trim & "-" & If(d.SolutionTruckRouteConsFlag, "1", "0") & "-" & d.SolutionTruckRouteTypeCode & "-" & d.SolutionTruckCarrierTruckControl.ToString()
            .SolutionTruckAttributeControl = 0
            .SolutionTruckAttributeTypeControl = 0
            .SolutionTruckSolutionControl = 0
            .SolutionTruckCarrierEquipmentCodes = ""
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function


    Friend Shared Function selectDTOData(ByVal d As LTS.spGetLoadPlanningTruckData365Result, ByVal db As NGLMasBookDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckControl",
                                                      "SolutionTruckUpdated",
                                                      "SolutionTruckKey",
                                                      "SolutionTruckAttributeControl",
                                                      "SolutionTruckAttributeTypeControl",
                                                      "SolutionTruckSolutionControl",
                                                      "SolutionTruckCarrierEquipmentCodes",
                                                      "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckControl = 0
            .SolutionTruckUpdated = New Byte() {}
            .SolutionTruckKey = d.SolutionTruckCarrierControl.ToString & "-" & d.SolutionTruckConsPrefix.Trim & "-" & If(d.SolutionTruckRouteConsFlag, "1", "0") & "-" & d.SolutionTruckRouteTypeCode & "-" & d.SolutionTruckCarrierTruckControl.ToString()
            .SolutionTruckAttributeControl = 0
            .SolutionTruckAttributeTypeControl = 0
            .SolutionTruckSolutionControl = 0
            .SolutionTruckCarrierEquipmentCodes = ""
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.spGetMatchingRoutedLoadsResult, ByVal db As NGLMasBookDataContext, ByVal CompControl As Integer, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionTruck
        Dim oDTO As New DTO.tblSolutionTruck
        Dim skipObjs As New List(Of String) From {"SolutionTruckUpdated",
                                                      "SolutionTruckCapacityPreference",
                                                      "SolutionTruckCompControl",
                                                      "SolutionTruckKey",
                                                      "SolutionTruckAttributeControl",
                                                      "SolutionTruckAttributeTypeControl",
                                                      "SolutionTruckSolutionControl", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionTruckUpdated = New Byte() {}
            .SolutionTruckCapacityPreference = db.udfGetRouteCapacityPreference(d.SolutionTruckStaticRouteControl)
            .SolutionTruckCompControl = CompControl
            .SolutionTruckKey = d.SolutionTruckCarrierControl.ToString & "-" & d.SolutionTruckConsPrefix.Trim & "-" & If(d.SolutionTruckRouteConsFlag, "1", "0") & "-" & d.SolutionTruckRouteTypeCode & "-" & d.SolutionTruckCarrierTruckControl.ToString()
            .SolutionTruckAttributeControl = 0
            .SolutionTruckAttributeTypeControl = 0
            .SolutionTruckSolutionControl = 0
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    Friend Shared Function selectDTODetailData(ByVal d As LTS.spGetLoadPlanningBookDataResult, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblSolutionDetail
        Dim oDTO As New DTO.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailControl", "SolutionDetailSolutionTruckControl", "SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailControl = 0
            .SolutionDetailSolutionTruckControl = 0
            .SolutionDetailUpdated = d.SolutionDetailUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    ''' <summary>
    ''' calls the spUpdateLoadPlanningCarrier NGL stored procedure 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookRouteConsFlag"></param>
    ''' <param name="BookStopNo"></param>
    ''' <param name="CarrierControl"></param>
    ''' <remarks>
    ''' the runNGLStoredProcedrue method throws the following Fault Exceptions:
    ''' E_DataValidationFailure
    ''' E_FailedToExecute
    ''' E_DBLoginFailure
    ''' E_DBConnectionFailure
    ''' E_SQLException
    ''' E_UnExpected
    ''' </remarks>
    Public Sub UpdateLoadPlanningCarrier(ByVal BookControl As Integer,
                                                   ByVal BookConsPrefix As String,
                                                   ByVal BookRouteConsFlag As Boolean,
                                                   ByVal BookStopNo As Integer,
                                                   ByVal CarrierControl As Integer,
                                                   Optional ByVal BookCarrTruckControl As System.Nullable(Of Integer) = Nothing,
                                                   Optional ByVal BookHoldLoad As System.Nullable(Of Integer) = Nothing)
        Dim strProcName As String = "dbo.spUpdateLoadPlanningCarrier"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", BookConsPrefix)
        oCmd.Parameters.AddWithValue("@BookRouteConsFlag", BookRouteConsFlag)
        oCmd.Parameters.AddWithValue("@BookStopNo", BookStopNo)
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        If BookCarrTruckControl.HasValue Then oCmd.Parameters.AddWithValue("@BookCarrTruckControl", BookCarrTruckControl.Value)
        If BookHoldLoad.HasValue Then oCmd.Parameters.AddWithValue("@BookHoldLoad", BookHoldLoad.Value)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

    ''' <summary>
    ''' calls the spUpdateLoadPlanningStopNo NGL stored procedure
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookStopNo"></param>
    ''' <remarks> 
    ''' the runNGLStoredProcedrue method throws the following Fault Exceptions:
    ''' E_DataValidationFailure
    ''' E_FailedToExecute
    ''' E_DBLoginFailure
    ''' E_DBConnectionFailure
    ''' E_SQLException
    ''' E_UnExpected</remarks>
    Public Sub UpdateLoadPlanningStopNo(ByVal BookControl As Integer,
                                                   ByVal BookStopNo As Integer)
        Dim strProcName As String = "dbo.spUpdateLoadPlanningStopNo"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@BookStopNo", BookStopNo)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

    Public Function UpdateLoadPlanningSplitOrder(ByVal BookControl As Integer,
                                                   ByVal TempOrderSequence As Integer,
                                                   ByVal BookConsPrefix As String,
                                                   ByVal BookRouteConsFlag As Boolean,
                                                   ByVal BookStopNo As Integer,
                                                   ByVal CarrierControl As Integer,
                                                   Optional ByVal BookCarrTruckControl As System.Nullable(Of Integer) = Nothing,
                                                   Optional ByVal BookHoldLoad As System.Nullable(Of Integer) = Nothing) As LTS.spUpdateLoadPlanningSplitOrderResult

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try


                Dim oResults = db.spUpdateLoadPlanningSplitOrder(BookControl,
                                                        TempOrderSequence,
                                                       BookConsPrefix,
                                                       BookRouteConsFlag,
                                                       BookStopNo,
                                                       CarrierControl,
                                                       BookCarrTruckControl,
                                                       BookHoldLoad,
                                                       Me.Parameters.UserName)

                If Not oResults Is Nothing Then
                    Return oResults.FirstOrDefault
                Else
                    Return Nothing
                End If


            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' executes spTenderLoadFromLoadPlanning which updates the 
    ''' BookTranCode to PC (Modified to no longer assign carriers or costs)
    ''' The caller must manage carrier assignment and costing
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <remarks></remarks>
    Public Sub TenderLoadFromLoadPlanning(ByVal BookConsPrefix As String)
        throwDepreciatedException(buildProcedureName("TenderLoadFromLoadPlanning"))
        Return
        Dim strProcName As String = "dbo.spTenderLoadFromLoadPlanning"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookConsPrefix", Left(BookConsPrefix, 20))
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

    ''' <summary>
    ''' updates the booking record for BookProNumber with the assigned carrier, 
    ''' consolidation, stop number. and other data using spUpdateLoadPlanningCarrierWithPro
    ''' </summary>
    ''' <param name="BookProNumber"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookRouteConsFlag"></param>
    ''' <param name="BookStopNo"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="BookCarrTruckControl"></param>
    ''' <param name="BookHoldLoad"></param>
    ''' <remarks></remarks>
    Public Sub UpdateLoadPlanningCarrier(ByVal BookProNumber As String,
                                             ByVal BookConsPrefix As String,
                                             ByVal BookRouteConsFlag As Boolean,
                                             ByVal BookStopNo As Integer,
                                             ByVal CarrierControl As Integer,
                                             ByVal BookCarrTruckControl As Integer,
                                             ByVal BookHoldLoad As Integer, Optional ByVal BookType As System.Nullable(Of Integer) = Nothing)

        Dim strProcName As String = "dbo.spUpdateLoadPlanningCarrierWithPro"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookProNumber", BookProNumber)
        oCmd.Parameters.AddWithValue("@BookConsPrefix", BookConsPrefix)
        oCmd.Parameters.AddWithValue("@BookRouteConsFlag", BookRouteConsFlag)
        oCmd.Parameters.AddWithValue("@BookStopNo", BookStopNo)
        oCmd.Parameters.AddWithValue("@CarrierControl", CarrierControl)
        oCmd.Parameters.AddWithValue("@BookCarrTruckControl", BookCarrTruckControl)
        oCmd.Parameters.AddWithValue("@BookHoldLoad", BookHoldLoad)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
    End Sub

    ''' <summary>
    ''' This method attempts to route loads according to the routing guide 
    ''' any orders that cannot be routed are returned in the result list  
    ''' </summary>
    ''' <param name="OptimizeCapacity"></param>
    ''' <remarks>
    '''  Modified by RHR for v-8.3.0.001 on 09/15/2020 
    '''     added logic to exclude the current CNS number when routing new bookings
    ''' Modified by RHR for v-8.2.1.007 on 06/17/2021 fixed bug where loop did not exit correctly on failure
    ''' </remarks>
    Public Function RouteLoads(ByVal OptimizeCapacity As Boolean) As List(Of DTO.tblSolutionDetail)
        'enmRtCapPref 
        '0 = Sequence
        '1 = Cases
        '2 = Weight
        '3 = Pallets
        '4 = Cubes   
        Dim strCurrentOrder As String = "{Not Found}"
        OrdersNotLoaded = New List(Of DTO.tblSolutionDetail)
        Try

            Dim enmRtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference = DataTransferObjects.tblSolutionTruck.RoutingCapacityPreference.Pallets
            Dim lTrucks As New List(Of DTO.tblSolutionTruck)
            Dim providerStaticRoute As NGLtblStaticRouteData = Me.NDPBaseClassFactory("NGLtblStaticRouteData")
            Dim BookItemData As NGLBookItemData = Me.NDPBaseClassFactory("NGLBookItemData")
            Dim strUnexpectedErrors As New List(Of String)
            Dim strNotLoadedErrors As New List(Of String)
            Dim blnStopProcessing As Boolean = False
            'Debug Error Message to Test Routing 
            'Utilities.SaveAppError("RouteLoads Running.", Me.Parameters)
            Do
                Dim oSorted As New List(Of DTO.tblSolutionDetail)
                'Lock other threads
                SyncLock mSharedPadLock
                    mSharedLoadsBeingRouted = True
                    'Debug Error Message to Test Routing 
                    'Utilities.SaveAppError("Loads being routed Is now true ", Me.Parameters)
                    'Debug Error Message to Test Routing 
                    'Utilities.SaveAppError("Shared Order Available " & mSharedOrdersToRoute.Count & " Shared Orders.", Me.Parameters)
                    'Debug Error Message to Test Routing 
                    'Utilities.SaveAppError("Sorted Orders Available " & oSorted.Count & " Sorted Orders.", Me.Parameters)
                    If Not mSharedOrdersToRoute Is Nothing AndAlso mSharedOrdersToRoute.Count > 0 Then
                        'sort the records by location and order
                        oSorted = (From d In mSharedOrdersToRoute Order By d.LaneLocation, d.OrderNumber Descending Select d).ToList
                    End If
                    If Not oSorted Is Nothing AndAlso oSorted.Count > 0 Then
                        'Debug Error Message to Test Routing 
                        'Utilities.SaveAppError("Read " & oSorted.Count & " Sorted Orders.", Me.Parameters)
                        For Each o In oSorted
                            'Debug Error Message to Test Routing 
                            'Utilities.SaveAppError("Searching For ProNumber to remove: " & o.SolutionDetailProNumber, Me.Parameters)
                            For Each m In mSharedOrdersToRoute
                                If m.SolutionDetailProNumber = o.SolutionDetailProNumber Then
                                    'Debug Error Message to Test Routing 
                                    'Utilities.SaveAppError("Removed ProNumber: " & o.SolutionDetailProNumber, Me.Parameters)
                                    mSharedOrdersToRoute.Remove(m)
                                    Exit For
                                End If
                            Next
                        Next
                        'Debug Error Message to Test Routing 
                        'Utilities.SaveAppError("Shared Order After Remove " & mSharedOrdersToRoute.Count & " Shared Orders.", Me.Parameters)
                    Else
                        'allow RouteLoads to be called again
                        mSharedLoadsBeingRouted = False
                        'Debug Error Message to Test Routing 
                        'Utilities.SaveAppError("Loads being routed Is now false no orders left", Me.Parameters)
                        blnStopProcessing = True
                        Exit Do
                    End If
                End SyncLock
                'Debug Error Message to Test Routing 
                'Utilities.SaveAppError("Routing " & oSorted.Count & " Sorted Orders.", Me.Parameters)
                '*********************************  TO DO ****************************************************               
                'Add code to use lcarriers when selecting or creating a new truck.  we always start with the smallest piece of equipment for a new truck that will hold the order or load to be routed
                '
                'Complete the code that checks if a larger truck is available
                '
                'Add logic to check if we fill the largest truck first based on the route guide setting
                '
                'Add code to get the Route Configuration for each piece of equipment? or for each order?  (by order my be the best option now that I think of it)
                '
                'Check the logic that uses the LN list:
                '   Dim MasterBuild = lTrucks.Where(Function(p) p.Orders.Any(Function(y) LN.Contains(y.LaneLocation)))
                '   this may not work as expected because we are refreshing the truck list with each order (some thought and understanding of this is required)
                '
                '**********************************************************************************************
                If oSorted Is Nothing OrElse oSorted.Count < 1 Then Continue Do
                For Each o In oSorted
                    'Debug Error Message to Test Routing 
                    'Utilities.SaveAppError("Route Loads Is finished processing order number " & mSharedLastOrderNumber & ".", Me.Parameters)
                    strCurrentOrder = o.OrderNumber
                    'lock other threads
                    SyncLock mSharedPadLock
                        mSharedLastOrderNumber = strCurrentOrder
                    End SyncLock
                    'Debug Error Message to Test Routing 
                    'Utilities.SaveAppError("Route Loads Is starting to process order number " & mSharedLastOrderNumber & ".", Me.Parameters)
                    Try
                        Dim blnLoaded As Boolean = False
                        'get the static route data for this order
                        If o.SolutionDetailRouteGuideControl = 0 Then
                            'note:  we need to add a message or attribute control for why this load was not loaded
                            'to be appended to the attributes list
                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it has Not been assigned a routing guide.<br />" & vbCrLf)
                            AddToNotLoaded(o, OrdersNotLoaded)
                            Continue For
                        End If
                        StaticRouteData = providerStaticRoute.GettblStaticRouteFiltered(o.SolutionDetailRouteGuideControl)
                        If StaticRouteData Is Nothing Then
                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because the system could Not read the static route guide settings.<br />" & vbCrLf)
                            AddToNotLoaded(o, OrdersNotLoaded)
                            Continue For
                        End If
                        o.Details = BookItemData.GetBookItemsFiltered(o.SolutionDetailBookLoadControl).ToList

                        enmRtCapPref = StaticRouteData.StaticRouteCapacityPreference
                        'get a list of available carriers and their trucks, we pass zero for capacity to be sure we can route smaller loads together and that we can split orders.
                        ListAvailableEquipment = GetCarriersForRoute(o.SolutionDetailCompControl, o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode, o.SolutionDetailTransType, o.SolutionDetailDestState, o.SolutionDetailDateLoad, o.SolutionDetailDateRequired, 0, 0, 0, 0, o.SolutionDetailCom, o.SolutionDetailIsHazmat).ToList
                        If ListAvailableEquipment Is Nothing OrElse ListAvailableEquipment.Count < 1 Then
                            'note:  we need to add a message or attribute control for why this load was not loaded
                            'to be appended to the attributes list
                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because no equipment types match the order requirements.<br />" & vbCrLf)
                            AddToNotLoaded(o, OrdersNotLoaded)
                            Continue For
                        End If
                        'check the route type and filter the matching loads/trucks as needed
                        Dim sThisCNS = o.SolutionDetailConsPrefix
                        Select Case o.SolutionDetailRouteTypeCode
                            Case DTO.tblSolutionTruck.RouteTypeCodes.Single_LTL
                                lTrucks = Nothing
                                addNewTruck(lTrucks, enmRtCapPref, o)
                            Case DTO.tblSolutionTruck.RouteTypeCodes.Full_Load
                                'filter the list by address
                                Dim strLaneLocation As String = o.LaneLocation
                                'Modified by RHR for v-8.3.0.001 on 09/15/2020 added logic to exclude the current CNS number when routing new bookings
                                lTrucks = GetMatchingRoutedLoads(o.SolutionDetailCompControl, o.SolutionDetailSolutionTruckControl, o.SolutionDetailDateLoad, o.SolutionDetailTransType, "P", o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode).Where(Function(x) x.Orders(0).LaneLocation = strLaneLocation).ToList().Where(Function(x) x.CNS <> sThisCNS).ToList()
                            Case Else
                                'get all available loads that match
                                'Modified by RHR for v-8.3.0.001 on 09/15/2020 added logic to exclude the current CNS number when routing new bookings
                                lTrucks = GetMatchingRoutedLoads(o.SolutionDetailCompControl, o.SolutionDetailSolutionTruckControl, o.SolutionDetailDateLoad, o.SolutionDetailTransType, "P", o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode).ToList().Where(Function(x) x.CNS <> sThisCNS).ToList()
                        End Select
                        'if the list of matching loads is empty we need to add a default truck
                        Dim lInvalidTrucks As New List(Of DTO.tblSolutionTruck)
                        If lTrucks Is Nothing OrElse lTrucks.Count < 1 Then
                            addNewTruck(lTrucks, enmRtCapPref)
                        Else
                            Dim oBookLoadControls As List(Of Integer) = (From t In lTrucks From l In t.Orders Select l.SolutionDetailBookLoadControl).ToList()
                            Dim oDetailsList As List(Of DTO.BookItem) = BookItemData.GetBookItemsFiltered(oBookLoadControls)
                            For Each t In lTrucks
                                t.Parameters = Me.Parameters
                                t.TruckDataObject = Me
                                t.BatchProcessingDataObject = New NGLBatchProcessDataProvider(Me.Parameters)
                                t.BookItemDataObject = New NGLBookItemData(Me.Parameters)
                                t.RouteConfig = GetRouteConfigForEquipment(Me.StaticRouteData.StaticRouteControl, t.SolutionTruckCarrierControl, t.SolutionTruckCarrierTruckControl)
                                If t.RouteConfig Is Nothing Then
                                    lInvalidTrucks.Add(t)
                                Else
                                    If Not t.Orders Is Nothing AndAlso t.Orders.Count > 0 Then
                                        For Each order In t.Orders
                                            Dim intBookLoadControl = order.SolutionDetailBookLoadControl
                                            order.Details = (From d In oDetailsList Where d.BookItemBookLoadControl = intBookLoadControl Select d).ToList()
                                            'order.Details = BookItemData.GetBookItemsFiltered(order.SolutionDetailBookLoadControl).ToList
                                        Next
                                    End If
                                End If
                            Next
                        End If
                        If Not lInvalidTrucks Is Nothing AndAlso lInvalidTrucks.Count > 0 Then
                            For Each t In lInvalidTrucks
                                If lTrucks.Contains(t) Then lTrucks.Remove(t)
                            Next
                            If lTrucks Is Nothing OrElse lTrucks.Count < 1 Then
                                addNewTruck(lTrucks, enmRtCapPref)
                            End If
                        End If

                        Dim LN As New List(Of String)
                        LN.Add(o.LaneLocation)

                        If enmRtCapPref = DTO.tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                            If Not lTrucks Is Nothing AndAlso lTrucks.Count > 0 Then
                                Dim previousTruck As DTO.tblSolutionTruck = Nothing
                                Dim tCount = lTrucks.Count
                                Dim cCount = 0
                                For Each t In lTrucks.OrderBy(Function(x) x.MinSequence)
                                    cCount += 1
                                    If previousTruck Is Nothing Then
                                        If cCount = tCount OrElse t.MaxSequence = 0 OrElse t.MaxSequence >= o.RouteSequence Then
                                            If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                blnLoaded = True
                                                Exit For
                                            Else
                                                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                                AddToNotLoaded(o, OrdersNotLoaded)
                                                Exit For
                                            End If
                                        Else
                                            previousTruck = t
                                        End If
                                    Else
                                        If cCount = tCount OrElse t.MinSequence = 0 OrElse t.MaxSequence = 0 Then
                                            If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                blnLoaded = True
                                                Exit For
                                            Else
                                                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                                AddToNotLoaded(o, OrdersNotLoaded)
                                                Exit For
                                            End If
                                        Else
                                            If t.MinSequence > o.RouteSequence Then
                                                'try to put this order on the previous truck
                                                If previousTruck.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                    blnLoaded = True
                                                    Exit For
                                                Else
                                                    'try to put this order ont the current truck
                                                    If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                        blnLoaded = True
                                                        Exit For
                                                    Else
                                                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                                        AddToNotLoaded(o, OrdersNotLoaded)
                                                        Exit For
                                                    End If
                                                End If

                                            ElseIf t.MinSequence = o.RouteSequence Then
                                                'this truck already has an order with this sequence so see if there is room for this order
                                                If t.masterBuild(o, lTrucks, enmRtCapPref, True) Then
                                                    blnLoaded = True
                                                    Exit For
                                                Else
                                                    'try to put this order on the previous truck
                                                    If previousTruck.masterBuild(o, lTrucks, enmRtCapPref, True) Then
                                                        blnLoaded = True
                                                        Exit For
                                                    Else
                                                        'try again to put this order on the current truck but this time without the match seq only flag
                                                        If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                            blnLoaded = True
                                                            Exit For
                                                        Else
                                                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                                            AddToNotLoaded(o, OrdersNotLoaded)
                                                            Exit For
                                                        End If
                                                    End If
                                                End If
                                            ElseIf t.MinSequence <= o.RouteSequence And t.MaxSequence >= o.RouteSequence Then
                                                If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                    blnLoaded = True
                                                    Exit For
                                                Else
                                                    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                                    AddToNotLoaded(o, OrdersNotLoaded)
                                                    Exit For
                                                End If
                                            Else
                                                previousTruck = t
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Else
                            'we group orders by destination
                            'Build a list of trucks that have orders with the same lane number (typically one but may be more if more than a full truck load of orders exists for any given lane)
                            Dim MasterBuild = lTrucks.Where(Function(p) p.Orders.Any(Function(y) LN.Contains(y.LaneLocation)))
                            'If Not MasterBuild Is Nothing AndAlso MasterBuild.Count > 0 Then
                            '    'we use a for loop because items may be added or removed from the collection
                            '    For truck = 0 To MasterBuild.Count - 1
                            '        Dim t As DTO.tblSolutionTruck = MasterBuild(truck)
                            '        If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                            '            blnLoaded = True
                            '            Exit For
                            '        Else
                            '            'master build failed, trying next truck
                            '        End If
                            '    Next
                            'End If
                            If Not MasterBuild Is Nothing AndAlso MasterBuild.Count > 0 Then
                                'we use a for loop because items may be added or removed from the collection
                                For truck = 0 To MasterBuild.Count - 1
                                    Dim t As DTO.tblSolutionTruck = MasterBuild(truck)
                                    If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                        blnLoaded = True
                                        Exit For
                                    Else
                                        'master build failed, trying next truck
                                    End If
                                    'Modified by RHR for v-8.2.1.007 on 06/17/2021
                                    'fixed bug where loop did not exit correctly on failure
                                    'we now check if trucks still exist and increment the truck count as needed
                                    truck = truck + 1
                                    If truck > MasterBuild.Count - 1 Then Exit For
                                    'End Modified by RHR for v-8.2.1.007 on 06/17/2021
                                Next
                            End If
                            'If no match found then try to fit the load on any truck available sorted by trucks with the smallest capacity
                            If Not blnLoaded Then
                                For Each t In lTrucks
                                    If t.addOrder(o, enmRtCapPref) Then
                                        blnLoaded = True
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        'if we get here no trucks are available so create a new truck
                        If Not blnLoaded Then
                            Dim newTruck As DTO.tblSolutionTruck = addNewTruck(lTrucks, enmRtCapPref, o, True)
                            'Modified by RHR 2/19/13 we do not need to call lTrucks.Add after addNewTruck because 
                            'it is already added to the collection.
                            'lTrucks.Add(newTruck)
                            If Not newTruck.addOrderWSplit(o, lTrucks, enmRtCapPref) Then
                                If newTruck.Orders Is Nothing OrElse newTruck.Orders.Count < 1 Then lTrucks.Remove(newTruck)
                                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did Not meet the routing guide requirements.<br />" & vbCrLf)
                                AddToNotLoaded(o, OrdersNotLoaded)
                            End If
                        End If
                        'optimize capacity and equipment size


                        If enmRtCapPref <> DTO.tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                            'optimize capacity
                            If Me.StaticRouteData.StaticRouteFillLargestFirst Then
                                'try to move orders from smaller loads onto the larger trucks
                                For Each t In lTrucks.OrderByDescending(Function(x) x.TotalWgt).Where(Function(y) y.atCapacity = False)
                                    Dim spaceleft As Double = 0
                                    Dim strCNS As String = t.CNS
                                    Dim dblMaxWeight As Double = t.maxWgt
                                    Dim blnMatchFound As Boolean = False
                                    Dim trucksleft As List(Of DTO.tblSolutionTruck) = (From lt In lTrucks Where ((lt.maxWgt = dblMaxWeight And lt.atCapacity = False) Or (lt.maxWgt < dblMaxWeight)) And lt.CNS <> strCNS Order By lt.maxWgt Select lt).ToList
                                    If Not trucksleft Is Nothing AndAlso trucksleft.Count > 0 Then
                                        optimizeLoadByCapacity(t, trucksleft, enmRtCapPref)
                                    End If
                                Next
                            Else
                                'optimize for smallest truck
                                'try to move orders from non full larger trucks to fill smaller trucks (later will will call the resize equipment requirement to maximize cost by size)
                                For Each t In lTrucks.OrderBy(Function(x) x.TotalWgt).Where(Function(y) y.atCapacity = False)
                                    Dim spaceleft As Double = 0
                                    Dim strCNS As String = t.CNS
                                    Dim dblMaxWeight As Double = t.maxWgt
                                    Dim blnMatchFound As Boolean = False
                                    Dim trucksleft As List(Of DTO.tblSolutionTruck) = (From lt In lTrucks Where lt.atCapacity = False And lt.CNS <> strCNS Order By lt.maxWgt Descending Select lt).ToList
                                    If Not trucksleft Is Nothing AndAlso trucksleft.Count > 0 Then
                                        optimizeLoadByCapacity(t, trucksleft, enmRtCapPref)
                                    End If
                                Next
                            End If
                        End If

                    Catch ex As FaultException(Of SqlFaultInfo)
                        Utilities.SaveAppError(ex, Me.Parameters)
                        strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                        AddToNotLoaded(o, OrdersNotLoaded)
                        Continue For
                    Catch ex As Exception
                        Utilities.SaveAppError(ex, Me.Parameters)
                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                        AddToNotLoaded(o, OrdersNotLoaded)
                        strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because: <br />" & ex.ToString & "<br />" & vbCrLf)
                        Continue For
                    End Try

                    For Each t In lTrucks
                        'this method resized the required equipment to match smaller loads so that we use the smallest piece of equipment
                        'needed on loads that do not fill a full truck of larger capacity.
                        moveLoadToSmallestTruck(t)
                    Next
                    'save the changes
                    For Each t In lTrucks.Where(Function(x) x.TrackingState <> TrackingInfo.Unchanged)
                        Try
                            'If t.TrackingState <> TrackingInfo.Unchanged Then t.SaveChanges()
                            t.SaveChanges()
                        Catch ex As FaultException(Of SqlFaultInfo)
                            Utilities.SaveAppError(ex, Me.Parameters)
                            strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                        Catch ex As Exception
                            Utilities.SaveAppError(ex, Me.Parameters)
                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                            strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected system error: <br />" & ex.ToString & "<br />" & vbCrLf)
                        End Try
                    Next
                    'save the changes to any ready to tender loads
                    If Not TrucksReadyToTender Is Nothing AndAlso TrucksReadyToTender.Count > 0 Then
                        For Each t In TrucksReadyToTender
                            Try
                                t.SaveChanges()
                            Catch ex As FaultException(Of SqlFaultInfo)
                                Utilities.SaveAppError(ex, Me.Parameters)
                                strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                            Catch ex As Exception
                                Utilities.SaveAppError(ex, Me.Parameters)
                                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                                strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected system error: <br />" & ex.ToString & "<br />" & vbCrLf)
                            End Try
                        Next
                    End If
                Next

            Loop While (Not blnStopProcessing)
            'lock other threads
            SyncLock mSharedPadLock
                mSharedLoadsBeingRouted = False
            End SyncLock
            If Not strNotLoadedErrors Is Nothing AndAlso strNotLoadedErrors.Count > 0 Then
                Dim strEmailBody As String = String.Concat(strNotLoadedErrors.ToArray())
                Utilities.SaveAppError(strEmailBody, Me.Parameters)
                Utilities.SendToNGLEmailService("Warning! Some orders could not be routed", strEmailBody, Me.Parameters, True, False)
            End If
            If Not strUnexpectedErrors Is Nothing AndAlso strUnexpectedErrors.Count > 0 Then
                Dim strEmailBody As String = String.Concat(strUnexpectedErrors.ToArray())
                Utilities.SaveAppError(strEmailBody, Me.Parameters)
                Utilities.SendToNGLEmailService("System Error! Unexpected Routing Guide Exception", strEmailBody, Me.Parameters, False)
            End If
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Utilities.SendToNGLEmailService("System Error!  Unexpected Routing Guide Exception", "Unable to route order number, " & strCurrentOrder & ", using routing guide! <br />" & vbCrLf & ex.ToString, Me.Parameters, False)
        Finally
            SyncLock mSharedPadLock
                mSharedLoadsBeingRouted = False
            End SyncLock
        End Try
        Return OrdersNotLoaded

    End Function

    ''' <summary>
    ''' This method attempts to route a load according to the routing guide 
    ''' it returns true if the order was routed or false if the order could not be routed
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="strUnexpectedErrors"></param>
    ''' <param name="strNotLoadedErrors"></param>
    ''' <param name="OptimizeCapacity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.1 4/22/2016
    '''     To avoid invalid error messages the caller should test to see if 
    '''     the order has been assigned a routing guide number.  a reference  
    '''     to the record is stored in SolutionDetailRouteGuideControl.
    ''' Modified by RHR for v-7.0.5.110 9/27/16
    '''   removed the logic to send emails on error all errors are now returned to the caller
    '''   the caller will log the errors and generate alerts as expected 
    '''  Modified by RHR for v-8.3.0.001 on 09/15/2020 
    '''     added logic to exclude the current CNS number when routing new bookings
    ''' Modified by RHR for v-8.5.3.007 on 06/20/2023
    '''     added logic to use ERP Route Optimiztion 
    ''' </remarks>
    Public Function RouteLoad(ByRef o As DTO.tblSolutionDetail,
                                  ByRef strUnexpectedErrors As List(Of String),
                                  ByRef strNotLoadedErrors As List(Of String),
                                  ByVal OptimizeCapacity As Boolean,
                                  ByRef blnGetNewMiles As Boolean) As Boolean

        'enmRtCapPref
        '0 = Sequence
        '1 = Cases
        '2 = Weight
        '3 = Pallets
        '4 = Cubes   
        Dim strCurrentOrder As String = "{Not Found}"
        OrdersNotLoaded = New List(Of DTO.tblSolutionDetail)
        Dim blnLoaded As Boolean = False
        Try

            Dim enmRtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference = DataTransferObjects.tblSolutionTruck.RoutingCapacityPreference.Pallets
            Dim lTrucks As New List(Of DTO.tblSolutionTruck)
            Dim providerStaticRoute As NGLtblStaticRouteData = Me.NDPBaseClassFactory("NGLtblStaticRouteData")
            Dim BookItemData As NGLBookItemData = Me.NDPBaseClassFactory("NGLBookItemData")
            If strUnexpectedErrors Is Nothing Then strUnexpectedErrors = New List(Of String)
            If strNotLoadedErrors Is Nothing Then strNotLoadedErrors = New List(Of String)
            blnGetNewMiles = False
            ' Begin Modified by RHR for v-8.5.3.007 on 06/20/2023

            ' old validation removed.
            'If o Is Nothing OrElse o.SolutionDetailRouteGuideControl = 0 Then
            '    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it has not been assigned a routing guide.<br />" & vbCrLf)
            '    Return False
            'End If
            If o Is Nothing Then
                strNotLoadedErrors.Add("Cannot Route Order because it is not valid.<br />" & vbCrLf)
                Return False
            ElseIf Not String.IsNullOrWhiteSpace(o.SolutionDetailRouteGuideNumber) Then
                If o.SolutionDetailRouteGuideControl = 0 Then
                    ' when this happens we are using ERP Routes or the routing guide is not valid
                    If GetParValue("ERPRouteOptimizationEnabled", o.SolutionDetailCompControl) = 1 Then
                        'get the CNS from matching book records
                        Using db As New NGLMasBookDataContext(ConnectionString)
                            Try
                                Dim sRouteGuideNUmber = o.SolutionDetailRouteGuideNumber
                                Dim dblERPRouteingAssignCarrier = GetParValue("ERPRouteOptimizationAutoAssignCarrier", o.SolutionDetailCompControl)
                                'If The route guid number exists use the same CNS 
                                If (db.Books.Any(Function(x) x.BookRouteGuideNumber = sRouteGuideNUmber AndAlso (x.BookTranCode = "N" Or x.BookTranCode = "P"))) Then
                                    Dim sCNS = db.Books.Where(Function(x) x.BookRouteGuideNumber = sRouteGuideNUmber AndAlso (x.BookTranCode = "N" Or x.BookTranCode = "P")).Select(Function(y) y.BookConsPrefix).FirstOrDefault()
                                    If (Not String.IsNullOrWhiteSpace(sCNS) AndAlso sCNS <> o.SolutionDetailConsPrefix) Then
                                        Dim iBookControl = o.SolutionDetailBookControl
                                        Dim oBook = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                                        If Not oBook Is Nothing AndAlso oBook.BookControl = iBookControl Then
                                            oBook.BookConsPrefix = sCNS
                                            db.SubmitChanges()

                                        End If
                                    End If
                                End If
                                blnGetNewMiles = True ' tell the caller to get miles,  this can only be called from the BLL library
                                If dblERPRouteingAssignCarrier = 1 Then
                                    Return False ' the system will try to assign the lowest cost carrier
                                Else
                                    Return True ' we are done and do not need to assign a carrier in the calling procedure
                                End If
                            Catch ex As Exception
                                Return False
                            End Try
                        End Using
                    Else
                        ' when this happens we are using ERP Routes or the routing guide is not valid
                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it has not been assigned a routing guide.<br />" & vbCrLf)
                        Return False
                    End If
                End If

            ElseIf o.SolutionDetailRouteGuideControl = 0 Then
                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it has not been assigned a routing guide.<br />" & vbCrLf)
                Return False
            End If

            ' End Modified by RHR for v-8.5.3.007 on 06/20/2023

            strCurrentOrder = o.OrderNumber
            Try

                StaticRouteData = providerStaticRoute.GettblStaticRouteFiltered(o.SolutionDetailRouteGuideControl)
                If StaticRouteData Is Nothing Then
                    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because the system could not read the static route guide settings.<br />" & vbCrLf)
                    Return False
                End If
                o.Details = BookItemData.GetBookItemsFiltered(o.SolutionDetailBookLoadControl).ToList()

                enmRtCapPref = StaticRouteData.StaticRouteCapacityPreference
                'get a list of available carriers and their trucks, we pass zero for capacity to be sure we can route smaller loads together and that we can split orders.
                ListAvailableEquipment = GetCarriersForRoute(o.SolutionDetailCompControl, o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode, o.SolutionDetailTransType, o.SolutionDetailDestState, o.SolutionDetailDateLoad, o.SolutionDetailDateRequired, 0, 0, 0, 0, o.SolutionDetailCom, o.SolutionDetailIsHazmat).ToList
                If ListAvailableEquipment Is Nothing OrElse ListAvailableEquipment.Count < 1 Then
                    'note:  we need to add a message or attribute control for why this load was not loaded
                    'to be appended to the attributes list
                    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because no equipment types match the order requirements.<br />" & vbCrLf)
                    Return False
                End If
                'check the route type and filter the matching loads/trucks as needed
                ' Modified by RHR for v-8.3.0.001 added code to remove trucks with the same cns number
                Dim sThisCNS = o.SolutionDetailConsPrefix
                Select Case o.SolutionDetailRouteTypeCode
                    Case DTO.tblSolutionTruck.RouteTypeCodes.Single_LTL
                        lTrucks = Nothing
                        addNewTruck(lTrucks, enmRtCapPref, o)
                    Case DTO.tblSolutionTruck.RouteTypeCodes.Full_Load
                        'filter the list by address
                        Dim strLaneLocation As String = o.LaneLocation
                        'Modified by RHR for v-8.3.0.001 on 09/15/2020 added logic to exclude the current CNS number when routing new bookings
                        lTrucks = GetMatchingRoutedLoads(o.SolutionDetailCompControl, o.SolutionDetailSolutionTruckControl, o.SolutionDetailDateLoad, o.SolutionDetailTransType, "P", o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode).Where(Function(x) x.Orders(0).LaneLocation = strLaneLocation).ToList().Where(Function(x) x.CNS <> sThisCNS).ToList()
                    Case Else
                        'get all available loads that match
                        'Modified by RHR for v-8.3.0.001 on 09/15/2020 added logic to exclude the current CNS number when routing new bookings
                        lTrucks = GetMatchingRoutedLoads(o.SolutionDetailCompControl, o.SolutionDetailSolutionTruckControl, o.SolutionDetailDateLoad, o.SolutionDetailTransType, "P", o.SolutionDetailRouteGuideControl, o.SolutionDetailRouteTypeCode).ToList().Where(Function(x) x.CNS <> sThisCNS).ToList()
                End Select
                'if the list of matching loads is empty we need to add a default truck
                Dim lInvalidTrucks As New List(Of DTO.tblSolutionTruck)
                If lTrucks Is Nothing OrElse lTrucks.Count < 1 Then
                    addNewTruck(lTrucks, enmRtCapPref)
                Else
                    Dim oBookLoadControls As List(Of Integer) = (From t In lTrucks From l In t.Orders Select l.SolutionDetailBookLoadControl).ToList()
                    Dim oDetailsList As List(Of DTO.BookItem) = BookItemData.GetBookItemsFiltered(oBookLoadControls)
                    For Each t In lTrucks
                        t.Parameters = Me.Parameters
                        t.TruckDataObject = Me
                        t.BatchProcessingDataObject = New NGLBatchProcessDataProvider(Me.Parameters)
                        t.BookItemDataObject = New NGLBookItemData(Me.Parameters)
                        t.RouteConfig = GetRouteConfigForEquipment(Me.StaticRouteData.StaticRouteControl, t.SolutionTruckCarrierControl, t.SolutionTruckCarrierTruckControl)
                        If t.RouteConfig Is Nothing Then
                            lInvalidTrucks.Add(t)
                        Else
                            If Not t.Orders Is Nothing AndAlso t.Orders.Count > 0 Then
                                For Each order In t.Orders
                                    Dim intBookLoadControl = order.SolutionDetailBookLoadControl
                                    order.Details = (From d In oDetailsList Where d.BookItemBookLoadControl = intBookLoadControl Select d).ToList()
                                    'order.Details = BookItemData.GetBookItemsFiltered(order.SolutionDetailBookLoadControl).ToList
                                Next
                            End If
                        End If
                    Next
                End If
                If Not lInvalidTrucks Is Nothing AndAlso lInvalidTrucks.Count > 0 Then
                    For Each t In lInvalidTrucks
                        If lTrucks.Contains(t) Then lTrucks.Remove(t)
                    Next
                    If lTrucks Is Nothing OrElse lTrucks.Count < 1 Then
                        addNewTruck(lTrucks, enmRtCapPref)
                    End If
                End If

                Dim LN As New List(Of String)
                LN.Add(o.LaneLocation)

                If enmRtCapPref = DTO.tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                    If Not lTrucks Is Nothing AndAlso lTrucks.Count > 0 Then
                        Dim previousTruck As DTO.tblSolutionTruck = Nothing
                        Dim tCount = lTrucks.Count
                        Dim cCount = 0
                        For Each t In lTrucks.OrderBy(Function(x) x.MinSequence)
                            cCount += 1
                            If previousTruck Is Nothing Then
                                If cCount = tCount OrElse t.MaxSequence = 0 OrElse t.MaxSequence >= o.RouteSequence Then
                                    If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                        blnLoaded = True
                                        Exit For
                                    Else
                                        'If we get here we are out of trucks so no match is found
                                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                                        Return False
                                    End If
                                Else
                                    previousTruck = t
                                End If
                            Else
                                If cCount = tCount OrElse t.MinSequence = 0 OrElse t.MaxSequence = 0 Then
                                    If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                        blnLoaded = True
                                        Exit For
                                    Else
                                        'If we get here we are out of trucks so no match is found
                                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                                        Return False
                                    End If
                                Else
                                    If t.MinSequence > o.RouteSequence Then
                                        'try to put this order on the previous truck
                                        If previousTruck.masterBuild(o, lTrucks, enmRtCapPref) Then
                                            blnLoaded = True
                                            Exit For
                                        Else
                                            'try to put this order ont the current truck
                                            If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                blnLoaded = True
                                                Exit For
                                            Else
                                                'If we get here we are out of trucks so no match is found
                                                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                                                Return False
                                            End If
                                        End If

                                    ElseIf t.MinSequence = o.RouteSequence Then
                                        'this truck already has an order with this sequence so see if there is room for this order
                                        If t.masterBuild(o, lTrucks, enmRtCapPref, True) Then
                                            blnLoaded = True
                                            Exit For
                                        Else
                                            'try to put this order on the previous truck
                                            If previousTruck.masterBuild(o, lTrucks, enmRtCapPref, True) Then
                                                blnLoaded = True
                                                Exit For
                                            Else
                                                'try again to put this order on the current truck but this time without the match seq only flag
                                                If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                                    blnLoaded = True
                                                    Exit For
                                                Else
                                                    'If we get here we are out of trucks so no match is found
                                                    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                                                    Return False
                                                End If
                                            End If
                                        End If
                                    ElseIf t.MinSequence <= o.RouteSequence And t.MaxSequence >= o.RouteSequence Then
                                        If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                            blnLoaded = True
                                            Exit For
                                        Else
                                            'If we get here we are out of trucks so no match is found
                                            strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                                            Return False
                                        End If
                                    Else
                                        previousTruck = t
                                    End If
                                End If
                            End If
                        Next
                    End If
                Else
                    'we group orders by destination
                    'Build a list of trucks that have orders with the same lane number (typically one but may be more if more than a full truck load of orders exists for any given lane)
                    Dim MasterBuild = lTrucks.Where(Function(p) p.Orders.Any(Function(y) LN.Contains(y.LaneLocation)))
                    If Not MasterBuild Is Nothing AndAlso MasterBuild.Count > 0 Then
                        'we use a for loop because items may be added or removed from the collection
                        For truck = 0 To MasterBuild.Count - 1
                            Dim t As DTO.tblSolutionTruck = MasterBuild(truck)
                            If t.masterBuild(o, lTrucks, enmRtCapPref) Then
                                blnLoaded = True
                                Exit For
                            Else
                                'master build failed, trying next truck
                            End If
                        Next
                    End If
                    'If no match found then try to fit the load on any truck available sorted by trucks with the smallest capacity
                    If Not blnLoaded Then
                        For Each t In lTrucks
                            If t.addOrder(o, enmRtCapPref) Then
                                blnLoaded = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                'if we get here no trucks are available so create a new truck
                If Not blnLoaded Then
                    Dim newTruck As DTO.tblSolutionTruck = addNewTruck(lTrucks, enmRtCapPref, o, True)
                    'Modified by RHR 2/19/13 we do not need to call lTrucks.Add after addNewTruck because 
                    'it is already added to the collection.
                    'lTrucks.Add(newTruck)
                    If Not newTruck.addOrderWSplit(o, lTrucks, enmRtCapPref) Then
                        If newTruck.Orders Is Nothing OrElse newTruck.Orders.Count < 1 Then lTrucks.Remove(newTruck)
                        'If we get here we cannot use this list of trucks so no match is found
                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because it did not meet the routing guide requirements; check equipment and capacity settings.<br />" & vbCrLf)
                        Return False
                    End If
                End If
                'optimize capacity and equipment size


                If enmRtCapPref <> DTO.tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                    'optimize capacity
                    If Me.StaticRouteData.StaticRouteFillLargestFirst Then
                        'try to move orders from smaller loads onto the larger trucks
                        For Each t In lTrucks.OrderByDescending(Function(x) x.TotalWgt).Where(Function(y) y.atCapacity = False)
                            Dim spaceleft As Double = 0
                            Dim strCNS As String = t.CNS
                            Dim dblMaxWeight As Double = t.maxWgt
                            Dim blnMatchFound As Boolean = False
                            Dim trucksleft As List(Of DTO.tblSolutionTruck) = (From lt In lTrucks Where ((lt.maxWgt = dblMaxWeight And lt.atCapacity = False) Or (lt.maxWgt < dblMaxWeight)) And lt.CNS <> strCNS Order By lt.maxWgt Select lt).ToList
                            If Not trucksleft Is Nothing AndAlso trucksleft.Count > 0 Then
                                optimizeLoadByCapacity(t, trucksleft, enmRtCapPref)
                            End If
                        Next
                    Else
                        'optimize for smallest truck
                        'try to move orders from non full larger trucks to fill smaller trucks (later will will call the resize equipment requirement to maximize cost by size)
                        For Each t In lTrucks.OrderBy(Function(x) x.TotalWgt).Where(Function(y) y.atCapacity = False)
                            Dim spaceleft As Double = 0
                            Dim strCNS As String = t.CNS
                            Dim dblMaxWeight As Double = t.maxWgt
                            Dim blnMatchFound As Boolean = False
                            Dim trucksleft As List(Of DTO.tblSolutionTruck) = (From lt In lTrucks Where lt.atCapacity = False And lt.CNS <> strCNS Order By lt.maxWgt Descending Select lt).ToList
                            If Not trucksleft Is Nothing AndAlso trucksleft.Count > 0 Then
                                optimizeLoadByCapacity(t, trucksleft, enmRtCapPref)
                            End If
                        Next
                    End If
                End If

            Catch ex As FaultException(Of SqlFaultInfo)
                Utilities.SaveAppError(ex, Me.Parameters)
                strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                Return False
            Catch ex As Exception
                Utilities.SaveAppError(ex, Me.Parameters)
                strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because: <br />" & ex.ToString & "<br />" & vbCrLf)
                Return False
            End Try

            'TODO:  all the exception handlers below need to be evaluated to determine if we return true or false?

            For Each t In lTrucks
                'this method resized the required equipment to match smaller loads so that we use the smallest piece of equipment
                'needed on loads that do not fill a full truck of larger capacity.
                moveLoadToSmallestTruck(t)
            Next
            'save the changes
            For Each t In lTrucks.Where(Function(x) x.TrackingState <> TrackingInfo.Unchanged)
                Try
                    'If t.TrackingState <> TrackingInfo.Unchanged Then t.SaveChanges()
                    t.SaveChanges()
                Catch ex As FaultException(Of SqlFaultInfo)
                    Utilities.SaveAppError(ex, Me.Parameters)
                    strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                Catch ex As Exception
                    Utilities.SaveAppError(ex, Me.Parameters)
                    strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                    strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected system error: <br />" & ex.ToString & "<br />" & vbCrLf)
                End Try
            Next
            'save the changes to any ready to tender loads
            If Not TrucksReadyToTender Is Nothing AndAlso TrucksReadyToTender.Count > 0 Then
                For Each t In TrucksReadyToTender
                    Try
                        t.SaveChanges()
                    Catch ex As FaultException(Of SqlFaultInfo)
                        Utilities.SaveAppError(ex, Me.Parameters)
                        strNotLoadedErrors.Add(String.Format("Cannot Load Order Number, {0}, because: <br />Reason: {1} <br />Message: {2} <br />Details: {3} <br />{4}", o.OrderNumber, ex.Reason, ex.Detail.Message, ex.Detail.Details, vbCrLf))
                    Catch ex As Exception
                        Utilities.SaveAppError(ex, Me.Parameters)
                        strNotLoadedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected error: <br />" & ex.Message & "<br />" & vbCrLf)
                        strUnexpectedErrors.Add("Cannot Route Order Number, " & o.OrderNumber & ", because of an unexpected system error: <br />" & ex.ToString & "<br />" & vbCrLf)
                    End Try
                Next
            End If
            'Modified by RHR for v-7.0.5.110 9/27/16
            'Begin
            '************************************************************************************
            'If Not strNotLoadedErrors Is Nothing AndAlso strNotLoadedErrors.Count > 0 Then
            '    Dim strEmailBody As String = String.Concat(strNotLoadedErrors.ToArray())
            '    Utilities.SaveAppError(strEmailBody, Me.Parameters)
            '    Utilities.SendToNGLEmailService("Warning! Some orders could not be routed", strEmailBody, Me.Parameters, True, False)
            'End If
            'If Not strUnexpectedErrors Is Nothing AndAlso strUnexpectedErrors.Count > 0 Then
            '    Dim strEmailBody As String = String.Concat(strUnexpectedErrors.ToArray())
            '    Utilities.SaveAppError(strEmailBody, Me.Parameters)
            '    Utilities.SendToNGLEmailService("System Error! Unexpected Routing Guide Exception", strEmailBody, Me.Parameters, False)
            'End If

        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            strUnexpectedErrors.Add("System Error! Unexpected Routing Guide Exception Unable to route order number, " & strCurrentOrder & ", using routing guide! <br />" & ex.Message & "<br />" & vbCrLf)
            'Utilities.SendToNGLEmailService("System Error!  Unexpected Routing Guide Exception", "Unable to route order number, " & strCurrentOrder & ", using routing guide! <br />" & vbCrLf & ex.Message, Me.Parameters, False)
        Finally

        End Try
        '***************************************************************
        'End change 9/27/16
        If blnLoaded Then Return True

        If Not OrdersNotLoaded Is Nothing AndAlso OrdersNotLoaded.Count > 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function GetRouteConfigForEquipment(ByVal RouteControl As Integer, ByVal CarrierControl As Integer, ByVal CarrierTruckControl As Integer) As DTO.RouteConfigForEquip
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the first record that matches the criteria in TruckFilter
                Dim RouteConfig As DTO.RouteConfigForEquip = (From d In db.spGetRouteConfigForEquipment(RouteControl, CarrierControl, CarrierTruckControl)
                                                              Select New DTO.RouteConfigForEquip With {.StaticRouteControl = d.StaticRouteControl _
                                                                                                          , .StaticRouteCarrControl = d.StaticRouteCarrControl _
                                                                                                          , .StaticRouteEquipControl = d.StaticRouteEquipControl _
                                                                                                          , .StaticRouteAutoTenderFlag = d.StaticRouteAutoTenderFlag _
                                                                                                          , .StaticRouteUseShipDateFlag = d.StaticRouteUseShipDateFlag _
                                                                                                          , .StaticRouteGuideDateSelectionDaysBefore = d.StaticRouteGuideDateSelectionDaysBefore _
                                                                                                          , .StaticRouteGuideDateSelectionDaysAfter = d.StaticRouteGuideDateSelectionDaysAfter _
                                                                                                          , .StaticRouteSplitOversizedLoads = d.StaticRouteSplitOversizedLoads _
                                                                                                          , .StaticRouteCapacityPreference = d.StaticRouteCapacityPreference _
                                                                                                          , .StaticRouteRequireAutoTenderApproval = d.StaticRouteRequireAutoTenderApproval _
                                                                                                          , .StaticRouteFillLargestFirst = d.StaticRouteFillLargestFirst _
                                                                                                          , .StaticRoutePlaceOnHold = d.StaticRoutePlaceOnHold _
                                                                                                          , .StaticRouteCarrCarrierControl = d.StaticRouteCarrCarrierControl _
                                                                                                          , .StaticRouteCarrRouteTypeCode = d.StaticRouteCarrRouteTypeCode _
                                                                                                          , .StaticRouteCarrAutoTenderFlag = d.StaticRouteCarrAutoTenderFlag _
                                                                                                          , .StaticRouteCarrTendLeadTime = d.StaticRouteCarrTendLeadTime _
                                                                                                          , .StaticRouteCarrMaxStops = d.StaticRouteCarrMaxStops _
                                                                                                          , .StaticRouteCarrHazmatFlag = d.StaticRouteCarrHazmatFlag _
                                                                                                          , .StaticRouteCarrTransType = d.StaticRouteCarrTransType _
                                                                                                          , .StaticRouteCarrRouteSequence = d.StaticRouteCarrRouteSequence _
                                                                                                          , .StaticRouteCarrRequireAutoTenderApproval = d.StaticRouteCarrRequireAutoTenderApproval _
                                                                                                          , .StaticRouteCarrAutoAcceptLoads = d.StaticRouteCarrAutoAcceptLoads _
                                                                                                          , .StaticRouteStateFilter = d.StaticRouteStateFilter}).FirstOrDefault




                Return RouteConfig

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.ToString, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.ToString, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "The route equipment configuration has not been completed for route " & RouteControl.ToString, .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.ToString, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub AddToNotLoaded(ByRef o As DTO.tblSolutionDetail, ByRef OrdersNotLoaded As List(Of DTO.tblSolutionDetail))

        If OrdersNotLoaded Is Nothing Then OrdersNotLoaded = New List(Of DTO.tblSolutionDetail)
        If Not OrdersNotLoaded.Contains(o) Then OrdersNotLoaded.Add(o)

    End Sub

    Public Sub AddToReadyToTender(ByRef t As DTO.tblSolutionTruck)

        If TrucksReadyToTender Is Nothing Then TrucksReadyToTender = New List(Of DTO.tblSolutionTruck)
        If Not TrucksReadyToTender.Contains(t) Then TrucksReadyToTender.Add(t)

    End Sub

    Public Sub optimizeLoadByCapacity(ByRef t As DTO.tblSolutionTruck, ByRef trucksleft As List(Of DTO.tblSolutionTruck), ByVal enmRtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference)
        Dim blnMatchFound As Boolean = False
        Dim strCNS As String = t.CNS
        Dim dblMaxWeight As Double = t.maxWgt
        Dim spaceleft As Double = 0

        Do
            blnMatchFound = False
            Select Case enmRtCapPref
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cases
                    For Each tl In trucksleft
                        Dim movedLanes As New List(Of String)
                        Dim movedOrders As New List(Of DTO.tblSolutionDetail)
                        For Each o In tl.Orders
                            If Not movedLanes.Contains(o.LaneLocation) Then
                                Dim ordersByLane = tl.consolidateOrders(o, o.LaneLocation)
                                If Not ordersByLane Is Nothing AndAlso ordersByLane.Sum(Function(x) x.Cases) <= (t.maxCases - t.TotalCases) Then
                                    Dim OrderSummary As DTO.clsOrderSummary = tl.getOrderSummary(ordersByLane, o.LaneLocation)
                                    If Not OrderSummary Is Nothing Then
                                        'try to add the orders to this truck
                                        If t.addOrders(ordersByLane, OrderSummary, enmRtCapPref, False) Then
                                            blnMatchFound = True
                                            If Not movedLanes.Contains(o.LaneLocation) Then movedLanes.Add(o.LaneLocation)
                                            For Each movedOrder In ordersByLane
                                                If Not movedOrders.Contains(movedOrder) Then movedOrders.Add(movedOrder)
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        For Each movedOrder In movedOrders
                            If tl.Orders.Contains(movedOrder) Then
                                tl.Orders.Remove(movedOrder)
                                tl.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                    Next
                    spaceleft = t.maxCases - t.TotalCases
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Weight
                    For Each tl In trucksleft
                        Dim movedLanes As New List(Of String)
                        Dim movedOrders As New List(Of DTO.tblSolutionDetail)
                        For Each o In tl.Orders
                            If Not movedLanes.Contains(o.LaneLocation) Then
                                Dim ordersByLane = tl.consolidateOrders(o, o.LaneLocation)
                                If Not ordersByLane Is Nothing AndAlso ordersByLane.Sum(Function(x) x.Wgt) <= (t.maxWgt - t.TotalWgt) Then
                                    Dim OrderSummary As DTO.clsOrderSummary = tl.getOrderSummary(ordersByLane, o.LaneLocation)
                                    If Not OrderSummary Is Nothing Then
                                        'try to add the orders to this truck
                                        If t.addOrders(ordersByLane, OrderSummary, enmRtCapPref, False) Then
                                            blnMatchFound = True
                                            If Not movedLanes.Contains(o.LaneLocation) Then movedLanes.Add(o.LaneLocation)
                                            For Each movedOrder In ordersByLane
                                                If Not movedOrders.Contains(movedOrder) Then movedOrders.Add(movedOrder)
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        For Each movedOrder In movedOrders
                            If tl.Orders.Contains(movedOrder) Then
                                tl.Orders.Remove(movedOrder)
                                tl.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                    Next
                    spaceleft = t.maxWgt - t.TotalWgt
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cubes
                    For Each tl In trucksleft
                        Dim movedLanes As New List(Of String)
                        Dim movedOrders As New List(Of DTO.tblSolutionDetail)
                        For Each o In tl.Orders
                            If Not movedLanes.Contains(o.LaneLocation) Then
                                Dim ordersByLane = tl.consolidateOrders(o, o.LaneLocation)
                                If Not ordersByLane Is Nothing AndAlso ordersByLane.Sum(Function(x) x.Cubes) <= (t.maxCubes - t.TotalCubes) Then
                                    Dim OrderSummary As DTO.clsOrderSummary = tl.getOrderSummary(ordersByLane, o.LaneLocation)
                                    If Not OrderSummary Is Nothing Then
                                        'try to add the orders to this truck
                                        If t.addOrders(ordersByLane, OrderSummary, enmRtCapPref, False) Then
                                            blnMatchFound = True
                                            If Not movedLanes.Contains(o.LaneLocation) Then movedLanes.Add(o.LaneLocation)
                                            For Each movedOrder In ordersByLane
                                                If Not movedOrders.Contains(movedOrder) Then movedOrders.Add(movedOrder)
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        For Each movedOrder In movedOrders
                            If tl.Orders.Contains(movedOrder) Then
                                tl.Orders.Remove(movedOrder)
                                tl.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                    Next
                    spaceleft = t.maxCubes - t.TotalCubes
                Case Else
                    For Each tl In trucksleft
                        Dim movedLanes As New List(Of String)
                        Dim movedOrders As New List(Of DTO.tblSolutionDetail)
                        For Each o In tl.Orders
                            If Not movedLanes.Contains(o.LaneLocation) Then
                                Dim ordersByLane = tl.consolidateOrders(o, o.LaneLocation)
                                If Not ordersByLane Is Nothing AndAlso ordersByLane.Sum(Function(x) x.Plts) <= (t.maxPLTs - t.TotalPlts) Then
                                    Dim OrderSummary As DTO.clsOrderSummary = tl.getOrderSummary(ordersByLane, o.LaneLocation)
                                    If Not OrderSummary Is Nothing Then
                                        'try to add the orders to this truck
                                        If t.addOrders(ordersByLane, OrderSummary, enmRtCapPref, False) Then
                                            blnMatchFound = True
                                            If Not movedLanes.Contains(o.LaneLocation) Then movedLanes.Add(o.LaneLocation)
                                            For Each movedOrder In ordersByLane
                                                If Not movedOrders.Contains(movedOrder) Then movedOrders.Add(movedOrder)
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        For Each movedOrder In movedOrders
                            If tl.Orders.Contains(movedOrder) Then
                                tl.Orders.Remove(movedOrder)
                                tl.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                    Next
                    spaceleft = t.maxPLTs - t.TotalPlts
            End Select
        Loop While blnMatchFound And spaceleft > 0
    End Sub

    Public Function getTempCNSNbr() As String
        Return "TempCNS00" & TempCNSSeed
    End Function

    Public Function addNewTruck(ByRef lTrucks As List(Of DTO.tblSolutionTruck), ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, Optional ByRef o As DTO.tblSolutionDetail = Nothing, Optional ByVal GetLargest As Boolean = False) As DTO.tblSolutionTruck
        If lTrucks Is Nothing Then lTrucks = New List(Of DTO.tblSolutionTruck)
        Dim newTruck As DTO.tblSolutionTruck = getNewTruck(RtCapPref, New NGLBatchProcessDataProvider(Me.Parameters), New NGLBookItemData(Me.Parameters), o, GetLargest)
        lTrucks.Add(newTruck)
        Return newTruck

    End Function

    Public Function getNewTruck(ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, ByRef BatchProcessingDataObject As NGLBatchProcessDataProvider, ByRef BookItemDataObject As NGLBookItemData, Optional ByRef o As DTO.tblSolutionDetail = Nothing, Optional ByVal GetLargest As Boolean = False) As DTO.tblSolutionTruck

        If Not o Is Nothing Then
            Return getNewTruck(RtCapPref, New List(Of DTO.tblSolutionDetail) From {o}, BatchProcessingDataObject, BookItemDataObject, GetLargest)
        End If
        Dim e As DTO.CarriersForRoute
        Dim newTruck As New DTO.tblSolutionTruck(Me.Parameters, Me)
        If ListAvailableEquipment Is Nothing OrElse ListAvailableEquipment.Count < 1 Then Return newTruck
        If GetLargest Then
            e = (From d In ListAvailableEquipment Order By d.CarrierTruckMaxWgt Descending, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
        Else
            e = (From d In ListAvailableEquipment Order By d.CarrierTruckMaxWgt, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
        End If
        If Not e Is Nothing Then newTruck.updateEquipmentInfo(e)
        Return newTruck
    End Function

    Public Function getNewTruck(ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, ByRef items As List(Of DTO.tblSolutionDetail), ByRef BatchProcessingDataObject As NGLBatchProcessDataProvider, ByRef BookItemDataObject As NGLBookItemData, Optional ByVal GetLargest As Boolean = False) As DTO.tblSolutionTruck

        Dim e As DTO.CarriersForRoute
        Dim newTruck As New DTO.tblSolutionTruck(Me.Parameters, Me, BatchProcessingDataObject, BookItemDataObject)
        If ListAvailableEquipment Is Nothing OrElse ListAvailableEquipment.Count < 1 Then Return newTruck
        If GetLargest Then
            'just get the largest truck 
            e = (From d In ListAvailableEquipment Order By d.CarrierTruckMaxWgt Descending, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
        Else
            'find the best truck that will hold the orders
            Select Case RtCapPref
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cases
                    Dim totalCases = items.Sum(Function(x) x.Cases)
                    'get the smallest piece of equipment that can hold the items based on the capaciy setting
                    e = (From d In ListAvailableEquipment Where d.CarrierTruckMaxCases >= totalCases Order By d.CarrierTruckMaxCases, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Weight
                    Dim totalWgt = items.Sum(Function(x) x.Wgt)
                    'get the smallest piece of equipment that can hold the items based on the capaciy setting
                    e = (From d In ListAvailableEquipment Where d.CarrierTruckMaxWgt >= totalWgt Order By d.CarrierTruckMaxWgt, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cubes
                    Dim totalCubes = items.Sum(Function(x) x.Cubes)
                    'get the smallest piece of equipment that can hold the items based on the capaciy setting
                    e = (From d In ListAvailableEquipment Where d.CarrierTruckMaxCubes >= totalCubes Order By d.CarrierTruckMaxCubes, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
                Case Else
                    'we use pallets
                    Dim totalPallets = items.Sum(Function(x) x.Plts)
                    'get the smallest piece of equipment that can hold the items based on the capaciy setting
                    e = (From d In ListAvailableEquipment Where d.CarrierTruckMaxPlts >= totalPallets Order By d.CarrierTruckMaxPlts, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
            End Select
            If e Is Nothing Then
                'just get the largest truck the system should split the load to fit as needed
                e = (From d In ListAvailableEquipment Order By d.CarrierTruckMaxWgt Descending, d.StaticRouteCarrRouteSequence Select d).FirstOrDefault
            End If
        End If
        If Not e Is Nothing Then newTruck.updateEquipmentInfo(e)
        Return newTruck
    End Function

    Public Function doesLargerTruckExist(ByVal smallTruck As DTO.tblSolutionTruck, ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, Optional ByVal spaceNeeded As Double = 0) As Boolean

        For Each e In ListAvailableEquipment.OrderBy(Function(x) x.CarrierTruckMaxWgt)
            Select Case RtCapPref
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cases
                    If e.CarrierTruckMaxCases > smallTruck.maxCases AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxCases >= spaceNeeded) Then Return True
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Weight
                    If e.CarrierTruckMaxWgt > smallTruck.maxWgt AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxWgt >= spaceNeeded) Then Return True
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cubes
                    If e.CarrierTruckMaxCubes > smallTruck.maxCubes AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxCubes >= spaceNeeded) Then Return True
                Case Else
                    'we use pallets
                    If e.CarrierTruckMaxPlts > smallTruck.maxPLTs AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxPlts >= spaceNeeded) Then Return True
            End Select
        Next

        Return False


    End Function

    ''' <summary>
    ''' We always use weight to determine if this is the largest truck
    ''' </summary>
    ''' <param name="thisTruck"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isLargestTruck(ByVal thisTruck As DTO.tblSolutionTruck) As Boolean

        Try
            'here we only hace 3 types in produciton we would use a linq query to select the next truck fromn a list of available trucks
            For Each e In ListAvailableEquipment.Where(Function(x) x.CarrierTruckMaxWgt > thisTruck.maxWgt)
                Return False 'if any trucks exist

            Next
        Catch ex As Exception
            'do nothing
        End Try
        'if we get here not trucks are larger so return true
        Return True


    End Function

    Public Function sortTrucksByCapacity(ByRef trucks As List(Of DTO.tblSolutionTruck), ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, Optional ByVal decending As Boolean = False) As List(Of DTO.tblSolutionTruck)
        Select Case RtCapPref
            Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cases
                If decending Then
                    Return trucks.OrderByDescending(Function(s) s.TotalCases).ToList
                Else
                    Return trucks.OrderBy(Function(s) s.TotalCases).ToList
                End If
            Case DTO.tblSolutionTruck.RoutingCapacityPreference.Weight
                If decending Then
                    Return trucks.OrderByDescending(Function(s) s.TotalWgt).ToList
                Else
                    Return trucks.OrderBy(Function(s) s.TotalWgt).ToList
                End If

            Case DTO.tblSolutionTruck.RoutingCapacityPreference.Pallets
                If decending Then
                    Return trucks.OrderByDescending(Function(s) s.TotalPlts).ToList
                Else
                    Return trucks.OrderBy(Function(s) s.TotalPlts).ToList
                End If

            Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cubes
                If decending Then
                    Return trucks.OrderByDescending(Function(s) s.TotalCubes).ToList
                Else
                    Return trucks.OrderBy(Function(s) s.TotalCubes).ToList
                End If

            Case DTO.tblSolutionTruck.RoutingCapacityPreference.Sequence
                If decending Then
                    Return trucks.OrderByDescending(Function(s) s.TotalPlts).ToList
                Else
                    Return trucks.OrderBy(Function(s) s.TotalPlts).ToList
                End If

            Case Else
                Return trucks
        End Select
    End Function

    Public Function moveLoadToNextLargerTruck(ByRef smallTruck As DTO.tblSolutionTruck, ByVal RtCapPref As DTO.tblSolutionTruck.RoutingCapacityPreference, Optional ByVal spaceNeeded As Double = 0) As Boolean

        For Each e In ListAvailableEquipment.OrderBy(Function(x) x.CarrierTruckMaxWgt)
            Select Case RtCapPref
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cases
                    If e.CarrierTruckMaxCases > smallTruck.maxCases AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxCases >= spaceNeeded) Then
                        smallTruck.updateEquipmentInfo(e)
                        Return True
                    End If
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Weight
                    If e.CarrierTruckMaxWgt > smallTruck.maxWgt AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxWgt >= spaceNeeded) Then
                        smallTruck.updateEquipmentInfo(e)
                        Return True
                    End If
                Case DTO.tblSolutionTruck.RoutingCapacityPreference.Cubes
                    If e.CarrierTruckMaxCubes > smallTruck.maxCubes AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxCubes >= spaceNeeded) Then
                        smallTruck.updateEquipmentInfo(e)
                        Return True
                    End If
                Case Else
                    'we use pallets
                    If e.CarrierTruckMaxPlts > smallTruck.maxPLTs AndAlso (spaceNeeded = 0 OrElse e.CarrierTruckMaxPlts >= spaceNeeded) Then
                        smallTruck.updateEquipmentInfo(e)
                        Return True
                    End If
            End Select
        Next
        Return False


    End Function

    Public Function moveLoadToSmallestTruck(ByRef largeTruck As DTO.tblSolutionTruck) As Boolean

        For Each e In ListAvailableEquipment.OrderBy(Function(x) x.CarrierTruckMaxWgt) 'order by smallest equipment first
            If e.CarrierTruckMaxCases >= largeTruck.TotalCases _
                    AndAlso e.CarrierTruckMaxWgt >= largeTruck.TotalWgt _
                    AndAlso e.CarrierTruckMaxCubes >= largeTruck.TotalCubes _
                    AndAlso e.CarrierTruckMaxPlts >= largeTruck.TotalPlts _
                    AndAlso
                    (e.CarrierTruckMaxCases < largeTruck.maxCases Or e.CarrierTruckMaxCubes < largeTruck.maxCubes Or e.CarrierTruckMaxPlts < largeTruck.maxPLTs Or e.CarrierTruckMaxWgt < e.CarrierTruckMaxWgt) Then
                largeTruck.updateEquipmentInfo(e)
                Return True
            End If
        Next
        Return False


    End Function

#End Region


#Region "LTS & 365 Updates"


    Public Function GetLoadPlanningTrucks365Filtered(ByVal filters As Models.AllFilters, Optional ByVal blnReadNGLMessages As Boolean = True) As DTO.tblSolutionTruck()

        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oFDetails As New Models.FilterDetails()
            Dim CompNameFrom As String = ""
            Dim CompNameTo As String = ""
            copyAllFilterDataToProperties(filters, CompNameFrom, CompNameTo, "CompName")
            Dim CompNumberFrom As Integer?
            Dim CompNumberTo As Integer?
            copyAllFilterDataToProperties(filters, CompNumberFrom, CompNumberTo, "CompNumber")
            Dim CarrierNameFrom As String = ""
            Dim CarrierNameTo As String = ""
            copyAllFilterDataToProperties(filters, CarrierNameFrom, CarrierNameTo, "CarrierName")
            Dim CarrierNumberFrom As Integer?
            Dim CarrierNumberTo As Integer?
            copyAllFilterDataToProperties(filters, CarrierNumberFrom, CarrierNumberTo, "CarrierNumber")
            Dim BookDateLoadFrom As Date?
            Dim BookDateLoadTo As Date?
            copyAllFilterDataToProperties(filters, BookDateLoadFrom, BookDateLoadTo, "BookDateLoad")
            Dim BookDateRequiredFrom As Date?
            Dim BookDateRequiredTo As Date?
            copyAllFilterDataToProperties(filters, BookDateRequiredFrom, BookDateRequiredTo, "BookDateRequired")
            Dim BookOrigNameFrom As String = ""
            Dim BookOrigNameTo As String = ""
            copyAllFilterDataToProperties(filters, BookOrigNameFrom, BookOrigNameTo, "BookOrigName")
            Dim BookOrigAddress1From As String = ""
            Dim BookOrigAddress1To As String = ""
            copyAllFilterDataToProperties(filters, BookOrigAddress1From, BookOrigAddress1To, "BookOrigAddress1")
            Dim BookOrigCityFrom As String = ""
            Dim BookOrigCityTo As String = ""
            copyAllFilterDataToProperties(filters, BookOrigCityFrom, BookOrigCityTo, "BookOrigCity")
            Dim BookOrigZipFrom As String = ""
            Dim BookOrigZipTo As String = ""
            copyAllFilterDataToProperties(filters, BookOrigZipFrom, BookOrigZipTo, "BookOrigZip")
            Dim BookOrigStateFrom As String = ""
            Dim BookOrigStateTo As String = ""
            copyAllFilterDataToProperties(filters, BookOrigStateFrom, BookOrigStateTo, "BookOrigState")
            Dim BookOrigState2From As String = ""
            Dim BookOrigState2To As String = ""
            copyAllFilterDataToProperties(filters, BookOrigState2From, BookOrigState2To, "BookOrigState2")
            Dim BookOrigCountryFrom As String = ""
            Dim BookOrigCountryTo As String = ""
            copyAllFilterDataToProperties(filters, BookOrigCountryFrom, BookOrigCountryTo, "BookOrigCountry")
            Dim BookDestNameFrom As String = ""
            Dim BookDestNameTo As String = ""
            copyAllFilterDataToProperties(filters, BookDestNameFrom, BookDestNameTo, "BookDestName")
            Dim BookDestAddress1From As String = ""
            Dim BookDestAddress1To As String = ""
            copyAllFilterDataToProperties(filters, BookDestAddress1From, BookDestAddress1To, "BookDestAddress1")
            Dim BookDestCityFrom As String = ""
            Dim BookDestCityTo As String = ""
            copyAllFilterDataToProperties(filters, BookDestCityFrom, BookDestCityTo, "BookDestCity")
            Dim BookDestZipFrom As String = ""
            Dim BookDestZipTo As String = ""
            copyAllFilterDataToProperties(filters, BookDestZipFrom, BookDestZipTo, "BookDestZip")
            Dim BookDestStateFrom As String = ""
            Dim BookDestStateTo As String = ""
            copyAllFilterDataToProperties(filters, BookDestStateFrom, BookDestStateTo, "BookDestState")
            Dim BookDestState2From As String = ""
            Dim BookDestState2To As String = ""
            copyAllFilterDataToProperties(filters, BookDestState2From, BookDestState2To, "BookDestState2")
            Dim BookDestCountryFrom As String = ""
            Dim BookDestCountryTo As String = ""
            copyAllFilterDataToProperties(filters, BookDestCountryFrom, BookDestCountryTo, "BookDestCountry")
            Dim BookTransTypeFrom As String = ""
            Dim BookTransTypeTo As String = ""
            copyAllFilterDataToProperties(filters, BookTransTypeFrom, BookTransTypeTo, "BookTransType")
            Dim BookTranCodeFrom As String = "P"
            Dim BookTranCodeTo As String = "P"
            copyAllFilterDataToProperties(filters, BookTranCodeFrom, BookTranCodeTo, "BookTranCode")
            Dim BookConsPrefixFrom As String = ""
            Dim BookConsPrefixTo As String = ""
            copyAllFilterDataToProperties(filters, BookConsPrefixFrom, BookConsPrefixTo, "BookConsPrefix")
            Dim BookSHIDFrom As String = ""
            Dim BookSHIDTo As String = ""
            copyAllFilterDataToProperties(filters, BookSHIDFrom, BookSHIDTo, "BookSHID")
            Dim LaneNameFrom As String = ""
            Dim LaneNameTo As String = ""
            copyAllFilterDataToProperties(filters, LaneNameFrom, LaneNameTo, "LaneName")
            Dim LaneNumberFrom As String = ""
            Dim LaneNumberTo As String = ""
            copyAllFilterDataToProperties(filters, LaneNumberFrom, LaneNumberTo, "LaneNumber")
            Dim UserSecurityControl As Integer = Me.Parameters.UserControl
            Dim UserLEControl As Integer = Me.Parameters.UserLEControl
            Dim page As Integer = filters.page
            Dim pagesize As Integer = 1000
            If filters.FilterValues.Any(Function(x) x.filterName = "PageSize") Then
                Dim sPageSize = filters.FilterValues.Where(Function(x) x.filterName = "PageSize").Select(Function(x) x.filterValueFrom).FirstOrDefault()
                Integer.TryParse(sPageSize, pagesize)
            End If
            If pagesize = 0 Then
                pagesize = filters.take
            End If


            Try
                db.Log = New DebugTextWriter
                'Return all the records that match the criteria in TruckFilter
                Dim tblSolutionTrucks() As DTO.tblSolutionTruck = (
                    From d In db.spGetLoadPlanningTruckData365(CompNameFrom _
                        , CompNumberFrom _
                        , CompNameTo _
                        , CompNumberTo _
                        , CarrierNameFrom _
                        , CarrierNumberFrom _
                        , CarrierNameTo _
                        , CarrierNumberTo _
                        , BookDateLoadFrom _
                        , BookDateLoadTo _
                        , BookDateRequiredFrom _
                        , BookDateRequiredTo _
                        , BookOrigNameFrom _
                        , BookOrigNameTo _
                        , BookOrigAddress1From _
                        , BookOrigAddress1To _
                        , BookOrigCityFrom _
                        , BookOrigCityTo _
                        , BookOrigZipFrom _
                        , BookOrigZipTo _
                        , BookOrigStateFrom _
                        , BookOrigStateTo _
                        , BookOrigState2From _
                        , BookOrigState2To _
                        , BookOrigCountryFrom _
                        , BookOrigCountryTo _
                        , BookDestNameFrom _
                        , BookDestNameTo _
                        , BookDestAddress1From _
                        , BookDestAddress1To _
                        , BookDestCityFrom _
                        , BookDestCityTo _
                        , BookDestZipFrom _
                        , BookDestZipTo _
                        , BookDestStateFrom _
                        , BookDestStateTo _
                        , BookDestState2From _
                        , BookDestState2To _
                        , BookDestCountryFrom _
                        , BookDestCountryTo _
                        , BookTransTypeFrom _
                        , BookTransTypeTo _
                        , BookTranCodeFrom _
                        , BookTranCodeTo _
                        , BookConsPrefixFrom _
                        , BookConsPrefixTo _
                        , BookSHIDFrom _
                        , BookSHIDTo _
                        , LaneNameFrom _
                        , LaneNameTo _
                        , LaneNumberFrom _
                        , LaneNumberTo _
                        , UserSecurityControl _
                        , UserLEControl _
                        , page _
                        , pagesize)
                    Select selectDTOData(d, db)).ToArray()
                If Not tblSolutionTrucks Is Nothing AndAlso tblSolutionTrucks.Count > 0 Then
                    For Each tblSolutionTruck In tblSolutionTrucks
                        If Not tblSolutionTruck Is Nothing Then
                            Dim iCompControl = tblSolutionTruck.SolutionTruckCompControl
                            'get the details
                            Dim Details As List(Of DTO.tblSolutionDetail) = (
                                From d In db.spGetLoadPlanningBookData(tblSolutionTruck.SolutionTruckConsPrefix, tblSolutionTruck.SolutionTruckRouteConsFlag, tblSolutionTruck.SolutionTruckCarrierTruckControl, tblSolutionTruck.SolutionTruckCarrierControl)
                                Select selectDTODetailData(d)).ToList()
                            If Not Details Is Nothing Then tblSolutionTruck.SolutionDetails = Details
                            If blnReadNGLMessages Then
                                'Notes:
                                'database key NMMTRefControl maps to TruckFilter.CompControlFilter 
                                'database key NMMTRefAlphaControl maps to tblSolutionTruck.SolutionTruckKey
                                Dim oMessages = db.tblNGLMessageRefBooks.Where(Function(x) x.NMNMTControl = Utilities.NGLMessageKeyRef.LoadPlanningTruck And x.NMMTRefControl = iCompControl And x.NMMTRefAlphaControl = tblSolutionTruck.SolutionTruckKey).ToList()

                                If Not oMessages Is Nothing AndAlso oMessages.Count() > 0 Then
                                    tblSolutionTruck.addMessages(oMessages)
                                End If
                            End If
                        End If
                    Next
                End If
                Return tblSolutionTrucks
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadPlanningTrucks365Filtered"))
            End Try
            Return Nothing
        End Using
    End Function



#End Region

#Region "Protected or Private Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblSolutionTruck)
        Dim skipObjs As New List(Of String) From {"SolutionTruckUpdated", "SolutionTruckModDate", "SolutionTruckModUser", "Page", "Pages", "RecordCount", "PageSize"}
        Dim oLTS As New LTS.tblSolutionTruck
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .SolutionTruckModDate = Date.Now
            .SolutionTruckModUser = Me.Parameters.UserName
            .SolutionTruckUpdated = If(d.SolutionTruckUpdated Is Nothing, New Byte() {}, d.SolutionTruckUpdated)
        End With
        Return oLTS
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing ' GettblSolutionTruckFiltered(Control:=CType(LinqTable, LTS.tblSolutionTruck).SolutionTruckControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults

        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblSolutionTruck = TryCast(LinqTable, LTS.tblSolutionTruck)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblSolutionTrucks
                       Where d.SolutionTruckControl = source.SolutionTruckControl
                       Select New DTO.QuickSaveResults With {.Control = d.SolutionTruckControl _
                                                                , .ModDate = d.SolutionTruckModDate _
                                                                , .ModUser = d.SolutionTruckModUser _
                                                                , .Updated = d.SolutionTruckUpdated.ToArray}).First

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class


Public Class NGLtblBookItemCommCodeXrefData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.tblBookItemCommCodeXrefs
        Me.LinqDB = db
        Me.SourceClass = "NGLtblBookItemCommCodeXrefData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.tblBookItemCommCodeXrefs
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
        Return GettblBookItemCommCodeXrefFiltered(Control)
    End Function

    ''' <summary>
    ''' GetRecordsFiltered not supported because paging is required try GettblBookItemCommCodeXrefsFiltered
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GettblBookItemCommCodeXrefFiltered(ByVal Control As Integer) As DTO.tblBookItemCommCodeXref
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim BookFee As DTO.tblBookItemCommCodeXref = (
                    From d In db.tblBookItemCommCodeXrefs
                    Where
                        d.BICCXrefControl = Control
                    Select selectDTOData(d)).First()

                Return BookFee

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblBookItemCommCodeXrefFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblBookItemCommCodeXrefsFiltered(ByVal page As Integer, ByVal pagesize As Integer) As DTO.tblBookItemCommCodeXref()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If page < 1 Then page = 1
                If pagesize = 0 Then pagesize = 1000
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                intRecordCount = db.tblBookItemCommCodeXrefs.Count()
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize


                'Return all the fees that match the criteria sorted by caption
                Dim BICCXref() As DTO.tblBookItemCommCodeXref = (
                    From d In db.tblBookItemCommCodeXrefs
                    Order By d.BICCXrefCompControl, d.BICCXrefFieldName
                    Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return BICCXref

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblBookItemCommCodeXrefsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblBookItemCommCodeXrefsFiltered(ByVal CompControl As Integer, ByVal page As Integer, ByVal pagesize As Integer) As DTO.tblBookItemCommCodeXref()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If page < 1 Then page = 1
                If pagesize = 0 Then pagesize = 1000
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                intRecordCount = db.tblBookItemCommCodeXrefs.Count(Function(x) x.BICCXrefCompControl = CompControl)
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the fees that match the criteria sorted by caption
                Dim BICCXref() As DTO.tblBookItemCommCodeXref = (
                    From d In db.tblBookItemCommCodeXrefs
                    Where
                        (d.BICCXrefCompControl = CompControl)
                    Order By d.BICCXrefFieldName
                    Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return BICCXref

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblBookItemCommCodeXrefsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DTO.tblBookItemCommCodeXref)
        Using db As New NGLMasBookDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.tblBookItemCommCodeXrefs.Attach(nObject, True)
            db.tblBookItemCommCodeXrefs.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Friend Sub UpdateLTSWithDTO(ByRef d As DTO.tblBookItemCommCodeXref, ByRef t As LTS.tblBookItemCommCodeXref)

        With t
            .BICCXrefControl = d.BICCXrefControl
            .BICCXrefCompControl = d.BICCXrefCompControl
            .BICCXrefFieldName = d.BICCXrefFieldName
            .BICCXrefFilter = d.BICCXrefFilter
            .BICCXrefCommCode = d.BICCXrefCommCode
            .BICCXrefModDate = Date.Now
            .BICCXrefModUser = Parameters.UserName
        End With

    End Sub

    Friend Overloads Function GetLTSFromDTO(ByVal oData As DTO.BookFee) As LTS.tblBookItemCommCodeXref
        Return CopyDTOToLinq(oData)
    End Function

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblBookItemCommCodeXref)
        'Create New Record
        Return New LTS.tblBookItemCommCodeXref With {.BICCXrefControl = d.BICCXrefControl _
                                                        , .BICCXrefCompControl = d.BICCXrefCompControl _
                                                        , .BICCXrefFieldName = d.BICCXrefFieldName _
                                                        , .BICCXrefFilter = d.BICCXrefFilter _
                                                        , .BICCXrefCommCode = d.BICCXrefCommCode _
                                                        , .BICCXrefModDate = Date.Now _
                                                        , .BICCXrefModUser = Parameters.UserName _
                                                        , .BICCXrefUpdated = If(d.BICCXrefUpdated Is Nothing, New Byte() {}, d.BICCXrefUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblBookItemCommCodeXrefFiltered(Control:=CType(LinqTable, LTS.tblBookItemCommCodeXref).BICCXrefControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.tblBookItemCommCodeXref = TryCast(LinqTable, LTS.tblBookItemCommCodeXref)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblBookItemCommCodeXrefs
                       Where d.BICCXrefControl = source.BICCXrefControl
                       Select New DTO.QuickSaveResults With {.Control = d.BICCXrefControl _
                                                                , .ModDate = d.BICCXrefModDate _
                                                                , .ModUser = d.BICCXrefModUser _
                                                                , .Updated = d.BICCXrefUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblBookItemCommCodeXref, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblBookItemCommCodeXref

        Return New DTO.tblBookItemCommCodeXref With {.BICCXrefControl = d.BICCXrefControl _
                                                        , .BICCXrefCompControl = d.BICCXrefCompControl _
                                                        , .BICCXrefFieldName = d.BICCXrefFieldName _
                                                        , .BICCXrefFilter = d.BICCXrefFilter _
                                                        , .BICCXrefCommCode = d.BICCXrefCommCode _
                                                        , .BICCXrefModDate = d.BICCXrefModDate _
                                                        , .BICCXrefModUser = d.BICCXrefModUser _
                                                        , .BICCXrefUpdated = d.BICCXrefUpdated.ToArray(),
                                                        .Page = page,
                                                        .Pages = pagecount,
                                                        .RecordCount = recordcount,
                                                        .PageSize = pagesize}

    End Function


#End Region

End Class


Public Class NGLAPMassEntryMsg : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.APMassEntryMsgs
        Me.LinqDB = db
        Me.SourceClass = "NGLAPMassEntryMsg"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then


                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.APMassEntryMsgs
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

    Public Function GetAPMassEntryMsg(ByVal iAPMMsgControl As Integer) As LTS.vAPMassEntryMsg
        If iAPMMsgControl < 0 Then Return Nothing
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.vAPMassEntryMsgs.Where(Function(y) y.APMMsgControl = iAPMMsgControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryMsg"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' Returns the APMassEntryMsg assoicated with a APMassEntry Record the APControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>LTS.APMassEntryMsg()</returns>
    ''' <remarks>
    ''' Created by LVV on 7/26/19 for v-8.2  
    ''' Modified by RHR for v-8.5.3.007 change sort newest to oldest
    ''' </remarks>
    Public Function GetAPMassEntryMsgs(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vAPMassEntryMsg()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vAPMassEntryMsg
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        'we need a APMMsgAPControl filter or a parent control number
        If Not filters.FilterValues.Any(Function(x) x.filterName = "APMMsgAPControl") Then
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingParent" 'The reference to the parent record is missing. Please select a valid parent record and try again.
                throwNoDataFaultException(sMsg)
            Else
                filterWhere = " (APMMsgAPControl = " & filters.ParentControl & ") "
                sFilterSpacer = " And "
            End If
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAPMassEntryMsg)
                iQuery = db.vAPMassEntryMsgs
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "APMMsgCreateDate"
                    filters.sortDirection = "DESC" 'Modified by RHR for v-8.5.3.007 change sort newest to oldest
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryMsgs"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' A method that only updates Resolved, ModDate/User.
    ''' </summary>
    ''' <param name="APMMsgControl"></param>
    ''' <param name="resolved"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by LVV on 7/26/19 for v-8.2 
    ''' </remarks>
    Public Function UpdateResolvedFlag(ByVal APMMsgControl As Integer, ByVal resolved As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim lts = db.APMassEntryMsgs.Where(Function(x) x.APMMsgControl = APMMsgControl).FirstOrDefault()
                If lts Is Nothing Then Return blnRet
                lts.APMMsgResolved = resolved
                lts.APMMsgModDate = Date.Now
                lts.APMMsgModUser = Parameters.UserName
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateResolvedFlag"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function InsertOrUpdateAPMassEntryMsg(ByVal oData As LTS.APMassEntryMsg) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iAPMMsgAPControl = oData.APMMsgAPControl
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify that an APMassEntry record exists
                If iAPMMsgAPControl = 0 Then
                    Dim lDetails As New List(Of String) From {"APMassEntry Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.APMassEntries.Any(Function(x) x.APControl = iAPMMsgAPControl) Then
                    Dim lDetails As New List(Of String) From {"APMassEntry Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.APMMsgModUser = Parameters.UserName
                oData.APMMsgModDate = Date.Now
                If oData.APMMsgControl = 0 Then
                    db.APMassEntryMsgs.InsertOnSubmit(oData)
                Else
                    db.APMassEntryMsgs.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAPMassEntryMsg"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteAPMassEntryMsg(ByVal iAPMMsgControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAPMMsgControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oExisting = db.APMassEntryMsgs.Where(Function(x) x.APMMsgControl = iAPMMsgControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.APMMsgControl = 0 Then Return True
                db.APMassEntryMsgs.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAPMassEntryMsg"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"


#End Region

End Class

Public Class NGLAPMassEntryFees : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.APMassEntryFees
        Me.LinqDB = db
        Me.SourceClass = "NGLAPMassEntryFees"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.APMassEntryFees
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

    ''' <summary>
    ''' Returns the APMassEntryFees assoicated with a APMassEntry Record the APControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>LTS.APMassEntryMsg()</returns>
    ''' <remarks>
    ''' Created by LVV on 8/2/19 for v-8.2  
    ''' </remarks>
    Public Function GetAPMassEntryFees(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vAPMassEntryFee()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vAPMassEntryFee
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        'we need a APMFeesAPControl filter or a parent control number
        If Not filters.FilterValues.Any(Function(x) x.filterName = "APMFeesAPControl") Then
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingParent" 'The reference to the parent record is missing. Please select a valid parent record and try again.
                throwNoDataFaultException(sMsg)
            Else
                filterWhere = " (APMFeesAPControl = " & filters.ParentControl & ") "
                sFilterSpacer = " And "
            End If
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAPMassEntryFee)
                iQuery = db.vAPMassEntryFees
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "APMFeesControl"
                    filters.sortDirection = "ASC" 'sort by the oldest to newest
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryFees"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' Copy all the pending and approved  not overridden to the APMassEntryFees table 
    ''' </summary>
    ''' <param name="iAPControl"></param>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 on 8/20/19
    ''' Modified by RHR for v-8.2.1.004 on 01/03/2020
    '''     added support for missing fees flag
    ''' </remarks>
    Public Function UpdateAPMassEntryFees(ByVal iAPControl As Integer, ByVal BookSHID As String) As Boolean
        Dim blnRet As Boolean = False
        If iAPControl = 0 Then Return False 'nothing to do
        Dim oPFDAL = New NGLBookFeePendingData(Me.Parameters)
        Dim sFees = NGLBookFeePendingObjData.GetSettlementFeesForSHID(BookSHID, False, False)
        If sFees Is Nothing OrElse sFees.Count() < 1 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim dtModDate As Date = Date.Now()
                Dim sModUser As String = Me.Parameters.UserName

                Dim oExisting = db.APMassEntryFees.Where(Function(x) x.APMFeesAPControl = iAPControl).ToList()
                If oExisting Is Nothing OrElse oExisting.Count() < 1 Then
                    'just insert all the fees this is a new freight bill
                    For Each f As Models.SettlementFee In sFees
                        Dim oApFee = New LTS.APMassEntryFee With {.APMFeesAccessorialCode = f.AccessorialCode,
                            .APMFeesAPControl = iAPControl,
                            .APMFeesBookControl = f.BookControl,
                            .APMFeesCaption = f.Caption,
                            .APMFeesEDICode = f.EDICode,
                            .APMFeesModDate = dtModDate,
                            .APMFeesModUser = sModUser,
                            .APMFeesOrderNumber = f.BookCarrOrderNumber,
                            .APMFeesStopSequence = f.StopSequence,
                            .APMFeesValue = f.Cost,
                            .APMFeesMissingFee = f.MissingFee}
                        db.APMassEntryFees.InsertOnSubmit(oApFee)
                    Next
                Else
                    'rule 1.  If oExisting does not contain fee add it else update it
                    For Each oItem In sFees
                        If (oExisting.Any(Function(e) e.APMFeesBookControl = oItem.BookControl And e.APMFeesAccessorialCode = oItem.AccessorialCode)) Then
                            Dim apfee = oExisting.Where(Function(e) e.APMFeesBookControl = oItem.BookControl And e.APMFeesAccessorialCode = oItem.AccessorialCode).FirstOrDefault()
                            With apfee
                                .APMFeesCaption = oItem.Caption
                                .APMFeesEDICode = oItem.EDICode
                                .APMFeesModDate = dtModDate
                                .APMFeesModUser = sModUser
                                .APMFeesOrderNumber = oItem.BookCarrOrderNumber
                                .APMFeesStopSequence = oItem.StopSequence
                                .APMFeesValue = oItem.Cost
                                .APMFeesMissingFee = oItem.MissingFee
                            End With
                        Else
                            db.APMassEntryFees.InsertOnSubmit(New LTS.APMassEntryFee With {.APMFeesAccessorialCode = oItem.AccessorialCode,
                                                            .APMFeesAPControl = iAPControl,
                                                            .APMFeesBookControl = oItem.BookControl,
                                                            .APMFeesCaption = oItem.Caption,
                                                            .APMFeesEDICode = oItem.EDICode,
                                                            .APMFeesModDate = dtModDate,
                                                            .APMFeesModUser = sModUser,
                                                            .APMFeesOrderNumber = oItem.BookCarrOrderNumber,
                                                            .APMFeesStopSequence = oItem.StopSequence,
                                                            .APMFeesValue = oItem.Cost,
                                                            .APMFeesMissingFee = oItem.MissingFee})
                        End If
                    Next

                    'rule 2.  If sFees  does not contain oExisting delete it unless the blnIsFuel flag is false
                    ' and the fee is fuel 
                    For Each e In oExisting
                        'If (e.APMFeesAccessorialCode <> 2 And e.APMFeesAccessorialCode <> 9 And e.APMFeesAccessorialCode <> 15) Then
                        If (Not sFees.Any(Function(x) x.BookControl = e.APMFeesBookControl And x.AccessorialCode = e.APMFeesAccessorialCode And x.Cost = e.APMFeesValue)) Then
                            db.APMassEntryFees.DeleteOnSubmit(e)
                        End If
                        'End If
                    Next
                    'rule 3. check for duplicates and remove the oldest one
                    If oExisting.Count() > sFees.Count() Then
                        For Each f In sFees
                            Dim oExists = oExisting.Where(Function(e) e.APMFeesBookControl = f.BookControl And e.APMFeesAccessorialCode = f.AccessorialCode And e.APMFeesValue = f.Cost).ToArray()
                            If Not oExists Is Nothing Then
                                Dim ict As Integer = oExists.Count()
                                If ict > 1 Then
                                    For Each r In oExists.OrderBy(Function(x) x.APMFeesControl)
                                        If ict > 1 Then
                                            db.APMassEntryFees.DeleteOnSubmit(r)
                                        Else
                                            Exit For
                                        End If
                                        ict -= 1
                                    Next
                                End If
                            End If
                        Next
                    End If

                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateAPMassEntryFees"), db)
            End Try
        End Using
        Return blnRet

    End Function


    Public Function UpdatePendingAPMassEntryFee(ByVal oBookFeesPending As LTS.BookFeesPending) As Integer
        Dim iAPControl As Integer

        If oBookFeesPending Is Nothing OrElse oBookFeesPending.BookFeesPendingControl = 0 Then Return 0 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim dtModDate As Date = Date.Now()
                Dim sModUser As String = Me.Parameters.UserName
                Dim oAPData = db.spGetAPMassEntryByBookControl(oBookFeesPending.BookFeesPendingBookControl).FirstOrDefault()
                If Not oAPData Is Nothing AndAlso oAPData.APControl <> 0 Then
                    iAPControl = oAPData.APControl
                    UpdateAPMassEntryFees(oAPData.APControl, oAPData.APSHID)
                End If
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdatePendingAPMassEntryFee"), db)
            End Try
        End Using
        Return iAPControl

    End Function

    ''' <summary>
    ''' Updates the Freight Bill Fuel Costs
    ''' </summary>
    ''' <param name="iAPControl"></param>
    ''' <param name="sBookSHID"></param>
    ''' <param name="dTotalCost"></param>
    ''' <param name="sCaption"></param>
    ''' <param name="sEDICode"></param>
    ''' <param name="blnFuelFeeMissing"></param>
    ''' <param name="sMsg"></param>
    ''' <param name="iErrNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 01/02/2020
    '''     Added New logic to save Missing Fee flag to Pending Fees Table
    ''' </remarks>
    Public Function UpdateFreightBillFuelCosts(ByVal iAPControl As Integer,
                                               ByVal sBookSHID As String,
                                               ByVal dTotalCost As Decimal,
                                               ByVal sCaption As String,
                                               ByVal sEDICode As String,
                                               ByVal blnFuelFeeMissing As Boolean,
                                               ByRef sMsg As String,
                                               ByRef iErrNumber As Integer) As Models.SettlementFee()
        Dim oFees As New List(Of Models.SettlementFee)
        If iAPControl = 0 Then
            iErrNumber = 1
            sMsg = " Invalid AP control number."
            Return oFees.ToArray()
        End If
        If String.IsNullOrWhiteSpace(sBookSHID) Then
            iErrNumber = 1
            sMsg = " Invalid booking shipment id number."
            Return oFees.ToArray()
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oResults = db.spUpdateFreightBillFuelCosts(iAPControl, sBookSHID, dTotalCost, sCaption, sEDICode, blnFuelFeeMissing).ToArray()
                If Not oResults Is Nothing AndAlso oResults.Count() > 0 Then
                    For Each r In oResults
                        If r.ErrNumber <> 0 Then
                            iErrNumber = r.ErrNumber
                            sMsg = r.RetMsg
                        End If
                        oFees.Add(New Models.SettlementFee With {.AccessorialCode = r.AccessorialCode,
                                  .AutoApprove = r.AutoApprove,
                                  .BookCarrOrderNumber = r.BookCarrOrderNumber,
                                  .BookControl = r.BookControl,
                                  .Caption = r.Caption,
                                  .Control = r.FeeControl,
                                  .Cost = r.Cost,
                                  .EDICode = r.EDICode,
                                  .StopSequence = r.StopSequence,
                                  .MissingFee = blnFuelFeeMissing})
                    Next
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateFreightBillFuelCosts"), db)
            End Try
        End Using
        Return oFees.ToArray()

    End Function

    Public Function DeleteAPMassEntryFee(ByVal iAPMFeesControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAPMFeesControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oExisting = db.APMassEntryFees.Where(Function(x) x.APMFeesControl = iAPMFeesControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.APMFeesControl = 0 Then Return True
                db.APMassEntryFees.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAPMassEntryFee"), db)
            End Try
        End Using
        Return blnRet
    End Function



#End Region

#Region "Protected Functions"


#End Region

End Class

Public Class NGLAPMassEntryHistories : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.APMassEntryHistories
        Me.LinqDB = db
        Me.SourceClass = "NGLAPMassEntryHistories"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.APMassEntryHistories
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

    Public Function GetAPMassEntryHistory(ByVal iAPMHControl As Integer) As LTS.APMassEntryHistory
        If iAPMHControl < 0 Then Return Nothing
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.APMassEntryHistories.Where(Function(y) y.APMHControl = iAPMHControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryHistory"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' Returns the APMassEntryHistory filtered
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>LTS.APMassEntryHistory()</returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 onn 8/12/19
    ''' </remarks>
    Public Function GetAPMassEntryHistories(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.APMassEntryHistory()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.APMassEntryHistory
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.APMassEntryHistory)
                iQuery = db.APMassEntryHistories
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "APMHReceivedDate"
                    filters.sortDirection = "DESC" 'sort by newest
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryHistories"), db)
            End Try
        End Using
        Return Nothing
    End Function


    Public Function InsertOrUpdateAPMassEntryHistory(ByRef oData As LTS.APMassEntryHistory) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oData.APMHModUser = Parameters.UserName
                oData.APMHModDate = Date.Now
                If oData.APMHControl = 0 Then
                    db.APMassEntryHistories.InsertOnSubmit(oData)
                Else
                    db.APMassEntryHistories.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAPMassEntryHistory"), db)
            End Try
        End Using
        Return blnRet
    End Function


    Public Function DeleteAPMassEntryHistory(ByVal iAPMHControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAPMHControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oExisting = db.APMassEntryHistories.Where(Function(x) x.APMHControl = iAPMHControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.APMHControl = 0 Then Return True
                db.APMassEntryHistories.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAPMassEntryHistory"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"


#End Region

End Class

Public Class NGLAPMassEntryHistoryFees : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.APMassEntryHistoryFees
        Me.LinqDB = db
        Me.SourceClass = "NGLAPMassEntryHistoryFees"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.APMassEntryHistoryFees
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

    Public Function GetAPMassEntryHistoryFee(ByVal iAPMHFeesControl As Integer) As LTS.APMassEntryHistoryFee
        If iAPMHFeesControl < 0 Then Return Nothing
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.APMassEntryHistoryFees.Where(Function(y) y.APMHFeesControl = iAPMHFeesControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryHistoryFee"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the APMassEntryHistoryFees assoicated with a APMassEntry Record the APControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns>LTS.APMassEntryHistoryFee()</returns>
    ''' <remarks>
    ''' Created by LVV on 8/2/19 for v-8.2   iAPMHControl 
    ''' </remarks>
    Public Function GetAPMassEntryHistoryFees(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.APMassEntryHistoryFee()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.APMassEntryHistoryFee
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        'we need a APMHFeesAPMHControl filter or a parent control number
        If Not filters.FilterValues.Any(Function(x) x.filterName = "APMHFeesAPMHControl") Then
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingParent" 'The reference to the parent record is missing. Please select a valid parent record and try again.
                throwNoDataFaultException(sMsg)
            Else
                filterWhere = " (APMHFeesAPMHControl = " & filters.ParentControl & ") "
                sFilterSpacer = " And "
            End If
        End If
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.APMassEntryHistoryFee)
                iQuery = db.APMassEntryHistoryFees
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "APMHFeesControl"
                    filters.sortDirection = "ASC" 'sort by the oldest to newest
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAPMassEntryHistoryFees"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function InsertOrUpdateAPMassEntryHistoryFee(ByRef oData As LTS.APMassEntryHistoryFee) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oData.APMHFeesModUser = Parameters.UserName
                oData.APMHFeesModDate = Date.Now
                If oData.APMHFeesControl = 0 Then
                    db.APMassEntryHistoryFees.InsertOnSubmit(oData)
                Else
                    db.APMassEntryHistoryFees.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAPMassEntryHistoryFee"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteAPMassEntryHistoryFee(ByVal iAPMHFeesControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iAPMHFeesControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oExisting = db.APMassEntryHistoryFees.Where(Function(x) x.APMHFeesControl = iAPMHFeesControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.APMHFeesControl = 0 Then Return True
                db.APMassEntryHistoryFees.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAPMassEntryHistoryFee"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"


#End Region

End Class

Public Class NGLBookImageData : Inherits NGLLinkDataBaseClass

#Region " Constructors "


    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookImages
        Me.LinqDB = db
        Me.SourceClass = "NGLBookImage"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get

            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookImages
            Me.LinqDB = db

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetBookImage(ByVal iBookImageControl As Integer) As LTS.BookImage
        Dim oRet As LTS.BookImage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.BookImages.Where(Function(x) x.BookImageControl = iBookImageControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookImage"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the Book Image Summaryrecords assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 01/06/2021
    '''     reads Image records like POD
    ''' </remarks>
    Public Function GetBookImageSummaries(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookImageSummary()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iBookImageControl As Integer = 0
        Dim iBookControl As Integer = 0
        If Not filters.FilterValues.Any(Function(x) x.filterName = "BookImageControl") Then
            'we need a BookPkgControl fliter or a parent control number
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingParent" ' The reference to the parent record is missing. Please select a valid parent record and try again.
                throwNoDataFaultException(sMsg)
            End If
            filterWhere = " (BookImageBookControl = " & filters.ParentControl & ") "
            sFilterSpacer = " And "
            iBookControl = filters.ParentControl
        Else
            Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "BookImageControl").FirstOrDefault()
            Integer.TryParse(tFilter.filterValueFrom, iBookImageControl)
        End If

        Dim oRet() As LTS.vBookImageSummary
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vBookImageSummary)

                iQuery = db.vBookImageSummaries
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookImageControl"
                    filters.sortDirection = "asc"
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookImageSummaries"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the Images assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 01/06/2021
    '''     reads Image records like POD
    '''     typically we use GetBookImageSummaries
    ''' </remarks>
    Public Function GetBookImages(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.BookImage()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iBookImageControl As Integer = 0
        Dim iBookControl As Integer = 0
        If Not filters.FilterValues.Any(Function(x) x.filterName = "BookImageControl") Then
            'we need a BookPkgControl fliter or a parent control number
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                throwNoDataFaultException(sMsg)
            End If
            filterWhere = " (BookImageBookControl = " & filters.ParentControl & ") "
            sFilterSpacer = " And "
            iBookControl = filters.ParentControl
        Else
            Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "BookImageControl").FirstOrDefault()
            Integer.TryParse(tFilter.filterValueFrom, iBookImageControl)
        End If

        Dim oRet() As LTS.BookImage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.BookImage)

                iQuery = db.BookImages
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookImageControl"
                    filters.sortDirection = "asc"
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookImages"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns the Book Images Details View records assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 01/06/2021
    '''     reads Image records like POD
    '''     typically we use GetBookImageSummaries
    ''' </remarks>
    Public Function GetvBookImages(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookImage()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iBookImageControl As Integer = 0
        Dim iBookControl As Integer = 0
        If Not filters.FilterValues.Any(Function(x) x.filterName = "BookImageControl") Then
            'we need a BookPkgControl fliter or a parent control number
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                throwNoDataFaultException(sMsg)
            End If
            filterWhere = " (BookImageBookControl = " & filters.ParentControl & ") "
            sFilterSpacer = " And "
            iBookControl = filters.ParentControl
        Else
            Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "BookImageControl").FirstOrDefault()
            Integer.TryParse(tFilter.filterValueFrom, iBookImageControl)
        End If

        Dim oRet() As LTS.vBookImage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vBookImage)

                iQuery = db.vBookImages
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookImageControl"
                    filters.sortDirection = "asc"
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookImages"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns all the raw bookImage records for the provided book control number
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 01/06/2021
    ''' </remarks>
    Public Function GetBookImages(ByVal iBookControl As Integer) As LTS.BookImage()
        Dim oRet() As LTS.BookImage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.BookImages.Where(Function(x) x.BookImageBookControl = iBookControl).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookImages"), db)
            End Try
        End Using
        Return oRet
    End Function

    ' it is not supported to save a book image record through this interface.  We only support Read and Delete
    '''' <summary>
    '''' Saves or Inserts a Book Image Record.  
    '''' The BookImageTypeCode is required and cannot be zero or empty if missing the system will use 
    '''' BookImageTypeCode 2 for Receipt
    '''' </summary>
    '''' <param name="oData"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Created by RHR for v-8.2 on 10/07/2018
    ''''     save dispatching Image records.  This data is not the actual pallet counts assigned to the load 
    ''''     only the Images/pallets that are needed for dispatching and rating
    '''' </remarks>
    'Public Function SaveBookImage(ByVal oData As LTS.BookImage) As Boolean
    '    Dim blnRet As Boolean = False
    '    If oData Is Nothing Then Return False 'nothing to do
    '    Dim iBookControl = oData.BookImageBookControl

    '    Using db As New NGLMasBookDataContext(ConnectionString)
    '        Try
    '            If oData.BookImagePalletTypeID < 1 Then
    '                'look up the default PLT
    '                oData.BookImagePalletTypeID = db.PalletTypeRefBooks.Where(Function(x) x.PalletTypeDescription = "PLT").Select(Function(x) x.ID).FirstOrDefault()
    '            End If
    '            If oData.BookImagePalletTypeID < 1 Then oData.BookImagePalletTypeID = 19 'use default for PLT but may be wrong zero is not allowed.
    '            'verify that a booking record exists
    '            If iBookControl = 0 Then
    '                Dim lDetails As New List(Of String) From {"Booking Record Reference", " was not provided and "}
    '                throwInvalidKeyParentRequiredException(lDetails)
    '                Return False
    '            End If
    '            If Not db.Books.Any(Function(x) x.BookControl = iBookControl) Then
    '                Dim lDetails As New List(Of String) From {"Booking Record Reference", " was not found and "}
    '                throwInvalidKeyParentRequiredException(lDetails)
    '                Return False
    '            End If
    '            Dim blnProcessed As Boolean = False
    '            oData.BookImageModDate = Date.Now()
    '            oData.BookImageModUser = Me.Parameters.UserName

    '            If oData.BookImageControl = 0 Then
    '                db.BookImages.InsertOnSubmit(oData)
    '            Else
    '                db.BookImages.Attach(oData, True)
    '            End If
    '            db.SubmitChanges()
    '            'allocate the Images to the item details and booking table.
    '            'for now we do not process the return values or any errors.
    '            'this may need to change in the future for now it is not clear 
    '            'what should happen. the users cannot really fix any errors here.
    '            Dim oResults = db.spAllocateBookPalletToBooking(oData.BookImageBookControl, Parameters.UserName)
    '            blnRet = True
    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("SaveBookImage"), db)
    '        End Try
    '    End Using
    '    Return blnRet
    'End Function

    Public Function DeleteBookImage(ByVal iBookImageControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookImageControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.BookImages.Where(Function(x) x.BookImageControl = iBookImageControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookImageControl = 0 Then Return True
                db.BookImages.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookImage"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class


