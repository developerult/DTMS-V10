Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblTarBracketType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _TarBracketTypeControl As Integer = 0
        <DataMember()> _
        Public Property TarBracketTypeControl() As Integer
            Get
                Return Me._TarBracketTypeControl
            End Get
            Set(value As Integer)
                If ((Me._TarBracketTypeControl = value) _
                   = False) Then
                    Me._TarBracketTypeControl = value
                    Me.SendPropertyChanged("TarBracketTypeControl")
                End If
            End Set
        End Property



        Private _TarBracketTypeName As String
        <DataMember()> _
        Public Property TarBracketTypeName() As String
            Get
                Return Left(Me._TarBracketTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarBracketTypeName, value) = False) Then
                    Me._TarBracketTypeName = Left(value, 50)
                    Me.SendPropertyChanged("TarBracketTypeName")
                End If
            End Set
        End Property

        Private _TarBracketTypeDesc As String
        <DataMember()> _
        Public Property TarBracketTypeDesc() As String
            Get
                Return Left(Me._TarBracketTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarBracketTypeDesc, value) = False) Then
                    Me._TarBracketTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("TarBracketTypeDesc")
                End If
            End Set
        End Property

        Private _TarBracketTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TarBracketTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._TarBracketTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._TarBracketTypeModDate.Equals(value) = False) Then
                    Me._TarBracketTypeModDate = value
                    Me.SendPropertyChanged("TarBracketTypeModDate")
                End If
            End Set
        End Property

        Private _TarBracketTypeModUser As String
        <DataMember()> _
        Public Property TarBracketTypeModUser() As String
            Get
                Return Left(Me._TarBracketTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarBracketTypeModUser, value) = False) Then
                    Me._TarBracketTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("TarBracketTypeModUser")
                End If
            End Set
        End Property


        Private _TarBracketTypeUpdated As Byte()
        <DataMember()> _
        Public Property TarBracketTypeUpdated() As Byte()
            Get
                Return Me._TarBracketTypeUpdated
            End Get
            Set(value As Byte())
                Me._TarBracketTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblTarBracketType
            instance = DirectCast(MemberwiseClone(), tblTarBracketType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
