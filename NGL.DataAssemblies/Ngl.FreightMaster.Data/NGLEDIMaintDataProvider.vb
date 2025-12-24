Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.Linq.Dynamic
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation

''' <summary>
''' NGLDocumentTypeData for Document Type Linq to SQL Interface
''' </summary>
''' <remarks>
''' Created by SN for v-8.1 on 1/24/18
''' </remarks>
Public Class NGLEDIDocumentTypeData : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDITypes
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIDocumentTypeData"
    End Sub

#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDITypes
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
    ''' Get tblEDIType single record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIType</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocument(ByVal Control As Integer) As LTS.tblEDIType()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIType()
        Dim iQuery As IQueryable(Of LTS.tblEDIType) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDITypes", "Invalid EDIDocType, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDITypes.Any(Function(x) x.EDITControl = Control) Then
                    Dim ediEDITypes As LTS.tblEDIType = db.tblEDITypes.Where(Function(x) x.EDITControl = Control).FirstOrDefault()
                    iQuery = ediEDITypes
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocument"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIDocumentType Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIType</returns>
    ''' <remarks>
    ''' Created by SN for v-8.1 on 2/14/18
    ''' </remarks>
    Public Function GetEDIDocuments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIType()
        Dim oRet As LTS.tblEDIType()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.tblEDIType) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDITypes

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDITypes.ToArray()
                End If
                'oRet = db.tblEDIDocmentTypes.ToArray()


                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocuments"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDocumentType Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDocumentType</param>
    ''' <returns>tblEDIType</returns>
    ''' <remarks>
    ''' Created by SN for v-8.1 on 2/14/18
    ''' </remarks>
    Public Function UpdateEDIDocumentType(ByVal oData As LTS.tblEDIType) As LTS.tblEDIType
        Dim iControl As Integer = oData.EDITControl
        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIType", "Invalid EDI DocumentType, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.EDITModDate = Date.Now
                oData.EDITModUser = Parameters.UserName

                db.tblEDITypes.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDocumentType"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDocumentType Data
    ''' </summary>
    ''' <param name="oData">tblEDIDocumentType</param>
    ''' <returns>tblEDIType</returns>
    ''' <remarks>
    ''' Created by SN for v-8.1 on 2/14/18
    ''' </remarks>
    Public Function InsertEDIDocumentType(ByVal oData As LTS.tblEDIType) As LTS.tblEDIType
        Dim iControl As Integer = oData.EDITControl

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.EDITCreateUser = Parameters.UserName
                oData.EDITModUser = Parameters.UserName
                oData.EDITCreateDate = Date.Now
                oData.EDITModDate = Date.Now
                db.tblEDITypes.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDocumentType"), db)
            End Try
        End Using
        Return oData
    End Function


    Public Function DeleteEDIDocumentType(ByVal oData As LTS.tblEDIType) As Boolean

        Dim blnRet As Boolean = False

        Dim iQuery As IQueryable(Of LTS.tblEDIType) = Nothing
        If oData.EDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITypes", "Invalid tblEDIType, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDITypes.Any(Function(x) x.EDITControl = oData.EDITControl) Then
                    Dim oEDIDocType As LTS.tblEDIType = db.tblEDITypes.Where(Function(x) x.EDITControl = oData.EDITControl).FirstOrDefault()
                    Dim oEDIDocSegmentElement As LTS.tblEDIMasterDocument = db.tblEDIMasterDocuments.Where(Function(x) x.MasterDocEDITControl = oData.EDITControl).FirstOrDefault()
                    If oEDIDocSegmentElement Is Nothing Then
                        db.tblEDITypes.DeleteOnSubmit(oEDIDocType)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = True 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEEDIDocumentType"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
#Region "Protected Methods"


#End Region
End Class

Public Class NGLEDISegment : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDISegments
        Me.LinqDB = db
        Me.SourceClass = "NGLEDISegment"
    End Sub

#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDISegments
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
    ''' Get tblEDISegment record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDISegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 1/26/18
    ''' </remarks>(ByVal oData As LTS.tblEDISegment) As LTS.tblEDISegment
    Public Function GetEDISegment(ByVal Control As Integer) As LTS.tblEDISegment()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDISegment()
        Dim iQuery As IQueryable(Of LTS.tblEDISegment) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDISegments", "Invalid segment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDISegments.Any(Function(x) x.SegmentControl = Control) Then
                    Dim edielements As LTS.tblEDISegment = db.tblEDISegments.Where(Function(x) x.SegmentControl = Control).FirstOrDefault()
                    iQuery = edielements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDISegment"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDISegment Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDISegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function GetEDISegments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDISegment()
        Dim oRet As LTS.tblEDISegment()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDISegment) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDISegments

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDISegments.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDISegments"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDISegment Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIElement</param>
    ''' <returns>tblEDISegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function UpdateEDISegment(ByVal oData As LTS.tblEDISegment) As LTS.tblEDISegment
        Dim iControl As Integer = oData.SegmentControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDISegments", "Invalid EDI Segment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.SegmentModUser = Parameters.UserName
                oData.SegmentModDate = Date.Now
                db.tblEDISegments.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDISegment"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDISegments Data
    ''' </summary>
    ''' <param name="oData">tblEDISegments</param>
    ''' <returns>tblEDISegments</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function InsertEDISegment(ByVal oData As LTS.tblEDISegment) As LTS.tblEDISegment
        Dim iControl As Integer = oData.SegmentControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.SegmentCreateUser = Parameters.UserName
                oData.SegmentModUser = Parameters.UserName
                oData.SegmentCreateDate = Date.Now
                oData.SegmentModDate = Date.Now
                db.tblEDISegments.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDISegment"), db)
            End Try
        End Using
        Return oData
    End Function


    Public Function DeleteEDISegment(ByVal oData As LTS.tblEDISegment) As Boolean

        Dim blnRet As Boolean = False

        Dim iQuery As IQueryable(Of LTS.tblEDISegment) = Nothing
        If oData.SegmentControl = 0 Then
            throwInvalidRequiredKeysException("tblEDISegments", "Invalid tblEDISegment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDISegments.Any(Function(x) x.SegmentControl = oData.SegmentControl) Then
                    Dim oEDISegment As LTS.tblEDISegment = db.tblEDISegments.Where(Function(x) x.SegmentControl = oData.SegmentControl).FirstOrDefault()
                    Dim oEDIMasterDocStructSegment As LTS.tblEDIMasterDocStructSegment = db.tblEDIMasterDocStructSegments.Where(Function(x) x.MDSSegSegmentControl = oData.SegmentControl).FirstOrDefault()
                    If oEDIMasterDocStructSegment Is Nothing AndAlso oEDIMasterDocStructSegment Is Nothing Then
                        Dim oEDIDocStructSegment As LTS.tblEDIDocStructSegment = db.tblEDIDocStructSegments.Where(Function(x) x.DSSegSegmentControl = oData.SegmentControl).FirstOrDefault()
                        If oEDIDocStructSegment Is Nothing AndAlso oEDIDocStructSegment Is Nothing Then
                            db.tblEDISegments.DeleteOnSubmit(oEDISegment)
                            db.SubmitChanges()
                            blnRet = True
                        Else
                            blnRet = True 'return true if the record does not exist (already deleted)
                        End If
                    End If
                    Else
                    blnRet = True 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDISegment"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
#Region "Protected Methods"

#End Region
End Class

Public Class NGLEDILoop : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDILoops
        Me.LinqDB = db
        Me.SourceClass = "NGLEDILoop"
    End Sub

#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDILoops
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
    ''' Get tblEDILoop Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDILoop</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function GetEDILoops(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDILoop()
        Dim oRet As LTS.tblEDILoop()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDILoop) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDILoops

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDILoops.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDILoops"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDILoop Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDILoop</param>
    ''' <returns>tblEDILoop</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function UpdateEDILoop(ByVal oData As LTS.tblEDILoop) As LTS.tblEDILoop
        Dim iControl As Integer = oData.LoopControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDILoops", "Invalid EDI Loop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.LoopModDate = Date.Now
                oData.LoopModUser = Parameters.UserName
                db.tblEDILoops.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDILoop"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDILoops Data
    ''' </summary>
    ''' <param name="oData">tblEDILoops</param>
    ''' <returns>tblEDILoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/26/18
    ''' </remarks>
    Public Function InsertEDILoop(ByVal oData As LTS.tblEDILoop) As LTS.tblEDILoop
        Dim iControl As Integer = oData.LoopControl

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.LoopCreateUser = Parameters.UserName
                oData.LoopModUser = Parameters.UserName
                oData.LoopCreateDate = Date.Now
                oData.LoopModDate = Date.Now
                db.tblEDILoops.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDILoop"), db)
            End Try
        End Using
        Return oData
    End Function

    Public Function DeleteEDILoop(ByVal oData As LTS.tblEDILoop) As Boolean

        Dim blnRet As Boolean = False
        Dim iQuery As IQueryable(Of LTS.tblEDILoop) = Nothing
        If oData.LoopControl = 0 Then
            throwInvalidRequiredKeysException("tblEDILoops", "Invalid tblEDILoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDILoops.Any(Function(x) x.LoopControl = oData.LoopControl) Then
                    Dim oEDILoop As LTS.tblEDILoop = db.tblEDILoops.Where(Function(x) x.LoopControl = oData.LoopControl).FirstOrDefault()
                    Dim oEDIMDocStructLoops As LTS.tblEDIMasterDocStructLoops = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopLoopControl = oData.LoopControl).FirstOrDefault()
                    If oEDIMDocStructLoops Is Nothing Then
                        Dim oEDIDocStructLoops As LTS.tblEDIDocStructLoop = db.tblEDIDocStructLoops.Where(Function(x) x.DSLoopLoopControl = oData.LoopControl).FirstOrDefault()
                        If oEDIMDocStructLoops Is Nothing Then
                            db.tblEDILoops.DeleteOnSubmit(oEDILoop)
                            db.SubmitChanges()
                            blnRet = True
                        Else
                            blnRet = False 'return true if the record does not exist (already deleted)
                        End If
                    End If
                    Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEEDIDocumentType"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
End Class

Public Class NGLEDIDocSegmentElement : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDocSegmentElements
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIDocSegmentElement"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDocSegmentElements
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
    ''' Get tblEDIDocSegmentElement Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDocSegmentElement</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function GetEDIDocSegmentElements(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDocSegmentElement()
        Dim oRet As LTS.tblEDIDocSegmentElement()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocSegmentElement) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDocSegmentElements

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDocSegmentElements.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocSegmentElements"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetEDIDocSegElementsbySeg(ByVal segment As Integer) As LTS.tblEDIDocSegmentElement()
        Dim oRet As LTS.tblEDIDocSegmentElement()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocSegmentElement) = Nothing
                Dim filterWhere = ""

                If db.tblEDIDocSegmentElements.Any(Function(x) x.DSESegmentControl = segment) Then
                    Dim edielements = db.tblEDIDocSegmentElements.Where(Function(x) x.DSESegmentControl = segment)
                    iQuery = edielements
                    oRet = iQuery.ToArray()
                    Return oRet
                End If
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocSegElementsbySeg"), db)
            End Try
            Return Nothing
        End Using
    End Function

#End Region
End Class

Public Class NGLEDIDataMapField : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDataMapFields
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIDataMapField"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDataMapFields
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
    ''' Get tblEDIDataMapField record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDataMapField</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDataMapField(ByVal Control As Integer) As LTS.tblEDIDataMapField()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDataMapField()
        Dim iQuery As IQueryable(Of LTS.tblEDIDataMapField) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIDataMapFields", "Invalid EDIDataMapField, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDataMapFields.Any(Function(x) x.DataMapFieldControl = Control) Then
                    Dim ediDataMapFields As LTS.tblEDIDataMapField = db.tblEDIDataMapFields.Where(Function(x) x.DataMapFieldControl = Control).FirstOrDefault()
                    iQuery = ediDataMapFields
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDataMapField"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDIDataMapField Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDataMapField</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function GetEDIDataMapFields(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDataMapField()
        Dim oRet As LTS.tblEDIDataMapField()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDataMapField) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDataMapFields

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDataMapFields.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDataMapFields"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDataMapField Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDataMapField</param>
    ''' <returns>GettblEDIDataMapField</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function UpdateEDIDataMapField(ByVal oData As LTS.tblEDIDataMapField) As LTS.tblEDIDataMapField
        Dim iControl As Integer = oData.DataMapFieldControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDataMapFields", "Invalid EDI Data MapField, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DataMapFieldModDate = Date.Now
                oData.DataMapFieldModUser = Parameters.UserName
                db.tblEDIDataMapFields.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDataMapField"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDataMapField Data
    ''' </summary>
    ''' <param name="oData">tblEDIDataMapField</param>
    ''' <returns>tblEDIDataMapField</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function InsertEDIDataMapField(ByVal oData As LTS.tblEDIDataMapField) As LTS.tblEDIDataMapField
        Dim iControl As Integer = oData.DataMapFieldControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DataMapFieldCreateUser = Parameters.UserName
                oData.DataMapFieldModUser = Parameters.UserName
                oData.DataMapFieldCreateDate = Date.Now
                oData.DataMapFieldModDate = Date.Now
                db.tblEDIDataMapFields.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDataMapField"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Get tblEDIDataMapField record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDataMapField</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>


    Public Function GetEDIDataMapFieldByTableId(ByVal Control As String) As LTS.tblEDIDataMapField()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDataMapField()
        Dim iQuery As IQueryable(Of LTS.tblEDIDataMapField) = Nothing
        If Control = "" Then
            throwInvalidRequiredKeysException("tblEDIDataMapFields", "Invalid EDIDataMapField, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDataMapFields.Any(Function(x) x.DataMapFieldTable = Control) Then
                    Dim ediDataMapFields = db.tblEDIDataMapFields.Where(Function(x) x.DataMapFieldTable = Control)
                    iQuery = ediDataMapFields
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDataMapField"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get Table Records
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetTableRecords() As LTS.vw_tblEDIDataMapField()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)


            Try
                Dim compcarrierlist = db.vw_tblEDIDataMapFields()
                Return compcarrierlist.ToArray()



            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTableRecords"), db)
            End Try
        End Using
        Return Nothing
    End Function
