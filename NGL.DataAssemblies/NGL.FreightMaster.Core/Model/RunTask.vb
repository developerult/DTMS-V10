
Imports Ngl.FreightMaster.Core.UserConfiguration
Imports Ngl.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation

Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class RunTask : Inherits Ngl.Core.DirectDataObject
#Region "Class Variables and Properties"
        Private _oUserConfiguration As UserConfiguration = Nothing
        Public Property oUserConfiguration() As UserConfiguration
            Get
                If _oUserConfiguration Is Nothing Then
                    _oUserConfiguration = New UserConfiguration
                End If
                Return _oUserConfiguration
            End Get
            Set(ByVal value As UserConfiguration)
                _oUserConfiguration = value
            End Set
        End Property

        Private _strUnmatched As String = ""
        Public ReadOnly Property Unmatched() As String
            Get
                Return _strUnmatched

            End Get
        End Property

        Private _intRowsAffected As Integer = 0
        Public ReadOnly Property RowsAffected() As Integer
            Get
                Return _intRowsAffected
            End Get
        End Property

        Private _strName As String = "RunTask"
        Private _intControl As Integer = 0
        Private _intTimeOut As Integer = 0

        Protected Property CommandTimeOut() As Integer
            Get
                If _intTimeOut < 100 Then
                    _intTimeOut = Me.oUserConfiguration.ShortTimeOut
                End If
                Return _intTimeOut
            End Get
            Set(ByVal value As Integer)
                _intTimeOut = value
            End Set
        End Property


        Private _Adapter As tblRunTaskTableAdapter
        Protected ReadOnly Property Adapter() As tblRunTaskTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New tblRunTaskTableAdapter
                    _Adapter.SetConnectionString(ConnectionString)
                End If

                Return _Adapter
            End Get
        End Property
#End Region


#Region "Constructors"

        Public Sub New()
            MyBase.new()
        End Sub

        Public Sub New(ByVal oConfig As UserConfiguration)
            MyBase.new()
            Me.oUserConfiguration = oConfig
            With oConfig
                Me.Debug = .Debug
                Me.Database = .Database
                Me.Server = .DBServer
                Me.Source = .Source
                Me.ConnectionString = .ConnectionString
            End With
        End Sub


#End Region

