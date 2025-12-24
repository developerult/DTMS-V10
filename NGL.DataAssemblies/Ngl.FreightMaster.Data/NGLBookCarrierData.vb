Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Public Class NGLBookCarrierData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vBookCarriers
        Me.LinqDB = db
        Me.SourceClass = "NGLBookCarrierData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vBookCarriers
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
        Return GetBookCarrierFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetBookCarrierFiltered(ByVal Control As Integer) As DataTransferObjects.BookCarrier
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oBookCarrier As DataTransferObjects.BookCarrier = (
                        From d In db.vBookCarriers
                        Where
                        d.BookControl = Control
                        Select selectDTOData(d, db)).First



                Return oBookCarrier

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

    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                                                          ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        SaveChanges(oData)
        ' Return the updated order
        Return GetBookCarrierFiltered(Control:=oData.BookControl)

    End Function

    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                                             ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        SaveChanges(oData)
    End Sub


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookCarrier)
        'Create New Record
        Return New LTS.vBookCarrier With {.BookControl = d.BookControl _
            , .BookCarrFBNumber = d.BookCarrFBNumber _
            , .BookCarrOrderNumber = d.BookCarrOrderNumber _
            , .BookCarrBLNumber = d.BookCarrBLNumber _
            , .BookCarrBookDate = d.BookCarrBookDate _
            , .BookCarrBookTime = d.BookCarrBookTime _
            , .BookCarrBookContact = d.BookCarrBookContact _
            , .BookCarrScheduleDate = d.BookCarrScheduleDate _
            , .BookCarrScheduleTime = d.BookCarrScheduleTime _
            , .BookCarrActualDate = d.BookCarrActualDate _
            , .BookCarrActualTime = d.BookCarrActualTime _
            , .BookCarrActLoadComplete_Date = d.BookCarrActLoadComplete_Date _
            , .BookCarrActLoadCompleteTime = d.BookCarrActLoadCompleteTime _
            , .BookCarrDockPUAssigment = d.BookCarrDockPUAssigment _
            , .BookCarrPODate = d.BookCarrPODate _
            , .BookCarrPOTime = d.BookCarrPOTime _
            , .BookCarrApptDate = d.BookCarrApptDate _
            , .BookCarrApptTime = d.BookCarrApptTime _
            , .BookCarrActDate = d.BookCarrActDate _
            , .BookCarrActTime = d.BookCarrActTime _
            , .BookCarrActUnloadCompDate = d.BookCarrActUnloadCompDate _
            , .BookCarrActUnloadCompTime = d.BookCarrActUnloadCompTime _
            , .BookCarrDockDelAssignment = d.BookCarrDockDelAssignment _
            , .BookCarrVarDay = d.BookCarrVarDay _
            , .BookCarrVarHrs = d.BookCarrVarHrs _
            , .BookCarrTrailerNo = d.BookCarrTrailerNo _
            , .BookCarrSealNo = d.BookCarrSealNo _
            , .BookCarrDriverNo = d.BookCarrDriverNo _
            , .BookCarrDriverName = d.BookCarrDriverName _
            , .BookCarrRouteNo = d.BookCarrRouteNo _
            , .BookCarrTripNo = d.BookCarrTripNo _
            , .BookModDate = Date.Now _
            , .BookModUser = Me.Parameters.UserName _
            , .BookWhseAuthorizationNo = d.BookWhseAuthorizationNo _
            , .BookCarrStartLoadingDate = d.BookCarrStartLoadingDate _
            , .BookCarrStartLoadingTime = d.BookCarrStartLoadingTime _
            , .BookCarrFinishLoadingDate = d.BookCarrFinishLoadingDate _
            , .BookCarrFinishLoadingTime = d.BookCarrFinishLoadingTime _
            , .BookCarrStartUnloadingDate = d.BookCarrStartUnloadingDate _
            , .BookCarrStartUnloadingTime = d.BookCarrStartUnloadingTime _
            , .BookCarrFinishUnloadingDate = d.BookCarrFinishUnloadingDate _
            , .BookCarrFinishUnloadingTime = d.BookCarrFinishUnloadingTime _
            , .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl _
            , .BookAMSPickupApptControl = d.BookAMSPickupApptControl _
            , .BookFinAPActWgt = d.BookFinAPActWgt}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetRecordFiltered(Control:=CType(LinqTable, LTS.vBookCarrier).BookControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be inserted from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.0 on 10/25/2016
    '''   added logic to update dependency data after save so appointment data
    '''   for similar locations are updated based on parameter settings
    ''' </remarks>
    Private Sub SaveChanges(ByVal oData As DataTransferObjects.BookCarrier)
        Dim intBookControl As Integer = 0
        Using LinqDB
            'Note: the ValidateData Function must throw a FaultException error on failure
            ValidateUpdatedRecord(LinqDB, oData)

            With oData
                Try
                    intBookControl = .BookControl
                    'Open the existing Record
                    Dim d = (From e In CType(LinqDB, NGLMasBookDataContext).vBookCarriers Where e.BookControl = .BookControl Select e).First
                    If d Is Nothing Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                    Else
                        'Check for conflicts
                        'Modified by RHR for v-8.5.2.006 on 12/29/2022 check for seconds because D365 does not save milliseconds
                        Dim iSeconds = DateDiff(DateInterval.Second, .BookModDate.Value, d.BookModDate.Value)
                        If iSeconds > 0 Then
                            'the data may have changed so check each field for conflicts
                            Dim ConflictData As New List(Of KeyValuePair(Of String, String))
                            Dim blnConflictFound As Boolean = False
                            addToConflicts("BookCarrFBNumber", .BookCarrFBNumber, d.BookCarrFBNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrOrderNumber", .BookCarrOrderNumber, d.BookCarrOrderNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBLNumber", .BookCarrBLNumber, d.BookCarrBLNumber, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBookDate", .BookCarrBookDate, d.BookCarrBookDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBookTime", .BookCarrBookTime, d.BookCarrBookTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrBookContact", .BookCarrBookContact, d.BookCarrBookContact, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrScheduleDate", .BookCarrScheduleDate, d.BookCarrScheduleDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrScheduleTime", .BookCarrScheduleTime, d.BookCarrScheduleTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActualDate", .BookCarrActualDate, d.BookCarrActualDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActualTime", .BookCarrActualTime, d.BookCarrActualTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActLoadComplete_Date", .BookCarrActLoadComplete_Date, d.BookCarrActLoadComplete_Date, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActLoadCompleteTime", .BookCarrActLoadCompleteTime, d.BookCarrActLoadCompleteTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrDockPUAssigment", .BookCarrDockPUAssigment, d.BookCarrDockPUAssigment, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrPODate", .BookCarrPODate, d.BookCarrPODate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrPOTime", .BookCarrPOTime, d.BookCarrPOTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrApptDate", .BookCarrApptDate, d.BookCarrApptDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrApptTime", .BookCarrApptTime, d.BookCarrApptTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActDate", .BookCarrActDate, d.BookCarrActDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActTime", .BookCarrActTime, d.BookCarrActTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActUnloadCompDate", .BookCarrActUnloadCompDate, d.BookCarrActUnloadCompDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrActUnloadCompTime", .BookCarrActUnloadCompTime, d.BookCarrActUnloadCompTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrDockDelAssignment", .BookCarrDockDelAssignment, d.BookCarrDockDelAssignment, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrVarDay", .BookCarrVarDay, If(d.BookCarrVarDay.HasValue, d.BookCarrVarDay, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookCarrVarHrs", .BookCarrVarHrs, If(d.BookCarrVarHrs.HasValue, d.BookCarrVarHrs, 0), ConflictData, blnConflictFound)
                            addToConflicts("BookCarrTrailerNo", .BookCarrTrailerNo, d.BookCarrTrailerNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrSealNo", .BookCarrSealNo, d.BookCarrSealNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrSealNo", .BookCarrSealNo, d.BookCarrSealNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrDriverNo", .BookCarrDriverNo, d.BookCarrDriverNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrDriverName", .BookCarrDriverName, d.BookCarrDriverName, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrRouteNo", .BookCarrRouteNo, d.BookCarrRouteNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrTripNo", .BookCarrTripNo, d.BookCarrTripNo, ConflictData, blnConflictFound)
                            addToConflicts("BookWhseAuthorizationNo", .BookWhseAuthorizationNo, d.BookWhseAuthorizationNo, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrStartLoadingDate", .BookCarrStartLoadingDate, d.BookCarrStartLoadingDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrStartLoadingTime", .BookCarrStartLoadingTime, d.BookCarrStartLoadingTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrFinishLoadingDate", .BookCarrFinishLoadingDate, d.BookCarrFinishLoadingDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrFinishLoadingTime", .BookCarrFinishLoadingTime, d.BookCarrFinishLoadingTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrStartUnloadingDate", .BookCarrStartUnloadingDate, d.BookCarrStartUnloadingDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrStartUnloadingTime", .BookCarrStartUnloadingTime, d.BookCarrStartUnloadingTime, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrFinishUnloadingDate", .BookCarrFinishUnloadingDate, d.BookCarrFinishUnloadingDate, ConflictData, blnConflictFound)
                            addToConflicts("BookCarrFinishUnloadingTime", .BookCarrFinishUnloadingTime, d.BookCarrFinishUnloadingTime, ConflictData, blnConflictFound)
                            addToConflicts("BookFinAPActWgt", .BookFinAPActWgt, d.BookFinAPActWgt, ConflictData, blnConflictFound)
                            If blnConflictFound Then
                                'We only add the mod date and mod user if one or more other fields have been modified
                                addToConflicts("BookModDate", .BookModDate, d.BookModDate, ConflictData, blnConflictFound)
                                addToConflicts("BookModUser", .BookModUser, d.BookModUser, ConflictData, blnConflictFound)
                                Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(ConflictData))
                                conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                                Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                            End If
                        End If
                        'Update the table data
                        d.BookCarrFBNumber = .BookCarrFBNumber
                        d.BookCarrOrderNumber = .BookCarrOrderNumber
                        d.BookCarrBLNumber = .BookCarrBLNumber
                        d.BookCarrBookDate = .BookCarrBookDate
                        d.BookCarrBookTime = .BookCarrBookTime
                        d.BookCarrBookContact = .BookCarrBookContact
                        d.BookCarrScheduleDate = .BookCarrScheduleDate
                        d.BookCarrScheduleTime = .BookCarrScheduleTime
                        d.BookCarrActualDate = .BookCarrActualDate
                        d.BookCarrActualTime = .BookCarrActualTime
                        d.BookCarrActLoadComplete_Date = .BookCarrActLoadComplete_Date
                        d.BookCarrActLoadCompleteTime = .BookCarrActLoadCompleteTime
                        d.BookCarrDockPUAssigment = .BookCarrDockPUAssigment
                        d.BookCarrPODate = .BookCarrPODate
                        d.BookCarrPOTime = .BookCarrPOTime
                        d.BookCarrApptDate = .BookCarrApptDate
                        d.BookCarrApptTime = .BookCarrApptTime
                        d.BookCarrActDate = .BookCarrActDate
                        d.BookCarrActTime = .BookCarrActTime
                        d.BookCarrActUnloadCompDate = .BookCarrActUnloadCompDate
                        d.BookCarrActUnloadCompTime = .BookCarrActUnloadCompTime
                        d.BookCarrDockDelAssignment = .BookCarrDockDelAssignment
                        d.BookCarrVarDay = .BookCarrVarDay
                        d.BookCarrVarHrs = .BookCarrVarHrs
                        d.BookCarrTrailerNo = .BookCarrTrailerNo
                        d.BookCarrSealNo = .BookCarrSealNo
                        d.BookCarrDriverNo = .BookCarrDriverNo
                        d.BookCarrDriverName = .BookCarrDriverName
                        d.BookCarrRouteNo = .BookCarrRouteNo
                        d.BookCarrTripNo = .BookCarrTripNo
                        d.BookModDate = Date.Now
                        d.BookModUser = Me.Parameters.UserName
                        d.BookWhseAuthorizationNo = .BookWhseAuthorizationNo
                        d.BookCarrStartLoadingDate = .BookCarrStartLoadingDate
                        d.BookCarrStartLoadingTime = .BookCarrStartLoadingTime
                        d.BookCarrFinishLoadingDate = .BookCarrFinishLoadingDate
                        d.BookCarrFinishLoadingTime = .BookCarrFinishLoadingTime
                        d.BookCarrStartUnloadingDate = .BookCarrStartUnloadingDate
                        d.BookCarrStartUnloadingTime = .BookCarrStartUnloadingTime
                        d.BookCarrFinishUnloadingDate = .BookCarrFinishUnloadingDate
                        d.BookCarrFinishUnloadingTime = .BookCarrFinishUnloadingTime
                        d.BookFinAPActWgt = .BookFinAPActWgt
                    End If
                    LinqDB.SubmitChanges()
                Catch ex As FaultException
                    Throw
                Catch ex As SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
                Catch conflictEx As ChangeConflictException
                    Try
                        Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                        conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                        Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                    Catch ex As FaultException
                        Throw
                    Catch ex As Exception
                        Utilities.SaveAppError(ex.Message, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                    End Try
                Catch ex As InvalidOperationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_RecordDeleted"}, New FaultReason("E_InvalidOperationException"))
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
                'Modified by RHR for v-7.0.6.0 on 10/25/2016
                If intBookControl <> 0 Then
                    DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookDependencies(intBookControl, 0)
                End If
            End With
        End Using
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.vBookCarrier, ByRef db As NGLMasBookDataContext) As DataTransferObjects.BookCarrier

        Return New DataTransferObjects.BookCarrier With {.BookControl = d.BookControl _
            , .BookCarrFBNumber = d.BookCarrFBNumber _
            , .BookCarrOrderNumber = d.BookCarrOrderNumber _
            , .BookCarrBLNumber = d.BookCarrBLNumber _
            , .BookCarrBookDate = d.BookCarrBookDate _
            , .BookCarrBookTime = d.BookCarrBookTime _
            , .BookCarrBookContact = d.BookCarrBookContact _
            , .BookCarrScheduleDate = d.BookCarrScheduleDate _
            , .BookCarrScheduleTime = d.BookCarrScheduleTime _
            , .BookCarrActualDate = d.BookCarrActualDate _
            , .BookCarrActualTime = d.BookCarrActualTime _
            , .BookCarrActLoadComplete_Date = d.BookCarrActLoadComplete_Date _
            , .BookCarrActLoadCompleteTime = d.BookCarrActLoadCompleteTime _
            , .BookCarrDockPUAssigment = d.BookCarrDockPUAssigment _
            , .BookCarrPODate = d.BookCarrPODate _
            , .BookCarrPOTime = d.BookCarrPOTime _
            , .BookCarrApptDate = d.BookCarrApptDate _
            , .BookCarrApptTime = d.BookCarrApptTime _
            , .BookCarrActDate = d.BookCarrActDate _
            , .BookCarrActTime = d.BookCarrActTime _
            , .BookCarrActUnloadCompDate = d.BookCarrActUnloadCompDate _
            , .BookCarrActUnloadCompTime = d.BookCarrActUnloadCompTime _
            , .BookCarrDockDelAssignment = d.BookCarrDockDelAssignment _
            , .BookCarrVarDay = If(d.BookCarrVarDay.HasValue, d.BookCarrVarDay, 0) _
            , .BookCarrVarHrs = If(d.BookCarrVarHrs.HasValue, d.BookCarrVarHrs, 0) _
            , .BookCarrTrailerNo = d.BookCarrTrailerNo _
            , .BookCarrSealNo = d.BookCarrSealNo _
            , .BookCarrDriverNo = d.BookCarrDriverNo _
            , .BookCarrDriverName = d.BookCarrDriverName _
            , .BookCarrRouteNo = d.BookCarrRouteNo _
            , .BookCarrTripNo = d.BookCarrTripNo _
            , .BookModDate = d.BookModDate _
            , .BookModUser = d.BookModUser _
            , .BookWhseAuthorizationNo = d.BookWhseAuthorizationNo _
            , .BookCarrStartLoadingDate = d.BookCarrStartLoadingDate _
            , .BookCarrStartLoadingTime = d.BookCarrStartLoadingTime _
            , .BookCarrFinishLoadingDate = d.BookCarrFinishLoadingDate _
            , .BookCarrFinishLoadingTime = d.BookCarrFinishLoadingTime _
            , .BookCarrStartUnloadingDate = d.BookCarrStartUnloadingDate _
            , .BookCarrStartUnloadingTime = d.BookCarrStartUnloadingTime _
            , .BookCarrFinishUnloadingDate = d.BookCarrFinishUnloadingDate _
            , .BookCarrFinishUnloadingTime = d.BookCarrFinishUnloadingTime _
            , .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl _
            , .BookAMSPickupApptControl = d.BookAMSPickupApptControl _
            , .BookFinAPActWgt = If(d.BookFinAPActWgt.HasValue, d.BookFinAPActWgt, 0)}
    End Function


#End Region

End Class