#End Region
End Class


Public Class NGLEDIMasterDocument : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIMasterDocuments
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIMasterDocument"
    End Sub



#End Region


#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIMasterDocuments
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region
#Region "Public Methods"
    ''Added By SRP on 2/12/18 AnimalAddExample
    Public Function GetMDocLoopPreview(ByVal MasterDocControl As Integer) As LTS.sp_PreviewEDIMasterDocumentConfigResult()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.sp_PreviewEDIMasterDocumentConfig(MasterDocControl).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocLoopPreview"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Get tblEDIDataMapField record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDataMapField</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocument(ByVal Control As Integer) As LTS.tblEDIMasterDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocument) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocuments", "Invalid EDIMasterDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocControl = Control) Then
                    Dim ediMasterDocuments = db.tblEDIMasterDocuments.Where(Function(x) x.MasterDocControl = Control)
                    iQuery = ediMasterDocuments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocument"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIMasterDocuments Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIMasterDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function GetEDIMasterDocuments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIMasterDocument()
        Dim oRet As LTS.tblEDIMasterDocument()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocument) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIMasterDocuments

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIMasterDocuments.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocuments"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIMasterDocuments Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocuments</param>
    ''' <returns>GettblEDIMasterDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function UpdateEDIMasterDocument(ByVal oData As LTS.tblEDIMasterDocument) As LTS.tblEDIMasterDocument
        Dim iControl As Integer = oData.MasterDocControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocument", "Invalid EDI MasterDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MasterDocModDate = Date.Now
                oData.MasterDocModUser = Parameters.UserName
                db.tblEDIMasterDocuments.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIMasterDocument"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Get tblEDITPDocument record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMsDocumentByEDITControl(ByVal MasterDocEDITControl As Integer, ByVal MasterDocInbound As Boolean) As LTS.tblEDIMasterDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocument) = Nothing
        If MasterDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocuments", "Invalid EDIMasterDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocEDITControl = MasterDocEDITControl And x.MasterDocInbound = MasterDocInbound) Then
                    Dim ediEDITPDocuments = db.tblEDIMasterDocuments.Where(Function(x) x.MasterDocEDITControl = MasterDocEDITControl And x.MasterDocInbound = MasterDocInbound)
                    iQuery = ediEDITPDocuments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMsDocumentByEDITControl"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Insert tblEDIMasterDocuments Data
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocuments</param>
    ''' <returns>tblEDIMasterDocuments</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/27/18
    ''' </remarks>
    Public Function InsertEDIMasterDocument(ByVal oData As LTS.tblEDIMasterDocument) As LTS.tblEDIMasterDocument
        Dim iControl As Integer = oData.MasterDocControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If Not db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocEDITControl = oData.MasterDocEDITControl AndAlso x.MasterDocInbound = oData.MasterDocInbound) Then
                    'If (oData.MasterDocEDITControl == LTS.tblEDIMasterDocument) Then
                    oData.MasterDocCreateUser = Parameters.UserName
                    oData.MasterDocModUser = Parameters.UserName
                    oData.MasterDocCreateDate = Date.Now
                    oData.MasterDocModDate = Date.Now
                    db.tblEDIMasterDocuments.InsertOnSubmit(oData)
                    db.SubmitChanges()
                Else
                    'throwInvalidRequiredKeysException("tblEDIMasterDocuments", "Duplicate Master EDI document")
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIMasterDocument"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Deletes an EDI Master document from the database
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterDocument(ByVal oData As LTS.tblEDIMasterDocument) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIType()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocument) = Nothing
        If oData.MasterDocControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITypes", "Invalid Master EDI document, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocControl = oData.MasterDocControl) Then
                    Dim oEDIMasterDocument As LTS.tblEDIMasterDocument = db.tblEDIMasterDocuments.Where(Function(x) x.MasterDocControl = oData.MasterDocControl And x.MasterDocPublished = False).FirstOrDefault()
                    Dim oEDIMasterDocStructLoops As LTS.tblEDIMasterDocStructLoops = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopMasterDocControl = oData.MasterDocControl).FirstOrDefault()
                    If oEDIMasterDocStructLoops Is Nothing Then
                        db.tblEDIMasterDocuments.DeleteOnSubmit(oEDIMasterDocument)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = True 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocument"), db)
            End Try
        End Using
        Return blnRet
    End Function
    ''' <summary>
    ''' Deletes an EDI Master document from the database
    ''' </summary>
    ''' <param name="MDocControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterDocumentFull(ByVal MDocControl As Integer) As Integer

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.SpDeleteMasterDoc(MDocControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocumentFull"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetEDIMasterDoc() As LTS.vw_GetMasterDocument()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim EDIMAsterlist = db.vw_GetMasterDocuments()
                Return EDIMAsterlist.ToArray()
                
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDoc"), db)
            End Try
        End Using
        Return Nothing
    End Function
#End Region
End Class


Public Class NGLEDIMDocStructLoop : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIMasterDocStructLoops
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIMDocStructLoop"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIMasterDocStructLoops
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
    ''' Get tblEDIMasterDocStructLoop record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructLoop</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructLoop(ByVal Control As Integer) As LTS.tblEDIMasterDocStructLoops()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructLoops", "Invalid EDIMasterDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructLoops.Any(Function(x) x.MDSLoopControl = Control) Then
                    Dim edielements As LTS.tblEDIMasterDocStructLoops = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopControl = Control).FirstOrDefault()
                    iQuery = edielements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructLoop"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDIMasterDocStructLoop record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructLoop</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructLoopbyParent(ByVal Control As Integer) As LTS.tblEDIMasterDocStructLoops()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructLoops", "Invalid EDIMasterDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructLoops.Any(Function(x) x.MDSLoopParentLoopID = Control) Then
                    Dim edielements = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopParentLoopID = Control)
                    iQuery = edielements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructLoop"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Get EDIMasterDocStructLoops Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIMasterDocStructLoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function GetMDocStructLoops(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIMasterDocStructLoops()
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=" + filters.filterValue + ")"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIMasterDocStructLoops

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIMasterDocStructLoops.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocStructLoops"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIMasterDocStructLoops Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocStructLoops</param>
    ''' <returns>tblEDIMasterDocStructLoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function UpdateMDocStructLoop(ByVal oData As LTS.tblEDIMasterDocStructLoops) As LTS.tblEDIMasterDocStructLoops
        Dim iControl As Integer = oData.MDSLoopControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructLoop", "Invalid MDoc StructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSLoopModDate = Date.Now
                oData.MDSLoopModUser = Parameters.UserName
                db.tblEDIMasterDocStructLoops.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMDocStructLoop"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIMasterDocStructLoops Data
    ''' </summary>
    ''' <param name="oData">EDIMasterDocStructLoops</param>
    ''' <returns>tblEDIMasterDocStructLoops</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function InsertMDocStructLoop(ByVal oData As LTS.tblEDIMasterDocStructLoops) As LTS.tblEDIMasterDocStructLoops
        Dim iControl As Integer = oData.MDSLoopControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSLoopCreateUser = Parameters.UserName
                oData.MDSLoopModUser = Parameters.UserName
                oData.MDSLoopCreateDate = Date.Now
                oData.MDSLoopModDate = Date.Now
                db.tblEDIMasterDocStructLoops.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertMDocStructLoop"), db)
            End Try
        End Using
        Return oData
    End Function

    Public Function DeleteEDIMasterDocStructLoops(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructLoops", "Invalid tblEDIMasterDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructLoops.Any(Function(x) x.MDSLoopControl = oData) Then
                    Dim oEDIDocStructLoops = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopControl = oData).FirstOrDefault()
                    If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocControl = oEDIDocStructLoops.MDSLoopMasterDocControl And x.MasterDocPublished = False) Then
                        'Deleting Elements related to Struct Segment
                        If db.tblEDIMasterDocStructSegments.Any(Function(x) x.MDSSegMDSLoopControl = oEDIDocStructLoops.MDSLoopControl) Then
                            Dim oEDIDocStructSegments = db.tblEDIMasterDocStructSegments.Where(Function(x) x.MDSSegMDSLoopControl = oEDIDocStructLoops.MDSLoopControl)
                            'Deleting Elements related to Struct Element
                            For Each item As LTS.tblEDIMasterDocStructSegment In oEDIDocStructSegments
                                If db.tblEDIMasterDocStructElements.Any(Function(x) x.MDSElementMDSSegControl = item.MDSSegSegmentControl) Then
                                    Dim oEDIDocStructElements = db.tblEDIMasterDocStructElements.Where(Function(x) x.MDSElementMDSSegControl = item.MDSSegSegmentControl)
                                    'Deleting Elements related to Struct Element Attributes
                                    For Each itemelement As LTS.tblEDIMasterDocStructElement In oEDIDocStructElements
                                        If db.tblEDIMasterDocStructElmntAttributes.Any(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl) Then
                                            Dim oEDIMasterDocStructElmntAttributes = db.tblEDIMasterDocStructElmntAttributes.Where(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl)
                                            For Each itemelementattr As LTS.tblEDIMasterDocStructElmntAttribute In oEDIMasterDocStructElmntAttributes
                                                db.tblEDIMasterDocStructElmntAttributes.DeleteOnSubmit(itemelementattr)
                                                db.SubmitChanges()
                                            Next


                                        End If
                                        db.tblEDIMasterDocStructElements.DeleteOnSubmit(itemelement)
                                        db.SubmitChanges()
                                    Next


                                End If
                                db.tblEDIMasterDocStructSegments.DeleteOnSubmit(item)
                                db.SubmitChanges()
                            Next
                            db.tblEDIMasterDocStructLoops.DeleteOnSubmit(oEDIDocStructLoops)
                            db.SubmitChanges()

                        End If

                        blnRet = True

                    Else
                        blnRet = False 'return true if the record does not exist (already deleted)
                    End If
                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructLoops"), db)
            End Try
        End Using
        Return blnRet
    End Function
    ''' <summary>
    ''' Deletes an EDI Master Parent Loop from the database
    ''' </summary>
    ''' <param name="MDSLoopControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterParentLoop(ByVal MDSLoopControl As Integer) As Boolean

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.spDeleteMDocLoops(MDSLoopControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterParentLoop"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''check for existing Master struct loop
    Public Function GetEDIStructLoopbyLoopIdParentId(ByVal MasterDocEDITControl As Integer) As LTS.tblEDIMasterDocStructLoops()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing
        If MasterDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructLoops", "Invalid tblEDIMasterDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructLoops.Any(Function(x) x.MDSLoopLoopControl = MasterDocEDITControl) Then
                    Dim ediEDITPDocuments = db.tblEDIMasterDocStructLoops.Where(Function(x) x.MDSLoopLoopControl = MasterDocEDITControl)
                    iQuery = ediEDITPDocuments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMsDocumentByEDITControl"), db)
            End Try
            Return Nothing
        End Using
    End Function
#End Region
End Class


Public Class NGLEDIMDocStructLoopSegment : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIMasterDocStructSegments
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIMDocStructLoopSegment"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIMasterDocStructSegments
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region
#Region "Public Methods"
    '''Added By SRP on 3/8/18 SegmentElementAddExample
    Public Function InsertSegmentElement(ByVal MDSSegControl As Integer) As Integer
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.spAddEDIMasterDocStructElements(MDSSegControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertSegmentElement"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''Added By SRP on 3/19/18 SegmentElementlistAddExample

    Public Function InsertSegmentElementlist(ByVal segmentlength As Integer, ByVal SegmentControl As Integer, ByVal MDSSegMDSLoopControl As Integer, ByVal MDSSegModUser As String) As LTS.spPopulateEDIMasterLoopSegmentElementsResult()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Dim oRet As LTS.spPopulateEDIMasterLoopSegmentElementsResult()
            Dim iQuery As IQueryable(Of LTS.spPopulateEDIMasterLoopSegmentElementsResult) = Nothing
            Try
                Dim ediEDIMasterDocStructSegments = db.spPopulateEDIMasterLoopSegmentElements(segmentlength, SegmentControl, MDSSegMDSLoopControl, MDSSegModUser)
                Return ediEDIMasterDocStructSegments.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertSegmentElementlist"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Get tblEDIMasterDocStructSegment record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructSegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructSegment(ByVal Control As Integer) As LTS.tblEDIMasterDocStructSegment()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructSegment) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructSegments", "Invalid EDIMasterDocStructSegment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructSegments.Any(Function(x) x.MDSSegControl = Control) Then
                    Dim ediEDIMasterDocStructSegments As LTS.tblEDIMasterDocStructSegment = db.tblEDIMasterDocStructSegments.Where(Function(x) x.MDSSegControl = Control).FirstOrDefault()
                    iQuery = ediEDIMasterDocStructSegments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructSegment"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDIMasterDocStructSegment record by segment and Loop 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructSegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructLoopSegment() As LTS.tblEDIMasterDocStructSegment()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructSegment) = Nothing
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim ediEDIMasterDocStructSegments = db.tblEDIMasterDocStructSegments
                iQuery = ediEDIMasterDocStructSegments
                    oRet = iQuery.ToArray()
                    Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructLoopSegment"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIMasterDocStructSegments Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIMasterDocStructSegments</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function GetMDocStructLoopSegments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIMasterDocStructSegment()
        Dim oRet As LTS.tblEDIMasterDocStructSegment()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructSegment) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIMasterDocStructSegments

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIMasterDocStructSegments.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocStructLoopSegments"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIMasterDocStructSegments Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocStructSegments</param>
    ''' <returns>tblEDIMasterDocStructSegments</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function UpdateMDocStructLoopSegment(ByVal oData As LTS.tblEDIMasterDocStructSegment) As LTS.tblEDIMasterDocStructSegment
        Dim iControl As Integer = oData.MDSSegControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructSegment", "Invalid MDoc StructLoop Segment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSSegModDate = Date.Now
                oData.MDSSegModUser = Parameters.UserName
                db.tblEDIMasterDocStructSegments.Attach(oData, True)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMDocStructLoopSegment"), db)
            End Try
        End Using
        Return oData
    End Function

    Public Function PopulateEDIMasterLoopSegmentElements(ByVal SegmentControl As String, ByVal segmentlength As String, ByVal MDSSegMDSLoopControl As String) As LTS.spPopulateEDIMasterLoopSegmentElementsResult()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.spPopulateEDIMasterLoopSegmentElementsResult()
        Dim iQuery As IQueryable(Of LTS.spPopulateEDIMasterLoopSegmentElementsResult) = Nothing
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim MDSSegCreateUser = Parameters.UserName
                Dim edielements = InsertSegmentElementlist(Convert.ToInt32(segmentlength), Convert.ToInt32(SegmentControl), Convert.ToInt32(MDSSegMDSLoopControl), Parameters.UserName)

                Return edielements.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("PopulateEDIMasterLoopSegmentElements"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetDocSegmentElementListbySegment(ByVal SegmentControl As Integer) As LTS.tblEDIMasterDocStructElement()
        Dim oRet As LTS.tblEDIMasterDocStructElement()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElement) = Nothing
        Dim blnRet As Boolean = False
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Dim ediEDIMasterDocStructSegments
            Try

                If db.tblEDIMasterDocStructElements.Any(Function(x) x.MDSElementMDSSegControl = SegmentControl) Then
                    ediEDIMasterDocStructSegments = db.tblEDIMasterDocStructElements.Where(Function(x) x.MDSElementMDSSegControl = SegmentControl)
                    iQuery = ediEDIMasterDocStructSegments

                    oRet = iQuery.ToArray()
                    Return oRet

                Else
                    blnRet = True 'return true if the record does not exist 

                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDocSegmentElementListbySegment"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetDocSegmentElementListbySegmentLoop() As LTS.vwGetElementsBySegmentLoop()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vwGetElementsBySegmentLoops()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDocSegmentElementListbySegment"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetMDocSegmentsByLoops() As LTS.vwGetMDocSegmentsByLoop()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vwGetMDocSegmentsByLoops()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocSegmentsByLoops"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Deletes an EDIMasterDocStructSegments from the database
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterDocStructSegments(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructSegment) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructSegments", "Invalid tblEDIMasterDocStructSegment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                'Deleting Elements related to Struct Segment
                If db.tblEDIMasterDocStructSegments.Any(Function(x) x.MDSSegControl = oData) Then
                    Dim oEDIDocStructSegments = db.tblEDIMasterDocStructSegments.Where(Function(x) x.MDSSegControl = oData)
                    'Deleting Elements related to Struct Element
                    For Each item As LTS.tblEDIMasterDocStructSegment In oEDIDocStructSegments
                        If db.tblEDIMasterDocStructElements.Any(Function(x) x.MDSElementMDSSegControl = item.MDSSegSegmentControl) Then
                            Dim oEDIDocStructElements = db.tblEDIMasterDocStructElements.Where(Function(x) x.MDSElementMDSSegControl = item.MDSSegSegmentControl)
                            'Deleting Elements related to Struct Element Attributes
                            For Each itemelement As LTS.tblEDIMasterDocStructElement In oEDIDocStructElements
                                If db.tblEDIMasterDocStructElmntAttributes.Any(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl) Then
                                    Dim oEDIMasterDocStructElmntAttributes = db.tblEDIMasterDocStructElmntAttributes.Where(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl)

                                    For Each itemelementattr As LTS.tblEDIMasterDocStructElmntAttribute In oEDIMasterDocStructElmntAttributes
                                        db.tblEDIMasterDocStructElmntAttributes.DeleteOnSubmit(itemelementattr)
                                        db.SubmitChanges()
                                    Next

                                End If
                                db.tblEDIMasterDocStructElements.DeleteOnSubmit(itemelement)
                                db.SubmitChanges()
                            Next


                        End If
                        db.tblEDIMasterDocStructSegments.DeleteOnSubmit(item)
                        db.SubmitChanges()
                    Next
                    blnRet = True

                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructLoops"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Insert tblEDIMasterDocStructSegments Data
    ''' </summary>
    ''' <param name="oData">EDIMasterDocStructSegments</param>
    ''' <returns>tblEDIMasterDocStructSegments</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function InsertMDocStructLoopSegment(ByVal oData As LTS.tblEDIMasterDocStructSegment) As LTS.tblEDIMasterDocStructSegment
        Dim iControl As Integer = oData.MDSSegControl

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSSegCreateUser = Parameters.UserName
                oData.MDSSegModUser = Parameters.UserName
                oData.MDSSegCreateDate = Date.Now
                oData.MDSSegModDate = Date.Now
                db.tblEDIMasterDocStructSegments.InsertOnSubmit(oData)
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertMDocStructLoopSegment"), db)
            End Try
        End Using
        Return oData
    End Function
#End Region
End Class
Public Class NGLEDIMDocStructelement : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIMasterDocStructElements
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIMDocStructelement"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIMasterDocStructElements
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
    ''' Get tblEDIMasterDocStructElmntAttribute record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructElmntAttribute</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructElement(ByVal Control As Integer) As LTS.tblEDIMasterDocStructElement()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructElement()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElement) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElements", "Invalid EDIMasterDocStructElement, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructElements.Any(Function(x) x.MDSElementControl = Control) Then
                    Dim ediMasterDocStructElements As LTS.tblEDIMasterDocStructElement = db.tblEDIMasterDocStructElements.Where(Function(x) x.MDSElementControl = Control).FirstOrDefault()
                    iQuery = ediMasterDocStructElements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructElement"), db)
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Get EDIMasterDocStructElements Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIMasterDocStructElements</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function GetMDocSegmentElements(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIMasterDocStructElement()
        Dim oRet As LTS.tblEDIMasterDocStructElement()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElement) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIMasterDocStructElements

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIMasterDocStructElements.ToArray()
                End If

                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocSegmentElements"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIMasterDocStructElement Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocStructElement</param>
    ''' <returns>tblEDIMasterDocStructElement</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function UpdateMDocSegmentElement(ByVal oData As LTS.tblEDIMasterDocStructElement) As LTS.tblEDIMasterDocStructElement
        Dim iControl As Integer = oData.MDSElementControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElement", "Invalid MDoc Segment Element, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim updateCust = (From cust In db.tblEDIMasterDocStructElements
                                  Where cust.MDSElementControl = oData.MDSElementControl).ToList()(0)
                updateCust.MDSElementModDate = Date.Now
                updateCust.MDSElementModUser = Parameters.UserName
                updateCust.MDSElementDesc = oData.MDSElementDesc
                updateCust.MDSElementEDIDataTypeControl = oData.MDSElementEDIDataTypeControl
                updateCust.MDSElementUsage = oData.MDSElementUsage
                updateCust.MDSElementMinCount = oData.MDSElementMinCount
                updateCust.MDSElementMaxCount = oData.MDSElementMaxCount
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMDocSegmentElement"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIMasterDocStructElement Data
    ''' </summary>
    ''' <param name="oData">EDIMasterDocStructElement</param>
    ''' <returns>tblEDIMasterDocStructElement</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function InsertMDocSegmentElement(ByVal oData As LTS.tblEDIMasterDocStructElement) As LTS.tblEDIMasterDocStructElement
        Dim iControl As Integer = oData.MDSElementControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSElementCreateUser = Parameters.UserName
                oData.MDSElementModUser = Parameters.UserName
                oData.MDSElementCreateDate = Date.Now
                oData.MDSElementModDate = Date.Now
                db.tblEDIMasterDocStructElements.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertMDocSegmentElement"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Deletes an EDIMasterDocStructSegments from the database
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterDocStructElements(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructElement()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElement) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElements", "Invalid tblEDIMasterDocStructElement, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                If db.tblEDIMasterDocStructElements.Any(Function(x) x.MDSElementControl = oData) Then
                    Dim oEDIDocStructElements = db.tblEDIMasterDocStructElements.Where(Function(x) x.MDSElementControl = oData)
                    'Deleting Elements related to Struct Element Attributes
                    For Each itemelement As LTS.tblEDIMasterDocStructElement In oEDIDocStructElements
                        If db.tblEDIMasterDocStructElmntAttributes.Any(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl) Then
                            Dim oEDIMasterDocStructElmntAttributes = db.tblEDIMasterDocStructElmntAttributes.Where(Function(x) x.MDSAttrMDSElementControl = itemelement.MDSElementControl)

                            For Each itemelementattr As LTS.tblEDIMasterDocStructElmntAttribute In oEDIMasterDocStructElmntAttributes
                                db.tblEDIMasterDocStructElmntAttributes.DeleteOnSubmit(itemelementattr)
                                db.SubmitChanges()
                            Next
                        End If
                        'Deleting Elements related to Struct Element
                        db.tblEDIMasterDocStructElements.DeleteOnSubmit(itemelement)
                        db.SubmitChanges()
                    Next

                    blnRet = True

                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructLoops"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
