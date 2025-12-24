Imports System
Imports System.Text
Imports System.Reflection
Imports PCMDLLTONET64.PCM
Imports PCMINT = PCMDLLINT64.PCM


Module PCMilerClient


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

    Dim strLastError As String = ""
    Dim gLastError As String = ""
    Dim Debug As Boolean = True
    Dim KeepLogDays As Integer = 1
    Dim SaveOldLog As Boolean = False
    Dim UseZipOnly As Boolean = False
    Dim gServerID As Short

    Sub Main()

        Try
            Dim orign As String = "60611"
            Dim dest As String = "37726"
            'Console.WriteLine(Miles(orign, dest))
            Console.WriteLine(testPracticalMiles())
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Try
                PCMilerEnd()
            Catch ex As Exception
                'do nothing
            End Try
            Console.WriteLine("Press Enter To Continue...")
            Console.ReadLine()
        End Try

    End Sub

#Region "PC Miler Methods"

    Public Function Miles(ByVal Origin As String, ByVal Destination As String) As Single
        Dim mi As Integer = 0
        Dim oo As String = ""
        Dim dd As String = ""
        Dim sglRet As Single = -1.0!
        strLastError = ""
        Try
            If Not gServerID > 0 Then
                If Not PCMilerStart() > 0 Then
                    Return sglRet
                End If
            End If

            mi = PCMSCalcDistance(gServerID, Origin, Destination)
            ' Check for errors before converting tenths to miles
            If mi > 0 Then
                sglRet = mi / 10.0!
            End If


        Catch ex As System.AccessViolationException
            LogError("Cannot get miles for " & Origin & " To " & Destination & ": PC Miler is no longer running.", ex)
        Catch ex As Exception
            LogError("Cannot not calculate miles. Please check your results for errors.", ex)
        End Try
        Return sglRet

    End Function


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

    Public Function testPracticalMiles() As Double

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
            'add the test stops
            Ret = PCMSAddStop(intTrip1, "60611")
            Ret = PCMSAddStop(intTrip1, "37726")
             
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

End Module
