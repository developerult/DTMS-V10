Imports System.ServiceModel

Public Class NGLBookPackage : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookPackages
        Me.LinqDB = db
        Me.SourceClass = "NGLBookPackage"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            ' If _LinqTable Is Nothing Then
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookPackages
            _LinqDB = db
            ' End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Returns the Packages assoicated with a Booking Record the BookControl value must be provided in the filters.ParentControl parameter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     reads dispatching Package records.  this data is not the actual pallet counts assigned to the load 
    '''     only the Packages/pallets that are needed for dispatching and rating
    '''     If the BookPackages Table is empty the results are populated with one record called pallet 
    '''     PalletTypeID of 19
    '''     and a count (BookPkgCount) equal to the total number of pallets
    ''' </remarks>
    Public Function GetBookPackages(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer, Optional ByVal blnUseParameterForOverride As Boolean = True) As LTS.vBookPackage()
        If filters Is Nothing Then Return Nothing
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iBookPkgControl As Integer = 0
        Dim iBookControl As Integer = 0
        If Not filters.FilterValues.Any(Function(x) x.filterName = "BookPkgControl") Then
            'we need a BookPkgControl fliter or a parent control number
            If filters.ParentControl = 0 Then
                Dim sMsg As String = "E_MissingBookingParent" ' "  The reference to the parent booking record is missing. Please select a valid booking record from the load planning page and try again."
                throwNoDataFaultException(sMsg)
            End If
            filterWhere = " (BookPkgBookControl = " & filters.ParentControl & ") "
            sFilterSpacer = " And "
            iBookControl = filters.ParentControl
        Else
            Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "BookPkgControl").FirstOrDefault()
            Integer.TryParse(tFilter.filterValueFrom, iBookPkgControl)
        End If

        Dim oRet() As LTS.vBookPackage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim intPalletCt As Integer = 1
                Dim dblPkgWeight As Double = 1
                Dim lFaks As New List(Of String)
                Dim dNMFC As New Dictionary(Of String, String)
                Dim lNMFC As New List(Of String)
                Dim sFak = "100"
                Dim sNMFC = ""
                Dim sNMFCSub = ""
                Dim iBookPkgPalletTypeID As Integer = 19
                Dim blnBookPkgStackable As Boolean = False
                Dim dBookPkgLength As Double = 48
                Dim dBookPkgWidth As Double = 42
                Dim dBookPkgHeight As Double = 48
                Dim blnAddingNew As Boolean = False

                If iBookControl > 0 AndAlso Not db.BookPackages.Any(Function(x) x.BookPkgBookControl = iBookControl) Then
                    'we create the first package record by default
                    'get the book item totals

                    getItemPackageSettings(db, iBookControl, intPalletCt, dblPkgWeight, sFak, sNMFC, sNMFCSub, iBookPkgPalletTypeID, blnBookPkgStackable, dBookPkgLength, dBookPkgWidth, dBookPkgHeight)
                    Dim oBookPackage As New LTS.BookPackage() With {
                            .BookPkgBookControl = filters.ParentControl,
                            .BookPkgCount = intPalletCt,
                            .BookPkgPalletTypeID = iBookPkgPalletTypeID,
                            .BookPkgWeight = dblPkgWeight,
                            .BookPkgFAKClass = sFak,
                            .BookPkgNMFCClass = sNMFC,
                            .BookPkgNMFCSubClass = sNMFCSub,
                            .BookPkgLength = dBookPkgLength,
                            .BookPkgWidth = dBookPkgWidth,
                            .BookPkgHeight = dBookPkgHeight,
                            .BookPkgStackable = blnBookPkgStackable,
                            .BookPkgUpdated = New Byte() {},
                            .BookPkgModDate = Date.Now(),
                            .BookPkgModUser = "System"
                            }
                    db.BookPackages.InsertOnSubmit(oBookPackage)
                    blnAddingNew = True
                    db.SubmitChanges()
                End If
                Dim iQuery As IQueryable(Of LTS.vBookPackage)

                iQuery = db.vBookPackages
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookPkgControl"
                    filters.sortDirection = "asc"
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                If blnUseParameterForOverride Then
                    If Not blnAddingNew AndAlso Not oRet Is Nothing AndAlso oRet.Count() > 0 Then
                        iBookControl = oRet(0).BookPkgBookControl
                        Dim iCompControl As Integer = db.Books.Where(Function(x) x.BookControl = iBookControl).Select(Function(x) x.BookCustCompControl).FirstOrDefault()
                        Dim dblCheckForChanges = GetParValue("APIUpdatePackagesWithItemDetailChanges", iCompControl)
                        If dblCheckForChanges = 1 Then
                            For Each oItem In oRet
                                If oItem.BookPkgBookControl > 0 Then
                                    getItemPackageSettings(db, oItem.BookPkgBookControl, intPalletCt, dblPkgWeight, sFak, sNMFC, sNMFCSub, iBookPkgPalletTypeID, blnBookPkgStackable, dBookPkgLength, dBookPkgWidth, dBookPkgHeight)
                                    With oItem
                                        .BookPkgCount = intPalletCt
                                        .BookPkgWeight = dblPkgWeight
                                        .BookPkgFAKClass = sFak
                                        .BookPkgNMFCClass = sNMFC
                                        .BookPkgNMFCSubClass = sNMFCSub
                                        .BookPkgPalletTypeID = iBookPkgPalletTypeID
                                        .BookPkgLength = dBookPkgLength
                                        .BookPkgWidth = dBookPkgWidth
                                        .BookPkgHeight = dBookPkgHeight
                                        .BookPkgStackable = blnBookPkgStackable
                                    End With

                                End If
                            Next
                        End If
                    End If
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookPackages"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns all the raw bookPackage records for the provided book control number
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.118 on 9/6/19
    ''' </remarks>
    Public Function GetBookPackages(ByVal iBookControl As Integer) As LTS.BookPackage()
        Dim oRet() As LTS.BookPackage
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRet = db.BookPackages.Where(Function(x) x.BookPkgBookControl = iBookControl).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBookPackages"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Read items and return the pallet information for spot rate and rate shopping
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="iBookControl"></param>
    ''' <param name="intPalletCt"></param>
    ''' <param name="dblPkgWeight"></param>
    ''' <param name="sFak"></param>
    ''' <param name="sNMFC"></param>
    ''' <param name="sNMFCSub"></param>
    ''' <param name="iBookPkgPalletTypeID"></param>
    ''' <param name="blnBookPkgStackable"></param>
    ''' <param name="dBookPkgLength"></param>
    ''' <param name="dBookPkgWidth"></param>
    ''' <param name="dBookPkgHeight"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.006 on 05/09/2024 fixed the default values for the pallets
    ''' </remarks>
    Public Sub getItemPackageSettings(ByRef db As NGLMasBookDataContext,
                                      ByVal iBookControl As Integer,
                                      ByRef intPalletCt As Integer,
                                      ByRef dblPkgWeight As Double,
                                      ByRef sFak As String,
                                      ByRef sNMFC As String, ByRef sNMFCSub As String,
                                      ByRef iBookPkgPalletTypeID As Integer,
                                      ByRef blnBookPkgStackable As Boolean,
                                      ByRef dBookPkgLength As Double,
                                      ByRef dBookPkgWidth As Double,
                                      ByRef dBookPkgHeight As Double)
        intPalletCt = 1
        dblPkgWeight = 1
        Dim lFaks As New List(Of String)
        Dim dNMFC As New Dictionary(Of String, String)
        Dim lNMFC As New List(Of String)
        sFak = "100"
        sNMFC = ""
        sNMFCSub = ""
        blnBookPkgStackable = False
        Try
            'get the default pallet type 
            iBookPkgPalletTypeID = db.PalletTypeRefBooks.Where(Function(x) x.PalletTypeDescription = "PLT").Select(Function(x) x.ID).FirstOrDefault()
            If iBookPkgPalletTypeID < 1 Then iBookPkgPalletTypeID = 19 'use default for PLT but may be wrong zero is not allowed.
        Catch ex As Exception

        End Try
        Dim iBookLoadControls() As Integer = db.BookLoads.Where(Function(x) x.BookLoadBookControl = iBookControl).Select(Function(x) x.BookLoadControl).ToArray()

        If Not iBookLoadControls Is Nothing AndAlso iBookLoadControls.Count() > 0 Then
            Dim oBookItem As LTS.BookItem = db.BookItems.Where(Function(x) iBookLoadControls.Contains(x.BookItemBookLoadControl)).OrderByDescending(Function(x) x.BookItemWeight).FirstOrDefault()
            If Not oBookItem Is Nothing Then
                sFak = oBookItem.BookItemFAKClass
                sNMFC = oBookItem.BookItemNMFCClass
                sNMFCSub = oBookItem.BookItemNMFCSubClass
                iBookPkgPalletTypeID = If(oBookItem.BookItemPalletTypeID > 4, oBookItem.BookItemPalletTypeID, iBookPkgPalletTypeID)
                blnBookPkgStackable = oBookItem.BookItemStackable
            End If
        End If


        ' Modified by RHR for v-8.5.4.006 on 05/09/2024 fixed the default values for the pallets
        dBookPkgLength = GetParValueByLegalEntity("RatingDefaultPltLength", Me.Parameters.UserLEControl)
        If dBookPkgLength <= 0 Then dBookPkgLength = 48

        dBookPkgWidth = GetParValueByLegalEntity("RatingDefaultPltWidth", Me.Parameters.UserLEControl)
        If dBookPkgWidth <= 0 Then dBookPkgWidth = 42

        dBookPkgHeight = GetParValueByLegalEntity("RatingDefaultPltHeight", Me.Parameters.UserLEControl)
        If dBookPkgHeight <= 0 Then dBookPkgHeight = 48


        Dim iPkgTypeID As Integer = iBookPkgPalletTypeID
        Dim oPalletTypeRefBook As LTS.PalletTypeRefBook = db.PalletTypeRefBooks.Where(Function(x) x.ID = iPkgTypeID).FirstOrDefault()
        If Not oPalletTypeRefBook Is Nothing Then
            dBookPkgLength = If(oPalletTypeRefBook.PalletTypeDepth > 0, oPalletTypeRefBook.PalletTypeDepth, 48)
            dBookPkgWidth = If(oPalletTypeRefBook.PalletTypeWidth > 0, oPalletTypeRefBook.PalletTypeWidth, 42)
            dBookPkgHeight = If(oPalletTypeRefBook.PalletTypeHeight > 0, oPalletTypeRefBook.PalletTypeHeight, 48)
        End If
        Dim oBook As LTS.Book = db.Books.Where(Function(x) x.BookControl = iBookControl).FirstOrDefault()

        If Not oBook Is Nothing Then
            If String.IsNullOrWhiteSpace(sNMFC) And String.IsNullOrWhiteSpace(sFak) Then
                sFak = GetParText("UseFAKDefault", oBook.BookCustCompControl)
            End If

            If oBook.BookTotalPL.HasValue() AndAlso oBook.BookTotalPL.Value > 0 Then
                intPalletCt = Math.Ceiling(oBook.BookTotalPL.Value)
            End If
            If oBook.BookTotalWgt.HasValue() AndAlso oBook.BookTotalWgt.Value > 0 Then
                dblPkgWeight = oBook.BookTotalWgt.Value
            End If
        End If
    End Sub

    ''' <summary>
    ''' Saves or Inserts a Book Package Record.  
    ''' The BookPkgPalletTypeID is required and cannot be zero or empty if missing the system will use 
    ''' PalletTypeID 19 for Pallets
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 10/07/2018
    '''     save dispatching Package records.  This data is not the actual pallet counts assigned to the load 
    '''     only the Packages/pallets that are needed for dispatching and rating
    ''' </remarks>
    Public Function SaveBookPackage(ByVal oData As LTS.BookPackage) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iBookControl = oData.BookPkgBookControl

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If oData.BookPkgPalletTypeID < 1 Then
                    'look up the default PLT
                    oData.BookPkgPalletTypeID = db.PalletTypeRefBooks.Where(Function(x) x.PalletTypeDescription = "PLT").Select(Function(x) x.ID).FirstOrDefault()
                End If
                If oData.BookPkgPalletTypeID < 1 Then oData.BookPkgPalletTypeID = 19 'use default for PLT but may be wrong zero is not allowed.
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
                Dim blnProcessed As Boolean = False
                oData.BookPkgModDate = Date.Now()
                oData.BookPkgModUser = Me.Parameters.UserName

                If oData.BookPkgControl = 0 Then
                    db.BookPackages.InsertOnSubmit(oData)
                Else
                    db.BookPackages.Attach(oData, True)
                End If
                db.SubmitChanges()
                'allocate the packages to the item details and booking table.
                'for now we do not process the return values or any errors.
                'this may need to change in the future for now it is not clear 
                'what should happen. the users cannot really fix any errors here.
                Dim oResults = db.spAllocateBookPalletToBooking(oData.BookPkgBookControl, Parameters.UserName)
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveBookPackage"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteBookPackage(ByVal iBookPkgControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iBookPkgControl = 0 Then Return False 'nothing to do
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.BookPackages.Where(Function(x) x.BookPkgControl = iBookPkgControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.BookPkgControl = 0 Then Return True
                db.BookPackages.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookPackage"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class