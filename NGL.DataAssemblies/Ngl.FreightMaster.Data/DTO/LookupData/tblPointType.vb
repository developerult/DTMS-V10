Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblPointType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _PointTypeControl As Integer = 0
        <DataMember()> _
        Public Property PointTypeControl() As Integer
            Get
                Return Me._PointTypeControl
            End Get
            Set(value As Integer)
                If ((Me._PointTypeControl = value) _
                   = False) Then
                    Me._PointTypeControl = value
                    Me.SendPropertyChanged("PointTypeControl")
                End If
            End Set
        End Property



        Private _PointTypeName As String
        <DataMember()> _
        Public Property PointTypeName() As String
            Get
                Return Left(Me._PointTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._PointTypeName, value) = False) Then
                    Me._PointTypeName = Left(value, 50)
                    Me.SendPropertyChanged("PointTypeName")
                End If
            End Set
        End Property

        Private _PointTypeDesc As String
        <DataMember()> _
        Public Property PointTypeDesc() As String
            Get
                Return Left(Me._PointTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._PointTypeDesc, value) = False) Then
                    Me._PointTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("PointTypeDesc")
                End If
            End Set
        End Property

        Private _PointTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property PointTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._PointTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._PointTypeModDate.Equals(value) = False) Then
                    Me._PointTypeModDate = value
                    Me.SendPropertyChanged("PointTypeModDate")
                End If
            End Set
        End Property

        Private _PointTypeModUser As String
        <DataMember()> _
        Public Property PointTypeModUser() As String
            Get
                Return Left(Me._PointTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._PointTypeModUser, value) = False) Then
                    Me._PointTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("PointTypeModUser")
                End If
            End Set
        End Property


        Private _PointTypeUpdated As Byte()
        <DataMember()> _
        Public Property PointTypeUpdated() As Byte()
            Get
                Return Me._PointTypeUpdated
            End Get
            Set(value As Byte())
                Me._PointTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblPointType
            instance = DirectCast(MemberwiseClone(), tblPointType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
