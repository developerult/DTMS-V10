Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports NGL.Service.PCMiler64

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub TestMethod1()
        Dim oIFMStops As New List(Of clsFMStopData)
        Dim dblBatchID As Double = 100000011
        Dim blnKeepStopNumbers As Boolean = True
        Dim oPCMReportRecords As clsPCMReportRecord()
        Dim DebugMode As Boolean = True
        Dim LoggingOn As Boolean = True
        Dim KeepLogDays As Boolean = False
        Dim SaveOldLog As Boolean = False
        Dim LogFileName As String = "C:\NGLPCMLog.txt"
        Dim UseZipOnly As Boolean = False
        Dim LastError As String = ""
        Dim oGlobalStopData As clsGlobalStopData = Nothing
        Try
            'Create Stops
            Dim oFMStop As New clsFMStopData()
            oIFMStops.Add(New clsFMStopData() With {
                .BookControl = 8882,
                .BookCustCompControl = 1,
                .BookODControl = 68,
                .BookProNumber = "HIN764616",
                .City = "Hinton",
                .DistType = 0,
                .LocationisOrigin = True,
                .RouteType = 3,
                .State = "VA",
                .StopNumber = 0,
                .Street = "6349 Rawley Pike",
                .Zip = "22831"})

            oIFMStops.Add(New clsFMStopData() With {
                .BookControl = 8882,
                .BookCustCompControl = 1,
                .BookODControl = 68,
                .BookProNumber = "HIN764616",
                .City = "Linden",
                .DistType = 0,
                .LocationisOrigin = False,
                .RouteType = 3,
                .State = " NJ",
                .StopNumber = 1,
                .Street = "1911 Pennsylvania Avenue",
                .Zip = "07036"})
            oIFMStops.Add(New clsFMStopData() With {
               .BookControl = 8836,
               .BookCustCompControl = 2,
               .BookODControl = 237,
               .BookProNumber = "HAR764570",
               .City = "Swedesboro",
               .DistType = 0,
               .LocationisOrigin = False,
               .RouteType = 3,
               .State = " NJ",
               .StopNumber = 2,
               .Street = "2600 Oldmans Creek Road",
               .Zip = "08085"})

            Dim oPCmiles = New PCM64UT.PCMiles()
            'Dim oPCmiles As New PCMiles
            oPCmiles.Debug = DebugMode
            oPCmiles.LoggingOn = LoggingOn
            oPCmiles.KeepLogDays = KeepLogDays
            oPCmiles.SaveOldLog = SaveOldLog
            oPCmiles.LogFileName = LogFileName
            oPCmiles.UseZipOnly = UseZipOnly
            'Begin Modified by RHR for v-7.0.6.101 on 2/9/2017 
            Dim intPCMOptFlag As Integer = 3 '53 foot
            Dim intRouteType As Integer = 0 ' 0 = Practical, 1 = Shortest, and 4 = Air routing.
            Dim intPCMCalcType As Integer = INGL_Service_PCMiler.PCMEX_Route_Type.ROUTE_TYPE_PRACTICAL
            Dim intPCMVelType As Integer = INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK
            oGlobalStopData = oPCmiles.PCMReSyncMultiStopEx(oIFMStops.ToArray(), intPCMCalcType, intPCMOptFlag, intPCMVelType, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
            'oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, dblBatchID, blnKeepStopNumbers, oPCMReportRecords)
            'End Modified by RHR for v-7.0.6.101 on 2/9/2017 
            LastError = oPCmiles.LastError


        Catch ex As Exception
            LastError = ex.Message
        End Try


    End Sub

End Class