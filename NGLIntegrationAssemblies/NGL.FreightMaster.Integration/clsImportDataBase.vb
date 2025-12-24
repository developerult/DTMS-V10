Public Class clsImportDataBase

    Public Function Item(ByVal strKey As String) As Object
        Dim oRet As Object
        Try
            Dim oProp = Me.[GetType]().GetProperty(strKey)
            If Not oProp Is Nothing Then
                oRet = oProp.GetValue(Me, Nothing)
            Else
                Return Nothing
            End If

        Catch ex As System.NullReferenceException
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
        Return oRet
    End Function

    Protected Function cleanDate(ByVal strDate As String, Optional ByVal strFormat As String = "MM/dd/yyyy HH:mm:ss") As String
        Dim dtVal As Date
        Try

            'test for date value
            If String.IsNullOrEmpty(strDate) Then
                Return ""
            ElseIf IsDate(strDate) Then
                dtVal = CDate(strDate)
                Return Format(dtVal, strFormat)
            Else
                strDate = Trim(strDate)
                If InStr(1, strDate, "-") Or InStr(1, strDate, "\") Then
                    Return ""
                Else
                    If Len(strDate) <> 8 Then
                        Return ""
                    Else
                        Dim strYear As String = Right(strDate, 4)
                        Dim strReturn As String = Left(strDate, Len(strDate) - 4)
                        Dim strDay As String = Right(strReturn, 2)
                        Dim strMonth = Left(strReturn, Len(strReturn) - 2)
                        strReturn = strMonth & "/" & strDay & "/" & strYear
                        If validateDate(strReturn, "M/d/yyyy", dtVal) Then  'default is short date
                            Return Format(dtVal, strFormat)
                        Else
                            Return ""
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return ""

    End Function

    Protected Function CleanTime(ByVal strVal As String, Optional ByVal strDefault As String = "") As String
        Dim blnRet As Boolean = False
        Dim strhour As String
        Dim strminute As String
        Dim strseconds As String
        Dim dtVal As Date
        Try

            'test for Time value
            If String.IsNullOrEmpty(strVal) Then
                If Not String.IsNullOrEmpty(strDefault.Trim) AndAlso IsDate(strDefault) Then
                    Return strDefault
                End If
            ElseIf IsDate(strVal) Then
                Return strVal
            Else
                If InStr(1, strVal, "-") Or InStr(1, strVal, "\") Or InStr(1, strVal, ":") Then
                    'the IsDate function should have worked on a valid time string
                    Return ""
                ElseIf strVal.Length = 4 Then
                    'convert the time format HHmm to a string containing the name of the day of the week, 
                    'the name of the month, the numeric day of the hours, minutes equivalent 
                    'to the time value of this instance using the date Jan 1st 2000 
                    strminute = Right(strVal.ToString, 2)
                    strhour = Left(strVal.ToString, 2)
                    If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute, dtVal) Then
                        Return dtVal.ToShortTimeString
                    End If
                ElseIf strVal.Length = 8 Then
                    'convert the time format HHmmss to a string containing the name of the day of the week, 
                    'the name of the month, the numeric day of the hours, minutes, and seconds equivalent 
                    'to the time value of this instance using the date Jan 1st 2000
                    strseconds = Right(strVal.ToString, 2)
                    strminute = strVal.Substring(2, 2)
                    strhour = Left(strVal.ToString, 2)
                    If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute & ":" & strseconds, dtVal) Then
                        Return dtVal.ToShortTimeString
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return ""
    End Function

    Protected Function validateDate(ByVal strDate As String, ByVal strFormat As String, ByRef dtVal As Date) As Boolean
        Dim blnRet As Boolean = False
        Try
            dtVal = DateTime.ParseExact(strDate, strFormat, Nothing).ToString
            Return True
        Catch ex As System.ArgumentNullException
            Return False
        Catch ex As System.FormatException
            Return False
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function
End Class