End Class

Public Class NGLEDIMDocStructelementattribute : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIMasterDocStructElmntAttributes
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIMDocStructelementattribute"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIMasterDocStructElmntAttributes
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
    ''' Get tblEDIMasterDocStructElmntAttribute record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructElmntAttribute</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIMasterDocStructElmntAttribute(ByVal Control As Integer) As LTS.tblEDIMasterDocStructElmntAttribute()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructElmntAttribute()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElmntAttribute) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElmntAttributes", "Invalid MasterDocStructElmntAttribute, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocStructElmntAttributes.Any(Function(x) x.MDSAttrMDSElementControl = Control) Then
                    Dim ediMasterDocStructElmntAttributes = db.tblEDIMasterDocStructElmntAttributes.Where(Function(x) x.MDSAttrMDSElementControl = Control)
                    iQuery = ediMasterDocStructElmntAttributes
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMasterDocStructElmntAttribute"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIMasterDocStructElmntAttributes Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIMasterDocStructElmntAttributes</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function GetMDocSegElementAttributes(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIMasterDocStructElmntAttribute()
        Dim oRet As LTS.tblEDIMasterDocStructElmntAttribute()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElmntAttribute) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIMasterDocStructElmntAttributes

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIMasterDocStructElmntAttributes.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocSegElementAttributes"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIMasterDocStructElmntAttributes Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIMasterDocStructElmntAttributes</param>
    ''' <returns>tblEDIMasterDocStructElmntAttributes</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function UpdateMDocSegmentElementAtribute(ByVal oData As LTS.tblEDIMasterDocStructElmntAttribute) As LTS.tblEDIMasterDocStructElmntAttribute
        Dim iControl As Integer = oData.MDSAttrControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElmntAttribute", "Invalid MDoc Segment Element Atribute, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim updateCust = (From cust In db.tblEDIMasterDocStructElmntAttributes
                                  Where cust.MDSAttrControl = oData.MDSAttrControl).ToList()(0)
                updateCust.MDSAttrModDate = Date.Now
                updateCust.MDSAttrModUser = Parameters.UserName
                updateCust.MDSAttrCreateDate = oData.MDSAttrCreateDate
                updateCust.MDSAttrCreateUser = oData.MDSAttrCreateUser
                updateCust.MDSAttrMDSElementControl = oData.MDSAttrMDSElementControl
                updateCust.MDSAttrQualifyingElementControl = oData.MDSAttrQualifyingElementControl
                updateCust.MDSAttrNotes = oData.MDSAttrNotes
                updateCust.MDSAttrQualifyingValue = oData.MDSAttrQualifyingValue
                updateCust.MDSAttrUsage = oData.MDSAttrUsage
                updateCust.MDSAttrDefaultVal = oData.MDSAttrDefaultVal
                updateCust.MDSAttrTransformationTypeControl = oData.MDSAttrTransformationTypeControl
                updateCust.MDSAttrValidationTypeControl = oData.MDSAttrValidationTypeControl
                updateCust.MDSAttrFormattingFnControl = oData.MDSAttrFormattingFnControl
                updateCust.MDSAttrDataMapFieldControl = oData.MDSAttrDataMapFieldControl

                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMDocSegmentElementAtribute"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIMasterDocStructElmntAttributes Data
    ''' </summary>
    ''' <param name="oData">EDIMasterDocStructElmntAttributes</param>
    ''' <returns>tblEDIMasterDocStructElmntAttributes</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 2/28/18
    ''' </remarks>
    Public Function InsertMDocSegElementAttributes(ByVal oData As LTS.tblEDIMasterDocStructElmntAttribute) As LTS.tblEDIMasterDocStructElmntAttribute
        Dim iControl As Integer = oData.MDSAttrControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.MDSAttrCreateUser = Parameters.UserName
                oData.MDSAttrUsage = "O"
                oData.MDSAttrModUser = Parameters.UserName
                oData.MDSAttrCreateDate = Date.Now
                oData.MDSAttrModDate = Date.Now
                db.tblEDIMasterDocStructElmntAttributes.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertMDocSegElementAttributes"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' GetDocSegmentbyLoop
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetMasterDocStructElmntAttributes() As LTS.vw_tblEDIMasterDocStructElmntAttribute()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vw_tblEDIMasterDocStructElmntAttributes()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMasterDocStructElmntAttributes"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Deletes an EDIMasterDocStructElementAttribute from the database
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by PKN for v-8.1 on 5/7/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIMasterDocStructElementAttribute(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIMasterDocStructElmntAttribute()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructElmntAttribute) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIMasterDocStructElmntAttributes", "Invalid tblEDIMasterDocStructElmntAttributes, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                If db.tblEDIMasterDocStructElmntAttributes.Any(Function(x) x.MDSAttrControl = oData) Then
                    Dim oEleAttrib As LTS.tblEDIMasterDocStructElmntAttribute = db.tblEDIMasterDocStructElmntAttributes.Where(Function(x) x.MDSAttrControl = oData).FirstOrDefault()
                    If Not oEleAttrib Is Nothing Then
                        db.tblEDIMasterDocStructElmntAttributes.DeleteOnSubmit(oEleAttrib)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = False
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructElementAttribute"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
End Class


