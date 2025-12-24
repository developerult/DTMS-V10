Imports System
Imports System.Text
Imports System.Reflection
Imports System.IO
Imports PCMDLLTONET64.PCM
Imports PCMINT = PCMDLLINT64.PCM

Public Class PCMWrapper


#Region "Enums"
    Enum PCMEX_Route_Type As Integer
        ROUTE_TYPE_PRACTICAL = 0
        ROUTE_TYPE_SHORTEST = 1
        ROUTE_TYPE_NATIONAL = 2
        ROUTE_TYPE_AVOIDTOLL = 3
        ROUTE_TYPE_AIR = 4
        ROUTE_TYPE_53FOOT = 6
    End Enum

    Enum PCMEX_Opt_Flags As Integer
        OPT_NONE = 0
        OPT_AVOIDTOLL = 256
        OPT_NATIONAL = 512
        CALCEX_OPT_FIFTYTHREE = 1024
    End Enum

    Enum PCMEX_Veh_Type As Integer
        CALCEX_VEH_TRUCK = 0
        CALCEX_VEH_AUTO = 16777216
    End Enum

    Enum PCMEX_Dist_Type As Integer
        DIST_TYPE_MILES = 0
        DIST_TYPE_KILO = 1
    End Enum
#End Region

    Public strLastError As String = ""
    Public gLastError As String = ""
    Public Debug As Boolean = True
    Public KeepLogDays As Integer = 1
    Public SaveOldLog As Boolean = False
    Public UseZipOnly As Boolean = False
    Public gServerID As Short


    Public Sub ProcessData()
        
        'OrigCity, OrigState, OrigZip, DestCity, DestState, DestZip, FlatRate, Miles, MileRate
        Dim strInput As String = "Chicago,IL,60606,Inverness,IL,60010,200"
        Dim path As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString() & "InPut.txt"
        If Not File.Exists(path) Then
            Throw New ApplicationException("File Not Found: " & path)
        End If
        'delete any old OutPut.txt files
        Dim OutPutPath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString() & "OutPut.txt"
        If File.Exists(OutPutPath) Then
            File.Delete(OutPutPath)
        End If
        'File.Create(OutPutPath)
        Dim introw As Integer = 0
        Using sw As StreamWriter = New StreamWriter(OutPutPath, True)

            Using sr As StreamReader = File.OpenText(path)
                Do While sr.Peek() >= 0
                    introw += 1
                    Console.SetCursorPosition(0, 1)
                    Console.Write("Processing Row {0}", introw)
                    strInput = sr.ReadLine()
                    strLastError = ""
                    Dim oRoute As New Route
                    With oRoute
                        If .parseInPutCSV(strInput) Then
                            .Miles = Math.Round(getPracticalMiles(.getPCMOrigAddress, .getPCMDestAddress, .Message), 0)
                            .calculateMileRate()
                        End If
                        .Message &= strLastError
                        sw.WriteLine(.getOutPutCSV)
                    End With
                Loop
            End Using

        End Using
        Console.WriteLine()
        Console.WriteLine("Success!")

        'Console.WriteLine(Miles(orign, dest))
        'Console.WriteLine(testPracticalMiles())
    End Sub

#Region "File IO Methods"


#End Region


#Region "PC Miler Methods"


    Public Function PCMilerEnd() As Short
        Dim retVal As Short = 0
        Try
            retVal = PCMSCloseServer(gServerID)
            'Console.WriteLine("PCMiler End Executed")
        Catch ex As Exception
            LogError("Cannot execute PCMilerEnd.", ex)
        Finally
            gServerID = 0
        End Try

        Return retVal

    End Function

    Public Function PCMilerStart() As Short
        Dim errBuff As StringBuilder = New StringBuilder(100)
        Dim Ret As Short = 0
        If Not gServerID > 0 Then
            Try
                gServerID = PCMSOpenServer(0, 0)
                If Not gServerID > 0 Then
                    Ret = PCMSGetErrorString(PCMSGetError(), errBuff, 100)
                    LogError("PCMiler could not start. Cannot execute PCMilerStart.  " & errBuff.ToString.Replace(Chr(0), "").Trim)
                End If
            Catch ex As System.Runtime.InteropServices.SEHException
                LogError("PCMiler could not start. The application is not available or may not be configured correctly.")
            Catch ex As Exception
                LogError("PCMiler could not start. Cannot execute PCMilerStart.  ", ex)
            End Try
        End If

        Return gServerID

    End Function

    Private Sub LogError(ByVal strMsg As String, Optional ByVal e As Exception = Nothing)
        Dim strErr As String = ""
        If Not IsNothing(e) Then
            If Debug Then
                strLastError = strMsg & " " & e.ToString
            Else
                strLastError = strMsg & " " & e.Message
            End If
            strMsg &= " " & e.ToString
        Else
            strLastError = strMsg
        End If
        Console.WriteLine(strMsg)
    End Sub

    Public Function getPracticalMiles(ByVal Origin As String, ByVal Destination As String, ByRef Message As String) As Double

        Dim Ret As Integer = 0
        Dim mi As Integer = 0
        Dim intTrip1 As Integer = 0
        Dim intDash As Integer = 0
        Dim dblTotalMiles As Double = 0
        strLastError = ""
        Try
            If Not gServerID > 0 Then
                If Not PCMilerStart() > 0 Then
                    Return Nothing
                End If
            End If

            intTrip1 = PCMSNewTrip(gServerID)

            Call PCMSSetCalcType(intTrip1, 0)

            Call PCMSSetMiles(intTrip1)

            PCMSClearStops(intTrip1)
            'check the stops  
            Dim blnAddressValid As Boolean = True
            If PCMSLookup(intTrip1, Origin, 2) < 1 Then
                Message &= " Origin Address Is Not Valid "
                blnAddressValid = False
            End If
            If PCMSLookup(intTrip1, Destination, 2) < 1 Then
                Message &= " Destination Address Is Not Valid "
                blnAddressValid = False
            End If
            If Not blnAddressValid Then Return 0
            'add the stops
            Ret = PCMSAddStop(intTrip1, Origin)
            Ret = PCMSAddStop(intTrip1, Destination)

            Ret = PCMSOptimize(intTrip1)
            mi = PCMSCalculate(intTrip1)
            ' Check for errors before converting tenths to miles
            If -1 <> mi Then dblTotalMiles = mi / 10.0!
            Return dblTotalMiles


        Catch ex As System.AccessViolationException
            LogError("Cannot get miles: PC Miler is no longer running.", ex)

        Catch ex As Exception
            LogError("Cannot get miles: ", ex)

        Finally
            If intTrip1 > 0 Then
                Try
                    PCMSDeleteTrip(intTrip1)
                Catch ex As Exception

                End Try
            End If

        End Try
        Return dblTotalMiles

    End Function

#End Region
End Class
