
Imports System.IO

Public Class clsLog


#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strFileName As String, ByVal intKeepLogDays As Integer, ByVal blnSaveOldLog As Boolean)
        MyBase.New()
        _strFileName = strFileName
        _intKeepLogDays = intKeepLogDays
        _blnSaveOldLog = blnSaveOldLog
        Open(strFileName, intKeepLogDays, blnSaveOldLog)
    End Sub

#End Region

#Region "Properties"

    Private _blnDebug As Boolean = False
    Public Property Debug() As Boolean
        Get
            Debug = _blnDebug
        End Get
        Set(ByVal Value As Boolean)
            _blnDebug = Value
        End Set
    End Property

    Private _strLastErr As String = ""
    Public Property LastError() As String
        Get
            Return _strLastErr

        End Get
        Protected Set(ByVal Value As String)
            _strLastErr = Value
        End Set
    End Property


    Private _strFileName As String = "C:\NGLApplication-Log.txt"
    Public Property FileName() As String
        Get
            Return _strFileName
        End Get
        Set(ByVal value As String)
            _strFileName = value
        End Set
    End Property

    Private _intKeepLogDays As Integer = 7
    Public Property KeepLogDays() As Integer
        Get
            Return _intKeepLogDays
        End Get
        Set(ByVal value As Integer)
            _intKeepLogDays = value
        End Set
    End Property

    Private _blnSaveOldLog As Boolean = False
    Public Property SaveOldLog() As Boolean
        Get
            Return _blnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            _blnSaveOldLog = value
        End Set
    End Property

#End Region

#Region "Protected Methods"



#End Region

#Region "Public Methods"

    Public Function Open() As StreamWriter
        Return Open(_strFileName, _intKeepLogDays, _blnSaveOldLog)
    End Function

    Public Function Open(ByVal strFileName As String) As StreamWriter
        Return Open(strFileName, 30, True)
    End Function

    Public Function Open(ByVal strFileName As String, ByVal intKeepLogDays As Integer) As StreamWriter
        Return Open(strFileName, intKeepLogDays, True)
    End Function

    Public Function Open(ByVal strFileName As String, ByVal intKeepLogDays As Integer, ByVal blnSaveOldLog As Boolean) As StreamWriter
        Dim ioLog As StreamWriter = Nothing
        _strFileName = strFileName
        _intKeepLogDays = intKeepLogDays
        _blnSaveOldLog = blnSaveOldLog
        Try
            Dim fi As FileInfo = New FileInfo(strFileName)
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
                    If _blnDebug Then
                        Console.WriteLine("File Date = " & fi.CreationTime.ToString)
                    End If
                End If
            End If
            ioLog = File.AppendText(strFileName)
            If _blnDebug Then
                Console.WriteLine("***********************************")
                Console.WriteLine("Log Open: " & strFileName)
                Console.WriteLine("-----------------------------------")
            End If
        Catch ex As System.IO.FileNotFoundException
            If _blnDebug Then
                Console.WriteLine("clsLog.Open File Not Found Error Re-creating file.")
                LastError = ex.ToString
            Else
                LastError = ex.Message
            End If

        Catch ex As Exception
            'ignore any errors
            If _blnDebug Then
                Console.WriteLine("clsLog.Open Error: " & ex.ToString)
                LastError = ex.ToString
            Else
                LastError = ex.Message
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
            If _blnDebug Then
                Console.WriteLine(intlines.ToString & " => " & logMessage)
            End If
            w = File.AppendText(_strFileName)
            Try
                w.Write(ControlChars.CrLf & "Log Entry ")
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString())
                w.WriteLine("  {0}", logMessage)
                w.WriteLine("----------------------------------------------")
                ' Update the underlying file.
                w.Flush()
            Catch ex As Exception
                If _blnDebug Then Console.WriteLine("Write To Log Failure:" & ex.ToString)
                'Throw
            Finally
                w.Close()
            End Try

        Catch ex As Exception
            'ignore any errors while writing to the log
            If _blnDebug Then Console.WriteLine("Write To Log Failure:" & ex.ToString)
            'Throw

        End Try

    End Sub

    Public Sub closeLog(ByVal intReturn As Integer, ByRef ioLog As StreamWriter)
        Try
            ioLog = File.AppendText(_strFileName)
            Try
                ioLog.WriteLine("Return Value: " & intReturn.ToString)
                ioLog.Flush()
            Catch ex As Exception
                If _blnDebug Then
                    Console.WriteLine("Close Log Error (ignored when debug is off):" & ex.ToString)
                    LastError = ex.ToString
                Else
                    LastError = ex.Message
                End If
            Finally
                ioLog.Close()
            End Try
            If _blnDebug Then
                Console.WriteLine("***********************************")
                Console.WriteLine("Log Closed")
                Console.WriteLine("-----------------------------------")
            End If
        Catch ex As Exception
            'ignore any errors when closing the log file
            If _blnDebug Then
                Console.WriteLine("Close Log Error (Ignored when debug is off):" & ex.ToString)
                LastError = ex.ToString
            Else
                LastError = ex.Message
            End If
        End Try
    End Sub

    Public Sub closeLog(ByRef ioLog As StreamWriter)
        Try

            ioLog.Close()

        Catch ex As Exception
            'ignore any errors when closing the log file
            If _blnDebug Then
                Console.WriteLine("Close Log Error (Ignored when debug is off):" & ex.ToString)
                LastError = ex.ToString
            Else
                LastError = ex.Message
            End If

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



#End Region

End Class
