Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports System.Data.SqlClient
Imports Ngl.Core.ChangeTracker
Imports Ngl.FM.BLL
Imports Ngl.Core.Utility

<TestClass()> Public Class SecurityDataTest
    Inherits TestBase

    <TestMethod()>
    Public Sub GetSubscribedSystemAlertsByUser2Test()
        Dim target As New DAL.NGLSecurityDataProvider(testParameters)

        Dim UserSecurityControl As Integer = 83 '117

        Dim res = target.GetSubscribedSystemAlertsByUser(UserSecurityControl)
        Dim ct = res.Count()

        'Dim res2 = target.GetSubscribedSystemAlertsByUser2(UserSecurityControl)
        'Dim ct2 = res2.Count()

        'Dim final = res.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Dim final2 = res2.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Assert.AreEqual(ct, ct2)

    End Sub

    <TestMethod()>
    Public Sub SystemAlertSaveAllUserSettingsTest()
        Dim target As New DAL.NGLSecurityDataProvider(testParameters)

        Dim UserSecurityControl As Integer = 117

        Dim res = target.SystemAlertSaveAllUserSettings(UserSecurityControl, False, False)
        Dim ct = res.Count()

        'Dim res2 = target.GetSubscribedSystemAlertsByUser2(UserSecurityControl)
        'Dim ct2 = res2.Count()

        'Dim final = res.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Dim final2 = res2.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Assert.AreEqual(ct, ct2)

    End Sub

    <TestMethod()>
    Public Sub GetSubcriptionAlertsFilteredTest()
        Dim target As New DAL.NGLtblAlertMessageData(testParameters)

        Dim blnAllProcedures As Boolean = True
        Dim procControl As Integer = 0
        Dim blnOnlyMyAlerts As Boolean = True
        Dim UserSecControl As Integer = 117
        Dim StartDate As DateTime? = "2016-07-24 09:29:54.033"
        Dim EndDate As DateTime? = Nothing
        Dim page As Integer = 1
        Dim pagesize As Integer = 1000
        Dim skip As Integer = 0
        Dim take As Integer = 0

        Dim res = target.GetSubcriptionAlertsFiltered(blnAllProcedures, procControl, blnOnlyMyAlerts, UserSecControl, StartDate, EndDate, page, pagesize, skip, take)
        Dim ct = res.Count()

        'Dim res2 = target.GetSubscribedSystemAlertsByUser2(UserSecurityControl)
        'Dim ct2 = res2.Count()

        'Dim final = res.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Dim final2 = res2.OrderBy(Function(x) x.ProcedureControl).Select(Function(x) x.ProcedureControl)

        'Assert.AreEqual(ct, ct2)

    End Sub

    <TestMethod()>
    Public Sub CreateSubcriptionAlertTest()
        testParameters.ConnectionString = "Server=ROBWIN8LAP\SQL2016;User ID=nglweb;Password=5529;Database=NGLMASDEV7051"
        testParameters.DBServer = "ROBWIN8LAP\SQL2016"
        testParameters.Database = "NGLMASDEV7051"
        testParameters.WCFAuthCode = "NGLSystem"
        testParameters.UserName = "RobWin8Lap\Rob"
        Dim target As New DAL.NGLtblAlertMessageData(testParameters)

        Dim Subject As String = "Long Test Subject XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX start T  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
        Dim Note1 As String = "Long Note 1 YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY start T  YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY"
        Dim Note2 As String = "Long Note 2 ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ start T  ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
        Dim Note3 As String = "Long Note 3 AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA start T  AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
        Dim Note4 As String = "Long Note 4 BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB start T  BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB"
        Dim Note5 As String = "Long Note 5 CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC start T  CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"
        Dim Body As String = "Long test Body " & vbCrLf & Note1 & vbCrLf & Note2 & vbCrLf & Note3 & vbCrLf & Note4 & vbCrLf & Note5
        target.InsertAlertMessage("TESTAlert", "TEST Alert", Subject, Body, 0, 0, 0, 0, Note1, Note2, Note3, Note4, Note5, False)



    End Sub

    <TestMethod()>
    Public Sub ExecuteAlertMessageNoEmailTest()
        testParameters.ConnectionString = "Server=ROBWIN8LAP\SQL2016;User ID=nglweb;Password=5529;Database=NGLMASDEV7051"
        testParameters.DBServer = "ROBWIN8LAP\SQL2016"
        testParameters.Database = "NGLMASDEV7051"
        testParameters.WCFAuthCode = "NGLSystem"
        testParameters.UserName = "RobWin8Lap\Rob"
        Dim target As New DAL.NGLBatchProcessDataProvider(testParameters)

        Dim Subject As String = "Long Test Subject XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX start T  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
        Dim Note1 As String = "Long Note 1 YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY start T  YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY"
        Dim Note2 As String = "Long Note 2 ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ start T  ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
        Dim Note3 As String = "Long Note 3 AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA start T  AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
        Dim Note4 As String = "Long Note 4 BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB start T  BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB"
        Dim Note5 As String = "Long Note 5 CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC start T  CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"
        Dim Body As String = "Long test Body " & vbCrLf & Note1 & vbCrLf & Note2 & vbCrLf & Note3 & vbCrLf & Note4 & vbCrLf & Note5
        target.executeInsertAlertMessageNoEmail("AlertEDI", 0, 0, 0, 0, Subject, Body, Note1, Note2, Note3, Note4, Note5, False)



    End Sub

    <TestMethod()>
    Public Sub GetMenuTreeDataTest()
        Dim target As New DAL.NGLSecurityDataProvider(testParameters)

        Dim UserSecControl As Integer = 172

        Dim res = target.getMenuTreeData(UserSecControl)

    End Sub

    <TestMethod()>
    Public Sub GetActiveLEUsers365Test()
        Dim target As New DAL.NGLUserSecurityLegalEntityData(testParameters)

        Dim RecordCount As Integer = 0
        Dim filters As New DAL.Models.AllFilters
        filters.LEAdminControl = 1

        Dim res = target.GetActiveLEUsers365(RecordCount, filters)
        Dim z = 5
    End Sub


    <TestMethod()>
    Public Sub NGLClientLoginTest()
        Dim oProperties = New DAL.WCFParameters
        With oProperties
            .Database = "NGLMASPROD"
            .DBServer = "DESKTOP-0R0EJUB"
            .UserRemotePassword = "@NGL_Integrated_Security_2011!@"
            .UserName = "DESKTOP-0R0EJUB\rkrte" 'System.Environment.UserDomainName & "\" & System.Environment.UserName
            .WCFAuthCode = "NGLSystem"
            .FormControl = 0
            .FormName = ""
            .ValidateAccess = True
            .UseToken = True
            .SSOAName = "NGL"
        End With
        Dim oData As New DAL.NGLtblUserSecurityAccessTokenData(oProperties)
        Dim sAppToken = oData.Login()

    End Sub

