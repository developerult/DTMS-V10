Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class UpdateLoadPlanningResults


#Region " Enums"
        'Modified by RHR v-7.0.5.100 5/17/2016
        'Added new message enum MSGRecalcCostNoValidTariffWarning
        Public Enum MessageEnum
            None
            MSGRecalcCostNoTariffWarning 'Could not recalculate costs because a tariff could not be found.  Check your capacity and tariff settings.  The load may not have a tariff for the last stop.  All costs are set to zero.
            MSGBadAddressCount 'The system found {0} bad address warnings.
            MSGNoTruck 'Cannot update load detail costs or miles because a valid truck has not been selected.  Please try again."
            MSGRecalcCostNoValidTariffWarning 'Could not recalculate costs because a valid tariff could not be found for the current carrier.  Check the carrier qualifications, equipment capacity, tariff settings, or rates for the last stop.  All costs are set to zero.
        End Enum

#End Region


#Region " Data Members"

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


        Private _LPTruck As tblSolutionTruck
        <DataMember()> _
        Public Property UpdatedLPTruck() As tblSolutionTruck
            Get
                Return _LPTruck
            End Get
            Set(ByVal value As tblSolutionTruck)
                _LPTruck = value
            End Set
        End Property

        Private _Exception As Object
        <DataMember()> _
        Public Property Exception() As Object
            Get
                Return _Exception
            End Get
            Set(ByVal value As Object)
                _Exception = value
            End Set
        End Property

#End Region


#Region "       Message Enum Processing"


        Public Sub AddMessage(ByVal key As String, ByVal par As List(Of NGLMessage), ByVal blnLog As Boolean, ByVal sLog As String)
            Try

                If par Is Nothing Then par = New List(Of NGLMessage)

                If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                If Not Messages.ContainsKey(key) Then
                    Messages.Add(key, par)
                Else
                    Messages(key) = par
                End If
                If blnLog Then
                    If par Is Nothing OrElse par.Count < 1 Then
                        AddLog(sLog)
                    Else
                        Dim p As New List(Of String)
                        For Each m In par
                            If Not m Is Nothing Then
                                p.Add(m.Message)
                            Else
                                p.Add("")
                            End If
                        Next
                        AddLog(String.Format(sLog, p.ToArray()))
                    End If
                End If
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal key As String, ByVal blnLog As Boolean, ByVal sLog As String, ByVal ParamArray p() As String)
            Try

                Dim par As New List(Of NGLMessage)
                If Not p Is Nothing AndAlso p.Length > 0 Then
                    For Each s In p
                        par.Add(New NGLMessage(s))
                    Next
                End If
                AddMessage(key, par, blnLog, sLog)
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal key As String, ByVal ParamArray p() As String)
            AddMessage(key, False, "", p)
        End Sub
        Public Sub AddMessage(ByVal key As String)
            AddMessage(key, Nothing, False, "")
        End Sub

        Public Sub AddMessage(ByVal key As String, ByVal par As List(Of NGLMessage))
            AddMessage(key, par, False, "")
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
                    Case MessageEnum.None
                        strReturn = "No Messages found."
                    Case MessageEnum.MSGRecalcCostNoTariffWarning
                        strReturn = "Could not recalculate costs because a tariff could not be found.  Check your capacity and tariff settings.  The load may not have a tariff for the last stop.  All costs are set to zero."
                    Case MessageEnum.MSGBadAddressCount
                        strReturn = "The system found {0} bad address warnings."
                    Case MessageEnum.MSGNoTruck
                        strReturn = "Cannot update load detail costs or miles because a valid truck has not been selected.  Please try again."
                    Case MessageEnum.MSGRecalcCostNoValidTariffWarning
                        'Modified by RHR v-7.0.5.100 5/17/2016
                        'Added new message enum MSGRecalcCostNoValidTariffWarning
                        strReturn = "Could not recalculate costs because a valid tariff could not be found for the current carrier.  Check the carrier qualifications, equipment capacity, tariff settings, and rates for the last stop.  All costs are set to zero."
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


    End Class

End Namespace
