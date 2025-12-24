Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Destructurama
Imports Destructurama.Attributed

Namespace DataTransferObjects
    <DataContract()>
    Public Class CarrierCostResults
        Inherits DTOBaseClass

#Region " Enums"

        Public Enum MessageEnum
            None
            M_NoOrdersFound '"No BookRevenue data found."
            M_FinalizedCannotChangeCarrier '"Cannot change carrier while load has been finalized; Order Number {0}; Pro Number {1}."
            M_CostsAreLockedCannotRecalculate '"Cannot recalculate or change carrier while Costs are locked; Order Number {0}; Pro Number {1}."
            M_InvoicedCannotRecalculate '"Cannot recalculate or change carrier after load has been invoiced; Order Number {0}; Pro Number {1}."
            M_NoCarrierCannotRecalculate    '"Cannot recalculate or change carrier because a carrier has not been assigned or selected; Order Number {0}; Pro Number {1}."
            M_LegacyStopPickandFuelProblem  '"There was a problem with the Stop, Pick or Fuel Fee calculation,  check the Carrier Assignment Log for details."
            M_InvalidLineHaulCannotRateLoad '"Unable to rate load because the line haul is not valid."
            M_SQLFaultCannotReadTariff '"Cannot Read Some Tariff Information. Reason: {0} Message: {1} Details: {2}."
            M_NoTariffsFound '"No tariffs could be found that match the selected parameters."
            M_AtLeastOneOrderReq '"At Least One Order Required."
            M_ReqFieldMissing '"Required Fields missing."
            M_DistanceRateFound '"Distance rate available for tariff ID {0}."
            M_LTLRateFound '"LTL rate available for tariff ID {0}."
            M_UOMRateFound '"Unit of Measure rate available for tariff ID {0}."
            M_FlatRateFound '"Flat rate available for tariff ID {0}."
            M_DistanceRateRestricted '"Distance rates are restricted on this load."
            M_FlatRateRestricted '"Flat rates are restricted on this load."
            M_LTLRateRestricted '"LTL rates are restricted on this load."
            M_UOMRateRestricted '"Unit of Measure rates are restricted on this load."
            M_SQLFaultCannotReadDefaultClassCodes '"Cannot read default class codes setting default value to 100."
            M_SQLFaultCannotSaveCarrierAssignment '"Cannot save the assigned carrier information. Reason: {0} Message: {1} Details: {2}."
            E_UnExpected '"An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
            MSGRevExceedsTolerance '*** Warning *** Billed Revenue Exceeds Tolerance.
            MSGBFCHighValueWarning '*** Warning Large Value *** Please Verify BFC!
            M_DistanceRequired 'Unable to process , could not find distance.
            M_OrigCountryCityStZipNotValid 'Cannot recalculate or change carrier if part of the origin address is missing. Please check that the Country, City, State, and Postal Code are all populated for Order Number {0}; Pro Number {1}.
            M_DestCountryCityStZipNotValid 'Cannot recalculate or change carrier if part of the destination address is missing. Please check that the Country, City, State, and Postal Code are all populated for Order Number {0}; Pro Number {1}.
            MSGPCMGetMilesFailedWarning 'Get Practical Distance Failure! There was a problem with PC Miler: {0}
            M_ReqFieldMissingDensityRating ' To return density rating, enter length width height.
            M_InvalidClassTypeForLTLTariff '"Use Carrier,{0} , for LTL Rating because of an invalid Class Type; check the settings for tariff ID {1}."
            M_OrigRateShopCountryZipNotValid 'Cannot get rates when the origin address Country or Postal Code are missing.
            M_DestRateShopCountryZipNotValid 'Cannot get rates when the destination Country or Postal Code are missing.

        End Enum

#End Region

