Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports CoreUtility = Ngl.Core.Utility

'Added by LVV 7/1/16 for v-7.0.5.110 DAT

Namespace DataTransferObjects
    <DataContract()> _
    Public Class DATResults
        Inherits DTOBaseClass

#Region "Enums"

        Public Enum MessageType
            Errors
            Warnings
            Messages
        End Enum

        Public Enum MessageEnum
            None
            Success '"Success"       
            E_UnExpected_Error '"An Unexpected Error Has Occurred: {0}.  You should manually refresh your data to be sure you have the latest changes."
            E_NotFoundSSOAByUser '"No Single Sign On Accounts exist for selected user."
            M_Success '"Success!"
            E_InvalidSSOAName '"No record exists in the database with SSOAName: {0}."
            E_InvalidParameterNameValue 'Invalid Parameter: No record exists in the database for {0}: {1}.
            E_RequiredFieldDATEquipType 'The DATEquipType field is required and cannot be null. Please check that a DATEquipType mapping exists for all Temp Type Codes in the Lane Code Maintenance Window and that the Company Level Parameter DATDefaultMixTempType is populated correctly.
            W_UserNoDATAccount 'User {0} does not have an SSOA account set up for DAT
            E_DATGeneralRetMsg '{0}
            E_DATInvalidFeature 'Failure - Could not execute DAT program. Invalid DAT Feature {0}
            E_DATDeletePostServiceError 'DAT Delete Post Failed -- {0}
            E_DATPostFailedServiceError 'DAT Post Failed -- {0}
            E_DATDeleteFailedNoUser 'Could not delete{0}load from DAT: No DAT Account found for User {1} using UserSecurityControl {2} and SSOAControl 2.
            E_NoUserSecurityForUser 'Could not find User Security record using UserName: {0}
            E_NoLoadTenderForBook 'Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0.
            E_DATDeleteStillTenderedToCarrier '{3}: Warning - The Load with SHID {0} is still tendered to Carrier {1} {2} in TMS but is no longer Posted on the {3} Load Board.
            E_DATExpireFail '{2}: Warning - The Load with SHID {0} is expired but could not be reset to N Status as there was a problem with the Save. The {2} Posting with Reference ID {1} was not deleted from the {2} Load Board.
            E_DATDeleteFail '{2}: Warning - The Load with SHID {0} could not be reset to N Status as there was a problem with the Save. The {2} Posting with Reference ID {1} was not deleted from the {2} Load Board.
            E_DATSpFailNoLTControl 'The stored procedure spGetDATData did not return a valid LoadTenderControl.
            E_DATDPostFail '{1}: Warning - The {1} Posting with Reference ID {0} was successfully posted to the {1} Load Board. However, the Load with could not be assigned to the Load Board Carrier and set to PC status as there was a problem with the Save.
            E_DATUpdatePostFailedServiceError 'DAT Update Post Failed for SHID: {0} and DATRefID: {1} -- {2}
            E_LBSpFailNoLTControl 'The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl.
            E_LoadBoardSpFailNoLTControl 'The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl.
            E_LBDeleteFailedNoUser 'Could not delete{0}load from {3}: No {3} Account found for User {1} using UserSecurityControl {2}.
        End Enum

#End Region

