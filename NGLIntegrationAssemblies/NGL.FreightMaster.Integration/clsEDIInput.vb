Imports System.Text
Imports System.Text.RegularExpressions
Imports Ngl.FreightMaster.Integration.Configuration
Imports DAL = Ngl.FreightMaster.Data 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects 'Added by RHR for v-7.0.6.105 on 5/12/2017 for EDI 204 inbound processing

<Serializable()>
Public Class clsEDIInput : Inherits clsDownload

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 On 6/2/2017 
    ''' </remarks>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor with config parameter
    ''' </summary>
    ''' <param name="config"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 On 6/2/2017 
    ''' </remarks>
    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Private _EDI997Response As String = ""
    Public ReadOnly Property EDI997Response As String
        Get
            Return _EDI997Response
        End Get
    End Property

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _AlertMsgList As Dictionary(Of Integer, String)
    Public Property AlertMsgList() As Dictionary(Of Integer, String)
        Get
            Return _AlertMsgList
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _AlertMsgList = value
        End Set
    End Property

    'Add optional parameter CarrierControl with default 0
    '@TODO Edit the Edi command line program to pass the selected carrier control to this method
    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' <summary>
    ''' EDI input procedure to parse the EDI data into the correct EDI objects and save to db as needed,  
    ''' new sConfigFile may be provided for additional mapping and configuration settings configured by 
    ''' trading partner.
    ''' </summary>
    ''' <param name="sEDIData"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="fileName"></param>
    ''' <param name="DateProcessed"></param>
    ''' <param name="sConfigFile"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Add optional parameter CarrierControl with default 0
    ''' @TODO Edit the Edi command line program to pass the selected carrier control to this method
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Modified by RHR for v-7.0.6.105 on 5/18/2017 
    '''   added new optional sConfigFile parameter
    ''' Modified By LVV on 10/10/19
    '''  We didn't want to keep sending error emails for if the 997 config does not exist, or if
    '''  the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
    ''' </remarks>
    Public Function ProcessData(ByVal sEDIData As String,
                                ByVal strConnection As String,
                                Optional ByVal CarrierControl As Integer = 0,
                                Optional ByVal fileName As String = "",
                                Optional ByVal DateProcessed As Date = Nothing,
                                Optional ByVal sConfigFile As String = "",
                                Optional ByVal CarrierEDIAcknowledgementRequested As Boolean = True) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "EDI Input Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            'Modified by RHR for v-7.0.6.105 on 5/18/2017 added sConfigFile
            parseEDI(sEDIData, CarrierControl, fileName, DateProcessed, sConfigFile, CarrierEDIAcknowledgementRequested) 'Added By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process EDI Input Data Warning", "The following errors or warnings were reported some EDI Input information may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process EDI Input Data Failure", "The following errors or warnings were reported some EDI Input information may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " EDI Input messages were processed."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " EDI Input messages could not be processed."
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If

            Else
                If Me.RecordErrors > 0 Then
                    strTitle = "Process Data Failure"
                    strMsg = "ERROR!  " & Me.RecordErrors & " EDI Input messages could not be processed."
                    intRet = ProcessDataReturnValues.nglDataIntegrationFailure
                Else
                    strMsg = "No Supported EDI Input messages were found."
                    'we return complete as long as there are no errors
                    intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process EDI Input Data Failure", "Could not process the provided EDI data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDIInput.ProcessData")
        Finally
            closeConnection()
        End Try

        Return intRet
    End Function


    ''' <summary>
    ''' Does the same thing as ProcessData but handles the log/error messages differently.
    ''' Returns the messages to the caller instead of sending emails and writing to log tables
    ''' EDI input procedure to parse the EDI data into the correct EDI objects and save to db as needed
    ''' </summary>
    ''' <param name="sEDIData"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="CarrierControl">Optional; Default 0</param>
    ''' <param name="fileName"></param>
    ''' <param name="DateProcessed"></param>
    ''' <param name="sConfigFile">Optional - May be provided for additional mapping and configuration settings configured by trading partner</param>
    ''' <param name="CarrierEDIAcknowledgementRequested">Optional - Don't want to send error emails if 997 config does not exist, or if AcknowledgementRequested flag is off for 210/214 (support case 201910021431)</param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV om 11/13/2019 - Wanted to return the log messages to display on EDILogs screen</remarks>
    Public Function ProcessDataManual(ByVal sEDIData As String,
                                      ByVal strConnection As String,
                                      Optional ByVal CarrierControl As Integer = 0,
                                      Optional ByVal fileName As String = "",
                                      Optional ByVal DateProcessed As Date = Nothing,
                                      Optional ByVal sConfigFile As String = "",
                                      Optional ByVal CarrierEDIAcknowledgementRequested As Boolean = True) As List(Of String)
        Dim results As New List(Of String)
        CreatedDate = Now.ToString
        CreateUser = "Data Integration DLL"
        Source = "EDI Input Data Integration"
        DBConnection = strConnection
        If Not openConnection() Then Return results 'try the connection
        Dim strSuccess = "Success! {0} EDI messages were processed."
        Dim strWErrors = "Process Data Complete With Errors - {0} EDI messages were processed successfully, {1} EDI messages could not be processed."
        Dim strFail = "Process Data Failure - {0} EDI messages could not be processed."
        Dim strException = "Could not process the provided EDI data - {0}"
        Try
            parseEDI(sEDIData, CarrierControl, fileName, DateProcessed, sConfigFile, CarrierEDIAcknowledgementRequested) 'Added By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
            If GroupEmailMsg.Trim.Length > 0 Then results.Add(GroupEmailMsg)
            If ITEmailMsg.Trim.Length > 0 Then results.Add(ITEmailMsg)
            If TotalRecords > 0 Then
                If RecordErrors > 0 Then results.Add(String.Format(strWErrors, TotalRecords, RecordErrors)) Else results.Add(String.Format(strSuccess, TotalRecords))
            Else
                If RecordErrors > 0 Then results.Add(String.Format(strFail, RecordErrors)) Else results.Add("No Supported EDI messages were found.")
            End If
        Catch ex As Exception
            Dim oWCFPar = New DAL.WCFParameters() With {.Database = Me.Database, .DBServer = Me.DBServer, .UserName = "EDI Integration", .WCFAuthCode = "NGLSystem"}
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(oWCFPar)
            Dim exMsg = oLocalize.GetLocalizedValueByKey(ex.Message, ex.Message)
            If Debug Then results.Add(String.Format(strException, ex.ToString)) Else results.Add(String.Format(strException, exMsg))
        Finally
            closeConnection()
        End Try
        Return results
    End Function


    'Add an optional parameter for CarrierControl with 0 as default

    ''' <summary>
    ''' Parse the EDI file and process each record
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="CarrierControl">optional parameter for CarrierControl with 0 as default</param>
    ''' <param name="fileName">optional parameter for fileName with "" as default</param>
    ''' <param name="DateProcessed">optional parameter for DateProcessed with Nothing as default</param>
    ''' <remarks>
    ''' Modified by RHR 1/6/2015 v-7.0
    ''' Removed References to old RequireCNS logic
    '''   We now always use the BookSHID as the key value
    '''   mapped to segment B204 
    '''   Removed old code with comment tags no longer needed
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added fileName and DataProcessed functionality, 820 document type
    ''' Modified by RHR for v-7.0.6.105 on 5/12/2017
    '''     added new logic to process EDI 204 inbound Single
    '''     added optional sConfigFile parameter 
    '''     added logic to support EDI 204 inbound 997 notifications
    ''' Modified By LVV on 10/10/19
    '''  We didn't want to keep sending error emails for if the 997 config does not exist, or if 
    '''  the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
    ''' </remarks>
    Private Sub parseEDI(ByVal e As String,
                         Optional ByVal CarrierControl As Integer = 0,
                         Optional ByVal fileName As String = "",
                         Optional ByVal DateProcessed As Date = Nothing,
                         Optional ByVal sConfigFile As String = "",
                         Optional ByVal CarrierEDIAcknowledgementRequested As Boolean = True)
        '1st read first 106 characters
        Dim strISA As String = Left(e, 106)
        Dim strSegSep As String = Right(strISA, 1)
        'now split any ISA sections
        Dim sISAs() As String = Regex.Split(e, "\" & strSegSep & "ISA\*")
        'now we have a list of ISAs so split each ISAs by GS
        Dim strISAHeader As String = ""
        Dim blnISAHeaderAdded As Boolean = False
        Dim strGSHeader As String = ""
        Dim blnGSHeaderAdded As Boolean = False
        Dim oISA As clsEDIISA
        Dim oGS As clsEDIGS
        Dim strMsg As String = ""
        'create a new 997 object for sending message acknowledgments
        Dim oR997s As New List(Of clsEDI997)
        Dim oR990s As New List(Of clsEDI990)
        'Check if the require CNS Flag is turned on.
        For Each s In sISAs
            'Add the ISA to the array we have to add the ISA* string back in if it does not exist
            If Left(s, 4) = "ISA*" Then
                strISAHeader = Left(s, 105)
            Else
                strISAHeader = "ISA*" & Left(s, 101)
            End If
            oISA = New clsEDIISA(strISAHeader)
            oISA.SegmentTerminator = strSegSep
            'strip off the IEA segment
            Dim sIEAs() As String = Regex.Split(s, "\" & strSegSep & "IEA\*")
            'get the GS data
            Dim sGSs() As String = Regex.Split(sIEAs(0), "\" & strSegSep & "GS\*")
            For i = 1 To sGSs.Length - 1 'we skip the first item because it holds the ISA data
                Dim sg As String = sGSs(i)
                If Left(sg, 2) = "FA" Then 'this is a 997 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim o997 As New clsEDI997(sSEs(0), strSegSep, sSEs(1))
                            'call the process Data function
                            strMsg = ""
                            If Me.openConnection Then
                                'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                If o997.processData(oISA, oGS, CarrierControl, Me.DBServer, Me.Database, strMsg, DBCon, fileName, DateProcessed) Then
                                    TotalRecords += 1
                                Else
                                    RecordErrors += 1
                                    If strMsg = "Success!" Then strMsg = ""
                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 997 message without success.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                    Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 997 Message Failed!" & strMsg)
                                End If
                            End If
                        End If
                    Next
                ElseIf Left(sg, 2) = "QM" Then 'this is a 214 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    Dim o214997 As New clsEDI997()
                    Dim int214Rec As Integer = 0
                    With o214997
                        .ST.ST01 = "997"
                        .ST.ST02 = "1"
                        .AK1.AK101 = "QM"
                        .AK1.AK102 = oGS.GS06
                    End With
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'Split out the 200 loops
                                Dim s200s() As String = Regex.Split(sSEs(0), "\" & strSegSep & "LX\*")
                                'the s200s(0) refers to the first part of the segment including any 100 loops
                                'so get the 100 loops
                                Dim s100s() As String = Regex.Split(s200s(0), "\" & strSegSep & "N1\*")
                                Dim o214 As New clsEDI214(s100s(0), strSegSep, Nothing)
                                o214.SE = oSE
                                If s100s.Length > 1 Then
                                    'we have some 100s so parse the 100 data
                                    Dim o100s(s100s.Length - 2) As clsEDI214Loop100
                                    For i100 = 1 To s100s.Length - 1
                                        o100s(i100 - 1) = New clsEDI214Loop100(s100s(i100), strSegSep)
                                    Next
                                    'add the 100 loop to the 214 object
                                    o214.Loop100 = o100s
                                End If
                                If s200s.Length > 1 Then
                                    'we have some 200s so parse the 200 data
                                    Dim o200s(s200s.Length - 2) As clsEDI214Loop200
                                    For i200 As Integer = 1 To s200s.Length - 1
                                        'split the 200 loop by L11 to give us the header, loop 205 records and the footer elements
                                        Dim s200HeaderFooter() As String = Regex.Split(s200s(i200), "\" & strSegSep & "L11\*")
                                        If s200HeaderFooter.Length > 1 Then
                                            'we have data so split out the 205 loop data
                                            Dim s205s() As String = Regex.Split(s200HeaderFooter(0), "\" & strSegSep & "AT7\*")
                                            o200s(i200 - 1) = New clsEDI214Loop200(s205s(0), strSegSep)
                                            'Create the 200 Footer data
                                            Dim s200Footer As String = ""
                                            For iCT As Integer = 1 To s200HeaderFooter.Count - 1
                                                If Left(s200HeaderFooter(iCT), 4) <> "L11*" Then
                                                    s200HeaderFooter(iCT) = "L11*" & s200HeaderFooter(iCT)
                                                End If
                                                o200s(i200 - 1).addFooterFromString(strSegSep, s200HeaderFooter(iCT))
                                            Next
                                            If s205s.Length > 1 Then
                                                'Loop through each 205 loop
                                                Dim o205s(s205s.Length - 2) As clsEDI214Loop205
                                                For i205 As Integer = 1 To s205s.Length - 1
                                                    o205s(i205 - 1) = New clsEDI214Loop205(s205s(i205), strSegSep)
                                                Next
                                                'add the 205 loop to the 200 Loop object
                                                o200s(i200 - 1).Loop205 = o205s
                                            End If
                                        End If
                                    Next
                                    'add the 200 Loop to the 214 object
                                    o214.Loop200 = o200s
                                End If
                                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                Dim insertErrorMsg As String = ""
                                'call the process data procedure
                                If Me.openConnection Then
                                    strMsg = "" 'Modified by RHR for v-7.0.6.105 on 6/13/2017
                                    'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                    If o214.processData(oISA, oGS, Me.DBServer, Me.Database, strMsg, DBCon, fileName, DateProcessed, insertErrorMsg) Then
                                        TotalRecords += 1
                                        int214Rec += 1
                                    Else
                                        RecordErrors += 1
                                        If strMsg = "Success!" Then strMsg = ""
                                        ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 214 message without success.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                        Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 214 Message Failed!" & strMsg)
                                    End If
                                    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                    If insertErrorMsg.Trim.Length > 0 Then
                                        ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIObjects.clsEDI214.processData, attempted to insert a 214 record into tblEDI214 without success.<br />" & vbCrLf & insertErrorMsg & "<br />" & vbCrLf
                                        Log("NGL.FreightMaster.Integration.clsEDIObjects.clsEDI214.processData 214 Data Warning! " & insertErrorMsg)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    'if we have 214s finish the 997 object
                    If int214Rec > 0 AndAlso CarrierEDIAcknowledgementRequested Then 'Modified By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
                        With o214997
                            .AK9.AK901 = "A"
                            .AK9.AK902 = int214Rec.ToString
                            .AK9.AK903 = int214Rec.ToString
                            .AK9.AK904 = int214Rec.ToString
                            .SE.SE01 = 4
                            .SE.SE02 = 1
                        End With
                        oR997s.Add(o214997)
                    End If
                ElseIf Left(sg, 2) = "GF" Then 'this is a 990 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    Dim o990997 As New clsEDI997()
                    Dim int990Rec As Integer = 0
                    With o990997
                        .ST.ST01 = "997"
                        .ST.ST02 = "1"
                        .AK1.AK101 = "GF"
                        .AK1.AK102 = oGS.GS06
                    End With
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim o990 As New clsEDI990(sSEs(0), strSegSep, sSEs(1))
                            'call the process Data function
                            strMsg = ""
                            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                            Dim insertErrorMsg As String = ""
                            If Me.openConnection Then
                                'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                If o990.processData(oISA, oGS, CarrierControl, Me.DBServer, Me.Database, strMsg, DBCon, fileName, DateProcessed, insertErrorMsg) Then
                                    TotalRecords += 1
                                    int990Rec += 1
                                Else
                                    RecordErrors += 1
                                    If strMsg = "Success!" Then strMsg = ""
                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 990 message without success.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                    Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 990 Message Failed!" & strMsg)
                                End If
                                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                If insertErrorMsg.Trim.Length > 0 Then
                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIObjects.clsEDI990.processData, attempted to insert a 990 record into tblEDI990 without success.<br />" & vbCrLf & insertErrorMsg & "<br />" & vbCrLf
                                    Log("NGL.FreightMaster.Integration.clsEDIObjects.clsEDI990.processData 990 Record Warning! " & insertErrorMsg)
                                End If
                            End If
                        End If
                    Next
                    'if we have 990s finish the 997 object
                    If int990Rec > 0 Then
                        With o990997
                            .AK9.AK901 = "A"
                            .AK9.AK902 = int990Rec.ToString
                            .AK9.AK903 = int990Rec.ToString
                            .AK9.AK904 = int990Rec.ToString
                            .SE.SE01 = 4
                            .SE.SE02 = 1
                        End With
                        oR997s.Add(o990997)
                    End If
                ElseIf Left(sg, 2) = "IM" Then 'this is a 210 so get the ST Segments  
                    '********Merge from 6.0.4.70*************
                    'Modified by RHR for v-6.0.4.7 on 10/20/2017 added  EDI 210 Config Settings
                    Dim obj210InSetting As New clsEDI210InSetting()
                    If Not String.IsNullOrWhiteSpace(sConfigFile) Then
                        If System.IO.File.Exists(sConfigFile) Then
                            Dim oSetting As New clsEDISettings()
                            'TODO: in v-8.0.1 add logic to pass in trading partner ID (From , To) and document type 
                            'to new dynamic constructer for reading EDI document settings from the db.
                            obj210InSetting = oSetting.readEDI210InSetting(sConfigFile)
                        End If
                    End If
                    '********Merge from 6.0.4.70*************
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    Dim oR997 As New clsEDI997()
                    Dim intDocsRec As Integer = 0
                    With oR997
                        .ST.ST01 = "997"
                        .ST.ST02 = "1"
                        .AK1.AK101 = "IM"
                        .AK1.AK102 = oGS.GS06
                    End With
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'NOTE: Each ST Section is split into sub sections for parsing.
                                '* Record identification (B3 and C3). 
                                '* N9 Loop Purchase Order, Bill of Lading or Bill To numbers.
                                '* General information (G62 and K1) like Dates and Notes 
                                '* Loop 100 (N1,N2,N3,N4) Address information.
                                '* Loop 200 (N7) Equipment Details.
                                '* Loop 400 (LX,L5, L0,L1) Fees and Charges.
                                '* Total Weights and Charges (L3).
                                'Split the B3 and C3 and N9 Loop using the G62 segment 
                                Dim sG62s() As String = Regex.Split(sSEs(0), "\" & strSegSep & "G62\*")
                                'the G62 element is required.
                                If sG62s.Length > 1 Then
                                    'Item 0 has the B3, C3 and N9 Loop segments.  Split out the N9 Loop segment
                                    Dim sN9s() As String = Regex.Split(sG62s(0), "\" & strSegSep & "N9\*")
                                    'The first item in the sN9s list contains the B3 and C3 data.  We use this to create the 210 object
                                    Dim o210 As New clsEDI210(sN9s(0), strSegSep, Nothing)
                                    '********Merge from 6.0.4.70*************
                                    'Modified by RHR for v-6.0.4.7 on 10/20/2017 added  EDI 210 Config Settings
                                    o210.EDI210InSetting = obj210InSetting
                                    '********Merge from 6.0.4.70*************
                                    'update/add the closing SE segment
                                    o210.SE = oSE
                                    'loop through all of the Remaining N9 segments and add them to the N9Loop
                                    If sN9s.Length > 1 Then
                                        Dim oN9s(sN9s.Length - 2) As clsEDI210N9Loop
                                        For iN9 = 1 To sN9s.Length - 1
                                            oN9s(iN9 - 1) = New clsEDI210N9Loop(sN9s(iN9), strSegSep)
                                        Next
                                        o210.LoopN9 = oN9s
                                    End If
                                    'The second item in the sG62s array contains the remainder of the data.  
                                    'We need to split the remaining items into a Loop 100 array.
                                    'The first item contains the G62 and K1 data.
                                    'The middle items each contain a loop 100 section.
                                    'The last item contains the final loop 100 section and the remaining record information.
                                    Dim segs() As String = Regex.Split(sG62s(1), "\" & strSegSep & "N1\*")
                                    'Add the G62 and K1 Data 
                                    o210.addG62andK1DataFromString(segs(0), strSegSep, Nothing)
                                    If segs.Length > 1 Then
                                        Dim o100s(segs.Length - 2) As clsEDI210Loop100
                                        For i100 = 1 To segs.Length - 2
                                            'add the middle items as loop 100 sections
                                            o100s(i100 - 1) = New clsEDI210Loop100(segs(i100), strSegSep)
                                        Next
                                        'Split out the last loop 100 segment from  the remaining data
                                        Dim s200s() As String = Regex.Split(segs(segs.Length - 1), "\" & strSegSep & "N7\*")
                                        Dim s400s() As String
                                        If s200s.Length > 1 Then
                                            'we have a 200 loop.                                            '
                                            'the first item is the last Loop 100 section
                                            o100s(segs.Length - 2) = New clsEDI210Loop100(s200s(0), strSegSep)
                                            Dim o200s(s200s.Length - 2) As clsEDI210Loop200
                                            For i200 = 1 To s200s.Length - 2
                                                'add the middle items to the loop 200 sections
                                                o200s(i200 - 1) = New clsEDI210Loop200(s200s(i200), strSegSep)
                                            Next
                                            'Split the last 200 loop from the remaining data
                                            s400s = Regex.Split(s200s(s200s.Length - 1), "\" & strSegSep & "LX\*")
                                            'the first item in the 400 loop is the last item in the Loop 200 section
                                            o200s(s200s.Length - 2) = New clsEDI210Loop200(s400s(0), strSegSep)
                                            'add the 200 loop to the 210 object
                                            o210.Loop200 = o200s
                                        Else
                                            'The 200 loop is optional so if we get here we move straight to the 400 loop
                                            s400s = Regex.Split(segs(segs.Length - 1), "\" & strSegSep & "LX\*")

                                            'the first item in the 400 loop is the last item in the Loop 100 section
                                            o100s(segs.Length - 2) = New clsEDI210Loop100(s400s(0), strSegSep)
                                        End If
                                        'add the 100 loop to the 210 object
                                        o210.Loop100 = o100s
                                        If s400s.Length > 1 Then
                                            Dim o400s(s400s.Length - 2) As clsEDI210Loop400
                                            For i400 = 1 To s400s.Length - 2
                                                'add the middle items to the loop 400 sections
                                                o400s(i400 - 1) = New clsEDI210Loop400(s400s(i400), strSegSep)
                                            Next
                                            'Split the last 200 loop from the remaining data
                                            Dim sL3s() As String = Regex.Split(s400s(s400s.Length - 1), "\" & strSegSep & "L3\*")
                                            'the first item is the last Loop 400 section
                                            o400s(s400s.Length - 2) = New clsEDI210Loop400(sL3s(0), strSegSep)
                                            'add the 400 loop to the 210 object
                                            o210.Loop400 = o400s
                                            If sL3s.Length > 1 Then
                                                'add the L3 segment
                                                If Left(sL3s(1), 3) <> "L3*" Then sL3s(1) = "L3*" & sL3s(1)
                                                o210.L3 = New clsEDIL3(sL3s(1))
                                            End If
                                            ''write the data to a file for debugging
                                            'Try
                                            '    o210.writeToFile(oISA, oGS)
                                            'Catch ex As Exception
                                            'End Try
                                            'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                            Dim insertErrorMsg As String = ""
                                            'call the process data procedure
                                            If Me.openConnection Then
                                                strMsg = "" 'Modified by RHR for v-7.0.6.105 on 6/13/2017
                                                'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                                If o210.processData(oISA, oGS, Me.DBServer, Me.Database, Me.AdminEmail, Me.GroupEmail, Me.SMTPServer, Me.FromEmail, strMsg, DBCon, fileName, DateProcessed, insertErrorMsg) Then
                                                    TotalRecords += 1
                                                    intDocsRec += 1
                                                    'Modified By LVV on 2/19/18 for v-8.1 TMS 365 PQ EDI  
                                                    'Need to log any messages returned from Freight Bill Processing
                                                    If Not String.IsNullOrWhiteSpace(strMsg) AndAlso strMsg <> "Success!" Then
                                                        ITEmailMsg &= "<br />" & Source & " v: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, processing 210 Freight Bill returned messages.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                                        Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 210 Freight Bill Processing Returned Message!" & strMsg)
                                                    End If
                                                Else
                                                    RecordErrors += 1
                                                    If strMsg = "Success!" Then strMsg = ""
                                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 210 message without success.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                                    Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 210 Message Failed!" & strMsg)
                                                End If
                                                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                                If insertErrorMsg.Trim.Length > 0 Then
                                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIObjects.clsEDI210.processData, attempted to insert a 210 record into tblEDI210 without success.<br />" & vbCrLf & insertErrorMsg & "<br />" & vbCrLf
                                                    Log("NGL.FreightMaster.Integration.clsEDIObjects.clsEDI210.processData 210 Record Insert Failed!" & insertErrorMsg)
                                                End If
                                            End If
                                        Else
                                            RecordErrors += 1
                                            If strMsg = "Success!" Then strMsg = ""
                                            ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 210 message without success.<br />" & vbCrLf & "The required LX Loop 400 segment could not be found.<br />" & vbCrLf
                                            Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 210 Message Failed! No LX segment found.")
                                            Exit For
                                        End If
                                    Else
                                        RecordErrors += 1
                                        If strMsg = "Success!" Then strMsg = ""
                                        ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 210 message without success.<br />" & vbCrLf & "The required N1 Loop 100 segment could not be found.<br />" & vbCrLf
                                        Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 210 Message Failed! No N1 segment found.")
                                        Exit For
                                    End If
                                Else
                                    RecordErrors += 1
                                    If strMsg = "Success!" Then strMsg = ""
                                    ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process a 210 message without success.<br />" & vbCrLf & "The required G62 segment could not be found.<br />" & vbCrLf
                                    Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 210 Message Failed! No G62 segment found.")
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                    Dim oWCFPar = New DAL.WCFParameters() With {.Database = Me.Database, .DBServer = Me.DBServer, .UserName = "EDI Integration", .WCFAuthCode = "NGLSystem"}
                    Dim oEDIData As New DAL.NGLEDIData(oWCFPar)
                    oEDIData.UpdateFeeLabels210In()
                    'if we have 210s finish the 997 object
                    If intDocsRec > 0 AndAlso CarrierEDIAcknowledgementRequested Then 'Added By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
                        With oR997
                            .AK9.AK901 = "A"
                            .AK9.AK902 = intDocsRec.ToString
                            .AK9.AK903 = intDocsRec.ToString
                            .AK9.AK904 = intDocsRec.ToString
                            .SE.SE01 = 4
                            .SE.SE02 = 1
                        End With
                        oR997s.Add(oR997)
                    End If
                    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                ElseIf Left(sg, 2) = "RA" Then 'this is an 820 so get the ST Segments    
                    'stip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                Dim o820 As New clsEDI820
                                o820.SE = oSE
                                'Split out the RMR loops
                                Dim sRMRs() As String = Regex.Split(sSEs(0), "\" & strSegSep & "RMR\*")
                                'Get the DTM
                                Dim sDTMs() As String = Regex.Split(sRMRs(0), "\" & strSegSep & "DTM\*")
                                Dim sN1s() As String = Regex.Split(sDTMs(1), "\" & strSegSep & "N1\*")
                                Dim DTM As New clsEDIDTM(sN1s(0))
                                o820.DTM = DTM
                                'Get the header REF
                                Dim sHREFs() As String = Regex.Split(sDTMs(0), "\" & strSegSep & "REF\*")
                                Dim HRef As New clsEDIREF(sHREFs(1))
                                o820.REF = HRef
                                'Get the RMRs
                                Dim RMRList As New List(Of clsEDIRMR)
                                For j = 1 To (sRMRs.Length - 1)
                                    Dim sREFs() As String = Regex.Split(sRMRs(j), "\" & strSegSep)
                                    Dim RMR As New clsEDIRMR(sREFs(0))
                                    RMRList.Add(RMR)
                                Next
                                o820.LoopRMR = RMRList.ToArray()

                                'call the process data procedure
                                If Me.openConnection Then
                                    strMsg = ""
                                    If Not o820.processData(oISA, oGS, Me.DBServer, Me.Database, RecordErrors, TotalRecords, fileName) Then
                                        If strMsg = "Success!" Then strMsg = ""
                                        ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI, attempted to process an 820 message without success.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                        Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 820 Message Failed!" & strMsg)
                                    End If
                                End If
                            End If
                        End If
                    Next
                    'Modified by RHR for v-7.0.6.105 6/2/2017  process 204 inbound doucments
                ElseIf Left(sg, 2) = "SM" Then 'this is a 204 so get the ST Segments  
                    Dim obj204inSetting As New clsEDI204InSetting()
                    If Not String.IsNullOrWhiteSpace(sConfigFile) Then
                        Dim oSetting As New clsEDISettings()
                        obj204inSetting = oSetting.readEDI204InSetting(sConfigFile)
                    End If
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    Dim oR997 As New clsEDI997()
                    Dim int204InRec As Integer = 0
                    ' Modified by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/7/217
                    '   provides a way for EDI 204 ins to send a 997
                    With oR997
                        .ST.ST01 = "997"
                        .ST.ST02 = "1"
                        .AK1.AK101 = "SM"
                        .AK1.AK102 = oGS.GS06
                        .ExternalSecurityQualifier = oISA.ISA01 'maps to the Comp Security qual
                        .ExternalSecurityCode = oISA.ISA02  'maps to the Comp security Code
                        .ExternalPartnerQualifier = oISA.ISA05 'maps to the Comp partner qual
                        .ExternalPartnerCode = oISA.ISA06 'maps to the Comp partner code
                        .InternalSecurityQualifier = oISA.ISA03 'maps to the carrier security qual
                        .InternalSecurityCode = oISA.ISA04 'maps to the carrier security code
                        .InternalPartnerQualifier = oISA.ISA07 'maps to the Carrier partner qual
                        .InternalPartnerCode = oISA.ISA08 'maps to the carrier partner code
                    End With

                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'Begin'Modified by RHR for v-7.0.6.105 on 5/12/2017  EDI 204 Migration
                                Dim o204In As New clsEDI204In(Me.oConfig)
                                'split the remaining elements in the sSEs(0) into a list of string and call insertElements
                                Dim strData = "ST*" & sSEs(0)
                                Dim strElements = strData.Split("~")
                                Dim o204Data As New Ngl.FreightMaster.Integration.clsEDI204In(oConfig)
                                o204Data.EDI204InSetting = obj204inSetting
                                Dim intIndex As Integer = 0
                                o204In.insertElements(o204Data, strElements, intIndex)
                                If Not o204Data Is Nothing Then
                                    o204Data.SE = oSE
                                    Dim insertErrorMsg As String = ""
                                    strMsg = ""
                                    Dim StatusCode As DTO.tblLoadTender.LoadTenderStatusCodeEnum = o204Data.processData204InSingle(oISA, oGS, CarrierControl, strMsg, fileName, DateProcessed, insertErrorMsg)
                                    'Modified by RHR for v-6.0.4.7 on 6/9/2017
                                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                                        If strMsg <> "Success!" Then
                                            GroupEmailMsg &= "<br />" & Source & " Warning: Attempted to process a 204 Inbound message without success for file: <br /> " & vbCrLf & fileName & vbCrLf & "<br />  Details: <br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                            ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI attempted to process a 204 Inbound message without success for file: <br /> " & vbCrLf & fileName & vbCrLf & "<br />  Details: <br />" & vbCrLf & strMsg & "<br />" & vbCrLf
                                            Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI 204In Message Failed for file: " & fileName & "  Details: " & strMsg)
                                        End If
                                    End If
                                    If Not String.IsNullOrWhiteSpace(insertErrorMsg) Then
                                        ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsEDIInput.parseEDI failed to save results data in tblEDI204In for a 204 Inbound message.<br />" & vbCrLf & insertErrorMsg & "<br />" & vbCrLf & "<br /> File Name: " & fileName & vbCrLf
                                        Log("NGL.FreightMaster.Integration.clsEDIInput.parseEDI failed to save results data in tblEDI204In for a 204 Inbound message!" & insertErrorMsg & "  File Name: " & fileName)
                                    End If
                                    Select Case StatusCode
                                        Case DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Accept
                                            'Todo: add logic to send 990 Accept
                                            TotalRecords += 1
                                            int204InRec += 1
                                        Case DTO.tblLoadTender.LoadTenderStatusCodeEnum.EDI990Reject
                                            'Todo: add logic to send 990 Reject
                                            TotalRecords += 1
                                            int204InRec += 1
                                    End Select
                                End If
                                'End Modified by RHR for v-7.0.6.105 on 5/12/2017  EDI 204 Migration
                            End If
                        End If
                    Next
                    If int204InRec > 0 Then
                        'we have 204s so finish the 997 object
                        With oR997
                            .AK9.AK901 = "A"
                            .AK9.AK902 = int204InRec.ToString
                            .AK9.AK903 = int204InRec.ToString
                            .AK9.AK904 = int204InRec.ToString
                            .SE.SE01 = 4
                            .SE.SE02 = 1
                        End With
                        oR997s.Add(oR997)
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''
                End If
            Next
            If Not oR997s Is Nothing AndAlso oR997s.Count > 0 Then
                Me._EDI997Response &= get997sForCarrier(oR997s, oISA)
                oR997s = New List(Of clsEDI997)
            End If
        Next
    End Sub

    Public Sub TestparseEDI214(ByVal e As String, ByRef strLogMsg As String)
        '1st read first 106 characters
        Dim strISA As String = Left(e, 106)
        Dim strSegSep As String = Right(strISA, 1)
        'now split any ISA sections
        Dim sISAs() As String = Regex.Split(e, "\" & strSegSep & "ISA\*")
        'now we have a list of ISAs so split each ISAs by GS
        Dim strISAHeader As String = ""
        Dim blnISAHeaderAdded As Boolean = False
        Dim strGSHeader As String = ""
        Dim blnGSHeaderAdded As Boolean = False
        Dim oISA As clsEDIISA
        Dim oGS As clsEDIGS
        Dim strMsg As String = ""
        'create a new 997 object for sending message acknowledgments
        Dim oR997s As New List(Of clsEDI997)
        'Check if the require CNS Flag is turned on.
        For Each s In sISAs
            'Add the ISA to the array we have to add the ISA* string back in if it does not exist
            If Left(s, 4) = "ISA*" Then
                strISAHeader = Left(s, 105)
            Else
                strISAHeader = "ISA*" & Left(s, 101)
            End If
            oISA = New clsEDIISA(strISAHeader)
            oISA.SegmentTerminator = strSegSep
            'strip off the IEA segment
            Dim sIEAs() As String = Regex.Split(s, "\" & strSegSep & "IEA\*")
            'get the GS data
            Dim sGSs() As String = Regex.Split(sIEAs(0), "\" & strSegSep & "GS\*")
            For i = 1 To sGSs.Length - 1 'we skip the first item because it holds the ISA data
                Dim sg As String = sGSs(i)
                If Left(sg, 2) = "QM" Then 'this is a 214 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'Split out the 200 loops
                                Dim s200s() As String = Regex.Split(sSEs(0), "\" & strSegSep & "LX\*")
                                'the s200s(0) refers to the first part of the segment including any 100 loops
                                'so get the 100 loops
                                Dim s100s() As String = Regex.Split(s200s(0), "\" & strSegSep & "N1\*")
                                Dim o214 As New clsEDI214(s100s(0), strSegSep, Nothing)
                                o214.SE = oSE
                                If s100s.Length > 1 Then
                                    'we have some 100s so parse the 100 data
                                    Dim o100s(s100s.Length - 2) As clsEDI214Loop100
                                    For i100 = 1 To s100s.Length - 1
                                        o100s(i100 - 1) = New clsEDI214Loop100(s100s(i100), strSegSep)
                                    Next
                                    'add the 100 loop to the 214 object
                                    o214.Loop100 = o100s
                                End If
                                If s200s.Length > 1 Then
                                    'we have some 200s so parse the 200 data
                                    Dim o200s(s200s.Length - 2) As clsEDI214Loop200
                                    For i200 As Integer = 1 To s200s.Length - 1
                                        'split the 200 loop by L11 to give us the header, loop 205 records and the footer elements
                                        Dim s200HeaderFooter() As String = Regex.Split(s200s(i200), "\" & strSegSep & "L11\*")
                                        If s200HeaderFooter.Length > 1 Then
                                            'we have data so split out the 205 loop data
                                            Dim s205s() As String = Regex.Split(s200HeaderFooter(0), "\" & strSegSep & "AT7\*")
                                            o200s(i200 - 1) = New clsEDI214Loop200(s205s(0), strSegSep)
                                            'Create the 200 Footer data
                                            Dim s200Footer As String = ""
                                            For iCT As Integer = 1 To s200HeaderFooter.Count - 1
                                                If Left(s200HeaderFooter(iCT), 4) <> "L11*" Then
                                                    s200HeaderFooter(iCT) = "L11*" & s200HeaderFooter(iCT)
                                                End If
                                                o200s(i200 - 1).addFooterFromString(strSegSep, s200HeaderFooter(iCT))
                                            Next
                                            If s205s.Length > 1 Then
                                                'Loop through each 205 loop
                                                Dim o205s(s205s.Length - 2) As clsEDI214Loop205
                                                For i205 As Integer = 1 To s205s.Length - 1
                                                    o205s(i205 - 1) = New clsEDI214Loop205(s205s(i205), strSegSep)
                                                Next
                                                'add the 205 loop to the 200 Loop object
                                                o200s(i200 - 1).Loop205 = o205s
                                            End If
                                        End If
                                    Next
                                    'add the 200 Loop to the 214 object
                                    o214.Loop200 = o200s
                                End If
                                'call the process data procedure
                                If Me.openConnection Then
                                    If o214.processData(oISA, oGS, Me.DBServer, Me.Database, strMsg, DBCon) Then
                                        TotalRecords += 1
                                    Else
                                        RecordErrors += 1
                                        If strMsg = "Success!" Then strMsg = ""
                                        strLogMsg &= "NGL.FreightMaster.Integration.clsEDIInput.parseEDI 214 Message Failed!" & strMsg
                                    End If
                                End If
                            End If
                        End If
                    Next

                End If
            Next
        Next
    End Sub

    Public Function testEDI(ByVal e As String, Optional ByVal dblRequireCNS As Double = 1) As String
        Dim strRet As String = ""
        '1st read first 106 characters
        Dim strISA As String = Left(e, 106)
        Dim strSegSep As String = Right(strISA, 1)
        'now split any ISA sections
        Dim sISAs() As String = Regex.Split(e, "\" & strSegSep & "ISA\*")
        'now we have a list of ISAs so split each ISAs by GS
        Dim strISAHeader As String = ""
        Dim blnISAHeaderAdded As Boolean = False
        Dim strGSHeader As String = ""
        Dim blnGSHeaderAdded As Boolean = False
        Dim oISA As clsEDIISA
        Dim oGS As clsEDIGS
        Dim strMsg As String = ""

        For Each s In sISAs
            'Add the ISA to the array we have to add the ISA* string back in if it does not exist
            If Left(s, 4) = "ISA*" Then
                strISAHeader = Left(s, 105)
            Else
                strISAHeader = "ISA*" & Left(s, 101)
            End If
            oISA = New clsEDIISA(strISAHeader)
            oISA.SegmentTerminator = strSegSep
            'strip off the IEA segment
            Dim sIEAs() As String = Regex.Split(s, "\" & strSegSep & "IEA\*")
            'get the GS data
            Dim sGSs() As String = Regex.Split(sIEAs(0), "\" & strSegSep & "GS\*")
            For i = 1 To sGSs.Length - 1 'we skip the first item because it holds the ISA data
                Dim sg As String = sGSs(i)
                If Left(sg, 2) = "FA" Then 'this is a 997 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim o997 As New clsEDI997(sSEs(0), strSegSep, sSEs(1))
                            strRet &= o997.getRecord(oISA, oGS)
                            TotalRecords += 1
                        End If
                    Next
                ElseIf Left(sg, 2) = "SM" Then 'this is a 204 so just split the segments for now
                    'we do not have code to parse the 204 so we just format it 
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim sElems() As String = sSEs(0).Split(strSegSep)
                            Dim sb As New System.Text.StringBuilder()
                            For Each e In sElems
                                sb.Append(e)
                                sb.Append(vbCrLf)
                            Next
                            strRet = sb.ToString()

                            TotalRecords += 1
                        End If
                    Next
                ElseIf Left(sg, 2) = "QM" Then 'this is a 214 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'Split out the 200 loops
                                Dim s200s() As String = Regex.Split(sSEs(0), "\" & strSegSep & "LX\*")
                                'the s200s(0) refers to the first part of the segment including any 100 loops
                                'so get the 100 loops
                                Dim s100s() As String = Regex.Split(s200s(0), "\" & strSegSep & "N1\*")
                                Dim o214 As New clsEDI214(s100s(0), strSegSep, Nothing)
                                o214.SE = oSE
                                If s100s.Length > 1 Then
                                    'we have some 100s so parse the 100 data
                                    Dim o100s(s100s.Length - 2) As clsEDI214Loop100
                                    For i100 = 1 To s100s.Length - 1
                                        o100s(i100 - 1) = New clsEDI214Loop100(s100s(i100), strSegSep)
                                    Next
                                    'add the 100 loop to the 214 object
                                    o214.Loop100 = o100s
                                End If
                                If s200s.Length > 1 Then
                                    'we have some 200s so parse the 200 data
                                    Dim o200s(s200s.Length - 2) As clsEDI214Loop200
                                    For i200 As Integer = 1 To s200s.Length - 1
                                        'split the 200 loop by L11 to give us the header, loop 205 records and the footer elements
                                        Dim s200HeaderFooter() As String = Regex.Split(s200s(i200), "\" & strSegSep & "L11\*")
                                        If s200HeaderFooter.Length > 1 Then
                                            'we have data so split out the 205 loop data
                                            Dim s205s() As String = Regex.Split(s200HeaderFooter(0), "\" & strSegSep & "AT7\*")
                                            o200s(i200 - 1) = New clsEDI214Loop200(s205s(0), strSegSep)
                                            'Create the 200 Footer data
                                            Dim s200Footer As String = ""
                                            For iCT As Integer = 1 To s200HeaderFooter.Count - 1
                                                If Left(s200HeaderFooter(iCT), 4) <> "L11*" Then
                                                    s200HeaderFooter(iCT) = "L11*" & s200HeaderFooter(iCT)
                                                End If
                                                o200s(i200 - 1).addFooterFromString(strSegSep, s200HeaderFooter(iCT))
                                            Next
                                            If s205s.Length > 1 Then
                                                'Loop through each 205 loop
                                                Dim o205s(s205s.Length - 2) As clsEDI214Loop205
                                                For i205 As Integer = 1 To s205s.Length - 1
                                                    o205s(i205 - 1) = New clsEDI214Loop205(s205s(i205), strSegSep)
                                                Next
                                                'add the 205 loop to the 200 Loop object
                                                o200s(i200 - 1).Loop205 = o205s
                                            End If
                                        End If
                                    Next
                                    'add the 200 Loop to the 214 object
                                    o214.Loop200 = o200s
                                End If
                                strRet &= o214.getRecord(oISA, oGS)
                                strRet &= vbCrLf & vbCrLf & o214.processDataTest(oISA, oGS, strMsg, dblRequireCNS)
                                TotalRecords += 1
                            End If
                        End If
                    Next
                ElseIf Left(sg, 2) = "GF" Then 'this is a 990 so get the ST Segments                    
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim o990 As New clsEDI990(sSEs(0), strSegSep, sSEs(1))
                            strRet &= o990.getRecord(oISA, oGS)
                            TotalRecords += 1
                        End If
                    Next
                ElseIf Left(sg, 2) = "IM" Then 'this is a 210 so get the ST Segments                   
                    'strip off the GE segment
                    Dim sGEs() As String = Regex.Split(sg, "\" & strSegSep & "GE\*")
                    'split the records by ST
                    Dim sSTs() As String = Regex.Split(sGEs(0), "\" & strSegSep & "ST\*")
                    oGS = New clsEDIGS("GS*" & sSTs(0))
                    Dim oR997 As New clsEDI997()
                    Dim intDocsRec As Integer = 0
                    With oR997
                        .ST.ST01 = "997"
                        .ST.ST02 = "1"
                        .AK1.AK101 = "IM"
                        .AK1.AK102 = oGS.GS06
                    End With
                    'now split each ST record
                    For ist = 1 To sSTs.Length - 1
                        Dim st As String = sSTs(ist)
                        Dim sSEs() As String = Regex.Split(st, "\" & strSegSep & "SE\*")
                        If sSEs.Length > 1 Then
                            Dim oSE = New clsEDISE("SE*" & sSEs(1))
                            Dim intElements As Integer = 0
                            If Integer.TryParse(oSE.SE01, intElements) Then
                                'NOTE: Each ST Section is split into sub sections for parsing.
                                '* Record identification (B3 and C3). 
                                '* N9 Loop Purchase Order, Bill of Lading or Bill To numbers.
                                '* General information (G62 and K1) like Dates and Notes 
                                '* Loop 100 (N1,N2,N3,N4) Address information.
                                '* Loop 200 (N7) Equipment Details.
                                '* Loop 400 (LX,L5, L0,L1) Fees and Charges.
                                '* Total Weights and Charges (L3).
                                'Split the B3 and C3 and N9 Loop using the G62 segment 
                                Dim sG62s() As String = Regex.Split(sSEs(0), "\" & strSegSep & "G62\*")
                                'the G62 element is required.
                                If sG62s.Length > 1 Then
                                    'Item 0 has the B3, C3 and N9 Loop segments.  Split out the N9 Loop segment
                                    Dim sN9s() As String = Regex.Split(sG62s(0), "\" & strSegSep & "N9\*")
                                    'The first item in the sN9s list contains the B3 and C3 data.  We use this to create the 210 object
                                    Dim o210 As New clsEDI210(sN9s(0), strSegSep, Nothing)
                                    'update/add the closing SE segment
                                    o210.SE = oSE
                                    'loop through all of the Remaining N9 segments and add them to the N9Loop
                                    If sN9s.Length > 1 Then
                                        Dim oN9s(sN9s.Length - 2) As clsEDI210N9Loop
                                        For iN9 = 1 To sN9s.Length - 1
                                            oN9s(iN9 - 1) = New clsEDI210N9Loop(sN9s(iN9), strSegSep)
                                        Next
                                        o210.LoopN9 = oN9s
                                    End If
                                    'The second item in the sG62s array contains the remainder of the data.  
                                    'We need to split the remaining items into a Loop 100 array.
                                    'The first item contains the G62 and K1 data.
                                    'The middle items each contain a loop 100 section.
                                    'The last item contains the final loop 100 section and the remaining record information.
                                    Dim segs() As String = Regex.Split(sG62s(1), "\" & strSegSep & "N1\*")
                                    'Add the G62 and K1 Data 
                                    o210.addG62andK1DataFromString(segs(0), strSegSep, Nothing)
                                    If segs.Length > 1 Then
                                        Dim o100s(segs.Length - 2) As clsEDI210Loop100
                                        For i100 = 1 To segs.Length - 2
                                            'add the middle items as loop 100 sections
                                            o100s(i100 - 1) = New clsEDI210Loop100(segs(i100), strSegSep)
                                        Next
                                        'Split out the last loop 100 segment from  the remaining data
                                        Dim s200s() As String = Regex.Split(segs(segs.Length - 1), "\" & strSegSep & "N7\*")
                                        Dim s400s() As String
                                        If s200s.Length > 1 Then
                                            'we have a 200 loop.                                            '
                                            'the first item is the last Loop 100 section
                                            o100s(segs.Length - 2) = New clsEDI210Loop100(s200s(0), strSegSep)
                                            Dim o200s(s200s.Length - 2) As clsEDI210Loop200
                                            For i200 = 1 To s200s.Length - 2
                                                'add the middle items to the loop 200 sections
                                                o200s(i200 - 1) = New clsEDI210Loop200(s200s(i200), strSegSep)
                                            Next
                                            'Split the last 200 loop from the remaining data
                                            s400s = Regex.Split(s200s(s200s.Length - 1), "\" & strSegSep & "LX\*")
                                            'the first item in the 400 loop is the last item in the Loop 200 section
                                            o200s(s200s.Length - 2) = New clsEDI210Loop200(s400s(0), strSegSep)
                                            'add the 200 loop to the 210 object
                                            o210.Loop200 = o200s
                                        Else
                                            'The 200 loop is optional so if we get here we move straight to the 400 loop
                                            s400s = Regex.Split(segs(segs.Length - 1), "\" & strSegSep & "LX\*")

                                            'the first item in the 400 loop is the last item in the Loop 100 section
                                            o100s(segs.Length - 2) = New clsEDI210Loop100(s400s(0), strSegSep)
                                        End If
                                        'add the 100 loop to the 210 object
                                        o210.Loop100 = o100s
                                        If s400s.Length > 1 Then
                                            Dim o400s(s400s.Length - 2) As clsEDI210Loop400
                                            For i400 = 1 To s400s.Length - 2
                                                'add the middle items to the loop 400 sections
                                                o400s(i400 - 1) = New clsEDI210Loop400(s400s(i400), strSegSep)
                                            Next
                                            'Split the last 200 loop from the remaining data
                                            Dim sL3s() As String = Regex.Split(s400s(s400s.Length - 1), "\" & strSegSep & "L3\*")
                                            'the first item is the last Loop 400 section
                                            o400s(s400s.Length - 2) = New clsEDI210Loop400(sL3s(0), strSegSep)
                                            'add the 400 loop to the 210 object
                                            o210.Loop400 = o400s
                                            If sL3s.Length > 1 Then
                                                'add the L3 segment
                                                If Left(sL3s(1), 3) <> "L3*" Then sL3s(1) = "L3*" & sL3s(1)
                                                o210.L3 = New clsEDIL3(sL3s(1))
                                            End If
                                            strRet &= o210.getRecord(oISA, oGS)
                                            TotalRecords += 1

                                        Else
                                            RecordErrors += 1
                                            strRet &= "Warning: NGL.FreightMaster.Integration.clsEDIInput.testEDI, attempted to process a 210 message without success." & vbCrLf & "The required LX Loop 400 segment could not be found." & vbCrLf
                                            Exit For
                                        End If
                                    Else
                                        RecordErrors += 1
                                        strRet &= "Warning: NGL.FreightMaster.Integration.clsEDIInput.testEDI, attempted to process a 210 message without success." & vbCrLf & "The required N1 Loop 100 segment could not be found." & vbCrLf
                                        Exit For
                                    End If
                                Else
                                    RecordErrors += 1
                                    strRet &= "Warning: NGL.FreightMaster.Integration.clsEDIInput.testEDI, attempted to process a 210 message without success." & vbCrLf & "The required G62 segment could not be found." & vbCrLf
                                    Exit For
                                End If
                            End If
                        End If
                    Next

                End If
            Next
        Next
        Return strRet
    End Function

    ''' <summary>
    ''' Read the Carrier EDI configuration and prepare the 997 EDI document
    ''' </summary>
    ''' <param name="o997s"></param>
    ''' <param name="OrigISA"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modifed by RHR for v-7.0.6.105 on 6/8/2017
    '''   added logic to use 997 configuration settings for 204 inbound data
    ''' </remarks>
    Protected Function get997sForCarrier(ByRef o997s As List(Of clsEDI997), ByRef OrigISA As clsEDIISA) As String
        If o997s Is Nothing OrElse o997s.Count < 1 Then Return "" 'nothing to do
        Dim oCarrierEDIBLL As New clsCarrierEDIBLL
        Dim oISA As New clsEDIISA
        Dim oIEA As New clsEDIIEA
        Dim oGS As New clsEDIGS
        Dim oGE As New clsEDIGE
        Dim intSegCounter As Integer = 0
        Dim EDIString As String = ""
        'share the current settings including the db connection
        oCarrierEDIBLL.shareSettings(Me)
        'Modifed by RHR for v-7.0.6.105 on 6/8/2017
        If String.IsNullOrWhiteSpace(o997s(0).InternalPartnerCode) Then
            Dim blnDoesConfigExist As New Boolean 'Modified by LVV on 10/10/19 - do not log an error if the 997 config does not exist
            'fill EDI Objects loads the ISA and GS objects with data by partner code
            If Not oCarrierEDIBLL.fillEDIObjects(oISA, oGS, OrigISA.ISA06, "997", blnDoesConfigExist, False) Then
                If blnDoesConfigExist Then LogError("Cannot read Carrier EDI Settings", "The 997 EDI Settings for Partner Code , " & OrigISA.ISA06 & " are not valid." & vbCrLf & oCarrierEDIBLL.LastError, Me.AdminEmail)
                Return ""
            End If
            'update the company settings 
            With oISA
                .ISA01 = OrigISA.ISA03
                .ISA02 = OrigISA.ISA04
                .ISA05 = OrigISA.ISA07
                .ISA06 = OrigISA.ISA08
                EDIString &= .getEDIString(oISA.SegmentTerminator)
            End With
            With oGS
                .GS02 = OrigISA.ISA08
                EDIString &= .getEDIString(oISA.SegmentTerminator)
            End With
        Else
            oCarrierEDIBLL.fillEDIObjects(oISA, oGS, o997s(0), "997")
            EDIString &= oISA.getEDIString(oISA.SegmentTerminator)
            EDIString &= oGS.getEDIString(oISA.SegmentTerminator)
        End If

        'add the 997s
        For Each o997 In o997s
            intSegCounter += 1
            EDIString &= o997.getEDIString(oISA.SegmentTerminator)
        Next
        'close the GS with a GE
        With oGE
            .GE01 = intSegCounter.ToString
            .GE02 = oGS.GS06
            EDIString &= .getEDIString(oISA.SegmentTerminator)
        End With
        'close the ISA with and ISE
        With oIEA
            .IEA01 = 1
            .IEA02 = oISA.ISA13
            EDIString &= .getEDIString(oISA.SegmentTerminator)
        End With
        Return EDIString
    End Function


End Class
