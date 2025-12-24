Imports System.ServiceModel
Imports Ngl.Core.Utility

Public Class NGLBookTrackData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMasBookDataContext(ConnectionString)
        'Me.LinqTable = db.BookTracks
        'Me.LinqDB = db
        Me.SourceClass = "NGLBookTrackData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.BookTracks
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
        Return GetBookTrackFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookTracksFiltered()
    End Function

    Public Function GetBookTrackFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.BookTrack
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim BookTrack As DataTransferObjects.BookTrack = (
                        From d In db.BookTracks
                        Where
                        (d.BookTrackControl = If(Control = 0, d.BookTrackControl, Control))
                        Order By d.BookTrackControl Descending
                        Select New DataTransferObjects.BookTrack With {.BookTrackControl = d.BookTrackControl _
                        , .BookTrackBookControl = d.BookTrackBookControl _
                        , .BookTrackDate = d.BookTrackDate _
                        , .BookTrackContact = d.BookTrackContact _
                        , .BookTrackComment = d.BookTrackComment _
                        , .BookTrackStatus = If(d.BookTrackStatus.HasValue, d.BookTrackStatus, 0) _
                        , .BookTrackCommentLocalized = d.BookTrackCommentLocalized _
                        , .BookTrackCommentKeys = d.BookTrackCommentKeys _
                        , .BookTrackModDate = d.BookTrackModDate _
                        , .BookTrackModUser = d.BookTrackModUser _
                        , .BookTrackUpdated = d.BookTrackUpdated.ToArray()}).First
                Return BookTrack
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

    Public Function GetBookTracksFiltered(Optional ByVal BookControl As Integer = 0) As DataTransferObjects.BookTrack()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria sorted by name
                Dim BookTracks() As DataTransferObjects.BookTrack = (
                        From d In db.BookTracks
                        Where
                        (d.BookTrackBookControl = If(BookControl = 0, d.BookTrackBookControl, BookControl))
                        Order By d.BookTrackDate Descending
                        Select selectDTOData(d)).ToArray()
                Return BookTracks
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

    Public Function GetBookTracksFiltered(ByVal BookControl As Integer, ByVal ModUser As String) As DataTransferObjects.BookTrack()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrEmpty(ModUser) Then Return Nothing
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria sorted by name
                Dim BookTracks() As DataTransferObjects.BookTrack = (
                        From d In db.BookTracks
                        Where
                        (d.BookTrackBookControl = If(BookControl = 0, d.BookTrackBookControl, BookControl)) And d.BookTrackModUser.ToUpper().Equals(ModUser.ToUpper())
                        Order By d.BookTrackDate Descending
                        Select selectDTOData(d)).ToArray()
                Return BookTracks
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

    Public Sub UpdateBookTracksForLoad(ByVal BookControl As Integer,
                                       ByVal BookTrackComment As String,
                                       ByVal BookTrackStatus As Integer,
                                       ByVal BookTrackContact As String,
                                       ByVal BookTrackDate As System.Nullable(Of Date),
                                       ByVal BookTrackCommentLocalized As String,
                                       ByVal BookTrackCommentKeys As String)
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If Not String.IsNullOrEmpty(BookTrackComment) Then BookTrackComment = Left(BookTrackComment, 255)
                If Not String.IsNullOrEmpty(BookTrackContact) Then BookTrackContact = Left(BookTrackContact, 100)
                If Not String.IsNullOrEmpty(BookTrackCommentLocalized) Then BookTrackCommentLocalized = Left(BookTrackCommentLocalized, 255)
                If Not String.IsNullOrEmpty(BookTrackCommentKeys) Then BookTrackCommentKeys = Left(BookTrackCommentKeys, 4000)
                Dim oRet = db.spUpdateBookTracksForLoad(BookControl, BookTrackComment, BookTrackStatus, BookTrackContact, BookTrackDate, BookTrackCommentLocalized, BookTrackCommentKeys, Me.Parameters.UserName).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookTracksForLoad"))
            End Try
        End Using
    End Sub



    Public Sub UpdateBookTracksForAP(ByVal BookControl As Integer,
                                     ByVal BookTrackComment As String,
                                     ByVal BookTrackStatus As Integer,
                                     ByVal BookTrackContact As String,
                                     ByVal BookTrackDate As System.Nullable(Of Date),
                                     ByVal BookTrackCommentLocalized As String,
                                     ByVal BookTrackCommentKeys As String,
                                     ByVal APControl As Integer,
                                     ByVal LoadStatusCode As Integer)


        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If Not String.IsNullOrEmpty(BookTrackComment) Then BookTrackComment = Left(BookTrackComment, 255)
                If Not String.IsNullOrEmpty(BookTrackContact) Then BookTrackContact = Left(BookTrackContact, 100)
                If Not String.IsNullOrEmpty(BookTrackCommentLocalized) Then BookTrackCommentLocalized = Left(BookTrackCommentLocalized, 255)
                If Not String.IsNullOrEmpty(BookTrackCommentKeys) Then BookTrackCommentKeys = Left(BookTrackCommentKeys, 4000)
                Dim iRet As Integer = db.spUpdateBookTracksForAP(BookControl, BookTrackComment, BookTrackStatus, BookTrackContact, BookTrackDate, BookTrackCommentLocalized, BookTrackCommentKeys, APControl, LoadStatusCode, Me.Parameters.UserName)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookTracksForAP"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Query a list of users associated with the currently logged in user
    ''' 2 types of users 1 = Carrier User, 2 = Legal Entity User
    ''' Carrier User - show comments by users who are associated with the same Carrier And are also Carrier Users
    ''' Legal Entity User - Ari And John said they should be able to see all comments 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/15/19 for v-8.2
    ''' </remarks>
    Public Function GetAllCommentsHoverOverData(ByVal USC As Integer, ByVal BookControl As Integer) As LTS.spGetAllCommentsHoverOverDataResult()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spGetAllCommentsHoverOverData(USC, BookControl).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAllCommentsHoverOverData"))
            End Try
            Return Nothing
        End Using
    End Function