Public Class EDITPDocument : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDITPDocuments
        Me.LinqDB = db
        Me.SourceClass = "EDITPDocument"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDITPDocuments
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
    ''' Get tblEDITPDocument record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDITPDocument(ByVal oData As LTS.tblEDITPDocument) As LTS.tblEDITPDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDITPDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDITPDocument) = Nothing
        If oData.TPDocControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITPDocuments", "Invalid EDITPDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDITPDocuments.Any(Function(x) x.TPDocControl = oData.TPDocControl) Then
                    Dim ediEDITPDocuments As LTS.tblEDITPDocument = db.tblEDITPDocuments.Where(Function(x) x.TPDocControl = oData.TPDocControl).FirstOrDefault()
                    iQuery = ediEDITPDocuments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDITPDocument"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDITPDocument record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDITPDocumentByCarrier(ByVal TPDocCCEDIControl As Integer, ByVal TPDocEDITControl As Integer, ByVal TPDocInbound As Boolean) As LTS.tblEDITPDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDITPDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDITPDocument) = Nothing
        If TPDocCCEDIControl = 0 And TPDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITPDocuments", "Invalid EDITPDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocControl = TPDocEDITControl) Then
                    Dim ediEDIMasterDocuments As LTS.tblEDIMasterDocument = db.tblEDIMasterDocuments.Where(Function(x) x.MasterDocControl = TPDocEDITControl).FirstOrDefault()
                    If db.tblEDITPDocuments.Any(Function(x) x.TPDocCCEDIControl = TPDocCCEDIControl And x.TPDocEDITControl = ediEDIMasterDocuments.MasterDocEDITControl And x.TPDocInbound = TPDocInbound) Then
                        Dim ediEDITPDocuments = db.tblEDITPDocuments.Where(Function(x) x.TPDocCCEDIControl = TPDocCCEDIControl And x.TPDocEDITControl = ediEDIMasterDocuments.MasterDocEDITControl And x.TPDocInbound = TPDocInbound)
                        iQuery = ediEDITPDocuments
                        oRet = iQuery.ToArray()
                        Return oRet
                    Else
                        blnRet = True 'return true if the record does not exist 
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDITPDocumentByCarrier"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get tblEDITPDocument record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDITPDocumentByCarrierID(ByVal TPDocCCEDIControl As Integer, ByVal TPDocEDITControl As Integer, ByVal TPDocInbound As Boolean) As LTS.tblEDITPDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDITPDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDITPDocument) = Nothing
        If TPDocCCEDIControl = 0 And TPDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITPDocuments", "Invalid EDITPDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocEDITControl = TPDocEDITControl) Then
                    If db.tblEDITPDocuments.Any(Function(x) x.TPDocCCEDIControl = TPDocCCEDIControl And x.TPDocEDITControl = TPDocEDITControl And x.TPDocInbound = TPDocInbound) Then
                        Dim ediEDITPDocuments = db.tblEDITPDocuments.Where(Function(x) x.TPDocCCEDIControl = TPDocCCEDIControl And x.TPDocEDITControl = TPDocEDITControl And x.TPDocInbound = TPDocInbound)
                        iQuery = ediEDITPDocuments
                        oRet = iQuery.ToArray()
                        Return oRet
                    Else
                        blnRet = True 'return true if the record does not exist 
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDITPDocumentByCarrierID"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDITPDocuments Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/1/18
    ''' </remarks>
    Public Function GetEDITPDocuments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDITPDocument()
        Dim oRet As LTS.tblEDITPDocument()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDITPDocument) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDITPDocuments

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDITPDocuments.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMDocSegElementAttributes"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDITPDocument Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDITPDocument</param>
    ''' <returns>tblEDITPDocuments</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/1/18
    ''' </remarks>
    Public Function UpdateEDITPDocument(ByVal oData As LTS.tblEDITPDocument) As LTS.tblEDITPDocument
        Dim iControl As Integer = oData.TPDocControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITPDocument", "Invalid EDI TP Document, a control number is required and cannot be zero")
        End If

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim updateCust = (From cust In db.tblEDITPDocuments
                                  Where cust.TPDocControl = oData.TPDocControl).ToList()(0)
                updateCust.TPDocModDate = Date.Now
                updateCust.TPDocModUser = Parameters.UserName
                updateCust.TPDocCCEDIControl = oData.TPDocCCEDIControl
                updateCust.TPDocInbound = oData.TPDocInbound
                updateCust.TPDocEDITControl = oData.TPDocEDITControl
                db.SubmitChanges()


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDITPDocuments"), db)
            End Try

        End Using

        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDITPDocument Data
    ''' </summary>
    ''' <param name="oData">EDITPDocuments</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/1/18
    ''' </remarks>
    'Public Function InsertEDITPDocument(ByVal TPDocControl As Integer, ByVal TPDocCCEDIControl As Integer, ByVal TPDocEDITControl As Integer, ByVal TPDocInbound As Boolean, ByVal Action As String) As LTS.tblEDITPDocument
    Public Function InsertEDITPDocument(ByVal oData As LTS.tblEDITPDocument, ByVal Action As String) As LTS.tblEDITPDocument
        Dim updateCust
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.TPDocInbound = oData.TPDocInbound
                oData.TPDocCCEDIControl = oData.TPDocCCEDIControl
                oData.TPDocCreateUser = Parameters.UserName
                oData.TPDocModUser = Parameters.UserName
                oData.TPDocCreateDate = Date.Now
                oData.TPDocModDate = Date.Now
                If (Action = "EDIT") Then
                    oData.TPDocEDITControl = oData.TPDocEDITControl

                    Dim oldtp = oData.TPDocControl
                    db.tblEDITPDocuments.InsertOnSubmit(oData)
                    db.SubmitChanges()
                    db.spCopyTPDocConfigToNewTPDoc(oldtp, oData.TPDocControl)
                Else
                    If db.tblEDIMasterDocuments.Any(Function(x) x.MasterDocControl = oData.TPDocEDITControl) Then
                        updateCust = (From cust In db.tblEDIMasterDocuments
                                      Where cust.MasterDocControl = oData.TPDocEDITControl).ToList()(0)

                        oData.TPDocEDITControl = updateCust.MasterDocEDITControl

                        db.tblEDITPDocuments.InsertOnSubmit(oData)
                        db.SubmitChanges()
                        db.spCopyMDocConfigtoTPEDIDoc(updateCust.MasterDocControl, oData.TPDocControl)
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDITPDocument"), db)
            End Try
        End Using
        Return oData
    End Function

    ''Added By SRP on 3/30/18 
    Public Function GetTPDocumentPreview(ByVal TPDocControl As Integer) As LTS.sp_PreviewEDITPDocumentConfigResult()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.sp_PreviewEDITPDocumentConfig(TPDocControl).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTPDocumentPreview"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''Added By SRP on 3/30/18 
    Public Function CopyMDocConfigtoTPEDIDoc(ByVal MasterDocControl As Integer, ByVal TPDocControl As Integer) As Boolean

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.spCopyMDocConfigtoTPEDIDoc(MasterDocControl, TPDocControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTPDocumentPreview"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Get tblEDITPDocument record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDITPDocument</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDITPDocumentByEDITControl(ByVal MasterDocEDITControl As Integer, ByVal MasterDocInbound As Boolean) As LTS.tblEDITPDocument()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDITPDocument()
        Dim iQuery As IQueryable(Of LTS.tblEDITPDocument) = Nothing
        If MasterDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDITPDocuments", "Invalid EDITPDocument, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDITPDocuments.Any(Function(x) x.TPDocEDITControl = MasterDocEDITControl And x.TPDocInbound = MasterDocInbound) Then
                    Dim ediEDITPDocuments = db.tblEDITPDocuments.Where(Function(x) x.TPDocEDITControl = MasterDocEDITControl And x.TPDocInbound = MasterDocInbound)
                    iQuery = ediEDITPDocuments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIMsDocumentByEDITControl"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''Added By SRP on 3/30/18 
    Public Function CopyTPDocConfigToMasterDoc(ByVal MasterDocControl As Integer, ByVal TPDocControl As Integer) As Boolean
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Return db.spCopyTPDocConfigToMasterDoc(MasterDocControl, TPDocControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTPDocumentPreview"), db)
            End Try
        End Using
        Return Nothing
    End Function


    Public Function GetEDITPDocumentList(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.spEDIDocumentListResult()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            ''Dim oRet As LTS.spEDIDocumentListResult()
            ''Dim iQuery As IQueryable(Of LTS.spEDIDocumentListResult) = Nothing
            Try
                Dim dbp = New NGLMASEDIMaintDataContext
                If String.IsNullOrEmpty(filters.filterValue) Then
                    Dim productlist = db.spEDIDocumentList()
                    Return productlist.ToArray()
                Else

                    Dim productlist = db.spEDIDocumentList()

                    Return productlist.ToArray()

                End If
            Catch ex As Exception
                'ManageLinqDataExceptions(ex, buildProcedureName("GetEDITPDocumentList"), db)
                Return Nothing

            End Try
        End Using
        Return Nothing
    End Function


    Public Function GetCompCarrierList() As LTS.vwGetCompCarrier()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                'Getting Campanies and Carrier List 
                Dim dbp = New NGLMASEDIMaintDataContext

                Dim compcarrierlist = db.vwGetCompCarriers()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                'ManageLinqDataExceptions(ex, buildProcedureName("GetEDITPDocumentList"), db)
                Return Nothing

            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Deletes an EDI TP document from the database
    ''' </summary>
    ''' <param name="TPDocControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDITPDocumentFull(ByVal TPDocControl As Integer) As Integer

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Return db.SpDeleteTPDoc(TPDocControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocumentFull"), db)
            End Try
        End Using
        Return Nothing
    End Function
#End Region
End Class

Public Class EDIDocStructLoop : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDocStructLoops
        Me.LinqDB = db
        Me.SourceClass = "EDIDocStructLoop"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDocStructLoops
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
    ''' Get tblEDIDocStructLoops record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDocStructLoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocStructLoop(ByVal oData As LTS.tblEDIDocStructLoop) As LTS.tblEDIDocStructLoop()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructLoop()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructLoop) = Nothing
        If oData.DSLoopControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructLoops", "Invalid EDIDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructLoops.Any(Function(x) x.DSLoopControl = oData.DSLoopControl) Then
                    Dim ediDocStructLoops As LTS.tblEDIDocStructLoop = db.tblEDIDocStructLoops.Where(Function(x) x.DSLoopControl = oData.DSLoopControl).FirstOrDefault()
                    iQuery = ediDocStructLoops
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructLoop"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetEDIDocStructLoopbyTPDoc(ByVal TPControl As Integer) As LTS.tblEDIDocStructLoop()
        Dim oRet As LTS.tblEDIDocStructLoop()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructLoop) = Nothing
        Dim blnRet As Boolean = False
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Dim ediEDIMasterDocStructSegments
            Try

                If db.tblEDIDocStructLoops.Any(Function(x) x.DSLoopTPDocControl = TPControl) Then
                    ediEDIMasterDocStructSegments = db.tblEDIDocStructLoops.Where(Function(x) x.DSLoopTPDocControl = TPControl)
                    iQuery = ediEDIMasterDocStructSegments

                    oRet = iQuery.ToArray()
                    Return oRet

                Else

                    Return Nothing
                End If



            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructLoopbyTPDoc"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetEDIDocStructLoopbyTPDocument(ByVal TPControl As Integer) As LTS.tblEDIMasterDocStructLoops()
        Dim oRet As LTS.tblEDIMasterDocStructLoops()
        Dim iQuery As IQueryable(Of LTS.tblEDIMasterDocStructLoops) = Nothing
        Dim blnRet As Boolean = False
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Dim ediEDIMasterDocStructSegments
            Try

                ediEDIMasterDocStructSegments = db.tblEDIMasterDocStructLoops
                iQuery = ediEDIMasterDocStructSegments

                oRet = iQuery.ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructLoopbyTPDoc"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Get EDIDocStructLoopDetails Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDocStructLoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function GetEDIDocStructLoops(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDocStructLoop()
        Dim oRet As LTS.tblEDIDocStructLoop()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocStructLoop) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDocStructLoops

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDocStructLoops.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructLoops"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDocStructLoop Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDocStructLoop</param>
    ''' <returns>tblEDIDocStructLoop</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/1/18
    ''' </remarks>
    Public Function UpdateEDIDocStructLoop(ByVal oData As LTS.tblEDIDocStructLoop) As LTS.tblEDIDocStructLoop
        Dim iControl As Integer = oData.DSLoopControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructLoops", "Invalid EDI TP Document, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim updateCust = (From cust In db.tblEDIDocStructLoops
                                  Where cust.DSLoopControl = oData.DSLoopControl).ToList()(0)
                updateCust.DSLoopModDate = Date.Now
                updateCust.DSLoopModUser = Parameters.UserName
                updateCust.DSLoopParentLoopID = oData.DSLoopParentLoopID
                updateCust.DSLoopLoopControl = oData.DSLoopLoopControl
                updateCust.DSLoopDisabled = oData.DSLoopDisabled
                updateCust.DSLoopMinCount = oData.DSLoopMinCount
                updateCust.DSLoopMaxCount = oData.DSLoopMaxCount
                updateCust.DSLoopUsage = oData.DSLoopUsage
                updateCust.DSLoopSeqIndex = oData.DSLoopSeqIndex
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDocStructLoop"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDocStructLoops Data
    ''' </summary>
    ''' <param name="oData">EDIDocStructLoops</param>
    ''' <returns>tblEDIDocStructLoops</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function InsertEDIDocStructLoop(ByVal oData As LTS.tblEDIDocStructLoop) As LTS.tblEDIDocStructLoop
        Dim iControl As Integer = oData.DSLoopControl

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DSLoopCreateUser = Parameters.UserName
                oData.DSLoopModUser = Parameters.UserName
                oData.DSLoopCreateDate = Date.Now
                oData.DSLoopModDate = Date.Now
                db.tblEDIDocStructLoops.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDocStructLoop"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Deletes an EDIDocumentType from the database
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIDocStructLoops(ByVal oData As LTS.tblEDIDocStructLoop) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructLoop()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructLoop) = Nothing
        If oData.DSLoopControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructLoops", "Invalid tblEDIMasterDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructLoops.Any(Function(x) x.DSLoopControl = oData.DSLoopControl) Then
                    Dim oEDIDocStructLoops As LTS.tblEDIDocStructLoop = db.tblEDIDocStructLoops.Where(Function(x) x.DSLoopControl = oData.DSLoopControl).FirstOrDefault()
                    If db.tblEDITPDocuments.Any(Function(x) x.TPDocControl = oData.DSLoopTPDocControl And x.TPDocPublished = False) Then
                        'Deleting Segments related to Loop
                        db.tblEDIDocStructLoops.DeleteOnSubmit(oEDIDocStructLoops)
                        db.SubmitChanges()
                        blnRet = True

                    Else
                        blnRet = False 'return true if the record does not exist (already deleted)
                    End If
                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructLoops"), db)
            End Try
        End Using
        Return blnRet
    End Function
    ''' <summary>
    ''' Deletes an EDI Master Parent Loop from the database
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIParentLoop(ByVal DSLoopControl As Integer) As Boolean

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Return db.spDeleteTPDocLoops(DSLoopControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIParentLoop"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''check for existing Master struct loop
    Public Function GetEDIStructLoopbyLoopId(ByVal MasterDocEDITControl As Integer) As LTS.tblEDIDocStructLoop()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructLoop()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructLoop) = Nothing
        If MasterDocEDITControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructLoops", "Invalid tblEDIDocStructLoop, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructLoops.Any(Function(x) x.DSLoopLoopControl = MasterDocEDITControl) Then
                    Dim ediEDIdocstructloops = db.tblEDIDocStructLoops.Where(Function(x) x.DSLoopLoopControl = MasterDocEDITControl)
                    iQuery = ediEDIdocstructloops
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIStructLoopbyLoopId"), db)
            End Try
            Return Nothing
        End Using
    End Function
#End Region
End Class


Public Class EDIDocStructSegment : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDocStructSegments
        Me.LinqDB = db
        Me.SourceClass = "EDIDocStructSegment"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDocStructSegments
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
    ''' Get tblEDIDocStructSegment record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDocStructSegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocStructSegment(ByVal oData As LTS.tblEDIDocStructSegment) As LTS.tblEDIDocStructSegment()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructSegment) = Nothing
        If oData.DSSegControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructSegments", "Invalid EDIDocStructSegment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructSegments.Any(Function(x) x.DSSegControl = oData.DSSegControl) Then
                    Dim ediEDIDocStructSegments As LTS.tblEDIDocStructSegment = db.tblEDIDocStructSegments.Where(Function(x) x.DSSegControl = oData.DSSegControl).FirstOrDefault()
                    iQuery = ediEDIDocStructSegments
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructSegment"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Get tblEDIDocStructSegments Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDocStructSegments</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function GetEDIDocStructSegments(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDocStructSegment()
        Dim oRet As LTS.tblEDIDocStructSegment()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocStructSegment) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDocStructSegments

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDocStructSegments.ToArray()
                End If

                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructSegments"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDocStructSegment Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDocStructSegment</param>
    ''' <returns>tblEDIDocStructSegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/1/18
    ''' </remarks>
    Public Function UpdateEDIDocStructSegment(ByVal oData As LTS.tblEDIDocStructSegment) As LTS.tblEDIDocStructSegment
        Dim iControl As Integer = oData.DSSegControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructSegments", "Invalid EDI TP Document, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DSSegModDate = Date.Now
                oData.DSSegModUser = Parameters.UserName
                db.tblEDIDocStructSegments.Attach(oData, True)
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDocStructSegment"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDocStructSegments Data
    ''' </summary>
    ''' <param name="oData">EDIDocStructSegments</param>
    ''' <returns>tblEDIDocStructSegments</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function InsertEDIDocStructSegment(ByVal oData As LTS.tblEDIDocStructSegment) As LTS.tblEDIDocStructSegment
        Dim iControl As Integer = oData.DSSegControl

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DSSegCreateUser = Parameters.UserName
                oData.DSSegModUser = Parameters.UserName

                oData.DSSegCreateDate = Date.Now
                oData.DSSegModDate = Date.Now
                db.tblEDIDocStructSegments.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDocStructLoop"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' GetDocSegmentbyLoop
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetSegmentsByLoops() As LTS.vw_GetSegmentByLoop()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vw_GetSegmentByLoops()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSegmentsByLoops"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Get tblEDIDocStructSegment record by segment and Loop 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructSegment</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetStructSegmentByLoopSegment() As LTS.tblEDIDocStructSegment()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructSegment) = Nothing
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim ediEDIMasterDocStructSegments = db.tblEDIDocStructSegments
                iQuery = ediEDIMasterDocStructSegments
                oRet = iQuery.ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetStructSegmentByLoopSegment"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''Added By SRP on 3/19/18 SegmentElementlistAddExample

    Public Function InsertSegmentElementAttribute(ByVal segmentlength As Integer, ByVal SegmentControl As Integer, ByVal MDSSegMDSLoopControl As Integer, ByVal MDSSegModUser As String) As LTS.spPopulateEDITPLoopSegmentElementsResult()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Dim oRet As LTS.spPopulateEDITPLoopSegmentElementsResult()
            Dim iQuery As IQueryable(Of LTS.spPopulateEDITPLoopSegmentElementsResult) = Nothing
            Try
                Dim ediEDIMasterDocStructSegments = db.spPopulateEDITPLoopSegmentElements(segmentlength, SegmentControl, MDSSegMDSLoopControl, MDSSegModUser)
                Return ediEDIMasterDocStructSegments.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertSegmentElementAttribute"), db)
            End Try
        End Using
        Return Nothing
    End Function


    Public Function PopulateEDILoopSegmentElements(ByVal SegmentControl As String, ByVal segmentlength As String, ByVal MDSSegMDSLoopControl As String) As LTS.spPopulateEDITPLoopSegmentElementsResult()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.spPopulateEDITPLoopSegmentElementsResult()
        Dim iQuery As IQueryable(Of LTS.spPopulateEDITPLoopSegmentElementsResult) = Nothing
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim MDSSegCreateUser = Parameters.UserName
                Dim edielements = InsertSegmentElementAttribute(Convert.ToInt32(segmentlength), Convert.ToInt32(SegmentControl), Convert.ToInt32(MDSSegMDSLoopControl), Parameters.UserName)

                Return edielements.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("PopulateEDILoopSegmentElements"), db)
            End Try
        End Using
        Return Nothing
    End Function
    ''' <summary>
    ''' Deletes an EDIDocStructSegments from the database
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIDocStructSegments(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructSegment()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructSegment) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructSegments", "Invalid tblEDIDocStructSegment, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                'Deleting Elements related to Struct Segment
                If db.tblEDIDocStructSegments.Any(Function(x) x.DSSegControl = oData) Then
                    Dim oEDIDocStructSegments = db.tblEDIDocStructSegments.Where(Function(x) x.DSSegControl = oData)
                    'Deleting Elements related to Struct Element
                    For Each item As LTS.tblEDIDocStructSegment In oEDIDocStructSegments
                        If db.tblEDIDocStructElements.Any(Function(x) x.DSElementDSSegControl = item.DSSegSegmentControl) Then
                            Dim oEDIDocStructElements = db.tblEDIDocStructElements.Where(Function(x) x.DSElementDSSegControl = item.DSSegSegmentControl)
                            'Deleting Elements related to Struct Element Attributes
                            For Each itemelement As LTS.tblEDIDocStructElement In oEDIDocStructElements
                                If db.tblEDIDocStructElmntAttributes.Any(Function(x) x.DSAttrDSElementControl = itemelement.DSElementControl) Then
                                    Dim oEDIMasterDocStructElmntAttributes = db.tblEDIDocStructElmntAttributes.Where(Function(x) x.DSAttrDSElementControl = itemelement.DSElementControl)

                                    For Each itemelementattr As LTS.tblEDIDocStructElmntAttribute In oEDIMasterDocStructElmntAttributes
                                        db.tblEDIDocStructElmntAttributes.DeleteOnSubmit(itemelementattr)
                                        db.SubmitChanges()
                                    Next
                                End If
                                db.tblEDIDocStructElements.DeleteOnSubmit(itemelement)
                                db.SubmitChanges()
                            Next

                        End If
                        db.tblEDIDocStructSegments.DeleteOnSubmit(item)
                        db.SubmitChanges()
                    Next
                    blnRet = True

                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIDocStructSegments"), db)
            End Try
        End Using
        Return blnRet
    End Function
    ''' <summary>
    ''' GetDocSegmentElementList
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetSegmentElementbySegmentLoop() As LTS.vwGetSegmentElementsBySegmentLoop()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vwGetSegmentElementsBySegmentLoops()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSegmentElementbySegmentLoop"), db)
            End Try
        End Using
        Return Nothing
    End Function
#End Region
End Class

Public Class NGLEDIDocStructElement : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDocStructElements
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIDocStructElement"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDocStructElements
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
    ''' Get tblEDIDocStructElement record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDocStructElement</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocStructElement(ByVal oData As LTS.tblEDIDocStructElement) As LTS.tblEDIDocStructElement()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructElement()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElement) = Nothing
        If oData.DSElementControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElements", "Invalid EDIDocStructElement, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructElements.Any(Function(x) x.DSElementControl = oData.DSElementControl) Then
                    Dim editblEDIDocStructElements As LTS.tblEDIDocStructElement = db.tblEDIDocStructElements.Where(Function(x) x.DSElementControl = oData.DSElementControl).FirstOrDefault()
                    iQuery = editblEDIDocStructElements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructElement"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIDocStructLoopDetails Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDocStructLoops</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function GetEDIDocStructElements(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDocStructElement()
        Dim oRet As LTS.tblEDIDocStructElement()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElement) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDocStructElements

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDocStructElements.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructElements"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDocStructElement Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDocStructElement</param>
    ''' <returns>tblEDIDocStructElement</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function UpdateEDIDocStructElement(ByVal oData As LTS.tblEDIDocStructElement) As LTS.tblEDIDocStructElement
        Dim iControl As Integer = oData.DSElementControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElements", "Invalid EDI TP Document, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim updateCust = (From cust In db.tblEDIDocStructElements
                                  Where cust.DSElementControl = oData.DSElementControl).ToList()(0)
                updateCust.DSElementModDate = Date.Now
                updateCust.DSElementModUser = Parameters.UserName
                updateCust.DSElementDesc = oData.DSElementDesc
                updateCust.DSElementEDIDataTypeControl = oData.DSElementEDIDataTypeControl
                updateCust.DSElementUsage = oData.DSElementUsage
                updateCust.DSElementMinCount = oData.DSElementMinCount
                updateCust.DSElementMaxCount = oData.DSElementMaxCount
                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDocStructElement"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDocStructSegments Data
    ''' </summary>
    ''' <param name="oData">EDIDocStructSegments</param>
    ''' <returns>tblEDIDocStructSegments</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function InsertEDIDocStructElement(ByVal oData As LTS.tblEDIDocStructElement) As LTS.tblEDIDocStructElement
        Dim iControl As Integer = oData.DSElementControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DSElementCreateUser = Parameters.UserName
                oData.DSElementModUser = Parameters.UserName
                oData.DSElementCreateDate = Date.Now
                oData.DSElementModDate = Date.Now
                db.tblEDIDocStructElements.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDocStructElement"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Deletes an EDIMasterDocStructSegments from the database
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/9/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDIDocStructElements(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructElement()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElement) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElements", "Invalid tblEDIDocStructElement, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructElements.Any(Function(x) x.DSElementControl = oData) Then
                    Dim oEDIDocStructElements = db.tblEDIDocStructElements.Where(Function(x) x.DSElementControl = oData)
                    'Deleting Elements related to Struct Element Attributes
                    For Each itemelement As LTS.tblEDIDocStructElement In oEDIDocStructElements
                        If db.tblEDIDocStructElmntAttributes.Any(Function(x) x.DSAttrDSElementControl = itemelement.DSElementControl) Then
                            Dim oEDIMasterDocStructElmntAttributes = db.tblEDIDocStructElmntAttributes.Where(Function(x) x.DSAttrDSElementControl = itemelement.DSElementControl)

                            For Each itemelementattr As LTS.tblEDIDocStructElmntAttribute In oEDIMasterDocStructElmntAttributes
                                db.tblEDIDocStructElmntAttributes.DeleteOnSubmit(itemelementattr)
                                db.SubmitChanges()
                            Next

                        End If
                        'Deleting Elements related to Struct Element
                        db.tblEDIDocStructElements.DeleteOnSubmit(itemelement)
                        db.SubmitChanges()
                    Next

                    blnRet = True

                Else
                    blnRet = False 'return true if the record does not exist (already deleted)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDIMasterDocStructLoops"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
