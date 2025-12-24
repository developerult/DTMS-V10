Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports System.Linq.Dynamic
Imports System.Configuration
Imports System.ComponentModel
Imports Serilog

''' <summary>
''' LTS Class for tblLoadTenderLog data
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5.3.001 on 05/27/2022
''' </remarks>
Public Class NGLLoadTenderLogData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        'Me.LinqTable = db.tblLoadTenderLogs
        'Me.LinqDB = db
        Me.SourceClass = "NGLoadTenderLogData"
        Me.Logger = Me.Logger.ForContext(Of NGLLoadTenderLogData)

    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASIntegrationDataContext(ConnectionString)
                _LinqTable = db.tblLoadTenderLogs
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _Logs As New List(Of LTS.tblLoadTenderLog)
    Public Property Logs() As List(Of LTS.tblLoadTenderLog)
        Get
            If _Logs Is Nothing Then _Logs = New List(Of LTS.tblLoadTenderLog)
            Return _Logs
        End Get
        Set(ByVal Value As List(Of LTS.tblLoadTenderLog))
            If Value Is Nothing Then Value = New List(Of LTS.tblLoadTenderLog)
            _Logs = Value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function GetRecord(control As Integer) As LTS.tblLoadTenderLog
        Dim oRet As New LTS.tblLoadTenderLog
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblLoadTenderLogs.Where(Function(x) x.LTLogControl = control).FirstOrDefault()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"))
            End Try

            Return oRet

        End Using
    End Function
    Public Function GetAll(ByVal iLoadTenderControl As Integer) As List(Of LTS.tblLoadTenderLog)
        Dim oRet As New List(Of LTS.tblLoadTenderLog)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblLoadTenderLogs.Where(Function(x) x.LTLogLoadTenderControl = iLoadTenderControl).ToList()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetAll"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetPage(ByVal iLoadTenderControl As Integer, ByVal skip As Integer, ByVal take As Integer, ByRef RecordCount As Integer) As List(Of LTS.tblLoadTenderLog)
        Dim oRet As New List(Of LTS.tblLoadTenderLog)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                RecordCount = db.tblLoadTenderLogs.Where(Function(x) x.LTLogLoadTenderControl = iLoadTenderControl).Count()


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
                oRet = db.tblLoadTenderLogs.Where(Function(x) x.LTLogLoadTenderControl = iLoadTenderControl).Skip(skip).Take(pagesize).ToList()


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetPage"))
            End Try

            Return oRet

        End Using

    End Function

    Public Function Save(oRecord As LTS.tblLoadTenderLog) As LTS.tblLoadTenderLog

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oTable = db.tblLoadTenderLogs
                oRecord.LTLogModDate = Date.Now()
                oRecord.LTLogModUser = Me.Parameters.UserName
                oTable.Attach(oRecord, True)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Save"))
            End Try

            Return oRecord

        End Using
    End Function
    Public Function InsertOrUpdate(ByVal oRecord As LTS.tblLoadTenderLog) As LTS.tblLoadTenderLog
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                If oRecord.LTLogLoadTenderControl = 0 OrElse Not db.tblLoadTenders.Any(Function(x) x.LoadTenderControl = oRecord.LTLogLoadTenderControl) Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Load Tender Log", "Record"})
                End If

                oRecord.LTLogModDate = Date.Now()
                oRecord.LTLogModUser = Me.Parameters.UserName
                If oRecord.LTLogControl = 0 Then
                    'Insert
                    db.tblLoadTenderLogs.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.tblLoadTenderLogs.Attach(oRecord, True)
                End If
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdate"), db)
            End Try

            Return oRecord

        End Using
    End Function
    Public Function Delete(ByVal oRecord As LTS.tblLoadTenderLog) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim oTable = db.tblLoadTenderLogs
            Try
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"), db)
            End Try
        End Using
        Return blnRet
    End Function
    Public Function Delete(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim oTable = db.tblLoadTenderLogs
            Try
                Dim oRecord As LTS.tblLoadTenderLog = db.tblLoadTenderLogs.Where(Function(x) x.LTLogControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LTLogControl = 0) Then Return False
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Delete"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all Bid Cost Adj using BidCostAdjLTLogControl as the key filter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/20/2018 to support standard content management processing
    ''' </remarks>
    Public Function GetLoadTenderLogs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As List(Of LTS.tblLoadTenderLog)
        If filters Is Nothing Then Return Nothing

        Dim oRet As New List(Of LTS.tblLoadTenderLog)
        Dim iLTLogLoadTenderControl As Integer = filters.ParentControl
        Dim sLTLogLoadTenderControl As String = ""
        Dim iLTLogControl As Integer = filters.PrimaryKey
        Dim sLTLogControl As String = ""
        If Not filters.FilterValues Is Nothing AndAlso filters.FilterValues.Count > 0 Then
            If filters.FilterValues.Any(Function(x) x.filterName = "LTLogLoadTenderControl") Then
                sLTLogLoadTenderControl = filters.FilterValues.Where(Function(x) x.filterName = "LTLogLoadTenderControl").Select(Function(x) x.filterValueFrom).FirstOrDefault()
            End If
            If filters.FilterValues.Any(Function(x) x.filterName = "LTLogControl") Then
                sLTLogControl = filters.FilterValues.Where(Function(x) x.filterName = "LTLogControl").Select(Function(x) x.filterValueFrom).FirstOrDefault()
            End If
        End If
        'Rules for default filters
        '(a)Parent FK is always required unless the Primary key is
        '(b) parent key can be provided as filters.ParentControl or as one of the filterValueFrom lists for matching field name in table or view
        '(c) Primary key can be provided as filters.PrimaryKey or as one of the filterValueFrom lists for matching field name in table or view
        '(e) if filters.PrimaryKey is provided and filterName does not exist in FilterValues for primary key, iQuery must include a filter for filters.PrimaryKey
        '(f) if filters.ParentControl is provided and filterName does not exist in FilterValues for primary key, iQuery must include a filter for filters.ParentControl
        '(g) if string filters are provided for filters.PrimaryKey or filters.ParentControl and these strings do not parse to integer the data is not valid
        '(h) if a primary key is provided all other filters and sorting are ignored and one record is returned in the list

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblLoadTenderLog)


                Dim blnUsingParentKey As Boolean = False
                Dim iParseValue As Integer = 0
                If iLTLogControl = 0 And Not String.IsNullOrWhiteSpace(sLTLogControl) Then Integer.TryParse(sLTLogControl, iLTLogControl)
                If iLTLogControl = 0 Then
                    'validate parent key
                    If Not String.IsNullOrWhiteSpace(sLTLogLoadTenderControl) Then
                        If Integer.TryParse(sLTLogLoadTenderControl, iParseValue) Then
                            blnUsingParentKey = True ' we use the filter provided 
                        End If
                    End If
                    If Not blnUsingParentKey Then
                        If iLTLogLoadTenderControl <> 0 Then
                            blnUsingParentKey = True ' we need to add a filter to iQuery
                            iQuery = db.tblLoadTenderLogs.Where(Function(x) x.LTLogLoadTenderControl = iLTLogLoadTenderControl)
                        End If
                    End If
                Else
                    oRet = db.tblLoadTenderLogs.Where(Function(x) x.LTLogControl = iLTLogControl).ToList()
                    Return oRet
                End If
                If Not blnUsingParentKey Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Load Tender Log", "Record"})
                End If

                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToList()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadTenderLogs"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub AddToCollection(ByVal oRecord As LTS.tblLoadTenderLog)
        Logs.Add(oRecord)
    End Sub

    Public Sub AddToCollection(ByVal LTLogMessage As String, ByVal LTLogReason As String, ByVal LTLogDetails As String, ByVal LTLogModUser As String)
        Dim LTLogModDate As DateTime = Date.Now
        AddToCollection(New LTS.tblLoadTenderLog() With {.LTLogMessage = Left(LTLogMessage, 500), .LTLogReason = Left(LTLogReason, 50), .LTLogDetails = Left(LTLogDetails, 4000), .LTLogModDate = LTLogModDate, .LTLogModUser = LTLogModUser})
    End Sub

    Public Sub AddToCollection(ByVal LTLogMessage As String, ByVal LTLogReason As String, ByVal LTLogDetails As String)
        Dim LTLogModDate As DateTime = Date.Now
        AddToCollection(New LTS.tblLoadTenderLog() With {.LTLogMessage = Left(LTLogMessage, 500), .LTLogReason = Left(LTLogReason, 50), .LTLogDetails = Left(LTLogDetails, 4000), .LTLogModDate = LTLogModDate})
    End Sub

    Public Sub AddToCollection(ByVal LTLogMessage As String, ByVal LTLogDetails As String)
        Dim LTLogModDate As DateTime = Date.Now
        AddToCollection(New LTS.tblLoadTenderLog() With {.LTLogMessage = Left(LTLogMessage, 500), .LTLogDetails = Left(LTLogDetails, 4000), .LTLogModDate = LTLogModDate})
    End Sub

    Public Sub AddToCollection(ByVal LTLogMessage As String)
        Dim LTLogModDate As DateTime = Date.Now
        AddToCollection(New LTS.tblLoadTenderLog() With {.LTLogMessage = Left(LTLogMessage, 500), .LTLogModDate = LTLogModDate})
    End Sub

    Public Sub SaveCollectionToDB(ByVal iLoadTenderControl As Integer, ByVal sModUser As String)
        Try
            If Logs.Count() > 0 Then
                For Each olog As LTS.tblLoadTenderLog In Me.Logs.OrderBy(Function(x) x.LTLogModDate).ToList()
                    olog.LTLogLoadTenderControl = iLoadTenderControl
                    If String.IsNullOrWhiteSpace(olog.LTLogModUser) Then olog.LTLogModUser = sModUser
                    If olog.LTLogModDate Is Nothing Then olog.LTLogModDate = Date.Now()
                    Me.InsertOrUpdate(olog)
                Next
            End If

        Catch ex As Exception
            'just log any errors when writing to the log table.
            Utilities.SaveAppError("Save load tender log failure:" & ex.Message, Me.Parameters)
        End Try

    End Sub

#End Region

#Region "Protected Functions"

#End Region

End Class


