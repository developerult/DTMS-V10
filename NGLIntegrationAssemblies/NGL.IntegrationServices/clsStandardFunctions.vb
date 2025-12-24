Imports System.Diagnostics
Imports Microsoft.Win32



Public Class clsStandardFunctions : Inherits clsStandard

    


    Public Enum ViewMode
        LargeIcons = 0
        Details = 1
        SmallIcons = 2
        ListView = 3
    End Enum

    Public Enum PrintMode
        PrintPreview
        Printer
        PDF
        DOC
        EXCEL
        HTML
        RTF
        TXT
    End Enum


    Public Sub New(Optional ByVal strAppName As String = "FreightMaster" _
        , Optional ByVal strAppKey As String = "FreightMaster" _
        , Optional ByVal Key As String = "StandardFunctions")

        MyBase.New(strAppName, strAppKey, Key)

    End Sub

    Public Function writeRegistry(ByVal ValueName As String _
        , ByVal Value As Object) As Boolean
        Try
            Return writeRegistry(Registry.CurrentUser, "Software\" & mstrAppKey, ValueName, Value)

        Catch ex As Exception

            mobjLastException = ex
            Return False
        End Try

    End Function

    Public Function writeRegistry(ByVal ParentKey As RegistryKey, ByVal SubKey As String, _
        ByVal ValueName As String, ByVal Value As Object) As Boolean

        Dim Key As RegistryKey

        Try
            'Open the registry key.
            Key = ParentKey.OpenSubKey(SubKey, True)
            If Key Is Nothing Then 'if the key doesn't exist.
                Key = ParentKey.CreateSubKey(SubKey)
            End If

            'Set the value.
            Key.SetValue(ValueName, Value)
            Return True
        Catch ex As Exception
            mobjLastException = ex
            Return False
        End Try

    End Function

    Public Function readRegistry(ByVal ValueName As String _
        , ByRef Value As Object) As Boolean

        Dim blnRet As Boolean = False

        Try
            blnRet = readRegistry(Registry.CurrentUser, "Software\" & mstrAppKey, ValueName, Value)

        Catch ex As Exception
            mobjLastException = ex
            blnRet = False
        Finally
            readRegistry = blnRet
        End Try

    End Function

    Public Function readRegistry(ByVal ValueName As String _
        , ByRef Value As Object _
        , ByVal DefVal As Object) As Boolean

        Dim blnRet As Boolean = False

        Try
            blnRet = readRegistry(Registry.CurrentUser, "Software\" & mstrAppKey, ValueName, Value)
            If Value Is Nothing And blnRet Then
                Value = DefVal
            End If

        Catch ex As Exception
            mobjLastException = ex
            blnRet = False
        Finally
            readRegistry = blnRet
        End Try

    End Function

    Public Function readRegistry(ByVal ParentKey As RegistryKey, ByVal SubKey As String, _
        ByVal ValueName As String, ByRef Value As Object) As Boolean

        Dim Key As RegistryKey

        Try
            'Open the registry key.
            Key = ParentKey.OpenSubKey(SubKey, True)
            If Key Is Nothing Then 'if the key doesn't exist
                Throw New Exception("The registry key doesn't exist")
            End If

            'Get the value.
            Value = Key.GetValue(ValueName)

            Return True
        Catch ex As Exception
            mobjLastException = ex
            Return False
        End Try
    End Function

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
            mobjLastException = ex
            Return ""
        End Try

    End Function




    Public Function buildPath(ByVal strFolder As String, ByVal strFileName As String) As String
        Try
            If strFolder.Substring(strFolder.Length - 1, 1) <> "\" Then
                strFolder = strFolder & "\"
            End If
            Return strFolder & strFileName
        Catch ex As Exception
            mobjLastException = ex
            Return ""
        End Try

    End Function

    Public Function getViewModeString(ByVal Mode As ViewMode) As String
        Try
            Select Case Mode
                Case ViewMode.LargeIcons
                    Return "Large Icons"
                Case ViewMode.Details
                    Return "Details"
                Case ViewMode.SmallIcons
                    Return "Small Icons"
                Case ViewMode.ListView
                    Return "List View"
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            mobjLastException = ex
            Return ""
        End Try

    End Function

    Public Function getPrintModeString(ByVal Mode As PrintMode) As String
        Try
            Select Case Mode
                Case PrintMode.PrintPreview
                    Return "Print Preview"
                Case PrintMode.Printer
                    Return "Printer"
                Case PrintMode.PDF
                    Return "Save as PDF"
                Case PrintMode.DOC
                    Return "Save as Word"
                Case PrintMode.EXCEL
                    Return "Save as Excel"
                Case PrintMode.HTML
                    Return "Save as HTML"
                Case PrintMode.RTF
                    Return "Save as Rich Text"
                Case PrintMode.TXT
                    Return "Save as Text"
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            mobjLastException = ex
            Return ""
        End Try

    End Function

    Public Function getPrintModeExtension(ByVal Mode As PrintMode) As String
        Try
            Select Case Mode
                Case PrintMode.PrintPreview
                    Return ""
                Case PrintMode.Printer
                    Return ""
                Case PrintMode.PDF
                    Return ".pdf"
                Case PrintMode.DOC
                    Return ".doc"
                Case PrintMode.EXCEL
                    Return ".xls"
                Case PrintMode.HTML
                    Return ".htm"
                Case PrintMode.RTF
                    Return ".rtf"
                Case PrintMode.TXT
                    Return ".txt"
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            mobjLastException = ex
            Return ""
        End Try

    End Function



End Class