#Region "Functions"

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetData() As ApplicationData.tblRunTaskDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())

        End Function


        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetActive() As ApplicationData.tblRunTaskDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetActive())

        End Function


        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetAvailable() As ApplicationData.tblRunTaskDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetAvailable())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
            Public Function Add(ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As Global.System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskLastError As String, _
                    ByVal RunTaskInstructions As String, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean
            Dim blnRet As Boolean = False
            Try
                ''STEP 1: Execute Business Logic Rules
                If Not executeInsertBusinessRules( _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn) Then
                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Create a new record
                Dim oTable As New ApplicationData.tblRunTaskDataTable
                Dim oRow As ApplicationData.tblRunTaskRow = oTable.NewtblRunTaskRow

                ''STEP 3: Assign the new values to the row
                AssignValues(oRow, _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn)

                ''STEP 4: Add the new row to the table
                oTable.AddtblRunTaskRow(oRow)

                ''STEP 5: Update the table
                _intRowsAffected = Adapter.Update(oTable)

                ''STEP 6: Return true if precisely one row was inserted
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = "Unexpected Error!  Could not add new record, no database rows were inserted." 'TODO: Add error format function. String.Format(readWebConfig("ErrAddRecord", "Could not add a new {0} record, no database rows were affected."), _strName)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function Update( _
                    ByVal RunTaskControl As Integer, _
                    ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As Global.System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskLastError As String, _
                    ByVal RunTaskInstructions As String, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected primary key
                _intControl = RunTaskControl
                ''STEP 1: Execute Business Logic Rules
                If Not executeUpdateBusinessRules( _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn) Then
                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Read in the current database information
                Dim oTable As ApplicationData.tblRunTaskDataTable = Adapter.GetDataByControl(_intControl)

                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    Return False
                End If

                Dim oRow As ApplicationData.tblRunTaskRow = oTable(0)

                ''STEP 3: Assign the new values to the row
                AssignValues(oRow, _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn)

                'STEP 4: Update the record
                _intRowsAffected = Adapter.Update(oRow)
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = String.Format("Could not save your changes to the selected {0} record based on control number {1}.", _strName, _intControl) 'TODO: Add error format function. String.Format(readWebConfig("ErrSaveRecord", "Could not save your changes to the selected {0} record based on record id number {1}."), _strName, _intControl)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch dbConcurEx As System.Data.DBConcurrencyException
                LastError = String.Format("The selected {0} record has been modified by another user since you starting editing.  Your changes have been replaced by the current values.  Please review the existing values and make any changes needed.", _strName) 'TODO: Add error format function. String.Format(readWebConfig("ErrDBConcurrency", "The selected {0} record has been modified by another user since you starting editing.  Your changes have been replaced by the current values.  Please review the existing values and make any changes needed."), _strName)
                Throw New System.ApplicationException(LastError, dbConcurEx.InnerException)
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function



        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function Update(ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As Global.System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskLastError As String, _
                    ByVal RunTaskInstructions As String, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date), _
                    ByVal Original_RunTaskControl As Integer, _
                    ByVal Original_RunTaskName As String, _
                    ByVal Original_RunTaskDescription As String, _
                    ByVal Original_RunTaskType As Integer, _
                    ByVal Original_RunTaskCommand As String, _
                    ByVal Original_RunTaskMinutes As Integer, _
                    ByVal Original_RunTaskHours As Integer, _
                    ByVal Original_RunTaskDays As Integer, _
                    ByVal Original_RunTaskMonths As Integer, _
                    ByVal Original_RunTaskWeekDays As Integer, _
                    ByVal Original_RunTaskModUser As String, _
                    ByVal Original_RunTaskModDate As System.Nullable(Of Date), _
                    ByVal Original_RunTaskEnabled As Boolean, _
                    ByVal Original_RunTaskHold As Boolean, _
                    ByVal Original_RunTaskRunning As Boolean, _
                    ByVal Original_RunTaskAppPath As String, _
                    ByVal Original_RunTaskRunNow As Boolean, _
                    ByVal Original_RunTaskUserName As String, _
                    ByVal Original_RunTaskPassword As String, _
                    ByVal Original_RunTaskIniKey As String, _
                    ByVal Original_RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal Original_RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal Original_RunTaskHadErrors As Boolean, _
                    ByVal Original_RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected primary key
                _intControl = Original_RunTaskControl
                ''STEP 1: Execute Business Logic Rules
                If Not executeUpdateBusinessRules( _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn) Then
                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Read in the current database information
                Dim oTable As ApplicationData.tblRunTaskDataTable = Adapter.GetDataByControl(_intControl)

                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    Return False
                End If

                Dim oRow As ApplicationData.tblRunTaskRow = oTable(0)

                ''STEP 3: Assign the original values to the row
                AssignOriginalValues(oRow, _
                    Original_RunTaskName, _
                    Original_RunTaskDescription, _
                    Original_RunTaskType, _
                    Original_RunTaskCommand, _
                    Original_RunTaskMinutes, _
                    Original_RunTaskHours, _
                    Original_RunTaskDays, _
                    Original_RunTaskMonths, _
                    Original_RunTaskWeekDays, _
                    Original_RunTaskModUser, _
                    Original_RunTaskModDate, _
                    Original_RunTaskEnabled, _
                    Original_RunTaskHold, _
                    Original_RunTaskRunning, _
                    Original_RunTaskAppPath, _
                    Original_RunTaskRunNow, _
                    Original_RunTaskUserName, _
                    Original_RunTaskPassword, _
                    Original_RunTaskIniKey, _
                    Original_RunTaskLastRanOn, _
                    Original_RunTaskLastEndOn, _
                    Original_RunTaskHadErrors, _
                    Original_RunTaskNextRunOn)

                ''STEP 4: Accept the changes
                oRow.AcceptChanges()

                ''STEP 5: Assign the new values to the row
                AssignValues(oRow, _
                    RunTaskName, _
                    RunTaskDescription, _
                    RunTaskType, _
                    RunTaskCommand, _
                    RunTaskMinutes, _
                    RunTaskHours, _
                    RunTaskDays, _
                    RunTaskMonths, _
                    RunTaskWeekDays, _
                    RunTaskModUser, _
                    RunTaskModDate, _
                    RunTaskEnabled, _
                    RunTaskHold, _
                    RunTaskRunning, _
                    RunTaskAppPath, _
                    RunTaskRunNow, _
                    RunTaskUserName, _
                    RunTaskPassword, _
                    RunTaskIniKey, _
                    RunTaskLastRanOn, _
                    RunTaskLastEndOn, _
                    RunTaskHadErrors, _
                    RunTaskLastError, _
                    RunTaskInstructions, _
                    RunTaskNextRunOn)



                'STEP 6: Update the record
                _intRowsAffected = Adapter.Update(oRow)
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = String.Format("Could not save your changes to the selected {0} record based on control number {1}.", _strName, _intControl) 'TODO: Add error format function. String.Format(readWebConfig("ErrSaveRecord", "Could not save your changes to the selected {0} record based on record id number {1}."), _strName, _intControl)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch dbConcurEx As System.Data.DBConcurrencyException
                LastError = String.Format("The selected {0} record has been modified by another user since you starting editing.  Your changes have been replaced by the current values.  Please review the existing values and make any changes needed.", _strName) 'TODO: Add error format function. String.Format(readWebConfig("ErrDBConcurrency", "The selected {0} record has been modified by another user since you starting editing.  Your changes have been replaced by the current values.  Please review the existing values and make any changes needed."), _strName)

                Throw New System.ApplicationException(LastError, dbConcurEx.InnerException)
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function

        Public Function Update(ByVal oTable As ApplicationData.tblRunTaskDataTable) As Boolean
            Dim blnRet As Boolean = False
            Try
                Dim changes As ApplicationData.tblRunTaskDataTable = oTable.GetChanges()


                For Each oRow As ApplicationData.tblRunTaskRow In changes
                    'save the currently selected primary key
                    _intControl = oRow.RunTaskControl
                    If Not Me.executeUpdateBusinessRules( _
                                                        oRow.RunTaskName, _
                                                        oRow.RunTaskDescription, _
                                                        oRow.RunTaskType, _
                                                        oRow.RunTaskCommand, _
                                                        oRow.RunTaskMinutes, _
                                                        oRow.RunTaskHours, _
                                                        oRow.RunTaskDays, _
                                                        oRow.RunTaskMonths, _
                                                        oRow.RunTaskWeekDays, _
                                                        oRow.RunTaskModUser, _
                                                        oRow.RunTaskModDate, _
                                                        oRow.RunTaskEnabled, _
                                                        oRow.RunTaskHold, _
                                                        oRow.RunTaskRunning, _
                                                        oRow.RunTaskAppPath, _
                                                        oRow.RunTaskRunNow, _
                                                        oRow.RunTaskUserName, _
                                                        oRow.RunTaskPassword, _
                                                        oRow.RunTaskIniKey, _
                                                        oRow.RunTaskLastRanOn, _
                                                        oRow.RunTaskLastEndOn, _
                                                        oRow.RunTaskHadErrors, _
                                                        oRow.RunTaskLastError, _
                                                        oRow.RunTaskInstructions, _
                                                        oRow.RunTaskNextRunOn) Then
                        Throw New System.ApplicationException(LastError)
                        Return False
                    End If
                Next

                _intRowsAffected = Adapter.Update(changes)
                Return _intRowsAffected = changes.Rows.Count

            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function Delete(ByVal RunTaskControl As Integer) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected record by key 
                _intControl = RunTaskControl

                ''STEP 1: Get the data table                
                Dim oTable As ApplicationData.tblRunTaskDataTable = Adapter.GetDataByControl(_intControl)
                If oTable.Count = 0 Then
                    Return False
                End If
                Dim oRow As ApplicationData.tblRunTaskRow = oTable(0)
                With oRow
                    Dim Original_RunTaskModDate As System.Nullable(Of Date)
                    If Not .IsRunTaskModDateNull Then
                        Original_RunTaskModDate = .RunTaskModDate
                    End If

                    _intRowsAffected = Adapter.Delete( _
                                        .RunTaskControl, _
                                        .RunTaskName, _
                                        .RunTaskDescription, _
                                        .RunTaskType, _
                                        .RunTaskCommand, _
                                        .RunTaskMinutes, _
                                        .RunTaskHours, _
                                        .RunTaskDays, _
                                        .RunTaskMonths, _
                                        .RunTaskWeekDays, _
                                        .RunTaskModUser, _
                                        Original_RunTaskModDate, _
                                        .RunTaskEnabled, _
                                        .RunTaskHold, _
                                        .RunTaskRunning, _
                                        .RunTaskAppPath, _
                                        .RunTaskRunNow, _
                                        .RunTaskUserName, _
                                        .RunTaskPassword, _
                                        .RunTaskIniKey, _
                                        .RunTaskLastRanOn, _
                                        .RunTaskLastEndOn, _
                                        .RunTaskHadErrors, _
                                        .RunTaskNextRunOn)
                End With
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = String.Format("Could not delete the selected {0} record based on record id number {1}.", _strName, _intControl) 'TODO: Add error format function. String.Format(readWebConfig("ErrDeleteRecord", "Could not delete the selected {0} record based on record id number {1}."), _strName, _strKey)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function


        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function Delete( _
                    ByVal Original_RunTaskControl As Integer, _
                    ByVal Original_RunTaskName As String, _
                    ByVal Original_RunTaskDescription As String, _
                    ByVal Original_RunTaskType As Integer, _
                    ByVal Original_RunTaskCommand As String, _
                    ByVal Original_RunTaskMinutes As Integer, _
                    ByVal Original_RunTaskHours As Integer, _
                    ByVal Original_RunTaskDays As Integer, _
                    ByVal Original_RunTaskMonths As Integer, _
                    ByVal Original_RunTaskWeekDays As Integer, _
                    ByVal Original_RunTaskModUser As String, _
                    ByVal Original_RunTaskModDate As System.Nullable(Of Date), _
                    ByVal Original_RunTaskEnabled As Boolean, _
                    ByVal Original_RunTaskHold As Boolean, _
                    ByVal Original_RunTaskRunning As Boolean, _
                    ByVal Original_RunTaskAppPath As String, _
                    ByVal Original_RunTaskRunNow As Boolean, _
                    ByVal Original_RunTaskUserName As String, _
                    ByVal Original_RunTaskPassword As String, _
                    ByVal Original_RunTaskIniKey As String, _
                    ByVal Original_RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal Original_RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal Original_RunTaskHadErrors As Boolean, _
                    ByVal Original_RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected record by key 
                _intControl = Original_RunTaskControl
                
                ''STEP 1: Execute Business Logic Rules
                If Not executeDeleteBusinessRules(_intControl) Then

                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                _intRowsAffected = Adapter.Delete( _
                                    Original_RunTaskControl, _
                                    Original_RunTaskName, _
                                    Original_RunTaskDescription, _
                                    Original_RunTaskType, _
                                    Original_RunTaskCommand, _
                                    Original_RunTaskMinutes, _
                                    Original_RunTaskHours, _
                                    Original_RunTaskDays, _
                                    Original_RunTaskMonths, _
                                    Original_RunTaskWeekDays, _
                                    Original_RunTaskModUser, _
                                    Original_RunTaskModDate, _
                                    Original_RunTaskEnabled, _
                                    Original_RunTaskHold, _
                                    Original_RunTaskRunning, _
                                    Original_RunTaskAppPath, _
                                    Original_RunTaskRunNow, _
                                    Original_RunTaskUserName, _
                                    Original_RunTaskPassword, _
                                    Original_RunTaskIniKey, _
                                    Original_RunTaskLastRanOn, _
                                    Original_RunTaskLastEndOn, _
                                    Original_RunTaskHadErrors, _
                                    Original_RunTaskNextRunOn)
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = String.Format("Could not delete the selected {0} record based on record id number {1}.", _strName, _intControl) 'TODO: Add error format function. String.Format(readWebConfig("ErrDeleteRecord", "Could not delete the selected {0} record based on record id number {1}."), _strName, _strKey)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function

        Private Sub AssignValues(ByRef oRow As ApplicationData.tblRunTaskRow, _
                    ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskLastError As String, _
                    ByVal RunTaskInstructions As String, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date))


            With oRow
                .RunTaskName = CleanNullableString(RunTaskName, 50, "The Task Name Is Not Valid; it cannot be empty and must be at lease 4 characters long.", 4) 'TODO: Add message string format
                .RunTaskDescription = CleanNullableString(RunTaskDescription, 255, "The Task Description Is Not Valid; it cannot be empty and must be at lease 4 characters long.", 4) 'TODO: Add message string format
                .RunTaskType = RunTaskType
                .RunTaskCommand = CleanNullableString(RunTaskCommand, 255, "The Task Command Is Not Valid; it cannot be empty and must be at lease 4 characters long.", 4) 'TODO: Add message string format
                .RunTaskMinutes = RunTaskMinutes
                .RunTaskHours = RunTaskHours
                .RunTaskDays = RunTaskDays
                .RunTaskMonths = RunTaskMonths
                .RunTaskWeekDays = RunTaskWeekDays
                .RunTaskModUser = RunTaskModUser
                If Not RunTaskModDate.HasValue Then .SetRunTaskModDateNull() Else .RunTaskModDate = RunTaskModDate.Value
                .RunTaskEnabled = RunTaskEnabled
                .RunTaskHold = RunTaskHold
                .RunTaskRunning = RunTaskRunning
                If String.IsNullOrEmpty(RunTaskAppPath) Then .SetRunTaskAppPathNull() Else .RunTaskAppPath = CleanNullableString(RunTaskAppPath, 255)
                .RunTaskRunNow = RunTaskRunNow
                If String.IsNullOrEmpty(RunTaskUserName) Then .SetRunTaskUserNameNull() Else .RunTaskUserName = CleanNullableString(RunTaskUserName, 50)
                If String.IsNullOrEmpty(RunTaskPassword) Then .SetRunTaskPasswordNull() Else .RunTaskPassword = CleanNullableString(RunTaskPassword, 50)
                If String.IsNullOrEmpty(RunTaskIniKey) Then .SetRunTaskIniKeyNull() Else .RunTaskIniKey = CleanNullableString(RunTaskIniKey, 20)
                If Not RunTaskLastRanOn.HasValue Then .SetRunTaskLastRanOnNull() Else .RunTaskLastRanOn = RunTaskLastRanOn.Value
                If Not RunTaskLastEndOn.HasValue Then .SetRunTaskLastEndOnNull() Else .RunTaskLastEndOn = RunTaskLastEndOn.Value
                .RunTaskHadErrors = RunTaskHadErrors
                If String.IsNullOrEmpty(RunTaskLastError) Then .SetRunTaskLastErrorNull() Else .RunTaskLastError = RunTaskLastError
                If String.IsNullOrEmpty(RunTaskInstructions) Then .SetRunTaskInstructionsNull() Else .RunTaskInstructions = RunTaskInstructions
                If Not RunTaskNextRunOn.HasValue Then .SetRunTaskNextRunOnNull() Else .RunTaskNextRunOn = RunTaskNextRunOn.Value

            End With

        End Sub

        Private Function executeInsertBusinessRules( _
                    ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskLastError As String, _
                    ByVal RunTaskInstructions As String, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean



            Dim blnRet As Boolean = False
            Try
                If String.IsNullOrEmpty(RunTaskName) OrElse RunTaskName.Trim.Length < 4 Then
                    LastError = "The Task Name Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If String.IsNullOrEmpty(RunTaskDescription) OrElse RunTaskDescription.Trim.Length < 4 Then
                    LastError = "The Task Description Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If RunTaskType < 0 Or RunTaskType > 1 Then
                    LastError = "The Task Type Is Not Valid; it must have a value of 1 or zero." 'TODO: Add error string format
                    Return False
                End If
                If String.IsNullOrEmpty(RunTaskCommand) OrElse RunTaskCommand.Trim.Length < 4 Then
                    LastError = "The Task Command Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If validateKey(RunTaskName, RunTaskCommand) Then
                    LastError = "New Task Name or Command Already Exists" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet
        End Function



        Public Function validateKey(ByVal TaskName As String, ByVal TaskCommand As String, Optional ByVal TaskControl As Integer = 0) As Boolean
            Dim blnRet As Boolean = False
            Try

                Dim oTable As ApplicationData.tblRunTaskDataTable
                If TaskControl > 0 Then
                    oTable = Adapter.GetByTaskNameOrTaskCommandNotSelected(TaskName, TaskCommand, TaskControl)
                Else
                    oTable = Adapter.GetByTaskNameOrTaskCommand(TaskName, TaskCommand)
                End If

                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    LastError = "No Key Found" 'TODO: Add error format function. String.Format(readWebConfig("ErrValidateID", "The {0} id, {1}, does not reference a valid {0} record."), _strName, ID)
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
            End Try
            Return blnRet
        End Function



        Private Function executeUpdateBusinessRules( _
                    ByRef RunTaskName As String, _
                    ByRef RunTaskDescription As String, _
                    ByRef RunTaskType As Integer, _
                    ByRef RunTaskCommand As String, _
                    ByRef RunTaskMinutes As Integer, _
                    ByRef RunTaskHours As Integer, _
                    ByRef RunTaskDays As Integer, _
                    ByRef RunTaskMonths As Integer, _
                    ByRef RunTaskWeekDays As Integer, _
                    ByRef RunTaskModUser As String, _
                    ByRef RunTaskModDate As Global.System.Nullable(Of Date), _
                    ByRef RunTaskEnabled As Boolean, _
                    ByRef RunTaskHold As Boolean, _
                    ByRef RunTaskRunning As Boolean, _
                    ByRef RunTaskAppPath As String, _
                    ByRef RunTaskRunNow As Boolean, _
                    ByRef RunTaskUserName As String, _
                    ByRef RunTaskPassword As String, _
                    ByRef RunTaskIniKey As String, _
                    ByRef RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByRef RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByRef RunTaskHadErrors As Boolean, _
                    ByRef RunTaskLastError As String, _
                    ByRef RunTaskInstructions As String, _
                    ByRef RunTaskNextRunOn As System.Nullable(Of Date)) As Boolean
            Dim blnRet As Boolean = False
            Try
                If String.IsNullOrEmpty(RunTaskName) OrElse RunTaskName.Trim.Length < 4 Then
                    LastError = "The Task Name Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If String.IsNullOrEmpty(RunTaskDescription) OrElse RunTaskDescription.Trim.Length < 4 Then
                    LastError = "The Task Description Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If RunTaskType < 0 Or RunTaskType > 1 Then
                    LastError = "The Task Type Is Not Valid; it must have a value of 1 or zero." 'TODO: Add error string format
                    Return False
                End If
                If String.IsNullOrEmpty(RunTaskCommand) OrElse RunTaskCommand.Trim.Length < 4 Then
                    LastError = "The Task Command Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If validateKey(RunTaskName, RunTaskCommand, _intControl) Then
                    LastError = "The Task Name or Command Already Exists" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the run task business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format
            End Try
            Return blnRet

        End Function
        ''' <summary>
        ''' This procedure is used to populate original values from the consumer
        ''' When we do not care if the server copy has changed.  Typically this is used
        ''' to force an overwrite of the server data with the data from the consumer.
        ''' The Unmatched property is used to store the data that was changed and may be used
        ''' to notify the consumer of what was over written.  This cannot be undone.
        ''' </summary>
        ''' <param name="oRow"></param>
        ''' <param name="RunTaskName"></param>
        ''' <param name="RunTaskDescription"></param>
        ''' <param name="RunTaskType"></param>
        ''' <param name="RunTaskCommand"></param>
        ''' <param name="RunTaskMinutes"></param>
        ''' <param name="RunTaskHours"></param>
        ''' <param name="RunTaskDays"></param>
        ''' <param name="RunTaskMonths"></param>
        ''' <param name="RunTaskWeekDays"></param>
        ''' <param name="RunTaskModUser"></param>
        ''' <param name="RunTaskModDate"></param>
        ''' <param name="RunTaskEnabled"></param>
        ''' <param name="RunTaskHold"></param>
        ''' <param name="RunTaskRunning"></param>
        ''' <param name="RunTaskAppPath"></param>
        ''' <param name="RunTaskRunNow"></param>
        ''' <param name="RunTaskUserName"></param>
        ''' <param name="RunTaskPassword"></param>
        ''' <param name="RunTaskIniKey"></param>
        ''' <param name="RunTaskLastRanOn"></param>
        ''' <param name="RunTaskLastEndOn"></param>
        ''' <param name="RunTaskHadErrors"></param>
        ''' <param name="RunTaskNextRunOn"></param>
        ''' <remarks></remarks>
        Protected Sub AssignOriginalValues(ByRef oRow As ApplicationData.tblRunTaskRow, _
                    ByVal RunTaskName As String, _
                    ByVal RunTaskDescription As String, _
                    ByVal RunTaskType As Integer, _
                    ByVal RunTaskCommand As String, _
                    ByVal RunTaskMinutes As Integer, _
                    ByVal RunTaskHours As Integer, _
                    ByVal RunTaskDays As Integer, _
                    ByVal RunTaskMonths As Integer, _
                    ByVal RunTaskWeekDays As Integer, _
                    ByVal RunTaskModUser As String, _
                    ByVal RunTaskModDate As System.Nullable(Of Date), _
                    ByVal RunTaskEnabled As Boolean, _
                    ByVal RunTaskHold As Boolean, _
                    ByVal RunTaskRunning As Boolean, _
                    ByVal RunTaskAppPath As String, _
                    ByVal RunTaskRunNow As Boolean, _
                    ByVal RunTaskUserName As String, _
                    ByVal RunTaskPassword As String, _
                    ByVal RunTaskIniKey As String, _
                    ByVal RunTaskLastRanOn As System.Nullable(Of Date), _
                    ByVal RunTaskLastEndOn As System.Nullable(Of Date), _
                    ByVal RunTaskHadErrors As Boolean, _
                    ByVal RunTaskNextRunOn As System.Nullable(Of Date))
            _strUnmatched = ""

            With oRow
                If String.IsNullOrEmpty(RunTaskName) Then
                    Throw New System.ApplicationException("The Task Name cannot be empty.") 'TODO: Add error format
                    Return
                Else
                    If .RunTaskName <> RunTaskName Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Name", CleanNullableString(.RunTaskName)) 'TODO: Add error format
                        .RunTaskName = RunTaskName
                    End If
                End If
                If String.IsNullOrEmpty(RunTaskDescription) Then
                    Throw New System.ApplicationException("The Task Description cannot be empty.") 'TODO: Add error format
                    Return
                Else
                    If .RunTaskDescription <> RunTaskDescription Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Description", CleanNullableString(.RunTaskDescription)) 'TODO: Add error format
                        .RunTaskDescription = RunTaskDescription
                    End If
                End If
                .RunTaskType = RunTaskType
                If String.IsNullOrEmpty(RunTaskCommand) Then
                    Throw New System.ApplicationException("The Task Command cannot be empty.") 'TODO: Add error format
                    Return
                Else
                    If .RunTaskCommand <> RunTaskCommand Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Description", CleanNullableString(.RunTaskDescription)) 'TODO: Add error format
                        .RunTaskCommand = RunTaskCommand
                    End If
                End If
                If .RunTaskMinutes <> RunTaskMinutes Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Minutes", CleanNullableString(.RunTaskMinutes)) 'TODO: Add error format
                    .RunTaskMinutes = RunTaskMinutes
                End If
                If .RunTaskHours <> RunTaskHours Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Hours", CleanNullableString(.RunTaskHours)) 'TODO: Add error format
                    .RunTaskHours = RunTaskHours
                End If
                If .RunTaskDays <> RunTaskDays Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Days", CleanNullableString(.RunTaskDays)) 'TODO: Add error format
                    .RunTaskDays = RunTaskDays
                End If
                If .RunTaskMonths <> RunTaskMonths Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Months", CleanNullableString(.RunTaskMonths)) 'TODO: Add error format
                    .RunTaskMonths = RunTaskMonths
                End If
                If .RunTaskWeekDays <> RunTaskWeekDays Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Week Days", CleanNullableString(.RunTaskWeekDays)) 'TODO: Add error format
                    .RunTaskWeekDays = RunTaskWeekDays
                End If
                If Not String.IsNullOrEmpty(RunTaskModUser) Then .RunTaskModUser = RunTaskModUser
                If Not RunTaskModDate.HasValue Then
                    .SetRunTaskModDateNull()
                Else
                    .RunTaskModDate = RunTaskModDate.Value
                End If
                If .RunTaskEnabled <> RunTaskEnabled Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Enabled", CleanNullableString(.RunTaskEnabled)) 'TODO: Add error format
                    .RunTaskEnabled = RunTaskEnabled
                End If
                If .RunTaskHold <> RunTaskHold Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Hold", CleanNullableString(.RunTaskHold)) 'TODO: Add error format
                    .RunTaskHold = RunTaskHold
                End If
                If .RunTaskRunning <> RunTaskRunning Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Running", CleanNullableString(.RunTaskRunning)) 'TODO: Add error format
                    .RunTaskRunning = RunTaskRunning
                End If
                If String.IsNullOrEmpty(RunTaskAppPath) Then
                    If Not .IsRunTaskAppPathNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Server's Application Path", CleanNullableString(.RunTaskAppPath)) 'TODO: Add error format
                        .SetRunTaskAppPathNull()
                    End If
                Else
                    If .RunTaskAppPath <> RunTaskAppPath Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Server's Application Path", CleanNullableString(.RunTaskAppPath)) 'TODO: Add error format
                        .RunTaskAppPath = RunTaskAppPath
                    End If
                End If
                If .RunTaskRunNow <> RunTaskRunNow Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Run Now", CleanNullableString(.RunTaskRunNow)) 'TODO: Add error format
                    .RunTaskRunNow = RunTaskRunNow
                End If
                If String.IsNullOrEmpty(RunTaskUserName) Then
                    If Not .IsRunTaskUserNameNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task User Name", CleanNullableString(.RunTaskUserName)) 'TODO: Add error format
                        .SetRunTaskUserNameNull()
                    End If
                Else
                    If .RunTaskUserName <> RunTaskUserName Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task User Name", CleanNullableString(.RunTaskUserName)) 'TODO: Add error format
                        .RunTaskUserName = RunTaskUserName
                    End If
                End If
                If String.IsNullOrEmpty(RunTaskPassword) Then
                    If Not .IsRunTaskPasswordNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Password", CleanNullableString(.RunTaskPassword)) 'TODO: Add error format
                        .SetRunTaskPasswordNull()
                    End If
                Else
                    If .RunTaskPassword <> RunTaskPassword Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Password", CleanNullableString(.RunTaskPassword)) 'TODO: Add error format
                        .RunTaskPassword = RunTaskPassword
                    End If
                End If
                If String.IsNullOrEmpty(RunTaskIniKey) Then
                    If Not .IsRunTaskIniKeyNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task INI File Key", CleanNullableString(.RunTaskIniKey)) 'TODO: Add error format
                        .SetRunTaskIniKeyNull()
                    End If
                Else
                    If .RunTaskIniKey <> RunTaskIniKey Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task INI File Key", CleanNullableString(.RunTaskIniKey)) 'TODO: Add error format
                        .RunTaskIniKey = RunTaskIniKey
                    End If
                End If
                If Not RunTaskLastRanOn.HasValue Then
                    If Not .IsRunTaskLastRanOnNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Last Ran On", CleanNullableString(.RunTaskLastRanOn)) 'TODO: Add error format
                        .SetRunTaskLastRanOnNull()
                    End If
                Else
                    If .RunTaskLastRanOn <> RunTaskLastRanOn.Value Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Last Ran On", CleanNullableString(.RunTaskLastRanOn)) 'TODO: Add error format
                        .RunTaskLastRanOn = RunTaskLastRanOn.Value
                    End If
                End If
                If Not RunTaskLastEndOn.HasValue Then
                    If Not .IsRunTaskLastRanOnNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Last End On", CleanNullableString(.RunTaskLastEndOn)) 'TODO: Add error format
                        .SetRunTaskLastRanOnNull()
                    End If
                Else
                    If .RunTaskLastEndOn <> RunTaskLastEndOn.Value Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Last End On", CleanNullableString(.RunTaskLastEndOn)) 'TODO: Add error format
                        .RunTaskLastEndOn = RunTaskLastEndOn.Value
                    End If
                End If
                If .RunTaskHadErrors <> RunTaskHadErrors Then
                    _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Had Errors", CleanNullableString(.RunTaskHadErrors)) 'TODO: Add error format
                    .RunTaskHadErrors = RunTaskHadErrors
                End If
                If Not RunTaskNextRunOn.HasValue Then
                    If Not .IsRunTaskNextRunOnNull Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Next Run On", CleanNullableString(.RunTaskNextRunOn)) 'TODO: Add error format
                        .SetRunTaskNextRunOnNull()
                    End If
                Else
                    If .RunTaskNextRunOn <> RunTaskNextRunOn.Value Then
                        _strUnmatched &= String.Format("The {0} value {1} has been replaced.", "Task Next Run On", CleanNullableString(.RunTaskNextRunOn)) 'TODO: Add error format
                        .RunTaskNextRunOn = RunTaskNextRunOn.Value
                    End If
                End If
            End With
        End Sub

        Private Function executeDeleteBusinessRules(ByVal intControl As Integer) As Boolean
            Dim blnRet As Boolean = False
            Try

                Dim oTable As ApplicationData.tblRunTaskDataTable = Adapter.GetDataByControl(intControl)
                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    LastError = String.Format("The {0} id, {1}, does not reference a valid {0} record.", _strName, intControl) 'TODO: Add error format function. String.Format(readWebConfig("ErrValidateID", "The {0} id, {1}, does not reference a valid {0} record."), _strName, ID)
                    blnRet = False
                Else
                    blnRet = True
                End If
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the delete task manager record business rules could not be executed. Your changes could not be saved at this time! The actual error is: "

                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet

        End Function

#End Region

    End Class

End Namespace
