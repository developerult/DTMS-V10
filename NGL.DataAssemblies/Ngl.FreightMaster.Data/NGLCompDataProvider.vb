Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.Linq.Dynamic
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Windows.Forms

Public Class NGLCompData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.Comps
        Me.LinqDB = db
        Me.SourceClass = "NGLCompData"
    End Sub

#End Region


#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.Comps
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property


#End Region

#Region "Public Methods"


    Public Function GetComp(ByVal Control As Integer) As LTS.Comp
        If Control = 0 Then Return Nothing
        Dim oRet As New LTS.Comp
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                oRet = db.Comps.Where(Function(x) x.CompControl = Control).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetComp"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function GetComps(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.Comp()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.Comp

        Dim iQuery As IQueryable(Of LTS.Comp) = Nothing
        Dim filterWhere = ""
        If (Not String.IsNullOrWhiteSpace(filters.filterName)) AndAlso (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
            filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
        End If
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                iQuery = db.Comps
                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                End If
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetComps"), db)
            End Try

            Return Nothing

        End Using
        Return oRet
    End Function

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCompFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompsFiltered()
    End Function

    ''' <summary>
    ''' Method to Get the Company Dashboard data
    ''' </summary>
    ''' <remarks>
    ''' Added By CHA On 7-June-2021
    ''' </remarks>
    ''' <returns>LTS Object for vCompDashboard</returns>
    Public Function GetCompDashboard() As LTS.vCompDashboard()
        Dim oRet As New List(Of LTS.vCompDashboard)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iCarrierControl = Me.Parameters.UserCarrierControl
                Dim iQuery = db.vCompDashboards
                If Me.Parameters.IsUserCarrier Then

                    oRet = iQuery.Where(Function(x) x.CarrierControl = iCarrierControl).ToList()
                    'oRet = oRet.wh
                    ''db.vCompDashboards.Where(Function(x) x.CarrierControl = Me.Parameters.UserCarrierControl).ToArray()
                Else
                    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                    oRet = iQuery.Where(Function(x) oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(x.CompControl)).ToList()
                End If

                'Dim iQuery As IQueryable(Of LTS.vCompDashboard)
                'iQuery = db.vCompDashboard
                'oRet = iQuery.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompDashboard"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetCompDashboard(ByVal blnInbound As Boolean) As LTS.vCompDashboard()
        Dim oRet As New List(Of LTS.vCompDashboard)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iCarrierControl = Me.Parameters.UserCarrierControl
                Dim iQuery = db.vCompDashboards
                If Me.Parameters.IsUserCarrier Then

                    oRet = iQuery.Where(Function(x) x.CarrierControl = iCarrierControl And x.Inbound = blnInbound).ToList()
                    'oRet = oRet.wh
                    ''db.vCompDashboards.Where(Function(x) x.CarrierControl = Me.Parameters.UserCarrierControl).ToArray()
                Else
                    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                    oRet = iQuery.Where(Function(x) oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(x.CompControl) And x.Inbound = blnInbound).ToList()
                End If

                'Dim iQuery As IQueryable(Of LTS.vCompDashboard)
                'iQuery = db.vCompDashboard
                'oRet = iQuery.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompDashboard"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function


    Public Function GetPlanningDashboard(ByVal blnInbound As Boolean) As LTS.vPlanningDashboard()
        Dim oRet As New List(Of LTS.vPlanningDashboard)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iCarrierControl = Me.Parameters.UserCarrierControl
                Dim iQuery = db.vPlanningDashboards
                If Me.Parameters.IsUserCarrier Then
                    oRet = iQuery.Where(Function(x) x.CarrierControl = iCarrierControl And x.Inbound = blnInbound And x.BookTranCode = "PC").ToList()
                Else
                    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                    oRet = iQuery.Where(Function(x) oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(x.CompControl) And x.Inbound = blnInbound).ToList()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPlanningDashboard"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetPlanningDashboard() As LTS.vPlanningDashboard()
        Dim oRet As New List(Of LTS.vPlanningDashboard)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iCarrierControl = Me.Parameters.UserCarrierControl
                Dim iQuery = db.vPlanningDashboards
                If Me.Parameters.IsUserCarrier Then
                    oRet = iQuery.Where(Function(x) x.CarrierControl = iCarrierControl And x.BookTranCode = "PC").ToList()
                Else
                    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                    oRet = iQuery.Where(Function(x) oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(x.CompControl)).ToList()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPlanningDashboard"), db)
            End Try
        End Using
        Return oRet.ToArray()
    End Function

    Public Function GetLoadsDeliveredDashboard(ByVal blnInbound As Boolean) As List(Of Models.LoadsDeliveredSummary)
        Dim lLoadsDelivered As New List(Of LTS.vLoadsDeliveredDashboard)
        Dim oRet = New List(Of Models.LoadsDeliveredSummary)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iCarrierControl = Me.Parameters.UserCarrierControl
                Dim iQuery = db.vLoadsDeliveredDashboards
                'LaneOriginAddressUse is true on inbound.
                If Me.Parameters.IsUserCarrier Then
                    lLoadsDelivered = iQuery.Where(Function(x) x.CarrierControl = iCarrierControl And x.LaneOriginAddressUse = blnInbound).ToList()
                Else
                    Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                    lLoadsDelivered = iQuery.Where(Function(x) oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(x.CompControl) And x.LaneOriginAddressUse = blnInbound).ToList()
                End If
                oRet = lLoadsDelivered.GroupBy(Function(x) New With {Key x.iYear, Key x.iMonth}).Select(Function(grp) New Models.LoadsDeliveredSummary With {.iMonth = grp.Key.iMonth, .Loads = grp.Count()}).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadsDeliveredDashboard"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function GetCompsFiltered() As DTO.Comp()

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Comp() As DTO.Comp = (
                    From d In db.Comps
                    Order By d.CompNumber
                    Select selectDTOData(d, db)).ToArray()
                Return Comp

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
    ''' Returns a reference to an existing company record if one exists or a new dto.comp data object or nothing on error.
    ''' If CompNumber, CompLegalEntity, and  CompAlphaCode validation is performed to avoid duplicates.
    ''' Throws SqlFaultInfo with a message value of E_InvalidKeyFilterMetaData if a duplicate is found and  
    ''' validation fails
    ''' Throws sqlFaultInfo with a message value of E_InvalidRecordKeyField if a required key value is missing
    ''' </summary>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 2/13/14 updated logic for validating compnumber
    ''' </remarks>
    Public Function GetCompFilteredByLegalEntity(ByVal CompLegalEntity As String, ByVal CompAlphaCode As String, ByVal CompNumber As Integer) As DTO.Comp

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Rules:
                '1. test compnumber first if <> 0 check for an existing record
                '   a.  if Legal Entity or Alpha code are provided they both must match
                '       so if a record exists for compnumber where the Legal Entity or Alpha Codes
                '       do not match the validation fails
                '2. If compnumber is zero we just lookup using legal entity and alpha code
                '   if a match is not found we can return a new dto.comp object or nothing
                If CompNumber = 0 Then
                    'check if a record exists for the alpha code and legal entity
                    'Unit test set compnumber = 0 pass alpha code and legal entity
                    '   1. If CompAlphaCode or CompLegalEntity are empty an exception is thrown
                    '   2. if a record exists the correct comp record shouild be returned
                    '   3. if a match is not found a blank comp record should be returned
                    If String.IsNullOrWhiteSpace(CompLegalEntity) Then
                        throwFieldRequiredException("CompLegalEntity")
                    End If
                    If String.IsNullOrWhiteSpace(CompAlphaCode) Then
                        throwFieldRequiredException("CompAlphaCode")
                    End If

                    Dim Comp As DTO.Comp = (
                        From d In db.Comps
                        Where d.CompAlphaCode.ToLower = CompAlphaCode.ToLower _
                        And d.CompLegalEntity.ToLower = CompLegalEntity.ToLower
                        Order By d.CompNumber
                        Select selectDTOData(d, db)).FirstOrDefault()
                    Return Comp
                Else
                    'we need to test if the the alpha code and legal entity are provided and are already being used by another company
                    'number.  this is not allowed because the CompNumber, Alpha Code, and Legal Entity are a unique key.
                    If (Not String.IsNullOrWhiteSpace(CompLegalEntity)) AndAlso (Not String.IsNullOrWhiteSpace(CompAlphaCode)) Then
                        If db.Comps.Any(Function(x) x.CompLegalEntity = CompLegalEntity And x.CompAlphaCode = CompAlphaCode And x.CompNumber <> CompNumber) Then
                            Dim largs As New List(Of String) From {"Comp", "CompNumber", CompNumber, CompAlphaCode}
                            throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveRequiredValueDoesNotMatch, largs)
                        End If
                    End If
                    'Bug Fixed by RHR 2/18/14  the select Comp method must be called each time it should not be 
                    'included in an else statement Else and End if were modified
                    'Else
                    'Ok to save so see if we are inserting or updating the database
                    Dim Comp As DTO.Comp = (
                       From d In db.Comps
                       Where d.CompNumber = CompNumber
                       Select selectDTOData(d, db)).FirstOrDefault()
                    Return Comp
                    'End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function SaveComp(ByVal oData As LTS.Comp) As LTS.Comp
        Dim iControl As Integer = oData.CompControl
        If iControl = 0 Then
            throwInvalidRequiredKeysException("Company", "Invalid company, a control nunber is required and cannot be zero")
        End If
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'update
                db.Comps.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveComp"))
            End Try
        End Using

        Return oData
    End Function


    Public Function DeleteComp(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then
            throwInvalidRequiredKeysException("Comp", "Invalid Comp, a control nunber is required and cannot be zero")
        End If
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If db.Comps.Any(Function(x) x.CompControl = iControl) Then
                    Dim oComp As LTS.Comp = db.Comps.Where(Function(x) x.CompControl = iControl).FirstOrDefault()
                    If Not oComp Is Nothing AndAlso oComp.CompControl <> 0 Then
                        db.Comps.DeleteOnSubmit(oComp)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = True 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteComp"), db)
            End Try
        End Using

        Return blnRet
    End Function




    Public Function GetCompsFilteredByLegalEntity(ByVal CompLegalEntity As String) As DTO.Comp()

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Comp() As DTO.Comp = (
                    From d In db.Comps
                    Where d.CompLegalEntity = CompLegalEntity
                    Order By d.CompNumber
                    Select selectDTOData(d, db)).ToArray()
                Return Comp
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompsFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function DoesRecordExist(ByVal CompControl As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim blnExists = db.Comps.Any(Function(x) x.CompControl = CompControl)
                Return blnExists
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesRecordExist"))
            End Try
        End Using
    End Function


    ''' <summary>
    ''' Returns a spGetCompNumberOnImportResult object that contains the matching compnumber and compcontrol 
    ''' if found or zeros if a match does not exist
    ''' </summary>
    ''' <param name="CustNumber"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCompNumberOnImport(ByVal CustNumber As String, ByVal CompLegalEntity As String, ByVal CompAlphaCode As String) As LTS.spGetCompNumberOnImportResult

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return db.spGetCompNumberOnImport(CustNumber, CompLegalEntity, CompAlphaCode).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompNumberOnImport"))
            End Try
            Return Nothing
        End Using

    End Function

    ''' <summary>
    ''' Deletes a Company record by control number without validation. It can only be called by system only process.
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim nObject = db.Comps.Where(Function(x) x.CompControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.CompControl <> 0 Then
                db.Comps.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
                End Try
            End If
        End Using
    End Sub

    Public Function GetCompNumber(ByVal CompControl As Integer) As Integer

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim nCompNo As Integer = (From c In db.Comps
                                          Where c.CompControl = CompControl
                                          Select c.CompNumber).FirstOrDefault
                Return nCompNo
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompNumber"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetNextCompNumber() As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Try
                    intRet = db.Comps.Max(Function(x) x.CompNumber)
                Catch ex As Exception
                    Return 1
                End Try
                ''if there are no null values to deal with we can build the array directly
                'intRet = ( _
                'From t In db.Comps Order By t.CompNumber Descending _
                '    Select t.CompNumber).FirstOrDefault()
                intRet += 1
                Return intRet

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'No Data so just return 1
                Return 1
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return intRet

        End Using

    End Function

    Public Function GetCompVLookup(ByVal compontrol As Integer) As DTO.vLookupList

        Try
            Using db As New NGLMASCompDataContext(ConnectionString)
                Dim result As DTO.vLookupList = (From t In db.Comps
                                                 Where t.CompControl = If(compontrol = 0, t.CompControl, compontrol)
                                                 Select New DTO.vLookupList _
                                                With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber}).FirstOrDefault
                Return result
            End Using


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
    End Function

    Public Function GetCompFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0, Optional ByVal Name As String = "") As DTO.Comp

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Comp)(Function(t As LTS.Comp) t.CompConts)
                db.LoadOptions = oDLO
                'if there are no null values to deal with we can build the array directly
                Dim Comp As DTO.Comp = (
                From t In db.Comps
                Where
                    (t.CompControl = If(Control = 0, t.CompControl, Control)) _
                    And
                    (t.CompNumber = If(Number = 0, t.CompNumber, Number)) _
                    And
                    (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.CompName = Name)
                Select selectDTOData(t, db, True)).Single
                Return Comp

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
    ''' Returns a company record if a match is found for all parameter except that the CompAlphaCode in the db can be blank
    ''' If the CompAlphaCode is blank we update the CompAlphaCode with the provided value.
    ''' NOTE:  this overload is created for v-7.x or later older versions must implement their own behavior.
    ''' Additionally we check the AlphacompanyXref table for backwards compatibility.
    ''' </summary>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompAbrev"></param>
    ''' <param name="CompStreetAddress1"></param>
    ''' <param name="CompStreetCity"></param>
    ''' <param name="CompStreetState"></param>
    ''' <param name="CompStreetZip"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 5/19/2017
    ''' </remarks>
    Public Function GetCompFilteredByAddress(ByVal CompAlphaCode As String,
                                             ByVal CompAbrev As String,
                                             ByVal CompStreetAddress1 As String,
                                             ByVal CompStreetCity As String,
                                             ByVal CompStreetState As String,
                                             ByVal CompStreetZip As String,
                                             ByVal CompLegalEntity As String) As DTO.Comp

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                'db.Log = New DebugTextWriter
                Dim Comp As DTO.Comp = (
                From t In db.Comps
                Where
                    ((t.CompAlphaCode Is Nothing OrElse t.CompAlphaCode.Trim.Length = 0) OrElse t.CompAlphaCode = CompAlphaCode) _
                    And
                    (t.CompAbrev = CompAbrev) _
                    And
                    (t.CompStreetAddress1 = CompStreetAddress1) _
                    And
                    (t.CompStreetState = CompStreetState) _
                    And
                    (t.CompStreetZip = CompStreetZip) Select selectDTOData(t, db, False)).FirstOrDefault()

                If Not Comp Is Nothing AndAlso Comp.CompControl <> 0 Then
                    'we have a match so update the Alpha Code and Legal Entity if they do not match
                    Dim blnChanged As Boolean = False
                    If Comp.CompAlphaCode <> CompAlphaCode Then
                        Comp.CompAlphaCode = CompAlphaCode
                        blnChanged = True
                    End If
                    If Comp.CompLegalEntity <> CompLegalEntity Then
                        Comp.CompLegalEntity = CompLegalEntity
                        blnChanged = True
                    End If
                    If blnChanged Then
                        Comp.CompModDate = Date.Now()
                        Comp.CompModUser = Parameters.UserName
                        Dim oAlphaCompXref As New LTS.AlphaCompanyXrefRefComp()
                        If db.AlphaCompanyXrefRefComps.Any(Function(x) x.ACXCompNumber = Comp.CompNumber) Then
                            oAlphaCompXref = db.AlphaCompanyXrefRefComps.Where(Function(x) x.ACXCompNumber = Comp.CompNumber).FirstOrDefault()
                            If oAlphaCompXref.ACXAlphaNumber <> Comp.CompAlphaCode Then
                                oAlphaCompXref.ACXAlphaNumber = Comp.CompAlphaCode
                            End If
                        Else
                            oAlphaCompXref.ACXCompNumber = Comp.CompNumber
                            oAlphaCompXref.ACXAlphaNumber = Comp.CompAlphaCode
                            db.AlphaCompanyXrefRefComps.InsertOnSubmit(oAlphaCompXref)
                        End If
                        db.SubmitChanges()
                    End If

                End If
                Return Comp

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompFilteredByAddress"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetvComps() As DTO.vComp()

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions

                Dim vComp As DTO.vComp() = (
                From t In db.vComps
                Order By t.CompName Ascending
                Select New DTO.vComp With {.CompControl = t.CompControl _
                                              , .CompNumber = If(t.CompNumber.HasValue, t.CompNumber.Value, 0) _
                                              , .CompName = t.CompName _
                                              , .CompAbrev = t.CompAbrev}).ToArray()
                Return vComp

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

    Public Function GetvCompsByCompControls(ByVal compIDList As String) As DTO.vComp()

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim arr() As String = compIDList.Split(",")
                Dim oDLO As New DataLoadOptions

                Dim vComp As DTO.vComp() = (
                From t In db.vComps
                Where arr.Contains(t.CompControl)
                Order By t.CompName Ascending
                Select New DTO.vComp With {.CompControl = t.CompControl _
                                              , .CompNumber = If(t.CompNumber.HasValue, t.CompNumber.Value, 0) _
                                              , .CompName = t.CompName _
                                              , .CompAbrev = t.CompAbrev}).ToArray()
                Return vComp

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

    Public Function GetCompValidateCredit(ByVal Control As Integer, ByVal BFC As Double) As DTO.CompValidateCredit
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompValidateCredit As DTO.CompValidateCredit = (
                From t In db.udfValidateCredit(Control, BFC)
                Select New DTO.CompValidateCredit With {.CompFinCreditAvail = If(t.CompFinCreditAvail.HasValue, t.CompFinCreditAvail.Value, 0) _
                                            , .Details = t.Details _
                                            , .Message = t.Message}).First
                Return CompValidateCredit

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

    Public Function GetCompControlByAlpha(ByVal compNumber As String) As Integer
        Dim intCompControl As Integer
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get compcontrol number
                Dim Control = db.udfGetCompControlByAlpha(compNumber)
                If Control.HasValue Then
                    intCompControl = Control.Value
                End If
                Return intCompControl

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompControlByAlpha"))
            End Try


            Return intCompControl

        End Using
    End Function

    Public Function GetCompNumberByAlpha(ByVal CompAlpha As String) As Integer
        Dim intCompNumber As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get compcontrol number
                Dim sNunmber = db.udfGetCompNumberByAlpha(CompAlpha)
                Integer.TryParse(sNunmber, intCompNumber)
                Return intCompNumber

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompNumberByAlpha"))
            End Try

            Return intCompNumber

        End Using
    End Function

    ''' <summary>
    ''' Reads the first CompAbrev from the Comp table that matches, first by CompNumber then by CompAlphaCode.
    ''' CompNumber values of zero are ignored and only the CompAlphaCode is used.
    ''' If both value fail to return a result the optional sDefault parameter value is returned
    ''' </summary>
    ''' <param name="compNumber"></param>
    ''' <param name="compAlphaCode"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' follows standard design patterns and throws SQLFaultExceptions on error
    ''' Created 1/09/2016 by RHR v-7.0.5 
    ''' </remarks>
    Public Function GetCompAbrevByNumberOrAlpha(ByVal compNumber As Integer, ByVal compAlphaCode As String, Optional sDefault As String = "") As String
        Using db As New NGLMASCompDataContext(ConnectionString)

            Dim strCompAbrev As String = sDefault
            Try
                If compNumber <> 0 Then
                    strCompAbrev = db.Comps.Where(Function(x) x.CompNumber = compNumber).Select(Function(x) x.CompAbrev).FirstOrDefault()
                Else
                    strCompAbrev = db.Comps.Where(Function(x) x.CompAlphaCode = compAlphaCode).Select(Function(x) x.CompAbrev).FirstOrDefault()
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, "GetCompAbrevByNumberOrAlpha", db)
            End Try

            Return strCompAbrev

        End Using
    End Function


    ''' <summary>
    ''' Validates the company name and abbreviation then checks if a matching company recored exists based on the parameters provided; 
    ''' also gets the next company nunmber if the CompNumber parameter is zero;  if the validation fails the function returns 
    ''' false and updates the ValidationMsg with details about the failure.
    ''' </summary>
    ''' <param name="CompNumber"></param>
    ''' <param name="CompName"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompAbrev"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCompBeforeInsert(ByRef CompNumber As Integer, ByVal CompName As String, ByVal CompLegalEntity As String, ByVal CompAlphaCode As String, ByVal CompAbrev As String, ByRef ValidationMsg As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Validate that required fields have been entered
                If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
                Dim strSpacer As String = ""
                If String.IsNullOrEmpty(CompName) OrElse CompName.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company Name is not valid"
                    strSpacer = ", and "
                End If

                If String.IsNullOrEmpty(CompAbrev) OrElse CompAbrev.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company Abbreviation is not valid"
                    strSpacer = ", and "
                End If

                Dim blnExists As Boolean = False
                If blnRet Then
                    If CompNumber = 0 Then
                        If Not String.IsNullOrEmpty(CompAlphaCode) AndAlso CompAlphaCode.Trim.Length > 0 Then
                            If Not String.IsNullOrEmpty(CompLegalEntity) AndAlso CompLegalEntity.Trim.Length > 0 Then
                                'it is possible for the CompAlphaCode to duplciate across Legal Entities
                                blnExists = db.Comps.Any(Function(x) x.CompName = CompName And x.CompAlphaCode = CompAlphaCode And x.CompLegalEntity = CompLegalEntity)
                            Else
                                blnExists = db.Comps.Any(Function(x) x.CompName = CompName And x.CompAlphaCode = CompAlphaCode)
                            End If
                        Else
                            blnExists = db.Comps.Any(Function(x) x.CompName = CompName)
                        End If
                        If Not blnExists Then
                            'get the new company number
                            CompNumber = GetNextCompNumber()
                            If CompNumber = 0 Then
                                blnRet = False
                                ValidationMsg &= strSpacer & "the Company Number is not valid"
                                strSpacer = ", and "
                            End If
                        End If
                    Else
                        Dim intCompNumberFilter As Integer = CompNumber 'we cannot use a byRef argument in a lambda expression.
                        If Not String.IsNullOrEmpty(CompAlphaCode) AndAlso CompAlphaCode.Trim.Length > 0 Then
                            If Not String.IsNullOrEmpty(CompLegalEntity) AndAlso CompLegalEntity.Trim.Length > 0 Then
                                'it is possible for the CompAlphaCode to duplciate across Legal Entities
                                blnExists = db.Comps.Any(Function(x) x.CompName = CompName And x.CompNumber = intCompNumberFilter And x.CompAlphaCode = CompAlphaCode And x.CompLegalEntity = CompLegalEntity)
                            Else
                                blnExists = db.Comps.Any(Function(x) x.CompName = CompName And x.CompNumber = intCompNumberFilter And x.CompAlphaCode = CompAlphaCode)
                            End If
                        Else
                            blnExists = db.Comps.Any(Function(x) x.CompName = CompName And x.CompNumber = intCompNumberFilter)
                        End If
                    End If
                End If

                If blnExists Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company record already exists"
                    strSpacer = ", and "
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCompBeforeInsert"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Validates the company name, abbreviation and company number then checks if a matching company recored exists for a company other than the current compcontrol company
    ''' based on the parameters provided; if the validation fails the function returns 
    ''' false and updates the ValidationMsg with details about the failure.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="CompName"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompAbrev"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCompBeforeUpdate(ByVal CompControl As Integer, ByVal CompNumber As Integer, ByVal CompName As String, ByVal CompLegalEntity As String, ByVal CompAlphaCode As String, ByVal CompAbrev As String, ByRef ValidationMsg As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Validate that required fields have been entered
                If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
                Dim strSpacer As String = ""
                If String.IsNullOrEmpty(CompName) OrElse CompName.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company Name is not valid"
                    strSpacer = ", and "
                End If

                If String.IsNullOrEmpty(CompAbrev) OrElse CompAbrev.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company Abbreviation is not valid"
                    strSpacer = ", and "
                End If

                If CompNumber = 0 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company Number is not valid"
                    strSpacer = ", and "
                End If

                Dim blnExists As Boolean = False
                If blnRet Then
                    If Not String.IsNullOrEmpty(CompAlphaCode) AndAlso CompAlphaCode.Trim.Length > 0 Then
                        If Not String.IsNullOrEmpty(CompLegalEntity) AndAlso CompLegalEntity.Trim.Length > 0 Then
                            'it is possible for the CompAlphaCode to duplciate across Legal Entities
                            blnExists = db.Comps.Any(Function(x) x.CompControl <> CompControl And x.CompName = CompName And x.CompNumber = CompNumber And x.CompAlphaCode = CompAlphaCode And x.CompLegalEntity = CompLegalEntity)
                        Else
                            blnExists = db.Comps.Any(Function(x) x.CompControl <> CompControl And x.CompName = CompName And x.CompNumber = CompNumber And x.CompAlphaCode = CompAlphaCode)
                        End If
                    Else
                        blnExists = db.Comps.Any(Function(x) x.CompControl <> CompControl And x.CompName = CompName And x.CompNumber = CompNumber)
                    End If
                End If

                If blnExists Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Company record already exists"
                    strSpacer = ", and "
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCompBeforeUpdate"))
            End Try
        End Using
        Return blnRet
    End Function

    Public Function InsertComp(ByVal oData As LTS.Comp) As DTO.Comp
        Using db As New NGLMASCompDataContext(ConnectionString)

            db.Comps.InsertOnSubmit(oData)
            Try
                db.SubmitChanges()

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

            Return GetCompFiltered(oData.CompControl)

        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LinqTable"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.001 on 06/13/2022 updates the user company security for 
    ''' </remarks>
    Protected Overrides Sub CreateCleanUp(ByVal LinqTable As Object)
        Try
            Dim LegalEntity As String = DirectCast(LinqTable, LTS.Comp).CompLegalEntity
            Dim CompNumber As Integer = DirectCast(LinqTable, LTS.Comp).CompNumber
            Dim blnIsUpdate As Boolean = False
            AddVisibleCompForLEUsers(LegalEntity, CompNumber, blnIsUpdate)
        Catch ex As Exception
            ' Do Nothing
        End Try
    End Sub


#Region "TMS365 Methods"

    ''' <summary>
    ''' If the provided LEControl exists in the LEAdmin table use it to look up the CompAbrev. 
    ''' Otherwise, try to use UserControl.
    ''' </summary>
    ''' <param name="LEControl"></param>
    ''' <param name="UserControl"></param>
    ''' <param name="sDefault"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV for v-8.0 on 8/15/17 TMS 365 
    ''' </remarks>
    Public Function GetCompAbrevByUserOrLE(ByVal LEControl As Integer, ByVal UserControl As Integer, Optional sDefault As String = "") As String
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim strCompAbrev = sDefault
            Try
                Dim res = (From d In db.spGetCompAbrevByUserOrLE(LEControl, UserControl) Select d).FirstOrDefault()
                strCompAbrev = res.CompAbrev
            Catch ex As Exception
                ManageLinqDataExceptions(ex, "GetCompAbrevByUserOrLE", db)
            End Try
            Return strCompAbrev
        End Using
    End Function

    Public Sub UpdateCompLegalEntity(ByVal CompControl As Integer, ByVal LegalEntity As String)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord = db.Comps.Where(Function(x) x.CompControl = CompControl).FirstOrDefault()
                If Not oRecord Is Nothing Then
                    oRecord.CompModDate = Date.Now
                    oRecord.CompModUser = Parameters.UserName
                    oRecord.CompLegalEntity = LegalEntity

                    'Update
                    'db.Comps.Attach(oRecord, True)
                    db.SubmitChanges()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCompLegalEntity"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4 on 06/02/2021 added new filter logic
    '''     set default filter to active = true
    '''     added user company level restrictions
    ''' Modiied by RHR for v-8.5.3.007 on 03/30/2023 
    '''     added user legal entity filter if filters.LEAdminControl is zero
    ''' </remarks>
    Public Function GetLECompsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLEComp365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLEComp365
        Dim iLEAdiminControl = filters.LEAdminControl
        If (iLEAdiminControl = 0) Then
            iLEAdiminControl = Me.Parameters.UserLEControl
        End If

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vLEComp365)
                iQuery = db.vLEComp365s.Where(Function(x) x.LEAdminControl = iLEAdiminControl And x.UserSecurityControl = Me.Parameters.UserControl)
                Dim filterWhere = ""
                'set the default filter for active to true if the user has not provided one
                'first see if a filter exists
                If Not filters.FilterValues.Any(Function(x) x.filterName = "CompActive") Then
                    filterWhere = " (CompActive = true) "
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompsFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Method to Read the Per and currency values for Toleranaces
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <remarks>
    ''' Added for Tolerance changes -Company data Migration By ManoRama 13-Aug-2020
    ''' </remarks>
    ''' <returns></returns>
    Public Function ReadCompTolerance(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLEComp365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLEComp365
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vLEComp365)
                iQuery = db.vLEComp365s.Where(Function(x) x.CompControl = filters.ParentControl)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ReadCompTolerance"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Duplicate function, deprecated call GetLECompsFiltered instead
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4 on 06/02/2021 we now use GetLECompsFiltered
    '''     this function has been deprecated
    ''' </remarks>
    Public Function GetLEComp365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLEComp365()
        Return GetLECompsFiltered(RecordCount, filters)
    End Function

    Public Function GetLECompsList(ByVal LEAdminControl As Integer) As DTO.vLookupList()
        Dim vList() As DTO.vLookupList
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                vList = (
                    From t In db.vLEComp365s
                    Where t.LEAdminControl = LEAdminControl
                    Order By t.CompName
                    Select New DTO.vLookupList _
                    With {.Control = t.CompControl, .Name = t.CompName, .Description = t.CompNumber}).ToArray()

                Return vList
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompsList"), db)
            End Try
        End Using
        Return Nothing
    End Function

    '** TODO ** FIGURE OUT A BETTER WAY TO DO THIS - THIS METHOD IS TERRIBLE
    ' I THINK IT BREAKS THE OPTIMISTIC CONCURRENCY THING WITH COMPUPDATE ANYWAYS
    ''' <summary>
    ''' Updates the Company - Called from Company Maint Page 365
    ''' </summary>
    ''' <param name="uComp"></param>
    ''' <remarks>
    ''' Modified By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
    ''' Modified by RHR for v- 8.4.003 on 07/17/2021 added new logo fields
    ''' </remarks>
    Public Sub UpdateCompMaint365(ByVal uComp As LTS.Comp)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord = db.Comps.Where(Function(x) x.CompControl = uComp.CompControl).FirstOrDefault()
                If Not oRecord Is Nothing Then
                    oRecord.CompModDate = Date.Now
                    oRecord.CompModUser = Parameters.UserName
                    'oRecord.CompNumber = uComp.CompNumber
                    oRecord.CompActive = uComp.CompActive
                    oRecord.CompName = uComp.CompName
                    'oRecord.CompLegalEntity = uComp.CompLegalEntity
                    oRecord.CompAlphaCode = uComp.CompAlphaCode
                    oRecord.CompAbrev = uComp.CompAbrev
                    oRecord.CompStreetAddress1 = uComp.CompStreetAddress1
                    oRecord.CompStreetAddress2 = uComp.CompStreetAddress2
                    oRecord.CompStreetAddress3 = uComp.CompStreetAddress3
                    oRecord.CompStreetCity = uComp.CompStreetCity
                    oRecord.CompStreetState = uComp.CompStreetState
                    oRecord.CompStreetCountry = uComp.CompStreetCountry
                    oRecord.CompStreetZip = uComp.CompStreetZip
                    oRecord.CompEmail = uComp.CompEmail
                    oRecord.CompWeb = uComp.CompWeb
                    oRecord.CompNatNumber = uComp.CompNatNumber 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompNatName = uComp.CompNatName 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompFAAShipID = uComp.CompFAAShipID 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompFDAShipID = uComp.CompFDAShipID 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompLatitude = uComp.CompLatitude 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompLongitude = uComp.CompLongitude 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompTypeCategory = uComp.CompTypeCategory 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompNEXTStopAcctNo = uComp.CompNEXTStopAcctNo 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompNEXTStopPsw = uComp.CompNEXTStopPsw 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompNextstopSubmitRFP = uComp.CompNextstopSubmitRFP 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompNEXTrack = uComp.CompNEXTrack 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompAMS = uComp.CompAMS 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompFinUseImportFrtCost = uComp.CompFinUseImportFrtCost 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompSilentTender = uComp.CompSilentTender 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRestrictCarrierSelection = uComp.CompRestrictCarrierSelection 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompWarnOnRestrictedCarrierSelection = uComp.CompWarnOnRestrictedCarrierSelection 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailAddress1 = uComp.CompMailAddress1 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailAddress2 = uComp.CompMailAddress2 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailAddress3 = uComp.CompMailAddress3 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailCity = uComp.CompMailCity 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailState = uComp.CompMailState 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailZip = uComp.CompMailZip 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompMailCountry = uComp.CompMailCountry 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRailStationName = uComp.CompRailStationName 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRailSPLC = uComp.CompRailSPLC 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRailFSAC = uComp.CompRailFSAC 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRail333 = uComp.CompRail333 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompRailR260 = uComp.CompRailR260 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompUser1 = uComp.CompUser1 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompUser2 = uComp.CompUser2 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompUser3 = uComp.CompUser3 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    oRecord.CompUser4 = uComp.CompUser4 'Added By LVV On 10/29/20 For v-8.3.0.002 - Task #20201028134331 - Add Fields To Company Maint
                    ' Modified by RHR for v- 8.4.003 on 07/17/2021 added new logo fields
                    oRecord.CompHeaderLogo = uComp.CompHeaderLogo
                    oRecord.CompHeaderLogoLink = uComp.CompHeaderLogoLink
                    'Update
                    'db.Comps.Attach(oRecord, True)
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCompMaint365"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Method to Save the Tolerance values
    ''' </summary>
    ''' <remarks>
    ''' Added for Tolerance changes for Company Data Migration-ManoRama on 13-Aug-2020.
    ''' </remarks>
    ''' <param name="uComp">Comp Object</param>
    Public Sub SaveCompTolerances(ByVal uComp As LTS.Comp)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord = db.Comps.Where(Function(x) x.CompControl = uComp.CompControl).FirstOrDefault()
                If Not oRecord Is Nothing Then
                    oRecord.CompModDate = Date.Now
                    oRecord.CompModUser = Parameters.UserName


                    'oRecord.CompNumber = uComp.CompNumber
                    oRecord.CompActive = uComp.CompActive
                    oRecord.CompName = uComp.CompName
                    'oRecord.CompLegalEntity = uComp.CompLegalEntity
                    oRecord.CompAlphaCode = uComp.CompAlphaCode
                    oRecord.CompAbrev = uComp.CompAbrev
                    oRecord.CompStreetAddress1 = uComp.CompStreetAddress1
                    oRecord.CompStreetAddress2 = uComp.CompStreetAddress2
                    oRecord.CompStreetAddress3 = uComp.CompStreetAddress3
                    oRecord.CompStreetCity = uComp.CompStreetCity
                    oRecord.CompStreetState = uComp.CompStreetState
                    oRecord.CompStreetCountry = uComp.CompStreetCountry
                    oRecord.CompStreetZip = uComp.CompStreetZip
                    oRecord.CompEmail = uComp.CompEmail
                    oRecord.CompWeb = uComp.CompWeb
                    oRecord.CompPayTolPerLo = uComp.CompPayTolPerLo
                    oRecord.CompPayTolPerHi = uComp.CompPayTolPerHi
                    oRecord.CompPayTolCurLo = uComp.CompPayTolCurLo
                    oRecord.CompPayTolCurHi = uComp.CompPayTolCurHi
                    'Update
                    'db.Comps.Attach(oRecord, True)
                    db.SubmitChanges()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCompMaint365"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Deletes the Company record if validation passes
    ''' I didn't want to modify the deletes above because I am not sure where they are being used
    ''' Didn't want to add validation and break something
    ''' We can probably get rid of them when we get rid of the desktop app
    ''' </summary>
    ''' <param name="iCompControl"></param>
    Public Sub DeleteComp365(ByVal iCompControl As Integer)
        If iCompControl = 0 Then
            throwInvalidRequiredKeysException("Comp", "Invalid Comp, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oComp As LTS.Comp = db.Comps.Where(Function(x) x.CompControl = iCompControl).FirstOrDefault()
                If Not oComp Is Nothing AndAlso oComp.CompControl <> 0 Then
                    'Perform validation
                    Try
                        'Check if this company is the main company associated with any tblLegalEntityAdmin records
                        Dim lea = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminCompControl = iCompControl).FirstOrDefault()
                        If Not lea Is Nothing Then
                            Dim msg = "Warehouse " & oComp.CompName & " is linked to the Legal Entity " & lea.LEAdminLegalEntity & " and cannot be deleted."
                            Utilities.SaveAppError(msg, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = msg}, New FaultReason("E_DataValidationFailure"))
                        End If
                        'Add code here to call the Book and Lane data providers when they are created
                        Dim dpBook As New NGLBookData(Me.Parameters)
                        Dim dpLane As New NGLLaneData(Me.Parameters)
                        Dim oBooks() As DTO.Book
                        Dim oLanes() As DTO.Lane
                        Try
                            oBooks = dpBook.GetBooksByComp(iCompControl)
                        Catch ex As FaultException
                            If ex.Message <> "E_NoData" Then Throw
                        End Try
                        Try
                            oLanes = dpLane.GetLanesByComp(iCompControl)
                        Catch ex As FaultException
                            If ex.Message <> "E_NoData" Then Throw
                        End Try
                        If (Not oBooks Is Nothing AndAlso oBooks.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                            Dim msg = "Warehouse " & oComp.CompName & " (" & oComp.CompNumber & ") is being used and cannot be deleted. Check the book or lane information."
                            Utilities.SaveAppError(msg, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = msg}, New FaultReason("E_DataValidationFailure"))
                        End If
                    Catch ex As FaultException
                        Throw
                    Catch ex As InvalidOperationException
                        'do nothing this is the desired result.
                    End Try

                    'Do the Delete
                    db.Comps.DeleteOnSubmit(oComp)
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteComp365"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Synchronizes Visible Companies list for the user associated with the Legal Entity.
    ''' RULES:
    '''   1. On comp insert if a valid LegalEntity Is provided update all visible companies to include this company for the Legal Entity.  
    '''      If the Legal Entity Is Not valid do nothing.
    '''   2. On comp update if a valid Legal Entity Is provided check for users in other legal entities that may have access to this company And remove them,
    '''      then update all visible companies if they do Not already exist.
    ''' </summary>
    ''' <param name="LegalEntity"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="blnIsUpdate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 4/15/20
    ''' </remarks>
    Public Function AddVisibleCompForLEUsers(ByVal LegalEntity As String, ByVal CompNumber As Integer, ByVal blnIsUpdate As Boolean) As Models.ResultObject
        Dim result As New Models.ResultObject With {.Success = False}
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim spRes = db.spAddVisibleCompForLEUsers(LegalEntity, CompNumber, blnIsUpdate).FirstOrDefault()
                If spRes IsNot Nothing Then
                    If spRes.ErrNumber = 0 Then
                        result.Success = True
                    Else
                        result.ErrMsg = spRes.RetMsg
                        result.ErrTitle = "Add Visible Comp For LE Users Failure"
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddVisibleCompForLEUsers"), db)
            End Try
        End Using
        Return result
    End Function

    ''' <summary>
    ''' Couldn't use GetCompFilteredByAddress because we won't have the Comp Abrev or AlphaCode to filter by.
    ''' Instead use only the address fields and the legal entity of the current user to select the company.
    ''' If more than one match exists select FirstOrDefault
    ''' </summary>
    ''' <param name="CompStreetAddress1"></param>
    ''' <param name="CompStreetCity"></param>
    ''' <param name="CompStreetState"></param>
    ''' <param name="CompStreetZip"></param>
    ''' <returns></returns>
    Public Function GetCompFilteredByAddressAndUserLE(ByVal CompStreetAddress1 As String, ByVal CompStreetCity As String, ByVal CompStreetState As String, ByVal CompStreetZip As String) As LTS.Comp
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim legalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = Parameters.UserLEControl).Select(Function(y) y.LEAdminLegalEntity).FirstOrDefault()
                Dim comp = db.Comps.Where(Function(x) x.CompLegalEntity = legalEntity AndAlso x.CompStreetAddress1.Trim() = CompStreetAddress1.Trim() AndAlso x.CompStreetState.Trim() = CompStreetState.Trim() AndAlso x.CompStreetZip.Trim() = CompStreetZip.Trim()).FirstOrDefault()
                Return comp
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompFilteredByAddressAndUserLE"))
            End Try
            Return Nothing
        End Using
    End Function


    Public Function GetCompFilteredByAddressAndUserLE(ByVal CompName As String, ByVal CompStreetAddress1 As String, ByVal CompStreetCity As String, ByVal CompStreetState As String, ByVal CompStreetZip As String) As LTS.Comp
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim legalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = Parameters.UserLEControl).Select(Function(y) y.LEAdminLegalEntity).FirstOrDefault()
                Dim comp = db.Comps.Where(Function(x) x.CompLegalEntity = legalEntity AndAlso x.CompStreetAddress1.Trim() = CompStreetAddress1.Trim() AndAlso x.CompStreetState.Trim() = CompStreetState.Trim() AndAlso x.CompStreetZip.Trim() = CompStreetZip.Trim() AndAlso x.CompName = CompName).FirstOrDefault()
                If comp Is Nothing Then
                    comp = GetCompFilteredByAddressAndUserLE(CompStreetAddress1, CompStreetCity, CompStreetState, CompStreetZip)
                End If
                Return comp
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompFilteredByAddressAndUserLE"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Saves the Lattitude and Longitude for the Company
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="dblLat"></param>
    ''' <param name="dblLong"></param>
    ''' <remarks>Created By LVV on 6/15/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes</remarks>
    Public Sub SaveCompLatLong(ByVal CompControl As Integer, ByVal dblLat As Double, ByVal dblLong As Double)
        If CompControl = 0 Then throwInvalidRequiredKeysException("Company", "Invalid company, a control nunber is required and cannot be zero")
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim comp = db.Comps.Where(Function(x) x.CompControl = CompControl).FirstOrDefault()
                comp.CompLatitude = dblLat
                comp.CompLongitude = dblLong
                comp.CompModDate = Date.Now
                comp.CompModUser = Parameters.UserName
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCompLatLong"))
            End Try
        End Using
    End Sub


#Region "SCHEDULER - APPOINTMENT DETAIL METHODS"
    '*** SCHEDULER - APPOINTMENT DETAIL METHODS ***

    ''' <summary>
    ''' Determines which AMS Appointment Detail fields have visibility (in hover over and label)
    ''' turned on for this CompControl and returns the results in the reference
    ''' parameters. Primarily called by other backend methods and not intended to be called
    ''' by UI methods.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="ApptNumberOn"></param>
    ''' <param name="CarrierOn"></param>
    ''' <param name="DockNameOn"></param>
    ''' <param name="CNSNumbersOn"></param>
    ''' <param name="OrderNumbersOn"></param>
    ''' <param name="ProNumbersOn"></param>
    ''' <param name="PONumbersOn"></param>
    ''' <remarks>
    ''' Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub GetCompAMSApptDetailFieldsVisibility(ByVal CompControl As Integer,
                                                    ByRef ApptNumberOn As Boolean,
                                                    ByRef CarrierOn As Boolean,
                                                    ByRef DockNameOn As Boolean,
                                                    ByRef CNSNumbersOn As Boolean,
                                                    ByRef OrderNumbersOn As Boolean,
                                                    ByRef ProNumbersOn As Boolean,
                                                    ByRef PONumbersOn As Boolean,
                                                    ByRef ApptNotesOn As Boolean)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the current integers for the ApptDetailsVisible config
                Dim intFieldsVisible As Integer = 0
                intFieldsVisible = db.Comps.Where(Function(x) x.CompControl = CompControl).Select(Function(y) y.CompAMSApptDetFieldsVisible).FirstOrDefault()
                If intFieldsVisible = Nothing Then intFieldsVisible = 0
                Dim bwDetailFields As New Ngl.Core.Utility.BitwiseFlags(intFieldsVisible)
                ApptNumberOn = bwDetailFields.isBitFlagOn(1)
                CarrierOn = bwDetailFields.isBitFlagOn(2)
                DockNameOn = bwDetailFields.isBitFlagOn(3)
                CNSNumbersOn = bwDetailFields.isBitFlagOn(4)
                OrderNumbersOn = bwDetailFields.isBitFlagOn(5)
                ProNumbersOn = bwDetailFields.isBitFlagOn(6)
                PONumbersOn = bwDetailFields.isBitFlagOn(7)
                ApptNotesOn = bwDetailFields.isBitFlagOn(8)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompAMSApptDetailFieldsVisibility"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets all the AMS Appointment Details Fields
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetCompAMSApptDetailFields(ByVal CompControl As Integer) As Models.DockPTType()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLook As New NGLLookupDataProvider(Parameters)
                Dim retFields = oLook.GetAllAMSApptDetailFields() 'Get all fields and set them all to checked off by default

                Dim intFlag = db.Comps.Where(Function(x) x.CompControl = CompControl).Select(Function(y) y.CompAMSApptDetFieldsVisible).FirstOrDefault()
                If intFlag = Nothing Then intFlag = 0
                Dim bwDetailFields As New Ngl.Core.Utility.BitwiseFlags(intFlag)

                'Get the current enumns of all fields checked on
                Dim onList = bwDetailFields.refreshPositiveBitPositions()

                'If any items in the default return list match the IDs from onList set those to On
                For Each r In retFields
                    If onList.Contains(r.PTBitPos) Then r.PTOn = True
                Next

                Return retFields

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompAMSApptDetailFields"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Sub SaveCompAMSApptDetailFields(ByVal CompControl As Integer, ByVal fieldsToTurnOn() As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim bwDetailFields As New Ngl.Core.Utility.BitwiseFlags() 'start with a bitwise flag where all positions are off
                'Turn on the selected fields
                For Each p In fieldsToTurnOn
                    bwDetailFields.turnBitFlagOn(p)
                Next

                Dim comp = db.Comps.Where(Function(x) x.CompControl = CompControl).FirstOrDefault()

                If Not comp Is Nothing Then
                    'Update
                    comp.CompAMSApptDetFieldsVisible = bwDetailFields.FlagSource.ToString()
                    comp.CompModDate = Date.Now
                    comp.CompModUser = Parameters.UserName
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCompAMSApptDetailFields"), db)
            End Try
        End Using
    End Sub

    Public Function GetCompWeekendLoadSettings(ByVal CompControl As Integer) As Models.DriveDays
        Dim oRet As New Models.DriveDays()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                oRet = (
                    From t In db.Comps
                    Where t.CompControl = CompControl
                    Select New Models.DriveDays _
                    With {.Control = t.CompControl, .DriveSun = t.CompWillLoadOnSunday, .DriveSat = t.CompWillLoadOnSaturday}).FirstOrDefault()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompWeekendLoadSettings"), db)
            End Try
        End Using
        Return oRet
    End Function

#End Region


#End Region

#End Region

#Region "Protected Methods"

    'Modified by LVV 10/25/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    'Modified By LVV on 4/22/20 Added CompAMSApptDetFieldsVisible
    'Modified by by RHR for v-8.5.3.006 on 12/01/2022 added CompWillLoadOnSunday and  CompWillLoadOnSaturday
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.Comp)
        'Create New Record
        Return New LTS.Comp With {.CompControl = d.CompControl _
                                          , .CompNumber = d.CompNumber _
                                          , .CompName = d.CompName _
                                          , .CompNatNumber = d.CompNatNumber _
                                          , .CompNatName = d.CompNatName _
                                          , .CompStreetAddress1 = d.CompStreetAddress1 _
                                          , .CompStreetAddress2 = d.CompStreetAddress2 _
                                          , .CompStreetAddress3 = d.CompStreetAddress3 _
                                          , .CompStreetCity = d.CompStreetCity _
                                          , .CompStreetState = d.CompStreetState _
                                          , .CompStreetCountry = d.CompStreetCountry _
                                          , .CompStreetZip = d.CompStreetZip _
                                          , .CompMailAddress1 = d.CompMailAddress1 _
                                          , .CompMailAddress2 = d.CompMailAddress2 _
                                          , .CompMailAddress3 = d.CompMailAddress3 _
                                          , .CompMailCity = d.CompMailCity _
                                          , .CompMailState = d.CompMailState _
                                          , .CompMailCountry = d.CompMailCountry _
                                          , .CompMailZip = d.CompMailZip _
                                          , .CompWeb = d.CompWeb _
                                          , .CompEmail = d.CompEmail _
                                          , .CompModDate = Date.Now _
                                          , .CompModUser = Parameters.UserName _
                                          , .CompDirections = d.CompDirections _
                                          , .CompAbrev = d.CompAbrev _
                                          , .CompActive = d.CompActive _
                                          , .CompNEXTrack = d.CompNEXTrack _
                                          , .CompNEXTStopAcctNo = d.CompNEXTStopAcctNo _
                                          , .CompNEXTStopPsw = d.CompNEXTStopPsw _
                                          , .CompNextstopSubmitRFP = d.CompNextstopSubmitRFP _
                                          , .CompTypeCategory = d.CompTypeCategory _
                                          , .CompFAAShipID = d.CompFAAShipID _
                                          , .CompFAAShipDate = d.CompFAAShipDate _
                                          , .CompUpdated = If(d.CompUpdated Is Nothing, New Byte() {}, d.CompUpdated) _
                                          , .CompBudSeasDescription = d.CompBudSeasDescription _
                                          , .CompBudSeasMo1 = d.CompBudSeasMo1 _
                                          , .CompBudSeasMo2 = d.CompBudSeasMo2 _
                                          , .CompBudSeasMo3 = d.CompBudSeasMo3 _
                                          , .CompBudSeasMo4 = d.CompBudSeasMo4 _
                                          , .CompBudSeasMo5 = d.CompBudSeasMo5 _
                                          , .CompBudSeasMo6 = d.CompBudSeasMo6 _
                                          , .CompBudSeasMo7 = d.CompBudSeasMo7 _
                                          , .CompBudSeasMo8 = d.CompBudSeasMo8 _
                                          , .CompBudSeasMo9 = d.CompBudSeasMo9 _
                                          , .CompBudSeasMo10 = d.CompBudSeasMo10 _
                                          , .CompBudSeasMo11 = d.CompBudSeasMo11 _
                                          , .CompBudSeasMo12 = d.CompBudSeasMo12 _
                                          , .CompBudSlsBudgetMo1 = d.CompBudSlsBudgetMo1 _
                                          , .CompBudSlsBudgetMo2 = d.CompBudSlsBudgetMo2 _
                                          , .CompBudSlsBudgetMo3 = d.CompBudSlsBudgetMo3 _
                                          , .CompBudSlsBudgetMo4 = d.CompBudSlsBudgetMo4 _
                                          , .CompBudSlsBudgetMo5 = d.CompBudSlsBudgetMo5 _
                                          , .CompBudSlsBudgetMo6 = d.CompBudSlsBudgetMo6 _
                                          , .CompBudSlsBudgetMo7 = d.CompBudSlsBudgetMo7 _
                                          , .CompBudSlsBudgetMo8 = d.CompBudSlsBudgetMo8 _
                                          , .CompBudSlsBudgetMo9 = d.CompBudSlsBudgetMo9 _
                                          , .CompBudSlsBudgetMo10 = d.CompBudSlsBudgetMo10 _
                                          , .CompBudSlsBudgetMo11 = d.CompBudSlsBudgetMo11 _
                                          , .CompBudSlsBudgetMo12 = d.CompBudSlsBudgetMo12 _
                                          , .CompBudSlsBudgetMoTotal = d.CompBudSlsBudgetMoTotal _
                                          , .CompBudSlsActualMo1 = d.CompBudSlsActualMo1 _
                                          , .CompBudSlsActualMo2 = d.CompBudSlsActualMo2 _
                                          , .CompBudSlsActualMo3 = d.CompBudSlsActualMo3 _
                                          , .CompBudSlsActualMo4 = d.CompBudSlsActualMo4 _
                                          , .CompBudSlsActualMo5 = d.CompBudSlsActualMo5 _
                                          , .CompBudSlsActualMo6 = d.CompBudSlsActualMo6 _
                                          , .CompBudSlsActualMo7 = d.CompBudSlsActualMo7 _
                                          , .CompBudSlsActualMo8 = d.CompBudSlsActualMo8 _
                                          , .CompBudSlsActualMo9 = d.CompBudSlsActualMo9 _
                                          , .CompBudSlsActualMo10 = d.CompBudSlsActualMo10 _
                                          , .CompBudSlsActualMo11 = d.CompBudSlsActualMo11 _
                                          , .CompBudSlsActualMo12 = d.CompBudSlsActualMo12 _
                                          , .CompBudSlsActualMoTotal = d.CompBudSlsActualMoTotal _
                                          , .CompBudSlsMarginBudget = d.CompBudSlsMarginBudget _
                                          , .CompBudSlsMarginActual = d.CompBudSlsMarginActual _
                                          , .CompBudCogsBudgetMo1 = d.CompBudCogsBudgetMo1 _
                                          , .CompBudCogsBudgetMo2 = d.CompBudCogsBudgetMo2 _
                                          , .CompBudCogsBudgetMo3 = d.CompBudCogsBudgetMo3 _
                                          , .CompBudCogsBudgetMo4 = d.CompBudCogsBudgetMo4 _
                                          , .CompBudCogsBudgetMo5 = d.CompBudCogsBudgetMo5 _
                                          , .CompBudCogsBudgetMo6 = d.CompBudCogsBudgetMo6 _
                                          , .CompBudCogsBudgetMo7 = d.CompBudCogsBudgetMo7 _
                                          , .CompBudCogsBudgetMo8 = d.CompBudCogsBudgetMo8 _
                                          , .CompBudCogsBudgetMo9 = d.CompBudCogsBudgetMo9 _
                                          , .CompBudCogsBudgetMo10 = d.CompBudCogsBudgetMo10 _
                                          , .CompBudCogsBudgetMo11 = d.CompBudCogsBudgetMo11 _
                                          , .CompBudCogsBudgetMo12 = d.CompBudCogsBudgetMo12 _
                                          , .CompBudCogsBudgetMoTotal = d.CompBudCogsBudgetMoTotal _
                                          , .CompBudCogsActualMo1 = d.CompBudCogsActualMo1 _
                                          , .CompBudCogsActualMo2 = d.CompBudCogsActualMo2 _
                                          , .CompBudCogsActualMo3 = d.CompBudCogsActualMo3 _
                                          , .CompBudCogsActualMo4 = d.CompBudCogsActualMo4 _
                                          , .CompBudCogsActualMo5 = d.CompBudCogsActualMo5 _
                                          , .CompBudCogsActualMo6 = d.CompBudCogsActualMo6 _
                                          , .CompBudCogsActualMo7 = d.CompBudCogsActualMo7 _
                                          , .CompBudCogsActualMo8 = d.CompBudCogsActualMo8 _
                                          , .CompBudCogsActualMo9 = d.CompBudCogsActualMo9 _
                                          , .CompBudCogsActualMo10 = d.CompBudCogsActualMo10 _
                                          , .CompBudCogsActualMo11 = d.CompBudCogsActualMo11 _
                                          , .CompBudCogsActualMo12 = d.CompBudCogsActualMo12 _
                                          , .CompBudCogsActualMoTotal = d.CompBudCogsActualMoTotal _
                                          , .CompBudCogsMarginBudget = d.CompBudCogsMarginBudget _
                                          , .CompBudCogsMarginActual = d.CompBudCogsMarginActual _
                                          , .CompBudProfitBudgetMo1 = d.CompBudProfitBudgetMo1 _
                                          , .CompBudProfitBudgetMo2 = d.CompBudProfitBudgetMo2 _
                                          , .CompBudProfitBudgetMo3 = d.CompBudProfitBudgetMo3 _
                                          , .CompBudProfitBudgetMo4 = d.CompBudProfitBudgetMo4 _
                                          , .CompBudProfitBudgetMo5 = d.CompBudProfitBudgetMo5 _
                                          , .CompBudProfitBudgetMo6 = d.CompBudProfitBudgetMo6 _
                                          , .CompBudProfitBudgetMo7 = d.CompBudProfitBudgetMo7 _
                                          , .CompBudProfitBudgetMo8 = d.CompBudProfitBudgetMo8 _
                                          , .CompBudProfitBudgetMo9 = d.CompBudProfitBudgetMo9 _
                                          , .CompBudProfitBudgetMo10 = d.CompBudProfitBudgetMo10 _
                                          , .CompBudProfitBudgetMo11 = d.CompBudProfitBudgetMo11 _
                                          , .CompBudProfitBudgetMo12 = d.CompBudProfitBudgetMo12 _
                                          , .CompBudProfitBudgetMoTotal = d.CompBudProfitBudgetMoTotal _
                                          , .CompBudProfitActualMo1 = d.CompBudProfitActualMo1 _
                                          , .CompBudProfitActualMo2 = d.CompBudProfitActualMo2 _
                                          , .CompBudProfitActualMo3 = d.CompBudProfitActualMo3 _
                                          , .CompBudProfitActualMo4 = d.CompBudProfitActualMo4 _
                                          , .CompBudProfitActualMo5 = d.CompBudProfitActualMo5 _
                                          , .CompBudProfitActualMo6 = d.CompBudProfitActualMo6 _
                                          , .CompBudProfitActualMo7 = d.CompBudProfitActualMo7 _
                                          , .CompBudProfitActualMo8 = d.CompBudProfitActualMo8 _
                                          , .CompBudProfitActualMo9 = d.CompBudProfitActualMo9 _
                                          , .CompBudProfitActualMo10 = d.CompBudProfitActualMo10 _
                                          , .CompBudProfitActualMo11 = d.CompBudProfitActualMo11 _
                                          , .CompBudProfitActualMo12 = d.CompBudProfitActualMo12 _
                                          , .CompBudProfitActualMoTotal = d.CompBudProfitActualMoTotal _
                                          , .CompBudProfitMarginBudget = d.CompBudProfitMarginBudget _
                                          , .CompBudProfitMarginActual = d.CompBudProfitMarginActual _
                                          , .CompFinDuns = d.CompFinDuns _
                                          , .CompFinTaxID = d.CompFinTaxID _
                                          , .CompFinPaymentForm = d.CompFinPaymentForm _
                                          , .CompFinSIC = d.CompFinSIC _
                                          , .CompFinPaymentDiscount = d.CompFinPaymentDiscount _
                                          , .CompFinPaymentDays = d.CompFinPaymentDays _
                                          , .CompFinPaymentNetDays = d.CompFinPaymentNetDays _
                                          , .CompFinCommTerms = d.CompFinCommTerms _
                                          , .CompFinCommTermsPer = d.CompFinCommTermsPer _
                                          , .CompFinCommCompControl = d.CompFinCommCompControl _
                                          , .CompFinCreditLimit = d.CompFinCreditLimit _
                                          , .CompFinCreditUsed = d.CompFinCreditUsed _
                                          , .CompFinCreditAvail = d.CompFinCreditAvail _
                                          , .CompFinYTDbookedCurr = d.CompFinYTDbookedCurr _
                                          , .CompFinYTDbookedLast = d.CompFinYTDbookedLast _
                                          , .CompFinYTDcarrierCurr = d.CompFinYTDcarrierCurr _
                                          , .CompFinYTDcarrierLast = d.CompFinYTDcarrierLast _
                                          , .CompFinYTDsavingsCurr = d.CompFinYTDsavingsCurr _
                                          , .CompFinYTDsavingsLast = d.CompFinYTDsavingsLast _
                                          , .CompFinYTDRevenuesCur = d.CompFinYTDRevenuesCur _
                                          , .CompFinYTDRevenuesLast = d.CompFinYTDRevenuesLast _
                                          , .CompFinYTDnoLoadsCurr = d.CompFinYTDnoLoadsCurr _
                                          , .CompFinYTDnoLoadsLast = d.CompFinYTDnoLoadsLast _
                                          , .CompFinInvPrnCode = d.CompFinInvPrnCode _
                                          , .CompFinInvEMailCode = d.CompFinInvEMailCode _
                                          , .CompFinCurType = d.CompFinCurType _
                                          , .CompFinUser1 = d.CompFinUser1 _
                                          , .CompFinUser2 = d.CompFinUser2 _
                                          , .CompFinUser3 = d.CompFinUser3 _
                                          , .CompFinUser4 = d.CompFinUser4 _
                                          , .CompFinUser5 = d.CompFinUser5 _
                                          , .CompFinCustomerSince = d.CompFinCustomerSince _
                                          , .CompFinCardType = d.CompFinCardType _
                                          , .CompFinCardName = d.CompFinCardName _
                                          , .CompFinCardExpires = d.CompFinCardExpires _
                                          , .CompFinCardAuthorizor = d.CompFinCardAuthorizor _
                                          , .CompFinCardAuthPassword = d.CompFinCardAuthPassword _
                                          , .CompFinUseImportFrtCost = d.CompFinUseImportFrtCost _
                                          , .CompFinBkhlFlatFee = d.CompFinBkhlFlatFee _
                                          , .CompFinBkhlCostPerc = d.CompFinBkhlCostPerc _
                                          , .CompLatitude = d.CompLatitude _
                                          , .CompLongitude = d.CompLongitude _
                                          , .CompMailTo = d.CompMailTo _
                                          , .CompFDAShipID = d.CompFDAShipID _
                                          , .CompAMS = d.CompAMS _
                                          , .CompPayTolPerLo = d.CompPayTolPerLo _
                                          , .CompPayTolPerHi = d.CompPayTolPerHi _
                                          , .CompPayTolCurLo = d.CompPayTolCurLo _
                                          , .CompPayTolCurHi = d.CompPayTolCurHi _
                                          , .CompPayTolWgtFrom = d.CompPayTolWgtFrom _
                                          , .CompPayTolWgtTo = d.CompPayTolWgtTo _
                                          , .CompPayTolTaxPerLo = d.CompPayTolTaxPerLo _
                                          , .CompPayTolTaxPerHi = d.CompPayTolTaxPerHi _
                                          , .CompFinBillToCompControl = d.CompFinBillToCompControl _
                                          , .CompSilentTender = d.CompSilentTender _
                                          , .CompTimeZone = d.CompTimeZone _
                                          , .CompRailStationName = d.CompRailStationName _
                                          , .CompRailSPLC = d.CompRailSPLC _
                                          , .CompRailFSAC = d.CompRailFSAC _
                                          , .CompRail333 = d.CompRail333 _
                                          , .CompRailR260 = d.CompRailR260 _
                                          , .CompIsTransLoad = d.CompIsTransLoad _
                                          , .CompUser1 = d.CompUser1 _
                                          , .CompUser2 = d.CompUser2 _
                                          , .CompUser3 = d.CompUser3 _
                                          , .CompUser4 = d.CompUser4 _
                                          , .CompAlphaCode = d.CompAlphaCode _
                                          , .CompLegalEntity = d.CompLegalEntity _
                                          , .CompFinFBToleranceHigh = d.CompFinFBToleranceHigh _
                                          , .CompFinFBToleranceLow = d.CompFinFBToleranceLow _
                                          , .CompRestrictCarrierSelection = d.CompRestrictCarrierSelection _
                                          , .CompWarnOnRestrictedCarrierSelection = d.CompWarnOnRestrictedCarrierSelection _
                                          , .CompNotifyEmail = d.CompNotifyEmail _
                                          , .CompPhone = d.CompPhone _
                                          , .CompFax = d.CompFax _
                                          , .CompCarrierLoadAcceptanceAllowedMinutes = d.CompCarrierLoadAcceptanceAllowedMinutes _
                                            , .CompRejectedLoadsTo = d.CompRejectedLoadsTo _
                                            , .CompRejectedLoadsCc = d.CompRejectedLoadsCc _
                                            , .CompExpiredLoadsTo = d.CompExpiredLoadsTo _
                                            , .CompExpiredLoadsCc = d.CompExpiredLoadsCc _
                                            , .CompAcceptedLoadsTo = d.CompAcceptedLoadsTo _
                                            , .CompAcceptedLoadsCc = d.CompAcceptedLoadsCc _
                                            , .CompHeaderLogo = d.CompHeaderLogo _
                                            , .CompHeaderLogoLink = d.CompHeaderLogoLink _
                                            , .CompAMSApptDetFieldsVisible = d.CompAMSApptDetFieldsVisible _
                                            , .CompWillLoadOnSunday = d.CompWillLoadOnSunday _
                                            , .CompWillLoadOnSaturday = d.CompWillLoadOnSaturday}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompFiltered(Control:=CType(LinqTable, LTS.Comp).CompControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.Comp = TryCast(LinqTable, LTS.Comp)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.Comps
                       Where d.CompControl = source.CompControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompControl _
                                                            , .ModDate = d.CompModDate _
                                                            , .ModUser = d.CompModUser _
                                                            , .Updated = d.CompUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed       
        With CType(oData, DTO.Comp)
            Try
                'Validate that required fields have been entered
                Dim strKeys As String = ""
                Dim strVals As String = ""
                Dim blnValidationErr As Boolean = False
                Dim strValidationMsg As String = ""
                Dim strSpacer As String = ""
                If .CompNumber = 0 Then
                    blnValidationErr = True
                    strValidationMsg = "Company Number"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CompName) OrElse .CompName.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Company Name"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CompAbrev) OrElse .CompAbrev.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Comp Abbreviation"
                    strSpacer = "; "
                End If
                If blnValidationErr Then
                    throwInvalidRequiredKeysException("Company", strValidationMsg)
                End If
                Dim blnExists As Boolean = False
                strKeys = "Company Name; Company Number"
                strVals = .CompName & "; " & .CompNumber
                If Not String.IsNullOrEmpty(.CompAlphaCode) AndAlso .CompAlphaCode.Trim.Length > 0 Then
                    If Not String.IsNullOrEmpty(.CompLegalEntity) AndAlso .CompLegalEntity.Trim.Length > 0 Then
                        'it is possible for the CompAlphaCode and company name to duplciate across Legal Entities
                        strKeys &= "; Company Alpha Code; Company Legal Entity"
                        strVals &= "; " & .CompAlphaCode & "; " & .CompLegalEntity
                        blnExists = CType(oDB, NGLMASCompDataContext).Comps.Any(Function(x) x.CompName = .CompName And x.CompNumber = .CompNumber And x.CompAlphaCode = .CompAlphaCode And x.CompLegalEntity = .CompLegalEntity)
                    Else
                        strKeys &= ": Company Alpha Code"
                        strVals &= "; " & .CompAlphaCode
                        blnExists = CType(oDB, NGLMASCompDataContext).Comps.Any(Function(x) x.CompName = .CompName And x.CompNumber = .CompNumber And x.CompAlphaCode = .CompAlphaCode)
                    End If
                Else
                    blnExists = CType(oDB, NGLMASCompDataContext).Comps.Any(Function(x) x.CompName = .CompName And x.CompNumber = .CompNumber)
                End If

                If blnExists Then throwInvalidKeysAlreadyExistsException("Company", strKeys, strVals)

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateNewRecord"))
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Comp)
            Try
                'Validate that required fields have been entered
                Dim blnValidationErr As Boolean = False
                Dim strValidationMsg As String = ""
                Dim strSpacer As String = ""
                If .CompNumber = 0 Then
                    blnValidationErr = True
                    strValidationMsg = "Company Number"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CompName) OrElse .CompName.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Company Name"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CompAbrev) OrElse .CompAbrev.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Company Abbreviation"
                    strSpacer = "; "
                End If
                If blnValidationErr Then
                    throwInvalidRequiredKeysException("Company", strValidationMsg)
                End If
                Dim blnExists As Boolean = False
                blnExists = CType(oDB, NGLMASCompDataContext).Comps.Any(Function(x) x.CompControl <> .CompControl And x.CompNumber = .CompNumber)
                If blnExists Then throwInvalidKeyAlreadyExistsException("Company", "Company Number", .CompNumber)
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateUpdatedRecord"))
            End Try

        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the record is being used 
        With CType(oData, DTO.Comp)
            Try
                'Add code here to call the Book and Lane data providers when they are created
                Dim dpBook As New NGLBookData(Me.Parameters)
                Dim dpLane As New NGLLaneData(Me.Parameters)
                Dim oBooks() As DTO.Book
                Dim oLanes() As DTO.Lane
                Try
                    oBooks = dpBook.GetBooksByComp(.CompControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                Try
                    oLanes = dpLane.GetLanesByComp(.CompControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                If (Not oBooks Is Nothing AndAlso oBooks.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                    Utilities.SaveAppError("Cannot delete Comp data.  The company number, " & .CompNumber & " is being used and cannot be deleted. check the book or lane information.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)
        With CType(LinqTable, LTS.Comp)
            'Add Detail Records
            .CompConts.AddRange(
                From d In CType(oData, DTO.Comp).CompConts
                Select New LTS.CompCont With {.CompContControl = d.CompContControl _
                                                , .CompContCompControl = d.CompContCompControl _
                                                , .CompContName = d.CompContName _
                                                , .CompContTitle = d.CompContTitle _
                                                , .CompCont800 = d.CompCont800 _
                                                , .CompContPhone = d.CompContPhone _
                                                , .CompContPhoneExt = d.CompContPhoneExt _
                                                , .CompContFax = d.CompContFax _
                                                , .CompContEmail = d.CompContEmail _
                                                , .CompContTender = d.CompContTender _
                                                , .CompContUpdated = If(d.CompContUpdated Is Nothing, New Byte() {}, d.CompContUpdated)})
        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCompDataContext)
            .CompConts.InsertAllOnSubmit(CType(LinqTable, LTS.Comp).CompConts)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        With CType(oDB, NGLMASCompDataContext)
            ' Process any inserted detail records 
            .CompConts.InsertAllOnSubmit(GetCompContChanges(oData, TrackingInfo.Created))
            ' Process any updated detail records
            .CompConts.AttachAll(GetCompContChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted detail records
            Dim deletedDetails = GetCompContChanges(oData, TrackingInfo.Deleted)
            .CompConts.AttachAll(deletedDetails, True)
            .CompConts.DeleteAllOnSubmit(deletedDetails)
        End With
    End Sub

    Protected Function GetCompContChanges(ByVal source As DTO.Comp, ByVal changeType As TrackingInfo) As List(Of LTS.CompCont)
        ' Tease out order details with specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CompCont) = (
          From d In source.CompConts
          Where d.TrackingState = changeType
          Select New LTS.CompCont With {.CompContControl = d.CompContControl,
                                            .CompContCompControl = d.CompContCompControl,
                                            .CompContName = d.CompContName,
                                            .CompContTitle = d.CompContTitle,
                                            .CompCont800 = d.CompCont800,
                                            .CompContPhone = d.CompContPhone,
                                            .CompContPhoneExt = d.CompContPhoneExt,
                                            .CompContEmail = d.CompContEmail,
                                            .CompContTender = d.CompContTender,
                                            .CompContUpdated = If(d.CompContUpdated Is Nothing, New Byte() {}, d.CompContUpdated)})
        Return details.ToList()
    End Function

    'Private Function selectDTOData(ByVal d As LTS.Comp, ByRef db As NGLMASCompDataContext) As DTO.Comp

    '    Return New DTO.Comp With {.CompControl = d.CompControl _
    '                                          , .CompNumber = If(d.CompNumber.HasValue, d.CompNumber.Value, 0) _
    '                                          , .CompName = d.CompName _
    '                                          , .CompNatNumber = If(d.CompNatNumber.HasValue, d.CompNatNumber.Value, 0) _
    '                                          , .CompNatName = d.CompNatName _
    '                                          , .CompStreetAddress1 = d.CompStreetAddress1 _
    '                                          , .CompStreetAddress2 = d.CompStreetAddress2 _
    '                                          , .CompStreetAddress3 = d.CompStreetAddress3 _
    '                                          , .CompStreetCity = d.CompStreetCity _
    '                                          , .CompStreetState = d.CompStreetState _
    '                                          , .CompStreetCountry = d.CompStreetCountry _
    '                                          , .CompStreetZip = d.CompStreetZip _
    '                                          , .CompMailAddress1 = d.CompMailAddress1 _
    '                                          , .CompMailAddress2 = d.CompMailAddress2 _
    '                                          , .CompMailAddress3 = d.CompMailAddress3 _
    '                                          , .CompMailCity = d.CompMailCity _
    '                                          , .CompMailState = d.CompMailState _
    '                                          , .CompMailCountry = d.CompMailCountry _
    '                                          , .CompMailZip = d.CompMailZip _
    '                                          , .CompWeb = d.CompWeb _
    '                                          , .CompEmail = d.CompEmail _
    '                                          , .CompModDate = d.CompModDate _
    '                                          , .CompModUser = d.CompModUser _
    '                                          , .CompDirections = d.CompDirections _
    '                                          , .CompAbrev = d.CompAbrev _
    '                                          , .CompActive = d.CompActive _
    '                                          , .CompNEXTrack = d.CompNEXTrack _
    '                                          , .CompNEXTStopAcctNo = d.CompNEXTStopAcctNo _
    '                                          , .CompNEXTStopPsw = d.CompNEXTStopPsw _
    '                                          , .CompNextstopSubmitRFP = d.CompNextstopSubmitRFP _
    '                                          , .CompTypeCategory = If(d.CompTypeCategory.HasValue, d.CompTypeCategory.Value, 0) _
    '                                          , .CompFAAShipID = d.CompFAAShipID _
    '                                          , .CompFAAShipDate = d.CompFAAShipDate _
    '                                          , .CompUpdated = d.CompUpdated.ToArray() _
    '                                          , .CompBudSeasDescription = d.CompBudSeasDescription _
    '                                          , .CompBudSeasMo1 = If(d.CompBudSeasMo1.HasValue, d.CompBudSeasMo1.Value, 0) _
    '                                          , .CompBudSeasMo2 = If(d.CompBudSeasMo2.HasValue, d.CompBudSeasMo2.Value, 0) _
    '                                          , .CompBudSeasMo3 = If(d.CompBudSeasMo3.HasValue, d.CompBudSeasMo3.Value, 0) _
    '                                          , .CompBudSeasMo4 = If(d.CompBudSeasMo4.HasValue, d.CompBudSeasMo4.Value, 0) _
    '                                          , .CompBudSeasMo5 = If(d.CompBudSeasMo5.HasValue, d.CompBudSeasMo5.Value, 0) _
    '                                          , .CompBudSeasMo6 = If(d.CompBudSeasMo6.HasValue, d.CompBudSeasMo6.Value, 0) _
    '                                          , .CompBudSeasMo7 = If(d.CompBudSeasMo7.HasValue, d.CompBudSeasMo7.Value, 0) _
    '                                          , .CompBudSeasMo8 = If(d.CompBudSeasMo8.HasValue, d.CompBudSeasMo8.Value, 0) _
    '                                          , .CompBudSeasMo9 = If(d.CompBudSeasMo9.HasValue, d.CompBudSeasMo9.Value, 0) _
    '                                          , .CompBudSeasMo10 = If(d.CompBudSeasMo10.HasValue, d.CompBudSeasMo10.Value, 0) _
    '                                          , .CompBudSeasMo11 = If(d.CompBudSeasMo11.HasValue, d.CompBudSeasMo11.Value, 0) _
    '                                          , .CompBudSeasMo12 = If(d.CompBudSeasMo12.HasValue, d.CompBudSeasMo12.Value, 0) _
    '                                          , .CompBudSlsBudgetMo1 = If(d.CompBudSlsBudgetMo1.HasValue, d.CompBudSlsBudgetMo1.Value, 0) _
    '                                          , .CompBudSlsBudgetMo2 = If(d.CompBudSlsBudgetMo2.HasValue, d.CompBudSlsBudgetMo2.Value, 0) _
    '                                          , .CompBudSlsBudgetMo3 = If(d.CompBudSlsBudgetMo3.HasValue, d.CompBudSlsBudgetMo3.Value, 0) _
    '                                          , .CompBudSlsBudgetMo4 = If(d.CompBudSlsBudgetMo4.HasValue, d.CompBudSlsBudgetMo4.Value, 0) _
    '                                          , .CompBudSlsBudgetMo5 = If(d.CompBudSlsBudgetMo5.HasValue, d.CompBudSlsBudgetMo5.Value, 0) _
    '                                          , .CompBudSlsBudgetMo6 = If(d.CompBudSlsBudgetMo6.HasValue, d.CompBudSlsBudgetMo6.Value, 0) _
    '                                          , .CompBudSlsBudgetMo7 = If(d.CompBudSlsBudgetMo7.HasValue, d.CompBudSlsBudgetMo7.Value, 0) _
    '                                          , .CompBudSlsBudgetMo8 = If(d.CompBudSlsBudgetMo8.HasValue, d.CompBudSlsBudgetMo8.Value, 0) _
    '                                          , .CompBudSlsBudgetMo9 = If(d.CompBudSlsBudgetMo9.HasValue, d.CompBudSlsBudgetMo9.Value, 0) _
    '                                          , .CompBudSlsBudgetMo10 = If(d.CompBudSlsBudgetMo10.HasValue, d.CompBudSlsBudgetMo10.Value, 0) _
    '                                          , .CompBudSlsBudgetMo11 = If(d.CompBudSlsBudgetMo11.HasValue, d.CompBudSlsBudgetMo11.Value, 0) _
    '                                          , .CompBudSlsBudgetMo12 = If(d.CompBudSlsBudgetMo12.HasValue, d.CompBudSlsBudgetMo12.Value, 0) _
    '                                          , .CompBudSlsBudgetMoTotal = If(d.CompBudSlsBudgetMoTotal.HasValue, d.CompBudSlsBudgetMoTotal.Value, 0) _
    '                                          , .CompBudSlsActualMo1 = If(d.CompBudSlsActualMo1.HasValue, d.CompBudSlsActualMo1.Value, 0) _
    '                                          , .CompBudSlsActualMo2 = If(d.CompBudSlsActualMo2.HasValue, d.CompBudSlsActualMo2.Value, 0) _
    '                                          , .CompBudSlsActualMo3 = If(d.CompBudSlsActualMo3.HasValue, d.CompBudSlsActualMo3.Value, 0) _
    '                                          , .CompBudSlsActualMo4 = If(d.CompBudSlsActualMo4.HasValue, d.CompBudSlsActualMo4.Value, 0) _
    '                                          , .CompBudSlsActualMo5 = If(d.CompBudSlsActualMo5.HasValue, d.CompBudSlsActualMo5.Value, 0) _
    '                                          , .CompBudSlsActualMo6 = If(d.CompBudSlsActualMo6.HasValue, d.CompBudSlsActualMo6.Value, 0) _
    '                                          , .CompBudSlsActualMo7 = If(d.CompBudSlsActualMo7.HasValue, d.CompBudSlsActualMo7.Value, 0) _
    '                                          , .CompBudSlsActualMo8 = If(d.CompBudSlsActualMo8.HasValue, d.CompBudSlsActualMo8.Value, 0) _
    '                                          , .CompBudSlsActualMo9 = If(d.CompBudSlsActualMo9.HasValue, d.CompBudSlsActualMo9.Value, 0) _
    '                                          , .CompBudSlsActualMo10 = If(d.CompBudSlsActualMo10.HasValue, d.CompBudSlsActualMo10.Value, 0) _
    '                                          , .CompBudSlsActualMo11 = If(d.CompBudSlsActualMo11.HasValue, d.CompBudSlsActualMo11.Value, 0) _
    '                                          , .CompBudSlsActualMo12 = If(d.CompBudSlsActualMo12.HasValue, d.CompBudSlsActualMo12.Value, 0) _
    '                                          , .CompBudSlsActualMoTotal = If(d.CompBudSlsActualMoTotal.HasValue, d.CompBudSlsActualMoTotal.Value, 0) _
    '                                          , .CompBudSlsMarginBudget = If(d.CompBudSlsMarginBudget.HasValue, d.CompBudSlsMarginBudget.Value, 0) _
    '                                          , .CompBudSlsMarginActual = If(d.CompBudSlsMarginActual.HasValue, d.CompBudSlsMarginActual.Value, 0) _
    '                                          , .CompBudCogsBudgetMo1 = If(d.CompBudCogsBudgetMo1.HasValue, d.CompBudCogsBudgetMo1.Value, 0) _
    '                                          , .CompBudCogsBudgetMo2 = If(d.CompBudCogsBudgetMo2.HasValue, d.CompBudCogsBudgetMo2.Value, 0) _
    '                                          , .CompBudCogsBudgetMo3 = If(d.CompBudCogsBudgetMo3.HasValue, d.CompBudCogsBudgetMo3.Value, 0) _
    '                                          , .CompBudCogsBudgetMo4 = If(d.CompBudCogsBudgetMo4.HasValue, d.CompBudCogsBudgetMo4.Value, 0) _
    '                                          , .CompBudCogsBudgetMo5 = If(d.CompBudCogsBudgetMo5.HasValue, d.CompBudCogsBudgetMo5.Value, 0) _
    '                                          , .CompBudCogsBudgetMo6 = If(d.CompBudCogsBudgetMo6.HasValue, d.CompBudCogsBudgetMo6.Value, 0) _
    '                                          , .CompBudCogsBudgetMo7 = If(d.CompBudCogsBudgetMo7.HasValue, d.CompBudCogsBudgetMo7.Value, 0) _
    '                                          , .CompBudCogsBudgetMo8 = If(d.CompBudCogsBudgetMo8.HasValue, d.CompBudCogsBudgetMo8.Value, 0) _
    '                                          , .CompBudCogsBudgetMo9 = If(d.CompBudCogsBudgetMo9.HasValue, d.CompBudCogsBudgetMo9.Value, 0) _
    '                                          , .CompBudCogsBudgetMo10 = If(d.CompBudCogsBudgetMo10.HasValue, d.CompBudCogsBudgetMo10.Value, 0) _
    '                                          , .CompBudCogsBudgetMo11 = If(d.CompBudCogsBudgetMo11.HasValue, d.CompBudCogsBudgetMo11.Value, 0) _
    '                                          , .CompBudCogsBudgetMo12 = If(d.CompBudCogsBudgetMo12.HasValue, d.CompBudCogsBudgetMo12.Value, 0) _
    '                                          , .CompBudCogsBudgetMoTotal = If(d.CompBudCogsBudgetMoTotal.HasValue, d.CompBudCogsBudgetMoTotal.Value, 0) _
    '                                          , .CompBudCogsActualMo1 = If(d.CompBudCogsActualMo1.HasValue, d.CompBudCogsActualMo1.Value, 0) _
    '                                          , .CompBudCogsActualMo2 = If(d.CompBudCogsActualMo2.HasValue, d.CompBudCogsActualMo2.Value, 0) _
    '                                          , .CompBudCogsActualMo3 = If(d.CompBudCogsActualMo3.HasValue, d.CompBudCogsActualMo3.Value, 0) _
    '                                          , .CompBudCogsActualMo4 = If(d.CompBudCogsActualMo4.HasValue, d.CompBudCogsActualMo4.Value, 0) _
    '                                          , .CompBudCogsActualMo5 = If(d.CompBudCogsActualMo5.HasValue, d.CompBudCogsActualMo5.Value, 0) _
    '                                          , .CompBudCogsActualMo6 = If(d.CompBudCogsActualMo6.HasValue, d.CompBudCogsActualMo6.Value, 0) _
    '                                          , .CompBudCogsActualMo7 = If(d.CompBudCogsActualMo7.HasValue, d.CompBudCogsActualMo7.Value, 0) _
    '                                          , .CompBudCogsActualMo8 = If(d.CompBudCogsActualMo8.HasValue, d.CompBudCogsActualMo8.Value, 0) _
    '                                          , .CompBudCogsActualMo9 = If(d.CompBudCogsActualMo9.HasValue, d.CompBudCogsActualMo9.Value, 0) _
    '                                          , .CompBudCogsActualMo10 = If(d.CompBudCogsActualMo10.HasValue, d.CompBudCogsActualMo10.Value, 0) _
    '                                          , .CompBudCogsActualMo11 = If(d.CompBudCogsActualMo11.HasValue, d.CompBudCogsActualMo11.Value, 0) _
    '                                          , .CompBudCogsActualMo12 = If(d.CompBudCogsActualMo12.HasValue, d.CompBudCogsActualMo12.Value, 0) _
    '                                          , .CompBudCogsActualMoTotal = If(d.CompBudCogsActualMoTotal.HasValue, d.CompBudCogsActualMoTotal.Value, 0) _
    '                                          , .CompBudCogsMarginBudget = If(d.CompBudCogsMarginBudget.HasValue, d.CompBudCogsMarginBudget.Value, 0) _
    '                                          , .CompBudCogsMarginActual = If(d.CompBudCogsMarginActual.HasValue, d.CompBudCogsMarginActual.Value, 0) _
    '                                          , .CompBudProfitBudgetMo1 = If(d.CompBudProfitBudgetMo1.HasValue, d.CompBudProfitBudgetMo1.Value, 0) _
    '                                          , .CompBudProfitBudgetMo2 = If(d.CompBudProfitBudgetMo2.HasValue, d.CompBudProfitBudgetMo2.Value, 0) _
    '                                          , .CompBudProfitBudgetMo3 = If(d.CompBudProfitBudgetMo3.HasValue, d.CompBudProfitBudgetMo3.Value, 0) _
    '                                          , .CompBudProfitBudgetMo4 = If(d.CompBudProfitBudgetMo4.HasValue, d.CompBudProfitBudgetMo4.Value, 0) _
    '                                          , .CompBudProfitBudgetMo5 = If(d.CompBudProfitBudgetMo5.HasValue, d.CompBudProfitBudgetMo5.Value, 0) _
    '                                          , .CompBudProfitBudgetMo6 = If(d.CompBudProfitBudgetMo6.HasValue, d.CompBudProfitBudgetMo6.Value, 0) _
    '                                          , .CompBudProfitBudgetMo7 = If(d.CompBudProfitBudgetMo7.HasValue, d.CompBudProfitBudgetMo7.Value, 0) _
    '                                          , .CompBudProfitBudgetMo8 = If(d.CompBudProfitBudgetMo8.HasValue, d.CompBudProfitBudgetMo8.Value, 0) _
    '                                          , .CompBudProfitBudgetMo9 = If(d.CompBudProfitBudgetMo9.HasValue, d.CompBudProfitBudgetMo9.Value, 0) _
    '                                          , .CompBudProfitBudgetMo10 = If(d.CompBudProfitBudgetMo10.HasValue, d.CompBudProfitBudgetMo10.Value, 0) _
    '                                          , .CompBudProfitBudgetMo11 = If(d.CompBudProfitBudgetMo11.HasValue, d.CompBudProfitBudgetMo11.Value, 0) _
    '                                          , .CompBudProfitBudgetMo12 = If(d.CompBudProfitBudgetMo12.HasValue, d.CompBudProfitBudgetMo12.Value, 0) _
    '                                          , .CompBudProfitBudgetMoTotal = If(d.CompBudProfitBudgetMoTotal.HasValue, d.CompBudProfitBudgetMoTotal.Value, 0) _
    '                                          , .CompBudProfitActualMo1 = If(d.CompBudProfitActualMo1.HasValue, d.CompBudProfitActualMo1.Value, 0) _
    '                                          , .CompBudProfitActualMo2 = If(d.CompBudProfitActualMo2.HasValue, d.CompBudProfitActualMo2.Value, 0) _
    '                                          , .CompBudProfitActualMo3 = If(d.CompBudProfitActualMo3.HasValue, d.CompBudProfitActualMo3.Value, 0) _
    '                                          , .CompBudProfitActualMo4 = If(d.CompBudProfitActualMo4.HasValue, d.CompBudProfitActualMo4.Value, 0) _
    '                                          , .CompBudProfitActualMo5 = If(d.CompBudProfitActualMo5.HasValue, d.CompBudProfitActualMo5.Value, 0) _
    '                                          , .CompBudProfitActualMo6 = If(d.CompBudProfitActualMo6.HasValue, d.CompBudProfitActualMo6.Value, 0) _
    '                                          , .CompBudProfitActualMo7 = If(d.CompBudProfitActualMo7.HasValue, d.CompBudProfitActualMo7.Value, 0) _
    '                                          , .CompBudProfitActualMo8 = If(d.CompBudProfitActualMo8.HasValue, d.CompBudProfitActualMo8.Value, 0) _
    '                                          , .CompBudProfitActualMo9 = If(d.CompBudProfitActualMo9.HasValue, d.CompBudProfitActualMo9.Value, 0) _
    '                                          , .CompBudProfitActualMo10 = If(d.CompBudProfitActualMo10.HasValue, d.CompBudProfitActualMo10.Value, 0) _
    '                                          , .CompBudProfitActualMo11 = If(d.CompBudProfitActualMo11.HasValue, d.CompBudProfitActualMo11.Value, 0) _
    '                                          , .CompBudProfitActualMo12 = If(d.CompBudProfitActualMo12.HasValue, d.CompBudProfitActualMo12.Value, 0) _
    '                                          , .CompBudProfitActualMoTotal = If(d.CompBudProfitActualMoTotal.HasValue, d.CompBudProfitActualMoTotal.Value, 0) _
    '                                          , .CompBudProfitMarginBudget = If(d.CompBudProfitMarginBudget.HasValue, d.CompBudProfitMarginBudget.Value, 0) _
    '                                          , .CompBudProfitMarginActual = If(d.CompBudProfitMarginActual.HasValue, d.CompBudProfitMarginActual.Value, 0) _
    '                                          , .CompFinDuns = d.CompFinDuns _
    '                                          , .CompFinTaxID = d.CompFinTaxID _
    '                                          , .CompFinPaymentForm = d.CompFinPaymentForm _
    '                                          , .CompFinSIC = d.CompFinSIC _
    '                                          , .CompFinPaymentDiscount = If(d.CompFinPaymentDiscount.HasValue, d.CompFinPaymentDiscount.Value, 0) _
    '                                          , .CompFinPaymentDays = If(d.CompFinPaymentDays.HasValue, d.CompFinPaymentDays.Value, 0) _
    '                                          , .CompFinPaymentNetDays = If(d.CompFinPaymentNetDays.HasValue, d.CompFinPaymentNetDays.Value, 0) _
    '                                          , .CompFinCommTerms = d.CompFinCommTerms _
    '                                          , .CompFinCommTermsPer = If(d.CompFinCommTermsPer.HasValue, d.CompFinCommTermsPer.Value, 0) _
    '                                          , .CompFinCommCompControl = d.CompFinCommCompControl _
    '                                          , .CompFinCreditLimit = If(d.CompFinCreditLimit.HasValue, d.CompFinCreditLimit.Value, 0) _
    '                                          , .CompFinCreditUsed = If(d.CompFinCreditUsed.HasValue, d.CompFinCreditUsed.Value, 0) _
    '                                          , .CompFinCreditAvail = If(d.CompFinCreditAvail.HasValue, d.CompFinCreditAvail.Value, 0) _
    '                                          , .CompFinYTDbookedCurr = If(d.CompFinYTDbookedCurr.HasValue, d.CompFinYTDbookedCurr.Value, 0) _
    '                                          , .CompFinYTDbookedLast = If(d.CompFinYTDbookedLast.HasValue, d.CompFinYTDbookedLast.Value, 0) _
    '                                          , .CompFinYTDcarrierCurr = If(d.CompFinYTDcarrierCurr.HasValue, d.CompFinYTDcarrierCurr.Value, 0) _
    '                                          , .CompFinYTDcarrierLast = If(d.CompFinYTDcarrierLast.HasValue, d.CompFinYTDcarrierLast.Value, 0) _
    '                                          , .CompFinYTDsavingsCurr = If(d.CompFinYTDsavingsCurr.HasValue, d.CompFinYTDsavingsCurr.Value, 0) _
    '                                          , .CompFinYTDsavingsLast = If(d.CompFinYTDsavingsLast.HasValue, d.CompFinYTDsavingsLast.Value, 0) _
    '                                          , .CompFinYTDRevenuesCur = If(d.CompFinYTDRevenuesCur.HasValue, d.CompFinYTDRevenuesCur.Value, 0) _
    '                                          , .CompFinYTDRevenuesLast = If(d.CompFinYTDRevenuesLast.HasValue, d.CompFinYTDRevenuesLast.Value, 0) _
    '                                          , .CompFinYTDnoLoadsCurr = If(d.CompFinYTDnoLoadsCurr.HasValue, d.CompFinYTDnoLoadsCurr.Value, 0) _
    '                                          , .CompFinYTDnoLoadsLast = If(d.CompFinYTDnoLoadsLast.HasValue, d.CompFinYTDnoLoadsLast.Value, 0) _
    '                                          , .CompFinInvPrnCode = If(d.CompFinInvPrnCode.HasValue, d.CompFinInvPrnCode.Value, False) _
    '                                          , .CompFinInvEMailCode = If(d.CompFinInvEMailCode.HasValue, d.CompFinInvEMailCode.Value, False) _
    '                                          , .CompFinCurType = If(d.CompFinCurType.HasValue, d.CompFinCurType.Value, 0) _
    '                                          , .CompFinUser1 = If(d.CompFinUser1.HasValue, d.CompFinUser1.Value, 0) _
    '                                          , .CompFinUser2 = If(d.CompFinUser2.HasValue, d.CompFinUser2.Value, 0) _
    '                                          , .CompFinUser3 = If(d.CompFinUser3.HasValue, d.CompFinUser3.Value, 0) _
    '                                          , .CompFinUser4 = d.CompFinUser4 _
    '                                          , .CompFinUser5 = d.CompFinUser5 _
    '                                          , .CompFinCustomerSince = d.CompFinCustomerSince _
    '                                          , .CompFinCardType = d.CompFinCardType _
    '                                          , .CompFinCardName = d.CompFinCardName _
    '                                          , .CompFinCardExpires = d.CompFinCardExpires _
    '                                          , .CompFinCardAuthorizor = d.CompFinCardAuthorizor _
    '                                          , .CompFinCardAuthPassword = d.CompFinCardAuthPassword _
    '                                          , .CompFinUseImportFrtCost = If(d.CompFinUseImportFrtCost.HasValue, d.CompFinUseImportFrtCost.Value, False) _
    '                                          , .CompFinBkhlFlatFee = If(d.CompFinBkhlFlatFee.HasValue, d.CompFinBkhlFlatFee.Value, 0) _
    '                                          , .CompFinBkhlCostPerc = If(d.CompFinBkhlCostPerc.HasValue, d.CompFinBkhlCostPerc.Value, 0) _
    '                                          , .CompLatitude = If(d.CompLatitude.HasValue, d.CompLatitude.Value, 0) _
    '                                          , .CompLongitude = If(d.CompLongitude.HasValue, d.CompLongitude.Value, 0) _
    '                                          , .CompMailTo = d.CompMailTo _
    '                                          , .CompFDAShipID = d.CompFDAShipID _
    '                                          , .CompAMS = If(d.CompAMS.HasValue, d.CompAMS.Value, False) _
    '                                          , .CompPayTolPerLo = If(d.CompPayTolPerLo.HasValue, d.CompPayTolPerLo.Value, 0) _
    '                                          , .CompPayTolPerHi = If(d.CompPayTolPerHi.HasValue, d.CompPayTolPerHi.Value, 0) _
    '                                          , .CompPayTolCurLo = If(d.CompPayTolCurLo.HasValue, d.CompPayTolCurLo.Value, 0) _
    '                                          , .CompPayTolCurHi = If(d.CompPayTolCurHi.HasValue, d.CompPayTolCurHi.Value, 0) _
    '                                          , .CompPayTolWgtFrom = If(d.CompPayTolWgtFrom.HasValue, d.CompPayTolWgtFrom.Value, 0) _
    '                                          , .CompPayTolWgtTo = If(d.CompPayTolWgtTo.HasValue, d.CompPayTolWgtTo.Value, 0) _
    '                                          , .CompPayTolTaxPerLo = If(d.CompPayTolTaxPerLo.HasValue, d.CompPayTolTaxPerLo.Value, 0) _
    '                                          , .CompPayTolTaxPerHi = If(d.CompPayTolTaxPerHi.HasValue, d.CompPayTolTaxPerHi.Value, 0) _
    '                                          , .CompFinBillToCompControl = If(d.CompFinBillToCompControl.HasValue, d.CompFinBillToCompControl.Value, 0) _
    '                                          , .CompSilentTender = d.CompSilentTender _
    '                                          , .CompAMSActive = d.CompAMSActive}
    'End Function

    'Modified by LVV 10/25/16 for v-7.0.5.110 Lane Default Carrier Enhancements

    ''' <summary>
    ''' selectDTOData
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="addContacts"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 4/22/20 Added CompAMSApptDetFieldsVisible
    ''' Modified by by RHR for v-8.5.3.006 on 12/01/2022 added CompWillLoadOnSunday and  CompWillLoadOnSaturday
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.Comp, ByVal db As NGLMASCompDataContext, Optional ByVal addContacts As Boolean = False) As DTO.Comp

        Dim oData As New DTO.Comp With {.CompControl = d.CompControl _
                                              , .CompNumber = If(d.CompNumber.HasValue, d.CompNumber.Value, 0) _
                                              , .CompName = d.CompName _
                                              , .CompNatNumber = If(d.CompNatNumber.HasValue, d.CompNatNumber.Value, 0) _
                                              , .CompNatName = d.CompNatName _
                                              , .CompStreetAddress1 = d.CompStreetAddress1 _
                                              , .CompStreetAddress2 = d.CompStreetAddress2 _
                                              , .CompStreetAddress3 = d.CompStreetAddress3 _
                                              , .CompStreetCity = d.CompStreetCity _
                                              , .CompStreetState = d.CompStreetState _
                                              , .CompStreetCountry = d.CompStreetCountry _
                                              , .CompStreetZip = d.CompStreetZip _
                                              , .CompMailAddress1 = d.CompMailAddress1 _
                                              , .CompMailAddress2 = d.CompMailAddress2 _
                                              , .CompMailAddress3 = d.CompMailAddress3 _
                                              , .CompMailCity = d.CompMailCity _
                                              , .CompMailState = d.CompMailState _
                                              , .CompMailCountry = d.CompMailCountry _
                                              , .CompMailZip = d.CompMailZip _
                                              , .CompWeb = d.CompWeb _
                                              , .CompEmail = d.CompEmail _
                                              , .CompModDate = d.CompModDate _
                                              , .CompModUser = d.CompModUser _
                                              , .CompDirections = d.CompDirections _
                                              , .CompAbrev = d.CompAbrev _
                                              , .CompActive = d.CompActive _
                                              , .CompNEXTrack = d.CompNEXTrack _
                                              , .CompNEXTStopAcctNo = d.CompNEXTStopAcctNo _
                                              , .CompNEXTStopPsw = d.CompNEXTStopPsw _
                                              , .CompNextstopSubmitRFP = d.CompNextstopSubmitRFP _
                                              , .CompTypeCategory = If(d.CompTypeCategory.HasValue, d.CompTypeCategory.Value, 0) _
                                              , .CompFAAShipID = d.CompFAAShipID _
                                              , .CompFAAShipDate = d.CompFAAShipDate _
                                              , .CompUpdated = d.CompUpdated.ToArray() _
                                              , .CompBudSeasDescription = d.CompBudSeasDescription _
                                              , .CompBudSeasMo1 = If(d.CompBudSeasMo1.HasValue, d.CompBudSeasMo1.Value, 0) _
                                              , .CompBudSeasMo2 = If(d.CompBudSeasMo2.HasValue, d.CompBudSeasMo2.Value, 0) _
                                              , .CompBudSeasMo3 = If(d.CompBudSeasMo3.HasValue, d.CompBudSeasMo3.Value, 0) _
                                              , .CompBudSeasMo4 = If(d.CompBudSeasMo4.HasValue, d.CompBudSeasMo4.Value, 0) _
                                              , .CompBudSeasMo5 = If(d.CompBudSeasMo5.HasValue, d.CompBudSeasMo5.Value, 0) _
                                              , .CompBudSeasMo6 = If(d.CompBudSeasMo6.HasValue, d.CompBudSeasMo6.Value, 0) _
                                              , .CompBudSeasMo7 = If(d.CompBudSeasMo7.HasValue, d.CompBudSeasMo7.Value, 0) _
                                              , .CompBudSeasMo8 = If(d.CompBudSeasMo8.HasValue, d.CompBudSeasMo8.Value, 0) _
                                              , .CompBudSeasMo9 = If(d.CompBudSeasMo9.HasValue, d.CompBudSeasMo9.Value, 0) _
                                              , .CompBudSeasMo10 = If(d.CompBudSeasMo10.HasValue, d.CompBudSeasMo10.Value, 0) _
                                              , .CompBudSeasMo11 = If(d.CompBudSeasMo11.HasValue, d.CompBudSeasMo11.Value, 0) _
                                              , .CompBudSeasMo12 = If(d.CompBudSeasMo12.HasValue, d.CompBudSeasMo12.Value, 0) _
                                              , .CompBudSlsBudgetMo1 = If(d.CompBudSlsBudgetMo1.HasValue, d.CompBudSlsBudgetMo1.Value, 0) _
                                              , .CompBudSlsBudgetMo2 = If(d.CompBudSlsBudgetMo2.HasValue, d.CompBudSlsBudgetMo2.Value, 0) _
                                              , .CompBudSlsBudgetMo3 = If(d.CompBudSlsBudgetMo3.HasValue, d.CompBudSlsBudgetMo3.Value, 0) _
                                              , .CompBudSlsBudgetMo4 = If(d.CompBudSlsBudgetMo4.HasValue, d.CompBudSlsBudgetMo4.Value, 0) _
                                              , .CompBudSlsBudgetMo5 = If(d.CompBudSlsBudgetMo5.HasValue, d.CompBudSlsBudgetMo5.Value, 0) _
                                              , .CompBudSlsBudgetMo6 = If(d.CompBudSlsBudgetMo6.HasValue, d.CompBudSlsBudgetMo6.Value, 0) _
                                              , .CompBudSlsBudgetMo7 = If(d.CompBudSlsBudgetMo7.HasValue, d.CompBudSlsBudgetMo7.Value, 0) _
                                              , .CompBudSlsBudgetMo8 = If(d.CompBudSlsBudgetMo8.HasValue, d.CompBudSlsBudgetMo8.Value, 0) _
                                              , .CompBudSlsBudgetMo9 = If(d.CompBudSlsBudgetMo9.HasValue, d.CompBudSlsBudgetMo9.Value, 0) _
                                              , .CompBudSlsBudgetMo10 = If(d.CompBudSlsBudgetMo10.HasValue, d.CompBudSlsBudgetMo10.Value, 0) _
                                              , .CompBudSlsBudgetMo11 = If(d.CompBudSlsBudgetMo11.HasValue, d.CompBudSlsBudgetMo11.Value, 0) _
                                              , .CompBudSlsBudgetMo12 = If(d.CompBudSlsBudgetMo12.HasValue, d.CompBudSlsBudgetMo12.Value, 0) _
                                              , .CompBudSlsBudgetMoTotal = If(d.CompBudSlsBudgetMoTotal.HasValue, d.CompBudSlsBudgetMoTotal.Value, 0) _
                                              , .CompBudSlsActualMo1 = If(d.CompBudSlsActualMo1.HasValue, d.CompBudSlsActualMo1.Value, 0) _
                                              , .CompBudSlsActualMo2 = If(d.CompBudSlsActualMo2.HasValue, d.CompBudSlsActualMo2.Value, 0) _
                                              , .CompBudSlsActualMo3 = If(d.CompBudSlsActualMo3.HasValue, d.CompBudSlsActualMo3.Value, 0) _
                                              , .CompBudSlsActualMo4 = If(d.CompBudSlsActualMo4.HasValue, d.CompBudSlsActualMo4.Value, 0) _
                                              , .CompBudSlsActualMo5 = If(d.CompBudSlsActualMo5.HasValue, d.CompBudSlsActualMo5.Value, 0) _
                                              , .CompBudSlsActualMo6 = If(d.CompBudSlsActualMo6.HasValue, d.CompBudSlsActualMo6.Value, 0) _
                                              , .CompBudSlsActualMo7 = If(d.CompBudSlsActualMo7.HasValue, d.CompBudSlsActualMo7.Value, 0) _
                                              , .CompBudSlsActualMo8 = If(d.CompBudSlsActualMo8.HasValue, d.CompBudSlsActualMo8.Value, 0) _
                                              , .CompBudSlsActualMo9 = If(d.CompBudSlsActualMo9.HasValue, d.CompBudSlsActualMo9.Value, 0) _
                                              , .CompBudSlsActualMo10 = If(d.CompBudSlsActualMo10.HasValue, d.CompBudSlsActualMo10.Value, 0) _
                                              , .CompBudSlsActualMo11 = If(d.CompBudSlsActualMo11.HasValue, d.CompBudSlsActualMo11.Value, 0) _
                                              , .CompBudSlsActualMo12 = If(d.CompBudSlsActualMo12.HasValue, d.CompBudSlsActualMo12.Value, 0) _
                                              , .CompBudSlsActualMoTotal = If(d.CompBudSlsActualMoTotal.HasValue, d.CompBudSlsActualMoTotal.Value, 0) _
                                              , .CompBudSlsMarginBudget = If(d.CompBudSlsMarginBudget.HasValue, d.CompBudSlsMarginBudget.Value, 0) _
                                              , .CompBudSlsMarginActual = If(d.CompBudSlsMarginActual.HasValue, d.CompBudSlsMarginActual.Value, 0) _
                                              , .CompBudCogsBudgetMo1 = If(d.CompBudCogsBudgetMo1.HasValue, d.CompBudCogsBudgetMo1.Value, 0) _
                                              , .CompBudCogsBudgetMo2 = If(d.CompBudCogsBudgetMo2.HasValue, d.CompBudCogsBudgetMo2.Value, 0) _
                                              , .CompBudCogsBudgetMo3 = If(d.CompBudCogsBudgetMo3.HasValue, d.CompBudCogsBudgetMo3.Value, 0) _
                                              , .CompBudCogsBudgetMo4 = If(d.CompBudCogsBudgetMo4.HasValue, d.CompBudCogsBudgetMo4.Value, 0) _
                                              , .CompBudCogsBudgetMo5 = If(d.CompBudCogsBudgetMo5.HasValue, d.CompBudCogsBudgetMo5.Value, 0) _
                                              , .CompBudCogsBudgetMo6 = If(d.CompBudCogsBudgetMo6.HasValue, d.CompBudCogsBudgetMo6.Value, 0) _
                                              , .CompBudCogsBudgetMo7 = If(d.CompBudCogsBudgetMo7.HasValue, d.CompBudCogsBudgetMo7.Value, 0) _
                                              , .CompBudCogsBudgetMo8 = If(d.CompBudCogsBudgetMo8.HasValue, d.CompBudCogsBudgetMo8.Value, 0) _
                                              , .CompBudCogsBudgetMo9 = If(d.CompBudCogsBudgetMo9.HasValue, d.CompBudCogsBudgetMo9.Value, 0) _
                                              , .CompBudCogsBudgetMo10 = If(d.CompBudCogsBudgetMo10.HasValue, d.CompBudCogsBudgetMo10.Value, 0) _
                                              , .CompBudCogsBudgetMo11 = If(d.CompBudCogsBudgetMo11.HasValue, d.CompBudCogsBudgetMo11.Value, 0) _
                                              , .CompBudCogsBudgetMo12 = If(d.CompBudCogsBudgetMo12.HasValue, d.CompBudCogsBudgetMo12.Value, 0) _
                                              , .CompBudCogsBudgetMoTotal = If(d.CompBudCogsBudgetMoTotal.HasValue, d.CompBudCogsBudgetMoTotal.Value, 0) _
                                              , .CompBudCogsActualMo1 = If(d.CompBudCogsActualMo1.HasValue, d.CompBudCogsActualMo1.Value, 0) _
                                              , .CompBudCogsActualMo2 = If(d.CompBudCogsActualMo2.HasValue, d.CompBudCogsActualMo2.Value, 0) _
                                              , .CompBudCogsActualMo3 = If(d.CompBudCogsActualMo3.HasValue, d.CompBudCogsActualMo3.Value, 0) _
                                              , .CompBudCogsActualMo4 = If(d.CompBudCogsActualMo4.HasValue, d.CompBudCogsActualMo4.Value, 0) _
                                              , .CompBudCogsActualMo5 = If(d.CompBudCogsActualMo5.HasValue, d.CompBudCogsActualMo5.Value, 0) _
                                              , .CompBudCogsActualMo6 = If(d.CompBudCogsActualMo6.HasValue, d.CompBudCogsActualMo6.Value, 0) _
                                              , .CompBudCogsActualMo7 = If(d.CompBudCogsActualMo7.HasValue, d.CompBudCogsActualMo7.Value, 0) _
                                              , .CompBudCogsActualMo8 = If(d.CompBudCogsActualMo8.HasValue, d.CompBudCogsActualMo8.Value, 0) _
                                              , .CompBudCogsActualMo9 = If(d.CompBudCogsActualMo9.HasValue, d.CompBudCogsActualMo9.Value, 0) _
                                              , .CompBudCogsActualMo10 = If(d.CompBudCogsActualMo10.HasValue, d.CompBudCogsActualMo10.Value, 0) _
                                              , .CompBudCogsActualMo11 = If(d.CompBudCogsActualMo11.HasValue, d.CompBudCogsActualMo11.Value, 0) _
                                              , .CompBudCogsActualMo12 = If(d.CompBudCogsActualMo12.HasValue, d.CompBudCogsActualMo12.Value, 0) _
                                              , .CompBudCogsActualMoTotal = If(d.CompBudCogsActualMoTotal.HasValue, d.CompBudCogsActualMoTotal.Value, 0) _
                                              , .CompBudCogsMarginBudget = If(d.CompBudCogsMarginBudget.HasValue, d.CompBudCogsMarginBudget.Value, 0) _
                                              , .CompBudCogsMarginActual = If(d.CompBudCogsMarginActual.HasValue, d.CompBudCogsMarginActual.Value, 0) _
                                              , .CompBudProfitBudgetMo1 = If(d.CompBudProfitBudgetMo1.HasValue, d.CompBudProfitBudgetMo1.Value, 0) _
                                              , .CompBudProfitBudgetMo2 = If(d.CompBudProfitBudgetMo2.HasValue, d.CompBudProfitBudgetMo2.Value, 0) _
                                              , .CompBudProfitBudgetMo3 = If(d.CompBudProfitBudgetMo3.HasValue, d.CompBudProfitBudgetMo3.Value, 0) _
                                              , .CompBudProfitBudgetMo4 = If(d.CompBudProfitBudgetMo4.HasValue, d.CompBudProfitBudgetMo4.Value, 0) _
                                              , .CompBudProfitBudgetMo5 = If(d.CompBudProfitBudgetMo5.HasValue, d.CompBudProfitBudgetMo5.Value, 0) _
                                              , .CompBudProfitBudgetMo6 = If(d.CompBudProfitBudgetMo6.HasValue, d.CompBudProfitBudgetMo6.Value, 0) _
                                              , .CompBudProfitBudgetMo7 = If(d.CompBudProfitBudgetMo7.HasValue, d.CompBudProfitBudgetMo7.Value, 0) _
                                              , .CompBudProfitBudgetMo8 = If(d.CompBudProfitBudgetMo8.HasValue, d.CompBudProfitBudgetMo8.Value, 0) _
                                              , .CompBudProfitBudgetMo9 = If(d.CompBudProfitBudgetMo9.HasValue, d.CompBudProfitBudgetMo9.Value, 0) _
                                              , .CompBudProfitBudgetMo10 = If(d.CompBudProfitBudgetMo10.HasValue, d.CompBudProfitBudgetMo10.Value, 0) _
                                              , .CompBudProfitBudgetMo11 = If(d.CompBudProfitBudgetMo11.HasValue, d.CompBudProfitBudgetMo11.Value, 0) _
                                              , .CompBudProfitBudgetMo12 = If(d.CompBudProfitBudgetMo12.HasValue, d.CompBudProfitBudgetMo12.Value, 0) _
                                              , .CompBudProfitBudgetMoTotal = If(d.CompBudProfitBudgetMoTotal.HasValue, d.CompBudProfitBudgetMoTotal.Value, 0) _
                                              , .CompBudProfitActualMo1 = If(d.CompBudProfitActualMo1.HasValue, d.CompBudProfitActualMo1.Value, 0) _
                                              , .CompBudProfitActualMo2 = If(d.CompBudProfitActualMo2.HasValue, d.CompBudProfitActualMo2.Value, 0) _
                                              , .CompBudProfitActualMo3 = If(d.CompBudProfitActualMo3.HasValue, d.CompBudProfitActualMo3.Value, 0) _
                                              , .CompBudProfitActualMo4 = If(d.CompBudProfitActualMo4.HasValue, d.CompBudProfitActualMo4.Value, 0) _
                                              , .CompBudProfitActualMo5 = If(d.CompBudProfitActualMo5.HasValue, d.CompBudProfitActualMo5.Value, 0) _
                                              , .CompBudProfitActualMo6 = If(d.CompBudProfitActualMo6.HasValue, d.CompBudProfitActualMo6.Value, 0) _
                                              , .CompBudProfitActualMo7 = If(d.CompBudProfitActualMo7.HasValue, d.CompBudProfitActualMo7.Value, 0) _
                                              , .CompBudProfitActualMo8 = If(d.CompBudProfitActualMo8.HasValue, d.CompBudProfitActualMo8.Value, 0) _
                                              , .CompBudProfitActualMo9 = If(d.CompBudProfitActualMo9.HasValue, d.CompBudProfitActualMo9.Value, 0) _
                                              , .CompBudProfitActualMo10 = If(d.CompBudProfitActualMo10.HasValue, d.CompBudProfitActualMo10.Value, 0) _
                                              , .CompBudProfitActualMo11 = If(d.CompBudProfitActualMo11.HasValue, d.CompBudProfitActualMo11.Value, 0) _
                                              , .CompBudProfitActualMo12 = If(d.CompBudProfitActualMo12.HasValue, d.CompBudProfitActualMo12.Value, 0) _
                                              , .CompBudProfitActualMoTotal = If(d.CompBudProfitActualMoTotal.HasValue, d.CompBudProfitActualMoTotal.Value, 0) _
                                              , .CompBudProfitMarginBudget = If(d.CompBudProfitMarginBudget.HasValue, d.CompBudProfitMarginBudget.Value, 0) _
                                              , .CompBudProfitMarginActual = If(d.CompBudProfitMarginActual.HasValue, d.CompBudProfitMarginActual.Value, 0) _
                                              , .CompFinDuns = d.CompFinDuns _
                                              , .CompFinTaxID = d.CompFinTaxID _
                                              , .CompFinPaymentForm = d.CompFinPaymentForm _
                                              , .CompFinSIC = d.CompFinSIC _
                                              , .CompFinPaymentDiscount = If(d.CompFinPaymentDiscount.HasValue, d.CompFinPaymentDiscount.Value, 0) _
                                              , .CompFinPaymentDays = If(d.CompFinPaymentDays.HasValue, d.CompFinPaymentDays.Value, 0) _
                                              , .CompFinPaymentNetDays = If(d.CompFinPaymentNetDays.HasValue, d.CompFinPaymentNetDays.Value, 0) _
                                              , .CompFinCommTerms = d.CompFinCommTerms _
                                              , .CompFinCommTermsPer = If(d.CompFinCommTermsPer.HasValue, d.CompFinCommTermsPer.Value, 0) _
                                              , .CompFinCommCompControl = d.CompFinCommCompControl _
                                              , .CompFinCreditLimit = If(d.CompFinCreditLimit.HasValue, d.CompFinCreditLimit.Value, 0) _
                                              , .CompFinCreditUsed = If(d.CompFinCreditUsed.HasValue, d.CompFinCreditUsed.Value, 0) _
                                              , .CompFinCreditAvail = If(d.CompFinCreditAvail.HasValue, d.CompFinCreditAvail.Value, 0) _
                                              , .CompFinYTDbookedCurr = If(d.CompFinYTDbookedCurr.HasValue, d.CompFinYTDbookedCurr.Value, 0) _
                                              , .CompFinYTDbookedLast = If(d.CompFinYTDbookedLast.HasValue, d.CompFinYTDbookedLast.Value, 0) _
                                              , .CompFinYTDcarrierCurr = If(d.CompFinYTDcarrierCurr.HasValue, d.CompFinYTDcarrierCurr.Value, 0) _
                                              , .CompFinYTDcarrierLast = If(d.CompFinYTDcarrierLast.HasValue, d.CompFinYTDcarrierLast.Value, 0) _
                                              , .CompFinYTDsavingsCurr = If(d.CompFinYTDsavingsCurr.HasValue, d.CompFinYTDsavingsCurr.Value, 0) _
                                              , .CompFinYTDsavingsLast = If(d.CompFinYTDsavingsLast.HasValue, d.CompFinYTDsavingsLast.Value, 0) _
                                              , .CompFinYTDRevenuesCur = If(d.CompFinYTDRevenuesCur.HasValue, d.CompFinYTDRevenuesCur.Value, 0) _
                                              , .CompFinYTDRevenuesLast = If(d.CompFinYTDRevenuesLast.HasValue, d.CompFinYTDRevenuesLast.Value, 0) _
                                              , .CompFinYTDnoLoadsCurr = If(d.CompFinYTDnoLoadsCurr.HasValue, d.CompFinYTDnoLoadsCurr.Value, 0) _
                                              , .CompFinYTDnoLoadsLast = If(d.CompFinYTDnoLoadsLast.HasValue, d.CompFinYTDnoLoadsLast.Value, 0) _
                                              , .CompFinInvPrnCode = If(d.CompFinInvPrnCode.HasValue, d.CompFinInvPrnCode.Value, False) _
                                              , .CompFinInvEMailCode = If(d.CompFinInvEMailCode.HasValue, d.CompFinInvEMailCode.Value, False) _
                                              , .CompFinCurType = If(d.CompFinCurType.HasValue, d.CompFinCurType.Value, 0) _
                                              , .CompFinUser1 = If(d.CompFinUser1.HasValue, d.CompFinUser1.Value, 0) _
                                              , .CompFinUser2 = If(d.CompFinUser2.HasValue, d.CompFinUser2.Value, 0) _
                                              , .CompFinUser3 = If(d.CompFinUser3.HasValue, d.CompFinUser3.Value, 0) _
                                              , .CompFinUser4 = d.CompFinUser4 _
                                              , .CompFinUser5 = d.CompFinUser5 _
                                              , .CompFinCustomerSince = d.CompFinCustomerSince _
                                              , .CompFinCardType = d.CompFinCardType _
                                              , .CompFinCardName = d.CompFinCardName _
                                              , .CompFinCardExpires = d.CompFinCardExpires _
                                              , .CompFinCardAuthorizor = d.CompFinCardAuthorizor _
                                              , .CompFinCardAuthPassword = d.CompFinCardAuthPassword _
                                              , .CompFinUseImportFrtCost = If(d.CompFinUseImportFrtCost.HasValue, d.CompFinUseImportFrtCost.Value, False) _
                                              , .CompFinBkhlFlatFee = If(d.CompFinBkhlFlatFee.HasValue, d.CompFinBkhlFlatFee.Value, 0) _
                                              , .CompFinBkhlCostPerc = If(d.CompFinBkhlCostPerc.HasValue, d.CompFinBkhlCostPerc.Value, 0) _
                                              , .CompLatitude = If(d.CompLatitude.HasValue, d.CompLatitude.Value, 0) _
                                              , .CompLongitude = If(d.CompLongitude.HasValue, d.CompLongitude.Value, 0) _
                                              , .CompMailTo = d.CompMailTo _
                                              , .CompFDAShipID = d.CompFDAShipID _
                                              , .CompAMS = If(d.CompAMS.HasValue, d.CompAMS.Value, False) _
                                              , .CompPayTolPerLo = If(d.CompPayTolPerLo.HasValue, d.CompPayTolPerLo.Value, 0) _
                                              , .CompPayTolPerHi = If(d.CompPayTolPerHi.HasValue, d.CompPayTolPerHi.Value, 0) _
                                              , .CompPayTolCurLo = If(d.CompPayTolCurLo.HasValue, d.CompPayTolCurLo.Value, 0) _
                                              , .CompPayTolCurHi = If(d.CompPayTolCurHi.HasValue, d.CompPayTolCurHi.Value, 0) _
                                              , .CompPayTolWgtFrom = If(d.CompPayTolWgtFrom.HasValue, d.CompPayTolWgtFrom.Value, 0) _
                                              , .CompPayTolWgtTo = If(d.CompPayTolWgtTo.HasValue, d.CompPayTolWgtTo.Value, 0) _
                                              , .CompPayTolTaxPerLo = If(d.CompPayTolTaxPerLo.HasValue, d.CompPayTolTaxPerLo.Value, 0) _
                                              , .CompPayTolTaxPerHi = If(d.CompPayTolTaxPerHi.HasValue, d.CompPayTolTaxPerHi.Value, 0) _
                                              , .CompFinBillToCompControl = If(d.CompFinBillToCompControl.HasValue, d.CompFinBillToCompControl.Value, 0) _
                                              , .CompSilentTender = d.CompSilentTender _
                                              , .CompTimeZone = d.CompTimeZone _
                                              , .CompRailStationName = d.CompRailStationName _
                                              , .CompRailSPLC = d.CompRailSPLC _
                                              , .CompRailFSAC = d.CompRailFSAC _
                                              , .CompRail333 = d.CompRail333 _
                                              , .CompRailR260 = d.CompRailR260 _
                                              , .CompIsTransLoad = d.CompIsTransLoad _
                                              , .CompUser1 = d.CompUser1 _
                                              , .CompUser2 = d.CompUser2 _
                                              , .CompUser3 = d.CompUser3 _
                                              , .CompUser4 = d.CompUser4 _
                                              , .CompAlphaCode = d.CompAlphaCode _
                                              , .CompLegalEntity = d.CompLegalEntity _
                                              , .CompFinFBToleranceHigh = If(d.CompFinFBToleranceHigh.HasValue, d.CompFinFBToleranceHigh.Value, 0) _
                                              , .CompFinFBToleranceLow = If(d.CompFinFBToleranceLow.HasValue, d.CompFinFBToleranceLow.Value, 0) _
                                              , .CompRestrictCarrierSelection = d.CompRestrictCarrierSelection _
                                              , .CompWarnOnRestrictedCarrierSelection = d.CompWarnOnRestrictedCarrierSelection _
                                              , .CompNotifyEmail = d.CompNotifyEmail _
                                              , .CompPhone = d.CompPhone _
                                              , .CompFax = d.CompFax _
                                                , .CompCarrierLoadAcceptanceAllowedMinutes = If(d.CompCarrierLoadAcceptanceAllowedMinutes, 0) _
                                                , .CompRejectedLoadsTo = d.CompRejectedLoadsTo _
                                                , .CompRejectedLoadsCc = d.CompRejectedLoadsCc _
                                                , .CompExpiredLoadsTo = d.CompExpiredLoadsTo _
                                                , .CompExpiredLoadsCc = d.CompExpiredLoadsCc _
                                                , .CompAcceptedLoadsTo = d.CompAcceptedLoadsTo _
                                                , .CompAcceptedLoadsCc = d.CompAcceptedLoadsCc _
                                                , .CompHeaderLogo = d.CompHeaderLogo _
                                                , .CompHeaderLogoLink = d.CompHeaderLogoLink _
                                                , .CompAMSApptDetFieldsVisible = d.CompAMSApptDetFieldsVisible _
                                                , .CompWillLoadOnSunday = d.CompWillLoadOnSunday _
                                                , .CompWillLoadOnSaturday = d.CompWillLoadOnSaturday}




        If addContacts Then
            oData.CompConts = (From t In d.CompConts Select selectDTOContData(t, db)).ToList()
        End If

        Return oData
    End Function

    Friend Function selectDTOContData(ByVal d As LTS.CompCont, ByVal db As NGLMASCompDataContext) As DTO.CompCont
        Return New DTO.CompCont With {.CompContControl = d.CompContControl _
                                                        , .CompContCompControl = d.CompContCompControl _
                                                        , .CompContName = d.CompContName _
                                                        , .CompContTitle = d.CompContTitle _
                                                        , .CompCont800 = d.CompCont800 _
                                                        , .CompContPhone = d.CompContPhone _
                                                        , .CompContPhoneExt = d.CompContPhoneExt _
                                                        , .CompContFax = d.CompContFax _
                                                        , .CompContEmail = d.CompContEmail _
                                                        , .CompContTender = d.CompContTender _
                                                        , .CompContUpdated = d.CompContUpdated.ToArray()}
    End Function

#End Region

#Region "Scrub Methods"


    Public Shared Function getAKAByIndex(ByVal oAKA As LTS.AKARefComp, ByVal index As Integer) As String
        Dim strRet As String = ""
        Try
            Select Case index
                Case 1
                    strRet = Trim(oAKA.AKA1.ToString)
                Case 2
                    strRet = Trim(oAKA.AKA2.ToString)
                Case 3
                    strRet = Trim(oAKA.AKA3.ToString)
                Case 4
                    strRet = Trim(oAKA.AKA4.ToString)
                Case 5
                    strRet = Trim(oAKA.AKA5.ToString)
                Case 6
                    strRet = Trim(oAKA.AKA6.ToString)
                Case 7
                    strRet = Trim(oAKA.AKA7.ToString)
                Case 8
                    strRet = Trim(oAKA.AKA8.ToString)
                Case 9
                    strRet = Trim(oAKA.AKA9)
                Case 10
                    strRet = Trim(oAKA.AKA10)
                Case Else
                    strRet = ""
            End Select
        Catch ex As Exception
            'do nothing
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iCompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where X.CompActive = 1 converts to negative 1 instead of true
    '''     we now use X.CompActive = True
    ''' </remarks>
    Public Function Scrub(Optional ByVal iCompControl As Integer = 0) As Integer
        Dim iProcessed As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                Dim dblCaseType As Double = 0
                Dim oComps = db.Comps.Where(Function(X) X.CompActive = True And (iCompControl = 0 OrElse X.CompControl = iCompControl)).ToArray()
                If Not oComps Is Nothing AndAlso oComps.Count() > 0 Then
                    Dim oAKAList = db.AKARefComps.ToArray()
                    If Not oAKAList Is Nothing AndAlso oAKAList.Count() > 0 Then
                        iProcessed = oComps.Count()
                        For Each oAKA In oAKAList
                            Dim strValue As String = oAKA.AKAValue.ToString
                            Dim arrAKAs As String() = {Trim(oAKA.AKA1),
                                Trim(oAKA.AKA2),
                                Trim(oAKA.AKA3),
                                Trim(oAKA.AKA4),
                                Trim(oAKA.AKA5),
                                Trim(oAKA.AKA6),
                                Trim(oAKA.AKA7),
                                Trim(oAKA.AKA8),
                                Trim(oAKA.AKA9),
                                Trim(oAKA.AKA10)}
                            For i As Integer = 0 To 9
                                Dim strAKA As String = arrAKAs(i)
                                If strAKA.Length > 0 Then
                                    For Each oComp In oComps
                                        dblCaseType = GetParValue("GlobalAddressScrubbingCapitalCaseType", oComp.CompControl)
                                        ScrubComp(strAKA, strValue, oComp, dblCaseType)
                                    Next
                                End If
                            Next
                        Next
                    End If
                End If
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Scrub"), db)
            End Try
        End Using
        Return iProcessed
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iCompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where X.CompActive = 1 converts to negative 1 instead of true
    '''     we now use X.CompActive = True
    ''' </remarks>
    Public Function CaseType(Optional ByVal iCompControl As Integer = 0) As Integer
        Dim iProcessed As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim dblCaseType As Double = 0
                Dim oComps = db.Comps.Where(Function(X) X.CompActive = True And (iCompControl = 0 OrElse X.CompControl = iCompControl)).ToArray()
                If Not oComps Is Nothing AndAlso oComps.Count() > 0 Then
                    iProcessed = oComps.Count()
                    For Each oComp In oComps
                        dblCaseType = GetParValue("GlobalAddressScrubbingCapitalCaseType", oComp.CompControl)
                        Select Case dblCaseType
                            Case 0
                                oComp.CompStreetAddress1 = Utilities.UpperFirst(oComp.CompStreetAddress1)
                                oComp.CompStreetAddress2 = Utilities.UpperFirst(oComp.CompStreetAddress2)
                                oComp.CompStreetAddress3 = Utilities.UpperFirst(oComp.CompStreetAddress3)
                                oComp.CompStreetCity = Utilities.UpperFirst(oComp.CompStreetCity)
                                oComp.CompStreetState = oComp.CompStreetState.ToUpper
                                oComp.CompStreetCountry = oComp.CompStreetCountry.ToUpper
                                oComp.CompStreetZip = oComp.CompStreetZip.ToUpper
                                oComp.CompMailAddress1 = Utilities.UpperFirst(oComp.CompMailAddress1)
                                oComp.CompMailAddress2 = Utilities.UpperFirst(oComp.CompMailAddress2)
                                oComp.CompMailAddress3 = Utilities.UpperFirst(oComp.CompMailAddress3)
                                oComp.CompMailCity = Utilities.UpperFirst(oComp.CompMailCity)
                                oComp.CompMailState = oComp.CompMailState.ToUpper
                                oComp.CompMailCountry = oComp.CompMailCountry.ToUpper
                                oComp.CompMailZip = oComp.CompMailZip.ToUpper
                            Case 1
                                oComp.CompStreetAddress1 = oComp.CompStreetAddress1.ToUpper
                                oComp.CompStreetAddress2 = oComp.CompStreetAddress2.ToUpper
                                oComp.CompStreetAddress3 = oComp.CompStreetAddress3.ToUpper
                                oComp.CompStreetCity = oComp.CompStreetCity.ToUpper
                                oComp.CompStreetState = oComp.CompStreetState.ToUpper
                                oComp.CompStreetCountry = oComp.CompStreetCountry.ToUpper
                                oComp.CompStreetZip = oComp.CompStreetZip.ToUpper
                                oComp.CompMailAddress1 = oComp.CompMailAddress1.ToUpper
                                oComp.CompMailAddress2 = oComp.CompMailAddress2.ToUpper
                                oComp.CompMailAddress3 = oComp.CompMailAddress3.ToUpper
                                oComp.CompMailCity = oComp.CompMailCity.ToUpper
                                oComp.CompMailState = oComp.CompMailState.ToUpper
                                oComp.CompMailCountry = oComp.CompMailCountry.ToUpper
                                oComp.CompMailZip = oComp.CompMailZip.ToUpper
                            Case Else
                                oComp.CompStreetAddress1 = oComp.CompStreetAddress1.ToLower
                                oComp.CompStreetAddress2 = oComp.CompStreetAddress2.ToLower
                                oComp.CompStreetAddress3 = oComp.CompStreetAddress3.ToLower
                                oComp.CompStreetCity = oComp.CompStreetCity.ToLower
                                oComp.CompStreetState = oComp.CompStreetState.ToLower
                                oComp.CompStreetCountry = oComp.CompStreetCountry.ToLower
                                oComp.CompStreetZip = oComp.CompStreetZip.ToLower
                                oComp.CompMailAddress1 = oComp.CompMailAddress1.ToLower
                                oComp.CompMailAddress2 = oComp.CompMailAddress2.ToLower
                                oComp.CompMailAddress3 = oComp.CompMailAddress3.ToLower
                                oComp.CompMailCity = oComp.CompMailCity.ToLower
                                oComp.CompMailState = oComp.CompMailState.ToLower
                                oComp.CompMailCountry = oComp.CompMailCountry.ToLower
                                oComp.CompMailZip = oComp.CompMailZip.ToLower
                        End Select
                    Next
                End If
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CaseType"), db)
            End Try
        End Using
        Return iProcessed
    End Function


    Public Sub ScrubComp(ByVal strAKA As String, ByVal strValue As String, ByRef oComp As LTS.Comp, ByVal dblCaseType As Double)
        Dim strAddress As String = " " & oComp.CompStreetAddress1.ToLower & " "
        strAKA = strAKA.ToLower
        strValue = strValue.ToLower
        If InStr(strAddress, " " & Trim(strAKA) & " ", CompareMethod.Text) > 0 Then
            oComp.CompStreetAddress1 = strAddress.Replace(" " & Trim(strAKA) & " ", " " & strValue & " ")
            If dblCaseType = 1 Then oComp.CompStreetAddress1 = oComp.CompStreetAddress1.ToUpper
            If dblCaseType = 0 Then oComp.CompStreetAddress1 = Utilities.UpperFirst(oComp.CompStreetAddress1)
        End If
    End Sub

#End Region


End Class

Public Class NGLCompCalData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompCals
        Me.LinqDB = db
        Me.SourceClass = "NGLCompCalData"
    End Sub

#End Region


#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompCals
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
        Return getCompCalFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompCalsFiltered()
    End Function

    Public Function GetCompCalsFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompCal()

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim CompCals() As DTO.CompCal = (
                    From t In db.CompCals
                    Where t.CompCalCompControl = CompControl
                    Order By t.CompCalMonth, t.CompCalDay
                    Select New DTO.CompCal With {.CompCalControl = t.CompCalControl,
                                                 .CompCalCompControl = t.CompCalCompControl,
                                                 .CompCalMonth = t.CompCalMonth,
                                                 .CompCalDay = t.CompCalDay,
                                                 .CompCalOpen = t.CompCalOpen,
                                                 .CompCalStartTime = t.CompCalStartTime,
                                                 .CompCalEndTime = t.CompCalEndTime,
                                                 .CompCalIsHoliday = t.CompCalIsHoliday,
                                                 .CompCalUpdated = t.CompCalUpdated.ToArray()}).ToArray()
                Return CompCals
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

    Public Function getCompCalFiltered(ByVal Control As Integer) As DTO.CompCal

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim CompCal As DTO.CompCal = (
                From t In db.CompCals
                Where t.CompCalControl = Control
                Select New DTO.CompCal With {.CompCalControl = t.CompCalControl,
                                                 .CompCalCompControl = t.CompCalCompControl,
                                                 .CompCalMonth = t.CompCalMonth,
                                                 .CompCalDay = t.CompCalDay,
                                                 .CompCalOpen = t.CompCalOpen,
                                                 .CompCalStartTime = t.CompCalStartTime,
                                                 .CompCalEndTime = t.CompCalEndTime,
                                                 .CompCalIsHoliday = t.CompCalIsHoliday,
                                                 .CompCalUpdated = t.CompCalUpdated.ToArray()}).Single
                Return CompCal

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

    Public Sub InsertOrUpdateCompCal70(ByVal CompLegalEntity As String,
                                            ByVal CompNumber As Integer,
                                            ByVal CompAlphaCode As String,
                                            ByVal Month As Integer,
                                            ByVal MonthAllowUpdate As Boolean,
                                            ByVal Day As Integer,
                                            ByVal DayAllowUpdate As Boolean,
                                            ByVal Open As Boolean,
                                            ByVal OpenAllowUpdate As Boolean,
                                            ByVal StartTime As String,
                                            ByVal StartTimeAllowUpdate As Boolean,
                                            ByVal EndTime As String,
                                            ByVal EndTimeAllowUpdate As Boolean,
                                            ByVal IsHoliday As Boolean,
                                            ByVal IsHolidayAllowUpdate As Boolean)

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRet = db.spInsertOrUpdateCompCal70(CompLegalEntity, CompNumber, CompAlphaCode, Month, MonthAllowUpdate, Day, DayAllowUpdate, Open, OpenAllowUpdate, StartTime, StartTimeAllowUpdate, EndTime, EndTimeAllowUpdate, IsHoliday, IsHolidayAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCompCal70"))
            End Try
        End Using
    End Sub

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = TryCast(oData, DTO.CompCal)
        'Create New Record
        Return New LTS.CompCal With {.CompCalControl = t.CompCalControl,
                                                 .CompCalCompControl = t.CompCalCompControl,
                                                 .CompCalMonth = t.CompCalMonth,
                                                 .CompCalDay = t.CompCalDay,
                                                 .CompCalOpen = t.CompCalOpen,
                                                 .CompCalStartTime = t.CompCalStartTime,
                                                 .CompCalEndTime = t.CompCalEndTime,
                                                 .CompCalIsHoliday = t.CompCalIsHoliday,
                                                 .CompCalUpdated = If(t.CompCalUpdated Is Nothing, New Byte() {}, t.CompCalUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return getCompCalFiltered(Control:=CType(LinqTable, LTS.CompCal).CompCalControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompCal = TryCast(LinqTable, LTS.CompCal)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CompCals
                       Where d.CompCalControl = source.CompCalControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompCalControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.CompCalUpdated.ToArray}).First

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

Public Class NGLCompContData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompConts
        Me.LinqDB = db
        Me.SourceClass = "NGLCompContData"
    End Sub

#End Region


#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompConts
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
        Return GetCompContFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompContsFiltered()
    End Function

    Public Function GetCompContFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DTO.CompCont
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompCont As DTO.CompCont = (
                From t In db.CompConts
                Where
                    (Control = 0 OrElse t.CompContControl = Control) _
                    And
                    (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.CompContName = Name) _
                    And
                    (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse t.CompContEmail = Email)
                Order By t.CompContControl Descending
                Select New DTO.CompCont With {.CompContControl = t.CompContControl _
                                            , .CompContCompControl = t.CompContCompControl _
                                            , .CompContName = t.CompContName _
                                            , .CompContTitle = t.CompContTitle _
                                            , .CompCont800 = t.CompCont800 _
                                            , .CompContPhone = t.CompContPhone _
                                            , .CompContPhoneExt = t.CompContPhoneExt _
                                            , .CompContFax = t.CompContFax _
                                            , .CompContEmail = t.CompContEmail _
                                            , .CompContTender = t.CompContTender _
                                            , .CompContUpdated = t.CompContUpdated.ToArray()}).First
                Return CompCont

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

    Public Function GetCompContsFiltered(Optional ByVal CompControl As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DTO.CompCont()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompConts() As DTO.CompCont = (
                From t In db.CompConts
                Where
                    (CompControl = 0 OrElse t.CompContCompControl = CompControl) _
                    And
                    (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.CompContName = Name) _
                    And
                    (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse t.CompContEmail = Email)
                Order By t.CompContName
                Select New DTO.CompCont With {.CompContControl = t.CompContControl _
                                            , .CompContCompControl = t.CompContCompControl _
                                            , .CompContName = t.CompContName _
                                            , .CompContTitle = t.CompContTitle _
                                            , .CompCont800 = t.CompCont800 _
                                            , .CompContPhone = t.CompContPhone _
                                            , .CompContPhoneExt = t.CompContPhoneExt _
                                            , .CompContFax = t.CompContFax _
                                            , .CompContEmail = t.CompContEmail _
                                            , .CompContTender = t.CompContTender _
                                            , .CompContUpdated = t.CompContUpdated.ToArray()}).ToArray()
                Return CompConts

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
    ''' Returns the first company contact records for the provided compcontrol sorted by the lowest CompContControl number
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFirstCompContFiltered(ByVal CompControl As Integer) As DTO.CompCont
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompCont As DTO.CompCont = (
                From t In db.CompConts
                Where
                    (t.CompContCompControl = CompControl)
                Order By t.CompContControl
                Select New DTO.CompCont With {.CompContControl = t.CompContControl _
                                            , .CompContCompControl = t.CompContCompControl _
                                            , .CompContName = t.CompContName _
                                            , .CompContTitle = t.CompContTitle _
                                            , .CompCont800 = t.CompCont800 _
                                            , .CompContPhone = t.CompContPhone _
                                            , .CompContPhoneExt = t.CompContPhoneExt _
                                            , .CompContFax = t.CompContFax _
                                            , .CompContEmail = t.CompContEmail _
                                            , .CompContTender = t.CompContTender _
                                            , .CompContUpdated = t.CompContUpdated.ToArray()}).FirstOrDefault()
                Return CompCont

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstCompContFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Sub InsertOrUpdateCompContact70(ByVal CompLegalEntity As String,
                                            ByVal CompNumber As Integer,
                                            ByVal CompAlphaCode As String,
                                            ByVal CompContName As String,
                                            ByVal CompContTitle As String,
                                            ByVal CompContTitleAllowUpdate As Boolean,
                                            ByVal CompCont800 As String,
                                            ByVal CompCont800AllowUpdate As Boolean,
                                            ByVal CompContPhone As String,
                                            ByVal CompContPhoneAllowUpdate As Boolean,
                                            ByVal CompContPhoneExt As String,
                                            ByVal CompContPhoneExtAllowUpdate As Boolean,
                                            ByVal CompContFax As String,
                                            ByVal CompContFaxAllowUpdate As Boolean,
                                            ByVal CompContEmail As String,
                                            ByVal CompContEmailAllowUpdate As Boolean)

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRet = db.spInsertOrUpdateCompContact70(CompLegalEntity, CompNumber, CompAlphaCode, CompContName, CompContTitle, CompContTitleAllowUpdate, CompCont800, CompCont800AllowUpdate, CompContPhone, CompContPhoneAllowUpdate, CompContPhoneExt, CompContPhoneExtAllowUpdate, CompContFax, CompContFaxAllowUpdate, CompContEmail, CompContEmailAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCompContact70"))
            End Try
        End Using
    End Sub

#Region "TMS 365"

    ''' <summary>
    ''' I added this to bypass all that Polymorphic Service Assemblies stuff so I could
    ''' get right to the point
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetCompContsFiltered365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.CompCont()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.CompCont
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.CompCont)
                iQuery = db.CompConts
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompContsFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts Or Updates a CompCont record based on if CompContControl = 0
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added by LVV on 04/24/2018 for v-8.1 VSTS Task #327 Ted Page
    ''' </remarks>
    Public Sub InsertOrUpdateCompCont365(ByVal oRecord As LTS.CompCont)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                If oRecord.CompContControl = 0 Then
                    'Insert
                    db.CompConts.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.CompConts.Attach(oRecord, True)
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCompCont365"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' I added this to bypass all that Polymorphic Service Assemblies stuff so I could
    ''' get right to the point
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <returns></returns>
    Public Function DeleteCompContact(ByVal ControlNumber As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim nObject = db.CompConts.Where(Function(x) x.CompContControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.CompContControl <> 0 Then
                db.CompConts.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                    blnRet = True
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("DeleteCompContact"), db)
                End Try
            End If
        End Using
        Return blnRet
    End Function

#End Region



#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompCont)
        'Create New Record
        Return New LTS.CompCont With {.CompContControl = d.CompContControl _
                                            , .CompContCompControl = d.CompContCompControl _
                                            , .CompContName = d.CompContName _
                                            , .CompContTitle = d.CompContTitle _
                                            , .CompCont800 = d.CompCont800 _
                                            , .CompContPhone = d.CompContPhone _
                                            , .CompContPhoneExt = d.CompContPhoneExt _
                                            , .CompContFax = d.CompContFax _
                                            , .CompContEmail = d.CompContEmail _
                                            , .CompContTender = d.CompContTender _
                                            , .CompContUpdated = If(d.CompContUpdated Is Nothing, New Byte() {}, d.CompContUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompContFiltered(Control:=CType(LinqTable, LTS.CompCont).CompContControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompCont = TryCast(LinqTable, LTS.CompCont)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CompConts
                       Where d.CompContControl = source.CompContControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompContControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.CompContUpdated.ToArray}).First

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

Public Class NGLCompEDIData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompEDIs
        Me.LinqDB = db
        Me.SourceClass = "NGLCompEDIData"
    End Sub

#End Region


#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompEDIs
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
        Return GetCompEDIFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompEDIsFiltered()
    End Function

    Public Function GetCompEDIFiltered(Optional ByVal Control As Integer = 0, Optional ByVal EDIXaction As String = "") As DTO.CompEDI
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim oRecord As DTO.CompEDI = (
               From t In db.CompEDIs
               Where
                   (Control = 0 OrElse t.CompEDIControl = Control) _
                    And
                    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompEDIXaction = EDIXaction) Order By t.CompEDIControl Descending
               Select selectDTOData(t)).First 'added On 12Aug2020 as per CompEDi Changes - ManoRama'
                Return oRecord

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

    Public Function GetCompEDIsFiltered(Optional ByVal CompControl As Integer = 0, Optional ByVal EDIXaction As String = "", Optional ByVal page As Integer = 1,
                                                      Optional ByVal pagesize As Integer = 1000) As DTO.CompEDI()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oRecords = (
               From t In db.CompEDIs
               Where
                    (CompControl = 0 OrElse t.CompEDICompControl = CompControl) _
                    And
                    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompEDIXaction = EDIXaction) Order By t.CompEDIControl Descending
               Select t.CompEDIControl).ToArray()
                'added On 12Aug2020 as per CompEDi Changes - ManoRama'

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim compEDIS() As DTO.CompEDI = (
                From d In db.CompEDIs
                Where oRecords.Contains(d.CompEDIControl)
                Order By d.CompEDICompControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return compEDIS


                'Dim CompEDIs() As DTO.CompEDI = (
                'From t In db.CompEDIs
                'Where
                '    (CompControl = 0 OrElse t.CompEDICompControl = CompControl) _
                '    And
                '    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompEDIXaction = EDIXaction)
                'Order By t.CompEDIControl
                'Select New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl _
                '                            , .CompEDICompControl = t.CompEDICompControl _
                '                            , .CompEDIXaction = t.CompEDIXaction _
                '                            , .CompEDIComment = t.CompEDIComment _
                '                            , .CompEDISecurityQual = t.CompEDISecurityQual _
                '                            , .CompEDISecurityCode = t.CompEDISecurityCode _
                '                            , .CompEDIPartnerQual = t.CompEDIPartnerQual _
                '                            , .CompEDIPartnerCode = t.CompEDIPartnerCode _
                '                            , .CompEDIISASequence = t.CompEDIISASequence _
                '                            , .CompEDISequence = t.CompEDISequence _
                '                            , .CompEDIEmailNotificationOn = t.CompEDIEmailNotificationOn _
                '                            , .CompEDIEmailAddress = t.CompEDIEmailAddress _
                '                            , .CompEDIAcknowledgementRequested = t.CompEDIAcknowledgementRequested _
                '                            , .CompEDIAcceptOn997 = t.CompEDIAcceptOn997 _
                '                            , .CompEDIMethodOfPayment = t.CompEDIMethodOfPayment _
                '                            , .CompEDIUpdated = t.CompEDIUpdated.ToArray()}).ToArray()
                'Return CompEDIs

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
    ''' Method to Get the Company EDI Info Based On Selected Company
    ''' </summary>
    ''' <param name="filters">Contains ComapnyID</param>
    ''' <param name="RecordCount">No of Records</param>
    ''' <remarks>
    ''' Added By ManoRama On 12-AUG-2020
    ''' </remarks>
    ''' <returns>LTS Object for vCompEDI</returns>
    Public Function GetCompEDIs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCompEDI()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vCompEDI

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                If filters.ParentControl = 0 Then
                    Dim sMsg As String = " The Company ID is missing. Please return to the Company page and select a valid Company."
                    throwNoDataFaultException(sMsg)
                End If

                Dim iQuery As IQueryable(Of LTS.vCompEDI)
                iQuery = db.vCompEDIs
                Dim filterWhere As String = " (CompEDICompControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CompEDIComment"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompEDIServices"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Method to Save the Company EDI Details.
    ''' </summary>
    ''' <param name="cmpny">CompanyEDI LTS Object</param>
    ''' <remarks>
    ''' Added for Company EDI save by ManoRama12-AUG2020
    ''' </remarks>
    ''' <returns>true/false</returns>
    Public Function SaveCompanyEDI(ByVal cmpny As LTS.CompEDI) As Boolean
        Dim blnRet As Boolean = False
        If cmpny Is Nothing Then Return False 'nothing to do
        Dim blnNewcmpny As Boolean = False
        Dim oNew As New LTS.CompEDI()
        Dim odto As New DTO.CompEDI()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'verify the cmpny contract
                If cmpny.CompEDICompControl = 0 Then
                    Dim lDetails As New List(Of String) From {"CompanyID Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If

                If cmpny.CompEDIControl = 0 Then
                    odto = selectDTOData(cmpny)
                    ValidateNewRecord(db, odto)
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CompEDIModUser", "CompEDIModDate", "Comp"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, cmpny, skipObjs, strMSG)
                    With oNew
                        .CompEDIModDate = Date.Now
                        .CompEDIModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CompEDIs.InsertOnSubmit(oNew)
                    blnNewcmpny = True
                Else
                    odto = selectDTOData(cmpny)
                    ValidateUpdatedRecord(db, odto)
                    Dim oExisting = db.CompEDIs.Where(Function(x) x.CompEDIControl = cmpny.CompEDIControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CompEDIControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for CompanyEDI Action : " & cmpny.CompEDIXaction)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CompEDIModUser", "CompEDIModDate", "Comp"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, cmpny, skipObjs, strMSG)
                    With oExisting
                        .CompEDIModDate = Date.Now
                        .CompEDIModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCompanyEDI"), db)
            End Try
        End Using
        'If blnNewcmpny Then
        '    'we need to add the break point and the first lines in the equipmatrix tables
        '    cmpny.CarrTarEquipMatCarrTarMatBPControl = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarMatBPData", False), NGLCarrTarMatBPData).createCarrTarEquipMatBreakPointForTariff(cmpny.CompEDICompControl, cmpny.CarrTarEquipMatTarRateTypeControl, cmpny.CarrTarEquipMatClassTypeControl, cmpny.CarrTarEquipMatTarBracketTypeControl)
        '    cmpny.CompEDIControl = oNew.CompEDIControl
        '    blnRet = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData).createNewCarrTarEquipMatrixFromcmpny(cmpny)
        'End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Method to Delete the CompanyEDI Record from the database
    ''' </summary>
    ''' <param name="iControl">CompEDIControl(PK)</param>
    ''' <remarks>
    ''' Added for Delete functionality by ManoRama On 12-AUG-2020
    ''' </remarks>
    ''' <returns>true/false</returns>
    Public Function DeleteCompanyEDI(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Delte the Record
                Dim oToDelete = db.CompEDIs.Where(Function(x) x.CompEDIControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CompEDIControl = 0 Then Return True 'already deleted

                db.CompEDIs.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCompanyEDI"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompEDI)
        'Create New Record
        Return New LTS.CompEDI With {.CompEDIControl = d.CompEDIControl _
                                    , .CompEDICompControl = d.CompEDICompControl _
                                    , .CompEDIXaction = d.CompEDIXaction _
                                    , .CompEDIComment = d.CompEDIComment _
                                    , .CompEDISecurityQual = d.CompEDISecurityQual _
                                    , .CompEDISecurityCode = d.CompEDISecurityCode _
                                    , .CompEDIPartnerQual = d.CompEDIPartnerQual _
                                    , .CompEDIPartnerCode = d.CompEDIPartnerCode _
                                    , .CompEDIISASequence = d.CompEDIISASequence _
                                    , .CompEDISequence = d.CompEDISequence _
                                    , .CompEDIEmailNotificationOn = d.CompEDIEmailNotificationOn _
                                    , .CompEDIEmailAddress = d.CompEDIEmailAddress _
                                    , .CompEDIAcknowledgementRequested = d.CompEDIAcknowledgementRequested _
                                    , .CompEDIAcceptOn997 = d.CompEDIAcceptOn997 _
                                    , .CompEDIMethodOfPayment = d.CompEDIMethodOfPayment _
                                    , .CompEDIUpdated = If(d.CompEDIUpdated Is Nothing, New Byte() {}, d.CompEDIUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompEDIFiltered(Control:=CType(LinqTable, LTS.CompEDI).CompEDIControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompEDI = TryCast(LinqTable, LTS.CompEDI)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CompEDIs
                       Where d.CompEDIControl = source.CompEDIControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompEDIControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.CompEDIUpdated.ToArray}).First

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

    Protected Function selectDTOData(ByVal t As LTS.CompEDI, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CompEDI
        Return New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl,
                                             .CompEDICompControl = t.CompEDICompControl,
                                             .CompEDIXaction = t.CompEDIXaction,
                                             .CompEDIComment = t.CompEDIComment,
                                             .CompEDISecurityQual = t.CompEDISecurityQual,
                                             .CompEDISecurityCode = t.CompEDISecurityCode,
                                             .CompEDIPartnerQual = t.CompEDIPartnerQual,
                                             .CompEDIPartnerCode = t.CompEDIPartnerCode,
                                             .CompEDIISASequence = t.CompEDIISASequence,
                                             .CompEDISequence = t.CompEDISequence,
                                             .CompEDIEmailNotificationOn = t.CompEDIEmailNotificationOn,
                                             .CompEDIEmailAddress = t.CompEDIEmailAddress,
                                             .CompEDIAcknowledgementRequested = t.CompEDIAcknowledgementRequested,
                                             .CompEDIAcceptOn997 = t.CompEDIAcceptOn997,
                                             .CompEDIMethodOfPayment = t.CompEDIMethodOfPayment,
                                             .CompEDIUpdated = t.CompEDIUpdated.ToArray(),
                                             .CompEDIModUser = t.CompEDIModUser,
                                             .CompEDIModDate = t.CompEDIModDate,
                                             .page = page,
                                             .Pages = pagecount,
                                             .recordcount = recordcount,
                                             .pagesize = pagesize}
    End Function

    Protected Friend Function selectDTOData(ByVal t As LTS.CompEDI, ByRef db As NGLMASCompDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CompEDI
        Return New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl,
                                             .CompEDICompControl = t.CompEDICompControl,
                                             .CompEDIXaction = t.CompEDIXaction,
                                             .CompEDIComment = t.CompEDIComment,
                                             .CompEDISecurityQual = t.CompEDISecurityQual,
                                             .CompEDISecurityCode = t.CompEDISecurityCode,
                                             .CompEDIPartnerQual = t.CompEDIPartnerQual,
                                             .CompEDIPartnerCode = t.CompEDIPartnerCode,
                                             .CompEDIISASequence = t.CompEDIISASequence,
                                             .CompEDISequence = t.CompEDISequence,
                                             .CompEDIEmailNotificationOn = t.CompEDIEmailNotificationOn,
                                             .CompEDIEmailAddress = t.CompEDIEmailAddress,
                                             .CompEDIAcknowledgementRequested = t.CompEDIAcknowledgementRequested,
                                             .CompEDIAcceptOn997 = t.CompEDIAcceptOn997,
                                             .CompEDIMethodOfPayment = t.CompEDIMethodOfPayment,
                                             .CompEDIUpdated = t.CompEDIUpdated.ToArray(),
                                             .CompEDIModUser = t.CompEDIModUser,
                                             .CompEDIModDate = t.CompEDIModDate,
                                             .page = page,
                                             .Pages = pagecount,
                                             .recordcount = recordcount,
                                             .pagesize = pagesize}
    End Function
    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.CompEDI)
            Try
                Dim CarrierEDI As DTO.CompEDI = (
                    From t In CType(oDB, NGLMASCompDataContext).CompEDIs
                    Where
                        (t.CompEDICompControl = .CompEDICompControl _
                        And
                        t.CompEDIXaction = .CompEDIXaction)
                    Select New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl}).First

                If Not CarrierEDI Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Carrier EDI data.  The Carrier EDI XAction, " & .CompEDIXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.CompEDI)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierEDI As DTO.CompEDI = (
                From t In CType(oDB, NGLMASCompDataContext).CompEDIs
                Where
                    (t.CompEDIControl <> .CompEDIControl) _
                    And
                    (t.CompEDICompControl = .CompEDICompControl _
                    And
                    t.CompEDIXaction = .CompEDIXaction)
                Select New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl}).First

                If Not CarrierEDI Is Nothing Then
                    Utilities.SaveAppError("Cannot save Carrier EDI changes.  The Carrier EDI XAction, " & .CompEDIXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLCompMasterEDIData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompEDIs
        Me.LinqDB = db
        Me.SourceClass = "NGLCompMasterEDIData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompEDIs
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
        Return GetCompMasterEDIFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompMasterEDIsFiltered()
    End Function

    Public Function GetCompMasterEDIFiltered(Optional ByVal Control As Integer = 0, Optional ByVal EDIXaction As String = "") As DTO.CompMasterEDI
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim oRecord As DTO.CompMasterEDI = (
               From t In db.CompMasterEDIs
               Where
                   (Control = 0 OrElse t.CompMasterEDIControl = Control) _
                    And
                    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompMasterEDIXaction = EDIXaction) Order By t.CompMasterEDIControl Descending
               Select selectDTOData(t)).First 'added On 12Aug2020 as per CompEDi Changes - ManoRama'
                Return oRecord

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

    Public Function GetCompMasterEDIsFiltered(Optional ByVal CompControl As Integer = 0, Optional ByVal EDIXaction As String = "", Optional ByVal page As Integer = 1,
                                                      Optional ByVal pagesize As Integer = 1000) As DTO.CompMasterEDI()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oRecords = (
               From t In db.CompMasterEDIs
               Where
                    (CompControl = 0 OrElse t.CompMasterEDIControl = CompControl) _
                    And
                    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompMasterEDIXaction = EDIXaction) Order By t.CompMasterEDIControl Descending
               Select t.CompMasterEDIControl).ToArray()
                'added On 12Aug2020 as per CompEDi Changes - ManoRama'

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim compMasterEDIS() As DTO.CompMasterEDI = (
                From d In db.CompMasterEDIs
                Where oRecords.Contains(d.CompMasterEDIControl)
                Order By d.CompMasterEDIControl
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return compMasterEDIS


                'Dim CompEDIs() As DTO.CompEDI = (
                'From t In db.CompEDIs
                'Where
                '    (CompControl = 0 OrElse t.CompEDICompControl = CompControl) _
                '    And
                '    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompEDIXaction = EDIXaction)
                'Order By t.CompEDIControl
                'Select New DTO.CompEDI With {.CompEDIControl = t.CompEDIControl _
                '                            , .CompEDICompControl = t.CompEDICompControl _
                '                            , .CompEDIXaction = t.CompEDIXaction _
                '                            , .CompEDIComment = t.CompEDIComment _
                '                            , .CompEDISecurityQual = t.CompEDISecurityQual _
                '                            , .CompEDISecurityCode = t.CompEDISecurityCode _
                '                            , .CompEDIPartnerQual = t.CompEDIPartnerQual _
                '                            , .CompEDIPartnerCode = t.CompEDIPartnerCode _
                '                            , .CompEDIISASequence = t.CompEDIISASequence _
                '                            , .CompEDISequence = t.CompEDISequence _
                '                            , .CompEDIEmailNotificationOn = t.CompEDIEmailNotificationOn _
                '                            , .CompEDIEmailAddress = t.CompEDIEmailAddress _
                '                            , .CompEDIAcknowledgementRequested = t.CompEDIAcknowledgementRequested _
                '                            , .CompEDIAcceptOn997 = t.CompEDIAcceptOn997 _
                '                            , .CompEDIMethodOfPayment = t.CompEDIMethodOfPayment _
                '                            , .CompEDIUpdated = t.CompEDIUpdated.ToArray()}).ToArray()
                'Return CompEDIs

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

    Friend Function selectDTOData(ByVal t As LTS.CompMasterEDI, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CompMasterEDI
        Return New DTO.CompMasterEDI With {.CompMasterEDIControl = t.CompMasterEDIControl,
                                             .CompMasterEDIXaction = t.CompMasterEDIXaction,
                                             .CompMasterEDIComment = t.CompMasterEDIComment,
                                             .CompMasterEDISecurityQual = t.CompMasterEDISecurityQual,
                                             .CompMasterEDISecurityCode = t.CompMasterEDISecurityCode,
                                             .CompMasterEDIPartnerQual = t.CompMasterEDIPartnerQual,
                                             .CompMasterEDIPartnerCode = t.CompMasterEDIPartnerCode,
                                             .CompMasterEDIISASequence = t.CompMasterEDIISASequence,
                                             .CompMasterEDISequence = t.CompMasterEDISequence,
                                             .CompMasterEDIEmailNotificationOn = t.CompMasterEDIEmailNotificationOn,
                                             .CompMasterEDIEmailAddress = t.CompMasterEDIEmailAddress,
                                             .CompMasterEDIAcknowledgementRequested = t.CompMasterEDIAcknowledgementRequested,
                                             .CompMasterEDIAcceptOn997 = t.CompMasterEDIAcceptOn997,
                                             .CompMasterEDIMethodOfPayment = t.CompMasterEDIMethodOfPayment,
                                             .CompMasterEDIUpdated = t.CompMasterEDIUpdated.ToArray(),
                                             .CompMasterEDIModUser = t.CompMasterEDIModUser,
                                             .CompMasterEDIModDate = t.CompMasterEDIModDate,
                                             .page = page,
                                             .Pages = pagecount,
                                             .recordcount = recordcount,
                                             .pagesize = pagesize}
    End Function

    Friend Function selectDTOData(ByVal t As LTS.CompMasterEDI, ByRef db As NGLMASCompDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CompMasterEDI
        Return New DTO.CompMasterEDI With {.CompMasterEDIControl = t.CompMasterEDIControl,
                                             .CompMasterEDIXaction = t.CompMasterEDIXaction,
                                             .CompMasterEDIComment = t.CompMasterEDIComment,
                                             .CompMasterEDISecurityQual = t.CompMasterEDISecurityQual,
                                             .CompMasterEDISecurityCode = t.CompMasterEDISecurityCode,
                                             .CompMasterEDIPartnerQual = t.CompMasterEDIPartnerQual,
                                             .CompMasterEDIPartnerCode = t.CompMasterEDIPartnerCode,
                                             .CompMasterEDIISASequence = t.CompMasterEDIISASequence,
                                             .CompMasterEDISequence = t.CompMasterEDISequence,
                                             .CompMasterEDIEmailNotificationOn = t.CompMasterEDIEmailNotificationOn,
                                             .CompMasterEDIEmailAddress = t.CompMasterEDIEmailAddress,
                                             .CompMasterEDIAcknowledgementRequested = t.CompMasterEDIAcknowledgementRequested,
                                             .CompMasterEDIAcceptOn997 = t.CompMasterEDIAcceptOn997,
                                             .CompMasterEDIMethodOfPayment = t.CompMasterEDIMethodOfPayment,
                                             .CompMasterEDIUpdated = t.CompMasterEDIUpdated.ToArray(),
                                             .CompMasterEDIModUser = t.CompMasterEDIModUser,
                                             .CompMasterEDIModDate = t.CompMasterEDIModDate,
                                             .page = page,
                                             .Pages = pagecount,
                                             .recordcount = recordcount,
                                             .pagesize = pagesize}
    End Function

    ''' <summary>
    ''' Get all of the company master EDI settings by filter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.003 on 10/23/2023
    '''   removed parent ID filter no needed until we add the Legal Entity
    ''' </remarks>
    Public Function GetCompMasterEDI(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CompMasterEDI()
        If filters Is Nothing Then Return Nothing
        'If filters.ParentControl = 0 Then
        '    Dim sMsg As String = " The Company EDI is missing. Please return to the Company EDI contract page and select a valid Company Master EDI."
        '    throwNoDataFaultException(sMsg)
        'End If
        Dim oRet() As LTS.CompMasterEDI

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.CompMasterEDI)
                iQuery = db.CompMasterEDIs
                'Dim filterWhere As String = " (CompMasterEDIControl = " & filters.ParentControl & ") "
                'Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CompMasterEDIControl"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, "")
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompMasterEDI"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function SaveCompMasterEDI(ByVal service As LTS.CompMasterEDI) As Boolean
        Dim blnRet As Boolean = False
        If service Is Nothing Then Return False 'nothing to do
        Dim blnNewService As Boolean = False
        Dim oNew As New LTS.CompMasterEDI()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'verify the service contract
                'If service.CompMasterEDIControl = 0 Then
                '    Dim lDetails As New List(Of String) From {"Company Master EDI Contract Reference", " was not provided and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                'If Not db.CompMasterEDIs.Any(Function(x) x.CompMasterEDIControl = service.CompMasterEDIControl) Then
                '    Dim lDetails As New List(Of String) From {"Company Master EDI Contract Reference", " was not found and "}
                '    throwInvalidKeyParentRequiredException(lDetails)
                '    Return False
                'End If
                If service.CompMasterEDIControl = 0 Then
                    If db.CompMasterEDIs.Any(Function(x) x.CompMasterEDIControl = service.CompMasterEDIControl) Then
                        Dim sCompEDIXaction As String = db.CompMasterEDIs.Where(Function(x) x.CompMasterEDIControl = service.CompMasterEDIControl).Select(Function(x) x.CompMasterEDIXaction).FirstOrDefault()
                        'Cannot save changes to {0}.  Only one {1} is allowed for each {0}; a {1} of {2} is already configured for this {0}.
                        throwInvalidKeyAlreadyExistsException("Company Master EDI", "Action", sCompEDIXaction)
                    End If

                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CompMasterEDIModUser", "CompMasterEDIModDate", "CompMasterEDIUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, service, skipObjs, strMSG)
                    With oNew
                        .CompMasterEDIModDate = Date.Now
                        .CompMasterEDIModUser = Me.Parameters.UserName
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CompMasterEDIs.InsertOnSubmit(oNew)
                    blnNewService = True
                Else
                    Dim oExisting = db.CompMasterEDIs.Where(Function(x) x.CompMasterEDIControl = service.CompMasterEDIControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CompMasterEDIControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for Company MasterEDI service: " & service.CompMasterEDIXaction)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CompMasterEDIModUser", "CompMasterEDIModDate", "CompMasterEDIUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, service, skipObjs, strMSG)
                    With oExisting
                        .CompMasterEDIModDate = Date.Now
                        .CompMasterEDIModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCompMasterEDI"), db)
            End Try
        End Using
        'If blnNewService Then
        '    'we need to add the break point and the first lines in the equipmatrix tables
        '    service.CarrTarEquipMatCarrTarMatBPControl = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarMatBPData", False), NGLCarrTarMatBPData).createCarrTarEquipMatBreakPointForTariff(service.CarrTarEquipCarrTarControl, service.CarrTarEquipMatTarRateTypeControl, service.CarrTarEquipMatClassTypeControl, service.CarrTarEquipMatTarBracketTypeControl)
        '    service.CarrTarEquipControl = oNew.CarrTarEquipControl
        '    blnRet = DirectCast(Me.NDPBaseClassFactory("NGLCarrTarEquipMatData", False), NGLCarrTarEquipMatData).createNewCarrTarEquipMatrixFromService(service)
        'End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Removes the selected Control number from the CompMasterEDI table
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.003 on 10/23/2023 
    '''     fixed bug where validation prevented delete record 
    ''' </remarks>
    Public Function DeleteCompMasterEDI(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CompMasterEDIs.Where(Function(x) x.CompMasterEDIControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CompMasterEDIControl = 0 Then Return True 'already deleted
                db.CompMasterEDIs.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCompMasterEDI"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompMasterEDI)
        'Create New Record
        Return New LTS.CompMasterEDI With {.CompMasterEDIControl = d.CompMasterEDIControl _
                                    , .CompMasterEDIXaction = d.CompMasterEDIXaction _
                                    , .CompMasterEDIComment = d.CompMasterEDIComment _
                                    , .CompMasterEDISecurityQual = d.CompMasterEDISecurityQual _
                                    , .CompMasterEDISecurityCode = d.CompMasterEDISecurityCode _
                                    , .CompMasterEDIPartnerQual = d.CompMasterEDIPartnerQual _
                                    , .CompMasterEDIPartnerCode = d.CompMasterEDIPartnerCode _
                                    , .CompMasterEDIISASequence = d.CompMasterEDIISASequence _
                                    , .CompMasterEDISequence = d.CompMasterEDISequence _
                                    , .CompMasterEDIEmailNotificationOn = d.CompMasterEDIEmailNotificationOn _
                                    , .CompMasterEDIEmailAddress = d.CompMasterEDIEmailAddress _
                                    , .CompMasterEDIAcknowledgementRequested = d.CompMasterEDIAcknowledgementRequested _
                                    , .CompMasterEDIAcceptOn997 = d.CompMasterEDIAcceptOn997 _
                                    , .CompMasterEDIMethodOfPayment = d.CompMasterEDIMethodOfPayment _
                                    , .CompMasterEDIModDate = d.CompMasterEDIModDate _
                                    , .CompMasterEDIModUser = d.CompMasterEDIModUser _
                                    , .CompMasterEDIUpdated = If(d.CompMasterEDIUpdated Is Nothing, New Byte() {}, d.CompMasterEDIUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompMasterEDIFiltered(Control:=CType(LinqTable, LTS.CompMasterEDI).CompMasterEDIControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompMasterEDI = TryCast(LinqTable, LTS.CompMasterEDI)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CompMasterEDIs
                       Where d.CompMasterEDIControl = source.CompMasterEDIControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompMasterEDIControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.CompMasterEDIUpdated.ToArray}).First

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


Public Class NGLCompGoalData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompGoals
        Me.LinqDB = db
        Me.SourceClass = "NGLCompGoalData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompGoals
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
        Return GetCompGoalFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompGoalsFiltered()
    End Function

    Public Function GetCompGoalFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Year As Short = 0) As DTO.CompGoal
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompGoal As DTO.CompGoal = (
                From t In db.CompGoals
                Where
                    (Control = 0 OrElse t.CompGoalControl = Control) _
                    And
                    (Year = 0 OrElse t.CompGoalYear = Year)
                Order By t.CompGoalControl Descending
                Select New DTO.CompGoal With {.CompGoalControl = t.CompGoalControl _
                                            , .CompGoalCompControl = t.CompGoalCompControl _
                                            , .CompGoalYear = If(t.CompGoalYear.HasValue, t.CompGoalYear.Value, 0) _
                                            , .CompGoalGrossSales = If(t.CompGoalGrossSales.HasValue, t.CompGoalGrossSales.Value, 0) _
                                            , .CompGoalCOGS = If(t.CompGoalCOGS.HasValue, t.CompGoalCOGS.Value, 0) _
                                            , .CompGoalCOGSPer = If(t.CompGoalCOGSPer.HasValue, t.CompGoalCOGSPer.Value, 0) _
                                            , .CompGoalEstFreight = If(t.CompGoalEstFreight.HasValue, t.CompGoalEstFreight.Value, 0) _
                                            , .CompGoalEstFrtPer = If(t.CompGoalEstFrtPer.HasValue, t.CompGoalEstFrtPer.Value, 0) _
                                            , .CompGoalEstControlledFrt = If(t.CompGoalEstControlledFrt.HasValue, t.CompGoalEstControlledFrt.Value, 0) _
                                            , .CompGoalEstControlledPer = If(t.CompGoalEstControlledPer.HasValue, t.CompGoalEstControlledPer.Value, 0) _
                                            , .CompGoalEstSavings = If(t.CompGoalEstSavings.HasValue, t.CompGoalEstSavings.Value, 0) _
                                            , .CompGoalEstSavPer = If(t.CompGoalEstSavPer.HasValue, t.CompGoalEstSavPer.Value, 0) _
                                            , .CompGoalModDate = t.CompGoalModDate _
                                            , .CompGoalModUser = t.CompGoalModUser _
                                            , .CompGoalUpdated = t.CompGoalUpdated.ToArray()}).First
                Return CompGoal

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

    Public Function GetCompGoalsFiltered(Optional ByVal CompControl As Integer = 0, Optional ByVal Year As Short = 0) As DTO.CompGoal()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompGoals() As DTO.CompGoal = (
                From t In db.CompGoals
                Where
                    (CompControl = 0 OrElse t.CompGoalCompControl = CompControl) _
                    And
                    (Year = 0 OrElse t.CompGoalYear = Year)
                Order By t.CompGoalControl
                Select New DTO.CompGoal With {.CompGoalControl = t.CompGoalControl _
                                            , .CompGoalCompControl = t.CompGoalCompControl _
                                            , .CompGoalYear = If(t.CompGoalYear.HasValue, t.CompGoalYear.Value, 0) _
                                            , .CompGoalGrossSales = If(t.CompGoalGrossSales.HasValue, t.CompGoalGrossSales.Value, 0) _
                                            , .CompGoalCOGS = If(t.CompGoalCOGS.HasValue, t.CompGoalCOGS.Value, 0) _
                                            , .CompGoalCOGSPer = If(t.CompGoalCOGSPer.HasValue, t.CompGoalCOGSPer.Value, 0) _
                                            , .CompGoalEstFreight = If(t.CompGoalEstFreight.HasValue, t.CompGoalEstFreight.Value, 0) _
                                            , .CompGoalEstFrtPer = If(t.CompGoalEstFrtPer.HasValue, t.CompGoalEstFrtPer.Value, 0) _
                                            , .CompGoalEstControlledFrt = If(t.CompGoalEstControlledFrt.HasValue, t.CompGoalEstControlledFrt.Value, 0) _
                                            , .CompGoalEstControlledPer = If(t.CompGoalEstControlledPer.HasValue, t.CompGoalEstControlledPer.Value, 0) _
                                            , .CompGoalEstSavings = If(t.CompGoalEstSavings.HasValue, t.CompGoalEstSavings.Value, 0) _
                                            , .CompGoalEstSavPer = If(t.CompGoalEstSavPer.HasValue, t.CompGoalEstSavPer.Value, 0) _
                                            , .CompGoalModDate = t.CompGoalModDate _
                                            , .CompGoalModUser = t.CompGoalModUser _
                                            , .CompGoalUpdated = t.CompGoalUpdated.ToArray()}).ToArray()
                Return CompGoals

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompGoal)
        'Create New Record
        Return New LTS.CompGoal With {.CompGoalControl = d.CompGoalControl _
                                            , .CompGoalCompControl = d.CompGoalCompControl _
                                            , .CompGoalYear = d.CompGoalYear _
                                            , .CompGoalGrossSales = d.CompGoalGrossSales _
                                            , .CompGoalCOGS = d.CompGoalCOGS _
                                            , .CompGoalCOGSPer = d.CompGoalCOGSPer _
                                            , .CompGoalEstFreight = d.CompGoalEstFreight _
                                            , .CompGoalEstFrtPer = d.CompGoalEstFrtPer _
                                            , .CompGoalEstControlledFrt = d.CompGoalEstControlledFrt _
                                            , .CompGoalEstControlledPer = d.CompGoalEstControlledPer _
                                            , .CompGoalEstSavings = d.CompGoalEstSavings _
                                            , .CompGoalEstSavPer = d.CompGoalEstSavPer _
                                            , .CompGoalModDate = Date.Now _
                                            , .CompGoalModUser = Parameters.UserName _
                                            , .CompGoalUpdated = If(d.CompGoalUpdated Is Nothing, New Byte() {}, d.CompGoalUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompGoalFiltered(Control:=CType(LinqTable, LTS.CompGoal).CompGoalControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompGoal = TryCast(LinqTable, LTS.CompGoal)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CompGoals
                       Where d.CompGoalControl = source.CompGoalControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompGoalControl _
                                                                , .ModDate = d.CompGoalModDate _
                                                                , .ModUser = d.CompGoalModUser _
                                                                , .Updated = d.CompGoalUpdated.ToArray}).First

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

'Public Class NGLCompMasterEDIData : Inherits NDPBaseClass

'#Region " Constructors "

'    Public Sub New(ByVal oParameters As WCFParameters)
'        MyBase.New()
'        processParameters(oParameters)
'        Dim db As New NGLMASCompDataContext(ConnectionString)
'        Me.LinqTable = db.CompMasterEDIs
'        Me.LinqDB = db
'        Me.SourceClass = "NGLCompMasterEDIData"
'    End Sub

'#End Region

'#Region " Properties "

'    Protected Overrides Property LinqTable() As Object
'        Get
'            Dim db As New NGLMASCompDataContext(ConnectionString)
'            _LinqTable = db.CompMasterEDIs
'            Me.LinqDB = db
'            Return _LinqTable
'        End Get
'        Set(ByVal value As Object)
'            _LinqTable = value
'        End Set
'    End Property


'#End Region
'#Region "Public Methods"

'    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
'        Return GetCompMasterEDIFiltered(Control:=Control)
'    End Function

'    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
'        Return GetCompMasterEDIsFiltered()
'    End Function

'    Public Function GetCompMasterEDIFiltered(Optional ByVal Control As Integer = 0, Optional ByVal EDIXaction As String = "") As DTO.CompMasterEDI
'        Using db As New NGLMASCompDataContext(ConnectionString)
'            Try
'                'Get the newest record that matches the provided criteria
'                Dim CompMasterEDI As DTO.CompMasterEDI = (
'                From t In db.CompMasterEDIs
'                Where
'                    (Control = 0 OrElse t.CompMasterEDIControl = Control) _
'                    And
'                    (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse t.CompMasterEDIXaction = EDIXaction)
'                Order By t.CompMasterEDIControl Descending
'                Select New DTO.CompMasterEDI With {.CompMasterEDIControl = t.CompMasterEDIControl _
'                                            , .CompMasterEDIXaction = t.CompMasterEDIXaction _
'                                            , .CompMasterEDIComment = t.CompMasterEDIComment _
'                                            , .CompMasterEDISecurityQual = t.CompMasterEDISecurityQual _
'                                            , .CompMasterEDISecurityCode = t.CompMasterEDISecurityCode _
'                                            , .CompMasterEDIPartnerQual = t.CompMasterEDIPartnerQual _
'                                            , .CompMasterEDIPartnerCode = t.CompMasterEDIPartnerCode _
'                                            , .CompMasterEDIISASequence = t.CompMasterEDIISASequence _
'                                            , .CompMasterEDISequence = t.CompMasterEDISequence _
'                                            , .CompMasterEDIEmailNotificationOn = t.CompMasterEDIEmailNotificationOn _
'                                            , .CompMasterEDIEmailAddress = t.CompMasterEDIEmailAddress _
'                                            , .CompMasterEDIAcknowledgementRequested = t.CompMasterEDIAcknowledgementRequested _
'                                            , .CompMasterEDIAcceptOn997 = t.CompMasterEDIAcceptOn997 _
'                                            , .CompMasterEDIMethodOfPayment = t.CompMasterEDIMethodOfPayment _
'                                            , .CompMasterEDIUpdated = t.CompMasterEDIUpdated.ToArray()}).First
'                Return CompMasterEDI

'            Catch ex As System.Data.SqlClient.SqlException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
'            Catch ex As InvalidOperationException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
'            Catch ex As Exception
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
'            End Try


'            Return Nothing

'        End Using
'    End Function

'    Public Function GetCompMasterEDIsFiltered(Optional ByVal EDIXaction As String = "") As DTO.CompMasterEDI()
'        Using db As New NGLMASCompDataContext(ConnectionString)
'            Try
'                'Return all the contacts that match the criteria sorted by name
'                Dim CompMasterEDIs() As DTO.CompMasterEDI = (
'                From t In db.CompMasterEDIs
'                Where
'                    (t.CompMasterEDIXaction.Contains(EDIXaction))
'                Order By t.CompMasterEDIControl
'                Select New DTO.CompMasterEDI With {.CompMasterEDIControl = t.CompMasterEDIControl _
'                                            , .CompMasterEDIXaction = t.CompMasterEDIXaction _
'                                            , .CompMasterEDIComment = t.CompMasterEDIComment _
'                                            , .CompMasterEDISecurityQual = t.CompMasterEDISecurityQual _
'                                            , .CompMasterEDISecurityCode = t.CompMasterEDISecurityCode _
'                                            , .CompMasterEDIPartnerQual = t.CompMasterEDIPartnerQual _
'                                            , .CompMasterEDIPartnerCode = t.CompMasterEDIPartnerCode _
'                                            , .CompMasterEDIISASequence = t.CompMasterEDIISASequence _
'                                            , .CompMasterEDISequence = t.CompMasterEDISequence _
'                                            , .CompMasterEDIEmailNotificationOn = t.CompMasterEDIEmailNotificationOn _
'                                            , .CompMasterEDIEmailAddress = t.CompMasterEDIEmailAddress _
'                                            , .CompMasterEDIAcknowledgementRequested = t.CompMasterEDIAcknowledgementRequested _
'                                            , .CompMasterEDIAcceptOn997 = t.CompMasterEDIAcceptOn997 _
'                                            , .CompMasterEDIMethodOfPayment = t.CompMasterEDIMethodOfPayment _
'                                            , .CompMasterEDIUpdated = t.CompMasterEDIUpdated.ToArray()}).ToArray()
'                Return CompMasterEDIs

'            Catch ex As System.Data.SqlClient.SqlException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
'            Catch ex As InvalidOperationException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
'            Catch ex As Exception
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
'            End Try


'            Return Nothing

'        End Using
'    End Function

'#End Region

'#Region "Protected Methods"

'    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
'        Dim d = TryCast(oData, DTO.CompMasterEDI)

'        'Create New Record
'        Return New LTS.CompMasterEDI With {.CompMasterEDIControl = d.CompMasterEDIControl _
'                                    , .CompMasterEDIXaction = d.CompMasterEDIXaction _
'                                    , .CompMasterEDIComment = d.CompMasterEDIComment _
'                                    , .CompMasterEDISecurityQual = d.CompMasterEDISecurityQual _
'                                    , .CompMasterEDISecurityCode = d.CompMasterEDISecurityCode _
'                                    , .CompMasterEDIPartnerQual = d.CompMasterEDIPartnerQual _
'                                    , .CompMasterEDIPartnerCode = d.CompMasterEDIPartnerCode _
'                                    , .CompMasterEDIISASequence = d.CompMasterEDIISASequence _
'                                    , .CompMasterEDISequence = d.CompMasterEDISequence _
'                                    , .CompMasterEDIEmailNotificationOn = d.CompMasterEDIEmailNotificationOn _
'                                    , .CompMasterEDIEmailAddress = d.CompMasterEDIEmailAddress _
'                                    , .CompMasterEDIAcknowledgementRequested = d.CompMasterEDIAcknowledgementRequested _
'                                    , .CompMasterEDIAcceptOn997 = d.CompMasterEDIAcceptOn997 _
'                                    , .CompMasterEDIMethodOfPayment = d.CompMasterEDIMethodOfPayment _
'                                    , .CompMasterEDIUpdated = If(d.CompMasterEDIUpdated Is Nothing, New Byte() {}, d.CompMasterEDIUpdated)}
'    End Function

'    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
'        Return GetCompMasterEDIFiltered(Control:=CType(LinqTable, LTS.CompMasterEDI).CompMasterEDIControl)
'    End Function

'    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
'        Dim ret As DTO.QuickSaveResults
'        Using db As New NGLMASCompDataContext(ConnectionString)
'            Try
'                Dim source As LTS.CompMasterEDI = TryCast(LinqTable, LTS.CompMasterEDI)
'                If source Is Nothing Then Return Nothing
'                'Note this data source does not have a Mod Date or Mod User data field
'                ret = (From d In db.CompMasterEDIs
'                       Where d.CompMasterEDIControl = source.CompMasterEDIControl
'                       Select New DTO.QuickSaveResults With {.Control = d.CompMasterEDIControl _
'                                                            , .ModDate = Date.Now _
'                                                            , .ModUser = Parameters.UserName _
'                                                            , .Updated = d.CompMasterEDIUpdated.ToArray}).First

'            Catch ex As System.Data.SqlClient.SqlException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
'            Catch ex As InvalidOperationException
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
'            Catch ex As Exception
'                Utilities.SaveAppError(ex.Message, Me.Parameters)
'                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
'            End Try

'        End Using
'        Return ret
'    End Function

'#End Region

'End Class

Public Class NGLCompParameterData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompParameters
        Me.LinqDB = db
        Me.SourceClass = "NGLCompParameterData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompParameters
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
        Return GetCompParameterFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompParametersFiltered()
    End Function

    Public Function GetCompParameterFiltered(Optional ByVal Control As Integer = 0) As DTO.CompParameter
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompParameter As DTO.CompParameter = (
                    From t In db.CompParameters
                    Join p In db.Comps On t.CompParCompControl Equals p.CompControl
                    Where
                        (Control = 0 OrElse t.CompParControl = Control)
                    Order By t.CompParControl Descending
                    Select New DTO.CompParameter With {.CompParControl = t.CompParControl _
                                                , .CompParCompControl = t.CompParCompControl _
                                                , .CompParKey = t.CompParKey _
                                                , .CompParValue = t.CompParValue _
                                                , .CompParText = t.CompParText _
                                                , .CompParDescription = t.CompParDescription _
                                                , .CompParOLE = t.CompParOLE.ToArray() _
                                                , .CompParCategoryControl = t.CompParCategoryControl _
                                                , .CompNumber = p.CompNumber _
                                                , .CompParUpdated = t.CompParUpdated.ToArray()}).First
                Return CompParameter

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

    Public Function GetCompParametersFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompParameter()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompParameters() As DTO.CompParameter = (
                    From t In db.CompParameters
                    Join p In db.Comps On t.CompParCompControl Equals p.CompControl
                    Where
                        (CompControl = 0 OrElse t.CompParCompControl = CompControl)
                    Order By t.CompParControl
                    Select New DTO.CompParameter With {.CompParControl = t.CompParControl _
                                                , .CompParCompControl = t.CompParCompControl _
                                                , .CompParKey = t.CompParKey _
                                                , .CompParValue = t.CompParValue _
                                                , .CompParText = t.CompParText _
                                                , .CompParDescription = t.CompParDescription _
                                                , .CompParOLE = t.CompParOLE.ToArray() _
                                                , .CompParCategoryControl = t.CompParCategoryControl _
                                                , .CompNumber = p.CompNumber _
                                                , .CompParUpdated = t.CompParUpdated.ToArray()}).ToArray()
                Return CompParameters

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
    ''' Checks if a company level parameter exists
    ''' if found return company level parameter
    ''' if not found return the default global parameter
    ''' </summary>
    ''' <param name="parKey"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetParValue(ByVal parKey As String, ByVal compControl As Integer) As Double

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim dblVal = db.udfgetParValueByCompControl(parKey, compControl)

                Return If(dblVal.HasValue, dblVal.Value, 0)

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex,
                                        getSourceCaller("GetParValue"),
                                        "ParKey: " & parKey & " CompControl: " & compControl.ToString,
                                        sysErrorParameters.sysErrorSeverity.Unexpected,
                                        sysErrorParameters.sysErrorState.UserLevelFault)
            End Try

            Return 0

        End Using

    End Function

    ''' <summary>
    ''' Get company level parameter using multiple key values,  returns the first match found starting with company number.
    ''' </summary>
    ''' <param name="sParKey"></param>
    ''' <param name="sCompNumber"></param>
    ''' <param name="sCompAlpha"></param>
    ''' <param name="sCompLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 09/07/2017
    '''  new parameter lookup which attempts to find the compcontrol using various filters
    '''  in the following order of precedence: compNumber, compalphacode and legal entity combined 
    ''' </remarks>
    Public Function GetParValueUsingMultipleKeys(ByVal sParKey As String, ByVal sCompNumber As String, ByVal sCompAlpha As String, ByVal sCompLegalEntity As String) As Double
        Dim dblRet As Double = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim compControl As System.Nullable(Of Integer) = Nothing
                Dim intCompNbr As Integer = 0
                Dim blnFound As Boolean = False
                If Not String.IsNullOrWhiteSpace(sCompNumber) Then
                    If Integer.TryParse(sCompNumber, intCompNbr) Then
                        'we have a company number see if it exists
                        If db.Comps.Any(Function(x) x.CompNumber = intCompNbr) Then
                            blnFound = True
                            compControl = db.Comps.Where(Function(x) x.CompNumber = intCompNbr).Select(Function(x) x.CompControl).FirstOrDefault()
                        End If
                    End If
                End If
                If Not blnFound Then
                    'try to use the legal entity and alpha code
                    If Not String.IsNullOrWhiteSpace(sCompAlpha) AndAlso Not String.IsNullOrWhiteSpace(sCompLegalEntity) Then
                        If db.Comps.Any(Function(x) x.CompLegalEntity = sCompLegalEntity And x.CompAlphaCode = sCompAlpha) Then
                            blnFound = True
                            compControl = db.Comps.Where(Function(x) x.CompLegalEntity = sCompLegalEntity And x.CompAlphaCode = sCompAlpha).Select(Function(x) x.CompControl).FirstOrDefault()
                        End If
                    End If
                End If
                If blnFound AndAlso compControl.HasValue AndAlso compControl.Value <> 0 Then
                    dblRet = db.udfgetParValueByCompControl(sParKey, compControl.Value)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetParValueUsingMultipleKeys"), db)
            End Try
        End Using

        Return dblRet

    End Function

    ''' <summary>
    ''' Checks if a company level parameter exists
    ''' if found return company level parameter
    ''' if not found return the default global parameter
    ''' </summary>
    ''' <param name="parKey"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetParText(ByVal parKey As String, ByVal compControl As Integer) As String

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return db.udfgetParTextByCompControl(parKey, compControl)
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex,
                                        getSourceCaller("GetParText"),
                                        "ParKey: " & parKey & " CompControl: " & compControl.ToString,
                                        sysErrorParameters.sysErrorSeverity.Unexpected,
                                        sysErrorParameters.sysErrorState.UserLevelFault)
            End Try

            Return ""

        End Using

    End Function


#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompParameter)
        'Create New Record
        Return New LTS.CompParameter With {.CompParControl = d.CompParControl _
                                                , .CompParCompControl = d.CompParCompControl _
                                                , .CompParKey = d.CompParKey _
                                                , .CompParValue = d.CompParValue _
                                                , .CompParText = d.CompParText _
                                                , .CompParDescription = d.CompParDescription _
                                                , .CompParOLE = If(d.CompParOLE Is Nothing, New Byte() {}, d.CompParOLE) _
                                                , .CompParCategoryControl = d.CompParCategoryControl _
                                                , .CompParUpdated = If(d.CompParUpdated Is Nothing, New Byte() {}, d.CompParUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompParameterFiltered(Control:=CType(LinqTable, LTS.CompParameter).CompParControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompParameter = TryCast(LinqTable, LTS.CompParameter)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CompParameters
                       Where d.CompParControl = source.CompParControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompParControl _
                                                                , .ModDate = Date.Now _
                                                                , .ModUser = Parameters.UserName _
                                                                , .Updated = d.CompParUpdated.ToArray}).First

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

Public Class NGLCompTrackData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompTracks
        Me.LinqDB = db
        Me.SourceClass = "NGLCompTrackData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompTracks
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
        Return GetCompTrackFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompTracksFiltered()
    End Function

    Public Function GetCompTrackFiltered(Optional ByVal Control As Integer = 0) As DTO.CompTrack
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompTrack As DTO.CompTrack = (
                    From t In db.CompTracks
                    Where
                        (Control = 0 OrElse t.CompTrackControl = Control)
                    Order By t.CompTrackControl Descending
                    Select New DTO.CompTrack With {.CompTrackControl = t.CompTrackControl _
                                                , .CompTrackCompControl = t.CompTrackCompControl _
                                                , .CompTrackDate = t.CompTrackDate _
                                                , .CompTrackContact = t.CompTrackContact _
                                                , .CompTrackBetween = t.CompTrackBetween _
                                                , .CompTrackRegards = t.CompTrackRegards _
                                                , .CompTrackFollowUpOn = t.CompTrackFollowUpOn _
                                                , .CompTrackComment = t.CompTrackComment _
                                                , .CompTrackFollowUpOnComplete = t.CompTrackFollowUpOnComplete _
                                                , .CompTrackCompletionDate = t.CompTrackCompletionDate _
                                                , .CompTrackModDate = t.CompTrackModDate _
                                                , .CompTrackModUser = t.CompTrackModUser _
                                                , .CompTrackUpdated = t.CompTrackUpdated.ToArray()}).First
                Return CompTrack

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

    Public Function GetCompTracksFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompTrack()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompTracks() As DTO.CompTrack = (
                    From t In db.CompTracks
                    Where
                        (CompControl = 0 OrElse t.CompTrackCompControl = CompControl)
                    Order By t.CompTrackControl
                    Select New DTO.CompTrack With {.CompTrackControl = t.CompTrackControl _
                                                , .CompTrackCompControl = t.CompTrackCompControl _
                                                , .CompTrackDate = t.CompTrackDate _
                                                , .CompTrackContact = t.CompTrackContact _
                                                , .CompTrackBetween = t.CompTrackBetween _
                                                , .CompTrackRegards = t.CompTrackRegards _
                                                , .CompTrackFollowUpOn = t.CompTrackFollowUpOn _
                                                , .CompTrackComment = t.CompTrackComment _
                                                , .CompTrackFollowUpOnComplete = t.CompTrackFollowUpOnComplete _
                                                , .CompTrackCompletionDate = t.CompTrackCompletionDate _
                                                , .CompTrackModDate = t.CompTrackModDate _
                                                , .CompTrackModUser = t.CompTrackModUser _
                                                , .CompTrackUpdated = t.CompTrackUpdated.ToArray()}).ToArray()
                Return CompTracks

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompTrack)
        'Create New Record
        Return New LTS.CompTrack With {.CompTrackControl = d.CompTrackControl _
                                            , .CompTrackCompControl = d.CompTrackCompControl _
                                            , .CompTrackDate = d.CompTrackDate _
                                            , .CompTrackContact = d.CompTrackContact _
                                            , .CompTrackBetween = d.CompTrackBetween _
                                            , .CompTrackRegards = d.CompTrackRegards _
                                            , .CompTrackFollowUpOn = d.CompTrackFollowUpOn _
                                            , .CompTrackComment = d.CompTrackComment _
                                            , .CompTrackFollowUpOnComplete = d.CompTrackFollowUpOnComplete _
                                            , .CompTrackCompletionDate = d.CompTrackCompletionDate _
                                            , .CompTrackModDate = Date.Now _
                                            , .CompTrackModUser = Parameters.UserName _
                                            , .CompTrackUpdated = If(d.CompTrackUpdated Is Nothing, New Byte() {}, d.CompTrackUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompTrackFiltered(Control:=CType(LinqTable, LTS.CompTrack).CompTrackControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompTrack = TryCast(LinqTable, LTS.CompTrack)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CompTracks
                       Where d.CompTrackControl = source.CompTrackControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompTrackControl _
                                                                , .ModDate = d.CompTrackModDate _
                                                                , .ModUser = d.CompTrackModUser _
                                                                , .Updated = d.CompTrackUpdated.ToArray}).First

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

Public Class NGLOptimizerCompSettingsData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.Comps
        Me.LinqDB = db
        Me.SourceClass = "NGLOptimizerCompSettingsData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.Comps
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
        Return GetOptimizerCompSettingsFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetOptimizerCompSettingsFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0) As DTO.OptimizerCompSettings
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim OptimizerCompSettings As DTO.OptimizerCompSettings = (
                    From t In db.Comps
                    Where
                        (Control = 0 OrElse t.CompControl = Control) _
                        And
                        (Number = 0 OrElse t.CompNumber = Number)
                    Order By t.CompNumber Descending
                    Select New DTO.OptimizerCompSettings With {.CompControl = t.CompControl _
                                                , .CompLatitude = If(t.CompLatitude.HasValue, t.CompLatitude.Value, 0) _
                                                , .CompLongitude = If(t.CompLongitude.HasValue, t.CompLongitude.Value, 0) _
                                                , .CompName = t.CompName _
                                                , .CompNatName = t.CompNatName _
                                                , .CompNatNumber = If(t.CompNatNumber.HasValue, t.CompNatNumber.Value, 0) _
                                                , .CompNumber = If(t.CompNumber.HasValue, t.CompNumber.Value, 0) _
                                                , .CompStreetAddress1 = t.CompStreetAddress1 _
                                                , .CompStreetAddress2 = t.CompStreetAddress2 _
                                                , .CompStreetAddress3 = t.CompStreetAddress3 _
                                                , .CompStreetCity = t.CompStreetCity _
                                                , .CompStreetCountry = t.CompStreetCountry _
                                                , .CompStreetState = t.CompStreetState _
                                                , .CompStreetZip = t.CompStreetZip}).First
                Return OptimizerCompSettings

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = TryCast(oData, DTO.OptimizerCompSettings)
        'Create New Record
        Return New LTS.Comp With {.CompControl = t.CompControl _
                                                , .CompLatitude = t.CompLatitude _
                                                , .CompLongitude = t.CompLongitude _
                                                , .CompName = t.CompName _
                                                , .CompNatName = t.CompNatName _
                                                , .CompNatNumber = t.CompNatNumber _
                                                , .CompNumber = t.CompNumber _
                                                , .CompStreetAddress1 = t.CompStreetAddress1 _
                                                , .CompStreetAddress2 = t.CompStreetAddress2 _
                                                , .CompStreetAddress3 = t.CompStreetAddress3 _
                                                , .CompStreetCity = t.CompStreetCity _
                                                , .CompStreetCountry = t.CompStreetCountry _
                                                , .CompStreetState = t.CompStreetState _
                                                , .CompStreetZip = t.CompStreetZip}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetOptimizerCompSettingsFiltered(Control:=CType(LinqTable, LTS.Comp).CompControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow company records to be added from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow company records to be updated from this class
        Utilities.SaveAppError("Cannot save data.  Records cannot be updated using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

#End Region

End Class

Public Class NGLCompCreditData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.Comps
        Me.LinqDB = db
        Me.SourceClass = "NGLCompCreditData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.Comps
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
        Return GetCompCreditFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompCredits()
    End Function

    Public Function GetCompCreditFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0) As DTO.CompCredit
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompCredit As DTO.CompCredit = (
                    From t In db.Comps
                    Where
                        (Control = 0 OrElse t.CompControl = Control) _
                        And
                        (Number = 0 OrElse t.CompNumber = Number) _
                         And
                        (t.CompActive = True)
                    Order By t.CompName Ascending
                    Select New DTO.CompCredit With {.CompControl = t.CompControl _
                                                , .CompName = t.CompName _
                                                , .CompNumber = If(t.CompNumber.HasValue, t.CompNumber.Value, 0) _
                                                , .CompCreditAssigned = If(t.CompFinCreditLimit.HasValue, t.CompFinCreditLimit.Value, 0) _
                                                , .CompCreditUsed = If(t.CompFinCreditUsed.HasValue, t.CompFinCreditUsed.Value, 0) _
                                                , .CompCreditAvailable = If(t.CompFinCreditAvail.HasValue, t.CompFinCreditAvail.Value, 0)}).First
                Return CompCredit

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

    Public Function GetCompCredits(Optional ByVal page As Integer = 1,
                                      Optional ByVal pagesize As Integer = 1000) As DTO.CompCredit()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'Get the newest record that matches the provided criteria
                Dim oQuery = (
                    From t In db.Comps
                    Where
                        (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                        And
                        (t.CompActive = True)
                    Order By t.CompName Ascending
                    Select t)

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DTO.CompCredit = (
                        From d In oQuery
                        Order By d.CompName
                        Select selectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords

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

    Public Function CopyDTOToInheritedLinq(ByVal comp As DataTransferObjects.Comp, ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        'CompCredit really only updates fields in the Company table
        'this method copies the data from CompCredit to the supplied company object.
        Dim t = TryCast(oData, DTO.CompCredit)
        'Create New Record
        If Not comp.CompControl = t.CompControl Then Return Nothing
        comp.CompFinCreditLimit = t.CompCreditAssigned
        'the rest are not updatable
        Return comp
    End Function

#Region "TMS 365"

    ''' <summary>
    ''' Saves the credit limit for the company. 
    ''' Updates and calculates credit available based on the new credit limit value.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="CreditLimit"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
    '''  I created this method because there is no built in polymorphic add/insert/delete functions for this class
    '''  because the data does not have its own table - it is part of the Company table. The desktop sent the entire 
    '''  company object but for 365 I didn't want to be sending all that data to only update a few fields.
    ''' Modified By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
    '''  Added logic to update/recalculate CompFinCreditAvail when CompFinCreditLimit changes. 
    '''  This was done in the client in the Desktop application and I missed it originally. 
    '''  Moved it to the DAL so it would not be dependent on a client page
    ''' Modified By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
    '''  Changed the name of the function from UpdateCreditLimit to SaveCreditLimit to make it more clear what is happening
    '''  and to help differentiate it From UpdateCreditRoutine365 (the "Update Credit Now" button function)
    ''' </remarks>
    Public Function SaveCreditLimit(ByVal CompControl As Integer, ByVal CreditLimit As Integer) As Models.ResultObject
        Dim oResults As New Models.ResultObject() With {.Success = False}
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim comp = db.Comps.Where(Function(x) x.CompControl = CompControl).FirstOrDefault()
                If Not comp Is Nothing Then
                    comp.CompFinCreditAvail = (CreditLimit - comp.CompFinCreditUsed) 'Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                    comp.CompFinCreditLimit = CreditLimit
                    comp.CompModDate = Date.Now
                    comp.CompModUser = Parameters.UserName
                    db.SubmitChanges()
                    oResults.Success = True
                    oResults.SuccessMsg = oLocalize.GetLocalizedValueByKey("M_CreditLimitSaveSuccess", "Credit values have been saved. Click ""Update Credit Now!"" to apply the changes.")
                Else
                    oResults.ErrTitle = oLocalize.GetLocalizedValueByKey("E_CreditLimitSaveFail", "Save Credit Values Failed")
                    oResults.ErrMsg = String.Format(oLocalize.GetLocalizedValueByKey("E_InvalidParameterNameValue", "Invalid Parameter: No record exists in the database for {0}: {1}."), "CompanyControl", CompControl)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCreditLimit"), db)
            End Try
            Return oResults
        End Using
    End Function

    ''' <summary>
    ''' Save method called from the Company Finance page in 365
    ''' </summary>
    ''' <param name="CompFin"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold</remarks>
    Public Function SavevCompFin(ByVal CompFin As LTS.vCompFin) As Models.ResultObject
        Dim oResults As New Models.ResultObject() With {.Success = False}
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim compControl = CompFin.CompFinCompControl
                Dim comp = db.Comps.Where(Function(x) x.CompControl = compControl).FirstOrDefault()
                If Not comp Is Nothing Then
                    comp.CompFinCreditAvail = (CompFin.CompFinCreditLimit - CompFin.CompFinCreditUsed) 'Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                    comp.CompFinCreditLimit = CompFin.CompFinCreditLimit
                    comp.CompFinDuns = CompFin.CompFinDuns
                    comp.CompFinSIC = CompFin.CompFinSIC
                    comp.CompFinTaxID = CompFin.CompFinTaxID
                    comp.CompFinUser1 = CompFin.CompFinUser1
                    comp.CompFinUser2 = CompFin.CompFinUser2
                    comp.CompFinUser3 = CompFin.CompFinUser3
                    comp.CompFinUser4 = CompFin.CompFinUser4
                    comp.CompFinUser5 = CompFin.CompFinUser5
                    comp.CompFinCustomerSince = CompFin.CompFinCustomerSince
                    comp.CompFinCurType = CompFin.CompFinCurType
                    comp.CompFinInvPrnCode = CompFin.CompFinInvPrnCode
                    comp.CompFinInvEMailCode = CompFin.CompFinInvEMailCode
                    comp.CompFinBillToCompControl = CompFin.CompFinBillToCompControl
                    comp.CompMailTo = CompFin.CompMailTo
                    comp.CompFinYTDbookedCurr = CompFin.CompFinYTDbookedCurr
                    comp.CompFinYTDcarrierCurr = CompFin.CompFinYTDcarrierCurr
                    comp.CompFinYTDsavingsCurr = CompFin.CompFinYTDsavingsCurr
                    comp.CompFinYTDnoLoadsCurr = CompFin.CompFinYTDnoLoadsCurr
                    comp.CompFinYTDbookedLast = CompFin.CompFinYTDbookedLast
                    comp.CompFinYTDcarrierLast = CompFin.CompFinYTDcarrierLast
                    comp.CompFinYTDsavingsLast = CompFin.CompFinYTDsavingsLast
                    comp.CompFinYTDnoLoadsLast = CompFin.CompFinYTDnoLoadsLast
                    comp.CompFinPaymentDiscount = CompFin.CompFinPaymentDiscount
                    comp.CompFinPaymentDays = CompFin.CompFinPaymentDays
                    comp.CompFinPaymentNetDays = CompFin.CompFinPaymentNetDays
                    comp.CompFinCommTerms = CompFin.CompFinCommTerms
                    comp.CompFinCommTermsPer = CompFin.CompFinCommTermsPer
                    comp.CompFinPaymentForm = CompFin.CompFinPaymentForm
                    comp.CompFinCommCompControl = CompFin.CompFinCommCompControl
                    comp.CompFinBkhlFlatFee = CompFin.CompFinBkhlFlatFee
                    comp.CompFinBkhlCostPerc = CompFin.CompFinBkhlCostPerc
                    comp.CompFinCardType = CompFin.CompFinCardType
                    comp.CompFinCardName = CompFin.CompFinCardName
                    comp.CompFinCardExpires = CompFin.CompFinCardExpires
                    comp.CompFinCardAuthorizor = CompFin.CompFinCardAuthorizor
                    comp.CompFinCardAuthPassword = CompFin.CompFinCardAuthPassword
                    comp.CompModDate = Date.Now
                    comp.CompModUser = Parameters.UserName
                    db.SubmitChanges()
                    oResults.Success = True
                    oResults.SuccessMsg = oLocalize.GetLocalizedValueByKey("M_Success", "Success!")
                Else
                    oResults.ErrTitle = oLocalize.GetLocalizedValueByKey("SaveIncompleteMessage", "Save was unable to complete")
                    oResults.ErrMsg = oLocalize.GetLocalizedValueByKey("SaveIncompleteMessage", "Save was unable to complete")
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavevCompFin"), db)
            End Try
            Return oResults
        End Using
    End Function

    ''' <summary>
    ''' Method to Get the CompanyEDI Info based On Selected Company.
    ''' </summary>
    ''' <param name="RecordCount">No of Records</param>
    ''' <param name="filters">CompanyID(pk)</param>
    ''' <returns></returns>
    Public Function GetLECompsFins(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vCompFin()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vCompFin
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the user's Legal Entity (If a Legal Entity is provided use that else use the users associated LE)
                Dim leaControl As Integer = 0
                If filters.LEAdminControl <> 0 Then leaControl = filters.LEAdminControl Else leaControl = Parameters.UserLEControl
                Dim userLegalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = leaControl).Select(Function(y) y.LEAdminLegalEntity).FirstOrDefault()
                If String.IsNullOrWhiteSpace(userLegalEntity) Then Return Nothing
                'Get the companies the user is allowed to access
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vCompFin)
                'iQuery = db.vCompFins.Where(Function(x) x.CompLegalEntity = userLegalEntity)
                iQuery = (From t In db.vCompFins
                          Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompFinCompControl)) And t.CompLegalEntity = userLegalEntity
                          Select t)
                If iQuery Is Nothing Then Return Nothing
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompsFins"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Updated version of spUpdateCreditRoutine that is filtered by Legal Entity and includes an optional parameter to process a single company record
    ''' </summary>
    ''' <param name="LEAControl"></param>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold</remarks>
    Private Function UpdateCreditRoutine365(ByVal LEAControl As Integer, Optional ByVal CompControl As Integer = 0) As LTS.spUpdateCreditRoutine365Result
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return db.spUpdateCreditRoutine365(LEAControl, CompControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateCreditRoutine365"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Wrapper method that updates the Credit for a single company
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold</remarks>
    Public Function UpdateCreditRoutine365Single(CompControl As Integer) As LTS.spUpdateCreditRoutine365Result
        Return UpdateCreditRoutine365(0, CompControl)
    End Function

    ''' <summary>
    ''' Wrapper method that updates the Credit for all companies associated with the Legal Entity
    ''' </summary>
    ''' <param name="LEAControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold</remarks>
    Public Function UpdateCreditRoutine365LE(LEAControl As Integer) As LTS.spUpdateCreditRoutine365Result
        Return UpdateCreditRoutine365(LEAControl, 0)
    End Function

    ''' <summary>
    ''' Gets the credit info for the company
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold</remarks>
    Public Function GetvLoadBoardAdjustCredit(ByVal CompControl As Integer) As LTS.vLoadBoardAdjustCredit
        Dim oRet As New LTS.vLoadBoardAdjustCredit()
        If CompControl = 0 Then Return oRet 'nothing to do
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                oRet = db.vLoadBoardAdjustCredits.Where(Function(x) x.CompControl = CompControl).FirstOrDefault()
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvLoadBoardAdjustCredit"), db)
            End Try
        End Using
        Return oRet
    End Function

#End Region

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = TryCast(oData, DTO.CompCredit)
        'Create New Record
        Return New LTS.Comp With {.CompControl = t.CompControl _
                                               , .CompName = t.CompName _
                                                , .CompNumber = t.CompNumber _
                                                , .CompFinCreditLimit = t.CompCreditAssigned _
                                                , .CompFinCreditUsed = t.CompCreditUsed _
                                                , .CompFinCreditAvail = t.CompCreditAvailable}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompCreditFiltered(Control:=CType(LinqTable, LTS.Comp).CompControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow company records to be added from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        Return
        ''We do not allow company records to be updated from this class
        'Utilities.SaveAppError("Cannot save data.  Records cannot be updated using this interface!", Me.Parameters)
        'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Friend Function selectDTOData(ByVal t As LTS.Comp, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CompCredit
        Return New DTO.CompCredit With {.CompControl = t.CompControl _
                                                , .CompName = t.CompName _
                                        , .CompNumber = If(t.CompNumber.HasValue, t.CompNumber.Value, 0) _
                                         , .CompCreditAssigned = If(t.CompFinCreditLimit.HasValue, t.CompFinCreditLimit.Value, 0) _
                                        , .CompCreditUsed = If(t.CompFinCreditUsed.HasValue, t.CompFinCreditUsed.Value, 0) _
                                       , .CompCreditAvailable = If(t.CompFinCreditAvail.HasValue, t.CompFinCreditAvail.Value, 0) _
                                                   , .page = page _
                                                     , .Pages = pagecount _
                                                     , .recordcount = recordcount _
                                                      , .pagesize = pagesize}
    End Function

#End Region

End Class

Public Class NGLCompDockDoorData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompDockDoors
        Me.LinqDB = db
        Me.SourceClass = "NGLCompDockDoorData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.CompDockDoors
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
        Return GetCompDockDoorFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompDockDoorsFiltered()
    End Function

    ''' <summary>
    ''' CreateNew DTO.CompDockDoor from LTS.CompDockdoor.
    ''' The title is misleading because it does not actually create any records in the database
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 6/27/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''  Added new field CompDockInbound
    ''' </remarks>
    Private Function CreateNew(ByVal t As LTS.CompDockDoor) As DTO.CompDockDoor
        Return New DTO.CompDockDoor With {.CompDockCompControl = t.CompDockCompControl _
                                         , .CompDockControl = t.CompDockContol _
                                         , .CompDockDockDoorID = t.DockDoorID _
                                         , .CompDockDockDoorName = t.DockDoorName _
                                         , .CompDockBookingSeq = t.CompDockBookingSeq _
                                         , .CompDockValidation = t.CompDockValidation _
                                         , .CompDockOverrideAlert = t.CompDockOverrideAlert _
                                         , .CompDockNotificationAlert = t.CompDockNotificationAlert _
                                         , .CompDockNotificationEmail = t.CompDockNotificationEmail _
                                         , .CompDockInbound = t.CompDockInbound _
                                         , .CompDockDoorModUser = t.CompDockDoorModUser _
                                         , .CompDockDoorModDate = t.CompDockDoorModDate _
                                         , .CompDockDoorUpdated = t.CompDockDoorUpdated.ToArray()}
    End Function

    Public Function GetCompDockDoorsFiltered() As DTO.CompDockDoor()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim docks() As DTO.CompDockDoor = (
                        From t In db.CompDockDoors
                        Order By t.DockDoorID
                        Select CreateNew(t)).ToArray()
                Return docks

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetCompDockDoorFiltered(ByVal Control As Integer) As DTO.CompDockDoor
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim dock As DTO.CompDockDoor = (
                    From t In db.CompDockDoors
                    Where
                        (t.CompDockContol = If(Control = 0, t.CompDockContol, Control))
                    Select CreateNew(t)).Single
                Return dock

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

    '***** CALLED BY GetDockDoors() IN vComp.cs *****
    ''' <summary>
    ''' CALLED BY GetDockDoors() IN vComp.cs 
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''  Added field CompDockInbound
    ''' </remarks>
    Public Function GetCompDockDoorsFiltered(ByVal CompControl As Integer) As DTO.CompDockDoor()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim docks As DTO.CompDockDoor() = (
                    From t In db.CompDockDoors
                    Join s In db.tblDockSettings On t.CompDockContol Equals s.DockSettingCompDockContol
                    Where
                        t.CompDockCompControl = CompControl And s.DockSettingEnumID = NGLDockSettingData.DockSetting.DefaultApptMins
                    Select New DTO.CompDockDoor With {.CompDockCompControl = t.CompDockCompControl _
                                             , .CompDockControl = t.CompDockContol _
                                             , .CompDockDockDoorID = t.DockDoorID _
                                             , .CompDockDockDoorName = t.DockDoorName _
                                             , .CompDockBookingSeq = t.CompDockBookingSeq _
                                             , .CompDockValidation = t.CompDockValidation _
                                             , .CompDockOverrideAlert = t.CompDockOverrideAlert _
                                             , .CompDockNotificationAlert = t.CompDockNotificationAlert _
                                             , .CompDockNotificationEmail = t.CompDockNotificationEmail _
                                             , .CompDockInbound = t.CompDockInbound _
                                             , .CompDockDoorModUser = t.CompDockDoorModUser _
                                             , .CompDockDoorModDate = t.CompDockDoorModDate _
                                             , .CompDockDoorUpdated = t.CompDockDoorUpdated.ToArray() _
                                             , .AvgApptTime = If(Integer.TryParse(s.DockSettingFixed, New Integer), Integer.Parse(s.DockSettingFixed), 30)}).ToArray()
                Return docks
            Catch ex As SqlException
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
    ''' Uses CompControl and DockID to return the DockName.
    ''' If DockName is null or empty then DockID is returned
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="DockID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/12/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetCompDockName(ByVal CompControl As Integer, ByVal DockID As String) As String
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim strRet As String = ""
                strRet = (From t In db.CompDockDoors Where t.CompDockCompControl = CompControl And t.DockDoorID = DockID Select t.DockDoorName).FirstOrDefault()
                If String.IsNullOrWhiteSpace(strRet) Then strRet = DockID
                Return strRet
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
    ''' Checks to make sure the DookDoorID does not already exist, inserts the record, creates some default settings for the dock, and 
    ''' returns the DTO object of the newly created dock.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="DockDoorID"></param>
    ''' <param name="DockDoorName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''  Added field CompDockInbound
    ''' </remarks>
    Public Function AddNewResource365(ByVal CompControl As Integer,
                                          ByVal DockDoorID As String,
                                          ByVal DockDoorName As String,
                                          ByVal BookingSeq As Integer,
                                          ByVal blnValidation As Boolean,
                                          ByVal blnOverrideAlert As Boolean,
                                          ByVal blnNotificationAlert As Boolean,
                                          ByVal NotificationEmail As String,
                                          ByVal blnDockInbound As Boolean) As DTO.CompDockDoor
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'add the dock following the old rules but also have to create all the settings
                If db.CompDockDoors.Any(Function(x) x.CompDockCompControl = CompControl And x.DockDoorID.ToUpper() = DockDoorID.ToUpper()) Then
                    Utilities.SaveAppError("Record cannot be created.  Cannot add another dock door with same ID for the same company.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CannotInsertDockDoorValidation"}, New FaultReason("E_DataValidationFailure"))
                End If
                Dim oRecord As New LTS.CompDockDoor With {.CompDockCompControl = CompControl _
                                             , .DockDoorID = DockDoorID _
                                             , .DockDoorName = DockDoorName _
                                             , .CompDockBookingSeq = BookingSeq _
                                             , .CompDockValidation = blnValidation _
                                             , .CompDockOverrideAlert = blnOverrideAlert _
                                             , .CompDockNotificationAlert = blnNotificationAlert _
                                             , .CompDockNotificationEmail = NotificationEmail _
                                             , .CompDockInbound = blnDockInbound _
                                             , .CompDockDoorModUser = Parameters.UserName _
                                             , .CompDockDoorModDate = Date.Now}
                db.CompDockDoors.InsertOnSubmit(oRecord)
                db.SubmitChanges()
                Try
                    Dim oDS As New NGLDockSettingData(Parameters)
                    oDS.CreateDefaultDockSettings(db, oRecord.CompDockContol)
                Catch ex As Exception
                    'basically ignore these errors
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                End Try
                Return CreateNew(oRecord)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddNewResource365"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Copies the configuration and settings from an existing Dock to either a new or preexisting Dock
    ''' If copying to a New resource, set CopyToNew = true, CopyToDockControl to 0, and pass through the DockDoorID and DockDoorName
    ''' If copying to an existing resource, set CopyToNew = false, DockDoorID and DockDoorName to "", and pass through CopyToDockControl
    ''' If spCopyResourceSettingsResult.ErrNumber != 0 then the procedure failed -- show the spCopyResourceSettingsResult.RetMsg to the user
    ''' If spCopyResourceSettingsResult.ErrNumber = 0 then success
    ''' </summary>
    ''' <param name="CopyToDockControl"></param>
    ''' <param name="CopyFromDockControl"></param>
    ''' <param name="CopyToNew"></param>
    ''' <param name="DockDoorID"></param>
    ''' <param name="DockDoorName"></param>
    ''' <returns>LTS.spCopyResourceSettingsResult</returns>
    ''' <remarks>
    ''' Added By LVV on 7/2/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function CopyResourceSettings(ByVal CopyToDockControl As Integer, ByVal CopyFromDockControl As Integer, ByVal CopyToNew As Boolean, ByVal DockDoorID As String, ByVal DockDoorName As String) As LTS.spCopyResourceSettingsResult
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If CopyToNew Then
                    'Make sure we can add this DockDoorID to the CompControl of the CopyFromDock
                    Dim compControl = db.CompDockDoors.Where(Function(x) x.CompDockContol = CopyFromDockControl).Select(Function(y) y.CompDockCompControl).FirstOrDefault()
                    If db.CompDockDoors.Any(Function(x) x.CompDockCompControl = compControl And x.DockDoorID.ToUpper() = DockDoorID.ToUpper()) Then
                        Utilities.SaveAppError("Record cannot be created.  Cannot add another dock door with same ID for the same company.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CannotInsertDockDoorValidation"}, New FaultReason("E_DataValidationFailure"))
                    End If
                End If
                Return db.spCopyResourceSettings(CopyToDockControl, CopyFromDockControl, CopyToNew, DockDoorID, DockDoorName, Parameters.UserName).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CopyResourceSettings"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets all the docks for the provided CompControl EXCEPT the one with CompDockContol = CopyFromDockControl
    ''' This is because you cannot copy settings from a dock to itself
    ''' Return values DTO.vLookupList: Control = CompDockContol, Name = DockDoorName, Description = t.DockDoorID
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="CopyFromDockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/3/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetCopyToExisingDocks(ByVal CompControl As Integer, ByVal CopyFromDockControl As Integer) As DTO.vLookupList()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim docks As DTO.vLookupList() = (
                        From t In db.CompDockDoors
                        Where
                            t.CompDockCompControl = CompControl AndAlso t.CompDockContol <> CopyFromDockControl
                        Select New DTO.vLookupList With {.Control = t.CompDockContol, .Name = t.DockDoorName, .Description = t.DockDoorID}).ToArray()
                Return docks
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCopyToExisingDocks"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns true on success, returns false on fail. 
    ''' If returns false, the fail reason will be passed back through reference parameter strMsg
    ''' </summary>
    ''' <param name="strMsg"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="newPwd"></param>
    ''' <param name="confirmPwd"></param>
    ''' <remarks>
    ''' Added By LVV on 7/2/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    ''' <returns></returns>
    Public Function ChangeOverridesPwd(ByRef strMsg As String, ByVal DockControl As Integer, ByVal newPwd As String, ByVal confirmPwd As String) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                '** TODO LVV ** Do I need to add logic to check if the user is a LEAdmin/Super/has permissions to run the procedure? (Since this is not yet in CM)
                If newPwd.Length < 6 OrElse newPwd.Length > 20 Then
                    strMsg = String.Format(oLocalize.GetLocalizedValueByKey("E_InvalidNewPwdLength", "The new password is not valid. Passwords must be between {0} and {1} characters long."), 6, 20)
                    Return blnRet
                End If
                If Not String.Equals(newPwd, confirmPwd) Then
                    strMsg = oLocalize.GetLocalizedValueByKey("PasswordNotMatch", "The passwords do not match")
                    Return blnRet
                End If
                Dim epass = DTran.Encrypt(newPwd, "NGL")
                Dim ltsDock = db.CompDockDoors.Where(Function(x) x.CompDockContol = DockControl).FirstOrDefault()
                ltsDock.CompDockOverridePwd = epass
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ChangeOverridesPwd"), db)
            End Try
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Get the dock door for this appointment
    ''' </summary>
    ''' <param name="Appointment"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR for v-8.3.0.001 on 09/27/2020
    '''     new method that can be called from the AMS module directly
    ''' </remarks>
    Public Function GetCompDockDoorForAppt(ByVal Appointment As DTO.AMSAppointment) As LTS.CompDockDoor
        Dim oRet As New LTS.CompDockDoor()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                oRet = db.CompDockDoors.Where(Function(x) x.DockDoorID = Appointment.AMSApptDockdoorID And x.CompDockCompControl = Appointment.AMSApptCompControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompDockDoorForAppt"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Do not Use the DockDoorID parameter is not unique
    ''' </summary>
    ''' <param name="DockDoorID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  Depreciated because the DockDoorID parameter is not unique
    ''' </remarks>
    Public Function GetCompDockDoorByID(ByVal DockDoorID As String) As LTS.CompDockDoor
        throwDepreciatedException("Do not Use GetCompDockDoorByID the DockDoorID parameter is not unique.")
        Return Nothing
        'Using db As New NGLMASCompDataContext(ConnectionString)
        '    Return GetCompDockDoorByID(db, DockDoorID)
        'End Using
    End Function


    ''' <summary>
    ''' Do not Use the DockDoorID parameter is not unique
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="DockDoorID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  Depreciated because the DockDoorID parameter is not unique
    ''' </remarks>
    Public Function GetCompDockDoorByID(ByRef db As NGLMASCompDataContext, ByVal DockDoorID As String) As LTS.CompDockDoor

        throwDepreciatedException("Do not Use GetCompDockDoorByID the DockDoorID parameter is not unique.")
        'Try
        '    Return db.CompDockDoors.Where(Function(x) x.DockDoorID = DockDoorID).FirstOrDefault()
        'Catch ex As Exception
        '    ManageLinqDataExceptions(ex, buildProcedureName("GetCompDockDoorByID"), db)
        'End Try
        Return Nothing
    End Function

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' CopyDTOToLinq
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 6/27/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''  Added field CompDockInbound
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompDockDoor)
        'Create New Record
        Return New LTS.CompDockDoor With {.CompDockCompControl = d.CompDockCompControl _
                                                           , .CompDockContol = d.CompDockControl _
                                                           , .DockDoorID = d.CompDockDockDoorID _
                                                           , .DockDoorName = d.CompDockDockDoorName _
                                                           , .CompDockBookingSeq = d.CompDockBookingSeq _
                                                           , .CompDockValidation = d.CompDockValidation _
                                                           , .CompDockOverrideAlert = d.CompDockOverrideAlert _
                                                           , .CompDockOverridePwd = d.CompDockOverridePwd _
                                                           , .CompDockNotificationAlert = d.CompDockNotificationAlert _
                                                           , .CompDockNotificationEmail = d.CompDockNotificationEmail _
                                                           , .CompDockInbound = d.CompDockInbound _
                                                           , .CompDockDoorModDate = Date.Now _
                                                           , .CompDockDoorModUser = Parameters.UserName _
                                                           , .CompDockDoorUpdated = If(d.CompDockDoorUpdated Is Nothing, New Byte() {}, d.CompDockDoorUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompDockDoorFiltered(Control:=CType(LinqTable, LTS.CompDockDoor).CompDockContol)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompDockDoor = TryCast(LinqTable, LTS.CompDockDoor)
                If source Is Nothing Then Return Nothing
                ret = (From d In db.CompDockDoors
                       Where d.CompDockContol = source.CompDockContol
                       Select New DTO.QuickSaveResults With {.Control = d.CompDockContol _
                                                                , .ModDate = d.CompDockDoorModDate _
                                                                , .ModUser = d.CompDockDoorModUser _
                                                                , .Updated = d.CompDockDoorUpdated.ToArray}).First
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

    ''' <summary>
    ''' ValidateDeletedRecord 
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get appt is not valid with only the DockDoorID
    '''  compcontrol is required
    ''' </remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Dim oDataObject As DTO.CompDockDoor = Nothing
            Dim appt As LTS.AMSAppointment = Nothing
            Try
                '   Dim source As LTS.CompDockdoor = TryCast(LinqTable, LTS.CompDockdoor)
                oDataObject = TryCast(oData, DTO.CompDockDoor)
                'Modified by RHR for v-8.2.1 on 10/16/2019 
                appt = (From d In db.AMSAppointments
                        Where d.AMSApptDockdoorID.ToUpper = oDataObject.CompDockDockDoorID.ToUpper AndAlso d.AMSApptCompControl = oDataObject.CompDockCompControl).FirstOrDefault

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Return
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            If (appt IsNot Nothing) Then
                If appt.AMSApptControl <> 0 Then
                    Utilities.SaveAppError("Record cannot be deleted while there is an appointment associated with the dock door!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CannotDeleteDockDoorValidation", .Details = "Record cannot be deleted while there is an appointment associated with the dock door!"}, New FaultReason("E_DataValidationFailure"))
                End If
            End If
        End Using
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow records to be updated from this class
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim oDataObject As DTO.CompDockDoor = Nothing
            Dim sourceappt As LTS.CompDockDoor = Nothing
            Try
                oDataObject = TryCast(oData, DTO.CompDockDoor)
                sourceappt = (From d In db.CompDockDoors
                              Where d.CompDockContol = oDataObject.CompDockControl).FirstOrDefault
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Return
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            If sourceappt Is Nothing Then Return
            If sourceappt.CompDockContol <> 0 Then
                'compare new verse old.'
                If Not sourceappt.DockDoorID.ToUpper.Equals(oDataObject.CompDockDockDoorID.ToUpper) Then
                    Utilities.SaveAppError("Record cannot be updated.  Cannot change DockDoorID field!", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CannotUpdateDoockDoorIDValidation"}, New FaultReason("E_DataValidationFailure"))
                End If
            End If
        End Using
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim sourceappt As LTS.CompDockDoor = Nothing
            Try
                Dim oDataObject As DTO.CompDockDoor = TryCast(oData, DTO.CompDockDoor)
                sourceappt = (From d In db.CompDockDoors
                              Where d.CompDockCompControl = oDataObject.CompDockCompControl And d.DockDoorID.ToUpper = oDataObject.CompDockDockDoorID.ToUpper).FirstOrDefault
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
                Return
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            If sourceappt Is Nothing Then Return
            If sourceappt.CompDockContol <> 0 Then
                'compare new verse old.'
                Utilities.SaveAppError("Record cannot be created.  Cannot add another dock door with same ID for the same company.", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CannotInsertDockDoorValidation"}, New FaultReason("E_DataValidationFailure"))
            End If
        End Using
    End Sub

#End Region

End Class

Public Class NGLCompAMSApptTrackingSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompAMSApptTrackingSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLCompAMSApptTrackingSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.CompAMSApptTrackingSettings
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
        Return GetCompAMSApptTrackingSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompAMSApptTrackingSettingsFiltered()
    End Function

    Public Function GetCompAMSApptTrackingSettingFiltered(Optional ByVal Control As Integer = 0) As DTO.CompAMSApptTrackingSetting
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompAMSApptTrackingSetting As DTO.CompAMSApptTrackingSetting = (
                    From d In db.CompAMSApptTrackingSettings
                    Where
                        (Control = 0 OrElse d.CompAMSApptTrackingSettingControl = Control)
                    Order By d.CompAMSApptTrackingSettingControl Descending
                    Select selectDTOData(d, db)).First
                Return CompAMSApptTrackingSetting

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

    Public Function GetCompAMSApptTrackingSettingsFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompAMSApptTrackingSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria sorted by name
                Dim CompAMSApptTrackingSettings() As DTO.CompAMSApptTrackingSetting = (
                    From d In db.CompAMSApptTrackingSettings
                    Where
                        (CompControl = 0 OrElse d.CompAMSApptTrackingSettingCompControl = CompControl)
                    Order By d.CompAMSApptTrackingSettingName
                    Select selectDTOData(d, db)).ToArray()
                Return CompAMSApptTrackingSettings

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompAMSApptTrackingSetting)
        'Create New Record
        Return New LTS.CompAMSApptTrackingSetting With {.CompAMSApptTrackingSettingControl = d.CompAMSApptTrackingSettingControl _
                                                           , .CompAMSApptTrackingSettingCompControl = d.CompAMSApptTrackingSettingCompControl _
                                                           , .CompAMSApptTrackingSettingName = d.CompAMSApptTrackingSettingName _
                                                           , .CompAMSApptTrackingSettingDesc = d.CompAMSApptTrackingSettingDesc _
                                                           , .CompAMSApptTrackingSettingModDate = Date.Now _
                                                           , .CompAMSApptTrackingSettingModUser = Parameters.UserName _
                                                           , .CompAMSApptTrackingSettingUpdated = If(d.CompAMSApptTrackingSettingUpdated Is Nothing, New Byte() {}, d.CompAMSApptTrackingSettingUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompAMSApptTrackingSettingFiltered(Control:=CType(LinqTable, LTS.CompAMSApptTrackingSetting).CompAMSApptTrackingSettingControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompAMSApptTrackingSetting = TryCast(LinqTable, LTS.CompAMSApptTrackingSetting)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CompAMSApptTrackingSettings
                       Where d.CompAMSApptTrackingSettingControl = source.CompAMSApptTrackingSettingControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompAMSApptTrackingSettingControl _
                                                                , .ModDate = d.CompAMSApptTrackingSettingModDate _
                                                                , .ModUser = d.CompAMSApptTrackingSettingModUser _
                                                                , .Updated = d.CompAMSApptTrackingSettingUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.CompAMSApptTrackingSetting, ByRef db As NGLMASCompDataContext) As DTO.CompAMSApptTrackingSetting
        Return New DTO.CompAMSApptTrackingSetting With {.CompAMSApptTrackingSettingControl = d.CompAMSApptTrackingSettingControl _
                                                           , .CompAMSApptTrackingSettingCompControl = d.CompAMSApptTrackingSettingCompControl _
                                                           , .CompAMSApptTrackingSettingName = d.CompAMSApptTrackingSettingName _
                                                           , .CompAMSApptTrackingSettingDesc = d.CompAMSApptTrackingSettingDesc _
                                                           , .CompAMSApptTrackingSettingModDate = d.CompAMSApptTrackingSettingModDate _
                                                           , .CompAMSApptTrackingSettingModUser = d.CompAMSApptTrackingSettingModUser _
                                                           , .CompAMSApptTrackingSettingUpdated = d.CompAMSApptTrackingSettingUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLCompAMSUserFieldSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompAMSUserFieldSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLCompAMSUserFieldSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.CompAMSUserFieldSettings
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
        Return GetCompAMSUserFieldSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompAMSUserFieldSettingsFiltered()
    End Function

    Public Function GetCompAMSUserFieldSettingFiltered(Optional ByVal Control As Integer = 0) As DTO.CompAMSUserFieldSetting
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompAMSUserFieldSetting As DTO.CompAMSUserFieldSetting = (
                    From d In db.CompAMSUserFieldSettings
                    Where
                        (Control = 0 OrElse d.CompAMSUserFieldSettingControl = Control)
                    Order By d.CompAMSUserFieldSettingControl Descending
                    Select selectDTOData(d, db)).First
                Return CompAMSUserFieldSetting

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

    Public Function GetCompAMSUserFieldSettingsFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompAMSUserFieldSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria sorted by name
                Dim CompAMSUserFieldSettings() As DTO.CompAMSUserFieldSetting = (
                    From d In db.CompAMSUserFieldSettings
                    Where
                        (CompControl = 0 OrElse d.CompAMSUserFieldSettingCompControl = CompControl)
                    Order By d.CompAMSUserFieldSettingFieldName
                    Select selectDTOData(d, db)).ToArray()
                Return CompAMSUserFieldSettings

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompAMSUserFieldSetting)
        'Create New Record
        Return New LTS.CompAMSUserFieldSetting With {.CompAMSUserFieldSettingControl = d.CompAMSUserFieldSettingControl _
                                                           , .CompAMSUserFieldSettingCompControl = d.CompAMSUserFieldSettingCompControl _
                                                           , .CompAMSUserFieldSettingFieldName = d.CompAMSUserFieldSettingFieldName _
                                                           , .CompAMSUserFieldSettingFieldDesc = d.CompAMSUserFieldSettingFieldDesc _
                                                           , .CompAMSUserFieldSettingModDate = Date.Now _
                                                           , .CompAMSUserFieldSettingModUser = Parameters.UserName _
                                                           , .CompAMSUserFieldSettingUpdated = If(d.CompAMSUserFieldSettingUpdated Is Nothing, New Byte() {}, d.CompAMSUserFieldSettingUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompAMSUserFieldSettingFiltered(Control:=CType(LinqTable, LTS.CompAMSUserFieldSetting).CompAMSUserFieldSettingControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompAMSUserFieldSetting = TryCast(LinqTable, LTS.CompAMSUserFieldSetting)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CompAMSUserFieldSettings
                       Where d.CompAMSUserFieldSettingControl = source.CompAMSUserFieldSettingControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompAMSUserFieldSettingControl _
                                                                , .ModDate = d.CompAMSUserFieldSettingModDate _
                                                                , .ModUser = d.CompAMSUserFieldSettingModUser _
                                                                , .Updated = d.CompAMSUserFieldSettingUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.CompAMSUserFieldSetting, ByRef db As NGLMASCompDataContext) As DTO.CompAMSUserFieldSetting
        Return New DTO.CompAMSUserFieldSetting With {.CompAMSUserFieldSettingControl = d.CompAMSUserFieldSettingControl _
                                                           , .CompAMSUserFieldSettingCompControl = d.CompAMSUserFieldSettingCompControl _
                                                           , .CompAMSUserFieldSettingFieldName = d.CompAMSUserFieldSettingFieldName _
                                                           , .CompAMSUserFieldSettingFieldDesc = d.CompAMSUserFieldSettingFieldDesc _
                                                           , .CompAMSUserFieldSettingModDate = d.CompAMSUserFieldSettingModDate _
                                                           , .CompAMSUserFieldSettingModUser = d.CompAMSUserFieldSettingModUser _
                                                           , .CompAMSUserFieldSettingUpdated = d.CompAMSUserFieldSettingUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLCompAMSColorCodeSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.CompAMSColorCodeSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLCompAMSColorCodeSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.CompAMSColorCodeSettings
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
        Return GetCompAMSColorCodeSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCompAMSColorCodeSettingsFiltered()
    End Function

    Public Function GetCompAMSColorCodeSettingFiltered(Optional ByVal Control As Integer = 0) As DTO.CompAMSColorCodeSetting
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CompAMSColorCodeSetting As DTO.CompAMSColorCodeSetting = (
                    From d In db.CompAMSColorCodeSettings
                    Where
                        (Control = 0 OrElse d.CompAMSColorCodeSettingControl = Control)
                    Order By d.CompAMSColorCodeSettingControl Descending
                    Select selectDTOData(d, db)).First
                Return CompAMSColorCodeSetting

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

    Public Function GetCompAMSColorCodeSettingsFiltered(Optional ByVal CompControl As Integer = 0) As DTO.CompAMSColorCodeSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria sorted by name
                Dim CompAMSColorCodeSettings() As DTO.CompAMSColorCodeSetting = (
                    From d In db.CompAMSColorCodeSettings
                    Where
                        (CompControl = 0 OrElse d.CompAMSColorCodeSettingCompControl = CompControl)
                    Order By d.CompAMSColorCodeSettingFieldName
                    Select selectDTOData(d, db)).ToArray()
                Return CompAMSColorCodeSettings

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

    Public Function GetCompAMSColorCodeSettingsFiltered(ByVal CompControl As Integer, ByVal colorType As Integer) As DTO.CompAMSColorCodeSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Return all the records that match the criteria sorted by name
                Dim CompAMSColorCodeSettings() As DTO.CompAMSColorCodeSetting = (
                    From d In db.CompAMSColorCodeSettings
                    Where
                        (CompControl = 0 OrElse d.CompAMSColorCodeSettingCompControl = CompControl) _
                        And d.CompAMSColorCodeSettingType = colorType
                    Order By d.CompAMSColorCodeSettingFieldName
                    Select selectDTOData(d, db)).ToArray()
                Return CompAMSColorCodeSettings

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.CompAMSColorCodeSetting)
        'Create New Record
        Return New LTS.CompAMSColorCodeSetting With {.CompAMSColorCodeSettingControl = d.CompAMSColorCodeSettingControl _
                                                           , .CompAMSColorCodeSettingCompControl = d.CompAMSColorCodeSettingCompControl _
                                                           , .CompAMSColorCodeSettingType = d.CompAMSColorCodeSettingType _
                                                           , .CompAMSColorCodeSettingKey = d.CompAMSColorCodeSettingKey _
                                                           , .CompAMSColorCodeSettingColorCode = d.CompAMSColorCodeSettingColorCode _
                                                           , .CompAMSColorCodeSettingFieldName = d.CompAMSColorCodeSettingFieldName _
                                                           , .CompAMSColorCodeSettingFieldDesc = d.CompAMSColorCodeSettingFieldDesc _
                                                           , .CompAMSColorCodeSettingModDate = Date.Now _
                                                           , .CompAMSColorCodeSettingModUser = Parameters.UserName _
                                                           , .CompAMSColorCodeSettingUpdated = If(d.CompAMSColorCodeSettingUpdated Is Nothing, New Byte() {}, d.CompAMSColorCodeSettingUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompAMSColorCodeSettingFiltered(Control:=CType(LinqTable, LTS.CompAMSColorCodeSetting).CompAMSColorCodeSettingControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim source As LTS.CompAMSColorCodeSetting = TryCast(LinqTable, LTS.CompAMSColorCodeSetting)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CompAMSColorCodeSettings
                       Where d.CompAMSColorCodeSettingControl = source.CompAMSColorCodeSettingControl
                       Select New DTO.QuickSaveResults With {.Control = d.CompAMSColorCodeSettingControl _
                                                                , .ModDate = d.CompAMSColorCodeSettingModDate _
                                                                , .ModUser = d.CompAMSColorCodeSettingModUser _
                                                                , .Updated = d.CompAMSColorCodeSettingUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.CompAMSColorCodeSetting, ByRef db As NGLMASCompDataContext) As DTO.CompAMSColorCodeSetting
        Return New DTO.CompAMSColorCodeSetting With {.CompAMSColorCodeSettingControl = d.CompAMSColorCodeSettingControl _
                                                           , .CompAMSColorCodeSettingCompControl = d.CompAMSColorCodeSettingCompControl _
                                                           , .CompAMSColorCodeSettingType = d.CompAMSColorCodeSettingType _
                                                           , .CompAMSColorCodeSettingKey = d.CompAMSColorCodeSettingKey _
                                                           , .CompAMSColorCodeSettingColorCode = d.CompAMSColorCodeSettingColorCode _
                                                           , .CompAMSColorCodeSettingFieldName = d.CompAMSColorCodeSettingFieldName _
                                                           , .CompAMSColorCodeSettingFieldDesc = d.CompAMSColorCodeSettingFieldDesc _
                                                           , .CompAMSColorCodeSettingModDate = d.CompAMSColorCodeSettingModDate _
                                                           , .CompAMSColorCodeSettingModUser = d.CompAMSColorCodeSettingModUser _
                                                           , .CompAMSColorCodeSettingUpdated = d.CompAMSColorCodeSettingUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLCompAMSSettingsData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.Comps
        Me.LinqDB = db
        Me.SourceClass = "NGLCompAMSSettingsData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.Comps
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
        Return GetCompAMSSettingsFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCompAMSSettingsFiltered(ByVal CompControl As Integer) As DTO.CompAMSSettings
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oAMSColorData As New NGLCompAMSColorCodeSettingData(Me.Parameters)
                Dim oAMSFieldData As New NGLCompAMSUserFieldSettingData(Me.Parameters)
                Dim oAMSTrackingData As New NGLCompAMSApptTrackingSettingData(Me.Parameters)

                Dim oAMSColors = (From t In db.CompAMSColorCodeSettings Where t.CompAMSColorCodeSettingCompControl = CompControl Select t)
                Dim oAMSFields = (From t In db.CompAMSUserFieldSettings Where t.CompAMSUserFieldSettingCompControl = CompControl Select t)
                Dim oAMSTracking = (From t In db.CompAMSApptTrackingSettings Where t.CompAMSApptTrackingSettingCompControl = CompControl Select t)

                Dim CompAMSSettings As New DTO.CompAMSSettings With {.CompAMSSettingsCompControl = CompControl _
                                                                        , .CompAMSColorCodeSettings = (From c In oAMSColors Select oAMSColorData.selectDTOData(c, db)).ToList _
                                                                        , .CompAMSUserFieldSettings = (From f In oAMSFields Select oAMSFieldData.selectDTOData(f, db)).ToList _
                                                                        , .CompAMSApptTrackingSettings = (From t In oAMSTracking Select oAMSTrackingData.selectDTOData(t, db)).ToList}

                Return CompAMSSettings

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

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Throw New NotImplementedException("Not Implemented")
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Throw New NotImplementedException("Not Implemented")
        Return Nothing
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow records to be deleted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow records to be updated from this class
        Utilities.SaveAppError("Cannot update data.  Records cannot be saved using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub


#End Region

End Class

Public Class NGLDynamicsTMSSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.DynamicsTMSSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLDynamicsTMSSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.DynamicsTMSSettings
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
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


    Private _LastDynamicsTMSSettingControl As Integer
    Public Property LastDynamicsTMSSettingControl() As Integer
        Get
            Return _LastDynamicsTMSSettingControl
        End Get
        Set(ByVal value As Integer)
            _LastDynamicsTMSSettingControl = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetDynamicsTMSSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetDynamicsTMSSettings()
    End Function

    Public Function GetDynamicsTMSSettingFiltered(Optional ByVal Control As Integer = 0) As DTO.DynamicsTMSSetting
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim DynamicsTMSSetting As DTO.DynamicsTMSSetting = (
                    From d In db.DynamicsTMSSettings
                    Where
                        (d.DTMSControl = If(Control = 0, d.DTMSControl, Control))
                    Order By d.DTMSControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return DynamicsTMSSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDynamicsTMSSettingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetDynamicsTMSSettingFiltered(ByVal LegalEntity As String) As DTO.DynamicsTMSSetting
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(LegalEntity) Then Return New DTO.DynamicsTMSSetting
                'db.Log = New DebugTextWriter
                'If(d.DynamicsTMSSettingFixOffInvAllow.HasValue, d.DynamicsTMSSettingFixOffInvAllow, 0)
                'Return all the contacts that match the criteria sorted by name
                Dim DynamicsTMSSetting As DTO.DynamicsTMSSetting = (
                    From d In db.DynamicsTMSSettings
                    Where
                        (d.DTMSLegalEntity = LegalEntity)
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return DynamicsTMSSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDynamicsTMSSettingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Gets all the Dynamics TMS Settings the caller must filter the result set
    ''' Results are limited to 1000 records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDynamicsTMSSettings() As DTO.DynamicsTMSSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim DynamicsTMSSetting As DTO.DynamicsTMSSetting() = (
                        From d In db.DynamicsTMSSettings
                        Select selectDTOData(d, db)).Take(1000).ToArray()
                Return DynamicsTMSSetting

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDynamicsTMSSettings"))
            End Try

            Return Nothing

        End Using
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.DynamicsTMSSetting)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.DynamicsTMSSetting = TryCast(LinqTable, LTS.DynamicsTMSSetting)
        If oData Is Nothing Then Return Nothing
        Return GetDynamicsTMSSettingFiltered(Control:=oData.DTMSControl)
    End Function

    Public Function QuickSaveResults(ByVal DTMSControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                ret = (From d In db.DynamicsTMSSettings
                       Where d.DTMSControl = DTMSControl
                       Select New DTO.QuickSaveResults With {.Control = d.DTMSControl _
                                                                , .ModDate = d.DTMSModDate _
                                                                , .ModUser = d.DTMSModUser _
                                                                , .Updated = d.DTMSUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.DynamicsTMSSetting = TryCast(LinqTable, LTS.DynamicsTMSSetting)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.DTMSControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.DynamicsTMSSetting, ByRef db As NGLMASCompDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.DynamicsTMSSetting

        Dim oDTO As New DTO.DynamicsTMSSetting
        Dim skipObjs As New List(Of String) From {"DTMSUpdated",
                                                      "Page",
                                                      "Pages",
                                                      "RecordCount",
                                                      "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .DTMSUpdated = d.DTMSUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO


    End Function

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DTO.DynamicsTMSSetting, ByVal UserName As String) As LTS.DynamicsTMSSetting
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.DynamicsTMSSetting
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    ''' <summary>
    ''' Typically used to update an existing LTS object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <param name="UserName"></param>
    ''' <remarks></remarks>
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DTO.DynamicsTMSSetting, ByRef t As LTS.DynamicsTMSSetting, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"DTMSModDate", "DTMSModUser", "DTMSUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .DTMSModDate = Date.Now
            .DTMSModUser = UserName
            .DTMSUpdated = If(d.DTMSUpdated Is Nothing, New Byte() {}, d.DTMSUpdated)
        End With
    End Sub

#End Region

End Class


Public Class NGLDynamicsTMSIntegrationLogData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.DynamicsTMSIntegrationLogs
        Me.LinqDB = db
        Me.SourceClass = "NGLDynamicsTMSIntegrationLogData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.DynamicsTMSIntegrationLogs
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
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


    Private _LastDynamicsTMSIntegrationLogControl As Integer
    Public Property LastDynamicsTMSIntegrationLogControl() As Integer
        Get
            Return _LastDynamicsTMSIntegrationLogControl
        End Get
        Set(ByVal value As Integer)
            _LastDynamicsTMSIntegrationLogControl = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetDynamicsTMSIntegrationLogFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetDynamicsTMSIntegrationLogFiltered(Optional ByVal Control As Integer = 0) As DTO.DynamicsTMSIntegrationLog
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim DynamicsTMSIntegrationLog As DTO.DynamicsTMSIntegrationLog = (
                    From d In db.DynamicsTMSIntegrationLogs
                    Where
                        (d.DTMSINTLogControl = If(Control = 0, d.DTMSINTLogControl, Control))
                    Order By d.DTMSINTLogControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return DynamicsTMSIntegrationLog

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDynamicsTMSIntegrationLogFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetDynamicsTMSIntegrationLogFiltered(ByVal LegalEntity As String) As DTO.DynamicsTMSIntegrationLog
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(LegalEntity) Then Return New DTO.DynamicsTMSIntegrationLog
                'db.Log = New DebugTextWriter
                'If(d.DynamicsTMSIntegrationLogFixOffInvAllow.HasValue, d.DynamicsTMSIntegrationLogFixOffInvAllow, 0)
                'Return all the contacts that match the criteria sorted by name
                Dim DynamicsTMSIntegrationLog As DTO.DynamicsTMSIntegrationLog = (
                    From d In db.DynamicsTMSIntegrationLogs
                    Where
                        (d.DTMSINTLogLegalEntity = LegalEntity)
                    Select selectDTOData(d, db)).FirstOrDefault()
                Return DynamicsTMSIntegrationLog

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDynamicsTMSIntegrationLogFiltered"))
            End Try

            Return Nothing

        End Using
    End Function



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.DynamicsTMSIntegrationLog)
        Return selectLTSData(d, Me.Parameters.UserName)

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.DynamicsTMSIntegrationLog = TryCast(LinqTable, LTS.DynamicsTMSIntegrationLog)
        If oData Is Nothing Then Return Nothing
        Return GetDynamicsTMSIntegrationLogFiltered(Control:=oData.DTMSINTLogControl)
    End Function

    Public Function QuickSaveResults(ByVal DTMSINTLogControl As Integer) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                ret = (From d In db.DynamicsTMSIntegrationLogs
                       Where d.DTMSINTLogControl = DTMSINTLogControl
                       Select New DTO.QuickSaveResults With {.Control = d.DTMSINTLogControl _
                                                                , .ModDate = d.DTMSINTLogModDate _
                                                                , .ModUser = d.DTMSINTLogModUser _
                                                                , .Updated = d.DTMSINTLogUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("QuickSaveResults"))
            End Try
        End Using
        Return ret
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim source As LTS.DynamicsTMSIntegrationLog = TryCast(LinqTable, LTS.DynamicsTMSIntegrationLog)
        If source Is Nothing Then Return Nothing
        Return QuickSaveResults(source.DTMSINTLogControl)
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.DynamicsTMSIntegrationLog, ByRef db As NGLMASCompDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.DynamicsTMSIntegrationLog

        Dim oDTO As New DTO.DynamicsTMSIntegrationLog
        Dim skipObjs As New List(Of String) From {"DTMSINTLogUpdated",
                                                      "Page",
                                                      "Pages",
                                                      "RecordCount",
                                                      "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .DTMSINTLogUpdated = d.DTMSINTLogUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO


    End Function

    ''' <summary>
    ''' Typically used when we want to insert a new LTS object in the DB
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function selectLTSData(ByVal d As DTO.DynamicsTMSIntegrationLog, ByVal UserName As String) As LTS.DynamicsTMSIntegrationLog
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim oLTS As New LTS.DynamicsTMSIntegrationLog
        UpdateLTSWithDTO(d, oLTS, UserName)
        Return oLTS

    End Function

    ''' <summary>
    ''' Typically used to update an existing LTS object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <param name="UserName"></param>
    ''' <remarks></remarks>
    Friend Shared Sub UpdateLTSWithDTO(ByRef d As DTO.DynamicsTMSIntegrationLog, ByRef t As LTS.DynamicsTMSIntegrationLog, ByVal UserName As String)
        Dim skipObjs As New List(Of String) From {"DTMSINTLogModDate", "DTMSINTLogModUser", "DTMSINTLogUpdated"}
        t = CopyMatchingFields(t, d, skipObjs)
        With t
            .DTMSINTLogModDate = Date.Now
            .DTMSINTLogModUser = UserName
            .DTMSINTLogUpdated = If(d.DTMSINTLogUpdated Is Nothing, New Byte() {}, d.DTMSINTLogUpdated)
        End With
    End Sub

#End Region

End Class


Public Class NGLLegalEntityAdminData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblLegalEntityAdmins
        Me.LinqDB = db
        Me.SourceClass = "NGLLegalEntityAdminData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblLegalEntityAdmins
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetLegalEntityAdmin(ByVal LEControl As Integer) As LTS.vLegalEntityAdmin
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.vLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLegalEntityAdmin"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLegalEntityAdmins() As LTS.vLegalEntityAdmin()
        Dim oRet As LTS.vLegalEntityAdmin()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                oRet = (From t In db.vLegalEntityAdmins
                        Order By t.CompName
                        Select t).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLegalEntityAdmins"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function GetLEAdminsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLegalEntityAdmin()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLegalEntityAdmin
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vLegalEntityAdmin)
                iQuery = db.vLegalEntityAdmins
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLEAdminsFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts a record into tblLEAdmin
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added by LVV on 03/26/2018 for v-8.1 TMS365
    ''' Modified by RHR for v-8.1 on 05/10/2018
    '''   added call to private InsertLEAdmin for new single code base method when as shared data context
    ''' </remarks>
    Public Function InsertLEAdmin(ByVal oRecord As LTS.tblLegalEntityAdmin) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            intRet = InsertLEAdmin(oRecord, db)
        End Using
        Return intRet
    End Function

    ''' <summary>
    ''' Private function to insert a new tblLegalEntityAdmin record using a shared data context
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <param name="db"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 05/10/2018
    '''   added logic to support nested call using same data context
    ''' </remarks>
    Private Function InsertLEAdmin(ByVal oRecord As LTS.tblLegalEntityAdmin, ByRef db As NGLMASCompDataContext) As Integer
        If db Is Nothing OrElse db.Connection Is Nothing Then Return InsertLEAdmin(oRecord)
        Dim intRet As Integer = 0
        Try
            oRecord.LEAdminModDate = Date.Now
            oRecord.LEAdminModUser = Parameters.UserName

            db.tblLegalEntityAdmins.InsertOnSubmit(oRecord)

            db.SubmitChanges()

            intRet = oRecord.LEAdminControl

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("InsertLEAdmin"), db)
        End Try
        Return intRet
    End Function

    ''' <summary>
    ''' Process new Legal Entity Admin record with default values, caller must handle exceptions
    ''' </summary>
    ''' <param name="intCompControl"></param>
    ''' <param name="strLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 05/10/2018
    '''   logic moved here from BLL to make it easier to process default values
    '''   caller must handle exceptions
    ''' Modified by RHR for v-8.5.2.006 on 2023-01-25 added reference to new property LEAdminSecurityLevel with default value of 2
    ''' </remarks>
    Public Function createNewLEAdmin(ByVal intCompControl As Integer, ByVal strLegalEntity As String) As Integer

        Dim LEA As New LTS.tblLegalEntityAdmin
        With LEA
            .LEAdminCompControl = intCompControl
            .LEAdminLegalEntity = strLegalEntity
            .LEAdminCNSPrefix = "CNS"
            .LEAdminCNSNumber = 1111
            .LEAdminPRONumber = 1111
            .LEAdminCNSNumberHigh = 999999
            .LEAdminCNSNumberLow = 1111
            .LEAdminAllowCreateOrderSeq = 1
            .LEAdminAutoAssignOrderSeqSeed = 0
            .LEAdminBOLLegalText = GetParText("GlobalDefaultBOLLegalText", 0)
            .LEAdminDispatchLegalText = GetParText("GlobalDefaultDispatchLegalText", 0)
            .LEAdminCarApptAutomation = False
            .LEAdminApptModCutOffMinutes = 2880   'Default = To 48 hours stored As minutes
            .LEAdminDefaultLastLoadTime = "15:00" 'Default = 15:00	equal to 3 pm
            .LEAdminApptNotSetAlertMinutes = 2880  'Default = To 48 hours stored As minutes
            .LEAdminSecurityLevel = 2
        End With
        Return InsertLEAdmin(LEA)

    End Function

    ''' <summary>
    ''' Deprecated do not use
    ''' </summary>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="NewLegalEntity"></param>
    ''' <param name="CNSNumberLow"></param>
    ''' <param name="CNSNumberHigh"></param>
    ''' <param name="CNSPrefix"></param>
    ''' <param name="AllowCreateOrderSeq"></param>
    ''' <param name="AutoAssignOrderSeqSeed"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 05/10/2018
    '''     we now use the LTS object overload
    ''' </remarks>
    Public Function UpdateLEAdminLegalEntity(ByVal LEAdminControl As Integer, ByVal NewLegalEntity As String, ByVal CNSNumberLow As Integer, ByVal CNSNumberHigh As Integer, ByVal CNSPrefix As String, ByVal AllowCreateOrderSeq As Boolean, ByVal AutoAssignOrderSeqSeed As Integer) As LTS.spUpdateLEAdminLegalEntityResult
        throwDepreciatedException("This version of " & buildProcedureName("UpdateLEAdminLegalEntity") & " has been Deprecated.  Please use the LTS object overload")
        Return Nothing
    End Function

    ''' <summary>
    ''' Update Origin and Destination information with new company data for N or P Orders and Active Lanes
    ''' </summary>
    ''' <param name="iOldCompControl"></param>
    ''' <param name="iNewCompControl"></param>
    ''' <param name="sRetMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 06/01/2021
    ''' </remarks>
    Public Function UpdateLEChangeOrderLaneComp(ByVal iOldCompControl As Integer, ByVal iNewCompControl As Integer, ByRef sRetMessage As String) As Boolean
        Dim oRet As Boolean = True
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRes = db.spUpdateLEChangeOrdersLanesComp(iOldCompControl, iNewCompControl, Parameters.UserName).FirstOrDefault()
                If oRes.ErrNumber <> 0 Then
                    oRet = False
                End If
                sRetMessage = oRes.RetMsg
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLEChangeOrderLaneComp"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Update Origin and Destination information with new company data for N or P Orders and Active Lanes
    ''' </summary>
    ''' <param name="iLegalEntityControl"></param>
    ''' <param name="sRetMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 06/01/2021
    ''' </remarks>
    Public Function ResetSecuritySettings(ByVal iLegalEntityControl As Integer, ByRef sRetMessage As String) As Boolean
        Dim oRet As Boolean = True
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRes = db.spResetSecuritySettingsByLE(iLegalEntityControl, Parameters.UserName).FirstOrDefault()
                If oRes.ErrNumber <> 0 Then
                    oRet = False
                End If
                sRetMessage = oRes.RetMsg
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResetSecuritySettings"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Update the legal enty settings and dependent company records as needed
    ''' </summary>
    ''' <param name="otblLegalEntityAdmin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 05/10/2018 
    '''   we now query and save the LTS data directly then call the modified version of spUpdateLEAdminLegalEntity
    '''   this makes it easier to add new fields to the tblLegalEntityAdmin table
    ''' Modified by RHR for v-8.2 on 09/06/2018 after insert we do not execute an update
    ''' Modified By LVV on 4/15/20 for bug fix Syncronize CompLE changes with UserGroup settings
    '''   If the CompControl has changed we update any dependent user group records
    '''   Fixed message handling to prevent null reference exceptions
    ''' </remarks>
    Public Function UpdateLEAdminLegalEntity(ByVal otblLegalEntityAdmin As LTS.tblLegalEntityAdmin) As LTS.spUpdateLEAdminLegalEntityResult
        Dim oRet As New LTS.spUpdateLEAdminLegalEntityResult() With {.ErrNumber = 1, .RetMsg = "Invalid Data: The LEAdminControl cannot be zero. Please select a record to edit."}
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If otblLegalEntityAdmin Is Nothing OrElse otblLegalEntityAdmin.LEAdminControl = 0 Then Return oRet
                'Dim oOldLegalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = otblLegalEntityAdmin.LEAdminControl).FirstOrDefault()

                If db.tblLegalEntityAdmins.Any(Function(x) x.LEAdminControl = otblLegalEntityAdmin.LEAdminControl) Then
                    Dim strLEName = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = otblLegalEntityAdmin.LEAdminControl).Select(Function(y) y.LEAdminLegalEntity).FirstOrDefault()
                    Dim intLECompControlOld = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = otblLegalEntityAdmin.LEAdminControl).Select(Function(y) y.LEAdminCompControl).FirstOrDefault()
                    'update the new table and chekc for changes to the Legal Entity Name
                    'we use the same LEAdminUpdated value because we do not care about optimistic concurancy
                    If otblLegalEntityAdmin.LEAdminUpdated Is Nothing Then
                        otblLegalEntityAdmin.LEAdminUpdated = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = otblLegalEntityAdmin.LEAdminControl).Select(Function(y) y.LEAdminUpdated).FirstOrDefault()
                    End If
                    otblLegalEntityAdmin.LEAdminModDate = Date.Now()
                    otblLegalEntityAdmin.LEAdminModUser = Me.Parameters.UserName
                    db.tblLegalEntityAdmins.Attach(otblLegalEntityAdmin, True)
                    db.SubmitChanges()
                    'if the Legal Entity Name has changed we update any dependent company records
                    If otblLegalEntityAdmin.LEAdminLegalEntity <> strLEName Then
                        oRet = db.spUpdateLEAdminLegalEntity(otblLegalEntityAdmin.LEAdminControl, strLEName, otblLegalEntityAdmin.LEAdminLegalEntity, Me.Parameters.UserName).FirstOrDefault()
                    Else
                        oRet.ErrNumber = 0
                        oRet.RetMsg = "Success!"
                    End If
                    'if the CompControl has changed we update any dependent user records
                    If otblLegalEntityAdmin.LEAdminCompControl <> intLECompControlOld Then
                        'Modified By LVV on 4/15/20 - fixed message handling to prevent null reference exceptions
                        Dim r = db.spUpdateUSLECompControl(otblLegalEntityAdmin.LEAdminControl, otblLegalEntityAdmin.LEAdminCompControl).FirstOrDefault()
                        If r IsNot Nothing Then
                            If r.ErrNumber <> 0 Then
                                If oRet.ErrNumber <> 0 Then
                                    oRet.RetMsg += (" " + r.RetMsg)
                                Else
                                    oRet.ErrNumber = r.ErrNumber
                                    oRet.RetMsg = r.RetMsg
                                End If
                            End If
                        End If
                        'Added By LVV on 4/15/20 for bug fix Syncronize CompLE changes with UserGroup settings
                        'if the CompControl has changed we update any dependent user group records
                        Dim s = db.spUpdateUserGroupLECompControl(otblLegalEntityAdmin.LEAdminControl, intLECompControlOld, otblLegalEntityAdmin.LEAdminCompControl).FirstOrDefault()
                        If s IsNot Nothing Then
                            If s.ErrNumber <> 0 Then
                                If oRet.ErrNumber <> 0 Then
                                    oRet.RetMsg += (" " + s.RetMsg)
                                Else
                                    oRet.ErrNumber = s.ErrNumber
                                    oRet.RetMsg = s.RetMsg
                                End If
                            End If
                        End If
                    Else
                        oRet.ErrNumber = 0
                        oRet.RetMsg = "Success!"
                    End If
                Else
                    'we juse need to insert the new record 
                    InsertLEAdmin(otblLegalEntityAdmin, db)
                    'Must update that Company's CompLE field
                    Dim oComp As New NGLCompData(Parameters)
                    oComp.UpdateCompLegalEntity(otblLegalEntityAdmin.LEAdminCompControl, otblLegalEntityAdmin.LEAdminLegalEntity)
                    oRet.ErrNumber = 0
                    oRet.RetMsg = "Success!"
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLEAdminLegalEntity"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' Delete a tblLegalEntityAdmin record
    ''' </summary>
    ''' <param name="LEAControl"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added By LVV on 12/19/2017 for v-8.0 TMS 365
    ''' </remarks>
    Public Function DeleteLegalEntityAdmin(ByVal LEAControl As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim oTable = db.tblLegalEntityAdmins
            Try
                '** TODO ** SHOULD THIS ALSO DELETE ALL COMPLEGALENTITY FIELDS ASSOCIATED WITH THIS LEA??
                Dim oRecord As LTS.tblLegalEntityAdmin = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEAControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LEAdminControl = 0) Then Return False

                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLegalEntityAdmin"), db)
            End Try
        End Using
        Return False
    End Function

    ''' <summary>
    ''' Checks if there is a record in tblLegalEntityAdmin with the provided LegalEntity
    ''' </summary>
    ''' <param name="LE"></param>
    ''' <returns></returns>
    Public Function DoesLegalEntityAdminExist(ByVal LE As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim LEControl = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminLegalEntity = LE).Select(Function(x) x.LEAdminControl).FirstOrDefault()
                If LEControl <> 0 Then blnRet = True
                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesLegalEntityAdminExist"), db)
            End Try
            Return blnRet
        End Using
    End Function

    Public Function getDispatchLegalText(Optional ByVal intLEControl As Integer = 0) As String
        Dim sDispatchLegalText As String = ""
        Dim sBOLLegalText As String = ""
        getBOLAndDispatchLegalText(sBOLLegalText, sDispatchLegalText, intLEControl)
        Return sDispatchLegalText
    End Function

    Public Function getBOLLegalText(Optional ByVal intLEControl As Integer = 0) As String
        Dim sDispatchLegalText As String = ""
        Dim sBOLLegalText As String = ""
        getBOLAndDispatchLegalText(sBOLLegalText, sDispatchLegalText, intLEControl)
        Return sBOLLegalText
    End Function

    Public Sub getBOLAndDispatchLegalText(ByRef sBOLLegalText As String, ByRef sDispatchLegalText As String, Optional ByVal intLEControl As Integer = 0)

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If intLEControl = 0 Then intLEControl = Parameters.UserLEControl
                Dim oLegalEntityAdmin = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = intLEControl).FirstOrDefault()
                If Not oLegalEntityAdmin Is Nothing AndAlso oLegalEntityAdmin.LEAdminControl > 0 Then
                    sBOLLegalText = oLegalEntityAdmin.LEAdminBOLLegalText
                    sDispatchLegalText = oLegalEntityAdmin.LEAdminDispatchLegalText
                End If
                If String.IsNullOrWhiteSpace(sBOLLegalText) Then sBOLLegalText = GetParText("GlobalDefaultBOLLegalText", 0)
                If String.IsNullOrWhiteSpace(sDispatchLegalText) Then sDispatchLegalText = GetParText("GlobalDefaultDispatchLegalText", 0)
            Catch ex As Exception
                'do nothing
            End Try
        End Using

    End Sub


    ''' <summary>
    ''' Get vLookupList of all tblLegalEntityAdmin
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by LVV for v-8.1 on 1/24/18 LookupLists
    ''' </remarks>
    Public Function GetLEAdminList() As DTO.vLookupList()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim vList = (
                        From t In db.tblLegalEntityAdmins
                        Order By t.LEAdminLegalEntity
                        Select New DTO.vLookupList _
                        With {.Control = t.LEAdminControl, .Name = t.LEAdminLegalEntity, .Description = ""}).ToArray()

                Return vList
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLEAdminList"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function ChangeAMSCarrierAuto(ByVal LEAControl As Integer, ByVal blnLECarApptAutomation As Boolean) As Models.ResultObject
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim retVal As New Models.ResultObject
            Try
                'Only SuperUsers are allowed to perform this action
                If Parameters.CatControl <> 4 Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    retVal.Success = False
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("E_NotAuthProcedure", "You are not authorized to execute this procedure.")
                    Return retVal
                End If
                Dim oRecord As LTS.tblLegalEntityAdmin = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEAControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LEAdminControl = 0) Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    retVal.Success = False
                    retVal.ErrMsg = String.Format(oLocalize.GetLocalizedValueByKey("E_InvalidParameterNameValue", "Invalid Parameter: No record exists in the database for {0}: {1}."), "LEAdminControl", LEAControl)
                Else
                    oRecord.LEAdminModDate = Date.Now
                    oRecord.LEAdminModUser = Parameters.UserName
                    oRecord.LEAdminCarApptAutomation = blnLECarApptAutomation
                    db.SubmitChanges()
                    retVal.Success = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ChangeAMSCarrierAuto"), db)
            End Try
            Return retVal
        End Using
    End Function

    ''' <summary>
    ''' Return the password security level setting for the provied legal entity control
    ''' </summary>
    ''' <param name="intLEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.2.006 on 2023-01-25 method to look up security level used by new password validation logic
    ''' Modified by RHR for v-8.5.4.005 on 01/29/2024 fixed issue with null LEAdminSecurityLevel
    '''     we now convert null to 1 to prevent null reference exception
    ''' </remarks>
    Public Function GetLEAdminSecurityLevel(ByVal intLEControl As Integer) As Integer
        Dim iRet As Integer = 1
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                If intLEControl = 0 Then Return iRet
                iRet = If(db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = intLEControl).Select(Function(y) y.LEAdminSecurityLevel).FirstOrDefault(), 1)
            Catch ex As Exception
                'do nothing just return level 1
            End Try
        End Using
        Return iRet

    End Function



#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLSubscriptionRequestPendingData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblSubscriptionRequestPendings
        Me.LinqDB = db
        Me.SourceClass = "NGLSubscriptionRequestPendingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblSubscriptionRequestPendings
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetSubscriptionRequest(ByVal UserControl As Integer) As LTS.vSubscriptionRequest
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.vSubscriptionRequests.Where(Function(x) x.SUBPDUserSecurityControl = UserControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSubscriptionRequest"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetSubscriptionRequests() As LTS.vSubscriptionRequest()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.vSubscriptionRequests.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSubscriptionRequests"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetSubscriptionRequestsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vSubscriptionRequest()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vSubscriptionRequest
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vSubscriptionRequest)
                iQuery = db.vSubscriptionRequests
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSubscriptionRequestsFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' Inserts a record into tblSubscriptionRequestPendings
    ''' On fail throws an exception else success
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added by LVV on 04/11/2018 for v-8.1 VSTS Task #93 Ted Page
    ''' </remarks>
    Public Sub InsertSubscriptionRequestPending(ByVal oRecord As LTS.tblSubscriptionRequestPending)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                oRecord.SUBPDModDate = Date.Now
                oRecord.SUBPDModUser = Parameters.UserName

                db.tblSubscriptionRequestPendings.InsertOnSubmit(oRecord)

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertSubscriptionRequestPending"), db)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Delete a tblSubscriptionRequestPending record
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added by LVV on 04/12/2018 for v-8.1 VSTS Task #93 Ted Page
    ''' </remarks>
    Public Function DeleteSubscriptionRequest(ByVal Control As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim oTable = db.tblSubscriptionRequestPendings
            Try
                Dim oRecord As LTS.tblSubscriptionRequestPending = db.tblSubscriptionRequestPendings.Where(Function(x) x.SUBPDControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.SUBPDControl = 0) Then Return False

                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSubscriptionRequest"), db)
            End Try
        End Using
        Return False
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class

'Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
Public Class NGLDockBlockOutPeriodData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblDockBlockOutPeriods
        Me.LinqDB = db
        Me.SourceClass = "NGLDockBlockOutPeriodData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblDockBlockOutPeriods
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
    ''' Gets a tblDockBlockOutPeriod record by DockBlockControl
    ''' and returns a Models.RecurrenceEvent object
    ''' </summary>
    ''' <param name="DockBlockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetDockBlockOutPeriod(ByVal DockBlockControl As Integer) As Models.RecurrenceEvent
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim evnt As New Models.RecurrenceEvent()
                Dim rule As New Models.RecurrenceRule()

                Dim ltsDBP = db.tblDockBlockOutPeriods.Where(Function(x) x.DockBlockControl = DockBlockControl).FirstOrDefault()
                Dim ltsDBPR = db.tblDockBlockOutPeriodRules.Where(Function(x) x.DockBlockRuleDockBlockContol = DockBlockControl).FirstOrDefault()

                If (ltsDBP Is Nothing OrElse ltsDBPR Is Nothing) Then Return Nothing

                With rule
                    .UNTIL = ltsDBPR.DockBlockRuleUntil
                    .COUNT = ltsDBPR.DockBlockRuleCount
                    .INTERVAL = ltsDBPR.DockBlockRuleInterval
                End With
                rule.resetBYDAYs(ltsDBPR.DockBlockRuleBYDAY)

                Dim compControl = db.CompDockDoors.Where(Function(x) x.CompDockContol = ltsDBP.DockBlockCompDockContol).Select(Function(y) y.CompDockCompControl).FirstOrDefault()
                Dim colorCode = db.CompAMSColorCodeSettings.Where(Function(x) x.CompAMSColorCodeSettingCompControl = compControl AndAlso x.CompAMSColorCodeSettingType = 0 AndAlso x.CompAMSColorCodeSettingKey = ltsDBP.DockBlockRecurrenceType).Select(Function(y) y.CompAMSColorCodeSettingColorCode).FirstOrDefault()

                With evnt
                    .Id = ltsDBP.DockBlockControl 'Map DockBlockControl to ID
                    .Title = ltsDBP.DockBlockTitle
                    .Description = ltsDBP.DockBlockDesc
                    .StartDate = ltsDBP.DockBlockStartDate
                    .EndDate = ltsDBP.DockBlockEndDate
                    .StartTimezone = ltsDBP.DockBlockStartTimeZone
                    .EndTimezone = ltsDBP.DockBlockEndTimeZone
                    .IsAllDay = ltsDBP.DockBlockIsAllDay
                    .recurrenceException = ltsDBP.DockBlockRecurrenceException
                    .recurrenceId = ltsDBP.DockBlockRecurrenceType.ToString() 'Map DockBlockRecurrenceType to recurrenceID
                    .Rule = rule
                    .DockControl = ltsDBP.DockBlockCompDockContol
                    .DockBlockExpired = ltsDBP.DockBlockExpired
                    .DockBlockOn = ltsDBP.DockBlockOn
                    .RecTypeColorCode = colorCode
                End With

                Return evnt

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDockBlockOutPeriod"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets all tblDockBlockOutPeriod records by DockControl
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetDockBlockOutPeriodsByDock(ByVal DockControl As Integer) As Models.RecurrenceEvent()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Return GetDockBlockOutPeriodsByDock(db, DockControl)
        End Using
    End Function

    Public Function GetDockBlockOutPeriodsByDock(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer) As Models.RecurrenceEvent()
        Try
            Dim evntList As New List(Of Models.RecurrenceEvent)

            Dim ltsDockBlocks = db.tblDockBlockOutPeriods.Where(Function(x) x.DockBlockCompDockContol = DockControl).ToArray()

            For Each l In ltsDockBlocks
                Dim r = db.tblDockBlockOutPeriodRules.Where(Function(x) x.DockBlockRuleDockBlockContol = l.DockBlockControl).FirstOrDefault()
                If Not r Is Nothing Then
                    Dim evnt As New Models.RecurrenceEvent()
                    Dim rule As New Models.RecurrenceRule()

                    With rule
                        .UNTIL = r.DockBlockRuleUntil
                        .COUNT = r.DockBlockRuleCount
                        .INTERVAL = r.DockBlockRuleInterval
                    End With
                    rule.resetBYDAYs(r.DockBlockRuleBYDAY)

                    Dim compControl = db.CompDockDoors.Where(Function(x) x.CompDockContol = l.DockBlockCompDockContol).Select(Function(y) y.CompDockCompControl).FirstOrDefault()
                    Dim colorCode = db.CompAMSColorCodeSettings.Where(Function(x) x.CompAMSColorCodeSettingCompControl = compControl AndAlso x.CompAMSColorCodeSettingType = 0 AndAlso x.CompAMSColorCodeSettingKey = l.DockBlockRecurrenceType).Select(Function(y) y.CompAMSColorCodeSettingColorCode).FirstOrDefault()

                    With evnt
                        .Id = l.DockBlockControl 'Map DockBlockControl to ID
                        .Title = l.DockBlockTitle
                        .Description = l.DockBlockDesc
                        .StartDate = l.DockBlockStartDate
                        .EndDate = l.DockBlockEndDate
                        .StartTimezone = l.DockBlockStartTimeZone
                        .EndTimezone = l.DockBlockEndTimeZone
                        .IsAllDay = l.DockBlockIsAllDay
                        .recurrenceException = l.DockBlockRecurrenceException
                        .recurrenceId = l.DockBlockRecurrenceType.ToString() 'Map DockBlockRecurrenceType to recurrenceID
                        .Rule = rule
                        .DockControl = l.DockBlockCompDockContol
                        .DockBlockExpired = l.DockBlockExpired
                        .DockBlockOn = l.DockBlockOn
                        .RecTypeColorCode = colorCode
                    End With
                    evntList.Add(evnt)
                End If
            Next
            Return evntList.ToArray()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetDockBlockOutPeriodsByDock"), db)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the BlockOutPeriods for the DockControl where DockBlockOn = True and DockBlockExpired = False
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    Public Function GetActiveBlockOutPeriodsByDock(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer) As Models.RecurrenceEvent()
        Try
            Dim evntList As New List(Of Models.RecurrenceEvent)

            Dim ltsDockBlocks = db.tblDockBlockOutPeriods.Where(Function(x) x.DockBlockCompDockContol = DockControl AndAlso x.DockBlockOn = True AndAlso x.DockBlockExpired = False).ToArray()

            For Each l In ltsDockBlocks
                Dim r = db.tblDockBlockOutPeriodRules.Where(Function(x) x.DockBlockRuleDockBlockContol = l.DockBlockControl).FirstOrDefault()
                If Not r Is Nothing Then
                    Dim evnt As New Models.RecurrenceEvent()
                    Dim rule As New Models.RecurrenceRule()

                    With rule
                        .UNTIL = r.DockBlockRuleUntil
                        .COUNT = r.DockBlockRuleCount
                        .INTERVAL = r.DockBlockRuleInterval
                    End With
                    rule.resetBYDAYs(r.DockBlockRuleBYDAY)

                    Dim compControl = db.CompDockDoors.Where(Function(x) x.CompDockContol = l.DockBlockCompDockContol).Select(Function(y) y.CompDockCompControl).FirstOrDefault()
                    Dim colorCode = db.CompAMSColorCodeSettings.Where(Function(x) x.CompAMSColorCodeSettingCompControl = compControl AndAlso x.CompAMSColorCodeSettingType = 0 AndAlso x.CompAMSColorCodeSettingKey = l.DockBlockRecurrenceType).Select(Function(y) y.CompAMSColorCodeSettingColorCode).FirstOrDefault()

                    With evnt
                        .Id = l.DockBlockControl 'Map DockBlockControl to ID
                        .Title = l.DockBlockTitle
                        .Description = l.DockBlockDesc
                        .StartDate = l.DockBlockStartDate
                        .EndDate = l.DockBlockEndDate
                        .StartTimezone = l.DockBlockStartTimeZone
                        .EndTimezone = l.DockBlockEndTimeZone
                        .IsAllDay = l.DockBlockIsAllDay
                        .recurrenceException = l.DockBlockRecurrenceException
                        .recurrenceId = l.DockBlockRecurrenceType.ToString() 'Map DockBlockRecurrenceType to recurrenceID
                        .Rule = rule
                        .DockControl = l.DockBlockCompDockContol
                        .DockBlockExpired = l.DockBlockExpired
                        .DockBlockOn = l.DockBlockOn
                        .RecTypeColorCode = colorCode
                    End With
                    evntList.Add(evnt)
                End If
            Next
            Return evntList.ToArray()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetDockBlockOutPeriodsByDock"), db)
        End Try
        Return Nothing
    End Function


    ''' <summary>
    ''' Gets the BlockOutPeroids by Company where DockBlockOn flag = true
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    Public Function GetDockBlockOutPeriodsByComp(ByVal CompControl As Integer) As Models.RecurrenceEvent()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim evntList As New List(Of Models.RecurrenceEvent)
                'Get the list of DockControls for the CompControl, and then get the list of DockBlocks for the DockControls
                'Dim dockControls = db.CompDockDoors.Where(Function(x) x.CompDockCompControl = CompControl).Select(Function(y) y.CompDockContol).ToArray()
                Dim docks = db.CompDockDoors.Where(Function(x) x.CompDockCompControl = CompControl).ToArray()
                Dim dockControls = docks.Select(Function(y) y.CompDockContol).ToArray()

                Dim ltsDockBlocks = db.tblDockBlockOutPeriods.Where(Function(x) dockControls.Contains(x.DockBlockCompDockContol) AndAlso x.DockBlockOn = True).ToArray()

                For Each l In ltsDockBlocks
                    Dim r = db.tblDockBlockOutPeriodRules.Where(Function(x) x.DockBlockRuleDockBlockContol = l.DockBlockControl).FirstOrDefault()
                    If Not r Is Nothing Then
                        Dim evnt As New Models.RecurrenceEvent()
                        Dim rule As New Models.RecurrenceRule()

                        With rule
                            .UNTIL = r.DockBlockRuleUntil
                            .COUNT = r.DockBlockRuleCount
                            .INTERVAL = r.DockBlockRuleInterval
                        End With
                        rule.resetBYDAYs(r.DockBlockRuleBYDAY)

                        Dim colorCode = db.CompAMSColorCodeSettings.Where(Function(x) x.CompAMSColorCodeSettingCompControl = CompControl AndAlso x.CompAMSColorCodeSettingType = 0 AndAlso x.CompAMSColorCodeSettingKey = l.DockBlockRecurrenceType).Select(Function(y) y.CompAMSColorCodeSettingColorCode).FirstOrDefault()
                        Dim lDock = docks.Where(Function(x) x.CompDockContol = l.DockBlockCompDockContol).FirstOrDefault()

                        With evnt
                            .Id = l.DockBlockControl 'Map DockBlockControl to ID
                            .Title = l.DockBlockTitle
                            .Description = l.DockBlockDesc
                            .StartDate = l.DockBlockStartDate
                            .EndDate = l.DockBlockEndDate
                            .StartTimezone = l.DockBlockStartTimeZone
                            .EndTimezone = l.DockBlockEndTimeZone
                            .IsAllDay = l.DockBlockIsAllDay
                            .recurrenceException = l.DockBlockRecurrenceException
                            .recurrenceId = l.DockBlockRecurrenceType.ToString() 'Map DockBlockRecurrenceType to recurrenceID
                            .Rule = rule
                            .DockControl = l.DockBlockCompDockContol
                            .DockBlockExpired = l.DockBlockExpired
                            .DockBlockOn = l.DockBlockOn
                            .RecTypeColorCode = colorCode
                            .DockDoorID = lDock.DockDoorID
                            .DockDoorName = lDock.DockDoorName
                        End With
                        evntList.Add(evnt)
                    End If
                Next
                Return evntList.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDockBlockOutPeriodsByComp"), db)
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Gets records in tblDockBlockOutPeriod filtered
    ''' **NOTE: In the caller set filters.ParentControl to the DockControl - to have a built in filter for DockControl and still allow the possibility of other filters.
    ''' Eventually we will be able to stack that filter when Rob finishes writing the new AllFilters logic
    ''' NOTE: We could also put the Start and End dates in filterTo and filterFrom 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetDockBlockOutPeriodsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblDockBlockOutPeriod()
        ''If filters Is Nothing Then Return Nothing
        ''Dim oRet() As LTS.tblDockBlockOutPeriod
        ''Using db As New NGLMASCompDataContext(ConnectionString)
        ''    Try
        ''        'Get the data iqueryable
        ''        Dim iQuery As IQueryable(Of LTS.tblDockBlockOutPeriod)
        ''        iQuery = db.tblDockBlockOutPeriods
        ''        'Dim filterWhere = ""
        ''        Dim filterWhere = " (DockBlockCompDockContol = " & filters.ParentControl & ") "
        ''        ApplyAllFilters(iQuery, filters, filterWhere)
        ''        PrepareQuery(iQuery, filters, RecordCount)
        ''        oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
        ''        Return oRet
        ''    Catch ex As Exception
        ''        ManageLinqDataExceptions(ex, buildProcedureName("GetDockBlockOutPeriodsFiltered"), db)
        ''    End Try
        ''End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' RecurrenceType maps to recurrenceId field
    ''' DockBlockControl maps to Id field
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <param name="evnt"></param>
    ''' <remarks>
    ''' Modified By LVV on 7/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub InsertOrUpdateDockBlockOutPeriod(ByVal DockControl As Integer, ByVal DockBlockOn As Boolean, ByVal evnt As Models.RecurrenceEvent)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim ltsDockBlock As New LTS.tblDockBlockOutPeriod
                Dim ltsDockBlockRule As New LTS.tblDockBlockOutPeriodRule

                If evnt.Id > 0 Then
                    'This is an update so get the existing record from the database
                    ltsDockBlock = db.tblDockBlockOutPeriods.Where(Function(x) x.DockBlockControl = evnt.Id).FirstOrDefault()
                    ltsDockBlockRule = db.tblDockBlockOutPeriodRules.Where(Function(x) x.DockBlockRuleDockBlockContol = evnt.Id).FirstOrDefault()
                End If

                Dim intRecurrenceType As Integer = 0
                Integer.TryParse(evnt.recurrenceId, intRecurrenceType)

                With ltsDockBlock
                    .DockBlockTitle = evnt.Title
                    .DockBlockDesc = evnt.Description
                    .DockBlockRecurrenceType = If(intRecurrenceType = 0, 1, intRecurrenceType)
                    .DockBlockStartDate = evnt.StartDate
                    .DockBlockEndDate = evnt.EndDate
                    .DockBlockStartTimeZone = ""
                    .DockBlockEndTimeZone = ""
                    .DockBlockIsAllDay = False
                    .DockBlockExpired = False
                    .DockBlockOn = DockBlockOn
                    .DockBlockModDate = Date.Now
                    .DockBlockModUser = Parameters.UserName
                End With

                If evnt.Id = 0 Then
                    'Insert only
                    ltsDockBlock.DockBlockCompDockContol = DockControl
                    db.tblDockBlockOutPeriods.InsertOnSubmit(ltsDockBlock)
                End If

                db.SubmitChanges()

                With ltsDockBlockRule
                    .DockBlockRuleFREQ = Models.RecurranceRuleFREQ.WEEKLY 'in 8.2 value is read only set to WEEKLY
                    .DockBlockRuleUntil = evnt.Rule.UNTIL 'only one Of UNTIL For COUNT are alowed, UNTIL Is processed first
                    .DockBlockRuleCount = evnt.Rule.COUNT
                    .DockBlockRuleInterval = 1            'in 8.2 interval should be zero Or 1 because we only support every week
                    .DockBlockRuleBYDAY = evnt.Rule.getBYDayBitString()
                    .DockBlockRuleModDate = Date.Now
                    .DockBlockRuleModUser = Parameters.UserName
                End With

                If evnt.Id = 0 Then
                    'Insert only
                    ltsDockBlockRule.DockBlockRuleDockBlockContol = ltsDockBlock.DockBlockControl
                    db.tblDockBlockOutPeriodRules.InsertOnSubmit(ltsDockBlockRule)
                End If
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateDockBlockOutPeriod"), db)
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' Delete a tblDockBlockOutPeriod record
    ''' The child table tblDockBlockOutPeriodRule has OnDeleteCascade so 
    ''' those records will be deleted automatically by the system
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DeleteDockBlockOutPeriod(ByVal Control As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblDockBlockOutPeriod = db.tblDockBlockOutPeriods.Where(Function(x) x.DockBlockControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.DockBlockControl = 0) Then Return False
                'Maybe we should add some logic like this?? 
                'throwSQLFaultException("No record exists with that control number. Refresh your data before deleting")

                db.tblDockBlockOutPeriods.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteDockBlockOutPeriod"), db)
            End Try
        End Using
        Return False
    End Function

    Public Function DoesApptOverlapBlockOut(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer, ByVal ApptStart As Date, ByVal ApptEnd As Date) As Boolean
        Try
            Dim blnRet = False
            Dim blocks = GetActiveBlockOutPeriodsByDock(db, DockControl)
            For Each b In blocks
                Dim blockStart As Date?
                Dim blockEnd As Date?
                Dim blnAllDay As Boolean
                If b.getAppointmentByDate(ApptStart, blockStart, blockEnd, blnAllDay) Then
                    If Not blockStart Is Nothing AndAlso Not blockEnd Is Nothing Then
                        If (ApptStart < blockEnd AndAlso blockStart < ApptEnd) Then
                            'Overlap!
                            blnRet = True
                            Exit For
                        End If
                    End If
                End If
            Next
            Return blnRet
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("DoesApptOverlapBlockOut"), db)
        End Try
    End Function



#End Region

#Region "Protected Functions"

#End Region

End Class

'Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
Public Class NGLDockTimeCalcFactorData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblDockTimeCalcFactors
        Me.LinqDB = db
        Me.SourceClass = "NGLDockTimeCalcFactorData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblDockTimeCalcFactors
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
    ''' Gets the record from tblDockTimeCalcFactor with the corresponding DockTFCControl
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetTimeCalcFactor(ByVal Control As Integer) As LTS.tblDockTimeCalcFactor
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.tblDockTimeCalcFactors.Where(Function(x) x.DockTCFControl = Control).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTimeCalcFactor"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Gets records in tblDockTimeCalcFactor filtered
    ''' **NOTE: In the caller set filters.ParentControl to the DockControl - to have a built in filter for DockControl and still allow the possibility of other filters.
    ''' Eventually we will be able to stack that filter when Rob finishes writing the new AllFilters logic
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetDockTimeCalcFactorsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblDockTimeCalcFactor()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblDockTimeCalcFactor
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblDockTimeCalcFactor)
                iQuery = db.tblDockTimeCalcFactors
                'Dim filterWhere = ""
                Dim filterWhere = " (DockTCFCompDockContol = " & filters.ParentControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDockTimeCalcFactorsFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Inserts or Updates a record in tblDockTimeCalcFactor
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function InsertOrUpdateDockTimeCalcFactor(ByVal oRecord As LTS.tblDockTimeCalcFactor) As LTS.tblDockTimeCalcFactor
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                oRecord.DockTCFModDate = Date.Now
                oRecord.DockTCFModUser = Parameters.UserName

                If oRecord.DockTCFControl = 0 Then
                    'Insert

                    If oRecord.DockTCFCalcFactorTypeControl = 1 AndAlso db.tblDockTimeCalcFactors.Any(Function(x) x.DockTCFControl = oRecord.DockTCFControl And x.DockTCFCalcFactorTypeControl = 1) Then
                        'return an exception or message that says only one quantity config is allowed per resource

                    End If

                    db.tblDockTimeCalcFactors.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblDockTimeCalcFactors.Attach(oRecord, True)
                End If

                db.SubmitChanges()

                Return GetTimeCalcFactor(oRecord.DockTCFControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateDockTimeCalcFactor"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Delete a tblDockTimeCalcFactor record
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DeleteDockTimeCalcFactor(ByVal Control As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Dim oTable = db.tblDockTimeCalcFactors
            Try
                Dim oRecord As LTS.tblDockTimeCalcFactor = db.tblDockTimeCalcFactors.Where(Function(x) x.DockTCFControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.DockTCFControl = 0) Then Return False
                'Maybe we should add some logic like this?? 
                'throwSQLFaultException("No record exists with that control number. Refresh your data before deleting")

                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteDockTimeCalcFactor"), db)
            End Try
        End Using
        Return False
    End Function

    ''' <summary>
    ''' Checks to see if the Dock already has a Quantity configuration in the tblDockTimeCalcFactor table
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DockHasQuantityConfig(ByVal DockControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'CalcFactorTypeControl = 1 = Quantity
                If db.tblDockTimeCalcFactors.Any(Function(x) x.DockTCFCompDockContol = DockControl And x.DockTCFCalcFactorTypeControl = 1) Then
                    blnRet = True
                End If
                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DockHasQuantityConfig"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Checks to see if the Dock already has all possible Weight configurations in the tblDockTimeCalcFactor table
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DockHasAllWgtConfigs(ByVal DockControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'CalcFactorTypeControl = 2 = Weight
                Dim calcType As Integer = 2
                If DoesCalcUOMConfigExist(db, DockControl, calcType, "LBS") AndAlso DoesCalcUOMConfigExist(db, DockControl, calcType, "KG") Then
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DockHasAllWgtConfigs"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Checks to see if the Dock already has all possible Package configurations in the tblDockTimeCalcFactor table
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DockHasAllPkgConfigs(ByVal DockControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'CalcFactorTypeControl = 3 = Package
                Dim pkgs = db.udfGetPkgUOMAvailForDockTCF(DockControl).ToArray()
                If pkgs Is Nothing OrElse pkgs.Count < 1 Then
                    blnRet = True
                End If
                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DockHasAllPkgConfigs"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Checks to see if the Dock already has all possible Cube configurations in the tblDockTimeCalcFactor table
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DockHasAllCubeConfigs(ByVal DockControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'CalcFactorTypeControl = 4 = Cube
                Dim calcType As Integer = 4
                If DoesCalcUOMConfigExist(db, DockControl, calcType, "CUI") AndAlso
                        DoesCalcUOMConfigExist(db, DockControl, calcType, "CUFT") AndAlso
                        DoesCalcUOMConfigExist(db, DockControl, calcType, "CCM") AndAlso
                        DoesCalcUOMConfigExist(db, DockControl, calcType, "CBM") Then
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DockHasAllCubeConfigs"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Checks to see if a CalcType-UOM configuration exists for this Dock
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="CalcFactorControl"></param>
    ''' <param name="strUOM"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/26/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function DoesCalcUOMConfigExist(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer, ByVal CalcFactorControl As Integer, ByVal strUOM As String) As Boolean
        Return db.tblDockTimeCalcFactors.Any(Function(x) x.DockTCFCompDockContol = DockControl And x.DockTCFCalcFactorTypeControl = CalcFactorControl And x.DockTCFUOM.ToUpper() = strUOM.ToUpper())
    End Function

#End Region

#Region "Protected Functions"

#End Region

End Class

'Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
Public Class NGLDockSettingData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblDockSettings
        Me.LinqDB = db
        Me.SourceClass = "NGLDockSettingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblDockSettings
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Enums"

    Public Enum DockSetting
        PackageTypes = 1
        TempTypes
        Monday
        Tuesday
        Wednesday
        Thursday
        Friday
        Saturday
        Sunday
        DefaultApptMins
        BlockOutOverlap
        'ApptMinsSetup
        'ApptMinsBreakdown
        InboundOutbound 'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    End Enum

    ''Public Enum NotificationMsg
    ''    NoAptDockOverMax = 1
    ''    NoAptDockNoSlots
    ''    SchedOrdersNoTemp
    ''    SchedOrdersNoPack
    ''    DockCarrierDeleteAppt
    ''    DockCarrierModifyAppt
    ''    DockCarrierBookedAppt
    ''    MissingRequestEmail
    ''End Enum

    ''Public Enum AlertMsg
    ''    NoAppointmentAvailable = 1
    ''    AlertOrdersNoTemp
    ''    AlertOrdersNoPack
    ''    CarrierDeleteAppt
    ''    CarrierModifyAppt
    ''    CarrierBookedAppt
    ''End Enum

    ''Public Enum CarrierMsg
    ''    DeleteConfirm = 1
    ''    ModifyConfirm
    ''    BookedConfirm
    ''    WHDeleteNotify
    ''    WHModifyNotify
    ''    WHBookedNotify
    ''End Enum

    Public Enum AMSMsgType
        AMSRequest = 1
        AMSAlert
        AMSResourceNotify
        AMSCarrierMsg
    End Enum

    Public Enum AMSMsg
        DeleteConfirm = 1
        ModifyConfirm
        BookedConfirm
        CarrierDelete
        CarrierModify
        CarrierBooked
        WHDelete
        WHModify
        WHBooked
        NoAppointmentAvailable
        OrdersNoTemp
        OrdersNoPack
        MaxApptLenExceeded
        NoTimeSlotAvailable
        MissingRequestEmail
    End Enum


#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Gets the record from tblDockSettings with the corresponding DockControl
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/2/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetSettingsForDock(ByVal DockControl As Integer) As LTS.tblDockSetting()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettingsForDock"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Updates the fields DockSettingOn, DockSettingRequireReasonCode, DockSettingRequireSupervisorPwd, and DockSettingDescription
    ''' Called from Overrides Tab of ManageSchedule page
    ''' </summary>
    ''' <param name="DockSettingControl"></param>
    ''' <param name="desc"></param>
    ''' <param name="DSOn"></param>
    ''' <param name="blnReqReasonCode"></param>
    ''' <param name="blnReqSupPass"></param>
    Public Sub UpdateOverrideSettings(ByVal DockSettingControl As Integer, ByVal desc As String, ByVal DSOn As Boolean, ByVal blnReqReasonCode As Boolean, ByVal blnReqSupPass As Boolean)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord = db.tblDockSettings.Where(Function(x) x.DockSettingControl = DockSettingControl).FirstOrDefault()

                oRecord.DockSettingDescription = desc
                oRecord.DockSettingOn = DSOn
                oRecord.DockSettingRequireReasonCode = blnReqReasonCode
                oRecord.DockSettingRequireSupervisorPwd = blnReqSupPass
                oRecord.DockSettingModDate = Date.Now
                oRecord.DockSettingModUser = Parameters.UserName

                'Update
                'db.tblDockSettings.Attach(oRecord, True)

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateOverrideSettings"), db)
            End Try
        End Using
    End Sub


#Region "CREATE DEFAULT SETTINGS METHODS"

    ''' <summary>
    ''' Creates the Default Dock Settings for a new Dock
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="DockControl"></param>
    ''' <remarks>
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement - added new setting InboundOutbound
    ''' </remarks>
    Public Sub CreateDefaultDockSettings(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer)
        Try
            'Check if a record exists for this Dock and Setting Type and if not insert the default
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.PackageTypes) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.PackageTypes, "List of Package Types supported by the resource", "", "0", ""))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.TempTypes) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.TempTypes, "List of Temp Types supported by the resource", "", "0", ""))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Monday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Monday, "Monday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Tuesday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Tuesday, "Tuesday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Wednesday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Wednesday, "Wednesday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Thursday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Thursday, "Thursday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Friday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Friday, "Friday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Saturday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Saturday, "Saturday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.Sunday) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.Sunday, "Sunday Start Time, End Time, and Maximum Allowed Appointments", "7/9/2018 6:00:00 AM", "6", "7/9/2018 6:00:00 PM"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.DefaultApptMins) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.DefaultApptMins, "Default values for the minimum, average, and maximum number of minutes for an appointment", "0", "120", "0"))
            End If
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.BlockOutOverlap) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.BlockOutOverlap, "Determines if an Appointment is allowed to overlap with a Block Out Period", "0", "0", "0"))
            End If
            'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
            If Not db.tblDockSettings.Any(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.InboundOutbound) Then
                db.tblDockSettings.InsertOnSubmit(CreateDefDockSetLTS(DockControl, DockSetting.InboundOutbound, "Determines if the Inbound/Outbound status of the Appointment must match that of the Resource", "0", "0", "0"))
            End If
            db.SubmitChanges()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("CreateDefaultDockSettings"), db)
        End Try
    End Sub

    Private Function CreateDefDockSetLTS(ByVal DockControl As Integer, ByVal settingType As DockSetting, ByVal desc As String, ByVal strStart As String, ByVal strFixed As String, ByVal strEnd As String) As LTS.tblDockSetting
        Dim setting = New LTS.tblDockSetting
        With setting
            .DockSettingCompDockContol = DockControl
            .DockSettingEnumID = settingType
            .DockSettingName = settingType.ToString()
            .DockSettingDescription = desc
            .DockSettingRequireReasonCode = True
            .DockSettingRequireSupervisorPwd = False
            .DockSettingStart = strStart
            .DockSettingFixed = strFixed
            .DockSettingEnd = strEnd
            .DockSettingOn = True
            .DockSettingModDate = Date.Now
            .DockSettingModUser = Parameters.UserName
        End With
        Return setting
    End Function

#End Region

#Region "PACKAGE TYPE SETTING METHODS"

    Public Function GetSupportedPackageTypesForDock(ByVal DockControl As Integer) As Models.DockPTType()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.PackageTypes).FirstOrDefault()

                If Not setting Is Nothing Then
                    'Get the current integers for the Package Type config
                    Dim intPkgs As Long = 0
                    If String.IsNullOrWhiteSpace(setting.DockSettingFixed) Then Return Nothing
                    If Not Long.TryParse(setting.DockSettingFixed.Trim(), intPkgs) Then Return Nothing

                    Dim bwPackageTypes As New Ngl.Core.Utility.BitwiseFlags(intPkgs)
                    Dim pkgList = bwPackageTypes.refreshPositiveBitPositions()

                    Dim oLook As New NGLLookupDataProvider(Parameters)

                    Dim ltsPT = oLook.GetPalletTypesByEnumIDs(pkgList.ToArray())

                    If ltsPT Is Nothing Then Return Nothing

                    Return (From t In ltsPT
                            Order By t.PalletTypeDescription
                            Select New Models.DockPTType With {.PTBitPos = t.PalletTypeBitPos, .PTCaption = t.PalletTypeDescription, .PTOn = True}
                                ).ToArray()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSupportedPackageTypesForDock"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetEditablePackageTypesForDock(ByVal DockControl As Integer) As Models.DockPTType()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLook As New NGLLookupDataProvider(Parameters)
                Dim intPkgs As Long = 0
                Dim retPkgs = oLook.GetAllPackageTypes() 'Default is all packages none are checked

                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.PackageTypes).FirstOrDefault()

                If Not setting Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(setting.DockSettingFixed) AndAlso Long.TryParse(setting.DockSettingFixed.Trim(), intPkgs) Then
                        Dim bwPackageTypes As New Ngl.Core.Utility.BitwiseFlags(intPkgs)
                        'Get the current enumns for all selected Packages
                        Dim pkgList = bwPackageTypes.refreshPositiveBitPositions()
                        'If any items in the default return list match the IDs from pkgList set those to On
                        For Each r In retPkgs
                            If pkgList.Contains(r.PTBitPos) Then r.PTOn = True
                        Next
                    End If
                End If

                Return retPkgs

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEditablePackageTypesForDock"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Sub SavePackageTypeConfig(ByVal DockControl As Integer, ByVal pkgsToTurnOn() As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim bwPackageTypes As New Ngl.Core.Utility.BitwiseFlags() 'start with a bitwise flag where all positions are off
                'Turn on the selected package types
                For Each p In pkgsToTurnOn
                    bwPackageTypes.turnBitFlagOn(p)
                Next

                'Check if a PackageType setting exists for this Dock
                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.PackageTypes).FirstOrDefault()

                If setting Is Nothing Then
                    'Insert
                    setting = New LTS.tblDockSetting
                    With setting
                        .DockSettingCompDockContol = DockControl
                        .DockSettingEnumID = DockSetting.PackageTypes
                        .DockSettingName = "PackageTypes"
                        .DockSettingDescription = "List of Package Types supported by the resource"
                        .DockSettingRequireReasonCode = False
                        .DockSettingRequireSupervisorPwd = False
                        .DockSettingFixed = bwPackageTypes.FlagSource.ToString()
                        .DockSettingOn = True
                        .DockSettingModDate = Date.Now
                        .DockSettingModUser = Parameters.UserName
                    End With
                    db.tblDockSettings.InsertOnSubmit(setting)
                Else
                    'Update
                    setting.DockSettingFixed = bwPackageTypes.FlagSource.ToString()
                    setting.DockSettingModDate = Date.Now
                    setting.DockSettingModUser = Parameters.UserName
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavePackageTypeConfig"), db)
            End Try
        End Using
    End Sub

#End Region

#Region "TEMP TYPE SETTING METHODS"

    Public Function GetSupportedTempTypesForDock(ByVal DockControl As Integer) As Models.DockPTType()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.TempTypes).FirstOrDefault()

                If Not setting Is Nothing Then
                    'Get the current integers for the Temp Type config
                    Dim intTmps As Long = 0
                    If String.IsNullOrWhiteSpace(setting.DockSettingFixed) Then Return Nothing
                    If Not Long.TryParse(setting.DockSettingFixed.Trim(), intTmps) Then Return Nothing

                    Dim bwTempTypes As New Ngl.Core.Utility.BitwiseFlags(intTmps)
                    Dim tmpList = bwTempTypes.refreshPositiveBitPositions()

                    Dim oTemp As New NGLTempTypeData(Parameters)

                    Dim ltsPT = oTemp.GetTempTypesByEnumIDs(tmpList.ToArray())

                    If ltsPT Is Nothing Then Return Nothing

                    Return (From t In ltsPT
                            Order By t.TempType
                            Select New Models.DockPTType With {.PTBitPos = t.TempTypeBitPos, .PTCaption = t.TempType, .PTOn = True}
                                ).ToArray()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSupportedTempTypesForDock"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetEditableTempTypesForDock(ByVal DockControl As Integer) As Models.DockPTType()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oTemp As New NGLTempTypeData(Parameters)
                Dim intTmps As Long = 0
                Dim retTmps = oTemp.GetAllTempTypes() 'Default is all temps none are checked

                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.TempTypes).FirstOrDefault()

                If Not setting Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(setting.DockSettingFixed) AndAlso Long.TryParse(setting.DockSettingFixed.Trim(), intTmps) Then
                        Dim bwTempTypes As New Ngl.Core.Utility.BitwiseFlags(intTmps)
                        'Get the current enumns for all selected Temps
                        Dim tmpList = bwTempTypes.refreshPositiveBitPositions()
                        'If any items in the default return list match the IDs from tmpList set those to On
                        For Each r In retTmps
                            If tmpList.Contains(r.PTBitPos) Then r.PTOn = True
                        Next
                    End If
                End If

                Return retTmps

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEditableTempTypesForDock"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Sub SaveTempTypeConfig(ByVal DockControl As Integer, ByVal tmpsToTurnOn() As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim bwTempTypes As New Ngl.Core.Utility.BitwiseFlags() 'start with a bitwise flag where all positions are off
                'Turn on the selected temp types
                For Each t In tmpsToTurnOn
                    bwTempTypes.turnBitFlagOn(t)
                Next

                'Check if a TempType setting exists for this Dock
                Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.TempTypes).FirstOrDefault()

                If setting Is Nothing Then
                    'Insert
                    setting = New LTS.tblDockSetting
                    With setting
                        .DockSettingCompDockContol = DockControl
                        .DockSettingEnumID = DockSetting.TempTypes
                        .DockSettingName = "TempTypes"
                        .DockSettingDescription = "List of Temp Types supported by the resource"
                        .DockSettingRequireReasonCode = False
                        .DockSettingRequireSupervisorPwd = False
                        .DockSettingFixed = bwTempTypes.FlagSource.ToString()
                        .DockSettingOn = True
                        .DockSettingModDate = Date.Now
                        .DockSettingModUser = Parameters.UserName
                    End With
                    db.tblDockSettings.InsertOnSubmit(setting)
                Else
                    'Update
                    setting.DockSettingFixed = bwTempTypes.FlagSource.ToString()
                    setting.DockSettingModDate = Date.Now
                    setting.DockSettingModUser = Parameters.UserName
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveTempTypeConfig"), db)
            End Try
        End Using
    End Sub

#End Region

#Region "DOCK APPT TIMES SETTING METHODS"

    Public Function GetDockApptTimeSettings(ByVal DockControl As Integer) As Models.DockApptSettings
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim settings = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl).ToArray()
                If settings Is Nothing OrElse settings.Length < 1 Then Return Nothing

                Dim mon = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Monday).FirstOrDefault()
                Dim tue = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Tuesday).FirstOrDefault()
                Dim wed = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Wednesday).FirstOrDefault()
                Dim thu = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Thursday).FirstOrDefault()
                Dim fri = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Friday).FirstOrDefault()
                Dim sat = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Saturday).FirstOrDefault()
                Dim sun = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.Sunday).FirstOrDefault()
                Dim min = settings.Where(Function(x) x.DockSettingEnumID = DockSetting.DefaultApptMins).FirstOrDefault()

                Dim temp As Integer = 0
                Dim retVal As New Models.DockApptSettings
                With retVal
                    .DockControl = DockControl
                    .MonStart = getDateVal(mon.DockSettingStart)
                    .MonEnd = getDateVal(mon.DockSettingEnd)
                    .MonMaxAppt = getIntVal(mon.DockSettingFixed)
                    .TueStart = getDateVal(tue.DockSettingStart)
                    .TueEnd = getDateVal(tue.DockSettingEnd)
                    .TueMaxAppt = getIntVal(tue.DockSettingFixed)
                    .WedStart = getDateVal(wed.DockSettingStart)
                    .WedEnd = getDateVal(wed.DockSettingEnd)
                    .WedMaxAppt = getIntVal(wed.DockSettingFixed)
                    .ThuStart = getDateVal(thu.DockSettingStart)
                    .ThuEnd = getDateVal(thu.DockSettingEnd)
                    .ThuMaxAppt = getIntVal(thu.DockSettingFixed)
                    .FriStart = getDateVal(fri.DockSettingStart)
                    .FridEnd = getDateVal(fri.DockSettingEnd)
                    .FriMaxAppt = getIntVal(fri.DockSettingFixed)
                    .SatStart = getDateVal(sat.DockSettingStart)
                    .SatEnd = getDateVal(sat.DockSettingEnd)
                    .SatMaxAppt = getIntVal(sat.DockSettingFixed)
                    .SunStart = getDateVal(sun.DockSettingStart)
                    .SunEnd = getDateVal(sun.DockSettingEnd)
                    .SunMaxAppt = getIntVal(sun.DockSettingFixed)
                    .ApptMinsMin = getIntVal(min.DockSettingStart)
                    .ApptMinsAvg = getIntVal(min.DockSettingFixed)
                    .ApptMinsMax = getIntVal(min.DockSettingEnd)
                End With
                Return retVal
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDockApptTimeSettings"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Sub SaveDockApptTimeSettings(ByVal s As Models.DockApptSettings)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Monday, s.MonStart, s.MonEnd, s.MonMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Tuesday, s.TueStart, s.TueEnd, s.TueMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Wednesday, s.WedStart, s.WedEnd, s.WedMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Thursday, s.ThuStart, s.ThuEnd, s.ThuMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Friday, s.FriStart, s.FridEnd, s.FriMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Saturday, s.SatStart, s.SatEnd, s.SatMaxAppt)
                SaveDayOfWeekSetting(db, s.DockControl, DockSetting.Sunday, s.SunStart, s.SunEnd, s.SunMaxAppt)

                SaveDefaultApptMinsSetting(db, s.DockControl, s.ApptMinsMin, s.ApptMinsAvg, s.ApptMinsMax)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveDockApptTimeSettings"), db)
            End Try
        End Using
    End Sub

    Private Sub SaveDayOfWeekSetting(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer, ByVal DayOfWeek As DockSetting, ByVal DayStart As Date?, ByVal DayEnd As Date?, ByVal DayMax As Integer)
        Try
            If db Is Nothing Then Return

            'Check if a setting exists for this Dock
            Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DayOfWeek).FirstOrDefault()

            If setting Is Nothing Then
                'Insert
                setting = New LTS.tblDockSetting
                With setting
                    .DockSettingCompDockContol = DockControl
                    .DockSettingEnumID = DayOfWeek
                    .DockSettingName = DayOfWeek.ToString() + "Settings"
                    .DockSettingDescription = DayOfWeek.ToString() + " Start Time, End Time, and Maximum Allowed Appointments"
                    .DockSettingRequireReasonCode = False
                    .DockSettingRequireSupervisorPwd = False
                    .DockSettingStart = DayStart.ToString()
                    .DockSettingFixed = DayMax.ToString()
                    .DockSettingEnd = DayEnd.ToString()
                    .DockSettingOn = True
                    .DockSettingModDate = Date.Now
                    .DockSettingModUser = Parameters.UserName
                End With
                db.tblDockSettings.InsertOnSubmit(setting)
            Else
                'Update
                setting.DockSettingStart = DayStart.ToString()
                setting.DockSettingFixed = DayMax.ToString()
                setting.DockSettingEnd = DayEnd.ToString()
                setting.DockSettingModDate = Date.Now
                setting.DockSettingModUser = Parameters.UserName
            End If

            db.SubmitChanges()

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SaveDayOfWeekSetting"), db)
        End Try
    End Sub

    Private Sub SaveDefaultApptMinsSetting(ByRef db As NGLMASCompDataContext, ByVal DockControl As Integer, ByVal ApptMin As Integer, ByVal ApptAvg As Integer, ByVal ApptMax As Integer)
        Try
            If db Is Nothing Then Return

            'Check if a setting exists for this Dock
            Dim setting = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.DefaultApptMins).FirstOrDefault()

            If setting Is Nothing Then
                'Insert
                setting = New LTS.tblDockSetting
                With setting
                    .DockSettingCompDockContol = DockControl
                    .DockSettingEnumID = DockSetting.DefaultApptMins
                    .DockSettingName = "DefaultApptMinsSettings"
                    .DockSettingDescription = "Default values for the minimum, average, and maximum number of minutes for an appointment"
                    .DockSettingRequireReasonCode = False
                    .DockSettingRequireSupervisorPwd = False
                    .DockSettingStart = ApptMin.ToString()
                    .DockSettingFixed = ApptAvg.ToString()
                    .DockSettingEnd = ApptMax.ToString()
                    .DockSettingOn = True
                    .DockSettingModDate = Date.Now
                    .DockSettingModUser = Parameters.UserName
                End With
                db.tblDockSettings.InsertOnSubmit(setting)
            Else
                'Update
                setting.DockSettingStart = ApptMin.ToString()
                setting.DockSettingFixed = ApptAvg.ToString()
                setting.DockSettingEnd = ApptMax.ToString()
                setting.DockSettingModDate = Date.Now
                setting.DockSettingModUser = Parameters.UserName
            End If

            db.SubmitChanges()

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SaveDefaultApptMinsSetting"), db)
        End Try
    End Sub

    Private Function getIntVal(ByVal strInput As String) As Integer
        Dim temp As Integer = 0
        Integer.TryParse(strInput, temp)
        Return temp
    End Function

    Private Function getDateVal(ByVal strInput As String) As Date?
        Dim dt As Date? = Nothing
        If Not String.IsNullOrWhiteSpace(strInput) Then
            dt = Date.Parse(strInput)
            If Not dt.HasValue Then dt = Nothing
        End If
        Return dt
    End Function

#End Region

#Region "VALIDATION METHODS"

    ''' <summary>
    ''' This method is the main point of entry for validation
    ''' Called by CreateOrUpdateAppointment() and AddOrdersToAppointment() in AMSDataProvider
    ''' </summary>
    ''' <param name="blnEditOrdersOnly"></param>
    ''' <param name="blnRunAll"></param>
    ''' <param name="blnDateChanged"></param>
    ''' <param name="blnTimeChanged"></param>
    ''' <param name="BookControls"></param>
    ''' <param name="Appointment"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get dock is not valid with only the DockDoorID
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the function was modified to accept 
    '''  a DTO.AMSAppointment object as a parmeter instead of some of the data
    '''  We now throw an InvalidKeyParentRequiredException if the Appointment,  
    '''  the Dock or start and end dates are not valid
    '''  Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''   Added new rule to validate Inbound/Outbound
    '''  Modified by RHR for v-8.3.0.001 on 09/27/2020
    '''     we now use a shard copy of the dock so the caller can perform some 
    '''     dock validation before calling ValidateDockSettingsForAppt specifically 
    '''     when dock.CompDockValidation is false
    ''' </remarks>
    Public Function ValidateDockSettingsForAppt(ByVal blnEditOrdersOnly As Boolean, ByVal blnRunAll As Boolean, ByVal blnDateChanged As Boolean, ByVal blnTimeChanged As Boolean, ByVal BookControls As Integer(), ByRef Appointment As DTO.AMSAppointment, Optional ByRef dock As LTS.CompDockDoor = Nothing) As Models.AMSValidation
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim vrDockHours As String = "0" 'Default Pass
                Dim vrMaxAppts As String = "0"  'Default Pass
                Dim vrTemp As String = "0"      'Default Pass
                Dim vrPackage As String = "0"   'Default Pass
                Dim vrDoubleBook As String = "0"  'Default Pass
                Dim vrBlockOut As String = "0"  'Default Pass
                Dim vrMinTime As String = "0"  'Default Pass
                Dim vrMaxTime As String = "0"  'Default Pass
                Dim vrIO As String = "0"  'Default Pass 'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                If Appointment Is Nothing OrElse Appointment.AMSApptCompControl = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Record"})
                End If
                If Not Appointment.AMSApptStartDate.HasValue Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Start Date"})
                End If
                If Not Appointment.AMSApptEndDate.HasValue Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "End Date"})
                End If
                Dim ApptControl As Integer = Appointment.AMSApptControl
                Dim DockDoorID As String = Appointment.AMSApptDockdoorID
                Dim StartDate As Date = Appointment.AMSApptStartDate.Value()
                Dim EndDate As Date = Appointment.AMSApptEndDate.Value()
                Dim CompControl As Integer = Appointment.AMSApptCompControl
                If dock Is Nothing OrElse dock.CompDockContol = 0 Then
                    dock = NGLCompDockDoorObjData.GetCompDockDoorForAppt(Appointment)
                    If dock Is Nothing OrElse dock.CompDockContol = 0 Then
                        throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "Dock"})
                    End If
                End If

                Dim DockControl = dock.CompDockContol

                'If Resource level validation flag is off then we do not validate and everything automatically passes, Else run validation
                If dock.CompDockValidation Then
                    If blnEditOrdersOnly Then
                        '** Only the orders have changed - aka the caller is AddOrdersToAppointment() **
                        'We only need to validate TempType and PackType
                        ValidateTempTypeSetting(db, vrTemp, DockControl, BookControls) 'TEMP TYPE
                        ValidatePackTypeSetting(db, vrPackage, DockControl, BookControls) 'PACK TYPE
                        ValidateInboundOutboundSetting(db, vrIO, DockControl, dock.CompDockInbound, CompControl, BookControls) 'INBOUND/OUTBOUND 'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                    Else
                        '** The appointment has changed - aka the caller is CreateOrUpdateAppointment() **
                        'If this is an Insert, or if this is an Update and the Dock has changed - we need to run all validation
                        If blnRunAll Then
                            ValidateDayOfWeekSetting(db, vrDockHours, vrMaxAppts, DockControl, StartDate, EndDate) 'DAY OF WEEK
                            ValidateTempTypeSetting(db, vrTemp, DockControl, BookControls) 'TEMP TYPE
                            ValidatePackTypeSetting(db, vrPackage, DockControl, BookControls) 'PACK TYPE
                            ValidateDoubleBookingSetting(db, vrDoubleBook, DockControl, StartDate, EndDate, ApptControl) 'DOUBLE BOOKING
                            ValidateBlockOutOverlapSetting(db, vrBlockOut, DockControl, StartDate, EndDate) 'BLOCK OUT OVERLAP
                            ValidateDefAptMinSetting(db, vrMinTime, vrMaxTime, DockControl, StartDate, EndDate) 'DEFAULT APPOINTMENT MINUTES
                            ValidateInboundOutboundSetting(db, vrIO, DockControl, dock.CompDockInbound, CompControl, BookControls) 'INBOUND/OUTBOUND 'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
                        Else
                            'If this is an Update and neither the date nor the time changed, then validation automatically passes.
                            If blnDateChanged OrElse blnTimeChanged Then
                                '  -MaxApptsByDay - only check this if the Day has changed
                                '  -DockHours - check if either has changed
                                '  -DoubleBooking - check if either has changed
                                '  -BlockOut - check if either has changed
                                '  -DefaultApptMin - always check if time has changed (or if both date and time changed. If only date has changed do not check))

                                'Check the ones that are common to all scenarios
                                ValidateDayOfWeekSetting(db, vrDockHours, vrMaxAppts, DockControl, StartDate, EndDate)
                                ValidateDoubleBookingSetting(db, vrDoubleBook, DockControl, StartDate, EndDate, ApptControl)
                                ValidateBlockOutOverlapSetting(db, vrBlockOut, DockControl, StartDate, EndDate)
                                'Check the ones that are scenario specific
                                If blnTimeChanged Then
                                    ValidateDefAptMinSetting(db, vrMinTime, vrMaxTime, DockControl, StartDate, EndDate)
                                End If
                                If Not blnDateChanged Then
                                    'If the Date hasn't changed we don't validate MaxApptsByDay, so reset this to 'validation pass'
                                    vrMaxAppts = "0"
                                End If
                            End If
                        End If
                    End If
                End If

                'DockHours,MaxAppts,TempType,PackType,DoubleBook,BlockOut,MinTime,MaxTime,(?ApptTime/CalcTime(CarrierOnly)?)
                Dim strValidation = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", vrDockHours, vrMaxAppts, vrTemp, vrPackage, vrDoubleBook, vrBlockOut, vrMinTime, vrMaxTime, vrIO) 'Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement - Added rule Inbound

                Return parseValidationString(strValidation)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateDockSettingsForAppt"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Called at the end of ValidateDockSettingsForAppt() to interpret the results. 
    ''' Transforms the bit string into a Models.AMSValidation object to return to the client.
    ''' 
    ''' Parse the validation bit string to determine if validation failed, and if so
    ''' determine if an override is allowed and if a password, reason code, or both
    ''' is required to perform the override
    ''' </summary>
    ''' <param name="strValidation"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/27/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function parseValidationString(ByVal strValidation As String) As Models.AMSValidation
        Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
        Dim retVal As New Models.AMSValidation()

        retVal.BitString = strValidation 'always set the BitString so it can be passed back and forth
        'reset defaults
        retVal.Success = True
        retVal.NoOverride = False
        retVal.SPRequired = False
        retVal.RCRequired = False
        retVal.InvalidRC = False
        retVal.InvalidSP = False
        retVal.FailedMsg = ""
        retVal.FailedMsgDetails = Nothing

        If strValidation = "00000" Then
            'Everything passed
            retVal.Success = True
            Return retVal
        End If

        'If we get here it failed in some way so get the details
        'Eventually, if this gets too slow (especially later with localization) we can move this logic to a separate controller method that only gets called when expand More Details is clicked
        retVal.FailedMsgDetails = getFailMessageDetails(strValidation)

        If strValidation.Contains("1") Then
            'One means Fail with no option to override - if any one setting cannot be overridden then none can
            retVal.Success = False
            retVal.NoOverride = True
            retVal.FailedMsg = oLocalize.GetLocalizedValueByKey("SchedFailedMsgNoOverride", "Appointment cannot be booked for this Resource")
            Return retVal
        End If

        Dim strFailMsgOverrideAllowed = oLocalize.GetLocalizedValueByKey("SchedFailedMsgOverride", "This appointment violates one or more Resource Configuration Rules")

        If strValidation.Contains("4") Then 'At least one setting requires both options so we can stop here
            retVal.Success = False
            retVal.SPRequired = True
            retVal.RCRequired = True
            retVal.FailedMsg = strFailMsgOverrideAllowed
            Return retVal
        End If

        If strValidation.Contains("2") Then 'If at least one value contains a 2 then we require a password
            retVal.Success = False
            retVal.SPRequired = True
            retVal.FailedMsg = strFailMsgOverrideAllowed
        End If
        If strValidation.Contains("3") Then 'If at least one value contains a 3 then we require a reason code
            retVal.Success = False
            retVal.RCRequired = True
            retVal.FailedMsg = strFailMsgOverrideAllowed
        End If

        Return retVal
    End Function

    ''' <summary>
    ''' Called by parseValidationString()
    ''' Parses the validation bit string to return the validation failed messages
    ''' to be shown to the user
    ''' </summary>
    ''' <param name="strValidation"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/27/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    '''  Added rule InboundOutbound
    ''' </remarks>
    Private Function getFailMessageDetails(ByVal strValidation As String) As List(Of String)
        Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
        Dim details As New List(Of String)
        For i = 0 To strValidation.Length - 1
            If strValidation(i) <> "0" Then
                Select Case i
                    Case 0 'DockHours
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedDockHourValFail", "The appointment is outside of the Resource Hours"))
                    Case 1 'MaxAppts
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedMaxApptsValFail", "The maximum allowed appointments per day have been met"))
                    Case 2 'TempType
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedTempTypeValFail", "An order contains one or more unsupported Temperature Codes"))
                    Case 3 'PackageType
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedPackTypeValFail", "An order contains one or more unsupported Package Types"))
                    Case 4 'DoubleBooking
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedDoubleBookValFail", "Double Booking is not allowed"))
                    Case 5 'BlockOutPeriod
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedBlockOutValFail", "The Appointment overlaps a Block Out Period"))
                    Case 6 'MinTime
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedMinLengthValFail", "The Appointment is less than the minimum required minutes"))
                    Case 7 'MaxTime
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedMaxLengthValFail", "The Appointment is greater than the maximum allowed minutes"))
                    Case 8 'InboundOutbound
                        details.Add(oLocalize.GetLocalizedValueByKey("SchedIOValFail", "An order does not match the Inbound/Outbound status of the Resource"))
                    Case Else
                        'do nothing
                End Select
            End If
        Next
        Return details
    End Function

    ''' <summary>
    ''' Determines which day of the week the provided date is on
    ''' and returns the corresponding DockSetting enumerator
    ''' Called by ValidateDayOfWeekSetting() and GetPossibleDocks()
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/27/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Function getDayofWeekDockSettingEnum(ByVal dt As Date) As DockSetting
        Dim retVal As DockSetting
        Select Case dt.DayOfWeek
            Case DayOfWeek.Sunday
                retVal = DockSetting.Sunday
            Case DayOfWeek.Monday
                retVal = DockSetting.Monday
            Case DayOfWeek.Tuesday
                retVal = DockSetting.Tuesday
            Case DayOfWeek.Wednesday
                retVal = DockSetting.Wednesday
            Case DayOfWeek.Thursday
                retVal = DockSetting.Thursday
            Case DayOfWeek.Friday
                retVal = DockSetting.Friday
            Case DayOfWeek.Saturday
                retVal = DockSetting.Saturday
        End Select
        Return retVal
    End Function

#Region "Validation Rule Sub Methods"
    '********************************************************************
    '* These methods are all called by ValidateDockSettingsForAppt()    *
    '* Each one is the method to call to validate each individual rule  *
    '********************************************************************

    Private Sub ValidateDayOfWeekSetting(ByRef db As NGLMASCompDataContext, ByRef vrDockHours As String, ByRef vrMaxAppts As String, ByVal DockControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date)
        'Get the Day of the week the apointment is on so we can lookup ByDay validation rules
        Dim dockSetEnum As DockSetting
        dockSetEnum = getDayofWeekDockSettingEnum(StartDate)
        Dim spDay = db.spAMSValidateDayOfWeekSetting(DockControl, StartDate, EndDate, dockSetEnum).FirstOrDefault()
        vrDockHours = spDay.DockHours.Value.ToString() 'validation results for DockHours
        vrMaxAppts = spDay.MaxPerDay.Value.ToString()  'validation results for MaxApptsByDay
    End Sub

    Private Sub ValidateTempTypeSetting(ByRef db As NGLMASCompDataContext, ByRef vrTemp As String, ByVal DockControl As Integer, ByVal BookControls As Integer())
        Dim ts = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.TempTypes).FirstOrDefault()
        Dim intTempDB = 0
        If Not Integer.TryParse(ts.DockSettingFixed, intTempDB) Then intTempDB = 0 'TODO: WHAT HAPPENS IF WE CAN"T READ CONFIG FROM DB?? AUTO PASS OR AUTO FAIL??
        If ts.DockSettingOn Then 'Only validate if this setting is turned ON
            'If nothing is selected (0) that means all temp types are supported - validation automatically passes -- Else we have to validate
            If intTempDB <> 0 Then
                Dim oList As New List(Of LTS.spAMSGetTempTypesForOrderResult)
                Dim bwTmp As New Ngl.Core.Utility.BitwiseFlags(intTempDB)
                Dim tmpAllowed = bwTmp.refreshPositiveBitPositions() 'List of temptypes supported by the resource (whitelist - bit positions turned on)
                'Get the Temp Types associated with the BookControls
                For Each b In BookControls
                    Dim temp = db.spAMSGetTempTypesForOrder(b).ToList()
                    oList.AddRange(temp)
                Next
                Dim ordTemps = oList.Select(Function(t) t.TempTypeBitPos).Distinct() 'List of TempTypeBitPos for the provided BookControls
                'if there are any order temps which are NOT in tmpAllowed then validation fails
                If ordTemps.Any(Function(a) Not tmpAllowed.Contains(a)) Then vrTemp = db.udfGetDockSetValidationFailCode(ts.DockSettingRequireSupervisorPwd, ts.DockSettingRequireReasonCode).ToString() 'FAILED VALIDATION
            End If
        End If
    End Sub

    Private Sub ValidatePackTypeSetting(ByRef db As NGLMASCompDataContext, ByRef vrPackage As String, ByVal DockControl As Integer, ByVal BookControls As Integer())
        Dim ps = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.PackageTypes).FirstOrDefault()
        Dim intPackDB = 0
        If Not Integer.TryParse(ps.DockSettingFixed, intPackDB) Then intPackDB = 0 'TODO: WHAT HAPPENS IF WE CAN"T READ CONFIG FROM DB?? AUTO PASS OR AUTO FAIL??
        If ps.DockSettingOn Then 'Only validate if this setting is turned ON
            'If nothing is selected (0) that means all package types are supported - validation automatically passes -- Else we have to validate
            If intPackDB <> 0 Then
                Dim oList As New List(Of LTS.spAMSGetPackTypesForOrderResult)
                Dim bwPkg As New Ngl.Core.Utility.BitwiseFlags(intPackDB)
                Dim pkgAllowed = bwPkg.refreshPositiveBitPositions() 'List of packagetypes supported by the resource (whitelist - bit positions turned on)
                For Each b In BookControls
                    Dim pack = db.spAMSGetPackTypesForOrder(b).ToList() 'Get the Package Types associated with the BookControl
                    oList.AddRange(pack)
                Next
                Dim ordPacks = oList.Select(Function(p) p.PalletTypeBitPos).Distinct() 'List of PalletTypeBitPos for the provided BookControls
                'if there are any order packages which are NOT in pkgAllowed then validation fails
                If ordPacks.Any(Function(a) Not pkgAllowed.Contains(a)) Then vrPackage = db.udfGetDockSetValidationFailCode(ps.DockSettingRequireSupervisorPwd, ps.DockSettingRequireReasonCode).ToString() 'FAILED VALIDATION
            End If
        End If
    End Sub

    Private Sub ValidateDefAptMinSetting(ByRef db As NGLMASCompDataContext, ByRef vrMinTime As String, ByRef vrMaxTime As String, ByVal DockControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date)
        Dim spDAM = db.spAMSValidateDefApptMinutesSetting(DockControl, StartDate, EndDate, DockSetting.DefaultApptMins).FirstOrDefault()
        vrMinTime = spDAM.MinTime.Value.ToString() 'validation results for MinTime
        vrMaxTime = spDAM.MaxTime.Value.ToString()  'validation results for MaxTime
    End Sub

    Private Sub ValidateDoubleBookingSetting(ByRef db As NGLMASCompDataContext, ByRef vrDoubleBook As String, ByVal DockControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date, ByVal ApptControl As Integer)
        Dim spDblBook = db.spAMSValidateDoubleBooking(DockControl, StartDate, EndDate, ApptControl).FirstOrDefault()
        vrDoubleBook = spDblBook.DoubleBooking.Value.ToString() 'validation results for DoubleBooking
    End Sub

    Private Sub ValidateBlockOutOverlapSetting(ByRef db As NGLMASCompDataContext, ByRef vrBlockOut As String, ByVal DockControl As Integer, ByVal ApptStart As Date, ByVal ApptEnd As Date)
        Dim bs = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.BlockOutOverlap).FirstOrDefault()
        'If the setting is turned on we have to validate
        If bs.DockSettingOn Then
            Dim oBlock As New NGLDockBlockOutPeriodData(Parameters)
            If oBlock.DoesApptOverlapBlockOut(db, DockControl, ApptStart, ApptEnd) Then
                'this is a fail code scenario
                vrBlockOut = db.udfGetDockSetValidationFailCode(bs.DockSettingRequireSupervisorPwd, bs.DockSettingRequireReasonCode).ToString() 'FAILED VALIDATION
            End If
        End If
    End Sub


    ''' <summary>
    ''' Use new Enumerator to check if a dock is configured for Inbound or Outbound
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="vrIO"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="DockInbound"></param>
    ''' <param name="WarehouseCompControl"></param>
    ''' <param name="BookControls"></param>
    ''' <remarks>
    ''' Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
    ''' Modified by RHR for v-8.2.1.006 on 04/03/2020
    '''     fixed issues where a null reference exception can occure when no records are found
    ''' </remarks>
    Private Sub ValidateInboundOutboundSetting(ByRef db As NGLMASCompDataContext, ByRef vrIO As String, ByVal DockControl As Integer, ByVal DockInbound As Boolean, ByVal WarehouseCompControl As Integer, ByVal BookControls As Integer())
        If Not BookControls Is Nothing AndAlso BookControls.Count() > 0 AndAlso DockControl <> 0 Then
            ' Modified by RHR for v-8.2.1.006 on 04/03/2020 fixed null reference exception bug
            '   we can only validate if we have some booking records and a valid dock door control number
            Dim ios = db.tblDockSettings.Where(Function(x) x.DockSettingCompDockContol = DockControl And x.DockSettingEnumID = DockSetting.InboundOutbound).FirstOrDefault()
            ' Modified by RHR for v-8.2.1.006 on 04/03/2020 fixed null reference exception bug for ios when dockcontrol does not have an InboundOutbound setting
            If Not ios Is Nothing AndAlso ios.DockSettingOn Then 'Only validate if this setting is turned ON
                Dim fail As Boolean = False
                For Each b In BookControls
                    Dim spRes = db.spAMSIsOrderInbound(b, WarehouseCompControl).FirstOrDefault() 'Get the Inbound(Del)/Outbound(Pick) status associated with the BookControl
                    ' Modified by RHR for v-8.2.1.006 on 04/03/2020 fixed null reference exception bug for spRes and Inbound nullable boolean
                    If Not spRes Is Nothing AndAlso spRes.Inbound.HasValue AndAlso spRes.Inbound <> DockInbound Then
                        fail = True
                        Exit For
                    End If
                Next
                'TODO:  we need to provide QA testing on this validation method below.
                '       Why would we make an extra call to SQL Server to run this test?  this is a bad design. 
                '       at the least the value of vrIO should be returned with the ios 
                'if there are any orders which do NOT match the Inbound/Outbound status of the Dock then validation fails
                If fail = True Then vrIO = db.udfGetDockSetValidationFailCode(ios.DockSettingRequireSupervisorPwd, ios.DockSettingRequireReasonCode).ToString() 'FAILED VALIDATION
            End If
        End If

    End Sub

#End Region

#End Region

#Region "Appointment Automation Algorithm Methods"

    Public Function GetAMSCarrierAutomationSettings(ByRef db As NGLMASCompDataContext,
                                                        ByRef blnCarrierAutomation As Boolean,
                                                        ByRef intApptModCutOffMinutes As Integer,
                                                        ByRef strDefaultLastLoadTime As String,
                                                        ByVal CompControl As Integer,
                                                        ByVal CarrierControl As Integer) As Boolean
        'Get the settings
        Dim LEAControl = db.udfGetLEAdminControl(CompControl)
        Dim ltsLE = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEAControl).FirstOrDefault()
        Dim ltsLECompCar = db.tblLegalEntityCompCarriers.Where(Function(x) x.LECompCarLEAControl = LEAControl AndAlso x.LECompCarCompControl = CompControl AndAlso x.LECompCarCarrierControl = CarrierControl).FirstOrDefault()

        'TODO -- IF CARRIER AUTOMATION IS OFF FOR LE THEN ERROR CANT DO IT - MAYBE THROW SOME KIND OF SPECIAL CASE EXCEPTION??
        'This should never happen because we added filtering to the views so only records with this parameter set to true will be in the grid
        If ltsLE Is Nothing OrElse Not ltsLE.LEAdminCarApptAutomation Then Return False

        If ltsLECompCar Is Nothing Then
            blnCarrierAutomation = ltsLE.LEAdminCarApptAutomation 'Will always be true here because if it was false we would not get to this point (off means user didn't buy module)
            intApptModCutOffMinutes = ltsLE.LEAdminApptModCutOffMinutes
            strDefaultLastLoadTime = ltsLE.LEAdminDefaultLastLoadTime
        Else
            blnCarrierAutomation = ltsLECompCar.LECompCarApptAutomation 'If this is off it means the user has access to the module but turned off Carrier Automation for this Carrier at this Comp
            intApptModCutOffMinutes = ltsLECompCar.LECompCarApptModCutOffMinutes
            strDefaultLastLoadTime = ltsLECompCar.LECompCarDefaultLastLoadTime
        End If
        Return True
    End Function

    Public Function GetCarrierAvailableAppointmentsEXPERIMENT(ByVal SHID As String, ByVal CompControl As String, ByVal blnIsPickup As Boolean) As Models.AMSCarrierResults
        Dim oAMS As New NGLAMSAppointmentData(Parameters)
        Dim record As New Models.AMSCarrierRecord
        Dim Orders As String = ""
        Dim sep = ""

        If (blnIsPickup) Then
            Dim res = oAMS.GetAMSCarrierPickNeedApptBySHID(SHID, CompControl)
            With record
                .SHID = res(0).BookSHID
                .Warehouse = res(0).Warehouse
                .EquipID = res(0).BookCarrTrailerNo
                .CarrierControl = res(0).BookCarrierControl
                .CarrierName = res(0).CarrierName
                .CarrierNumber = res(0).CarrierNumber.Value
                .BookDateLoad = res(0).BookDateLoad
                .BookDateRequired = res(0).BookDateRequired
                .ScheduledDate = res(0).ScheduledDate
                .ScheduledTime = res(0).ScheduledTime
                .Inbound = res(0).Inbound.Value
                .IsTransfer = res(0).IsTransfer.Value
                .IsPickup = res(0).IsPickup.Value
                .BookControl = res(0).BookControl
                .CompControl = res(0).BookCustCompControl
            End With
            For Each r In res
                Orders += (sep + r.BookCarrOrderNumber)
                sep = ", "
            Next
        Else
            Dim res = oAMS.GetAMSCarrierDelNeedApptBySHID(SHID, CompControl)
            With record
                .SHID = res(0).BookSHID
                .Warehouse = res(0).Warehouse
                .EquipID = res(0).BookCarrTrailerNo
                .CarrierControl = res(0).BookCarrierControl
                .CarrierName = res(0).CarrierName
                .CarrierNumber = res(0).CarrierNumber.Value
                .BookDateLoad = res(0).BookDateLoad
                .BookDateRequired = res(0).BookDateRequired
                .ScheduledDate = res(0).ScheduledDate
                .ScheduledTime = res(0).ScheduledTime
                .Inbound = res(0).Inbound.Value
                .IsTransfer = res(0).IsTransfer.Value
                .IsPickup = res(0).IsPickup.Value
                .BookControl = res(0).BookControl
                .CompControl = res(0).BookCustCompControl
            End With
            For Each r In res
                Orders += (sep + r.BookCarrOrderNumber)
                sep = ", "
            Next
        End If

        Return GetCarrierAvailableAppointmentsEXPERIMENT(record, Orders)
    End Function

    ''' <summary>
    ''' LVV NOTE: This is me messing around to try to optimize and make it more clear. Not entirely necessary at this point but I want to fix it eventually because I hate it
    ''' Even I can't always follow it. We need to get more values from the database instead of passing everything in from the client. 
    ''' </summary>
    ''' <param name="record"></param>
    ''' <returns></returns>
    Public Function GetCarrierAvailableAppointmentsEXPERIMENT(ByVal record As Models.AMSCarrierRecord, ByVal Orders As String) As Models.AMSCarrierResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oBook As New NGLBookData(Parameters)
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                'Create a return object based on timeDictionary
                Dim retVals As New List(Of Models.AMSCarrierAvailableSlots)
                Dim result As New Models.AMSCarrierResults With {.AvailableSlots = retVals, .blnMustRequestAppt = False, .RequestSendToEmail = "", .Body = "", .Subject = ""}
                Dim timeDictionary As New Dictionary(Of Date, String()) 'The collection of available times across all possible docks
                Dim dt As Date
                Dim SendRequestEmails = ""
                Dim blnCarrierAutomation As Boolean
                Dim intApptModCutOffMinutes As Integer
                Dim strDefaultLastLoadTime As String = ""

                'Get the settings
                If Not GetAMSCarrierAutomationSettings(db, blnCarrierAutomation, intApptModCutOffMinutes, strDefaultLastLoadTime, record.CompControl, record.CarrierControl) Then Return Nothing

                If Not blnCarrierAutomation Then
                    'Carrier Automation is turned off for this company so they have to request an appointment
                    Dim email = db.CompDockDoors.Where(Function(x) x.CompDockCompControl = record.CompControl).Select(Function(y) y.CompDockNotificationEmail).ToArray()
                    Dim esep = ""
                    For Each e In email
                        SendRequestEmails += (esep + e)
                        SendRequestEmails += e
                        esep = ";"
                    Next
                    result = GetCreateRequestObjectEXPERIMENT(oLocalize, record, Orders, SendRequestEmails)
                    result.Message = ""
                    Return result
                End If

                If record.IsPickup Then dt = record.BookDateLoad.Value Else dt = record.BookDateRequired.Value 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery

                'Calculate the cut off time
                Dim dtTemp As Date = dt
                If dtTemp.TimeOfDay = Nothing OrElse dtTemp.TimeOfDay = TimeSpan.Zero Then
                    'The default cut-off time for last appointment of the day will be used when a time value has Not been entered for the Load/Ship date. 
                    'Note: A value of 12:00 am in the Load Date will be considered as Not entered.
                    Dim ts = TimeSpan.Parse(If(String.IsNullOrWhiteSpace(strDefaultLastLoadTime), "15:00", strDefaultLastLoadTime))
                    dtTemp = dtTemp.Date.Add(ts)
                End If
                'If this appointment Is inside the cut off hours the create option will be disabled And the carrier can only request an appointment.
                Dim dtCutOffTime = dtTemp.AddMinutes(-intApptModCutOffMinutes)
                If Date.Now < dtCutOffTime Then
                    'Allow Create
                    'Get all the Book records that are included in this Carrier Appointment
                    Dim books = oBook.GetAMSDependentBooks(record.BookControl, record.IsPickup, True)

                    timeDictionary = GetAvailableApptsAlgorithm(db, Orders, SendRequestEmails, books, record.CompControl, dt, record.Warehouse, record.EquipID, record.SHID, record.CarrierControl, record.CarrierName, record.CarrierNumber, record.ScheduledDate, record.ScheduledTime)

                    If timeDictionary?.Count > 0 Then
                        'Create a return object based on timeDictionary
                        Dim strBooks = ""
                        Dim sep = ""
                        For Each b In books
                            strBooks += (sep + b.BookControl.ToString())
                            sep = ","
                        Next
                        For Each t In timeDictionary
                            'Note: strArray(0) = Docks, strArray(1) = strEndTimes
                            Dim di = t.Value
                            Dim strDocks = di(0)
                            Dim strEndTimes = di(1)
                            retVals.Add(New Models.AMSCarrierAvailableSlots() With {.Date = t.Key, .StartTime = t.Key, .EndTime = strEndTimes, .Docks = strDocks, .Warehouse = record.Warehouse, .books = strBooks, .CarrierNumber = record.CarrierNumber, .CarrierName = record.CarrierName, .CompControl = record.CompControl, .CarrierControl = record.CarrierControl})
                        Next
                        result.AvailableSlots = retVals.OrderBy(Function(x) x.StartTime).ToList()
                    Else
                        '* No times were available at any docks so the Carrier must request an appointment *
                        Dim smp As New Models.SchedMessagingParams With {.CompControl = record.CompControl, .Warehouse = record.Warehouse, .CarrierControl = record.CarrierControl, .CarrierNumber = record.CarrierNumber, .CarrierName = record.CarrierName, .SHID = record.SHID, .EquipID = record.EquipID, .Orders = Orders, .BookDateLoad = record.BookDateLoad.Value, .BookDateRequired = record.BookDateRequired.Value}
                        'Send Subscription Alert
                        SendSchedulerAlertAsync(AMSMsg.NoAppointmentAvailable, smp)
                        'must submit a create request
                        result = GetCreateRequestObjectEXPERIMENT(oLocalize, record, Orders, SendRequestEmails)
                        result.Message = oLocalize.GetLocalizedValueByKey("SchedNoApptAvailable", "No appointments were available at any resources")
                        Return result
                    End If
                Else
                    'must submit a create request                               
                    result = GetCreateRequestObjectEXPERIMENT(oLocalize, record, Orders, SendRequestEmails)
                    result.Message = ""
                    Return result
                End If

                Return result

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierAvailableAppointments"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Validates that there are still appointments available at the selected start time 
    ''' and if so returns the DockID of the available dock with the lowest sequence number
    ''' and the calculated appointment end time at that dock
    ''' </summary>
    ''' <param name="strDockID"></param>
    ''' <param name="dtApptReqEndTime"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="Warehouse"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="ApptReqStartTime"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency</remarks>
    Public Function ValidateCarrierAppointmentAvailability(ByRef strDockID As String, ByRef dtApptReqEndTime As Date, ByVal CarrierControl As Integer, ByVal CompControl As Integer, ByVal BookControl As Integer, ByVal Warehouse As String, ByVal CarrierName As String, ByVal CarrierNumber As Integer, ByVal ApptReqStartTime As Date) As Boolean
        Dim blnRet As Boolean = False 'default to false
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Check the Carrier Automation settings
                Dim blnCarrierAutomation As Boolean
                Dim intApptModCutOffMinutes As Integer
                Dim strDefaultLastLoadTime As String = ""
                If Not GetAMSCarrierAutomationSettings(db, blnCarrierAutomation, intApptModCutOffMinutes, strDefaultLastLoadTime, CompControl, CarrierControl) Then Return blnRet
                If Not blnCarrierAutomation Then Return blnRet 'If Carrier Automation is turned off we return false and the message will tell the user to refresh the data (Then if they try again after refresh the code will handle all the alerts and requests and such. Sending them now would be redundant)

                'Get all the Book records that are included in this Carrier Appointment (By SHID and also EquipID)
                Dim oBook As New NGLBookData(Parameters)
                Dim books = oBook.GetAMSDependentBooks(BookControl, True)
                Dim b = books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                'Get from the booking record
                Dim IsPickup As Boolean = oBook.IsBookAMSPickup(b.BookOrigCompControl, b.BookAMSPickupApptControl, b.BookCarrActualDate)
                Dim EquipmentID As String = b.BookCarrTrailerNo
                Dim BookDateLoad As Date = b.BookDateLoad
                Dim BookDateRequired As Date = b.BookDateRequired
                Dim SHID As String = b.BookSHID

                Dim dt As Date
                If IsPickup Then dt = BookDateLoad Else dt = BookDateRequired 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery

                'Calculate the cut off time
                Dim dtTemp As Date = dt
                If dtTemp.TimeOfDay = Nothing OrElse dtTemp.TimeOfDay = TimeSpan.Zero Then
                    'The default cut-off time for last appointment of the day will be used when a time value has Not been entered for the Load/Ship date. 
                    'Note: A value of 12:00 am in the Load Date will be considered as Not entered.
                    Dim ts = TimeSpan.Parse(If(String.IsNullOrWhiteSpace(strDefaultLastLoadTime), "15:00", strDefaultLastLoadTime))
                    dtTemp = dtTemp.Date.Add(ts)
                End If
                'If this appointment is inside the cut off hours, we will return false and the caller will send a message that tells the user to refresh the data. 
                'When they retry after refresh that code will handle the disabling of the create option and showing the request an appointment window. Don't need to do it now.
                Dim dtCutOffTime = dtTemp.AddMinutes(-intApptModCutOffMinutes)
                If Date.Now < dtCutOffTime Then

                    'Cache all records from the Pallet Type And Temp Type tables
                    Dim AllPalletTypes = db.CompRefPalletTypes.ToArray()
                    Dim AllTempTypes = db.CompRefTempTypes.ToArray()

                    'Get the lists of distinct package and temp types used on the loads so we can use them for validation
                    Dim orderPkgs() As LTS.CompRefPalletType
                    Dim orderTmps() As Integer
                    Dim Orders As String = ""
                    GetPackTempForApptBooks(orderPkgs, orderTmps, Orders, books, AllPalletTypes, AllTempTypes, CompControl, CarrierControl, CarrierNumber, True)

                    Dim bookControls = books.Select(Function(x) x.BookControl).ToArray()
                    'The docks this appointment can go to based on configuration rules
                    Dim pdks = GetPossibleDocks(db, CompControl, dt, orderPkgs, orderTmps, bookControls, Orders, SHID, True, False) 'match the load to dock doors (set SendAlerts to false so we don't send duplicate alerts)

                    If pdks?.Count > 0 Then
                        '* At least one dock matched the criteria so continue processing *
                        Dim possibleDocks = pdks.OrderBy(Function(x) x.DockBookingSeq).ToArray() 'ORDER THE DOCKS BY SEQUENCE NUMBER
                        'Get all the Time Calc Factor Rules for all the possible docks and store them in an array
                        Dim possibleDockControls = possibleDocks.Select(Function(x) x.DockControl).ToList()
                        Dim AllCTFRs = (From d In db.tblDockTimeCalcFactors Where possibleDockControls.Contains(d.DockTCFCompDockContol) AndAlso d.DockTCFOn = True Select d).ToArray()
                        'Loop through the docks by sequence and see if this time slot is available
                        For Each dock In possibleDocks
                            'Get the CTFRs for this dock from the main list  
                            Dim dockCTFRs = AllCTFRs.Where(Function(x) x.DockTCFCompDockContol = dock.DockControl).ToArray()
                            'Get the PalletTypeID's for all package types that have CTFRs for this dock (PalletTypeID is what is stored in BookItem and PalletType is what is stored in tblDockTimeCalcFactors so we need translate using AllPalletTypes)
                            Dim ctfrPalletIDs = (From d In dockCTFRs Join x In AllPalletTypes On d.DockTCFUOM Equals x.PalletType Where d.DockTCFCalcFactorTypeControl = 3 Select x).ToArray()
                            'We use the min length because we know the appointment length can't possibly be less than this (If min is 0 use avg)
                            Dim minLen = If(dock.DAMMin = 0, dock.DAMAvg, dock.DAMMin)
                            'Calculate how many minutes are needed for the appointment at this dock
                            Dim NeededApptMins As Integer = 0
                            NeededApptMins = GetCalculatedMinutes(dockCTFRs, books, ctfrPalletIDs, orderPkgs, dock.DAMAvg)
                            'If NeededApptMins is less than DockMinimumMinutes Then we use the DockMinimumMinutes
                            If NeededApptMins < minLen Then NeededApptMins = minLen
                            'If this number is greater than DockMaxMinutes Then we cannot schedule at this dock (If DockMaxMinutes Is 0 then there Is no max any length Is fine)
                            If dock.DAMMax <> 0 Then
                                If NeededApptMins > dock.DAMMax Then Continue For
                            End If

                            'If ScheduledTime.HasValue AndAlso ScheduledTime.Value.TimeOfDay <> TimeSpan.Zero Then 'Note: Remember ScheduledTime will always be Nothing from Warehouse
                            'Check to see if the selected time slot is available at this dock
                            'Dim schedStart = schedStart.Add(ApptReqStartTime.TimeOfDay)
                            'schedStart = schedStart.Add(ScheduledTime.Value.TimeOfDay)
                            Dim schedEnd As Date = ApptReqStartTime.AddMinutes(NeededApptMins)
                            Dim oBlock As New NGLDockBlockOutPeriodData(Parameters)
                            Dim oAMS As New NGLAMSAppointmentData(Parameters)
                            Dim blnOverlapBlockOut = oBlock.DoesApptOverlapBlockOut(db, dock.DockControl, ApptReqStartTime, schedEnd)
                            Dim blnIsAvailable = oAMS.IsTimeSlotOpen(CompControl, dock.DockDoorID, ApptReqStartTime, schedEnd) 'Modified by RHR for v-8.2.1 on 10/16/2019 - fixed bug where DockDoorID is not unique with warehouse
                            If Not blnOverlapBlockOut AndAlso blnIsAvailable Then
                                'The time is still available at this dock so we can book the appointment
                                strDockID = dock.DockDoorID
                                dtApptReqEndTime = schedEnd
                                blnRet = True
                                Exit For
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierAppointmentAvailability"), db)
            End Try
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Validates that there are still appointments available at the selected start time 
    ''' and if so returns the DockID of the available dock with the lowest sequence number
    ''' and the calculated appointment end time at that dock.
    ''' Wrapper method that executes Warehouse specific logic before calling 
    ''' ValidateAppointmentAvailability() to do the real work
    ''' </summary>
    ''' <param name="strDockID"></param>
    ''' <param name="dtApptReqEndTime"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="Warehouse"></param>
    ''' <param name="ApptReqStartTime"></param>
    ''' <param name="AptControl"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency</remarks>
    Public Function ValidateWarehouseSuggestedAppointmentAvailability(ByRef strDockID As String, ByRef dtApptReqEndTime As Date, ByVal CarrierControl As Integer, ByVal CarrierName As String, ByVal CarrierNumber As Integer, ByVal CompControl As Integer, ByVal Warehouse As String, ByVal ApptReqStartTime As Date, ByVal BookControl As Integer, ByVal AptControl As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oBook As New NGLBookData(Parameters)
                Dim oAMS As New NGLAMSAppointmentData(Parameters)
                Dim books() As LTS.Book
                Dim dt As Date

                If AptControl <> 0 Then
                    'Get the orders on the appointment
                    Dim AMSOrders As DTO.AMSOrderList() = oAMS.GetAMSOrdersByAppointment(AptControl)
                    'Populate the variables
                    BookControl = AMSOrders(0).BookControl
                    'Get all the Book records that are included in this Appointment
                    Dim bookControls = AMSOrders.Select(Function(y) y.BookControl).ToList()
                    books = oBook.GetBookByControls(bookControls)
                    'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery
                    If AMSOrders(0).BookOrigCompControl = AMSOrders(0).BookCustCompControl Then dt = AMSOrders(0).BookDateLoad
                    If AMSOrders(0).BookDestCompControl = AMSOrders(0).BookCustCompControl Then dt = AMSOrders(0).BookDateRequired
                Else
                    'Get all the Book records that are included in this Appointment (By SHID and also EquipID)
                    If BookControl = 0 Then Return False
                    books = oBook.GetAMSDependentBooks(BookControl, True)
                    Dim b = books.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
                    'Get from the booking record
                    Dim IsPickup As Boolean = oBook.IsBookAMSPickup(b.BookOrigCompControl, b.BookAMSPickupApptControl, b.BookCarrActualDate)
                    If IsPickup Then dt = b.BookDateLoad Else dt = b.BookDateRequired 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery
                End If
                'Validate an appointment with the selected start time is still available
                Return ValidateAppointmentAvailability(strDockID, dtApptReqEndTime, dt, books, CarrierControl, CompControl, BookControl, Warehouse, CarrierName, CarrierNumber, ApptReqStartTime)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateWarehouseSuggestedAppointmentAvailability"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Validates that there are still appointments available at the selected start time 
    ''' and if so returns the DockID of the available dock with the lowest sequence number
    ''' and the calculated appointment end time at that dock
    ''' </summary>
    ''' <param name="strDockID"></param>
    ''' <param name="dtApptReqEndTime"></param>
    ''' <param name="dt"></param>
    ''' <param name="books"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="Warehouse"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="ApptReqStartTime"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency</remarks>
    Public Function ValidateAppointmentAvailability(ByRef strDockID As String, ByRef dtApptReqEndTime As Date, ByVal dt As Date, ByVal books() As LTS.Book, ByVal CarrierControl As Integer, ByVal CompControl As Integer, ByVal BookControl As Integer, ByVal Warehouse As String, ByVal CarrierName As String, ByVal CarrierNumber As Integer, ByVal ApptReqStartTime As Date) As Boolean
        Dim blnRet As Boolean = False 'default to false
        Using db As New NGLMASCompDataContext(ConnectionString)
            'Get from the booking record
            Dim oBook As New NGLBookData(Parameters)
            Dim SHID As String = books.Where(Function(x) x.BookControl = BookControl).Select(Function(y) y.BookSHID).FirstOrDefault()

            'Cache all records from the Pallet Type And Temp Type tables
            Dim AllPalletTypes = db.CompRefPalletTypes.ToArray()
            Dim AllTempTypes = db.CompRefTempTypes.ToArray()

            'Get the lists of distinct package and temp types used on the loads so we can use them for validation
            Dim orderPkgs() As LTS.CompRefPalletType
            Dim orderTmps() As Integer
            Dim Orders As String = ""
            GetPackTempForApptBooks(orderPkgs, orderTmps, Orders, books, AllPalletTypes, AllTempTypes, CompControl, CarrierControl, CarrierNumber, True)

            Dim bookControls = books.Select(Function(x) x.BookControl).ToArray()
            'The docks this appointment can go to based on configuration rules
            Dim pdks = GetPossibleDocks(db, CompControl, dt, orderPkgs, orderTmps, bookControls, Orders, SHID, True, False) 'match the load to dock doors (set SendAlerts to false so we don't send duplicate alerts)

            If pdks?.Count > 0 Then
                '* At least one dock matched the criteria so continue processing *
                Dim possibleDocks = pdks.OrderBy(Function(x) x.DockBookingSeq).ToArray() 'ORDER THE DOCKS BY SEQUENCE NUMBER
                'Get all the Time Calc Factor Rules for all the possible docks and store them in an array
                Dim possibleDockControls = possibleDocks.Select(Function(x) x.DockControl).ToList()
                Dim AllCTFRs = (From d In db.tblDockTimeCalcFactors Where possibleDockControls.Contains(d.DockTCFCompDockContol) AndAlso d.DockTCFOn = True Select d).ToArray()
                'Loop through the docks by sequence and see if this time slot is available
                For Each dock In possibleDocks
                    'Get the CTFRs for this dock from the main list  
                    Dim dockCTFRs = AllCTFRs.Where(Function(x) x.DockTCFCompDockContol = dock.DockControl).ToArray()
                    'Get the PalletTypeID's for all package types that have CTFRs for this dock (PalletTypeID is what is stored in BookItem and PalletType is what is stored in tblDockTimeCalcFactors so we need translate using AllPalletTypes)
                    Dim ctfrPalletIDs = (From d In dockCTFRs Join x In AllPalletTypes On d.DockTCFUOM Equals x.PalletType Where d.DockTCFCalcFactorTypeControl = 3 Select x).ToArray()
                    'We use the min length because we know the appointment length can't possibly be less than this (If min is 0 use avg)
                    Dim minLen = If(dock.DAMMin = 0, dock.DAMAvg, dock.DAMMin)
                    'Calculate how many minutes are needed for the appointment at this dock
                    Dim NeededApptMins As Integer = 0
                    NeededApptMins = GetCalculatedMinutes(dockCTFRs, books, ctfrPalletIDs, orderPkgs, dock.DAMAvg)
                    'If NeededApptMins is less than DockMinimumMinutes Then we use the DockMinimumMinutes
                    If NeededApptMins < minLen Then NeededApptMins = minLen
                    'If this number is greater than DockMaxMinutes Then we cannot schedule at this dock (If DockMaxMinutes Is 0 then there Is no max any length Is fine)
                    If dock.DAMMax <> 0 Then
                        If NeededApptMins > dock.DAMMax Then Continue For
                    End If

                    'Check to see if the selected time slot is available at this dock
                    Dim schedEnd As Date = ApptReqStartTime.AddMinutes(NeededApptMins)
                    Dim oBlock As New NGLDockBlockOutPeriodData(Parameters)
                    Dim oAMS As New NGLAMSAppointmentData(Parameters)
                    Dim blnOverlapBlockOut = oBlock.DoesApptOverlapBlockOut(db, dock.DockControl, ApptReqStartTime, schedEnd)
                    Dim blnIsAvailable = oAMS.IsTimeSlotOpen(CompControl, dock.DockDoorID, ApptReqStartTime, schedEnd) 'Modified by RHR for v-8.2.1 on 10/16/2019 - fixed bug where DockDoorID is not unique with warehouse
                    If Not blnOverlapBlockOut AndAlso blnIsAvailable Then
                        'The time is still available at this dock so we can book the appointment
                        strDockID = dock.DockDoorID
                        dtApptReqEndTime = schedEnd
                        blnRet = True
                        Exit For
                    End If
                Next
            End If
            Return blnRet
        End Using
    End Function


    'Does special rules when algorithm is run from the Carrier Self Service Portal
    Public Function GetCarrierAvailableAppointments(ByVal CarrierControl As Integer,
                                                        ByVal CompControl As Integer,
                                                        ByVal BookControl As Integer,
                                                        ByVal EquipmentID As String,
                                                        ByVal BookDateLoad As Date,
                                                        ByVal BookDateRequired As Date,
                                                        ByVal IsPickup As Boolean,
                                                        ByVal Inbound As Boolean,
                                                        ByVal Warehouse As String,
                                                        ByVal CarrierName As String,
                                                        ByVal CarrierNumber As Integer,
                                                        ByVal ScheduledDate As Date?,
                                                        ByVal ScheduledTime As Date?,
                                                        ByVal SHID As String) As Models.AMSCarrierResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oBook As New NGLBookData(Parameters)
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                'Create a return object based on timeDictionary
                Dim retVals As New List(Of Models.AMSCarrierAvailableSlots)
                Dim blnMustRequest As Boolean = False
                Dim result As New Models.AMSCarrierResults With {.AvailableSlots = retVals, .blnMustRequestAppt = blnMustRequest, .RequestSendToEmail = "", .Body = "", .Subject = ""}
                Dim timeDictionary As New Dictionary(Of Date, String()) 'The collection of available times across all possible docks
                Dim dt As Date
                Dim Orders As String = ""
                Dim SendRequestEmails = ""
                Dim blnCarrierAutomation As Boolean
                Dim intApptModCutOffMinutes As Integer
                Dim strDefaultLastLoadTime As String

                'Get the settings
                Dim LEAControl = db.udfGetLEAdminControl(CompControl)
                Dim ltsLE = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEAControl).FirstOrDefault()
                Dim ltsLECompCar = db.tblLegalEntityCompCarriers.Where(Function(x) x.LECompCarLEAControl = LEAControl AndAlso x.LECompCarCompControl = CompControl AndAlso x.LECompCarCarrierControl = CarrierControl).FirstOrDefault()

                'TODO -- IF CARRIER AUTOMATION IS OFF FOR LE THEN ERROR CANT DO IT - MAYBE THROW SOME KIND OF SPECIAL CASE EXCEPTION??
                'This should never happen because we added filtering to the views so only records with this parameter set to true will be in the grid
                If ltsLE Is Nothing OrElse Not ltsLE.LEAdminCarApptAutomation Then Return Nothing

                If ltsLECompCar Is Nothing Then
                    blnCarrierAutomation = ltsLE.LEAdminCarApptAutomation 'Will always be true here because if it was false we would not get to this point (off means user didn't buy module)
                    intApptModCutOffMinutes = ltsLE.LEAdminApptModCutOffMinutes
                    strDefaultLastLoadTime = ltsLE.LEAdminDefaultLastLoadTime
                Else
                    blnCarrierAutomation = ltsLECompCar.LECompCarApptAutomation 'If this is off it means the user has access to the module but turned off Carrier Automation for this Carrier at this Comp
                    intApptModCutOffMinutes = ltsLECompCar.LECompCarApptModCutOffMinutes
                    strDefaultLastLoadTime = ltsLECompCar.LECompCarDefaultLastLoadTime
                End If

                'Get request params in case is needed later
                Dim refShid = "" 'This is not needed here because SHID is passed in as a parameter, but I put it here because I don't know what would happen if I passed in "" as ref
                Orders = GetOrdersForAppt(refShid, CarrierControl, CompControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound)

                If Not blnCarrierAutomation Then
                    'Carrier Automation is turned off for this company so they have to request an appointment
                    Dim email = db.CompDockDoors.Where(Function(x) x.CompDockCompControl = CompControl).Select(Function(y) y.CompDockNotificationEmail).ToArray()
                    Dim esep = ""
                    For Each e In email
                        SendRequestEmails += (esep + e)
                        SendRequestEmails += e
                        esep = ";"
                    Next
                    result = GetCreateRequestObject(oLocalize, CompControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Orders, Warehouse, SHID, CarrierName, SendRequestEmails)
                    result.Message = ""
                    Return result
                End If

                If IsPickup Then dt = BookDateLoad Else dt = BookDateRequired 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery

                'Calculate the cut off time
                Dim dtTemp As Date = dt
                If dtTemp.TimeOfDay = Nothing OrElse dtTemp.TimeOfDay = TimeSpan.Zero Then
                    'The default cut-off time for last appointment of the day will be used when a time value has Not been entered for the Load/Ship date. 
                    'Note: A value of 12:00 am in the Load Date will be considered as Not entered.
                    Dim ts = TimeSpan.Parse(If(String.IsNullOrWhiteSpace(strDefaultLastLoadTime), "15:00", strDefaultLastLoadTime))
                    dtTemp = dtTemp.Date.Add(ts)
                End If
                'If this appointment Is inside the cut off hours the create option will be disabled And the carrier can only request an appointment.
                Dim dtCutOffTime = dtTemp.AddMinutes(-intApptModCutOffMinutes)
                If Date.Now < dtCutOffTime Then
                    'Allow Create
                    'Get all the Book records that are included in this Carrier Appointment (By SHID and also EquipID)
                    Dim books = oBook.GetAMSDependentBooks(BookControl, IsPickup, True)

                    If String.IsNullOrWhiteSpace(SHID) AndAlso books?.Length > 0 Then SHID = books.Where(Function(x) x.BookControl = BookControl).Select(Function(y) y.BookSHID).FirstOrDefault()

                    timeDictionary = GetAvailableApptsAlgorithm(db, Orders, SendRequestEmails, books, CompControl, dt, Warehouse, EquipmentID, SHID, CarrierControl, CarrierName, CarrierNumber, ScheduledDate, ScheduledTime)

                    If timeDictionary?.Count > 0 Then
                        'Create a return object based on timeDictionary
                        Dim strBooks = ""
                        Dim sep = ""
                        For Each b In books
                            strBooks += (sep + b.BookControl.ToString())
                            sep = ","
                        Next
                        For Each t In timeDictionary
                            'Note: strArray(0) = Docks, strArray(1) = strEndTimes
                            Dim di = t.Value
                            Dim strDocks = di(0)
                            Dim strEndTimes = di(1)
                            retVals.Add(New Models.AMSCarrierAvailableSlots() With {.Date = t.Key, .StartTime = t.Key, .EndTime = strEndTimes, .Docks = strDocks, .Warehouse = Warehouse, .books = strBooks, .CarrierNumber = CarrierNumber, .CarrierName = CarrierName, .CompControl = CompControl, .CarrierControl = CarrierControl})
                        Next
                        result.AvailableSlots = retVals.OrderBy(Function(x) x.StartTime).ToList()
                    Else
                        '* No times were available at any docks so the Carrier must request an appointment *
                        blnMustRequest = True
                        Dim smp As New Models.SchedMessagingParams With {.CompControl = CompControl, .Warehouse = Warehouse, .CarrierControl = CarrierControl, .CarrierNumber = CarrierNumber, .CarrierName = CarrierName, .SHID = SHID, .EquipID = EquipmentID, .Orders = Orders, .BookDateLoad = BookDateLoad, .BookDateRequired = BookDateRequired}
                        'Send Subscription Alert
                        SendSchedulerAlertAsync(AMSMsg.NoAppointmentAvailable, smp)
                    End If
                Else
                    'must submit a create request                               
                    result = GetCreateRequestObject(oLocalize, CompControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Orders, Warehouse, SHID, CarrierName, SendRequestEmails)
                    result.Message = ""
                    Return result
                End If

                If blnMustRequest Then
                    result = GetCreateRequestObject(oLocalize, CompControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Orders, Warehouse, SHID, CarrierName, SendRequestEmails)
                    result.Message = oLocalize.GetLocalizedValueByKey("SchedNoApptAvailable", "No appointments were available at any resources")
                End If

                Return result

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierAvailableAppointments"), db)
            End Try
            Return Nothing
        End Using
    End Function

    'Does special rules when algorithm is run from the Warehouse Scheduler
    Public Function GetWarehouseAvailableAppointments(ByVal AMSOrders As DTO.AMSOrderList()) As Models.AMSCarrierAvailableSlots()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oBook As New NGLBookData(Parameters)

                Dim retVals As New List(Of Models.AMSCarrierAvailableSlots)
                ''Dim timeDictionary As New Dictionary(Of Models.TimeSlot, String) 'The collection of available times across all possible docks
                Dim timeDictionary As New Dictionary(Of Date, String()) 'The collection of available times across all possible docks
                Dim dt As Date
                Dim Orders As String = ""
                Dim SendRequestEmails As String = ""

                'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery
                If AMSOrders(0).BookOrigCompControl = AMSOrders(0).BookCustCompControl Then dt = AMSOrders(0).BookDateLoad
                If AMSOrders(0).BookDestCompControl = AMSOrders(0).BookCustCompControl Then dt = AMSOrders(0).BookDateRequired

                'Get all the Book records that are included in this Appointment
                Dim bookControls = AMSOrders.Select(Function(y) y.BookControl).ToList()
                Dim books = oBook.GetBookByControls(bookControls)

                timeDictionary = GetAvailableApptsAlgorithm(db, Orders, SendRequestEmails, books, AMSOrders(0).BookCustCompControl, dt, "", AMSOrders(0).EquipmentID, AMSOrders(0).BookSHID, AMSOrders(0).BookCarrierControl, AMSOrders(0).CarrierName, AMSOrders(0).CarrierNumber, Nothing, Nothing, False)

                If timeDictionary?.Count > 0 Then
                    'Create a return object based on timeDictionary
                    Dim strBooks = ""
                    Dim sep = ""
                    For Each b In books
                        strBooks += (sep + b.BookControl.ToString())
                        sep = ","
                    Next
                    For Each t In timeDictionary
                        ''retVals.Add(New Models.AMSCarrierAvailableSlots() With {.Date = t.Key.Start, .StartTime = t.Key.Start, .EndTime = t.Key.End, .Docks = t.Value, .Warehouse = "", .Books = strBooks, .CarrierNumber = AMSOrders(0).CarrierNumber, .CarrierName = AMSOrders(0).CarrierName, .CompControl = AMSOrders(0).BookCustCompControl, .CarrierControl = AMSOrders(0).BookCarrierControl})
                        'Note: strArray(0) = Docks, strArray(1) = strEndTimes
                        Dim di = t.Value
                        Dim strDocks = di(0)
                        Dim strEndTimes = di(1)
                        retVals.Add(New Models.AMSCarrierAvailableSlots() With {.Date = t.Key, .StartTime = t.Key, .EndTime = strEndTimes, .Docks = strDocks, .Warehouse = "", .books = strBooks, .CarrierNumber = AMSOrders(0).CarrierNumber, .CarrierName = AMSOrders(0).CarrierName, .CompControl = AMSOrders(0).BookCustCompControl, .CarrierControl = AMSOrders(0).BookCarrierControl})
                    Next
                End If

                Return retVals.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetWarehouseAvailableAppointments"), db)
            End Try
            Return Nothing
        End Using
    End Function

    'The method that does the real work
    ''' <summary>
    ''' returns a collection of available times across all possible docks for the selected warehouse
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="Orders"></param>
    ''' <param name="SendRequestEmails"></param>
    ''' <param name="Books"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="dt"></param>
    ''' <param name="Warehouse"></param>
    ''' <param name="EquipmentID"></param>
    ''' <param name="SHID"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="ScheduledDate"></param>
    ''' <param name="ScheduledTime"></param>
    ''' <param name="blnCarrierAlg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019
    '''     fixed bug where DockDoorID is not unique with warehouse
    ''' </remarks>
    Private Function GetAvailableApptsAlgorithm(ByRef db As NGLMASCompDataContext,
                                                   ByRef Orders As String,
                                                   ByRef SendRequestEmails As String,
                                                   ByVal Books() As LTS.Book,
                                                   ByVal CompControl As Integer,
                                                   ByVal dt As Date,
                                                   ByVal Warehouse As String,
                                                   ByVal EquipmentID As String,
                                                   ByVal SHID As String,
                                                   ByVal CarrierControl As Integer,
                                                   ByVal CarrierName As String,
                                                   ByVal CarrierNumber As Integer,
                                                   Optional ByVal ScheduledDate As Date? = Nothing,
                                                   Optional ByVal ScheduledTime As Date? = Nothing,
                                                   Optional ByVal blnCarrierAlg As Boolean = True) As Dictionary(Of Date, String())
        'Create the return object
        Dim timeDictionary As New Dictionary(Of Date, String()) 'The collection of available times across all possible docks 
        Dim strArray As String() 'Note: strArray(0) = Docks, strArray(1) = strEndTimes

        'Carrier Only (ScheduledDate and ScheduledTime will always be nothing when this is called from the Warehouse side)
        'When a scheduled date And time already exist we must adhere to the those values when booking an appointment
        'If time Is null Or 12:00am still adhere to the date but the time can be whatever Is available
        'If that Then time slot Is Not available at any resource they must Call For an appointment
        If Not ScheduledDate Is Nothing Then
            dt = ScheduledDate
        End If

        'Cache all records from the Pallet Type And Temp Type tables
        Dim AllPalletTypes = db.CompRefPalletTypes.ToArray()
        Dim AllTempTypes = db.CompRefTempTypes.ToArray()

        'Get the lists of distinct package and temp types used on the loads so we can use them for validation
        Dim orderPkgs() As LTS.CompRefPalletType
        Dim orderTmps() As Integer
        GetPackTempForApptBooks(orderPkgs, orderTmps, Orders, Books, AllPalletTypes, AllTempTypes, CompControl, CarrierControl, CarrierNumber, blnCarrierAlg)

        Dim bookControls = Books.Select(Function(x) x.BookControl).ToArray()
        'The docks this appointment can go to based on configuration rules
        Dim pdks = GetPossibleDocks(db, CompControl, dt, orderPkgs, orderTmps, bookControls, Orders, SHID, blnCarrierAlg) 'match the load to dock doors

        If pdks?.Count > 0 Then
            '* At least one dock matched the criteria so continue processing *
            Dim possibleDocks = pdks.OrderBy(Function(x) x.DockBookingSeq).ToArray()
            'Get all the Time Calc Factor Rules for all the possible docks and store them in an array
            Dim possibleDockControls = possibleDocks.Select(Function(x) x.DockControl).ToList()
            Dim AllCTFRs = (From d In db.tblDockTimeCalcFactors Where possibleDockControls.Contains(d.DockTCFCompDockContol) AndAlso d.DockTCFOn = True Select d).ToArray()

            Dim esep = ""
            For Each dock In possibleDocks
                If dock.NotificationAlert AndAlso Not String.IsNullOrWhiteSpace(dock.NotificationEmail) Then
                    SendRequestEmails += (esep + dock.NotificationEmail)
                    esep = ";"
                End If

                'Calculate how many minutes are needed for the appointment at this dock
                Dim NeededApptMins As Integer = 0
                Dim dockCTFRs = AllCTFRs.Where(Function(x) x.DockTCFCompDockContol = dock.DockControl).ToArray() 'Get the CTFRs for this dock from the main list  
                Dim ctfrPalletIDs = (From d In dockCTFRs Join x In AllPalletTypes On d.DockTCFUOM Equals x.PalletType Where d.DockTCFCalcFactorTypeControl = 3 Select x).ToArray() 'Get the PalletTypeID's for all package types that have CTFRs for this dock (PalletTypeID is what is stored in BookItem and PalletType is what is stored in tblDockTimeCalcFactors so we need translate using AllPalletTypes                   
                Dim minLen = If(dock.DAMMin = 0, dock.DAMAvg, dock.DAMMin) 'We use the min length because we know the appointment length can't possibly be less than this (If min is 0 use avg)

                NeededApptMins = GetCalculatedMinutes(dockCTFRs, Books, ctfrPalletIDs, orderPkgs, dock.DAMAvg)
                If NeededApptMins < minLen Then NeededApptMins = minLen 'If this number Is less than DockMinimumMinutes Then we use the DockMinimumMinutes  
                'If this number Is greater than DockMaxMinutes Then we cannot schedule at this dock (If DockMaxMinutes Is 0 then there Is no max any length Is fine)
                If dock.DAMMax <> 0 Then
                    If NeededApptMins > dock.DAMMax Then
                        'If this is called from the Carrier algorithm then send the notification if necessary
                        If blnCarrierAlg AndAlso dock.NotificationAlert AndAlso Not String.IsNullOrWhiteSpace(dock.NotificationEmail) Then
                            Dim smp As New Models.SchedMessagingParams With {.CarrierName = CarrierName, .dt = dt, .Warehouse = Warehouse, .ogDockName = dock.DockDoorName, .SHID = SHID, .EquipID = EquipmentID, .Orders = Orders}
                            SendResourceNotificationAsync(dock.NotificationEmail, AMSMsg.MaxApptLenExceeded, smp)
                        End If
                        Exit For
                    End If
                End If

                If ScheduledTime.HasValue AndAlso ScheduledTime.Value.TimeOfDay <> TimeSpan.Zero Then 'Note: Remember ScheduledTime will always be Nothing from Warehouse
                    'We have to use the scheduled time
                    Dim schedStart As Date = ScheduledDate
                    schedStart = schedStart.Add(ScheduledTime.Value.TimeOfDay)
                    Dim schedEnd As Date = schedStart.AddMinutes(NeededApptMins)
                    Dim oBlock As New NGLDockBlockOutPeriodData(Parameters)
                    Dim oAMS As New NGLAMSAppointmentData(Parameters)
                    Dim blnOverlapBlockOut = oBlock.DoesApptOverlapBlockOut(db, dock.DockControl, schedStart, schedEnd)
                    ' Modified by RHR for v-8.2.1 on 10/16/2019
                    '     fixed bug where DockDoorID is not unique with warehouse
                    Dim blnIsAvailable = oAMS.IsTimeSlotOpen(CompControl, dock.DockDoorID, schedStart, schedEnd)
                    If Not blnOverlapBlockOut AndAlso blnIsAvailable Then
                        Dim a As New Models.TimeSlot With {.Start = schedStart, .End = schedEnd}
                        ''timeDictionary.Add(a, dock.DockDoorID)
                        'Note: strArray(0) = Docks, strArray(1) = strEndTimes
                        strArray = New String() {dock.DockDoorID, schedEnd.ToString()}
                        timeDictionary.Add(schedStart, strArray)
                    End If
                Else
                    'Any time is fine
                    Dim dockScheduledSlots = GetScheduledSlots(db, dt, dock.DockControl, dock.DockDoorID, dock.DockHourStart, dock.DockHourEnd) 'Populate dockScheduledSlots with 1. Appointments already scheduled for that day 2. Block Out Periods on the day -- (and order by start time)

                    Dim dockOpenSlots = GetAvailableOpenings(NeededApptMins, minLen, dockScheduledSlots) 'The collection of available times for a single docks

                    'If this is called from the Carrier algorithm then send the notification if necessary
                    If dockOpenSlots Is Nothing OrElse dockOpenSlots.Count < 1 Then
                        If blnCarrierAlg AndAlso dock.NotificationAlert AndAlso Not String.IsNullOrWhiteSpace(dock.NotificationEmail) Then
                            Dim smp As New Models.SchedMessagingParams With {.CarrierName = CarrierName, .dt = dt, .Warehouse = Warehouse, .ogDockName = dock.DockDoorName, .SHID = SHID, .EquipID = EquipmentID, .Orders = Orders}
                            SendResourceNotificationAsync(dock.NotificationEmail, AMSMsg.NoTimeSlotAvailable, smp)
                        End If
                    End If

                    For Each a In dockOpenSlots
                        'So now we have all possible available slots for this dock add them to timeDictionary
                        'A distinct list of appointment times will be displayed. The system will silently assign the dock door based on sequence numbers for resource.
                        'Since the docks are iterated through in order of BookingSeq then this string list will contain the dock controls in this same order for duplicate avaiability across docks.
                        'Note: strArray(0) = Docks, strArray(1) = strEndTimes
                        If timeDictionary.ContainsKey(a.Start) Then
                            Dim di = timeDictionary.Item(a.Start)
                            di(0) += ("," + dock.DockDoorID)
                            di(1) += ("," + a.End.ToString())
                            timeDictionary.Item(a.Start) = di
                        Else
                            strArray = New String() {dock.DockDoorID, a.End.ToString()}
                            timeDictionary.Add(a.Start, strArray)
                        End If
                    Next
                End If
            Next
        End If

        Return timeDictionary

    End Function


    ''' <summary>
    ''' Scheduler - Carrier Self Service Portal (Carrier Automation)
    ''' Checks to see if the user has the correct permissions to either 
    ''' Delete or Edit a previously scheduled appointment. The method either
    ''' allows the user to perform the action, or gives them the option to
    ''' submit a Change/Delete request
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="EquipmentID"></param>
    ''' <param name="BookDateLoad"></param>
    ''' <param name="BookDateRequired"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="Inbound"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/9/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function ModifyCarrierBookedAppointment(ByVal CarrierControl As Integer,
                                                       ByVal CompControl As Integer,
                                                       ByVal BookControl As Integer,
                                                       ByVal SHID As String,
                                                       ByVal EquipmentID As String,
                                                       ByVal BookDateLoad As Date,
                                                       ByVal BookDateRequired As Date,
                                                       ByVal IsPickup As Boolean,
                                                       ByVal Inbound As Boolean,
                                                       ByVal Warehouse As String,
                                                       ByVal CarrierName As String,
                                                       ByVal IsDelete As Boolean,
                                                       ByVal BookAMSPickupApptControl As Integer,
                                                       ByVal BookAMSDeliveryApptControl As Integer,
                                                       ByVal CarrierNumber As Integer) As Models.AMSCarrierResults
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim result As New Models.AMSCarrierResults

                'Get the settings
                Dim LEAControl = db.udfGetLEAdminControl(CompControl)
                Dim ltsLE = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEAControl).FirstOrDefault()
                Dim ltsLECompCar = db.tblLegalEntityCompCarriers.Where(Function(x) x.LECompCarLEAControl = LEAControl AndAlso x.LECompCarCompControl = CompControl AndAlso x.LECompCarCarrierControl = CarrierControl).FirstOrDefault()

                'TODO -- IF CARRIER AUTOMATION IS OFF FOR LE THEN ERROR CANT DO IT - MAYBE THROW SOME KIND OF SPECIAL CASE EXCEPTION??
                'This should never happen because we added filtering to the views so only records with this parameter set to true will be in the grid
                If ltsLE Is Nothing OrElse Not ltsLE.LEAdminCarApptAutomation Then Return Nothing

                Dim blnCarrierAutomation As Boolean
                Dim intApptModCutOffMinutes As Integer
                Dim strDefaultLastLoadTime As String
                Dim blnAllowApptEdit As Boolean
                Dim blnAllowApptDelete As Boolean


                If ltsLECompCar Is Nothing Then
                    blnCarrierAutomation = ltsLE.LEAdminCarApptAutomation 'Will always be true here because if it was false we would not get to this point (off means user didn't buy module)
                    intApptModCutOffMinutes = ltsLE.LEAdminApptModCutOffMinutes
                    strDefaultLastLoadTime = ltsLE.LEAdminDefaultLastLoadTime
                    blnAllowApptEdit = ltsLE.LEAdminAllowApptEdit
                    blnAllowApptDelete = ltsLE.LEAdminAllowApptDelete
                Else
                    blnCarrierAutomation = ltsLECompCar.LECompCarApptAutomation 'If this is off it means the user has access to the module but turned off Carrier Automation for this Carrier at this Comp
                    intApptModCutOffMinutes = ltsLECompCar.LECompCarApptModCutOffMinutes
                    strDefaultLastLoadTime = ltsLECompCar.LECompCarDefaultLastLoadTime
                    blnAllowApptEdit = ltsLECompCar.LECompCarAllowApptEdit
                    blnAllowApptDelete = ltsLECompCar.LECompCarAllowApptDelete
                End If


                Dim dt As Date
                If IsPickup Then dt = BookDateLoad Else dt = BookDateRequired 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery

                'Check Carrier Appointment Automation (Self Service) on/off 
                If Not blnCarrierAutomation Then
                    'CAN ONLY SUBMIT CHANGE/DELETE REQUESTS IF CARRIER AUTOMATION IS TURNED OFF
                    If IsDelete Then
                        'This is a Delete
                        result = GetDeleteRequestObject(CompControl, CarrierControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, SHID, CarrierName, BookAMSPickupApptControl, BookAMSDeliveryApptControl)
                    Else
                        'This is an Edit
                        result = GetEditRequestObject(CompControl, CarrierControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, SHID, CarrierName, BookAMSPickupApptControl, BookAMSDeliveryApptControl)
                    End If
                Else
                    'CARRIER AUTOMATION IS ON 
                    'Calculate the cut off time
                    Dim dtTemp As Date = dt
                    If dtTemp.TimeOfDay = Nothing OrElse dtTemp.TimeOfDay = TimeSpan.Zero Then
                        'The default cut-off time for last appointment of the day will be used when a time value has Not been entered for the Load/Ship date. 
                        'Note: A value of 12:00 am in the Load Date will be considered as Not entered.
                        Dim ts = TimeSpan.Parse(If(String.IsNullOrWhiteSpace(strDefaultLastLoadTime), "15:00", strDefaultLastLoadTime))
                        dtTemp = dtTemp.Date.Add(ts)
                    End If
                    'If the setting is turned off (Or if this appointment Is inside the cut off hours) the delete/edit option will be disabled And the carrier can only request an appointment change.
                    Dim dtCutOffTime = dtTemp.AddMinutes(-intApptModCutOffMinutes)
                    If IsDelete Then
                        'This is a Delete
                        If blnAllowApptDelete AndAlso Date.Now < dtCutOffTime Then
                            'Allow Delete
                            Dim oAMS As New NGLAMSAppointmentData(Parameters)
                            Dim apptControl As Integer = 0
                            If IsPickup Then apptControl = BookAMSPickupApptControl Else apptControl = BookAMSDeliveryApptControl
                            oAMS.DeleteAMSAppointment(apptControl, True)
                            result = New Models.AMSCarrierResults With {.AvailableSlots = Nothing, .blnMustRequestAppt = False, .RequestSendToEmail = "", .Body = "", .Subject = ""}
                        Else
                            'must submit a delete request            
                            result = GetDeleteRequestObject(CompControl, CarrierControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, SHID, CarrierName, BookAMSPickupApptControl, BookAMSDeliveryApptControl)
                        End If
                    Else
                        'This is an Edit
                        If blnAllowApptEdit AndAlso Date.Now < dtCutOffTime Then
                            'Allow Edit
                            result = GetCarrierAvailableAppointments(CarrierControl, CompControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, CarrierName, CarrierNumber, Nothing, Nothing, SHID)
                            'Make sure we return a Change Request instead of a Create Request
                            If result.blnMustRequestAppt Then result = GetEditRequestObject(CompControl, CarrierControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, SHID, CarrierName, BookAMSPickupApptControl, BookAMSDeliveryApptControl)
                        Else
                            'must submit a change request                          
                            result = GetEditRequestObject(CompControl, CarrierControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound, Warehouse, SHID, CarrierName, BookAMSPickupApptControl, BookAMSDeliveryApptControl)
                            If blnAllowApptEdit Then result.Message = "Changes made after the modification cut-off time are by request only"
                        End If
                    End If
                End If

                Return result

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ModifyCarrierBookedAppointment"), db)
            End Try
            Return Nothing
        End Using
    End Function


#Region "Algorithm Sub Methods"

    ''' <summary>
    ''' Determines which docks this appointment can be scheduled at based on the dock configuration rules
    ''' and the characteristics of the orders on the appointment
    ''' Rules:
    ''' If validation is turned off for the resource it cannot be used for Carrier Automation
    ''' Checks to see if the orders violates allowed Temp/Package types if the validation rules are turned on
    ''' Checks to make sure the maximum appointments allowed per day is not be violated if the rule is turned on
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="dt"></param>
    ''' <param name="orderPkgs"></param>
    ''' <param name="orderTmps"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 3/10/20 - Inbound/Outbound Validation
    '''  Rob said we need to ensure that when Inbound/Outbound validation is turned off that this dock does not show up to carriers. 
    '''  Note: This will also effect on the Warehouse screen when the user click "View Availability" because the same code base is used.
    ''' Modified By LVV on 8/7/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Added optional param blnSendAlerts (default true) because if we are validating appt availability we don't want to keep sending 
    '''  duplicate alerts as it was already sent with the initial GetAppts request
    ''' </remarks>
    Private Function GetPossibleDocks(ByRef db As NGLMASCompDataContext,
                                          ByVal CompControl As Integer,
                                          ByVal dt As Date,
                                          ByVal orderPkgs() As LTS.CompRefPalletType,
                                          ByVal orderTmps() As Integer,
                                          ByVal BookControls() As Integer,
                                          ByVal Orders As String,
                                          ByVal SHID As String,
                                      ByVal blnCarrierAlg As Boolean,
                                      Optional ByVal blnSendAlerts As Boolean = True) As List(Of LTS.spAMSGetPossibleDockDataResult)
        Try
            Dim possibleDocks As New List(Of LTS.spAMSGetPossibleDockDataResult) 'The docks this appointment can go to based on configuration rules

            Dim enumDayOfWeek = getDayofWeekDockSettingEnum(dt)
            Dim data = db.spAMSGetPossibleDockData(CompControl, dt, enumDayOfWeek, DockSetting.TempTypes, DockSetting.PackageTypes, DockSetting.DefaultApptMins, DockSetting.InboundOutbound).ToArray()

            For Each d In data
                Dim blnCanUseDock As Boolean = True

                'If the rule is turned ON for the resource, make sure max appointments has not been met
                If d.ValidateMaxApptOn Then
                    If d.ApptCount >= d.MaxAppt Then blnCanUseDock = False
                End If

                'If the rule is turned ON for the resource, make sure the temp types are all allowed for this dock
                If d.ValidateTempOn Then
                    Dim intTempDB = If(d.TempFlagSource.HasValue, d.TempFlagSource.Value, 0)
                    'If nothing is selected (0) that means all temp types are supported - validation automatically passes -- Else we have to validate
                    If intTempDB <> 0 Then
                        Dim bwTmp As New Ngl.Core.Utility.BitwiseFlags(intTempDB)
                        Dim tmpAllowed = bwTmp.refreshPositiveBitPositions() 'List of temptypes supported by the resource (whitelist - bit positions turned on)
                        If orderTmps?.Length > 0 Then
                            'if there are any order temps which are NOT in tmpAllowed then validation fails
                            If orderTmps.Any(Function(a) Not tmpAllowed.Contains(a)) Then blnCanUseDock = False
                        Else
                            'If validation is on and there are no temp types provided for the load then the Carrier has to request and appointment
                            blnCanUseDock = False
                            'If this is called from the Carrier algorithm then send the notification if necessary
                            If blnCarrierAlg AndAlso d.NotificationAlert AndAlso Not String.IsNullOrWhiteSpace(d.NotificationEmail) Then
                                If blnSendAlerts Then
                                    Dim smp As New Models.SchedMessagingParams With {.SHID = SHID, .Orders = Orders}
                                    SendResourceNotificationAsync(d.NotificationEmail, AMSMsg.OrdersNoTemp, smp)
                                End If
                            End If
                        End If
                    End If
                End If

                'If the rule is turned ON for the resource, make sure the package types are all allowed for this dock
                If d.ValidatePackOn Then
                    Dim intPackDB = If(d.PackFlagSource.HasValue, d.PackFlagSource.Value, 0)
                    'If nothing is selected (0) that means all package types are supported - validation automatically passes -- Else we have to validate
                    If intPackDB <> 0 Then
                        Dim bwPkg As New Ngl.Core.Utility.BitwiseFlags(intPackDB)
                        Dim pkgAllowed = bwPkg.refreshPositiveBitPositions() 'List of package types supported by the resource (whitelist - bit positions turned on)
                        If orderPkgs?.Length > 0 Then
                            'if there are any order packages which are NOT in pkgAllowed then validation fails
                            If orderPkgs.Any(Function(a) Not pkgAllowed.Contains(a.PalletTypeBitPos)) Then blnCanUseDock = False
                        Else
                            'If validation is on and there are no package types provided for the load then the Carrier has to request and appointment
                            blnCanUseDock = False
                            'If this is called from the Carrier algorithm then send the notification if necessary
                            If blnCarrierAlg AndAlso d.NotificationAlert AndAlso Not String.IsNullOrWhiteSpace(d.NotificationEmail) Then
                                If blnSendAlerts Then
                                    Dim smp As New Models.SchedMessagingParams With {.SHID = SHID, .Orders = Orders}
                                    SendResourceNotificationAsync(d.NotificationEmail, AMSMsg.OrdersNoPack, smp)
                                End If
                            End If
                        End If
                    End If
                End If

                If d.ValidateIOOn Then 'Only validate if this setting is turned ON
                    For Each b In BookControls
                        Dim spRes = db.spAMSIsOrderInbound(b, CompControl).FirstOrDefault() 'Get the Inbound(Del)/Outbound(Pick) status associated with the BookControl
                        If spRes.Inbound <> d.DockInbound Then
                            'if there are any orders which do NOT match the Inbound/Outbound status of the Dock then validation fails. The Carrier has to request and appointment
                            blnCanUseDock = False
                            Exit For
                        End If
                    Next
                Else
                    blnCanUseDock = False 'Added By LVV on 3/10/20 - Rob said we need to ensure that when Inbound/Outbound validation is turned off that this dock does not show up to carriers. Note: This will also effect on the Warehouse screen when the user click "View Availability" because the same code base is used.
                End If

                'If any one Of these tests fail Then this dock cannot be considered 
                If blnCanUseDock Then possibleDocks.Add(d)
            Next

            Return possibleDocks

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetPossibleDocks"), db)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Calculates the number of minutes required for the appointment based on the
    ''' characteristics of the orders and the Time Calculation Factor Rules configured
    ''' for the dock
    ''' </summary>
    ''' <param name="CTFRs"></param>
    ''' <param name="Books"></param>
    ''' <param name="iCTFRPalletIDs"></param>
    ''' <param name="orderPkgs"></param>
    ''' <param name="iAvgMinutes"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Function GetCalculatedMinutes(ByVal CTFRs() As LTS.tblDockTimeCalcFactor, ByVal Books() As LTS.Book, ByVal iCTFRPalletIDs() As LTS.CompRefPalletType, ByVal orderPkgs() As LTS.CompRefPalletType, ByVal iAvgMinutes As Integer) As Integer
        Dim iRet As Integer = iAvgMinutes
        If CTFRs?.Length < 1 Then Return iRet 'If there are no rules configured then return the average
        If iCTFRPalletIDs?.Length < 1 Then Return iRet 'If there are no rules configured for package type then return the average (to avoid null reference exception below - because apparently the rule is the packages types from the load all must have CTFR or else we use the average)

        Dim blnInvalidPackageType As Boolean = False
        Dim intTotalWMinutes As Integer = 0
        Dim intTotalQMinutes As Integer = 0
        Dim intTotalPMinutes As Integer = 0
        Dim intTotalCMinutes As Integer = 0

        Dim CompWgtUOM = GetParText("DefaultWeightUOM", Books(0).BookCustCompControl) 'Check the Company Parameter for Weight UOM for this BookCustCompControl (all orders should have same comp)
        Dim CompCubeUOM = GetParText("DefaultCubeUOM", Books(0).BookCustCompControl)

        For Each ctfr In CTFRs
            'Do this here so we don't repeat this calulcation for each book 
            Dim blnCalcWgt = False
            Dim blnCalcQty = False
            Dim blnCalcCube = False
            Dim blnCalcPkg = False
            If ctfr.DockTCFCalcFactorTypeControl = 2 AndAlso ctfr.DockTCFUOM.ToUpper() = CompWgtUOM.ToUpper() Then blnCalcWgt = True 'See If we have a CTFR for Weight in that UOM
            If ctfr.DockTCFCalcFactorTypeControl = 1 Then blnCalcQty = True 'See If we have a CTFR for Quantity
            If ctfr.DockTCFCalcFactorTypeControl = 4 AndAlso ctfr.DockTCFUOM.ToUpper() = CompCubeUOM.ToUpper() Then blnCalcCube = True 'See If we have a CTFR for Cube in that UOM
            If ctfr.DockTCFCalcFactorTypeControl = 3 Then
                'if the pkg UOM for the rule is not a pkg type used in the orders then we can skip processing this rule (to avoid a situation where we process a million loops for no reason - they could have rules for all pkg types but the order only uses 2)
                If orderPkgs.Select(Function(x) x.PalletType).Contains(ctfr.DockTCFUOM) Then
                    blnCalcPkg = True 'See If we have a CTFR for PackageType
                End If
            End If

            If ctfr.DockTCFAmount = 0 Then Exit For 'Avoid divide by 0 error

            If blnCalcWgt OrElse blnCalcQty OrElse blnCalcCube OrElse blnCalcPkg Then 'If none of these are true we can skip looping through all the records (ex in the case of where the rule is for wgt Kg but the load is in lbs)
                For Each book In Books
                    If blnCalcWgt Then
                        Dim wgt = If(book.BookTotalWgt.HasValue, book.BookTotalWgt.Value, 0)
                        intTotalWMinutes += System.Math.Ceiling((wgt * ctfr.DockTCFTimeFactor) / ctfr.DockTCFAmount)  'apply rule
                    ElseIf blnCalcQty Then
                        Dim qty = If(book.BookTotalCases.HasValue, book.BookTotalCases.Value, 0)
                        intTotalQMinutes += System.Math.Ceiling((qty * ctfr.DockTCFTimeFactor) / ctfr.DockTCFAmount)  'apply rule
                    ElseIf blnCalcCube Then
                        Dim cube = If(book.BookTotalCube.HasValue, book.BookTotalCube.Value, 0)
                        intTotalCMinutes += System.Math.Ceiling((cube * ctfr.DockTCFTimeFactor) / ctfr.DockTCFAmount)  'apply rule
                    ElseIf blnCalcPkg Then
                        For Each bookLoad In book.BookLoads
                            For Each itm In bookLoad.BookItems
                                If Not iCTFRPalletIDs.Select(Function(x) x.ID).Contains(itm.BookItemPalletTypeID) Then
                                    blnInvalidPackageType = True
                                    Exit For
                                End If
                                '*Try to match the rule to the load
                                If itm.BookItemPalletTypeID = iCTFRPalletIDs.Where(Function(x) x.PalletType.ToUpper() = ctfr.DockTCFUOM.ToUpper()).Select(Function(y) y.ID).FirstOrDefault() Then
                                    intTotalPMinutes += System.Math.Ceiling((itm.BookItemPallets * ctfr.DockTCFTimeFactor) / ctfr.DockTCFAmount)  'apply rule
                                End If
                            Next
                            If blnInvalidPackageType = True Then Exit For
                        Next
                    End If
                    If blnInvalidPackageType Then Exit For
                Next
            End If
            If blnInvalidPackageType Then Exit For
        Next
        If Not blnInvalidPackageType Then
            iRet = intTotalWMinutes
            If intTotalQMinutes > iRet Then iRet = intTotalQMinutes
            If intTotalCMinutes > iRet Then iRet = intTotalCMinutes
            If intTotalPMinutes > iRet Then iRet = intTotalPMinutes
        End If
        Return iRet
    End Function

    ''' <summary>
    ''' Gets all appointments and block out periods scheduled at this dock on this day
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="dt"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="DockDoorID"></param>
    ''' <param name="DockHourStart"></param>
    ''' <param name="DockHourEnd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/10/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''   we no longer use the DockDoorID parameter, it 
    '''   is left in the contract for backward compatibility
    '''   wke now pass DockControl to the new overload of GetApptTimesOnDay
    ''' </remarks>
    Private Function GetScheduledSlots(ByRef db As NGLMASCompDataContext, ByVal dt As Date, ByVal DockControl As Integer, ByVal DockDoorID As String, ByVal DockHourStart As Date, ByVal DockHourEnd As Date) As Models.TimeSlot()
        Try
            Dim oAMS As New NGLAMSAppointmentData(Parameters)
            Dim oBlock As New NGLDockBlockOutPeriodData(Parameters)
            Dim slotList As New List(Of Models.TimeSlot)

            'Get a list of all appointments scheduled at the dock for the day 
            'Modified by RHR for v-8.2.1 on 10/16/2019
            slotList = oAMS.GetApptTimesOnDay(DockControl, dt)
            'Add all block out periods scheduled at the dock for the day to the list
            Dim blocks = oBlock.GetActiveBlockOutPeriodsByDock(db, DockControl)
            For Each b In blocks
                Dim blockStart As Date?
                Dim blockEnd As Date?
                Dim blnAllDay As Boolean
                'If the BlockOut is on this date and the start and end times are valid, add it to the list
                If b.getAppointmentByDate(dt, blockStart, blockEnd, blnAllDay) Then
                    If Not blockStart Is Nothing AndAlso Not blockEnd Is Nothing Then
                        slotList.Add(New Models.TimeSlot With {.Start = blockStart.Value, .End = blockEnd.Value})
                    End If
                End If
            Next

            'Dock Hours
            'When calculating the open slots available we must make sure we only pick slots that are within the configured Dock Hours

            'If any appointment End times In the list are less than DockHourStart Then remove these from the list (Carriers cannot schedule before DockHourStart) 
            'If any appointment start times In the list are greater than DockHourEnd Then remove these from the list (Carriers cannot schedule after DockHourEnd)
            Dim retVals As New List(Of Models.TimeSlot)
            For Each s In slotList
                If (s.End.TimeOfDay >= DockHourStart.TimeOfDay) AndAlso (s.Start.TimeOfDay <= DockHourEnd.TimeOfDay) Then
                    retVals.Add(s)
                End If
            Next

            If retVals?.Count > 0 Then
                'Sort by start time and put into an array so we can accurately access the first and last records
                Dim tempArray = retVals.OrderBy(Function(x) x.Start).ToArray()

                'If the start time Of the first record is Greater than DockHourStart --> add a dummy record that has both the start And end times = DockHourStart
                If tempArray(0).Start.TimeOfDay > DockHourStart.TimeOfDay Then
                    Dim DHStart = dt.Date.Add(DockHourStart.TimeOfDay)
                    retVals.Add(New Models.TimeSlot With {.Start = DHStart, .End = DHStart})
                End If
                'If the End time Of the last record is Less than DockHourEnd --> add a dummy record that has both the start And end times = DockHourEnd
                If tempArray(tempArray.Length - 1).End.TimeOfDay < DockHourEnd.TimeOfDay Then
                    Dim DHEnd = dt.Date.Add(DockHourEnd.TimeOfDay)
                    retVals.Add(New Models.TimeSlot With {.Start = DHEnd, .End = DHEnd})
                End If
            Else
                'If there is nothing scheduled then we have to add in dummy records for the Dock Hour Start and End times
                Dim DHStart = dt.Date.Add(DockHourStart.TimeOfDay)
                Dim DHEnd = dt.Date.Add(DockHourEnd.TimeOfDay)
                retVals.Add(New Models.TimeSlot With {.Start = DHStart, .End = DHStart})
                retVals.Add(New Models.TimeSlot With {.Start = DHEnd, .End = DHEnd})
            End If

            Return retVals.OrderBy(Function(x) x.Start).ToArray()

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetScheduledSlots"), db)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Uses the minimum appointment length for the dock, required minutes for the appointment, and the scheduled time slots to calculate
    ''' the available time slots at this dock
    ''' </summary>
    ''' <param name="NeededApptMins"></param>
    ''' <param name="minLen"></param>
    ''' <param name="scheduledSlots"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Function GetAvailableOpenings(ByVal NeededApptMins As Integer, ByVal minLen As Integer, ByVal scheduledSlots As Models.TimeSlot()) As List(Of Models.TimeSlot)
        Try
            Dim availableSlots As New List(Of Models.TimeSlot)

            For i As Integer = 0 To scheduledSlots.Length - 2
                'Get the first 2 appointments
                Dim recordA = scheduledSlots(i)
                Dim recordB = scheduledSlots(i + 1)

                'Read end time from first record And read start time from next record to get the open interval between them
                Dim iST = recordA.End
                Dim iET = recordB.Start

                'Get #of min between the start and end of the time interval (#of mins available between existing appointments)
                Dim AvailableMins = Math.Floor((iET.TimeOfDay - iST.TimeOfDay).TotalMinutes)

                'If the open interval is larger than the time needed then divide the interval up into all possible appointments and add them to a list
                'Else we can't schedule anything here so move on to the next interval
                If AvailableMins > NeededApptMins Then
                    '--> Calculate the time slots

                    Dim firstStart = iST 'The first start time will be the interval start time (aka the endtime of the first record)
                    Dim lastStart = iET.AddMinutes(-NeededApptMins) 'The last start time will be NeededApptMins subtracted from the interval end time (aka start time from second record)

                    'The first available time slot is always firstStart + NeededApptMins (because we already determined that the interval is large enough to contain at least one possibility)
                    availableSlots.Add(New Models.TimeSlot With {.Start = firstStart, .End = firstStart.AddMinutes(NeededApptMins)})

                    Dim nextStart = firstStart.AddMinutes(minLen) 'Add minLen to the firstStart to get the next possible start time
                    Do While nextStart < lastStart
                        'If this time is after the last possible start time Then we know it won't fit in the time interval so exit the loop
                        'If the next start time is before the last possible start then we know the appointment will fit here so add it to the list (because of how last start time is calculated)
                        availableSlots.Add(New Models.TimeSlot With {.Start = nextStart, .End = nextStart.AddMinutes(NeededApptMins)})
                        'Add minLen to nextStart to get the next start time to be tested and repeat the process
                        nextStart = nextStart.AddMinutes(minLen)
                    Loop
                    'Don't forget to add the last start time to the list
                    availableSlots.Add(New Models.TimeSlot With {.Start = lastStart, .End = iET})
                End If
            Next

            Return availableSlots

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetAvailableOpenings"))
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets all the Temp Types and Package Types represented by the appointment orders
    ''' --RULES:
    ''' --First lets look at booking records that have item details. 
    ''' --     Map to the BookItemPalletTypeID, BookItemCommCode
    ''' --     Conversion may be required some tables store control numbers And some store characters. 
    ''' --     If one items value Is are empty Or zero ignore this items in the evaluation. 
    ''' --     If all values are empty Or zero use the BookLoad rule below.
    ''' --For booking records that do Not have items we must reference the BookLoad table
    ''' --	1.	Use the BookLoadCom for temperature
    ''' --	2.	Use the BookLoadPType for the pallet type, this should map to the PalletType.PalletType field (text).  
    ''' --      This Is a newer field And may Not always have a value. 
    ''' --      If there are no items And this field Is null Or empty assume any dock door Is acceptable.
    ''' </summary>
    ''' <param name="orderPkgs"></param>
    ''' <param name="orderTmps"></param>
    ''' <param name="Books"></param>
    ''' <param name="AllPalletTypes"></param>
    ''' <param name="AllTempTypes"></param>
    ''' <remarks>
    ''' Added By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Sub GetPackTempForApptBooks(ByRef orderPkgs() As LTS.CompRefPalletType, ByRef orderTmps() As Integer, ByRef Orders As String, ByVal Books() As LTS.Book, ByVal AllPalletTypes() As LTS.CompRefPalletType, ByVal AllTempTypes() As LTS.CompRefTempType, ByVal CompControl As Integer, ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer, ByVal blnCarrierAlg As Boolean)
        ''--RULES:
        ''--First lets look at booking records that have item details. 
        ''--     Map to the BookItemPalletTypeID, BookItemCommCode
        ''--     Conversion may be required some tables store control numbers And some store characters. 
        ''--     If one items value Is are empty Or zero ignore this items in the evaluation. 
        ''--     If all values are empty Or zero use the BookLoad rule below.
        ''--For booking records that do Not have items we must reference the BookLoad table
        ''--	1.	Use the BookLoadCom for temperature
        ''--	2.	Use the BookLoadPType for the pallet type, this should map to the PalletType.PalletType field (text).  
        ''--      This Is a newer field And may Not always have a value. 
        ''--      If there are no items And this field Is null Or empty assume any dock door Is acceptable.

        Dim blnGetOrderString As Boolean = False
        If String.IsNullOrWhiteSpace(Orders) Then blnGetOrderString = True

        Dim pkgList As New List(Of LTS.CompRefPalletType)
        Dim tmpList As New List(Of Integer)
        Dim sep = ""
        For Each book In Books
            If blnGetOrderString Then
                Orders += (sep + book.BookCarrOrderNumber)
                sep = ", "
            End If
            For Each bookLoad In book.BookLoads
                Dim blnPkgUseBookLoad As Boolean = True
                Dim blnTmpUseBookLoad As Boolean = True

                For Each itm In bookLoad.BookItems
                    If itm.BookItemPalletTypeID <> 0 Then
                        Dim p = AllPalletTypes.Where(Function(x) x.ID = itm.BookItemPalletTypeID).FirstOrDefault()
                        If Not p Is Nothing Then
                            pkgList.Add(p)
                            blnPkgUseBookLoad = False
                        End If
                    End If
                    If Not String.IsNullOrWhiteSpace(itm.BookItemCommCode) Then
                        Dim t = AllTempTypes.Where(Function(x) x.CommCodeType = itm.BookItemCommCode).Select(Function(y) y.TempTypeBitPos).FirstOrDefault()
                        If t <> Nothing AndAlso t <> 0 Then
                            tmpList.Add(t)
                            blnTmpUseBookLoad = False
                        End If
                    End If
                Next

                If blnPkgUseBookLoad Then
                    Dim p = AllPalletTypes.Where(Function(x) x.PalletType = bookLoad.BookLoadPType).FirstOrDefault()
                    If Not p Is Nothing Then pkgList.Add(p)
                End If
                If blnTmpUseBookLoad Then
                    Dim t = AllTempTypes.Where(Function(x) x.CommCodeType = bookLoad.BookLoadCom).Select(Function(y) y.TempTypeBitPos).FirstOrDefault()
                    If t <> Nothing AndAlso t <> 0 Then tmpList.Add(t)
                End If
            Next
        Next
        orderPkgs = pkgList.Distinct().ToArray()
        orderTmps = tmpList.Distinct().ToArray()

        'Send Alerts if necessary
        If blnCarrierAlg Then
            Dim shid = Books.Where(Function(x) x.BookSHID <> Nothing AndAlso x.BookSHID.Length > 0).Select(Function(y) y.BookSHID).FirstOrDefault()
            Dim smp As New Models.SchedMessagingParams With {.CompControl = CompControl, .CarrierControl = CarrierControl, .CarrierNumber = CarrierNumber, .Orders = Orders, .shid = shid}
            If orderPkgs Is Nothing OrElse orderPkgs?.Length < 1 Then
                'Send Subscription Alert
                SendSchedulerAlertAsync(AMSMsg.OrdersNoPack, smp)
            End If
            If orderTmps Is Nothing OrElse orderPkgs?.Length < 1 Then
                'Send Subscription Alert
                SendSchedulerAlertAsync(AMSMsg.OrdersNoTemp, smp)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets the string of Order Numbers for the appointment. Also returns the SHID by reference
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="EquipmentID"></param>
    ''' <param name="BookDateLoad"></param>
    ''' <param name="BookDateRequired"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="Inbound"></param>
    ''' <returns></returns>
    Private Function GetOrdersForAppt(ByRef SHID As String,
                                          ByVal CarrierControl As Integer,
                                          ByVal CompControl As Integer,
                                          ByVal BookControl As Integer,
                                          ByVal EquipmentID As String,
                                          ByVal BookDateLoad As Date,
                                          ByVal BookDateRequired As Date,
                                          ByVal IsPickup As Boolean,
                                          ByVal Inbound As Boolean) As String
        Dim Orders As String = ""
        Try
            Dim oBook As New NGLBookData(Parameters)
            Dim dt As Date
            If IsPickup Then dt = BookDateLoad Else dt = BookDateRequired 'Only do this for one day and that day is the BookDateLoad on Pickup And BookDateRequired on Delivery
            'Get all the Book records that are included in this Carrier Appointment (By SHID and also EquipID)
            Dim books = oBook.GetAMSDependentBooks(BookControl, IsPickup, True)
            SHID = books.Where(Function(x) x.BookControl = BookControl).Select(Function(y) y.BookSHID).FirstOrDefault()
            Dim sep = ""
            For Each book In books
                Orders += (sep + book.BookCarrOrderNumber)
                sep = ", "
            Next
        Catch ex As Exception
            'ignore any errors
        End Try
        Return Orders
    End Function

    ''' <summary>
    ''' Calculates if the provided books will fit on an existing appointment. Also returns the string of Order Numbers by reference
    ''' </summary>
    ''' <param name="strOrders"></param>
    ''' <param name="books"></param>
    ''' <param name="AMSApptControl"></param>
    ''' <returns></returns>
    Public Function DoesOrderFitOnAppointment(ByRef strOrders As String, ByVal books() As LTS.Book, ByVal AMSApptControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the appointment 
                Dim oAppt As New NGLAMSAppointmentData(Parameters)
                Dim a = oAppt.GetAppointment(AMSApptControl)
                Dim dockControl = db.CompDockDoors.Where(Function(x) x.DockDoorID = a.AMSApptDockdoorID).Select(Function(y) y.CompDockContol).FirstOrDefault()
                'Get the average minutes for the dock where the appointment is scheduled
                Dim ds = db.tblDockSettings.Where(Function(x) x.DockSettingControl = dockControl AndAlso x.DockSettingEnumID = DockSetting.DefaultApptMins).FirstOrDefault()
                Dim intAvg = 0
                Integer.TryParse(ds.DockSettingFixed, intAvg)
                'Cache all records from the Pallet Type And Temp Type tables
                Dim AllPalletTypes = db.CompRefPalletTypes.ToArray()
                Dim AllTempTypes = db.CompRefTempTypes.ToArray()
                'Get the lists of distinct package and temp types used on the loads so we can use them for validation
                Dim orderPkgs() As LTS.CompRefPalletType
                Dim orderTmps() As Integer
                GetPackTempForApptBooks(orderPkgs, orderTmps, strOrders, books, AllPalletTypes, AllTempTypes, a.AMSApptCompControl, a.AMSApptCarrierControl, 0, False)
                'Get the Calculation Time Factor Rules for the dock
                Dim dockCTFRs = (From d In db.tblDockTimeCalcFactors Where d.DockTCFCompDockContol = dockControl AndAlso d.DockTCFOn = True Select d).ToArray()
                'Get the PalletTypeID's for all package types that have CTFRs for this dock (PalletTypeID is what is stored in BookItem and PalletType is what is stored in tblDockTimeCalcFactors so we need translate using AllPalletTypes                   
                Dim ctfrPalletIDs = (From d In dockCTFRs Join x In AllPalletTypes On d.DockTCFUOM Equals x.PalletType Where d.DockTCFCalcFactorTypeControl = 3 Select x).ToArray()
                'Calculate the minutes needed for the old appointment plus the new order in question
                Dim newNeededMins = GetCalculatedMinutes(dockCTFRs, books, ctfrPalletIDs, orderPkgs, intAvg)
                'Calucate the number of minutes the current appointment is scheduled for
                Dim oldNeededMins = DateDiff(DateInterval.Minute, a.AMSApptStartDate.Value, a.AMSApptEndDate.Value)
                'If the new required minutes are less than or equal to the original then we can add this order to the appointment
                If newNeededMins <= oldNeededMins Then
                    blnRet = True
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesOrderFitOnAppointment"), db)
            End Try
            Return blnRet
        End Using
    End Function

#End Region

#Region "Request Object Methods"

    Private Function GetDeleteRequestObject(ByVal CompControl As Integer,
                                                ByVal CarrierControl As Integer,
                                                ByVal BookControl As Integer,
                                                ByVal EquipmentID As String,
                                                ByVal BookDateLoad As Date,
                                                ByVal BookDateRequired As Date,
                                                ByVal IsPickup As Boolean,
                                                ByVal Inbound As Boolean,
                                                ByVal Warehouse As String,
                                                ByVal SHID As String,
                                                ByVal CarrierName As String,
                                                ByVal BookAMSPickupApptControl As Integer,
                                                ByVal BookAMSDeliveryApptControl As Integer) As Models.AMSCarrierResults
        Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

        Dim Orders As String = ""
        Dim refshid = ""
        Orders = GetOrdersForAppt(refshid, CarrierControl, CompControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound)

        Dim strEmail = GetRequestEmail(CompControl, Warehouse)
        Dim fmSub = oLocalize.GetLocalizedValueByKey("SchedDeleteApptReq", "{0} - Carrier Delete Appointment Request")
        Dim strSubject = String.Format(fmSub, SHID)
        Dim strBody = ""
        If IsPickup Then
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestDeletePick", "Carrier {1} ({9}) is requesting to delete Appointment #{8} at Warehouse {2} for the following Pickup:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), BookAMSPickupApptControl, Parameters.UserName + "/" + Parameters.UserEmail)
        Else
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestDeleteDel", "Carrier {1} ({9}) is requesting to delete Appointment #{8} at Warehouse {2} for the following Delivery:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), BookAMSDeliveryApptControl, Parameters.UserName + "/" + Parameters.UserEmail)
        End If

        Dim result As New Models.AMSCarrierResults With {
                    .AvailableSlots = Nothing,
                    .blnMustRequestAppt = True,
                    .RequestSendToEmail = strEmail,
                    .Body = strBody,
                    .Subject = strSubject}

        Return result
    End Function

    Private Function GetEditRequestObject(ByVal CompControl As Integer,
                                              ByVal CarrierControl As Integer,
                                              ByVal BookControl As Integer,
                                              ByVal EquipmentID As String,
                                              ByVal BookDateLoad As Date,
                                              ByVal BookDateRequired As Date,
                                              ByVal IsPickup As Boolean,
                                              ByVal Inbound As Boolean,
                                              ByVal Warehouse As String,
                                              ByVal SHID As String,
                                              ByVal CarrierName As String,
                                              ByVal BookAMSPickupApptControl As Integer,
                                              ByVal BookAMSDeliveryApptControl As Integer) As Models.AMSCarrierResults
        Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

        Dim Orders As String = ""
        Dim refshid = ""
        Orders = GetOrdersForAppt(refshid, CarrierControl, CompControl, BookControl, EquipmentID, BookDateLoad, BookDateRequired, IsPickup, Inbound)

        Dim strEmail = GetRequestEmail(CompControl, Warehouse)
        Dim fmSub = oLocalize.GetLocalizedValueByKey("SchedChangeApptReq", "{0} - Carrier Change Appointment Request")
        Dim strSubject = String.Format(fmSub, SHID)
        Dim strBody = ""
        If IsPickup Then
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestChangePick", "Carrier {1} ({9}) is requesting a change to Appointment #{8} at Warehouse {2} for the following Pickup:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), BookAMSPickupApptControl, Parameters.UserName + "/" + Parameters.UserEmail)
        Else
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestChangeDel", "Carrier {1} ({9}) is requesting a change to Appointment #{8} at Warehouse {2} for the following Delivery:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), BookAMSDeliveryApptControl, Parameters.UserName + "/" + Parameters.UserEmail)
        End If

        Dim result As New Models.AMSCarrierResults With {
                                .AvailableSlots = Nothing,
                                .blnMustRequestAppt = True,
                                .RequestSendToEmail = strEmail,
                                .Body = strBody,
                                .Subject = strSubject}

        Return result
    End Function

    ''' <summary>
    ''' LVV NOTE: This is me messing around to try to optimize and make it more clear. Not entirely necessary at this point but I want to fix it eventually because I hate it
    ''' Even I can't always follow it. We need to get more values from the database instead of passing everything in from the client.
    ''' </summary>
    ''' <param name="oLocalize"></param>
    ''' <param name="record"></param>
    ''' <param name="Orders"></param>
    ''' <param name="SendRequestEmails"></param>
    ''' <returns></returns>
    Private Function GetCreateRequestObjectEXPERIMENT(ByRef oLocalize As NGLcmLocalizeKeyValuePairData,
                                                          ByVal record As Models.AMSCarrierRecord,
                                                          ByVal Orders As String,
                                                          ByVal SendRequestEmails As String) As Models.AMSCarrierResults
        Dim strEmail = GetRequestEmail(record.CompControl, record.Warehouse, SendRequestEmails)
        Dim fmSub = oLocalize.GetLocalizedValueByKey("SchedCreateApptReq", "{0} - Carrier Create Appointment Request")
        Dim strSubject = String.Format(fmSub, record.SHID)
        Dim strBody = ""
        If record.IsPickup Then
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestCreatePick", "Carrier {1} ({8}) is requesting an appointment at Warehouse {2} for the following Pickup:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", record.CarrierName, record.Warehouse, record.SHID, record.EquipID, Orders, record.BookDateLoad.Value.ToString("M/d/yy HH:mm"), record.BookDateRequired.Value.ToString("M/d/yy HH:mm"), Parameters.UserName + "/" + Parameters.UserEmail)
        Else
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestCreateDel", "Carrier {1} ({8}) is requesting an appointment at Warehouse {2} for the following Delivery:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", record.CarrierName, record.Warehouse, record.SHID, record.EquipID, Orders, record.BookDateLoad.Value.ToString("M/d/yy HH:mm"), record.BookDateRequired.Value.ToString("M/d/yy HH:mm"), Parameters.UserName + "/" + Parameters.UserEmail)
        End If
        Dim result As New Models.AMSCarrierResults With {
                                .AvailableSlots = Nothing,
                                .blnMustRequestAppt = True,
                                .RequestSendToEmail = strEmail,
                                .Body = strBody,
                                .Subject = strSubject}
        Return result
    End Function

    Private Function GetCreateRequestObject(ByRef oLocalize As NGLcmLocalizeKeyValuePairData,
                                                ByVal CompControl As Integer,
                                                ByVal EquipmentID As String,
                                                ByVal BookDateLoad As Date,
                                                ByVal BookDateRequired As Date,
                                                ByVal IsPickup As Boolean,
                                                ByVal Orders As String,
                                                ByVal Warehouse As String,
                                                ByVal SHID As String,
                                                ByVal CarrierName As String,
                                                ByVal SendRequestEmails As String) As Models.AMSCarrierResults

        Dim strEmail = GetRequestEmail(CompControl, Warehouse, SendRequestEmails)
        Dim fmSub = oLocalize.GetLocalizedValueByKey("SchedCreateApptReq", "{0} - Carrier Create Appointment Request")
        Dim strSubject = String.Format(fmSub, SHID)
        Dim strBody = ""
        If IsPickup Then
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestCreatePick", "Carrier {1} ({8}) is requesting an appointment at Warehouse {2} for the following Pickup:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), Parameters.UserName + "/" + Parameters.UserEmail)
        Else
            Dim sFormat = oLocalize.GetLocalizedValueByKey("SchedRequestCreateDel", "Carrier {1} ({8}) is requesting an appointment at Warehouse {2} for the following Delivery:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
            strBody = String.Format(sFormat, "<br />", CarrierName, Warehouse, SHID, EquipmentID, Orders, BookDateLoad.ToString("M/d/yy HH:mm"), BookDateRequired.ToString("M/d/yy HH:mm"), Parameters.UserName + "/" + Parameters.UserEmail)
        End If

        Dim result As New Models.AMSCarrierResults With {
                                .AvailableSlots = Nothing,
                                .blnMustRequestAppt = True,
                                .RequestSendToEmail = strEmail,
                                .Body = strBody,
                                .Subject = strSubject}

        Return result
    End Function

    ''' <summary>
    ''' Get the Carrier Request Email from the Comp Parameter
    '''   If not exists, try provided SendRequestEmails
    '''     If not exists, check if this Company has any associated CompContacts
    '''       If yes, try to get an email from a contact with Notify checked
    '''         If not exists, get the first contact with an email that is not null
    '''       Else, try to get the CompEmail from the Company
    ''' (I assume it wont get here and at least one of the above will exist)
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="SendRequestEmails">Optional; only used for Create Requests (these are Resource Notification emails)</param>
    ''' <returns>String</returns>
    ''' <remarks>Added By LVV on 9/26/18 for v-8.3 TMS365 Scheduler</remarks>
    Private Function GetRequestEmail(ByVal CompControl As Integer, ByVal Warehouse As String, Optional ByVal SendRequestEmails As String = "") As String
        Dim strEmail As String = ""
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                strEmail = GetParText("CarrierRequestAppointmentEmail", CompControl)
                If String.IsNullOrWhiteSpace(strEmail) Then
                    strEmail = SendRequestEmails
                    If String.IsNullOrWhiteSpace(strEmail) Then
                        If db.CompConts.Any(Function(x) x.CompContCompControl = CompControl AndAlso (x.CompContEmail <> Nothing AndAlso x.CompContEmail.Length > 0)) Then
                            strEmail = db.CompConts.Where(Function(x) x.CompContCompControl = CompControl AndAlso (x.CompContEmail <> Nothing AndAlso x.CompContEmail.Length > 0) AndAlso x.CompContTender = True).Select(Function(y) y.CompContEmail).FirstOrDefault()
                            If String.IsNullOrWhiteSpace(strEmail) Then
                                strEmail = db.CompConts.Where(Function(x) x.CompContCompControl = CompControl AndAlso (x.CompContEmail <> Nothing AndAlso x.CompContEmail.Length > 0)).OrderBy(Function(g) g.CompContControl).Select(Function(y) y.CompContEmail).FirstOrDefault()
                            End If
                        Else
                            strEmail = db.Comps.Where(Function(x) x.CompControl = CompControl).Select(Function(y) y.CompEmail).FirstOrDefault()
                        End If
                    End If
                    'If the parameter was blank but we found another email then send that contact an email notifying them that the parameter is blank
                    If Not String.IsNullOrWhiteSpace(strEmail) Then
                        Dim smp As New Models.SchedMessagingParams With {.Warehouse = Warehouse}
                        SendResourceNotificationAsync(strEmail, AMSMsg.MissingRequestEmail, smp)
                    End If
                End If
                Return strEmail
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetRequestEmail"), db)
            End Try
        End Using
        Return strEmail
    End Function

#End Region

#Region "Resource Notification Methods"

    'Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler
    Public Delegate Sub SendResourceNotificationDelegate(ByVal EmailTo As String, ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)

    Public Sub SendResourceNotificationAsync(ByVal EmailTo As String, ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)
        Dim fetcher As New SendResourceNotificationDelegate(AddressOf Me.SendResourceNotification)
        ' Launch thread
        fetcher.BeginInvoke(EmailTo, e, smp, Nothing, Nothing)
    End Sub

    Private Function SendResourceNotification(ByVal EmailTo As String, ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams) As Boolean
        Try
            Dim MsgType = AMSMsgType.AMSResourceNotify
            Select Case e
                Case AMSMsg.MaxApptLenExceeded
                    SendAMSNoAptAvailableMessage(MsgType, AMSMsg.MaxApptLenExceeded, EmailTo, smp.SHID, smp.Orders, smp.EquipID, smp.ogDockName, smp.BookDateLoad, smp.BookDateRequired, smp.dt, smp.CompControl, smp.CompNumber, smp.Warehouse, smp.CarrierControl, smp.CarrierNumber, smp.CarrierName)
                Case AMSMsg.NoTimeSlotAvailable
                    SendAMSNoAptAvailableMessage(MsgType, AMSMsg.NoTimeSlotAvailable, EmailTo, smp.SHID, smp.Orders, smp.EquipID, smp.ogDockName, smp.BookDateLoad, smp.BookDateRequired, smp.dt, smp.CompControl, smp.CompNumber, smp.Warehouse, smp.CarrierControl, smp.CarrierNumber, smp.CarrierName)
                Case AMSMsg.OrdersNoTemp
                    SendAMSNoPackTempMessage(MsgType, AMSMsg.OrdersNoTemp, EmailTo, smp.SHID, smp.Orders, smp.CompControl, smp.CompNumber, smp.CarrierControl, smp.CarrierNumber)
                Case AMSMsg.OrdersNoPack
                    SendAMSNoPackTempMessage(MsgType, AMSMsg.OrdersNoPack, EmailTo, smp.SHID, smp.Orders, smp.CompControl, smp.CompNumber, smp.CarrierControl, smp.CarrierNumber)
                Case AMSMsg.CarrierDelete
                    SendAMSDeleteMessage(MsgType, AMSMsg.CarrierDelete, smp.spDets, smp.ApptControl, smp.CompControl)
                Case AMSMsg.CarrierModify
                    SendAMSModifyMessage(MsgType, AMSMsg.CarrierModify, smp.ApptControl, smp.CompControl, smp.ogDockID, smp.ogApptStart, smp.ogApptEnd)
                Case AMSMsg.CarrierBooked
                    SendAMSCreateMessage(MsgType, AMSMsg.CarrierBooked, smp.ApptControl, smp.CompControl)
                Case AMSMsg.MissingRequestEmail
                    SendAMSMissingRequestEmailMessage(MsgType, AMSMsg.MissingRequestEmail, EmailTo, smp.Warehouse)
            End Select
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendResourceNotification"))
        End Try
    End Function

#End Region

#Region "Scheduler Alert Methods"

    Public Delegate Sub SendSchedulerAlertDelegate(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)

    ''' <summary>
    ''' Caller methods -> Find out the Models.SchedMessagingParams fields required for each message type by looking at the code below
    ''' Message Type : Required Fields
    '''   OrdersNoTemp/OrdersNoPack : CompControl, CompNumber, CarrierControl, CarrierNumber, SHID, Orders
    '''   CarrierDelete : ApptControl, CompControl, spDets
    '''   CarrierModify : ApptControl, CompControl, ogDockID, ogApptStart, ogApptEnd
    '''   CarrierBooked : ApptControl, CompControl
    '''   NoAppointmentAvailable :  CompControl, CompNumber, CarrierControl, CarrierNumber, CarrierName, Warehouse, SHID, EquipID, Orders, BookDateLoad, BookDateRequired
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="smp"></param>
    Public Sub SendSchedulerAlertAsync(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)
        Dim fetcher As New SendSchedulerAlertDelegate(AddressOf Me.SendSchedulerAlert)
        fetcher.BeginInvoke(e, smp, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Caller methods -> Find out the Models.SchedMessagingParams fields required for each message type by looking at the code below
    ''' Message Type : Required Fields
    '''   OrdersNoTemp/OrdersNoPack : CompControl, CompNumber, CarrierControl, CarrierNumber, SHID, Orders
    '''   CarrierDelete : ApptControl, CompControl, spDets
    '''   CarrierModify : ApptControl, CompControl, ogDockID, ogApptStart, ogApptEnd
    '''   CarrierBooked : ApptControl, CompControl
    '''   NoAppointmentAvailable :  CompControl, CompNumber, CarrierControl, CarrierNumber, CarrierName, Warehouse, SHID, EquipID, Orders, BookDateLoad, BookDateRequired
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="smp"></param>
    Private Sub SendSchedulerAlert(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)
        Try
            Dim CompNumber As Integer = 0 'TODO -- SEE IF WE CAN GET THIS FROM CALLER 
            Dim MsgType = AMSMsgType.AMSAlert
            Select Case e
                Case AMSMsg.NoAppointmentAvailable
                    SendAMSNoAptAvailableMessage(MsgType, AMSMsg.NoAppointmentAvailable, Nothing, smp.SHID, smp.Orders, smp.EquipID, smp.ogDockName, smp.BookDateLoad, smp.BookDateRequired, smp.dt, smp.CompControl, smp.CompNumber, smp.Warehouse, smp.CarrierControl, smp.CarrierNumber, smp.CarrierName)
                Case AMSMsg.OrdersNoTemp
                    SendAMSNoPackTempMessage(MsgType, AMSMsg.OrdersNoTemp, Nothing, smp.SHID, smp.Orders, smp.CompControl, smp.CompNumber, smp.CarrierControl, smp.CarrierNumber)
                Case AMSMsg.OrdersNoPack
                    SendAMSNoPackTempMessage(MsgType, AMSMsg.OrdersNoPack, Nothing, smp.SHID, smp.Orders, smp.CompControl, smp.CompNumber, smp.CarrierControl, smp.CarrierNumber)
                Case AMSMsg.CarrierDelete
                    SendAMSDeleteMessage(MsgType, AMSMsg.CarrierDelete, smp.spDets, smp.ApptControl, smp.CompControl)
                Case AMSMsg.CarrierModify
                    SendAMSModifyMessage(MsgType, AMSMsg.CarrierModify, smp.ApptControl, smp.CompControl, smp.ogDockID, smp.ogApptStart, smp.ogApptEnd)
                Case AMSMsg.CarrierBooked
                    SendAMSCreateMessage(MsgType, AMSMsg.CarrierBooked, smp.ApptControl, smp.CompControl)
            End Select
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendSchedulerAlert"))
        End Try
    End Sub

#End Region

#Region "Carrier Confirmation/Notification Methods"

    'Added By LVV on 9/26/18 for v-8.3 TMS365 Scheduler
    Public Delegate Sub SchedulerSendCarrierEmailDelegate(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)

    ''' <summary>
    ''' Caller methods -> Find out the Models.SchedMessagingParams fields required for each message type by looking at the code below
    ''' Message Type : Required Fields
    '''   DeleteConfirm/WHDelete : ApptControl, CompControl, spDets,
    '''   ModifyConfirm/WHModify : ApptControl, CompControl, ogDockID, ogApptStart, ogApptEnd
    '''   BookedConfirm/WHBooked : ApptControl, CompControl
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="smp"></param>
    Public Sub SchedulerSendCarrierEmailAsync(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)
        Dim fetcher As New SchedulerSendCarrierEmailDelegate(AddressOf Me.SchedulerSendCarrierEmail)
        ' Launch thread
        fetcher.BeginInvoke(e, smp, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Caller methods -> Find out the Models.SchedMessagingParams fields required for each message type by looking at the code below
    ''' Message Type : Required Fields
    '''   DeleteConfirm/WHDelete : ApptControl, CompControl, spDets,
    '''   ModifyConfirm/WHModify : ApptControl, CompControl, ogDockID, ogApptStart, ogApptEnd
    '''   BookedConfirm/WHBooked : ApptControl, CompControl
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="smp"></param>
    Private Sub SchedulerSendCarrierEmail(ByVal e As AMSMsg, ByVal smp As Models.SchedMessagingParams)
        Try
            Dim MsgType = AMSMsgType.AMSCarrierMsg
            Select Case e
                Case AMSMsg.DeleteConfirm
                    SendAMSDeleteMessage(MsgType, AMSMsg.DeleteConfirm, smp.spDets, smp.ApptControl, smp.CompControl)
                Case AMSMsg.ModifyConfirm
                    SendAMSModifyMessage(MsgType, AMSMsg.ModifyConfirm, smp.ApptControl, smp.CompControl, smp.ogDockID, smp.ogApptStart, smp.ogApptEnd)
                Case AMSMsg.BookedConfirm
                    SendAMSCreateMessage(MsgType, AMSMsg.BookedConfirm, smp.ApptControl, smp.CompControl)
                Case AMSMsg.WHDelete
                    SendAMSDeleteMessage(MsgType, AMSMsg.WHDelete, smp.spDets, smp.ApptControl, smp.CompControl)
                Case AMSMsg.WHModify
                    SendAMSModifyMessage(MsgType, AMSMsg.WHModify, smp.ApptControl, smp.CompControl, smp.ogDockID, smp.ogApptStart, smp.ogApptEnd)
                Case AMSMsg.WHBooked
                    SendAMSCreateMessage(MsgType, AMSMsg.WHBooked, smp.ApptControl, smp.CompControl)
            End Select
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SchedulerSendCarrierEmail"))
        End Try
    End Sub

#End Region

#Region "Shared Messaging Methods"

    '** MESSAGE CONSTRUCTION **
    Private Sub SendAMSDeleteMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal spDets As LTS.spAMSGetDetailsForMessagingResult, ByVal ApptControl As Integer, ByVal CompControl As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                If CompControl = 0 OrElse spDets Is Nothing Then Return

                Dim blnSendCarNotification As Boolean = True
                Dim blnSendAlert As Boolean = True
                Dim blnResCanSendEmail As Boolean = False
                Dim subject As String = "", body As String = "", alertName As String = "", alertDesc As String = ""
                Dim strDt = "", strStart = "", strEnd = ""

                'Format the dates
                strDt = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortDateString(), "")
                strStart = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortTimeString(), "")
                strEnd = If(spDets.EndDate.HasValue, spDets.EndDate.Value.ToShortTimeString(), "")
                'Check to see if the dock(s) has notifications on and a valid (non null) email address
                If spDets.DockNotificationAlert AndAlso Not String.IsNullOrWhiteSpace(spDets.DockNotificationEmail) Then blnResCanSendEmail = True

                'Get the Details part of the message
                Dim strDetails As String = ""
                Select Case e
                    Case AMSMsg.DeleteConfirm, AMSMsg.CarrierDelete, AMSMsg.WHDelete
                        Dim p = New String() {"<br />", ApptControl, spDets.CompName, strDt, strStart, strEnd, spDets.DockName, spDets.EquipID, spDets.SHID, spDets.Orders}
                        Dim fmDet = oLocalize.GetLocalizedValueByKey("AMSDetailMed", "Appointment #{1}{0}Warehouse: {2}{0}Date: {3} {4}-{5}{0}Resource: {6}{0}Equipment ID: {7}{0}SHID: {8}{0}Orders: {9}{0}")
                        strDetails = String.Format(fmDet, p)
                End Select
                'Get the Subject
                Select Case e
                    Case AMSMsg.WHDelete, AMSMsg.CarrierDelete
                        Dim fmSub = oLocalize.GetLocalizedValueByKey("AMSSubjectApptDeleted", "{0} - Appointment Deleted")
                        subject = String.Format(fmSub, spDets.SHID)
                    Case AMSMsg.DeleteConfirm
                        subject = oLocalize.GetLocalizedValueByKey("ConfirmDeleteAppt", "Delete Appointment Confirmation")
                End Select
                'Get the Body
                Select Case e
                    Case AMSMsg.DeleteConfirm
                        body = subject + "<br /><br />" + strDetails
                    Case AMSMsg.WHDelete
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyWarehouseDeleteAppt", "An appointment was deleted by Warehouse {0}")
                        body = String.Format(fmHdr, spDets.CompName) + "<br /><br />" + strDetails
                    Case AMSMsg.CarrierDelete
                        alertName = "AlertCarrierDeleteAppt"
                        alertDesc = "Scheduler - Carrier Deleted an appointment" 'Modified by RHR on 1/31/2020 alertDesc limited to 50 characters
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyCarrierDeleteAppt", "Carrier {0} deleted an appointment via the Self-Service Portal")
                        body = String.Format(fmHdr, spDets.CarrierName) + "<br /><br />" + strDetails
                    Case Else
                        'do nothing
                        blnSendCarNotification = False
                        blnSendAlert = False
                        blnResCanSendEmail = False
                End Select

                Select Case t
                    Case AMSMsgType.AMSRequest
                        'call method
                    Case AMSMsgType.AMSAlert
                        'The rules for sending the Subscription Alert and the Dock Notification are the same in some cases so we can call both from one place                   
                        If e = AMSMsg.CarrierDelete Then SendAMSResourceMsg(spDets.DockNotificationEmail, subject, body, blnResCanSendEmail)
                        SendAMSAlertMsg(blnSendAlert, alertName, alertDesc, subject, body, CompControl, spDets.CompNumber, spDets.CarrierControl, spDets.CarrierNumber)
                    Case AMSMsgType.AMSResourceNotify
                        SendAMSResourceMsg(spDets.DockNotificationEmail, subject, body, blnResCanSendEmail)
                    Case AMSMsgType.AMSCarrierMsg
                        SendAMSCarrierMsg(db, spDets.CarrierControl, CompControl, subject, body, blnSendCarNotification)
                    Case Else
                        'do nothing
                End Select

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SendAMSDeleteMessage"))
            End Try
        End Using
    End Sub

    Private Sub SendAMSCreateMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal ApptControl As Integer, ByVal CompControl As Integer)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                '** CompControl and ApptControl are REQUIRED **
                If CompControl = 0 OrElse ApptControl = 0 Then Return

                Dim blnSendCarNotification As Boolean = True, blnSendAlert = True, blnResCanSendEmail = False

                Dim subject As String = "", body As String = "", alertName As String = "", alertDesc As String = ""
                Dim SHID = "", Orders = "", EquipID = "", Warehouse = "", CarrierName = ""
                Dim strDt = "", strStart = "", strEnd = "", dockName = "", dockNotificationEmail = ""
                Dim CarrierControl As Integer = 0, CarrierNumber = 0, CompNumber = 0
                Dim dockNotificationAlert As Boolean = False

                'Get the info from the sp using the ApptControl
                Dim spDets = db.spAMSGetDetailsForMessaging(ApptControl).FirstOrDefault()
                If Not spDets Is Nothing Then
                    SHID = spDets.SHID
                    Orders = spDets.Orders
                    EquipID = spDets.EquipID
                    Warehouse = spDets.CompName
                    dockName = spDets.DockName
                    dockNotificationAlert = spDets.DockNotificationAlert
                    dockNotificationEmail = spDets.DockNotificationEmail
                    strDt = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortDateString(), "")
                    strStart = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortTimeString(), "")
                    strEnd = If(spDets.EndDate.HasValue, spDets.EndDate.Value.ToShortTimeString(), "")
                    CompNumber = spDets.CompNumber
                    CarrierName = spDets.CarrierName
                    CarrierNumber = spDets.CarrierNumber
                    CarrierControl = spDets.CarrierControl
                End If

                'Check to see if the dock(s) has notifications on and a valid (non null) email address
                If dockNotificationAlert AndAlso Not String.IsNullOrWhiteSpace(dockNotificationEmail) Then blnResCanSendEmail = True

                'Get the Details part of the message
                Dim strDetails As String = ""
                Select Case e
                    Case AMSMsg.BookedConfirm, AMSMsg.CarrierBooked, AMSMsg.WHBooked
                        Dim p = New String() {"<br />", ApptControl, Warehouse, strDt, strStart, strEnd, dockName, EquipID, SHID, Orders}
                        Dim fmDet = oLocalize.GetLocalizedValueByKey("AMSDetailMed", "Appointment #{1}{0}Warehouse: {2}{0}Date: {3} {4}-{5}{0}Resource: {6}{0}Equipment ID: {7}{0}SHID: {8}{0}Orders: {9}{0}")
                        strDetails = String.Format(fmDet, p)
                End Select
                'Get the Subject
                Select Case e
                    Case AMSMsg.WHBooked, AMSMsg.CarrierBooked
                        Dim fmSub = oLocalize.GetLocalizedValueByKey("AMSSubjectApptBooked", "{0} - Appointment Scheduled")
                        subject = String.Format(fmSub, SHID)
                    Case AMSMsg.BookedConfirm
                        subject = oLocalize.GetLocalizedValueByKey("ConfirmBookedAppt", "Scheduled Appointment Confirmation")
                End Select
                'Get the Body
                Select Case e
                    Case AMSMsg.BookedConfirm
                        body = subject + "<br /><br />" + strDetails
                    Case AMSMsg.WHBooked
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyWarehouseBookedAppt", "An appointment was scheduled by Warehouse {0}")
                        body = String.Format(fmHdr, Warehouse) + "<br /><br />" + strDetails
                    Case AMSMsg.CarrierBooked
                        alertName = "AlertCarrierBookedAppt"
                        alertDesc = "Scheduler - Carrier Booked an appointment" 'Modified by RHR on 1/31/2020 alertDesc limited to 50 characters
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyCarrierBookedAppt", "Carrier {0} scheduled an appointment via the Self-Service Portal")
                        body = String.Format(fmHdr, CarrierName) + "<br /><br />" + strDetails
                    Case Else
                        'do nothing
                        blnSendCarNotification = False
                        blnSendAlert = False
                        blnResCanSendEmail = False
                End Select

                Select Case t
                    Case AMSMsgType.AMSRequest
                        'call method
                    Case AMSMsgType.AMSAlert
                        If e = AMSMsg.CarrierBooked Then SendAMSResourceMsg(dockNotificationEmail, subject, body, blnResCanSendEmail) 'The rules for sending the Subscription Alert and the Dock Notification are the same in some cases so we can call both from one place  
                        SendAMSAlertMsg(blnSendAlert, alertName, alertDesc, subject, body, CompControl, CompNumber, CarrierControl, CarrierNumber)
                    Case AMSMsgType.AMSResourceNotify
                        SendAMSResourceMsg(dockNotificationEmail, subject, body, blnResCanSendEmail)
                    Case AMSMsgType.AMSCarrierMsg
                        SendAMSCarrierMsg(db, CarrierControl, CompControl, subject, body, blnSendCarNotification)
                End Select

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SendAMSCreateMessage"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Generate or Send AMS alerts and messages as required for this appointment
    ''' </summary>
    ''' <param name="t"></param>
    ''' <param name="e"></param>
    ''' <param name="ApptControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="ogDockID"></param>
    ''' <param name="ogApptStart"></param>
    ''' <param name="ogApptEnd"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019
    ''' fixed bug where query to get ltsOGDock is not valid with only the DockDoorID we added compcontrol  
    ''' </remarks>
    Private Sub SendAMSModifyMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal ApptControl As Integer, ByVal CompControl As Integer, ByVal ogDockID As String, ByVal ogApptStart As Date, ByVal ogApptEnd As Date)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)

                '** CompControl and ApptControl are REQUIRED **
                If CompControl = 0 OrElse ApptControl = 0 Then Return

                Dim blnSendCarNotification As Boolean = True, blnSendAlert = True, blnResCanSendEmail = False, blnOGResCanSendEmail = False

                Dim subject As String = "", body As String = "", alertName As String = "", alertDesc As String = ""
                Dim SHID As String = "", Orders = "", EquipID = "", Warehouse = "", CarrierName = ""
                Dim strDt = "", strStart = "", strEnd = "", dockNotificationEmail = "", dockName = ""
                Dim CarrierControl As Integer = 0, CarrierNumber = 0, CompNumber = 0
                Dim dockNotificationAlert As Boolean = False, ogDockNotificationAlert As Boolean = False
                Dim ogDockNotificationEmail As String = "", ogDockName As String = ""

                'Get the info from the sp using the ApptControl
                Dim spDets = db.spAMSGetDetailsForMessaging(ApptControl).FirstOrDefault()
                If Not spDets Is Nothing Then
                    SHID = spDets.SHID
                    Orders = spDets.Orders
                    EquipID = spDets.EquipID
                    Warehouse = spDets.CompName
                    dockName = spDets.DockName
                    dockNotificationAlert = spDets.DockNotificationAlert
                    dockNotificationEmail = spDets.DockNotificationEmail
                    strDt = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortDateString(), "")
                    strStart = If(spDets.StartDate.HasValue, spDets.StartDate.Value.ToShortTimeString(), "")
                    strEnd = If(spDets.EndDate.HasValue, spDets.EndDate.Value.ToShortTimeString(), "")
                    CompNumber = spDets.CompNumber
                    CarrierName = spDets.CarrierName
                    CarrierNumber = spDets.CarrierNumber
                    CarrierControl = spDets.CarrierControl
                End If

                'Get the original dock fields using smp.ogDockID
                'Modified by RHR for v-8.2.1 on 10/16/2019
                'fixed bug where query to get ltsOGDock is not valid with only the DockDoorID we added compcontrol                
                Dim ltsOGDock = db.CompDockDoors.Where(Function(x) x.CompDockCompControl = CompControl AndAlso x.DockDoorID = ogDockID).FirstOrDefault()
                If Not ltsOGDock Is Nothing Then
                    ogDockName = ltsOGDock.DockDoorName
                    ogDockNotificationAlert = ltsOGDock.CompDockNotificationAlert
                    ogDockNotificationEmail = ltsOGDock.CompDockNotificationEmail
                End If
                'Check to see if the dock(s) has notifications on and a valid (non null) email address
                If ogDockNotificationAlert AndAlso Not String.IsNullOrWhiteSpace(ogDockNotificationEmail) Then blnOGResCanSendEmail = True
                If dockNotificationAlert AndAlso Not String.IsNullOrWhiteSpace(dockNotificationEmail) Then blnResCanSendEmail = True
                'Format the ogDock and modDock appointment dates and times
                Dim strOGDt = ogApptStart.ToShortDateString()
                Dim strOGStart = ogApptStart.ToShortTimeString()
                Dim strOGEnd = ogApptEnd.ToShortTimeString()

                'Get the Details part of the message
                Dim strDetails As String = ""
                Select Case e
                    Case AMSMsg.ModifyConfirm, AMSMsg.CarrierModify, AMSMsg.WHModify
                        'In this case strDt, strStart, and strEnd are all the current values in the db and therefore are the "Current" times
                        Dim p = New String() {"<br />", ApptControl, Warehouse, strOGDt, strOGStart, strOGEnd, ogDockName, strDt, strStart, strEnd, dockName, EquipID, SHID, Orders} 'we can get this stuff using apptcontrol from sp
                        Dim fmDet = oLocalize.GetLocalizedValueByKey("AMSDetailMod", "Appointment #{1}{0}Warehouse: {2}{0}Original: {3} {4}-{5} {6}{0}Current: {7} {8}-{9} {10}{0}Equipment ID: {11}{0}SHID: {12}{0}Orders: {13}{0}")
                        strDetails = String.Format(fmDet, p)
                End Select
                'Get the Subject
                Select Case e
                    Case AMSMsg.WHModify, AMSMsg.CarrierModify
                        Dim fmSub = oLocalize.GetLocalizedValueByKey("AMSSubjectApptModified", "{0} - Appointment Modified")
                        subject = String.Format(fmSub, SHID)
                    Case AMSMsg.ModifyConfirm
                        subject = oLocalize.GetLocalizedValueByKey("ConfirmModifyAppt", "Modify Appointment Confirmation")
                End Select
                'Get the Body
                Select Case e
                    Case AMSMsg.ModifyConfirm
                        body = subject + "<br /><br />" + strDetails
                    Case AMSMsg.WHModify
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyWarehouseModifyAppt", "An appointment was modified by Warehouse {0}")
                        body = String.Format(fmHdr, Warehouse) + "<br /><br />" + strDetails
                    Case AMSMsg.CarrierModify
                        alertName = "AlertCarrierModifyAppt"
                        alertDesc = "Scheduler - Carrier Modified an appointment" 'Modified by RHR on 1/31/2020 alertDesc limited to 50 characters
                        Dim fmHdr = oLocalize.GetLocalizedValueByKey("AMSBdyCarrierModifyAppt", "Carrier {0} modified an appointment via the Self-Service Portal")
                        body = String.Format(fmHdr, CarrierName) + "<br /><br />" + strDetails
                    Case Else
                        'do nothing
                        blnSendCarNotification = False
                        blnSendAlert = False
                        blnResCanSendEmail = False
                        blnOGResCanSendEmail = False
                End Select

                Select Case t
                    Case AMSMsgType.AMSRequest
                        'call method
                    Case AMSMsgType.AMSAlert
                        'The rules for sending the Subscription Alert and the Dock Notification are the same in some cases so we can call both from one place                   
                        If e = AMSMsg.CarrierModify Then
                            If ogDockName.Trim() = dockName.Trim() Then 'If the dock didn't change then only send one email, else send one to the original dock and one to the current dock (if notifications are on for those docks and a valid (non null) email address was provided                          
                                SendAMSResourceMsg(ogDockNotificationEmail, subject, body, blnOGResCanSendEmail)
                            Else
                                SendAMSResourceMsg(ogDockNotificationEmail, subject, body, blnOGResCanSendEmail)
                                SendAMSResourceMsg(dockNotificationEmail, subject, body, blnResCanSendEmail)
                            End If
                        End If
                        SendAMSAlertMsg(blnSendAlert, alertName, alertDesc, subject, body, CompControl, CompNumber, CarrierControl, CarrierNumber)
                    Case AMSMsgType.AMSResourceNotify
                        SendAMSResourceMsg(dockNotificationEmail, subject, body, blnResCanSendEmail)
                    Case AMSMsgType.AMSCarrierMsg
                        SendAMSCarrierMsg(db, CarrierControl, CompControl, subject, body, blnSendCarNotification)
                    Case Else
                        'do nothing
                End Select

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SendAMSModifyMessage"))
            End Try
        End Using
    End Sub

    Private Sub SendAMSNoPackTempMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal emailTo As String, ByVal SHID As String, ByVal Orders As String, ByVal CompControl As Integer, ByVal CompNumber As Integer, ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer)
        Try
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
            Dim blnSendAlert As Boolean = True, blnResCanSendEmail As Boolean = True
            Dim subject As String = "", body As String = "", fmSub As String = "", fmBody As String = "", alertName As String = "", alertDesc As String = ""

            'Create
            Select Case e
                Case AMSMsg.OrdersNoPack
                    alertName = "AlertOrdersNoPack"
                    alertDesc = "Scheduler - Orders have no package type preferences"
                    fmSub = oLocalize.GetLocalizedValueByKey("NoPackNotification", "{0} - No Package Type Notification")
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedOrdersNoPack", "The following orders do not have any specified package type preference: {0}")
                    subject = String.Format(fmSub, SHID)
                    body = String.Format(fmBody, Orders)
                Case AMSMsg.OrdersNoTemp
                    alertName = "AlertOrdersNoTemp"
                    alertDesc = "Scheduler - Orders have no temperature preferences"
                    fmSub = oLocalize.GetLocalizedValueByKey("NoTempNotification", "{0} - No Temp Notification")
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedOrdersNoTemp", "The following orders do not have any specified temperature preference: {0}")
                    subject = String.Format(fmSub, SHID)
                    body = String.Format(fmBody, Orders)
                Case Else
                    'do nothing
                    blnSendAlert = False
                    blnResCanSendEmail = False
            End Select

            'Send
            Select Case t
                Case AMSMsgType.AMSAlert
                    SendAMSAlertMsg(blnSendAlert, alertName, alertDesc, subject, body, CompControl, CompNumber, CarrierControl, CarrierNumber)
                Case AMSMsgType.AMSResourceNotify
                    SendAMSResourceMsg(emailTo, subject, body, blnResCanSendEmail)
            End Select

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSNoPackTempMessage"))
        End Try
    End Sub

    Private Sub SendAMSNoAptAvailableMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal dockEmailTo As String, ByVal SHID As String, ByVal Orders As String, ByVal EquipID As String, ByVal DockName As String, ByVal BookDateLoad As Date, ByVal BookDateRequired As Date, ByVal dt As Date, ByVal CompControl As Integer, ByVal CompNumber As Integer, ByVal Warehouse As String, ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer, ByVal CarrierName As String)
        Try
            'TODO -- GET MORE INFO HERE SO CALLER PASS IN LESS
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
            Dim blnSendAlert As Boolean = True, blnResCanSendEmail As Boolean = True
            Dim subject As String = "", body As String = "", fmSub As String = "", fmBody As String = "", reason As String = "", alertName As String = "", alertDesc As String = ""

            'Format BookDateLoad and BookDateRequired
            Dim strLoadDt = BookDateLoad.ToString("M/d/yy HH:mm")
            Dim strReqDt = BookDateRequired.ToString("M/d/yy HH:mm")

            'Get the subject
            fmSub = oLocalize.GetLocalizedValueByKey("NoAppointmentsAvailable", "{0} - No Appointments Available")
            subject = String.Format(fmSub, SHID)

            'Get the body
            Select Case e
                Case AMSMsg.NoAppointmentAvailable
                    alertName = "AlertNoAppointmentAvailable"
                    alertDesc = "Scheduler - No appointments are available on date" 'Modified by RHR on 1/31/2020 alertDesc limited to 50 characters
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedNoApptAvail", "Appointment availability could not be determined for Carrier {1} at Warehouse {2} for the following:{0}SHID: {3}{0}Equipment ID: {4}{0}Orders: {5}{0}Load Date: {6}{0}Required Date: {7}{0}")
                    body = String.Format(fmBody, "<br />", CarrierName, Warehouse, SHID, EquipID, Orders, strLoadDt, strReqDt)
                Case AMSMsg.MaxApptLenExceeded
                    reason = oLocalize.GetLocalizedValueByKey("SchedCalcMinGRTMax", "The calculated time required for the appointment is greater than the maximum length configured for this Resource")
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedNoApptAvailDock", "No Appointments available for Carrier {1} on {2} at Warehouse {3}, Resource {4} for the following:{0}SHID: {5}{0}Equipment ID: {6}{0}Orders: {7}{0}Reason: {8}{0}")
                    body = String.Format(fmBody, "<br />", CarrierName, dt.ToString(), Warehouse, DockName, SHID, EquipID, Orders, reason)
                Case AMSMsg.NoTimeSlotAvailable
                    reason = oLocalize.GetLocalizedValueByKey("SchedNoSlotsFound", "No available time slots were found matching the criteria")
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedNoApptAvailDock", "No Appointments available for Carrier {1} on {2} at Warehouse {3}, Resource {4} for the following:{0}SHID: {5}{0}Equipment ID: {6}{0}Orders: {7}{0}Reason: {8}{0}")
                    body = String.Format(fmBody, "<br />", CarrierName, dt.ToString(), Warehouse, DockName, SHID, EquipID, Orders, reason)
                Case Else
                    'do nothing
                    blnSendAlert = False
                    blnResCanSendEmail = False
            End Select

            'Send
            Select Case t
                Case AMSMsgType.AMSAlert
                    SendAMSAlertMsg(blnSendAlert, alertName, alertDesc, subject, body, CompControl, CompNumber, CarrierControl, CarrierNumber)
                Case AMSMsgType.AMSResourceNotify
                    SendAMSResourceMsg(dockEmailTo, subject, body, blnResCanSendEmail)
            End Select

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSNoAptAvailableMessage"))
        End Try
    End Sub

    Private Sub SendAMSMissingRequestEmailMessage(ByVal t As AMSMsgType, ByVal e As AMSMsg, ByVal emailTo As String, ByVal Warehouse As String)
        Try
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
            Dim subject As String = "", body As String = "", fmSub As String = "", fmBody As String = ""
            'Create
            Select Case e
                Case AMSMsg.MissingRequestEmail
                    fmSub = oLocalize.GetLocalizedValueByKey("SchedNoReqEmailSub", "Missing Company Parameter CarrierRequestAppointmentEmail - {0}")
                    fmBody = oLocalize.GetLocalizedValueByKey("SchedNoReqEmailBody", "The company parameter CarrierRequestAppointmentEmail is missing for Company/Warehouse {0}.{1}Please populate CarrierRequestAppointmentEmail with a valid email address.{1}All Create/Delete/Modify Appointment Requests from the Carrier Self Service Portal to Warehouse {0} will be sent to that email.")
                    subject = String.Format(fmSub, Warehouse)
                    body = String.Format(fmBody, Warehouse, "<br />")
                Case Else
                    'do nothing
            End Select
            'Send
            Select Case t
                Case AMSMsgType.AMSResourceNotify
                    SendAMSResourceMsg(emailTo, subject, body, True)
            End Select
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSMissingRequestEmailMessage"))
        End Try
    End Sub

    '** GET DETAILS FOR DELETE **
    Public Function AMSGetDetailsForMessaging(ByVal ApptControl As Integer) As LTS.spAMSGetDetailsForMessagingResult
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return db.spAMSGetDetailsForMessaging(ApptControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AMSGetDetailsForMessaging"))
            End Try
        End Using
    End Function


    '** SEND ALERT/CARRIER/RESOURCE MESSAGES **
    ''' <summary>
    ''' For sending Carrier Confirmation/Notification 
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="subject"></param>
    ''' <param name="body"></param>
    ''' <param name="blnSendCarNotification"></param>
    Private Sub SendAMSCarrierMsg(ByRef db As NGLMASCompDataContext, ByVal CarrierControl As Integer, ByVal CompControl As Integer, ByVal subject As String, ByVal body As String, ByVal blnSendCarNotification As Boolean)
        Try
            Dim oMail As New NGLEmailData(Parameters)
            'Get the BookCarrierContControl from the order and lookup the email From the CarrierCont. If Carrier Contact info not set up use Global Group Email
            Dim CarEmailTo As String = "", ccEmail As String = "", subjectLocalized As String = "", bodyLocalized As String = "", subjectKeys As String = "", bodyKeys As String = ""
            Dim segSep As String = ""
            Dim emails = db.spAMSGetSchedulerCarrierEmail(CarrierControl, CompControl).ToArray()
            For Each r In emails
                CarEmailTo += (segSep + r.Email)
                segSep = "; "
            Next
            If String.IsNullOrWhiteSpace(CarEmailTo) Then
                CarEmailTo = GetParText("GlobalGroupEmail", CompControl)
                body += "<br />NOTE: Carrier Contact information has not been set up. Please add Carrier Contacts and assign one as a Scheduler Contact. A valid email address must be provided."
            End If
            body += ("<br /><br />" & Me.DBInfo) 'Added By LVV on 8/30/19 - added database info to end of email
            If blnSendCarNotification AndAlso Not String.IsNullOrWhiteSpace(CarEmailTo) Then
                oMail.GenerateEmail(CarEmailTo, ccEmail, subject, body, subjectLocalized, bodyLocalized, subjectKeys, bodyKeys)
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSCarrierMsg"))
        End Try
    End Sub

    Private Sub SendAMSResourceMsg(ByVal EmailTo As String, ByVal subject As String, ByVal body As String, ByVal blnResCanSendEmail As Boolean)
        Try
            If String.IsNullOrWhiteSpace(EmailTo) Then Return
            Dim oMail As New NGLEmailData(Parameters)
            Dim ccEmail As String = "", subjectLocalized As String = "", bodyLocalized As String = "", subjectKeys As String = "", bodyKeys As String = ""
            body += ("<br /><br />" & Me.DBInfo) 'Added By LVV on 8/30/19 - added database info to end of email
            If blnResCanSendEmail Then
                oMail.GenerateEmail(EmailTo, ccEmail, subject, body, subjectLocalized, bodyLocalized, subjectKeys, bodyKeys)
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSResourceMsg"))
        End Try
    End Sub

    Private Sub SendAMSAlertMsg(ByVal blnSendAlert As Boolean, ByVal alertName As String, ByVal alertDesc As String, ByVal subject As String, ByVal body As String, ByVal CompControl As Integer, ByVal CompNumber As Integer, ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer)
        Try
            Dim oAlert As New NGLtblAlertMessageData(Parameters)
            If blnSendAlert Then
                oAlert.InsertAlertMessage(alertName, alertDesc, subject, body, CompControl, CompNumber, CarrierControl, CarrierNumber)
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("SendAMSAlertMsg"))
        End Try
    End Sub


#End Region

#Region "Unit Test Wrapper Methods"

    'For Unit Testing
    Public Function GetScheduledSlotsTEST(ByVal dt As Date, ByVal DockControl As Integer, ByVal DockDoorID As String, ByVal DockHourStart As Date, ByVal DockHourEnd As Date) As Models.TimeSlot()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return GetScheduledSlots(db, dt, DockControl, DockDoorID, DockHourStart, DockHourEnd)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetScheduledSlotsTEST"), db)
            End Try
            Return Nothing
        End Using
    End Function

    'For Unit Testing
    Public Function GetAvailableOpeningsTEST(ByVal NeededApptMins As Integer, ByVal minLen As Integer, ByVal scheduledSlots As Models.TimeSlot()) As List(Of Models.TimeSlot)
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return GetAvailableOpenings(NeededApptMins, minLen, scheduledSlots)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAvailableOpeningsTEST"), db)
            End Try
            Return Nothing
        End Using
    End Function

#End Region


#End Region

#End Region

End Class

'Added By LVV on 9/5/18 for v-8.3 TMS365 Scheduler
Public Class NGLLECompCarData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblLegalEntityCompCarriers
        Me.LinqDB = db
        Me.SourceClass = "NGLLECompCarData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            Me.LinqTable = db.tblLegalEntityCompCarriers
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetLECompCar(ByVal LECompCarControl As Integer) As LTS.vLECompCar
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Return db.vLECompCars.Where(Function(x) x.LECompCarControl = LECompCarControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompCar"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLECompCarsFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLECompCar()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLECompCar
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vLECompCar)
                iQuery = db.vLECompCars
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompCarsFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Private Sub InsertOrUpdateLECompCar(ByRef db As NGLMASCompDataContext, ByVal oRecord As LTS.tblLegalEntityCompCarrier)
        If db Is Nothing OrElse db.Connection Is Nothing Then InsertOrUpdateLECompCar(oRecord)
        Try
            oRecord.LECompCarModDate = Date.Now
            oRecord.LECompCarModUser = Parameters.UserName
            If oRecord.LECompCarControl = 0 Then
                'Insert
                db.tblLegalEntityCompCarriers.InsertOnSubmit(oRecord)
            Else
                'Update
                db.tblLegalEntityCompCarriers.Attach(oRecord, True)
            End If
            db.SubmitChanges()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLECompCar"), db)
        End Try
    End Sub

    Public Sub InsertOrUpdateLECompCar(ByVal oRecord As LTS.tblLegalEntityCompCarrier)
        Using db As New NGLMASCompDataContext(ConnectionString)
            InsertOrUpdateLECompCar(db, oRecord)
        End Using
    End Sub

    Public Function DeleteLECompCar(ByVal LECompCarControl As Integer) As Boolean
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.tblLegalEntityCompCarrier = db.tblLegalEntityCompCarriers.Where(Function(x) x.LECompCarControl = LECompCarControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LECompCarControl = 0) Then Return False

                db.tblLegalEntityCompCarriers.DeleteOnSubmit(oRecord)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLECompCar"), db)
            End Try
        End Using
        Return False
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class

Public Class NGLLECompAccessorialData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASCompDataContext(ConnectionString)
        Me.LinqTable = db.tblLECompAccessorials
        Me.LinqDB = db
        Me.SourceClass = "NGLLECompAccessorialData"
    End Sub

#End Region

#Region " Properties "


    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASCompDataContext(ConnectionString)
            _LinqTable = db.tblLECompAccessorials
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetLECompAccessorials(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLECompAccessorial()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLECompAccessorial
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vLECompAccessorial)
                iQuery = db.vLECompAccessorials
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLECompAccessorials"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function SaveLECompAccessorial(ByVal oData As LTS.tblLECompAccessorial) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iLECompAccCompControl = oData.LECompAccessorialCompControl
        Dim iLECompAccLEAControl = oData.LECompAccessorialLEAControl
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'verify that a Comp record exists
                If iLECompAccCompControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Company Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.Comps.Any(Function(x) x.CompControl = iLECompAccCompControl) Then
                    Dim lDetails As New List(Of String) From {"Company Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'verify that a LegalEntityAdmin record exists
                If iLECompAccLEAControl = 0 Then
                    Dim lDetails As New List(Of String) From {"LEAdmin Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.tblLegalEntityAdmins.Any(Function(x) x.LEAdminControl = iLECompAccLEAControl) Then
                    Dim lDetails As New List(Of String) From {"LEAdmin Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.LECompAccessorialModUser = Parameters.UserName
                oData.LECompAccessorialModDate = Date.Now
                If oData.LECompAccessorialControl = 0 Then
                    db.tblLECompAccessorials.InsertOnSubmit(oData)
                Else
                    db.tblLECompAccessorials.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveLECompAccessorial"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLECompAccessorial(ByVal iLECompAccessorialControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iLECompAccessorialControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblLECompAccessorials.Where(Function(x) x.LECompAccessorialControl = iLECompAccessorialControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.LECompAccessorialControl = 0 Then Return True
                db.tblLECompAccessorials.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLECompAccessorial"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function CopyLECompAccessorialConfig(ByVal LEAdminControl As Integer, ByVal CopyFromCompControl As Integer, ByVal CopyToCompControls() As Integer) As String
        Dim strRet As String = ""
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'verify that a CopyToComp record was provided
                If CopyToCompControls?.Length < 1 Then
                    Dim lDetails As New List(Of String) From {"Copy To Company Record References", " were not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'verify that a LegalEntityAdmin record exists
                If LEAdminControl = 0 OrElse Not db.tblLegalEntityAdmins.Any(Function(x) x.LEAdminControl = LEAdminControl) Then
                    Dim lDetails As New List(Of String) From {"LEAdmin Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'verify that a CopyFromComp record exists
                If CopyFromCompControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Copy From Company Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                If Not db.Comps.Any(Function(x) x.CompControl = CopyFromCompControl) Then
                    Dim lDetails As New List(Of String) From {"Copy From Company Record Reference", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return strRet
                End If
                'Copy the configs
                For Each ct In CopyToCompControls
                    Dim spRet = db.spCopyLECompAccessorialConfig(LEAdminControl, CopyFromCompControl, ct).FirstOrDefault()
                    Dim sSep As String = ""
                    If spRet.ErrNumber > 0 AndAlso Not String.IsNullOrWhiteSpace(spRet.RetMsg) Then
                        strRet += (sSep + spRet.RetMsg)
                        sSep = " "
                    End If
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CopyLECompAccessorialConfig"), db)
            End Try
        End Using
        Return strRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class