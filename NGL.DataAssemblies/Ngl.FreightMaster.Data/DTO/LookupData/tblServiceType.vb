Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblServiceType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ServiceTypeControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTypeControl() As Integer
            Get
                Return Me._ServiceTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTypeControl = value) _
                   = False) Then
                    Me._ServiceTypeControl = value
                    Me.SendPropertyChanged("ServiceTypeControl")
                End If
            End Set
        End Property

        Private _ServiceTypeName As String
        <DataMember()> _
        Public Property ServiceTypeName() As String
            Get
                Return Left(Me._ServiceTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTypeName, value) = False) Then
                    Me._ServiceTypeName = Left(value, 50)
                    Me.SendPropertyChanged("ServiceTypeName")
                End If
            End Set
        End Property

        Private _ServiceTypeDesc As String
        <DataMember()> _
        Public Property ServiceTypeDesc() As String
            Get
                Return Left(Me._ServiceTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTypeDesc, value) = False) Then
                    Me._ServiceTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("ServiceTypeDesc")
                End If
            End Set
        End Property

        Private _ServiceTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ServiceTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._ServiceTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ServiceTypeModDate.Equals(value) = False) Then
                    Me._ServiceTypeModDate = value
                    Me.SendPropertyChanged("ServiceTypeModDate")
                End If
            End Set
        End Property

        Private _ServiceTypeModUser As String
        <DataMember()> _
        Public Property ServiceTypeModUser() As String
            Get
                Return Left(Me._ServiceTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTypeModUser, value) = False) Then
                    Me._ServiceTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("ServiceTypeModUser")
                End If
            End Set
        End Property


        Private _ServiceTypeUpdated As Byte()
        <DataMember()> _
        Public Property ServiceTypeUpdated() As Byte()
            Get
                Return Me._ServiceTypeUpdated
            End Get
            Set(value As Byte())
                Me._ServiceTypeUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblServiceType
            instance = DirectCast(MemberwiseClone(), tblServiceType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
