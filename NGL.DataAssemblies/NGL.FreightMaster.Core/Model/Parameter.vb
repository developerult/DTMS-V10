Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation

Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class Parameter : Inherits Ngl.Core.DirectDataObject

#Region "Class Variables and Properties"

        Private _oParameterTable As ApplicationData.ParameterDataTable = Nothing
        Public ReadOnly Property oParameterTable() As ApplicationData.ParameterDataTable
            Get
                If _oParameterTable Is Nothing Then
                    _oParameterTable = Me.GetData
                End If
                Return _oParameterTable
            End Get
        End Property


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

        Private _strName As String = "Parameter"
        Private _strKey As String = ""
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


        Private _Adapter As ParameterTableAdapter
        Protected ReadOnly Property Adapter() As ParameterTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New ParameterTableAdapter
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



        Public Function getParText(ByVal strKey As String, Optional ByVal intCompControl As Integer = 0) As String
            Dim strRet As String = "** ERROR **"
            Dim blnCompFound As Boolean = False
            Try
                If intCompControl > 0 Then
                    Dim oCompParameter As New CompParameter
                    oCompParameter.oUserConfiguration = Me.oUserConfiguration
                    blnCompFound = oCompParameter.getParText(strKey, intCompControl, strRet)
                End If
                If Not blnCompFound Then
                    Dim oRow As ApplicationData.ParameterRow = Me.oParameterTable.Rows.Find(strKey)
                    strRet = oRow.ParText
                End If

            Catch ex As Exception
                'In the case of an unhandled exception we simply return the default of "*** ERROR ***" while saving the last error message
                LastError = ex.Message
                'Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return strRet

        End Function

        Public Function getParValue(ByVal strKey As String, Optional ByVal intCompControl As Integer = 0) As Double
            Dim dblRet As Double = 0
            Dim blnCompFound As Boolean = False
            Try
                If intCompControl > 0 Then
                    Dim oCompParameter As New CompParameter(Me.oUserConfiguration)
                    blnCompFound =  oCompParameter.getParValue(strKey, intCompControl, dblRet)
                End If
                If Not blnCompFound Then
                    Dim oRow As ApplicationData.ParameterRow = Me.oParameterTable.Rows.Find(strKey)
                    dblRet = oRow.ParValue
                End If
            Catch ex As Exception
                'In the case of an unhandled exception we simply return the default of 0  while saving the last error message
                LastError = ex.Message
                'Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return dblRet

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
            Public Function GetData() As ApplicationData.ParameterDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByKey(ByVal strKey As String) As ApplicationData.ParameterDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByKey(strKey))

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetTaskParameters() As ApplicationData.ParameterDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetTaskParameters())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
            Public Function Add(ByVal ParKey As String, _
                ByVal ParText As String, _
                ByVal ParValue As Nullable(Of Double), _
                ByVal ParDescription As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                ''STEP 1: Execute Business Logic Rules
                If Not executeInsertBusinessRules(ParKey, _
                        ParText, _
                        ParValue, _
                        ParDescription) Then
                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Create a new record
                Dim oTable As New ApplicationData.ParameterDataTable
                Dim oRow As ApplicationData.ParameterRow = oTable.NewParameterRow

                ''STEP 3: Assign the new values to the row
                AssignValues(oRow, _
                        ParKey, _
                        ParText, _
                        ParValue, _
                        ParDescription)

                ''STEP 4: Add the new row to the table
                oTable.AddParameterRow(oRow)

                ''STEP 5: Update the table
                _intRowsAffected = Adapter.Update(oTable)

                ''STEP 6: Return true if precisely one row was inserted
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = "" 'TODO: Add error format function. String.Format(readWebConfig("ErrAddRecord", "Could not add a new {0} record, no database rows were affected."), _strName)
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
        Public Function Update(ByVal ParKey As String, _
                ByVal ParText As String, _
                ByVal ParValue As Nullable(Of Double), _
                ByVal ParDescription As String, _
                ByVal Original_ParText As String, _
                ByVal Original_ParValue As Nullable(Of Double), _
                ByVal Original_ParDescription As String, _
                ByVal Original_ParKey As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected primary key
                _strKey = Original_ParKey
                ''STEP 1: Execute Business Logic Rules
                If Not executeUpdateBusinessRules(ParKey, _
                        ParText, _
                        ParValue, _
                        ParDescription, _
                        Original_ParKey, _
                        Original_ParText, _
                        Original_ParValue, _
                        Original_ParDescription) Then

                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Read in the current database information
                Dim oTable As ApplicationData.ParameterDataTable = Adapter.GetDataByKey(Original_ParKey)

                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    Return False
                End If

                Dim oRow As ApplicationData.ParameterRow = oTable(0)

                ''STEP 3: Assign the original values to the row
                AssignOriginalValues(oRow, _
                        Original_ParKey, _
                        Original_ParText, _
                        Original_ParValue, _
                        Original_ParDescription)

                ''STEP 4: Accept the changes
                oRow.AcceptChanges()

                ''STEP 5: Assign the new values to the row
                AssignValues(oRow, _
                        ParKey, _
                        ParText, _
                        ParValue, _
                        ParDescription)



                'STEP 6: Update the record
                _intRowsAffected = Adapter.Update(oRow)
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = "" 'TODO: Add error format function. String.Format(readWebConfig("ErrSaveRecord", "Could not save your changes to the selected {0} record based on record id number {1}."), _strName, _strKey)
                    Throw New System.ApplicationException(LastError)
                End If
            Catch dbConcurEx As System.Data.DBConcurrencyException
                LastError = "" 'TODO: Add error format function. String.Format(readWebConfig("ErrDBConcurrency", "The selected {0} record has been modified by another user since you starting editing.  Your changes have been replaced by the current values.  Please review the existing values and make any changes needed."), _strName)

                Throw New System.ApplicationException(LastError, dbConcurEx.InnerException)
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If Me.Debug Then LastError = ex.ToString Else LastError = ex.Message
                Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet
        End Function

        Public Function Update(ByVal oTable As ApplicationData.ParameterDataTable) As Boolean
            Dim blnRet As Boolean = False
            Try
                Dim changes As ApplicationData.ParameterDataTable = oTable.GetChanges()


                For Each oRow As ApplicationData.ParameterRow In changes
                    If Not Me.executeUpdateBusinessRules(oRow) Then
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
        Public Function Delete(ByVal Original_ParText As String, _
            ByVal Original_ParValue As Nullable(Of Double), _
            ByVal Original_ParDescription As String, _
            ByVal Original_ParKey As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                
                'save the currently selected primary key
                _strKey = Original_ParKey
                ''STEP 1: Execute Business Logic Rules
                If Not executeDeleteBusinessRules(Original_ParKey, _
                        Original_ParText, _
                        Original_ParValue, _
                        Original_ParDescription) Then

                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                _intRowsAffected = Adapter.Delete(Original_ParKey, _
                        Original_ParText, _
                        Original_ParValue, _
                        Original_ParDescription)
                If _intRowsAffected = 1 Then
                    Return True
                Else
                    LastError = "Cannot Delete" 'TODO: Add error format function. String.Format(readWebConfig("ErrDeleteRecord", "Could not delete the selected {0} record based on record id number {1}."), _strName, _strKey)
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

        Public Function validateKey(ByVal strKey As String) As Boolean
            Dim blnRet As Boolean = False
            Try

                Dim oTable As ApplicationData.ParameterDataTable = Adapter.GetDataByKey(strKey)

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

        Public Sub AssignValues(ByRef oRow As ApplicationData.ParameterRow, _
            ByVal ParKey As String, _
            ByVal ParText As String, _
            ByVal ParValue As Nullable(Of Double), _
            ByVal ParDescription As String)


            With oRow
                .ParKey = CleanNullableString(ParKey, 50, "The Parameter Key Is Not Valid; it cannot be empty and must be at lease 4 characters long.", 4) 'TODO: Add message string format
                If ParText Is Nothing Then .SetParTextNull() Else .ParText = CleanNullableString(ParText, 255)
                If Not ParValue.HasValue Then .SetParValueNull() Else .ParValue = ParValue.Value
                If ParDescription Is Nothing Then .SetParDescriptionNull() Else .ParDescription = ParDescription
            End With

        End Sub

        Protected Sub AssignOriginalValues(ByRef oRow As ApplicationData.ParameterRow, _
            ByVal ParKey As String, _
            ByVal ParText As String, _
            ByVal ParValue As Nullable(Of Double), _
            ByVal ParDescription As String)
            _strUnmatched = ""

            With oRow

                If ParKey Is Nothing Then
                    Throw New System.ApplicationException("The Parameter Key cannot be empty.") 'TODO: Add error format
                    Return
                Else
                    If .ParKey <> ParKey Then
                        _strUnmatched &= "Parameter Key"
                        .ParKey = ParKey
                    End If
                End If

                If ParText Is Nothing Then
                    If Not .IsParTextNull Then
                        _strUnmatched &= "Parameter Text"
                    End If
                Else
                    If .ParText <> ParText Then
                        _strUnmatched &= "Parameter Text"
                        .ParText = ParText
                    End If
                End If

                If Not ParValue.HasValue Then
                    If Not .IsParValueNull Then
                        _strUnmatched &= "Parameter Value"
                    End If
                Else
                    If .ParValue <> ParValue.Value Then
                        _strUnmatched &= "Parameter Value"
                        .ParValue = ParValue.Value
                    End If
                End If
                If ParDescription Is Nothing Then
                    If Not .IsParDescriptionNull Then
                        _strUnmatched &= "Parameter Description"
                    End If
                Else
                    If .ParDescription <> ParDescription Then
                        _strUnmatched &= "Parameter Description"
                        .ParDescription = ParDescription
                    End If
                End If
            End With
        End Sub

        Private Function executeUpdateBusinessRules(ByRef oRow As ApplicationData.ParameterRow) As Boolean
            Dim ParKey As String = oRow.ParKey
            Dim ParText As String = oRow.ParText
            Dim ParValue As Nullable(Of Double) = oRow.ParValue
            Dim ParDescription As String = oRow.ParDescription
            If Not executeUpdateBusinessRules(ParKey, _
                ParText, _
                ParValue, _
                ParDescription, _
                oRow.Item("ParText", DataRowVersion.Original), _
                oRow.Item("ParValue", DataRowVersion.Original), _
                oRow.Item("ParDescription", DataRowVersion.Original), _
                oRow.Item("ParKey", DataRowVersion.Original)) Then
                Return False
            End If
            AssignValues(oRow, ParKey, ParText, ParValue, ParDescription)
            Return True


        End Function
        ''' <summary>
        ''' Executes the required business rules before an update can be performed
        ''' </summary>
        ''' <param name="ParKey"></param>
        ''' <param name="ParText"></param>
        ''' <param name="ParValue"></param>
        ''' <param name="ParDescription"></param>
        ''' <param name="Original_ParText"></param>
        ''' <param name="Original_ParValue"></param>
        ''' <param name="Original_ParDescription"></param>
        ''' <param name="Original_ParKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeUpdateBusinessRules(ByRef ParKey As String, _
                ByRef ParText As String, _
                ByRef ParValue As Nullable(Of Double), _
                ByRef ParDescription As String, _
                ByVal Original_ParText As String, _
                ByVal Original_ParValue As Nullable(Of Double), _
                ByVal Original_ParDescription As String, _
                ByVal Original_ParKey As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If ParKey.Trim <> Original_ParKey.Trim Then
                    LastError = "Parameter Key Has Changed" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format
            End Try
            Return blnRet


        End Function
        ''' <summary>
        ''' Executes the required business rules before an insert can be performed
        ''' </summary>
        ''' <param name="ParKey"></param>
        ''' <param name="ParText"></param>
        ''' <param name="ParValue"></param>
        ''' <param name="ParDescription"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeInsertBusinessRules(ByRef ParKey As String, _
                ByRef ParText As String, _
                ByRef ParValue As Nullable(Of Double), _
                ByRef ParDescription As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If ParKey Is Nothing OrElse ParKey.Trim.Length < 4 Then
                    LastError = "The Parameter Key Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If validateKey(ParKey) Then
                    LastError = "New Parameter Key Already Exists" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet
        End Function
        ''' <summary>
        ''' Executes the required business rules before a delete can be performed
        ''' </summary>
        ''' <param name="Original_ParText"></param>
        ''' <param name="Original_ParValue"></param>
        ''' <param name="Original_ParDescription"></param>
        ''' <param name="Original_ParKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeDeleteBusinessRules(ByVal Original_ParText As String, _
            ByVal Original_ParValue As Nullable(Of Double), _
            ByVal Original_ParDescription As String, _
            ByVal Original_ParKey As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If Original_ParKey Is Nothing OrElse Original_ParKey.Trim.Length < 4 Then
                    LastError = "The Parameter Key Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If

                If Not validateKey(Original_ParKey) Then
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "

                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet

        End Function

#End Region

    End Class

End Namespace