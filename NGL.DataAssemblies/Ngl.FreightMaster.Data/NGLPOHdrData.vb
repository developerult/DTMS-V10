Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Linq.Dynamic

Public Class NGLPOHdrData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        ' processParameters(oParameters)
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.POHdrs
        Me.LinqDB = db
        Me.SourceClass = "NGLPOHdrData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = db.POHdrs
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
        Return GetPOHdrFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetPOHdrs()
    End Function

    Public Function GetPOHdrFiltered(Optional ByVal Control As Long = 0) As DTO.POHdr
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim POHdr As DTO.POHdr = (
                From d In db.POHdrs
                Where
                    (d.POHdrControl = If(Control = 0, d.POHdrControl, Control))
                Select selectDTOData(d, db)).First

                Return POHdr

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

    Public Function GetPOHdrFiltered(ByVal PONumber As String) As DTO.POHdr
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim POHdr As DTO.POHdr = (
                From d In db.POHdrs
                Where
                    (d.POHDRnumber = PONumber)
                Select selectDTOData(d, db)).First

                Return POHdr

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
    Public Function GetPOHdrFiltered(ByVal ordernumber As String, ByVal sequenece As String, ByVal CompNumber As Integer) As DTO.POHdr
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim POHdr As DTO.POHdr = (
                From d In db.POHdrs
                Where
                    (d.POHDROrderNumber = ordernumber) _
                 And d.POHDROrderSequence = sequenece _
                 And d.POHDRDefaultCustomer = CompNumber
                Select selectDTOData(d, db)).FirstOrDefault

                Return POHdr

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

    Public Function GetPOHdrs(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.POHdr()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(dbo.POHdr.POHdrControl) from dbo.POHdr")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the contacts that match the criteria sorted by name
                Dim POHdrs() As DTO.POHdr = (
                From d In db.POHdrs
                Order By d.POHDRCreateDate
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return POHdrs

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

    Public Function GetPOHdrsFiltered(ByVal StartDate As Date,
                                      ByVal EndDate As Date,
                                      Optional ByVal CompNumber As Integer = 0,
                                      Optional ByVal FrtTyp As Byte = 0,
                                      Optional ByVal CreateUser As String = "",
                                      Optional ByVal NatAcctNbr As Integer = 0,
                                      Optional ByVal page As Integer = 1,
                                      Optional ByVal pagesize As Integer = 1000) As DTO.POHdr()

        Dim intRecordCount As Integer = 0
        Dim intPageCount As Integer = 1
        If pagesize < 1 Then pagesize = 1
        If intRecordCount < 1 Then intRecordCount = 1
        If page < 1 Then page = 1
        Dim intSkip As Integer = (page - 1) * pagesize
        'Dim oCompNumbers As New List(Of Integer)
        'Check if we need to filter by NatAcctNbr
        StartDate = DTran.formatStartDateFilter(StartDate)
        EndDate = DTran.formatEndDateFilter(EndDate)
        If NatAcctNbr > 0 Then
            Using db As New NGLMASIntegrationDataContext(ConnectionString)

                'db.Log = New DebugTextWriter
                Try

                    Dim oNatComps = From s In db.CompRefIntegrations Where s.CompNatNumber = NatAcctNbr Select Number = s.CompNumber

                    'Count the records
                    Dim pos = From d In db.POHdrs
                              Where
                                  oNatComps.Contains(d.POHDRDefaultCustomer) _
                                  And
                                  (d.POHDRCreateDate >= StartDate And d.POHDRCreateDate < EndDate) _
                                  And
                                  (FrtTyp = 0 OrElse d.POHDRFrt = FrtTyp) _
                               And
                                  (d.POHDRCreateUser.Contains(CreateUser))
                              Select Control = d.POHdrControl
                    intRecordCount = pos.Count
                    If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                    'oCompNumbers.Contains(d.POHDRDefaultCustomer) _
                    'Dim oDLO As New DataLoadOptions
                    'oDLO.LoadWith(Of LTS.POHdr)(Function(t As LTS.POHdr) t.POItems)
                    'db.LoadOptions = oDLO
                    'Return all the contacts that match the criteria sorted by name
                    Dim POHdrs() As DTO.POHdr = (
                    From d In db.POHdrs
                    Where
                        oNatComps.Contains(d.POHDRDefaultCustomer) _
                        And
                        (d.POHDRCreateDate >= StartDate And d.POHDRCreateDate < EndDate) _
                        And
                        (FrtTyp = 0 OrElse d.POHDRFrt = FrtTyp) _
                        And
                        (d.POHDRCreateUser.Contains(CreateUser))
                    Order By d.POHDRCreateDate
                    Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                    Return POHdrs

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
        Else
            Using db As New NGLMASIntegrationDataContext(ConnectionString)

                'db.Log = New DebugTextWriter
                Try

                    'Count the records
                    Dim pos = From d In db.POHdrs
                              Where
                               (d.POHDRCreateDate >= StartDate And d.POHDRCreateDate < EndDate) _
                              And
                              If(CompNumber > 0, d.POHDRDefaultCustomer = CompNumber, True) _
                              And
                              If(FrtTyp = 0, True, d.POHDRFrt = FrtTyp) _
                              And
                              (d.POHDRCreateUser.Contains(CreateUser))
                              Select Control = d.POHdrControl
                    intRecordCount = pos.Count
                    If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                    Dim POHdrs() As DTO.POHdr = (
                    From d In db.POHdrs
                    Where
                     (d.POHDRCreateDate >= StartDate And d.POHDRCreateDate < EndDate) _
                    And
                    If(CompNumber > 0, d.POHDRDefaultCustomer = CompNumber, True) _
                    And
                    If(FrtTyp = 0, True, d.POHDRFrt = FrtTyp) _
                    And
                    (d.POHDRCreateUser.Contains(CreateUser))
                    Order By d.POHDRCreateDate
                    Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                    Return POHdrs

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

        End If

    End Function

    Public Function CountPOHdrsFiltered(Optional ByVal CompNumber As Integer = 0, Optional ByVal NatAcctNbr As Integer = 0) As Integer

        Dim strSQL As String = ""
        If NatAcctNbr > 0 Then
            strSQL = "select COUNT(dbo.POHdr.POHdrControl) from dbo.POHdr Where POHdr.POHDRDefaultCustomer in (Select distinct compnumber from dbo.Comp where CompNatNumber = " & NatAcctNbr & ")"

        ElseIf CompNumber > 0 Then
            strSQL = "select COUNT(dbo.POHdr.POHdrControl) from dbo.POHdr Where POHdr.POHDRDefaultCustomer = " & CompNumber & " "

        Else
            Return 0
        End If
        Try
            Return getScalarInteger(strSQL)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Function

    Public Function GetAllPOHdrsFilteredByCompOrNatAcct(Optional ByVal CompNumber As Integer = 0,
                                                        Optional ByVal NatAcctNbr As Integer = 0,
                                                        Optional ByVal page As Integer = 1,
                                                        Optional ByVal pagesize As Integer = 1000) As DTO.POHdr()
        'Check that a company number or national account number is provided.
        If CompNumber = 0 And NatAcctNbr = 0 Then
            Utilities.SaveAppError("Cannot get all POHdr records; the selected company and the selected national account number cannot both be blank or zero", Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidFilterSelection"}, New FaultReason("E_DataValidationFailure"))
        End If

        Dim oCompNumbers As New List(Of Integer)
        Dim intRecordCount As Integer = 0
        Dim intPageCount As Integer = 1
        If pagesize < 1 Then pagesize = 1
        If intRecordCount < 1 Then intRecordCount = 1
        If page < 1 Then page = 1
        Dim intSkip As Integer = (page - 1) * pagesize

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oNatComps = From s In db.CompRefIntegrations Where s.CompNatNumber = NatAcctNbr Select Number = s.CompNumber

                'Count the records
                Dim pos = From d In db.POHdrs
                          Where
                          (CompNumber = 0 OrElse d.POHDRDefaultCustomer = CompNumber) _
                          And
                          (NatAcctNbr = 0 OrElse oNatComps.Contains(d.POHDRDefaultCustomer))
                          Select Control = d.POHdrControl
                intRecordCount = pos.Count
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                Dim POHdrs() As DTO.POHdr = (
                From d In db.POHdrs
                Where
                    (CompNumber = 0 OrElse d.POHDRDefaultCustomer = CompNumber) _
                    And
                    (NatAcctNbr = 0 OrElse oNatComps.Contains(d.POHDRDefaultCustomer))
                Order By d.POHDRCreateDate
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return POHdrs

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
    ''' Returns Count number of pohdr records filterd by company and ModVerify like "No Pro"
    ''' </summary>
    ''' <param name="ModVerify"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="Count"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPOHDRsByModVerify(ByVal ModVerify As String, ByVal CompNumber As Integer, Optional ByVal Count As Integer = 1) As List(Of DTO.POHdr)
        Dim lRet As New List(Of DTO.POHdr)
        Try
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                lRet = (From d In db.POHdrs
                        Where d.POHDRDefaultCustomer = CompNumber And d.POHDRModVerify = ModVerify
                        Order By d.POHdrControl
                        Select selectDTOData(d, db)).Take(Count).ToList()
            End Using
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetPOHDRsByModVerify"))
        End Try
        Return lRet
    End Function

    ''' <summary>
    ''' used to add a record to the POHDRHistoryTable then import it into the POHDR table
    ''' simulating the process when orders are imported via web services.  Auto Tender and
    ''' Silent Tender will not be activated by this procedure however the created order
    ''' may be processed by the normal import procedures/web services.
    ''' </summary>
    ''' <param name="CompanyNumber"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="PONumber"></param>
    ''' <param name="ModeTypeControl"></param>
    ''' <param name="TempType"></param>
    ''' <param name="ShipInstructions"></param>
    ''' <param name="LoadDate"></param>
    ''' <param name="OrderDate"></param>
    ''' <param name="ReqDate"></param>
    ''' <param name="Wgt"></param>
    ''' <param name="Cube"></param>
    ''' <param name="Qty"></param>
    ''' <param name="Pallets"></param>
    ''' <remarks></remarks>
    Public Sub CreatePOData(ByVal CompanyNumber As String,
                            ByVal LaneNumber As String,
                            ByVal OrderNumber As String,
                            ByVal PONumber As String,
                            ByVal ModeTypeControl As Integer,
                            ByVal TempType As String,
                            ByVal ShipInstructions As String,
                            ByVal LoadDate As Date,
                            ByVal OrderDate As Date,
                            ByVal ReqDate As Date,
                            ByVal Wgt As Double,
                            ByVal Cube As Integer,
                            ByVal Qty As Integer,
                            ByVal Pallets As Integer)
        Try
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Dim oRet = db.spCreatePOData(Left(CompanyNumber, 50),
                                             Left(LaneNumber, 50),
                                             Left(OrderNumber, 20),
                                             Left(PONumber, 20),
                                             ModeTypeControl,
                                             Left(TempType, 1),
                                             Left(ShipInstructions, 255),
                                             LoadDate,
                                             OrderDate,
                                             ReqDate,
                                             Wgt,
                                             Cube,
                                             Qty,
                                             Pallets,
                                             Left(Parameters.UserName, 100)).FirstOrDefault()
                If Not oRet Is Nothing AndAlso oRet.ErrNbr <> 0 Then
                    throwSQLFaultException(oRet.Msg)
                End If

            End Using

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("CreatePOData"))
        End Try
    End Sub

    ''' <summary>
    ''' Returns an object with validation results and other key data 
    ''' The primary result is  stoed in the boolean property AllowSilentForOrder 
    ''' errors are stored in RetMsg and ErrNumber.  ErrNumber 1 indicates 
    ''' a problem reading the data and has a RetMsg of "Data validation failure: Order no longer available in POHDR table!"
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="POHDROrderNumber"></param>
    ''' <returns>
    ''' Expected Non Error RetMsg  ErrNumber is zero
    ''' "Success!"
    ''' "Silent Tendering is Off!"
    ''' "Silent Tendering of PC Loads is Off!"
    ''' "Silent Tendering of EDI PC Loads is Off!"
    ''' </returns>
    ''' <remarks>
    ''' Created by RHR 2/9/2016 v-7.0.5.0
    '''   Used to query the database and test if the associated POHDR Record 
    '''   can be auto imported and processed by the Silent Tender procedure
    '''   The spPreSilentTenderValidationResult object holds key information 
    '''   and data fields needed by the integration silent tender process
    '''   Stored Procdure Errors are encapsulated inside the result object
    '''   but data access and other sql errors follow the standard NGL SQLFaultInfo
    '''   Exception management design patterns
    ''' </remarks>
    Public Function PreSilentTenderValidation(ByVal CompControl As Integer, ByVal POHDROrderNumber As String) As LTS.spPreSilentTenderValidationResult
        Dim oRet As New LTS.spPreSilentTenderValidationResult()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.spPreSilentTenderValidation(CompControl, POHDROrderNumber).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("PreSilentTenderValidation"))
            End Try
        End Using

        Return oRet
    End Function

#Region "TMS 365"

    Public Function GetPOHdrsFiltered365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters, ByVal CompNumber As Integer, ByVal NatAcctNbr As Integer, ByVal FrtTyp As Integer) As LTS.vPOHdr()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vPOHdr
        'Check that a company number or national account number is provided.
        If CompNumber = 0 And NatAcctNbr = 0 Then
            Return Nothing 'NOTE: Commented out this error because it was really annoying
            Utilities.SaveAppError("Cannot get all POHdr records; the selected company and the selected national account number cannot both be blank or zero", Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidFilterSelection"}, New FaultReason("E_DataValidationFailure"))
        End If
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oNatComps = From s In db.CompRefIntegrations Where s.CompNatNumber = NatAcctNbr Select Number = s.CompNumber
                Dim iQuery As IQueryable(Of LTS.vPOHdr)
                iQuery = (From d In db.vPOHdrs
                          Where
                              (CompNumber = 0 OrElse d.POHDRDefaultCustomer = CompNumber) _
                              And
                              (NatAcctNbr = 0 OrElse oNatComps.Contains(d.POHDRDefaultCustomer)) _
                              And
                              (FrtTyp = 0 OrElse d.POHDRFrt = FrtTyp)
                          Order By d.POHDRCreateDate
                          Select d)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPOHdrsFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetPOHdrFiltered365(ByVal sPOHDRControl As String) As LTS.vPOHdr

        Dim oRet As New LTS.vPOHdr
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.vPOHdrs.Where(Function(x) x.POHdrControl = sPOHDRControl).FirstOrDefault()

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPOHdrFiltered365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function ToggleHoldStatus(ByVal POHdrControl As Long) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim blnHold As Boolean
                Dim oRecord As LTS.POHdr = db.POHdrs.Where(Function(x) x.POHdrControl = POHdrControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.POHdrControl = 0) Then Return False
                If oRecord.POHDRHoldLoad = True Then blnHold = False Else blnHold = True

                oRecord.POHDRModUser = Parameters.UserName
                oRecord.POHDRHoldLoad = blnHold
                'db.POHdrs.Attach(oRecord, True)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ToggleHoldStatus"), db)
            End Try
        End Using
        Return False
    End Function

    Public Function DeletePOHdr(ByVal iPOHdrControl As Long) As Boolean
        Dim blnRet As Boolean = False
        If iPOHdrControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oRecord = db.POHdrs.Where(Function(x) x.POHdrControl = iPOHdrControl).FirstOrDefault()
                If oRecord Is Nothing OrElse oRecord.POHdrControl = 0 Then Return True 'already deleted
                db.POHdrs.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletePOHdr"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.POHdr)
        'Create New Record
        Return New LTS.POHdr With {.POHdrControl = d.POHdrControl _
                                  , .POHDRnumber = d.POHDRnumber _
                                  , .POHDRvendor = d.POHDRvendor _
                                  , .POHDRPOdate = d.POHDRPOdate _
                                  , .POHDRShipdate = d.POHDRShipdate _
                                  , .POHDRBuyer = d.POHDRBuyer _
                                  , .POHDRFrt = d.POHDRFrt _
                                  , .POHDRCreateUser = d.POHDRCreateUser _
                                  , .POHDRCreateDate = d.POHDRCreateDate _
                                  , .POHDRModUser = Parameters.UserName _
                                  , .POHDRTotalFrt = d.POHDRTotalFrt _
                                  , .POHDRTotalCost = d.POHDRTotalCost _
                                  , .POHDRWgt = d.POHDRWgt _
                                  , .POHDRCube = d.POHDRCube _
                                  , .POHDRQty = d.POHDRQty _
                                  , .POHDRLines = d.POHDRLines _
                                  , .POHDRConfirm = d.POHDRConfirm _
                                  , .POHDRDefaultCustomer = d.POHDRDefaultCustomer _
                                  , .POHDRDefaultCustomerName = d.POHDRDefaultCustomerName _
                                  , .POHDRDefaultCarrier = d.POHDRDefaultCarrier _
                                  , .POHDRReqDate = d.POHDRReqDate _
                                  , .POHDROrderNumber = d.POHDROrderNumber _
                                  , .POHDRShipInstructions = d.POHDRShipInstructions _
                                  , .POHDRCooler = d.POHDRCooler _
                                  , .POHDRFrozen = d.POHDRFrozen _
                                  , .POHDRDry = d.POHDRDry _
                                  , .POHDRTemp = d.POHDRTemp _
                                  , .POHDRModVerify = d.POHDRModVerify _
                                  , .POHDROrigName = d.POHDROrigName _
                                  , .POHDROrigCity = d.POHDROrigCity _
                                  , .POHDROrigState = d.POHDROrigState _
                                  , .POHDROrigZip = d.POHDROrigZip _
                                  , .POHDRDestName = d.POHDRDestName _
                                  , .POHDRDestCity = d.POHDRDestCity _
                                  , .POHDRDestState = d.POHDRDestState _
                                  , .POHDRDestZip = d.POHDRDestZip _
                                  , .POHDRCarType = d.POHDRCarType _
                                  , .POHDRShipVia = d.POHDRShipVia _
                                  , .POHDRShipViaType = d.POHDRShipViaType _
                                  , .POHDRPallets = d.POHDRPallets _
                                  , .POHDROtherCost = d.POHDROtherCost _
                                  , .POHDRStatusFlag = d.POHDRStatusFlag _
                                  , .POHDRSortOrder = d.POHDRSortOrder _
                                  , .POHDRPRONumber = d.POHDRPRONumber _
                                  , .POHDRHoldLoad = d.POHDRHoldLoad _
                                  , .POHDROrderSequence = d.POHDROrderSequence _
                                  , .POHDRChepGLID = d.POHDRChepGLID _
                                  , .POHDRCarrierEquipmentCodes = d.POHDRCarrierEquipmentCodes _
                                  , .POHDRCarrierTypeCode = d.POHDRCarrierTypeCode _
                                  , .POHDRPalletPositions = d.POHDRPalletPositions _
                                  , .POHDRSchedulePUDate = d.POHDRSchedulePUDate _
                                  , .POHDRSchedulePUTime = d.POHDRSchedulePUTime _
                                  , .POHDRScheduleDelDate = d.POHDRScheduleDelDate _
                                  , .POHDRScheduleDelTime = d.POHDRScheduleDelTime _
                                  , .POHDRActPUDate = d.POHDRActPUDate _
                                  , .POHDRActPUTime = d.POHDRActPUTime _
                                  , .POHDRActDelDate = d.POHDRActDelDate _
                                  , .POHDRActDelTime = d.POHDRActDelTime _
                                , .POHDROrigCompNumber = d.POHdrOrigCompNumber _
                                , .POHDROrigAddress1 = d.POHDROrigAddress1 _
                                , .POHDROrigAddress2 = d.POHDROrigAddress2 _
                                , .POHDROrigAddress3 = d.POHDROrigAddress3 _
                                , .POHDROrigCountry = d.POHDROrigCountry _
                                , .POHDROrigContactPhone = d.POHDROrigContactPhone _
                                , .POHDROrigContactPhoneExt = d.POHDROrigContactPhoneExt _
                                , .POHDROrigContactFax = d.POHDROrigContactFax _
                                , .POHDRDestCompNumber = d.POHDRDestCompNumber _
                                , .POHDRDestAddress1 = d.POHDRDestAddress1 _
                                , .POHDRDestAddress2 = d.POHDRDestAddress2 _
                                , .POHDRDestAddress3 = d.POHDRDestAddress3 _
                                , .POHDRDestCountry = d.POHDRDestCountry _
                                , .POHDRDestContactPhone = d.POHDRDestContactPhone _
                                , .POHDRDestContactPhoneExt = d.POHDRDestContactPhoneExt _
                                , .POHDRDestContactFax = d.POHDRDestContactFax _
                                , .POHDRPalletExchange = d.POHDRPalletExchange _
                                , .POHDRPalletType = d.POHDRPalletType _
                                , .POHDRComments = d.POHDRComments _
                                , .POHDRCommentsConfidential = d.POHDRCommentsConfidential _
                                , .POHDRInbound = d.POHDRInbound _
                                , .POHDRDefaultRouteSequence = d.POHDRDefaultRouteSequence _
                                , .POHDRRouteGuideNumber = d.POHDRRouteGuideNumber _
                                   , .POHDRCompLegalEntity = d.POHDRCompLegalEntity _
                                   , .POHDRCompAlphaCode = d.POHDRCompAlphaCode _
                                    , .POHDRModeTypeControl = d.POHDRModeTypeControl _
                                    , .POHDRMustLeaveByDateTime = d.POHDRMustLeaveByDateTime _
                                   , .POHDRUser1 = d.POHDRUser1 _
                                   , .POHDRUser2 = d.POHDRUser2 _
                                   , .POHDRUser3 = d.POHDRUser3 _
                                   , .POHDRUser4 = d.POHDRUser4 _
                                , .POHdrUpdated = If(d.POHdrUpdated Is Nothing, New Byte() {}, d.POHdrUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetPOHdrFiltered(Control:=CType(LinqTable, LTS.POHdr).POHdrControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.POHdr = TryCast(LinqTable, LTS.POHdr)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.POHdrs
                       Where d.POHdrControl = source.POHdrControl
                       Select New DTO.QuickSaveResults With {.Control = d.POHdrControl _
                                                            , .ModDate = d.POHDRCreateDate _
                                                            , .ModUser = d.POHDRModUser _
                                                            , .Updated = d.POHdrUpdated.ToArray}).First

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

    'Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)

    '    With CType(LinqTable, LTS.POHdr)
    '        'Add POHdr contact Records
    '        .POItems.AddRange( _
    '                 From i In CType(oData, DTO.POHdr).POItems _
    '                 Select New LTS.POItem With {.POItemControl = i.POItemControl _
    '                                          , .ItemPONumber = i.ItemPONumber _
    '                                          , .FixOffInvAllow = i.FixOffInvAllow _
    '                                          , .FixFrtAllow = i.FixFrtAllow _
    '                                          , .ItemNumber = i.ItemNumber _
    '                                          , .QtyOrdered = i.QtyOrdered _
    '                                          , .FreightCost = i.FreightCost _
    '                                          , .ItemCost = i.ItemCost _
    '                                          , .Weight = i.Weight _
    '                                          , .Cube = i.Cube _
    '                                          , .Pack = i.Pack _
    '                                          , .Size = i.Size _
    '                                          , .Description = i.Description _
    '                                          , .Hazmat = i.Hazmat _
    '                                          , .CreatedUser = i.CreatedUser _
    '                                          , .CreatedDate = i.CreatedDate _
    '                                          , .Brand = i.Brand _
    '                                          , .CostCenter = i.CostCenter _
    '                                          , .LotNumber = i.LotNumber _
    '                                          , .LotExpirationDate = i.LotExpirationDate _
    '                                          , .GTIN = i.GTIN _
    '                                          , .CustItemNumber = i.CustItemNumber _
    '                                          , .CustomerNumber = i.CustomerNumber _
    '                                          , .POOrderSequence = i.POOrderSequence _
    '                                          , .PalletType = i.PalletType _
    '                                          , .POItemUpdated = If(i.POItemUpdated Is Nothing, New Byte() {}, i.POItemUpdated)})

    '    End With
    'End Sub

    'Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
    '    With CType(oDB, NGLMASIntegrationDataContext)
    '        .POItems.InsertAllOnSubmit(CType(LinqTable, LTS.POHdr).POItems)
    '    End With
    'End Sub

    'Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
    '    With CType(oDB, NGLMASIntegrationDataContext)
    '        ' Process any inserted contact records 
    '        .POItems.InsertAllOnSubmit(GetPOitemChanges(oData, TrackingInfo.Created))
    '        ' Process any updated contact records
    '        .POItems.AttachAll(GetPOitemChanges(oData, TrackingInfo.Updated), True)
    '        ' Process any deleted contact records
    '        Dim deletedPOItems = GetPOitemChanges(oData, TrackingInfo.Deleted)
    '        .POItems.AttachAll(deletedPOItems, True)
    '        .POItems.DeleteAllOnSubmit(deletedPOItems)
    '    End With
    'End Sub

    'Protected Function GetPOitemChanges(ByVal source As DTO.POHdr, ByVal changeType As TrackingInfo) As List(Of LTS.POItem)

    '    Dim details As IEnumerable(Of LTS.POItem) = ( _
    '      From i In source.POItems _
    '      Where i.TrackingState = changeType _
    '      Select New LTS.POItem With {.POItemControl = i.POItemControl _
    '                                      , .ItemPONumber = i.ItemPONumber _
    '                                      , .FixOffInvAllow = i.FixOffInvAllow _
    '                                      , .FixFrtAllow = i.FixFrtAllow _
    '                                      , .ItemNumber = i.ItemNumber _
    '                                      , .QtyOrdered = i.QtyOrdered _
    '                                      , .FreightCost = i.FreightCost _
    '                                      , .ItemCost = i.ItemCost _
    '                                      , .Weight = i.Weight _
    '                                      , .Cube = i.Cube _
    '                                      , .Pack = i.Pack _
    '                                      , .Size = i.Size _
    '                                      , .Description = i.Description _
    '                                      , .Hazmat = i.Hazmat _
    '                                      , .CreatedUser = i.CreatedUser _
    '                                      , .CreatedDate = i.CreatedDate _
    '                                      , .Brand = i.Brand _
    '                                      , .CostCenter = i.CostCenter _
    '                                      , .LotNumber = i.LotNumber _
    '                                      , .LotExpirationDate = i.LotExpirationDate _
    '                                      , .GTIN = i.GTIN _
    '                                      , .CustItemNumber = i.CustItemNumber _
    '                                      , .CustomerNumber = i.CustomerNumber _
    '                                      , .POOrderSequence = i.POOrderSequence _
    '                                      , .PalletType = i.PalletType _
    '                                      , .POItemUpdated = If(i.POItemUpdated Is Nothing, New Byte() {}, i.POItemUpdated)})
    '    Return details.ToList()
    'End Function

    'modified by RHR for testing typically protected

    Public Sub runRemoveDeletedWithData(ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer)
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
        oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
        oCmd.Parameters.AddWithValue("@POHDRDefaultCustomer", intDefCompNumber)
        runNGLStoredProcedure(oCmd, "dbo.spRemoveDeletedPOByComp", 0)
    End Sub

    Public Sub runDeleteOrderWithData(ByVal strBookProNumber As String,
                                                    ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer)
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookProNumber", strBookProNumber)
        runNGLStoredProcedure(oCmd, "dbo.spDeleteBookingByPro", 3)
        runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber)
    End Sub

    Public Sub validateNewCompPro(ByVal strBookProNumber As String,
                                     ByVal intCompNumber As Integer)
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@BookProNumber", strBookProNumber)
        oCmd.Parameters.AddWithValue("@NewCompNumber", intCompNumber)
        runNGLStoredProcedure(oCmd, "dbo.spValidateNewCompPro", 3)
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.POHdr, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.POHdr
        Return New DTO.POHdr With {.POHdrControl = d.POHdrControl _
                                    , .POHDRnumber = d.POHDRnumber _
                                    , .POHDRvendor = d.POHDRvendor _
                                    , .POHDRPOdate = d.POHDRPOdate _
                                    , .POHDRShipdate = d.POHDRShipdate _
                                    , .POHDRBuyer = d.POHDRBuyer _
                                    , .POHDRFrt = d.POHDRFrt _
                                    , .POHDRCreateUser = d.POHDRCreateUser _
                                    , .POHDRCreateDate = d.POHDRCreateDate _
                                    , .POHDRModUser = d.POHDRModUser _
                                    , .POHDRTotalFrt = d.POHDRTotalFrt _
                                    , .POHDRTotalCost = d.POHDRTotalCost _
                                    , .POHDRWgt = d.POHDRWgt _
                                    , .POHDRCube = d.POHDRCube _
                                    , .POHDRQty = d.POHDRQty _
                                    , .POHDRLines = d.POHDRLines _
                                    , .POHDRConfirm = d.POHDRConfirm _
                                    , .POHDRDefaultCustomer = d.POHDRDefaultCustomer _
                                    , .POHDRDefaultCustomerName = d.POHDRDefaultCustomerName _
                                    , .POHDRDefaultCarrier = d.POHDRDefaultCarrier _
                                    , .POHDRReqDate = d.POHDRReqDate _
                                    , .POHDROrderNumber = d.POHDROrderNumber _
                                    , .POHDRShipInstructions = d.POHDRShipInstructions _
                                    , .POHDRCooler = d.POHDRCooler _
                                    , .POHDRFrozen = d.POHDRFrozen _
                                    , .POHDRDry = d.POHDRDry _
                                    , .POHDRTemp = d.POHDRTemp _
                                    , .POHDRModVerify = d.POHDRModVerify _
                                    , .POHDROrigName = d.POHDROrigName _
                                    , .POHDROrigCity = d.POHDROrigCity _
                                    , .POHDROrigState = d.POHDROrigState _
                                    , .POHDROrigZip = d.POHDROrigZip _
                                    , .POHDRDestName = d.POHDRDestName _
                                    , .POHDRDestCity = d.POHDRDestCity _
                                    , .POHDRDestState = d.POHDRDestState _
                                    , .POHDRDestZip = d.POHDRDestZip _
                                    , .POHDRCarType = d.POHDRCarType _
                                    , .POHDRShipVia = d.POHDRShipVia _
                                    , .POHDRShipViaType = d.POHDRShipViaType _
                                    , .POHDRPallets = d.POHDRPallets _
                                    , .POHDROtherCost = d.POHDROtherCost _
                                    , .POHDRStatusFlag = d.POHDRStatusFlag _
                                    , .POHDRSortOrder = d.POHDRSortOrder _
                                    , .POHDRPRONumber = d.POHDRPRONumber _
                                    , .POHDRHoldLoad = d.POHDRHoldLoad _
                                    , .POHDROrderSequence = d.POHDROrderSequence _
                                    , .POHDRChepGLID = d.POHDRChepGLID _
                                    , .POHDRCarrierEquipmentCodes = d.POHDRCarrierEquipmentCodes _
                                    , .POHDRCarrierTypeCode = d.POHDRCarrierTypeCode _
                                    , .POHDRPalletPositions = d.POHDRPalletPositions _
                                    , .POHDRSchedulePUDate = d.POHDRSchedulePUDate _
                                    , .POHDRSchedulePUTime = d.POHDRSchedulePUTime _
                                    , .POHDRScheduleDelDate = d.POHDRScheduleDelDate _
                                    , .POHDRScheduleDelTime = d.POHDRScheduleDelTime _
                                    , .POHDRActPUDate = d.POHDRActPUDate _
                                    , .POHDRActPUTime = d.POHDRActPUTime _
                                    , .POHDRActDelDate = d.POHDRActDelDate _
                                    , .POHDRActDelTime = d.POHDRActDelTime _
                                    , .POHdrOrigCompNumber = d.POHDROrigCompNumber _
                                    , .POHDROrigAddress1 = d.POHDROrigAddress1 _
                                    , .POHDROrigAddress2 = d.POHDROrigAddress2 _
                                    , .POHDROrigAddress3 = d.POHDROrigAddress3 _
                                    , .POHDROrigCountry = d.POHDROrigCountry _
                                    , .POHDROrigContactPhone = d.POHDROrigContactPhone _
                                    , .POHDROrigContactPhoneExt = d.POHDROrigContactPhoneExt _
                                    , .POHDROrigContactFax = d.POHDROrigContactFax _
                                    , .POHDRDestCompNumber = d.POHDRDestCompNumber _
                                    , .POHDRDestAddress1 = d.POHDRDestAddress1 _
                                    , .POHDRDestAddress2 = d.POHDRDestAddress2 _
                                    , .POHDRDestAddress3 = d.POHDRDestAddress3 _
                                    , .POHDRDestCountry = d.POHDRDestCountry _
                                    , .POHDRDestContactPhone = d.POHDRDestContactPhone _
                                    , .POHDRDestContactPhoneExt = d.POHDRDestContactPhoneExt _
                                    , .POHDRDestContactFax = d.POHDRDestContactFax _
                                    , .POHDRPalletExchange = d.POHDRPalletExchange _
                                    , .POHDRPalletType = d.POHDRPalletType _
                                    , .POHDRComments = d.POHDRComments _
                                    , .POHDRCommentsConfidential = d.POHDRCommentsConfidential _
                                    , .POHDRInbound = d.POHDRInbound _
                                    , .POHDRDefaultRouteSequence = d.POHDRDefaultRouteSequence _
                                    , .POHDRRouteGuideNumber = d.POHDRRouteGuideNumber _
                                   , .POHDRCompLegalEntity = d.POHDRCompLegalEntity _
                                   , .POHDRCompAlphaCode = d.POHDRCompAlphaCode _
                                    , .POHDRModeTypeControl = d.POHDRModeTypeControl _
                                    , .POHDRMustLeaveByDateTime = d.POHDRMustLeaveByDateTime _
                                   , .POHDRUser1 = d.POHDRUser1 _
                                   , .POHDRUser2 = d.POHDRUser2 _
                                   , .POHDRUser3 = d.POHDRUser3 _
                                   , .POHDRUser4 = d.POHDRUser4,
                                    .Page = page,
                                    .Pages = pagecount,
                                    .RecordCount = recordcount,
                                    .PageSize = pagesize _
                                    , .POHdrUpdated = d.POHdrUpdated.ToArray()}
    End Function


#End Region

End Class


