Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLBookSpotRateData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookSpotRateDatas
        Me.LinqDB = db
        Me.SourceClass = "NGLBookSpotRateData"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMasBookDataContext(ConnectionString)
                _LinqTable = db.BookSpotRateDatas
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
    ''' Returns the SpotRates assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     reads dispatching SpotRate records.  
    ''' </remarks>
    Public Function GetBookSpotRateData(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookSpotRateData()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.FilterValues.Any(Function(x) x.filterName = "BookSpotRateControl") Then
            'we need a BookSpotRateControl fliter or a parent control number
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                throwNoDataFaultException(sMsg)
            Else
                filterWhere = " (BookSpotRateBookControl = " & filters.ParentControl & ") "
                sFilterSpacer = " And "
            End If
        End If

        Dim oRet() As LTS.vBookSpotRateData
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vBookSpotRateData)
                iQuery = db.vBookSpotRateDatas
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookSpotRateControl"
                    filters.sortDirection = "DESC" 'sort by the oldest for spot rate data
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookSpotRateData"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' returns a list of BookFees generated via the vSpotRateFees view inlcuding flat rate fuel if available.  
    ''' These fees are generated dynamically and may not be saved to the database
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/08/2018 
    ''' read view to select Booking Specific Accessorial Codes
    ''' used to store accessorial codes associated with dispatching
    ''' separate from BookFees where accessorial costs are tracked
    ''' Modified by RHR For v-8.2 On 12/10/2018
    '''     added union query To include logic For selecting flat fuel rate
    '''     Rules:
    '''     If BookSpotRateUserCarrierFuelAddendum Is 0 Then use BookSpotRateAvgFuelPrice As flat rate fuel
    '''     Else use Carrier fuel addendum so no fee Is added
    '''     AccessorialCode = 15 Is hard coded as the flat rate accessorial code
    ''' </remarks>
    Public Function GetSpotRateBookFees(ByVal BookControl As Integer) As List(Of BookFee)
        Dim oRet As New List(Of BookFee)

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'If BookSpotRateUserCarrierFuelAddendum Is 0 Then use BookSpotRateAvgFuelPrice As flat rate fuel
                'Else use Carrier fuel addendum.
                Logger.Information("NGLBookDataProvider.GetSpotRateBookFees({0})", BookControl)
                Dim oAccs As List(Of LTS.vSpotRateFee) = db.vSpotRateFees.Where(Function(x) x.BookFeesBookControl = BookControl).ToList()
                Logger.Information("NGLBookDataProvider.GetSpotRateBookFees - Filter fees results:\n{@0}", oAccs)
                Dim sSkip As New List(Of String) 'currently we do not skip any fields in from
                Dim strMsg As String = "" 'not currently used
                If Not oAccs Is Nothing AndAlso oAccs.Count() > 0 Then
                    For Each oItem In oAccs
                        Dim f As New DataTransferObjects.BookFee
                        f = CopyMatchingFields(f, oItem, sSkip, strMsg)
                        oRet.Add(f)
                    Next
                Else
                    Logger.Information("NGLBookDataProvider.GetSpotRateBooksFees({0}) - No fees found", BookControl)
                    oRet.Add(New DataTransferObjects.BookFee())
                End If


            Catch ex As Exception
                Logger.Error(ex, "Error in GetSpotRateBookFees")
                ManageLinqDataExceptions(ex, buildProcedureName("GetSpotRateBookFees"), db)
            End Try
        End Using

        Return oRet
    End Function




    ''' <summary>
    ''' Saves or Inserts a Book Spot Rate Data Record.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     save dispatching Spot Rate Data records.  
    ''' </remarks>
    Public Function SaveBookSpotRateData(ByVal oData As LTS.BookSpotRateData) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iBookControl = oData.BookSpotRateBookControl
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify that a booking record exists
                If iBookControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Booking Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.Books.Any(Function(x) x.BookControl = iBookControl) Then
                    Dim lDetails As New List(Of String) From {"Booking Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.BookSpotRateModDate = Date.Now()
                oData.BookSpotRateModUser = Me.Parameters.UserName
                Dim blnProcessed As Boolean = False
                If oData.BookSpotRateControl = 0 Then
                    db.BookSpotRateDatas.InsertOnSubmit(oData)
                Else
                    db.BookSpotRateDatas.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
                db.spUpdateBookSpotRateDataForSHID(oData.BookSpotRateControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveBookSpotRateData"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteBookSpotRateData(ByVal iBookSpotRateControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookSpotRateControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.BookSpotRateDatas.Where(Function(x) x.BookSpotRateControl = iBookSpotRateControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookSpotRateControl = 0 Then Return True
                db.BookSpotRateDatas.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookSpotRateData"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 12/01/2018
    '''   Creates a load tender record using an existing book record
    '''   used for dispatching a spot rate to a carrier, the new logic 
    '''   follow all other dispatching logic where we dispatch from the selected
    '''   load tender record.
    ''' </remarks>
    Public Function CreateLoadTenderFromBook(ByVal BookControl As Integer) As Integer
        Dim intRet As Integer = 0

        If BookControl = 0 Then Return 0 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Logger.Information("CreateLoadTenderFromBook({0}) - 31158", BookControl)
                Dim oRet = db.spCreateLoadTenderFromBook(BookControl, DataTransferObjects.tblLoadTender.LoadTenderTypeEnum.SpotRate, Me.Parameters.UserName).FirstOrDefault()
                If Not oRet Is Nothing Then
                    intRet = oRet.LoadTenderControl
                End If
            Catch ex As FaultException
                Logger.Error(ex, "CreateLoadTenderFromBook({0})", BookControl)
                'Throw
            Catch ex As Exception
                Logger.Error(ex, "CreateLoadTenderFromBook({0})", BookControl)
                ManageLinqDataExceptions(ex, buildProcedureName("CreateLoadTenderFromBook"), db)
            End Try
        End Using
        Return intRet
    End Function

#End Region

#Region "Protected Functions"


#End Region

End Class