Imports System.ServiceModel
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLBookLoadBoard : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vBookLoadBoards
        Me.LinqDB = db
        Me.SourceClass = "NGLBookLoadBoard"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.vBookLoadBoards
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    Public Function UpdateCNSNumber(ByVal iBookControl As Integer) As Boolean
        If iBookControl = 0 Then Return False
        Dim oRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oBook = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                If oBook Is Nothing OrElse oBook.BookControl = 0 OrElse oBook.BookCustCompControl = 0 Then Return False
                If Not String.IsNullOrWhiteSpace(oBook.BookSHID) Then
                    throwDataValidationException("Cannot change consolidation number after an SHID has been assigned, use the booking options to remove the order from the load.", SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, False)
                End If
                Dim oDAL = New NGLBatchProcessDataProvider(Parameters)
                Dim sCNS As String = oDAL.GetNextConsNumber(oBook.BookCustCompControl)
                If String.IsNullOrWhiteSpace(sCNS) Then Return False
                oBook.BookConsPrefix = sCNS
                db.SubmitChanges()
                oRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCNSNumber"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetBookLoadBoards(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookLoadBoard()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vBookLoadBoard
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vBookLoadBoard)
                'Dim iQuery As IEnumerable(Of LTS.vBookLoadBoard)
                iQuery = db.vBookLoadBoards

                'Dim provider As New NGLSecurityDataProvider(Me.Parameters)
                'Dim oUser As DTO.tblUserSecurity = provider.GettblUserSecurityByUserName(Parameters.UserName)

                Dim filterWhere As String

                'If oUser.UserUserGroupsControl <> 9 Then

                'End If
                filterWhere = " (UserSecurityControl = " & Parameters.UserControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookStopNo"
                    filters.sortDirection = "asc"
                    iQuery = iQuery.OrderBy("BookControl", True)
                    'filters.sortName = "BookControl"
                    'filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                If filters.sortName <> "BookStopNo" Then
                    iQuery = iQuery.OrderBy("BookStopNo", False)
                End If
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookLoadBoards"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns all Load Board Records filter by SHID, 
    ''' the source for SHID must be a BookControl provided in the filters ParentControl field, 
    ''' other fields and sorting options are optional
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/29/2018
    ''' </remarks>
    Public Function GetSelectedStops(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookLoadBoard()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Book Control is missing. Please return to the Load Board page and select a valid booking record."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vBookLoadBoard
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'get the SHID
                Dim strSHID = db.Books.Where(Function(x) x.BookControl = filters.ParentControl).Select(Function(x) x.BookSHID).FirstOrDefault()

                Dim iQuery As IQueryable(Of LTS.vBookLoadBoard)
                iQuery = db.vBookLoadBoards
                Dim filterWhere As String = " (UserSecurityControl = " & Parameters.UserControl & ") "
                Dim sFilterSpacer As String = " And "
                If String.IsNullOrWhiteSpace(strSHID) Then
                    'we just get the booking record for the book control no SHID assigned
                    filterWhere &= sFilterSpacer & " (BookControl = " & filters.ParentControl & ") "
                Else
                    filterWhere &= String.Concat(sFilterSpacer, "( BookSHID = """, strSHID, """)")
                End If


                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookStopNo"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSelectedStops"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns all Load Board Stop Records filtered by SHID, 
    ''' the source for SHID must be a BookControl provided in the filters ParentControl field, 
    ''' other fields and sorting options are optional
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 11/02/2018
    ''' </remarks>
    Public Function GetLoadBoardStops(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLoadBoardStop()
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Book Control is missing. Please return to the Load Board page and select a valid booking record."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet() As LTS.vLoadBoardStop
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'get the additional filters using bookcontrol
                'OrderFilter
                Dim oBookFilters As Models.OrderFilter = (From b In db.Books
                        Where b.BookControl = filters.ParentControl
                        Select New Models.OrderFilter With {.BookConsPrefix = b.BookConsPrefix, .BookSHID = b.BookSHID, .BookRouteConsFlag = b.BookRouteConsFlag}).FirstOrDefault()

                Dim iQuery As IQueryable(Of LTS.vLoadBoardStop)
                iQuery = db.vLoadBoardStops
                Dim filterWhere As String = " (UserSecurityControl = " & Parameters.UserControl & ") "
                Dim sFilterSpacer As String = " And "
                If Not String.IsNullOrWhiteSpace(oBookFilters.BookSHID) Then
                    'we use the SHID to filter the other records
                    filterWhere &= String.Concat(sFilterSpacer, "( BookSHID = """, oBookFilters.BookSHID, """)")
                ElseIf Not String.IsNullOrWhiteSpace(oBookFilters.BookConsPrefix) And oBookFilters.BookRouteConsFlag Then
                    'we use the CNS number and BookRouteConsFlag
                    filterWhere &= String.Concat(sFilterSpacer, "( BookConsPrefix = """, oBookFilters.BookConsPrefix, """) AND ( BookRouteConsFlag = true)")
                Else
                    'we just get the booking record for the book control no SHID assigned
                    filterWhere &= sFilterSpacer & " (BookControl = " & filters.ParentControl & ") "
                End If


                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookStopNo"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSelectedStops"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetLoadBoardStop(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLoadBoardStop
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Book Control is missing. Please return to the Load Board page and select a valid booking record."
            throwNoDataFaultException(sMsg)
        End If
        Dim oRet As LTS.vLoadBoardStop
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vLoadBoardStop)
                iQuery = db.vLoadBoardStops
                Dim filterWhere As String = " (UserSecurityControl = " & Parameters.UserControl & ") "
                Dim sFilterSpacer As String = " And "
                filterWhere &= sFilterSpacer & " (BookControl = " & filters.ParentControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSelectedStop"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' LTS save method for D365
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Modified by RHR for v-8.3.0.002 on 10/07/2020  new logic to check for conflicts before save.
    ''' Modified by RHR v-8.4.003 on 08/27/2021  due to javascript formating options 
    ''' </remarks>
    Public Function SaveBookLoadBoard(ByVal oData As LTS.vBookLoadBoard) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        If oData.BookControl = 0 Then Return False 'nothing to do
        Dim iBookControl = oData.BookControl
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookControl = 0 Then
                    throwRecordDeletedFaultException("Cannot save load boad booking changes for order number : " & oData.BookCarrOrderNumber)
                End If
                'Modified By LVV on 4/24/20 - On the Load Board Edit popup window do not allow the users to edit the CNS number when an SHID has been assigned. Users must remove the order from the current load before changing the CNS number.
                If oExisting.BookConsPrefix <> oData.BookConsPrefix AndAlso Not String.IsNullOrWhiteSpace(oExisting.BookSHID) Then
                    throwFaultException("Cannot change the CNS after an SHID has been assigned. You must remove the order from the current load before changing the CNS.", "", New List(Of String), "E_DataValidationFailure", False)
                End If
                'Modified by RHR for v-8.3.0.002 on 10/07/2020  new logic to check for conflicts before save.



                'Private _BookLoadControl As Integer

                'Private _BookLoadPONumber As String


                'Private _BookItemOrderNumbers As String

                'Private _TransType As String

                'Private _NEXTStop As System.Nullable(Of Boolean)

                'Private _DAT As System.Nullable(Of Boolean)

                'Private _BookRouteFinalCode As String

                'Private _txtConfirmed As String

                'Private _lblConfirmed As String
                'Modified by RHR for v-8.4.0.003 on 07/09/21  added logic to update BookDestStopNumber
                Dim blnUpdateDestStopNo As Boolean = False
                Dim BookStopNoOld As Short?
                Dim BookStopNoNew As Short?
                Dim BookDestStopNoOld As Short?
                If oData.BookStopNo <> oExisting.BookStopNo Then
                    blnUpdateDestStopNo = True
                    BookStopNoOld = oExisting.BookStopNo
                    BookStopNoNew = oData.BookStopNo
                    BookDestStopNoOld = oExisting.BookDestStopNumber
                End If

                'Modified by RHR v-8.4.003 on 08/27/2021  due to javascript formating options the acutal date may be off by milliseconds when converting from date to string and back to date
                'If oData.BookModDate <> oExisting.BookModDate Then
                If DateDiff("s", oData.BookModDate, oExisting.BookModDate) <> 0 Then
                    'the data may have changed so check each field for conflicts
                    'Modified by RHR 10/12/2020 we now use reflection via CheckForDataConflicts 
                    Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                    Dim oSkip As New List(Of String) From {"BookControl",
                            "LaneNumber",
                            "BookODControl",
                            "LaneName",
                            "LaneBenchMiles",
                            "CarrierName",
                            "CarrierNumber",
                            "CompName",
                            "CompNumber",
                            "BookCarrierControl",
                            "CompFinUseImportFrtCost",
                            "BookProNumber",
                            "BookAMSPickupApptControl",
                            "BookAMSDeliveryApptControl",
                            "BookCarrOrderNumber",
                            "BookLoadControl",
                            "BookLoadPONumber",
                            "BookLoadCom",
                            "BookNotesVisable1",
                            "BookNotesVisable2",
                            "BookNotesVisable3",
                            "BookNotesVisable4",
                            "LaneOriginAddressUse",
                            "UserSecurityControl",
                            "BookItemOrderNumbers",
                            "NEXTStop",
                            "DAT",
                            "txtConfirmed",
                            "lblConfirmed",
                            "CreditAvailable",
                            "OnCreditHold",
                            "BookModDate",
                            "BookModUser",
                            "BookUpdated",
                            "ModeTypeName"}
                    Dim blnConflictFound As Boolean = CheckForDataConflicts(oData, oExisting, oSkip, ConflictData)
                    If blnConflictFound Then
                        'We only add the mod date and mod user if one or more other fields have been modified
                        addToConflicts("BookModDate", oData.BookModDate, oExisting.BookModDate, ConflictData, blnConflictFound)
                        addToConflicts("BookModUser", oData.BookModUser, oExisting.BookModUser, ConflictData, blnConflictFound)
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                        conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                    End If
                End If


                Dim strMSG As String = ""
                Dim skipObjs As New List(Of String) From {"BookModDate",
                        "BookModUser",
                        "BookAMSPickupApptControl",
                        "BookAMSDeliveryApptControl",
                        "BookRevBilledBFC",
                        "BookRevTotalCost"}
                oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oData, skipObjs, strMSG)
                With oExisting
                    .BookModDate = Date.Now
                    .BookModUser = Me.Parameters.UserName
                    If blnUpdateDestStopNo Then
                        .BookDestStopNumber = calculateBookDestStopNumber(BookStopNoOld, BookStopNoNew, BookDestStopNoOld)
                    End If
                End With
                'update any notes.
                Dim oBookNotes = db.BookNotes.Where(Function(x) x.BookNotesBookControl = iBookControl).FirstOrDefault()
                If Not oBookNotes Is Nothing Then
                    oBookNotes.BookNotesVisable1 = oData.BookNotesVisable1
                    oBookNotes.BookNotesVisable2 = oData.BookNotesVisable2
                    oBookNotes.BookNotesVisable3 = oData.BookNotesVisable3

                    oBookNotes.BookNotesBookUser1 = oData.BookNotesBookUser1
                    oBookNotes.BookNotesBookUser2 = oData.BookNotesBookUser2
                    oBookNotes.BookNotesBookUser3 = oData.BookNotesBookUser3
                    oBookNotes.BookNotesBookUser4 = oData.BookNotesBookUser4
                Else
                    Dim oNewNotes = New LTS.BookNote With {.BookNotesBookControl = iBookControl,
                            .BookNotesVisable1 = oData.BookNotesVisable1,
                            .BookNotesVisable2 = oData.BookNotesVisable2,
                            .BookNotesVisable3 = oData.BookNotesVisable3,
                            .BookNotesBookUser1 = oData.BookNotesBookUser1,
                            .BookNotesBookUser2 = oData.BookNotesBookUser2,
                            .BookNotesBookUser3 = oData.BookNotesBookUser3,
                            .BookNotesBookUser4 = oData.BookNotesBookUser4}
                    db.BookNotes.InsertOnSubmit(oNewNotes)
                End If
                'Modified by RHR for v-8.5.4.006 on 04/16/24 update BookLoad data
                Dim iBookLoadControl = oData.BookLoadControl
                Dim oBookLoad = db.BookLoads.Where(Function(x) x.BookLoadControl = iBookLoadControl).FirstOrDefault()
                If Not oBookNotes Is Nothing Then
                    oBookLoad.BookLoadPONumber = oData.BookLoadPONumber
                    oBookLoad.BookLoadCom = oData.BookLoadCom
                End If

                '                BookLoad
                '                dbo.BookLoad.BookLoadControl,
                'dbo.BookLoad.BookLoadPONumber,
                'dbo.BookLoad.BookLoadCom,

                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveBookLoadBoard"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' updates the bookhold load value for the book control 
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <param name="blnHold"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR for v-8.4.0.001 on 2/11/2021
    ''' </remarks>
    Public Function UpdateBookHoldLoad(ByVal iBookControl As Integer, ByVal blnHold As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Dim iHoldLoad As Integer = 0
        If blnHold Then iHoldLoad = 1
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the records exist
                Dim oData = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                If oData Is Nothing OrElse oData.BookControl = 0 Then Return False 'nothing to do
                oData.BookHoldLoad = iHoldLoad
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookLoadBoardHold"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function ResetAPExport(ByVal iBookControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim sBookSHID As String = ""

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                sBookSHID = db.Books.Where(Function(x) x.BookControl = iBookControl).Select(Function(y) y.BookSHID).FirstOrDefault()
                Dim oData As New List(Of LTS.Book)
                If Not String.IsNullOrWhiteSpace(sBookSHID) Then
                    oData = db.Books.Where(Function(x) x.BookSHID = sBookSHID).ToList()
                Else
                    oData = db.Books.Where(Function(x) x.BookControl = iBookControl).ToList()
                End If
                'verify the records exist
                If oData Is Nothing OrElse oData.Count() < 1 Then Return False 'nothing to do
                For Each d In oData
                    d.BookFinAPExportDate = Nothing
                    d.BookFinAPExportRetry = 0
                    d.BookFinAPExportFlag = 0
                    d.BookTranCode = "PB"
                    d.BookFinAPCheck = Nothing
                    d.BookFinARCheck = Nothing
                    d.BookFinCommtCheck = Nothing
                Next

                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResetAPExport"), db)
            End Try
        End Using
        Return blnRet
    End Function



    ''' <summary>
    ''' updates all records with oChangeSHID.Current_SHID equal to oChangeSHID.New_SHID, 
    ''' optionaly updates the BookShipCarrierPro data with oChangeSHID.New_SHID when then
    ''' optional flag  oChangeSHID.Change_Carrier_Pro is true
    ''' </summary>
    ''' <param name="oChangeSHID"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR for v-8.2.1.004 on 12/02/2019
    ''' </remarks>
    Public Function UpdateBookLoadBoardSHID(ByVal oChangeSHID As LTS.vLoadBoardChangeSHID) As Boolean
        Dim blnRet As Boolean = False

        If oChangeSHID Is Nothing OrElse (String.IsNullOrWhiteSpace(oChangeSHID.CurrentSHID) Or String.IsNullOrWhiteSpace(oChangeSHID.NewSHID)) Then Return False 'nothing to do

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the records exist
                Dim oData = db.Books.Where(Function(x) x.BookSHID = oChangeSHID.CurrentSHID).ToArray()
                If oData Is Nothing OrElse oData.Count < 1 Then Return False 'nothing to do
                For Each ditem In oData
                    ditem.BookSHID = oChangeSHID.NewSHID
                    ditem.BookModDate = Date.Now
                    ditem.BookModUser = Me.Parameters.UserName
                    If oChangeSHID.ChangeCarrierPro.HasValue AndAlso oChangeSHID.ChangeCarrierPro = True Then
                        ditem.BookShipCarrierProNumber = oChangeSHID.NewSHID
                        ditem.BookShipCarrierProNumberRaw = oChangeSHID.NewSHID
                    End If
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBookLoadBoardSHID"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function GetBookLoadBoardChangeSHID(ByVal BookControl As Integer) As LTS.vLoadBoardChangeSHID
        Dim oRet As New LTS.vLoadBoardChangeSHID()
        If BookControl = 0 Then Return oRet 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vLoadBoardChangeSHIDs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookLoadBoardChangeSHID"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function DeleteBookLoadBoard(ByVal iBookControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookControl = 0 Then Return True
                db.Books.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookLoadBoard"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function GetLoadBoardSummary(ByVal iBookControl As Integer) As LTS.vLoadBoardSummary

        Dim oRet As New LTS.vLoadBoardSummary()

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.vLoadBoardSummaries.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardSummary"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Reads Summary Data For Load Board by SHID or CNS if SHID is not assigned
    ''' </summary>
    ''' <param name="sSHID"></param>
    ''' <param name="sCNS"></param>
    ''' <returns></returns>
    Public Function GetLoadBoardShipingSummary(ByVal sSHID As String, ByVal sCNS As String) As LTS.vLoadBoardShipingSummary

        Dim oRet As New LTS.vLoadBoardShipingSummary()

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If (Not String.IsNullOrWhiteSpace(sSHID)) Then
                    oRet = db.vLoadBoardShipingSummaries.Where(Function(x) x.BookSHID = sSHID).FirstOrDefault()
                End If
                If ((oRet Is Nothing OrElse String.IsNullOrWhiteSpace(oRet.BookSHID)) And Not String.IsNullOrWhiteSpace(sCNS)) Then
                    oRet = db.vLoadBoardShipingSummaries.Where(Function(x) x.BookConsPrefix = sCNS).FirstOrDefault()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardShipingSummary"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetLoadBoardShipingSummary(ByVal iBookControl As Integer) As LTS.vLoadBoardShipingSummary

        Dim oRet As New LTS.vLoadBoardShipingSummary()

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oBook = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                If oBook Is Nothing OrElse oBook.BookControl = 0 Then
                    Return oRet
                End If
                Return GetLoadBoardShipingSummary(oBook.BookSHID, oBook.BookConsPrefix)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBoardShipingSummary"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Checks if the Delivery date has changed and if it has adds a Load Status message.
    ''' NOTE: Currently we are only comparing date only - NOT including time
    ''' </summary>
    ''' <param name="newDelDate"></param>
    ''' <param name="BookControl"></param>
    ''' <remarks>Added By LVV on 7/23/20 for v-8.3.0.001 Task #20200609162226 - Load Board Delivered Date</remarks>
    Public Sub DeliveryDateManuallyChanged(ByVal newDelDate As Date?, ByVal BookControl As Integer)
        If BookControl = 0 Then Return  'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim strOldDelDate = "NULL"
                Dim strNewDelDate = "NULL"
                If Not db.Books.Any(Function(x) x.BookControl = BookControl) Then Return 'nothing to do
                Dim oldDelDate = db.Books.Where(Function(x) x.BookControl = BookControl).Select(Function(y) y.BookCarrActDate).FirstOrDefault()
                If (oldDelDate.HasValue) Then strOldDelDate = oldDelDate.Value.ToShortDateString()
                If (newDelDate.HasValue) Then strNewDelDate = newDelDate.Value.ToShortDateString()
                If strOldDelDate <> strNewDelDate Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    Dim strComment = String.Format(oLocalize.GetLocalizedValueByKey("MsgDelDateManuallyChanged", "Load Delivery Date Manually Updated From {0} To {1}"), strOldDelDate, strNewDelDate)
                    Dim oBookTrack = New LTS.BookTrack
                    With oBookTrack
                        .BookTrackBookControl = BookControl
                        .BookTrackDate = Date.Now
                        .BookTrackContact = Parameters.UserName
                        .BookTrackComment = strComment
                        .BookTrackModUser = Parameters.UserName
                        .BookTrackModDate = Date.Now
                        .BookTrackStatus = 0
                    End With
                    db.BookTracks.InsertOnSubmit(oBookTrack)
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeliveryDateManuallyChanged"), db)
            End Try
        End Using
    End Sub


    Public Function ConfirmShip(ByVal iBookControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim msg As String = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oBook As LTS.Book = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()
                'verify the records exist
                If oBook Is Nothing OrElse oBook.BookControl = 0 Then
                    msg = NGLcmLocalizeKeyValuePairObjData.GetLocalizedValueByKey("MSGConfirmShipInvlaidRecord", "Cannot confirm shipment the selected booking record is invalid or cannot be located.")
                    throwDataValidationException(msg, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, False)
                End If

                'check if the order has been finalized
                If Not oBook.BookRouteFinalFlag Then
                    msg = NGLcmLocalizeKeyValuePairObjData.GetLocalizedValueByKey("MSGConfirmShipInvlaidFinalFlag", "Cannot change the status of an order to ship confirmed until the order has been finalized.  Please finalize the order and try again.")
                    throwDataValidationException(msg, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, False)
                End If
                Dim strCNS As String = ""
                Dim strPrevValue As String = oBook.BookRouteFinalCode
                strCNS = oBook.BookConsPrefix
                Dim strPrevFinalCode = oBook.BookRouteFinalCode
                oBook.BookRouteFinalCode = "SC"
                oBook.BookModDate = Date.Now()
                oBook.BookModUser = Me.Parameters.UserName
                db.SubmitChanges()

                If GetParValue("ImportAddConfirmedToPicklist", oBook.BookCustCompControl) = 1 Then
                    Try
                        blnRet = NGLBatchProcessObjData.generatePickListRecord2Way(oBook.BookControl, oBook.BookConsPrefix)

                    Catch ex As Exception
                        msg = ex.Message
                        blnRet = False

                    End Try
                    If Not blnRet Then
                        oBook.BookRouteFinalCode = strPrevValue
                        db.SubmitChanges()
                        throwDataValidationException(msg, SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, True)
                    End If
                Else
                    blnRet = True
                End If

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ConfirmShip"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class