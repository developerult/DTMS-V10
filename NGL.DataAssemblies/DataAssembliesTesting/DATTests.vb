Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAT = NGL.FM.DAT

Imports NGL.Core.Utility
Imports LTTypeEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum
Imports LTSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum

<TestClass()> Public Class DATTests
    Inherits TestBase

    <TestMethod()> Public Sub DATProcessDataTest()
        'Dim lt = createLoadTender()
        Dim lt = getLoadTender()
        Dim dat As New DAT.DAT(lt.SSOALoginURL, lt.SSOAUserName, lt.SSOAPassword)
        'dat.processData(lt, testParameters)

    End Sub

    <TestMethod()> Public Sub DATSystemDeleteExpiredPostTest()
        Dim b As New DAL.NGLBookRevenueData(testParameters)
        Dim bll As New Ngl.FM.BLL.NGLDATBLL(testParameters)
        Dim BookControl As Integer = 899509

        Dim bookRevs = b.GetBookRevenuesWDetailsFiltered(BookControl)
        'bll.DeleteDATPosting(bookRevs, NGL.FM.BLL.NGLBookBLL.AcceptRejectEnum.Rejected)
    End Sub

    <TestMethod()> Public Sub DATAcceptRejectTest()
        'Dim lt = createLoadTender()
        Dim lt = getLoadTender()
        Dim dat As New DAT.DAT(lt.SSOALoginURL, lt.SSOAUserName, lt.SSOAPassword)
        'dat.processData(lt, testParameters)

    End Sub

    <TestMethod()> Public Sub EnumMsgTest()
        Dim strEnum = "E_DATPostFailed"
        Dim strp = "This is the param message."
        Dim MsgEnum = DirectCast([Enum].Parse(GetType(DTO.WCFResults.MessageEnum), strEnum), DTO.WCFResults.MessageEnum)
        Dim s As String = ""
        s = DTO.WCFResults.getMessageLocalizedString(MsgEnum)
        'If s = "N/A" Then
        s = DTO.WCFResults.getMessageNotLocalizedString(MsgEnum)
        If s = "N/A" Then
            s = "Could not localize the following message enum: " + strEnum + " param: " + strp
        End If
        'End If
        Dim p() As String = {strp}
        Dim msg = String.Format(s, p)
    End Sub

    <TestMethod()> Public Sub AddRangeTest()
        Dim wcf1 As New DTO.WCFResults
        Dim wcf2 As New DTO.WCFResults
        Dim p As String() = {testParameters.UserName}
        wcf1.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoDATAccount, p)
        wcf1.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)

        Dim p2 As String() = {"This is the param message."}
        wcf2.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_DATPostFailed, p)
        wcf2.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)

        wcf1.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf2.Warnings)

        wcf1.Success = True
        wcf2.Success = True
       
    End Sub

    <TestMethod()> Public Sub testUDF()
        Dim oSec As New DAL.NGLtblSingleSignOnAccountData(testParameters)
        Dim blnRet As Boolean

        blnRet = oSec.DoesUserHaveSSOAAccount(117, 2)
        'in this case I know that user LVV (117) has an account for DAT (2)
        Assert.AreEqual(True, blnRet)

    End Sub

    Private Function createLoadTender() As DTO.tblLoadTender
        Dim lt As New DTO.tblLoadTender
        With lt
            .LTDATEquipType = "Reefer"
            .DATFeature = 1
            '.SSOALoginURL = "http://cnx.test.dat.com:9280/TfmiRequest"
            '.SSOAUserName = "nxt_cnx1"
            '.SSOAPassword = "nextgen"
            .UserName = "NGL\Lauren Van Vleet"
            .TokenExpiresDate = Nothing
            .TokenString = ""
            .LTDATEarliestAvailable = "2016-06-04 08:00:00.000"
            .LTDATLastestAvailable = "2016-06-04 17:00:00.000"
            .LTDATComment1 = "Total Stops: 1"
            .LTDATComment2 = ""
            .LTBookOrigCity = "Bedford Park"
            .LTBookOrigState = "IL"
            .LTBookOrigCountry = "US"
            .LTBookOrigZip = "60638"
            .LTBookDestCity = "Irwindale"
            .LTBookDestState = "CA"
            .LTBookDestCountry = "US"
            .LTBookDestZip = "91706"
            '.Database = "NGLMASDEV7051"
            '.DBServer = "NGLRDP06D"
            .LTBookTotalWgt = 3000
            .LTBookTotalCube = 300
            '.UserSecurityControl = 117
        End With

        'Dim oLT As New DAL.NGLLoadTenderData(testParameters)
        Dim oUSAT As New DAL.NGLtblUserSecurityAccessTokenData(testParameters)
        Dim NGLSecurityData As New DAL.NGLSecurityDataProvider(testParameters)

        'get the user security control for this user
        Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(lt.UserName)
        'get the token from the database for this user and DAT
        Dim USATToken = oUSAT.GettblUserSecurityAccessTokenFiltered(uSec.UserSecurityControl, DAL.Utilities.SSOAAccount.DAT)

        'GettblSingleSignOnAccountControl

        With lt
            If Not USATToken Is Nothing Then
                .TokenString = USATToken.USATToken
                .TokenExpiresDate = USATToken.USATExpires
            End If
            .Database = testParameters.Database
            .DBServer = testParameters.DBServer
            .ConnectionString = testParameters.ConnectionString
            '.UserName = testParameters.UserName
            .UserSecurityControl = uSec.UserSecurityControl
        End With
        Dim oBLL As New BLL.NGLDATBLL(testParameters)
        oBLL.getDATAccountInfo(uSec.UserSecurityControl, DAL.Utilities.SSOAAccount.DAT, lt)

        Return lt

    End Function

    Private Function getLoadTender() As DTO.tblLoadTender
        Dim lt As New DTO.tblLoadTender
        Dim oInt As New DAL.NGLLoadTenderData(testParameters)
        Dim oUSAT As New DAL.NGLtblUserSecurityAccessTokenData(testParameters)
        Dim NGLSecurityData As New DAL.NGLSecurityDataProvider(testParameters)
        Dim LTControl As Long = 137
        Dim UserName As String = "NGL\Lauren Van Vleet"

        lt = oInt.GetLoadTenderFiltered(LoadTenderControl:=LTControl)

        'get the user security control for this user
        Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(UserName)
        'get the token from the database for this user and DAT
        Dim USATToken = oUSAT.GettblUserSecurityAccessTokenFiltered(uSec.UserSecurityControl, DAL.Utilities.SSOAAccount.DAT)

        With lt
            If Not USATToken Is Nothing Then
                .TokenString = USATToken.USATToken
                .TokenExpiresDate = USATToken.USATExpires
            End If
            .Database = testParameters.Database
            .DBServer = testParameters.DBServer
            .ConnectionString = testParameters.ConnectionString
            .UserName = UserName
            .UserSecurityControl = uSec.UserSecurityControl
            .DATFeature = Ngl.FM.DAT.Infrastructure.Feature.Search
        End With
        Dim oBLL As New BLL.NGLDATBLL(testParameters)
        oBLL.getDATAccountInfo(uSec.UserSecurityControl, DAL.Utilities.SSOAAccount.DAT, lt)

        Return lt

    End Function

    <TestMethod()> Public Sub LoadBoardPostMethodTest()
        testParameters.UserName = "NGL\Lauren Van Vleet"
        Dim b As New DAL.NGLBookRevenueData(testParameters)
        Dim bll As New NGL.FM.BLL.NGLDATBLL(testParameters)

        Dim bwLB As New BitwiseFlags32

        'DAT
        'Dim BookControl As Integer = 902656
        'bwLB.turnBitFlagOn(LTTypeEnum.DAT)

        'NS
        'Dim BookControl As Integer = 902657
        'bwLB.turnBitFlagOn(LTTypeEnum.NextStop)

        'DAT and NS
        Dim BookControl As Integer = 902658
        bwLB.turnBitFlagOn(LTTypeEnum.NextStop)
        bwLB.turnBitFlagOn(LTTypeEnum.DAT)

        Dim selectedLoadBoardsInt As Integer = bwLB.FlagSource

        Dim bookRevs = b.GetBookRevenuesWDetailsFiltered(BookControl)
        bll.LoadBoardPostMethod(bookRevs, BookControl, selectedLoadBoardsInt)

    End Sub

    <TestMethod()> Public Sub NSAcceptTest()
        testParameters.UserName = "NGL\Lauren Van Vleet"
        Dim b As New DAL.NGLBookRevenueData(testParameters)
        Dim bll As New NGL.FM.BLL.NGLDATBLL(testParameters)

        Dim LTControl As Integer = 271
        Dim BookControl As Integer = 902661
        Dim BidControl As Integer = 41
        Dim BidCarrierControl As Integer = 9613
        Dim BidLineHaul As Decimal = 400
        Dim BidFuelUOM As String = "Flat Rate"
        Dim BidFuelVariable As Decimal = 50
        Dim CarrierContControl As Integer = 0

        Dim wcf = bll.NSAccept(LTControl, BookControl, BidControl, BidCarrierControl, BidLineHaul, BidFuelUOM, BidFuelVariable, CarrierContControl)

        If wcf.Success Then
            Dim str = "Yay"
        End If

    End Sub

End Class