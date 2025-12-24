Public Class ImportFieldFlags : Inherits List(Of ImportField)
    Public Sub AddNew(ByVal strImportFieldName As String, _
                        ByVal intImportFileType As Integer, _
                        ByVal blnImportFlag As Boolean, _
                        Optional ByVal strImportFileName As String = "")

        Dim oIF As ImportField
        Dim blnAddNew As Boolean = False

#Disable Warning BC42030 ' Variable 'oIF' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
        If Not fillIFByName(strImportFieldName, oIF) Then
#Enable Warning BC42030 ' Variable 'oIF' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not fillIFByType(intImportFileType, oIF) Then
                blnAddNew = True
            End If
        End If
        If blnAddNew OrElse oIF Is Nothing Then
            oIF = New ImportField
            If Not String.IsNullOrEmpty(strImportFieldName) Then oIF.ImportFieldName = strImportFieldName
            oIF.ImportFileType = intImportFileType
            oIF.ImportFlag = blnImportFlag
            oIF.ImportFileName = strImportFileName
            Me.Add(oIF)
        Else
            'only update values if they are not empty or zero
            If Not String.IsNullOrEmpty(strImportFieldName) Then oIF.ImportFieldName = strImportFieldName
            If intImportFileType > 0 Then oIF.ImportFileType = intImportFileType
            oIF.ImportFlag = blnImportFlag
            If Not String.IsNullOrEmpty(strImportFileName) Then oIF.ImportFileName = strImportFileName
        End If
    End Sub

    Private Function fillIFByName(ByVal strImportFieldName As String, ByRef oIF As ImportField) As Boolean
        Dim blnRet As Boolean = False
        If Not String.IsNullOrEmpty(strImportFieldName) Then
            Dim query = From q In Me Where q.ImportFieldName = strImportFieldName
            If query.count > 0 Then
                oIF = TryCast(query(0), ImportField)
                If Not oIF Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Private Function fillIFByType(ByVal intImportFileType As Integer, ByRef oIF As ImportField) As Boolean
        Dim blnRet As Boolean = False
        If intImportFileType > 0 Then
            Dim query = From q In Me Where q.ImportFileType = intImportFileType
            If query.count > 0 Then
                oIF = TryCast(query(0), ImportField)
                If Not oIF Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Note: the results of this function must be evauluated and converted to a boolean if desired
    ''' </summary>
    ''' <param name="strImportFieldName"></param>
    ''' <param name="intImportFileType"></param>
    ''' <returns>-1 No Match Found, 0 value is False, 1 value is True</returns>
    ''' <remarks></remarks>
    Public Function getImportFlag(ByVal strImportFieldName As String, ByVal intImportFileType As Integer) As Integer
        Dim intRet As Integer = -1
        If String.IsNullOrEmpty(strImportFieldName) Then Return -1 'the import field name is required
        Dim query = From q In Me Where q.ImportFieldName = strImportFieldName And q.ImportFileType = intImportFileType
        If query.Count > 0 Then
            Dim oIF As ImportField = TryCast(query(0), ImportField)
            If Not oIF Is Nothing Then intRet = CType(oIF.ImportFlag, Integer)
        End If
        Return intRet
    End Function
    
End Class

Public Class ImportField
    Public ImportFieldName As String = ""
    Public ImportFileType As Integer = -1
    Public ImportFileName As String = ""
    Public ImportFlag As Boolean = False
End Class
