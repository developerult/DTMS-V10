
Imports System.IO
Public Class clsLog

    Private mblnDebug As Boolean = False
    Private mstrFileName As String

    Public Property Debug() As Boolean
        Get
            Debug = mblnDebug
        End Get
        Set(ByVal Value As Boolean)
            mblnDebug = Value
        End Set
    End Property

    Public Function Open(ByVal strFileName As String) As StreamWriter
        Return Open(strFileName, 30, True)
    End Function

    Public Function Open(ByVal strFileName As String, ByVal intKeepLogDays As Integer) As StreamWriter
        Return Open(strFileName, intKeepLogDays, True)
    End Function

    Public Function Open(ByVal strFileName As String, ByVal intKeepLogDays As Integer, ByVal blnSaveOldLog As Boolean) As StreamWriter
        Dim ioLog As StreamWriter = Nothing
        mstrFileName = strFileName
        Try
            Dim fi As FileInfo = New FileInfo(strFileName)

            'If Not File.Exists(strFilename) Then
            '    'automatically delete the log file
            '    If mblnDebug Then
            '        Console.WriteLine("{0} deleting existing upload results file.", strFilename)
            '    End If
            '    fi.Delete()
            'End If
            If Not File.Exists(strFileName) Then
                'create it
                ioLog = fi.CreateText()
                ioLog.Close()
            Else
                Dim ndate As DateTime = Date.Now
                Dim fdate As DateTime = fi.CreationTime
                If DateDiff(DateInterval.Day, fdate, ndate) > intKeepLogDays Then
                    If blnSaveOldLog Then
                        fi.MoveTo(timeStampFileName(strFileName))
                    Else
                        fi.Delete()
                    End If
                    fi = New FileInfo(strFileName)
                    ioLog = fi.CreateText()
                    ioLog.Close()
                    fi.CreationTime = ndate
                    If mblnDebug Then
                        Console.WriteLine("File Date = " & fi.CreationTime.ToString)
                    End If
                End If
            End If
            ioLog = File.AppendText(strFileName)
            If mblnDebug Then
                Console.WriteLine("***********************************")
                Console.WriteLine("Log Open: " & strFileName)
                Console.WriteLine("-----------------------------------")
            End If
        Catch ex As System.IO.FileNotFoundException
            If mblnDebug Then
                Console.WriteLine("clsLog.Open File Not Found Error Re-creating file.")
            End If

        Catch ex As Exception
            'ignore any errors
            If mblnDebug Then
                Console.WriteLine("clsLog.Open Error: " & ex.ToString)
            End If
        Finally
            Try
                ioLog.Close()
            Catch ex As Exception

            End Try
        End Try
        Return ioLog


    End Function

    Public Sub Write(ByVal logMessage As String, ByRef w As StreamWriter)
        Static intlines As Integer = 0
        intlines += 1
        Try
            If mblnDebug Then
                Console.WriteLine(intlines.ToString & " => " & logMessage)
            End If
            w = File.AppendText(mstrFileName)
            Try
                w.Write(ControlChars.CrLf & "Log Entry : ")
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString())
                w.WriteLine("  :")
                w.WriteLine("  :{0}", logMessage)
                w.WriteLine("-------------------------------")
                ' Update the underlying file.
                w.Flush()
            Catch ex As Exception
                If mblnDebug Then Console.WriteLine("Write To Log Failure:" & ex.ToString)
            Finally
                w.Close()
            End Try

        Catch ex As Exception
            'ignore any errors while writing to the log
            If mblnDebug Then Console.WriteLine("Write To Log Failure:" & ex.ToString)

        End Try

    End Sub

    Public Sub closeLog(ByVal intReturn As Integer, ByRef ioLog As StreamWriter)
        Try
            ioLog = File.AppendText(mstrFileName)
            Try
                ioLog.WriteLine("Return Value: " & intReturn.ToString)
                ioLog.Flush()
            Catch ex As Exception
                If mblnDebug Then Console.WriteLine("Close Log Error:" & ex.ToString)
            Finally
                ioLog.Close()
            End Try
            If mblnDebug Then
                Console.WriteLine("***********************************")
                Console.WriteLine("Log Closed")
                Console.WriteLine("-----------------------------------")
            End If
        Catch ex As Exception
            'ignore any errors when closing the log file
            If mblnDebug Then Console.WriteLine("Close Log Error:" & ex.ToString)

        End Try
    End Sub

    Public Sub DumpLog(ByVal strLogFile As String)
        Dim r As StreamReader
        Try
            r = File.OpenText(strLogFile)

            Try


                ' While not at the end of the file, read and write lines.
                Dim line As String
                line = r.ReadLine()
                While Not line Is Nothing
                    Console.WriteLine(line)
                    line = r.ReadLine()
                End While
            Catch ex As Exception
                'do nothing
            Finally
                Try
                    r.Close()
                Catch ex As Exception

                End Try
            End Try


        Catch ex As Exception

        End Try
    End Sub

    Public Function timeStampFileName(ByVal strFileName As String, Optional ByVal strNewExtension As String = "", Optional ByVal blnNoSpaces As Boolean = False) As String


        Try
            Dim dt As DateTime = DateTime.Now
            Dim dfi As Globalization.DateTimeFormatInfo = New Globalization.DateTimeFormatInfo
            dfi.DateSeparator = "-"
            dfi.TimeSeparator = "-"

            If strNewExtension.Length < 1 Then
                strNewExtension = strFileName.Substring(strFileName.Length - 4, 4)
            End If
            If blnNoSpaces Then
                Return strFileName.Substring(0, strFileName.Length - 4) & dt.Month.ToString & dt.Day.ToString & dt.Year.ToString & dt.Hour.ToString & dt.Minute.ToString & dt.Second.ToString & strNewExtension
            Else
                Return strFileName.Substring(0, strFileName.Length - 4) & "-" & dt.ToString("g", dfi) & strNewExtension
            End If



        Catch ex As Exception

            Return ""
        End Try

    End Function

End Class
