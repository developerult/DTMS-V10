Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblAccessorialVariableCode
        Inherits DTOBaseClass


#Region " Data Members"
        Private _AccessorialVariableCodesControl As Integer
        <DataMember()> _
        Public Property AccessorialVariableCodesControl() As Integer
            Get
                Return Me._AccessorialVariableCodesControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._AccessorialVariableCodesControl = value) _
                   = False) Then
                    Me._AccessorialVariableCodesControl = value
                    Me.SendPropertyChanged("AccessorialVariableCodesControl")
                End If
            End Set
        End Property

        Private _AccessorialVariableCodesName As String = ""
        <DataMember()> _
        Public Property AccessorialVariableCodesName() As String
            Get
                Return Me._AccessorialVariableCodesName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialVariableCodesName, value) = False) Then
                    Me._AccessorialVariableCodesName = value
                    Me.SendPropertyChanged("AccessorialVariableCodesName")
                End If
            End Set
        End Property

        Private _AccessorialVariableCodesDescription As String = ""
        <DataMember()> _
        Public Property AccessorialVariableCodesDescription() As String
            Get
                Return Me._AccessorialVariableCodesDescription
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialVariableCodesDescription, value) = False) Then
                    Me._AccessorialVariableCodesDescription = value
                    Me.SendPropertyChanged("AccessorialVariableCodesDescription")
                End If
            End Set
        End Property

        Private _AccessorialVariableModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AccessorialVariableModDate() As System.Nullable(Of Date)
            Get
                Return Me._AccessorialVariableModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._AccessorialVariableModDate.Equals(value) = False) Then
                    Me.SendPropertyChanged("AccessorialVariableModDate")
                End If
            End Set
        End Property


        Private _AccessorialVariableModUser As String = ""
        <DataMember()> _
        Public Property AccessorialVariableModUser() As String
            Get
                Return Me._AccessorialVariableModUser
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AccessorialVariableModUser, value) = False) Then
                    Me._AccessorialVariableModUser = value
                    Me.SendPropertyChanged("AccessorialVariableModUser")
                End If
            End Set
        End Property


        Private _AccessorialVariableUpdated As Byte()
        <DataMember()> _
        Public Property AccessorialVariableUpdated() As Byte()
            Get
                Return _AccessorialVariableUpdated
            End Get
            Set(ByVal value As Byte())
                _AccessorialVariableUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblAccessorialVariableCode
            instance = DirectCast(MemberwiseClone(), tblAccessorialVariableCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace
