Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation

Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class CompParameter : Inherits Ngl.Core.DirectDataObject


#Region "Class Variables and Properties"

        Private _oCompParameterTable As ApplicationData.CompParameterDataTable = Nothing
        Public ReadOnly Property oCompParameterTable() As ApplicationData.CompParameterDataTable
            Get
                If _oCompParameterTable Is Nothing Then
                    _oCompParameterTable = Me.GetData
                End If
                Return _oCompParameterTable
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

        Private _strName As String = "CompParameter"
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


        Private _Adapter As CompParameterTableAdapter
        Protected ReadOnly Property Adapter() As CompParameterTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New CompParameterTableAdapter
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



        Public Function getParText(ByVal strKey As String, ByVal intCompControl As Integer, ByRef strRet As String) As Boolean
            Dim blnRet As Boolean = False
            'set the default return value to error
            strRet = "** ERROR **"
            Try
                Dim oTable As ApplicationData.CompParameterDataTable = Adapter.GetDataByKeyAndComp(strKey, intCompControl)

                If oTable.Count > 0 Then
                    Dim oRow As ApplicationData.CompParameterRow = oTable(0)
                    strRet = oRow.CompParText
                    blnRet = True
                End If
            Catch ex As Exception
                'In the case of an unhandled exception we simply return the default of  false but we save the error message
                LastError = ex.Message
                'Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet

        End Function

        Public Function getParValue(ByVal strKey As String, ByVal intCompControl As Integer, ByRef dblRet As Double) As Boolean
            Dim blnRet As Boolean = False
            Try
                Dim oTable As ApplicationData.CompParameterDataTable = Adapter.GetDataByKeyAndComp(strKey, intCompControl)

                If oTable.Count > 0 Then
                    Dim oRow As ApplicationData.CompParameterRow = oTable(0)
                    dblRet = oRow.CompParValue
                    blnRet = True
                End If


            Catch ex As Exception
                'In the case of an unhandled exception we simply return the default of false but we save the message
                LastError = ex.Message
                'Throw New System.ApplicationException(LastError, ex.InnerException)
            End Try
            Return blnRet

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
            Public Function GetData() As ApplicationData.CompParameterDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByKeyAndComp(ByVal strKey As String, ByVal intCompControl As Integer) As ApplicationData.CompParameterDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByKeyAndComp(strKey, intCompControl))

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
            Public Function Add(ByVal CompParCompControl As Integer, _
                ByVal CompParKey As String, _
                ByVal CompParText As String, _
                ByVal CompParValue As Nullable(Of Double), _
                ByVal CompParDescription As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                ''STEP 1: Execute Business Logic Rules
                If Not executeInsertBusinessRules(CompParCompControl, _
                        CompParKey, _
                        CompParText, _
                        CompParValue, _
                        CompParDescription) Then
                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Create a new record
                Dim oTable As New ApplicationData.CompParameterDataTable
                Dim oRow As ApplicationData.CompParameterRow = oTable.NewCompParameterRow

                ''STEP 3: Assign the new values to the row
                AssignValues(oRow, _
                        CompParCompControl, _
                        CompParKey, _
                        CompParText, _
                        CompParValue, _
                        CompParDescription)

                ''STEP 4: Add the new row to the table
                oTable.AddCompParameterRow(oRow)

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
        Public Function Update(ByVal CompParCompControl As Integer, _
                            ByVal CompParKey As String, _
                            ByVal CompParText As String, _
                            ByVal CompParValue As Global.System.Nullable(Of Double), _
                            ByVal CompParDescription As String, _
                            ByVal Original_CompParCompControl As Integer, _
                            ByVal Original_CompParKey As String, _
                            ByVal Original_CompParText As String, _
                            ByVal Original_CompParValue As Global.System.Nullable(Of Double), _
                            ByVal Original_CompParDescription As String, _
                            ByVal Original_CompParControl As Integer)


            Dim blnRet As Boolean = False
            Try
                'save the currently selected primary key
                _strKey = Original_CompParKey
                ''STEP 1: Execute Business Logic Rules
                If Not executeUpdateBusinessRules(CompParCompControl, _
                        CompParKey, _
                        CompParText, _
                        CompParValue, _
                        CompParDescription, _
                        Original_CompParCompControl, _
                        Original_CompParKey, _
                        Original_CompParText, _
                        Original_CompParValue, _
                        Original_CompParDescription, _
                        Original_CompParControl) Then

                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                ''STEP 2: Read in the current database information
                Dim oTable As ApplicationData.CompParameterDataTable = Adapter.GetDataByKeyAndComp(Original_CompParKey, Original_CompParCompControl)

                If oTable.Count = 0 Then
                    ' no matching record found, return false
                    Return False
                End If

                Dim oRow As ApplicationData.CompParameterRow = oTable(0)

                ''STEP 3: Assign the original values to the row
                AssignOriginalValues(oRow, _
                        Original_CompParCompControl, _
                        Original_CompParKey, _
                        Original_CompParText, _
                        Original_CompParValue, _
                        Original_CompParDescription)

                ''STEP 4: Accept the changes
                oRow.AcceptChanges()

                ''STEP 5: Assign the new values to the row
                AssignValues(oRow, _
                        CompParCompControl, _
                        CompParKey, _
                        CompParText, _
                        CompParValue, _
                        CompParDescription)



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

        Public Function Update(ByVal oTable As ApplicationData.CompParameterDataTable) As Boolean
            Dim blnRet As Boolean = False
            Try
                Dim changes As ApplicationData.CompParameterDataTable = oTable.GetChanges()


                For Each oRow As ApplicationData.CompParameterRow In changes
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
        Public Function Delete(ByVal Original_CompParCompControl As Integer, _
            ByVal Original_CompParKey As String, _
            ByVal Original_CompParText As String, _
            ByVal Original_CompParValue As Nullable(Of Double), _
            ByVal Original_CompParDescription As String, _
            ByVal Original_CompParControl As Integer) As Boolean
            Dim blnRet As Boolean = False
            Try
                'save the currently selected record by key 
                _strKey = Original_CompParKey
                ''save the currently selected primary key
                '_strKey = Original_ParKey
                ''STEP 1: Execute Business Logic Rules
                If Not executeDeleteBusinessRules(Original_CompParCompControl, _
                        Original_CompParKey, _
                        Original_CompParText, _
                        Original_CompParValue, _
                        Original_CompParDescription, _
                        Original_CompParControl) Then

                    Throw New System.ApplicationException(LastError)
                    Return False
                End If
                _intRowsAffected = Adapter.Delete(Original_CompParControl, _
                        Original_CompParCompControl, _
                        Original_CompParKey, _
                        Original_CompParText, _
                        Original_CompParValue, _
                        Original_CompParDescription)
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

        Public Function validateKey(ByVal strKey As String, ByVal intCompControl As Integer) As Boolean
            Dim blnRet As Boolean = False
            Try

                Dim oTable As ApplicationData.CompParameterDataTable = Adapter.GetDataByKeyAndComp(strKey, intCompControl)

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

        Public Sub AssignValues(ByRef oRow As ApplicationData.CompParameterRow, _
            ByVal CompParCompControl As Integer, _
            ByVal CompParKey As String, _
            ByVal CompParText As String, _
            ByVal CompParValue As Nullable(Of Double), _
            ByVal CompParDescription As String)


            With oRow
                .CompParCompControl = CompParCompControl
                .CompParKey = CleanNullableString(CompParKey, 50, "The Company Parameter Key Is Not Valid; it cannot be empty and must be at lease 4 characters long.", 4) 'TODO: Add message string format
                If CompParText Is Nothing Then .SetCompParTextNull() Else .CompParText = CleanNullableString(CompParText, 255)
                If Not CompParValue.HasValue Then .SetCompParValueNull() Else .CompParValue = CompParValue.Value
                If CompParDescription Is Nothing Then .SetCompParDescriptionNull() Else .CompParDescription = CompParDescription
            End With

        End Sub

        Protected Sub AssignOriginalValues(ByRef oRow As ApplicationData.CompParameterRow, _
            ByVal Original_CompParCompControl As Integer, _
            ByVal Original_CompParKey As String, _
            ByVal Original_CompParText As String, _
            ByVal Original_CompParValue As Nullable(Of Double), _
            ByVal Original_CompParDescription As String)
            _strUnmatched = ""

            With oRow

                If Original_CompParKey Is Nothing Then
                    Throw New System.ApplicationException("The Company Parameter Key cannot be empty.") 'TODO: Add error format
                    Return
                Else
                    If .CompParCompControl <> Original_CompParCompControl Then
                        _strUnmatched &= "Company Control"
                        .CompParCompControl = Original_CompParCompControl
                    End If
                    If .CompParKey <> Original_CompParKey Then
                        _strUnmatched &= "Parameter Key"
                        .CompParKey = Original_CompParKey
                    End If
                End If

                If Original_CompParText Is Nothing Then
                    If Not .IsCompParTextNull Then
                        _strUnmatched &= "Parameter Text"
                    End If
                Else
                    If .CompParText <> Original_CompParText Then
                        _strUnmatched &= "Parameter Text"
                        .CompParText = Original_CompParText
                    End If
                End If

                If Not Original_CompParValue.HasValue Then
                    If Not .IsCompParValueNull Then
                        _strUnmatched &= "Parameter Value"
                    End If
                Else
                    If .CompParValue <> Original_CompParValue.Value Then
                        _strUnmatched &= "Parameter Value"
                        .CompParValue = Original_CompParValue.Value
                    End If
                End If
                If Original_CompParDescription Is Nothing Then
                    If Not .IsCompParDescriptionNull Then
                        _strUnmatched &= "Parameter Description"
                    End If
                Else
                    If .CompParDescription <> Original_CompParDescription Then
                        _strUnmatched &= "Parameter Description"
                        .CompParDescription = Original_CompParDescription
                    End If
                End If
            End With
        End Sub

        Private Function executeUpdateBusinessRules(ByRef oRow As ApplicationData.CompParameterRow) As Boolean
            Dim CompParCompControl As Integer = oRow.CompParCompControl
            Dim CompParKey As String = oRow.CompParKey
            Dim CompParText As String = oRow.CompParText
            Dim CompParValue As Nullable(Of Double) = oRow.CompParValue
            Dim CompParDescription As String = oRow.CompParDescription
            If Not executeUpdateBusinessRules(CompParCompControl, _
                CompParKey, _
                CompParText, _
                CompParValue, _
                CompParDescription, _
                oRow.Item("CompParCompControl", DataRowVersion.Original), _
                oRow.Item("CompParKey", DataRowVersion.Original), _
                oRow.Item("CompParText", DataRowVersion.Original), _
                oRow.Item("CompParValue", DataRowVersion.Original), _
                oRow.Item("CompParDescription", DataRowVersion.Original), _
                oRow.Item("CompParControl", DataRowVersion.Original)) Then
                Return False
            End If
            AssignValues(oRow, CompParCompControl, CompParKey, CompParText, CompParValue, CompParDescription)
            Return True


        End Function
        ''' <summary>
        ''' Executes the required business rules before an update can be performed
        ''' </summary>
        ''' <param name="CompParCompControl"></param>
        ''' <param name="CompParKey"></param>
        ''' <param name="CompParText"></param>
        ''' <param name="CompParValue"></param>
        ''' <param name="CompParDescription"></param>
        ''' <param name="Original_CompParCompControl"></param>
        ''' <param name="Original_CompParKey"></param>
        ''' <param name="Original_CompParText"></param>
        ''' <param name="Original_CompParValue"></param>
        ''' <param name="Original_CompParDescription"></param>
        ''' <param name="Original_CompParControl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeUpdateBusinessRules(ByRef CompParCompControl As Integer, _
                ByRef CompParKey As String, _
                ByRef CompParText As String, _
                ByRef CompParValue As Nullable(Of Double), _
                ByRef CompParDescription As String, _
                ByVal Original_CompParCompControl As Integer, _
                ByVal Original_CompParKey As String, _
                ByVal Original_CompParText As String, _
                ByVal Original_CompParValue As Nullable(Of Double), _
                ByVal Original_CompParDescription As String, _
                ByVal Original_CompParControl As Integer) As Boolean
            Dim blnRet As Boolean = False
            Try
                If CompParKey.Trim <> Original_CompParKey.Trim Then
                    LastError = "Parameter Key Has Changed" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the company parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format
            End Try
            Return blnRet


        End Function
        ''' <summary>
        ''' Executes the required business rules before an insert can be performed
        ''' </summary>
        ''' <param name="CompParCompControl"></param>
        ''' <param name="CompParKey"></param>
        ''' <param name="CompParText"></param>
        ''' <param name="CompParValue"></param>
        ''' <param name="CompParDescription"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeInsertBusinessRules(ByRef CompParCompControl As Integer, _
                ByRef CompParKey As String, _
                ByRef CompParText As String, _
                ByRef CompParValue As Nullable(Of Double), _
                ByRef CompParDescription As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If CompParCompControl < 1 Then 'TODO: Add code to validate that CompParCompControl exists
                    LastError = "The Company Parameter Company Control Is Not Valid; it must be a valid company control id number." 'TODO: Add error string format
                    Return False
                End If
                If CompParKey Is Nothing OrElse CompParKey.Trim.Length < 4 Then
                    LastError = "The Company Parameter Key Is Not Valid; it cannot be empty and must be at lease 4 characters long." 'TODO: Add error string format
                    Return False
                End If
                If validateKey(CompParKey, CompParCompControl) Then
                    LastError = "New Company Parameter Key Already Exists For The Provided Company" 'TODO: Add error string format
                    Return False
                End If
                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the company parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "
                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet
        End Function
        ''' <summary>
        ''' Executes the required business rules before a delete can be performed
        ''' </summary>
        ''' <param name="Original_CompParCompControl"></param>
        ''' <param name="Original_CompParKey"></param>
        ''' <param name="Original_CompParText"></param>
        ''' <param name="Original_CompParValue"></param>
        ''' <param name="Original_CompParDescription"></param>
        ''' <param name="Original_CompParControl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function executeDeleteBusinessRules(ByVal Original_CompParCompControl As Integer, _
            ByVal Original_CompParKey As String, _
            ByVal Original_CompParText As String, _
            ByVal Original_CompParValue As Nullable(Of Double), _
            ByVal Original_CompParDescription As String, _
            ByVal Original_CompParControl As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If Original_CompParControl < 1 Then
                    LastError = "The Company Parameter Control Number Is Not Valid." 'TODO: Add error string format
                    Return False
                End If

                blnRet = True
            Catch ex As Exception
                LastError = "A unexpected error has occurred and the company parameter business rules could not be executed. Your changes could not be saved at this time! The actual error is: "

                If Me.Debug Then LastError &= ex.ToString Else LastError &= ex.Message 'TODO: Add error string format

            End Try
            Return blnRet

        End Function

#End Region
    End Class
End Namespace