Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ZebraResult
        Inherits DTOBaseClass

#Region " Enums"

        Public Enum MessageEnum
            'don't forget to add these to Culture Resources in Localization
            M_UnableToCreateLabelString '"Unable to create label layout command string."
            M_CannotConnectToZebraPrinter '"Cannot connect to Zebra Printer with IP Address {0} and Port {1}."
            M_AtLeastOneLabelRequired '"At least one label must be selected to print."
            M_UnableToPrint '"Unable to print due to communication failure with the printer {0}"

            ' M_AtLeastOneOrderReq '"At Least One Order Required."
            ' M_ReqFieldMissing '"Required Fields missing."
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

        Private _ZPLCMDStrings As List(Of String)
        Public Property ZPLCMDStrings() As List(Of String)
            Get
                Return _ZPLCMDStrings
            End Get
            Set(ByVal value As List(Of String))
                _ZPLCMDStrings = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ZebraResult
            instance = DirectCast(MemberwiseClone(), ZebraResult)
            instance.Messages = Messages
            instance.Log = Log
            instance.Success = Success
            instance.ZPLCMDStrings = ZPLCMDStrings
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
                'Modified by RHR 8/4/14 to avoid unexpected system.ArgumentException
                If Not Messages.ContainsKey(key) Then Messages.Add(key, par)
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
            AddMessage(key, False, "", p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal blnLog As Boolean, ByVal ParamArray p() As String)
            Dim key As String = getMessageLocalizedString(item)
            Dim sLog As String = ""
            If blnLog Then sLog = getMessageNotLocalizedString(key)
            AddMessage(key, blnLog, sLog, p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal ParamArray p() As String)
            AddMessage(item, False, p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum)
            AddMessage(item, False, Nothing)
        End Sub

        Public Sub AddLog(ByVal s As String)
            If Me.Log Is Nothing Then Me.Log = New List(Of NGLMessage)
            Me.Log.Add(New NGLMessage(s))
        End Sub

        Public Sub AddZPL(ByVal s As String)
            If Me.ZPLCMDStrings Is Nothing Then Me.ZPLCMDStrings = New List(Of String)
            Me.ZPLCMDStrings.Add(s)
        End Sub

        Public Shared Function getMessageNotLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Select Case item
                    Case MessageEnum.M_UnableToCreateLabelString
                        strReturn = "Unable to create label layout command string."
                    Case MessageEnum.M_CannotConnectToZebraPrinter
                        strReturn = "Cannot connect to Zebra Printer with IP Address {0} and Port {1}."
                    Case MessageEnum.M_AtLeastOneLabelRequired
                        strReturn = "At least one label must be selected to print."
                    Case MessageEnum.M_UnableToPrint
                        strReturn = "Unable to print due to communication failure with the printer {0}"
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

#End Region

#End Region
    End Class


End Namespace