#Region "Data Members"

        Private _LTControl As Integer = 0
        <DataMember()> _
        Public Property LTControl() As Integer
            Get
                Return _LTControl
            End Get
            Set(ByVal value As Integer)
                _LTControl = value
            End Set
        End Property

        Private _LTTypeControl As Integer = 0
        <DataMember()> _
        Public Property LTTypeControl() As Integer
            Get
                Return _LTTypeControl
            End Get
            Set(ByVal value As Integer)
                _LTTypeControl = value
            End Set
        End Property

        Private _LTTypeName As String = ""
        <DataMember()> _
        Public Property LTTypeName() As String
            Get
                Return _LTTypeName
            End Get
            Set(ByVal value As String)
                _LTTypeName = value
            End Set
        End Property

        Private _LTStatusCode As tblLoadTender.LoadTenderStatusCodeEnum = tblLoadTender.LoadTenderStatusCodeEnum.None
        <DataMember()> _
        Public Property LTStatusCode() As tblLoadTender.LoadTenderStatusCodeEnum
            Get
                Return _LTStatusCode
            End Get
            Set(ByVal value As tblLoadTender.LoadTenderStatusCodeEnum)
                _LTStatusCode = value
            End Set
        End Property

        Private _LTMessage As String = ""
        <DataMember()> _
        Public Property LTMessage() As String
            Get
                Return _LTMessage
            End Get
            Set(ByVal value As String)
                _LTMessage = value
            End Set
        End Property

        Private _LTDATRefID As String = ""
        <DataMember()> _
        Public Property LTDATRefID() As String
            Get
                Return _LTDATRefID
            End Get
            Set(ByVal value As String)
                _LTDATRefID = value
            End Set
        End Property

        Private _LTBookControl As Integer = 0
        <DataMember()> _
        Public Property LTBookControl() As Integer
            Get
                Return _LTBookControl
            End Get
            Set(ByVal value As Integer)
                _LTBookControl = value
            End Set
        End Property

        Private _LTBookSHID As String = ""
        <DataMember()> _
        Public Property LTBookSHID() As String
            Get
                Return Left(_LTBookSHID, 50)
            End Get
            Set(ByVal value As String)
                _LTBookSHID = Left(value, 50)
            End Set
        End Property

        Private _LTCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property LTCarrierNumber() As Integer
            Get
                Return _LTCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _LTCarrierNumber = value
            End Set
        End Property

        Private _LTCarrierName As String = ""
        <DataMember()> _
        Public Property LTCarrierName() As String
            Get
                Return Left(_LTCarrierName, 40)
            End Get
            Set(ByVal value As String)
                _LTCarrierName = Left(value, 40)
            End Set
        End Property

        Private _LTCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LTCarrierControl() As Integer
            Get
                Return _LTCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LTCarrierControl = value
            End Set
        End Property

        Private _LTBookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property LTBookCustCompControl() As Integer
            Get
                Return _LTBookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _LTBookCustCompControl = value
            End Set
        End Property

        Private _LTCompName As String = ""
        <DataMember()> _
        Public Property LTCompName() As String
            Get
                Return Left(_LTCompName, 40)
            End Get
            Set(ByVal value As String)
                _LTCompName = Left(value, 40)
            End Set
        End Property

        Private _UserName As String = ""
        <DataMember()> _
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Private _BookTrackComment As String = ""
        <DataMember()> _
        Public Property BookTrackComment() As String
            Get
                Return _BookTrackComment
            End Get
            Set(ByVal value As String)
                _BookTrackComment = value
            End Set
        End Property

        Private _BookTrackStatus As Integer = 0
        <DataMember()> _
        Public Property BookTrackStatus() As Integer
            Get
                Return _BookTrackStatus
            End Get
            Set(ByVal value As Integer)
                _BookTrackStatus = value
            End Set
        End Property

        Private _Success As Boolean = False
        <DataMember()> _
        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Private _Errors As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()> _
        Public Property Errors() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Errors
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Errors = value
            End Set
        End Property

        Private _Warnings As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()> _
        Public Property Warnings() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Warnings
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Warnings = value
            End Set
        End Property

        Private _Messages As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()> _
        Public Property Messages() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Messages
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Messages = value
            End Set
        End Property

        Private _Log As New List(Of NGLMessage)
        <DataMember()> _
        Public Property Log() As List(Of NGLMessage)
            Get
                Return _Log
            End Get
            Set(ByVal value As List(Of NGLMessage))
                _Log = value
            End Set
        End Property

#End Region

