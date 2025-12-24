Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added by LVV 5/4/16 for v-7.0.5.1 DAT

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblDATEquipType
        Inherits DTOBaseClass


#Region " Data Members"

        Private _DATEquipTypeControl As Integer = 0
        <DataMember()> _
        Public Property DATEquipTypeControl() As Integer
            Get
                Return Me._DATEquipTypeControl
            End Get
            Set(value As Integer)
                If ((Me._DATEquipTypeControl = value) _
                   = False) Then
                    Me._DATEquipTypeControl = value
                    Me.SendPropertyChanged("DATEquipTypeControl")
                End If
            End Set
        End Property


        Private _DATEquipTypeName As String
        <DataMember()> _
        Public Property DATEquipTypeName() As String
            Get
                Return Left(Me._DATEquipTypeName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._DATEquipTypeName, value) = False) Then
                    Me._DATEquipTypeName = Left(value, 50)
                    Me.SendPropertyChanged("DATEquipTypeName")
                End If
            End Set
        End Property

        Private _DATEquipTypeDesc As String
        <DataMember()> _
        Public Property DATEquipTypeDesc() As String
            Get
                Return Left(Me._DATEquipTypeDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._DATEquipTypeDesc, value) = False) Then
                    Me._DATEquipTypeDesc = Left(value, 255)
                    Me.SendPropertyChanged("DATEquipTypeDesc")
                End If
            End Set
        End Property

        Private _DATEquipTypeModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DATEquipTypeModDate() As System.Nullable(Of Date)
            Get
                Return Me._DATEquipTypeModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._DATEquipTypeModDate.Equals(value) = False) Then
                    Me._DATEquipTypeModDate = value
                    Me.SendPropertyChanged("DATEquipTypeModDate")
                End If
            End Set
        End Property

        Private _DATEquipTypeModUser As String
        <DataMember()> _
        Public Property DATEquipTypeModUser() As String
            Get
                Return Left(Me._DATEquipTypeModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._DATEquipTypeModUser, value) = False) Then
                    Me._DATEquipTypeModUser = Left(value, 100)
                    Me.SendPropertyChanged("DATEquipTypeModUser")
                End If
            End Set
        End Property


        Private _DATEquipTypeUpdated As Byte()
        <DataMember()> _
        Public Property DATEquipTypeUpdated() As Byte()
            Get
                Return Me._DATEquipTypeUpdated
            End Get
            Set(value As Byte())
                Me._DATEquipTypeUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblDATEquipType
            instance = DirectCast(MemberwiseClone(), tblDATEquipType)
            Return instance
        End Function

#End Region

    End Class
End Namespace