#Region " Data Members"


        Private _Messages As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()>
        Public Property Messages() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Messages
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Messages = value
            End Set
        End Property

        Private _CarriersByCost As New List(Of CarriersByCost)
        <DataMember()>
        Public Property CarriersByCost() As List(Of CarriersByCost)
            Get
                Return _CarriersByCost
            End Get
            Set(ByVal value As List(Of CarriersByCost))
                _CarriersByCost = value
            End Set
        End Property

        Private _BookRevs As New List(Of BookRevenue)
        <DataMember()>
        Public Property BookRevs() As List(Of BookRevenue)
            Get
                Return _BookRevs
            End Get
            Set(ByVal value As List(Of BookRevenue))
                _BookRevs = value
            End Set
        End Property

        Private _Log As New List(Of NGLMessage)
        <DataMember()>
        Public Property Log() As List(Of NGLMessage)
            Get
                Return _Log
            End Get
            Set(ByVal value As List(Of NGLMessage))
                _Log = value
            End Set
        End Property

        Private _Success As Boolean = False
        <DataMember()>
        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Private _DecParameter As Decimal
        <DataMember()>
        Public Property DecParameter() As Decimal
            Get
                Return _DecParameter
            End Get
            Set(ByVal value As Decimal)
                _DecParameter = value
            End Set
        End Property

        Private _IsNextrackOnly As Boolean = True
        <DataMember()>
        Public Property IsNextrackOnly() As Boolean
            Get
                Return _IsNextrackOnly
            End Get
            Set(ByVal value As Boolean)
                _IsNextrackOnly = value
            End Set
        End Property

        ''' <summary>
        ''' set to true when a specific carrier rate is not available but messages or warnings are still required
        ''' Not to be used for logs
        ''' </summary>
        ''' <remarks>
        ''' Created by RHR on 05/25/2022 for v-8.5.3.001
        ''' </remarks>
        Private _postMessagesOnly As Boolean = False
        Public Property postMessagesOnly() As Boolean
            Get
                Return _postMessagesOnly
            End Get
            Set(ByVal value As Boolean)
                _postMessagesOnly = value
            End Set
        End Property


#End Region

#Region " Public Methods"

        Public Overrides Function ToString() As String
            Return $"CarrierCostResults: {CarriersByCost?.Select(function(cbcItem) 
                                                                    Return $"{cbcItem.CarrierName}[{cbcItem.BookCarrTarName}] {cbcItem.RateTypeName} {cbcItem.BookCarrTarEquipMatName} {cbcItem.BracketTypeName}{Environment.NewLine}"
                                                                End function).SelectMany(Function(o)
                                                                                             return o
                                                                                             End Function)} Success={Success}, DecParameter={DecParameter}, IsNextrackOnly={IsNextrackOnly}, postMessagesOnly={postMessagesOnly}, CarriersByCost={CarriersByCost?.Count}, BookRevs={BookRevs?.Count}, Messages={Messages?.Count}, Log={Log?.Count}"
        End Function

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierCostResults
            instance = DirectCast(MemberwiseClone(), CarrierCostResults)
            instance.CarriersByCost = New List(Of CarriersByCost)
            For Each item In CarriersByCost
                instance.CarriersByCost.Add(DirectCast(item.Clone, CarriersByCost))
            Next
            instance.BookRevs = New List(Of BookRevenue)
            For Each item In BookRevs
                instance.BookRevs.Add(DirectCast(item.Clone, BookRevenue))
            Next

            If Not Messages Is Nothing AndAlso Messages.Count > 0 Then
                instance.Messages = (From x In Messages Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If

            If Not Log Is Nothing AndAlso Log.Count > 0 Then
                instance.Log = (From d In Log Select d).ToList() 'creates a copy (clone)
            End If


            Return instance
        End Function

#Region "       Message Enum Processing"

        Public Sub AddMessage(ByVal key As String, ByVal blnLog As Boolean, ByVal sLog As String, ByVal ParamArray p() As String)
            Try

                Dim par As New List(Of NGLMessage)
                If Not p Is Nothing AndAlso p.Length > 0 Then
                    For Each s In p
                        par.Add(New NGLMessage(s))
                    Next
                End If

                If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                If Not Messages.ContainsKey(key) Then
                    Messages.Add(key, par)
                Else
                    Messages(key) = par
                End If
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

        Public Sub AddMessage(ByVal key As String, ByVal ParamArray p() As String)
            AddMessage(key, True, "", p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal blnLog As Boolean, ByVal ParamArray p() As String)
            Dim key As String = getMessageLocalizedString(item)
            Dim sLog As String = ""
            If blnLog Then sLog = getMessageNotLocalizedString(item)
            AddMessage(key, blnLog, sLog, p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal ParamArray p() As String)
            AddMessage(item, False, p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum)
            AddMessage(item, True, Nothing)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal s As List(Of String), Optional ByVal blnLog As Boolean = True)
            AddMessage(item, blnLog, s.ToArray())
        End Sub

#Region "       New Message Processing v-8.5.3.001"


        Public Sub AddMessage(ByVal key As String, msg As NGLMessage)
            Try

                Dim par As New List(Of NGLMessage)
                par.Add(msg)


                If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                If Not Messages.ContainsKey(key) Then
                    Messages.Add(key, par)
                Else
                    par = Messages(key)
                    par.Add(msg)
                    Messages(key) = par
                End If

            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub


        Public Sub AddMessage(ByVal item As MessageEnum, ByVal sDetails As String, ByVal sDefault As String, ByVal sFieldName As String)
            Dim msg = New NGLMessage()
            Dim key As String = getMessageLocalizedString(item)
            msg.MessageLocalCode = getMessageNotLocalizedString(item, sDefault)
            msg.Message = getMessageNotLocalizedString(item, sDefault)
            msg.Details = sDetails
            msg.FieldName = sFieldName
            msg.bLogged = False

            AddMessage(key, msg)
        End Sub

        Public Sub AddMessage(ByVal msg As NGLMessage)
            Dim item As MessageEnum = getMessageEnumFromString(msg.MessageLocalCode)
            Dim key As String = getMessageLocalizedString(item)
            AddMessage(key, msg)

        End Sub

        Public Function GetMessages() As List(Of NGLMessage)
            Dim lMsgRet As New List(Of NGLMessage)
            If Messages Is Nothing Then
                Return lMsgRet
            Else
                For Each kvp As KeyValuePair(Of String, List(Of NGLMessage)) In Messages
                    'Dim v1 As Integer = kvp.Key 'key is a string not an integer
                    Dim v2 As List(Of NGLMessage) = kvp.Value
                    If Not v2 Is Nothing AndAlso v2.Count() > 0 Then
                        lMsgRet.AddRange(v2)
                    End If
                Next
            End If
            Return lMsgRet
        End Function

        Public Function concateMessages() As String
            Return concatMessage()

        End Function

#End Region

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


        Public Shared Function getMessageNotLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Select Case item
                    Case MessageEnum.M_NoOrdersFound
                        strReturn = "No BookRevenue data found."
                    Case MessageEnum.M_FinalizedCannotChangeCarrier
                        strReturn = "Cannot change carrier while load has been finalized; Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_CostsAreLockedCannotRecalculate
                        strReturn = "Cannot change carrier while Costs are locked; Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_InvoicedCannotRecalculate
                        strReturn = "Cannot change carrier after load has been invoiced; Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_NoCarrierCannotRecalculate
                        strReturn = "Cannot recalculate Or change carrier because a carrier has Not been assigned Or selected; Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_LegacyStopPickandFuelProblem
                        strReturn = "There was a problem with the Stop, Pick Or Fuel Fee calculation,  check the Carrier Assignment Log for details."
                    Case MessageEnum.M_InvalidLineHaulCannotRateLoad
                        strReturn = "Unable to rate load because the line haul Is Not valid."
                    Case MessageEnum.M_SQLFaultCannotReadTariff
                        strReturn = "Cannot Read Some Tariff Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_NoTariffsFound
                        strReturn = "No tariffs could be found that match the selected parameters."
                    Case MessageEnum.M_DistanceRateFound
                        strReturn = "Distance rate available for tariff ID {0}."
                    Case MessageEnum.M_LTLRateFound
                        strReturn = "LTL rate available for tariff ID {0}."
                    Case MessageEnum.M_UOMRateFound
                        strReturn = "Unit of Measure rate available for tariff ID {0}."
                    Case MessageEnum.M_FlatRateFound
                        strReturn = "Flat rate available for tariff ID {0}."
                    Case MessageEnum.M_DistanceRateRestricted
                        strReturn = "Distance rates are restricted on this load."
                    Case MessageEnum.M_FlatRateRestricted
                        strReturn = "Flat rates are restricted on this load."
                    Case MessageEnum.M_LTLRateRestricted
                        strReturn = "LTL rates are restricted on this load. Possibly due to multi-stop or multi-pick."
                    Case MessageEnum.M_UOMRateRestricted
                        strReturn = "Unit of Measure rates are restricted on this load."
                    Case MessageEnum.M_SQLFaultCannotReadDefaultClassCodes
                        strReturn = "Cannot read default class codes setting default value to 100."
                    Case MessageEnum.M_SQLFaultCannotSaveCarrierAssignment
                        strReturn = "Cannot save the assigned carrier information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.E_UnExpected
                        strReturn = "An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
                    Case MessageEnum.MSGRevExceedsTolerance
                        strReturn = "*** Warning *** Billed Revenue Exceeds Tolerance."
                    Case MessageEnum.MSGBFCHighValueWarning
                        strReturn = "*** Warning Large Value *** Please Verify BFC!"
                    Case MessageEnum.M_DistanceRequired
                        strReturn = "Unable to process , could not find distance."
                    Case MessageEnum.M_OrigCountryCityStZipNotValid
                        strReturn = "Cannot recalculate or change carrier if part of the origin address is missing. Please check that the Country, City, State, and Postal Code are all populated for Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_DestCountryCityStZipNotValid
                        strReturn = "Cannot recalculate or change carrier if part of the destination address is missing. Please check that the Country, City, State, and Postal Code are all populated for Order Number {0}; Pro Number {1}."
                    Case MessageEnum.M_InvalidClassTypeForLTLTariff
                        strReturn = "Use Carrier,{0} , for LTL Rating because of an invalid Class Type; check the settings for tariff ID {1}."
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
        ''' Parses the MessageEnum using strEnum and returns the actual Enum if strEnum is not valid retuns the default MessageEnum E_UnExpected
        ''' </summary>
        ''' <param name="strEnum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMessageEnumFromString(ByVal strEnum As String) As MessageEnum
            Dim enmVal As MessageEnum = MessageEnum.None
            [Enum].TryParse(strEnum, enmVal)
            Return enmVal
        End Function

        Public Function concatMessage() As String
            If Messages Is Nothing OrElse Messages.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            Return concatMessage(sb).ToString()
        End Function

        Public Function concatMessage(ByRef sb As System.Text.StringBuilder) As System.Text.StringBuilder
            If sb Is Nothing Then sb = New System.Text.StringBuilder()

            If Messages Is Nothing OrElse Messages.Count < 1 Then Return sb

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
            Return sb
        End Function

#End Region

#End Region
    End Class


End Namespace