#Region "Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New DATResults
            instance = DirectCast(MemberwiseClone(), DTOBaseClass)

            If Not Errors Is Nothing AndAlso Errors.Count > 0 Then
                instance.Errors = (From x In Errors Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            If Not Warnings Is Nothing AndAlso Warnings.Count > 0 Then
                instance.Warnings = (From x In Warnings Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            If Not Messages Is Nothing AndAlso Messages.Count > 0 Then
                instance.Messages = (From x In Messages Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If

            Return instance
        End Function

        Public Function getErrorsAsSingleStr(ByVal delimiter As String, Optional ByVal blnUseNewLine As Boolean = True) As String
            Return getMessageAsSingleStr(MessageType.Errors, delimiter, blnUseNewLine)
        End Function

        ''' <summary>
        ''' Loops through each message defined by type and returns a single non localized string seperated by delimiter. 
        ''' A new line is added after each delimiter if the blnUseNewLine flag is true (default)
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="delimiter"></param>
        ''' <param name="blnUseNewLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMessageAsSingleStr(ByVal type As MessageType, ByVal delimiter As String, Optional ByVal blnUseNewLine As Boolean = True) As String
            Dim result As New StringBuilder
            Dim blnFirst As Boolean = True
            Dim strLineFeed As String = ""
            If blnUseNewLine Then strLineFeed = vbCrLf
            Dim oDict As New Dictionary(Of String, List(Of NGLMessage))
            Select Case type
                Case MessageType.Errors
                    oDict = Errors
                Case MessageType.Messages
                    oDict = Messages
                Case MessageType.Warnings
                    oDict = Warnings
            End Select
            If Not oDict Is Nothing AndAlso oDict.Count > 0 Then
                For Each e In oDict
                    Dim strMsg = formatMsgNotLocalized(e.Key, e.Value)
                    If blnFirst Then
                        result.Append(strMsg)
                    Else
                        result.AppendFormat(" {0} {1} {2}", delimiter, strLineFeed, strMsg)
                    End If
                    blnFirst = False
                Next
            Else
                Return ""
            End If

            Return result.ToString()
        End Function

        Public Shared Function ContainsNGLItem(ByVal intvalue As Integer, ByVal list As List(Of NGLListItem)) As Boolean
            Dim val = (From d In list Where d.intValue = intvalue Select d).FirstOrDefault
            If (val IsNot Nothing AndAlso val.intValue = intvalue) Then Return True
            Return False
        End Function

#End Region


#Region "Message Enum Processing"


        Public Sub AddFaultException(ByVal item As MessageEnum, ByVal r As String, ByVal m As String, ByVal d As String, ByVal ePars As List(Of String))
            Try
                Dim oMsg As New NGLMessage
                Dim par As New List(Of NGLMessage)
                oMsg.populateErrorInformationFromList(r, m, d, ePars)
                par.Add(oMsg)
                If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                Dim sKey As String = getMessageLocalizedString(item)
                If Errors.ContainsKey(sKey) Then
                    If Not Errors(sKey) Is Nothing Then
                        Errors(sKey).AddRange(par)
                    Else
                        Errors(sKey) = par
                    End If
                Else
                    Errors.Add(getMessageLocalizedString(item), par)
                End If
                AddLog(getMessageNotLocalizedString(item))

            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & item & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddUnexpectedError(ByVal ex As Exception)
            Dim Message = SqlFaultInfo.getFaultMessage(SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG)
            Dim Reason = SqlFaultInfo.getFaultReason(SqlFaultInfo.FaultReasons.E_UnExpected)
            Dim Details = SqlFaultInfo.getFaultDetailsKey(SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails)
            AddFaultException(MessageEnum.E_UnExpected_Error, Reason, Message, Details, New List(Of String) From {ex.Message})
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String, ByVal blnLog As Boolean, ByVal sLog As String, ByVal ParamArray p() As String)
            Try

                Dim par As New List(Of NGLMessage)
                If Not p Is Nothing AndAlso p.Length > 0 Then
                    For Each s In p
                        par.Add(New NGLMessage(s))
                    Next
                End If
                Select Case type
                    Case MessageType.Errors
                        If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                        If Errors.ContainsKey(key) Then
                            If Not Errors(key) Is Nothing Then
                                Errors(key).AddRange(par)
                            Else
                                Errors(key) = par
                            End If
                        Else
                            Errors.Add(key, par)
                        End If
                    Case MessageType.Warnings
                        If Warnings Is Nothing Then Warnings = New Dictionary(Of String, List(Of NGLMessage))
                        If Warnings.ContainsKey(key) Then
                            If Not Warnings(key) Is Nothing Then
                                Warnings(key).AddRange(par)
                            Else
                                Warnings(key) = par
                            End If
                        Else
                            Warnings.Add(key, par)
                        End If
                    Case MessageType.Messages
                        If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                        If Messages.ContainsKey(key) Then
                            If Not Messages(key) Is Nothing Then
                                Messages(key).AddRange(par)
                            Else
                                Messages(key) = par
                            End If
                        Else
                            Messages.Add(key, par)
                        End If
                End Select

                If blnLog Then
                    If p Is Nothing OrElse p.Length < 1 Then
                        AddLog(sLog)
                    Else
                        AddLog(String.Format(sLog, p.ToArray()))
                    End If
                End If
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String, ByVal ParamArray p() As String)
            AddMessage(type, key, True, "", p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String)
            If (Not String.IsNullOrWhiteSpace(key)) Then
                Dim enmVal As MessageEnum
                If ([Enum].TryParse(key, enmVal)) Then
                    Dim sLog As String = getMessageNotLocalizedString(enmVal, key)
                    AddMessage(type, key, True, sLog, Nothing)
                Else
                    AddMessage(type, key, False, "", Nothing)
                End If
            End If
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal blnLog As Boolean, ByVal ParamArray p() As String)
            Dim key As String = getMessageLocalizedString(item)
            Dim sLog As String = ""
            If blnLog Then sLog = getMessageNotLocalizedString(item)
            AddMessage(type, key, blnLog, sLog, p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal ParamArray p() As String)
            AddMessage(type, item, True, p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum)
            AddMessage(type, item, True, Nothing)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal s As List(Of String), Optional ByVal blnLog As Boolean = True)
            AddMessage(type, item, blnLog, s.ToArray())
        End Sub

        Public Sub AddLog(ByVal s As String)
            If Me.Log Is Nothing Then Me.Log = New List(Of NGLMessage)
            Me.Log.Add(New NGLMessage(s))
        End Sub

        ''' <summary>
        ''' Adds a new log message with reference link data to the logs
        ''' </summary>
        ''' <param name="m"></param>
        ''' <param name="c"></param>
        ''' <param name="n"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub AddLog(ByVal m As String, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            If Me.Log Is Nothing Then Me.Log = New List(Of NGLMessage)
            Me.Log.Add(New NGLMessage(m, c, n, e))
        End Sub

        Public Sub AddLog(ByVal item As MessageEnum, ByVal p As List(Of String))
            Dim key As String = " Undefined "
            Try
                key = getMessageLocalizedString(item)
                Dim sLog As String = getMessageNotLocalizedString(item)
                If p Is Nothing OrElse p.Count() < 1 Then
                    AddLog(sLog)
                Else
                    AddLog(String.Format(sLog, p.ToArray()))
                End If
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function formatMsgNotLocalized(ByVal m As String, ByVal p As List(Of NGLMessage)) As String
            If String.IsNullOrWhiteSpace(m) Then Return m
            Dim strRet As String = m
            Try
                If Not p Is Nothing AndAlso p.Count() > 0 Then
                    Dim sList = (From d In p Select d.Message).ToList()
                    strRet = String.Format(m, sList.ToArray())
                End If
            Catch ex As System.FormatException
                strRet = "Invalid Message Format; the message [ " & m & " ] may require missing parameters"
            Catch ex As Exception
                Throw
            End Try
            Return strRet
        End Function

        Public Shared Function getMessageNotLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Select Case item                  
                    Case MessageEnum.Success
                        strReturn = "Success"
                    Case MessageEnum.E_UnExpected_Error
                        strReturn = "An Unexpected Error Has Occurred: {0}.  You should manually refresh your data to be sure you have the latest changes."
                    Case MessageEnum.E_NotFoundSSOAByUser
                        strReturn = "No Single Sign On Accounts exist for selected user."
                    Case MessageEnum.M_Success
                        strReturn = "Success!"
                    Case MessageEnum.E_InvalidSSOAName
                        strReturn = "No record exists in the database with SSOAName: {0}."
                    Case MessageEnum.E_InvalidParameterNameValue
                        strReturn = "Invalid Parameter: No record exists in the database for {0}: {1}."
                    Case MessageEnum.E_RequiredFieldDATEquipType
                        strReturn = "The DATEquipType field is required and cannot be null. Please check that a DATEquipType mapping exists for all Temp Type Codes in the Lane Code Maintenance Window and that the Company Level Parameter DATDefaultMixTempType is populated correctly."
                    Case MessageEnum.W_UserNoDATAccount
                        strReturn = "User {0} does not have an SSOA account set up for DAT"
                    Case MessageEnum.E_DATGeneralRetMsg
                        strReturn = "{0}"
                    Case MessageEnum.E_DATInvalidFeature
                        strReturn = "Failure - Could not execute DAT program. Invalid DAT Feature {0}"
                    Case MessageEnum.E_DATDeletePostServiceError
                        strReturn = "DAT Delete Post Failed -- {0}"
                    Case MessageEnum.E_DATPostFailedServiceError
                        strReturn = "DAT Post Failed -- {0}"
                    Case MessageEnum.E_DATDeleteFailedNoUser
                        strReturn = "Could not delete{0}load from DAT: No DAT Account found for User {1} using UserSecurityControl {2} and SSOAControl 2."
                    Case MessageEnum.E_NoUserSecurityForUser
                        strReturn = "Could not find User Security record using UserName: {0}"
                    Case MessageEnum.E_NoLoadTenderForBook
                        strReturn = "Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0."
                    Case MessageEnum.E_DATDeleteStillTenderedToCarrier
                        strReturn = "{3}: Warning - The Load with SHID {0} is still tendered to Carrier {1} {2} in TMS but is no longer Posted on the {3} Load Board."
                    Case MessageEnum.E_DATExpireFail
                        strReturn = "{2}: Warning - The Load with SHID {0} is expired but could not be reset to N Status as there was a problem with the Save. The {2} Posting with Reference ID {1} was not deleted from the {2} Load Board."
                    Case MessageEnum.E_DATDeleteFail
                        strReturn = "{2}: Warning - The Load with SHID {0} could not be reset to N Status as there was a problem with the Save. The {2} Posting with Reference ID {1} was not deleted from the {2} Load Board."
                    Case MessageEnum.E_DATSpFailNoLTControl
                        strReturn = "The stored procedure spGetDATData did not return a valid LoadTenderControl."
                    Case MessageEnum.E_DATDPostFail
                        strReturn = "{1}: Warning - The {1} Posting with Reference ID {0} was successfully posted to the {1} Load Board. However, the Load with could not be assigned to the Load Board Carrier and set to PC status as there was a problem with the Save."
                    Case MessageEnum.E_DATUpdatePostFailedServiceError
                        strReturn = "DAT Update Post Failed for SHID: {0} and DATRefID: {1} -- {2}"
                    Case MessageEnum.E_LBSpFailNoLTControl
                        strReturn = "The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl."
                    Case MessageEnum.E_LoadBoardSpFailNoLTControl
                        strReturn = "The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl."
                    Case MessageEnum.E_LBDeleteFailedNoUser
                        strReturn = "Could not delete{0}load from {3}: No {3} Account found for User {1} using UserSecurityControl {2}."
                End Select

            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        Public Shared Function getMessageLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Dim Enumerator As Type = GetType(MessageEnum)
                strReturn = [Enum].GetName(Enumerator, item)
            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        ''' <summary>
        ''' Parses the MessageEnum using strEnum and returns the actual Enum if strEnum is not valid 
        ''' retuns the default MessageEnum.None
        ''' </summary>
        ''' <param name="strEnum"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Caller should test for MessageEnum.None and process the messages accordingly
        ''' </remarks>
        Public Function getMessageEnumFromString(ByVal strEnum As String) As MessageEnum
            Dim enmVal As MessageEnum = MessageEnum.None
            [Enum].TryParse(strEnum, enmVal)
            Return enmVal
        End Function

        Public Function getAllMessagesNotLocalized() As String
            Dim sb As New System.Text.StringBuilder()
            Return getAllMessagesNotLocalized(sb).ToString()
        End Function

        Public Function getAllMessagesNotLocalized(ByRef sb As System.Text.StringBuilder) As System.Text.StringBuilder
            concatMessage(sb, Messages)
            concatMessage(sb, Warnings)
            concatErrors(sb, Errors)
            Return sb
        End Function

        Public Function concatWarnings() As String
            If Warnings Is Nothing OrElse Warnings.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            For Each m In Warnings
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        Public Sub concatMessage(ByRef sb As System.Text.StringBuilder, ByVal dMsgs As Dictionary(Of String, List(Of NGLMessage)))
            If dMsgs Is Nothing OrElse dMsgs.Count < 1 Then Return
            If sb Is Nothing Then sb = New System.Text.StringBuilder()
            For Each m In dMsgs
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
        End Sub

        Public Function concatMessage() As String
            If Messages Is Nothing OrElse Messages.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            For Each m In Messages
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        Public Sub concatErrors(ByRef sb As System.Text.StringBuilder, ByVal dErrs As Dictionary(Of String, List(Of NGLMessage)))
            If dErrs Is Nothing OrElse dErrs.Count < 1 Then Return
            If sb Is Nothing Then sb = New System.Text.StringBuilder()
            Dim blnAppendDefault As Boolean = False
            For Each m In dErrs
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        sb.Append(sMsg)
                    Else
                        sb.Append(m.Key)
                    End If
                    sb.AppendLine()
                    If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                        'append the error Reason 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorReason) Then
                            blnAppendDefault = False
                            Dim eReason = SqlFaultInfo.getFaultReasonsEnumFromString(m.Value(0).ErrorReason)
                            Try
                                If eReason = SqlFaultInfo.FaultReasons.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sReason = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eReason, "")
                                    If Not String.IsNullOrWhiteSpace(sReason) Then
                                        sb.AppendFormat("{0}Error Reason: {1}", vbTab, sReason)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Reason: {1}", vbTab, m.Value(0).ErrorReason)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error message info 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorMessage) Then
                            blnAppendDefault = False
                            Dim eInfo = SqlFaultInfo.getFaultInfoMsgsEnumFromString(m.Value(0).ErrorMessage)
                            Try
                                If eInfo = SqlFaultInfo.FaultInfoMsgs.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sInfo = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eInfo, "")
                                    If Not String.IsNullOrWhiteSpace(sInfo) Then
                                        sb.AppendFormat("{0}Error Info: {1}", vbTab, sInfo)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Info: {1}", vbTab, m.Value(0).ErrorMessage)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error details 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorDetails) Then
                            blnAppendDefault = False
                            Dim eDet = SqlFaultInfo.getFaultDetailsKeyEnumFromString(m.Value(0).ErrorDetails)
                            Try
                                If eDet = SqlFaultInfo.FaultDetailsKey.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim strDet = SqlFaultInfo.getFaultDetailsKeyNotLocalizedString(eDet, "")
                                    If Not String.IsNullOrWhiteSpace(strDet) Then
                                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorCSVDetailsParameters) Then
                                            sb.AppendFormat("{0}Error Details: ", vbTab)
                                            sb.AppendFormat(strDet, m.Value(0).ErrorCSVDetailsParameters.Split(","))
                                        Else
                                            sb.AppendFormat("{0}Error Details: {1}", vbTab, strDet)
                                        End If
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Details: {1}", vbTab, m.Value(0).ErrorDetails)
                            End If
                            sb.AppendLine()
                        End If
                    End If
                End If
            Next
        End Sub

        Public Function concatErrors() As String
            If Errors Is Nothing OrElse Errors.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            Dim blnAppendDefault As Boolean = False
            For Each m In Errors
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        sb.Append(sMsg)
                    Else
                        sb.Append(m.Key)
                    End If
                    sb.AppendLine()
                    If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                        'append the error Reason 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorReason) Then
                            blnAppendDefault = False
                            Dim eReason = SqlFaultInfo.getFaultReasonsEnumFromString(m.Value(0).ErrorReason)
                            Try
                                If eReason = SqlFaultInfo.FaultReasons.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sReason = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eReason, "")
                                    If Not String.IsNullOrWhiteSpace(sReason) Then
                                        sb.AppendFormat("{0}Error Reason: {1}", vbTab, sReason)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Reason: {1}", vbTab, m.Value(0).ErrorReason)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error message info 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorMessage) Then
                            blnAppendDefault = False
                            Dim eInfo = SqlFaultInfo.getFaultInfoMsgsEnumFromString(m.Value(0).ErrorMessage)
                            Try
                                If eInfo = SqlFaultInfo.FaultInfoMsgs.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sInfo = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eInfo, "")
                                    If Not String.IsNullOrWhiteSpace(sInfo) Then
                                        sb.AppendFormat("{0}Error Info: {1}", vbTab, sInfo)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Info: {1}", vbTab, m.Value(0).ErrorMessage)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error details 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorDetails) Then
                            blnAppendDefault = False
                            Dim eDet = SqlFaultInfo.getFaultDetailsKeyEnumFromString(m.Value(0).ErrorDetails)
                            Try
                                If eDet = SqlFaultInfo.FaultDetailsKey.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim strDet = SqlFaultInfo.getFaultDetailsKeyNotLocalizedString(eDet, "")
                                    If Not String.IsNullOrWhiteSpace(strDet) Then
                                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorCSVDetailsParameters) Then
                                            sb.AppendFormat("{0}Error Details: ", vbTab)
                                            sb.AppendFormat(strDet, m.Value(0).ErrorCSVDetailsParameters.Split(","))
                                        Else
                                            sb.AppendFormat("{0}Error Details: {1}", vbTab, strDet)
                                        End If
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Details: {1}", vbTab, m.Value(0).ErrorDetails)
                            End If
                            sb.AppendLine()
                        End If
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Adds the entire range of the parameter dictionary d to the
        ''' DATResults object property specified by the parameter type
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="d"></param>
        ''' <remarks>
        ''' Added by LVV 7/6/16 for v-7.0.5.110 DAT
        ''' The reason I chose to make the parameter a dictionary instead of
        ''' another DATResults object is so that I can add the dictionaries of
        ''' other objects as well such as WCFResults etc.
        ''' </remarks>
        Public Sub AddRangeToDictionary(ByVal type As MessageType, ByVal d As Dictionary(Of String, List(Of NGLMessage)))

            If Not d Is Nothing Then

                Select Case type
                    Case MessageType.Errors
                        If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Errors.ContainsKey(kv.Key) Then
                                Errors.Add(kv.Key, kv.Value)
                            Else
                                Errors(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                    Case MessageType.Warnings
                        If Warnings Is Nothing Then Warnings = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Warnings.ContainsKey(kv.Key) Then
                                Warnings.Add(kv.Key, kv.Value)
                            Else
                                Warnings(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                    Case MessageType.Messages
                        If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Messages.ContainsKey(kv.Key) Then
                                Messages.Add(kv.Key, kv.Value)
                            Else
                                Messages(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                End Select
            End If
        End Sub



#End Region


    End Class

End Namespace
