Imports System.ServiceModel
Imports SerilogTracing
Imports Serilog
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLBookAccessorial : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookAccessorials
        Me.LinqDB = db
        Me.SourceClass = "NGLBookAccessorial"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookAccessorials
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Returns the Accessorials assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     reads dispatching accessorial records.  this data is not the actual fee assigned to the load only the accessorials that may require a fee
    '''     some fees may contain the anticipated/expected cost but the actual estimated cost is stored in the BookFees table
    ''' </remarks>
    Public Function GetBookAccessorials(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBookAccessorial()
        Dim oRet() As LTS.vBookAccessorial
        Dim newRet As New List(Of LTS.BookAccessorial)
        Using operation = Logger.StartActivity("GetBookAccessorials(Filters: {@filters}, RecordCount: {RecordCount}", filters, RecordCount)

            If filters Is Nothing Then
                operation.Complete()
                Return Nothing
            End If

            Dim filterWhere As String = ""
            Dim sFilterSpacer As String = ""
            Dim iBookAcssControl As Integer = 0
            Dim iBookControl As Integer = 0
            Dim dtMod As Date = Date.Now 'we use a variable so all mod dates are the same
            If Not filters.FilterValues.Any(Function(x) x.filterName = "BookAcssControl") Then
                'we need a BookAcssControl fliter or a parent control number
                If filters.ParentControl = 0 Then
                    Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                    throwNoDataFaultException(sMsg)
                End If
                filterWhere = " (BookAcssBookControl = " & filters.ParentControl & ") "
                sFilterSpacer = " And "
                iBookControl = filters.ParentControl
            Else
                Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "BookAcssControl").FirstOrDefault()
                Integer.TryParse(tFilter.filterValueFrom, iBookAcssControl)
            End If

            Using db As New NGLMasBookDataContext(ConnectionString)
                Try
                    If iBookControl > 0 AndAlso Not db.BookAccessorials.Any(Function(x) x.BookAcssBookControl = iBookControl) Then
                        'we do not have any fees for this booking record so try to add the defaults if available
                        'Add any order specific fees that have already been assigned.

                        Dim oBookFees() As LTS.BookFee = db.BookFees.Where(Function(x) x.BookFeesBookControl = iBookControl AndAlso x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order).ToArray()

                        Logger.Information("Found {BookFeesCount} Order Specific Fees for Booking Control: {BookingControl}", oBookFees.Count, iBookControl)

                        If Not oBookFees Is Nothing AndAlso oBookFees.Count > 0 Then

                            For Each oItem In oBookFees
                                Dim iAccCode = oItem.BookFeesAccessorialCode

                                If iAccCode.HasValue() AndAlso iAccCode.Value > 0 Then
                                    'check if the NAC code exists
                                    Logger.Information("Lookup tblAccessorialNGLAPIFeesXRefs where ANAFXrefAccessorialCode = iAccCode ({iAccCode})", iAccCode)
                                    Dim iNACControl As Integer = db.tblAccessorialNGLAPIFeesXrefs.Where(Function(x) x.ANAFXrefAccessorialCode = iAccCode)?.Select(Function(x) x.ANAFXrefNACControl).FirstOrDefault()
                                    If iNACControl > 0 Then
                                        Dim dValue As Decimal = oItem.BookFeesValue
                                        Logger.Information("Found {AccessorialCode} xRef to NACControl: {NACControl}.  BookFeesValue: {BookFeesValue}, check to see if that is less than or = 0", iAccCode, iNACControl, dValue)
                                        If dValue <= 0 Then
                                            If oItem.BookFeesMinimum.HasValue Then
                                                dValue = oItem.BookFeesMinimum.Value
                                                Logger.Information("{AccessorialCode} Accessorial Minimum Value: {AccessorialValue}", iAccCode, dValue)
                                            Else
                                                dValue = 0 'we set dValue to zero in the case where oItem.BookFeesValue is less than or equal to zero
                                                Logger.Information("{AccessorialCode} Accessorial Value {AccessorialValue} was > 0", iAccCode, dValue)
                                            End If
                                        End If
                                        Logger.Information("Adding BookAccessorial: {AccessorialCode} NACControl: {NACControl} BookControl: {BookControl} Value: {Value}", iAccCode, iNACControl, iBookControl, dValue)
                                        newRet.Add(New LTS.BookAccessorial With {
                                                   .BookAcssAccessorialCode = iAccCode,
                                                   .BookAcssNACControl = iNACControl,
                                                   .BookAcssBookControl = iBookControl,
                                                   .BookAcssValue = dValue,
                                                   .BookAcssModDate = dtMod,
                                                   .BookAcssModUser = "System Generated"
                                                   })

                                    End If
                                Else
                                    Logger.Information("Accessorial Code {AccessorialCode} was not found in tblAccessorialNGLAPIFeesXrefs", iAccCode)
                                End If

                            Next
                        Else
                            Logger.Information("No Order Specific Fees found for Booking Control: {BookingControl}", iBookControl)
                        End If

                        'Add any lane specific fees that have already been assigned.

                        Logger.Information("Lookup Lane Fees for Booking Control: {BookingControl}", iBookControl)

                        Dim iLaneControl As Integer = db.Books.Where(Function(x) x.BookControl = filters.ParentControl)?.Select(Function(x) x.BookODControl).FirstOrDefault()
                        If iLaneControl > 0 Then
                            'get any lane fees
                            Logger.Information("Lookup Lane Fees for LaneControl: {LaneControl}", iLaneControl)
                            Dim oLaneFees = db.LaneFeeRefBooks.Where(Function(x) x.LaneFeesLaneControl = iLaneControl)?.ToArray()

                            Logger.Information("Found {LaneFeesCount} Lane Fees for Lane Control: {LaneControl}", oLaneFees.Count, iLaneControl)

                            If Not oLaneFees Is Nothing AndAlso oLaneFees.Count() > 0 Then
                                For Each oItem In oLaneFees
                                    Dim iAccCode = oItem.LaneFeesAccessorialCode
                                    If iAccCode.HasValue() AndAlso iAccCode.Value > 0 Then
                                        'check if the NAC code exists
                                        Logger.Information("LaneFee - Lookup tblAccessorialNGLAPIFeesXRefs where ANAFXrefAccessorialCode = {iAccCode}", iAccCode)
                                        Dim iNACControl As Integer = db.tblAccessorialNGLAPIFeesXrefs.Where(Function(x) x.ANAFXrefAccessorialCode = iAccCode).Select(Function(x) x.ANAFXrefNACControl).FirstOrDefault()
                                        If iNACControl > 0 Then
                                            Dim dValue As Decimal = 0
                                            Logger.Information("{AccessorialCode} Accessorial Value: {AccessorialValue}", iAccCode, dValue)
                                            If oItem.LaneFeesMinimum.HasValue AndAlso oItem.LaneFeesMinimum.Value > 0 Then
                                                dValue = oItem.LaneFeesMinimum.Value
                                                Logger.Information("LaneFee - {AccessorialCode} Has Lane Fee Minimum Value: {AccessorialValue}", iAccCode, dValue)
                                            End If

                                            Logger.Information("Adding BookAccessorial: {AccessorialCode} NACControl: {NACControl} BookControl: {BookControl} Value: {Value}", iAccCode, iNACControl, iBookControl, dValue)
                                            newRet.Add(New LTS.BookAccessorial With {
                                                       .BookAcssAccessorialCode = iAccCode,
                                                       .BookAcssNACControl = iNACControl,
                                                       .BookAcssBookControl = iBookControl,
                                                       .BookAcssValue = dValue,
                                                       .BookAcssModDate = dtMod,
                                                       .BookAcssModUser = "System Generated"
                                                       })

                                        End If
                                    End If
                                Next
                            End If

                            If Not newRet Is Nothing AndAlso newRet.Count() > 0 Then
                                db.BookAccessorials.InsertAllOnSubmit(newRet)
                            End If
                            db.SubmitChanges()
                        End If
                    Else
                        Logger.Information("No Booking Control provided or Booking Control {BookingControl} already has fees assigned", iBookControl)
                    End If
                    Dim iQuery As IQueryable(Of LTS.vBookAccessorial)
                    iQuery = db.vBookAccessorials


                    If String.IsNullOrWhiteSpace(filters.sortName) Then
                        filters.sortName = "BookAcssControl"
                        filters.sortDirection = "asc"
                    End If

                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    'db.Log = New DebugTextWriter
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error in GetBookAccessorials")
                    'ManageLinqDataExceptions(ex, buildProcedureName("GetBookAccessorials"), db)
                End Try
            End Using
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Saves or Inserts a Book Accessorial Record.  
    ''' The BookAcssNACControl is unique for each book record so duplicates will be overwritten
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     save dispatching accessorial records.  this data is not the actual fee assigned to the load only the accessorials that may require a fee
    '''     some fees may contain the anticipated/expected cost but the actual estimated cost is stored in the BookFees table
    ''' </remarks>
    Public Function SaveBookAccessorial(ByVal oData As LTS.BookAccessorial) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iBookControl = oData.BookAcssBookControl
        Dim iNACControl As Integer = oData.BookAcssNACControl
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

                If iNACControl = 0 OrElse Not db.tblAccessorialNGLAPIFeesXrefs.Any(Function(x) x.ANAFXrefNACControl = iNACControl) Then
                    'build list for details to match message:
                    'Cannot save changes to {0}.  The following key fields are required: {1}
                    throwInvalidRequiredKeysException(" Book Accessorial Codes ", " Accessorial Fee or Code ")
                    Return False
                End If


                Dim blnProcessed As Boolean = False
                'We always look up the Accessorial Code because the user selects the NACControl from a list
                oData.BookAcssAccessorialCode = db.tblAccessorialNGLAPIFeesXrefs.Where(Function(x) x.ANAFXrefNACControl = iNACControl).Select(Function(x) x.ANAFXrefAccessorialCode).FirstOrDefault()
                oData.BookAcssModDate = Date.Now()
                oData.BookAcssModUser = Me.Parameters.UserName
                If oData.BookAcssControl = 0 Then
                    'this is a new record,  check for existing code if found we update, else we insert
                    'only one BookAcssNACControl should exist for each booking record
                    If Not db.BookAccessorials.Any(Function(x) x.BookAcssBookControl = iBookControl And x.BookAcssNACControl = iNACControl) Then
                        db.BookAccessorials.InsertOnSubmit(oData)
                        blnProcessed = True
                    Else
                        Dim oExisting = db.BookAccessorials.Where(Function(x) x.BookAcssBookControl = iBookControl And x.BookAcssNACControl = iNACControl).FirstOrDefault()
                        If Not oExisting Is Nothing AndAlso oExisting.BookAcssControl <> 0 Then
                            oExisting.BookAcssAccessorialCode = oData.BookAcssAccessorialCode
                            oExisting.BookAcssValue = oData.BookAcssValue
                        End If
                        blnProcessed = True
                    End If
                End If
                If Not blnProcessed Then
                    'validate that BookAcssNACControl is unique
                    If Not db.BookAccessorials.Any(Function(x) x.BookAcssBookControl = iBookControl And x.BookAcssNACControl = iNACControl And x.BookAcssControl <> oData.BookAcssControl) Then
                        db.BookAccessorials.Attach(oData, True)
                    Else
                        Dim sCode As String = db.tblNGLAPICodeRefBooks.Where(Function(x) x.NACControl = iNACControl).Select(Function(x) x.NACCode).FirstOrDefault()
                        throwInvalidKeyAlreadyExistsException("Book Accessorial", "Accessorial Code", sCode)
                    End If
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveBookAccessorial"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteBookAccessorial(ByVal iBookAcssControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookAcssControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.BookAccessorials.Where(Function(x) x.BookAcssControl = iBookAcssControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookAcssControl = 0 Then Return True
                db.BookAccessorials.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookAccessorial"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Lookup the NACCode in vLookupAcssCodes to get the Accessorial EDI Code
    ''' </summary>
    ''' <param name="NACCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-85.4.005 on 02/29/2024 used to help with API Fee Mapping
    ''' </remarks>
    Public Function GetAccessorialEDICodeFromNACCode(ByVal NACCode As String, Optional ByVal sDef As String = "MSC") As String
        Dim sRet As String = sDef
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Logger.Information("GetAccessorialEDICodeFromNACCode: {NACCode} ", NACCode)
                If db.vLookupAcssCodes.Any(Function(x) x.NACCode = NACCode) Then
                    Dim sAccessorialName = db.vLookupAcssCodes.Where(Function(x) x.NACCode = NACCode).Select(Function(y) y.NACName).FirstOrDefault()
                    Logger.Information("GetAccessorialEDICodeFromNACCode: {NACCode} found {sAccessorialName} ", NACCode, sAccessorialName)
                    If db.tblAccessorials.Any(Function(x) x.AccessorialName = sAccessorialName) Then
                        sRet = db.tblAccessorials.Where(Function(x) x.AccessorialName = sAccessorialName).Select(Function(y) y.AccessorialEDICode).FirstOrDefault()
                        Logger.Information("GetAccessorialEDICodeFromNACCode: {NACCode} found {sAccessorialName} with EDI Code {sRet} ", NACCode, sAccessorialName, sRet)
                    ElseIf (NACCode = "PRISON" Or NACCode = "GOV" Or NACCode = "CONS") Then
                        sRet = "ACH"
                        Logger.Information("GetAccessorialEDICodeFromNACCode: if Code is PRISON, GOV, or CONS then set to {sRet}", sRet)
                    End If
                End If

            Catch ex As Exception
                Logger.Error(ex, "GetAccessorialEDICodeFromNACCode: {NACCode} ", NACCode)

            End Try
        End Using
        Return sRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class