#Region "Security Type Configuration FlagSource"

    'SecurityType
    Private Enum SecurityType
        Everyone = 1
        CarrierDispatch = 2
        CarrierAccounting = 3
        Warehouse = 4
        NEXTrack = 5
        NEXTStop = 6
        LEOperations = 7
        LEAccounting = 8
        LEAdmin = 9
        Super = 10
        Inactive = 11
    End Enum

    ''' <summary>
    ''' Turns the bit flag on for each bit position in the list and returns the flag source
    ''' </summary>
    ''' <param name="bitPositionsOn"></param>
    Private Sub PrintConfigFlagSource(ByVal bitPositionsOn As List(Of SecurityType), ByVal intBulletNumber As Integer)
        Dim flagSource As Integer = 0
        Dim strTitle As String = ""
        Dim sSep As String = ""
        Dim bwTmp As New BitwiseFlags()
        For Each i In bitPositionsOn
            bwTmp.turnBitFlagOn(i)
            strTitle += (sSep + i.ToString())
            sSep = " + "
        Next
        flagSource = bwTmp.FlagSource
        Dim strPrint = "{0}. {1} = {2}"
        Console.WriteLine(String.Format(strPrint, intBulletNumber, strTitle, flagSource))
    End Sub

    ''' <summary>
    ''' Identify the most common bitwise flags
    ''' Generate bitflags for these combinations
    ''' Put in a numbered bullet list
    ''' </summary>
    <TestMethod()>
    Public Sub GetSecurityTypeOptionsFlagSource()
        'Set up the test connection
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.ConnectionString = "Server=NGLRDP07D;User ID=nglweb;Password=5529;Database=NGLMASPROD"
        testParameters.UserName = "ngl\lauren van vleet"
        Dim counter As Integer = 1
        'Everyone
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.Everyone}, counter)
        counter += 1
        'LEAdmin
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin}, counter)
        counter += 1
        'LEAdmin +LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEOperations}, counter)
        counter += 1
        'LEAdmin + LEAcct
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations}, counter)
        counter += 1
        'LEAdmin + NextTrack
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTrack}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack}, counter)
        counter += 1
        'LEAdmin + NextStop
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTStop}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + NextStop
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.NEXTStop}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + NextStop
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.NEXTStop}, counter)
        counter += 1
        'LEAdmin + Warehouse
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse}, counter)
        counter += 1
        'LEAdmin + Warehouse + LEAcct
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.LEAccounting}, counter)
        counter += 1
        'LEAdmin + Warehouse + LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.LEOperations}, counter)
        counter += 1
        'LEAdmin + Warehouse + LEAcct + LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.LEAccounting, SecurityType.LEOperations}, counter)
        counter += 1
        'LEAdmin + Warehouse + NextTrack
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.NEXTrack}, counter)
        counter += 1
        'LEAdmin + Warehouse + NextTrack + LEAcct
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.NEXTrack, SecurityType.LEAccounting}, counter)
        counter += 1
        'LEAdmin + Warehouse + NextTrack + LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.NEXTrack, SecurityType.LEOperations}, counter)
        counter += 1
        'LEAdmin + Warehouse + NextTrack + LEAcct + LEOps
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.Warehouse, SecurityType.NEXTrack, SecurityType.LEAccounting, SecurityType.LEOperations}, counter)
        counter += 1
        'CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.CarrierDispatch}, counter)
        counter += 1
        'CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.CarrierAccounting}, counter)
        counter += 1
        'CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + NextTrack + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTrack, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + NextTrack + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTrack, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + NextTrack + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTrack, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + NextStop + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTStop, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + NextStop + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTStop, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + NextStop + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.NEXTStop, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + NextStop + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + NextStop + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + NextTrack + NextStop + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + NextStop + CarrD
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierDispatch}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + NextStop + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierAccounting}, counter)
        counter += 1
        'LEAdmin + LEAcct + LEOps + NextTrack + NextStop + CarrD + CarrA
        PrintConfigFlagSource(New List(Of SecurityType) From {SecurityType.LEAdmin, SecurityType.LEAccounting, SecurityType.LEOperations, SecurityType.NEXTrack, SecurityType.NEXTStop, SecurityType.CarrierDispatch, SecurityType.CarrierAccounting}, counter)
        counter += 1
    End Sub

#End Region

End Class