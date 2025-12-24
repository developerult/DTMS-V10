Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Reflection
Imports System.Linq.Dynamic
Imports SerilogTracing

Public MustInherit Class NDPBaseClass : Inherits NGLLinkDataBaseClass

#Region " Constructors "
    Public Sub New()
        MyBase.New()
        'Logger.Verbose("NDPBaseClass Constructor")
        Logger = Logger.ForContext(Of NDPBaseClass)
    End Sub

#End Region

#Region " Properties"

    'Modified by RHR For v-8.2 On 10/06/2018
    '   NGLLinkDataBaseClass is not the super class to NDPBaseClass
    '    _dictNDPBaseClasses is no longer used
    'Private _dictNDPBaseClasses As New Dictionary(Of String, NDPBaseClass)
    ''' <summary>
    ''' Creates and Shares Instances of NDPBaseClass object using the Class type.
    ''' Only one instance per type will be created unless the blnAlwasyCreateNew 
    ''' flag is true: this is the default behavior because each object
    ''' with a shared instance must override teh LinqTable property
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 2/18/14 v.6.4 to use dictionary instad of list for more efficient look up of source object reference
    ''' Modified by RHR for v-8.2 on 10/06/2018
    '''   NGLLinkDataBaseClass is not the super class to NDPBaseClass
    '''   so we call LinkBaseClassFactory from NDPBaseClassFactory wrapper method
    ''' </remarks>
    Public Function NDPBaseClassFactory(ByVal source As String, Optional ByVal blnAlwaysCreateNew As Boolean = False) As NDPBaseClass
        Logger.Verbose("NDPBaseClassFactory - Source {source}, blnAlwaysCreateNew: {blnAlwaysCreateNew}", source, blnAlwaysCreateNew)
        Return LinkBaseClassFactory(source, blnAlwaysCreateNew)
    End Function

#End Region

#Region " Methods"
    Public MustOverride Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DTO.DTOBaseClass

    Public MustOverride Function GetRecordsFiltered() As DTO.DTOBaseClass()
    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter zero for the FKControl if a parent table key is not needed
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetFirstRecord(ByVal LowerControl As Int64, ByVal FKControl As Int64) As DTO.DTOBaseClass
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotSupported"}, New FaultReason("E_InvalidOperationException"))
    End Function

    ''' <summary>   
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter zero for the FKControl if a parent table key is not needed
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetPreviousRecord(ByVal CurrentControl As Int64, ByVal FKControl As Int64) As DTO.DTOBaseClass
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotSupported"}, New FaultReason("E_InvalidOperationException"))
    End Function
    ''' <summary> 
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter zero for the FKControl if a parent table key is not needed
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetNextRecord(ByVal CurrentControl As Int64, ByVal FKControl As Int64) As DTO.DTOBaseClass
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotSupported"}, New FaultReason("E_InvalidOperationException"))
    End Function
    ''' <summary> 
    ''' Enter zero as the UpperControl number return the record with the highest control PK 
    ''' Enter zero for the FKControl if a parent table key is not needed
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetLastRecord(ByVal UpperControl As Int64, ByVal FKControl As Int64) As DTO.DTOBaseClass
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotSupported"}, New FaultReason("E_InvalidOperationException"))
    End Function

    Protected MustOverride Function CopyDTOToLinq(ByVal oData As DTO.DTOBaseClass) As Object

    Protected MustOverride Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DTO.DTOBaseClass

    Protected Overridable Sub NoReturnCleanUp(ByVal LinqTable As Object)

    End Sub

    Protected Overridable Sub DeleteCleanUp(ByVal LinqTable As Object)

    End Sub

    Protected Overridable Sub CreateCleanUp(ByVal LinqTable As Object)

    End Sub

    Protected Overridable Function GetQuickSaveResults(ByVal LinqTable As Object) As DTO.QuickSaveResults
        Return Nothing
    End Function

    'Added By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Protected Overridable Sub PostUpdate(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        Return
    End Sub

    Protected Overridable Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)

    End Sub

    Protected Overridable Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)

    End Sub

    Protected Overridable Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)

    End Sub

    Protected Overridable Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)

    End Sub

    Protected Overridable Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)

    End Sub

    Protected Overridable Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)

    End Sub

    Public Overridable Function CreateRecord(ByVal oData As DTO.DTOBaseClass) As DTO.DTOBaseClass
        Dim results As DTO.DTOBaseClass
        Using Logger.StartActivity(Of DTO.DTOBaseClass)("CreateRecord(oData)", oData)
            results = Add(oData, LinqTable)
        End Using
        Return results
    End Function

    Public Overridable Function CreateRecordWithDetails(ByVal oData As DTO.DTOBaseClass) As DTO.DTOBaseClass
        Return AddWithDetails(oData, LinqTable)
    End Function

    Public Overridable Sub DeleteRecord(ByVal oData As DTO.DTOBaseClass)
        Delete(oData, LinqTable)
    End Sub

    Public Overridable Function UpdateRecord(ByVal oData As DTO.DTOBaseClass) As DTO.DTOBaseClass
        Return Update(oData, LinqTable)
    End Function

    Public Overridable Function UpdateRecordQuick(ByVal oData As DTO.DTOBaseClass) As DTO.QuickSaveResults
        Return UpdateQuick(oData, LinqTable)
    End Function

    Public Overridable Sub UpdateRecordNoReturn(ByVal oData As DTO.DTOBaseClass)
        UpdateNoReturn(oData, LinqTable)
    End Sub

    Public Overridable Function UpdateRecordWithDetails(ByVal oData As DTO.DTOBaseClass) As DTO.DTOBaseClass
        Return UpdateWithDetails(oData, LinqTable)
    End Function

    Public Overridable Sub UpdateRecordWithDetailsNoReturn(ByVal oData As DTO.DTOBaseClass)
        UpdateWithDetailsNoReturn(oData, LinqTable)
    End Sub

    Public Overridable Function Add(Of TEntity As Class)(ByVal oData As DTO.DTOBaseClass,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DTO.DTOBaseClass
        'Using LinqDB
        'Note: the ValidateData Function must throw a FaultException error on failure
        ValidateNewRecord(LinqDB, oData)
        'Create New Record 
        Dim nObject = CopyDTOToLinq(oData)

        'Use the table from THIS DataContext
        LinqDB.GetTable(Of TEntity)().InsertOnSubmit(nObject)

        'oLinqTable.InsertOnSubmit(nObject)

        Try
            LinqDB.SubmitChanges()
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
        CreateCleanUp(nObject)
        Return GetDTOUsingLinqTable(nObject)
        'End Using

    End Function

    Public Overridable Function AddWithDetails(Of TEntity As Class)(ByVal oData As DTO.DTOBaseClass,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DTO.DTOBaseClass
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateNewRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            AddDetailsToLinq(nObject, oData)
            oLinqTable.InsertOnSubmit(nObject)
            InsertAllDetails(LinqDB, nObject)
            Try
                LinqDB.SubmitChanges()
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
            Return GetDTOUsingLinqTable(nObject)
        End Using

    End Function

    Public Overridable Sub Delete(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateDeletedRecord(LinqDB, oData)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            oLinqTable.Attach(nObject, True)
            oLinqTable.DeleteOnSubmit(nObject)
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

    Public Overridable Function Update(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            'This method optionally performs any additional functions or cleanup needed after a save
            PostUpdate(LinqDB, oData)
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    Public Overridable Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DTO.QuickSaveResults
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            'This method optionally performs any additional functions or cleanup needed after a save
            PostUpdate(LinqDB, oData)
            ' Return the quick results object
            Return GetQuickSaveResults(nObject)
        End Using
    End Function

    Public Overridable Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)
            'Create New Record 
            Dim nObject = CopyDTOToLinq(oData)
            ' Attach the record 
            oLinqTable.Attach(nObject, True)
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            'This method optionally performs any additional functions or cleanup needed after a save
            PostUpdate(LinqDB, oData)

            NoReturnCleanUp(nObject)
        End Using
    End Sub

    Public Overridable Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            'This method optionally performs any additional functions or cleanup needed after a save
            PostUpdate(LinqDB, oData)
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    Public Overridable Sub UpdateWithDetailsNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                    ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Try
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
            'This method optionally performs any additional functions or cleanup needed after a save
            PostUpdate(LinqDB, oData)

            NoReturnCleanUp(nObject)
        End Using
    End Sub

    Protected Overridable Sub populateStateListsFromFilter(ByRef filters As DTO.LoadPlanningTruckDataFilter,
                                               ByRef OrigStates As List(Of String),
                                               ByRef blnUseOrigStateFilter As Boolean,
                                               ByRef DestStates As List(Of String),
                                               ByRef blnUseDestStateFilter As Boolean)

        If OrigStates Is Nothing Then OrigStates = New List(Of String)
        If DestStates Is Nothing Then DestStates = New List(Of String)

        If Not String.IsNullOrEmpty(filters.OrigSt1Filter.Trim) Then
            blnUseOrigStateFilter = True
            OrigStates.Add(filters.OrigSt1Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.OrigSt2Filter.Trim) Then
            blnUseOrigStateFilter = True
            OrigStates.Add(filters.OrigSt2Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.OrigSt3Filter.Trim) Then
            blnUseOrigStateFilter = True
            OrigStates.Add(filters.OrigSt3Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.OrigSt4Filter.Trim) Then
            blnUseOrigStateFilter = True
            OrigStates.Add(filters.OrigSt4Filter.Trim)
        End If


        If Not String.IsNullOrEmpty(filters.DestSt1Filter.Trim) Then
            blnUseDestStateFilter = True
            DestStates.Add(filters.DestSt1Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.DestSt2Filter.Trim) Then
            blnUseDestStateFilter = True
            DestStates.Add(filters.DestSt2Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.DestSt3Filter.Trim) Then
            blnUseDestStateFilter = True
            DestStates.Add(filters.DestSt3Filter.Trim)
        End If
        If Not String.IsNullOrEmpty(filters.DestSt4Filter.Trim) Then
            blnUseDestStateFilter = True
            DestStates.Add(filters.DestSt4Filter.Trim)
        End If
    End Sub

    ''' <summary>
    ''' creates a list of required Orig and Dest states from the AllFilters list for Booking
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="OrigStates"></param>
    ''' <param name="blnUseOrigStateFilter"></param>
    ''' <param name="DestStates"></param>
    ''' <param name="blnUseDestStateFilter"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.007 on 01/17/2023
    ''' </remarks>
    Protected Overridable Sub populateStateListsFromFilter(ByRef filters As Models.AllFilters,
                                               ByRef OrigStates As List(Of String),
                                               ByRef blnUseOrigStateFilter As Boolean,
                                               ByRef DestStates As List(Of String),
                                               ByRef blnUseDestStateFilter As Boolean,
                                               Optional ByVal sOrigStateFilterName As String = "BookOrigState2",
                                               Optional ByVal sDestStateFilterName As String = "BookDestState2")

        If OrigStates Is Nothing Then OrigStates = New List(Of String)
        If DestStates Is Nothing Then DestStates = New List(Of String)
        Dim NGLlookupDataObj = New NGLLookupDataProvider(Me.Parameters)
        Dim stateList = NGLlookupDataObj.GetViewLookupList("State", 1)
        Dim oFDetails As New Models.FilterDetails()
        If (filters.FilterValues.Any(Function(x) x.filterName = sOrigStateFilterName)) Then
            oFDetails = filters.FilterValues.Where(Function(x) x.filterName = sOrigStateFilterName).FirstOrDefault()
            If Not oFDetails Is Nothing Then
                If Not String.IsNullOrEmpty(oFDetails.filterValueFrom) Then
                    blnUseOrigStateFilter = True
                    OrigStates.Add(oFDetails.filterValueFrom)
                    If Not String.IsNullOrEmpty(oFDetails.filterValueTo) Then
                        If Not stateList Is Nothing AndAlso stateList.Count > 0 Then
                            For Each sState In stateList.Where(Function(x) x.Name > oFDetails.filterValueFrom And x.Name < oFDetails.filterValueTo)
                                OrigStates.Add(sState.Name)
                            Next
                        End If
                        OrigStates.Add(oFDetails.filterValueTo)
                    End If
                End If
            End If

        End If
        If (filters.FilterValues.Any(Function(x) x.filterName = sDestStateFilterName)) Then
            oFDetails = filters.FilterValues.Where(Function(x) x.filterName = sDestStateFilterName).FirstOrDefault()
            If Not oFDetails Is Nothing Then
                If Not String.IsNullOrEmpty(oFDetails.filterValueFrom) Then
                    blnUseDestStateFilter = True
                    DestStates.Add(oFDetails.filterValueFrom)
                    If Not String.IsNullOrEmpty(oFDetails.filterValueTo) Then
                        If Not stateList Is Nothing AndAlso stateList.Count > 0 Then
                            For Each sState In stateList.Where(Function(x) x.Name > oFDetails.filterValueFrom And x.Name < oFDetails.filterValueTo)
                                DestStates.Add(sState.Name)
                            Next
                        End If
                        DestStates.Add(oFDetails.filterValueTo)
                    End If
                End If
            End If

        End If


    End Sub

    Protected Shared Sub copyAllFilterDataToProperties(ByRef filters As Models.AllFilters,
                                               ByRef oFromProperty As String,
                                               ByRef oToProperty As String,
                                               ByVal sFilterName As String)


        Dim oFDetails As New Models.FilterDetails()
        If (filters.FilterValues.Any(Function(x) x.filterName = sFilterName)) Then
            oFDetails = filters.FilterValues.Where(Function(x) x.filterName = sFilterName).FirstOrDefault()
            oFromProperty = oFDetails.filterValueFrom
            oToProperty = oFDetails.filterValueTo
        End If


    End Sub

    Protected Shared Sub copyAllFilterDataToProperties(ByRef filters As Models.AllFilters,
                                              ByRef oFromProperty As Integer?,
                                              ByRef oToProperty As Integer?,
                                              ByVal sFilterName As String)

        Dim iValue As Integer = 0
        Dim oFDetails As New Models.FilterDetails()
        If (filters.FilterValues.Any(Function(x) x.filterName = sFilterName)) Then
            oFDetails = filters.FilterValues.Where(Function(x) x.filterName = sFilterName).FirstOrDefault()
            If Integer.TryParse(oFDetails.filterValueFrom, iValue) Then
                oFromProperty = iValue
                If Integer.TryParse(oFDetails.filterValueTo, iValue) Then
                    oToProperty = iValue
                End If
            End If
        End If


    End Sub


    Protected Shared Sub copyAllFilterDataToProperties(ByRef filters As Models.AllFilters,
                                              ByRef oFromProperty As Date?,
                                              ByRef oToProperty As Date?,
                                              ByVal sFilterName As String)


        Dim oFDetails As New Models.FilterDetails()
        If (filters.FilterValues.Any(Function(x) x.filterName = sFilterName)) Then
            oFDetails = filters.FilterValues.Where(Function(x) x.filterName = sFilterName).FirstOrDefault()
            If oFDetails.filterFrom.HasValue Then
                oFromProperty = oFDetails.filterFrom.Value
                If oFDetails.filterTo.HasValue Then
                    oToProperty = oFDetails.filterTo.Value
                End If
            End If
        End If


    End Sub


    'Protected Shared Function CopyMatchingFieldsOld(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String)) As Object
    '    If toObj Is Nothing Or fromObj Is Nothing Then
    '        Return Nothing
    '    End If

    '    Dim fromType As Type = fromObj.[GetType]()
    '    Dim toType As Type = toObj.[GetType]()

    '    ' Get all FieldInfo. 
    '    Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
    '    Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
    '    For Each fProp As PropertyInfo In fProps
    '        Dim propValue As Object = fProp.GetValue(fromObj)
    '        'Removed by RHR 10/8/14 did not update nullable fields when null
    '        'If propValue IsNot Nothing Then
    '        If Not skipObjs.Contains(fProp.Name) Then
    '            For Each tProp In tProps
    '                If tProp.Name = fProp.Name Then
    '                    If tProp.PropertyType() = fProp.PropertyType() Then
    '                        Try
    '                            tProp.SetValue(toObj, propValue)
    '                        Catch ex As Exception
    '                            Dim strMsg As String = ex.Message
    '                            Throw
    '                        End Try
    '                    End If
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '        'End If
    '    Next
    '    Return toObj

    'End Function


    'Protected Overridable Function CreateNewtblSolutionDetail(ByRef source As DTO.tblSolutionDetail, _
    '                                                       ByRef d As LTS.tblSolutionDetail, _
    '                                                       ByRef page As Integer, _
    '                                                       ByRef pageCount As Integer, _
    '                                                       ByRef PageSize As Integer) As DTO.tblSolutionDetail

    '    Return New DTO.tblSolutionDetail With {.SolutionDetailPOHdrControl = d.SolutionDetailPOHdrControl _
    '                    , .SolutionDetailProNumber = d.SolutionDetailProNumber _
    '                    , .SolutionDetailPONumber = d.SolutionDetailPONumber _
    '                    , .SolutionDetailOrderNumber = d.SolutionDetailOrderNumber _
    '                    , .SolutionDetailOrderSequence = d.SolutionDetailOrderSequence _
    '                    , .SolutionDetailCom = d.SolutionDetailCom _
    '                    , .SolutionDetailConsPrefix = d.SolutionDetailConsPrefix _
    '                    , .SolutionDetailCompControl = d.SolutionDetailCompControl _
    '                    , .SolutionDetailCompNumber = If(d.SolutionDetailCompNumber.HasValue, d.SolutionDetailCompNumber.Value, 0) _
    '                    , .SolutionDetailCompName = d.SolutionDetailCompName _
    '                    , .SolutionDetailCompNatNumber = d.SolutionDetailCompNatNumber _
    '                    , .SolutionDetailCompNatName = d.SolutionDetailCompNatName _
    '                    , .SolutionDetailODControl = d.SolutionDetailODControl _
    '                    , .SolutionDetailCarrierControl = d.SolutionDetailCarrierControl _
    '                    , .SolutionDetailCarrierNumber = d.SolutionDetailCarrierNumber _
    '                    , .SolutionDetailCarrierName = d.SolutionDetailCarrierName _
    '                    , .SolutionDetailOrigCompControl = d.SolutionDetailOrigCompControl _
    '                    , .SolutionDetailOrigName = d.SolutionDetailOrigName _
    '                    , .SolutionDetailOrigAddress1 = d.SolutionDetailOrigAddress1 _
    '                    , .SolutionDetailOrigAddress2 = d.SolutionDetailOrigAddress2 _
    '                    , .SolutionDetailOrigAddress3 = d.SolutionDetailOrigAddress3 _
    '                    , .SolutionDetailOrigCity = d.SolutionDetailOrigCity _
    '                    , .SolutionDetailOrigState = d.SolutionDetailOrigState _
    '                    , .SolutionDetailOrigCountry = d.SolutionDetailOrigCountry _
    '                    , .SolutionDetailOrigZip = d.SolutionDetailOrigZip _
    '                    , .SolutionDetailDestCompControl = d.SolutionDetailDestCompControl _
    '                    , .SolutionDetailDestName = d.SolutionDetailDestName _
    '                    , .SolutionDetailDestAddress1 = d.SolutionDetailDestAddress1 _
    '                    , .SolutionDetailDestAddress2 = d.SolutionDetailDestAddress2 _
    '                    , .SolutionDetailDestAddress3 = d.SolutionDetailDestAddress3 _
    '                    , .SolutionDetailDestCity = d.SolutionDetailDestCity _
    '                    , .SolutionDetailDestState = d.SolutionDetailDestState _
    '                    , .SolutionDetailDestCountry = d.SolutionDetailDestCountry _
    '                    , .SolutionDetailDestZip = d.SolutionDetailDestZip _
    '                    , .SolutionDetailDateOrdered = d.SolutionDetailDateOrdered _
    '                    , .SolutionDetailDateLoad = d.SolutionDetailDateLoad _
    '                    , .SolutionDetailDateRequired = d.SolutionDetailDateRequired _
    '                    , .SolutionDetailTotalCases = If(d.SolutionDetailTotalCases.HasValue, d.SolutionDetailTotalCases.Value, 0) _
    '                    , .SolutionDetailTotalWgt = If(d.SolutionDetailTotalWgt.HasValue, d.SolutionDetailTotalWgt.Value, 0) _
    '                    , .SolutionDetailTotalPL = If(d.SolutionDetailTotalPL.HasValue, d.SolutionDetailTotalPL.Value, 0) _
    '                    , .SolutionDetailTotalCube = If(d.SolutionDetailTotalCube.HasValue, d.SolutionDetailTotalCube.Value, 0) _
    '                    , .SolutionDetailTotalPX = d.SolutionDetailTotalPX _
    '                    , .SolutionDetailTotalBFC = If(d.SolutionDetailTotalBFC.HasValue, d.SolutionDetailTotalBFC.Value, 0) _
    '                    , .SolutionDetailTranCode = d.SolutionDetailTranCode _
    '                    , .SolutionDetailPayCode = d.SolutionDetailPayCode _
    '                    , .SolutionDetailTypeCode = d.SolutionDetailTypeCode _
    '                    , .SolutionDetailStopNo = d.SolutionDetailStopNo _
    '                    , .SolutionDetailMilesFrom = d.SolutionDetailMilesFrom _
    '                    , .SolutionDetailHoldLoad = d.SolutionDetailHoldLoad _
    '                    , .SolutionDetailTransType = d.SolutionDetailTransType _
    '                    , .SolutionDetailDateRequested = d.SolutionDetailDateRequested _
    '                    , .SolutionDetailCarrierEquipmentCodes = d.SolutionDetailCarrierEquipmentCodes _
    '                    , .SolutionDetailRouteTypeCode = If(d.SolutionDetailRouteTypeCode.HasValue, d.SolutionDetailRouteTypeCode.Value, 0) _
    '                    , .SolutionDetailModDate = d.SolutionDetailModDate _
    '                    , .SolutionDetailModUser = d.SolutionDetailModUser _
    '                    , .SolutionDetailUpdated = New Byte() {} _
    '                    , .Page = page _
    '                    , .Pages = pageCount _
    '                    , .PageSize = PageSize}


    'End Function



#End Region

End Class
