Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblClassType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property ClassTypeControl() As Integer
            Get
                Return Me._ClassTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ClassTypeControl = value) _
                   = False) Then
                    Me._ClassTypeControl = value
                    Me.SendPropertyChanged("ClassTypeControl")
                End If
            End Set
        End Property



        Private _ClassTypeName As String
        <DataMember()> _
        Public Property ClassTypeName() As String
            Get
                Return Left(Me._ClassTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._ClassTypeName, value) = False) Then
                    Me._ClassTypeName = Left(value, 50)
                    Me.SendPropertyChanged("ClassTypeName")
                End If
            End Set
        End Property

        Private _ClassTypeDesc As String
        <DataMember()> _
        Public Property ClassTypeDesc() As String
            Get
                Return Left(Me._ClassTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ClassTypeDesc, value) = False) Then
                    Me._ClassTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("ClassTypeDesc")
                End If
            End Set
        End Property

        Private _ClassTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ClassTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._ClassTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ClassTypeModDate.Equals(value) = False) Then
                    Me._ClassTypeModDate = value
                    Me.SendPropertyChanged("ClassTypeModDate")
                End If
            End Set
        End Property

        Private _ClassTypeModUser As String
        <DataMember()> _
        Public Property ClassTypeModUser() As String
            Get
                Return Left(Me._ClassTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ClassTypeModUser, value) = False) Then
                    Me._ClassTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("ClassTypeModUser")
                End If
            End Set
        End Property


        Private _ClassTypeUpdated As Byte()
        <DataMember()> _
        Public Property ClassTypeUpdated() As Byte()
            Get
                Return Me._ClassTypeUpdated
            End Get
            Set(value As Byte())
                Me._ClassTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblClassType
            instance = DirectCast(MemberwiseClone(), tblClassType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
