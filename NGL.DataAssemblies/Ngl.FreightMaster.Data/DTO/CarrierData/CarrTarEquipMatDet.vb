Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarEquipMatDet
        Inherits DTOBaseClass

#Region " Data Members"
        Private _CarrTarEquipMatDetControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatDetControl() As Integer
            Get
                Return _CarrTarEquipMatDetControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatDetControl <> value) Then
                    Me._CarrTarEquipMatDetControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatDetControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetCarrTarEquipMatControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatDetCarrTarEquipMatControl() As Integer
            Get
                Return _CarrTarEquipMatDetCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatDetCarrTarEquipMatControl <> value) Then
                    Me._CarrTarEquipMatDetCarrTarEquipMatControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatDetCarrTarEquipMatControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetID As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatDetID() As Integer
            Get
                Return _CarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatDetID <> value) Then
                    Me._CarrTarEquipMatDetID = value
                    Me.SendPropertyChanged("CarrTarEquipMatDetID")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property CarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _CarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                If (Me._CarrTarEquipMatDetValue.Equals(value) = False) Then
                    Me._CarrTarEquipMatDetValue = value
                    Me.SendPropertyChanged("CarrTarEquipMatDetValue")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetModUser As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatDetModUser() As String
            Get
                Return Left(_CarrTarEquipMatDetModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatDetModUser, value) = False) Then
                    Me._CarrTarEquipMatDetModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarEquipMatDetModUser")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEquipMatDetModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarEquipMatDetModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarEquipMatDetModDate.Equals(value) = False) Then
                    Me._CarrTarEquipMatDetModDate = value
                    Me.SendPropertyChanged("CarrTarEquipMatDetModDate")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatDetUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarEquipMatDetUpdated() As Byte()
            Get
                Return _CarrTarEquipMatDetUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarEquipMatDetUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarEquipMatDet
            instance = DirectCast(MemberwiseClone(), CarrTarEquipMatDet)
            Return instance
        End Function

#End Region

    End Class
End Namespace