End Class

Public Class NGLEDIDocStructElmntAttribute : Inherits NGLLinkDataBaseClass
#Region "Constructors"

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIDocStructElmntAttributes
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIDocStructElmntAttribute"
    End Sub



#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASEDIMaintDataContext(ConnectionString)
            _LinqTable = db.tblEDIDocStructElmntAttributes
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
    ''' Get tblEDIDocStructElmntAttribute record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIDocStructElmntAttribute</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocStructElmntAttribute(ByVal oData As LTS.tblEDIDocStructElmntAttribute) As LTS.tblEDIDocStructElmntAttribute()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructElmntAttribute()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElmntAttribute) = Nothing
        If oData.DSAttrControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElmntAttributes", "Invalid EDIDocStructElement, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructElmntAttributes.Any(Function(x) x.DSAttrControl = oData.DSAttrControl) Then
                    Dim ediEDIDocStructElements As LTS.tblEDIDocStructElmntAttribute = db.tblEDIDocStructElmntAttributes.Where(Function(x) x.DSAttrControl = oData.DSAttrControl).FirstOrDefault()
                    iQuery = ediEDIDocStructElements
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructElmntAttribute"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Get EDIDocStructElmntAttributeDetail Filtered
    ''' </summary>
    ''' <param name="filters">filtered data</param>
    ''' <param name="RecordCount">RecordCount</param>
    ''' <returns>tblEDIDocStructElmntAttributeDetails</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function GetEDIDocStructElmntAttributes(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDIDocStructElmntAttribute()
        Dim oRet As LTS.tblEDIDocStructElmntAttribute()
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElmntAttribute) = Nothing

                If Not filters Is Nothing Then
                    Dim filterWhere = ""
                    If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                        If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                            Dim dblVal = 0
                            If Double.TryParse(filters.filterValue, dblVal) Then
                                filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                            Else
                                filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                            End If
                        End If
                        If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                            Dim StartDate = DTran.formatStartDateFilter(filters.filterFrom)
                            Dim EndDate = DTran.formatEndDateFilter(filters.filterTo)
                            filterWhere = "((" + filters.filterName + " = NULL) OR (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) AND " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        End If
                    End If

                    iQuery = db.tblEDIDocStructElmntAttributes

                    If Not String.IsNullOrWhiteSpace(filterWhere) Then
                        iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                    End If

                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Else
                    oRet = db.tblEDIDocStructElmntAttributes.ToArray()
                End If



                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructElmntAttributes"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Saves an tblEDIDocStructElmntAttribute Data to the database
    ''' </summary>
    ''' <param name="oData">tblEDIDocStructElmntAttribute</param>
    ''' <returns>tblEDIDocStructElmntAttribute</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function UpdateEDIDocStructElmntAttribute(ByVal oData As LTS.tblEDIDocStructElmntAttribute) As LTS.tblEDIDocStructElmntAttribute
        Dim iControl As Integer = oData.DSAttrControl

        If iControl = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElmntAttributes", "Invalid EDI TP Document, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                Dim updateCust = (From cust In db.tblEDIDocStructElmntAttributes
                                  Where cust.DSAttrControl = oData.DSAttrControl).ToList()(0)
                updateCust.DSAttrModDate = Date.Now
                updateCust.DSAttrModUser = Parameters.UserName
                updateCust.DSAttrCreateDate = oData.DSAttrCreateDate
                updateCust.DSAttrDSElementControl = oData.DSAttrDSElementControl
                updateCust.DSAttrQualifyingElementControl = oData.DSAttrQualifyingElementControl
                updateCust.DSAttrNotes = oData.DSAttrNotes
                updateCust.DSAttrQualifyingValue = oData.DSAttrQualifyingValue
                updateCust.DSAttrUsage = oData.DSAttrUsage
                updateCust.DSAttrDefaultVal = oData.DSAttrDefaultVal
                updateCust.DSAttrTransformationTypeControl = oData.DSAttrTransformationTypeControl
                updateCust.DSAttrValidationTypeControl = oData.DSAttrValidationTypeControl
                updateCust.DSAttrFormattingFnControl = oData.DSAttrFormattingFnControl
                updateCust.DSAttrDataMapFieldControl = oData.DSAttrDataMapFieldControl

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateEDIDocStructElmntAttribute"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Insert tblEDIDocStructElmntAttributes Data
    ''' </summary>
    ''' <param name="oData">EDIDocStructElmntAttributes</param>
    ''' <returns>tblEDIDocStructElmntAttributes</returns>
    ''' <tblEDISegments/>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/2/18
    ''' </remarks>
    Public Function InsertEDIDocStructElmntAttribute(ByVal oData As LTS.tblEDIDocStructElmntAttribute) As LTS.tblEDIDocStructElmntAttribute
        Dim iControl As Integer = oData.DSAttrControl


        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                oData.DSAttrCreateUser = Parameters.UserName
                oData.DSAttrUsage = "O"
                oData.DSAttrModUser = Parameters.UserName
                oData.DSAttrCreateDate = Date.Now
                oData.DSAttrModDate = Date.Now
                db.tblEDIDocStructElmntAttributes.InsertOnSubmit(oData)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDIDocStructElmntAttribute"), db)
            End Try
        End Using
        Return oData
    End Function
    ''' <summary>
    ''' Get tblEDIMasterDocStructElmntAttribute record by Control 
    ''' </summary>
    ''' <param name="Control">Control</param>
    ''' <returns>tblEDIMasterDocStructElmntAttribute</returns>
    ''' <remarks>
    ''' Created by SRP for v-8.1 on 3/7/18
    ''' </remarks>
    Public Function GetEDIDocStructElmntAttribute(ByVal Control As Integer) As LTS.tblEDIDocStructElmntAttribute()
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructElmntAttribute()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElmntAttribute) = Nothing
        If Control = 0 Then
            throwInvalidRequiredKeysException("tblEDIDocStructElmntAttributes", "Invalid MasterDocStructElmntAttribute, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try
                If db.tblEDIDocStructElmntAttributes.Any(Function(x) x.DSAttrDSElementControl = Control) Then
                    Dim ediMasterDocStructElmntAttributes = db.tblEDIDocStructElmntAttributes.Where(Function(x) x.DSAttrDSElementControl = Control)
                    iQuery = ediMasterDocStructElmntAttributes
                    oRet = iQuery.ToArray()
                    Return oRet
                Else
                    blnRet = True 'return true if the record does not exist 
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIDocStructElmntAttribute"), db)
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' GetDocSegmentbyLoop
    ''' </summary>
    ''' <returns>tblEDIDocSegmentElements_GetResult</returns>
    Public Function GetDocStructElmntAttributes() As LTS.vw_tblEDIDocStructElmntAttribute()

        Using db As New NGLMASEDIMaintDataContext(ConnectionString)

            Try
                Dim compcarrierlist = db.vw_tblEDIDocStructElmntAttributes()
                Return compcarrierlist.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDocStructElmntAttributes"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Deletes an EDITPDocStructElementAttribute from the database
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by PKN for v-8.1 on 5/7/18 LookupLists
    ''' </remarks>
    Public Function DeleteEDITPDocStructElementAttribute(ByVal oData As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As LTS.tblEDIDocStructElmntAttribute()
        Dim iQuery As IQueryable(Of LTS.tblEDIDocStructElmntAttribute) = Nothing
        If oData = 0 Then
            throwInvalidRequiredKeysException("tblEDDocStructElmntAttributes", "Invalid tblEDIDocStructElmntAttributes, a control number is required and cannot be zero")
        End If
        Using db As New NGLMASEDIMaintDataContext(ConnectionString)
            Try

                If db.tblEDIDocStructElmntAttributes.Any(Function(x) x.DSAttrControl = oData) Then
                    Dim oEleAttrib As LTS.tblEDIDocStructElmntAttribute = db.tblEDIDocStructElmntAttributes.Where(Function(x) x.DSAttrControl = oData).FirstOrDefault()
                    If Not oEleAttrib Is Nothing Then
                        db.tblEDIDocStructElmntAttributes.DeleteOnSubmit(oEleAttrib)
                        db.SubmitChanges()
                        blnRet = True
                    End If
                Else
                    blnRet = False
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteEDITPDocStructElementAttribute"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region
End Class