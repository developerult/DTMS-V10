Imports System.Data.Linq
Imports System.ServiceModel
Imports Ngl.FreightMaster.Data.LTS

Public Class NGLCarrierContData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierConts
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierContData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierConts
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

#Region "Overridden data methods"
    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrierContFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierContsFiltered()
    End Function


    ''' <summary>
    ''' Override for Insert provides custom mapping to LE Carrier Control Xref
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    ''' </remarks>
    Public Overrides Function Add(Of TEntity As Class)(ByVal oData As DataTransferObjects.DTOBaseClass, ByVal oLinqTable As Table(Of TEntity)) As DataTransferObjects.DTOBaseClass
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateNewRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            oLinqTable.InsertOnSubmit(nObject)
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddCarrierCont"), LinqDB)
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            CreateCleanUp(nObject)
            If nObject.CarrierContactDefault = True Then
                UpdateCarrierContactDefaultFlag(nObject.CarrierContControl, nObject.CarrierContCarrierControl, nObject.CarrierContactDefault, nObject.CarrierContLECarControl)
            End If
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    '''' <summary>
    '''' Override for update provides custom mapping to LE Carrier Control Xref
    '''' </summary>
    '''' <typeparam name="TEntity"></typeparam>
    '''' <param name="oData"></param>
    '''' <param name="oLinqTable"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Modified by RHR for v-8.2 on 04/29/2019
    ''''  we now call GetLECarControl to look up the Legal Entity Carrier Control XRef Value
    '''' </remarks>
    'Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
    '                            ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
    '    Dim nObject As Object
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        'Note: the ValidateData Function must throw a FaultException error on failure
    '        ValidateUpdatedRecord(db, oData)
    '        With DirectCast(oData, DTO.CarrierCont)
    '            If .CarrierContLECarControl = 0 Then
    '                .CarrierContLECarControl = GetLECarControl(.CarrierContCarrierControl, db)
    '            End If
    '        End With
    '        'Create The Record 
    '        nObject = CopyDTOToLinq(oData)
    '        ' Attach the record 
    '        db.CarrierConts.Attach(nObject, True)
    '        Try
    '            db.SubmitChanges()
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("UpdateCarrierCont"), db)
    '        End Try
    '    End Using

    '    PostUpdate(LinqDB, oData)
    '    ' Return the updated order
    '    Return GetDTOUsingLinqTable(nObject)
    'End Function

    'Public Overrides Function UpdateRecordQuick(ByVal oData As DTO.DTOBaseClass) As DTO.QuickSaveResults
    '    Dim nObject As Object
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        'Note: the ValidateData Function must throw a FaultException error on failure
    '        ValidateUpdatedRecord(db, oData)
    '        With DirectCast(oData, DTO.CarrierCont)
    '            If .CarrierContLECarControl = 0 Then
    '                .CarrierContLECarControl = GetLECarControl(.CarrierContCarrierControl, db)
    '            End If
    '        End With
    '        'Create The Record 
    '        nObject = CopyDTOToLinq(oData)
    '        ' Attach the record 
    '        db.CarrierConts.Attach(nObject, True)

    '        Try
    '            db.SubmitChanges()
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("UpdateCarrierContQuick"), db)
    '        End Try
    '    End Using
    '    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    '    'This method optionally performs any additional functions or cleanup needed after a save
    '    PostUpdate(LinqDB, oData)
    '    ' Return the quick results object
    '    Return GetQuickSaveResults(nObject)

    'End Function

    'Public Overrides Sub UpdateRecordNoReturn(ByVal oData As DTO.DTOBaseClass)
    '    Dim nObject As Object
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        'Note: the ValidateData Function must throw a FaultException error on failure
    '        ValidateUpdatedRecord(db, oData)
    '        With DirectCast(oData, DTO.CarrierCont)
    '            If .CarrierContLECarControl = 0 Then
    '                .CarrierContLECarControl = GetLECarControl(.CarrierContCarrierControl, db)
    '            End If
    '        End With
    '        'Create The Record 
    '        nObject = CopyDTOToLinq(oData)
    '        ' Attach the record 
    '        db.CarrierConts.Attach(nObject, True)

    '        Try
    '            db.SubmitChanges()
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("UpdateCarrierContNoReturn"), db)
    '        End Try
    '    End Using
    '    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    '    'This method optionally performs any additional functions or cleanup needed after a save
    '    PostUpdate(LinqDB, oData)
    'End Sub



#End Region


    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Public Function GetCarrierContFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DataTransferObjects.CarrierCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierCont As DataTransferObjects.CarrierCont = (
                        From d In db.CarrierConts
                        Where
                        (d.CarrierContControl = If(Control = 0, d.CarrierContControl, Control)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse d.CarrierContName = Name) _
                        And
                        (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse d.CarrierContactEMail = Email)
                        Select New DataTransferObjects.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                        .CarrierContCarrierControl = d.CarrierContCarrierControl,
                        .CarrierContName = d.CarrierContName,
                        .CarrierContTitle = d.CarrierContTitle,
                        .CarrierContactPhone = d.CarrierContactPhone,
                        .CarrierContPhoneExt = d.CarrierContPhoneExt,
                        .CarrierContactFax = d.CarrierContactFax,
                        .CarrierContact800 = d.CarrierContact800,
                        .CarrierContactEMail = d.CarrierContactEMail,
                        .CarrierContactDefault = d.CarrierContactDefault,
                        .CarrierContLECarControl = d.CarrierContLECarControl,
                        .CarrierContSchedContact = d.CarrierContSchedContact,
                        .CarrierContUpdated = d.CarrierContUpdated.ToArray()}).First

                Return CarrierCont

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

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Public Function GetCarrierContsFiltered(Optional ByVal CarrierControl As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DataTransferObjects.CarrierCont()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierConts() As DataTransferObjects.CarrierCont = (
                        From d In db.CarrierConts
                        Where
                        (d.CarrierContCarrierControl = If(CarrierControl = 0, d.CarrierContCarrierControl, CarrierControl)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse d.CarrierContName = Name) _
                        And
                        (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse d.CarrierContactEMail = Email)
                        Order By d.CarrierContName
                        Select New DataTransferObjects.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                        .CarrierContCarrierControl = d.CarrierContCarrierControl,
                        .CarrierContName = d.CarrierContName,
                        .CarrierContTitle = d.CarrierContTitle,
                        .CarrierContactPhone = d.CarrierContactPhone,
                        .CarrierContPhoneExt = d.CarrierContPhoneExt,
                        .CarrierContactFax = d.CarrierContactFax,
                        .CarrierContact800 = d.CarrierContact800,
                        .CarrierContactEMail = d.CarrierContactEMail,
                        .CarrierContactDefault = d.CarrierContactDefault,
                        .CarrierContLECarControl = d.CarrierContLECarControl,
                        .CarrierContSchedContact = d.CarrierContSchedContact,
                        .CarrierContUpdated = d.CarrierContUpdated.ToArray()}).ToArray()
                Return CarrierConts

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

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Public Function GetFirstCarrierContForCarrier(ByVal CarrierControl As Integer) As DataTransferObjects.CarrierCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierCont As DataTransferObjects.CarrierCont = (
                        From d In db.CarrierConts
                        Where
                        (d.CarrierContCarrierControl = CarrierControl)
                        Order By d.CarrierContactDefault Descending, d.CarrierContControl Ascending
                        Select New DataTransferObjects.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                        .CarrierContCarrierControl = d.CarrierContCarrierControl,
                        .CarrierContName = d.CarrierContName,
                        .CarrierContTitle = d.CarrierContTitle,
                        .CarrierContactPhone = d.CarrierContactPhone,
                        .CarrierContPhoneExt = d.CarrierContPhoneExt,
                        .CarrierContactFax = d.CarrierContactFax,
                        .CarrierContact800 = d.CarrierContact800,
                        .CarrierContactEMail = d.CarrierContactEMail,
                        .CarrierContactDefault = d.CarrierContactDefault,
                        .CarrierContLECarControl = d.CarrierContLECarControl,
                        .CarrierContSchedContact = d.CarrierContSchedContact,
                        .CarrierContUpdated = d.CarrierContUpdated.ToArray()}).FirstOrDefault()
                Return CarrierCont

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


    ''' <summary>
    ''' Returns the default carrier contact or the oldest carrier contact if a default has not been assigned
    ''' If Optional parameter CarrierContControl is valid and not 0 just get that record
    ''' If Parameters.UserLEControl != 0 then use it and the CarrierControl to get the LECarControl
    ''' and apply the LECarControl as a filter to the query
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl">Optional</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/8/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Modified by RHR for v-7.0.6.0 on 12/14/2016
    '''   added logic for backward compatibility to also return the oldest carrier contact
    '''   if the Default flag has not be set.
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added Optional parameter CarrierContControl, and logic if CarrierContControl param is not 0 just get that one unless it does not exist
    '''   Added functionality to look up the user LegalEntity and use it and the CarrierControl to get the LECarControl,
    '''   then apply the LECarControl as a filter to the query
    ''' Modified by RHR for v-8.2 on 04/29/2019
    '''  we now call GetLECarControl to look up the Legal Entity Carrier Control XRef Value
    ''' </remarks>
    Public Function GetDefaultContactForCarrier(ByVal CarrierControl As Integer, Optional ByVal CarrierContControl As Integer = 0) As DataTransferObjects.CarrierCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim CarrierCont As New DataTransferObjects.CarrierCont

                'if CarrierContControl param is not 0 just get that one unless it does not exist
                If CarrierContControl <> 0 AndAlso db.CarrierConts.Any(Function(x) x.CarrierContControl = CarrierContControl) Then
                    CarrierCont = (From d In db.CarrierConts Where d.CarrierContControl = CarrierContControl
                                   Order By d.CarrierContactDefault Descending, d.CarrierContControl Ascending
                                   Select selectDTOData(d, db)).FirstOrDefault()
                    Return CarrierCont
                End If

                'Lookup the users LEControl and CarrierControl to lookup the LECarControl and apply that as a filter to the query
                Dim leCarControl = GetLECarControl(CarrierControl, db)

                CarrierCont = (From d In db.CarrierConts
                               Where (leCarControl = 0 Or d.CarrierContLECarControl = leCarControl) And (d.CarrierContCarrierControl = CarrierControl)
                               Order By d.CarrierContactDefault Descending, d.CarrierContControl Ascending
                               Select selectDTOData(d, db)).FirstOrDefault()
                Return CarrierCont
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDefaultContactForCarrier"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Calls a stored procedure which determines whether to insert a new record or update an existing one, then carries out the desired action
    ''' </summary>
    ''' <param name="CarrierLegalEntity"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CarrierAlphaCode"></param>
    ''' <param name="CarrierContName"></param>
    ''' <param name="CarrierContTitle"></param>
    ''' <param name="CarrierContTitleAllowUpdate"></param>
    ''' <param name="CarrierContact800"></param>
    ''' <param name="CarrierContact800AllowUpdate"></param>
    ''' <param name="CarrierContactPhone"></param>
    ''' <param name="CarrierContactPhoneAllowUpdate"></param>
    ''' <param name="CarrierContPhoneExt"></param>
    ''' <param name="CarrierContPhoneExtAllowUpdate"></param>
    ''' <param name="CarrierContactFax"></param>
    ''' <param name="CarrierContactFaxAllowUpdate"></param>
    ''' <param name="CarrierContactEmail"></param>
    ''' <param name="CarrierContactEmailAllowUpdate"></param>
    ''' <remarks></remarks>
    Public Sub InsertOrUpdateCarrierContact70(ByVal CarrierLegalEntity As String,
                                              ByVal CarrierNumber As Integer,
                                              ByVal CarrierAlphaCode As String,
                                              ByVal CarrierContName As String,
                                              ByVal CarrierContTitle As String,
                                              ByVal CarrierContTitleAllowUpdate As Boolean,
                                              ByVal CarrierContact800 As String,
                                              ByVal CarrierContact800AllowUpdate As Boolean,
                                              ByVal CarrierContactPhone As String,
                                              ByVal CarrierContactPhoneAllowUpdate As Boolean,
                                              ByVal CarrierContPhoneExt As String,
                                              ByVal CarrierContPhoneExtAllowUpdate As Boolean,
                                              ByVal CarrierContactFax As String,
                                              ByVal CarrierContactFaxAllowUpdate As Boolean,
                                              ByVal CarrierContactEmail As String,
                                              ByVal CarrierContactEmailAllowUpdate As Boolean)

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oRet = db.spInsertOrUpdateCarrierContact70(CarrierLegalEntity, CarrierNumber, CarrierAlphaCode, CarrierContName, CarrierContTitle, CarrierContTitleAllowUpdate, CarrierContact800, CarrierContact800AllowUpdate, CarrierContactPhone, CarrierContactPhoneAllowUpdate, CarrierContPhoneExt, CarrierContPhoneExtAllowUpdate, CarrierContactFax, CarrierContactFaxAllowUpdate, CarrierContactEmail, CarrierContactEmailAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCarrierContact70"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' If the CarrierContactDefault is set to 1 for this record then set 
    ''' CarrierContactDefault to 0 for all other records for this CarrierContCarrierControl. 
    ''' If the CarrierContactDefault is being set to 0 nothing else is required.
    ''' </summary>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="CarrierContCarrierControl"></param>
    ''' <param name="CarrierContactDefault"></param>
    ''' <remarks>
    ''' Added By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    ''' Modified by RHR on 11/30/2018 for v-8.2 
    '''   Fixed bug so support backward compatibility if LECarControl Is zero (old way)
    '''   we set the default to zero and let the stored procedure look up the first matching value in the table
    ''' </remarks>
    Public Sub UpdateCarrierContactDefaultFlag(ByVal CarrierContControl As Integer, ByVal CarrierContCarrierControl As Integer, ByVal CarrierContactDefault As Boolean, Optional ByVal CarrierContLECarControl As Integer = 0)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                db.spUpdateCarrierContactDefaultFlag(CarrierContControl, CarrierContCarrierControl, CarrierContactDefault, CarrierContLECarControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCarrierContactDefaultFlag"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Returns all the CarrierContact records based on the FK LECarControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/04/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.0.117 on 08/30/2019
    '''   added logic to include carrier contacts where CarrierContLECarControl = 0
    ''' </remarks>
    Public Function GetCarContsByLECarrier(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.CarrierCont()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.CarrierCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iCarrierControl = db.tblLegalEntityCarriers.Where(Function(l) l.LECarControl = filters.ParentControl).Select(Function(d) d.LECarCarrierControl).FirstOrDefault()
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.CarrierCont)
                iQuery = db.CarrierConts
                Dim filterWhere = " (CarrierContLECarControl = " & filters.ParentControl.ToString() & ") Or ( CarrierContCarrierControl = " & iCarrierControl.ToString() & " And CarrierContLECarControl = 0)"
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarContsByLECarrier"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts Or Updates a CarrierCont record based on if ContactControl = 0
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added By LVV on 10/04/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub InsertOrUpdateCarrierContByLECarrier(ByVal oRecord As LTS.CarrierCont)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If oRecord.CarrierContControl = 0 Then
                    'Insert
                    db.CarrierConts.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.CarrierConts.Attach(oRecord, True)
                End If
                db.SubmitChanges()
                'only call sp it if CarrierContactDefault is 1
                If oRecord.CarrierContactDefault = True Then
                    UpdateCarrierContactDefaultFlag(oRecord.CarrierContControl, oRecord.CarrierContCarrierControl, oRecord.CarrierContactDefault, oRecord.CarrierContLECarControl)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCarrierContByLECarrier"), db)
            End Try
        End Using
    End Sub

    Public Function DeleteCarrierContact(ByVal ControlNumber As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim nObject = db.CarrierConts.Where(Function(x) x.CarrierContControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.CarrierContControl <> 0 Then
                Try
                    Try
                        'Check if the contact is being used
                        Dim dpBook As New NGLBookData(Me.Parameters)
                        If dpBook.IsCarrierContactUsed(nObject.CarrierContControl) Then
                            Dim strDet = "Cannot delete Carrier Contact data. The Carrier Contact, " & nObject.CarrierContName & ", is being used and cannot be deleted."
                            Utilities.SaveAppError("Cannot delete Carrier Contact data. The Carrier Contact, " & nObject.CarrierContName & ", is being used and cannot be deleted.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = strDet}, New FaultReason("E_DataValidationFailure"))
                        End If
                        'Check if this is the last contact record at least one is required
                        Dim c = (From d In db.CarrierConts Where d.CarrierContCarrierControl = nObject.CarrierContCarrierControl And d.CarrierContLECarControl = nObject.CarrierContLECarControl Select d.CarrierContControl).Count()
                        If c < 2 Then
                            Dim strDet = "Cannot delete last Carrier Contact record. At least one Carrier Contact is required."
                            Utilities.SaveAppError("Cannot delete last Carrier Contact record. At least one Carrier Contact is required.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = strDet}, New FaultReason("E_DataValidationFailure"))
                        End If
                    Catch ex As FaultException
                        Throw
                    Catch ex As InvalidOperationException
                        'do nothing this is the desired result.
                    End Try
                    db.CarrierConts.DeleteOnSubmit(nObject)
                    db.SubmitChanges()
                    blnRet = True
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierContact"), db)
                End Try
            End If
        End Using
        Return blnRet
    End Function

    Public Function GetCarrierContact(ByVal CarrierContControl As Integer) As LTS.CarrierCont
        If CarrierContControl = 0 Then Return Nothing
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return db.CarrierConts.Where(Function(x) x.CarrierContControl = CarrierContControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierContact"), db)
            End Try
        End Using
        Return Nothing
    End Function

    '''' <summary>
    '''' Gets the Carrier Contacts using the provided CarrierControl and the user's LegalEntityControl
    '''' </summary>
    '''' <param name="CarrierControl"></param>
    '''' <returns></returns>
    ''Public Function GetUserLECarrierContacts(ByVal CarrierControl As Integer) As Models.Contact()
    ''    Using db As New NGLMASCarrierDataContext(ConnectionString)
    ''        Try
    ''            'Lookup the users LEControl and CarrierControl to lookup the LECarControl and apply that as a filter to the query
    ''            Dim leCarControl = 0
    ''            If Parameters.UserLEControl <> 0 Then
    ''                leCarControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = Parameters.UserLEControl AndAlso x.LECarCarrierControl = CarrierControl).Select(Function(y) y.LECarControl).FirstOrDefault()
    ''            End If
    ''            'Return all the contacts that match the criteria sorted by name
    ''            Dim CarrierConts() As Models.Contact = (
    ''            From d In db.CarrierConts
    ''            Where (leCarControl <> 0 And d.CarrierContLECarControl = leCarControl)
    ''            Order By d.CarrierContName
    ''            Select New Models.Contact With {
    ''                .ContactControl = d.CarrierContControl,
    ''                .ContactCarrierControl = d.CarrierContCarrierControl,
    ''                .ContactName = d.CarrierContName,
    ''                .ContactTitle = d.CarrierContTitle,
    ''                .ContactPhone = d.CarrierContactPhone,
    ''                .ContactPhoneExt = d.CarrierContPhoneExt,
    ''                .ContactFax = d.CarrierContactFax,
    ''                .Contact800 = d.CarrierContact800,
    ''                .ContactEmail = d.CarrierContactEMail,
    ''                .ContactDefault = d.CarrierContactDefault,
    ''                .ContactLECarControl = d.CarrierContLECarControl,
    ''                .ContactScheduler = d.CarrierContSchedContact
    ''                }).ToArray()
    ''            Return CarrierConts
    ''        Catch ex As Exception
    ''            ManageLinqDataExceptions(ex, buildProcedureName("GetUserLECarrierContacts"), db)
    ''        End Try
    ''        Return Nothing
    ''    End Using
    ''End Function

    ''' <summary>
    ''' Gets the Carrier Contacts using the provided CarrierControl and the user's LegalEntityControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 04/29/2019 
    '''  we now call GetLECarControl to look up the Legal Entity Carrier Control XRef Value
    '''   Modified by RHR for v-8.2.0.117 on 08/30/2019
    '''   added logic to include carrier contacts where CarrierContLECarControl = 0
    ''' </remarks>
    Public Function GetUserLECarrierContacts(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Models.Contact()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As Models.Contact
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim leCarControl = GetLECarControl(filters.CarrierControlFrom, db)
                Dim iCarrierControl = filters.CarrierControlFrom
                Dim iQuery As IQueryable(Of LTS.CarrierCont)
                'Get the data iqueryable
                iQuery = db.CarrierConts.Where(Function(x) x.CarrierContCarrierControl = iCarrierControl And (x.CarrierContLECarControl = 0 Or (leCarControl <> 0 And x.CarrierContLECarControl = leCarControl)))
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                Dim oContacts = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                If Not oContacts Is Nothing AndAlso oContacts.Count() > 0 Then
                    'convert to Models.Contact data
                    oRet = (From d In oContacts Order By d.CarrierContName
                            Select New Models.Contact With {
                                .ContactControl = d.CarrierContControl,
                                .ContactCarrierControl = d.CarrierContCarrierControl,
                                .ContactName = d.CarrierContName,
                                .ContactTitle = d.CarrierContTitle,
                                .ContactPhone = d.CarrierContactPhone,
                                .ContactPhoneExt = d.CarrierContPhoneExt,
                                .ContactFax = d.CarrierContactFax,
                                .Contact800 = d.CarrierContact800,
                                .ContactEmail = d.CarrierContactEMail,
                                .ContactDefault = d.CarrierContactDefault,
                                .ContactLECarControl = d.CarrierContLECarControl,
                                .ContactScheduler = d.CarrierContSchedContact}).ToArray()
                End If
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserLECarrierContacts"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the Carrier Contacts using the provided CarrierControl and PreferredCarrier Control,LaneControl
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks> 
    '''   added logic to include carrier contacts 
    ''' </remarks>                                
    Public Function GetUserLEPreferredCarrierContacts(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Models.Contact()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As Models.Contact
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim results As New List(Of LTS.vLELaneActiveCarrier)
                Dim oContacts = (From d In db.spPreferredActiveContactCarriers(filters.CarrierControlFrom, filters.PrimaryKey, filters.LaneControl)).ToArray() 'CarrierControl,Preferred carrier pk,LaneControl'

                If oContacts Is Nothing OrElse oContacts.Count < 1 Then Return Nothing

                If Not oContacts Is Nothing AndAlso oContacts.Count() > 0 Then
                    'convert to Models.Contact data
                    oRet = (From d In oContacts Order By d.CarrierContName
                            Select New Models.Contact With {
                                .ContactControl = d.CarrierContControl,
                                .ContactCarrierControl = d.CarrierContCarrierControl,
                                .ContactName = d.CarrierContName,
                                .ContactTitle = d.CarrierContTitle,
                                .ContactPhone = d.CarrierContactPhone,
                                .ContactPhoneExt = d.CarrierContPhoneExt,
                                .ContactFax = d.CarrierContactFax,
                                .Contact800 = d.CarrierContact800,
                                .ContactEmail = d.CarrierContactEMail,
                                .ContactDefault = d.CarrierContactDefault,
                                .ContactLECarControl = d.CarrierContLECarControl,
                                .ContactScheduler = d.CarrierContSchedContact}).ToArray()
                End If
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetUserLEPreferredCarrierContacts"), db)
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#Region "Protected Functions"

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierCont)
        'Create New Record
        Return New LTS.CarrierCont With {.CarrierContControl = d.CarrierContControl,
            .CarrierContCarrierControl = d.CarrierContCarrierControl,
            .CarrierContName = d.CarrierContName,
            .CarrierContTitle = d.CarrierContTitle,
            .CarrierContactPhone = d.CarrierContactPhone,
            .CarrierContPhoneExt = d.CarrierContPhoneExt,
            .CarrierContactFax = d.CarrierContactFax,
            .CarrierContact800 = d.CarrierContact800,
            .CarrierContactEMail = d.CarrierContactEMail,
            .CarrierContactDefault = d.CarrierContactDefault,
            .CarrierContLECarControl = d.CarrierContLECarControl,
            .CarrierContSchedContact = d.CarrierContSchedContact,
            .CarrierContUpdated = If(d.CarrierContUpdated Is Nothing, New Byte() {}, d.CarrierContUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierContFiltered(Control:=CType(LinqTable, LTS.CarrierCont).CarrierContControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierCont = TryCast(LinqTable, LTS.CarrierCont)
                If source Is Nothing Then Return Nothing
                'Note thise data source table does not have a mod date or mod user field
                ret = (From d In db.CarrierConts
                       Where d.CarrierContControl = source.CarrierContControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierContControl _
                           , .ModDate = Date.Now _
                           , .ModUser = Parameters.UserName _
                           , .Updated = d.CarrierContUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Confirm that a delete is allowed
        'Using db As New NGLMASCarrierDataContext(ConnectionString)
        With CType(oData, DataTransferObjects.CarrierCont)
            Try
                'Check if the contact is being used
                Dim dpBook As New NGLBookData(Me.Parameters)
                If dpBook.IsCarrierContactUsed(.CarrierContControl) Then
                    Utilities.SaveAppError("Cannot delete Carrier Contact data. The Carrier Contact, " & .CarrierContName & ", is being used and cannot be deleted.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
                'Check if this is the last contact record at lease one is required
                Dim c = (From d In CType(oDB, NGLMASCarrierDataContext).CarrierConts Where d.CarrierContCarrierControl = .CarrierContCarrierControl And d.CarrierContLECarControl = .CarrierContLECarControl Select d.CarrierContControl).Count()
                If c < 2 Then
                    Utilities.SaveAppError("Cannot delete last Carrier Contact record. At least one Carrier Contact is required.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
        'End Using
    End Sub

    ''' <summary>
    ''' run update validation code
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 04/29/2019
    '''  we now call GetLECarControl to look up the Legal Entity Carrier Control XRef Value
    '''  added logic to assign a default CarrierContLECarControl
    ''' </remarks>
    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Confirm that a delete is allowed
        'Using db As New NGLMASCarrierDataContext(ConnectionString)
        With CType(oData, DataTransferObjects.CarrierCont)
            Try
                If .CarrierContLECarControl = 0 Then
                    .CarrierContLECarControl = GetLECarControl(.CarrierContCarrierControl, oDB)
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
        'End Using
    End Sub

    ''' <summary>
    ''' run add new validation code
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>    ''' 
    ''' Modified by RHR for v-8.2 on 04/29/2019
    '''  we now call GetLECarControl to look up the Legal Entity Carrier Control XRef Value
    '''  added logic to assign a default CarrierContLECarControl
    '''  </remarks>
    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ' Using db As New NGLMASCarrierDataContext(ConnectionString)
        With CType(oData, DataTransferObjects.CarrierCont)
            Try
                If .CarrierContLECarControl = 0 Then
                    .CarrierContLECarControl = GetLECarControl(.CarrierContCarrierControl, oDB)
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
        'End Using
    End Sub


    'Added By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Protected Overrides Sub PostUpdate(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Confirm that a delete is allowed
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            With CType(oData, DataTransferObjects.CarrierCont)
                Try
                    'only call sp it if CarrierContactDefault is 1
                    If .CarrierContactDefault = True Then
                        UpdateCarrierContactDefaultFlag(.CarrierContControl, .CarrierContCarrierControl, .CarrierContactDefault, .CarrierContLECarControl)
                    End If

                Catch ex As FaultException
                    Throw
                Catch ex As InvalidOperationException
                    'do nothing this is the desired result.
                End Try
            End With

        End Using
    End Sub

    'Added By LVV on 2/19/2019
    Friend Function selectDTOData(ByVal d As LTS.CarrierCont, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierCont
        Return New DataTransferObjects.CarrierCont With {
            .CarrierContControl = d.CarrierContControl,
            .CarrierContCarrierControl = d.CarrierContCarrierControl,
            .CarrierContName = d.CarrierContName,
            .CarrierContTitle = d.CarrierContTitle,
            .CarrierContactPhone = d.CarrierContactPhone,
            .CarrierContPhoneExt = d.CarrierContPhoneExt,
            .CarrierContactFax = d.CarrierContactFax,
            .CarrierContact800 = d.CarrierContact800,
            .CarrierContactEMail = d.CarrierContactEMail,
            .CarrierContactDefault = d.CarrierContactDefault,
            .CarrierContLECarControl = d.CarrierContLECarControl,
            .CarrierContSchedContact = d.CarrierContSchedContact,
            .CarrierContUpdated = d.CarrierContUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize
            }
    End Function


    ''' <summary>
    ''' Return the LECarCarrier Control number for the provided carrier and the current user
    ''' </summary>
    ''' <param name="iCarrierControl"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 04/29/2019
    '''  used to look up the LE Carrier Cross Reference for the current user
    '''  typically used for carrier contact information
    ''' </remarks>
    Protected Function GetLECarControl(ByVal iCarrierControl As Integer, ByRef db As NGLMASCarrierDataContext) As Integer
        Dim iLECarControl As Integer = 0
        If iCarrierControl <> 0 Then
            If Parameters.UserLEControl <> 0 Then
                iLECarControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarLEAdminControl = Parameters.UserLEControl AndAlso x.LECarCarrierControl = iCarrierControl).Select(Function(y) y.LECarControl).FirstOrDefault()
            Else
                Dim oRet = db.spGetLECarControl(iCarrierControl, Parameters.UserName).FirstOrDefault()
                If Not oRet Is Nothing AndAlso oRet.LECarControl.HasValue Then
                    iLECarControl = oRet.LECarControl
                End If
            End If
        End If
        Return iLECarControl
    End Function

    Protected Function GetLEPreferredControl(ByVal iLLTCControl As Integer, ByRef db As NGLMASLaneDataContext) As Integer
        Dim iLECarControl As Integer = 0
        If iLLTCControl <> 0 Then
            If Parameters.UserLEControl <> 0 Then
                iLECarControl = db.LimitLaneToCarriers.Where(Function(x) x.LLTCControl = iLLTCControl).Select(Function(y) y.LLTCControl).FirstOrDefault()

            End If
        End If
        Return iLECarControl
    End Function
#End Region

End Class