Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblModeType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property ModeTypeControl() As Integer
            Get
                Return Me._ModeTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ModeTypeControl = value) _
                   = False) Then
                    Me._ModeTypeControl = value
                    Me.SendPropertyChanged("ModeTypeControl")
                End If
            End Set
        End Property



        Private _ModeTypeName As String
        <DataMember()> _
        Public Property ModeTypeName() As String
            Get
                Return Left(Me._ModeTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._ModeTypeName, value) = False) Then
                    Me._ModeTypeName = Left(value, 50)
                    Me.SendPropertyChanged("ModeTypeName")
                End If
            End Set
        End Property

        Private _ModeTypeDesc As String
        <DataMember()> _
        Public Property ModeTypeDesc() As String
            Get
                Return Left(Me._ModeTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ModeTypeDesc, value) = False) Then
                    Me._ModeTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("ModeTypeDesc")
                End If
            End Set
        End Property

        Private _ModeTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ModeTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._ModeTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ModeTypeModDate.Equals(value) = False) Then
                    Me._ModeTypeModDate = value
                    Me.SendPropertyChanged("ModeTypeModDate")
                End If
            End Set
        End Property

        Private _ModeTypeModUser As String
        <DataMember()> _
        Public Property ModeTypeModUser() As String
            Get
                Return Left(Me._ModeTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ModeTypeModUser, value) = False) Then
                    Me._ModeTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("ModeTypeModUser")
                End If
            End Set
        End Property


        Private _ModeTypeUpdated As Byte()
        <DataMember()> _
        Public Property ModeTypeUpdated() As Byte()
            Get
                Return Me._ModeTypeUpdated
            End Get
            Set(value As Byte())
                Me._ModeTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblModeType
            instance = DirectCast(MemberwiseClone(), tblModeType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
