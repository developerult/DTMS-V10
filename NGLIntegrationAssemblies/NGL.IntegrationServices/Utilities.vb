Imports System
Imports System.IO
Imports System.Linq
Imports System.Text.RegularExpressions

Public Class Utilities

    Public Shared Function splitLines(ByVal Data As String) As String()
        Dim strRet() As String = {""}
        If Not String.IsNullOrEmpty(Data) Then
            strRet = Data.Split(vbCrLf)
        End If
        Return strRet
    End Function

    Public Shared Function readFileToEnd(ByVal source As String, ByRef ErrMsg As String) As String
        Dim strData As String = ""
        Try
            If File.Exists(source) Then
                Using sr As StreamReader = New StreamReader(source)
                    strData = sr.ReadToEnd
                    sr.Close()
                End Using
            End If
        Catch ex As IOException
            ErrMsg = ex.Message
        Catch ex As Exception
            Throw
        End Try
        Return strData
    End Function


    Public Shared Function readFileToList(ByVal source As String, ByRef ErrMsg As String) As List(Of String)
        Dim strData As New List(Of String)
        Try
            If File.Exists(source) Then
                Using sr As StreamReader = New StreamReader(source)
                    Dim strLine As String = sr.ReadLine
                    Dim blnExtraLineFeed As Boolean = False
                    Dim strPreviousLine As String = ""
                    Do Until String.IsNullOrEmpty(strLine)
                        'The code for extra lines feeds corrects the problem where line feeds may be included in text data.  
                        If blnExtraLineFeed Then
                            strLine = strPreviousLine & " " & strLine
                        End If
                        blnExtraLineFeed = False
                        strPreviousLine = ""
                        strLine = strLine.Trim

                        'This extra line feed logic only works when all fields are ecapsulated in double quotes
                        If Right(strLine, 1) <> Chr(34) Then
                            blnExtraLineFeed = True
                            strPreviousLine = strLine
                        Else
                            blnExtraLineFeed = False
                            strData.Add(strLine)
                        End If
                            strLine = sr.ReadLine
                    Loop
                    sr.Close()
                End Using
            End If
        Catch ex As IOException
            ErrMsg = ex.Message
        Catch ex As Exception
            Throw
        End Try
        Return strData
    End Function

    Public Shared Function readDataToList(ByVal source As String) As List(Of String)
        Dim strData As New List(Of String)

        Dim strRet() As String = {""}
        If Not String.IsNullOrEmpty(source) Then
            strRet = source.Split(vbCrLf)
        End If
        Dim blnExtraLineFeed As Boolean = False
        Dim strPreviousLine As String = ""
        For Each strLine As String In strRet
            'The code for extra lines feeds corrects the problem where line feeds may be included in text data.  
            If blnExtraLineFeed Then
                strLine = strPreviousLine & " " & strLine
            End If
            blnExtraLineFeed = False
            strPreviousLine = ""
            strLine = strLine.Trim

            'This extra line feed logic only works when all fields are ecapsulated in double quotes
            If Right(strLine, 1) <> Chr(34) Then
                blnExtraLineFeed = True
                strPreviousLine = strLine
            Else
                blnExtraLineFeed = False
                strData.Add(strLine)
            End If
        Next
        Return strData
    End Function



    ''' <summary>
    ''' Splits a CSV record into an array of string values
    ''' </summary>
    ''' <param name="strLine"></param>
    ''' <returns></returns>
    ''' <remarks>Throw Application Exception</remarks>
    Public Shared Function DecodeCSV(ByVal strLine As String) As String()

        Dim strPattern As String
        Dim objMatch As Match
        strLine = strLine.Trim
        ' build a pattern
        strPattern = "^" ' anchor to start of the string
        strPattern += "(?:""(?<value>(?:""""|[^""\f\r])*)""|(?<value>[^,\f\r""]*))"
        strPattern += "(?:,(?:[ \t]*""(?<value>(?:""""|[^""\f\r])*)""|(?<value>[^,\f\r""]*)))*"
        strPattern += "$" ' anchor to the end of the string

        Try
            objMatch = Regex.Match(strLine, strPattern)

        Catch ex As Exception
            'return an empty string array
            Dim strVal As String = ""
            Dim arrOutput() As String = strVal.Split(",")
            Return arrOutput
        End Try
        ' get the match

        ' if RegEx match was ok
        If objMatch.Success Then
            Dim objGroup As Group = objMatch.Groups("value")
            Dim intCount As Integer = objGroup.Captures.Count
            Dim arrOutput(intCount - 1) As String

            ' transfer data to array
            For i As Integer = 0 To intCount - 1
                Dim objCapture As Capture = objGroup.Captures.Item(i)
                arrOutput(i) = objCapture.Value

                ' replace double-escaped quotes
                arrOutput(i) = arrOutput(i).Replace("""""", """")
            Next

            ' return the array
            Return arrOutput
        Else
            Throw New ApplicationException("Bad CSV line: " & strLine)
        End If

    End Function

    Public Shared Function CastToDouble(ByVal source As String, Optional ByVal def As Double = 0) As Double
        Double.TryParse(source, def)
        Return def
    End Function

    Public Shared Function CastToInteger(ByVal source As String, Optional ByVal def As Integer = 0, Optional ByVal roundupminimum As Boolean = True) As Integer
        'add logic to convert fractional values to 1 or -1
        If roundupminimum Then

            Dim dblVal As Double = CastToDouble(source)
            If dblVal > 0 And dblVal < 1 Then
                dblVal = 1
            End If
            source = dblVal.ToString
        End If

        Integer.TryParse(source, def)
        Return def
    End Function

    Public Shared Function CastToDate(ByVal source As String, ByVal def As Date) As Date
        Date.TryParse(source, def)
        Return def
    End Function


    'Friend Shared Sub LogResults(ByVal ModuleName As String, ByVal Result As Integer, ByVal LastError As String, ByVal AuthorizationCode As String)

    '    Try
    '        Using sw As New IO.StreamWriter(Me.LogFile, True)
    '            sw.WriteLine(String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"), _
    '                ModuleName, Result, LastError, AuthorizationCode))
    '            sw.Close()
    '        End Using
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Friend Shared Sub LogException(ByVal ModuleName As String, _
    '                               ByVal Result As Integer, _
    '                               ByVal logMessage As String, _
    '                               ByVal ex As Exception, _
    '                               ByVal AuthorizationCode As String, _
    '                               Optional ByVal strHeader As String = "")
    '    LogResults(ModuleName, Result, logMessage & ex.ToString, AuthorizationCode)
    '    Try
    '        Dim strMsg As String = "<p>" & logMessage & "</p>" & vbCrLf
    '        If strHeader.Trim.Length > 0 Then
    '            strMsg = "<h2>" & strHeader & vbCrLf & "</h2>" & strMsg
    '        End If
    '        strMsg &= "<hr />" & vbCrLf
    '        strMsg &= ex.ToString & vbCrLf
    '        strMsg &= "<hr />" & vbCrLf & vbCrLf & "<p>Using Authorization Code: " & AuthorizationCode & "</p>"

    '        SendEmail(ModuleName, strMsg)
    '    Catch e As Exception
    '        'Because this function is typically called when we are processing exceptions
    '        'we do nothing when sending an email from the web service 

    '    End Try


    'End Sub

    'Friend Shared Sub LogMessage(ByVal ModuleName As String, ByVal Msg As String)

    '    Try
    '        Using sw As New IO.StreamWriter(Me.LogFile, True)
    '            sw.WriteLine(String.Format("{0},{1},{2}", Now.ToString("MM/dd/yyyy hh:mm tt"), ModuleName, Msg))
    '            sw.Close()
    '        End Using
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Public Shared Sub SendEmail(ByVal Subject As String, _
    '                            ByVal Message As String)
    '    Try

    '        Dim oBatch As New BatchProcessingIntegration

    '        If Not oBatch.SendToNGLEmailService(My.Settings.EMailFrom, My.Settings.AdminEMail, "", Subject, Message) Then
    '            If Not String.IsNullOrEmpty(oBatch.LastError) Then LogMessage("Send Email", "Failure: " & oBatch.LastError)
    '        End If

    '    Catch ex As Exception
    '        'Because this function is typically called when we are processing exceptions
    '        'we do nothing when sending an email from the web service 
    '    End Try
    'End Sub

    '' ''Public Shared Function getHeaderFileName(ByVal detailfilename As String) As String
    '' ''    Return addPathToFile(My.Settings.HeaderFilePrefix & Regex.Split(Regex.Split(detailfilename, My.Settings.FileNameExtension)(0), My.Settings.DetailFilePrefix)(1) & My.Settings.FileNameExtension)
    '' ''End Function


    '' ''Public Shared Function getDetailFileName(ByVal filename As String) As String
    '' ''    Return addPathToFile(filename)
    '' ''End Function

    '' ''Public Shared Function addPathToFile(ByVal filename As String) As String
    '' ''    Dim strPath As String = My.Settings.LocalFilePath
    '' ''    If Not Right(strPath, 1) = "\" Then strPath &= "\"
    '' ''    Return strPath & filename
    '' ''End Function

    Public Shared Function createValidURL(ByVal baseURL As String, ByVal extension As String) As String
        Dim strFullURL As String = baseURL.Trim
        If Not Right(strFullURL, 1) = "/" Then strFullURL &= "/"
        Return strFullURL & extension
    End Function

End Class

Public Class CSVRecord
    Public Fields As List(Of String)

End Class