#Region "TMS 365"

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.BookTrack) As LTS.BookTrack
        Dim t As New LTS.BookTrack
        Dim skipObjs As New List(Of String) From {"BookTrackUpdated"}
        t = DataTransformation.CopyMatchingFields(t, d, skipObjs)
        t.BookTrackUpdated = If(d.BookTrackUpdated Is Nothing, New Byte() {}, d.BookTrackUpdated)
        Return t
    End Function

    Friend Shared Function selectLTSData(ByVal d As DataTransferObjects.BookTrackDetail) As LTS.BookTrackDetail
        Dim t As New LTS.BookTrackDetail
        Dim skipObjs As New List(Of String) From {"BookTrackUpdated"}
        t = DataTransformation.CopyMatchingFields(t, d, skipObjs)
        Return t
    End Function

    Public Function GetBookTracksBySHID(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vBookTrack()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vBookTrack
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim SHID As String = filters.Data
                Dim iQuery As IQueryable(Of LTS.vBookTrack)
                If Not String.IsNullOrWhiteSpace(SHID) AndAlso db.Books.Any(Function(x) x.BookSHID = SHID) Then
                    Dim bookControls = db.Books.Where(Function(x) x.BookSHID = SHID).Select(Function(y) y.BookControl).ToArray()
                    iQuery = db.vBookTracks.Where(Function(x) bookControls.Contains(x.BookTrackBookControl))
                Else
                    iQuery = db.vBookTracks.OrderByDescending(Function(x) x.BookTrackControl)
                End If
                Dim filterWhere = ""


                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookTracksBySHID"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetBookTracksBySHIDs(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters, ByVal shids As String()) As LTS.vBookTrack()
        If shids Is Nothing OrElse shids.Count < 1 Then Return Nothing
        Dim oRet() As LTS.vBookTrack
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim bookControls = db.Books.Where(Function(x) shids.Contains(x.BookSHID)).Select(Function(y) y.BookControl).ToArray()
                Dim iQuery As IQueryable(Of LTS.vBookTrack)
                iQuery = db.vBookTracks.Where(Function(x) bookControls.Contains(x.BookTrackBookControl)).OrderBy(Function(y) y.BookCarrOrderNumber)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookTracksBySHIDs"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetLoadBoardLoadStatus(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters, ByVal blnEntireLoad As Boolean) As LTS.vLBLoadStatus365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLBLoadStatus365
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If filters.BookControl = 0 Then Return Nothing
                Dim iQuery As IQueryable(Of LTS.vLBLoadStatus365)
                If blnEntireLoad Then
                    'Get the BookTracks for the SHID
                    Dim SHID As String = db.Books.Where(Function(x) x.BookControl = filters.BookControl).Select(Function(y) y.BookSHID).FirstOrDefault()
                    If Not String.IsNullOrWhiteSpace(SHID) AndAlso db.Books.Any(Function(x) x.BookSHID = SHID) Then
                        Dim bookControls = db.Books.Where(Function(x) x.BookSHID = SHID).Select(Function(y) y.BookControl).ToArray()
                        'iQuery = db.vLBLoadStatus365s.Where(Function(x) bookControls.Contains(x.BookTrackBookControl)).OrderByDescending(Function(x) x.BookCarrOrderNumber AndAlso x.BookTrackControl)
                        iQuery = (From x In db.vLBLoadStatus365s Where bookControls.Contains(x.BookTrackBookControl) Order By x.BookCarrOrderNumber Descending, x.BookTrackControl Descending)
                    Else
                        'Get the BookTracks for the BookControl
                        iQuery = db.vLBLoadStatus365s.Where(Function(x) x.BookTrackBookControl = filters.BookControl).OrderByDescending(Function(x) x.BookTrackControl)
                    End If
                Else
                    'Get the BookTracks for the BookControl
                    iQuery = db.vLBLoadStatus365s.Where(Function(x) x.BookTrackBookControl = filters.BookControl).OrderByDescending(Function(x) x.BookTrackControl)
                End If
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardLoadStatus"), db)
            End Try
        End Using
        Return Nothing
    End Function

#Region "Bing Maps"

    Public Delegate Sub UpdateLatLongFromAddressBingDelegate(ByVal StopControl As Integer, ByVal addr1 As String, ByVal city As String, ByVal state As String, ByVal zip As String, ByVal country As String)

    Public Sub UpdateLatLongFromAddressBingAsync(ByVal StopControl As Integer, ByVal addr1 As String, ByVal city As String, ByVal state As String, ByVal zip As String, ByVal country As String)
        Dim fetcher As New UpdateLatLongFromAddressBingDelegate(AddressOf Me.UpdateLatLongFromAddressBing)
        fetcher.BeginInvoke(StopControl, addr1, city, state, zip, country, Nothing, Nothing) 'Launch thread
    End Sub

    ''' <summary>
    ''' Uses BingMapsAPI to geocode a location (aka get latitude and longitude coordinates based on the address provided) 
    ''' and saves the values to tblStop for the provided StopControl.
    ''' Note: The user must have an SSOA set up for Bing in oder to have permissions to run this method (not sure what to do if we let the system run it in future)
    ''' </summary>
    ''' <param name="StopControl"></param>
    ''' <param name="addr1"></param>
    ''' <param name="city"></param>
    ''' <param name="state"></param>
    ''' <param name="zip"></param>
    ''' <param name="country"></param>
    ''' <remarks>
    ''' Added By LVV on 10/4/19 BingMaps
    ''' </remarks>
    Public Sub UpdateLatLongFromAddressBing(ByVal StopControl As Integer, ByVal addr1 As String, ByVal city As String, ByVal state As String, ByVal zip As String, ByVal country As String)
        Dim oSec = New NGLtblSingleSignOnAccountData(Parameters)
        Dim oStop = New NGLtblStopData(Parameters)
        Dim key = ""
        Dim SSOA = oSec.GetSingleSignOnAccountByUser(Parameters.UserControl, Utilities.SSOAAccount.BingMaps) 'Get the BingMapsKey
        For Each oS In SSOA
            Dim sVals = oS.TryGetKeys({"RefID"}, {""}) 'Modified by RHR for v-8.2 12/29/2018 to simplify reading of WCFResults keys 
            key = sVals(0)
        Next
        Dim gc = FM.Bing.BingLocationAPI.GeoCodeAddress(addr1, city, state, zip, country, key)
        oStop.UpdateStopCoordinates(StopControl, gc.Latitude, gc.Longitude)
    End Sub

    Public Sub InsertBookTracksWithDetails(ByVal oData() As DataTransferObjects.BookTrack)
        Using db As New NGLMasBookDataContext(ConnectionString)
            For Each ditem In oData
                InsertBookTrackWithDetails(db, ditem)
            Next
        End Using
    End Sub

    Public Sub InsertBookTrackWithDetails(ByVal oData As DataTransferObjects.BookTrack)
        Using db As New NGLMasBookDataContext(ConnectionString)
            InsertBookTrackWithDetails(db, oData)
        End Using
    End Sub

    Public Sub InsertBookTrackWithDetails(ByRef db As NGLMasBookDataContext, ByVal oData As DataTransferObjects.BookTrack)
        If oData Is Nothing Then Return 'nothing to do
        Try
            'Insert BookTrack
            Dim ltsBT = selectLTSData(oData)
            ltsBT.BookTrackModUser = Parameters.UserName
            ltsBT.BookTrackModDate = Date.Now
            db.BookTracks.InsertOnSubmit(ltsBT)
            db.SubmitChanges()

            Dim bookTrackControl = ltsBT.BookTrackControl

            Dim dets = oData.getDetails()
            For Each oBTD In dets
                If Not oBTD Is Nothing Then
                    Dim name = "", addr1 = "", city = "", state = "", country = "", zip = "", strlong = "", strlat = ""
                    Dim latitude As Double = 0
                    Dim longitude As Double = 0
                    'TODO: DON'T FORGET WE HAVE TO SET THE LOCATION TO WHATEVER EVEN IF THIS IS FALSE SO WE DONT TRY TO INSERT NULL STRINGS
                    If oBTD.BookTrackDetailMS1StatusUpdate Then
                        'The location is coming from a 214 (these fields will be populated in the BookTrackDetails table)
                        city = oBTD.BookTrackDetailMS101
                        state = oBTD.BookTrackDetailMS102
                        country = oBTD.BookTrackDetailMS103
                        strlong = oBTD.BookTrackDetailMS104
                        strlat = oBTD.BookTrackDetailMS105
                        If Not String.IsNullOrWhiteSpace(strlat) Then Double.TryParse(strlat, latitude)
                        If Not String.IsNullOrWhiteSpace(strlong) Then Double.TryParse(strlong, longitude)
                    Else
                        'The location is coming from the UI (the only location field populated in BookTracks will be the StopControl)
                        Dim s = oBTD.getStop()
                        If Not s Is Nothing AndAlso s.HasData() Then
                            name = s.StopName
                            addr1 = s.StopAddress1
                            city = s.StopCity
                            state = s.StopState
                            zip = s.StopZip
                            country = s.StopCountry
                            latitude = s.StopLatitude
                            longitude = s.StopLongitude
                        End If
                    End If
                    'Get the Stop Data if there is any
                    Dim stopControl As Integer = 0
                    If oBTD.getLinkStopRecord() Then
                        Dim blnUpdateStops = False
                        Dim spRes = db.spGetStopControl365(name, addr1, city, state, country, zip, latitude, longitude, Parameters.UserName).FirstOrDefault()
                        If Not spRes Is Nothing Then
                            If spRes.UpdateLatLong.HasValue Then blnUpdateStops = spRes.UpdateLatLong.Value
                            If spRes.StopControl.HasValue Then stopControl = spRes.StopControl.Value
                        End If
                        If blnUpdateStops Then
                            'Asynchronously look up latitude and longitute Using Bing Maps REST services and update tblStops
                            UpdateLatLongFromAddressBingAsync(stopControl, addr1, city, state, zip, country)
                            'UpdateLatLongFromAddressBing(stopControl, addr1, city, state, zip, country)
                        End If
                    End If
                    'Insert BookTrackDetail
                    Dim ltsDet = selectLTSData(oBTD)
                    ltsDet.BookTrackDetailBookTrackControl = bookTrackControl
                    ltsDet.BookTrackDetailStopControl = stopControl
                    ltsDet.BookTrackDetailModUser = Parameters.UserName
                    ltsDet.BookTrackDetailModDate = Date.Now
                    db.BookTrackDetails.InsertOnSubmit(ltsDet)
                End If
            Next
            db.SubmitChanges()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("InsertBookTrackWithDetails"), db)
        End Try
    End Sub


    ''' <summary>
    ''' Route 1 Map It
    ''' Get data of all the stop points (warehouses) on the route from the book table.
    ''' Display these as waypoints on the first calculated truck route.
    ''' Add lables to show what the picks And drops are at each stop.
    ''' Optional change color to indicate pick/drop status if time.
    ''' 
    ''' Route 2 Track It
    ''' Get all BookTrackDetails for the truck.
    ''' These will be used as the waypoints to show the progress of the truck.
    ''' If the Delivered Date is in the past for the last stop add the destination as a waypoint.
    ''' Optionally show the BookTrack Message in a hover over if time.
    ''' --Basically I have waypoints And pins 
    ''' --waypoints are 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/27/19 Bing Maps
    ''' </remarks>
    Public Function GetBingMapRoutes(ByVal BookControl As Integer) As Models.MapRouteWayPoints
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim routes As New Models.MapRouteWayPoints()
                Dim mapWaypoints As New List(Of Models.MapWaypoint)
                Dim trackWaypoints As New List(Of Models.MapWaypoint)
                Dim maxStop As Integer
                'GET MAP IT WAYPOINTS
                Dim spRes = db.spGetMapItWaypoints(BookControl).ToArray()

                'Dim oResult = spRes.GroupBy(Function(v) New With {Key v.City, Key v.Country}) _
                '.Where(Function(grp) grp.Count > 1).ToList()
                '  Dim array = spRes.GroupBy(Function(x) x.Name AndAlso x.Address1 AndAlso x.City AndAlso x.State AndAlso x.Zip AndAlso x.Country AndAlso x.LocationLatitude AndAlso x.LocationLongitude AndAlso x.StopNumber).ToArray()


                If spRes?.Count > 2 Then
                    maxStop = spRes.Select(Function(x) x.StopNumber.Value).Max()
                    If (maxStop = 0) Then
                        routes.ErrMsg = "Please run stop resequence to make sure stop data is accurate"
                        Return routes
                    End If
                    For i As Integer = 1 To maxStop
                        Dim wp As New Models.MapWaypoint()
                        Dim stopNumber = i
                        Dim srecords = spRes.Where(Function(x) x.StopNumber = stopNumber).ToArray() 'get all the records for this stop
                        If srecords?.Count() < 1 Then Continue For
                        'get the type of stop pick/del/both
                        Dim blnHasPickups = srecords.Any(Function(x) x.IsPickUpFlag = True)
                        Dim blnHasDropOffs = srecords.Any(Function(x) x.IsPickUpFlag = False)
                        Dim blnStopCompleted = srecords.Any(Function(x) x.StopCompleted = True)
                        Dim stopType = Models.MapWaypoint.StopType.Pickup 'set to pickup as default
                        If blnHasPickups AndAlso Not blnHasDropOffs Then stopType = Models.MapWaypoint.StopType.Pickup
                        If Not blnHasPickups AndAlso blnHasDropOffs Then stopType = Models.MapWaypoint.StopType.Delivery
                        If blnHasPickups AndAlso blnHasDropOffs Then stopType = Models.MapWaypoint.StopType.PickAndDel
                        'get the orders to be picked/dropped at the stop
                        Dim sbP As New System.Text.StringBuilder()
                        Dim sbD As New System.Text.StringBuilder()
                        Dim segP As String = ""
                        Dim segD As String = ""
                        For Each sr In srecords
                            If sr.IsPickUpFlag Then
                                sbP.Append(segP + sr.OrderNumber)
                                'strPickupOrders += (segP + sr.OrderNumber)
                                segP = ", "
                            Else
                                sbD.Append(segD + sr.OrderNumber)
                                'strDropOffOrders += (segD + sr.OrderNumber)
                                segD = ", "
                            End If
                        Next
                        If Not String.IsNullOrWhiteSpace(sbP.ToString()) Then sbP.Insert(0, "Pickup: ")
                        If Not String.IsNullOrWhiteSpace(sbD.ToString()) Then sbD.Insert(0, "Drop Off: ")
                        Dim strPickupOrders As String = sbP.ToString()
                        Dim strDropOffOrders As String = sbD.ToString()
                        'set the comment
                        Dim strComment = ""
                        If stopType = Models.MapWaypoint.StopType.Pickup Then strComment = strPickupOrders
                        If stopType = Models.MapWaypoint.StopType.Delivery Then strComment = strDropOffOrders
                        If stopType = Models.MapWaypoint.StopType.PickAndDel Then strComment = strPickupOrders + "</br>" + strDropOffOrders
                        'create the object for the stop
                        With wp
                            .StopNumber = stopNumber
                            .Name = srecords(0).Name
                            .Address1 = srecords(0).Address1
                            .Address2 = srecords(0).Address2
                            .Address3 = srecords(0).Address3
                            .City = srecords(0).City
                            .State = srecords(0).State
                            .Zip = srecords(0).Zip
                            .Country = srecords(0).Country
                            .AddressString = srecords(0).strAddress
                            .Lattitude = If(srecords(0).LocationLatitude.HasValue, Math.Round(srecords(0).LocationLatitude.Value, 6), 0)
                            .Longitude = If(srecords(0).LocationLongitude.HasValue, Math.Round(srecords(0).LocationLongitude.Value, 6), 0)
                            .StopCategory = stopType
                            .PickupOrders = strPickupOrders 'we don't really need this field because it is in the comments but I left it in case we need comments for something else later, or if we need the pick/drop string individually later for whatever reason
                            .DropoffOrders = strDropOffOrders 'we don't really need this field because it is in the comments but I left it in case we need comments for something else later, or if we need the pick/drop string individually later for whatever reason
                            .Comment = strComment
                            .DateDelivered = srecords(0).DateDelivered
                            .StopCompleted = blnStopCompleted
                        End With
                        mapWaypoints.Add(wp)
                    Next
                Else
                    maxStop = 2
                    Dim wpPick As New Models.MapWaypoint()
                    Dim wpDel As New Models.MapWaypoint()
                    Dim pickup = spRes.Where(Function(x) x.IsPickUpFlag = True).FirstOrDefault()
                    Dim delivery = spRes.Where(Function(x) x.IsPickUpFlag = False).FirstOrDefault()

                    If Not pickup Is Nothing AndAlso Not delivery Is Nothing Then

                        wpPick.StopCategory = Models.MapWaypoint.StopType.Pickup
                        wpDel.StopCategory = Models.MapWaypoint.StopType.Delivery

                        With wpPick
                            .StopNumber = 1
                            .Name = pickup.Name
                            .Address1 = pickup.Address1
                            .Address2 = pickup.Address2
                            .Address3 = pickup.Address3
                            .City = pickup.City
                            .State = pickup.State
                            .Zip = pickup.Zip
                            .Country = pickup.Country
                            .AddressString = pickup.strAddress
                            .Lattitude = If(pickup.LocationLatitude.HasValue, Math.Round(pickup.LocationLatitude.Value, 6), 0)
                            .Longitude = If(pickup.LocationLongitude.HasValue, Math.Round(pickup.LocationLongitude.Value, 6), 0)
                            .StopCategory = Models.MapWaypoint.StopType.Pickup
                            .PickupOrders = "Pickup: " + pickup.OrderNumber 'we don't really need this field because it is in the comments but I left it in case we need comments for something else later, or if we need the pick/drop string individually later for whatever reason                          
                            .Comment = "Pickup: " + pickup.OrderNumber
                            .StopCompleted = pickup.StopCompleted
                        End With
                        With wpDel
                            .StopNumber = 2
                            .Name = delivery.Name
                            .Address1 = delivery.Address1
                            .Address2 = delivery.Address2
                            .Address3 = delivery.Address3
                            .City = delivery.City
                            .State = delivery.State
                            .Zip = delivery.Zip
                            .Country = delivery.Country
                            .AddressString = delivery.strAddress
                            .Lattitude = If(delivery.LocationLatitude.HasValue, Math.Round(delivery.LocationLatitude.Value, 6), 0)
                            .Longitude = If(delivery.LocationLongitude.HasValue, Math.Round(delivery.LocationLongitude.Value, 6), 0)
                            .StopCategory = Models.MapWaypoint.StopType.Delivery
                            .PickupOrders = "Drop Off: " + delivery.OrderNumber 'we don't really need this field because it is in the comments but I left it in case we need comments for something else later, or if we need the pick/drop string individually later for whatever reason                          
                            .Comment = "Drop Off: " + delivery.OrderNumber
                            .StopCompleted = delivery.StopCompleted
                        End With
                        mapWaypoints.Add(wpPick)
                        mapWaypoints.Add(wpDel)
                    End If

                End If

                'GET TRACK IT WAYPOINTS
                Dim spResTrack = db.spGetTrackItWaypoints(BookControl).ToArray()
                If spResTrack?.Count > 0 Then
                    'Always add the first stop to the track it so we have an origin
                    'Dim firstStop = mapWaypoints.Where(Function(x) x.StopNumber = 1).FirstOrDefault()
                    'trackWaypoints.Add(firstStop)
                    'Add the tracking messages
                    For Each ditem In spResTrack
                        'Modified By LVV on 4/21/20 - don'ditem add empty waypoints that are not null. location is required
                        If String.IsNullOrWhiteSpace(ditem.strAddress) AndAlso ditem.Latitude = 0 AndAlso ditem.Longitude = 0 Then
                            'do nothing
                        Else
                            Dim wp As New Models.MapWaypoint()
                            With wp
                                .Name = ditem.Name
                                .Address1 = ditem.Address1
                                .City = ditem.City
                                .State = ditem.State
                                .Zip = ditem.Zip
                                .Country = ditem.Country
                                .AddressString = ditem.strAddress
                                .Lattitude = If(ditem.Latitude.HasValue, Math.Round(ditem.Latitude.Value, 6), 0)
                                .Longitude = If(ditem.Longitude.HasValue, Math.Round(ditem.Longitude.Value, 6), 0)
                                .StopCategory = Models.MapWaypoint.StopType.Track
                                .Comment = ditem.Comment
                                .ArrivedDate = ditem.ArrivedDate
                                .DepartDate = ditem.DepartDate
                                If .ArrivedDate.HasValue() Then
                                    Dim strArvDt = wp.ArrivedDate.Value.ToString("MM/dd/yyyy hh:mm tt")
                                    .Comment &= " " & strArvDt & " Local Time"
                                End If
                                If .DepartDate.HasValue() Then
                                    If .ArrivedDate.HasValue() Then
                                        If .DepartDate.Value <> .ArrivedDate.Value Then
                                            Dim strDepDt = wp.DepartDate.Value.ToString("MM/dd/yyyy hh:mm tt")
                                            .Comment &= " Departed: " & strDepDt & " Local Time"
                                        End If
                                    Else
                                        Dim strAltDt = wp.DepartDate.Value.ToString("MM/dd/yyyy hh:mm tt")
                                        .Comment &= " " & strAltDt & " Local Time"
                                    End If
                                End If
                            End With
                            trackWaypoints.Add(wp)
                        End If
                    Next
                    For Each ditem In mapWaypoints
                        For Each titem In trackWaypoints
                            If ditem.AddressString = titem.AddressString Then
                                ditem.Comment &= " " & titem.Comment
                            End If
                        Next
                    Next
                    'if the delivery date is in the past then show the dest as final dest pin (may not get an update when arrive so in case never updated)
                    'Dim lastStop = mapWaypoints.Where(Function(x) x.StopNumber = maxStop).FirstOrDefault()
                    'If lastStop.DateDelivered.HasValue AndAlso lastStop.DateDelivered.Value.CompareTo(Date.Now) < 0 Then trackWaypoints.Add(lastStop)
                End If
                routes.MapItWayPoints = mapWaypoints
                routes.TrackItWayPoints = trackWaypoints
                Return routes
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBingMapRoutes"), db)
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookTrack)
        'Create New Record
        Return New LTS.BookTrack With {.BookTrackControl = d.BookTrackControl _
            , .BookTrackBookControl = d.BookTrackBookControl _
            , .BookTrackDate = d.BookTrackDate _
            , .BookTrackContact = d.BookTrackContact _
            , .BookTrackComment = d.BookTrackComment _
            , .BookTrackStatus = d.BookTrackStatus _
            , .BookTrackCommentLocalized = d.BookTrackCommentLocalized _
            , .BookTrackCommentKeys = d.BookTrackCommentKeys _
            , .BookTrackModUser = Parameters.UserName _
            , .BookTrackModDate = Date.Now _
            , .BookTrackUpdated = If(d.BookTrackUpdated Is Nothing, New Byte() {}, d.BookTrackUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetBookTrackFiltered(Control:=CType(LinqTable, LTS.BookTrack).BookTrackControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.BookTrack = TryCast(LinqTable, LTS.BookTrack)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.BookTracks
                    Where d.BookTrackControl = source.BookTrackControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookTrackControl _
                        , .ModDate = d.BookTrackModDate _
                        , .ModUser = d.BookTrackModUser _
                        , .Updated = d.BookTrackUpdated.ToArray}).First

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

    Friend Shared Function selectDTOData(ByVal d As LTS.BookTrack, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookTrack
        Dim oDTO As New DataTransferObjects.BookTrack
        Dim skipObjs As New List(Of String) From {"BookTrackUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .BookTrackUpdated = d.BookTrackUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region

End Class